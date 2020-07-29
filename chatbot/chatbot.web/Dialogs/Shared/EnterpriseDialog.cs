// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;

using UjjivanBank_ChatBOT.Dialogs.Cancel;
using UjjivanBank_ChatBOT.Dialogs.Main;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace UjjivanBank_ChatBOT.Dialogs.Shared
{
    public class EnterpriseDialog : InterruptableDialog
    {
        protected const string LuisResultKey = "LuisResult";

        // Fields
        private readonly BotServices _services;
        private readonly CancelResponses _responder = new CancelResponses();

        public EnterpriseDialog(BotServices botServices, string dialogId)
            : base(dialogId)
        {
            _services = botServices;

            AddDialog(new CancelDialog());
        }

        protected override async Task<InterruptionStatus> OnDialogInterruptionAsync(DialogContext dc, CancellationToken cancellationToken)
        {
            // check luis intent
            _services.LuisServices.TryGetValue("general", out var luisService);

            if (luisService == null)
            {
                throw new Exception("The specified LUIS Model could not be found in your Bot Services configuration.");
            }
            else
            {
                var luisResult = await luisService.RecognizeAsync(dc.Context, cancellationToken);
                var intent = luisResult.GetTopScoringIntent();

                // Only triggers interruption if confidence level is high
                if (intent.score > 0.1)
                {
                    // Add the luis result (intent and entities) for further processing in the derived dialog
                    dc.Context.TurnState.Add(LuisResultKey, luisResult);

                    switch (intent.intent)
                    {
                        case Intent.Cancel:
                            {
                                return await OnCancel(dc);
                            }

                        case Intent.Help:
                            {
                                return await OnHelp(dc);
                            }

                        default:
                            {
                                return await OnInterruptingIntent(intent.intent, dc, cancellationToken);
                            }
                    }
                }
            }

            return InterruptionStatus.NoAction;
        }

        protected virtual async Task<InterruptionStatus> OnInterruptingIntent(string intent, DialogContext dc, CancellationToken cancellationToken)
        {
            await dc.Context.SendActivityAsync(new Microsoft.Bot.Schema.Activity(ActivityTypes.Typing));

            return InterruptionStatus.NoAction;
        }
        protected virtual async Task<InterruptionStatus> OnCancel(DialogContext dc)
        {
            if (dc.ActiveDialog.Id != nameof(CancelDialog))
            {
                // Don't start restart cancel dialog
                await dc.BeginDialogAsync(nameof(CancelDialog));

                // Signal that the dialog is waiting on user response
                return InterruptionStatus.Waiting;
            }

            // Else, continue
            return InterruptionStatus.NoAction;
        }

        protected virtual async Task<InterruptionStatus> OnHelp(DialogContext dc)
        {
            var view = new MainResponses();
            await view.ReplyWith(dc.Context, MainResponses.ResponseIds.Help);

            // Signal the conversation was interrupted and should immediately continue
            return InterruptionStatus.Interrupted;
        }
    }
}
