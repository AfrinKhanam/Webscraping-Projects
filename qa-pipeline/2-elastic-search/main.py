from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqConsumerPipe
from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
from building_blocks.elastic_search import Elastic
import json
from configparser import ConfigParser

config_file_path = '../../config.ini'
config = ConfigParser()
config.read(config_file_path)
index = config.get('elastic_search_credentials', 'index')


def callback(ch, method, properties, body):
        #---------------------------------------------------------------#
        log = {
            "INCOMING" : json.loads(body),
            "OUTGOING" : ''
        }
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        query = json.loads(body)
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        es_result = elastic.w_search(query)

        #---------------------------------------------------------------#
        if es_result == None:
            input_to_next_module = {
                    "UUID"                  : query["UUID"],
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
            input_to_next_module = {
                    "UUID"                  : query["UUID"],
                    "QUERY_STRING"          : query["QUERY_STRING"],
                    "PARSED_QUERY_STRING"   : query['PARSED_QUERY_STRING'],
                    "POTENTIAL_QUERY_LIST"  : query["POTENTIAL_QUERY_LIST"],
                    "QUERY_SYNONYMS"        : query['QUERY_SYNONYMS'],
                    "QUERY_SYNONYMS_DICT"   : query['QUERY_SYNONYMS_DICT'],
                    "CORRECT_QUERY"         : query['CORRECT_QUERY'],
                    "AUTO_CORRECT_QUERY"    : query['AUTO_CORRECT_QUERY'],
                    "ES_RESULT"             : { "DOCUMENTS" : es_result}
            }

        rabbitmq_producer.publish(json.dumps(input_to_next_module).encode())
        #---------------------------------------------------------------#

        #--------- LOGGING ---------------------------------------------#
        log['OUTGOING'] = input_to_next_module
        print(json.dumps(log, indent=4) )
        #---------------------------------------------------------------#

        return

#---------------------------------------------------------------#

# indian_bank_database_v2
elastic = Elastic(index=index)

rabbimq_consumer = RabbitmqConsumerPipe(
        exchange="elasticSearchEx",
        queue="esQueue",
        routing_key="queryParserResult",
        callback=callback,
        host='localhost')


rabbitmq_producer = RabbitmqProducerPipe(
        publish_exchange="esResultEx",
        routing_key="es_result",
        host="localhost")

print('Service is up and running...... [2-elasticsearch]webscrape')
rabbimq_consumer.start_consuming()
#---------------------------------------------------------------#
