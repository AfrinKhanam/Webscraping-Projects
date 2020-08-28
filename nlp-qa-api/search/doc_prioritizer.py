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
        if any(x in document['POTENTIAL_QUERY_LIST'] for x in ['godown','cold','storag']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                # print(json.dumps(record,indent=4))
                if record['url'] == 'https://www.indianbank.in/departments/agricultural-godowns-cold-storage/' or record['url'] == 'https://indianbank.in/departments/agricultural-godowns-cold-storage/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        
        if any(x in document['POTENTIAL_QUERY_LIST'] for x in ['tractor','farm']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/financing-agriculturists-for-purchase-of-tractors/':
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if any(x in document['POTENTIAL_QUERY_LIST'] for x in ['jewel']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/agricultural-jewel-loan-scheme/':
                    # print(record['url'])
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            # print(json.dumps(documents,indent=4))
        
        if any(x in document['POTENTIAL_QUERY_LIST'] for x in ['revers']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/ind-reverse-mortgage/':
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if any(x in document['POTENTIAL_QUERY_LIST'] for x in ['tradewel']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/ib-tradewell/':
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if any(x in document['POTENTIAL_QUERY_LIST'] for x in ['multipli']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/re-investment-plan/':
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]


        if any(x in document['POTENTIAL_QUERY_LIST'] for x in ['msme']):
            if not re.search(r'(gm)',document['POTENTIAL_QUERY_LIST']):
                documents = []
                if re.search(r'(vehicl)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        print(json.dumps(record, indent=4))
                        if record['url'] == "https://www.indianbank.in/departments/ind-msme-vehicle/":
                            documents.append(record)
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                else:
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == "https://www.indianbank.in/departments/ib-contractors-2/":
                            documents.append(record)
                    documents += document['ES_RESULT']['DOCUMENTS']
                    # print(json.dumps(documents[0:3], indent=4))
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        #---------------------LOANS-->CORPORATE--------------------------------------------------------------------------#
        

        if re.search(r'(work capit)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/working-capital/' or record['url'] == 'https://www.indianbank.in/departments/working-capital/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(term loan)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/term-loan/' or record['url'] == 'https://www.indianbank.in/departments/term-loan/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(bonu loan)', document['PARSED_QUERY_STRING']):
            if re.search(r'(elig)', document['POTENTIAL_QUERY_LIST']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if (record['url'] == 'https://indianbank.in/departments/bonus-loan/' and record['stemmed_title'].strip() == 'elig') or (record['url'] == 'https://www.indianbank.in/departments/bonus-loan/' and record['stemmed_title'].strip() == 'elig'):
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://indianbank.in/departments/bonus-loan/' or record['url'] == 'https://indianbank.in/departments/bonus-loan/':
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(covid emerg credit line)', document['PARSED_QUERY_STRING']):
            if re.search(r'(valid)', document['POTENTIAL_QUERY_LIST']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if (record['url'] == 'https://indianbank.in/departments/ind-covid-emergency-credit-line/' and record['stemmed_title'].strip() == 'valid') or (record['url'] == 'https://www.indianbank.in/departments/ind-covid-emergency-credit-line/' and record['stemmed_title'].strip() == 'valid'):
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://indianbank.in/departments/ind-covid-emergency-credit-line/' or record['url'] == 'https://www.indianbank.in/departments/ind-covid-emergency-credit-line/':
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(covid sahaya)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://indianbank.in/departments/shg-covid-sahaya-loan/" or record['url'] == "https:www.//indianbank.in/departments/shg-covid-sahaya-loan/":
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        #-----------------------------------------------------------------------------------------------#
        #--------------------------DEPOSIT PRODUCTS---------------------------------------------------------------------#
        if re.search(r'(sammaan)', document['PARSED_QUERY_STRING']):
            if re.search(r'(chequ)', document['POTENTIAL_QUERY_LIST']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if (record['url'] == 'https://indianbank.in/departments/ib-sammaan/' and record['stemmed_title'].strip() == 'chequ') or (record['url'] == 'https://www.indianbank.in/departments/ib-sammaan/' and record['stemmed_title'].strip() == 'chequ'):
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://indianbank.in/departments/ib-sammaan/' or record['url'] == 'https://www.indianbank.in/departments/ib-sammaan/':
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(mahila shakti women)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/ib-mahila-shakti-for-women/' or record['url'] == 'https://www.indianbank.in/departments/ib-mahila-shakti-for-women/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(kishor|save bank account minor)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/ib-kishore-savings-bank-account-for-minors/' or record['url'] == 'https://indianbank.in/departments/ib-kishore-savings-bank-account-for-minors/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(gen x|vibrant youth)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/ib-gen-x-for-the-vibrant-youth/' or record['url'] == 'https://www.indianbank.in/departments/ib-gen-x-for-the-vibrant-youth/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(salaam|special account defenc personnel)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/ib-salaam-special-account-for-defence-personnel/' or record['url'] == 'https://indianbank.in/departments/ib-salaam-special-account-for-defence-personnel/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(ib digi|onlin sb account)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/ib-digi-online-sb-account/' or record['url'] == 'https://indianbank.in/departments/ib-digi-online-sb-account/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(small account|small save)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/small-account/' or record['url'] == 'https://www.indianbank.in/departments/small-account/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(sb student govt scholarship & sb debit|sb student govt scholarship sb debit)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/sb-for-students-under-govt-scholarship-sb-for-dbt/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(sb central state govern consular offic & india pfm|sb central state govern consular offic and india pfm)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/48601/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(mact sb)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/mact-sb/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        #-----------------------------------------------------------------------------------------------#
        #-------------------------------------CURRENT ACCOUNT----------------------------------------------------------#
        if re.search(r'(comfort domest nre)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/ib-comfort-domestic-and-nre/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        #-----------------------------------------------------------------------------------------------#
        #-----------------------------------TERM DEPOSITS------------------------------------------------------------#
        if re.search(r'(short term deposit)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/short-term-deposits/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(macad|motor accid claim tribun deposit)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/motor-accident-claim-tribunal-depositmacad-scheme/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        #-----------------------------------------------------------------------------------------------#

        #----------------------------------COVID PRODUCTS-------------------------------------------------------------#
        if re.search(r'(mse covid emerg)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/ind-mse-covid-emergency-loan/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(covid emerg poultri|icepl)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/ind-covid-emergency-poultry-loan-icepl/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        #-----------------------------------------------------------------------------------------------#
        if re.search(r'(debebtur truste)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/debenture-trustee/#':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(sharehold pattern)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/shareholding-pattern/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'( mobil bank)', document['PARSED_QUERY_STRING']):
            if re.search(r'(comfort domest nre)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://indianbank.in/departments/ib-comfort-domestic-and-nre/':
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/ind-mobile-banking/':
                        documents.append(record)
                print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # What are the transactions possible using Ind Netbanking
        if re.search(r'(netbank)', document['PARSED_QUERY_STRING']):
            if re.search(r'(comfort domest nre)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://indianbank.in/departments/ib-comfort-domestic-and-nre/':
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/ind-netbanking/':
                        documents.append(record)
                print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(joint liabil group|jlg)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/joint-liability-group-jlg/':
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(standbi wc facil|wcdl)', document['PARSED_QUERY_STRING']):
            print("hii")
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/ib-standby-wc-facility-wcdl-for-msmes/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(varishtha)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/ib-varishtha/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(ib contractor|contractor)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/ib-contractors-2/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # What are the Salient features of Indian Bank ATM/Debit cards?
        if re.search(r'(ATM/Debit cards)', document['QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/atm-debit-cards/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

         # For what purposes can the remittance be made via Money Gram
        if re.search(r'(money gram)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/money-gram/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # What are the transactions possible using Ind Netbanking
        if re.search(r'(xpress money|inward remitt)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/xpress-money-inward-remittance-money-transfer-service-scheme/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(sm bank)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/sms-banking/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(atm debit)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/atm-debit-cards/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

         # Who can transfer funds through Ind RTGS \s+ is for space
        if re.search(r'(rtg\s+|\s+rtg\s+|\s+rtg|jet remit)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/ind-jet-remit-rtgs/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # When the funds transferred through NEFT are credited to the beneficiary's account
        if re.search(r'(neft)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/n-e-f-t/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # Who can apply for tradewell loan
       

        # What are the transactions possible using Ind Netbanking
        if re.search(r'(netbank)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/ind-mobile-banking/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(resid foreign currenc account|rfc)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/resident-foreign-currency-account-for-returning-indians/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # List of documents to be submitted for processing of the NRI plot loan application
        if re.search(r'(nri plot loan)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/nri-plot-loan/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(home loan)',  document['PARSED_QUERY_STRING']):
            if re.search(r'(nri|non resid indian)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/nri-home-loan/':
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(improv)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/home-improve/':
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(home secur)', document['PARSED_QUERY_STRING']):
                if re.search(r'(age)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/departments/ib-home-security-group-insurance-scheme-for-mortgage-borrowers-launch-in-association-with-kotak-mahindra-old-mutual-life-insurance-limited/' and record['stemmed_title'] == 'age group ':
                            documents.append(record)
                    # print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                else:
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/departments/ib-home-security-group-insurance-scheme-for-mortgage-borrowers-launch-in-association-with-kotak-mahindra-old-mutual-life-insurance-limited/':
                            documents.append(record)
                    # print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(combo)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/ib-home-loan-combo/':
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(plu)', document['PARSED_QUERY_STRING']):
                documents = []
                if re.search(r'(repay)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/departments/ib-home-loan-plus/' and re.search(r'(repay)', record['stemmed_title']) != None:
                            documents.append(record)
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                else:
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/departments/ib-home-loan-plus/':
                            documents.append(record)
                    # print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                if re.search(r'(valu ad)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/departments/ib-home-loan/' and re.search(r'(valu ad)', record['stemmed_title']) != None:
                            documents.append(record)
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                elif re.search(r'(document)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/departments/ib-home-loan/' and re.search(r'(document)', record['stemmed_title']) != None:
                            documents.append(record)
                    # print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                else:
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/departments/ib-home-loan/':
                            documents.append(record)
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(griha jeevan)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/ib-griha-jeevan-group-insurance-scheme-for-mortgage-borrowers-launched-in-association-with-lic/':
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        #-----------------------------------------------------------------------------------------------#
        if re.search(r'(nre fd/rip/rd)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/nre-fd-rip-rd-accounts/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(nre fd/rip/rd)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/nre-fd-rip-rd-accounts/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(chief vigil offic|cvo)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/cvo/':
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            document['ES_RESULT']['DOCUMENTS'] = document['ES_RESULT']['DOCUMENTS'][::-1]
            # print(json.dumps(document,indent=4))

        if re.search(r'(princip code complianc offic|nodal offic)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/nodal-officers/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
        
        if re.search(r'(bank execut)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://www.indianbank.in/departments/executives/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(director)', document['PARSED_QUERY_STRING']):
            if re.search(r'(execut|board|nomine|sharehold|non offici)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/board-of-directors/':
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                if re.search(r'(execut)', document['PARSED_QUERY_STRING']):
                    document['ES_RESULT']['DOCUMENTS'] = document['ES_RESULT']['DOCUMENTS'][::-1]

        if re.search(r'(standbi wc facil)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/ib-standby-wc-facility-wcdl-for-msmes/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # ['gm ',' gm','gm','cro','cdo','ra','clo',' bi','coo','r&gr','i&c']
        if re.search(r'((\sfgm$)|(^fgm\s)|(\sfgm\s)|(^fgm$)|(\sfgm\?$)|(\scro$)|(^cro\s)|(\scro\s)|(^cro$)|(\scro\?$)|(\sgm$)|(^gm\s)|(\sgm\s)|(^gm$)|(\sgm\?$)|(\sra$)|(^ra\s)|(\sra\s)|(^ra$)|(\sra\?$)|(\sclo$)|(^clo\s)|(\sclo\s)|(^clo$)|(\sclo\?$)|(\sbi$)|(^bi\s)|(\sbi\s)|(^bi$)|(\sbi\?$)|(\scoo$)|(^coo\s)|(\scoo\s)|(^coo$)|(\scoo\?$)|(\sr&gr$)|(^r&gr\s)|(\sr&gr\s)|(^r&gr$)|(\sr&gr\?$) |(\si&c$)|(^i&c\s)|(\si&c\s)|(^i&c$)|(\si&c\?$))',document['POTENTIAL_QUERY_LIST']):
            if not re.search(r'((\scvo$)|(^cvo\s)|(\scvo\s)|(^cvo$)|(\scvo\?$))',document['POTENTIAL_QUERY_LIST']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/general-managers/':
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                # document['ES_RESULT']['DOCUMENTS'] = document['ES_RESULT']['DOCUMENTS'][::-1]
                # print(json.dumps(document,indent=4))
        if re.search(r'((\scorpor credit$)|(^corpor credit\s)|(\scorpor credit\s)|(^corpor credit$)|(\scorpor credit\?$))',document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/corporate-credit-2/':
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            

        if re.search(r'(sb student govt scholarship & sb debit|sb student govt scholarship and sb debit)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/sb-for-students-under-govt-scholarship-sb-for-dbt/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(surya shakti)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/ind-surya-shakti/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(sme eas)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/ind-sme-ease/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(corpor govern|complianc report)', document['PARSED_QUERY_STRING']):
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == 'https://indianbank.in/departments/corporate-governance/' or record['url'] == 'https://www.indianbank.in/departments/corporate-governance/':
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'((\spo$)|(^po\s)|(\spo\s)|(^fpo$)|(\spo\?$)|(\spoint of sale$)|(^point of sale\s)|(\spoint of sale\s)|(^point of sale$)|(\spoint of sale\?$))', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(cash po)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/cash-at-pos/':
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(surabhi)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-surabhi/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                if re.search(r'(target custom)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/departments/pos/' and record['stemmed_title'] == 'target custom ':
                            documents.append(record)
                    # print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                else:
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/departments/pos/':
                            documents.append(record)
                    # print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(nre sb)', document['PARSED_QUERY_STRING']):
            if str(re.search(r'(tax benefit)', document['PARSED_QUERY_STRING'])) == 'None' and re.search(r'(benefit)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/nre-sb-accounts/' and record['stemmed_title'] == 'benefit ':
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/nre-sb-accounts/':
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # What is the eligibility for net banking?
        if re.search(r'(net)', document['PARSED_QUERY_STRING']):
            print("hi")
            if re.search(r'(internet bank)', document['PARSED_QUERY_STRING']):
                print("u")
                if re.search(r'(non financi)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://indianbank.in/departments/internet-banking/' and record['stemmed_title'] == 'non financi services account relat : ':
                            documents.append(record)
                    # print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

                elif re.search(r'(financi)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://indianbank.in/departments/internet-banking/' and record['stemmed_title'] == 'financi services fund transfer within indian bank ':
                            documents.append(record)
                    # print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

                else:
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == 'https://www.indianbank.in/departments/ind-netbanking/':
                            documents.append(record)
                    # print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                print("hello")
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
                    if record['url'] == "https://www.indianbank.in/departments/ib-tax-saver-scheme/" and record['stemmed_title'] == 'min amt ':
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-tax-saver-scheme/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
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
                    if record['url'] == "https://www.indianbank.in/departments/ib-yatra-suraksha-with-uiic-ltd/" and record['stemmed_title'] == 'who is elig   ':
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                # print(json.dumps(documents[0:3], indent=4))
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(cm plu)', document['PARSED_QUERY_STRING']):
            if re.search(r'(servic)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/cms-plus/" and record['stemmed_title'] == 'other requir detail ':
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                # print(json.dumps(documents[0:3], indent=4))
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'( recur deposit | recur )', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(variabl)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/variable-recurring-deposit/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/recurring-deposit/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
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

        if re.search(r'(annual report)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/annual-reports/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            # print(json.dumps(documents[0:3], indent=4))
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # What is the rate of interest on IB contractors loan
        if re.search(r'(contractor)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'( interest | interest rate | rate)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-contractors-2/":
                        if re.search(r'( interest rate )', record['stemmed_title']):
                            documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-contractors-2/":
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                # print(json.dumps(documents[0:3], indent=4))
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

         # What is the eligibility for net banking?
        if re.search(r'(pension)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(covid emerg pension)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/ind-covid-emergency-pension-loan/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(save bank account pension)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/savings-bank-account-for-pensioners/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(central|download)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/centralized-pension-processing-system/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(calcul)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/centralized-pension-processing-system/" and record['stemmed_title'] == 'method of calcul of pension ':
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
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
        if re.search(r'(smart kid)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ib-smart-kid/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            # print(json.dumps(documents[0:3], indent=4))
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(current account)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(suprem)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/supreme-current-accounts/":
                        documents.append(record)
                print(json.dumps(documents, indent=4))

                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(freedom)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-i-freedom-current-account/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(premium)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/premium-current-account/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'( term | term and condit | term | t&c | condit )', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/important-terms-and-conditions-2/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
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
        if re.search(r'(save bank|sb account|save account)', document['PARSED_QUERY_STRING']):
            if re.search(r'(corp sb|payrol packag scheme salari class)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-corp-sb-payroll-package-scheme-for-salaried-class/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(basic save bank deposit account|deposit account)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/vikas-savings-khata-a-no-frills-savings-bank-account/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(platinum)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/sb-platinum/":
                        # print("herrrrrrrrllo")
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(surabhi)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-surabhi/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(term | term | term and condit | term & condit )', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/important-terms-and-conditions/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/savings-bank/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

           # I want to open a saving bank account with indian bank?
        if re.search(r'(corp sb|payrol packag scheme salari class)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ib-corp-sb-payroll-package-scheme-for-salaried-class/":
                    documents.append(record)
            # print(json.dumps(documents, indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

         

        # What are the target groups for vidya mandir loan
        if re.search(r'(vidya mandir|vidya)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ib-vidhya-mandir/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(covid emerg agri proc)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://indianbank.in/departments/ind-covid-emergency-agro-proc-loan-iceapl/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(investor servic)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://indianbank.in/departments/investors-service/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(mutual fund)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/indian-bank-mutual-fund/" or record['url'] == "https://indianbank.in/departments/indian-bank-mutual-fund/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # What are the target groups for vidya mandir loan
        if re.search(r'(micro)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(appli)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-micro/" and record['stemmed_title'] == 'target group ':
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                   # print(json.dumps(documents[0:3], indent=4))
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'( servic charg | interest rate )', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-micro/" and record['stemmed_title'] == 'servic charg ':
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                   # print(json.dumps(documents[0:3], indent=4))
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-micro/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

         # I am professional doctor Am I eligible to apply for IB doctor plus Loan
        if re.search(r'(doctor plu|doctor loan)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://indianbank.in/departments/ib-doctor-plus/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(collect plu)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(custom)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/ib-collect-plus-2/" and record['stemmed_title'] == 'target segment : ':
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(merchant)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/ib-collect-plus-2/" and record['stemmed_title'] == 'benefit to institution merch : ':
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(end user)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/ib-collect-plus-2/" and record['stemmed_title'] == 'benefit to remitters end user : ':
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
        if re.search(r'(shop loan|my shop loan)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(repay)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/47556/" and record['stemmed_title'].strip() == 'repay term':
                        documents.append(record)
                # print(json.dumps(documents[0:3], indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/47556/":
                        documents.append(record)
                # print(json.dumps(documents[0:3], indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(vaahan)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://indianbank.in/departments/ind-sme-e-vaahan/":
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(sme mortgag)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(purpos)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ind-sme-mortgage-2/" and record['stemmed_title'] == 'purpos ':
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ind-sme-mortgage-2/":
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'( gc| gold card scheme )', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ibex-gold-card-scheme-gcs-for-exporters/":
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(v collect plu)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(languag)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-v-collect-plus-2/":
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if any(x in document['POTENTIAL_QUERY_LIST'] for x in ['indpay','indian bank app']):
            documents = []
            if re.search(r'(languag)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/indpay/" and record['stemmed_title'] == 'languag avail : ':
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/indpay/":
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

         # What is the interest rate for the Ind reverse mortgage loan?
        if re.search(r'(mortgag)', document['PARSED_QUERY_STRING']):
            if re.search(r'(revers mortgag)', document['PARSED_QUERY_STRING']):
                print("entering else if condition")
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ind-reverse-mortgage/":
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(griha jeevan)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == 'https://www.indianbank.in/departments/ib-griha-jeevan-group-insurance-scheme-for-mortgage-borrowers-launched-in-association-with-lic/':
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(home security)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-home-security-group-insurance-scheme-for-mortgage-borrowers-launch-in-association-with-kotak-mahindra-old-mutual-life-insurance-limited/":
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ind-mortgage/":
                        if re.search(r'(repay)', record['stemmed_title']):
                            documents.insert(0, record)
                        if re.search(r'(age criteria :)', record['stemmed_value']):
                            documents.insert(0, record)
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(lend rate|bplr)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://indianbank.in/lending-rates/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            # print(json.dumps(documents[0:3], indent=4))
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(servic charg forex rate|servic charg|forex rate)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://indianbank.in/service-charges-forex-rates/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            # print(json.dumps(documents[0:3], indent=4))
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        #-----------------------------------------------------------------------------------------------#
        if re.search(r'(forex)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://indianbank.in/service-charges-forex-rates/":
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(encash)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ib-rent-encash/":
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(shg|shg bank|shg bank linkag programm)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'(shg covid sahaya|covid sahaya)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/shg-covid-sahaya-loan/":
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/shg-bank-linkage-programme-direct-linkage-to-shgs/":
                        documents.append(record)
                # print(json.dumps(documents, indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(vehicl loan)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ib-vehicle-loan/":
                    if re.search(r'(repay)', record['stemmed_title']):
                        documents.insert(0, record)
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # What is IB clean loan (for salaried class)? |salari class|person
        if re.search(r'(clean)', document['POTENTIAL_QUERY_LIST']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/ib-clean-loan-to-salaried-class/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        # if re.search(r'(kcc | kisan credit card | kisan )', document['POTENTIAL_QUERY_LIST']):
        if re.search(r'(kcc|kisan credit card|kisan)', document['POTENTIAL_QUERY_LIST']):
            documents = []
            if re.search(r'(covid kcc sahaya loan|icksl)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/ind-covid-kcc-sahaya-loan-icksl/":
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/rupay-kisan-card/":
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(credit card)', document['PARSED_QUERY_STRING']):
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
                        if record['url'] == "https://www.indianbank.in/departments/credit-card/":
                            documents.append(record)
                    # print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(debit card)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/debit-cards/":
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(dri)', document['PARSED_QUERY_STRING']):
            documents = []
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/dri-scheme-revised-norms/":
                    documents.append(record)
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(tradewel)', document['PARSED_QUERY_STRING']):
            documents = []
            if re.search(r'( appli | elig | abil )', document['POTENTIAL_QUERY_LIST']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-tradewell/" and record['stemmed_title'] == 'target group ':
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-tradewell/":
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

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
            # -------------------------------------------------------------------------------
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
            # print(json.dumps(documents[0:count],indent=4))

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

        if re.search(r'(tractor|tractr|tract)', document['POTENTIAL_QUERY_LIST']):
            documents = []
            if re.search(r'(second hand |use|agriculturist)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/purchase-of-second-hand-pre-used-tractors-by-agriculturists/":
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            elif re.search(r'(tie | sugar | mill)', document['PARSED_QUERY_STRING']):
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
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(term deposit)', document['PARSED_QUERY_STRING']):
            if re.search(r'(domest)', document['PARSED_QUERY_STRING']):
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://indianbank.in/departments/deposit-rates/":
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/terms-and-conditions-term-deposit-account/":
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
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
            for record in document['ES_RESULT']['DOCUMENTS']:
                if record['url'] == "https://www.indianbank.in/departments/applications-supported-by-blocked-amount/":
                    documents.append(record)
            # print(json.dumps(documents,indent=4))
            documents += document['ES_RESULT']['DOCUMENTS']
            document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        if re.search(r'(educ)', document['QUERY_SYNONYMS']):
            if re.search(r'( revis | iba )', document['PARSED_QUERY_STRING']):
                if re.search(r'(secur)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == "https://www.indianbank.in/departments/revised-iba-model-educational-loan-scheme-2015/" and record['stemmed_title'] =='secur ':
                            documents.append(record)
                    print(json.dumps(documents, indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                else:
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == "https://www.indianbank.in/departments/revised-iba-model-educational-loan-scheme-2015/":
                            documents.append(record)
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(prime)', document['PARSED_QUERY_STRING']):
                if re.search(r'(amount)', document['PARSED_QUERY_STRING']):
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == "https://www.indianbank.in/departments/ib-educational-loan-prime/" and record['stemmed_title'] =='amount of loan':
                            documents.append(record)
                    # print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
                else:
                    for record in document['ES_RESULT']['DOCUMENTS']:
                        if record['url'] == "https://www.indianbank.in/departments/ib-educational-loan-prime/":
                            documents.append(record)
                    # print(json.dumps(documents,indent=4))
                    documents += document['ES_RESULT']['DOCUMENTS']
                    document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

            elif re.search(r'(subsidi)', document['PARSED_QUERY_STRING']):
                documents = []
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/hindi-education-loan-interest-subsidies/":
                        documents.append(record)
                # print(json.dumps(documents,indent=4))
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
                # print(json.dumps(documents,indent=4))
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]
            else:
                for record in document['ES_RESULT']['DOCUMENTS']:
                    if record['url'] == "https://www.indianbank.in/departments/ib-educational-loan-prime/":
                        documents.append(record)
                documents += document['ES_RESULT']['DOCUMENTS']
                document['ES_RESULT']['DOCUMENTS'] = documents[0:3]

        document['ES_RESULT']['DOCUMENTS'] = document['ES_RESULT']['DOCUMENTS'][0:3]
    # document['ES_RESULT']['DOCUMENTS'] = document['ES_RESULT']['DOCUMENTS'][0:20]

    #-----------------------------------------------------------------------------------------------#

        return document
