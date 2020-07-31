// using System;
// using System.Linq;

// using UjjivanBank_ChatBOT.Dialogs.Main;

// using Microsoft.Bot.Builder;

// namespace UjjivanBank_ChatBOT.Dialogs.Rates
// {
//     public class RatesDialog
//     {
//         #region Properties

//         private static RatesResponses _rateResponder = new RatesResponses();
//         private static MainResponses _responder = new MainResponses();
//         private static RatesResponses.RatesData _ratesData = new RatesResponses.RatesData();


//         #endregion

//         #region Methods

//         /// <summary>
//         /// Builds the rates sub menu card.
//         /// </summary>
//         /// <param name="turnContext">The turn context.</param>
//         /// <param name="result">The result.</param>
//         public static async void BuildRatesSubMenuCard(ITurnContext turnContext, RecognizerResult result)
//         {
//             try
//             {
//                 string entityName = string.Empty;
//                 string entityType = string.Empty;
//                 string rateEntity = "rates_entity";

//                 if (result.Entities[rateEntity] != null)
//                 {
//                     entityType = rateEntity;
//                     entityName = (string)result.Entities[rateEntity].Values<string>().FirstOrDefault();
//                 }

//                 if (entityType.Equals(rateEntity))
//                 {
//                     switch (entityName)
//                     {
//                         case RateEntities.DepositRates:
//                             {
//                                 _ratesData = RatesResponses.getRatesData(RateEntities.DepositRates);
//                                 await _rateResponder.ReplyWith(turnContext, RatesResponses.RateResponseIds.RatesCardDisplay, _ratesData);
//                                 break;
//                             }
//                         case RateEntities.LendingRates:
//                             {
//                                 _ratesData = RatesResponses.getRatesData(RateEntities.LendingRates);
//                                 await _rateResponder.ReplyWith(turnContext, RatesResponses.RateResponseIds.RatesCardDisplay, _ratesData);
//                                 break;
//                             }

//                         case RateEntities.ServiceCharges:
//                             {
//                                 _ratesData = RatesResponses.getRatesData(RateEntities.ServiceCharges);
//                                 await _rateResponder.ReplyWith(turnContext, RatesResponses.RateResponseIds.RatesCardDisplay, _ratesData);
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
//                     await turnContext.SendActivityAsync("Please find the types of Rates.\n Select any Rates type to proceed further.");
//                     await _responder.ReplyWith(turnContext, MainResponses.ResponseIds.RatesMenuCardDisplay);
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
