using System.Collections.Generic;
using System.IO;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.TemplateManager;
using Microsoft.Bot.Schema;

namespace IndianBank_ChatBOT.Dialogs.Rates
{
    public class RatesResponses : TemplateManager
    {
        #region Properties

        private static LanguageTemplateDictionary _responseTemplates = new LanguageTemplateDictionary
        {
            ["default"] = new TemplateIdMap
            {
                {RateResponseIds.RatesCardDisplay,(context, data) => BuildrateCardDisplay(context,data) }
            }
        };

        public static Dictionary<string, RatesData> keyValuePairs = new Dictionary<string, RatesData>
        {
            //Loan Menu Item
            {"deposit rates",new RatesData{RatesDataTitle="Deposit Rates",RatesDataText="To Know more about the current deposit rate. Please click on Read More",RatesDataLink=RateResponseLinks.DepositRatesLink,RatesDataImagePath=Path.Combine(".", @"Resources\rates", "depositRates.jpg")} },
            {"lending rates",new RatesData{RatesDataTitle="Lending Rates",RatesDataText="To Know more about the current lending rates. Please click on Read More",RatesDataLink=RateResponseLinks.LendingRatesLink,RatesDataImagePath=Path.Combine(".", @"Resources\rates", "lendingRates.jpg")} },
            {"service charges / forex rates",new RatesData{RatesDataTitle="Service Charges / Forex Rates",RatesDataText="To Know more about the current Service Charges or Forex Rates. Please click on Read More",RatesDataLink=RateResponseLinks.ServiceChargesLink,RatesDataImagePath=Path.Combine(".", @"Resources\rates", "serviceCharges.jpg")} }

        };

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RatesResponses"/> class.
        /// </summary>
        public RatesResponses()
        {
            Register(new DictionaryRenderer(_responseTemplates));
        }

        #endregion

        #region Methods

        public static IMessageActivity BuildrateCardDisplay(ITurnContext turnContext, RatesData data)
        {
            var attachment = new HeroCard()
            {
                Title = data.RatesDataTitle,
                Text = data.RatesDataText,
                Images = new List<CardImage> { new CardImage(data.RatesDataImagePath) },
                Buttons = new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.OpenUrl, title: "Read More", value: data.RatesDataLink)
                }
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        public static RatesData getRatesData(string result)
        {
            RatesData loanData = new RatesData();
            var res = keyValuePairs.TryGetValue(result, out loanData);
            return loanData;
        }

        #endregion

        #region Class

        public class RateResponseIds
        {
            // Constants
            public const string RatesCardDisplay = "buildRatesCardDisplay";

        }

        public class RateResponseLinks
        {
            // Links
            public const string DepositRatesLink = "https://www.indianbank.in/departments/deposit-rates/#!";
            public const string LendingRatesLink = "https://www.indianbank.in/departments/lending-rates/#!";
            public const string ServiceChargesLink = "https://www.indianbank.in/service-charges-forex-rates/#!";
        }

        public class RatesData
        {
            public string RatesDataTitle { get; set; }
            public string RatesDataText { get; set; }
            public string RatesDataLink { get; set; }
            public string RatesDataImagePath { get; set; }
        }

        #endregion
    }
}
