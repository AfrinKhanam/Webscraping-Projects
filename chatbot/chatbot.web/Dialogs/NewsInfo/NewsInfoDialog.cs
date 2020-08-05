using System.Collections.Generic;
using System.Linq;

using Microsoft.Bot.Builder;

namespace IndianBank_ChatBOT.Dialogs.NewsInfo
{
    public class NewsInfoDialog
    {
        private static NewsInfoResponses _newsInfoResponder = new NewsInfoResponses();
        private static NewsInfoResponses.NewsInfoData _newsInfoData = new NewsInfoResponses.NewsInfoData();

        public static async void NewsInfoSubMenuCard(ITurnContext turnContext, RecognizerResult result)
        {

            string EntityType = string.Empty;
            string EntityName = string.Empty;


            List<string> entityTypes = new List<string>
            {
                "news_info_entity",
                "customer_corner_entity",
                "related_info_entity",
                "codes_policy_disclosures_entity",
                "charters_schemes_entity"
            };

            foreach (var entity in entityTypes)
            {
                if (result.Entities[entity] != null)
                {
                    EntityType = entity;
                    EntityName = result.Entities[entity].Values<string>().FirstOrDefault();
                    break;
                }
            }

            if (EntityType.Equals("news_info_entity"))
            {
                switch (EntityName)
                {
                    case NewsInfoEntities.Notifications:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.NewsLetter:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.New:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.SMS_Banking:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.ScanAndPay:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.MyDesignCard:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.PressReleases:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.CustomerCorner:
                        {
                            await turnContext.SendActivityAsync("Please find the sub menus of Customer Corner. Select any Customer Corner sub menu to proceed further.");
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.CustomerCornerMenuCardDisplay);
                            break;
                        }
                    case NewsInfoEntities.RelatedInfo:
                        {
                            await turnContext.SendActivityAsync("Please find the sub menus of Related Info. Select any Related Info sub menu to proceed further.");
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.RelatedInfoMenuCardDisplay);
                            break;
                        }
                    case NewsInfoEntities.Downloads:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.Codes_policy_disclosures:
                        {
                            await turnContext.SendActivityAsync("Please find the sub menus of Codes/Policy/Disclosures. Select any Codes/Policy/Disclosures sub menu to proceed further.");
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.CodesPolicyDisclosuresMenuCardDisplay);
                            break;
                        }
                    case NewsInfoEntities.ChartersSchemes:
                        {
                            await turnContext.SendActivityAsync("Please find the sub menus of Charters/Schemes. Select any Charters/Schemes sub menu to proceed further.");
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.ChartersSchemesMenuCardDisplay);
                            break;
                        }
                    default:
                        {
                            await turnContext.SendActivityAsync("Sorry, I didn't understand. Please try with different query");
                            break;
                        }
                }
            }
            else if (EntityType.Equals("customer_corner_entity"))
            {
                switch (EntityName)
                {
                    case NewsInfoEntities.CustomerComplaintForm:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.OnlineCustomerComplaints:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.BankingOmbudsmanScheme:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.CustomerService:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.PrincipalCodeComplianceOfficer:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.DamodaranCommitteeRecommendations:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.BankingOmbudsman:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.RemitToIndia:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.AadhaarEnrolmentCorrectionForm:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.ProcedureOnLocker_SafeDeposit:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.CoinVendingMachines:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.IndianBankTrustRuralDevelopment:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.MobileBankingThroughUSSD:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    default:
                        {
                            await turnContext.SendActivityAsync("Sorry, I didn't understand. Please try with different query");
                            break;
                        }
                }
            }
            else if (EntityType.Equals("related_info_entity"))
            {
                switch (EntityName)
                {
                    case NewsInfoEntities.FAQs:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.PradhanMantri:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.RecoverAgentsEmpanelled_EngagedByBank:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.ECS_NoticeToCustomers:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.ListOfHolidays:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.Disclaimer:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.SecurityAlert:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    default:
                        {
                            await turnContext.SendActivityAsync("Sorry, I didn't understand. Please try with different query");
                            break;
                        }
                }
            }
            else if (EntityType.Equals("codes_policy_disclosures_entity"))
            {
                switch (EntityName)
                {
                    case NewsInfoEntities.RightsOfBankCustomers:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.DealingDishonourOfCheques:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.DepositPolicy:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.BestPracticesCodeOfTheBank:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.BanksCommitmentToCustomers:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.DeterminingMaritalSubsidiary:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.DisclosureOfMaritalEvents:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.RelatedPartyTransactions:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.GuidelinesOnEmpanelmentOfValuers:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.AppointmentOfStatutoryCentral:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.RightToInformationAct:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.Disclosures:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.CustomerCentricServices:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.DebtRestructingMechanism:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.FairLendingPractices:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.ProcessingFees:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.AgriTermLoans:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.ChargesOfHomePlotVehicle:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.ChargesOfSME_Products:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.BanksCommitmentMSC_Hindi:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.BanksCommitmentMSC:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    default:
                        {
                            await turnContext.SendActivityAsync("Sorry, I didn't understand. Please try with different query");
                            break;
                        }
                }
            }
            else if (EntityType.Equals("charters_schemes_entity"))
            {
                switch (EntityName)
                {
                    case NewsInfoEntities.AgriculturalDebtWaiver:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.ChartersBankingOmbudsman:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.FinancialInclusionPlan:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.RestructuredAccounts:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.ServicesRenderedFreeOfCharge:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.WelfareOfMinorities:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.WhistleBlowerPolicy:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.CentralizedPensionProcessing:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.AnotherOptionForPension:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.CitizensCharter:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
                            break;
                        }
                    case NewsInfoEntities.IndianBankMutualFund:
                        {
                            _newsInfoData = NewsInfoResponses.getNewsInfoData(EntityName);
                            await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.BuildNewsInfoCard, _newsInfoData);
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
                await turnContext.SendActivityAsync("Please find the sub menus of News/Info. Select any News/Info sub menu to proceed further.");
                await _newsInfoResponder.ReplyWith(turnContext, NewsInfoResponses.ResponseIds.NewsOrInfoMenuCardDisplay, _newsInfoData);
            }
        }



    }
}
