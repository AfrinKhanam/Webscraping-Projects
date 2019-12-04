from elasticsearch import Elasticsearch
from uuid import uuid1
import json

class Elastic():
    def __init__(self, host='localhost', port='9200', index='indian-bank-index-modified', doc_type='_doc'):
        self.es = Elasticsearch([{'host':'localhost','port':9200}])
        self.index = index
        self.doc_type = doc_type

    def index_document(self, document):
        # --------------------------------------------------------- #
        for document in document['document_list']:
            print('--------------------- DOCUMENT PUSHED ----------------------')
            print(json.dumps(document, indent=4))
            print('------------------------------------------------------------\n\n')

            self.es.index(index=self.index, doc_type=self.doc_type, 
                    id=str(uuid1()), body=document)
        # --------------------------------------------------------- #

        return document

    def get_main_title(self, document):
        return document['main_title']

    def get_main_title_stem(self, document):
        return document['main_title_stem']

    def get_document_name(self, document):
        return document['document_name']

    def get_url(self, document):
        return  document['url']


    def generate_individual_document(self, document):
        document['document_list'] = []

        self.document_out_of_content(document)
        self.document_out_of_table(document)

        return document

    def document_out_of_content(self, document):
        # --------------------------------------------------------- #
        document_list = []

        for element in document['subtitle']['elements']:
            for content in element['content']:
                for text, text_stem in zip(content['text'], content['text_stem']):
                    document_list.append({
                        "document_name"     : self.get_document_name(document),
                        "url"               : self.get_url(document),
                        "main_title"        : self.get_main_title(document),
                        "main_title_stem"   : self.get_main_title_stem(document),
                        "subtitle"          : element['text'],
                        "subtitle_stem"     : element['text_stem'],
                        "text"              : text,
                        "text_stem"         : text_stem,
                        "key"               : '',
                        "key_stem"          : '',
                        "value_stem"        : '',
                        "value"             : '',
                        "inner_table_keys"   : [],
                        "inner_table_values" : [],
                        "inner_table_keys_stem"   : [],
                        "inner_table_values_stem" : []
                    })

        document['document_list'] += document_list
        # --------------------------------------------------------- #

        return document


    def document_out_of_table(self, document):
        # --------------------------------------------------------- #
        document_list = []

        for element in document['subtitle']['elements']:
            for content in element['content']:
                if 'table' in content:
                    for table_row in content['table']:
                        for value, value_stem in zip(table_row['value'], table_row['value_stem']):
                            if isinstance(value, dict) and 'table' in value: 
                                document_list += self.document_out_of_inner_table(value, value_stem, 
                                        document, element, table_row)
                            else:
                                document_list.append({
                                    "document_name"     : self.get_document_name(document),
                                    "url"               : self.get_url(document),
                                    "main_title"        : self.get_main_title(document),
                                    "main_title_stem"   : self.get_main_title_stem(document),
                                    "subtitle"          : element['text'],
                                    "subtitle_stem"     : element['text_stem'],
                                    "text"              : '',
                                    "text_stem"         : '',
                                    "key"               : table_row['key'],
                                    "key_stem"          : table_row['key_stem'],
                                    "value"             : value,
                                    "value_stem"        : value_stem,
                                    "inner_table_keys"   : [],
                                    "inner_table_values" : [],
                                    "inner_table_keys_stem"   : [],
                                    "inner_table_values_stem" : []
                                    })

        document['document_list'] += document_list
        # --------------------------------------------------------- #

        return document

    def document_out_of_inner_table(self, inner_table, inner_table_stem, document, element, table_row):
        document_list = []

        #print('------------------------------------------------')
        #print(inner_table['table']['keys'])
        #print(inner_table_stem)
        #print('------------------------------------------------')

        keys = inner_table['table']['keys']
        values = inner_table['table']['values']

        keys_stem = inner_table_stem['table_stem']['keys_stem']
        values_stem = inner_table_stem['table_stem']['values_stem']

        for idx in range(1,len(keys)):
            inner_table_keys = [keys[0], keys[idx]]
            inner_table_keys_stem = [keys_stem[0], keys_stem[idx]]

            for record, record_stem in zip(values, values_stem):

                inner_table_values = [record[0], record[idx]]
                inner_table_values_stem = [record_stem[0], record_stem[idx]]

                document_list.append({
                    "document_name"     : self.get_document_name(document),
                    "url"               : self.get_url(document),
                    "main_title"        : self.get_main_title(document),
                    "main_title_stem"   : self.get_main_title_stem(document),
                    "subtitle"          : element['text'],
                    "subtitle_stem"     : element['text_stem'],
                    "text"              : '',
                    "text_stem"         : '',
                    "key"               : table_row['key'],
                    "key_stem"          : table_row['key_stem'],
                    "value"             : '',
                    "value_stem"        : '',
                    "inner_table_keys"   : inner_table_keys,
                    "inner_table_values" : inner_table_values,
                    "inner_table_keys_stem"   : inner_table_keys_stem,
                    "inner_table_values_stem" : inner_table_values_stem
                })

        return document_list
