using IndianBank_ChatBOT.Dialogs.EMI;
using IndianBank_ChatBOT.Dialogs.Loans;
using IndianBank_ChatBOT.Dialogs.Onboarding;
using IndianBank_ChatBOT.Dialogs.Shared;
using IndianBank_ChatBOT.Models;
using IndianBank_ChatBOT.Utils;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RabbitMQ.Client;

using ServiceStack.Redis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Options;

namespace IndianBank_ChatBOT.Dialogs.Main
{
    public class MainDialog : RouterDialog
    {
        #region properties

        private static readonly MemoryStorage _myStorage = new MemoryStorage();
        private BotServices _services;
        private UserState _userState;
        private ConversationState _conversationState;
        private MainResponses _responder = new MainResponses();

        public static Dictionary<string, ImageData> keyValuePairs = new Dictionary<string, ImageData>
        {
            //Contacts Menu Items
            {"quick_contacts.PNG",new ImageData{ImagePath=Path.Combine(".",@"Resources\contacts\QuickContacts", "quick_contacts.PNG")} },
            {"customer_complaints.PNG",new ImageData{ImagePath=Path.Combine(".",@"Resources\contacts\CustomerSupport", "customer_complaints.PNG")} },
            {"complaints_officers_list.PNG",new ImageData{ImagePath=Path.Combine(".",@"Resources\contacts\CustomerSupport", "complaints_officers_list.PNG")} },
            {"chief_vigilance_officer.PNG",new ImageData{ImagePath=Path.Combine(".",@"Resources\contacts\CustomerSupport", "chief_vigilance_officer.PNG")} },
            {"head_office.PNG",new ImageData{ImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "head_office.PNG")} },
            {"department.PNG",new ImageData{ImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "department.PNG")} },
            {"executives.PNG",new ImageData{ImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "executives.PNG")} },
            {"image.PNG",new ImageData{ImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "image.PNG")} },
            {"foreign_branches.PNG",new ImageData{ImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "foreign_branches.PNG")} },
            {"overseas_branches.PNG",new ImageData{ImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "overseas_branches.PNG")} },
            {"nri_branches.PNG",new ImageData{ImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "nri_branches.PNG")} },
            {"zonal_offices.PNG",new ImageData{ImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "zonal_offices.PNG")} },
            {"econfirmation_of_bank_guarantee.PNG",new ImageData{ImagePath=Path.Combine(".",@"Resources\contacts\EmailIDs", "econfirmation_of_bank_guarantee.PNG")} },
            {"SavingsBank.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "SavingsBank.PNG")} },
            {"PayrollPackage.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "PayrollPackage.PNG")} },
            {"VikasSavingsKhata.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "VikasSavingsKhata.PNG")} },
            {"IbSmartKid.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "IbSmartKid.PNG")} },
            {"SavingsTermsConditions.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "SavingsTermsConditions.PNG")} },
            {"SbPlatinum.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "SbPlatinum.PNG")} },
            {"IbSurabhi.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "IbSurabhi.PNG")} },

             //Current Account
            {"CurrentAccount.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "CurrentAccount.PNG")} },
            {"FreedomCurrentAccount.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "FreedomCurrentAccount.PNG")} },
            {"CurrentTermsAndConditions.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "CurrentTermsAndConditions.PNG")} },
            {"PremiumCurrentAccount.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "PremiumCurrentAccount.PNG")} },

            //term deposit entities constants
            {"FacilityDeposit.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "FacilityDeposit.PNG")} },
            {"CapitalGains.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "CapitalGains.PNG")} },
            {"Term-TermsConditions.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "Term-TermsConditions.PNG")}  },
            {"DepositSchemeForSeniorCitizens.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "DepositSchemeForSeniorCitizens.PNG")}  },

            {"RecurringDeposit.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "RecurringDeposit.PNG")} },
            {"IbTaxSaverScheme.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "IbTaxSaverScheme.PNG")}},
            {"InsuredRecurringDeposit.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "InsuredRecurringDeposit.PNG")}},
            {"ReInvestmentPlan.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "ReInvestmentPlan.PNG")}},
            {"FixedDeposit.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "FixedDeposit.PNG")}},
            {"VariableRecurringDeposit.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "VariableRecurringDeposit.PNG")}},

            //nri accounts entities constants
            {"ForeignCurrencyForReturningIndians.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "ForeignCurrencyForReturningIndians.PNG")}},
            {"NRE_FD_RIP_RD_Accounts.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "NRE_FD_RIP_RD_Accounts.PNG")}},
            {"NRE_SB_Accounts.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "NRE_SB_Accounts.PNG")}},
            {"NonResidentOrdinaryAccount.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "NonResidentOrdinaryAccount.PNG")}},
            {"FCNR_Accounts.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\deposits", "FCNR_Accounts.PNG")}},
        
            //agriculture
            {"AgriculturalGodowns.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "AgriculturalGodowns.PNG")} },
            {"LoansForMaintainenceOfTractorsUnderTie-UpWithSugarMills.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "LoansForMaintainenceOfTractorsUnderTie-UpWithSugarMills.PNG")} },
            {"AgriculturalProduceMarketingLoan.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "AgriculturalProduceMarketingLoan.PNG")} },
            {"FinancingAgriculturistsForPurchaseOfTractors.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "FinancingAgriculturistsForPurchaseOfTractors.PNG")} },
            {"PurchaseOfSecondHandTractorsByAgriculturists.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "PurchaseOfSecondHandTractorsByAgriculturists.PNG")} },
            {"AgriClinicAndAgriBusinessCentres.PNG", new ImageData{ ImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "AgriClinicAndAgriBusinessCentres.PNG")} },
            {"SHG_BankLinkageProgramme.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "SHG_BankLinkageProgramme.PNG")} },
            {"JointLiabilityGroup.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "JointLiabilityGroup.PNG")} },
            {"RupayKisanCard.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "RupayKisanCard.PNG")} },
            {"DRI_SchemeRevisedNorms.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "DRI_SchemeRevisedNorms.PNG")} },
            {"SHG_VidhyaShoba.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "SHG_VidhyaShoba.PNG")} },
            {"GraminMahilaSowbhagyaScheme.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "GraminMahilaSowbhagyaScheme.PNG")} },
            {"SugarPremiumScheme.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "SugarPremiumScheme.PNG")} },
            {"GoldenHarvestScheme.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "GoldenHarvestScheme.PNG")} },
            {"AgriculturalJewelLoanScheme.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "AgriculturalJewelLoanScheme.PNG")} },

            //groups
            {"GroupsAgriculturalGodowns.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\GroupsImages", "GroupsAgriculturalGodowns.PNG")} },
            {"GroupsSHG_BankLinkageProgrammeDirectLinkageToSHGS.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\GroupsImages", "GroupsSHG_BankLinkageProgrammeDirectLinkageToSHGS.PNG")} },
            {"GroupsSHG_VidhyaShoba.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\GroupsImages", "GroupsSHG_VidhyaShoba.PNG")} },

            //personal/individual
            {"IbHomeLoanCombo.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IbHomeLoanCombo.PNG")} },
            {"IbRentEncash.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IbRentEncash.PNG")} },
            {"Loan_OD_AgainstDeposits.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "Loan_OD_AgainstDeposits.PNG")} },
            {"IbCleanLoan.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IbCleanLoan.PNG")} },
            {"IbBalavidhyaScheme.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IbBalavidhyaScheme.PNG")} },
            {"IndReverseMortgage.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IndReverseMortgage.PNG")} },
            {"IbVehicleLoan.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IbVehicleLoan.PNG")} },
            {"IndMortgage.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IndMortgage.PNG")} },
            {"PlotLoan.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "PlotLoan.PNG")} },
            {"IbHomeLoan.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IbHomeLoan.PNG")} },
            {"IbPensionLoan.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IbPensionLoan.PNG")} },
            {"HomeImprove.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "HomeImprove.PNG")} },
            {"IbHomeLoanPlus.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IbHomeLoanPlus.PNG")} },
            {"Loan_OD_AgainstNSC.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "Loan_OD_AgainstNSC.PNG")} },
            {"HomeLoanAndVehicleLoan.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "HomeLoanAndVehicleLoan.PNG")} },
            
            //59 minutes loans
            {"59_min_loan_indian_bank.jpg",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\FNMinutesLoan", "indian_bank.jpg")} },
            
            //MSME Loan Menu Item
            {"ib_vidhya_mandir.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\MSME", "ib_vidhya_mandir.PNG")} },
            {"ib_my_own_shop.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\MSME", "ib_my_own_shop.PNG")} },
            {"ib_doctor_plus.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\MSME", "ib_doctor_plus.PNG")} },
            {"ib_contractors.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\MSME", "ib_contractors.PNG")} },
            {"tradewell.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\MSME", "tradewell.PNG")} },
            {"ind_sme_secure.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\MSME", "ind_sme_secure.PNG")} },
            //Education Loan Menu Item
            {"iba_model_educational_loan_schema.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\Education", "iba_model_educational_loan_schema.PNG")} },
            {"ib_educational_loan_prime.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\Education", "ib_educational_loan_prime.PNG")} },
            {"ib_skill_loan_scheme.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\Education", "ib_skill_loan_scheme.PNG")} },
            {"education_loan_interest_subsidies.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\Education", "education_loan_interest_subsidies.PNG")} },
            //nri Loan Menu Item
            {"nri_plot_loan.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\NRI", "nri_plot_loan.PNG")} },
            {"nri_home_loan.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\loans\NRI", "nri_home_loan.PNG")} },
        
            //news/info Menu Items
            //keys are same as entity
            { "Notifications.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "Notifications.jpg")} },
            { "NewsLetter.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "NewsLetter.jpg")} },
            { "WhatsNew.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "WhatsNew.jpg")} },
            { "SmsBanking.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "SmsBanking.jpg")} },
            { "MyDesignCard.PNG",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "MyDesignCard.PNG")} },
            { "PressReleases.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "PressReleases.jpg")} },
            { "Downloads.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "Downloads.jpg")} },

            //customer corner
            { "OnlineCustomerComplaints.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "OnlineCustomerComplaints.jpg")} },
            { "PrincipalCodeComplianceOfficer.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "PrincipalCodeComplianceOfficer.jpg")} },
            { "DamodaranCommitteeRecommendations.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "DamodaranCommitteeRecommendations.jpg")} },
            { "BankingOmbudsman.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "BankingOmbudsman.jpg")} },
            { "RemitToIndia.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "RemitToIndia.jpg")} },

            //related info
            { "FAQs.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "FAQs.jpg")} },
            { "Disclaimer.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "Disclaimer.jpg")} },

            //codes/policy/disclosure
            { "BestPracticesCodeOfTheBank.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "BestPracticesCodeOfTheBank.jpg")} },
            { "CustomerCentricServices.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "CustomerCentricServices.jpg")} },

            //charters/schemes
            { "AgriculturalDebtWaiver.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "AgriculturalDebtWaiver.jpg")} },
            { "FinancialInclusionPlan.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "FinancialInclusionPlan.jpg")} },
            { "RestructuredAccounts.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "RestructuredAccounts.jpg")} },
            { "ServicesRenderedFreeOfCharge.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "ServicesRenderedFreeOfCharge.jpg")} },
            { "WelfareOfMinorities.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "WelfareOfMinorities.jpg")} },
            { "WhistleBlowerPolicy.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "WhistleBlowerPolicy.jpg")} },
            { "CentralizedPensionProcessingSystem.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "CentralizedPensionProcessingSystem.jpg")} },
            { "AnotherOptionForPension.jpg",new ImageData{ImagePath=Path.Combine(".","Resources\\news_info", "AnotherOptionForPension.jpg")} },
        
            //Loan Menu Item
            {"depositRates.jpg",new ImageData{ImagePath=Path.Combine(".", @"Resources\rates", "depositRates.jpg")} },
            {"lendingRates.jpg",new ImageData{ImagePath=Path.Combine(".", @"Resources\rates", "lendingRates.jpg")} },
            {"serviceCharges.jpg",new ImageData{ImagePath=Path.Combine(".", @"Resources\rates", "serviceCharges.jpg")} },

            //Services Menu Items
            {"cms_plus.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\CMS Plus", "cms_plus.PNG")} },
            {"epayment_direct_taxes.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Direct Taxes", "epayment_direct_taxes.PNG")} },
            {"epayment_indirect_taxes.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Indirect Taxes", "epayment_indirect_taxes.PNG")} },

            //Preminum Services Menu Items
            {"mca_payment.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Premium Services", "mca_payment.PNG")} },
            {"money_gram.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Premium Services", "money_gram.PNG")} },
            {"atm_debit_cards.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Premium Services", "atm_debit_cards.PNG")} },
            {"ind_mobile_banking.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Premium Services", "ind_mobile_banking.PNG")} },
            {"ind_net_banking.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Premium Services", "ind_net_banking.PNG")} },
            {"credit_cards.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Premium Services", "credit_cards.PNG")} },
            {"xpress_money.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Premium Services", "xpress_money.PNG")} },
            {"neft.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Premium Services", "neft.PNG")} },
            {"rtgs.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Premium Services", "rtgs.PNG")} },
            {"multicity_cheque_facility.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Premium Services", "multicity_cheque_facility.PNG")} },

            //Insurance Services Menu Items
            {"ib_vidyarthi_suraksha.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "ib_vidyarthi_suraksha.PNG")} },
            {"ib_home_security.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "ib_home_security.PNG")} },
            {"universal_health_care.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "universal_health_care.PNG")} },
            {"jana_shree_bima_yojana.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "jana_shree_bima_yojana.PNG")} },
            {"new_ib_jeevan_vidya.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "new_ib_jeevan_vidya.PNG")} },
            {"ib_jeevan_kalyan.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "ib_jeevan_kalyan.PNG")} },
            {"ib_varishtha.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "ib_varishtha.PNG")} },
            {"arogya_raksha.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "arogya_raksha.PNG")} },
            {"ib_chhatra.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "ib_chhatra.PNG")} },
            {"ib_griha_jeevan.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "ib_griha_jeevan.PNG")} },
            {"ib_yatra_suraksha.PNG",new ImageData{ImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "ib_yatra_suraksha.PNG")} }

        };

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainDialog"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="conversationState">State of the conversation.</param>
        /// <param name="userState">State of the user.</param>
        /// <exception cref="ArgumentNullException">services</exception>

        private static  AppSettings appSettings;

        public MainDialog(BotServices services, ConversationState conversationState, UserState userState,AppSettings appsettings)
            : base(nameof(MainDialog))
        {
            appSettings = appsettings;

            _services = services ?? throw new ArgumentNullException(nameof(services));
            _conversationState = conversationState;
            _userState = userState;
            AddDialog(new VehicleLoanDialog(_services, conversationState, userState));
            AddDialog(new OnBoardingFormDialog(_services, conversationState, userState,appsettings));
            AddDialog(new EMICalculatorDialog(_services, conversationState, userState));
        }

        #endregion

        #region methods

        /// <summary>
        /// Called when /[start asynchronous].
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected override async Task OnStartAsync(DialogContext dc, CancellationToken cancellationToken = default(CancellationToken))
        {
            var view = new MainResponses();
            await dc.Context.SendActivityAsync("Hi! My name is ADYAa \U0001F603.\n Welcome to Indian Bank.\n I am your virtual assistant, here to assist you with all your banking queries 24x7");
            //  await view.ReplyWith(dc.Context, MainResponses.ResponseIds.Intro);

            await dc.BeginDialogAsync(nameof(OnBoardingFormDialog));
        }

        /// <summary>
        /// Routes the asynchronous.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// The specified LUIS Model could not be found in your Bot Services configuration.
        /// or
        /// The specified QnA Maker Service could not be found in your Bot Services configuration.
        /// or
        /// The specified QnA Maker Service could not be found in your Bot Services configuration.
        /// </exception>
        protected override async Task RouteAsync(DialogContext dc, CancellationToken cancellationToken = default(CancellationToken))
        {

            _services.LuisServices.TryGetValue("general", out var luisService);

            if (luisService == null)
            {
                throw new Exception("The specified LUIS Model could not be found in your Bot Services configuration.");

            }
            else if (luisService != null)
            {
                string entityName = string.Empty;
                string entityType = string.Empty;
                var data = string.Empty;

                var result = await luisService.RecognizeAsync(dc.Context, CancellationToken.None);

                List<string> entityTypes = new List<string>
                {
                    "what_entity",
                    "why_entity",
                    "can_entity",
                    "how_entity",
                    "when_entity",
                    "where_entity",
                    "which_entity",
                    "who_entity",
                    "scrollbar_entity",
                    "aboutus_entity",
                    "product_entity",
                    "services_entity",
                    "rates_entity",
                    "customersupport_entity",
                    "link_entity",
                    "atm_entity",
                    "lost_entity"
                };

                foreach (var entity in entityTypes)
                {
                    if (result.Entities[entity] != null)
                    {
                        entityType = entity;
                        entityName = result.Entities[entity].Values<string>().FirstOrDefault();
                        break;
                    }
                }

                var generalIntent = result.GetTopScoringIntent().intent;
                var generalIntentScore = result.GetTopScoringIntent().score;

                Console.WriteLine(generalIntent, generalIntentScore);

                BotChatActivityLogger.UpdateRaSaData(generalIntent, generalIntentScore, entityName);
                BotChatActivityLogger.UpdateResponseJsonText(string.Empty);
                BotChatActivityLogger.UpdateSource(ResponseSource.Rasa);
                string conversationID = dc.Context.Activity.Conversation.Id;
                UserInfo userInfo = BotChatActivityLogger.GetUserDetails(conversationID);

                if (generalIntentScore > 0.32)
                {
                    var messageData = result.Text.First().ToString().ToUpper() + result.Text.Substring(1);
                    if (generalIntent == "greet")
                    {
                       
                        await dc.Context.SendActivityAsync($"{messageData}!!! {userInfo.Name}. How may I help you today?");
                    }
                    else if (generalIntent == "small_talks_intent")
                    {
                        await dc.Context.SendActivityAsync($"Hello!! I'm IVA, your Indian Bank Virtual Assistant");
                    }
                    else if (generalIntent == "capabilities_intent")
                    {
                        await dc.Context.SendActivityAsync($"I am here to assist you with all your banking queries 24x7. \n Feel free to ask me any question by typing below or clicking on the dynamic scroll bar options for specific suggestions.");
                    }
                    else if (generalIntent == "bye_intent")
                    {
                        await dc.Context.SendActivityAsync($"{messageData}!!! {userInfo.Name}. It was nice talking to you today.");
                    }
                    else if (entityType == "atm_entity")
                    {
                        await dc.Context.SendActivityAsync("Please click on the URL below to find all Indian Bank ATM/Branch Locations. \n\n https://www.indianbank.in/branch-atm/");
                    }
                    else if (entityType == "lost_entity")
                    {
                        await dc.Context.SendActivityAsync($"Looks like your query requires futher assistance. Please contact customer care immediately on the following number's : \n\n <tel:180042500000> /  <tel:18004254422>");
                    }
                    else if (entityType == "aboutus_entity" || entityType == "product_entity" || entityType == "services_entity" || entityType == "rates_entity" || entityType == "customersupport_entity" || entityType == "link_entity")
                    {
                        SampleFAQDialog.DisplaySampleFAQ(dc, entityType, entityName);
                        await dc.EndDialogAsync();
                    }
                    else
                    {
                        await ExecuteRabbitMqQueryAsync(dc);
                    }
                }
                else if (generalIntent == "thankyouintent")
                {
                    var messageData = result.Text.First().ToString().ToUpper() + result.Text.Substring(1);
                    await dc.Context.SendActivityAsync($"{messageData}!!! {userInfo.Name}. It was nice talking to you today.");
                }
                else if (entityType == "scrollbar_entity")
                {
                    ScrollBarDialog.DisplayScrollBarMenu(dc, entityName);
                    await dc.EndDialogAsync();
                }
                else if (entityType == "aboutus_entity" || entityType == "product_entity" || entityType == "services_entity" || entityType == "rates_entity" || entityType == "customersupport_entity" || entityType == "link_entity")
                {
                    await dc.Context.SendActivityAsync("Display the predefined FAQ's");
                    SampleFAQDialog.DisplaySampleFAQ(dc, entityType, entityName);
                    await dc.EndDialogAsync();
                }
                else
                {
                    await ExecuteRabbitMqQueryAsync(dc);
                }
            }
            else
            {
                await Task.FromResult(true);
            }
            await Task.FromResult(true);
        }

        private async Task ExecuteRabbitMqQueryAsync(DialogContext dc)
        {
            var rabbitMqQuery = dc.Context.Activity.Text;

            var context = string.Empty;

            Console.WriteLine(dc.Context.Activity.ChannelData);

            var data = rabbitMq(rabbitMqQuery, context);

            await DisplayBackendResult(dc, context, data);
        }

        /// <summary>
        /// Called when [event asynchronous].
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected override async Task OnEventAsync(DialogContext dc, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Check if there was an action submitted from intro card
            if (dc.Context.Activity.Value != null)
            {
                dynamic value = dc.Context.Activity.Value;

                if (dc.Context.Activity.Value != null)
                {
                    if (value.action == "submit")
                    {
                        string name = value.name;
                        string phone = value.phone;
                        string email = value.email;
                        //checks if no fields are emplty
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(email))
                        {
                            int phoneLength = phone.Length;
                            if (phoneLength == 10)
                            {
                                bool result = Int64.TryParse(phone, out long number);
                                if (result)
                                {
                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                    if (isEmail)
                                    {
                                        await StoreUserDataAndDisplayFormAsync(dc, name, phone, email);
                                    }
                                    else
                                    {
                                        await dc.Context.SendActivityAsync("Please enter valid Email ID.");
                                        await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.Intro);
                                    }
                                }
                                else
                                {
                                    await dc.Context.SendActivityAsync("Phone Number cannot contain alphabets. Please enter a vaild mobile number");
                                    await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.Intro);
                                }

                            }
                            else
                            {
                                await dc.Context.SendActivityAsync("Your Mobile Number doesn't contain 10 digits. Please enter valid Mobile Number.");
                                await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.Intro);
                            }

                        }
                        else if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(phone) && string.IsNullOrEmpty(email))
                        {
                            await dc.Context.SendActivityAsync("Please fill the form");
                            await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.Intro);
                        }
                        else
                        {
                            //checks if either one of the fields is empty
                            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email))
                            {
                                if (string.IsNullOrEmpty(name))
                                {
                                    await dc.Context.SendActivityAsync("Name field cannot be empty.");
                                    await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.Intro);
                                }
                                else if (string.IsNullOrEmpty(phone))
                                {
                                    await dc.Context.SendActivityAsync("Phone field cannot be empty.");
                                    await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.Intro);
                                }
                                else
                                {
                                    await dc.Context.SendActivityAsync("Email field cannot be empty.");
                                    await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.Intro);
                                }
                            }
                        }
                    }
                    //else if (value.action == "emi_cal")
                    //{
                    //    string principal = value.principal;
                    //    string rate = value.rate;
                    //    string time = value.time;
                    //    if (!string.IsNullOrEmpty(principal) && !string.IsNullOrEmpty(rate) && !string.IsNullOrEmpty(time))
                    //    {
                    //        await CalculateEmiCalculateFormAsync(dc, Convert.ToInt32(principal), Convert.ToInt32(rate), Convert.ToInt32(time));
                    //    }
                    //    else if (string.IsNullOrEmpty(principal) && string.IsNullOrEmpty(rate) && string.IsNullOrEmpty(time))
                    //    {
                    //        await dc.Context.SendActivityAsync("Please fill the form");
                    //       // await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.BuildEmiCalculatorCard);
                    //    }
                    //    else

                    //    {
                    //        if (string.IsNullOrEmpty(principal) || string.IsNullOrEmpty(rate) || string.IsNullOrEmpty(time))
                    //        {
                    //            if (string.IsNullOrEmpty(principal))
                    //            {
                    //                await dc.Context.SendActivityAsync("Principal field cannot be empty.");
                    //               // await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.BuildEmiCalculatorCard);
                    //            }
                    //            else if (string.IsNullOrEmpty(rate))
                    //            {
                    //                await dc.Context.SendActivityAsync("Rate field cannot be empty.");
                    //             //   await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.BuildEmiCalculatorCard);
                    //            }
                    //            else
                    //            {
                    //                await dc.Context.SendActivityAsync("Time field cannot be empty.");
                    //             //   await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.BuildEmiCalculatorCard);
                    //            }
                    //        }
                    //    }
                    //}
                    else if (value.action == "VehicleLoanSubmit")
                    {
                        dynamic LoanDetails = dc.Context.Activity.Value;

                        VehicleLoanDetails vehicleLoanDetails = new VehicleLoanDetails
                        {
                            Name = Convert.ToString(LoanDetails.name),
                            Mobile = Convert.ToString(LoanDetails.mobile),
                            Email = Convert.ToString(LoanDetails.email),
                            Area = Convert.ToString(LoanDetails.area),
                            Address = Convert.ToString(LoanDetails.address),
                            LoanType = Convert.ToString(LoanDetails.loanType),
                            LoanAmount = Convert.ToString(LoanDetails.loanAmount),
                            State = Convert.ToString(LoanDetails.state),
                            City = Convert.ToString(LoanDetails.city),
                            Branch = Convert.ToString(LoanDetails.branch)
                        };

                        //Sending an Welcome Email to applicant
                        if (!string.IsNullOrEmpty(vehicleLoanDetails.Email))
                        {
                            EmailDetails welcomeEmailDetails = new EmailDetails
                            {
                                To = vehicleLoanDetails.Email,
                                From = "xxxxx",
                                Subject = $"Request for the {vehicleLoanDetails.LoanType} is submitted.",
                                Body = $"Your request for the {vehicleLoanDetails.LoanType} is submittd and we have started processing the loan request. Our loan team will get back to you soon."
                            };

                            EmailUtility.SendMail(welcomeEmailDetails);
                        }

                        //Sending an email to loan bot team
                        EmailDetails loanTeamEmailDetails = new EmailDetails
                        {
                            To = "xxxxxx",
                            From = "xxxxx",
                            Subject = $"New Lead for the {vehicleLoanDetails.LoanType} is created by the Iva BOT.",
                            Body = $"<html> <body> <table column=2> <tr> <th>Name</th> <td> {vehicleLoanDetails.Name}</td> </tr> <tr> <th>Mobile</th> <td> {vehicleLoanDetails.Mobile}</td> </tr> <tr> <th>Email</th> <td> {vehicleLoanDetails.Email}</td> </tr> <tr> <th>Area</th> <td> {vehicleLoanDetails.Area}</td> </tr> <tr> <th>Address</th> <td> {vehicleLoanDetails.Address}</td> </tr> <tr> <th>LoanType</th> <td> {vehicleLoanDetails.LoanType}</td> </tr> <tr> <th>LoanAmount</th> <td> {vehicleLoanDetails.LoanAmount}</td> </tr> <tr> <th>State</th> <td> {vehicleLoanDetails.State}</td> </tr> <tr> <th>City</th> <td> {vehicleLoanDetails.City}</td> </tr> <tr> <th>Branch</th> <td> {vehicleLoanDetails.Branch}</td> </tr> </table> </body> </html>"
                        };

                        EmailUtility.SendMail(loanTeamEmailDetails);

                        await dc.Context.SendActivityAsync("Thanks for filling up the form, an Indian Bank representative will be reaching out to you at the earliest.");
                        await dc.Context.SendActivityAsync("Thank you for talking to me. Hope you found this vehicle loan service useful.");
                        await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.FeedBack);

                    }
                    else if (value.action == "feedback")
                    {
                        await dc.Context.SendActivityAsync("Thank you for your response.");
                        await dc.Context.SendActivityAsync("Please find the menu as below");
                        await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.BuildWelcomeMenuCard);
                    }
                }
            }
        }

        //rabbitmq method
        public static string GenerateCoupon(int length, Random random)
        {
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }

        public static async Task DisplayBackendResult(DialogContext dialogContext, string context, string backendResult)
        {
            if (string.IsNullOrEmpty(backendResult))
            {
                await dialogContext.Context.SendActivityAsync("Sorry,I could not understand. Could you please rephrase the query.");
            }
            else
            {
                JsonObject jsonObject = JsonConvert.DeserializeObject<JsonObject>(backendResult);

                if (jsonObject.DOCUMENTS.Count >= 1)
                {
                    var main_title = jsonObject.DOCUMENTS[0].main_title;
                    var sub_title = jsonObject.DOCUMENTS[0].title;

                    BotChatActivityLogger.UpdateRaSaData(main_title, 0, "");
                    BotChatActivityLogger.UpdateMainTitle(main_title);
                    BotChatActivityLogger.UpdateSubTitle(sub_title);
                    BotChatActivityLogger.UpdateResponseJsonText(backendResult);
                    BotChatActivityLogger.UpdateSource(ResponseSource.ElasticSearch);
                }

                if (!String.IsNullOrEmpty(jsonObject.FILENAME))
                {
                    ImageData pathName = getImagePath(jsonObject.FILENAME);

                    if (pathName.ImagePath != null)
                    {
                        var attachment = new HeroCard()
                        {
                            Images = new List<CardImage> { new CardImage(pathName.ImagePath) }
                        }.ToAttachment();


                        var result = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
                        await dialogContext.Context.SendActivityAsync(result);
                    }

                    if (jsonObject.DOCUMENTS[0].value != null)
                    {
                        await dialogContext.Context.SendActivityAsync($"{jsonObject.DOCUMENTS[0].main_title}\n\n{jsonObject.DOCUMENTS[0].title}\n\n{jsonObject.DOCUMENTS[0].value}\n\n For further details please click on the link below:\n {jsonObject.DOCUMENTS[0].url}");
                    }
                    else
                    {
                        await dialogContext.Context.SendActivityAsync($"For further details please click on the link below:\n {jsonObject.DOCUMENTS[0].url}");
                    }

                }
                else
                {

                    if (jsonObject.WORD_SCORE == 0)
                    {
                        await dialogContext.Context.SendActivityAsync($"Your query seems to require further assistance. Please feel free to contact customer support on the following toll free numbers: <tel:180042500000> /  <tel:18004254422> \n\n Please click on the link below for futher contact details: \n\n https://indianbank.in/departments/quick-contact/ ");
                        await dialogContext.Context.SendActivityAsync($"Please feel free to ask me anything else about Indian Bank");
                    }
                    else if (jsonObject.WORD_SCORE < 0.6)
                    {
                        await dialogContext.Context.SendActivityAsync("I did not find an exact answer but here is something similar");
                        await dialogContext.Context.SendActivityAsync($"{jsonObject.DOCUMENTS[0].main_title}\n\n{jsonObject.DOCUMENTS[0].title}\n\n{jsonObject.DOCUMENTS[0].value}\n\n For further details please click on the link below:\n {jsonObject.DOCUMENTS[0].url}");
                    }
                    else
                    {
                        await dialogContext.Context.SendActivityAsync($"This is what I found on \"{jsonObject.CORRECT_QUERY}\"");
                        if (jsonObject.WORD_COUNT > 100)
                        {
                            await dialogContext.Context.SendActivityAsync($"{jsonObject.DOCUMENTS[0].main_title}\n\n{jsonObject.DOCUMENTS[0].title}\n\n{jsonObject.DOCUMENTS[0].value}");
                        }
                        else if (jsonObject.WORD_COUNT < 25)
                        {
                            await dialogContext.Context.SendActivityAsync($"{jsonObject.DOCUMENTS[0].value}\n\n For further details please click on the link below:\n {jsonObject.DOCUMENTS[0].url}");

                        }
                        else
                        {
                            var message = string.Empty;
                            var documentCount = jsonObject.DOCUMENTS.Count();
                            if (documentCount == 1)
                            {
                                message = $"{jsonObject.DOCUMENTS[0].value}\n\n For further details please click on the link below:\n {jsonObject.DOCUMENTS[0].url}";
                            }
                            else if (documentCount > 1 && documentCount <= 2)
                            {
                                message = $"{jsonObject.DOCUMENTS[0].value}\n\n For further details please click on the link below:\n {jsonObject.DOCUMENTS[0].url} \n\n\n For additional info:\n\n{jsonObject.DOCUMENTS[1].url}";

                            }
                            else
                            {
                                message = $"{jsonObject.DOCUMENTS[0].value}\n\n For further details please click on the link below:\n {jsonObject.DOCUMENTS[0].url} \n\n\n For additional info:\n\n{jsonObject.DOCUMENTS[1].url}\n\n{jsonObject.DOCUMENTS[2].url}";
                            }

                            await dialogContext.Context.SendActivityAsync(message);
                        }
                    }
                }
            }
        }


        public static string rabbitMq(string queryParam, string context)
        {
            Random rnd = new Random();
            var coupon = GenerateCoupon(10, rnd);
            var key = String.Join(Environment.NewLine, coupon);
            var redis_res = string.Empty;

            ConnectionFactory factory = new ConnectionFactory();
            // "guest"/"guest" by default, limited to localhost connections

            factory.UserName = appSettings.RabbitmqUsername;
            factory.Password = appSettings.RabbitmqPassword; 
            factory.VirtualHost = appSettings.RabbitmqVirtualHost;
            factory.HostName = appSettings.RabbitmqHostName;
            factory.HostName = "localhost";

            Console.WriteLine($"Key Generated is {key}");

            IConnection connection = factory.CreateConnection();

            var model = connection.CreateModel();
            Console.WriteLine("Creating Exchange");

            // set up the properties
            var properties = model.CreateBasicProperties();
            properties.Persistent = true;

            // Sending Message to Rvehicleabbitmq server 
            var message = $@" {{""UUID"": ""{key}"", ""CONTEXT"": ""{context}"", ""QUERY_STRING"": ""{queryParam}"" }}";

            byte[] messageBuffer = Encoding.Default.GetBytes(message);
            model.BasicPublish("queryExchange", "query", properties, messageBuffer);
            Console.WriteLine("Message Sent");

           // string host = "ashutosh-redis";
            string host = "localhost";

            int count = 0;
            while (count <= 25 && redis_res == string.Empty)
            {
                Thread.Sleep(2000);
                var keydata = Get(host, key.ToString());
                Console.WriteLine("FROM REDIS ************************************" + keydata);
                if (keydata != null)
                {
                    redis_res = Encoding.UTF8.GetString(keydata, 0, keydata.Length);
                }
                Console.WriteLine("FROM REDIS ************************************" + redis_res);
                count++;
                Console.WriteLine($"redis_res==string.Empty = {redis_res == string.Empty}");
            }

            if (!string.IsNullOrEmpty(redis_res))
            {
                var jObject = JObject.Parse(redis_res);
                Console.WriteLine("AFTER PARSING ************************************" + jObject);
            }

            return redis_res;
        }

        //rabbitmq method ends here

        //redis code
        public static byte[] Get(string host, string UUID)
        {
            using (RedisClient redisClient = new RedisClient(host))
            {
                return redisClient.Get(UUID);
            }
        }
        //redis code ends here 

        /// <summary>
        /// Displays User Form
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task StoreUserDataAndDisplayFormAsync(DialogContext dc, string name, string phone, string email)
        {
            var localeChangedEventActivity = dc.Context.Activity.CreateReply();
            localeChangedEventActivity.Type = ActivityTypes.Event;
            localeChangedEventActivity.ValueType = "cardsubmission";
            localeChangedEventActivity.Value = "somedata";
            await dc.Context.SendActivityAsync(localeChangedEventActivity);
            await dc.Context.SendActivityAsync($"Hi " + name + ", welcome to Indian Bank");
            await dc.Context.SendActivityAsync("Please find the menu.");
            await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.BuildWelcomeMenuCard);
        }

        public async Task CalculateEmiCalculateFormAsync(DialogContext dc, int principal, int rate, int time)
        {
            var localeChangedEventActivity = dc.Context.Activity.CreateReply();
            localeChangedEventActivity.Type = ActivityTypes.Event;
            localeChangedEventActivity.ValueType = "cardsubmission";
            localeChangedEventActivity.Value = "somedata";
            await dc.Context.SendActivityAsync(localeChangedEventActivity);

            double loanM = (rate / 1200.0);
            double numberMonths = time * 12;
            double negNumberMonths = 0 - numberMonths;
            double monthlyPayment = principal * loanM / (1 - System.Math.Pow((1 + loanM), negNumberMonths));
            await dc.Context.SendActivityAsync($"EMI per month : " + monthlyPayment.ToString());

        }

        public static ImageData getImagePath(string result)
        {
            ImageData image = new ImageData();
            try
            {
                var res = keyValuePairs.TryGetValue(result, out image);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
            return image;
        }

        /// <summary>
        /// Completes the asynchronous.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected override async Task CompleteAsync(DialogContext dc, CancellationToken cancellationToken = default(CancellationToken))
        {
            // The active dialog's stack ended with a complete status
            await _responder.ReplyWith(dc.Context, MainResponses.ResponseIds.Completed);
        }

        #endregion


        class JsonObject
        {
            public List<DOCUMENT> DOCUMENTS { get; set; }
            public int WORD_COUNT { get; set; }
            public double WORD_SCORE { get; set; }
            public string FILENAME { get; set; }
            public string AUTO_CORRECT_QUERY { get; set; }
            public string CORRECT_QUERY { get; set; }
        }

        class DOCUMENT
        {
            public string main_title { get; set; }
            public string title { get; set; }
            public string url { get; set; }
            public string value { get; set; }

        }

        public class ImageData
        {

            public string ImagePath { get; set; }
        }

        #region Class

        public class Details
        {
            public string Name { get; set; }

            public string PhoneNumber { get; set; }
            public string EmailId { get; set; }

            public override string ToString()
            {
                return ("Name :" + Name + "\t\t" + "PhoneNumber :" + "\t\t" + PhoneNumber + "\t\t" + "EmailId :" + EmailId);
            }
        }

        #endregion
    }

    public class CustomChannelData
    {
        [JsonProperty("context")]
        public string Context { get; set; }
    }
}
