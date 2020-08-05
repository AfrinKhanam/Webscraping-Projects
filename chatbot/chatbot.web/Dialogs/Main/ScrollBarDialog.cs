using System.Collections.Generic;
using System.Net.Http;

using IndianBank_ChatBOT.Dialogs.Shared;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace IndianBank_ChatBOT.Dialogs.Main
{
    public class ScrollBarDialog
    {
        public static async void DisplayScrollBarMenu(DialogContext dialogContext, string EntityName, IHttpClientFactory clientFactory)
        {
            //await dialogContext.Context.SendActivityAsync($"This is what I received from main Dialog {EntityName}");

            switch (EntityName)
            {
                case ScrollbarEntities.AboutUs:
                    {
                        BuildScrollBarMenu(dialogContext, ScrollbarResponses.SuggestedActionsForAboutUs.Actions);
                        break;
                    }
                case ScrollbarEntities.Products:
                    {
                        BuildScrollBarMenu(dialogContext, ScrollbarResponses.SuggestedActionsForProduct.Actions);
                        break;
                    }
                case ScrollbarEntities.Services:
                    {
                        BuildScrollBarMenu(dialogContext, ScrollbarResponses.SuggestedActionsForServices.Actions);
                        break;
                    }
                case ScrollbarEntities.Rates:
                    {
                        BuildScrollBarMenu(dialogContext, ScrollbarResponses.SuggestedActionsForRates.Actions);
                        break;
                    }
                case ScrollbarEntities.Contacts:
                    {
                        BuildScrollBarMenu(dialogContext, ScrollbarResponses.SuggestedActionsForContacts.Actions);
                        break;
                    }
                case ScrollbarEntities.Links:
                    {
                        BuildScrollBarMenu(dialogContext, ScrollbarResponses.SuggestedActionsForLinks.Actions);
                        break;
                    }
                default:
                    await MainDialog.SearchKB(dialogContext, clientFactory);

                    // await dialogContext.Context.SendActivityAsync("Sorry!! I could not understand the query. Could you please rephrase it and try again.");
                    break;
            }
        }

        public static async void BuildScrollBarMenu(DialogContext dialogContext, IList<CardAction> cardAction)
        {
            var attachment = new HeroCard()
            {
                Buttons = cardAction
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            await dialogContext.Context.SendActivityAsync(response);
        }
    }


}
