// using System.Collections.Generic;
// using System.IO;

// using UjjivanBank_ChatBOT.Dialogs.Shared;

// using Microsoft.Bot.Builder;
// using Microsoft.Bot.Builder.TemplateManager;
// using Microsoft.Bot.Schema;

// namespace UjjivanBank_ChatBOT.Dialogs.NewsInfo
// {
//     public class NewsInfoResponses : TemplateManager
//     {
//         #region Properties

//         private static LanguageTemplateDictionary _responseTemplates = new LanguageTemplateDictionary
//         {
//             ["default"] = new TemplateIdMap
//             {
//                 { ResponseIds.BuildNewsInfoCard, (context, data) => BuildNewsInfoCard(context, data) },
//                 { ResponseIds.NewsOrInfoMenuCardDisplay, (context,data) => BuildNewsOrInfoMenuCard(context) },
//                 { ResponseIds.CustomerCornerMenuCardDisplay, (context,data) => BuildCustomerCornerMenuCard(context) },
//                 { ResponseIds.RelatedInfoMenuCardDisplay, (context,data) => BuildRelatedInfoMenuCard(context) },
//                 { ResponseIds.CodesPolicyDisclosuresMenuCardDisplay, (context,data) => BuildCodesPolicyDisclosuresMenuCard(context) },
//                 { ResponseIds.ChartersSchemesMenuCardDisplay, (context,data) => BuildChartersSchemesMenuCard(context) },
//             }
//         };

//         public static Dictionary<string, NewsInfoResponses.NewsInfoData> keyValuePairs = new Dictionary<string, NewsInfoResponses.NewsInfoData>
//         {
//             //news/info Menu Items
//             //keys are same as entity
//             { "notifications",new NewsInfoData{NewsInfoDataTitle="Notifications",NewsInfoDataText="To know more about Notifications. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.Notification,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "Notifications.jpg")} },
//             { "news letter",new NewsInfoData{NewsInfoDataTitle="News Letter-IND NAVYA",NewsInfoDataText="To know more about News Letter-IND NAVYA. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.NewsLetter,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "NewsLetter.jpg")} },
//             { "new",new NewsInfoData{NewsInfoDataTitle="What Is New",NewsInfoDataText="To know more about What Is New. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.WhatsNew,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "WhatsNew.jpg")} },
//             { "sms banking",new NewsInfoData{NewsInfoDataTitle="SMS Banking",NewsInfoDataText="To know more about SMS Banking. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.SmsBanking,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "SmsBanking.jpg")} },
//             { "scan and pay",new NewsInfoData{NewsInfoDataTitle="Scan & Pay",NewsInfoDataText="To know more about Scan & Pay. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.ScanAndPay,NewsInfoDataImagePath=""} },
//             { "my design card",new NewsInfoData{NewsInfoDataTitle="My Design Card",NewsInfoDataText="To know more about My Design Card. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.MyDesignCard,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "MyDesignCard.PNG")} },
//             { "press releases",new NewsInfoData{NewsInfoDataTitle="Press Releases",NewsInfoDataText="To know more about Press Releases. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.PressReleases,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "PressReleases.jpg")} },
//             { "downloads",new NewsInfoData{NewsInfoDataTitle="Downloads",NewsInfoDataText="To know more about Downloads. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.Downloads,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "Downloads.jpg")} },

//             //customer corner
//             { "customer complaint form",new NewsInfoData{NewsInfoDataTitle="CustomerComplaintForm (.pdf)",NewsInfoDataText="To know more about CustomerComplaintForm (.pdf). Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.CustomerComplaintForm,NewsInfoDataImagePath=""} },
//             { "online customer complaints",new NewsInfoData{NewsInfoDataTitle="Online Customer Complaints",NewsInfoDataText="To know more about Online Customer Complaints. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.OnlineCustomerComplaints,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "OnlineCustomerComplaints.jpg")} },
//             { "banking ombudsman scheme",new NewsInfoData{NewsInfoDataTitle="Nodal Officers-Banking Ombudsman Scheme, 2006",NewsInfoDataText="To know more about Nodal Officers-Banking Ombudsman Scheme, 2006. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.NodalOfficersBankingOmbudsmanScheme,NewsInfoDataImagePath=""} },
//             { "customer service",new NewsInfoData{NewsInfoDataTitle="Nodal Officers-Customer Service",NewsInfoDataText="To know more about Nodal Officers-Customer Service. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.NodalOfficersCustomerService,NewsInfoDataImagePath=""} },
//             { "principal code compliance officer",new NewsInfoData{NewsInfoDataTitle="Principal Code Compliance Officer (BCSBI)",NewsInfoDataText="To know more about Principal Code Compliance Officer (BCSBI). Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.PrincipalCodeComplianceOfficer,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "PrincipalCodeComplianceOfficer.jpg")} },
//             { "damodaran committee recommendations",new NewsInfoData{NewsInfoDataTitle="Damodaran Committee Recommendations",NewsInfoDataText="To know more about Damodaran Committee Recommendations. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.DamodaranCommitteeRecommendations,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "DamodaranCommitteeRecommendations.jpg")} },
//             { "banking ombudsman",new NewsInfoData{NewsInfoDataTitle="Banking Ombudsman",NewsInfoDataText="To know more about Banking Ombudsman. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.BankingOmbudsman,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "BankingOmbudsman.jpg")} },
//             { "remit to india",new NewsInfoData{NewsInfoDataTitle="Remit To India",NewsInfoDataText="To know more about Remit To India. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.RemitToIndia,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "RemitToIndia.jpg")} },
//             { "aadhar enrolment correction form",new NewsInfoData{NewsInfoDataTitle="Aadhaar Enrollment/Correction Form",NewsInfoDataText="To know more about Aadhaar Enrollment/Correction Form. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.AadhaarEnrollment,NewsInfoDataImagePath=""} },
//             { "procedure on locker",new NewsInfoData{NewsInfoDataTitle="Procedure On Locker/Safe Deposit Of Articles",NewsInfoDataText="To know more about Procedure On Locker/Safe Deposit Of Articles. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.ProcedureOnLocker,NewsInfoDataImagePath=""} },
//             { "coin vending machines",new NewsInfoData{NewsInfoDataTitle="Coin Vending Machines (CVM's)-Location In Chennai",NewsInfoDataText="To know more about Coin Vending Machines (CVM's)-Location In Chennai. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.CoinVendingMachines,NewsInfoDataImagePath=""} },
//             { "indian bank trust rural development",new NewsInfoData{NewsInfoDataTitle="Indian Bank Trust For Rural Development (IBTRD)",NewsInfoDataText="To know more about Indian Bank Trust For Rural Development (IBTRD). Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.UjjivanBankTrustForRuralDevelopment,NewsInfoDataImagePath=""} },
//             { "mobile banking through ussd",new NewsInfoData{NewsInfoDataTitle="Low Cost Mobile Banking Through USSD",NewsInfoDataText="To know more about Low Cost Mobile Banking Through USSD. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.LowCostMobileBankingThroughUSSD,NewsInfoDataImagePath=""} },

//             //related info
//             { "faq",new NewsInfoData{NewsInfoDataTitle="F.A.Qs",NewsInfoDataText="To know more about F.A.Qs. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.FAQs,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "FAQs.jpg")} },
//             { "pradhan mantri jan dhan yojana",new NewsInfoData{NewsInfoDataTitle="FAQs-On Pradhan Mantri Jan-Dhan Yojana (PMJDY)-Hindi",NewsInfoDataText="To know more about FAQs-On Pradhan Mantri Jan-Dhan Yojana (PMJDY)-Hindi. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.FAQsOnPradhanMantriJanDhanYojana,NewsInfoDataImagePath=""} },
//             { "recovery agents empanelled",new NewsInfoData{NewsInfoDataTitle="Recovery Agents Empanelled/Engaged By Band (pdf)",NewsInfoDataText="To know more about Recovery Agents Empanelled/Engaged By Band (pdf). Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.RecoveryAgentsEmpanelledEngagedByBand,NewsInfoDataImagePath=""} },
//             { "ecs notice to customers",new NewsInfoData{NewsInfoDataTitle="ECS Notice To Customers (pdf)",NewsInfoDataText="To know more about ECS Notice To Customers (pdf). Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.ECS_NoticeToCustomers,NewsInfoDataImagePath=""} },
//             { "list of holidays",new NewsInfoData{NewsInfoDataTitle="List Of Holidays, 2018(pdf)",NewsInfoDataText="To know more about List Of Holidays, 2018(pdf). Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.ListOfHolidays,NewsInfoDataImagePath=""} },
//             { "disclaimer",new NewsInfoData{NewsInfoDataTitle="Disclaimer",NewsInfoDataText="To know more about Disclaimer. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.Disclaimer,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "Disclaimer.jpg")} },
//             { "security alert",new NewsInfoData{NewsInfoDataTitle="Security Alert (.pdf)",NewsInfoDataText="To know more about Security Alert (.pdf). Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.SecurityAlert,NewsInfoDataImagePath=""} },

//             //codes/policy/disclosure
//             { "rights of bank customers",new NewsInfoData{NewsInfoDataTitle="Rights of Bank's Customers",NewsInfoDataText="To know more about Rights of Bank's Customers. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.RightsOfBanksCustomers,NewsInfoDataImagePath=""} },
//             { "dealing dishonour of cheques",new NewsInfoData{NewsInfoDataTitle="Dealing Dishonour of Cheques",NewsInfoDataText="To know more about Dealing Dishonour of Cheques. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.DealingDishonourOfCheques,NewsInfoDataImagePath=""} },
//             { "deposit policy",new NewsInfoData{NewsInfoDataTitle="Deposit Policy",NewsInfoDataText="To know more about Deposit Policy. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.DepositPolicy,NewsInfoDataImagePath=""} },
//             { "best practices code of the bank",new NewsInfoData{NewsInfoDataTitle="Best Practices Code of the Bank",NewsInfoDataText="To know more about Best Practices Code of the Bank. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.BestPracticesCodeOfTheBank,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "BestPracticesCodeOfTheBank.jpg")} },
//             { "banks commitment to customers",new NewsInfoData{NewsInfoDataTitle="BCSBI-Codes of Bank's Commitment to Customers",NewsInfoDataText="To know more about BCSBI-Codes of Bank's Commitment to Customers. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.CodesOfBanksCommitmentToCustomers,NewsInfoDataImagePath=""} },
//             { "detremining marital subsidiary",new NewsInfoData{NewsInfoDataTitle="Policy On Determining Material Subsidiary",NewsInfoDataText="To know more about Policy On Determining Material Subsidiary. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.PolicyOnDeterminingMaterialSubsidiary,NewsInfoDataImagePath=""} },
//             { "disclosure of marital events",new NewsInfoData{NewsInfoDataTitle="Policy On Determination And Disclosure of Material Events/Information",NewsInfoDataText="To know more about Policy On Determination And Disclosure of Material Events/Information. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.PolicyOnDeterminationAndDisclosureOfMaterialEvents,NewsInfoDataImagePath=""} },
//             { "related party transactions",new NewsInfoData{NewsInfoDataTitle="Policy On Realted Party Transactions",NewsInfoDataText="To know more about Policy On Realted Party Transactions. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.PolicyOnRealtedPartyTransactions,NewsInfoDataImagePath=""}},
//             { "guidelines on empanelment of valuers",new NewsInfoData{NewsInfoDataTitle="Policy-Guidelines On Empanelment Of Valuers",NewsInfoDataText="To know more about Policy-Guidelines On Empanelment Of Valuers. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.PolicyGuidelinesOnEmpanelmentOfValuers,NewsInfoDataImagePath=""} },
//             { "statutory central",new NewsInfoData{NewsInfoDataTitle="Policy-Appointment of Statutory Central/Branch Auditors",NewsInfoDataText="To know more about Policy-Appointment of Statutory Central/Branch Auditors. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.PolicyAppointmentOfStatutoryCentral,NewsInfoDataImagePath=""} },
//             { "right to information act",new NewsInfoData{NewsInfoDataTitle="Right To Information Act-2005",NewsInfoDataText="To know more about Right To Information Act-2005. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.RightToInformationAct,NewsInfoDataImagePath=""} },
//             { "disclosures",new NewsInfoData{NewsInfoDataTitle="Base I-II Disclosures",NewsInfoDataText="To know more about Base I-II Disclosures. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.Disclosures,NewsInfoDataImagePath=""} },
//             { "customer centric services",new NewsInfoData{NewsInfoDataTitle="Customer Centric Services",NewsInfoDataText="To know more about Customer Centric Services. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.CustomerCentricServices,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "CustomerCentricServices.jpg")} },
//             { "debt restructing mechanism",new NewsInfoData{NewsInfoDataTitle="Debt Restructing Mechanism For SMES",NewsInfoDataText="To know more about Debt Restructing Mechanism For SMES. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.DebtRestructingMechanismForSMES,NewsInfoDataImagePath=""} },
//             { "fair lending practices",new NewsInfoData{NewsInfoDataTitle="Fair Lending Practices Code",NewsInfoDataText="To know more about Fair Lending Practices Code. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.FairLendingPracticesCode,NewsInfoDataImagePath=""} },
//             { "processing fees",new NewsInfoData{NewsInfoDataTitle="Processing Fees/Charges For Loan Products",NewsInfoDataText="To know more about Processing Fees/Charges For Loan Products. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.ProcessingFees,NewsInfoDataImagePath=""} },
//             { "agri term loans",new NewsInfoData{NewsInfoDataTitle="Processing Charges In Agri Term Loans, JL(Ag), SHG Loan & JL (NP)",NewsInfoDataText="To know more about Processing Charges In Agri Term Loans, JL(Ag), SHG Loan & JL (NP). Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.ProcessingChargesInAgriTermLoans,NewsInfoDataImagePath=""} },
//             { "charges of home plot vehicle",new NewsInfoData{NewsInfoDataTitle="Processing Charges of Home/ Plot/ Vehicle etc products",NewsInfoDataText="To know more about Processing Charges of Home/ Plot/ Vehicle etc products. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.ProcessingChargesofHome,NewsInfoDataImagePath=""} },
//             { "charges of sme products",new NewsInfoData{NewsInfoDataTitle="Processing Charges of SME Products",NewsInfoDataText="To know more about Processing Charges of SME Products. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.ProcessingChargesOfSME_Products,NewsInfoDataImagePath=""} },
//             { "banks commitment msc hindi",new NewsInfoData{NewsInfoDataTitle="Codes of Bank's Commitment to Customers/ MSE in Hindi",NewsInfoDataText="To know more about Codes of Bank's Commitment to Customers/ MSE in Hindi. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.CodesOfBanksCommitmentToCustomersHindi,NewsInfoDataImagePath=""}},
//             { "banks commitment msc",new NewsInfoData{NewsInfoDataTitle="Codes of Bank's Commitment to Customers/ MSE",NewsInfoDataText="To know more about Codes of Bank's Commitment to Customers/ MSE. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.CodesOfBanksCommitmentToCustomersMSE,NewsInfoDataImagePath=""} },

//             //charters/schemes
//             { "agricultural debt waiver",new NewsInfoData{NewsInfoDataTitle="Agricultural Debt Waiver And Debt Relief Scheme, 2008",NewsInfoDataText="To know more about Agricultural Debt Waiver And Debt Relief Scheme, 2008. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.AgriculturalDebtWaiver,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "AgriculturalDebtWaiver.jpg")} },
//             { "charters banking ombudsman",new NewsInfoData{NewsInfoDataTitle="Banking Ombudsman",NewsInfoDataText="To know more about Banking Ombudsman. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.ChartersBankingOmbudsman,NewsInfoDataImagePath=""} },
//             { "financial inclusion plan",new NewsInfoData{NewsInfoDataTitle="Financial Inclusion Plan 1020-12 Name of Villages and Field BCs",NewsInfoDataText="To know more about Financial Inclusion Plan 1020-12 Name of Villages and Field BCs. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.FinancialInclusionPlan,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "FinancialInclusionPlan.jpg")} },
//             { "restructured accounts",new NewsInfoData{NewsInfoDataTitle="RESTRUCTURED ACCOUNTS",NewsInfoDataText="To know more about RESTRUCTURED ACCOUNTS. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.RestructuredAccounts,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "RestructuredAccounts.jpg")} },
//             { "services rendered free of charge",new NewsInfoData{NewsInfoDataTitle="Services Rendered Free of Charge",NewsInfoDataText="To know more about Services Rendered Free of Charge. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.ServicesRenderedFreeOfCharge,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "ServicesRenderedFreeOfCharge.jpg")} },
//             { "welfare of minorities",new NewsInfoData{NewsInfoDataTitle="Welfare of Minorities",NewsInfoDataText="To know more about Welfare of Minorities. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.WelfareOfMinorities,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "WelfareOfMinorities.jpg")} },
//             { "whistle blower policy",new NewsInfoData{NewsInfoDataTitle="Whistle Blower Policy",NewsInfoDataText="To know more about Whistle Blower Policy. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.WhistleBlowerPolicy,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "WhistleBlowerPolicy.jpg")} },
//             { "centralized pension processing",new NewsInfoData{NewsInfoDataTitle="Centralized Pension Processing System",NewsInfoDataText="To know more about Centralized Pension Processing System. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.CentralizedPensionProcessingSystem,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "CentralizedPensionProcessingSystem.jpg")} },
//             { "another option for pension",new NewsInfoData{NewsInfoDataTitle="Another Option For Pension",NewsInfoDataText="To know more about Another Option For Pension. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.AnotherOptionForPension,NewsInfoDataImagePath=Path.Combine(".","Resources\\news_info", "AnotherOptionForPension.jpg")} },
//             { "citizens charter",new NewsInfoData{NewsInfoDataTitle="CITIZENS CHARTER",NewsInfoDataText="To know more about CITIZENS CHARTER. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.CitizensCharter,NewsInfoDataImagePath=""} },
//             { "indian bank mutual fund",new NewsInfoData{NewsInfoDataTitle="INDIAN BANK MUTUAL FUND",NewsInfoDataText="To know more about INDIAN BANK MUTUAL FUND. Please click on Read More",NewsInfoDataLink=NewsInfoResponseLinks.UjjivanBankMutualFund,NewsInfoDataImagePath=""} }
//         };

//         #endregion

//         #region Constructor

//         /// <summary>
//         /// Initializes a new instance of the <see cref="MainResponses"/> class.
//         /// </summary>
//         public NewsInfoResponses()
//         {
//             Register(new DictionaryRenderer(_responseTemplates));
//         }

//         #endregion

//         #region Methods

//         private static object BuildNewsInfoCard(ITurnContext context, NewsInfoData data)
//         {
//             var attachment = new HeroCard()
//             {
//                 Title = data.NewsInfoDataTitle,
//                 Text = data.NewsInfoDataText,
//                 Images = new List<CardImage> { new CardImage(data.NewsInfoDataImagePath) },
//                 Buttons = new List<CardAction>()
//                 {
//                    new CardAction(type: ActionTypes.OpenUrl, title: "Read More", value: data.NewsInfoDataLink)
//                 }
//             }.ToAttachment();

//             var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
//             return response;
//         }

//         /// <summary>
//         /// Code to display hero card for NewsOrInfoMenu
//         /// </summary>
//         /// <param name="turnContext"></param>
//         /// <returns></returns>
//         public static IMessageActivity BuildNewsOrInfoMenuCard(ITurnContext turnContext)
//         {
//             var attachment = new HeroCard()
//             {
//                 Buttons = SharedResponses.SuggestedActionsForNewsOrInfoMenu.Actions
//             }.ToAttachment();

//             var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
//             return response;
//         }

//         /// <summary>
//         /// Code to display hero card for CustomerCornerMenu
//         /// </summary>
//         /// <param name="turnContext"></param>
//         /// <returns></returns>
//         public static IMessageActivity BuildCustomerCornerMenuCard(ITurnContext turnContext)
//         {
//             var attachment = new HeroCard()
//             {
//                 Buttons = SharedResponses.SuggestedActionsForCustomerCornerMenu.Actions
//             }.ToAttachment();

//             var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
//             return response;
//         }

//         /// <summary>
//         /// Code to display hero card for RelatedInfoMenu
//         /// </summary>
//         /// <param name="turnContext"></param>
//         /// <returns></returns>
//         public static IMessageActivity BuildRelatedInfoMenuCard(ITurnContext turnContext)
//         {
//             var attachment = new HeroCard()
//             {
//                 Buttons = SharedResponses.SuggestedActionsForRelatedInfoMenu.Actions
//             }.ToAttachment();

//             var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
//             return response;
//         }

//         /// <summary>
//         /// Code to display hero card for CodesPolicyDisclosuresMenu
//         /// </summary>
//         /// <param name="turnContext"></param>
//         /// <returns></returns>
//         public static IMessageActivity BuildCodesPolicyDisclosuresMenuCard(ITurnContext turnContext)
//         {
//             var attachment = new HeroCard()
//             {
//                 Buttons = SharedResponses.SuggestedActionsForCodesPolicyDisclosuresMenu.Actions
//             }.ToAttachment();

//             var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
//             return response;
//         }

//         /// <summary>
//         /// Code to display hero card for ChartersSchemesMenu
//         /// </summary>
//         /// <param name="turnContext"></param>
//         /// <returns></returns>
//         public static IMessageActivity BuildChartersSchemesMenuCard(ITurnContext turnContext)
//         {
//             var attachment = new HeroCard()
//             {
//                 Buttons = SharedResponses.SuggestedActionsForChartersSchemesMenu.Actions
//             }.ToAttachment();

//             var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
//             return response;
//         }

//         /// <summary>
//         /// Gets the news information data.
//         /// </summary>
//         /// <param name="result">The result.</param>
//         /// <returns></returns>
//         public static NewsInfoData getNewsInfoData(string result)
//         {
//             NewsInfoData newsInfoData = new NewsInfoData();
//             var res = keyValuePairs.TryGetValue(result, out newsInfoData);
//             return newsInfoData;
//         }

//         #endregion

//         #region ResponseIds Class

//         public class ResponseIds
//         {
//             // news info ResponseIds constants
//             public const string NewsOrInfoMenuCardDisplay = "newsOrInfoMenuCardDisplay";
//             public const string CustomerCornerMenuCardDisplay = "customerCornerMenuCardDisplay";
//             public const string RelatedInfoMenuCardDisplay = "relatedInfoMenuCardDisplay";
//             public const string CodesPolicyDisclosuresMenuCardDisplay = "codesPolicyDisclosuresMenuCardDisplay";
//             public const string ChartersSchemesMenuCardDisplay = "chartersSchemesMenuCardDisplay";

//             //last submenu display
//             public const string BuildNewsInfoCard = "BuildNewsInfoCard";
//         }

//         #endregion

//         #region NewsInfoResponseLinks Class

//         public class NewsInfoResponseLinks
//         {
//             // news info Links
//             public const string Notification = "https://www.ujjivanbank.in/departments/notifications/#!";
//             public const string NewsLetter = "https://www.ujjivanbank.in/departments/newsletter/#!";
//             public const string WhatsNew = "https://www.ujjivanbank.in/what-is-new/#!";
//             public const string SmsBanking = "https://www.ujjivanbank.in/sms-banking/#!";
//             public const string ScanAndPay = "https://www.ujjivanbank.in/wp-content/uploads/2018/04/QR_UserGuidepdf.pdf";
//             public const string MyDesignCard = "https://apps.ujjivanbank.in/imagecard/";
//             public const string PressReleases = "https://www.ujjivanbank.in/press-releases/#!";
//             public const string Downloads = "https://www.ujjivanbank.in/departments/downloads/#!";

//             //customer corner links
//             public const string CustomerComplaintForm = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/customercomplaintform.pdf";
//             public const string OnlineCustomerComplaints = "https://www.ujjivanbank.in/departments/online-customer-complaints/#!";
//             public const string NodalOfficersBankingOmbudsmanScheme = "https://www.ujjivanbank.in/nodal-officers-banking-ombudsman-scheme-2016/#!";
//             public const string NodalOfficersCustomerService = "https://www.ujjivanbank.in/nodal-officers-customer-service/#!";
//             public const string PrincipalCodeComplianceOfficer = "https://www.ujjivanbank.in/principal-code-compliance-officers-bcsbi/#!";
//             public const string DamodaranCommitteeRecommendations = "https://www.ujjivanbank.in/damodaran-committee-recommendations/#!";
//             public const string BankingOmbudsman = "https://www.ujjivanbank.in/banking-ombudsman/#!";
//             public const string RemitToIndia = "https://www.ujjivanbank.in/remit-of-india/#!";
//             public const string AadhaarEnrollment = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/AADHAR_enrol_eng.pdf";
//             public const string ProcedureOnLocker = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/Procedure_safe_dep_locker-1.pdf";
//             public const string CoinVendingMachines = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/CVMs_Chennai.pdf";
//             public const string UjjivanBankTrustForRuralDevelopment = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/IBTRD_30082014.pdf";
//             public const string LowCostMobileBankingThroughUSSD = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/LowCostMobileBanking_USSD.pdf";

//             //Related Info links
//             public const string FAQs = "https://www.ujjivanbank.in/departments/f-a-qs/#!";
//             public const string OnlineCustomerComplaintsRelatedInfo = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/PMJDY_FAQ_2014-2.pdf";
//             public const string FAQsOnPradhanMantriJanDhanYojana = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/PMJDY_FAQ_2014-2.pdf";
//             public const string RecoveryAgentsEmpanelledEngagedByBand = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/RecoveryAgent_IB.pdf";
//             public const string ECS_NoticeToCustomers = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/ECSNoticetoCustomers.pdf";
//             public const string ListOfHolidays = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/ECSNoticetoCustomers.pdf";
//             public const string Disclaimer = "https://www.ujjivanbank.in/departments/disclaimer/#!";
//             public const string SecurityAlert = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/security_alert.pdf";

//             //codes/policy/disclosures links
//             public const string RightsOfBanksCustomers = "http://bcsbi.org.in/pdf/PictorialBookEnglish.pdf";
//             public const string DealingDishonourOfCheques = "https://www.ujjivanbank.in/wp-content/uploads/2018/11/Policy-on-Dealing-with-frequent-dishonour-of-Cheques-and-ECS-2018.19.pdf";
//             public const string DepositPolicy = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/policy_Deposit.pdf";
//             public const string BestPracticesCodeOfTheBank = "https://www.ujjivanbank.in/departments/best-practices-code-of-the-bank/#!";
//             public const string CodesOfBanksCommitmentToCustomers = "https://www.ujjivanbank.in/wp-content/uploads/2018/09/Code-of-Banks-Commitment-to-Customers-2018.pdf";
//             public const string PolicyOnDeterminingMaterialSubsidiary = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/Policyondeterminingmaterialsubsidiary.pdf";
//             public const string PolicyOnDeterminationAndDisclosureOfMaterialEvents = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/PolicyonDeterminingMaterialityofEvents.pdf";
//             public const string PolicyOnRealtedPartyTransactions = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/RPTPOLICY2015.pdf";
//             public const string PolicyGuidelinesOnEmpanelmentOfValuers = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/BankPolicyGuidelinesonEmpanelmentofValuers.pdf";
//             public const string PolicyAppointmentOfStatutoryCentral = "https://www.ujjivanbank.in/wp-content/uploads/2018/04/190114072416_0001.pdf";
//             public const string RightToInformationAct = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/drs-sme.pdf";
//             public const string Disclosures = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/drs-sme.pdf";
//             public const string CustomerCentricServices = "https://www.ujjivanbank.in/departments/customer-centric-services/#!";
//             public const string DebtRestructingMechanismForSMES = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/drs-sme-1.pdf";
//             public const string FairLendingPracticesCode = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/FairLendingPracticesCode.pdf";
//             public const string ProcessingFees = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/FairLendingPracticesCode.pdf";//page not found
//             public const string ProcessingChargesInAgriTermLoans = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/ChargesinAgriTermLoansJLSHGloanJL.pdf";
//             public const string ProcessingChargesofHome = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/FairlendingPractice_HomePlotVehcileLoan.pdf";
//             public const string ProcessingChargesOfSME_Products = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/FairLendingPractises_MSME.pdf";
//             public const string CodesOfBanksCommitmentToCustomersHindi = "http://www.bcsbi.org.in/Codes_RegionalLanguage.html";
//             public const string CodesOfBanksCommitmentToCustomersMSE = "http://www.bcsbi.org.in/Codes_RegionalLanguage.html";

//             //charters/schemes links
//             public const string AgriculturalDebtWaiver = "https://www.ujjivanbank.in/departments/agricultural-debt-waiver-and-debt-relief-scheme-2008/#!";
//             public const string ChartersBankingOmbudsman = "https://www.ujjivanbank.in/wp-content/uploads/2018/04/BankingOmbudsman2006.pdf";
//             public const string FinancialInclusionPlan = "https://www.ujjivanbank.in/departments/financial-inclusion-plan-2010-12-name-of-villages-and-field-bcs/#!";
//             public const string RestructuredAccounts = "https://www.ujjivanbank.in/departments/restructured-accounts/#!";
//             public const string ServicesRenderedFreeOfCharge = "https://www.ujjivanbank.in/departments/services-rendered-free-of-charge/#!";
//             public const string WelfareOfMinorities = "https://www.ujjivanbank.in/departments/welfare-of-minorities/#!";
//             public const string WhistleBlowerPolicy = "https://www.ujjivanbank.in/departments/whistle-blower-policy/#!";
//             public const string CentralizedPensionProcessingSystem = "https://www.ujjivanbank.in/departments/centralized-pension-processing-systems/#!";
//             public const string AnotherOptionForPension = "https://www.ujjivanbank.in/departments/another-option-for-pension/#!";
//             public const string CitizensCharter = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/citizen_charter-1.pdf";
//             public const string UjjivanBankMutualFund = "https://www.ujjivanbank.in/wp-content/uploads/2018/03/ibmf.pdf";
//         }

//         #endregion

//         #region NewsInfoData Class

//         public class NewsInfoData
//         {
//             public string NewsInfoDataTitle { get; set; }
//             public string NewsInfoDataText { get; set; }
//             public string NewsInfoDataLink { get; set; }
//             public string NewsInfoDataImagePath { get; set; }
//         }

//         #endregion
//     }
// }
