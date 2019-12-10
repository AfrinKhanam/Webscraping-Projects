using IndianBank_ChatBOT.Dialogs.Loans;
using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Dialogs.Loans
{
    public class AgricultureDialog
    {
            private static LoansResponses _loansResponses = new LoansResponses();
            static LoansResponses.LoanData loanData = new LoansResponses.LoanData();
        public static async void BuildAgricultureSubMenuCard(ITurnContext turnContext,string EntityName)
        {
            switch (EntityName)
            {
                case LoanEntities.AgriculturalGodowns:
                    {
                        loanData = LoansResponses.getLoansData(EntityName);
                        await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
                        break;
                    }
                case LoanEntities.LoansForMaintenanceOfTractors:
                    {
                        loanData = LoansResponses.getLoansData(EntityName);
                        await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
                        break;
                    }
                case LoanEntities.AgriculturalProduceMarketingLoan:
                    {
                        loanData = LoansResponses.getLoansData(EntityName);
                        await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
                        break;
                    }
                case LoanEntities.FinancingAgriculturistsForPurchaseOfTractors:
                    {
                        loanData = LoansResponses.getLoansData(EntityName);
                        await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
                        break;
                    }
                case LoanEntities.PurchaseSecondHandTractors:
                    {
                        loanData = LoansResponses.getLoansData(EntityName);
                        await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
                        break;
                    }
                case LoanEntities.AgriClinicAgriBusinessCentres:
                    {
                        loanData = LoansResponses.getLoansData(EntityName);
                        await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
                        break;
                    }
                case LoanEntities.SHG_BankLinkageProgramme:
                    {
                        loanData = LoansResponses.getLoansData(EntityName);
                        await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
                        break;
                    }
                case LoanEntities.JointLiabilityGroup:
                    {
                        loanData = LoansResponses.getLoansData(EntityName);
                        await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
                        break;
                    }
                case LoanEntities.RupayKisanCard:
                    {
                        loanData = LoansResponses.getLoansData(EntityName);
                        await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
                        break;
                    }
                case LoanEntities.DRI_SchemeRevisedNorms:
                    {
                        loanData = LoansResponses.getLoansData(EntityName);
                        await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
                        break;
                    }
                case LoanEntities.SHG_VidhyaShoba:
                    {
                        loanData = LoansResponses.getLoansData(EntityName);
                        await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
                        break;
                    }
                case LoanEntities.GraminMahilaSowbhagyaScheme:
                    {
                        loanData = LoansResponses.getLoansData(EntityName);
                        await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
                        break;
                    }
                case LoanEntities.SugarPremiumScheme:
                    {
                        loanData = LoansResponses.getLoansData(EntityName);
                        await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
                        break;
                    }
                case LoanEntities.GoldenHarvestScheme:
                    {
                        loanData = LoansResponses.getLoansData(EntityName);
                        await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
                        break;
                    }
                case LoanEntities.AgriculturalJewelLoanScheme:
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
