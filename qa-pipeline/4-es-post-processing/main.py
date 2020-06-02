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
            # es_post_processing.MainTitlePrioritization(document)
            document,bucket1,bucket2,bucket3 = es_post_processing.main_title_prioritization(document)
            es_post_processing.subtitle_prioritization(bucket1,bucket2,bucket3,document)


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
        queue="esPostProcessingResultQ",
        routing_key="es_post_processing", 
        callback=callback, 
        host='localhost')

rabbitmq_producer_query = RabbitmqProducerPipe(
        publish_exchange="queryExchange", 
        routing_key="query")

print('Service is up and running...... [4-postprocessing]')
rabbimq_consumer.start_consuming()
#---------------------------------------------------------------#

