// using System;
// using System.Linq;

// using Microsoft.Bot.Builder;

// namespace UjjivanBank_ChatBOT.Dialogs.Loans
// {
//     public class NRIDialog
//     {
//         #region Properties

//         private static LoansResponses _loanResponder = new LoansResponses();
//         private static LoansResponses.LoanData _loanData = new LoansResponses.LoanData();

//         #endregion

//         #region Methods

//         public static async void BuildNRISubMenuCard(ITurnContext turnContext, RecognizerResult result)
//         {
//             try
//             {
//                 string entityName = string.Empty;
//                 string entityType = string.Empty;
//                 string nriEntity = "nri_loan_entity";

//                 if (result.Entities[nriEntity] != null)
//                 {
//                     entityType = nriEntity;
//                     entityName = (string)result.Entities[nriEntity].Values<string>().FirstOrDefault();
//                 }

//                 if (entityType.Equals(nriEntity))
//                 {
//                     switch (entityName)
//                     {
//                         case LoansEntities.NRIPlotLoan:
//                             {
//                                 _loanData = LoansResponses.getLoansData(LoansEntities.NRIPlotLoan);
//                                 await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, _loanData);
//                                 break;
//                             }
//                         case LoansEntities.NRIHomeLoan:
//                             {
//                                 _loanData = LoansResponses.getLoansData(LoansEntities.NRIHomeLoan);
//                                 await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, _loanData);
//                                 break;
//                             }
//                         default:
//                             {
//                                 await turnContext.SendActivityAsync("Sorry, I didn't understand. Please try with different query");
//                                 break;
//                             }
//                     }
//                 }
//                 else
//                 {
//                     await turnContext.SendActivityAsync("Please find the types of NRI Loans.\n Select any NRI Loan type to proceed further.");
//                     await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.NRILoansMenuCard);
//                 }

//             }
//             catch (Exception e)
//             {
//                 System.Diagnostics.Debug.WriteLine(e);
//             }
//         }

//         #endregion
//     }
// }
