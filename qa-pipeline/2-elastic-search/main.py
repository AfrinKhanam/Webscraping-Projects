from MessageQueue.rabbitmq_pipe import RabbitmqConsumerPipe                                  
from MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe                                  
from Utils.elastic_search import Elastic
import json                                                                                  


                                                                                             
def callback(ch, method, properties, body):                                                  
        #---------------------------------------------------------------#
        query = json.loads(body)                                                           
        print(json.dumps(query, indent=4, sort_keys=True))
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        es_result = elastic.w_search(query)

        print('--------------------- ELASTIC SEARCH ------------------------------')
        print(es_result)
        print('-------------------------------------------------------------------')

        #---------------------------------------------------------------#
                                                                                             
        #---------------------------------------------------------------#
        if es_result == None:
            rabbitmq_producer.publish(json.dumps({
                    "UUID"                  : query["UUID"],
                    "QUERY_STRING"          : query["QUERY_STRING"],
                    "PARSED_QUERY_STRING"   : query['PARSED_QUERY_STRING'],
                    "POTENTIAL_QUERY_LIST"  : query["POTENTIAL_QUERY_LIST"],
                    "QUERY_SYNONYMS"        : query['QUERY_SYNONYMS'],
                    "QUERY_SYNONYMS_DICT"   : query['QUERY_SYNONYMS_DICT'],
                    "PROCESSING"            : query['PROCESSING'],
                    "AUTO_CORRECT_QUERY"    : query['AUTO_CORRECT_QUERY'],
                    "ES_RESULT"             : { "DOCUMENTS" : []} }).encode()) 
        else:
            rabbitmq_producer.publish(json.dumps({
                    "UUID"                  : query["UUID"],
                    "QUERY_STRING"          : query["QUERY_STRING"],
                    "PARSED_QUERY_STRING"   : query['PARSED_QUERY_STRING'],
                    "POTENTIAL_QUERY_LIST"  : query["POTENTIAL_QUERY_LIST"],
                    "QUERY_SYNONYMS"        : query['QUERY_SYNONYMS'],
                    "QUERY_SYNONYMS_DICT"   : query['QUERY_SYNONYMS_DICT'],
                    "PROCESSING"            : query['PROCESSING'],
                    "AUTO_CORRECT_QUERY"    : query['AUTO_CORRECT_QUERY'],
                    "CORRECT_QUERY"         : query['CORRECT_QUERY'],
                    "ES_RESULT"             : { "DOCUMENTS" : es_result} }).encode()) 
        #---------------------------------------------------------------#

        return                                                                               
                                                                                             
#---------------------------------------------------------------#
elastic = Elastic(index='indian-bank-index')
#elastic = Elastic(index='indian-bank-index-modified')

rabbimq_consumer = RabbitmqConsumerPipe(
        exchange="elasticSearchEx", 
        queue="esQueue",
        routing_key="es", 
        callback=callback, 
        host='localhost')


rabbitmq_producer = RabbitmqProducerPipe(
        publish_exchange="esResultEx", 
        routing_key="es_result",
        host="localhost")                                                                                                                     
'''
rabbitmq_producer = RabbitmqProducerPipe(
    publish_exchange='esPostProcessingEx',
    routing_key='es_post_processing'
)
'''

print('Service is up and running......')
rabbimq_consumer.start_consuming()
#---------------------------------------------------------------#
