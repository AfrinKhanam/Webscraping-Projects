import re
import numpy as np
import json
import base64
from nltk.stem import PorterStemmer


class ESPostProcessing:
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

        if re.search(r'(fix)', document['PARSED_QUERY_STRING']):

            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:

                if record['url'] == "https://www.indianbank.in/departments/fixed-deposit/":
                    documents.append(record)

            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:10]
            # print("***********************************8")
            # print(json.dumps(document,indent=4))
        elif re.search(r'(chief execut offic)', document['QUERY_SYNONYMS']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:

                if record['url'] == "https://www.indianbank.in/departments/managing-director-ceos-profile/":
                    documents.append(record)

            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:10]
        elif re.search(r'(agri clinic|agri busi)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/agri-clinic-and-agri-business-centres/":
                    documents.append(record)
            # print(json.dumps(documents,indent=4))

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
                    print("url--> ",record['url'])
                    if record['url'] == "https://www.indianbank.in/departments/loan-od-against-deposits/":
                        documents.append(record)
                print("hey")
                print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        else:
            document['ES_RESULT']['DOCUMENTS'] = document['ES_RESULT']['DOCUMENTS'][0:250]
# [0:250][]

# afrin
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
                    print(search_word, record['stemmed_main_title'], re.search(search_word, record['stemmed_main_title']))
                    if re.search(search_word, record['stemmed_main_title']):
                        documents.append(record)
                    else:
                        documents.
            else:
                documents.append(record)

        document['ES_RESULT']['DOCUMENTS'] = documents
        print("document['ES_RESULT']['DOCUMENTS'] :: ", document['ES_RESULT']['DOCUMENTS'])

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
        # print(json.dumps(document['ES_RESULT']['DOCUMENTS'],indent=4))
        # print(json.dumps(document[0:20],indent=4))
        '''
        "ib home loan"
        "ib home loan combo"
        "home improv loan"
        "home loan plu"
        '''

        #-----------------------------------------------------------------------------------------------#
        documents = []
        
        # What are the transactions possible using Ind Mobile Banking 
        if re.search(r'( mobil bank)', document['PARSED_QUERY_STRING']):
            print("hii")
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/ind-mobile-banking/':
                    print("hmmm")
                    documents.append(record)
            print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # What are the transactions possible using Ind Netbanking
        if re.search(r'(netbank)', document['PARSED_QUERY_STRING']):
            print("hii")
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/ind-netbanking/':
                    print("hmmm")
                    documents.append(record)
            print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        
        # What are the transactions possible using Ind Netbanking
        if re.search(r'(varishtha)', document['PARSED_QUERY_STRING']):
            print("hii")
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/ib-varishtha/':
                    print("hmmm")
                    documents.append(record)
            print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        
        # What are the Salient features of Indian Bank ATM/Debit cards?
        if re.search(r'(ATM/Debit cards)', document['QUERY_STRING']):
            print("hii")
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/atm-debit-cards/':
                    print("hmmm")
                    documents.append(record)
            print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        
         # For what purposes can the remittance be made via Money Gram
        if re.search(r'(money gram)', document['PARSED_QUERY_STRING']):
            print("hii")
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/money-gram/':
                    print("hmmm")
                    documents.append(record)
            print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        # What are the transactions possible using Ind Netbanking
        if re.search(r'(xpress money|inward remitt)', document['PARSED_QUERY_STRING']):
            print("hii")
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/xpress-money-inward-remittance-money-transfer-service-scheme/':
                    print("hmmm")
                    documents.append(record)
            print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

         # Who can transfer funds through Ind RTGS
        if re.search(r'(rtg|jet remit)', document['PARSED_QUERY_STRING']):
            print("hii")
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/ind-jet-remit-rtgs/':
                    print("hmmm")
                    documents.append(record)
            print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # When the funds transferred through NEFT are credited to the beneficiary's account
        if re.search(r'(neft)', document['PARSED_QUERY_STRING']):
            print("hii")
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/n-e-f-t/':
                    print("hmmm")
                    documents.append(record)
            print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        
        # What are the transactions possible using Ind Netbanking
        if re.search(r'(netbank)', document['PARSED_QUERY_STRING']):
            print("hii")
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/ind-mobile-banking/':
                    print("hmmm")
                    documents.append(record)
            print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(home)',  document['PARSED_QUERY_STRING']):

            if re.search(r'( nri | non resid indian )', document['PARSED_QUERY_STRING']):

                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/nri-home-loan/':
                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(improv)', document['PARSED_QUERY_STRING']):

                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/home-improve/':
                        #print("Keyword found : {}".format('home loan improve'))

                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


            elif re.search(r'(home secur|secur home)', document['PARSED_QUERY_STRING']):
                if re.search(r'(age)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/departments/ib-home-security-group-insurance-scheme-for-mortgage-borrowers-launch-in-association-with-kotak-mahindra-old-mutual-life-insurance-limited/' and record['stemmed_title']=='age group ':
                            #print("Keyword found : {}".format('home loan improve'))
                            documents.append(record)
                    print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                else:
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/departments/ib-home-security-group-insurance-scheme-for-mortgage-borrowers-launch-in-association-with-kotak-mahindra-old-mutual-life-insurance-limited/':
                            #print("Keyword found : {}".format('home loan improve'))
                            documents.append(record)
                    print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(combo)', document['PARSED_QUERY_STRING']):

                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/ib-home-loan-combo/':
                        #print("Keyword found : {}".format('home loan combo'))

                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


            elif re.search(r'(plu)', document['PARSED_QUERY_STRING']):

                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/ib-home-loan-plus/':
                        #print("Keyword found : {}".format('home loan plus'))

                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

           
            else:
                #print("Keyword found : {}".format('home loan'))
                if re.search(r'(valu ad)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/departments/ib-home-loan/' and re.search(r'(valu ad)', record['stemmed_title'])!=None:
                            documents.append(record)

                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                elif re.search(r'(document)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/departments/ib-home-loan/' and re.search(r'(document)', record['stemmed_title'])!=None:
                            documents.append(record)
                    # print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                else:
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/departments/ib-home-loan/':
                            if re.search(r'(repay)', record['stemmed_title']):
                                documents.insert(0, record)
                        documents.append(record)

                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(griha jeevan)', document['PARSED_QUERY_STRING']):

                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/ib-griha-jeevan-group-insurance-scheme-for-mortgage-borrowers-launched-in-association-with-lic/':
                        #print("Keyword found : {}".format('home loan combo'))

                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        #-----------------------------------------------------------------------------------------------#
        if re.search(r'(nre fd/rip/rd)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/nre-fd-rip-rd-accounts/':
                        print("hmmm")
                        documents.append(record)
                print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(nre fd/rip/rd)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/nre-fd-rip-rd-accounts/':
                        print("hmmm")
                        documents.append(record)
                print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'( pos )', document['QUERY_STRING']):
            documents=[]
            if re.search(r'(cash po)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/cash-at-pos/':
                        print("hmmm")
                        documents.append(record)
                print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                
            elif re.search(r'(surabhi)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-surabhi/":
                        print("herrrrrrrrllo")

                        documents.append(record)
                print(json.dumps(documents, indent=4))
                
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                if re.search(r'(target custom)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/departments/pos/' and record['stemmed_title']=='target custom ':
                            print("hmmm")
                            documents.append(record)
                    print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                else:
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/departments/pos/':
                            print("hmmm")
                            documents.append(record)
                    print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(nre sb)', document['PARSED_QUERY_STRING']):
            if str(re.search(r'(tax benefit)', document['PARSED_QUERY_STRING']))=='None' and re.search(r'(benefit)', document['PARSED_QUERY_STRING']):
                print("hiii")
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/nre-sb-accounts/' and record['stemmed_title']=='benefit ':
                        print("--------------")
                        documents.append(record)
                print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/nre-sb-accounts/':
                        print("hmmm")
                        documents.append(record)
                print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                
        # What is the eligibility for net banking?
        if re.search(r'(net)', document['PARSED_QUERY_STRING']):
            print("hey")
            if re.search(r'(internet)', document['PARSED_QUERY_STRING']):
                if re.search(r'(non financi)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://indianbank.in/departments/internet-banking/' and record['stemmed_title']=='non financi services account relat : ':
                            print("hmmm")
                            documents.append(record)
                    print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

                elif re.search(r'(financi)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://indianbank.in/departments/internet-banking/' and record['stemmed_title']=='financi services fund transfer within indian bank ':
                            print("hmmm")
                            documents.append(record)
                    print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

                else:
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://indianbank.in/departments/internet-banking/':
                            print("hmmm")
                            documents.append(record)
                    print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            else:
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ind-netbanking/":
                        if re.search(r'(elig)', record['stemmed_title']):
                            documents.insert(0, record)
                        elif re.search(r'(salient | featur)', record['stemmed_title']):
                            documents.insert(0, record)
                        else:
                            documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                # print(json.dumps(documents[0:3], indent=4))
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        
        # What is the min deposit required for the IB tax saver scheme?
        if re.search(r'(tax saver)', document['PARSED_QUERY_STRING']):
            if re.search(r'(min)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-tax-saver-scheme/" and record['stemmed_title']=='min amt ':
                        documents.append(record)
                print(json.dumps(documents, indent=4))
                
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-tax-saver-scheme/":
                        documents.append(record)
                print(json.dumps(documents, indent=4))
                
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # What are the features of Facility Deposits?
        if re.search(r'(facil deposit)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/facility-deposit/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            # print(json.dumps(documents[0:3], indent=4))
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(yatra suraksha|yatra)', document['PARSED_QUERY_STRING']):
            if re.search(r'(eligible)', document['QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-yatra-suraksha-with-uiic-ltd/" and record['stemmed_title']=='who is elig   ':
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                print(json.dumps(documents[0:3], indent=4))
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        
        if re.search(r'(cm plu)', document['PARSED_QUERY_STRING']):
            if re.search(r'(servic)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/cms-plus/" and record['stemmed_title']=='other requir detail ':
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                print(json.dumps(documents[0:3], indent=4))
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'( recur deposit | recur )', document['PARSED_QUERY_STRING']):
            print("--------->")
            documents = []
            if re.search(r'(variabl)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/variable-recurring-deposit/":
                        documents.append(record)
                print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                print("hiiiiiii")
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/recurring-deposit/":
                        documents.append(record)
                print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


        if re.search(r'(jeevan kalyan)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ib-jeevan-kalyan/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            # print(json.dumps(documents[0:3], indent=4))
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(capit gain)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/capital-gains/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            # print(json.dumps(documents[0:3], indent=4))
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # What is the rate of interest on IB contractors loan
        if re.search(r'(contractor)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'( interest | interest rate | rate)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-contractors/":
                        if re.search(r'(interest rate )', record['stemmed_title']):
                            print("meow")
                            documents.append(record)
                print(json.dumps(documents, indent=4))
                        
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-contractors/":
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                # print(json.dumps(documents[0:3], indent=4))
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

         # What is the eligibility for net banking?
        if re.search(r'(pension)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(central|download)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/centralized-pension-processing-system/":
                        documents.append(record)
                print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(calcul)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/centralized-pension-processing-system/" and record['stemmed_title']=='method of calcul of pension ':
                        documents.append(record)
                print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-pension-loan/":
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                # print(json.dumps(documents[0:3], indent=4))
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

          # What is the min amount limit for IB smart kid savings bank account
        if re.search(r'( smart kid | smart | kid )', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ib-smart-kid/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            # print(json.dumps(documents[0:3], indent=4))
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'( current | smart | kid )', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(suprem)', document['PARSED_QUERY_STRING']):
                print("hi")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/supreme-current-accounts/":
                        print("hello")
                        documents.append(record)
                print(json.dumps(documents, indent=4))
                
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(freedom)', document['PARSED_QUERY_STRING']):
                print("hi")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-i-freedom-current-account/":
                        print("hello")
                        documents.append(record)
                print(json.dumps(documents, indent=4))
                
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(premium)', document['PARSED_QUERY_STRING']):
                print("hiiiiiiiiiiiiiiiiii")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/premium-current-account/":
                        print("hello0000000000")
                        documents.append(record)
                print("-------------------------------------------")
                print(json.dumps(documents, indent=4))
                print("-------------------------------------------")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'( term | term and condit | term | t&c | condit )', document['PARSED_QUERY_STRING']):
                print("hiiiiiiiiiiiiiiiiii")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/important-terms-and-conditions-2/":
                        print("hello0000000000")
                        documents.append(record)
                print("-------------------------------------------")
                print(json.dumps(documents, indent=4))
                print("-------------------------------------------")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/current-account/":
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                # print(json.dumps(documents[0:3], indent=4))
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

          # I want to open a saving bank account with indian bank?
        if re.search(r'( save bank | save )', document['POTENTIAL_QUERY_LIST']):
            if re.search(r'( basic | deposit account )', document['PARSED_QUERY_STRING']):
                print("hi")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/vikas-savings-khata-a-no-frills-savings-bank-account/":
                        print("hello")

                        documents.append(record)
                print(json.dumps(documents, indent=4))
                
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(platinum)', document['PARSED_QUERY_STRING']):
                print("meow--->")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/sb-platinum/":
                        print("herrrrrrrrllo")

                        documents.append(record)
                print(json.dumps(documents, indent=4))
                
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(surabhi)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-surabhi/":
                        print("herrrrrrrrllo")

                        documents.append(record)
                print(json.dumps(documents, indent=4))
                
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(term | term | term and condit | term & condit )', document['PARSED_QUERY_STRING']):
                print("hi")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/important-terms-and-conditions/":
                        print("hello")

                        documents.append(record)
                print(json.dumps(documents, indent=4))
                
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

           
            else:
                print("hi")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/savings-bank/":
                        print("hello")

                        documents.append(record)
                print(json.dumps(documents, indent=4))
                
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

           # I want to open a saving bank account with indian bank?
        if re.search(r'( ib corp sb payrol packag | corp | payrol )', document['PARSED_QUERY_STRING']):
            print("hi")
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ib-corp-sb-payroll-package-scheme-for-salaried-class/":
                    print("hello")

                    documents.append(record)
            print(json.dumps(documents, indent=4))
            
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

         # What are the type of MSME loans offered by Indian Bank?
        if re.search(r'(msme | micro small & medium enterpris | micro small and medium enterpris)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(vehicl)',document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ind-msme-vehicle/":
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                # print(json.dumps(documents[0:3], indent=4))
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # # Anybody can apply for IB Micro loan?
        # if re.search(r'(micro)', document['PARSED_QUERY_STRING']):
        #     documents = []
        #     if re.search(r'(appli)',document['PARSED_QUERY_STRING']):
        #         for record in document['ES_RESULT']['DOCUMENTS']:
        #             if record['url'] == "https://www.indianbank.in/departments/ib-micro/" and record['stemmed_title']=='target group ':
        #                 documents.append(record)
        #         documents += document['ES_RESULT']['DOCUMENTS']
        #             # print(json.dumps(documents[0:3], indent=4))
        #         document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        #     else:
        #         print("else part")
        #         for record in document['ES_RESULT']['DOCUMENTS']:
        #             if record['url'] == "https://www.indianbank.in/departments/ib-micro/":
        #                 documents.append(record)
        #         print(json.dumps(documents, indent=4))
        #         documents += document['ES_RESULT']['DOCUMENTS']
        #         document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


        # What are the target groups for vidya mandir loan
        if re.search(r'( vidya mandir | vidya )', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ib-vidhya-mandir/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            # print(json.dumps(documents[0:3], indent=4))
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # What are the target groups for vidya mandir loan
        
        if re.search(r'(micro)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(appli)',document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-micro/" and record['stemmed_title']=='target group ':
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                    # print(json.dumps(documents[0:3], indent=4))
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'( servic charg | interest rate )',document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-micro/" and record['stemmed_title']=='servic charg ':
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                    # print(json.dumps(documents[0:3], indent=4))
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                print("else part")
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-micro/":
                        documents.append(record)
                print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

         # I am professional doctor Am I eligible to apply for IB doctor plus Loan
        if re.search(r'( doctor plu | doctor )', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ib-doctor-plus/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            # print(json.dumps(documents[0:3], indent=4))
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        
        if re.search(r'(collect plu)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(custom)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/ib-collect-plus-2/" and record['stemmed_title']=='target segment : ':
                        print("hi")
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(merchant)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/ib-collect-plus-2/" and record['stemmed_title']=='benefit to institution merch : ':
                        print("hi")
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(end user)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/ib-collect-plus-2/" and record['stemmed_title']=='benefit to remitters end user : ':
                        print("hi")
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/ib-collect-plus-2/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        
         # What is the maximum repayment tenture for IB My own Shop Loan
        if re.search(r'(shop)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/my-own-shop/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            # print(json.dumps(documents[0:3], indent=4))
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        
        if re.search(r'(vaahan)', document['PARSED_QUERY_STRING']):
            documents = []
            # print("0000000000>>>> ",document['POTENTIAL_QUERY_LIST'])
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ind-sme-e-vaahan/":
                    # print(record['url'],"--->",record['stemmed_title'],"---->",record['stemmed_title']=='target group ')
                    # print("**********************************")
                    documents.append(record)
            print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

       
         
        if re.search(r'( gc| gold card scheme )', document['PARSED_QUERY_STRING']):
            documents = []
            # print("0000000000>>>> ",document['POTENTIAL_QUERY_LIST'])
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ibex-gold-card-scheme-gcs-for-exporters/":
                    # print(record['url'],"--->",record['stemmed_title'],"---->",record['stemmed_title']=='target group ')
                    print("**********************************")
                    documents.append(record)
            print("00000000000000000000000000000000000000")
            print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(indpay)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(languag)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/indpay/" and record['stemmed_title']=='languag avail : ':
                        documents.append(record)
                print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/indpay/":
                        documents.append(record)
                print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

         # What is the interest rate for the Ind reverse mortgage loan?
        if re.search(r'(mortgag)', document['PARSED_QUERY_STRING']):
            if re.search(r'( revers mortgag )', document['PARSED_QUERY_STRING']):
                print("hii")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ind-reverse-mortgage/":
                        if re.search(r'(repay)', record['stemmed_title']):
                            documents.insert(0, record)
                        documents.append(record)
                # print("list-->",json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                # print(json.dumps(documents[0:3], indent=4))
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(griha jeevan)', document['PARSED_QUERY_STRING']):

                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/ib-griha-jeevan-group-insurance-scheme-for-mortgage-borrowers-launched-in-association-with-lic/':
                        #print("Keyword found : {}".format('home loan combo'))

                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(home security)', document['PARSED_QUERY_STRING']):
                print("hii")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-home-security-group-insurance-scheme-for-mortgage-borrowers-launch-in-association-with-kotak-mahindra-old-mutual-life-insurance-limited/":
                        documents.append(record)
                # print("list-->",json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                print("hii")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ind-mortgage/":
                        if re.search(r'(repay)', record['stemmed_title']):
                            documents.insert(0, record)
                        if re.search(r'(age criteria :)', record['stemmed_value']):
                            documents.insert(0, record)
                        documents.append(record)
                # print("list-->",json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                # print(json.dumps(documents[0:3], indent=4))
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

       



#  for record in document['ES_RESULT']['DOCUMENTS']:
#                 if record['url'] == "https://indianbank.in/departments/deposit-rates/":
#                     if re.search(r'(interest)', record['stemmed_title']) and re.search(r'(term)', record['stemmed_title']):
#                         documents.insert(0, record)
#                 if record['url'] == "https://indianbank.in/service-charges-forex-rates/":
#                     if re.search(r'(forex)', record['stemmed_title']) and  re.search(r'(card)', record['stemmed_title']):
#                         documents.insert(0, record)
#                     if record['title']=="" and record['value']=="Rent on Lockers":
#                         documents.insert(0, record)
#                 # else:
#                 #     documents.append(record)

#             documents += document['ES_RESULT']['DOCUMENTS']
#             print(json.dumps(documents, indent=4))
#             document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


# lending rate is not scraped
        # if re.search(r'(rates | rate | rent)', document['PARSED_QUERY_STRING']):
        # if re.search(r'(rates | rate )', document['PARSED_QUERY_STRING']):
        #     documents = []
        #     if re.search(r'(lend)', document['PARSED_QUERY_STRING']):
        #         if re.search(r'(agricultur)', document['PARSED_QUERY_STRING']):
        #             for record in document['ES_RESULT']['DOCUMENTS']:
        #                 if record['url'] == 'https://www.indianbank.in/lending-rates/':
        #                     documents.append(record)

        #             documents += document['ES_RESULT']['DOCUMENTS']
        #             print(json.dumps(documents[0:3], indent=4))
        #         else:
        #             for record in document['ES_RESULT']['DOCUMENTS']:
        #                 if record['url'] == 'https://indianbank.in/departments/lending-rates-for-id/':
        #                     documents.append(record)

        #             documents += document['ES_RESULT']['DOCUMENTS']
        #             # print(json.dumps(documents[0:3], indent=4))
        #     # "What is the interest rate on Loan/OD against Deposits? 
        #     elif re.search(r'(land deposit)', document['PARSED_QUERY_STRING']):

        #         documents = []
        #         for record in document['ES_RESULT']['DOCUMENTS']:
        #             if record['url'] == "https://www.indianbank.in/departments/loan-od-against-deposits/":
        #                 documents.append(record)
        #         # print(json.dumps(documents,indent=4))
        #         documents += document['ES_RESULT']['DOCUMENTS']
        #         document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        #     elif re.search(r'(subsidi)', document['PARSED_QUERY_STRING']):
        #         documents = []
        #         for record in document['ES_RESULT']['DOCUMENTS']:
        #             if record['url'] == "https://www.indianbank.in/departments/hindi-education-loan-interest-subsidies/":
        #                 documents.append(record)
        #         print(json.dumps(documents,indent=4))

        #         documents += document['ES_RESULT']['DOCUMENTS']
        #         document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        #     else:
        #         for record in document['ES_RESULT']['DOCUMENTS']:
        #             if record['url'] == "https://indianbank.in/departments/deposit-rates/":
        #                 if re.search(r'(interest)', record['stemmed_title']) and re.search(r'(term)', record['stemmed_title']):
        #                     documents.insert(0, record)
        #             if record['url'] == "https://indianbank.in/service-charges-forex-rates/":
        #                 if re.search(r'(forex)', record['stemmed_title']) and  re.search(r'(card)', record['stemmed_title']):
        #                     documents.insert(0, record)
        #                 if record['title']=="" and record['value']=="Rent on Lockers":
        #                     documents.insert(0, record)

        #         documents += document['ES_RESULT']['DOCUMENTS']
        #         print("=======================================")
        #         print(json.dumps(documents, indent=4))
        #         print("=======================================")
        #         document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        #-----------------------------------------------------------------------------------------------#
        if re.search(r'(forex)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://indianbank.in/service-charges-forex-rates/":
                    documents.append(record)
            print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        
        if re.search(r'(encash)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ib-rent-encash/":
                    documents.append(record)
            print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        
        if re.search(r'(shg|shg bank|shg bank linkag programm)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/shg-bank-linkage-programme-direct-linkage-to-shgs/":
                    documents.append(record)
            print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(vehicl)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ib-vehicle-loan/":
                    if re.search(r'(repay)', record['stemmed_title']):
                        documents.insert(0, record)
                    documents.append(record)

            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

      

        # What is IB clean loan (for salaried class)?
        if re.search(r'( clean | salari class )', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ib-clean-loan-to-salaried-class/":
                    documents.append(record)

            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(kcc | kisan credit card | kisan )', document['POTENTIAL_QUERY_LIST']):
            documents = []
            print("hi")
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/rupay-kisan-card/":
                    documents.append(record)
            # print(json.dumps(documents,indent=4))

            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(credit card)', document['PARSED_QUERY_STRING']):
            print("heeeeeeeeeeee")
            documents = []
            if re.search(r'(kcc | kisan credit card | kisan )', document['POTENTIAL_QUERY_LIST']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/rupay-kisan-card/":
                        documents.append(record)
                # print(json.dumps(documents,indent=4))

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                documents = []
                if re.search(r'(surabhi)', document['PARSED_QUERY_STRING']):
                    if re.search(r'()', document['POTENTIAL_QUERY_LIST']):
                        for record in document['ES_RESULT']['DOCUMENTS']:
                            if record['url'] == "https://www.indianbank.in/departments/ib-surabhi/":
                                documents.append(record)
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                else:
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        # https://indianbank.in/departments/credit-cards/
                        # https://www.indianbank.in/departments/credit-card/
                        if record['url'] == "https://www.indianbank.in/departments/credit-card/":
                            print("00000000000000000000000000000")
                            documents.append(record)
                    print(json.dumps(documents,indent=4))
                    print("9999999999999999999999999999")
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


        if re.search(r'(debit card)', document['PARSED_QUERY_STRING']):
            documents=[]
            print("-------hi---------")
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/debit-cards/":
                    documents.append(record)
                    print("99999999999999")
            print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            

            
        if re.search(r'(dri)', document['PARSED_QUERY_STRING']):
            documents = []
            print("hi")
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/dri-scheme-revised-norms/":
                    documents.append(record)
            # print(json.dumps(documents,indent=4))

            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        
        if re.search(r'(tradewel)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'( appli | elig | abil )', document['POTENTIAL_QUERY_LIST']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-tradewell/" and record['stemmed_title']=='target group ':
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-tradewell/":
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # --------------------------------------- Ind SME secure loan-------------------------
        # Who all can apply for Ind SME secure loan
        # if re.search(r'(secur)', document['PARSED_QUERY_STRING']):
        #     documents = []
        #     # print("0000000000>>>> ",document['POTENTIAL_QUERY_LIST'])
        #     for record in document['ES_RESULT']['DOCUMENTS']:
        #         if record['url'] == "https://www.indianbank.in/departments/ind-sme-secure/":
        #             # print(record['url'],"--->",record['stemmed_title'],"---->",record['stemmed_title']=='target group ')
        #             documents.append(record)
        #         # print(json.dumps(documents,indent=4))
        #     documents += document['ES_RESULT']['DOCUMENTS']
        #     document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        # --------------------------------------- Ind SME secure loan-------------------------


       

        

        # ---------------- indexing to quick contacts instead of indian bank mutual funds...............
#
        if re.search(r'(offic | contact)', document['PARSED_QUERY_STRING']):
            if re.search(r'(redempt)', document['PARSED_QUERY_STRING']):

                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/indian-bank-mutual-fund/":
                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(taxshield)', document['PARSED_QUERY_STRING']):

            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://indianbank.in/departments/indian-bank-mutual-fund/":
                    documents.append(record)

            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            # -----------------------------------------------------------------------------
        # if re.search(r'(fix)', document['QUERY_SYNONYMS']):

        #     documents = []
        #     for record in document['ES_RESULT']['DOCUMENTS']:

        #         if record['url'] == "https://www.indianbank.in/departments/fixed-deposit/":
        #             documents.append(record)

        #     documents += document['ES_RESULT']['DOCUMENTS']
        #     document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            # print(json.dumps(document,indent=4))
            # -------------------------------------------------------------------------------

        if re.search(r'(vision|mission)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/vision-and-mission/":
                    documents.append(record)

            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

# How to apply for door step banking
        if re.search(r'(door step|doorstep)', document['PARSED_QUERY_STRING']):
            documents = []
            count = 0
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/doorstep-banking/":
                    count+=1
                    documents.append(record)

            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            print(json.dumps(documents[0:count],indent=4))

      
        
        if re.search(r'(health|healthcare|health care|medi claim)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/universal-health-care-launched-in-association-with-uiic-ltd/":
                    documents.append(record)

            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        # agri clinic
       
    
        if re.search(r'(shareholder)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/board-of-directors":
                    documents.append(record)

            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

# for products-->tractors
        if re.search(r'(tractor|tractr|tract)', document['POTENTIAL_QUERY_LIST']):
            documents = []
            if re.search(r'(second hand |use|agriculturist)',document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/purchase-of-second-hand-pre-used-tractors-by-agriculturists/":
                        documents.append(record)
                # print("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%")
                # print(json.dumps(documents,indent=4))
                # print("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%")
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                

            elif re.search(r'(tie | sugar | mill)',document['PARSED_QUERY_STRING']):

                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/loans-for-maintenance-of-tractors-under-tie-up-with-sugar-mills/":
                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/financing-agriculturists-for-purchase-of-tractors/":
                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


        if re.search(r'(skill)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ib-skill-loan-scheme/":
                    documents.append(record)
            print(json.dumps(documents,indent=4))

            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(term deposit)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(foreclosur | charg )', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/deposit-rates/":
                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(senior citizen)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/deposit-rates/":
                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(platinum)', document['PARSED_QUERY_STRING']):
                print("heeeeeeeeeei")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/sb-platinum/":
                        print("hello")

                        documents.append(record)
                print(json.dumps(documents, indent=4))
                
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


            else:
                if re.search(r'(open)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == "https://www.indianbank.in/departments/terms-and-conditions-term-deposit-account/" and record['stemmed_title']=='account open : ':
                            documents.append(record)
                    print("----------")
                    print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                else:
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == "https://www.indianbank.in/departments/terms-and-conditions-term-deposit-account/":
                            documents.append(record)

                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(csi)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/central-scheme-to-provide-interest-subsidy-csis/":
                    documents.append(record)

            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        
        if re.search(r'(asba|applic support by block amount)', document['PARSED_QUERY_STRING']):
            documents = []
            print("hi")
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/applications-supported-by-blocked-amount/":
                    documents.append(record)
            print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(educ)', document['QUERY_SYNONYMS']):
            if re.search(r'( revis | iba )', document['PARSED_QUERY_STRING']):
                if re.search(r'(secur)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == "https://www.indianbank.in/departments/revised-iba-model-educational-loan-scheme-2015/" and record['stemmed_title']=='secur ':
                            documents.append(record)
                    print(json.dumps(documents,indent=4))

                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                else:
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == "https://www.indianbank.in/departments/revised-iba-model-educational-loan-scheme-2015/":
                            documents.append(record)
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(prime)', document['PARSED_QUERY_STRING']):
                print("hi")
                if re.search(r'(amount)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == "https://www.indianbank.in/departments/ib-educational-loan-prime/" and record['stemmed_title']=='amount of loan':
                            documents.append(record)
                    print(json.dumps(documents,indent=4))

                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                else:
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == "https://www.indianbank.in/departments/ib-educational-loan-prime/":
                            documents.append(record)
                    print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(subsidi)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/hindi-education-loan-interest-subsidies/":
                        documents.append(record)
                print(json.dumps(documents,indent=4))

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(jeevan vidya)', document['PARSED_QUERY_STRING']):

                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-balavidhya-scheme/":
                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(bal vidya)', document['PARSED_QUERY_STRING']):

                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-balavidhya-scheme/":
                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(vidyarthi suraksha |vidyarthi)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-vidyarthi-suraksha-with-pnb-metlife/":
                        documents.append(record)
                print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            else:
                for record in document['ES_RESULT']['DOCUMENTS']:

                    if record['url'] == "https://www.indianbank.in/departments/ib-educational-loan-prime/":
                        #print("Keyword found : {}".format('education loan'))
                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
              
            if re.search(r'( rfc | resident foreign currency )', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/resident-foreign-currency-account-for-returning-indians/':
                        print("hmmm")
                        documents.append(record)
                print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            

            
            

        document['ES_RESULT']['DOCUMENTS'] = document['ES_RESULT']['DOCUMENTS'][0:3]
        # document['ES_RESULT']['DOCUMENTS'] = document['ES_RESULT']['DOCUMENTS'][0:20]

        #-----------------------------------------------------------------------------------------------#

        return document
