using System;
using System.Collections.Generic;
using System.Linq;

using IndianBank_ChatBOT.Dialogs.Main;

using Microsoft.Bot.Builder;

namespace IndianBank_ChatBOT.Dialogs.Services
{
    public class ServicesDialog
    {
        #region Properties

        private static ServicesResponses _serviceResponder = new ServicesResponses();
        private static MainResponses _responder = new MainResponses();
        public static ServicesResponses.ServicesData _servicesData = new ServicesResponses.ServicesData();

        #endregion

        #region Methods

        /// <summary>
        /// Builds the rates sub menu card.
        /// </summary>
        /// <param name="turnContext">The turn context.</param>
        /// <param name="result">The result.</param>
        public static async void BuildServicesSubMenuCard(ITurnContext turnContext, RecognizerResult result)
        {
            try
            {

                string entityName = string.Empty;
                string entityType = string.Empty;
                List<string> entityTypes = new List<string>
                {
                    "services_entity",
                    "premium_services_entity",
                    "insurance_services_entity"
                };

                foreach (var entity in entityTypes)
                {
                    if (result.Entities[entity] != null)
                    {
                        entityType = entity;
                        entityName = (string)result.Entities[entity].Values<string>().FirstOrDefault();
                    }
                }

                if (entityType.Equals("services_entity"))
                {
                    switch (entityName)
                    {
                        case ServicesEntities.PremiumServices:
                            {
                                await turnContext.SendActivityAsync("Please find the types of Premium Services.\n Select any premium services type to proceed further.");
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.PremiumServicesDisplay);
                                break;
                            }
                        case ServicesEntities.InsuranceServices:
                            {
                                await turnContext.SendActivityAsync("Please find the types of Insurance Services.\nSelect any insurance services type to proceed further.");
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.InsuranceServicesDisplay);
                                break;
                            }
                        case ServicesEntities.CMSPlus:
                            {
                                _servicesData = getServiceData(ServicesEntities.CMSPlus);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.EpaymentofDirectTaxes:
                            {
                                _servicesData = getServiceData(ServicesEntities.EpaymentofDirectTaxes);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.EpaymentofIndirectTaxes:
                            {
                                _servicesData = getServiceData(ServicesEntities.EpaymentofIndirectTaxes);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        default:
                            {
                                await turnContext.SendActivityAsync("Sorry, I didn't understand. Please try with different query");
                                break;
                            }
                    }
                }
                else if (entityType.Equals("premium_services_entity"))
                {
                    switch (entityName)
                    {
                        case ServicesEntities.MCAPayment:
                            {
                                _servicesData = getServiceData(ServicesEntities.MCAPayment);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.MoneyGram:
                            {
                                _servicesData = getServiceData(ServicesEntities.MoneyGram);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.ATMDebitCard:
                            {
                                _servicesData = getServiceData(ServicesEntities.ATMDebitCard);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.IndMobileBanking:
                            {
                                _servicesData = getServiceData(ServicesEntities.IndMobileBanking);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.IndNetBanking:
                            {
                                _servicesData = getServiceData(ServicesEntities.IndNetBanking);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.CreditCards:
                            {
                                _servicesData = getServiceData(ServicesEntities.CreditCards);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.XpressMoney:
                            {
                                _servicesData = getServiceData(ServicesEntities.XpressMoney);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.NEFT:
                            {
                                _servicesData = getServiceData(ServicesEntities.NEFT);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.IndJetRemit:
                            {
                                _servicesData = getServiceData(ServicesEntities.IndJetRemit);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.MulticityChequeFacility:
                            {
                                _servicesData = getServiceData(ServicesEntities.MulticityChequeFacility);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        default:
                            {
                                await turnContext.SendActivityAsync("Sorry, I didn't understand. Please try with different query");
                                break;
                            }
                    }
                }
                else if (entityType.Equals("insurance_services_entity"))
                {
                    switch (entityName)
                    {
                        case ServicesEntities.IBVidyarthiSuraksha:
                            {
                                _servicesData = getServiceData(ServicesEntities.IBVidyarthiSuraksha);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.IBHomeSecurity:
                            {
                                _servicesData = getServiceData(ServicesEntities.IBHomeSecurity);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.UniversalHealthCare:
                            {
                                _servicesData = getServiceData(ServicesEntities.UniversalHealthCare);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.JanaShreeBimaYojana:
                            {
                                _servicesData = getServiceData(ServicesEntities.JanaShreeBimaYojana);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.NewIBJeevanVidya:
                            {
                                _servicesData = getServiceData(ServicesEntities.NewIBJeevanVidya);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.IBJeevanKalyan:
                            {
                                _servicesData = getServiceData(ServicesEntities.IBJeevanKalyan);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.IBVarishtha:
                            {
                                _servicesData = getServiceData(ServicesEntities.IBVarishtha);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.ArogyaRaksha:
                            {
                                _servicesData = getServiceData(ServicesEntities.ArogyaRaksha);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.IBChhatra:
                            {
                                _servicesData = getServiceData(ServicesEntities.IBChhatra);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.IBGrihaJeevan:
                            {
                                _servicesData = getServiceData(ServicesEntities.IBGrihaJeevan);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
                                break;
                            }
                        case ServicesEntities.IBYatraSuraksha:
                            {
                                _servicesData = getServiceData(ServicesEntities.IBYatraSuraksha);
                                await _serviceResponder.ReplyWith(turnContext, ServicesResponses.ServiceResponseIds.ServicesCardDisplay, _servicesData);
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
                    await turnContext.SendActivityAsync("Please find the types of Services.\n Select any Services type to proceed further.");
                    await _responder.ReplyWith(turnContext, MainResponses.ResponseIds.ServicesMenuCardDisplay);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        public static ServicesResponses.ServicesData getServiceData(string result)
        {
            ServicesResponses.ServicesData servicesData = new ServicesResponses.ServicesData();
            var res = ServicesResponses.keyValuePairs.TryGetValue(result, out servicesData);
            return servicesData;
        }

        #endregion
    }
}
