using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

using Newtonsoft.Json;


namespace IndianBank_ChatBOT.Middleware
{
    public class TranscriptLoggerMiddleware : IMiddleware
    {

        private static JsonSerializerSettings _jsonSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
        private ITranscriptLogger logger;
        public TranscriptLoggerMiddleware(ITranscriptLogger transcriptLogger)
        {
            logger = transcriptLogger ?? throw new ArgumentNullException("TranscriptLoggerMiddleware requires a ITranscriptLogger implementation.  ");
        }

        public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next, CancellationToken cancellationToken = default(CancellationToken))
        {
            Queue<IActivity> transcript = new Queue<IActivity>();


            // log incoming activity at beginning of turn
            if (turnContext.Activity != null)
            {
                if (turnContext.Activity.From == null)
                {
                    turnContext.Activity.From = new ChannelAccount();
                }

                if (string.IsNullOrEmpty((string)turnContext.Activity.From.Properties["role"]))
                {
                    turnContext.Activity.From.Properties["role"] = "user";
                }

                LogActivity(transcript, CloneActivity(turnContext.Activity));
            }
            // hook up onSend pipeline
            turnContext.OnSendActivities(async (ctx, activities, nextSend) =>
            {
                // run full pipeline
                var responses = await nextSend().ConfigureAwait(false);

                foreach (var activity in activities)
                {
                    LogActivity(transcript, CloneActivity(activity));
                }

                return responses;
            });
            // hook up update activity pipeline
            turnContext.OnUpdateActivity(async (ctx, activity, nextUpdate) =>
            {
                // run full pipeline
                var response = await nextUpdate().ConfigureAwait(false);

                // add Message Update activity
                var updateActivity = CloneActivity(activity);
                updateActivity.Type = ActivityTypes.MessageUpdate;
                LogActivity(transcript, updateActivity);
                return response;
            });
            // hook up delete activity pipeline
            turnContext.OnDeleteActivity(async (ctx, reference, nextDelete) =>
            {
                // run full pipeline
                await nextDelete().ConfigureAwait(false);

                // add MessageDelete activity
                // log as MessageDelete activity
                var deleteActivity = new Microsoft.Bot.Schema.Activity
                {
                    Type = ActivityTypes.MessageDelete,
                    Id = reference.ActivityId,
                }
                .ApplyConversationReference(reference, isIncoming: false)
                .AsMessageDeleteActivity();

                LogActivity(transcript, deleteActivity);
            });
            // process bot logic
            await nextTurn(cancellationToken).ConfigureAwait(false);

            // flush transcript at end of turn
            while (transcript.Count > 0)
            {
                var activity = transcript.Dequeue();

                // As we are deliberately not using await, disable teh associated warning.
                await logger.LogActivityAsync(activity).ContinueWith(
                    task =>
                    {
                        try
                        {
                            task.Wait();
                        }
                        catch (Exception err)
                        {
                            Trace.TraceError($"Transcript logActivity failed with {err}");
                        }
                    },
                    cancellationToken);
            }

        }

        private async Task nextTurn(CancellationToken cancellationToken)
        {
            await nextTurn(cancellationToken);
            //throw new NotImplementedException();
        }

        private void LogActivity(Queue<IActivity> transcript, IActivity activity)
        {
            if (activity.Timestamp == null)
            {
                activity.Timestamp = DateTime.UtcNow;
            }

            transcript.Enqueue(activity);
        }
        private static IActivity CloneActivity(IActivity activity)
        {
            activity = JsonConvert.DeserializeObject<Microsoft.Bot.Schema.Activity>(JsonConvert.SerializeObject(activity, _jsonSettings));
            return activity;
        }
    }

}
