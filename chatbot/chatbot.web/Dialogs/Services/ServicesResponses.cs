using System.Collections.Generic;
using System.IO;

using IndianBank_ChatBOT.Dialogs.Shared;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.TemplateManager;
using Microsoft.Bot.Schema;

namespace IndianBank_ChatBOT.Dialogs.Services
{
    public class ServicesResponses : TemplateManager
    {
        #region Properties

        private static LanguageTemplateDictionary _responseTemplates = new LanguageTemplateDictionary
        {
            ["default"] = new TemplateIdMap
            {
                //Services Menu Items
                {ServiceResponseIds.PremiumServicesDisplay,(context, data) => PremiumServicesMenuCardDisplay(context) },
                {ServiceResponseIds.InsuranceServicesDisplay,(context, data) => InsuranceServicesMenuCardDisplay(context) },

                {ServiceResponseIds.ServicesCardDisplay,(context, data) => BuildserviceCardDisplay(context,data) }
            }
        };

        public static Dictionary<string, ServicesResponses.ServicesData> keyValuePairs = new Dictionary<string, ServicesResponses.ServicesData>
        {
            //Services Menu Items
            {"cms plus",new ServicesData{ServiceDataTitle="CMS Plus",ServiceDataText="To know more about CMS Plus services. Please click on Read More",ServiceDataLink=ServicesResponseLinks.CMSPlusLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\CMS Plus", "cms_plus.PNG")} },
            {"direct taxes",new ServicesData{ServiceDataTitle="E Payment of Direct Taxes",ServiceDataText="To know more about Direct Tax services. Please click on Read More",ServiceDataLink=ServicesResponseLinks.EpaymentofDirectTaxesLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Direct Taxes", "epayment_direct_taxes.PNG")} },
            {"indirect taxes",new ServicesData{ServiceDataTitle="E Payment of Indirect Taxes",ServiceDataText="To know more about Indirect Tax services. Please click on Read More",ServiceDataLink=ServicesResponseLinks.EpaymentofIndirectTaxesLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Indirect Taxes", "epayment_indirect_taxes.PNG")} },

            //Preminum Services Menu Items
            {"mca payment",new ServicesData{ServiceDataTitle="MCA Payment",ServiceDataText="Eligibility On line : Customers having net banking facility with us. Offline : Corporates and other individuals who have to…",ServiceDataLink=PremiumServicesResponseLinks.MCAPaymentLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Premium Services", "mca_payment.PNG")} },
            {"money gram",new ServicesData{ServiceDataTitle="Money Gram",ServiceDataText="Eligibility Unlike various types of remittances from abroad, the beneficiary under Money Gram will always be a resident. All guidelines…",ServiceDataLink=PremiumServicesResponseLinks.MoneyGramLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Premium Services", "money_gram.PNG")} },
            {"atm debit card",new ServicesData{ServiceDataTitle="ATM/Debit Cards",ServiceDataText="Eligibility Our Savings Bank and Current account customers Salient Features Our 24 hour Hi-powered, value-added ATM cum Debit card ,…",ServiceDataLink=PremiumServicesResponseLinks.ATMDebitCardsLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Premium Services", "atm_debit_cards.PNG")} },
            {"ind mobile banking",new ServicesData{ServiceDataTitle="Ind Mobile banking",ServiceDataText="Eligibility Customers having Savings Bank, Current Account with us Salient Features Customers can use their mobile phones to do their…",ServiceDataLink=PremiumServicesResponseLinks.IndMobileBankingLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Premium Services", "ind_mobile_banking.PNG")} },
            {"ind net banking",new ServicesData{ServiceDataTitle="Ind Netbanking",ServiceDataText="Eligibility Customers having Savings Bank, Current Account with us Salient Features Customers can use the internet to do their banking…",ServiceDataLink=PremiumServicesResponseLinks.IndNetBankingLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Premium Services", "ind_net_banking.PNG")} },
            {"credit card",new ServicesData{ServiceDataTitle="Credit Cards",ServiceDataText="Eligibility Gold and Classic cards : Indian Nationals and NRIs in the age group of 18-80 years. Bharat cards :…",ServiceDataLink=PremiumServicesResponseLinks.CreditCardsLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Premium Services", "credit_cards.PNG")} },
            {"xpress money",new ServicesData{ServiceDataTitle="Xpress Money – Inward Remittance – Money Transfer Service Scheme",ServiceDataText="Amount of remittance permitted Single remittance limited to USD 2500 or its equivalent. Payments made in Indian Rupees only. Payment…",ServiceDataLink=PremiumServicesResponseLinks.XpressMoneyLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Premium Services", "xpress_money.PNG")} },
            {"neft",new ServicesData{ServiceDataTitle="N E F T",ServiceDataText="Eligibility Any customer of a CBS Branch for any amount. For non-customers NEFT is available against cash remittance up to…",ServiceDataLink=PremiumServicesResponseLinks.NEFTLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Premium Services", "neft.PNG")} },
            {"ind jet remit",new ServicesData{ServiceDataTitle="Ind Jet Remit (RTGS)",ServiceDataText="Eligibility Any customer of a CBS Branch Salient Features Transfer of funds to any account in any bank branch enabled…",ServiceDataLink=PremiumServicesResponseLinks.IndJetRemitLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Premium Services", "rtgs.PNG")} },
            {"multicity cheque facility",new ServicesData{ServiceDataTitle="Multicity Cheque Facility",ServiceDataText="Eligibility Customers having Savings Bank, Currenct account, OD/OCC accounts. Salient Features Cheques can be issued to beneficiaries all over India…",ServiceDataLink=PremiumServicesResponseLinks.MulticityChequeFacilityLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Premium Services", "multicity_cheque_facility.PNG")} },

            //Insurance Services Menu Items
            {"ib vidyarthi suraksha",new ServicesData{ServiceDataTitle="IB Vidyarthi Suraksha (With PNB-Metlife)",ServiceDataText="Circular Reference Our Dept.Circular CRA 76/2011-12 dt.06.08.2011 Eligibility All Educational Student Borrowers (New & Existing) Age Group 15-60 years Maximum…",ServiceDataLink= InsuranceServicesResponseLinks.IBVidyarthiSurakshaLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "ib_vidyarthi_suraksha.PNG")} },
            {"ib home security",new ServicesData{ServiceDataTitle="IB Home Security – Group Insurance Scheme for Mortgage Borrowers (Launch in association with Kotak Mahindra Old Mutual Life Insurance Limited)",ServiceDataText="Who is eligible? 1.All new Home Loan Borrowers 2. Existing borrowers as on 30/09/2009, not covered so far are also…",ServiceDataLink=InsuranceServicesResponseLinks.IBHomeSecurityLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "ib_home_security.PNG")} },
            {"universal health care",new ServicesData{ServiceDataTitle="Universal Health Care (launched in Association with UIIC Ltd)",ServiceDataText="Purpose / Objective To provide health coverage for whom regular medi claim policy is beyond reach. Eligibility Members of SHGs,…",ServiceDataLink=InsuranceServicesResponseLinks.UniversalHealthCareLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "universal_health_care.PNG")} },
            {"jana shree bima yojana",new ServicesData{ServiceDataTitle="Jana Shree Bima Yojana (Launched in association with LIC)",ServiceDataText="Purpose / Objective To provide life coverage. Eligibility Members of SHGs, Joint Liability Group (JLG), Rythu Mithra Group(RMG), Artisans, Farmers…",ServiceDataLink=InsuranceServicesResponseLinks.JanaShreeBimaYojanaLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "jana_shree_bima_yojana.PNG")} },
            {"new ib javan vidya",new ServicesData{ServiceDataTitle="New IB Jeevan Vidya",ServiceDataText="Who is eligible? All educational loan borrowers (Student) Reimbursement of Expenses – Nepal & Bhutan in INR For treatment while…",ServiceDataLink=InsuranceServicesResponseLinks.NewIBJeevanVidyaLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "new_ib_jeevan_vidya.PNG")} },
            {"ib jeevan kalyan",new ServicesData{ServiceDataTitle="IB Jeevan Kalyan",ServiceDataText="Who is eligible? All Accountholders Nature of cover Term Assurance Type of Cover Life Cover (Normal death /death due to…",ServiceDataLink=InsuranceServicesResponseLinks.IBJeevanKalyanLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "ib_jeevan_kalyan.PNG")} },
            {"ib varishtha",new ServicesData{ServiceDataTitle="IB Varishtha",ServiceDataText="Who is eligible? Term Deposit Customers aged between 56-64 years. Nature of cover Term Assurance Type of Cover Life Cover(Normal…",ServiceDataLink=InsuranceServicesResponseLinks.IBVarishthaLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "ib_varishtha.PNG")} },
            {"arogya raksha",new ServicesData{ServiceDataTitle="Arogya Raksha",ServiceDataText="Group Health Insurance Policy (By arrangement with M/s. United India Insurance Co. Ltd) Who is eligible? All A/c holders of…",ServiceDataLink=InsuranceServicesResponseLinks.ArogyaRakshaLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "arogya_raksha.PNG")} },
            {"ib chhatra",new ServicesData{ServiceDataTitle="IB Chhatra",ServiceDataText="Who is eligible? All Account holders. Nature of cover Term Assurance. Type of Cover Personal Accident Death Cover ( Death…",ServiceDataLink=InsuranceServicesResponseLinks.IBChhatraLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "ib_chhatra.PNG")} },
            {"ib griha jeevan",new ServicesData{ServiceDataTitle="IB Griha Jeevan Group Insurance Scheme for Mortgage Borrowers (launched in Association with LIC)",ServiceDataText="Who is eligible? Home Loan borrowers availing Loan from 01/10/2009 onwards. Age Group 18-60 (Maximum @ exit 65) Coverage Min…",ServiceDataLink=InsuranceServicesResponseLinks.IBGrihaJeevanLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "ib_griha_jeevan.PNG")} },
            {"ib yatra suraksha",new ServicesData{ServiceDataTitle="IB Yatra Suraksha ( With UIIC Ltd)",ServiceDataText="Who is eligible? All customers Eligibility Minimum five persons and maximum 20 persons per group in the age group of 3 years to 70 years",ServiceDataLink=InsuranceServicesResponseLinks.IBYatraSurakshaLink,ServiceDataImagePath=Path.Combine(".", @"Resources\services\Insurance Services", "ib_yatra_suraksha.PNG")} }

        };

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ServicesResponses"/> class.
        /// </summary>
        public ServicesResponses()
        {
            Register(new DictionaryRenderer(_responseTemplates));
        }
        #endregion

        #region Methods

        private static object BuildserviceCardDisplay(ITurnContext context, ServicesData data)
        {
            var attachment = new HeroCard()
            {
                Title = data.ServiceDataTitle,
                Text = data.ServiceDataText,
                Images = new List<CardImage> { new CardImage(data.ServiceDataImagePath) },
                Buttons = new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.OpenUrl, title: "Read More", value: data.ServiceDataLink)
                }
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        public static IMessageActivity PremiumServicesMenuCardDisplay(ITurnContext turnContext)
        {
            var attachment = new HeroCard()
            {
                Buttons = SharedResponses.SuggestedActionsForPremiumServices.Actions
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        public static IMessageActivity InsuranceServicesMenuCardDisplay(ITurnContext turnContext)
        {
            var attachment = new HeroCard()
            {
                Buttons = SharedResponses.SuggestedActionsForInsuranceServices.Actions
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }
        #endregion

        #region Class

        public class ServiceResponseIds
        {
            public const string ServicesCardDisplay = "buildServicesCardDisplay";

            // Services Constants
            public const string PremiumServicesDisplay = "premiumServicesCardDisplay";
            public const string InsuranceServicesDisplay = "insuranceServicesCardDisplay";
            public const string CMSPlusDisplay = "cmsPlusCardDisplay";
            public const string EpaymentofDirectTaxesDisplay = "ePaymentofDirectTaxesCardDisplay";
            public const string EpaymentofIndirectTaxesDisplay = "ePaymentofIndirectTaxesCardDisplay";

            // Premium Services Constants
            public const string MCAPaymentDisplay = "mcaPaymentCardDisplay";
            public const string MoneyGramDisplay = "moneyGramCardDisplay";
            public const string ATMDebitCardsDisplay = "atmDebitCardDisplay";
            public const string IndMobileBankingDisplay = "indMobileBankingCardDisplay";
            public const string IndNetBankingDisplay = "indNetBankingCardDisplay";
            public const string CreditCardsDisplay = "creditCardDisplay";
            public const string XpressMoneyDisplay = "xpressMoneyCardDisplay";
            public const string NEFTDisplay = "neftCardDisplay";
            public const string IndJetRemitDisplay = "indJetRemitCardDisplay";
            public const string MulticityChequeFacilityDisplay = "multicityChequeFacilityCardDisplay";

            // Insurance Services Constants
            public const string IBVidyarthiSurakshaDisplay = "ibVidyarthiSurakshaCardDisplay";
            public const string IBHomeSecurityDisplay = "ibHomeSecurityCardDisplay";
            public const string UniversalHealthCareDisplay = "universalHealthCareCardDisplay";
            public const string JanaShreeBimaYojanaDisplay = "janaShreeBimaYojanaCardDisplay";
            public const string NewIBJeevanVidyaDisplay = "newIBJeevanVidyaCardDisplay";
            public const string IBJeevanKalyanDisplay = "ibJeevanKalyanCardDisplay";
            public const string IBVarishthaDisplay = "ibVarishthaCardDisplay";
            public const string ArogyaRakshaDisplay = "arogyaRakshaCardDisplay";
            public const string IBChhatraDisplay = "ibChhatraCardDisplay";
            public const string IBGrihaJeevanDisplay = "ibGrihaJeevanCardDisplay";
            public const string IBYatraSurakshaDisplay = "ibYatraSurakshaCardDisplay";

        }

        public class ServicesResponseLinks
        {
            // Links
            public const string CMSPlusLink = "https://www.indianbank.in/departments/cms-plus/#!";
            public const string EpaymentofDirectTaxesLink = "https://www.indianbank.in/departments/e-payment-of-direct-taxes/#!";
            public const string EpaymentofIndirectTaxesLink = "https://www.indianbank.in/departments/e-payment-of-indirect-taxes/#!";
        }

        public class PremiumServicesResponseLinks
        {
            public const string MCAPaymentLink = "https://www.indianbank.in/departments/mca-payment/#!";
            public const string MoneyGramLink = "https://www.indianbank.in/departments/money-gram/#!";
            public const string ATMDebitCardsLink = "https://www.indianbank.in/departments/atm-debit-cards/#!";
            public const string IndMobileBankingLink = "https://www.indianbank.in/departments/ind-mobile-banking/#!";
            public const string IndNetBankingLink = "https://www.indianbank.in/departments/ind-netbanking/#!";
            public const string CreditCardsLink = "https://www.indianbank.in/departments/credit-cards/#!";
            public const string XpressMoneyLink = "https://www.indianbank.in/departments/xpress-money-inward-remittance-money-transfer-service-scheme/#!";
            public const string NEFTLink = "https://www.indianbank.in/departments/n-e-f-t/#!";
            public const string IndJetRemitLink = "https://www.indianbank.in/departments/ind-jet-remit-rtgs/#!";
            public const string MulticityChequeFacilityLink = "https://www.indianbank.in/departments/multicity-cheque-facility/#!";
        }

        public class InsuranceServicesResponseLinks
        {
            public const string IBVidyarthiSurakshaLink = "https://www.indianbank.in/departments/ib-vidyarthi-suraksha-with-pnb-metlife/#!";
            public const string IBHomeSecurityLink = "https://www.indianbank.in/departments/ib-home-security-group-insurance-scheme-for-mortgage-borrowers-launch-in-association-with-kotak-mahindra-old-mutual-life-insurance-limited/#!";
            public const string UniversalHealthCareLink = "https://www.indianbank.in/departments/universal-health-care-launched-in-association-with-uiic-ltd/#!";
            public const string JanaShreeBimaYojanaLink = "https://www.indianbank.in/departments/jana-shree-bima-yojana-launched-in-association-with-lic/#!";
            public const string NewIBJeevanVidyaLink = "https://www.indianbank.in/departments/new-ib-jeevan-vidya-2/#!";
            public const string IBJeevanKalyanLink = "https://www.indianbank.in/departments/ib-jeevan-kalyan/#!";
            public const string IBVarishthaLink = "https://www.indianbank.in/departments/ib-varishtha/#!";
            public const string ArogyaRakshaLink = "https://www.indianbank.in/departments/arogya-raksha/#!";
            public const string IBChhatraLink = "https://www.indianbank.in/departments/ib-chhatra/#!";
            public const string IBGrihaJeevanLink = "https://www.indianbank.in/departments/ib-griha-jeevan-group-insurance-scheme-for-mortgage-borrowers-launched-in-association-with-lic/#!";
            public const string IBYatraSurakshaLink = "https://www.indianbank.in/departments/ib-yatra-suraksha-with-uiic-ltd/#!";
        }

        public class ServicesData
        {
            public string ServiceDataTitle { get; set; }
            public string ServiceDataText { get; set; }
            public string ServiceDataLink { get; set; }
            public string ServiceDataImagePath { get; set; }
        }

        #endregion

    }

}
