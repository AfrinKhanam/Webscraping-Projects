from kafka import KafkaConsumer
from kafka import KafkaProducer

class KafkaPipe:
    def __init__(self, publish_topic_name=None, consumer_topic_name=None, hostname=['localhost:9092'], timeout=60):
        if publish_topic_name != None:
            self.publish_client = KafkaProducer()

        if consumer_topic_name != None:
            self.consumer_client = KafkaConsumer(
            consumer_topic_name, 
            group_id ="gid1",
            auto_offset_reset='earliest')


        self.publish_topic_name = publish_topic_name
        self.consumer_topic_name = consumer_topic_name
        self.timeout = 60

    def publish(self, message):
        future = self.publish_client.send(self.publish_topic_name, message)
        future.get(timeout=10)


    def consume(self):
        # result = self.consumer_client.poll(timeout_ms=1000, max_records=1)
        # return result
        for msg in self.consumer_client:
                print(msg)
