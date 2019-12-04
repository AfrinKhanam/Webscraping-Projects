from MessageQueue.rabbitmq_pipe import RabbitmqConsumerPipe
from building_blocks.elastic import Elastic
import json


def callback(ch, method, properties, body):
        print('--------------------------------------------------------')

        #---------------------------------------------------------------#
        document = json.loads(body, strict=False)
        print(json.dumps(document, indent=4))
        #---------------------------------------------------------------#


        #---------------------------------------------------------------#
        elastic.generate_individual_document(document)
        elastic.index_document(document)
        #---------------------------------------------------------------#


        print('--------------------------------------------------------\n\n')

        return

#---------------------------------------------------------------#
elastic = Elastic()

rabbimq_consumer = RabbitmqConsumerPipe(
        exchange="esEx",
        queue="esQueue",
        routing_key="es",
        callback=callback,
        host='localhost')
#---------------------------------------------------------------#

#---------------------------------------------------------------#
print('Service is up and running......[elastic search]')
rabbimq_consumer.start_consuming()
#---------------------------------------------------------------#

