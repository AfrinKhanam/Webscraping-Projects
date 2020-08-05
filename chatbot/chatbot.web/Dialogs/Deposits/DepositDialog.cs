using System.Collections.Generic;
using System.Linq;

using Microsoft.Bot.Builder;

namespace IndianBank_ChatBOT.Dialogs.Deposits
{
    public class DepositDialog
    {
        #region Properties

        private static DepositResponses _depositResponder = new DepositResponses();
        private static DepositResponses.DepositData depositData = new DepositResponses.DepositData();

        #endregion

        #region Methods  

        /// <summary>
        /// Builds the deposit sub menu card.
        /// </summary>
        /// <param name="turnContext">The turn context.</param>
        /// <param name="result">The result.</param>
        public static async void BuildDepositSubMenuCard(ITurnContext turnContext, RecognizerResult result)
        {

            string entityName = string.Empty;
            string entityType = string.Empty;
            List<string> entityTypes = new List<string>
                {
                    "deposits_entity",
                    "savings_bank_account_entity",
                    "current_account_entity",
                    "term_deposit_entity",
                    "nri_account_entity"

                };
            //to fetch entity name and entity type
            foreach (var entity in entityTypes)
            {
                if (result.Entities[entity] != null)
                {
                    entityType = entity;
                    entityName = (string)result.Entities[entity].Values<string>().FirstOrDefault();
                    break;
                }
            }

            if (entityType.Equals("deposits_entity"))
            {
                switch (entityName)
                {
                    case DepositEntities.SavingsBankAccount:
                        {
                            await turnContext.SendActivityAsync("Please find the types of Savings Bank Account. Select any Savings Bank Account type to proceed further.");
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.SavingsBankAccountMenuCardDisplay);
                            break;
                        }
                    case DepositEntities.CurrentAccountTypes:
                        {
                            await turnContext.SendActivityAsync("Please find the types of Current Account. Select any Current Account type to proceed further.");
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.CurrentAccountMenuCardDisplay);
                            break;
                        }
                    case DepositEntities.TermDeposits:
                        {
                            await turnContext.SendActivityAsync("Please find the types of Term Deposit Account. Select any Term Deposit Account type to proceed further.");
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.TermDepositMenuCardDisplay);
                            break;
                        }
                    case DepositEntities.NriAccounts:
                        {
                            await turnContext.SendActivityAsync("Please find the types of NRI Account. Select any NRI Account type to proceed further.");
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.NriAccountsMenuCardDisplay);
                            break;
                        }

                    default:
                        {
                            await turnContext.SendActivityAsync("Sorry, I didn't understand. Please try with different query");
                            break;
                        }
                }
            }
            else if (entityType.Equals("savings_bank_account_entity"))
            {
                switch (entityName)
                {
                    case DepositEntities.SavingsBank:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.IbCorpSbPayrollPackage:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.VikasSavingsKhata:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.IbSmartKid:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.SavingsTermsAndConditions:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.SbPlatinum:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.IbSurabhi:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    default:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                }
            }
            else if (entityType.Equals("current_account_entity"))
            {
                switch (entityName)
                {
                    case DepositEntities.CurrentAccount:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.FreedomCurrentAccount:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.CurrentTermsAndConditions:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.PremiumCurrentAccount:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    default:
                        {
                            await turnContext.SendActivityAsync("Sorry, I didn't understand. Please try with different query");
                            break;
                        }
                }
            }
            else if (entityType.Equals("term_deposit_entity"))
            {
                switch (entityName)
                {
                    case DepositEntities.FacilityDeposit:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.CapitalGains:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.TermConditionsOfTermDeposit:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.DepositSchemeForSeniorCitizens:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.RecurringDeposit:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.IbTaxSaverScheme:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.InsuredRecurringDeposit:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.ReInvestmentPlan:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.FixedDeposit:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.VariableRecurringDeposit:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    default:
                        {
                            await turnContext.SendActivityAsync("Sorry, I didn't understand. Please try with different query");
                            break;
                        }
                }
            }
            else if (entityType.Equals("nri_account_entity"))
            {
                switch (entityName)
                {
                    case DepositEntities.ForeignCurrencyForReturningIndians:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.FdRipRdAccounts:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.NreSbAccounts:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.NonResidentOrdinaryAccount:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
                            break;
                        }
                    case DepositEntities.FcnrAccounts:
                        {
                            depositData = DepositResponses.getDepositData(entityName);
                            await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositsSubMenuCardDisplay, depositData);
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
                await turnContext.SendActivityAsync("Please find the types of Deposits. Select any Deposit type to proceed further.");
                await _depositResponder.ReplyWith(turnContext, DepositResponses.ResponseIds.DepositMenuCardDisplay);
            }
        }

        #endregion
    }
}
