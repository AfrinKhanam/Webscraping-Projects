import pika

class RabbitmqProducerPipe:
    def __init__(self, publish_exchange, routing_key, host='localhost'):
        self.credentials = pika.PlainCredentials('demo', 'demo')
        self.publish_routing_key = routing_key
        self.publish_exchange = publish_exchange

        self.connection = pika.BlockingConnection(pika.ConnectionParameters(
            host, 
            virtual_host='demo',
            heartbeat = 0,
            credentials=self.credentials))
        self.channel = self.connection.channel()
        self.channel.exchange_declare(exchange=publish_exchange, exchange_type='direct', durable=True)


    def publish(self, message):
        self.channel.basic_publish(
            exchange=self.publish_exchange, 
            routing_key=self.publish_routing_key, 
            body=message,
            properties=pika.BasicProperties(delivery_mode=2)
            )

    def close(self):
        self.connection.close()

    
class RabbitmqConsumerPipe:
    def __init__(self, exchange, queue, routing_key, callback, host='localhost'):
        self.queue = queue
        self.exchange = exchange
        self.routing_key = routing_key
        self.credentials = pika.PlainCredentials('demo', 'demo')

        self.connection = pika.BlockingConnection(pika.ConnectionParameters(
            host, 
            virtual_host='demo',
            heartbeat=0,
            credentials=self.credentials))
        self.channel = self.connection.channel()
        self.channel.exchange_declare(exchange=self.exchange, exchange_type='direct',durable=True)
        self.channel.queue_declare(self.queue, durable=True)
        self.channel.queue_bind(exchange=self.exchange, queue=self.queue, routing_key=self.routing_key)

        self.channel.basic_qos(prefetch_count=1)
        self.channel.basic_consume(queue=self.queue, on_message_callback=callback, auto_ack=True)

    def start_consuming(self):
        self.channel.start_consuming()
    




