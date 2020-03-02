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

        else:
            document['ES_RESULT']['DOCUMENTS'] = document['ES_RESULT']['DOCUMENTS'][0:250]


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

        if re.search(r'(home)',  document['PARSED_QUERY_STRING']):
            if re.search(r'(improv)', document['PARSED_QUERY_STRING']):

                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/home-improve/':
                        #print("Keyword found : {}".format('home loan improve'))

                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']

            elif re.search(r'(combo)', document['PARSED_QUERY_STRING']):

                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/ib-home-loan-combo/':
                        #print("Keyword found : {}".format('home loan combo'))

                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(plu)', document['PARSED_QUERY_STRING']):

                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/ib-home-loan-combo/':
                        #print("Keyword found : {}".format('home loan plus'))

                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            else:
                #print("Keyword found : {}".format('home loan'))

                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/ib-home-loan/':
                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        #-----------------------------------------------------------------------------------------------#

        # What is the eligibility for net banking?
        if re.search(r'(net)', document['PARSED_QUERY_STRING']):
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
        if re.search(r'(rates | rate | rent)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(lend)', document['PARSED_QUERY_STRING']):
                if re.search(r'(agricultur)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/lending-rates/':
                            documents.append(record)

                    documents += document['ES_RESULT']['DOCUMENTS']
                    print(json.dumps(documents[0:3], indent=4))
                else:
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://indianbank.in/departments/lending-rates-for-id/':
                            documents.append(record)

                    documents += document['ES_RESULT']['DOCUMENTS']
                    # print(json.dumps(documents[0:3], indent=4))
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/deposit-rates/":
                        if re.search(r'(interest)', record['stemmed_title']) and re.search(r'(term)', record['stemmed_title']):
                            documents.insert(0, record)
                    if record['url'] == "https://indianbank.in/service-charges-forex-rates/":
                        if re.search(r'(forex)', record['stemmed_title']) and  re.search(r'(card)', record['stemmed_title']):
                            documents.insert(0, record)
                        if record['title']=="" and record['value']=="Rent on Lockers":
                            documents.insert(0, record)
                    # else:
                    #     documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                # print(json.dumps(documents, indent=4))
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        #-----------------------------------------------------------------------------------------------#
        if re.search(r'(vehicl)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ib-vehicle-loan/":
                    documents.append(record)

            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

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

        if re.search(r'(shareholder)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/board-of-directors":
                    documents.append(record)

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

        if re.search(r'(educ)', document['QUERY_SYNONYMS']):

            if re.search(r'(jeevan vidya)', document['PARSED_QUERY_STRING']):

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

            else:
                for record in document['ES_RESULT']['DOCUMENTS']:

                    if record['url'] == "https://www.indianbank.in/departments/ib-educational-loan-prime/":
                        #print("Keyword found : {}".format('education loan'))
                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        document['ES_RESULT']['DOCUMENTS'] = document['ES_RESULT']['DOCUMENTS'][0:3]
        # document['ES_RESULT']['DOCUMENTS'] = document['ES_RESULT']['DOCUMENTS'][0:20]

        #-----------------------------------------------------------------------------------------------#

        return document
