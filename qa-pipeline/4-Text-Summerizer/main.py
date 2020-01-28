from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqConsumerPipe
from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
import json
from summarizer import SingleModel
from nltk.tokenize import sent_tokenize, word_tokenize



def callback(ch, method, properties, body):
        #---------------------------------------------------------------#
        log = {
            "INCOMING" : json.loads(body),
            "OUTGOING" : ''
        }
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        # summarizer_lsa = LsaSummarizer()                                 
        # summarizer_lex = LexRankSummarizer()
        # result_documents = from_es['ES_RESULT']['DOCUMENTS']
        # for idx, document in enumerate(result_documents):
                # sentences = []
                # print('-----------------------------------------------------------')
                # print('value ::\n', document['value'])
                # parser = PlaintextParser.from_string(document['value'], Tokenizer("english"))
                # for sentence in summarizer_lex(parser.document, 1):
                        # sentences.append(str(sentence).split(" ( last")[0])         

                # sentences = ". ".join(sentences)                                   
                # print ("sentences ::: \n", sentences)
                # from_es['ES_RESULT']['DOCUMENTS'][idx]['value'] = sentences
                # print('-----------------------------------------------------------\n\n')
        #---------------------------------------------------------------#

        from_es = json.loads(body)
        result_documents = from_es['ES_RESULT']['DOCUMENTS']
        for idx, document in enumerate(result_documents):
                #print('-----------------------------------------------------------')
                if len(document['value'].split()) > 75:
                    result = model(document['value'])
                    final_data = sent_tokenize(result)
                    from_es['ES_RESULT']['DOCUMENTS'][idx]['value'] = ''.join(i.capitalize() for i in final_data)
                #print('-----------------------------------------------------------\n\n')

                if len(document['inner_table_values']) > 0:
                    document['value'] = ' : '.join(document['inner_table_values'])
        #---------------------------------------------------------------#

        if len(from_es['ES_RESULT']['DOCUMENTS']) > 0:
            from_es['WORD_COUNT'] = len(from_es['ES_RESULT']['DOCUMENTS'][0]['value'])

        rabbitmq_producer.publish(json.dumps(from_es).encode())
        #---------------------------------------------------------------#

        #--------- LOGGING ---------------------------------------------#
        log['OUTGOING'] = from_es
        print(json.dumps(log, indent=4))
        #---------------------------------------------------------------#

        return
#---------------------------------------------------------------#
rabbimq_consumer = RabbitmqConsumerPipe(
        exchange="postProcessingEx", 
        queue="postProcessingQ",
        routing_key="post_processing", 
        callback=callback, 
        host='localhost')

rabbitmq_producer = RabbitmqProducerPipe(
    publish_exchange='textSummerizerEx',
    routing_key='text_summerizer'
)

model = SingleModel()

print('Service is up and running..... [4-Text-Summerizer]')
rabbimq_consumer.start_consuming()
#---------------------------------------------------------------#

