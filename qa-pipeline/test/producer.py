from MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
import json
import uuid


test_query = "I want to apply for Affordable Housing Loans.Can I get the Home loan Form ?"


































test_query_string = {
    "UUID" : "absdj-1237812-asdjdas",
    "QUERY_STRING" : test_query,
    "CONTEXT" : ""

    }
rabbitmq_producer_query = RabbitmqProducerPipe(publish_exchange="queryExchange", 
        routing_key="query",
        host="localhost")
rabbitmq_producer_query.publish(json.dumps(test_query_string).encode())
