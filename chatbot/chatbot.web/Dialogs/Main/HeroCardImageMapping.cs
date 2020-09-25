using System.Collections.Generic;
using System.IO;

namespace IndianBank_ChatBOT.Dialogs.Main
{
    internal static class HeroCardImageMapping
    {
        public static Dictionary<string, string> MAPPING = new Dictionary<string, string>
        {
            //Contacts Menu Items
            {"quick_contacts.PNG", @"Resources\contacts\QuickContacts\quick_contacts.PNG"},
            {"customer_complaints.PNG", @"Resources\contacts\CustomerSupport\customer_complaints.PNG"},
            {"complaints_officers_list.PNG", @"Resources\contacts\CustomerSupport\complaints_officers_list.PNG"},
            {"chief_vigilance_officer.PNG", @"Resources\contacts\CustomerSupport\chief_vigilance_officer.PNG"},
            {"head_office.PNG", @"Resources\contacts\EmailIDs\head_office.PNG"},
            {"department.PNG", @"Resources\contacts\EmailIDs\department.PNG"},
            {"executives.PNG", @"Resources\contacts\EmailIDs\executives.PNG"},
            {"image.PNG", @"Resources\contacts\EmailIDs\image.PNG"},
            {"foreign_branches.PNG", @"Resources\contacts\EmailIDs\foreign_branches.PNG"},
            {"overseas_branches.PNG", @"Resources\contacts\EmailIDs\overseas_branches.PNG"},
            {"nri_branches.PNG", @"Resources\contacts\EmailIDs\nri_branches.PNG"},
            {"zonal_offices.PNG", @"Resources\contacts\EmailIDs\zonal_offices.PNG"},
            {"econfirmation_of_bank_guarantee.PNG", @"Resources\contacts\EmailIDs\econfirmation_of_bank_guarantee.PNG"},
            {"SavingsBank.PNG", @"Resources\deposits\SavingsBank.PNG"},
            {"PayrollPackage.PNG", @"Resources\deposits\PayrollPackage.PNG"},
            {"VikasSavingsKhata.PNG", @"Resources\deposits\VikasSavingsKhata.PNG"},
            {"IbSmartKid.PNG", @"Resources\deposits\IbSmartKid.PNG"},
            {"SavingsTermsConditions.PNG", @"Resources\deposits\SavingsTermsConditions.PNG"},
            {"SbPlatinum.PNG", @"Resources\deposits\SbPlatinum.PNG"},
            {"IbSurabhi.PNG", @"Resources\deposits\IbSurabhi.PNG"},

             //Current Account
            {"CurrentAccount.PNG", @"Resources\deposits\CurrentAccount.PNG"},
            {"FreedomCurrentAccount.PNG", @"Resources\deposits\FreedomCurrentAccount.PNG"},
            {"CurrentTermsAndConditions.PNG", @"Resources\deposits\CurrentTermsAndConditions.PNG"},
            {"PremiumCurrentAccount.PNG", @"Resources\deposits\PremiumCurrentAccount.PNG"},

            //term deposit entities constants
            {"FacilityDeposit.PNG", @"Resources\deposits\FacilityDeposit.PNG"},
            {"CapitalGains.PNG", @"Resources\deposits\CapitalGains.PNG"},
            {"Term-TermsConditions.PNG", @"Resources\deposits\Term-TermsConditions.PNG"},
            {"DepositSchemeForSeniorCitizens.PNG", @"Resources\deposits\DepositSchemeForSeniorCitizens.PNG"},

            {"RecurringDeposit.PNG", @"Resources\deposits\RecurringDeposit.PNG"},
            {"IbTaxSaverScheme.PNG", @"Resources\deposits\IbTaxSaverScheme.PNG"},
            {"InsuredRecurringDeposit.PNG", @"Resources\deposits\InsuredRecurringDeposit.PNG"},
            {"ReInvestmentPlan.PNG", @"Resources\deposits\ReInvestmentPlan.PNG"},
            {"FixedDeposit.PNG", @"Resources\deposits\FixedDeposit.PNG"},
            {"VariableRecurringDeposit.PNG", @"Resources\deposits\VariableRecurringDeposit.PNG"},

            //nri accounts entities constants
            {"ForeignCurrencyForReturningIndians.PNG", @"Resources\deposits\ForeignCurrencyForReturningIndians.PNG"},
            {"NRE_FD_RIP_RD_Accounts.PNG", @"Resources\deposits\NRE_FD_RIP_RD_Accounts.PNG"},
            {"NRE_SB_Accounts.PNG", @"Resources\deposits\NRE_SB_Accounts.PNG"},
            {"NonResidentOrdinaryAccount.PNG", @"Resources\deposits\NonResidentOrdinaryAccount.PNG"},
            {"FCNR_Accounts.PNG", @"Resources\deposits\FCNR_Accounts.PNG"},
        
            //agriculture
            {"AgriculturalGodowns.PNG", @"Resources\loans\AgricultureImages\AgriculturalGodowns.PNG"},
            {"LoansForMaintainenceOfTractorsUnderTie-UpWithSugarMills.PNG", @"Resources\loans\AgricultureImages\LoansForMaintainenceOfTractorsUnderTie-UpWithSugarMills.PNG"},
            {"AgriculturalProduceMarketingLoan.PNG", @"Resources\loans\AgricultureImages\AgriculturalProduceMarketingLoan.PNG"},
            {"FinancingAgriculturistsForPurchaseOfTractors.PNG", @"Resources\loans\AgricultureImages\FinancingAgriculturistsForPurchaseOfTractors.PNG"},
            {"PurchaseOfSecondHandTractorsByAgriculturists.PNG", @"Resources\loans\AgricultureImages\PurchaseOfSecondHandTractorsByAgriculturists.PNG"},
            {"AgriClinicAndAgriBusinessCentres.PNG", @"Resources\loans\AgricultureImages\AgriClinicAndAgriBusinessCentres.PNG"},
            {"SHG_BankLinkageProgramme.PNG", @"Resources\loans\AgricultureImages\SHG_BankLinkageProgramme.PNG"},
            {"JointLiabilityGroup.PNG", @"Resources\loans\AgricultureImages\JointLiabilityGroup.PNG"},
            {"RupayKisanCard.PNG", @"Resources\loans\AgricultureImages\RupayKisanCard.PNG"},
            {"DRI_SchemeRevisedNorms.PNG", @"Resources\loans\AgricultureImages\DRI_SchemeRevisedNorms.PNG"},
            {"SHG_VidhyaShoba.PNG", @"Resources\loans\AgricultureImages\SHG_VidhyaShoba.PNG"},
            {"GraminMahilaSowbhagyaScheme.PNG", @"Resources\loans\AgricultureImages\GraminMahilaSowbhagyaScheme.PNG"},
            {"SugarPremiumScheme.PNG", @"Resources\loans\AgricultureImages\SugarPremiumScheme.PNG"},
            {"GoldenHarvestScheme.PNG", @"Resources\loans\AgricultureImages\GoldenHarvestScheme.PNG"},
            {"AgriculturalJewelLoanScheme.PNG", @"Resources\loans\AgricultureImages\AgriculturalJewelLoanScheme.PNG"},

            //groups
            {"GroupsAgriculturalGodowns.PNG", @"Resources\loans\GroupsImages\GroupsAgriculturalGodowns.PNG"},
            {"GroupsSHG_BankLinkageProgrammeDirectLinkageToSHGS.PNG", @"Resources\loans\GroupsImages\GroupsSHG_BankLinkageProgrammeDirectLinkageToSHGS.PNG"},
            {"GroupsSHG_VidhyaShoba.PNG", @"Resources\loans\GroupsImages\GroupsSHG_VidhyaShoba.PNG"},

            //personal/individual
            {"IbHomeLoanCombo.PNG", @"Resources\loans\PersonalIndividualImages\IbHomeLoanCombo.PNG"},
            {"IbRentEncash.PNG", @"Resources\loans\PersonalIndividualImages\IbRentEncash.PNG"},
            {"Loan_OD_AgainstDeposits.PNG", @"Resources\loans\PersonalIndividualImages\Loan_OD_AgainstDeposits.PNG"},
            {"IbCleanLoan.PNG", @"Resources\loans\PersonalIndividualImages\IbCleanLoan.PNG"},
            {"IbBalavidhyaScheme.PNG", @"Resources\loans\PersonalIndividualImages\IbBalavidhyaScheme.PNG"},
            {"IndReverseMortgage.PNG", @"Resources\loans\PersonalIndividualImages\IndReverseMortgage.PNG"},
            {"IbVehicleLoan.PNG", @"Resources\loans\PersonalIndividualImages\IbVehicleLoan.PNG"},
            {"IndMortgage.PNG", @"Resources\loans\PersonalIndividualImages\IndMortgage.PNG"},
            {"PlotLoan.PNG", @"Resources\loans\PersonalIndividualImages\PlotLoan.PNG"},
            {"IbHomeLoan.PNG", @"Resources\loans\PersonalIndividualImages\IbHomeLoan.PNG"},
            {"IbPensionLoan.PNG", @"Resources\loans\PersonalIndividualImages\IbPensionLoan.PNG"},
            {"HomeImprove.PNG", @"Resources\loans\PersonalIndividualImages\HomeImprove.PNG"},
            {"IbHomeLoanPlus.PNG", @"Resources\loans\PersonalIndividualImages\IbHomeLoanPlus.PNG"},
            {"Loan_OD_AgainstNSC.PNG", @"Resources\loans\PersonalIndividualImages\Loan_OD_AgainstNSC.PNG"},
            {"HomeLoanAndVehicleLoan.PNG", @"Resources\loans\PersonalIndividualImages\HomeLoanAndVehicleLoan.PNG"},
            
            //59 minutes loans
            {"59_min_loan_indian_bank.jpg", @"Resources\loans\FNMinutesLoan\indian_bank.jpg"},
            
            //MSME Loan Menu Item
            {"ib_vidhya_mandir.PNG", @"Resources\loans\MSME\ib_vidhya_mandir.PNG"},
            {"ib_my_own_shop.PNG", @"Resources\loans\MSME\ib_my_own_shop.PNG"},
            {"ib_doctor_plus.PNG", @"Resources\loans\MSME\ib_doctor_plus.PNG"},
            {"ib_contractors.PNG", @"Resources\loans\MSME\ib_contractors.PNG"},
            {"tradewell.PNG", @"Resources\loans\MSME\tradewell.PNG"},
            {"ind_sme_secure.PNG", @"Resources\loans\MSME\ind_sme_secure.PNG"},
            //Education Loan Menu Item
            {"iba_model_educational_loan_schema.PNG", @"Resources\loans\Education\iba_model_educational_loan_schema.PNG"},
            {"ib_educational_loan_prime.PNG", @"Resources\loans\Education\ib_educational_loan_prime.PNG"},
            {"ib_skill_loan_scheme.PNG", @"Resources\loans\Education\ib_skill_loan_scheme.PNG"},
            {"education_loan_interest_subsidies.PNG", @"Resources\loans\Education\education_loan_interest_subsidies.PNG"},
            //nri Loan Menu Item
            {"nri_plot_loan.PNG", @"Resources\loans\NRI\nri_plot_loan.PNG"},
            {"nri_home_loan.PNG", @"Resources\loans\NRI\nri_home_loan.PNG"},
        
            //news/info Menu Items
            //keys are same as entity
            { "Notifications.jpg", @"Resources\news_info\Notifications.jpg"},
            { "NewsLetter.jpg", @"Resources\news_info\NewsLetter.jpg"},
            { "WhatsNew.jpg", @"Resources\news_info\WhatsNew.jpg"},
            { "SmsBanking.jpg", @"Resources\news_info\SmsBanking.jpg"},
            { "MyDesignCard.PNG", @"Resources\news_info\MyDesignCard.PNG"},
            { "PressReleases.jpg", @"Resources\news_info\PressReleases.jpg"},
            { "Downloads.jpg", @"Resources\news_info\Downloads.jpg"},

            //customer corner
            { "OnlineCustomerComplaints.jpg", @"Resources\news_info\OnlineCustomerComplaints.jpg"},
            { "PrincipalCodeComplianceOfficer.jpg", @"Resources\news_info\PrincipalCodeComplianceOfficer.jpg"},
            { "DamodaranCommitteeRecommendations.jpg", @"Resources\news_info\DamodaranCommitteeRecommendations.jpg"},
            { "BankingOmbudsman.jpg", @"Resources\news_info\BankingOmbudsman.jpg"},
            { "RemitToIndia.jpg", @"Resources\news_info\RemitToIndia.jpg"},

            //related info
            { "FAQs.jpg", @"Resources\news_info\FAQs.jpg"},
            { "Disclaimer.jpg", @"Resources\news_info\Disclaimer.jpg"},

            //codes/policy/disclosure
            { "BestPracticesCodeOfTheBank.jpg", @"Resources\news_info\BestPracticesCodeOfTheBank.jpg"},
            { "CustomerCentricServices.jpg", @"Resources\news_info\CustomerCentricServices.jpg"},

            //charters/schemes
            { "AgriculturalDebtWaiver.jpg", @"Resources\news_info\AgriculturalDebtWaiver.jpg"},
            { "FinancialInclusionPlan.jpg", @"Resources\news_info\FinancialInclusionPlan.jpg"},
            { "RestructuredAccounts.jpg", @"Resources\news_info\RestructuredAccounts.jpg"},
            { "ServicesRenderedFreeOfCharge.jpg", @"Resources\news_info\ServicesRenderedFreeOfCharge.jpg"},
            { "WelfareOfMinorities.jpg", @"Resources\news_info\WelfareOfMinorities.jpg"},
            { "WhistleBlowerPolicy.jpg", @"Resources\news_info\WhistleBlowerPolicy.jpg"},
            { "CentralizedPensionProcessingSystem.jpg", @"Resources\news_info\CentralizedPensionProcessingSystem.jpg"},
            { "AnotherOptionForPension.jpg", @"Resources\news_info\AnotherOptionForPension.jpg"},
        
            //Loan Menu Item
            {"depositRates.jpg", @"Resources\rates\depositRates.jpg"},
            {"lendingRates.jpg", @"Resources\rates\lendingRates.jpg"},
            {"serviceCharges.jpg", @"Resources\rates\serviceCharges.jpg"},

            //Services Menu Items
            {"cms_plus.PNG", @"Resources\services\CMS Plus\cms_plus.PNG"},
            {"epayment_direct_taxes.PNG", @"Resources\services\Direct Taxes\epayment_direct_taxes.PNG"},
            {"epayment_indirect_taxes.PNG", @"Resources\services\Indirect Taxes\epayment_indirect_taxes.PNG"},

            //Preminum Services Menu Items
            {"mca_payment.PNG", @"Resources\services\Premium Services\mca_payment.PNG"},
            {"money_gram.PNG", @"Resources\services\Premium Services\money_gram.PNG"},
            {"atm_debit_cards.PNG", @"Resources\services\Premium Services\atm_debit_cards.PNG"},
            {"ind_mobile_banking.PNG", @"Resources\services\Premium Services\ind_mobile_banking.PNG"},
            {"ind_net_banking.PNG", @"Resources\services\Premium Services\ind_net_banking.PNG"},
            {"credit_cards.PNG", @"Resources\services\Premium Services\credit_cards.PNG"},
            {"xpress_money.PNG", @"Resources\services\Premium Services\xpress_money.PNG"},
            {"neft.PNG", @"Resources\services\Premium Services\neft.PNG"},
            {"rtgs.PNG", @"Resources\services\Premium Services\rtgs.PNG"},
            {"multicity_cheque_facility.PNG", @"Resources\services\Premium Services\multicity_cheque_facility.PNG"},

            //Insurance Services Menu Items
            {"ib_vidyarthi_suraksha.PNG", @"Resources\services\Insurance Services\ib_vidyarthi_suraksha.PNG"},
            {"ib_home_security.PNG", @"Resources\services\Insurance Services\ib_home_security.PNG"},
            {"universal_health_care.PNG", @"Resources\services\Insurance Services\universal_health_care.PNG"},
            {"jana_shree_bima_yojana.PNG", @"Resources\services\Insurance Services\jana_shree_bima_yojana.PNG"},
            {"new_ib_jeevan_vidya.PNG", @"Resources\services\Insurance Services\new_ib_jeevan_vidya.PNG"},
            {"ib_jeevan_kalyan.PNG", @"Resources\services\Insurance Services\ib_jeevan_kalyan.PNG"},
            {"ib_varishtha.PNG", @"Resources\services\Insurance Services\ib_varishtha.PNG"},
            {"arogya_raksha.PNG", @"Resources\services\Insurance Services\arogya_raksha.PNG"},
            {"ib_chhatra.PNG", @"Resources\services\Insurance Services\ib_chhatra.PNG"},
            {"ib_griha_jeevan.PNG", @"Resources\services\Insurance Services\ib_griha_jeevan.PNG"},
            {"ib_yatra_suraksha.PNG", @"Resources\services\Insurance Services\ib_yatra_suraksha.PNG"}
        };
    }
}
