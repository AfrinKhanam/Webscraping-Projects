using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace IndianBank_ChatBOT.Middleware
{
    public class SetLocaleMiddleware : IMiddleware
    {
        private readonly string defaultLocale;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetLocaleMiddleware"/> class.
        /// </summary>
        /// <param name="defaultDefaultLocale">The default default locale.</param>
        public SetLocaleMiddleware(string defaultDefaultLocale)
        {
            defaultLocale = defaultDefaultLocale;
        }

        /// <summary>
        /// Called when /[turn asynchronous].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="next">The next.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task OnTurnAsync(ITurnContext context, NextDelegate next, CancellationToken cancellationToken = default(CancellationToken))
        {
            var cultureInfo = context.Activity.Locale != null ? new CultureInfo(context.Activity.Locale) : new CultureInfo(defaultLocale);
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture = cultureInfo;

            await next(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sets the locale asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="conversationState">State of the conversation.</param>
        /// <param name="localstorage">The localstorage.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        public static async Task<CultureInfo> SetLocaleAsync(ITurnContext context, ConversationState conversationState, string localstorage, CancellationToken cancellation = default(CancellationToken))
        {
            var storedLocalstorageProperty = conversationState.CreateProperty<string>("localstorage");

            await storedLocalstorageProperty.SetAsync(context, localstorage, cancellation);
            await conversationState.SaveChangesAsync(context);

            context.Activity.Locale = localstorage;

            var eventActivity = context.Activity.CreateReply();
            eventActivity.Type = ActivityTypes.Event;
            eventActivity.ValueType = nameof(localstorage);
            eventActivity.Value = localstorage;

            await context.SendActivityAsync(eventActivity);

            return new CultureInfo(localstorage);
        }
    }
}
