using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Dialogs.Loans
{
    public class MSMEDialog
    {
        #region Properties

        private static LoansResponses _loanResponder = new LoansResponses();
        private static LoansResponses.LoanData _loanData = new LoansResponses.LoanData();

        #endregion

        #region Methods

        public static async void BuildMSMESubMenuCard(ITurnContext turnContext, RecognizerResult result)
        {
            try
            {
                string entityName = string.Empty;
                string entityType = string.Empty;
                string msmeEntity = "msme_entity";

                if (result.Entities[msmeEntity] != null)
                {
                    entityType = msmeEntity;
                    entityName = (string)result.Entities[msmeEntity].Values<string>().FirstOrDefault();
                }

                if (entityType.Equals(msmeEntity))
                {
                    switch (entityName)
                    {
                        case LoansEntities.IBVidhyaMandir:
                            {
                                _loanData = LoansResponses.getLoansData(LoansEntities.IBVidhyaMandir);
                                await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, _loanData);
                                break;
                            }
                        case LoansEntities.IBMyOwnShop:
                            {
                                _loanData = LoansResponses.getLoansData(LoansEntities.IBMyOwnShop);
                                await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, _loanData);
                                break;
                            }

                        case LoansEntities.IBDoctorPlus:
                            {
                                _loanData = LoansResponses.getLoansData(LoansEntities.IBDoctorPlus);
                                await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, _loanData);
                                break;
                            }
                        case LoansEntities.IBContractors:
                            {
                                _loanData = LoansResponses.getLoansData(LoansEntities.IBContractors);
                                await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, _loanData);
                                break;
                            }
                        case LoansEntities.Tradewell:
                            {
                                _loanData = LoansResponses.getLoansData(LoansEntities.Tradewell);
                                await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, _loanData);
                                break;
                            }
                        case LoansEntities.IndSMESecure:
                            {
                                _loanData = LoansResponses.getLoansData(LoansEntities.IndSMESecure);
                                await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, _loanData);
                                break;
                            }
                        default:
                            {
                                await turnContext.SendActivityAsync("Sorry, I didn't understand. Please try with different query");
                                break;
                            }
                    }
                }
                else
                {
                    await turnContext.SendActivityAsync("Please find the types of Rates.\n Select any Rates type to proceed further.");
                    await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.MSMELoansMenuCard);
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        #endregion
    }
}
