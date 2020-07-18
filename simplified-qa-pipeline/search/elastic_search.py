import elasticsearch
import json

class Elastic:
    def __init__(self, index):
        self.es = elasticsearch.Elasticsearch()
        self.index = index

    def w_search(self, query):
        #print('----------------- TRYING WITH ORIGINAL QUERY ---------------------')
        #search_string = query['QUERY_SYNONYMS'] + ' ' + query['CONTEXT'] 
        #es_result = self.search_with_original_query(search_string)

        es_result = self.search_with_original_query(query['QUERY_SYNONYMS'])
        #es_result = self.search_with_original_query(query['PARSED_QUERY_STRING'])

        #print(json.dumps(es_result, indent=4, sort_keys=True))
        #print('------------------------------------------------------------------\n\n')

        if es_result == None:
            return None

        '''
        if es_result == None:
            print('--------------- TRYING WITH EQUIVALENT QUERY -----------------')
            print(query['QUERY_SYNONYMS'])
            es_result = self.search_with_equivalent_query(query['QUERY_SYNONYMS'])
            print(json.dumps(es_result, indent=4, sort_keys=True))
            print('------------------------------------------------------------------\n\n')
            if es_result == None:
                return []

        '''
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

            #print('----------------------------- record ----------------------')
            #print(record)
            #print('-----------------------------------------------------------\n\n')

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
        #---------------------------------------------------------------#
        #print("SEARCH STRING :: ", search_string)

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

        '''
        es_result = self.es.search(
            index = self.index, 
            body = 
            {
                "query": {
                    "multi_match" : {
                    "query":    search_string,
                    "fields": ["text_stem", "subtitle_stem^2", "main_title_stem"]
                    }
                }
            }
            )
        '''

        '''
        body = {"query": {"match_phrase": {"stemmed_value": {
            "query": "*" + search_string.lower() + "*" ,"slop": 30}}}})
        '''
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
        #print('EQUIVALENT QUERY ::: ', query)

        #---------------------------------------------------------------#
        # parsed_query = ''
        # for record in synonyms_dict:
            # parsed_query += " ".join(record) + ' '
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        # es_result = self.es.search(
            # index = "indianbankdata1", 
            # body = {"query": {"match": {"stemmed_value": {
                # "query": query}}}})
        #---------------------------------------------------------------#

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

