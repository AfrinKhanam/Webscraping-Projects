using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Rest;

using Newtonsoft.Json.Linq;

namespace UjjivanBank_ChatBOT.Middleware.Telemetry
{
    public class EmptyServiceCredentials : ServiceClientCredentials
    {

    }

    /// <summary>
    /// Rasa Recognizer
    /// </summary>
    /// <seealso cref="Microsoft.Bot.Builder.IRecognizer" />
    public class TelemetryRasaRecognizer : IRecognizer
    {
        #region Properties

        public const string LuisTraceLabel = "Luis Trace";
        private const string _metadataKey = "$instance";
        private readonly ILUISRuntimeClient _runtime;

        #endregion

        #region Constructor

        public TelemetryRasaRecognizer(string rasaUrl, string rasaProjectName)
        {
            _runtime = new RasaRuntimeClient(rasaUrl, rasaProjectName);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Runs an utterance through a recognizer and returns a strongly-typed recognizer result.
        /// </summary>
        /// <typeparam name="T">The recognition result type.</typeparam>
        /// <param name="turnContext">Turn context.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// Analysis of utterance.
        /// </returns>
        public async Task<T> RecognizeAsync<T>(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
            where T : IRecognizerConvert, new()
        {
            var result = new T();
            result.Convert(await RecognizeAsync(turnContext, cancellationToken).ConfigureAwait(false));
            return result;
        }

        /// <summary>
        /// Recognizes the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">context</exception>
        public async Task<RecognizerResult> RecognizeAsync(ITurnContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Call Luis Recognizer
            var recognizerResult = await RecognizeInternalAsync(context, cancellationToken);
            return recognizerResult;
        }

        /// <summary>
        /// Normalizeds the intent.
        /// </summary>
        /// <param name="intent">The intent.</param>
        /// <returns></returns>
        private static string NormalizedIntent(string intent) => intent?.Replace('.', '_')?.Replace(' ', '_');

        /// <summary>
        /// Gets the intents.
        /// </summary>
        /// <param name="luisResult">The luis result.</param>
        /// <returns></returns>
        private static IDictionary<string, IntentScore> GetIntents(LuisResult luisResult)
        {
            if (luisResult.Intents != null)
            {
                return luisResult.Intents.ToDictionary(
                    i => NormalizedIntent(i.Intent),
                    i => new IntentScore { Score = i.Score ?? 0 });
            }
            else
            {
                return new Dictionary<string, IntentScore>()
                {
                    {
                        NormalizedIntent(luisResult.TopScoringIntent.Intent),
                        new IntentScore() { Score = luisResult.TopScoringIntent.Score ?? 0 }
                    },
                };
            }
        }

        /// <summary>
        /// Extracts the entities and metadata.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="compositeEntities">The composite entities.</param>
        /// <param name="verbose">if set to <c>true</c> [verbose].</param>
        /// <returns></returns>
        private static JObject ExtractEntitiesAndMetadata(IList<EntityModel> entities, IList<CompositeEntityModel> compositeEntities, bool verbose)
        {
            var entitiesAndMetadata = new JObject();
            if (verbose)
            {
                entitiesAndMetadata[_metadataKey] = new JObject();
            }

            var compositeEntityTypes = new HashSet<string>();

            // We start by populating composite entities so that entities covered by them are removed from the entities list
            if (compositeEntities != null && compositeEntities.Any())
            {
                compositeEntityTypes = new HashSet<string>(compositeEntities.Select(ce => ce.ParentType));
                entities = compositeEntities.Aggregate(entities, (current, compositeEntity) => PopulateCompositeEntityModel(compositeEntity, current, entitiesAndMetadata, verbose));
            }

            foreach (var entity in entities)
            {
                // we'll address composite entities separately
                if (compositeEntityTypes.Contains(entity.Type))
                {
                    continue;
                }

                AddProperty(entitiesAndMetadata, ExtractNormalizedEntityName(entity), ExtractEntityValue(entity));

                if (verbose)
                {
                    AddProperty((JObject)entitiesAndMetadata[_metadataKey], ExtractNormalizedEntityName(entity), ExtractEntityMetadata(entity));
                }
            }

            return entitiesAndMetadata;
        }

        /// <summary>
        /// Numbers the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static JToken Number(dynamic value)
        {
            if (value == null)
            {
                return null;
            }

            return long.TryParse((string)value, out var longVal) ?
                            new JValue(longVal) :
                            new JValue(double.Parse((string)value));
        }

        /// <summary>
        /// Extracts the entity value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        private static JToken ExtractEntityValue(EntityModel entity)
        {
#pragma warning disable IDE0007 // Use implicit type
            if (entity.AdditionalProperties == null || !entity.AdditionalProperties.TryGetValue("resolution", out dynamic resolution))
#pragma warning restore IDE0007 // Use implicit type
            {
                return entity.Entity;
            }

            if (entity.Type.StartsWith("builtin.datetime."))
            {
                return JObject.FromObject(resolution);
            }
            else if (entity.Type.StartsWith("builtin.datetimeV2."))
            {
                if (resolution.values == null || resolution.values.Count == 0)
                {
                    return JArray.FromObject(resolution);
                }

                var resolutionValues = (IEnumerable<dynamic>)resolution.values;
                var type = resolution.values[0].type;
                var timexes = resolutionValues.Select(val => val.timex);
                var distinctTimexes = timexes.Distinct();
                return new JObject(new JProperty("type", type), new JProperty("timex", JArray.FromObject(distinctTimexes)));
            }
            else
            {
                switch (entity.Type)
                {
                    case "builtin.number":
                    case "builtin.ordinal": return Number(resolution.value);
                    case "builtin.percentage":
                        {
                            var svalue = (string)resolution.value;
                            if (svalue.EndsWith("%"))
                            {
                                svalue = svalue.Substring(0, svalue.Length - 1);
                            }

                            return Number(svalue);
                        }

                    case "builtin.age":
                    case "builtin.dimension":
                    case "builtin.currency":
                    case "builtin.temperature":
                        {
                            var units = (string)resolution.unit;
                            var val = Number(resolution.value);
                            var obj = new JObject();
                            if (val != null)
                            {
                                obj.Add("number", val);
                            }

                            obj.Add("units", units);
                            return obj;
                        }

                    default:
                        return resolution.value ?? JArray.FromObject(resolution.values);
                }
            }
        }

        /// <summary>
        /// Extracts the entity metadata.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        private static JObject ExtractEntityMetadata(EntityModel entity)
        {
            dynamic obj = JObject.FromObject(new
            {
                startIndex = (int)entity.StartIndex,
                endIndex = (int)entity.EndIndex + 1,
                text = entity.Entity,
                type = entity.Type,
            });
            if (entity.AdditionalProperties != null)
            {
                if (entity.AdditionalProperties.TryGetValue("score", out var score))
                {
                    obj.score = (double)score;
                }

#pragma warning disable IDE0007 // Use implicit type
                if (entity.AdditionalProperties.TryGetValue("resolution", out dynamic resolution) && resolution.subtype != null)
#pragma warning restore IDE0007 // Use implicit type
                {
                    obj.subtype = resolution.subtype;
                }
            }

            return obj;
        }

        /// <summary>
        /// Extracts the name of the normalized entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        private static string ExtractNormalizedEntityName(EntityModel entity)
        {
            var type = entity.Type.Split(':').Last();
            if (type.StartsWith("builtin.datetimeV2."))
            {
                type = "datetime";
            }

            if (type.StartsWith("builtin.currency"))
            {
                type = "money";
            }

            if (type.StartsWith("builtin."))
            {
                type = type.Substring(8);
            }

            var role = entity.AdditionalProperties != null && entity.AdditionalProperties.ContainsKey("role") ? (string)entity.AdditionalProperties["role"] : string.Empty;
            if (!string.IsNullOrWhiteSpace(role))
            {
                type = role;
            }

            return type.Replace('.', '_').Replace(' ', '_');
        }

        /// <summary>
        /// Populates the composite entity model.
        /// </summary>
        /// <param name="compositeEntity">The composite entity.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="entitiesAndMetadata">The entities and metadata.</param>
        /// <param name="verbose">if set to <c>true</c> [verbose].</param>
        /// <returns></returns>
        private static IList<EntityModel> PopulateCompositeEntityModel(CompositeEntityModel compositeEntity, IList<EntityModel> entities, JObject entitiesAndMetadata, bool verbose)
        {
            var childrenEntites = new JObject();
            var childrenEntitiesMetadata = new JObject();
            if (verbose)
            {
                childrenEntites[_metadataKey] = new JObject();
            }

            // This is now implemented as O(n^2) search and can be reduced to O(2n) using a map as an optimization if n grows
            var compositeEntityMetadata = entities.FirstOrDefault(e => e.Type == compositeEntity.ParentType && e.Entity == compositeEntity.Value);

            // This is an error case and should not happen in theory
            if (compositeEntityMetadata == null)
            {
                return entities;
            }

            if (verbose)
            {
                childrenEntitiesMetadata = ExtractEntityMetadata(compositeEntityMetadata);
                childrenEntites[_metadataKey] = new JObject();
            }

            var coveredSet = new HashSet<EntityModel>();
            foreach (var child in compositeEntity.Children)
            {
                foreach (var entity in entities)
                {
                    // We already covered this entity
                    if (coveredSet.Contains(entity))
                    {
                        continue;
                    }

                    // This entity doesn't belong to this composite entity
                    if (child.Type != entity.Type || !CompositeContainsEntity(compositeEntityMetadata, entity))
                    {
                        continue;
                    }

                    // Add to the set to ensure that we don't consider the same child entity more than once per composite
                    coveredSet.Add(entity);
                    AddProperty(childrenEntites, ExtractNormalizedEntityName(entity), ExtractEntityValue(entity));

                    if (verbose)
                    {
                        AddProperty((JObject)childrenEntites[_metadataKey], ExtractNormalizedEntityName(entity), ExtractEntityMetadata(entity));
                    }
                }
            }

            AddProperty(entitiesAndMetadata, ExtractNormalizedEntityName(compositeEntityMetadata), childrenEntites);
            if (verbose)
            {
                AddProperty((JObject)entitiesAndMetadata[_metadataKey], ExtractNormalizedEntityName(compositeEntityMetadata), childrenEntitiesMetadata);
            }

            // filter entities that were covered by this composite entity
            return entities.Except(coveredSet).ToList();
        }

        /// <summary>
        /// Composites the contains entity.
        /// </summary>
        /// <param name="compositeEntityMetadata">The composite entity metadata.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        private static bool CompositeContainsEntity(EntityModel compositeEntityMetadata, EntityModel entity)
                    => entity.StartIndex >= compositeEntityMetadata.StartIndex &&
                           entity.EndIndex <= compositeEntityMetadata.EndIndex;

        /// <summary>
        /// If a property doesn't exist add it to a new array, otherwise append it to the existing array.
        /// </summary>
        private static void AddProperty(JObject obj, string key, JToken value)
        {
            if (((IDictionary<string, JToken>)obj).ContainsKey(key))
            {
                ((JArray)obj[key]).Add(value);
            }
            else
            {
                obj[key] = new JArray(value);
            }
        }

        /// <summary>
        /// Adds the properties.
        /// </summary>
        /// <param name="luis">The luis.</param>
        /// <param name="result">The result.</param>
        private static void AddProperties(LuisResult luis, RecognizerResult result)
        {
            if (luis.SentimentAnalysis != null)
            {
                result.Properties.Add("sentiment", new JObject(
                    new JProperty("label", luis.SentimentAnalysis.Label),
                    new JProperty("score", luis.SentimentAnalysis.Score)));
            }
        }

        /// <summary>
        /// Recognizes the internal asynchronous.
        /// </summary>
        /// <param name="turnContext">The turn context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task<RecognizerResult> RecognizeInternalAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            BotAssert.ContextNotNull(turnContext);

            if (turnContext.Activity.Type != ActivityTypes.Message)
            {
                return null;
            }

            var utterance = turnContext.Activity?.AsMessageActivity()?.Text;

            if (string.IsNullOrWhiteSpace(utterance))
            {
                throw new ArgumentNullException(nameof(utterance));
            }

            var luisResult = await _runtime.Prediction.ResolveAsync(string.Empty,
                utterance,
                cancellationToken: cancellationToken).ConfigureAwait(false);

            var recognizerResult = new RecognizerResult
            {
                Text = utterance,
                AlteredText = luisResult.AlteredQuery,
                Intents = GetIntents(luisResult),
                Entities = ExtractEntitiesAndMetadata(luisResult.Entities, luisResult.CompositeEntities, true),
            };

            AddProperties(luisResult, recognizerResult);

            var traceInfo = JObject.FromObject(
                new
                {
                    recognizerResult,
                    luisResult,
                });

            return recognizerResult;
        }

        #endregion
    }
}
