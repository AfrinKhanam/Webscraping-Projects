import re
import numpy as np
import json
import base64
from nltk.stem import PorterStemmer


class DocPrioritizer:
    def __init__(self):
        self.score_size = 1
        self.ps = PorterStemmer()

        with open('./config_files/image_data.json') as file:
            self.image_data = file.read()

        self.image_data = json.loads(self.image_data)

        for record in self.image_data:
            record['Questions'] = re.sub(r'\?', '', record['Questions'])
            record['Questions'] = record['Questions'].lower()
            word_list = record['Questions'].split()
            word_list = [self.ps.stem((word)) for word in word_list]
            record['Questions'] = " ".join(word_list)

        #self.image_data = [self.image_data[11]]
        #print(json.dumps(self.image_data, indent=4) )

    def create_regex(self, string):
        # ------------------------------------- #
        word_list = string.split()
        # ------------------------------------- #

        # ------------------------------------- #
        regex = '('
        for word in word_list:
            regex = regex + word + '|'
        regex = regex + ')'
        regex = regex.replace('|)', ')')
        # ------------------------------------- #

        return regex

    def priortising_document(self, document):

        # ------ SELECT DOCUMENT CONTAING SEARCH KEYWORDS IN VALUE -- #
        '''
        documents = []
        regex = self.create_regex(document['PARSED_QUERY_STRING'])

        for record in document['ES_RESULT']['DOCUMENTS']:
            if re.search(regex, record['stemmed_value']):
                documents.append(record)

        document['ES_RESULT']['DOCUMENTS'] = documents
        '''
        # ----------------------------------------------------------- #

        # ----------------------------------------------------------- #
        self.calculate_word_count(document)
        self.calculate_word_score(document)
        # ----------------------------------------------------------- #

        # -------- PRIORTISE DOCUMENT BASED ON THE WORD SCORE ------- #
        es_result = document['ES_RESULT']['DOCUMENTS'].copy()
        documents = []
        word_score = self.sort_array_descending(document)

        for score in word_score:
            for idx, record in enumerate(es_result):
                if record['word_score'] == score:
                    documents.append(record)
                    del es_result[idx]

        document['ES_RESULT']['DOCUMENTS'] = documents
        # ----------------------------------------------------------- #

        # ------------ FILTERING  TOP 10 RESULT --------------------- #
        # print('___________________________________________________________________________\n')
        # print(word_score)
        # print(len(documents))
        #print('---------------- PRIORTISE DOCUMENT BASED ON THE WORD SCORE ---------------\n')
        #print(json.dumps(document['ES_RESULT']['DOCUMENTS'], indent=4))
        # print('---------------------------------------------------------------------------\n\n')

        if re.search(r'(fix deposit)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/fixed-deposit/" or record['url'] == "https://indianbank.in/departments/fixed-deposit/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:10]
            # print(json.dumps(document,indent=4))
        elif re.search(r'(chief execut offic)', document['QUERY_SYNONYMS']):
            # show me the MD & CEO message on amalgamation
            if re.search(r'(amalgam)', document['PARSED_QUERY_STRING']):
                document['ES_RESULT']['DOCUMENTS'] = document['ES_RESULT']['DOCUMENTS'][0:250]
            elif re.search(r'(annual gener meet)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/annual-general-meeting/' or record['url'] == 'https://indianbank.in/departments/annual-general-meeting/':
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/managing-director-ceos-profile/" or record['url'] == "https://indianbank.in/departments/managing-director-ceos-profile/":
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:10]

        elif re.search(r'(agri clinic|agri busi)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/agri-clinic-and-agri-business-centres/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        elif re.search(r'(harvest)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/golden-harvest-scheme/":
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

         # What is the salient feature of Loan/OD against Deposits?
        elif re.search(r'(land deposit)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                print("url--> ", record['url'])
                if record['url'] == "https://www.indianbank.in/departments/loan-od-against-deposits/" or record['url'] == "https://indianbank.in/departments/loan-od-against-deposits/":
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        else:
            document['ES_RESULT']['DOCUMENTS'] = document['ES_RESULT']['DOCUMENTS'][0:250]
# [0:250][]

        # document['ES_RESULT']['DOCUMENTS']  = document['ES_RESULT']['DOCUMENTS'][0:10]
        #print('---------------- PRIORTISE DOCUMENT BASED ON THE WORD SCORE ---------------\n')
        #print(json.dumps(document['ES_RESULT']['DOCUMENTS'], indent=4))
        # print('---------------------------------------------------------------------------\n\n')
        # ----------------------------------------------------------- #

        # -------- PRIORTISE DOCUMENT BASED ON THE WORD SCORE [1, <1, ...] -- #
        '''
        documents = []
        for record in document['ES_RESULT']['DOCUMENTS']:
            if record['word_score'] < 1:

                for search_word in record['word_match']:
                    print(search_word, record['stemmed_main_title'], re.search(
                        search_word, record['stemmed_main_title']))
                    if re.search(search_word, record['stemmed_main_title']):
                        documents.append(record)
                    else:
                        documents.
            else:
                documents.append(record)

        document['ES_RESULT']['DOCUMENTS'] = documents
        print("document['ES_RESULT']['DOCUMENTS'] :: ",
              document['ES_RESULT']['DOCUMENTS'])

        '''
        # ----------------------------------------------------------- #

        # ----------------------------------------------------------- #
        document['ES_RESULT']['WORD_COUNT'] = document['ES_RESULT']['DOCUMENTS'][0]['word_count']
        document['ES_RESULT']['WORD_SCORE'] = document['ES_RESULT']['DOCUMENTS'][0]['word_score']
        # ----------------------------------------------------------- #
        # print(json.dumps(document['ES_RESULT']['DOCUMENTS'],indent=4))
        # ------------ SENDING TOP 10 RESULT ------------------------ #
        document['ES_RESULT']['DOCUMENTS'] = document['ES_RESULT']['DOCUMENTS'][0:250]
        # document['ES_RESULT']['DOCUMENTS']  = document['ES_RESULT']['DOCUMENTS'][0:20]

        #print('------------------ PRIORTISE DOCUMENT BASED ON THE WORD SCORE != 1 ------\n')
        #print(json.dumps(document['ES_RESULT']['DOCUMENTS'], indent=4))
        # print('---------------------------------------------------------------------------\n\n')
        # print('___________________________________________________________________________\n')
        # ----------------------------------------------------------- #
        # print("document------------> ",json.dumps(document['ES_RESULT']['DOCUMENTS'],indent=4))
        return document

    def remove_unwanted_words(self, document):
        for record in document['ES_RESULT']['DOCUMENTS']:
            record['value'] = re.sub('View Profile', '', record['value'])

        return document

    def sort_array_descending(self, document):
        word_score = []
        # ------------------------------------------------ #
        for record in document['ES_RESULT']['DOCUMENTS']:
            word_score.append(record['word_score'])
        # ------------------------------------------------ #

        # ------------------------------------------------ #
        word_score = np.array(word_score)
        word_score.sort()
        word_score = word_score[::-1]
        # ----------------------------------------------- #

        return word_score

    def calculate_word_score(self, document):

        for record in document['ES_RESULT']['DOCUMENTS']:
            # ----------------------------------------------------------------------------- #
            record['word_score'] = 0
            record['word_match'] = []
            record['word_not_match'] = []
            # ----------------------------------------------------------------------------- #

            # ----------------------------------------------------------------------------- #
            chunk = ''
            chunk = chunk + record['stemmed_main_title'] + " "
            chunk = chunk + record['stemmed_title'] + " "
            chunk = chunk + record['stemmed_value'] + " "
            chunk = chunk + " ".join(record['inner_table_keys_stem']) + " "
            chunk = chunk + " ".join(record['inner_table_values_stem']) + " "
            chunk = set(chunk.split())
            # ----------------------------------------------------------------------------- #

            # ----------------------------------------------------------------------------- #
            for word_list_with_synonym in document['QUERY_SYNONYMS_DICT']:
                word_list = []
                for word in word_list_with_synonym:
                    word_list += word.split()

                word_list_with_synonym = set(word_list)
                #word_list_with_synonym = set(word_list_with_synonym)

                if chunk.intersection(word_list_with_synonym):
                    record['word_score'] += 1
                    record['word_match'] += list(
                        chunk.intersection(word_list_with_synonym))
                else:
                    record['word_not_match'] += list(
                        word_list_with_synonym.difference(chunk))

                # print('-----------------------------------------------------------\n')
                #print("word_list_with_synonym :: ", word_list_with_synonym)
                #print("chunk ::", chunk)
                #print("Intersection :: ", chunk.intersection(word_list_with_synonym))
                #print("Difference :: ", record['word_not_match'])
                # print('-----------------------------------------------------------\n\n')
            # ----------------------------------------------------------------------------- #

            # ----------------------------------------------------------------------------- #
            record['word_score'] = record['word_score'] / \
                len(document['QUERY_SYNONYMS_DICT'])
            # ----------------------------------------------------------------------------- #

        return document

    def calculate_word_count(self, document):
        # ------------------------------------------------- #
        for record in document['ES_RESULT']['DOCUMENTS']:
            word_count = len(record['value'].split())
            record['word_count'] = word_count
        # ------------------------------------------------- #

        return document

    def get_image(self, document):
        document['FILENAME'] = ''

        key = document['AUTO_CORRECT_QUERY']

        word_list = key.split()
        word_list = [self.ps.stem(word) for word in word_list]
        key = " ".join(word_list)

        #print('--------------> key : [{}] : '.format(key) )
        #print('--------------> autocorrect : [{}] : '.format(document['AUTO_CORRECT_QUERY']) )

        for record in self.image_data:
            question = record['Questions']

            #print('--------------> search'.format(re.search(key, question)) )

            if re.match(key, question):
                #print('--------------> record : [{}]'.format(question) )
                document['FILENAME'] = record['filename']
                break

        return document

    def give_high_weightage_to_document(self, document):
        # ------ SELECT DOCUMENT CONTAING SEARCH KEYWORDS IN VALUE -- #
        '''
        documents = []
        regex = self.create_regex(document['PARSED_QUERY_STRING'])

        for record in document['ES_RESULT']['DOCUMENTS']:
            if re.search(regex, record['stemmed_value']):
                documents.append(record)

        document['ES_RESULT']['DOCUMENTS'] = documents
        '''
        # ----------------------------------------------------------- #

        # ----------------------------------------------------------- #
        self.calculate_word_count(document)
        self.calculate_word_score(document)
        # ----------------------------------------------------------- #
       
        # -------- PRIORTISE DOCUMENT BASED ON THE WORD SCORE ------- #
        es_result = document['ES_RESULT']['DOCUMENTS'].copy()
        documents = []
        word_score = self.sort_array_descending(document)

        for score in word_score:
            for idx, record in enumerate(es_result):
                if record['word_score'] == score:
                    documents.append(record)
                    del es_result[idx]

        document['ES_RESULT']['DOCUMENTS'] = documents
        # ----------------------------------------------------------- #

        # ------------ FILTERING  TOP 10 RESULT --------------------- #
        # print('___________________________________________________________________________\n')
        # print(word_score)
        # print(len(documents))
        #print('---------------- PRIORTISE DOCUMENT BASED ON THE WORD SCORE ---------------\n')
        #print(json.dumps(document['ES_RESULT']['DOCUMENTS'], indent=4))
        # print('---------------------------------------------------------------------------\n\n')





        
##################################################################################################################################


        if re.search(r'(calcul)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    #print("url--> ",record['url'])
                    if record['url'] == "https://www.ujjivansfb.in/Ujjivan/":
                        documents.append(record)
                        print("30")
                        #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        else:
            print("error30")




        if re.search(r'(frequent ask question)', document['PARSED_QUERY_STRING']):
                print("exe27")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/faq":
                        documents.append(record)
                        print("27")
                #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        else:
            print("error0")



        if re.search(r'(mclr rate)', document['PARSED_QUERY_STRING']):
                print("exe1")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://ujjivansfb.in/assets/web_pdfs/20/original/MCLR_April_2020.pdf":
                        documents.append(record)
                        print("00")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error1")

##################################################################################################################################
        if re.search(r'(person net bank)', document['PARSED_QUERY_STRING']):
                print("exe2")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://netbanking.ujjivansfb.in/Ujjivan/":
                        documents.append(record)
                        print("01")
                #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error2")
            
############################################################################################################################################

        if re.search(r'(tasc fix deposit)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    #print("url--> ",record['url'])
                    if record['url'] == "https://www.ujjivansfb.in/institutional-deposits":
                        documents.append(record)
                        print("31")
                        #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        else:
            print("error31")


        if re.search(r'(fix deposit form)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    #print("url--> ",record['url'])
                    if record['url'] == "https://www.ujjivansfb.in/personal":
                        documents.append(record)
                        print("30")
                        #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        


        elif re.search(r'(fix deposit)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    #print("url--> ",record['url'])
                    if record['url'] == "https://www.ujjivansfb.in/personal-deposits#regular-fd":
                        documents.append(record)
                        print("31")
                        #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        else:
            print("error31")





#############################################################################################################################################
        if re.search(r'(agreement)', document['PARSED_QUERY_STRING']):
                print("exe3")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/agreements":
                        documents.append(record)
                        print("03")
                #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error3")


###########################################################################################################################################            

        if re.search(r'(custom servic)', document['PARSED_QUERY_STRING']):
                print("exe4")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/customer-service":
                        documents.append(record)
                        print("04")
                #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error4")


############################################################################################################################################

        if re.search(r'(two wheeler)', document['PARSED_QUERY_STRING']):
                print("exe5")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/personal-vehicle-loans#two-wheeler-loan":
                        documents.append(record)
                        print("05")
                #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error5")


        if re.search(r'(three wheeler)', document['PARSED_QUERY_STRING']):
                print("exe5")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/personal-vehicle-loans#three-wheeler-loan":
                        documents.append(record)
                        print("05")
                #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error5")


        if re.search(r'(tasc current account)', document['PARSED_QUERY_STRING']):
                print("exe5")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/corporate-current-account#tasc-account":
                        documents.append(record)
                        print("05")
                #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error5")

        if re.search(r'(digit account)', document['PARSED_QUERY_STRING']):
                print("exe5")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/rural-savings-account#digital-saving":
                        documents.append(record)
                        print("05")
                #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error5")


        if re.search(r'(schedul charg)', document['PARSED_QUERY_STRING']):
                print("exe5")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/service-charges-fees":
                        documents.append(record)
                        print("05")
                #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error5")

#################################################################################################################################

        if re.search(r'(construct purchas loan)', document['PARSED_QUERY_STRING']):
                print("exe6")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/personal-home-loans#construction-composite-loan":                       
                        documents.append(record)
                        print("06")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error6")
            

##################################################################################################################################

        if re.search(r'(home improv loan)', document['PARSED_QUERY_STRING']):
                print("exe6")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/personal-home-loans#home-improvement":
                        documents.append(record)
                        print("06")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error6")




##################################################################################################################################

        if re.search(r'(person loan)', document['PARSED_QUERY_STRING']):
                print("exe7")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/personal-loans#personal-loan":                    
                        documents.append(record)
                        print("07")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error7")
            

##################################################################################################################################

        if re.search(r'(term condit)', document['PARSED_QUERY_STRING']):
                print("exe8")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/terms-and-conditions":
                        documents.append(record)
                        print("08")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


        elif re.search(r'(term & condit)', document['PARSED_QUERY_STRING']):
                print("exe8")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/terms-and-conditions":
                        documents.append(record)
                        print("08")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]



        else:
            print("error8")

##################################################################################################################################

        if re.search(r'(individu loan)', document['PARSED_QUERY_STRING']):
                print("exe9")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/micro-loans#individual-loan":                  
                        documents.append(record)
                        print("09")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error9")


##########################################################################################################################################

            
        if re.search(r'(agri group loan form)', document['PARSED_QUERY_STRING']):
                print("exe10")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/form-center":
                        documents.append(record)
                        print("10")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


        elif re.search(r'(agri group loan)', document['PARSED_QUERY_STRING']):
                print("exe10")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/rural-agri-loans#agri-group-loan":
                        documents.append(record)
                        print("10")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

    


        else:
            print("error10")

##################################################################################################################################
        if re.search(r'(kisan pragati card)', document['PARSED_QUERY_STRING']):
                print("exe11 ")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/press-release":
                        documents.append(record)
                        print("11")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]



        else:
           print("error11")


        if re.search(r'(kisan suvidha)', document['PARSED_QUERY_STRING']):
                print("exe11 ")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/rural-agri-loans#kisan-suvidha-loan":
                        documents.append(record)
                        print("11")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]



        else:
           print("error11")



#############################################3333333##############################################################################

        if re.search(r'(kisan)', document['PARSED_QUERY_STRING']):
                print("exe12")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/kisan-kornar":                     
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error12")
        

##################################################################################################################################







##################################################################################################################################

        if re.search(r'(mobil bank)', document['PARSED_QUERY_STRING']):
                print("exe13")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/mobile-banking":
                        documents.append(record)
                        print("13")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error13")
            

##################################################################################################################################






##################################################################################################################################

        if re.search(r'(phone bank)', document['PARSED_QUERY_STRING']):
                print("exe14")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/phone-banking":
                        documents.append(record)
                        print("14")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error14")
            

###################################################################################################################################








###################################################################################################################################

        if re.search(r'(busi net bank)', document['PARSED_QUERY_STRING']):
                print("exe15")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/business-net-banking":
                        documents.append(record)
                        print("15")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error15")
            

#################################################################################################################################







#################################################################################################################################

        if re.search(r'(women leadership involv financi inclus )', document['PARSED_QUERY_STRING']):
                print("exe16")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/women-leaders-in-financial-inclusion":
                        documents.append(record)
                        print("16")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error16")
            

##################################################################################################################################

        """if re.search(r'(manag director ceo financi servic limit)', document['PARSED_QUERY_STRING']):
                print("exe17")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/our-initiatives":
                        documents.append(record)
                        print("17")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


        else:
            print("error17")"""







####################################################################################################################################

        if re.search(r'(corpor social respons)', document['PARSED_QUERY_STRING']):
                print("exe17")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/our-initiatives":
                        documents.append(record)
                        print("17")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


        else:
            print("error17")
            
############################################################################################################################################

        if re.search(r'(csr)', document['PARSED_QUERY_STRING']):
                print("exe17")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/our-initiatives":
                        documents.append(record)
                        print("17")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


        else:
            print("error17")

#################################################################################################################################









#################################################################################################################################

        if re.search(r'(mission)', document['PARSED_QUERY_STRING']):
                print("exe18")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/mission":
                        documents.append(record)
                        print("18")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error18")
            

###################################################################################################################################

        if re.search(r'(news)', document['PARSED_QUERY_STRING']):
                print("exe19")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/other-news":
                        documents.append(record)
                        print("19")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error19")


############################################################################################################################################



        if re.search(r'(corpor govern)', document['PARSED_QUERY_STRING']):
                print("exe20")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/corporate-governance-policies":
                        documents.append(record)
                        print("20")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error20")

    

############################################################################################################################################



        if re.search(r'(privaci polici)', document['PARSED_QUERY_STRING']):
                print("exe21")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/privacy-policy":
                        documents.append(record)
                        print("21")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error21")

        

###################################################################################################################################
        if re.search(r'(polici)', document['PARSED_QUERY_STRING']):
                print("exe21")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] =="https://www.ujjivansfb.in/policies":
                        documents.append(record)
                        print("21")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


        else:
            print("error22")  

####################################################################################################################################







###################################################################################################################################


    

###########################################################################################################################################



        if re.search(r'(mean interest rate)', document['PARSED_QUERY_STRING']):
                print("exe24")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/mean-interest-rate-for-loan-product":
                        documents.append(record)
                        print("24")
                #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error24")



############################################################################################################################################


        if re.search(r'(interest rate)', document['PARSED_QUERY_STRING']):
                print("exe25")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/support-interst-rates":
                        documents.append(record)
                        print("25")
                #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error25")



###################################################################################################################################

        if re.search(r'(locat)', document['PARSED_QUERY_STRING']):
                print("exe26")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/locate-us":
                        documents.append(record)
                        print("26")
                #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        elif re.search(r'(find branch)', document['PARSED_QUERY_STRING']):
                print("exe26")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/locate-us-branches":
                        documents.append(record)
                        print("26")
                #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


        elif re.search(r'(atm)', document['PARSED_QUERY_STRING']):
                print("exe26")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/locate-us-atm":
                        documents.append(record)
                        print("26")
                #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


        else:
            print("error26")


####################################################################################################################################

        if re.search(r'(compani secretari complianc offic)', document['PARSED_QUERY_STRING']):
                print("exe27")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/management-team":
                        documents.append(record)
                        print("27")
                #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]



        elif re.search(r'(chief busi offic)', document['PARSED_QUERY_STRING']):
                print("exe27")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/overview":
                        documents.append(record)
                        print("27")
                #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error27")


################################################################################################################################











######################################################################################################################################

        if re.search(r'(stock inform)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/stock-information":
                        documents.append(record)
                        print("28")
                        #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        else:
            print("error28")
            

######################################################################################################################################







######################################################################################################################################

        if re.search(r'(recur deposit form)', document['QUERY_SYNONYMS']):
                print("29")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/personal":
                        documents.append(record)
                        print("29")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


        else:
            print("error29")


####################################################################################################################################

        if re.search(r'(cfo)', document['QUERY_SYNONYMS']):
                print("29")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/management-team":
                        documents.append(record)
                        print("29")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


        else:
            print("error29")






###################################################################################################################################
            
        
            





#####################################################################################################################################


        if re.search(r'(head)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    #print("url--> ",record['url'])
                    if record['url'] == "https://www.ujjivansfb.in/management-team":
                        documents.append(record)
                        print("31")
                        #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        else:
            print("error31")

#####################################################################################################################################







####################################################################################################################################








###################################################################################################################################


        if re.search(r'(feedback)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                #print("url--> ",record['url'])
                    if record['url'] == "https://www.ujjivansfb.in/feedback-existing-customer":
                        documents.append(record)
                        print("32")
                    #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        else:
            print("error32")
            

#####################################################################################################################################

    

            
#####################################################################################################################################




        if re.search(r'(appli)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    #print("url--> ",record['url'])
                    if record['url'] == "https://www.ujjivansfb.in/personal":
                        documents.append(record)
                        print("30")
                        #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        else:
            print("error30")

#############################################################################################################################################



        if re.search(r'(offic)', document['PARSED_QUERY_STRING']):
                print("exe27")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.ujjivansfb.in/locate-us":
                        documents.append(record)
                        print("27")
                #print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        else:
            print("error32")





#############################################################################################################################################


# [0:250][]

# afrin
        # document['ES_RESULT']['DOCUMENTS']  = document['ES_RESULT']['DOCUMENTS'][0:10]
        #print('---------------- PRIORTISE DOCUMENT BASED ON THE WORD SCORE ---------------\n')
        #print(json.dumps(document['ES_RESULT']['DOCUMENTS'], indent=4))
        # print('---------------------------------------------------------------------------\n\n')
        # ----------------------------------------------------------- #

        # -------- PRIORTISE DOCUMENT BASED ON THE WORD SCORE [1, <1, ...] -- #
        """
        documents = []
        for record in document['ES_RESULT']['DOCUMENTS']:
            if record['word_score'] < 1:

                for search_word in record['word_match']:
                    print(search_word, record['stemmed_main_title'], re.search(search_word, record['stemmed_main_title']))
                    if re.search(search_word, record['stemmed_main_title']):
                        documents.append(record)
                    else:
                        documents.
            else:
                documents.append(record)

        document['ES_RESULT']['DOCUMENTS'] = documents
        print("document['ES_RESULT']['DOCUMENTS'] :: ", document['ES_RESULT']['DOCUMENTS'])

        """
        # ----------------------------------------------------------- #

        # ----------------------------------------------------------- #
        document['ES_RESULT']['WORD_COUNT'] = document['ES_RESULT']['DOCUMENTS'][0]['word_count']
        document['ES_RESULT']['WORD_SCORE'] = document['ES_RESULT']['DOCUMENTS'][0]['word_score']
        # ----------------------------------------------------------- #
        # print(json.dumps(document['ES_RESULT']['DOCUMENTS'],indent=4))
        # ------------ SENDING TOP 10 RESULT ------------------------ #
        document['ES_RESULT']['DOCUMENTS'] = document['ES_RESULT']['DOCUMENTS'][0:250]
        # document['ES_RESULT']['DOCUMENTS']  = document['ES_RESULT']['DOCUMENTS'][0:20]

        #print('------------------ PRIORTISE DOCUMENT BASED ON THE WORD SCORE != 1 ------\n')
        #print(json.dumps(document['ES_RESULT']['DOCUMENTS'], indent=4))
        # print('---------------------------------------------------------------------------\n\n')
        # print('___________________________________________________________________________\n')
        # ----------------------------------------------------------- #
        # print("document------------> ",json.dumps(document['ES_RESULT']['DOCUMENTS'],indent=4))
        return document
