#from summarizer import SingleModel
import json
from nltk.tokenize import sent_tokenize, word_tokenize

from search.query_preprocessor import QueryPreprocessor
from search.elastic_search import Elastic
from search.doc_prioritizer import DocPrioritizer
from search.doc_title_prioritizer import DocTitlePrioritizer


class QAPipeline:
    def __init__(self, config):
        index = config.get('elastic_search_credentials', 'index')

        self.__query_preprocessor = QueryPreprocessor()
        self.__elastic = Elastic(index=index)
        self.__doc_prioritizer = DocPrioritizer()
        self.__doc_title_prioritizer = DocTitlePrioritizer()
        #self.__summarizer_model = SingleModel()

    def search(self, query: str, context: str):
        result = self.__preprocess_query({
            'CONTEXT': query.strip(),
            'QUERY_STRING': context.strip()
        })
        result = self.__get_elasticsearch_results(result)
        result = self.__prioritize_results(result)
        result = self.__summarize_text(result)
        result = self.__post_process_results(result)

        return result
    
    def __preprocess_query(self, query):
        context_string_length = len(query['CONTEXT'].split())

        #---------------------------------------------------------------#
        query["QUERY_STRING"] += ' ' + query['CONTEXT']
        
        parsed_query_string, query_synonyms_dict, potential_query_list, synonym_query, auto_correct_string = self.__query_preprocessor.process(query['QUERY_STRING'])
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        correct_string = auto_correct_string.split()
        correct_string = correct_string[0:len(correct_string) - context_string_length]
        correct_string = " ".join(correct_string)

        ret_val = {
            "QUERY_STRING" : query["QUERY_STRING"],
            "QUERY_SYNONYMS" : synonym_query,
            "QUERY_SYNONYMS_DICT" : query_synonyms_dict,
            "PARSED_QUERY_STRING" : parsed_query_string,
            "POTENTIAL_QUERY_LIST": potential_query_list,
            "AUTO_CORRECT_QUERY"  : auto_correct_string.strip(),
            "CORRECT_QUERY"  : correct_string,
        }

        return ret_val
        #---------------------------------------------------------------#

    def __get_elasticsearch_results(self, query):

        #---------------------------------------------------------------#
        es_result = self.__elastic.w_search(query)

        #---------------------------------------------------------------#
        if es_result == None:
            ret_val = {
                "QUERY_STRING"          : query["QUERY_STRING"],
                "PARSED_QUERY_STRING"   : query['PARSED_QUERY_STRING'],
                "POTENTIAL_QUERY_LIST"  : query["POTENTIAL_QUERY_LIST"],
                "QUERY_SYNONYMS"        : query['QUERY_SYNONYMS'],
                "QUERY_SYNONYMS_DICT"   : query['QUERY_SYNONYMS_DICT'],
                "CORRECT_QUERY"         : query['CORRECT_QUERY'],
                "AUTO_CORRECT_QUERY"    : query['AUTO_CORRECT_QUERY'],
                "ES_RESULT"             : { "DOCUMENTS" : []}
            }
        else:
            ret_val = {
                "QUERY_STRING"          : query["QUERY_STRING"],
                "PARSED_QUERY_STRING"   : query['PARSED_QUERY_STRING'],
                "POTENTIAL_QUERY_LIST"  : query["POTENTIAL_QUERY_LIST"],
                "QUERY_SYNONYMS"        : query['QUERY_SYNONYMS'],
                "QUERY_SYNONYMS_DICT"   : query['QUERY_SYNONYMS_DICT'],
                "CORRECT_QUERY"         : query['CORRECT_QUERY'],
                "AUTO_CORRECT_QUERY"    : query['AUTO_CORRECT_QUERY'],
                "ES_RESULT"             : { "DOCUMENTS" : es_result}
            }

        return ret_val
        #---------------------------------------------------------------#

    def __prioritize_results(self, document):
        #---------------------------------------------------------------#
        #------Prioritize Documents and perform other pre-procesing-----#
        #---------------------------------------------------------------#
        if len(document['ES_RESULT']['DOCUMENTS']) != 0:
            self.__doc_prioritizer.priortising_document(document)
            self.__doc_prioritizer.remove_unwanted_words(document)
            self.__doc_prioritizer.get_image(document)
            self.__doc_prioritizer.give_high_weightage_to_document(document)
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        #-------------Re-prioritize Documents by Main Title-------------#
        #---------------------------------------------------------------#
        if len(document['ES_RESULT']['DOCUMENTS']) != 0:
            self.__doc_title_prioritizer.MainTitlePrioritization(document)
        #---------------------------------------------------------------#

        return document

    def __summarize_text(self, from_es):
        # result_documents = from_es['ES_RESULT']['DOCUMENTS']

        # for idx, document in enumerate(result_documents):
        #     if len(document['value'].split()) > 75:
        #         result = self.__summarizer_model(document['value'])
        #         final_data = sent_tokenize(result)
        #         from_es['ES_RESULT']['DOCUMENTS'][idx]['value'] = ''.join(
        #             i.capitalize() for i in final_data)

        #     if len(document['inner_table_values']) > 0:
        #         document['value'] = ' : '.join(document['inner_table_values'])
        # # # #---------------------------------------------------------------#

        # if len(from_es['ES_RESULT']['DOCUMENTS']) > 0:
        #     from_es['WORD_COUNT'] = len(
        #         from_es['ES_RESULT']['DOCUMENTS'][0]['value'])

        return from_es
        #---------------------------------------------------------------#

    def __post_process_results(self, from_es):
        if len(from_es['ES_RESULT']['DOCUMENTS']) == 0:
            final_result = {
                "CORRECT_QUERY"         : from_es['CORRECT_QUERY'],
                "AUTO_CORRECT_QUERY" : from_es['AUTO_CORRECT_QUERY'],
                "WORD_COUNT"    : 0,
                "WORD_SCORE"    : 0,
                "FILENAME"      : "",
                "DOCUMENTS"     : []
            }

            return final_result

        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        documents = from_es['ES_RESULT']['DOCUMENTS']

        for idx in range(len(documents)):
                documents[idx].pop('WORD', None)
                documents[idx].pop('WORD_COUNT', None)
                documents[idx].pop('score', None)
                documents[idx].pop('stemmed_value', None)
                documents[idx].pop('stemmed_main_title', None)
                documents[idx].pop('stemmed_title', None)
                documents[idx].pop('TRUE_WORD_COUNT', None)
                documents[idx].pop('word_count', None)
                documents[idx].pop('word_score', None)
                documents[idx].pop('inner_table_keys', None)
                documents[idx].pop('inner_table_keys_stem', None)
                documents[idx].pop('inner_table_values', None)
                documents[idx].pop('inner_table_values_stem', None)
                documents[idx].pop('word_not_match', None)
                documents[idx].pop('word_match', None)

        final_result = {
            "CORRECT_QUERY"         : from_es['CORRECT_QUERY'],
            "AUTO_CORRECT_QUERY" : from_es['AUTO_CORRECT_QUERY'],
            "WORD_COUNT"    : from_es['ES_RESULT']['WORD_COUNT'],
            "WORD_SCORE"    : from_es['ES_RESULT']['WORD_SCORE'],
            "DOCUMENTS"     : from_es['ES_RESULT']['DOCUMENTS'],
            "PARSED_QUERY"  :from_es['PARSED_QUERY_STRING'],
            "FILENAME" : from_es['FILENAME']
        }

        return final_result
        #---------------------------------------------------------------#
