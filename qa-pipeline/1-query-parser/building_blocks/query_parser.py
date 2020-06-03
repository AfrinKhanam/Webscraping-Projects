from nltk.stem import PorterStemmer                   
from nltk.tokenize import sent_tokenize, word_tokenize
from autocorrect import spell
from nltk.corpus import stopwords
from gingerit.gingerit import GingerIt
from building_blocks.autocorrect import w_autocorrect

import re

class QueryParser:
    def __init__(self):
        #-------------- Initialise all custom stop words ---------------------#
        self.ps = PorterStemmer()
        self.bag_of_words = []
        f = open('./config_files/custom-stop-words.txt', 'r')
        for word in f:
            self.bag_of_words.append(self.ps.stem(word.split("\n")[0].lower()))
        f.close()
        #--------------------------------------------------------------------#

        #-------------- Initialise all synonyms -----------------------------#

        # Reading Stemmed file
        self.synonyms_repo = []
        f = open('./config_files/synonyms.txt', 'r')
        content = f.read()
        self.synonyms_repo = content.split('\n')
        # print("synonyms------------->\n",self.synonyms_repo[0])
        f.close()

        # Creating list of synonyms words
        for idx, record in enumerate(self.synonyms_repo):
            self.synonyms_repo[idx] = re.split(r'=+', record)
        # print(f"synonyms repo--->{self.synonyms_repo}")

        # Stemming words
        synonyms_repo = []

        for record in self.synonyms_repo:
            synonyms = []
            for word in record:
                stem_word = [self.ps.stem(x) for x  in word.split()]
                synonyms.append( " ".join(stem_word) )

            synonyms_repo.append(synonyms)

        self.synonyms_repo = synonyms_repo

        '''
        # Stemming words
        for outer_index, record in enumerate(self.synonyms_repo):
            stemmed_word_list = []
            for word in record:
                for w in word.split():
                    stemmed_word_list.append(self.ps.stem(w))

            self.synonyms_repo[outer_index] = stemmed_word_list

        '''



        #print(self.synonyms_repo)
        #--------------------------------------------------------------------#

        #--------------------------------------------------------------------#
        self.parser = GingerIt()
        f = open("./config_files/corrections_text.txt", 'r')
        correct_word = {}

        for line in f:
            k, v = line.strip().split(':')
            correct_word[k.strip()] = v.strip()
            #print (correct_word)

        f.close()
        self.correct_word = correct_word
        #print(correct_word)
        #--------------------------------------------------------------------#

    def remove_hyphen(self,query):
        #query=query.replace('-',' ')
        # query['PARSED_QUERY_STRING'].replace('-',' ')
        # query['POTENTIAL_QUERY_LIST'].replace('-',' ')
        # query['AUTO_CORRECT_QUERY'].replace('-',' ')
        # query['CORRECT_QUERY'].replace('-',' ')

        return query

    def parse(self, query_string):
        #--------------------------------------------------------------------#
        # preprocessedQuery= self.remove_hyphen(query_string)
        # print("after removing hyphen------> ",preprocessedQuery)
        query_string = self.remove_unwanted_charater(query_string)
        print(f'after removing unwanted characters {query_string}')
       
        #print("unwanted charater removed :: ", query_string)
        #--------------------------------------------------------------------#

        #--------------------------------------------------------------------#
        #auto_correct_string = self.autocorrect(query_string)
        #auto_correct_string = self.ginger_autocorrection(query_string)

        try:
            auto_correct_string = w_autocorrect(query_string)
            print('Autocorrected query :: ' , auto_correct_string)
        except:
            #print('!!!!!!!!!!!!!!!! unable to do autocorrect')
            auto_correct_string = query_string
        
        #print('Autocorrected query[new] :: ' , auto_correct_string)
        #--------------------------------------------------------------------#
        

        #--------------------------------------------------------------------#
        parsed_query_string = self.remove_stopword(auto_correct_string)
        print('After removing stop words :: ' , parsed_query_string)
        #--------------------------------------------------------------------#

        #--------------------------------------------------------------------#
        #print(self.add_synonym_to_words(parsed_query_string))
        #--------------------------------------------------------------------#

        #--------------------------------------------------------------------#
        query_synonyms_dict, potential_query_list = self.add_synonym_to_input_query(parsed_query_string)

        #query_synonyms_dict, potential_query_list =  self.generate_potential_queries(parsed_query_string)
        #--------------------------------------------------------------------#

        #--------------------------------------------------------------------#
        synonym_query = self.gen_query_with_synonym(query_synonyms_dict)
        #--------------------------------------------------------------------#

        return (parsed_query_string, query_synonyms_dict, potential_query_list, synonym_query, auto_correct_string)

    def remove_unwanted_charater(self, string):
        string = re.sub('(\?|@|#|$|\"|\'|%|\\|&|\*|\(|\)|-|\^|")', ' ', string)
        string = re.sub('\.', ' ', string)
        string = re.sub('/', ' ', string)
        return string


    def gen_query_with_synonym(self, query_synonyms_dict):
        #---------------------------------------------#
        synonym_query = ""
        for record in query_synonyms_dict:
            synonym_query = synonym_query + " ".join(record)
            synonym_query = synonym_query + " "
        #---------------------------------------------#

        #---------------------------------------------#
        #stemmed_list=[]
        #for word in synonym_query.split():
        #    stemmed_list.append(self.ps.stem(word))

        #stemmed_synonym_query = " ".join(stemmed_list)
        #---------------------------------------------#
        # print('query_synonyms_dict :: ', query_synonyms_dict)
        # print("QUERY WITH SYNONYMS :: ", synonym_query)
        # print("QUERY WITH SYNONYMS :: ", stemmed_synonym_query)

        return synonym_query


    def remove_stopword(self, query_string):
        #---------------------------------------------#
        query_string = query_string.lower()
        stop_words = set(stopwords.words('english'))

        stop_word = stop_words.remove('what')
        stop_word = stop_words.remove('can')


        word_tokens = word_tokenize(query_string)
        #---------------------------------------------#

        #---------------------------------------------#
        print(f'word_tokens --> {word_tokens}')
        stemmed_list=[]
        for w in word_tokens:
            stemmed_list.append(self.ps.stem(w))

        stemmed_string= " ".join(stemmed_list)
        word_tokens = word_tokenize(stemmed_string)
        print(f'stemmed word tokens --{word_tokens}')
        #---------------------------------------------#

        #----------------------------------------------------------------------------------#
        filtered_sentence = []

        filtered_sentence_stemming = [w for w in word_tokens if not w in self.bag_of_words]
        print(f'filtered_sentence_stemming ---> {filtered_sentence_stemming}')
        filtered_sentence = [w for w in filtered_sentence_stemming if not w in stop_words]
        print(f'filtered_sentence ---> {filtered_sentence}')
        
        parsed_query_string = " ".join(filtered_sentence)
        # print(f'parsed_query_string --> {parsed_query_string}')
        #----------------------------------------------------------------------------------#

        return parsed_query_string

    def autocorrect(self, query_string):
        word_list = query_string.split()
        auto_correct_string = ''
        for word in word_list:
            auto_correct_string += spell(word) + ' '
            
        auto_correct_string = auto_correct_string.lower()

        #---------------------------------------------------------------#
        for key,value in self.correct_word.items():

            if key.lower() in auto_correct_string.split():
                #print("key : {} value : {} autostring : {}".format(key, value, auto_correct_string))
                auto_correct_string = (re.sub(key.lower(), value.lower(), auto_correct_string))
                #print(auto_correct_string)
        #---------------------------------------------------------------#

        #print('auto correct string :: ', auto_correct_string)

        return auto_correct_string


    def add_synonym_to_input_query(self, query):
        #print("--> parsed query string : ",format(query) )

        #-----------------------------------------------------------#
        query_synonyms_dict = []

        for record in self.synonyms_repo:
            for word in record:
                #regex = '^' + word
                regex = r'\b' + word + r'\b'


                if len(query_synonyms_dict) == 0: 
                    if re.findall(regex, query) and len(word) != 0:
                    #if re.search(regex, query) and len(word) != 0:
                        #print('------> : word : ', word)
                        #print('------> : record : ', record)
                        query_synonyms_dict.append(record)
                else:
                    if re.findall(regex, query) and len(word) != 0:
                    #if re.search(regex, query) and len(word) != 0:
                        #print('\n\n')
                        #print('------> : word : ', word)
                        #print('------> : record : ', record)
                        #print('------> : query :', query)
                        #print('\n\n')
                        for element in query_synonyms_dict:
                            element = set(element)

                        if len( element.intersection( set(record) ) ) == 0: 
                            query_synonyms_dict.append(record)
        #-----------------------------------------------------------#


        #-----------------------------------------------------------#
        query_with_synonym = ''
        for record in query_synonyms_dict:
            query_with_synonym += " ".join(record) + ' ' 
        #-----------------------------------------------------------#




        #-----------------------------------------------------------#
        query_with_synonym = set(query_with_synonym.split())
        query = set(query.split())

        for word in query.difference(query_with_synonym):
            query_synonyms_dict.append([word])
        #-----------------------------------------------------------#



        #-----------------------------------------------------------#
        query_with_synonym = list(query_with_synonym)
        query_with_synonym = " ".join(query_with_synonym)

        query = list(query)
        query = " ".join(query)
        #-----------------------------------------------------------#

        #-----------------------------------------------------------#
        query_with_synonym +=  ' ' + query
        query_with_synonym = set(query_with_synonym.split())
        query_with_synonym = " ".join(query_with_synonym)
        #print('-------> query_with_synonyms : ', query_with_synonym)
        #-----------------------------------------------------------#

        #print('-------> query_synonyms_dict : ', query_synonyms_dict)



        return  query_synonyms_dict, query_with_synonym



    def generate_potential_queries(self, parsed_query_string):
        #-------------------------------------------#
        parsed_query_words = parsed_query_string.split()
        parsed_query_words_copy = parsed_query_words.copy()
        #-------------------------------------------#

        #-------------------------------------------#
        query_synonyms_dict = []
        #print("world list out of input query : {}".format(parsed_query_words))

        for query_word in parsed_query_words:
            for record in self.synonyms_repo:
                if record.count(query_word) != 0:
                    record = list(set(record))
                    query_synonyms_dict.append(record)

                    #print('\n\n')
                    #print('record ::', record)
                    #print('query_word :: ', [query_word])
                    #print('parsed word ::', parsed_query_words_copy)
                    #print('\n\n')

                    #if query_word not in parsed_query_words_copy:
                        #print("NOT IN LIST")
                    #else:
                        #print('IT IS IN LIST')

                    parsed_query_words_copy.remove(query_word)

        if len(parsed_query_words_copy) != 0:
            for word in parsed_query_words_copy:
                query_synonyms_dict.append([word])
        #-------------------------------------------#

        #-------------------------------------------#
        #print([x for x  in query_synonyms_dict if len(x) == 1])
        #print('QUERY DICT :::', query_synonyms_dict)
        #-------------------------------------------#

        #-------------------------------------------#
        potential_query_list = ['']
        temp_list = []
        for record in query_synonyms_dict:
            for word in record:
                for temp_word in potential_query_list:
                    temp_word = temp_word + ' ' + word
                    temp_list.append(temp_word)
            #print(temp_list)
            potential_query_list = temp_list
            temp_list = []
        #-------------------------------------------#

        #-------------------------------------------#


        #print('--------------------------------------------------')
        #print('PARSED QUERY STRING :: ', parsed_query_words)
        #print('QUERY(SYNONYMS) DICT :: ', query_synonyms_dict)
        #print('POTENTIAL QUERY :: ', potential_query_list)
        #print('--------------------------------------------------\n\n\n')
        #-------------------------------------------#

        return query_synonyms_dict, potential_query_list

    def add_synonym_to_words(self, parsed_query_string):
        #-------------------------------------------#
        #print('parsed_query_string == ', parsed_query_string)
        parsed_query_words = parsed_query_string.split()
        parsed_query_words_copy = parsed_query_words.copy()
        query_synonyms_dict = []

        for query_word in parsed_query_words:

            for record in self.synonyms_repo:
                if record.count(query_word) != 0:
                    record = list(set(record))
                    query_synonyms_dict.append(record)
                    parsed_query_words_copy.remove(query_word)

        if len(parsed_query_words_copy) != 0:
            for word in parsed_query_words_copy:
                query_synonyms_dict.append([word])
        #-------------------------------------------#
        
        #-------------------------------------------#
        #print(set(query_synonyms_dict))
        return query_synonyms_dict
        #-------------------------------------------#



        

    def ginger_autocorrection(self, query_string):
        word_list = query_string.split()
        auto_correct_string = ''

        #---------------------------------------------------------------#
        #print('parsing....')
        for word in word_list:
            auto_correct_string += self.parser.parse(word)['result'] + ' '
        auto_correct_string = auto_correct_string.lower()
        #---------------------------------------------------------------#


        #---------------------------------------------------------------#
        for key,value in self.correct_word.items():

            if key.lower() in auto_correct_string.split():
                #print("key : {} value : {} autostring : {}".format(key, value, auto_correct_string))
                auto_correct_string = (re.sub(key.lower(), value.lower(), auto_correct_string))
                #print(auto_correct_string)
        #---------------------------------------------------------------#

        return auto_correct_string

