from elasticsearch import Elasticsearch
es = Elasticsearch()

try:
    es.indices.delete(index='indian_bank_database') 
    # es.indices.delete(index='indian_bank_database_v3', ignore=[400, 404]) 

    print("database deleted successfully..!!")
except Exception as e:
    print("No database found..!!")
