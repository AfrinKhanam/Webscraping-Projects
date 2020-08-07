import elasticsearch
import json

class Elastic:
    def __init__(self, index):
        self.es = elasticsearch.Elasticsearch()
        self.index = index

    def w_search(self, query):
        es_result = self.search_with_original_query(query['QUERY_SYNONYMS'])

        if es_result == None:
            return None

        #---------------------------------------------------------------#
        documents = []
        for record in es_result['hits']['hits']:

            if len(record['_source']['text']) == 0:
                value = record['_source']['value']
                value_stem = record['_source']['value_stem']
                title = record['_source']['key']
                title_stem = record['_source']['key_stem']
            else:
                value = record['_source']['text']
                value_stem = record['_source']['text_stem']
                title = record['_source']['subtitle']
                title_stem = record['_source']['subtitle_stem']

            documents.append({
                'main_title'        : record['_source']['main_title'],
                'stemmed_main_title': record['_source']['main_title_stem'],
                'url'               : record['_source']['url'],
                'score'             : record['_score'],
                'title'             : title,
                'stemmed_title'     : title_stem,
                'value'             : value,
                'stemmed_value'     : value_stem,
                'inner_table_keys'  : record['_source']['inner_table_keys'],
                'inner_table_keys_stem' : record['_source']['inner_table_keys_stem'],
                'inner_table_values'    : record['_source']['inner_table_values'],
                'inner_table_values_stem' : record['_source']['inner_table_values_stem']
            })
        #---------------------------------------------------------------#
        return documents

    def search_with_original_query(self, search_string):

        es_result = self.es.search(
            index = self.index, 
            body = 
            {
                "from" : 0, "size" : 200 ,
                "query": {
                    "bool": {
                        "should": [
                            {"match": {"main_title_stem":  search_string} },
                            {"match": {"subtitle_stem":    search_string} },
                            {"match": {"text_stem":  search_string} },
                            {"match": {"key_stem":  search_string} },
                            {"match": {"value_stem":  search_string} }
                        ]
                    }
                }
            })

        #---------------------------------------------------------------#

        if len(es_result['hits']['hits']) == 0:
            return None
        else:
            return es_result
    
    def create_equivalent_query(self, synonyms_dict):
        query = ''
        for idx, record in enumerate(synonyms_dict):
            query += '( '
            query += " OR ".join(record)
            query += ' ) '
            if idx != len(synonyms_dict) - 1:
                query += " OR "

        return query

    def search_with_equivalent_query(self, synonyms_dict):
        query = self.create_equivalent_query(synonyms_dict)
        
        #---------------------------------------------------------------#
        es_result = self.es.search(
            index = "database", 
            body = {"query": {
                "query_string" : {
                    "query" : query
                }
            }})
        #---------------------------------------------------------------#

        if len(es_result['hits']['hits']) == 0:
            synonyms_dict_copy = synonyms_dict.copy()
            synonyms_dict_copy.pop()
            return self.search_with_equivalent_query(synonyms_dict_copy)
        else:
            return es_result

