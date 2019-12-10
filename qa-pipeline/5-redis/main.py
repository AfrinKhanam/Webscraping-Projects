from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqConsumerPipe
from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
from building_blocks.InMemory.redis import RedisClient
import json



def callback(ch, method, properties, body):
        #---------------------------------------------------------------#
        from_es = json.loads(body)
        print(json.dumps(from_es, indent=4, sort_keys=True))

        if len(from_es['ES_RESULT']['DOCUMENTS']) == 0:
            final_result = {
                "CORRECT_QUERY"         : from_es['CORRECT_QUERY'],
                "AUTO_CORRECT_QUERY" : from_es['AUTO_CORRECT_QUERY'],
                "WORD_COUNT"    : 0,
                "WORD_SCORE"    : 0,
                "FILENAME"      : "",
                "DOCUMENTS"     : []
            }

            redis_client.set_value(from_es['UUID'], json.dumps(final_result))
            return 

        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        documents = from_es['ES_RESULT']['DOCUMENTS']
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
            "FILENAME" : from_es['FILENAME']
        }

        redis_client.set_value(from_es['UUID'], json.dumps(final_result))
        print(json.dumps(final_result, indent=4, sort_keys=True))
        #---------------------------------------------------------------#

        return
#---------------------------------------------------------------#
redis_client = RedisClient()

#rabbimq_consumer = RabbitmqConsumerPipe(
#         exchange="esPostProcessingEx", 
#         queue="esPostProcessingQ",
#         routing_key="es_post_processing", 
#         callback=callback, 
#         host='localhost')

rabbimq_consumer_text_summerizer = RabbitmqConsumerPipe(
       exchange="textSummerizerEx", 
       queue="textSummerizerQ",
       routing_key="text_summerizer", 
       callback=callback, 
       host='localhost')


print('Service is up and running.....')
rabbimq_consumer_text_summerizer.start_consuming()
#rabbimq_consumer.start_consuming()
#---------------------------------------------------------------#
