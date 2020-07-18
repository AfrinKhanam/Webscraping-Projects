import flask
from flask import request, jsonify
from building_blocks.html_to_json import HtmlToJson
from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
import json
import sys
import time
from elasticsearch import Elasticsearch
from configparser import ConfigParser
import requests
from datetime import datetime
import pathlib
import os
import re


path = "../../indian-bank-web-scraped-data/www.indianbank.in.1-Dec-2019/departments/"
app = flask.Flask(__name__)
app.config["DEBUG"] = True

# ----------------------------------------------------------- #
config_file_path = '../../config.ini'
config = ConfigParser()
config.read(config_file_path)
rescraping_url = config['urls']['rescrape_all_pages_url']
index = config.get('elastic_search_credentials', 'index')
synonyms_url = config['urls']['synonyms_url']
rescrape_status_url = config.get('urls', 'rescrape_status_url')


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

# ----------------------------------------------------------- #

# ----------------------------------------------------------- #


def rescrape(documents):
    for idx in range(len(documents)):
        value = True
        while value:
            try:
                config = (documents[idx]['pageConfig'])
                #check if the file is not in manually scraped list
                if not (config.get('filename') is None):
                    print("if condition..!")
                    print("filename :: ",
                          documents[idx]['pageConfig']['filename'])
                    print("url :: ", documents[idx]['pageConfig']['url'])
                    if documents[idx]['url'] == 'https://www.indianbank.in/departments/general-managers/' or documents[idx]['url'] == 'https://indianbank.in/departments/general-managers/':
                        html_to_json = HtmlToJson(documents[idx], source='web')
                        html_to_json.generate_json_for_general_managers(documents[idx])
                        rabbitmq_producer.publish(json.dumps(
                            documents[idx]).encode())
                    else:
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
                    print("else condition condition..!",json.dumps(documents[idx],indent=4))
                        
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
                print("exceptiom occured ",e.args)
                Id = documents[idx]['id']
                ScrapeStatus = 2
                ErrorMessage = "JSON configuration error"
                response = requests.put(
                    rescrape_status_url+str(Id)+"&ScrapeStatus="+str(ScrapeStatus)+"&ErrorMessage="+ErrorMessage)
                print(response.status_code)
            value = False


rabbitmq_producer = RabbitmqProducerPipe(
    publish_exchange="nlpEx",
    routing_key="nlp",
    queue_name='nlpQueue',
    host="localhost")

def drop_database():
    try:
        es = Elasticsearch(index=index)
        es.indices.delete(index=index)
        print("database deleted successfully..!!", index)
    except Exception as e:
        print("No database found..!!", e.args)


@app.route('/rescrape_all_pages', methods=['GET'])
def rescrape_all_pages():
    documents = read_config_files()
    # print(json.dumps(documents[0], indent=4))
    if documents != None:
        drop_database()
        time.sleep(5)
        rescrape(documents)
    return "successfully scraped the pages", 200

# ----------------------------------------------------------------------------
@app.route('/resync_synonyms', methods=['GET'])
def resync_synonyms():
    ROOT_DIR = os.path.abspath(os.pardir)
    try:
        r = requests.get(url=synonyms_url)
        data = r.json()
        synonyms = []
        for w in data:
            synonyms.append(re.sub(",", "=", w))
        value = "\n".join(synonyms)
        file = open(
            ROOT_DIR+"/../qa-pipeline/1-query-parser/config_files/synonyms.txt", "w+")
        file.write(value)
        file.close()
        return "successfully updated synonyms"
    except Exception as e:
        print("exception occurred..!!", e)
        return "failed to update synonyms", 400


# ----------------------------------------------------------------------------
app.run(port=6000)
