
from building_blocks.html_to_json import HtmlToJson
from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
import json
import sys
import time
from configparser import ConfigParser
import requests
from elasticsearch import Elasticsearch
from configparser import ConfigParser

path = "../../indian-bank-web-scraped-data/www.indianbank.in.1-Dec-2019/departments/"
config_file_path = '../../config.ini'
config = ConfigParser()
config.read(config_file_path)
rescraping_url = config['urls']['rescrape_all_pages_url']
index = config.get('elastic_search_credentials', 'index')
rescrape_status_url = config.get('urls', 'rescrape_status_url')

# ----------------------------------------------------------- #


def generate_json_structure(document):
    html_to_json = HtmlToJson(document, source='web')
    html_to_json.main_title(document)
    html_to_json.get_url(document)
    html_to_json.get_document_name(document)
    html_to_json.subtitles(document)
    html_to_json.content(document)
    html_to_json.post_processing(document)
    html_to_json.frame_json(document)


rabbitmq_producer = RabbitmqProducerPipe(
    publish_exchange="nlpEx",
    routing_key="nlp",
    queue_name='nlpQueue',
    host="localhost")
# ----------------------------------------------------------- #


def read_config_files():
    try:
        documents = []
        response = requests.get(
            url=rescraping_url)
        json_configurations = response.json()
        print(json.dumps(json_configurations, indent=4))
        if json_configurations != None:
            # ----------------------------------------------------------- #
            for json_config in json_configurations:
                print(type(json_config))
                url_list = json.loads(json_config['pageConfig'])
                # print("url_list ---> ", list(url_list.keys())[0])
                # if list(url_list.keys())[0] != 'document_name':
                if "document_name" not in url_list:
                    for url in url_list:
                        document = url_list[url]
                        document['url'] = url
                        document['filename'] = path + \
                            url.split('/')[-2] + '/index.html'
                        json_config['pageConfig'] = document
                        documents.append(json_config)
                else:
                    document = url_list
                    json_config['pageConfig'] = document

                    documents.append(json_config)

            return documents
    except Exception as e:
        print(e.args)


def rescrape(documents):
    for idx in range(len(documents)):
        value = True
        while value:
            try:
                config = (documents[idx]['pageConfig'])
                print(config.get("filename"))
                if not (config.get('filename') is None):
                    print("if condition..!")
                    print("filename :: ",
                          documents[idx]['pageConfig']['filename'])
                    print("url :: ", documents[idx]['pageConfig']['url'])

                    generate_json_structure(documents[idx]['pageConfig'])
                    #---------------------------------------------------------------#

                    #---------------------------------------------------------------#
                    print("PRINTING FINAL JSON STRUCTURE \n")
                    print(json.dumps(
                        documents[idx]['pageConfig']['html_to_json'], indent=4))
                    #---------------------------------------------------------------#

                    #---------------------------------------------------------------#
                    rabbitmq_producer.publish(json.dumps(
                        documents[idx]['pageConfig']['html_to_json']).encode())
                    #---------------------------------------------------------------#

                    print('---------------------------------------------------\n\n')

                else:
                    print("else condition condition..!",
                          json.dumps(documents[idx], indent=4))

                    rabbitmq_producer.publish(
                        json.dumps(documents[idx]['pageConfig']))

                Id = documents[idx]['id']
                ScrapeStatus = 1
                response = requests.put(
                    rescrape_status_url+str(Id)+"&ScrapeStatus="+str(ScrapeStatus))
                print(response.status_code)
                time.sleep(5)
                #------`---------------------------------------------------------#
            except ConnectionError as e:
                time.sleep(5)
                continue
            except Exception as e:
                print("exceptiom occured ", e.args)
                Id = documents[idx]['id']
                ScrapeStatus = 2
                ErrorMessage = "JSON configuration error"
                response = requests.put(
                    rescrape_status_url+str(Id)+"&ScrapeStatus="+str(ScrapeStatus)+"&ErrorMessage="+ErrorMessage)
                print(response.status_code)
            value = False


def drop_database():
    try:
        es = Elasticsearch(index=index)
        es.indices.delete(index=index)
        print("database deleted successfully..!!", index)
    except Exception as e:
        print("No database found..!!", e.args)


if __name__ == "__main__":
    documents = read_config_files()
    if documents != None:
        drop_database()
        time.sleep(15)
        rescrape(documents)
