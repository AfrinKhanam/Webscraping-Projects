
from building_blocks.html_to_json import HtmlToJson
from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
import json
import sys
import time
path = "../../indian-bank-web-scraped-data/www.indianbank.in.1-Dec-2019/departments/"

# ----------------------------------------------------------- #

loan_product_path = "./config_files/config_files_v2/loans/"

agriculture_config_file = loan_product_path + "agriculture.json"

personal_config_file = loan_product_path + 'personal_individual.json'

msme_config_file = loan_product_path + 'msme.json'

education_config_file = loan_product_path + 'education.json'

nri_config_file = loan_product_path + 'nri.json'
# ----------------------------------------------------------- #

# ----------------------------------------------------------- #
deposit_product = "./config_files/config_files_v2/deposits/"

saving_config_file = deposit_product + 'savings_bank.json'

current_config_file = deposit_product + 'current.json'

term_config_file = deposit_product + 'term.json'

depoit_nri_config_file = deposit_product + 'nri.json'
# ----------------------------------------------------------- #

# ----------------------------------------------------------- #
featured_product = "./config_files/config_files_v2/featured_products/"

featured_config_file = featured_product + "featured_products.json"
# ----------------------------------------------------------- #


# ----------------------------------------------------------- #
digital_product = "./config_files/config_files_v2/digital/"

pos_config_file = digital_product + "pos.json"

digital_cash_pos_config_file = digital_product + "cash@pos.json"
digital_debit_card = digital_product + 'debit_card.json'
digital_credit_card = digital_product + 'credit_card.json'
digital_ib_collect_plus = digital_product + 'ib_collect_plus.json'
digital_ib_v_collect_plus = digital_product + 'ib_v_collect_plus.json'
digital_sms_banking = digital_product + 'sms_banking.json'
# ----------------------------------------------------------- #


# ----------------------------------------------------------- #
about_us = "./config_files/config_files_v2/about_us/"

bank_profile = about_us + 'bank_profile.json'

vision_mission = about_us + "vision_mission.json"

management = about_us + "management/"
managing_director_profile = management + "management_director_ceo_profile.json"

board_of_director = management + "board_of_director.json"
cheif_vigilance_officer = management + "chief_vigilance_officer.json"
general_manager = management + "general_manager.json"


corporate_governance = about_us + "corporate_governance.json"
nodal_officers = about_us + "nodal_officers.json"
annual_general_meeting = about_us + "annual_general_meeting.json"
shareholding_pattern = about_us + "shareholding_pattern.json"
annual_reports = about_us + "annual_reports.json"
image = about_us + "image.json"
investors_service = about_us + "investors_service.json"
# ----------------------------------------------------------- #


# ----------------------------------------------------------- #
service = "./config_files/service/"

credit_cards = service + 'credit_cards.json'

premium = service + 'premium/'
mca_payment = premium + 'mca_payment.json'
money_gram = premium + 'money_gram.json'
atm_debit_cards = premium + 'atm_debit_cards.json'
mobile_banking = premium + 'mobile_banking.json'
netbanking = premium + 'netbanking.json'
premium_credit_cards = premium + 'credit_cards.json'
xpress_money = premium + 'xpress_money.json'
neft = premium + 'neft.json'
rtgs = premium + 'rtgs.json'
multicity_cheque_facility = premium + 'multicity_cheque_facility.json'
# ----------------------------------------------------------- #

# ----------------------------------------------------------- #
service = "./config_files/service/"
insurance = service + 'insurance/'

vidyarthi_suraksha = insurance + 'vidyarthi_suraksha.json'
home_security = insurance + 'home_security.json'
universal_health_care = insurance + 'universal_health_care.json'
jana_shree_bima_yojana = insurance + 'jana_shree_bima_yojana.json'
new_jeevan_vidaya = insurance + 'new_jeevan_vidaya.json'
jeevan_kalyan = insurance + 'jeevan_kalyan.json'
varishtha = insurance + 'varishtha.json'
chhatra = insurance + 'chhatra.json'
griha_jeevan_group = insurance + 'griha_jeevan_group.json'
yatra_suraksha = insurance + 'yatra_suraksha.json'


cms_plus = service + 'cms_plus.json'
doorstep_banking = service + 'doorstep_banking.json'
e_payment_direct_tax = service + 'e_payment_direct_tax.json'
e_payment_indirect_tax = service + 'e_payment_indirect_tax.json'
debenture_trustee = service + 'debenture_trustee.json'
# ----------------------------------------------------------- #

# ----------------------------------------------------------- #
rates = './config_files/rates/'
deposit_rates = rates + 'deposit.json'
lending = rates + 'lending.json'
forex = rates + 'forex.json'
# ----------------------------------------------------------- #

# ----------------------------------------------------------- #
contacts = './config_files/config_files_v2/contacts/'

quick_contacts = contacts + 'quick_contacts.json'

customer_support = contacts + 'customer_support/'
complaint_officer_list = customer_support + 'complaint_officer_list.json'
customer_complaints = customer_support + 'customer_complaints.json'
death_claim = customer_support + 'death_claim.json'


email = contacts + 'email/'
head_office = email + 'head_office.json'
department = email + 'department.json'
executives = email + 'executives.json'
image_email = email + 'image.json'
foreign_branches = email + 'foreign_branches.json'
overseas_branches = email + 'overseas_branches.json'
nri_branches = email + 'nri_branches.json'
zonal_offices = email + 'zonal_offices.json'
e_confirmation_bank_guarantee = email + 'e_confirmation_bank_guarantee.json'
# ----------------------------------------------------------- #

services_path = "./config_files/config_files_v2/services/"

services_credit_card = services_path + 'credit_cards.json'
services_doorstep_banking = services_path + 'doorstep_banking.json'
services_e_payment_indirect_tax = services_path + 'e_payment_indirect_tax.json'
services_debenture_trustee = services_path + 'debenture_trustee.json'
services_e_payment_direct_tax = services_path + 'e_payment_direct_tax.json'

services_atm_debit_cards = services_path + 'premium/' + 'atm_debit_cards.json'

services_atm_debit_cards = services_path + 'premium/' + 'atm_debit_cards.json'
services_ind_netbanking = services_path + 'premium/' + 'ind_netbanking.json'
services_money_gram = services_path + 'premium/' + 'money_gram.json'
services_neft = services_path + 'premium/' + 'neft.json'
services_ind_mobile_banking = services_path + \
    'premium/' + 'ind_mobile_banking.json'
services_mca_payment = services_path + 'premium/' + 'mca_payment.json'
services_multicity_cheque_facility = services_path + \
    'premium/' + 'multicity_cheque_facility.json'
services_rtgs = services_path + 'premium/' + 'rtgs.json'
services_xpress_money = services_path + 'premium/' + 'xpress_money.json'

services_chhatra = services_path + 'insurance/' + 'chhatra.json'
services_ib_home_security = services_path + \
    'insurance/' + 'ib_home_security.json'
services_jeevan_kalyan = services_path + 'insurance/' + 'jeevan_kalyan.json'
services_universal_health_care = services_path + \
    'insurance/' + 'universal_health_care.json'
services_vidyarthi_suraksha = services_path + \
    'insurance/' + 'vidyarthi_suraksha.json'
services_griha_jeevan = services_path + 'insurance/' + 'griha_jeevan.json'
services_jana_shree_bhima_yojana = services_path + \
    'insurance/' + 'jana_shree_bhima_yojana.json'
services_jeevan_vidya = services_path + 'insurance/' + 'jeevan_vidya.json'
services_jana_varishtha = services_path + 'insurance/' + 'varishtha.json'
services_yatra_suraksha = services_path + 'insurance/' + 'yatra_suraksha.json'

rates_path = "./config_files/config_files_v2/rates/"
rates_deposit = rates_path + 'deposit.json'
rates_forex = rates_path + 'forex.json'
rates_lending = rates_path + 'lending.json'

# -----------------------------------CORPORATE------------------------
corporate_path = './config_files/config_files_v2/loans/corporate/'
corporate_credit = corporate_path + 'corporate_credit.json'
working_capital = corporate_path + 'working_capital.json'
term_loan = corporate_path + 'term_loan.json'
corporate = corporate_path + 'corporate.json'
bonus_loan = corporate_path + 'bonus_loan.json'
covid_emergency_credit_line = corporate_path + 'covid_emergency_credit_line.json'
loans_against_lease_rentals = corporate_path + 'loans_against_lease_rentals.json'

# -----------------------------------SAVINGS BANK ACCOUNT------------------------
savings_bank_account_path = './config_files/config_files_v2/deposits/savings_bank_account/'
ib_sammaan = savings_bank_account_path + 'ib_sammaan.json'
mahila_shakti_for_women = savings_bank_account_path + 'mahila_shakti_for_women.json'
ib_kishore = savings_bank_account_path + 'ib_kishore.json'
ib_gen_x = savings_bank_account_path + 'ib_gen_x.json'
ib_salaam = savings_bank_account_path + 'ib_salaam.json'
sba_for_pensioners = savings_bank_account_path + 'sba_for_pensioners.json'
ib_digi = savings_bank_account_path + 'ib_digi.json'
small_account = savings_bank_account_path + 'small_account.json'
sb_for_students_under_govt_scholarship = savings_bank_account_path + \
    'sb_for_students_under_govt_scholarship.json'
sb_for_central_state_govt = savings_bank_account_path + \
    'sb_for_central_state_govt.json'
mact_sb = savings_bank_account_path + 'mact_sb.json'

# -----------------------------------CURRENT ACCOUNT------------------------
current_account_path = './config_files/config_files_v2/deposits/current_account/'
ib_comfort = current_account_path + 'ib_comfort.json'
ca_for_state_central_govt = current_account_path + 'ca_for_state_central_govt.json'
# --------------------------TERM DEPOSITS(DEPOSITS NEW)-------------------------------------------
term_deposits_path = './config_files/config_files_v2/deposits/term_deposits/'
money_mutiplier_deposits = term_deposits_path + 'money_mutiplier_deposits.json'
short_term_depoits = term_deposits_path + 'short_term_depoits.json'
motor_accident_claim = term_deposits_path + 'motor_accident_claim.json'
# ---------------------------COVID PRODUCTS-----------------------------------------------------------
covid_products_path = './config_files/config_files_v2/covid_products/'
covid_reassessment = covid_products_path + 'covid_reassessment.json'
emergency_credit_line = covid_products_path + 'emergency_credit_line.json'
ind_gecls_covid_19 = covid_products_path + 'ind_gecls_covid_19.json'
covid_emergency_salary_loan = covid_products_path + \
    'covid_emergency_salary_loan.json'
covid_emergency_pension_loan = covid_products_path + \
    'covid_emergency_pension_loan.json'
shg_covid_sahayak_loan = covid_products_path + 'shg_covid_sahayak_loan.json'
ind_covid_kcc_sahaya_loan = covid_products_path + 'ind_covid_kcc_sahaya_loan.json'
iceapl = covid_products_path + 'iceapl.json'
ind_covid_emergency_poultry_loan = covid_products_path + \
    'ind_covid_emergency_poultry_loan.json'
ind_mse_covid_emergency_loan = covid_products_path + \
    'ind_mse_covid_emergency_loan.json'

# ---------------------------ALLAHABAD BANK-----------------------------------------------------------
allahabad_bank_path = './config_files/config_files_v2/about_us/allahabad/'
md_ceo_msg_on_allahabad_bank = allahabad_bank_path + \
    'md_ceo_msg_on_allahabad_bank.json'
faqs_for_amalgamation = allahabad_bank_path + 'faqs_for_amalgamation.json'
amalgamation_of_allahabad_into_indianbank = allahabad_bank_path + \
    'amalgamation_of_allahabad_into_indianbank.json'
e_allahabad_bank_corner = allahabad_bank_path + 'e_allahabad_bank_corner.json'

# --------------------------LINKS-------------------------------------------
links_path = './config_files/config_files_v2/links/'
online_services = links_path + 'online_services.json'
slbc_puducherry = links_path + 'slbc_puducherry.json'
colombo_branch = links_path + 'colombo_branch.json'
jaffna_branch = links_path + 'jaffna_branch.json'
image = links_path + 'image.json'

# --------------------------NEWS-------------------------------------------
news_path = './config_files/config_files_v2/news/'
notifications = news_path + 'notifications.json'
corporate_social_responsibility = news_path + \
    'corporate_social_responsibility.json'
news_letter = news_path + 'news_letter.json'
what_is_new = news_path + 'what_is_new.json'
press_releases = news_path + 'press_releases.json'
damodaran_committee_recommendations = news_path + \
    'damodaran_committee_recommendations.json'
remit_of_india = news_path + 'remit_of_india.json'

# --------------------------MSME (Loan)-------------------------------------------
msme_path = './config_files/config_files_v2/loans/msme/'
ib_standby_wc_facility_for_msmes = msme_path + \
    'ib_standby_wc_facility_for_msmes.json'
ind_surya_shakti = msme_path + 'ind_surya_shakti.json'
ind_sme_ease = msme_path + 'ind_sme_ease.json'
ib_doctor_plus = msme_path + 'ib_doctor_plus.json'
ind_sme_e_vaahan = msme_path + 'ind_sme_e_vaahan.json'
agri_joint_liability_group = './config_files/config_files_v2/loans/' + \
    'agri_joint_liability_group.json'

json_files = [

    # --------------------------CORPORATE-------------------------------------------
    corporate_credit,
    working_capital,
    term_loan,
    corporate,
    bonus_loan,
    covid_emergency_credit_line,
    loans_against_lease_rentals,

    # --------------------------SAVINGS BANK ACCOUNT(DEPOSITS NEW)-------------------------------------------
    ib_sammaan,
    mahila_shakti_for_women,
    ib_kishore,
    ib_gen_x,
    ib_salaam,
    sba_for_pensioners,
    ib_digi,
    small_account,
    sb_for_students_under_govt_scholarship,
    sb_for_central_state_govt,
    mact_sb,
    # --------------------------CURRENT ACCOUNT(DEPOSITS NEW)-------------------------------------------
    ib_comfort,
    ca_for_state_central_govt,
    # --------------------------TERM DEPOSITS(DEPOSITS NEW)-------------------------------------------
    money_mutiplier_deposits,
    short_term_depoits,
    motor_accident_claim,
    # -------------------------COVID PRODUCTS-------------------------------------------
    covid_reassessment,
    emergency_credit_line,
    ind_gecls_covid_19,
    covid_emergency_salary_loan,
    covid_emergency_pension_loan,
    shg_covid_sahayak_loan,
    ind_covid_kcc_sahaya_loan,
    iceapl,
    ind_covid_emergency_poultry_loan,
    ind_mse_covid_emergency_loan,
    # --------------------------ALLAHABAD BANK-------------------------------------------
    md_ceo_msg_on_allahabad_bank,
    faqs_for_amalgamation,
    amalgamation_of_allahabad_into_indianbank,
    e_allahabad_bank_corner,
    # --------------------------LINKS-------------------------------------------
    online_services,

    # --------------------------MSME-------------------------------------------
    ib_standby_wc_facility_for_msmes,
    ind_sme_ease,
    ind_surya_shakti,
    ib_doctor_plus,
    ind_sme_e_vaahan,
    agri_joint_liability_group,


    investors_service,
    bank_profile,

    vision_mission,
    managing_director_profile,

    board_of_director,
    cheif_vigilance_officer,

    corporate_governance,
    nodal_officers,
    annual_general_meeting,
    shareholding_pattern,
    annual_reports,
    image,

    head_office,
    department,
    executives,
    image_email,
    foreign_branches,
    overseas_branches,
    nri_branches,
    zonal_offices,
    e_confirmation_bank_guarantee,

    death_claim,
    customer_complaints,
    quick_contacts,

    rates_deposit,
    rates_forex,
    services_doorstep_banking,
    services_e_payment_indirect_tax,
    services_debenture_trustee,
    services_e_payment_direct_tax,

    services_chhatra,
    services_ib_home_security,
    services_jeevan_kalyan,
    services_universal_health_care,
    services_vidyarthi_suraksha,
    services_griha_jeevan,
    services_jana_shree_bhima_yojana,
    services_jeevan_vidya,
    services_jana_varishtha,
    services_yatra_suraksha,

    agriculture_config_file,
    personal_config_file,

    msme_config_file,
    nri_config_file,

    saving_config_file,
    current_config_file,
    term_config_file,
    depoit_nri_config_file,

    pos_config_file,
    digital_cash_pos_config_file,
    digital_debit_card,
    digital_ib_collect_plus,
    digital_ib_v_collect_plus,
    digital_sms_banking,

    featured_config_file,
    services_credit_card,
    services_atm_debit_cards,
    services_ind_netbanking,
    services_money_gram,
    services_neft,
    services_xpress_money,
    services_ind_mobile_banking,
    services_mca_payment,
    services_multicity_cheque_facility,
    services_rtgs,

]
print("length of json files-----------> ", len(json_files))
documents = []
# ----------------------------------------------------------- #
for json_file in json_files:
    with open(json_file, "r") as file:
        url_list = json.loads(file.read())
        # print("url_list--------->",url_list,"\n")
        for url in url_list:
            # print("url--------->",url_list[url],"\n")
            document = url_list[url]
            # print("document--------->",document,"\n")

            document['url'] = url
            # print("document['url']--------->",document['url'],"\n")

            document['filename'] = path + url.split('/')[-2] + '/index.html'
            documents.append(document)
# ----------------------------------------------------------- #

print("documents length is--------------", len(documents))


def generate_json_structure(document):
    html_to_json = HtmlToJson(document, source='web')
    html_to_json.main_title(document)
    html_to_json.get_url(document)
    html_to_json.get_document_name(document)
    html_to_json.subtitles(document)
    html_to_json.content(document)
    html_to_json.post_processing(document)
    html_to_json.frame_json(document)

def main():
    for idx in range(len(documents)):
        print("document number is --------", idx)
        value = True
        while value:
            try:
                print("filename :: ", documents[idx]['filename'])
                print("url :: ", documents[idx]['url'])

                generate_json_structure(documents[idx])
                #---------------------------------------------------------------#

                #---------------------------------------------------------------#
                print(json.dumps(documents[idx]['html_to_json'], indent=4))
                #---------------------------------------------------------------#

                #---------------------------------------------------------------#
                rabbitmq_producer.publish(json.dumps(
                    documents[idx]['html_to_json']).encode())
                #---------------------------------------------------------------#

                print('---------------------------------------------------\n\n')
                time.sleep(5)
                #------`---------------------------------------------------------#
            except ConnectionError as e:
                print("filename :: ", documents[idx]['filename'])
                print("url :: ", documents[idx]['url'])
                print("Exception occurred..!! ",type(e),"\n\n Exception Message: ",e.args,"\n\n exception type is: ",e.__class__.__name__)
                time.sleep(5)
                continue
            except Exception as e:
                print("url,structure of the webpage is changed. Hence webpage is not scraped..!!")
                print("filename :: ", documents[idx]['filename'])
                print("url :: ", documents[idx]['url'])
            value = False

rabbitmq_producer = RabbitmqProducerPipe(
    publish_exchange="nlpEx",
    routing_key="nlp",
    queue_name='nlpQueue',
    host="localhost")
# ----------------------------------------------------------- #

if __name__ == "__main__":
    main()
