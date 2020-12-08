from nltk.stem import PorterStemmer                   
from nltk.tokenize import sent_tokenize, word_tokenize
from autocorrect import spell
from nltk.corpus import stopwords
from gingerit.gingerit import GingerIt
from search.autocorrect import w_autocorrect

import re

class QueryPreprocessor:
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
        f.close()

        # Creating list of synonyms words
        for idx, record in enumerate(self.synonyms_repo):
            self.synonyms_repo[idx] = re.split(r'=+', record)

        # Stemming words
        synonyms_repo = []

        for record in self.synonyms_repo:
            synonyms = []
            for word in record:
                stop_words = set(stopwords.words('english'))
                stop_word = stop_words.remove('can')
                stop_word = stop_words.remove('what')

                filtered_synonyms = [w.lower() for w in word.split() if not w in stop_words]
                filtered_synonyms = " ".join(filtered_synonyms)
                # print(f'filtered_synonyms ---> {filtered_synonyms}')
                stem_word = [self.ps.stem(x) for x  in filtered_synonyms.split()]
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

        #--------------------------------------------------------------------#
        self.parser = GingerIt()
        f = open("./config_files/corrections_text.txt", 'r')
        correct_word = {}

        for line in f:
            k, v = line.strip().split(':')
            correct_word[k.strip()] = v.strip()

        f.close()
        self.correct_word = correct_word
        #--------------------------------------------------------------------#

    def remove_hyphen(self,query):
        #query=query.replace('-',' ')
        # query['PARSED_QUERY_STRING'].replace('-',' ')
        # query['POTENTIAL_QUERY_LIST'].replace('-',' ')
        # query['AUTO_CORRECT_QUERY'].replace('-',' ')
        # query['CORRECT_QUERY'].replace('-',' ')

        return query

    def process(self, query_string):
        #--------------------------------------------------------------------#
        # preprocessedQuery= self.remove_hyphen(query_string)
        query_string = self.remove_unwanted_charater(query_string)
        #--------------------------------------------------------------------#

        #--------------------------------------------------------------------#
        #auto_correct_string = self.autocorrect(query_string)
        #auto_correct_string = self.ginger_autocorrection(query_string)

        try:
            auto_correct_string = w_autocorrect(query_string)
        except:
            auto_correct_string = query_string
        #--------------------------------------------------------------------#
        
        #--------------------------------------------------------------------#
        parsed_query_string = self.remove_stopword(auto_correct_string)
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
        # string = re.sub('-', ' ', string)

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
        stemmed_list=[]
        for w in word_tokens:
            stemmed_list.append(self.ps.stem(w))

        stemmed_string= " ".join(stemmed_list)
        word_tokens = word_tokenize(stemmed_string)
        #---------------------------------------------#

        #----------------------------------------------------------------------------------#
        filtered_sentence = []

        filtered_sentence_stemming = [w for w in word_tokens if not w in self.bag_of_words]
        filtered_sentence = [w for w in filtered_sentence_stemming if not w in stop_words]
        
        parsed_query_string = " ".join(filtered_sentence)
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
                auto_correct_string = (re.sub(key.lower(), value.lower(), auto_correct_string))
        #---------------------------------------------------------------#

        return auto_correct_string


    def add_synonym_to_input_query(self, query):
        #-----------------------------------------------------------#
        query_synonyms_dict = []

        for record in self.synonyms_repo:
            for word in record:
                #regex = '^' + word
                regex = r'\b' + word + r'\b'


                if len(query_synonyms_dict) == 0: 
                    if re.findall(regex, query) and len(word) != 0:
                    #if re.search(regex, query) and len(word) != 0:
                        query_synonyms_dict.append(record)
                else:
                    if re.findall(regex, query) and len(word) != 0:
                    #if re.search(regex, query) and len(word) != 0:
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
        #-----------------------------------------------------------#

        return  query_synonyms_dict, query_with_synonym

    def generate_potential_queries(self, parsed_query_string):
        #-------------------------------------------#
        parsed_query_words = parsed_query_string.split()
        parsed_query_words_copy = parsed_query_words.copy()
        #-------------------------------------------#

        #-------------------------------------------#
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
        potential_query_list = ['']
        temp_list = []
        for record in query_synonyms_dict:
            for word in record:
                for temp_word in potential_query_list:
                    temp_word = temp_word + ' ' + word
                    temp_list.append(temp_word)

            potential_query_list = temp_list
            temp_list = []
        #-------------------------------------------#

        return query_synonyms_dict, potential_query_list

    def add_synonym_to_words(self, parsed_query_string):
        #-------------------------------------------#
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
        return query_synonyms_dict
        #-------------------------------------------#

    def ginger_autocorrection(self, query_string):
        word_list = query_string.split()
        auto_correct_string = ''

        #---------------------------------------------------------------#
        for word in word_list:
            auto_correct_string += self.parser.parse(word)['result'] + ' '
        auto_correct_string = auto_correct_string.lower()
        #---------------------------------------------------------------#


        #---------------------------------------------------------------#
        for key,value in self.correct_word.items():

            if key.lower() in auto_correct_string.split():
                auto_correct_string = (re.sub(key.lower(), value.lower(), auto_correct_string))
        #---------------------------------------------------------------#

        return auto_correct_string

