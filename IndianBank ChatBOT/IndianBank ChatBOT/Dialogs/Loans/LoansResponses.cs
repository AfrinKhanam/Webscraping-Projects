using IndianBank_ChatBOT.Dialogs.Shared;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.TemplateManager;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Dialogs.Loans
{
    public class LoansResponses : TemplateManager
    {
        #region Properties

        private static LanguageTemplateDictionary _responseTemplates = new LanguageTemplateDictionary
        {
            ["default"] = new TemplateIdMap
            {
                { LoanResponseIds.BuildLoansCard, (context, data) => BuildLoansCard(context, data) },
                { LoanResponseIds.LoansMenuCardDisplay, (context,data) => BuildLoansMenuCard(context) },
                { LoanResponseIds.AgricultureMenuCardDisplay, (context,data) => BuildAgricultureMenuCard(context) },
                { LoanResponseIds.GroupsMenuCardDisplay, (context,data) => BuildGroupsMenuCard(context) },
                { LoanResponseIds.PersonalIndividualCardDisplay, (context,data) => BuildPersonalIndividualMenuCard(context) },
                { LoanResponseIds.MSMELoansMenuCard, (context,data) => MSMELoanMenuCardDisplay(context) },
                { LoanResponseIds.EducationLoansMenuCard, (context,data) => EducationLoanMenuCardDisplay(context) },
                { LoanResponseIds.NRILoansMenuCard, (context,data) => NRILoanMenuCardDisplay(context) }
            }
        };

        //key-value pair
        public static Dictionary<string, LoanData> keyValuePairs = new Dictionary<string, LoanData>
        {
            //agriculture
            {"agricultural godowns",new LoanData{LoansDataTitle="Agricultural Godowns / Cold Storage", LoansDataText="To know more about Agricultural Godowns / Cold Storage. Please click on Read More",LoansDataLink=LoansResponseLinks.AgriculturalGodowns,LoansDataImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "AgriculturalGodowns.PNG")} },
            {"loans for maintenance of tractors",new LoanData{LoansDataTitle="Loans for maintenance of Tractors under tie-up with Sugar Mills", LoansDataText="To know more about Loans for maintenance of Tractors under tie-up with Sugar Mills. Please click on Read More",LoansDataLink=LoansResponseLinks.LoansForMaintenanceOfTractors,LoansDataImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "LoansForMaintainenceOfTractorsUnderTie-UpWithSugarMills.PNG")} },
            {"agricultural produce marketing loan",new LoanData{LoansDataTitle="Agricultural Produce Marketing Loan", LoansDataText="To know more about Agricultural Produce Marketing loan. Please click on Read More",LoansDataLink=LoansResponseLinks.AgriculturalProduceMarketingLoan,LoansDataImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "AgriculturalProduceMarketingLoan.PNG")} },
            {"financing agriculturists for purchase of tractors",new LoanData{LoansDataTitle="Financing Agriculturists for Purchase of Tractors", LoansDataText="To know more about Financing Agriculturists for Purchase of Tractors. Please click on Read More",LoansDataLink=LoansResponseLinks.FinancingAgriculturistsForPurchaseOfTractors,LoansDataImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "FinancingAgriculturistsForPurchaseOfTractors.PNG")} },
            {"purchase second hand tractors",new LoanData{LoansDataTitle="Purchase of second hand (pre-used) Tractors by Agriculturists", LoansDataText="To know more about Purchase of second hand (pre-used) Tractors by Agriculturists. Please click on Read More",LoansDataLink=LoansResponseLinks.PurchaseSecondHandTractors,LoansDataImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "PurchaseOfSecondHandTractorsByAgriculturists.PNG")} },
            {"agri clinic agri business centres",new LoanData{LoansDataTitle="Agri Clinic and Agri Business Centres", LoansDataText="To know more about Agri Clinic and Agri Business Centres. Please click on Read More",LoansDataLink=LoansResponseLinks.AgriClinicAgriBusinessCentres,LoansDataImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "AgriClinicAndAgriBusinessCentres.PNG")} },
            {"shg bank linkage programme",new LoanData{LoansDataTitle="SHG Bank Linkage Programme – Direct Linkage to SHGs", LoansDataText="To know more about SHG Bank Linkage Programme – Direct Linkage to SHGs. Please click on Read More",LoansDataLink=LoansResponseLinks.SGH_BankLinkageProgramme,LoansDataImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "SHG_BankLinkageProgramme.PNG")} },
            {"joint liability group",new LoanData{LoansDataTitle="Joint liability group (JLG)", LoansDataText="To know more about Joint liability group (JLG). Please click on Read More",LoansDataLink=LoansResponseLinks.JointLiabilityGroup,LoansDataImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "JointLiabilityGroup.PNG")} },
            {"rupay kisan card",new LoanData{LoansDataTitle="Rupay Kisan card", LoansDataText="To know more about Rupay Kisan card. Please click on Read More",LoansDataLink=LoansResponseLinks.RupayKisanCard,LoansDataImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "RupayKisanCard.PNG")} },
            {"dri scheme revised norms",new LoanData{LoansDataTitle="DRI Scheme – Revised Norms", LoansDataText="To know more about DRI Scheme – Revised Norms. Please click on Read More",LoansDataLink=LoansResponseLinks.DRI_SchemeRevisedNorms,LoansDataImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "DRI_SchemeRevisedNorms.PNG")} },
            {"shg vidhya shoba",new LoanData{LoansDataTitle="SHG – Vidhya Shoba", LoansDataText="To know more about SHG – Vidhya Shoba. Please click on Read More",LoansDataLink=LoansResponseLinks.SGH_VidhyaShoba,LoansDataImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "SHG_VidhyaShoba.PNG")} },
            {"gramin mahila sowbhagya scheme",new LoanData{LoansDataTitle="Gramin Mahila Sowbhagya Scheme", LoansDataText="To know more about Gramin Mahila Sowbhagya Scheme. Please click on Read More",LoansDataLink=LoansResponseLinks.GraminMahilaSowbhagyaScheme,LoansDataImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "GraminMahilaSowbhagyaScheme.PNG")} },
            {"sugar premium scheme",new LoanData{LoansDataTitle="Sugar Premium Scheme",LoansDataText="To know more about Sugar Premium Scheme. Please click on Read More" ,LoansDataLink=LoansResponseLinks.SugarPremiumScheme,LoansDataImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "SugarPremiumScheme.PNG")} },
            {"golden harvest scheme",new LoanData{LoansDataTitle="Golden Harvest Scheme", LoansDataText="To know more about Golden Harvest Scheme. Please click on Read More",LoansDataLink=LoansResponseLinks.GoldenHarvestScheme,LoansDataImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "GoldenHarvestScheme.PNG")} },
            {"agricultural jewel loan scheme",new LoanData{LoansDataTitle="Agricultural Jewel Loan Scheme", LoansDataText="To know more about Agricultural Jewel Loan Scheme. Please click on Read More",LoansDataLink=LoansResponseLinks.AgriculturalJewelLoanScheme,LoansDataImagePath=Path.Combine(".", @"Resources\loans\AgricultureImages", "AgriculturalJewelLoanScheme.PNG")} },

            //groups
            {"groups agricultural godowns",new LoanData{LoansDataTitle="Agricultural Godowns / Cold Storage", LoansDataText="To know more about Agricultural Godowns / Cold Storage. Please click on Read More",LoansDataLink=LoansResponseLinks.GroupsAgriculturalGodowns,LoansDataImagePath=Path.Combine(".", @"Resources\loans\GroupsImages", "GroupsAgriculturalGodowns.PNG")} },
            {"groups shg bank linkage programme",new LoanData{LoansDataTitle="SHG Bank Linkage Programme – Direct Linkage to SHGs", LoansDataText="To know more about SHG Bank Linkage Programme – Direct Linkage to SHGs. Please click on Read More",LoansDataLink=LoansResponseLinks.Groups_SGH_BankLinkageProgramme,LoansDataImagePath=Path.Combine(".", @"Resources\loans\GroupsImages", "GroupsSHG_BankLinkageProgrammeDirectLinkageToSHGS.PNG")} },
            {"groups shg vidhya shoba",new LoanData{LoansDataTitle="SHG – Vidhya Shoba", LoansDataText="To know more about SHG – Vidhya Shoba. Please click on Read More",LoansDataLink=LoansResponseLinks.Groups_SGH_VidhyaShoba,LoansDataImagePath=Path.Combine(".", @"Resources\loans\GroupsImages", "GroupsSHG_VidhyaShoba.PNG")} },

            //personal/individual
            {"Ib home loan combo",new LoanData{LoansDataTitle="IB Home Loan Combo", LoansDataText="To know more about IB Home Loan Combo. Please click on Read More",LoansDataLink=LoansResponseLinks.IbHomeLoanCombo,LoansDataImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IbHomeLoanCombo.PNG")} },
            {"ib rent encash",new LoanData{LoansDataTitle="IB Rent Encash", LoansDataText="To know more about IB Rent Encash. Please click on Read More",LoansDataLink=LoansResponseLinks.IbRentEncash,LoansDataImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IbRentEncash.PNG")} },
            {"loan od against deposits",new LoanData{LoansDataTitle="Loan / OD against Deposits", LoansDataText="To know more about Loan / OD against Deposits. Please click on Read More",LoansDataLink=LoansResponseLinks.LoanOD_AgainstDeposits,LoansDataImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "Loan_OD_AgainstDeposits.PNG")} },
            {"ib clean loan",new LoanData{LoansDataTitle="IB Clean Loan (to Salaried Class)", LoansDataText="To know more about IB Clean Loan (to Salaried Class). Please click on Read More",LoansDataLink=LoansResponseLinks.IbCleanLoan,LoansDataImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IbCleanLoan.PNG")} },
            {"ib balavidhya scheme",new LoanData{LoansDataTitle="IB Balavidhya Scheme", LoansDataText="To know more about IB Balavidhya Scheme. Please click on Read More",LoansDataLink=LoansResponseLinks.IbBalavidhyaScheme,LoansDataImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IbBalavidhyaScheme.PNG")} },
            {"ind reverse mortgage",new LoanData{LoansDataTitle="Ind Reverse Mortgage", LoansDataText="To know more about Ind Reverse Mortgage. Please click on Read More",LoansDataLink=LoansResponseLinks.IndReverseMortgage,LoansDataImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IndReverseMortgage.PNG")} },
            {"ib vehicle loan",new LoanData{LoansDataTitle="IB Vehicle Loan", LoansDataText="To know more about IB Vehicle Loan. Please click on Read More",LoansDataLink=LoansResponseLinks.IbVehicleloan,LoansDataImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IbVehicleLoan.PNG")} },
            {"ind mortgage",new LoanData{LoansDataTitle="Ind Mortgage", LoansDataText="To know more about Ind Mortgage. Please click on Read More",LoansDataLink=LoansResponseLinks.IndMortgage,LoansDataImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IndMortgage.PNG")} },
            {"plot loan",new LoanData{LoansDataTitle="Plot Loan", LoansDataText="To know more about Plot Loan. Please click on Read More",LoansDataLink=LoansResponseLinks.PlotLoan,LoansDataImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "PlotLoan.PNG")} },
            {"ib home loan",new LoanData{LoansDataTitle="IB Home Loan", LoansDataText="To know more about IB Home Loan. Please click on Read More",LoansDataLink=LoansResponseLinks.IbHomeLoan,LoansDataImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IbHomeLoan.PNG")} },
            {"ib pension loan",new LoanData{LoansDataTitle="IB Pension Loan", LoansDataText="To know more about IB Pension Loan. Please click on Read More",LoansDataLink=LoansResponseLinks.IbPensionLoan,LoansDataImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IbPensionLoan.PNG")} },
            {"home improve",new LoanData{LoansDataTitle="IB Home Improve", LoansDataText="To know more about IB Home Improve. Please click on Read More",LoansDataLink=LoansResponseLinks.HomeImprove,LoansDataImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "HomeImprove.PNG")} },
            {"ib home loan plus",new LoanData{LoansDataTitle="IB Home Loan Plus", LoansDataText="To know more about IB Home Loan Plus. Please click on Read More",LoansDataLink=LoansResponseLinks.IbHomeLoanPlus,LoansDataImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "IbHomeLoanPlus.PNG")} },
            {"loan od against nsc",new LoanData{LoansDataTitle="Loan / OD against NSC / KVP / Relief bonds of RBI / LIC policies", LoansDataText="To know more about Loan / OD against NSC / KVP / Relief bonds of RBI / LIC policies. Please click on Read More",LoansDataLink=LoansResponseLinks.LoanOD_AgainstNSC,LoansDataImagePath=Path.Combine(".", @"Resources\loans\PersonalIndividualImages", "Loan_OD_AgainstNSC.PNG")} },
            
            //59 minutes loans
            {"59 minutes loans",new LoanData{LoansDataTitle="59 Minutes Loan",LoansDataText="To know more about 59 Minutes Loan. Please click on Read More",LoansDataLink=LoansResponseLinks.FNMinutesLoanLink,LoansDataImagePath=Path.Combine(".", @"Resources\loans\FNMinutesLoan", "indian_bank.jpg")} },
            
            //MSME Loan Menu Item
            {"ib vidhya mandir",new LoanData{LoansDataTitle="IB Vidhya Mandir",LoansDataText="Target Group Reputed Educational Institutions Purpose Construction of Pucca building with reinforced Cement Concrete (RCC) Roofing. Purchase of equipments. Nature…",LoansDataLink=MSMELoansResponseLinks.IBVidhyaMandirLink,LoansDataImagePath=Path.Combine(".", @"Resources\loans\MSME", "ib_vidhya_mandir.PNG")} },
            {"ib my own shop",new LoanData{LoansDataTitle="IB My Own Shop",LoansDataText="Target Group All Micro / Small – Service Enterprise Individuals, Professionals and Self Employed, Proprietary Concerns Registered Partnership firms, Private…",LoansDataLink=MSMELoansResponseLinks.IBMyOwnShopLink,LoansDataImagePath=Path.Combine(".", @"Resources\loans\MSME", "ib_my_own_shop.PNG")} },
            {"ib doctor plus",new LoanData{LoansDataTitle="IB Doctor Plus",LoansDataText="Target Group Professional Doctors. (Individuals, Proprietary Concern of Doctor, Registered Partnership firms, Private Limited / Public Limited Companies, Trust)…",LoansDataLink=MSMELoansResponseLinks.IBDoctorPlusLink,LoansDataImagePath=Path.Combine(".", @"Resources\loans\MSME", "ib_doctor_plus.PNG")} },
            {"ib contractors",new LoanData{LoansDataTitle="IB Contractors",LoansDataText="Target Group All well established contractors (Civil, Mechanical, Electrical etc) performing Contracts for Central / State Govt. / reputed PSUs…",LoansDataLink=MSMELoansResponseLinks.IBContractorsLink,LoansDataImagePath=Path.Combine(".", @"Resources\loans\MSME", "ib_contractors.PNG")} },
            {"tradewell",new LoanData{LoansDataTitle="Tradewell",LoansDataText="Target Group Traders / Trading Enterprises with 3 years experience in the trade Purpose To meet working capital requirements To…",LoansDataLink=MSMELoansResponseLinks.TradewellLink,LoansDataImagePath=Path.Combine(".", @"Resources\loans\MSME", "tradewell.PNG")} },
            {"ind sme secure",new LoanData{LoansDataTitle="IND SME Secure",LoansDataText="Target Group Micro, Small & Medium Enterprises (Manufacturing & Services Sector) Purpose Working Capital Term Loan for construction of Building,…",LoansDataLink=MSMELoansResponseLinks.IndSMESecureLink,LoansDataImagePath=Path.Combine(".", @"Resources\loans\MSME", "ind_sme_secure.PNG")} },
            //Education Loan Menu Item
            {"model education loan scheme",new LoanData{LoansDataTitle="Revised IBA Model Educational Loan Scheme (2015)",LoansDataText="Indian Bank provides convenient educational loans for meritorious/deserving students to acquire knowledge and skill in the field of their interest.…",LoansDataLink=EducationLoansResponseLinks.ModelEducationalLoanLink,LoansDataImagePath=Path.Combine(".", @"Resources\loans\Education", "iba_model_educational_loan_schema.PNG")} },
            {"educational loan prime",new LoanData{LoansDataTitle="IB Educational Loan Prime",LoansDataText="Indian Bank provides convenient educational loans for meritorious/deserving students to acquire knowledge and skill in the field of their interest.…",LoansDataLink=EducationLoansResponseLinks.IBEducationalLoanPrimeLink,LoansDataImagePath=Path.Combine(".", @"Resources\loans\Education", "ib_educational_loan_prime.PNG")} },
            {"skill loan scheme",new LoanData{LoansDataTitle="IB Skill Loan Scheme",LoansDataText="Indian Bank provides convenient educational loans for meritorious/deserving students to acquire knowledge and skill in the field of their interest.…",LoansDataLink=EducationLoansResponseLinks.IBSkillLoanSchemeLink,LoansDataImagePath=Path.Combine(".", @"Resources\loans\Education", "ib_skill_loan_scheme.PNG")} },
            {"education loan interest",new LoanData{LoansDataTitle="Education Loan Interest Subsidies",LoansDataText="Government of India provides interest charged upto the moratorium period as subsidy for education loans under three schemes subject to…",LoansDataLink=EducationLoansResponseLinks.EducationLoanInterestLink,LoansDataImagePath=Path.Combine(".", @"Resources\loans\Education", "education_loan_interest_subsidies.PNG")} },
            //nri Loan Menu Item
            {"nri plot loan",new LoanData{LoansDataTitle="NRI Plot Loan",LoansDataText="Purpose / Objective For purchase of House Site on ownership basis; House site should be located in layout approved by…",LoansDataLink=NRILoansResponseLinks.NRIPlotLoanLink,LoansDataImagePath=Path.Combine(".", @"Resources\loans\NRI", "nri_plot_loan.PNG")} },
            {"nri home loan",new LoanData{LoansDataTitle="NRI Home Loan",LoansDataText="Eligibility Maximum permissible age at the time of applying is 50 years & at the end of repayment period is…",LoansDataLink=NRILoansResponseLinks.NRIHomeLoanLink,LoansDataImagePath=Path.Combine(".", @"Resources\loans\NRI", "nri_home_loan.PNG")} }
        };

        #endregion

        #region Constructor

        public LoansResponses()
        {
            Register(new DictionaryRenderer(_responseTemplates));
        }

        #endregion

        #region Methods

        private static object BuildLoansCard(ITurnContext context, LoanData data)
        {
            var attachment = new HeroCard()
            {
                Title = data.LoansDataTitle,
                Text = data.LoansDataText,
                Images = new List<CardImage> { new CardImage(data.LoansDataImagePath) },
                Buttons = new List<CardAction>()
                {
                   new CardAction(type: ActionTypes.OpenUrl, title: "Read More", value: data.LoansDataLink)
                }
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        public static IMessageActivity MSMELoanMenuCardDisplay(ITurnContext turnContext)
        {
            var attachment = new HeroCard()
            {
                Buttons = SharedResponses.SuggestedActionsForMSMELoanMenu.Actions
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        public static IMessageActivity EducationLoanMenuCardDisplay(ITurnContext turnContext)
        {
            var attachment = new HeroCard()
            {
                Buttons = SharedResponses.SuggestedActionsForEducationLoanMenu.Actions
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        public static IMessageActivity NRILoanMenuCardDisplay(ITurnContext turnContext)
        {
            var attachment = new HeroCard()
            {
                Buttons = SharedResponses.SuggestedActionsForNRILoanMenu.Actions
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        /// <summary>
        /// Code to display hero card for loan menu
        /// </summary>
        /// <param name="turnContext"></param>
        /// <returns></returns>
        public static IMessageActivity BuildLoansMenuCard(ITurnContext turnContext)
        {
            var attachment = new HeroCard()
            {
                Buttons = SharedResponses.SuggestedActionsForLoanMenu.Actions
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        /// <summary>
        /// Code to display hero card for BuildAgricultureMenuCard
        /// </summary>
        /// <param name="turnContext"></param>
        /// <returns></returns>
        public static IMessageActivity BuildAgricultureMenuCard(ITurnContext turnContext)
        {
            var attachment = new HeroCard()
            {
                Buttons = SharedResponses.SuggestedActionsForAgricultureMenu.Actions
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        /// <summary>
        /// Code to display hero card for BuildGroupsMenuCard
        /// </summary>
        /// <param name="turnContext"></param>
        /// <returns></returns>
        public static IMessageActivity BuildGroupsMenuCard(ITurnContext turnContext)
        {
            var attachment = new HeroCard()
            {
                Buttons = SharedResponses.SuggestedActionsForGroups.Actions
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        /// <summary>
        /// Code to display hero card for BuildPersonalIndividualMenuCard
        /// </summary>
        /// <param name="turnContext"></param>
        /// <returns></returns>
        public static IMessageActivity BuildPersonalIndividualMenuCard(ITurnContext turnContext)
        {
            var attachment = new HeroCard()
            {
                Buttons = SharedResponses.SuggestedActionsForPersonalIndividual.Actions
            }.ToAttachment();

            var response = MessageFactory.Attachment(attachment, ssml: null, inputHint: InputHints.AcceptingInput);
            return response;
        }

        /// <summary>
        /// Gets the loans data.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static LoanData getLoansData(string result)
        {
            LoanData loanData = new LoanData();
            var res = keyValuePairs.TryGetValue(result, out loanData);
            return loanData;
        }

        #endregion


        #region Class 

        public class LoanResponseIds
        {
            // Loans ResponseIds constants
            public const string LoansMenuCardDisplay = "loansMenuCardDisplay";
            public const string AgricultureMenuCardDisplay = "agricultureMenuCardDisplay";
            public const string GroupsMenuCardDisplay = "groupsMenuCardDisplay";
            public const string PersonalIndividualCardDisplay = "personalIndividualMenuCardDisplay";
            public const string BuildLoansCard = "buildLoansCard";
            public const string MSMELoansMenuCard = "buildMSMESubmenuCard";
            public const string EducationLoansMenuCard = "buildEducationSubmenuCard";
            public const string NRILoansMenuCard = "buildNRISubmenuCard";

        }

        //urls
        //Loans links
        public class LoansResponseLinks
        {
            public const string FNMinutesLoanLink = "https://www.psbloansin59minutes.com/indianbank";

            //agriculture links
            public const string AgriculturalGodowns = "https://www.indianbank.in/departments/agricultural-godowns-cold-storage/#!";
            public const string LoansForMaintenanceOfTractors = "https://www.indianbank.in/departments/loans-for-maintenance-of-tractors-under-tie-up-with-sugar-mills/#!";
            public const string AgriculturalProduceMarketingLoan = "https://www.indianbank.in/departments/agricultural-produce-marketing-loan/#!";
            public const string FinancingAgriculturistsForPurchaseOfTractors = "https://www.indianbank.in/departments/financing-agriculturists-for-purchase-of-tractors/#!";
            public const string PurchaseSecondHandTractors = "https://www.indianbank.in/departments/purchase-of-second-hand-pre-used-tractors-by-agriculturists/#!";
            public const string AgriClinicAgriBusinessCentres = "https://www.indianbank.in/departments/agri-clinic-and-agri-business-centres/#!";
            public const string SGH_BankLinkageProgramme = "https://www.indianbank.in/departments/shg-bank-linkage-programme-direct-linkage-to-shgs/#!";
            public const string JointLiabilityGroup = "https://www.indianbank.in/departments/joint-liability-group-jlg/#!";
            public const string RupayKisanCard = "https://www.indianbank.in/departments/rupay-kisan-card/#!";
            public const string DRI_SchemeRevisedNorms = "https://www.indianbank.in/departments/dri-scheme-revised-norms/#!";
            public const string SGH_VidhyaShoba = "https://www.indianbank.in/departments/shg-vidhya-shoba/#!";
            public const string GraminMahilaSowbhagyaScheme = "https://www.indianbank.in/departments/gramin-mahila-sowbhagya-scheme/#!";
            public const string SugarPremiumScheme = "https://www.indianbank.in/departments/sugar-premium-scheme/#!";
            public const string GoldenHarvestScheme = "https://www.indianbank.in/departments/golden-harvest-scheme/#!";
            public const string AgriculturalJewelLoanScheme = "https://www.indianbank.in/departments/agricultural-jewel-loan-scheme/#!";

            //groups entities
            public const string GroupsAgriculturalGodowns = "https://www.indianbank.in/departments/agricultural-godowns-cold-storage/#!";
            public const string Groups_SGH_BankLinkageProgramme = "https://www.indianbank.in/departments/shg-bank-linkage-programme-direct-linkage-to-shgs/#!";
            public const string Groups_SGH_VidhyaShoba = "https://www.indianbank.in/departments/shg-vidhya-shoba/#!";

            //personal/individual entities
            public const string HomeLoanVehicleLoan = "https://www.indianbank.in/departments/home-loan-and-vehicle-loan-festive-offer/#!";
            public const string NewIbJavanVidya = "https://www.indianbank.in/departments/new-ib-jeevan-vidya/#!";
            public const string IbHomeLoanCombo = "https://www.indianbank.in/departments/ib-home-loan-combo/#!";
            public const string IbRentEncash = "https://www.indianbank.in/departments/ib-rent-encash/#!";
            public const string LoanOD_AgainstDeposits = "https://www.indianbank.in/departments/loan-od-against-deposits/#!";
            public const string IbCleanLoan = "https://www.indianbank.in/departments/ib-clean-loan-to-salaried-class/#!";
            public const string IbBalavidhyaScheme = "https://www.indianbank.in/departments/ib-balavidhya-scheme/#!";
            public const string IndReverseMortgage = "https://www.indianbank.in/departments/ind-reverse-mortgage/#!";
            public const string IbVehicleloan = "https://www.indianbank.in/departments/ib-vehicle-loan/#!";
            public const string IndMortgage = "https://www.indianbank.in/departments/ind-mortgage/#!";
            public const string PlotLoan = "https://www.indianbank.in/departments/plot-loan/#!";
            public const string IbHomeLoan = "https://www.indianbank.in/departments/ib-home-loan/#!";
            public const string IbPensionLoan = "https://www.indianbank.in/departments/ib-pension-loan/#!";
            public const string HomeImprove = "https://www.indianbank.in/departments/home-improve/#!";
            public const string IbHomeLoanPlus = "https://www.indianbank.in/departments/ib-home-loan-plus/#!";
            public const string LoanOD_AgainstNSC = "https://www.indianbank.in/departments/loan-od-against-nsc-kvp-relief-bonds-of-rbi-lic-policies/#!";

        }

        //MSME Loans links
        public class MSMELoansResponseLinks
        {
            public const string IBVidhyaMandirLink = "https://www.indianbank.in/departments/ib-vidhya-mandir/#!";
            public const string IBMyOwnShopLink = "https://www.indianbank.in/departments/my-own-shop/#!";
            public const string IBDoctorPlusLink = "https://www.indianbank.in/departments/ib-doctor-plus/#!";
            public const string IBContractorsLink = "https://www.indianbank.in/departments/ib-contractors/#!";
            public const string TradewellLink = "https://www.indianbank.in/departments/ib-tradewell/#!";
            public const string IndSMESecureLink = "https://www.indianbank.in/departments/ind-sme-secure/#!";
        }

        //Education Loans links
        public class EducationLoansResponseLinks
        {
            public const string ModelEducationalLoanLink = "https://www.indianbank.in/departments/revised-iba-model-educational-loan-scheme-2015/#!";
            public const string IBEducationalLoanPrimeLink = "https://www.indianbank.in/departments/ib-educational-loan-prime/#!";
            public const string IBSkillLoanSchemeLink = "https://www.indianbank.in/departments/ib-skill-loan-scheme/#!";
            public const string EducationLoanInterestLink = "https://www.indianbank.in/departments/hindi-education-loan-interest-subsidies/#!";
        }

        //NRI Loans links
        public class NRILoansResponseLinks
        {
            public const string NRIPlotLoanLink = "https://www.indianbank.in/departments/nri-plot-loan/#!";
            public const string NRIHomeLoanLink = "https://www.indianbank.in/departments/nri-home-loan/#!";

        }
        public class LoanData
        {
            public string LoansDataTitle { get; set; }
            public string LoansDataText { get; set; }
            public string LoansDataLink { get; set; }
            public string LoansDataImagePath { get; set; }
        }


        #endregion
    }



}
