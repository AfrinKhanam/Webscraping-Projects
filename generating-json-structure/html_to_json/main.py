from building_blocks.html_to_json import HtmlToJson
from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
import json
import sys

path = "../../indian-bank-web-scraped-data/www.indianbank.in.2-Nov-2019/en/departments/"

filename_list = [
    # ---------------------------- SERVICE -------------------------------- #
    #"/mca-payment/index.html",
    #"/money-gram/index.html",
    #"/atm-debit-cards/index.html",
    #"/ind-mobile-banking/index.html",
    #"/ind-netbanking/index.html",
    #"/credit-cards/index.html",
    #"/xpress-money-inward-remittance-money-transfer-service-scheme/index.html",
    #"/n-e-f-t/index.html",
    #"/ind-jet-remit-rtgs/index.html",




    #"/multicity-cheque-facility/index.html",
    #"/ib-vidyarthi-suraksha-with-pnb-metlife/index.html",



    # NOT WORKING
    #"/ib-home-security-group-insurance-scheme-for-mortgage-borrowers-launch-in-association-with-kotak-mahindra-old-mutual-life-insurance-limited/index.html",
    #"/ib-home-security-group-insurance-scheme-for-mortgage-borrowers-launch-in-association-with-kotak-mahindra-old-mutual-life-insurance-limited/index.html",
    #"/ib-griha-jeevan-group-insurance-scheme-for-mortgage-borrowers-launched-in-association-with-lic/index.html",






    #"/universal-health-care-launched-in-association-with-uiic-ltd/index.html",
    #"/jana-shree-bima-yojana-launched-in-association-with-lic/index.html",
    #"/new-ib-jeevan-vidya-2/index.html",
    #"/ib-jeevan-kalyan/index.html",
    #"/ib-varishtha/index.html",
    #"/ib-chhatra/index.html",
    #"/ib-yatra-suraksha-with-uiic-ltd/index.html", 


    #"/cms-plus/index.html",



    #"/doorstep-banking/index.html",



    #"/e-payment-of-direct-taxes/index.html", 
    #"/e-payment-of-indirect-taxes/index.html", 
    #"/debenture-trustee/index.html",
    #---------------------------------------------------------------------- #



    # ---------------------------- AGRICULTURE ---------------------------- #
    #"/agricultural-godowns-cold-storage/index.html",
    #"/loans-for-maintenance-of-tractors-under-tie-up-with-sugar-mills/index.html",
    #"/agricultural-produce-marketing-loan/index.html",
    #"/financing-agriculturists-for-purchase-of-tractors/index.html",
    #"/purchase-of-second-hand-pre-used-tractors-by-agriculturists/index.html",
    #"/agri-clinic-and-agri-business-centres/index.html",
    #"/shg-bank-linkage-programme-direct-linkage-to-shgs/index.html",

    # table inside list
    #"/rupay-kisan-card/index.html",

    #"/dri-scheme-revised-norms/index.html",

    #table inside list
    #"/sugar-premium-scheme/index.html",

    #"/golden-harvest-scheme/index.html",

    #table inside list
    #"/agricultural-jewel-loan-scheme/index.html",
    # ---------------------------- ------- -------------------------------- #



    # ---------------------------- PERSONAL LOAN -------------------------- #
    #"/ib-rent-encash/index.html",
    #"/loan-od-against-deposits/index.html",
    #"/ib-clean-loan-to-salaried-class/index.html",
    #"/ib-balavidhya-scheme/index.html",
    #"/ind-reverse-mortgage/index.html",


    #(<u></u>)
    #"/ib-vehicle-loan/index.html",

    #"/ind-mortgage/index.html",

    #(<strong>Age </strong>)
    #"/ib-home-loan/index.html", 

    #"/ib-home-loan-combo/index.html",
    #"/ib-pension-loan/index.html",
    #"/home-improve/index.html",
    #"/ib-home-loan-plus/index.html", 
    #"/loan-od-against-nsc-kvp-relief-bonds-of-rbi-lic-policies/index.html",
    #---------------------------------------------------------------------- #


    #------------------------------ MSME ---------------------------------- #
    #"/ib-vidhya-mandir/index.html", 
    #"/my-own-shop/index.html",

    #(<u></u>)
    #"/ib-doctor-plus/index.html", 

    #"/ib-contractors/index.html",
    #"/ib-tradewell/index.html",
    #"/ind-sme-secure/index.html", 
    #"/ind-msme-vehicle/index.html",
    #"/ib-micro/index.html", 
    #"/ind-sme-mortgage/index.html", 

    #(<u></u>)
    #"/ind-sme-e-vaahan/index.html",
    #---------------------------------------------------------------------- #


    #----------------------------- EDUCATION ------------------------------ #
    #"/revised-iba-model-educational-loan-scheme-2015/index.html",
    #"/ib-educational-loan-prime/index.html",
    #"/ib-skill-loan-scheme/index.html",
    #"/hindi-education-loan-interest-subsidies/index.html",
    #---------------------------------------------------------------------- #


    #----------------------------- Saving Bank ------------------------------ #
    #"/savings-bank/index.html",
    #"/ib-corp-sb-payroll-package-scheme-for-salaried-class/index.html",
    #"/vikas-savings-khata-a-no-frills-savings-bank-account/index.html",
    #"/ib-smart-kid/index.html",
    #"/important-terms-and-conditions/index.html",
    #"/sb-platinum/index.html",

    #"/ib-surabhi",(**** different table pattern)
    #---------------------------------------------------------------------- #


    #----------------------------- CURRENT ACCOUNT -------------------------- #
    #"/supreme-current-accounts/index.html", (table)

    #"/current-account/index.html",
    #"/ib-i-freedom-current-account/index.html",
    #"/important-terms-and-conditions-2/index.html",
    #"/premium-current-account/index.html", 
    #------------------------------------------------------------------------ #


    #----------------------------- TERM DEPOSIT ----------------------------- #
    #"/facility-deposit/index.html",
    #"/capital-gains/index.html",
    #"/terms-and-conditions-term-deposit-account/index.html",
    #"/recurring-deposit/index.html",
    #"/ib-tax-saver-scheme/index.html",
    #"/fixed-deposit/index.html",
    #"/variable-recurring-deposit/index.html",
    #------------------------------------------------------------------------ #


    #----------------------------- TERM DEPOSIT ----------------------------- #
    #"/resident-foreign-currency-account-for-returning-indians/index.html",
    #"/nre-fd-rip-rd-accounts/index.html",
    #"/non-resident-ordinary-account/index.html",
    #"/fcnr-b-accounts/index.html",
    #------------------------------------------------------------------------ #

    # ---------------------------- DIGITAL PRODUCT ------------------------ #
    #"/pos/index.html",
    #"/cash-at-pos/index.html",

    #"/indpay/index.html",(****)
    #"/ib-collect-plus-2/index.html", (*** not working)
    #"/ib-v-collect-plus-2/index.html", (*** not working)
    #"/internet-banking/index.html", (*** table pattern different)
    #"/debit-cards/index.html", (*** div class different)

    #"/credit-card/index.html",
    #---------------------------------------------------------------------- #


    # ---------------------------- FEATURED PRODUCT ----------------------- #
    #"/applications-supported-by-blocked-amount/index.html",
    #"/n-r-i-foreign-exchange/index.html",


    #"/central-scheme-to-provide-interest-subsidy-csis/index.html", (*****)
    #"/centralized-pension-processing-system/index.html", (totally different pattern)
    #---------------------------------------------------------------------- #

    #----------------------------- RATES ---------------------------------- #
    #"/service-charges-forex-rates/index.html",
    #"/lending-rates/index.html",
    #---------------------------------------------------------------------- #



    # ---------------------------- ABOUT US ------------------------------- #
    #"/managing-director-ceos-profile/index.html",
    #"/banks-profile/index.html",
    #"/vision-and-mission/index.html",

    #"/executive-directors-profile/index.html", (***)
    #"/general-managers/index.html", (***)

    #"/chief-vigilance-officer/index.html", 


    #"/financial-results/index.html", (totally different)

    #"/corporate-governance/index.html", 
    #"/nodal-officers/index.html",
    #"/annual-general-meeting/index.html",
    #"/shareholding-pattern/index.html",
    #"/annual-reports/index.html",

    #"/indian-bank-mutual-fund/index.html", (*** totally different)

    #"/index.html",
    #---------------------------------------------------------------------- #

    # ---------------------------- CONTACT -------------------------------- #
    #"/quick-contact/index.html",

    #"/complaints-officers-list/index.html", (*** table structure )

    #"/chief-vigilance-officer/index.html",
    #"/death-claim/index.html",


    #"/head-office/index.html",
    #"/department/index.html", 


    #"/executives/index.html",  (** table )
    #"/image-2/index.html", (** table)
    #"/foreign-branches/index.html", (** table)
    #"/overseas-branches/index.html", (** table)
    #"/nri-branches/index.html", (** table)
    #"/zonal-offices/index.html", (** table)
    #"/e-confirmation-of-bank-guarantee/index.html",(** table)
    #---------------------------------------------------------------------- #

    # ---------------------------- RBI KEHTA HAI -------------------------- #
    #"/rbi-kehta-hain/index.html",
    # --------------------------------------------------------------------- #

]

#filename_list_board_of_director = ["../data/indian-bank/web-scraping-data/departments/board-of-directors/index.html"]
#filename_list = ["../data/indian-bank/web-scraping-data/departments/universal-health-care-launched-in-association-with-uiic-ltd/index_modified.html"]

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
# ----------------------------------------------------------- #

# ----------------------------------------------------------- #
with open(cash_pos_config_file, "r") as file:
    url_list = json.loads(file.read())
# ----------------------------------------------------------- #


# ----------------------------------------------------------- #
documents = []
for url in url_list:
    document = url_list[url]
    document['url'] = url
    document['filename'] = path + url.split('/')[-2] + '/index.html'

    documents.append(document)
# ----------------------------------------------------------- #

# ----------------------------------------------------------- #
def main():
    for document in documents:

        #---------------------------------------------------------------#
        try:
            print('---------------------------------------------------\n')
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
            rabbitmq_producer.publish(json.dumps(document['html_to_json']).encode())
            #---------------------------------------------------------------#

            print('---------------------------------------------------\n\n')
        except Exception as e:
            print('ERROR :: ', str(e))
        #---------------------------------------------------------------#



        input("Press key to continue") 

# ----------------------------------------------------------- #

# ----------------------------------------------------------- #
rabbitmq_producer = RabbitmqProducerPipe(
        publish_exchange="nlpEx",
        routing_key="nlp",
        queue_name='nlp',
        host="localhost")
# ----------------------------------------------------------- #

if __name__ == "__main__":
    main()
