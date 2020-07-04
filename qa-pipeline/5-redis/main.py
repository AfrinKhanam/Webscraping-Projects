from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqConsumerPipe
from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
from building_blocks.InMemory.redis import RedisClient
import json



def callback(ch, method, properties, body):
        #---------------------------------------------------------------#
        log = {
            "INCOMING" : json.loads(body),
            "OUTGOING" : ''
        }
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        from_es = json.loads(body)

        if len(from_es['ES_RESULT']['DOCUMENTS']) == 0:
            final_result = {
                "CORRECT_QUERY"         : from_es['CORRECT_QUERY'],
                "AUTO_CORRECT_QUERY" : from_es['AUTO_CORRECT_QUERY'],
                "WORD_COUNT"    : 0,
                "WORD_SCORE"    : 0,
                "FILENAME"      : "",
                "DOCUMENTS"     : []
            }

            redis_client.set_value(from_es['UUID'], json.dumps(final_result), expiry_time=120)
            return 

        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        documents = from_es['ES_RESULT']['DOCUMENTS']
        # print(json.dumps(documents,indent=4))
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

        redis_client.set_value(from_es['UUID'], json.dumps(final_result), expiry_time=120)
        #---------------------------------------------------------------#

        #--------- LOGGING ---------------------------------------------#
        log['OUTGOING'] = final_result
        print(json.dumps(log, indent=4) )
        #---------------------------------------------------------------#

        return
#---------------------------------------------------------------#
redis_client = RedisClient()

rabbimq_consumer_text_summerizer = RabbitmqConsumerPipe(
       exchange="textSummerizerEx", 
       queue="textSummerizerQ",
       routing_key="text_summerizer", 
       callback=callback, 
       host='localhost')


print('Service is up and running..... [5-redis]')
rabbimq_consumer_text_summerizer.start_consuming()
#rabbimq_consumer.start_consuming()
#---------------------------------------------------------------#
