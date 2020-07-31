// using Microsoft.Bot.Builder;

// namespace UjjivanBank_ChatBOT.Dialogs.Loans
// {
//     public class PersonalIndividualDialog
//     {
//         private static LoansResponses _newsInfoResponder = new LoansResponses();
//         private static LoansResponses _loansResponses = new LoansResponses();
//         static LoansResponses.LoanData loanData = new LoansResponses.LoanData();
//         public static async void BuildPersonalIndividualSubMenuCard(ITurnContext turnContext, string EntityName)
//         {
//             switch (EntityName)
//             {
//                 case LoanEntities.IbHomeLoanCombo:
//                     {
//                         loanData = LoansResponses.getLoansData(EntityName);
//                         await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
//                         break;
//                     }
//                 case LoanEntities.IbRentEncash:
//                     {
//                         loanData = LoansResponses.getLoansData(EntityName);
//                         await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
//                         break;
//                     }
//                 case LoanEntities.LoanOD_AgainstDeposits:
//                     {
//                         loanData = LoansResponses.getLoansData(EntityName);
//                         await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
//                         break;
//                     }
//                 case LoanEntities.IbCleanLoan:
//                     {
//                         loanData = LoansResponses.getLoansData(EntityName);
//                         await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
//                         break;
//                     }
//                 case LoanEntities.IbBalavidhyaScheme:
//                     {
//                         loanData = LoansResponses.getLoansData(EntityName);
//                         await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
//                         break;
//                     }
//                 case LoanEntities.IndReverseMortgage:
//                     {
//                         loanData = LoansResponses.getLoansData(EntityName);
//                         await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
//                         break;
//                     }
//                 case LoanEntities.IbVehicleloan:
//                     {
//                         loanData = LoansResponses.getLoansData(EntityName);
//                         await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
//                         break;
//                     }
//                 case LoanEntities.IndMortgage:
//                     {
//                         loanData = LoansResponses.getLoansData(EntityName);
//                         await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
//                         break;
//                     }
//                 case LoanEntities.PlotLoan:
//                     {
//                         loanData = LoansResponses.getLoansData(EntityName);
//                         await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
//                         break;
//                     }
//                 case LoanEntities.IbHomeLoan:
//                     {
//                         loanData = LoansResponses.getLoansData(EntityName);
//                         await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
//                         break;
//                     }
//                 case LoanEntities.IbPensionLoan:
//                     {
//                         loanData = LoansResponses.getLoansData(EntityName);
//                         await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
//                         break;
//                     }
//                 case LoanEntities.HomeImprove:
//                     {
//                         loanData = LoansResponses.getLoansData(EntityName);
//                         await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
//                         break;
//                     }
//                 case LoanEntities.IbHomeLoanPlus:
//                     {
//                         loanData = LoansResponses.getLoansData(EntityName);
//                         await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
//                         break;
//                     }
//                 case LoanEntities.LoanOD_AgainstNSC:
//                     {
//                         loanData = LoansResponses.getLoansData(EntityName);
//                         await _loansResponses.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, loanData);
//                         break;
//                     }
//                 default:
//                     {
//                         await turnContext.SendActivityAsync("Sorry, I didn't understand. Please try with different query");
//                         break;
//                     }
//             }
//         }
//     }
// }
