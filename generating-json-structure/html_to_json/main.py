from building_blocks.html_to_json import HtmlToJson
from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
import json
import sys

path = "../../indian-bank-web-scraped-data/www.indianbank.in.1-Dec-2019/departments/"

# ----------------------------------------------------------- #

loan_product_path = "./config_files/products/loan/"

agriculture_config_file = loan_product_path + "agriculture.json"

personal_config_file = loan_product_path + 'personal.json'

msme_config_file = loan_product_path + 'msme.json'

education_config_file = loan_product_path + 'education.json'
# ----------------------------------------------------------- #

# ----------------------------------------------------------- #
deposit_product = "./config_files/products/deposit/"

saving_config_file = deposit_product + 'saving.json'

current_config_file = deposit_product + 'current.json'

term_config_file = deposit_product + 'term.json'

nri_config_file = deposit_product + 'nri.json'
# ----------------------------------------------------------- #

# ----------------------------------------------------------- #
featured_product = "./config_files/products/featured/"

featured_config_file = featured_product + "featured.json"
# ----------------------------------------------------------- #


# ----------------------------------------------------------- #
digital_product = "./config_files/products/digital/"

pos_config_file = digital_product + "pos.json"

cash_pos_config_file = digital_product + "cash@pos.json"
debit_card = digital_product + 'debit_card.json'
credit_card = digital_product + 'credit_card.json'
# ----------------------------------------------------------- #


# ----------------------------------------------------------- #
about_us = "./config_files/about_us/"

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
contacts = './config_files/contacts/'

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

json_files = [
    './config_files/hindi_config_files/agriculture.json'
    # agriculture_config_file,
    # personal_config_file,

    # msme_config_file

    # education_config_file,
    # saving_config_file,
    # current_config_file,
    # term_config_file,
    # nri_config_file,

    # # featured_config_file,

    # pos_config_file,
    # cash_pos_config_file,
    # debit_card,
    # credit_card,
    # bank_profile,
    # vision_mission,
    # managing_director_profile,
    # board_of_director,
    # cheif_vigilance_officer,
    # general_manager,
    # corporate_governance,
    # nodal_officers,
    # annual_general_meeting,
    # shareholding_pattern,
    # annual_reports,
    # image,
    # credit_cards,
    # mca_payment,
    # money_gram,
    # atm_debit_cards,
    # mobile_banking,
    # netbanking,
    # premium_credit_cards,
    # xpress_money,
    # neft,
    # rtgs,
    # multicity_cheque_facility,
    # vidyarthi_suraksha,
    # home_security,
    # universal_health_care,
    # jana_shree_bima_yojana,
    # new_jeevan_vidaya,
    # jeevan_kalyan,
    # varishtha,
    # chhatra,
    # griha_jeevan_group,
    # yatra_suraksha,
    # cms_plus,
    # doorstep_banking,
    # e_payment_direct_tax,
    # e_payment_indirect_tax,
    # debenture_trustee,
    # deposit_rates,
    # forex,
    # quick_contacts,
    # complaint_officer_list,
    # customer_complaints,
    # death_claim,
    # head_office,
    # department,
    # executives,
    # image_email,
    # foreign_branches,
    # overseas_branches,
    # nri_branches,
    # zonal_offices,
    # e_confirmation_bank_guarantee
]
print("length of json files-----------> ",len(json_files))
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

print("documents length is--------------",len(documents))

# ----------------------------------------------------------- #

# ----------------------------------------------------------- #

# ----------------------------------------------------------- #


def main():
    count=0

    for document in documents:
        count+=1
        print("document number is --------",count)
        #---------------------------------------------------------------#
        # try:
        # print('---------------------------------------------------\n')
        print("filename :: ", document['filename'])
        print("url :: ", document['url'])

        html_to_json = HtmlToJson(document, source='web')
        html_to_json.main_title(document)
        html_to_json.get_url(document)
        html_to_json.get_document_name(document)
        html_to_json.subtitles(document)
        html_to_json.content(document)
        html_to_json.post_processing(document)
        html_to_json.frame_json(document)

        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        print(json.dumps(document['html_to_json'], indent=4))
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        rabbitmq_producer.publish(json.dumps(
            document['html_to_json']).encode())
        #---------------------------------------------------------------#

        print('---------------------------------------------------\n\n')
        # input("Press key to continue")
        # except Exception as e:
        #print('ERROR :: ', str(e))
        #---------------------------------------------------------------#


# ----------------------------------------------------------- #
# ----------------------------------------------------------- #
# rabbitmq_producer = RabbitmqProducerPipe(
#     publish_exchange="nlpEx",
#     routing_key="nlp",
#     queue_name='nlp',
#     host="localhost")
rabbitmq_producer = RabbitmqProducerPipe(
    publish_exchange="nlpEx",
    routing_key="nlp",
    queue_name='nlpQueue',
    host="localhost")
# ----------------------------------------------------------- #

if __name__ == "__main__":
    main()
