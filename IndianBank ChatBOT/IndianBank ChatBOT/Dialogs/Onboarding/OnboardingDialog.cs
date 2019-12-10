// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;
using IndianBank_ChatBOT.Dialogs.Shared;
using Microsoft.ApplicationInsights;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace IndianBank_ChatBOT.Dialogs.Onboarding
{
    public class OnboardingDialog : EnterpriseDialog
    {
        private static OnboardingResponses _responder = new OnboardingResponses();
        private IStatePropertyAccessor<OnboardingState> _accessor;
        private OnboardingState _state;

        public OnboardingDialog(BotServices botServices, IStatePropertyAccessor<OnboardingState> accessor)
            
            : base(botServices, nameof(OnboardingDialog))
        {
            _accessor = accessor;
            InitialDialogId = nameof(OnboardingDialog);

            var onboarding = new WaterfallStep[]
            {
                AskForName,
                AskForEmail,
                AskForLocation,
                FinishOnboardingDialog,
            };

            
            AddDialog(new TextPrompt(DialogIds.NamePrompt));
            AddDialog(new TextPrompt(DialogIds.EmailPrompt));
            AddDialog(new TextPrompt(DialogIds.LocationPrompt));
        }

        /// <summary>
        /// Asks for name.
        /// </summary>
        /// <param name="sc">The sc.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<DialogTurnResult> AskForName(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            _state = await _accessor.GetAsync(sc.Context, () => new OnboardingState());

            if (!string.IsNullOrEmpty(_state.Name))
            {
                return await sc.NextAsync(_state.Name);
            }
            else
            {
                return await sc.PromptAsync(DialogIds.NamePrompt, new PromptOptions()
                {
                    Prompt = await _responder.RenderTemplate(sc.Context, sc.Context.Activity.Locale, OnboardingResponses.ResponseIds.NamePrompt),
                });
            }
        }

        /// <summary>
        /// Asks for email.
        /// </summary>
        /// <param name="sc">The sc.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<DialogTurnResult> AskForEmail(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            _state = await _accessor.GetAsync(sc.Context, () => new OnboardingState());
            var name = _state.Name = (string)sc.Result;

            await _responder.ReplyWith(sc.Context, OnboardingResponses.ResponseIds.HaveNameMessage, new { name });

            return await sc.PromptAsync(DialogIds.EmailPrompt, new PromptOptions()
            {
                Prompt = await _responder.RenderTemplate(sc.Context, sc.Context.Activity.Locale, OnboardingResponses.ResponseIds.EmailPrompt),
            });
        }
        /// <summary>
        /// Asks for location.
        /// </summary>
        /// <param name="sc">The sc.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<DialogTurnResult> AskForLocation(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            _state = await _accessor.GetAsync(sc.Context, () => new OnboardingState());
            var email = _state.Email = (string)sc.Result;

            await _responder.ReplyWith(sc.Context, OnboardingResponses.ResponseIds.HaveEmailMessage, new { email });

            return await sc.PromptAsync(DialogIds.LocationPrompt, new PromptOptions()
            {
                Prompt = await _responder.RenderTemplate(sc.Context, sc.Context.Activity.Locale, OnboardingResponses.ResponseIds.LocationPrompt),
            });
        }
        /// <summary>
        /// Finishes the onboarding dialog.
        /// </summary>
        /// <param name="sc">The sc.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<DialogTurnResult> FinishOnboardingDialog(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            _state = await _accessor.GetAsync(sc.Context);
            _state.Location = (string)sc.Result;

            await _responder.ReplyWith(sc.Context, OnboardingResponses.ResponseIds.HaveLocationMessage, new { _state.Name, _state.Location });
            return await sc.EndDialogAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        private class DialogIds
        {
            public const string NamePrompt = "namePrompt";
            public const string EmailPrompt = "emailPrompt";
            public const string LocationPrompt = "locationPrompt";
        }
    }
}
