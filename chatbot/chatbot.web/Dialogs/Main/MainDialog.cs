using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using IndianBank_ChatBOT.Dialogs.Onboarding;
using IndianBank_ChatBOT.Dialogs.Shared;
using IndianBank_ChatBOT.Models;
using IndianBank_ChatBOT.ViewModel;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Caching.Memory;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IndianBank_ChatBOT.Dialogs.Main
{
    public partial class MainDialog : RouterDialog
    {
        private readonly BotServices _services;
        private readonly AppSettings appSettings;

        private readonly AppDbContext dbContext;
        private readonly IMemoryCache memoryCache;
        private readonly IHttpClientFactory clientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainDialog"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="conversationState">State of the conversation.</param>
        /// <param name="userState">State of the user.</param>
        /// <exception cref="ArgumentNullException">services</exception>
        public MainDialog(BotServices services, AppSettings appSettings, AppDbContext dbContext, IMemoryCache _memoryCache, IHttpClientFactory clientFactory)
            : base(nameof(MainDialog))
        {
            this.dbContext = dbContext;
            memoryCache = _memoryCache;
            this.clientFactory = clientFactory;
            _services = services ?? throw new ArgumentNullException(nameof(services));
            this.appSettings = appSettings;
            AddDialog(new OnBoardingFormDialog(_services, dbContext));
        }

        #region methods

        /// <summary>
        /// Called when /[start asynchronous].
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected override async Task OnStartAsync(DialogContext dc, CancellationToken cancellationToken = default(CancellationToken))
        {
            await dc.Context.SendActivityAsync($"Hi! My name is ADYA \U0001F603. Welcome to Indian Bank. I am your virtual assistant, here to assist you with all your banking queries 24x7.");

            await dc.BeginDialogAsync(nameof(OnBoardingFormDialog));
        }

        /// <summary>
        /// Routes the asynchronous.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// The specified LUIS Model could not be found in your Bot Services configuration.
        /// or
        /// The specified QnA Maker Service could not be found in your Bot Services configuration.
        /// or
        /// The specified QnA Maker Service could not be found in your Bot Services configuration.
        /// </exception>
        protected override async Task RouteAsync(DialogContext dc, CancellationToken cancellationToken = default(CancellationToken))
        {
            string utterance = dc.Context.Activity.Text;
            int utterance_word_count = utterance.Split(" ").Length;

            _services.LuisServices.TryGetValue("general", out var luisService);

            if (luisService == null)
            {
                throw new Exception("The specified LUIS Model could not be found in your Bot Services configuration.");
            }
            else if (luisService != null)
            {
                string entityName = string.Empty;
                string entityType = string.Empty;

                var conversationID = dc.Context.Activity.Conversation.Id;

                var userInfo = dbContext.UserInfos.FirstOrDefault(e => e.ConversationId == conversationID);

                var result = await luisService.RecognizeAsync(dc.Context, CancellationToken.None);

                var entityTypes = new string[]
                {
                    "what_entity",
                    "why_entity",
                    "can_entity",
                    "how_entity",
                    "when_entity",
                    "where_entity",
                    "which_entity",
                    "who_entity"
                };

                foreach (var entity in entityTypes)
                {
                    if (result.Entities[entity] != null)
                    {
                        entityType = entity;
                        entityName = result.Entities[entity].Values<string>().FirstOrDefault();
                        break;
                    }
                }

                var generalIntent = result.GetTopScoringIntent().intent;
                var generalIntentScore = result.GetTopScoringIntent().score;

                Console.WriteLine(generalIntent, generalIntentScore);

                if (generalIntent == "thankyouintent")
                {
                    var messageData = result.Text.First().ToString().ToUpper() + result.Text.Substring(1);
                    await dc.Context.SendActivityAsync($"{messageData}!!! {userInfo.Name}. It was nice talking to you today.");
                }
                else if (utterance.Trim().ToLower() == "hi" || utterance.Trim().ToLower() == "hello" || utterance.Trim().ToLower() == "hey" || utterance.Trim().ToLower() == "good morning" || (utterance.Trim().ToLower() == "hii") || utterance.Trim().ToLower() == "greetings" || utterance.Trim().ToLower() == "whats up")
                {
                    var messageData = result.Text.First().ToString().ToUpper() + result.Text.Substring(1);
                    await dc.Context.SendActivityAsync($"{messageData}!!! {userInfo.Name}. How may I help you today?");
                }
                else if (utterance.Trim().ToLower() == "bye" || utterance.Trim().ToLower() == "bye bye" || utterance.Trim().ToLower() == "good bye" || utterance.Trim().ToLower() == "take care" || (utterance.Trim().ToLower() == "tata"))
                {
                    var messageData = result.Text.First().ToString().ToUpper() + result.Text.Substring(1);
                    await dc.Context.SendActivityAsync($"{messageData}!!! {userInfo.Name}. It was nice talking to you today.");
                }
                else if (generalIntentScore > 0.75)
                {
                    var messageData = result.Text.First().ToString().ToUpper() + result.Text.Substring(1);

                    if (generalIntent == "small_talks_intent")
                    {
                        await dc.Context.SendActivityAsync($"Hello!! I'm IVA, your Indian Bank Virtual Assistant");
                    }
                    else if (generalIntent == "capabilities_intent")
                    {
                        await dc.Context.SendActivityAsync($"I am here to assist you with all your banking queries 24x7. \n Feel free to ask me any question by typing below or clicking on the dynamic scroll bar options for specific suggestions.");
                    }
                    else if (generalIntent == "bye_intent")
                    {
                        await dc.Context.SendActivityAsync($"{messageData}!!! {userInfo.Name}. It was nice talking to you today.");
                    }
                    else if (entityType == "atm_entity")
                    {
                        await dc.Context.SendActivityAsync("Please click on the URL below to find all Indian Bank ATM/Branch Locations. \n\n https://www.indianbank.in/branch-atm/");
                    }
                    else if (entityType == "lost_entity")
                    {
                        await dc.Context.SendActivityAsync($"Looks like your query requires futher assistance. Please contact customer care immediately on the following number's : \n\n <tel:180042500000> /  <tel:18004254422>");
                    }
                    else
                    {
                        await SearchKB(dc, appSettings.QAEndPoint, clientFactory);
                    }
                }
                else
                {
                    await SearchKB(dc, appSettings.QAEndPoint, clientFactory);
                }
            }
        }

        public static async Task SearchKB(DialogContext dc, string qaEndPoint, IHttpClientFactory clientFactory)
        {
            var query = dc.Context.Activity.Text;

            var data = await GetKnowledgeBaseResults(query, qaEndPoint, clientFactory);

            await DisplayBackendResult(dc, data);
        }

        public static async Task<string> GetKnowledgeBaseResults(string query, string qaEndPoint, IHttpClientFactory clientFactory)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{qaEndPoint}?query={System.Net.WebUtility.UrlEncode(query)}"))
            {
                using (var client = clientFactory.CreateClient())
                {
                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }

            throw new Exception("Error fetching results from Knowledge Base.");
        }

        /// <summary>
        /// Called when [event asynchronous].
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected override async Task OnEventAsync(DialogContext dc, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Check if there was an action submitted from intro card
            if (dc.Context.Activity.Value != null)
            {
                dynamic value = dc.Context.Activity.Value;

                if (dc.Context.Activity.Value != null)
                {
                    if (value.action == "menu")
                    {
                        var selectedMenu = (value as JObject).ToObject<SelectedMenuItem>();

                        await ScrollBarUtils.DisplayScrollBarMenu(dc, selectedMenu, appSettings, memoryCache, clientFactory);
                    }
                }
            }
        }

        public static async Task DisplayBackendResult(DialogContext dialogContext, string backendResult)
        {
            var kbResult = JsonConvert.DeserializeObject<KnowledgeBaseResult>(backendResult);

            if (kbResult.DOCUMENTS.Count == 0)
            {
                dialogContext.Context.Activity.Conversation.Properties.Add(nameof(ExtendedLogData), JToken.FromObject(new ExtendedLogData
                {
                    IntentName = "bye_intent"
                }));

                await dialogContext.Context.SendActivityAsync("Sorry, I could not understand. Could you please rephrase the query.");
            }
            else
            {
                // var kbResult = JsonConvert.DeserializeObject<KnowledgeBaseResult>(backendResult);

                if (kbResult.DOCUMENTS.Count >= 1)
                {
                    dialogContext.Context.Activity.Conversation.Properties.Add(nameof(ExtendedLogData), JToken.FromObject(new ExtendedLogData
                    {
                        IntentName = kbResult.DOCUMENTS[0].main_title,
                        SubTitle = kbResult.DOCUMENTS[0].title,
                        ResponseJson = backendResult,
                        ResponseSource = ResponseSource.ElasticSearch
                    }));
                }

                var firstDoc = kbResult.DOCUMENTS.First();

                if (!String.IsNullOrEmpty(kbResult.FILENAME))
                {
                    var heroCard = new HeroCard
                    {
                        Text = $@"For further details please visit the page [{firstDoc.url}]({firstDoc.url})"
                    };

                    if (!string.IsNullOrWhiteSpace(firstDoc.value))
                    {
                        heroCard.Title = firstDoc.main_title;
                        heroCard.Subtitle = firstDoc.title;
                        heroCard.Text = $@"{firstDoc.value}

{heroCard.Text}";
                    }

                    if (HeroCardImageMapping.MAPPING.TryGetValue(kbResult.FILENAME, out var imgPath))
                    {
                        heroCard.Images = new List<CardImage> { new CardImage(imgPath) };
                    }

                    var activity = MessageFactory.Attachment(heroCard.ToAttachment());

                    await dialogContext.Context.SendActivityAsync(activity);
                }
                else
                {
                    if (kbResult.WORD_SCORE == 0)
                    {
                        await dialogContext.Context.SendActivityAsync($"Your query seems to require further assistance. Please feel free to contact customer support on the following toll free numbers: <tel:180042500000> /  <tel:18004254422> \n\n Please click on the link below for futher contact details: \n\n https://indianbank.in/departments/quick-contact/ ");
                        await dialogContext.Context.SendActivityAsync($"Please feel free to ask me anything else about Indian Bank");
                    }
                    else if (kbResult.WORD_SCORE < 0.6)
                    {
                        await dialogContext.Context.SendActivityAsync("I did not find an exact answer but here is something similar");
                        await dialogContext.Context.SendActivityAsync($"{firstDoc.main_title}\n\n{firstDoc.title}\n\n{firstDoc.value}\n\n For further details please click on the link below:\n {firstDoc.url}");
                    }
                    else
                    {
                        await dialogContext.Context.SendActivityAsync($"This is what I found on \"{kbResult.AUTO_CORRECT_QUERY}\"");
                        if (kbResult.WORD_COUNT > 100)
                        {
                            await dialogContext.Context.SendActivityAsync($"{firstDoc.main_title}\n\n{firstDoc.title}\n\n{firstDoc.value} \n\n {firstDoc.url}");
                        }
                        else if (kbResult.WORD_COUNT < 25)
                        {
                            await dialogContext.Context.SendActivityAsync($"{firstDoc.value}\n\n For further details please click on the link below:\n {firstDoc.url}");
                        }
                        else
                        {
                            var message = string.Empty;
                            var documentCount = kbResult.DOCUMENTS.Count();

                            if (documentCount > 0)
                            {
                                message = $"{firstDoc.value}\n\n For further details please click on the link below:\n {firstDoc.url}";
                            }

                            if (documentCount > 1 && documentCount <= 2 && firstDoc.url != kbResult.DOCUMENTS[1].url)
                            {
                                message += $"\n\n\n For additional info:\n\n{kbResult.DOCUMENTS[1].url}";
                            }

                            if (documentCount > 2 && kbResult.DOCUMENTS[1].url != kbResult.DOCUMENTS[2].url)
                            {
                                message += $"\n\n{kbResult.DOCUMENTS[2].url}";
                            }

                            await dialogContext.Context.SendActivityAsync(message);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
