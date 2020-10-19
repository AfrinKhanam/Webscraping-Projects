using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using IndianBank_ChatBOT.Dialogs.Main;
using IndianBank_ChatBOT.Models;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace IndianBank_ChatBOT
{
    /// <summary>
    /// Main entry point and orchestration for bot.
    /// </summary>
    public class IndianBank_ChatBOT : IBot, IDisposable
    {
        private DialogSet _dialogs;
        private readonly AppDbContext dbContext;
        private bool dbContextDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndianBank_ChatBOT"/> class.
        /// </summary>
        /// <param name="botServices">Bot services.</param>
        /// <param name="conversationState">Bot conversation state.</param>
        /// <param name="userState">Bot user state.</param>
        public IndianBank_ChatBOT(BotServices botServices, ConversationState conversationState, 
            IOptions<AppSettings> appSettings, AppDbContext dbContext, IMemoryCache memoryCache, IHttpClientFactory clientFactory)
        {
            this.dbContext = dbContext;

            _dialogs = new DialogSet(conversationState.CreateProperty<DialogState>(nameof(IndianBank_ChatBOT)));

            _dialogs.Add(new MainDialog(botServices, appSettings.Value, dbContext, memoryCache, clientFactory));
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

        protected virtual void Dispose(bool disposing)
        {
            if (!dbContextDisposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }

                dbContextDisposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
