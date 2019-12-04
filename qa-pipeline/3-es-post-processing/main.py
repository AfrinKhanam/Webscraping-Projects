from MessageQueue.rabbitmq_pipe import RabbitmqConsumerPipe                                  
from MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe                                  
from Utils.es_post_processing import ESPostProcessing
import json                                                                                  


                                                                                             
def callback(ch, method, properties, body):                                                  
        #---------------------------------------------------------------#
        document = json.loads(body)                                                           
        #print('-------------------- FROM ELASTIC ------------------------')
        #print(json.dumps(document, indent=4, sort_keys=False))
        #print('----------------------------------------------------------')
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        if len(document['ES_RESULT']['DOCUMENTS']) != 0:
            es_post_processing.priortising_document(document)
            es_post_processing.remove_unwanted_words(document)
            es_post_processing.get_image(document)
            es_post_processing.give_high_weightage_to_document(document)
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        #print(json.dumps(document, indent=4, sort_keys=False))
        rabbitmq_producer.publish(json.dumps(document).encode())                                                 
        #---------------------------------------------------------------#

        return                                                                               
                                                                                             
#---------------------------------------------------------------#
es_post_processing = ESPostProcessing()

rabbitmq_producer = RabbitmqProducerPipe(
    publish_exchange='esPostProcessingEx',
    routing_key='es_post_processing'
)

rabbimq_consumer = RabbitmqConsumerPipe(
        exchange="esResultEx", 
        queue="esResultQ",
        routing_key="es_result", 
        callback=callback, 
        host='localhost')

rabbitmq_producer_query = RabbitmqProducerPipe(
        publish_exchange="queryExchange", 
        routing_key="query")

print('Service is up and running......')
rabbimq_consumer.start_consuming()
#---------------------------------------------------------------#

