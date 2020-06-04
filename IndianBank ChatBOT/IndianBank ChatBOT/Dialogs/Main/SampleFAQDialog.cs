using IndianBank_ChatBOT.Dialogs.Shared;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;

namespace IndianBank_ChatBOT.Dialogs.Main
{
    public class SampleFAQDialog
    {
        public static async void DisplaySampleFAQ(DialogContext dialogContext, string EntityType, string EntityName)
        {
            //await dialogContext.Context.SendActivityAsync($"This is what I received from main Dialog {EntityName} and {EntityType}");
            await dialogContext.Context.SendActivityAsync("Please click on the options below or feel free to type your own query");


            if (EntityType == "aboutus_entity")
            {
                switch (EntityName)
                {
                    case AboutUsEntities.Profile:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForProfileFAQs.Actions);
                            break;
                        }
                    case AboutUsEntities.VisionMission:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForVisionFAQs.Actions);
                            break;
                        }
                    case AboutUsEntities.Management:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForManagementFAQs.Actions);
                            break;
                        }
                    case AboutUsEntities.FinanceResult:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForFinanceResultFAQs.Actions);
                            break;
                        }
                    case AboutUsEntities.CorporateGovernance:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForCorporateGovernanceFAQs.Actions);
                            break;
                        }
                    case AboutUsEntities.MutualFund:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForMutualFundFAQs.Actions);
                            break;
                        }
                    case AboutUsEntities.AnnualReport:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForAnnualReportFAQs.Actions);
                            break;
                        }
                    default:
                        await dialogContext.Context.SendActivityAsync("Sorry!! I could not understand the query. Could you please rephrase it and try again.");
                        break;
                }
            }
            else if (EntityType == "product_entity")
            {
                switch (EntityName)
                {
                    case ProductFAQEntities.LoanProducts:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForLoanProductsFAQs.Actions);
                            break;
                        }
                    case ProductFAQEntities.DepositProducts:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForDepositProductsFAQs.Actions);
                            break;
                        }
                    case ProductFAQEntities.DigitalProducts:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForDigitalProductsFAQs.Actions);
                            break;
                        }
                    case ProductFAQEntities.FeatureProducts:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForFeaturesFAQs.Actions);
                            break;
                        }
                    case ProductFAQEntities.Schemes:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForSchemesFAQs.Actions);
                            break;
                        }
                    default:
                        await dialogContext.Context.SendActivityAsync("Sorry!! I could not understand the query. Could you please rephrase it and try again.");
                        break;
                }
            }
            else if (EntityType == "services_entity")
            {
                switch (EntityName)
                {
                    case ServicesFAQEntities.PremiumServices:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForPremiumServicesFAQs.Actions);
                            break;
                        }
                    case ServicesFAQEntities.InsuranceServices:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForInsuranceServicesFAQs.Actions);
                            break;
                        }
                    case ServicesFAQEntities.CMSPlus:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForCMSPlusFAQs.Actions);
                            break;
                        }
                    case ServicesFAQEntities.DoorstepBanking:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForDoorStepBankingFAQs.Actions);
                            break;
                        }
                    case ServicesFAQEntities.TaxPayment:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForTaxPaymentFAQs.Actions);
                            break;
                        }
                    case ServicesFAQEntities.DebentureTrust:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForDebentureTrustFAQs.Actions);
                            break;
                        }
                    default:
                        await dialogContext.Context.SendActivityAsync("Sorry!! I could not understand the query. Could you please rephrase it and try again.");
                        break;
                }
            }
            else if (EntityType == "rates_entity")
            {
                switch (EntityName)
                {
                    case RatesFAQEntities.DepositRates:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForDepositRatesFAQs.Actions);
                            break;
                        }
                    case RatesFAQEntities.LendingRates:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForLendingRatesFAQs.Actions);
                            break;
                        }
                    case RatesFAQEntities.ServiceCharges:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForServiceChargesFAQs.Actions);
                            break;
                        }

                    default:
                        await dialogContext.Context.SendActivityAsync("Sorry!! I could not understand the query. Could you please rephrase it and try again.");
                        break;
                }
            }
            else if (EntityType == "customersupport_entity")
            {
                switch (EntityName)
                {
                    case ContactsFAQEntities.CustomerSupport:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForCustomerSupportFAQs.Actions);
                            break;
                        }
                    case ContactsFAQEntities.EmailID:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForEmailIDFAQs.Actions);
                            break;
                        }
                    default:
                        await dialogContext.Context.SendActivityAsync("Sorry!! I could not understand the query. Could you please rephrase it and try again.");
                        break;
                }
            }
            else if (EntityType == "link_entity")
            {
                switch (EntityName)
                {
                    case LinksFAQEntities.OnlineServices:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForOnlineServicesFAQs.Actions);
                            break;
                        }
                    case LinksFAQEntities.RelatedSites:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForRelatedSitesFAQs.Actions);
                            break;
                        }
                    case LinksFAQEntities.Alliances:
                        {
                            BuildFAQMenu(dialogContext, SampleFAQResponse.SuggestedActionsForAlliancesFAQs.Actions);
                            break;
                        }
                    default:
                        await dialogContext.Context.SendActivityAsync("Sorry!! I could not understand the query. Could you please rephrase it and try again.");
                        break;
                }
            }
            else
            {
                await dialogContext.Context.SendActivityAsync("Sorry!! I could not understand the query. Could you please rephrase it and try again.");

            }
        }

        public static async void BuildFAQMenu(DialogContext dialogContext, IList<CardAction> cardAction)
        {
            var attachment = new HeroCard()
            {
                Buttons = cardAction
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            await dialogContext.Context.SendActivityAsync(response);
        }
    }


}
