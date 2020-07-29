#region Intent Class

public class Intent
{
    //intents
    public const string Greet = "greet";
    public const string Loans = "loans";
    public const string Deposits = "deposits";
    public const string SavingsBank = "SavingsBank";
    public const string News_info = "news_info";
    public const string Contacts = "contacts";
    // public const string EMI = "emi";
    public const string Rates = "rates";
    public const string Services = "services";

    public const string EMI_Calculator = "emi_calculator";
    //vehcile loan
    public const string VehicleLoan = "vehicle_loan";
    public const string Help = "help";
    public const string Cancel = "cancel";
    public const string None = "none";
    public const string Escalate = "escalate";
}

#endregion

#region OnboardingEntities
public class ScrollbarEntities
{
    //intents
    public const string AboutUs = "about us";
    public const string Products = "product";
    public const string Services = "services";
    public const string Rates = "rates";
    public const string Contacts = "contacts";
    public const string Links = "links";

}

public class AboutUsEntities
{
    public const string Profile = "profiles";
    public const string VisionMission = "vision and mission";
    public const string Management = "management";
    public const string FinanceResult = "finance result";
    public const string CorporateGovernance = "corporate governance";
    public const string MutualFund = "mutual fund";
    public const string AnnualReport = "annual report";
}

public class ProductFAQEntities
{
    public const string LoanProducts = "loan products";
    public const string DepositProducts = "deposit products";
    public const string DigitalProducts = "digital products";
    public const string FeatureProducts = "feature products";
    public const string Schemes = "schemes";
}

public class ServicesFAQEntities
{
    public const string PremiumServices = "premium services";
    public const string InsuranceServices = "insurance services";
    public const string CMSPlus = "cms plus";
    public const string DoorstepBanking = "doorstep banking";
    public const string TaxPayment = "tax payment";
    public const string DebentureTrust = "debenture trust";
}

public class RatesFAQEntities
{
    public const string DepositRates = "deposit rates";
    public const string LendingRates = "lending rates";
    public const string ServiceCharges = "service charges";
    public const string ForexRates = "forex rates";
    public const string ForexCharges = "forex charges";

}

public class ContactsFAQEntities
{
    public const string CustomerSupport = "customer support";
    public const string EmailID = "email i d";
}

public class LinksFAQEntities
{
    public const string CreditCards = "credit cards";
    public const string OnlineServices = "online services";
    public const string OnlinePortal = "online portal";
    public const string RelatedSites = "related sites";
    public const string Payments = "payments";
    public const string Alliances = "alliances";
    public const string Challans = "challans";
}

#endregion

#region DepositEntities Class

public class DepositEntities
{
    //deposit entities constants
    public const string SavingsBankAccount = "savings bank account";
    public const string CurrentAccountTypes = "current account types";
    public const string TermDeposits = "term deposits";
    public const string NriAccounts = "nri accounts";

    //savings bank account entities constants
    public const string SavingsBank = "savings bank";
    public const string IbCorpSbPayrollPackage = "ib corp sb payroll package";
    public const string VikasSavingsKhata = "vikas savings khata";
    public const string IbSmartKid = "ib smart kid";
    public const string SavingsTermsAndConditions = "savings terms and conditions";
    public const string SbPlatinum = "sb platinum";
    public const string IbSurabhi = "ib surabhi";

    // current A/C entities constants
    public const string CurrentAccount = "current account";
    public const string FreedomCurrentAccount = "freedom current account";
    public const string CurrentTermsAndConditions = "current terms and conditions";
    public const string PremiumCurrentAccount = "premium current account";

    //term deposit entities constants
    public const string FacilityDeposit = "facility deposit";
    public const string CapitalGains = "capital gains";
    public const string TermConditionsOfTermDeposit = "term deposit terms and conditions";
    public const string DepositSchemeForSeniorCitizens = "deposit scheme for senior citizens";
    public const string RecurringDeposit = "recurring deposit";
    public const string IbTaxSaverScheme = "ib tax saver scheme";
    public const string InsuredRecurringDeposit = "insured recurring deposit";
    public const string ReInvestmentPlan = "reinvestment plan";
    public const string FixedDeposit = "fixed deposit";
    public const string VariableRecurringDeposit = "variable recurring deposit";

    //nri accounts entities constants
    public const string ForeignCurrencyForReturningIndians = "foreign currency for returning indians";
    public const string FdRipRdAccounts = "fd rip rd accounts";
    public const string NreSbAccounts = "nre sb accounts";
    public const string NonResidentOrdinaryAccount = "non resident ordinary account";
    public const string FcnrAccounts = "fcnr accounts";
}

#endregion

#region NewsInfoEntities Class
public class NewsInfoEntities
{
    //news info entities constants
    public const string Notifications = "notifications";
    public const string NewsLetter = "news letter";
    public const string New = "new";
    public const string SMS_Banking = "sms banking";
    public const string ScanAndPay = "scan and pay";
    public const string MyDesignCard = "my design card";
    public const string PressReleases = "press releases";
    public const string CustomerCorner = "customer corner";//has sub menus
    public const string RelatedInfo = "related info";//has sub menus
    public const string Downloads = "downloads";
    public const string Codes_policy_disclosures = "codes_policy_disclosures";//has sub menus
    public const string ChartersSchemes = "charters_schemes";//has sub menus

    //customer corner entities constants
    public const string CustomerComplaintForm = "customer complaint form";
    public const string OnlineCustomerComplaints = "online customer complaints";
    public const string BankingOmbudsmanScheme = "banking ombudsman scheme";
    public const string CustomerService = "customer service";
    public const string PrincipalCodeComplianceOfficer = "principal code compliance officer";
    public const string DamodaranCommitteeRecommendations = "damodaran committee recommendations";
    public const string BankingOmbudsman = "banking ombudsman";
    public const string RemitToIndia = "remit to india";
    public const string AadhaarEnrolmentCorrectionForm = "aadhar enrolment correction form";
    public const string ProcedureOnLocker_SafeDeposit = "procedure on locker";
    public const string CoinVendingMachines = "coin vending machines";
    public const string UjjivanBankTrustRuralDevelopment = "indian bank trust rural development";
    public const string MobileBankingThroughUSSD = "mobile banking through ussd";

    //related info entities constants
    public const string FAQs = "faq";
    public const string PradhanMantri = "pradhan mantri jan dhan yojana";
    public const string RecoverAgentsEmpanelled_EngagedByBank = "recovery agents empanelled";
    public const string ECS_NoticeToCustomers = "ecs notice to customers";
    public const string ListOfHolidays = "list of holidays";
    public const string Disclaimer = "disclaimer";
    public const string SecurityAlert = "security alert";

    //codes/policy/disclosure entities constants
    public const string RightsOfBankCustomers = "rights of bank customers";
    public const string DealingDishonourOfCheques = "dealing dishonour of cheques";
    public const string DepositPolicy = "deposit policy";
    public const string BestPracticesCodeOfTheBank = "best practices code of the bank";
    public const string BanksCommitmentToCustomers = "banks commitment to customers";
    public const string DeterminingMaritalSubsidiary = "detremining marital subsidiary";
    public const string DisclosureOfMaritalEvents = "disclosure of marital events";
    public const string RelatedPartyTransactions = "related party transactions";
    public const string GuidelinesOnEmpanelmentOfValuers = "guidelines on empanelment of valuers";
    public const string AppointmentOfStatutoryCentral = "statutory central";
    public const string RightToInformationAct = "right to information act";
    public const string Disclosures = "disclosures";
    public const string CustomerCentricServices = "customer centric services";
    public const string DebtRestructingMechanism = "debt restructing mechanism";
    public const string FairLendingPractices = "fair lending practices";
    public const string ProcessingFees = "processing fees";
    public const string AgriTermLoans = "agri term loans";
    public const string ChargesOfHomePlotVehicle = "charges of home plot vehicle";
    public const string ChargesOfSME_Products = "charges of sme products";
    public const string BanksCommitmentMSC_Hindi = "banks commitment msc hindi";
    public const string BanksCommitmentMSC = "banks commitment msc";

    //charters/schemes entities constants
    public const string AgriculturalDebtWaiver = "agricultural debt waiver";
    public const string ChartersBankingOmbudsman = "charters banking ombudsman";
    public const string FinancialInclusionPlan = "financial inclusion plan";
    public const string RestructuredAccounts = "restructured accounts";
    public const string ServicesRenderedFreeOfCharge = "services rendered free of charge";
    public const string WelfareOfMinorities = "welfare of minorities";
    public const string WhistleBlowerPolicy = "whistle blower policy";
    public const string CentralizedPensionProcessing = "centralized pension processing";
    public const string AnotherOptionForPension = "another option for pension";
    public const string CitizensCharter = "citizens charter";
    public const string UjjivanBankMutualFund = "indian bank mutual fund";
}

#endregion

#region LoanEntities Class
public class LoanEntities
{
    //loan constants
    public const string Agriculture = "agriculture";
    public const string Groups = "groups";
    public const string PersonalIndividual = "personal individual";

    //Agriculture Entities
    public const string AgriculturalGodowns = "agricultural godowns";
    public const string LoansForMaintenanceOfTractors = "loans for maintenance of tractors";
    public const string AgriculturalProduceMarketingLoan = "agricultural produce marketing loan";
    public const string FinancingAgriculturistsForPurchaseOfTractors = "financing agriculturists for purchase of tractors";
    public const string PurchaseSecondHandTractors = "purchase second hand tractors";
    public const string AgriClinicAgriBusinessCentres = "agri clinic agri business centres";
    public const string SHG_BankLinkageProgramme = "shg bank linkage programme";
    public const string JointLiabilityGroup = "joint liability group";
    public const string RupayKisanCard = "rupay kisan card";
    public const string DRI_SchemeRevisedNorms = "dri scheme revised norms";
    public const string SHG_VidhyaShoba = "shg vidhya shoba";
    public const string GraminMahilaSowbhagyaScheme = "gramin mahila sowbhagya scheme";
    public const string SugarPremiumScheme = "sugar premium scheme";
    public const string GoldenHarvestScheme = "golden harvest scheme";
    public const string AgriculturalJewelLoanScheme = "agricultural jewel loan scheme";

    //groups entities
    public const string GroupsAgriculturalGodowns = "groups agricultural godowns";
    public const string Groups_SGH_BankLinkageProgramme = "groups shg bank linkage programme";
    public const string Groups_SGH_VidhyaShoba = "groups shg vidhya shoba";

    //personal/individual entities
    public const string IbHomeLoanCombo = "Ib home loan combo";
    public const string IbRentEncash = "ib rent encash";
    public const string LoanOD_AgainstDeposits = "loan od against deposits";
    public const string IbCleanLoan = "ib clean loan";
    public const string IbBalavidhyaScheme = "ib balavidhya scheme";
    public const string IndReverseMortgage = "ind reverse mortgage";
    public const string IbVehicleloan = "ib vehicle loan";
    public const string IndMortgage = "ind mortgage";
    public const string PlotLoan = "plot loan";
    public const string IbHomeLoan = "ib home loan";
    public const string IbPensionLoan = "ib pension loan";
    public const string HomeImprove = "home improve";
    public const string IbHomeLoanPlus = "ib home loan plus";
    public const string LoanOD_AgainstNSC = "loan od against nsc";
}

#endregion

#region RateEntities
public class RateEntities
{
    public const string DepositRates = "deposit rates";
    public const string LendingRates = "lending rates";
    public const string ServiceCharges = "service charges / forex rates";
}

#endregion

#region ServicesEntities
public class ServicesEntities
{
    // Services Constants
    public const string PremiumServices = "premium services";
    public const string InsuranceServices = "insurance services";
    public const string CMSPlus = "cms plus";
    public const string EpaymentofDirectTaxes = "direct taxes";
    public const string EpaymentofIndirectTaxes = "indirect taxes";

    // Premium Services Constants
    public const string MCAPayment = "mca payment";
    public const string MoneyGram = "money gram";
    public const string ATMDebitCard = "atm debit card";
    public const string IndMobileBanking = "ind mobile banking";
    public const string IndNetBanking = "ind net banking";
    public const string CreditCards = "credit card";
    public const string XpressMoney = "xpress money";
    public const string NEFT = "neft";
    public const string IndJetRemit = "ind jet remit";
    public const string MulticityChequeFacility = "multicity cheque facility";

    // Insurance Services Constants
    public const string IBVidyarthiSuraksha = "ib vidyarthi suraksha";
    public const string IBHomeSecurity = "ib home security";
    public const string UniversalHealthCare = "universal health care";
    public const string JanaShreeBimaYojana = "jana shree bima yojana";
    public const string NewIBJeevanVidya = "new ib javan vidya";
    public const string IBJeevanKalyan = "ib jeevan kalyan";
    public const string IBVarishtha = "ib varishtha";
    public const string ArogyaRaksha = "arogya raksha";
    public const string IBChhatra = "ib chhatra";
    public const string IBGrihaJeevan = "ib griha jeevan";
    public const string IBYatraSuraksha = "ib yatra suraksha";
}

#endregion

#region ContactEntities Class

public class ContactEntities
{

    //Contacts Constants
    public const string QuickContacts = "quick contact";
    public const string CustomerSupport = "customer support";
    public const string EmailIDs = "email";

    //Customer Support Constants
    public const string CustomerComplaints = "customer complaints";
    public const string ComplaintsOfficers = "complaints officers";
    public const string ChiefVigilanceOfficer = "chief vigilance officer";

    //Email IDs Constants
    public const string HeadOffice = "head office";
    public const string Department = "department";
    public const string Executives = "executives";
    public const string IMAGE = "image";
    public const string ForeginBranches = "foregin branches";
    public const string OverseasBranches = "overseas branches";
    public const string NRIBranches = "nri branches";
    public const string ZonalOffices = "zonal offices";
    public const string EConformationOfBankGuarantee = "conformation of bank guarantee";
}

public class VehicleLoanEntities
{
    //vehicle loan
    public const string VehicleLoan = "vehicle loan";
}
public class LoansEntities
{
    // Loans Constants
    public const string Agriculture = "agriculture";
    public const string Groups = "groups";
    public const string PersonalOrIndividual = "personal individual";
    public const string MSME = "msme";
    public const string Education = "education";
    public const string NRI = "nri";
    public const string FNMinutesLoan = "59 minutes loans";

    //MSME Constants
    public const string IBVidhyaMandir = "ib vidhya mandir";
    public const string IBMyOwnShop = "ib my own shop";
    public const string IBDoctorPlus = "ib doctor plus";
    public const string IBContractors = "ib contractors";
    public const string Tradewell = "tradewell";
    public const string IndSMESecure = "ind sme secure";

    //Education Loan Constants

    public const string ModelEducationalLoan = "model education loan scheme";
    public const string IBEducationalLoanPrime = "educational loan prime";
    public const string IBSkillLoanScheme = "skill loan scheme";
    public const string EducationLoanInterest = "education loan interest";


    //NRI Loan Constants
    public const string NRIPlotLoan = "nri plot loan";
    public const string NRIHomeLoan = "nri home loan";



}

#endregion