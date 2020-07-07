import sys
from building_blocks.post_processing.post_processing_functions import post_processing_functions


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
        }


    def post_processing(self, document):
        if document['url'] in self.url_to_function_mapper and document['post_processing'] == True:
            self.url_to_function_mapper[document['url']](document)
            return document
        else:
            return document
