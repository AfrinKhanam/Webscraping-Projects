from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqConsumerPipe                                  
from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe                                  
from building_blocks.query_parser import  QueryParser 
import json                                                                                  

def callback(ch, method, properties, body):                                                  
        print('########################################################')
        #---------------------------------------------------------------#
        query = json.loads(body)                                                           
        print(json.dumps( query, indent=4) )

        if 'PROCESSING' not in query.keys():
                query['PROCESSING'] = True
        else:
                query['PROCESSING'] = False
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        context_string_length = len(query['CONTEXT'].split())
        print('context_string_length :: ', context_string_length)
        #---------------------------------------------------------------#


        #---------------------------------------------------------------#
        query["QUERY_STRING"] += ' ' + query['CONTEXT'] 
        parsed_query_string, query_synonyms_dict, potential_query_list, synonym_query, auto_correct_string = query_parser.parse(query['QUERY_STRING'])
        #---------------------------------------------------------------#
                                                                                             
        #---------------------------------------------------------------#
        correct_string = auto_correct_string.split()
        correct_string = correct_string[0:len(correct_string) - context_string_length]
        correct_string = " ".join(correct_string)

        rabbitmq_producer.publish(json.dumps({
                "UUID" : query["UUID"],
                "QUERY_STRING" : query["QUERY_STRING"],
                "QUERY_SYNONYMS" : synonym_query,
                "QUERY_SYNONYMS_DICT" : query_synonyms_dict,
                "PARSED_QUERY_STRING" : parsed_query_string,
                "POTENTIAL_QUERY_LIST": potential_query_list,
                "AUTO_CORRECT_QUERY"  : auto_correct_string,
                "CORRECT_QUERY"  : correct_string,
                "PROCESSING" : query['PROCESSING'],
        }).encode())                                                 
        #---------------------------------------------------------------#
        print('########################################################\n\n')

        return                                                                               
                                                                                             

query_parser = QueryParser()
#print(query_parser.ginger_autocorrection('pre used'))

rabbimq_consumer = RabbitmqConsumerPipe(
        exchange="queryExchange", 
        queue="queryQueue",
        routing_key="query", 
        callback=callback, 
        host='localhost')

rabbitmq_producer = RabbitmqProducerPipe(
        publish_exchange="elasticSearchEx", 
        routing_key="es",
        host="localhost")                                                                                                                     

print('Service is up and running......')
rabbimq_consumer.start_consuming()
