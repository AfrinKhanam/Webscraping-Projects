// using System.Collections.Generic;
// using System.Linq;

// using Microsoft.Bot.Builder;
// using Microsoft.Bot.Builder.Dialogs;

// namespace UjjivanBank_ChatBOT.Dialogs.Loans.Resources
// {
//     public class LoansDialog
//     {
//         private static LoansResponses _loanResponder = new LoansResponses();
//         private static LoansResponses.LoanData _loanData = new LoansResponses.LoanData();

//         public static async void BuildLoansSubMenuCard(ITurnContext turnContext, RecognizerResult result, DialogContext dc)
//         {

//             string entityName = string.Empty;
//             string entityType = string.Empty;
//             List<string> entityTypes = new List<string>
//                 {
//                     "loans_entity",
//                     "agriculture_entity",
//                     "groups_entity",
//                     "personal_individual_entity",
//                     "msme_entity",
//                     "eductaion_loan_entity",
//                     "nri_loan_entity"
//                 };

//             foreach (var entity in entityTypes)
//             {
//                 if (result.Entities[entity] != null)
//                 {
//                     entityType = entity;
//                     entityName = result.Entities[entity].Values<string>().FirstOrDefault();
//                     break;
//                 }
//             }

//             if (entityType.Equals("loans_entity"))
//             {
//                 switch (entityName)
//                 {
//                     case LoansEntities.Agriculture:
//                         {

//                             await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.AgricultureMenuCardDisplay);
//                             break;
//                         }
//                     case LoansEntities.Groups:
//                         {
//                             await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.GroupsMenuCardDisplay);
//                             break;
//                         }
//                     case LoansEntities.PersonalOrIndividual:
//                         {
//                             await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.PersonalIndividualCardDisplay);
//                             break;
//                         }
//                     case LoansEntities.MSME:
//                         {
//                             await turnContext.SendActivityAsync("Please find the types of MSME Loans.\n Select any MSME Loan type to proceed further.");
//                             await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.MSMELoansMenuCard);
//                             break;
//                         }
//                     case LoansEntities.Education:
//                         {
//                             await turnContext.SendActivityAsync("Please find the types of Education Loans.\n Select any Education Loan type to proceed further.");
//                             await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.EducationLoansMenuCard);
//                             break;
//                         }
//                     case LoansEntities.NRI:
//                         {
//                             await turnContext.SendActivityAsync("Please find the types of NRI Loans.\n Select any NRI Loan type to proceed further.");
//                             await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.NRILoansMenuCard);
//                             break;
//                         }
//                     case LoansEntities.FNMinutesLoan:
//                         {
//                             _loanData = LoansResponses.getLoansData(LoansEntities.FNMinutesLoan);
//                             await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, _loanData);
//                             break;
//                         }
//                     default:
//                         {

//                             await turnContext.SendActivityAsync("Sorry, I didn't understand. Please try with different query");

//                             break;
//                         }
//                 }
//             }
//             else if (entityType.Equals("agriculture_entity"))
//             {
//                 AgricultureDialog.BuildAgricultureSubMenuCard(turnContext, entityName);
//             }
//             else if (entityType.Equals("groups_entity"))
//             {
//                 GroupsDialog.BuildGroupsSubMenuCard(turnContext, entityName);
//             }
//             else if (entityType.Equals("personal_individual_entity"))
//             {
//                 PersonalIndividualDialog.BuildPersonalIndividualSubMenuCard(turnContext, entityName);
//             }
//             else if (entityType.Equals("msme_entity"))
//             {
//                 MSMEDialog.BuildMSMESubMenuCard(turnContext, result);
//             }
//             else if (entityType.Equals("eductaion_loan_entity"))
//             {
//                 EducationDialog.BuildEducationSubMenuCard(turnContext, result);
//             }
//             else if (entityType.Equals("nri_loan_entity"))
//             {
//                 NRIDialog.BuildNRISubMenuCard(turnContext, result);
//             }
//             else
//             {
//                 await turnContext.SendActivityAsync("Please find the sub menus of Loans. Select any Loans sub menu to proceed further.");
//                 //await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.MSMELoansMenuCard);
//                 await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.LoansMenuCardDisplay);
//             }



//         }


//     }

// }
