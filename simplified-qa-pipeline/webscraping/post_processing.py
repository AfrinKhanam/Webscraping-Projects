import sys
import json

from webscraping.post_processing_functions import post_processing_functions

class PostProcessing():
    def __init__(self):
        self.url_to_function_mapper = {
            "https://www.indianbank.in/departments/ib-vehicle-loan/" : post_processing_functions.ib_vehicle_loan,
            "https://www.indianbank.in/departments/ib-home-loan/" : post_processing_functions.ib_home_loan,
            "https://www.indianbank.in/departments/ib-doctor-plus/" : post_processing_functions.ib_doctor_plus,
            "https://www.indianbank.in/departments/ind-sme-e-vaahan/" : post_processing_functions.ind_sme_e_vaahan,
            "https://www.indianbank.in/lending-rates/" : post_processing_functions.lending_rates,
            "https://www.indianbank.in/departments/ind-mortgage/" : post_processing_functions.ind_mortgage,
            "https://www.indianbank.in/departments/ib-clean-loan-to-salaried-class/" : post_processing_functions.clean_loan,

            "https://www.indianbank.in/departments/department/" : post_processing_functions.attach_email_subtitle,
            "https://www.indianbank.in/departments/executives/" : post_processing_functions.attach_email_subtitle,
            "https://www.indianbank.in/departments/foreign-branches/" : post_processing_functions.attach_email_subtitle,
            "https://www.indianbank.in/departments/head-office/" : post_processing_functions.attach_email_subtitle,
            "https://www.indianbank.in/departments/image-2/" : post_processing_functions.attach_email_subtitle,
            "https://www.indianbank.in/departments/nri-branches/" : post_processing_functions.attach_email_subtitle,
            "https://www.indianbank.in/departments/overseas-branches/" : post_processing_functions.attach_email_subtitle,
            "https://www.indianbank.in/departments/zonal-offices/" : post_processing_functions.attach_email_subtitle,
            "https://indianbank.in/departments/faq-for-amalgamation/":post_processing_functions.faq_for_amalgamation,
            "https://indianbank.in/departments/amalgamation-of-allahabad-bank-into-indian-bank/":post_processing_functions.amalgamation_ahamadabad_indian_bank,
            "https://indianbank.in/departments/sb-for-students-under-govt-scholarship-sb-for-dbt/":post_processing_functions.sb_for_students_under_govt_scholarship,
            "https://www.indianbank.in/departments/48601/":post_processing_functions.sb_for_central_state_govt,
            "https://indianbank.in/departments/mact-sb/":post_processing_functions.mact_sb,
            "https://indianbank.in/departments/ind-covid-emergency-credit-line-valid-till-30-9-20-only/":post_processing_functions.covid_emergency_credit_line, #corporate-covid
            "https://indianbank.in/departments/ca-for-state-central-govt-consular-ind-pfms/":post_processing_functions.ca_for_state_central_govt,
            }


    def post_processing(self, document):
        #print("00000000000000000000MEOW000000000000000")
        #print(document)
        #if document['url'] in self.url_to_function_mapper and document['post_processing'] == True:
        if document['url'] in self.url_to_function_mapper and document['post_processing'] == True :
            self.url_to_function_mapper[document['url']](document)
            #print("-----------------------------------------hey-------------------")
            return document
        else:
            #print("------------------ELSE---------------")
            return document
