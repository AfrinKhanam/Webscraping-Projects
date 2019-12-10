using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Dialogs.Loans
{
    public class GroupsDialog
    {
        private static LoansResponses _newsInfoResponder = new LoansResponses();
        private static LoansResponses _loansResponses = new LoansResponses();
        static LoansResponses.LoanData loanData = new LoansResponses.LoanData();
        public static async void BuildGroupsSubMenuCard(ITurnContext turnContext, string EntityName)
        {
            switch (EntityName)
            {
                case LoanEntities.GroupsAgriculturalGodowns:
                    {
                        loanData = LoansResponses.getLoansData(EntityName);
                        await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
                        break;
                    }
                case LoanEntities.Groups_SGH_BankLinkageProgramme:
                    {
                        loanData = LoansResponses.getLoansData(EntityName);
                        await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
                        break;
                    }
                case LoanEntities.Groups_SGH_VidhyaShoba:
                    {
                        loanData = LoansResponses.getLoansData(EntityName);
                        await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
                        break;
                    }
                default:
                    {
                        await turnContext.SendActivityAsync("Sorry, I didn't understand. Please try with different query");
                        break;
                    }
            }
        }
    }
}
