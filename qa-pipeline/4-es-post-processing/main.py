from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqConsumerPipe
from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
from building_blocks.es_post_processing import ESPostProcessing
import json


def callback(ch, method, properties, body):
        #---------------------------------------------------------------#
        log = {
            "INCOMING" : json.loads(body),
            "OUTGOING" : ''
        }
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        document = json.loads(body)
        #print('-------------------- FROM ELASTIC ------------------------')
        #print(json.dumps(document, indent=4, sort_keys=False))
        #print('----------------------------------------------------------')
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        if len(document['ES_RESULT']['DOCUMENTS']) != 0:
            es_post_processing.getUnmatchedWordsFromMainTitle(document)
            # es_post_processing.priortising_document_subTitle(unMatched_words,document)
            # es_post_processing.priortising_document(document)
            # es_post_processing.remove_unwanted_words(document)
            # es_post_processing.get_image(document)
            # es_post_processing.give_high_weightage_to_document(document)
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        #print(json.dumps(document, indent=4, sort_keys=False))
        rabbitmq_producer.publish(json.dumps(document).encode())
        #---------------------------------------------------------------#

        #--------- LOGGING ---------------------------------------------#
        log['OUTGOING'] = document
        # print(json.dumps(log, indent=4) )
        #---------------------------------------------------------------#

        return
#---------------------------------------------------------------#
es_post_processing = ESPostProcessing()

rabbitmq_producer = RabbitmqProducerPipe(
    publish_exchange='postProcessingEx',
    routing_key='post_processing'
)

rabbimq_consumer = RabbitmqConsumerPipe(
        exchange="esPostProcessingEx", 
        queue="esResultQ",
        routing_key="es_post_processing", 
        callback=callback, 
        host='localhost')

rabbitmq_producer_query = RabbitmqProducerPipe(
        publish_exchange="queryExchange", 
        routing_key="query")

print('Service is up and running...... [4-postprocessing]')
rabbimq_consumer.start_consuming()
#---------------------------------------------------------------#

