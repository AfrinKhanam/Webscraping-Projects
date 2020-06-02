from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
import json
import sys
import array as arr

# ----------------------------------------------------------------------------
folder_1_path = './config_files/param/'
Business_mobile = folder_1_path + 'Business_mobile.json'
Institutional_Smart_Banking_3 = folder_1_path + 'Institutional_Smart Banking_3.json'
Institutional_Support_4 = folder_1_path + 'Institutional_Support_4.json'

# Rate_of_interest = folder_1_path + 'Rate_of_interest.json'
MEAN_INTEREST_RATE_FOR_LOAN_PRODUCT = folder_1_path + 'MEAN_INTEREST_RATE_FOR_LOAN_PRODUCT.json'
FD_INTEREST_RATES = folder_1_path + 'FD_INTEREST_RATES.json'
# Rate_of_interest = folder_1_path + 'Rate_of_interest.json'

recovery_agent = folder_1_path + 'recovery_agent.json'
ujjivansfb_business_current_account_Business_Edge_Current_Account = folder_1_path + 'ujjivansfb_business_current_account_Business Edge Current Account.json'
ujjivansfb_business_current_account_Premium_Current_Account = folder_1_path + 'ujjivansfb_business_current_account_Premium Current Account.json'
ujjivansfb_business_micro_loans = folder_1_path + 'ujjivansfb_business_micro_loans.json'
ujjivansfb_business_net_banking = folder_1_path + 'ujjivansfb_business_net_banking.json'
ujjivansfb_business_regular_Current_Account = folder_1_path + 'ujjivansfb_business_regular_Current_Account.json'
ujjivansfb_corporate_Business_Edge_Current_Account = folder_1_path + 'ujjivansfb_corporate_Business_Edge_Current_Account.json'
ujjivansfb_corporate_current_account_tasc_account = folder_1_path + 'ujjivansfb_corporate_current-account_tasc-account.json'
ujjivansfb_corporate_mse_loans = folder_1_path + 'ujjivansfb_corporate_mse_loans.json'
ujjivansfb_corporate_Premium_Current_Account = folder_1_path + 'ujjivansfb_corporate_Premium_Current_Account.json'
ujjivansfb_corporate_Regular_Current_Account = folder_1_path + 'ujjivansfb_corporate_Regular_Current_Account.json'
ujjivansfb_corporate_salary_account = folder_1_path + 'ujjivansfb_corporate_salary_account.json'
ujjivansfb_in_business_mse_loans = folder_1_path + 'ujjivansfb_in_business_mse_loans.json'
# ----------------------------------------------------------------------------
rural_products_path = './config_files/param/rural_products/'
rural_deposits = rural_products_path + 'rural_deposits.json'
rural_micro = rural_products_path + 'rural_micro.json'
rural_savings_account_Basic_Savings_Bank_Deposit_Account = rural_products_path + 'rural_savings_account_Basic Savings Bank Deposit Account.json'
rural_savings_account_Digital_Account = rural_products_path + 'rural_savings_account_Digital Account.json'
rural_savings_account_Minor_Savings_Account = rural_products_path + 'rural_savings_account_Minor Savings Account.json'
rural_savings_account_Privilege_Savings_Account = rural_products_path + 'rural_savings_account_Privilege Savings Account.json'
rural_savings_account_Regular_Savings_Account = rural_products_path + 'rural_savings_account_Regular_Savings Account.json'
rural_savings_account_Senior_Citizen_Savings_Account = rural_products_path + 'rural_savings_account_Senior Citizen Savings Account.json'
rural_agri_loans_Agri_Group_Loans = rural_products_path + 'rural-agri-loans_Agri Group Loans.json'
rural_agri_loans_Kisan_Suvidha_Loan = rural_products_path + 'rural-agri-loans_Kisan Suvidha Loan.json'
rural_home_loans = rural_products_path + 'rural-home-loans.json'
rural_mse_loans = rural_products_path + 'rural-mse-loans.json'
rural_vehicle_loans_Electric_Three_Wheeler_Loan = rural_products_path + 'rural-vehicle-loans_Electric Three Wheeler Loan.json'
Two_Wheeler_Loan = rural_products_path + 'Two Wheeler Loan.json'
# ----------------------------------------------------------------------------
nri_path = './config_files/param/nri/'
ujjivansfb_nri_savings_account_nre_fixed_deposits = nri_path + 'ujjivansfb_nri_savings_account_nre_fixed_deposits.json'
ujjivansfb_nri_savings_account_nre_saving_account = nri_path + 'ujjivansfb_nri_savings_account_nre_saving_account.json'
ujjivansfb_nri_savings_account_nro_fixed_deposits = nri_path + 'ujjivansfb_nri_savings_account_nro_fixed_deposits.json'
ujjivansfb_nri_savings_account_nro_saving_account = nri_path + 'ujjivansfb_nri_savings_account_nro_saving_account.json'
# ----------------------------------------------------------------------------
institutional_products_path = './config_files/param/institutional_products/'
ujjivansfb_institutional_current_account = institutional_products_path + 'ujjivansfb_institutional_current_account.json'
ujjivansfb_institutional_deposits = institutional_products_path + 'ujjivansfb_institutional_deposits.json'
ujjivansfb_institutional_savings_account = institutional_products_path + 'ujjivansfb_institutional_savings_account.json'
# ----------------------------------------------------------------------------
folder_2_path = './config_files/samyak/'
FD_Interest_Rates = folder_2_path + 'FD_Interest_Rates.json'
Form_Center = folder_2_path + 'Form_Center.json'
Provide_Financial_Security_To_Your_Loved_Ones = folder_2_path + 'Provide Financial Security To Your Loved Ones.json'
Sampoorna_Family_Banking = folder_2_path + 'Sampoorna_Family_Banking.json'
Special_Assistance_during_Unforeseen_Events = folder_2_path + 'Special_Assistance_during_Unforeseen_Events.json'
Terms_And_Conditions = folder_2_path + 'Terms_And_Conditions.json'
Third_Party_Product_Insurance = folder_2_path + 'Third_Party_Product_Insurance.json'
Ujjivan_Bank_Also_Assists_During_Other_Crucial_Events = folder_2_path + 'Ujjivan_Bank_Also_Assists_During_Other_Crucial_Events.json'
About_Life_Events = folder_2_path + 'About_Life_Events.json'
# ----------------------------------------------------------------------------
faq_path = './config_files/faqs/'
FAQ_Business_Net_Banking = faq_path + 'FAQ_Business_Net_Banking.json'
FAQ_Current_Acount = faq_path + 'FAQ_Current_Acount.json'
FAQ_Digital_Account = faq_path + 'FAQ_Digital_Account.json'
FAQ_Fixed_Deposit = faq_path + 'FAQ_Fixed_Deposit.json'
FAQ_Home_Loan = faq_path + 'FAQ_Home_Loan.json'
FAQ_IMPS = faq_path + 'FAQ_IMPS.json'
FAQ_Missed_Call_Banking = faq_path + 'FAQ_Missed_Call_Banking.json'
FAQ_Mobile_Banking = faq_path + 'FAQ_Mobile_Banking.json'
FAQ_MSE = faq_path + 'FAQ_MSE.json'
FAQ_NEFT = faq_path + 'FAQ_NEFT.json'
FAQ_Personal_Loans = faq_path + 'FAQ_Personal_Loans.json'
FAQ_Personal_Net_Banking = faq_path + 'FAQ_Personal_Net_Banking.json'
FAQ_Recurring_Deposits = faq_path + 'FAQ_Recurring_Deposits.json'
FAQ_RTGS = faq_path + 'FAQ_RTGS.json'
FAQ_Savings_Account = faq_path + 'FAQ_Savings_Account.json'
FAQ_SMS_Banking = faq_path + 'FAQ_SMS_Banking.json'
# ----------------------------------------------------------------------------
about_us_path = './config_files/about_us/'
Awards = about_us_path + 'Awards.json'
Board_of_Director = about_us_path + 'Board_of_Director.json'
Financial_Information = about_us_path + 'Financial_Information.json'
Life_At_Ujjivan = about_us_path + 'Life_At_Ujjivan.json'
Management_Team = about_us_path + 'Management_Team.json'
Mission_And_Vision = about_us_path + 'Mission_And_Vision.json'
Profile = about_us_path + 'Profile.json'
# ----------------------------------------------------------------------------
calculator_path = './config_files/calculator/'
Fixed_Deposit_Calculator = calculator_path + 'Fixed_Deposit_Calculator.json'
Recurring_Deposit_Calculator = calculator_path + 'Recurring_Deposit_Calculator.json'
# ----------------------------------------------------------------------------
customer_care_path = './config_files/customer_care/'
Customer_Guidelines = customer_care_path + 'Customer_Guidelines.json'
Customer_Service_Number = customer_care_path + 'Customer_Service_Number.json'
Feedback_or_Complaint = customer_care_path + 'Feedback_or_Complaint.json'
Grievance_Redressal_Business_Correspondents = customer_care_path + 'Grievance_Redressal_Business_Correspondents.json'
Grievance_Redressal_NEFT_RTGS = customer_care_path + 'Grievance_Redressal_NEFT_RTGS.json'
Grievance_Redressal = customer_care_path + 'Grievance_Redressal.json'
Senior_Citizen_Priority = customer_care_path + 'Senior_Citizen_Priority.json'
# ----------------------------------------------------------------------------
investor_relations_path = './config_files/investor_relations/'
Annual_Report = investor_relations_path + 'Annual_Report.json'
Annual_Return = investor_relations_path + 'Annual_Return.json'
Board_Committee = investor_relations_path + 'Board_Committee.json'
Corporate_Governance_Policies = investor_relations_path + 'Corporate_Governance_Policies.json'
DISCLOSURES_TO_STOCK_EXCHANGES = investor_relations_path + 'DISCLOSURES_TO_STOCK_EXCHANGES.json'
Financial_Results = investor_relations_path + 'Financial_Results.json'
Investor_Contacts = investor_relations_path + 'Investor_Contacts.json'
MDs_Letters = investor_relations_path + 'MDs_Letters.json'
Presentations_and_Concall_Transcript = investor_relations_path + 'Presentations and Concall Transcript.json'
Stock_Information = investor_relations_path + 'Stock_Information.json'
# ----------------------------------------------------------------------------
regulatory = './config_files/regulatory/'
Agreements = regulatory + 'Agreements.json'
Banking_Ombudsman = regulatory + 'Banking_Ombudsman.json'
Credit_Rating = regulatory + 'Credit_Rating.json'
GST_Registration_Numbers = regulatory + 'GST_Registration_Numbers.json'
List_of_Business_Correspondents = regulatory + 'List_of_Business_Correspondents.json'
List_of_DSAs = regulatory + 'List_of_DSAs.json'
MCLR_Rate = regulatory + 'MCLR_Rate.json'
Policies = regulatory + 'Policies.json'
Privacy_Policy = regulatory + 'Privacy_Policy.json'
Regulatory_Disclosure_Section = regulatory + 'Regulatory_Disclosure_Section.json'
Security = regulatory + 'Security.json'
TIMELINES_FOR_CONVEYING_CREDIT_DECISIONS = regulatory + 'TIMELINES_FOR_CONVEYING_CREDIT_DECISIONS.json'
Unclaimed_Deposits_Latest = regulatory + 'Unclaimed_Deposits_Latest.json'
Unclaimed_Deposits = regulatory + 'Unclaimed_Deposits.json'
# ----------------------------------------------------------------------------
support_path = './config_files/support/'
ATMs = support_path + 'ATMs.json'
Branches = support_path + 'Branches.json'
Cash_Deposit_Machines = support_path + 'Cash_Deposit_Machines.json'
Locate_Us = support_path + 'Locate_Us.json'
# ----------------------------------------------------------------------------
ujjivan_news_room_path = './config_files/ujjivan_news_room/'
Interviews = ujjivan_news_room_path + 'Interviews.json'
Media_Contact = ujjivan_news_room_path + 'Media_Contact.json'
Other_News = ujjivan_news_room_path + 'Other_News.json'
Overview = ujjivan_news_room_path + 'Overview.json'
Press_Release = ujjivan_news_room_path + 'Press Release.json'
Social_Media_Presence = ujjivan_news_room_path + 'Social_Media_Presence.json'


json_files = [
#   -------------------------folder_1----------------------------------------------------
Business_mobile,
Institutional_Smart_Banking_3,
Institutional_Support_4,

# Rate_of_interest, not scraped
MEAN_INTEREST_RATE_FOR_LOAN_PRODUCT,
FD_INTEREST_RATES,
# Rate_of_interest, not scraped

recovery_agent,
ujjivansfb_business_current_account_Business_Edge_Current_Account,
ujjivansfb_business_current_account_Premium_Current_Account,
ujjivansfb_business_micro_loans,
ujjivansfb_business_net_banking,
ujjivansfb_business_regular_Current_Account,
ujjivansfb_corporate_Business_Edge_Current_Account,
ujjivansfb_corporate_current_account_tasc_account,
ujjivansfb_corporate_mse_loans,
ujjivansfb_corporate_Premium_Current_Account,
ujjivansfb_corporate_Regular_Current_Account,
ujjivansfb_corporate_salary_account,
ujjivansfb_in_business_mse_loans,
#   ---------------------------rural_products--------------------------------------------------
rural_deposits,
rural_micro,
rural_savings_account_Basic_Savings_Bank_Deposit_Account,
rural_savings_account_Digital_Account,
rural_savings_account_Minor_Savings_Account,
rural_savings_account_Privilege_Savings_Account,
rural_savings_account_Regular_Savings_Account,
rural_savings_account_Senior_Citizen_Savings_Account,
rural_agri_loans_Agri_Group_Loans,
rural_agri_loans_Kisan_Suvidha_Loan,
rural_home_loans,
rural_mse_loans,
rural_vehicle_loans_Electric_Three_Wheeler_Loan,
Two_Wheeler_Loan,
#   ---------------------------nri--------------------------------------------------
ujjivansfb_nri_savings_account_nre_fixed_deposits,
ujjivansfb_nri_savings_account_nre_saving_account,
ujjivansfb_nri_savings_account_nro_fixed_deposits,
ujjivansfb_nri_savings_account_nro_saving_account,
#   ---------------------------institutional products--------------------------------------------------
ujjivansfb_institutional_current_account,
ujjivansfb_institutional_deposits,
ujjivansfb_institutional_savings_account,
#   ---------------------------samyak-----------3rd doc not scraped from drive---------------------------------------
FD_Interest_Rates, #wrong json format
Form_Center,
Provide_Financial_Security_To_Your_Loved_Ones,#wrong json format
Sampoorna_Family_Banking,
Special_Assistance_during_Unforeseen_Events,
Terms_And_Conditions,
Third_Party_Product_Insurance,
Ujjivan_Bank_Also_Assists_During_Other_Crucial_Events,
About_Life_Events,
#   ---------------------------faqs--------------------------------------------------
FAQ_Business_Net_Banking,
FAQ_Current_Acount,
FAQ_Digital_Account,
FAQ_Fixed_Deposit,
FAQ_Home_Loan,
FAQ_IMPS,
FAQ_Missed_Call_Banking,
FAQ_Mobile_Banking,
FAQ_MSE,
FAQ_NEFT,
FAQ_Personal_Loans,
FAQ_Personal_Net_Banking,
FAQ_Recurring_Deposits,
FAQ_RTGS,
FAQ_Savings_Account,
FAQ_SMS_Banking,
#   ---------------------------about us--------------------------------------------------
Awards,
Board_of_Director,
Financial_Information,
Life_At_Ujjivan,
Management_Team,
Mission_And_Vision,
Profile,
#   ---------------------------calculator-------------------------------------------------
Fixed_Deposit_Calculator,
Recurring_Deposit_Calculator,
#   ---------------------------customer care-------------------------------------------------
Customer_Guidelines,
Customer_Service_Number,
Feedback_or_Complaint,
Grievance_Redressal_Business_Correspondents,
Grievance_Redressal_NEFT_RTGS,
Grievance_Redressal,
Senior_Citizen_Priority,
#   ---------------------------investor_relations-------------------------------------------------
Annual_Report,
Annual_Return,
Board_Committee,
Corporate_Governance_Policies,
DISCLOSURES_TO_STOCK_EXCHANGES,
Financial_Results,
Investor_Contacts,
MDs_Letters,
Presentations_and_Concall_Transcript,
Stock_Information,
#   ---------------------------regulatory-------------------------------------------------
Agreements,
Banking_Ombudsman,
Credit_Rating,
GST_Registration_Numbers,
List_of_Business_Correspondents,
List_of_DSAs,
MCLR_Rate,
Policies,
Privacy_Policy,
Regulatory_Disclosure_Section,
Security,
TIMELINES_FOR_CONVEYING_CREDIT_DECISIONS,
Unclaimed_Deposits_Latest,
Unclaimed_Deposits,
#   ---------------------------support-------------------------------------------------
ATMs,
Branches,
Cash_Deposit_Machines,
Locate_Us,
#   ---------------------------support-------------------------------------------------
Interviews,
Media_Contact,
Other_News,
Overview,
Press_Release,
Social_Media_Presence
        ]
documents = []

for json_file in json_files:
        # print('############', json_file, indent=4)
        with open(json_file, "r") as file:
                read_json = json.loads(file.read())
                documents.append(read_json)
               
                print("---------------------------",read_json,"\n \n")



# for url in url_list:
#     document = url_list[url]
#     document['url'] = url
#     document['filename'] = path + url.split('/')[-2] + '/index.html'
print("the documents length is----------",documents.__len__)

def main():
    for document in documents:
        print("^^^^^^^^^^^^^",document)
        rabbitmq_producer.publish(json.dumps(document).encode())


    

rabbitmq_producer = RabbitmqProducerPipe(
        publish_exchange="nlpEx",
        routing_key="nlp",
        queue_name='nlpQueue',
        host="localhost")


if __name__ == "__main__":
    main()
