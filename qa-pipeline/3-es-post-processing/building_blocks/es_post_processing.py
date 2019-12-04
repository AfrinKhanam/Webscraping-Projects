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
            word_list = [self.ps.stem( (word) ) for word in word_list]
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
        #print('___________________________________________________________________________\n')
        #print(word_score)
        #print(len(documents))
        #print('---------------- PRIORTISE DOCUMENT BASED ON THE WORD SCORE ---------------\n')
        #print(json.dumps(document['ES_RESULT']['DOCUMENTS'], indent=4))
        #print('---------------------------------------------------------------------------\n\n')

        document['ES_RESULT']['DOCUMENTS']  = document['ES_RESULT']['DOCUMENTS'][0:10]
        #print('---------------- PRIORTISE DOCUMENT BASED ON THE WORD SCORE ---------------\n')
        #print(json.dumps(document['ES_RESULT']['DOCUMENTS'], indent=4))
        #print('---------------------------------------------------------------------------\n\n')
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

        # ------------ SENDING TOP 10 RESULT ------------------------ #
        document['ES_RESULT']['DOCUMENTS']  = document['ES_RESULT']['DOCUMENTS'][0:10]

        print('------------------ PRIORTISE DOCUMENT BASED ON THE WORD SCORE != 1 ------\n')
        print(json.dumps(document['ES_RESULT']['DOCUMENTS'], indent=4))
        print('---------------------------------------------------------------------------\n\n')
        print('___________________________________________________________________________\n')
        # ----------------------------------------------------------- #
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
            chunk = chunk + record['stemmed_title']      + " "
            chunk = chunk + record['stemmed_value']      + " "
            chunk = chunk + " ".join(record['inner_table_keys_stem']) + " "
            chunk = chunk + " ".join(record['inner_table_values_stem'])      + " "
            chunk = set(chunk.split())
            # ----------------------------------------------------------------------------- #


            # ----------------------------------------------------------------------------- #
            for word_list_with_synonym in document['QUERY_SYNONYMS_DICT']:
                word_list = []
                for word in word_list_with_synonym:
                    word_list +=  word.split()


                word_list_with_synonym = set(word_list)
                #word_list_with_synonym = set(word_list_with_synonym)

                if chunk.intersection(word_list_with_synonym):
                    record['word_score'] += 1
                    record['word_match'] += list(chunk.intersection(word_list_with_synonym))
                else:
                    record['word_not_match'] += list(word_list_with_synonym.difference(chunk))

                #print('-----------------------------------------------------------\n')
                #print("word_list_with_synonym :: ", word_list_with_synonym)
                #print("chunk ::", chunk)
                #print("Intersection :: ", chunk.intersection(word_list_with_synonym))
                #print("Difference :: ", record['word_not_match'])
                #print('-----------------------------------------------------------\n\n')
            # ----------------------------------------------------------------------------- #

            # ----------------------------------------------------------------------------- #
            record['word_score'] = record['word_score']/len(document['QUERY_SYNONYMS_DICT'])
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

        print('--------------> key : [{}] : '.format(key) )
        #print('--------------> autocorrect : [{}] : '.format(document['AUTO_CORRECT_QUERY']) )

        for record in self.image_data:
            question = record['Questions']

            #print('--------------> search'.format(re.search(key, question)) )


            if re.match(key, question):
                print('--------------> record : [{}]'.format(question) )
                document['FILENAME'] = record['filename']
                break

        return document

    def give_high_weightage_to_document(self, document):
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
                        print("Keyword found : {}".format('home loan improve'))

                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']

            elif re.search(r'(combo)', document['PARSED_QUERY_STRING']):

                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/ib-home-loan-combo/':
                        print("Keyword found : {}".format('home loan combo'))

                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(plu)', document['PARSED_QUERY_STRING']):

                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/ib-home-loan-combo/':
                        print("Keyword found : {}".format('home loan plus'))

                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            else:
                print("Keyword found : {}".format('home loan'))

                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/ib-home-loan/':
                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        #-----------------------------------------------------------------------------------------------#




        #-----------------------------------------------------------------------------------------------#
        if re.search(r'(vehicl)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ib-vehicle-loan/":
                    documents.append(record)

            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


        if re.search(r'(vision|mission)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/vision-and-mission/":
                    documents.append(record)

            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(door step|doorstep)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/doorstep-banking/":
                    documents.append(record)

            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

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


        if re.search(r'(educ)', document['PARSED_QUERY_STRING']):

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
                        print("Keyword found : {}".format('education loan'))
                        documents.append(record)

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


        document['ES_RESULT']['DOCUMENTS'] = document['ES_RESULT']['DOCUMENTS'][0:3]
        #-----------------------------------------------------------------------------------------------#

        return document

