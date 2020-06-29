from elasticsearch import Elasticsearch
import json
from uuid import uuid1

class Elastic():
    def __init__(self, host='localhost', port='9200', index='indian-bank-auto-suggestion-v2', doc_type='_doc'):
        self.es = Elasticsearch([{'host':'localhost','port':9200}])
        self.index = index
        self.doc_type = doc_type

    def index_document(self, document):
        # --------------------------------------------------------- #
        print('--------------------- DOCUMENT PUSHED ----------------------')
        print(json.dumps(document, indent=4))
        print('------------------------------------------------------------\n\n')

        self.es.index(index=self.index, doc_type=self.doc_type, id=str(uuid1()), body=document)
        # --------------------------------------------------------- #

        return document

    def search_with_original_query(self, search_string, context):
       #---------------------------------------------------------------#                  
       print("SEARCH STRING :: ", search_string)

       es_result = self.es.search(
           index = self.index,
           body =
           {
               "from" : 0, "size" : 50,
               "query": {
                   "bool": {
                       "should": [
                           {"match": {"Questions":  search_string} },
                           {"match": {"primary_context_and_keyword": context} }
                       ]
                   }
               }
           })

       if len(es_result['hits']['hits']) == 0:
            return None
       else:
            return es_result


elastic = Elastic()


with open('./auto-suggestion-question-v2.json') as file:
    question_list = json.loads(file.read())

    for question in question_list:
        elastic.index_document(question)

   



# result = elastic.search_with_original_query('cold', 'jewel loan')
# print(json.dumps(result, indent=4))
