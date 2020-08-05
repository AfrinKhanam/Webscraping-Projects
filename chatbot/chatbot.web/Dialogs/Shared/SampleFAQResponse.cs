using System.Collections.Generic;

using Microsoft.Bot.Builder.TemplateManager;
using Microsoft.Bot.Schema;

namespace IndianBank_ChatBOT.Dialogs.Shared
{
    /// <summary>
    /// ScrollbarResponses class
    /// </summary>
    public class SampleFAQResponse : TemplateManager
    {
        #region AboutUsFAQs

        public static SuggestedActions SuggestedActionsForProfileFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Who is the Managing Director of Indian Bank?", value: "Who is the Managing Director of Indian Bank?"),
                    new CardAction(type: ActionTypes.ImBack, title: "Who all are the executive director of indian bank?", value: "Who all are the executive director of indian bank?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is the Official address of chief vigiliance officer?", value: "What is the Official address of chief vigiliance officer?"),
                    new CardAction(type: ActionTypes.ImBack, title: "Who all are the General Managers of indian bank in different division?", value: "who all are the General Managers of indian bank in different division?")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForVisionFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What is the Vision & Mission of Indian Bank?", value: "What is the Vision and Mission of Indian Bank?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is the Vision of Indian Bank?", value: "What is the Vision of Indian Bank?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is the Mission of Indian Bank?", value: "What is the Mission of Indian Bank?")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForManagementFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Who are in the board of directors?", value: "Who are in the board of directors?"),
                    new CardAction(type: ActionTypes.ImBack, title: "Who is the executive director?", value: "Who is the executive director?"),
                    new CardAction(type: ActionTypes.ImBack, title: "Who is the Government Nominee Director?", value: "Who is the Government Nominee Director?"),
                    new CardAction(type: ActionTypes.ImBack, title: "Who is the RBI Nominee Director?", value: "Who is the RBI Nominee Director?")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForFinanceResultFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What is the Quarterly Financial Result of the bank?", value: "What is the Quarterly Financial Result of the bank?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is the Half Yearly Financial Result of the bank?", value: "What is the Half Yearly Financial Result of the bank?"),
                    new CardAction(type: ActionTypes.ImBack, title: "I would like to know Annual Financial Result of the bank?", value: "I would like to know Annual Financial Result of the bank?")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForCorporateGovernanceFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What is IB Corporate Governance?", value: "What is IB Corporate Governance?"),
                    new CardAction(type: ActionTypes.ImBack, title: "I want the Compliance Report for September 2019.", value: "I want the Compliance Report for September 2019."),
                    new CardAction(type: ActionTypes.ImBack, title: "IB compliance report", value: "IB compliance report")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForMutualFundFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What is IB Mutual Fund?", value: "indian bank mutual fund formed"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is Ind TaxShield?", value: "What is Ind TaxShield?"),
                    new CardAction(type: ActionTypes.ImBack, title: "Which office do i have to contact for Redemption Form?", value: "IB Mutual Fund")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForAnnualReportFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "IB Annual Reports", value: "What is the IB annual report for the financial year 2018-2019?")
                });
            }
        }

        #endregion


        #region ProductFAQs

        public static SuggestedActions SuggestedActionsForLoanProductsFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What is the eligibility criteria for the Agricultural Jewel Loan Scheme?", value: "What is the eligibility criteria for the Agricultural Jewel Loan Scheme?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is the purpose of a cold storage loan?", value: "What is the purpose of a cold storage loan?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What kind of benefits are the borrower eligible to upon applying for an  IB home loan combo?", value: "What kind of benefits are the borrower eligible to upon applying for an  IB home loan combo?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is the security required for Financing agriculturists", value: "What is the security required for Financing agriculturists")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForDepositProductsFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What is meant by a term deposit?", value: "What is meant by a term deposit?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What are the conditions for a term deposit account?", value: "What are the conditions for a term deposit account?"),
                    new CardAction(type: ActionTypes.ImBack, title: "How can I open a term deposit account?", value: "How can I open a term deposit account?")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForDigitalProductsFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What are the key features of IB POS?", value: "What are the key features of IB POS?"),
                    new CardAction(type: ActionTypes.ImBack, title: "Who are the target customers in POS?", value: "Who are the target customers in POS?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is Cash POS?", value: "What is Cash POS?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What advantages can I get from Cash POS as a merchent ?", value: "What advantages can I get from Cash POS as a merchent ?")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForFeaturesFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What are the applications supported by blocked amount(ASBA)?", value: "What are the applications supported by blocked amount(ASBA)?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is Centralized Pension Processing System?", value: "What is Centralized Pension Processing System?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What are the facilities provided under ASBA?", value: "What are the facilities provided under ASBA?"),
                    new CardAction(type: ActionTypes.ImBack, title: "How can I apply for the centralized pension?", value: "How can I apply for the centralized pension?")
                });
            }
        }

        // public static SuggestedActions SuggestedActionsForSchemesFAQs
        // {
        //     get
        //     {
        //         return new SuggestedActions(actions: new List<CardAction>()
        //         {
        //             new CardAction(type: ActionTypes.ImBack, title: "What can you tell me about NRI/Foreign Exchange loan schemes?", value: "What can you tell me about NRI/Foreign Exchange loan schemes?"),
        //             new CardAction(type: ActionTypes.ImBack, title: "What is the Central Scheme to provide Interest subsidy(CSIS)?", value: "What is the Central Scheme to provide Interest subsidy(CSIS)?"),
        //             new CardAction(type: ActionTypes.ImBack, title: "What are the NRI Deposit Schemes?", value: "What are the NRI Deposit Schemes?"),
        //             new CardAction(type: ActionTypes.ImBack, title: "What are the Salient features of CSIS?", value: "What are the Salient features of CSIS?")
        //         });
        //     }
        // }

        #endregion

        #region ServicesFAQs

        public static SuggestedActions SuggestedActionsForPremiumServicesFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What are the facilities provided in ATM/Debit cards?", value: "What are the facilities provided in ATM/Debit cards?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is the eligibility for net banking?", value: "What is the eligibility for net banking?"),
                    new CardAction(type: ActionTypes.ImBack, title: "Can customer transfer funds to any account in any bank branch using NEFT ?", value: "Can customer transfer funds to any account in any bank branch using NEFT ?")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForInsuranceServicesFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What is the period coverage for IB Vidyarthi Suraksha?", value: "What is the period coverage for IB Vidyarthi Suraksha?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What are the Benefits of Jana shree Bima yojana?", value: "What are the Benefits of Jana shree Bima yojana?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is  sum insured under IB Jeevan Kalyan?", value: "What is  sum insured under IB Jeevan Kalyan?")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForCMSPlusFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What is CMS Plus?", value: "What is CMS Plus?"),
                    new CardAction(type: ActionTypes.ImBack, title: "Show me the details regarding CMS Plus?", value: "Show me the details regarding CMS Plus?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is the eligibility Criteria of CMS Plus?", value: "What is the eligibility Criteria of CMS Plus?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What are the  salient features of CMS Plus?", value: "What are the  salient features of CMS Plus?")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForDoorStepBankingFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What is doorstep banking?", value: "What is doorstep banking?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What services does doorstep banking include?", value: "What services does doorstep banking include?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What are the services under door step banking?", value: "What are the services under door step banking?"),
                    new CardAction(type: ActionTypes.ImBack, title: "How can I apply for doorstep banking?", value: "How can I apply for doorstep banking?")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForTaxPaymentFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What is E payment of direct taxes?", value: "What is E payment of direct taxes?"),
                    new CardAction(type: ActionTypes.ImBack, title: "Show me the details regarding E payment of direct taxes?", value: "Show me the details regarding E payment of direct taxes?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is the Eligibility criteria for E payment of indirect taxes?", value: "What is the Eligibility criteria for E payment of indirect taxes?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What are the Salient features of E payment of indirect taxes?", value: "What are the Salient features of E payment of indirect taxes?")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForDebentureTrustFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What is Debenture Trustee?", value: "What is Debenture Trustee?"),
                    new CardAction(type: ActionTypes.ImBack, title: "Show me the details regarding Debenture Trustee?", value: "Show me the details regarding Debenture Trustee?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is the disclosure of compensation arrangement with client companies?", value: "What is the disclosure of compensation arrangement with client companies?")
                });
            }
        }

        #endregion

        #region RatesFAQs

        public static SuggestedActions SuggestedActionsForDepositRatesFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What are the Interest Rates on Domestic Term Deposits?", value: "What are the Interest Rates on Domestic Term Deposits?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What can you tell me about Domestic Term Deposists for Senior Citizen Accounts?", value: "What can you tell me about Domestic Term Deposists for Senior Citizen Accounts?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is the pre-closure procedure of FCNR(B) deposits?",value:"What is the pre-closure procedure of FCNR(B) deposits")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForLendingRatesFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What is the lending rate?", value: "What is the lending rate?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is the lending rate on personal segment loan products?", value: "What is the lending rate on personal segment loan products?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is the lending rate on Agricultural Products?", value: "What is the lending rate on Agricultural Products?"),
                });
            }
        }

        public static SuggestedActions SuggestedActionsForServiceChargesFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What are service charges on Forex Rate Cards?", value: "What are service charges on Forex Rate Cards?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What are the service charges on Forex transactions?", value: "What are the service charges on Forex transactions?"),
                    new CardAction(type: ActionTypes.ImBack, title: "What are the service charges on Loans and Advances?", value: "What are the service charges on Loans and Advances?")
                });
            }
        }

        #endregion

        #region ContactsFAQs


        public static SuggestedActions SuggestedActionsForCustomerSupportFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Show me the Customer Complaints Officers List.", value: "Show me the Customer Complaints Officers List."),
                    new CardAction(type: ActionTypes.ImBack, title: "Please provide some details regarding the nodal officer for Customer Service.", value: "Please provide some details regarding the nodal officer for Customer Service."),
                    new CardAction(type: ActionTypes.ImBack, title: "Please provide contact details regarding Grievance Redressal.", value: "Please provide contact details regarding Grievance Redressal.")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForEmailIDFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "What is the email ID of Bank Assurance Centre?", value: "What is the email ID of Bank Assurance Centre?"),
                    new CardAction(type: ActionTypes.ImBack, title: "How can I contact the Indian Bank Head office?", value: "How can I contact the Indian Bank Head office?"),
                    new CardAction(type: ActionTypes.ImBack, title: "Get me the email address of the Credit Card Centre.", value: "Get me the email address of the Credit Card Centre.")
                });
            }
        }

        #endregion


        #region LinksFAQs



        public static SuggestedActions SuggestedActionsForOnlineServicesFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Can I get the link for checking the status of my loan application", value: "Can I get the link for checking the status of my loan application"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is the link to download IB Interest Certificate?", value: "What is the link to download IB Interest Certificate?"),
                    new CardAction(type: ActionTypes.ImBack, title: "Please provide me the link for Jewel Loan Appointments.", value: "Please provide me the link for Jewel Loan Appointments."),
                    new CardAction(type: ActionTypes.ImBack, title: "How can I book my train ticket online?", value: "How can I book my train ticket online?")
                });
            }
        }



        public static SuggestedActions SuggestedActionsForRelatedSitesFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Give me a link to open a Merchant Bank account?", value: "Give me a link to open a Merchant Bank account?"),
                    new CardAction(type: ActionTypes.ImBack, title: "Give me the address of the Indian Bank Colombo Branch", value: "Give me the address of the Indian Bank Colombo Branch"),
                    new CardAction(type: ActionTypes.ImBack, title: "What is the telephone number of the Indian Bank Jaffna Branch", value: "What is the telephone number of the Indian Bank Jaffna Branch")
                });
            }
        }



        public static SuggestedActions SuggestedActionsForAlliancesFAQs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Provide a link to the Oriental Bank of Commerce", value: "Provide a link to the Oriental Bank of Commerce"),
                    new CardAction(type: ActionTypes.ImBack, title: "What are the services provided under Corporation Bank", value: "What are the services provided under Corporation Bank"),
                    new CardAction(type: ActionTypes.ImBack, title: "What can you tell me about United India Insurance Company LTD.", value: "What can you tell me about United India Insurance Company LTD.")
                });
            }
        }


        #endregion

    }
}
