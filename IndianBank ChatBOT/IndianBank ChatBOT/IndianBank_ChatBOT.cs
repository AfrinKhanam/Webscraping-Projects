using System;
using System.Threading;
using System.Threading.Tasks;

using IndianBank_ChatBOT.Dialogs.Main;
using IndianBank_ChatBOT.Models;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Options;

namespace IndianBank_ChatBOT
{
    /// <summary>
    /// Main entry point and orchestration for bot.
    /// </summary>
    public class IndianBank_ChatBOT : IBot
    {
        private readonly BotServices _services;
        private readonly ConversationState _conversationState;
        private readonly UserState _userState;

        private readonly AppSettings appSettings;
        // private readonly IBotTelemetryClient _telemetryClient;
        private DialogSet _dialogs;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndianBank_ChatBOT"/> class.
        /// </summary>
        /// <param name="botServices">Bot services.</param>
        /// <param name="conversationState">Bot conversation state.</param>
        /// <param name="userState">Bot user state.</param>
        public IndianBank_ChatBOT(BotServices botServices, ConversationState conversationState, UserState userState,IOptions<AppSettings> appsettings)

        {
            appSettings = appsettings.Value;

            _conversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
            _userState = userState ?? throw new ArgumentNullException(nameof(userState));
            _services = botServices ?? throw new ArgumentNullException(nameof(botServices));
            _dialogs = new DialogSet(_conversationState.CreateProperty<DialogState>(nameof(IndianBank_ChatBOT)));
            _dialogs.Add(new MainDialog(_services, _conversationState, _userState,appSettings));
           // _dialogs.Add(new VehicleLoanDialog(_services, _conversationState, _userState));
        }

        /// <summary>
        /// Run every turn of the conversation. Handles orchestration of messages.
        /// </summary>
        /// <param name="turnContext">Bot Turn Context.</param>
        /// <param name="cancellationToken">Task CancellationToken.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            // Client notifying this bot took to long to respond (timed out)
            if (turnContext.Activity.Code == EndOfConversationCodes.BotTimedOut)
            {
                return;
            }

            var dc = await _dialogs.CreateContextAsync(turnContext);

            if (dc.ActiveDialog != null)
            {
                await dc.ContinueDialogAsync();
            }
            else
            {
                await dc.BeginDialogAsync(nameof(MainDialog));
            }
        }
    }
}
