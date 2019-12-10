// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using IndianBank_ChatBOT.Dialogs.Main.Resources;
using IndianBank_ChatBOT.Dialogs.Shared;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.TemplateManager;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace IndianBank_ChatBOT.Dialogs.Main
{
    /// <summary>
    /// MainResponses class
    /// </summary>
    /// <seealso cref="Microsoft.Bot.Builder.TemplateManager.TemplateManager" />
    public class MainResponses : TemplateManager
    {
        #region Properties

        private static LanguageTemplateDictionary _responseTemplates = new LanguageTemplateDictionary
        {
            ["default"] = new TemplateIdMap
            {
                { ResponseIds.Cancelled,
                    (context, data) =>
                    MessageFactory.Text(
                        text: MainStrings.CANCELLED,
                        ssml: MainStrings.CANCELLED,
                        inputHint: InputHints.AcceptingInput)
                },
                //{ ResponseIds.Completed,
                //    (context, data) =>
                //    MessageFactory.Text(
                //        text: "",
                //        //ssml: MainStrings.COMPLETED,
                //        inputHint: InputHints.AcceptingInput)
                //},
                //{ ResponseIds.Confused,
                //    (context, data) =>
                //    MessageFactory.Text(
                //        text: MainStrings.CONFUSED,
                //        ssml: MainStrings.CONFUSED,
                //        inputHint: InputHints.AcceptingInput)
                //},
                { ResponseIds.Greeting,
                    (context, data) =>
                    MessageFactory.Text(
                        text: MainStrings.GREETING,
                        ssml: MainStrings.GREETING,
                        inputHint: InputHints.AcceptingInput)
                },
                { ResponseIds.Intro, (context, data) => BuildIntroductionCard(context, data) },
                //{ ResponseIds.BuildEmiCalculatorCard, (context, data) => BuildEmiCalculatorCard(context, data) },
                { ResponseIds.BuildWelcomeMenuCard, (context, data) => BuildWelcomeMenuCard(context) },
                { ResponseIds.ServicesMenuCardDisplay, (context,data) => BuildServiceMenuCard(context) },
                { ResponseIds.RatesMenuCardDisplay, (context,data) => BuildRatesMenuCard(context) },
                { ResponseIds.ContactsMenuCardDisplay, (context,data) => BuildContactsMenuCard(context) },
                { ResponseIds.FeedBack, (context, data) => BuildFeedbackCard(context, data) },
            }
        };


        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainResponses"/> class.
        /// </summary>
        public MainResponses()
        {
            Register(new DictionaryRenderer(_responseTemplates));
        }
        #endregion

        #region Methods

        /// <summary>
        /// Code to display hero card for main menu
        /// </summary>
        /// <param name="turnContext"></param>
        /// <returns></returns>
        public static IMessageActivity BuildWelcomeMenuCard(ITurnContext turnContext)
        {
            var attachment = new HeroCard()
            {
                Buttons = SharedResponses.SuggestedActionsForMenu.Actions
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        /// <summary>
        /// Code to display hero card for contacts menu
        /// </summary>
        /// <param name="turnContext"></param>
        /// <returns></returns>
        public static IMessageActivity BuildContactsMenuCard(ITurnContext turnContext)
        {
            var attachment = new HeroCard()
            {
                Buttons = SharedResponses.SuggestedActionsForContactsMenu.Actions
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        /// <summary>
        /// Code to display hero card for services menu
        /// </summary>
        /// <param name="turnContext"></param>
        /// <returns></returns>
        public static IMessageActivity BuildServiceMenuCard(ITurnContext turnContext)
        {
            var attachment = new HeroCard()
            {
                Buttons = SharedResponses.SuggestedActionsForServicesMenu.Actions
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        /// <summary>
        /// Code to display hero card for RatesMenu
        /// </summary>
        /// <param name="turnContext"></param>
        /// <returns></returns>
        public static IMessageActivity BuildRatesMenuCard(ITurnContext turnContext)
        {
            var attachment = new HeroCard()
            {
                Buttons = SharedResponses.SuggestedActionsForRatesMenu.Actions
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        /// <summary>
        /// Builds the introduction card.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private static object BuildIntroductionCard(ITurnContext context, dynamic data)
        {
            var path = Path.Combine(".", "Resources", "welcomecard.json");
            var cardAttachment = CreateAdaptiveCardAttachment(path);
            var response = MessageFactory.Attachment(cardAttachment);
           // context.SendActivityAsync(response);
            return response;
        }
        //private static object BuildEmiCalculatorCard(ITurnContext context, dynamic data)
        //{
        //    var path = Path.Combine(".", "Resources", "EMI_Calculator.json");
        //    var cardAttachment = CreateAdaptiveCardAttachment(path);
        //    var response = MessageFactory.Attachment(cardAttachment);
        //    return response;
        //}

        public static object BuildFeedbackCard(ITurnContext context, dynamic data)
        {
            var path = Path.Combine(".", "Resources", "FeedbackForm.json");
            var cardAttachment = CreateAdaptiveCardAttachment(path);
            var response = MessageFactory.Attachment(cardAttachment);
            //context.SendActivityAsync(response);
            return response;
        }
        /// <summary>
        /// Creates the adaptive card attachment.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        private static Attachment CreateAdaptiveCardAttachment(string filePath)
        {
            var adaptiveCardJson = File.ReadAllText(filePath);
            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson)
            };

            return adaptiveCardAttachment;
        }

        #endregion

        #region Class

        public class ResponseIds
        {
            // Constants
            public const string BuildWelcomeMenuCard = "buildWelcomeMenuCard";
            public const string ServicesMenuCardDisplay = "servicesMenuCardDisplay";
            public const string RatesMenuCardDisplay = "ratesMenuCardDisplay";
           // public const string NewsOrInfoMenuCardDisplay = "newsOrInfoMenuCardDisplay";
            //public const string CustomerCornerMenuCardDisplay = "customerCornerMenuCardDisplay";
            //public const string RelatedInfoMenuCardDisplay = "relatedInfoMenuCardDisplay";
            //public const string CodesPolicyDisclosuresMenuCardDisplay = "codesPolicyDisclosuresMenuCardDisplay";
            //public const string ChartersSchemesMenuCardDisplay = "chartersSchemesMenuCardDisplay";
            public const string ContactsMenuCardDisplay = "contactsMenuCardDisplay";
            public const string Cancelled = "cancelled";
          //  public const string BuildEmiCalculatorCard = "buildEmiCalculatorCard";
            public const string Completed = "completed";
            public const string Confused = "confused";
            public const string Greeting = "greeting";
            public const string Help = "help";
            public const string Intro = "intro";
            public const string FeedBack = "feedback";
        }

        #endregion
    }
}