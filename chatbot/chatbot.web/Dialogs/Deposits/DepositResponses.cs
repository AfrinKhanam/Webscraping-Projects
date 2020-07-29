// using System.Collections.Generic;
// using System.IO;

// using UjjivanBank_ChatBOT.Dialogs.Shared;

// using Microsoft.Bot.Builder;
// using Microsoft.Bot.Builder.TemplateManager;
// using Microsoft.Bot.Schema;

// namespace UjjivanBank_ChatBOT.Dialogs.Deposits
// {
//     public class DepositResponses : TemplateManager
//     {
//         #region Properties

//         private static LanguageTemplateDictionary _responseTemplate = new LanguageTemplateDictionary
//         {
//             ["default"] = new TemplateIdMap
//             {
//                 //deposits dictionary object
//                 { ResponseIds.DepositMenuCardDisplay, (context, data) => BuildDepositMenuCard(context) },
//                 { ResponseIds.SavingsBankAccountMenuCardDisplay, (context,data) => SavingsBankAccountMenuCardDisplay(context) },
//                 { ResponseIds.CurrentAccountMenuCardDisplay, (context,data) => BuildCurrentAccountMenuCard(context) },
//                 { ResponseIds.TermDepositMenuCardDisplay, (context,data) => BuildTermDepositMenuCard(context) },
//                 { ResponseIds.NriAccountsMenuCardDisplay, (context,data) => BuildNriAccountsMenuCard(context) },
//                 //last sub menu display
//                 { ResponseIds.DepositsSubMenuCardDisplay, (context,data) => BuildDepositSubMenuCard(context,data) },
//             }
//         };

//         //key-value pair
//         public static Dictionary<string, DepositData> keyValuePairs = new Dictionary<string, DepositData>
//         {
//             //Savings bank account
//             {"savings bank",new DepositData{DepositDataTitle="Savings Bank", DepositDataText="To know more about Savings Bank. Please click on Read More",DepositDataLink=DepositResponseLinks.SavingsBank,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "SavingsBank.PNG")} },
//             {"ib corp sb payroll package",new DepositData{DepositDataTitle="IB CORP SB-Payroll Package Scheme For Salaried Class", DepositDataText="To know more about IB CORP SB-Payroll Package Scheme For Salaried Class. Please click on Read More",DepositDataLink=DepositResponseLinks.IbCorpSbPayrollPackage,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "PayrollPackage.PNG")} },
//             {"vikas savings khata",new DepositData{DepositDataTitle="VIKAS SAVINGS KHATA-A No Frills Savings Bank Account", DepositDataText="To know more about VIKAS SAVINGS KHATA-A No Frills Savings Bank Account. Please click on Read More",DepositDataLink=DepositResponseLinks.VikasSavingsKhata,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "VikasSavingsKhata.PNG")} },
//             {"ib smart kid",new DepositData{DepositDataTitle="IB Smart Kid", DepositDataText="To know more about IB Smart Kid. Please click on Read More",DepositDataLink=DepositResponseLinks.IbSmartKid,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "IbSmartKid.PNG")} },
//             {"savings terms and conditions",new DepositData{DepositDataTitle="IMPORTANT TERMS AND CONDITIONS", DepositDataText="To know more about IMPORTANT TERMS AND CONDITIONS. Please click on Read More",DepositDataLink=DepositResponseLinks.SavingsTermsAndConditions,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "SavingsTermsConditions.PNG")} },
//             {"sb platinum",new DepositData{DepositDataTitle="SB Platinum", DepositDataText="To know more about SB Platinum. Please click on Read More",DepositDataLink=DepositResponseLinks.SbPlatinum,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "SbPlatinum.PNG")} },
//             {"ib surabhi",new DepositData{DepositDataTitle="IB-Surabhi", DepositDataText="To know more about IB-Surabhi. Please click on Read More",DepositDataLink=DepositResponseLinks.IbSurabhi,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "IbSurabhi.PNG")} },

//              //Current Account
//             {"current account",new DepositData{DepositDataTitle="Current Account", DepositDataText="To know more about Current Account. Please click on Read More",DepositDataLink=DepositResponseLinks.CurrentAccount,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "CurrentAccount.PNG")} },
//             {"freedom current account",new DepositData{DepositDataTitle="IB i-Freedom Current Account", DepositDataText="To know more about IB i-Freedom Current Account. Please click on Read More",DepositDataLink=DepositResponseLinks.FreedomCurrentAccount,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "FreedomCurrentAccount.PNG")} },
//             {"current terms and conditions",new DepositData{DepositDataTitle="IMPORTANT TERMS AND CONDITIONS", DepositDataText="To know more about IMPORTANT TERMS AND CONDITIONS. Please click on Read More",DepositDataLink=DepositResponseLinks.CurrentTermsAndConditions,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "CurrentTermsAndConditions.PNG")} },
//             {"premium current account",new DepositData{DepositDataTitle="Premium Current Account", DepositDataText="To know more about Premium Current Account. Please click on Read More",DepositDataLink=DepositResponseLinks.PremiumCurrentAccount,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "PremiumCurrentAccount.PNG")} },

//             //term deposit entities constants
//             {"facility deposit",new DepositData{DepositDataTitle="Facility Deposit", DepositDataText="To know more about Facility Deposit. Please click on Read More",DepositDataLink=DepositResponseLinks.FacilityDeposit,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "FacilityDeposit.PNG")} },
//             {"capital gains",new DepositData{DepositDataTitle="Capital Gains", DepositDataText="To know more about Capital Gains. Please click on Read More",DepositDataLink=DepositResponseLinks.CapitalGains,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "CapitalGains.PNG")} },
//             {"term deposit terms and conditions",new DepositData{DepositDataTitle="TERMS AND CONDITIONS-TERM DEPOSIT ACCOUNT", DepositDataText="To know more about TERMS AND CONDITIONS-TERM DEPOSIT ACCOUNT. Please click on Read More",DepositDataLink=DepositResponseLinks.TermConditionsOfTermDeposit,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "Term-TermsConditions.PNG")}  },
//             {"deposit scheme for senior citizens",new DepositData{DepositDataTitle="Deposit Scheme For Senior Citizens", DepositDataText="To know more about Deposit Scheme For Senior Citizens. Please click on Read More",DepositDataLink=DepositResponseLinks.DepositSchemeForSeniorCitizens,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "DepositSchemeForSeniorCitizens.PNG")}  },

//             {"recurring deposit",new DepositData{DepositDataTitle="Recurring Deposit", DepositDataText="To know more about Recurring Deposit. Please click on Read More",DepositDataLink=DepositResponseLinks.RecurringDeposit,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "RecurringDeposit.PNG")} },
//             {"ib tax saver scheme",new DepositData{DepositDataTitle="IB Tax Saver Scheme", DepositDataText="To know more about IB Tax Saver Scheme. Please click on Read More",DepositDataLink=DepositResponseLinks.IbTaxSaverScheme,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "IbTaxSaverScheme.PNG")}},
//             {"insured recurring deposit",new DepositData{DepositDataTitle="Insured Recurring Deposit", DepositDataText="To know more about Insured Recurring Deposit. Please click on Read More",DepositDataLink=DepositResponseLinks.InsuredRecurringDeposit,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "InsuredRecurringDeposit.PNG")}},
//             {"reinvestment plan",new DepositData{DepositDataTitle="Re-Investment Plan", DepositDataText="To know more about Re-Investment Plan. Please click on Read More",DepositDataLink=DepositResponseLinks.ReInvestmentPlan,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "ReInvestmentPlan.PNG")}},
//             {"fixed deposit",new DepositData{DepositDataTitle="Fixed Deposit", DepositDataText="To know more about Fixed Deposit. Please click on Read More",DepositDataLink=DepositResponseLinks.FixedDeposit,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "FixedDeposit.PNG")}},
//             {"variable recurring deposit",new DepositData{DepositDataTitle="Variable Recurring Deposit", DepositDataText="To know more about Variable Recurring Deposit. Please click on Read More",DepositDataLink=DepositResponseLinks.VariableRecurringDeposit,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "VariableRecurringDeposit.PNG")}},

//             //nri accounts entities constants
//             {"foreign currency for returning indians",new DepositData{DepositDataTitle="Resident Foreign Currency Account For Returning Indians", DepositDataText="To know more about Resident Foreign Currency Account For Returning Indians. Please click on Read More",DepositDataLink=DepositResponseLinks.ForeignCurrencyForReturningIndians,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "ForeignCurrencyForReturningIndians.PNG")}},
//             {"fd rip rd accounts",new DepositData{DepositDataTitle="NRE FD/ RIP/ RD ACCOUNTS", DepositDataText="To know more about NRE FD/ RIP/ RD ACCOUNTS. Please click on Read More",DepositDataLink=DepositResponseLinks.FdRipRdAccounts,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "NRE_FD_RIP_RD_Accounts.PNG")}},
//             {"nre sb accounts",new DepositData{DepositDataTitle="NRE SB ACCOUNTS", DepositDataText="To know more about NRE SB ACCOUNTS. Please click on Read More",DepositDataLink=DepositResponseLinks.NreSbAccounts,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "NRE_SB_Accounts.PNG")}},
//             {"non resident ordinary account",new DepositData{DepositDataTitle="Non-Resident Ordinary Account", DepositDataText="To know more about Non-Resident Ordinary Account. Please click on Read More",DepositDataLink=DepositResponseLinks.NonResidentOrdinaryAccount,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "NonResidentOrdinaryAccount.PNG")}},
//             {"fcnr accounts",new DepositData{DepositDataTitle="FCNR (B) Accounts", DepositDataText="To know more about FCNR (B) Accounts. Please click on Read More",DepositDataLink=DepositResponseLinks.FcnrAccounts,DepositDataImagePath=Path.Combine(".", @"Resources\deposits", "FCNR_Accounts.PNG")}}
//         };

//         #endregion

//         #region Constructor

//         /// <summary>
//         /// Registers _responseTemplate object reference into the dictionary object.
//         /// </summary>
//         public DepositResponses()
//         {
//             Register(new DictionaryRenderer(_responseTemplate));
//         }

//         #endregion

//         #region Methods
//         /// <summary>
//         /// Code to display hero card for deposit menu
//         /// </summary>
//         /// <param name="turnContext"></param>
//         /// <returns></returns>
//         public static IMessageActivity BuildDepositMenuCard(ITurnContext turnContext)
//         {
//             var attachment = new HeroCard()
//             {
//                 Buttons = SharedResponses.SuggestedActionsForDepositMenu.Actions
//             }.ToAttachment();
//             var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);

//             return response;
//         }

//         /// <summary>
//         /// Builds the savings menu card.
//         /// </summary>
//         /// <param name="turnContext">The turn context.</param>
//         /// <returns></returns>
//         public static IMessageActivity SavingsBankAccountMenuCardDisplay(ITurnContext turnContext)
//         {
//             var attachment = new HeroCard()
//             {
//                 Buttons = SharedResponses.SuggestedActionsForSavingsBankAccountMenu.Actions
//             }.ToAttachment();

//             var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
//             return response;
//         }

//         /// <summary>
//         /// Builds the current account menu card.
//         /// </summary>
//         /// <param name="turnContext">The turn context.</param>
//         /// <returns></returns>
//         public static IMessageActivity BuildCurrentAccountMenuCard(ITurnContext turnContext)
//         {
//             var attachment = new HeroCard()
//             {
//                 Buttons = SharedResponses.SuggestedActionsCurrentAccountMenu.Actions
//             }.ToAttachment();

//             var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
//             return response;
//         }

//         /// <summary>
//         /// Builds the term deposit menu card.
//         /// </summary>
//         /// <param name="turnContext">The turn context.</param>
//         /// <returns></returns>
//         public static IMessageActivity BuildTermDepositMenuCard(ITurnContext turnContext)
//         {
//             var attachment = new HeroCard()
//             {
//                 Buttons = SharedResponses.SuggestedActionsTermDepositsMenu.Actions
//             }.ToAttachment();

//             var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
//             return response;
//         }

//         /// <summary>
//         /// Builds the nri accounts menu card.
//         /// </summary>
//         /// <param name="turnContext">The turn context.</param>
//         /// <returns></returns>
//         public static IMessageActivity BuildNriAccountsMenuCard(ITurnContext turnContext)
//         {
//             var attachment = new HeroCard()
//             {
//                 Buttons = SharedResponses.SuggestedActionsNriAccountsMenu.Actions
//             }.ToAttachment();

//             var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
//             return response;
//         }

//         /// <summary>
//         /// Builds the loans card.
//         /// </summary>
//         /// <param name="context">The context.</param>
//         /// <param name="data">The data.</param>
//         /// <returns></returns>
//         private static object BuildDepositSubMenuCard(ITurnContext context, DepositData data)
//         {
//             if (data != null)
//             {
//                 var attachment = new HeroCard()
//                 {
//                     Title = data.DepositDataTitle,
//                     Text = data.DepositDataText,
//                     Images = new List<CardImage> { new CardImage(data.DepositDataImagePath) },
//                     Buttons = new List<CardAction>()
//                     {
//                        new CardAction(type: ActionTypes.OpenUrl, title: "Read More", value: data.DepositDataLink)
//                     }
//                 }.ToAttachment();

//                 var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
//                 return response;
//             }
//             var responseData = "Sorry!!! Couldn't understand...Please try again.";
//             return responseData;
//         }

//         /// <summary>
//         /// Gets the loans data.
//         /// </summary>
//         /// <param name="result">The result.</param>
//         /// <returns></returns>
//         public static DepositData getDepositData(string result)
//         {
//             DepositData depositData = new DepositData();
//             var res = keyValuePairs.TryGetValue(result, out depositData);
//             return depositData;
//         }

//         #endregion

//         #region DepositData Class

//         public class DepositData
//         {
//             public string DepositDataTitle { get; set; }
//             public string DepositDataText { get; set; }
//             public string DepositDataLink { get; set; }
//             public string DepositDataImagePath { get; set; }
//         }

//         #endregion

//         #region DepositResponseLinks Class

//         public class DepositResponseLinks
//         {
//             //savings bank account url constants
//             public const string SavingsBank = "https://www.ujjivanbank.in/departments/savings-bank/#!";
//             public const string IbCorpSbPayrollPackage = "https://www.ujjivanbank.in/departments/ib-corp-sb-payroll-package-scheme-for-salaried-class/#!";
//             public const string VikasSavingsKhata = "https://www.ujjivanbank.in/departments/vikas-savings-khata-a-no-frills-savings-bank-account/#!";
//             public const string IbSmartKid = "https://www.ujjivanbank.in/departments/ib-smart-kid/#!";
//             public const string SavingsTermsAndConditions = "https://www.ujjivanbank.in/departments/important-terms-and-conditions/#!";
//             public const string SbPlatinum = "https://www.ujjivanbank.in/departments/sb-platinum/#!";
//             public const string IbSurabhi = "https://www.ujjivanbank.in/departments/ib-surabhi/#!";

//             // current A/C url constants
//             public const string CurrentAccount = "https://www.ujjivanbank.in/departments/current-account/#!";
//             public const string FreedomCurrentAccount = "https://www.ujjivanbank.in/departments/ib-i-freedom-current-account/#!";
//             public const string CurrentTermsAndConditions = "https://www.ujjivanbank.in/departments/important-terms-and-conditions-2/#!";
//             public const string PremiumCurrentAccount = "https://www.ujjivanbank.in/departments/premium-current-account/#!";

//             //term deposit url constants
//             public const string FacilityDeposit = "https://www.ujjivanbank.in/departments/facility-deposit/#!";
//             public const string CapitalGains = "https://www.ujjivanbank.in/departments/capital-gains/#!";
//             public const string TermConditionsOfTermDeposit = "https://www.ujjivanbank.in/departments/terms-and-conditions-term-deposit-account/#!";
//             public const string DepositSchemeForSeniorCitizens = "https://www.ujjivanbank.in/departments/deposit-scheme-for-senior-citizens/#!";
//             public const string RecurringDeposit = "https://www.ujjivanbank.in/departments/recurring-deposit/#!";
//             public const string IbTaxSaverScheme = "https://www.ujjivanbank.in/departments/ib-tax-saver-scheme/#!";
//             public const string InsuredRecurringDeposit = "https://www.ujjivanbank.in/departments/insured-recurring-deposit/#!";
//             public const string ReInvestmentPlan = "https://www.ujjivanbank.in/departments/re-investment-plan/#!";
//             public const string FixedDeposit = "https://www.ujjivanbank.in/departments/fixed-deposit/#!";
//             public const string VariableRecurringDeposit = "https://www.ujjivanbank.in/departments/variable-recurring-deposit/#!";

//             //nri accounts entities constants
//             public const string ForeignCurrencyForReturningIndians = "https://www.ujjivanbank.in/departments/resident-foreign-currency-account-for-returning-indians/#!";
//             public const string FdRipRdAccounts = "https://www.ujjivanbank.in/departments/nre-fd-rip-rd-accounts/#!";
//             public const string NreSbAccounts = "https://www.ujjivanbank.in/departments/nre-sb-accounts/#!";
//             public const string NonResidentOrdinaryAccount = "https://www.ujjivanbank.in/departments/non-resident-ordinary-account/#!";
//             public const string FcnrAccounts = "https://www.ujjivanbank.in/departments/fcnr-b-accounts/#!";
//         }

//         #endregion

//         #region ResponseIds Class
//         public class RespsonseIds
//         {
//             //deposits sub menu
//             public const string DepositMenuCardDisplay = "depositMenuCardDisplay";
//             public const string SavingsBankAccountMenuCardDisplay = "savingsBankAccountMenuCardDisplay";
//             public const string CurrentAccountMenuCardDisplay = "currentAccountMenuCardDisplay";
//             public const string TermDepositMenuCardDisplay = "termDepositMenuCardDisplay";
//             public const string NriAccountsMenuCardDisplay = "nriAccountsMenuCardDisplay";

//             public const string DepositsSubMenuCardDisplay = "depositsSubMenuCardDisplay";
//         }

//         #endregion

//         #region ResponseIds Class

//         public class ResponseIds
//         {
//             // Constants
//             //deposits sub menu
//             public const string DepositMenuCardDisplay = "depositMenuCardDisplay";
//             public const string SavingsBankAccountMenuCardDisplay = "savingsBankAccountMenuCardDisplay";
//             public const string CurrentAccountMenuCardDisplay = "currentAccountMenuCardDisplay";
//             public const string TermDepositMenuCardDisplay = "termDepositMenuCardDisplay";
//             public const string NriAccountsMenuCardDisplay = "nriAccountsMenuCardDisplay";
//             public const string DepositsSubMenuCardDisplay = "depositsSubMenuCardDisplay";
//         }

//         #endregion
//     }
// }
