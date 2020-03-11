from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqConsumerPipe
from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
from building_blocks.pre_processing import PreProcessing
import json


def callback(ch, method, properties, body):
        print('--------------------------------------------------------')

        #---------------------------------------------------------------#
        document = json.loads(body, strict=False)
        # print(json.dumps(document, indent=4))
        #---------------------------------------------------------------#

        
        #---------------------------------------------------------------#
        processing.postProcessing(document)
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        print(json.dumps(document, indent=4, sort_keys=True))
        rabbitmq_producer.publish(json.dumps(document).encode())

        #---------------------------------------------------------------#


        print('--------------------------------------------------------\n\n')

        return

#---------------------------------------------------------------#
processing = PreProcessing()

rabbimq_consumer = RabbitmqConsumerPipe(
        exchange="preProcessingEx",
        queue="preProcessingQueue",
        routing_key="preProcessing",
        callback=callback,
        host='localhost')

# rabbitmq_producer = RabbitmqProducerPipe(
#         publish_exchange="esEx",
#         routing_key="es",
#         queue_name='esQueue',
#         host="localhost")

rabbitmq_producer = RabbitmqProducerPipe(
        publish_exchange="elasticEx",
        routing_key="elastic",
        queue_name='elasticQueue',
        host="localhost")
#---------------------------------------------------------------#

#---------------------------------------------------------------#
print('Service is up and running......[pre-processing]') 
rabbimq_consumer.start_consuming()       
#---------------------------------------------------------------#

