using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Dialogs.Loans
{
    public class EducationDialog
    {
        #region Properties

        private static LoansResponses _loanResponder = new LoansResponses();
        private static LoansResponses.LoanData _loanData = new LoansResponses.LoanData();

        #endregion

        #region Methods

        public static async void BuildEducationSubMenuCard(ITurnContext turnContext, RecognizerResult result)
        {
            try
            {
                string entityName = string.Empty;
                string entityType = string.Empty;
                string educationEntity = "eductaion_loan_entity";

                if (result.Entities[educationEntity] != null)
                {
                    entityType = educationEntity;
                    entityName = (string)result.Entities[educationEntity].Values<string>().FirstOrDefault();
                }

                if (entityType.Equals(educationEntity))
                {
                    switch (entityName)
                    {
                        case LoansEntities.ModelEducationalLoan:
                            {
                                _loanData = LoansResponses.getLoansData(LoansEntities.ModelEducationalLoan);
                                await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, _loanData);
                                break;
                            }
                        case LoansEntities.IBEducationalLoanPrime:
                            {
                                _loanData = LoansResponses.getLoansData(LoansEntities.IBEducationalLoanPrime);
                                await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, _loanData);
                                break;
                            }

                        case LoansEntities.IBSkillLoanScheme:
                            {
                                _loanData = LoansResponses.getLoansData(LoansEntities.IBSkillLoanScheme);
                                await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.BuildLoansCard, _loanData);
                                break;
                            }
                        case LoansEntities.EducationLoanInterest:
                            {
                                _loanData = LoansResponses.getLoansData(LoansEntities.EducationLoanInterest);
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
                    await turnContext.SendActivityAsync("Please find the types of Education Loans.\n Select any Education Loan type to proceed further.");
                    await _loanResponder.ReplyWith(turnContext, LoansResponses.LoanResponseIds.EducationLoansMenuCard);
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
