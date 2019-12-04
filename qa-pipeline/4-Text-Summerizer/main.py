from MessageQueue.rabbitmq_pipe import RabbitmqConsumerPipe                                  
from MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe                                  
import json                                                                                  
from sumy.parsers.plaintext import PlaintextParser
from sumy.nlp.tokenizers import Tokenizer
from sumy.summarizers.lsa import LsaSummarizer
from sumy.summarizers.lex_rank import LexRankSummarizer
from gensim.summarization.summarizer import summarize
from summarizer import SingleModel
from nltk.tokenize import sent_tokenize, word_tokenize



                                                                                             
def callback(ch, method, properties, body):                                                  
        #---------------------------------------------------------------#
        from_es = json.loads(body)                                                           
        print(json.dumps(from_es, indent=4, sort_keys=True))
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

        result_documents = from_es['ES_RESULT']['DOCUMENTS']
        for idx, document in enumerate(result_documents):
                print('-----------------------------------------------------------')
                if len(document['value'].split()) > 75:
                    result = model(document['value'])            
                    final_data = sent_tokenize(result)
                    from_es['ES_RESULT']['DOCUMENTS'][idx]['value'] = ''.join(i.capitalize() for i in final_data)
                print('-----------------------------------------------------------\n\n')

                if len(document['inner_table_values']) > 0:
                    document['value'] = ' : '.join(document['inner_table_values'])
        #---------------------------------------------------------------#

        if len(from_es['ES_RESULT']['DOCUMENTS']) > 0:
            from_es['WORD_COUNT'] = len(from_es['ES_RESULT']['DOCUMENTS'][0]['value'])

        print(json.dumps(from_es, indent=4, sort_keys=True))
        rabbitmq_producer.publish(json.dumps(from_es).encode())                                                 
        #---------------------------------------------------------------#

        return                                                                               
                                                                                             
#---------------------------------------------------------------#
rabbimq_consumer = RabbitmqConsumerPipe(
        exchange="esPostProcessingEx", 
        queue="esPostProcessingQ",
        routing_key="es_post_processing", 
        callback=callback, 
        host='localhost')

rabbitmq_producer = RabbitmqProducerPipe(
    publish_exchange='textSummerizerEx',
    routing_key='text_summerizer'
)

model = SingleModel()

print('Service is up and running.....')
rabbimq_consumer.start_consuming()
#---------------------------------------------------------------#

