using System.Collections.Generic;

using Microsoft.Bot.Builder.TemplateManager;
using Microsoft.Bot.Schema;

namespace IndianBank_ChatBOT.Dialogs.Shared
{
    /// <summary>
    /// SharedResponses class
    /// </summary>
    public class SharedResponses : TemplateManager
    {
        #region Methods
        /// <summary>
        /// Code to display suggested actions for main menu
        /// </summary>
        public static SuggestedActions SuggestedActionsForMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Loans", value: "loan"),
                    new CardAction(type: ActionTypes.ImBack, title: "Deposits", value: "deposits"),
                    new CardAction(type: ActionTypes.ImBack, title: "Services", value: "services"),
                    new CardAction(type: ActionTypes.ImBack, title: "Rates", value: "rates"),
                    new CardAction(type: ActionTypes.ImBack, title: "News/Info", value: "news"),
                    new CardAction(type: ActionTypes.ImBack, title: "Contacts", value: "contacts")
                });
            }
        }

        /// <summary>
        /// Code to display suggested actions for Deposit Menu
        /// </summary>
        public static SuggestedActions SuggestedActionsForDepositMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Savings Bank A/C", value: "savings bank account"),
                    new CardAction(type: ActionTypes.ImBack, title: "Current A/C", value: "current account types"),
                    new CardAction(type: ActionTypes.ImBack, title: "Term Deposits", value: "types of term deposit"),
                    new CardAction(type: ActionTypes.ImBack, title: "NRI A/C's", value: "nri accounts")
                });
            }
        }

        /// <summary>
        /// Code to display suggested actions for Loan Menu 
        /// </summary>
        public static SuggestedActions SuggestedActionsForLoanMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Agriculture", value: "agriculture"),
                    new CardAction(type: ActionTypes.ImBack, title: "Groups", value: "groups"),
                    new CardAction(type: ActionTypes.ImBack, title: "Personal/Individual", value: "personal"),
                    new CardAction(type: ActionTypes.ImBack, title: "MSME", value: "msme loan"),
                    new CardAction(type: ActionTypes.ImBack, title: "Education", value: "education"),
                    new CardAction(type: ActionTypes.ImBack, title: "NRI", value: "nri loan"),
                    new CardAction(type: ActionTypes.ImBack, title: "59 Minutes Loans", value: "59 minutes loans")
                });
            }
        }


        public static SuggestedActions SuggestedActionsForMSMELoanMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "IB Vidhya Mandir", value: "ib vidhya mandir"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB My Own Shop", value: "ib my own shop"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Doctor Plus", value: "ib doctor plus"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Contractors", value: "ib contractors"),
                    new CardAction(type: ActionTypes.ImBack, title: "Tradewell", value: "tradewell"),
                    new CardAction(type: ActionTypes.ImBack, title: "Ind SME Secure", value: "ind sme secure")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForEducationLoanMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Revised IBA Model Educational Loan Scheme (2015)", value: "model education loan scheme"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Educational Loan Prime", value: "educational loan prime"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Skill Loan Scheme", value: "skill loan scheme"),
                    new CardAction(type: ActionTypes.ImBack, title: "Education Loan Interest Subsidies", value: "education loan interest")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForNRILoanMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "NRI Plot Loan", value: "nri plot loan"),
                    new CardAction(type: ActionTypes.ImBack, title: "NRI Home Loan", value: "nri home loan")
                });
            }
        }

        /// <summary>
        /// Code to display suggested actions for Services Menu 
        /// </summary>
        public static SuggestedActions SuggestedActionsForServicesMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Premium Services", value: "premium services"),
                    new CardAction(type: ActionTypes.ImBack, title: "Insurance Services", value: "insurance services"),
                    new CardAction(type: ActionTypes.ImBack, title: "CMS Plus", value: "cms plus"),
                    new CardAction(type: ActionTypes.ImBack, title: "E Payment Of Direct Taxes", value: "direct taxes"),
                    new CardAction(type: ActionTypes.ImBack, title: "E Payment Of Indirect Taxes", value: "indirect taxes")
                });
            }
        }

        /// <summary>
        /// Code to display suggested actions for rates Menu 
        /// </summary>
        public static SuggestedActions SuggestedActionsForRatesMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Deposit Rates", value: "deposit rates"),
                    new CardAction(type: ActionTypes.ImBack, title: "Lending Rates", value: "lending rates"),
                    new CardAction(type: ActionTypes.ImBack, title: "Service Charges/Forex Rates", value: "Service Charges")
                });
            }
        }

        /// <summary>
        /// Code to display suggested actions for news/info Menu 
        /// </summary>
        public static SuggestedActions SuggestedActionsForNewsOrInfoMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Notifications", value: "notifications"),
                    new CardAction(type: ActionTypes.ImBack, title: "NewsLetter-IND NAVYA", value: "news letter"),
                    new CardAction(type: ActionTypes.ImBack, title: "What Is New", value: "what is new"),
                    new CardAction(type: ActionTypes.ImBack, title: "SMS Banking", value: "sms banking"),
                    new CardAction(type: ActionTypes.ImBack, title: "Scan & Pay", value: "scan and pay"),
                    new CardAction(type: ActionTypes.ImBack, title: "My Design Card", value: "design card"),
                    new CardAction(type: ActionTypes.ImBack, title: "Press Releases", value: "press releases"),
                    new CardAction(type: ActionTypes.ImBack, title: "Customer Corner", value: "customer corner"),
                    new CardAction(type: ActionTypes.ImBack, title: "Related Info", value: "related info"),
                    new CardAction(type: ActionTypes.ImBack, title: "Downloads-Application", value: "downloads"),
                    new CardAction(type: ActionTypes.ImBack, title: "Codes/Policy/Disclosures", value: "codes"),
                    new CardAction(type: ActionTypes.ImBack, title: "Charters/Schemes", value: "charters")
                });
            }
        }


        /// <summary>
        /// Code to display suggested actions for contacts Menu 
        /// </summary>
        public static SuggestedActions SuggestedActionsForContactsMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Quick Contacts", value: "quick contact"),
                    new CardAction(type: ActionTypes.ImBack, title: "Customer Support", value: "customer support"),
                    new CardAction(type: ActionTypes.ImBack, title: "Email IDs", value: "email")
                });
            }
        }

        /// <summary>
        /// Code to display suggested actions for CustomerCornerMenu
        /// </summary>
        public static SuggestedActions SuggestedActionsForCustomerCornerMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "CustomerComplaintForm (.pdf)", value: "customer complaint form"),
                    new CardAction(type: ActionTypes.ImBack, title: "Online Customer Complaints", value: "online customer complaints"),
                    new CardAction(type: ActionTypes.ImBack, title: "Nodal Officers-Banking Ombudsman Scheme, 2006", value: "banking ombudsman scheme"),
                    new CardAction(type: ActionTypes.ImBack, title: "Nodal Officers-Customer Service", value: "customer service"),
                    new CardAction(type: ActionTypes.ImBack, title: "Principal Code Compliance Officer (BCSBI)", value: "Principal Code Compliance Officer"),
                    new CardAction(type: ActionTypes.ImBack, title: "Damodaran Committee Recommendations", value: "damodaran committee recommendations"),
                    new CardAction(type: ActionTypes.ImBack, title: "Banking Ombudsman", value: "banking ombudsman"),
                    new CardAction(type: ActionTypes.ImBack, title: "Remit To India", value: "remit to india"),
                    new CardAction(type: ActionTypes.ImBack, title: "Aadhaar Enrollment/Correction Form", value: "aadhaar enrollment"),
                    new CardAction(type: ActionTypes.ImBack, title: "Procedure On Locker/Safe Deposit Of Articles", value: "procedure on locker"),
                    new CardAction(type: ActionTypes.ImBack, title: "Coin Vending Machines (CVM's)-Location In Chennai", value: "coin vending machines"),
                    new CardAction(type: ActionTypes.ImBack, title: "Indian Bank Trust For Rural Development (IBTRD)", value: "indian bank trust rural development"),
                    new CardAction(type: ActionTypes.ImBack, title: "Low Cost Mobile Banking Through USSD", value: "low cost mobile banking through ussd")
                });
            }
        }

        /// <summary>
        /// Code to display suggested actions for RelatedInfoMenu
        /// </summary>
        public static SuggestedActions SuggestedActionsForRelatedInfoMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "F.A.Qs", value: "F.A.Qs"),
                    new CardAction(type: ActionTypes.ImBack, title: "FAQs-On Pradhan Mantri Jan-Dhan Yojana (PMJDY)-Hindi", value: "pradhan mantri jan dhan yojana"),
                    //new CardAction(type: ActionTypes.ImBack, title: "Online Customer Complaints", value: "what is new rates"),//;;;
                    new CardAction(type: ActionTypes.ImBack, title: "Recovery Agents Empanelled/Engaged By Band (pdf)", value: "recovery agents empanelled"),
                    new CardAction(type: ActionTypes.ImBack, title: "ECS Notice To Customers (pdf)", value: "ecs notice to customers"),
                    new CardAction(type: ActionTypes.ImBack, title: "List Of Holidays, 2018(pdf)", value: "list of holidays"),
                    new CardAction(type: ActionTypes.ImBack, title: "Disclaimer", value: "disclaimer"),
                    new CardAction(type: ActionTypes.ImBack, title: "Security Alert (.pdf)", value: "security alert")
                });
            }
        }

        /// <summary>
        /// Code to display suggested actions for CodesPolicyDisclosuresMenu
        /// </summary>
        public static SuggestedActions SuggestedActionsForCodesPolicyDisclosuresMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Rights of Bank's Customers", value: "rights of bank customers"),
                    new CardAction(type: ActionTypes.ImBack, title: "Dealing Dishonour of Cheques", value: "dealing dishonour of cheques"),
                    new CardAction(type: ActionTypes.ImBack, title: "Deposit Policy", value: "deposit policy"),
                    new CardAction(type: ActionTypes.ImBack, title: "Best Practices Code of the Bank", value: "best practices code of the bank"),
                    new CardAction(type: ActionTypes.ImBack, title: "BCSBI-Codes of Bank's Commitment to Customers", value: "banks commitment to customers"),
                    new CardAction(type: ActionTypes.ImBack, title: "Policy On Determining Material Subsidiary", value: "detremining marital subsidiary"),
                    new CardAction(type: ActionTypes.ImBack, title: "Policy On Determination And Disclosure of Material Events/Information", value: "Determination And Disclosure of Material Events"),
                    new CardAction(type: ActionTypes.ImBack, title: "Policy On Realted Party Transactions", value: "Policy On Realted Party Transactions"),
                    new CardAction(type: ActionTypes.ImBack, title: "Policy-Guidelines On Empanelment Of Valuers", value: "guidelines on empanelment of valuers"),
                    new CardAction(type: ActionTypes.ImBack, title: "Policy-Appointment of Statutory Central/Branch Auditors", value: "Appointment of Statutory Central"),
                    new CardAction(type: ActionTypes.ImBack, title: "Right To Information Act-2005", value: "right to information act"),
                    new CardAction(type: ActionTypes.ImBack, title: "Base I-II Disclosures", value: "base disclosures"),
                    new CardAction(type: ActionTypes.ImBack, title: "Customer Centric Services", value: "customer centric services"),
                    new CardAction(type: ActionTypes.ImBack, title: "Debt Restructing Mechanism For SMES", value: "debt restructing mechanism"),
                    new CardAction(type: ActionTypes.ImBack, title: "Fair Lending Practices Code", value: "fair lending practices"),
                    new CardAction(type: ActionTypes.ImBack, title: "Processing Fees/Charges For Loan Products", value: "Processing Fees"),
                    new CardAction(type: ActionTypes.ImBack, title: "Processing Charges In Agri Term Loans, JL(Ag), SHG Loan & JL (NP)", value: "Processing Charges In Agri Term Loans"),
                    new CardAction(type: ActionTypes.ImBack, title: "Processing Charges of Home/ Plot/ Vehicle etc products", value: "Processing Charges of Home"),
                    new CardAction(type: ActionTypes.ImBack, title: "Processing Charges of SME Products", value: "Charges of SME Products"),
                    new CardAction(type: ActionTypes.ImBack, title: "Codes of Bank's Commitment to Customers/ MSE in Hindi", value: "MSE in Hindi"),
                    new CardAction(type: ActionTypes.ImBack, title: "Codes of Bank's Commitment to Customers/ MSE", value: "MSE")
                });
            }
        }

        /// <summary>
        /// Code to display suggested actions for ChartersSchemesMenu
        /// </summary>
        public static SuggestedActions SuggestedActionsForChartersSchemesMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Agricultural Debt Waiver And Debt Relief Scheme, 2008", value: "agricultural debt waiver"),
                    new CardAction(type: ActionTypes.ImBack, title: "Banking Ombudsman", value: "Charters Banking Ombudsman"),
                    new CardAction(type: ActionTypes.ImBack, title: "Financial Inclusion Plan 1020-12 Name of Villages and Field BCs", value: "Financial Inclusion Plan"),
                    new CardAction(type: ActionTypes.ImBack, title: "RESTRUCTURED ACCOUNTS", value: "RESTRUCTURED ACCOUNTS"),
                    new CardAction(type: ActionTypes.ImBack, title: "Services Rendered Free of Charge", value: "Services Rendered Free of Charge"),
                    new CardAction(type: ActionTypes.ImBack, title: "Welfare of Minorities", value: "Welfare of Minorities"),
                    new CardAction(type: ActionTypes.ImBack, title: "Whistle Blower Policy", value: "Whistle Blower Policy"),
                    new CardAction(type: ActionTypes.ImBack, title: "Centralized Pension Processing System", value: "Centralized Pension Processing System"),
                     new CardAction(type: ActionTypes.ImBack, title: "Another Option For Pension", value: "Another Option For Pension"),
                    new CardAction(type: ActionTypes.ImBack, title: "CITIZENS CHARTER", value: "CITIZENS CHARTER"),
                    new CardAction(type: ActionTypes.ImBack, title: "INDIAN BANK MUTUAL FUND", value: "INDIAN BANK MUTUAL FUND")
                });
            }
        }
        /// <summary>
        /// Gets the suggested actions for savings menu.
        /// </summary>
        public static SuggestedActions SuggestedActionsForSavingsBankAccountMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Savings Bank", value: "savings bank"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB CORP SB-Payroll Package Scheme For Salaried Class", value: "ib corp sb payroll package"),
                    new CardAction(type: ActionTypes.ImBack, title: "VIKAS SAVINGS KHATA-A No Frills Savings Bank Account", value: "vikas savings khata"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Smart Kid", value: "ib smart kid"),
                    new CardAction(type: ActionTypes.ImBack, title: "IMPORTANT TERMS AND CONDITIONS", value: "terms and conditions of savings bank account"),
                    new CardAction(type: ActionTypes.ImBack, title: "SB Platinum", value: "sb platinum"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB-Surabhi", value: "ib surabhi")
                });
            }
        }

        public static SuggestedActions SuggestedActionsCurrentAccountMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Current Account", value: "current account"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB I-Freedom Current Account", value: "freedom current account"),
                    new CardAction(type: ActionTypes.ImBack, title: "IMPORTANT TERMS AND CONDITIONS", value: "terms and conditions of current account"),
                    new CardAction(type: ActionTypes.ImBack, title: "Premium Current Account", value: "premium current account")
                });
            }
        }

        public static SuggestedActions SuggestedActionsTermDepositsMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Facility Deposit", value: "facility deposit"),
                    new CardAction(type: ActionTypes.ImBack, title: "Capital Gains", value: "capital gains"),
                    new CardAction(type: ActionTypes.ImBack, title: "TERMS AND CONDITIONS-TERM DEPOSIT ACCOUNT", value: "terms and conditions of term deposit"),
                    new CardAction(type: ActionTypes.ImBack, title: "Deposit Scheme For Senior Citizens", value: "deposit scheme for senior citizens"),
                    //new CardAction(type: ActionTypes.ImBack, title: "Recurring Deposit", value: "recurring deposit"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Tax Saver Scheme", value: "ib tax saver scheme"),
                    new CardAction(type: ActionTypes.ImBack, title: "Insured Recurring Deposit", value: "insured recurring deposit"),
                    new CardAction(type: ActionTypes.ImBack, title: "Re-Investment Plan", value: "re investment plan"),
                    new CardAction(type: ActionTypes.ImBack, title: "Fixed Deposit", value: "fixed deposit"),
                    new CardAction(type: ActionTypes.ImBack, title: "Variable Recurring Deposit", value: "variable recurring deposit")
                });
            }
        }

        public static SuggestedActions SuggestedActionsNriAccountsMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
               {
                   new CardAction(type: ActionTypes.ImBack, title: "Resident Foreign Currency Account For Returning Indians", value: "resident foreign currency account for returning indians"),
                   new CardAction(type: ActionTypes.ImBack, title: "NRE FD/ RIP/ RD ACCOUNTS", value: "nre fd"),
                   new CardAction(type: ActionTypes.ImBack, title: "NRE SB ACCOUNTS", value: "nre sb accounts"),
                   new CardAction(type: ActionTypes.ImBack, title: "Non-Resident Ordinary Account", value: "non resident ordinary account"),
                   new CardAction(type: ActionTypes.ImBack, title: "FCNR (B) Accounts", value: "fcnr accounts"),
               });
            }
        }
        /// <summary>
        /// Gets the suggested actions for Services menu.
        /// </summary>
        public static SuggestedActions SuggestedActionsForPremiumServices
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "MCA Payment", value: "mca payment"),
                    new CardAction(type: ActionTypes.ImBack, title: "Money Gram", value: "money gram"),
                    new CardAction(type: ActionTypes.ImBack, title: "ATM/DebitCard", value: "atm debit card"),
                    new CardAction(type: ActionTypes.ImBack, title: "Ind Mobile Banking", value: "ind mobile banking"),
                    new CardAction(type: ActionTypes.ImBack, title: "Ind Net Banking", value: "ind net banking"),
                    new CardAction(type: ActionTypes.ImBack, title: "Credit Cards", value: "credit card"),
                    new CardAction(type: ActionTypes.ImBack, title: "Xpress Money", value: "xpress money"),
                    new CardAction(type: ActionTypes.ImBack, title: "N E F T", value: "neft"),
                    new CardAction(type: ActionTypes.ImBack, title: "Ind Jet Remit(RTGS)", value: "ind jet remit"),
                    new CardAction(type: ActionTypes.ImBack, title: "Multicity Cheque Facility", value: "multicity cheque facility")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForInsuranceServices
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "IB Vidyarthi Suraksha", value: "ib vidyarthi suraksha"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Home Security", value: "ib home security"),
                    new CardAction(type: ActionTypes.ImBack, title: "Universal Health Care", value: "universal health care"),
                    new CardAction(type: ActionTypes.ImBack, title: "Jana Shree Bima Yojana", value: "jana shree bima yojana"),
                    new CardAction(type: ActionTypes.ImBack, title: "New IB Jeevan Vidya", value: "new ib jeevan vidya"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Jeevan Kalyan", value: "ib jeevan kalyan"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Varishtha", value: "ib varishtha"),
                    new CardAction(type: ActionTypes.ImBack, title: "Arogya Raksha", value: "arogya raksha"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Chhatra", value: "ib chhatra"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Griha Jeevan", value: "ib griha jeevan"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Yatra Suraksha", value: "ib yatra suraksha")
                });
            }
        }


        /// <summary>
        /// Gets the suggested actions for Services menu.
        /// </summary>
        public static SuggestedActions SuggestedActionsForCustomerSupport
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Customer Complaints", value: "customer complaints"),
                    new CardAction(type: ActionTypes.ImBack, title: "Complaints Officers List", value: "complaints officers"),
                    new CardAction(type: ActionTypes.ImBack, title: "Chief Vigilance Officer", value: "chief vigilance officer")
                });
            }
        }

        public static SuggestedActions SuggestedActionsForEmailIDs
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Head Office", value: "head office"),
                    new CardAction(type: ActionTypes.ImBack, title: "Department", value: "department"),
                    new CardAction(type: ActionTypes.ImBack, title: "Executives", value: "executives"),
                    new CardAction(type: ActionTypes.ImBack, title: "IMAGE", value: "image"),
                    new CardAction(type: ActionTypes.ImBack, title: "Foreign Branches", value: "foregin branches email ids"),
                    new CardAction(type: ActionTypes.ImBack, title: "Overseas Branches", value: "overseas branches"),
                    new CardAction(type: ActionTypes.ImBack, title: "NRI Branches", value: "nri branches"),
                    new CardAction(type: ActionTypes.ImBack, title: "Zonal Offices", value: "zonal offices"),
                    new CardAction(type: ActionTypes.ImBack, title: "E Conformation Of Bank Guarantee", value: "conformation of bank guarantee")
                });
            }
        }



        /// <summary>
        /// Gets the suggested actions for savings bank menu.
        /// </summary>
        /// <value>
        /// The suggested actions for savings bank menu.
        /// </value>
        public static SuggestedActions SuggestedActionsForSavingsBankMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Resident Foreign Currency Account For Returning Indians", value: "resident foreign currency account for returning indians"),
                    new CardAction(type: ActionTypes.ImBack, title: "NRE FD/ RIP/ RD ACCOUNTS", value: "nre fd"),
                    new CardAction(type: ActionTypes.ImBack, title: "NRE SB ACCOUNTS", value: "nre sb accounts"),
                    new CardAction(type: ActionTypes.ImBack, title: "Non-Resident Ordinary Account", value: "non resident ordinary account"),
                    new CardAction(type: ActionTypes.ImBack, title: "FCNR (B) Accounts", value: "fcnr accounts")
                });
            }
        }

        /// <summary>
        /// Gets the suggested actions Agriculture Menu
        /// </summary>
        /// <value>
        /// The suggested actions for savings bank menu.
        /// </value>
        public static SuggestedActions SuggestedActionsForAgricultureMenu
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Agricultural Godowns / Cold Storage", value: "agricultural godowns"),
                    new CardAction(type: ActionTypes.ImBack, title: "Loans for maintenance of Tractors under tie-up with Sugar Mills", value: "loans for maintenance of tractors"),
                    new CardAction(type: ActionTypes.ImBack, title: "Agricultural Produce Marketing Loan", value: "agricultural produce marketing loan"),
                    new CardAction(type: ActionTypes.ImBack, title: "Financing Agriculturists for Purchase of Tractors", value: "financing agriculturists for purchase of tractors"),
                    new CardAction(type: ActionTypes.ImBack, title: "Purchase of second hand (pre-used) Tractors by Agriculturists", value: "purchase of second hand tractors by agriculturists"),
                    new CardAction(type: ActionTypes.ImBack, title: "Agri Clinic and Agri Business Centres", value: "agri clinic and agri business centres"),
                    new CardAction(type: ActionTypes.ImBack, title: "SHG Bank Linkage Programme – Direct Linkage to SHGs", value: "shg bank linkage programme loan"),
                    new CardAction(type: ActionTypes.ImBack, title: "Joint liability group (JLG)", value: "joint liability group"),
                    new CardAction(type: ActionTypes.ImBack, title: "Rupay Kisan card", value: "rupay kisan card"),
                    new CardAction(type: ActionTypes.ImBack, title: "DRI Scheme – Revised Norms", value: "dri scheme revised norms"),
                    new CardAction(type: ActionTypes.ImBack, title: "SHG – Vidhya Shoba", value: "shg vidhya shoba loan"),
                    new CardAction(type: ActionTypes.ImBack, title: "Gramin Mahila Sowbhagya Scheme", value: "gramin mahila sowbhagya scheme"),
                    new CardAction(type: ActionTypes.ImBack, title: "Sugar Premium Scheme", value: "sugar premium scheme"),
                    new CardAction(type: ActionTypes.ImBack, title: "Golden Harvest Scheme", value: "golden harvest scheme"),
                    new CardAction(type: ActionTypes.ImBack, title: "Agricultural Jewel Loan Scheme", value: "agricultural jewel loan scheme")
                });
            }
        }

        /// <summary>
        /// Gets the suggested actions Groups Menu
        /// </summary>
        /// <value>
        /// The suggested actions for Groups Menu
        /// </value>
        public static SuggestedActions SuggestedActionsForGroups
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "Agricultural Godowns / Cold Storage", value: "groups agricultural godowns"),
                    new CardAction(type: ActionTypes.ImBack, title: "SHG Bank Linkage Programme – Direct Linkage to SHGs", value: "groups shg bank linkage programme"),
                    new CardAction(type: ActionTypes.ImBack, title: "SHG – Vidhya Shoba", value: "groups shg vidhya shoba")
                });
            }
        }

        /// <summary>
        /// Gets the suggested actions Groups Menu
        /// </summary>
        /// <value>
        /// The suggested actions for Groups Menu
        /// </value>
        public static SuggestedActions SuggestedActionsForPersonalIndividual
        {
            get
            {
                return new SuggestedActions(actions: new List<CardAction>()
                {
                    new CardAction(type: ActionTypes.ImBack, title: "IB Home Loan Combo", value: "ib home loan combo"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Rent Encash", value: "ib rent encash"),
                    new CardAction(type: ActionTypes.ImBack, title: "Loan / OD against Deposits", value: "loan od against deposits"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Clean Loan (to Salaried Class)", value: "ib clean loan"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Balavidhya Scheme", value: "ib balavidhya scheme"),
                    new CardAction(type: ActionTypes.ImBack, title: "Ind Reverse Mortgage", value: "ind reverse mortgage"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Vehicle Loan", value: "ib vehicle loan"),
                    new CardAction(type: ActionTypes.ImBack, title: "Ind Mortgage", value: "ind mortgage"),
                    new CardAction(type: ActionTypes.ImBack, title: "Plot Loan", value: "plot"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Home Loan", value: "ib home loan"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Pension Loan", value: "ib pension loan"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Home Improve", value: "home improve"),
                    new CardAction(type: ActionTypes.ImBack, title: "IB Home Loan Plus", value: "ib home loan plus"),
                    new CardAction(type: ActionTypes.ImBack, title: "Loan / OD against NSC / KVP / Relief bonds of RBI / LIC policies", value: "od against nsc"),
                });
            }
        }

        #endregion
    }
}
