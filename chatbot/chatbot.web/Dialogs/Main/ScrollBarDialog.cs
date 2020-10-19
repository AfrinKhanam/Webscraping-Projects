using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;

using IndianBank_ChatBOT.Dialogs.Shared;
using IndianBank_ChatBOT.Migrations;
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
    public class ScrollBarDialog
    {
        public static async Task DisplayScrollBarMenu(DialogContext dialogContext, SelectedMenuItem selectedMenu,
            AppSettings appSettings, IMemoryCache memoryCache, IHttpClientFactory clientFactory)
        {
            var menuManager = new MenuManager(appSettings.MenuItemsUrl, memoryCache);

            var menuItems = await menuManager.GetMenuItems();

            var text = selectedMenu.Text;

            if (selectedMenu.ParentMenuItems == null)
            {
                selectedMenu.ParentMenuItems = new[] { text };
            }
            else
            {
                selectedMenu.ParentMenuItems = selectedMenu.ParentMenuItems.Append(text).ToArray();
            }

            MenuViewModel menuItem = null;

            for (int i = 0; i < selectedMenu.ParentMenuItems.Length; i++)
            {
                if (i == 0)
                {
                    menuItem = menuItems.SingleOrDefault(m => m.Text == selectedMenu.ParentMenuItems[i]);
                }
                else
                {
                    menuItem = menuItem.ChildItems.SingleOrDefault(m => m.Text == selectedMenu.ParentMenuItems[i]);
                }
            }

            if (menuItem != null)
            {
                if (menuItem.ChildItems?.Length > 0)
                {
                    await DisplaySubMenu(dialogContext, menuItem.ChildItems);
                }
                else
                {
                    await DisplayMenuResult(dialogContext, menuItem, appSettings.QAEndPoint, clientFactory);
                }
            }

            await dialogContext.EndDialogAsync();
        }

        private static async Task DisplayMenuResult(DialogContext dialogContext, MenuViewModel menuItem, string qaEndPoint, IHttpClientFactory clientFactory)
        {
            var searchTerms = new List<string>(new[]
            {
                // Menu Text
                menuItem.Text,

                // Try and make a search term out of the page name in the url.
                // For example '/departments/executive-directors-profile/' can be changed to the search term 'executive directors profile'
                menuItem.Url.Split('/')?.Where(url => !string.IsNullOrWhiteSpace(url))?.Last()?.Replace('-', ' ')
            });

            if (menuItem.Parents != null && menuItem.Parents.Length > 0)
            {
                // A word soup with all the parent menu items and the current menu text. Might work as a last resort.
                searchTerms.Add(string.Join(" ", menuItem.Parents?.Append(menuItem.Text)));
            }

            var isSearchSucceeded = await SearchKnowledgeBase(dialogContext, searchTerms, menuItem, qaEndPoint, clientFactory);

            if (!isSearchSucceeded)
            {
                dialogContext.Context.Activity.Conversation.Properties.Add(nameof(ExtendedLogData), JToken.FromObject(new ExtendedLogData
                {
                    IntentName = "menu",
                    SubTitle = menuItem.Parents == null ? menuItem.Text : string.Join("=>", menuItem.Parents.Append(menuItem.Text)),
                    ResponseJson = null,
                    ResponseSource = ResponseSource.Menu
                }));

                var heroCard = new HeroCard
                {
                    Title = menuItem.Text,
                    Subtitle = "For further details please click on the link below:",
                    Text = $"[{menuItem.Url}]({menuItem.Url})",
                };

                var activity = MessageFactory.Attachment(heroCard.ToAttachment());

                await dialogContext.Context.SendActivityAsync(activity);
            }
        }

        private static async Task<bool> SearchKnowledgeBase(DialogContext dialogContext, IEnumerable<string> searchTerms,
            MenuViewModel menuItem, string qaEndPoint, IHttpClientFactory clientFactory)
        {
            foreach (var searchTerm in searchTerms)
            {
                var kbResponse = await MainDialog.GetKnowledgeBaseResults(searchTerm, qaEndPoint, clientFactory);

                var kbResult = JsonConvert.DeserializeObject<KnowledgeBaseResult>(kbResponse);

                var matchingDocs = kbResult.DOCUMENTS.Where(d => IsUrlMatching(d.url, menuItem.Url)).ToArray();

                if (matchingDocs != null && matchingDocs.Any())
                {
                    dialogContext.Context.Activity.Conversation.Properties.Add(nameof(ExtendedLogData), JToken.FromObject(new ExtendedLogData
                    {
                        IntentName = "menu",
                        SubTitle = string.Join("=>", menuItem.Parents.Append(menuItem.Text)),
                        ResponseJson = kbResponse,
                        ResponseSource = ResponseSource.ElasticSearch
                    }));

                    var heroCard = new HeroCard
                    {
                        Title = menuItem.Text,
                        Subtitle = string.Join("\n\n", matchingDocs.Select(d => d.value)),
                        Text = $@"For further details please visit the page [{menuItem.Url}]({menuItem.Url})",
                    };

                    var activity = MessageFactory.Attachment(heroCard.ToAttachment());

                    await dialogContext.Context.SendActivityAsync(activity);

                    return true;
                }
            }

            return false;
        }

        private static bool IsUrlMatching(string url1, string url2)
        {
            return Uri.Compare(
                new Uri(url1), new Uri(url2),
                UriComponents.Path,
                UriFormat.SafeUnescaped,
                StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        private static async Task DisplaySubMenu(DialogContext dialogContext, MenuViewModel[] childItems)
        {
            var items = childItems.Select(i => new CardAction
            {
                Title = i.Text,
                DisplayText = i.Text,
                Type = ActionTypes.PostBack,
                Value = new
                {
                    action = "menu",
                    parentMenuItems = i.Parents,
                    text = i.Text
                },
            }).ToList();

            var attachment = new HeroCard()
            {
                Buttons = items
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);

            await dialogContext.Context.SendActivityAsync(response);
        }

        public static async void DisplayScrollBarMenu(DialogContext dialogContext, string EntityName, string qaEndPoint, IHttpClientFactory clientFactory)
        {
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
                    await MainDialog.SearchKB(dialogContext, qaEndPoint, clientFactory);
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
