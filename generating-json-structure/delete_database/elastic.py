from elasticsearch import Elasticsearch
from configparser import ConfigParser

config_file_path = '../../config.ini'
config = ConfigParser()
config.read(config_file_path)
es = Elasticsearch()

try:
    index = config.get('elastic_search_credentials', 'index')

    es.indices.delete(index=index) 
    # es.indices.delete(index='indian_bank_database_v3', ignore=[400, 404]) 

    print("database deleted successfully..!!")
except Exception as e:
    print("No database found..!!")
