from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqConsumerPipe
from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
from building_blocks.stemming import Stemmer
import json


def callback(ch, method, properties, body):
        print('--------------------------------------------------------')

        #---------------------------------------------------------------#
        document = json.loads(body, strict=False)
        print(json.dumps(document, indent=4))
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        stemmer.w_stem(document)
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        print(json.dumps(document, indent=4, sort_keys=True))
        rabbitmq_producer.publish(json.dumps(document).encode())
        #---------------------------------------------------------------#


        print('--------------------------------------------------------\n\n')

        return

#---------------------------------------------------------------#
stemmer = Stemmer()

rabbimq_consumer = RabbitmqConsumerPipe(
        exchange="nlpEx",
        queue="nlpQueue",
        routing_key="nlp",
        callback=callback,
        host='localhost')

rabbitmq_producer = RabbitmqProducerPipe(
        publish_exchange="preProcessingEx",
        routing_key="preProcessing",
        queue_name='preProcessingQueue',
        host="localhost")
#---------------------------------------------------------------#

#---------------------------------------------------------------#
print('Service is up and running......') 
rabbimq_consumer.start_consuming()       
#---------------------------------------------------------------#

