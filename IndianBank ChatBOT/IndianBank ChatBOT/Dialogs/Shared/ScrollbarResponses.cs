using Microsoft.Bot.Builder.TemplateManager;
using Microsoft.Bot.Schema;
using System.Collections.Generic;

namespace IndianBank_ChatBOT.Dialogs.Shared
{
    /// <summary>
    /// ScrollbarResponses class
    /// </summary>
    public class ScrollbarResponses : TemplateManager
    {
        #region Methods

        public static SuggestedActions SuggestedActionsForAboutUs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Profile", value: "profiles"),
                    new CardAction(type: ActionTypes.ImBack, title: "Vision & Mission", value: "vision and mission"),
                    new CardAction(type: ActionTypes.ImBack, title: "Management", value: "management"),
                    //new CardAction(type: ActionTypes.ImBack, title: "Finance Result", value: "finance result"),
                    new CardAction(type: ActionTypes.ImBack, title: "Corporate Governance", value: "corporate governance"),
                    new CardAction(type: ActionTypes.ImBack, title: "Mutual Fund", value: "mutual fund"),
                    new CardAction(type: ActionTypes.ImBack, title: "Annual Report", value: "Annual Report")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForProduct
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Loan Products", value: "loan products"),
                    new CardAction(type: ActionTypes.ImBack, title: "Deposit Products", value: "deposit products"),
                    new CardAction(type: ActionTypes.ImBack, title: "Digital Products", value: "digital products"),
                    new CardAction(type: ActionTypes.ImBack, title: "Feature Products", value: "feature products"),
                });
            }
        }

        public static SuggestedActions SuggestedActionsForServices
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Premium Services", value: "premium services"),
                    new CardAction(type: ActionTypes.ImBack, title: "Insurance Services", value: "insurance services"),
                    new CardAction(type: ActionTypes.ImBack, title: "CMS Plus", value: "cms plus"),
                    new CardAction(type: ActionTypes.ImBack, title: "Doorstep Banking", value: "doorstep banking"),
                    new CardAction(type: ActionTypes.ImBack, title: "Tax Payment", value: "tax payment"),
                    new CardAction(type: ActionTypes.ImBack, title: "Debenture Trust", value: "debenture trust")
                });
            }
        }
        public static SuggestedActions SuggestedActionsForRates
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Deposit Rates", value: "deposit rates"),
                    new CardAction(type: ActionTypes.ImBack, title: "Lending Rates", value: "lending rates"),
                    new CardAction(type: ActionTypes.ImBack, title: "Service Charges", value: "service charges")
                });
            }
        }
        public static SuggestedActions SuggestedActionsForContacts
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Customer Support", value: "customer support"),
                    new CardAction(type: ActionTypes.ImBack, title: "Email ID", value: "email id")
                });
            }
        }
        public static SuggestedActions SuggestedActionsForLinks
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Online Services", value: "online services"),
                    new CardAction(type: ActionTypes.ImBack, title: "Related Sites", value: "related sites"),
                    new CardAction(type: ActionTypes.ImBack, title: "Alliances", value: "alliances")
                });
            }
        }

        #endregion
    }
}
