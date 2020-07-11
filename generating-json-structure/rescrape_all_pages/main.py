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
rescraping_url=config['urls']['rescrape_all_pages_url']
index = config.get('elastic_search_credentials', 'index')
synonyms_url=config['urls']['synonyms_url']
rescrape_status_url = config.get('urls','rescrape_status_url')

def read_config_files():
    try:
        documents = []
        response = requests.get(
            url=rescraping_url)
        json_configurations = response.json()
        if json_configurations != None:
            # ----------------------------------------------------------- #
            for json_config in json_configurations:
                print(type(json_config))
                url_list = json.loads(json_config['pageConfig'])
                for url in url_list:
                    document = url_list[url]
                    document['url'] = url
                    document['filename'] = path + \
                        url.split('/')[-2] + '/index.html'
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
                print("filename :: ", documents[idx]['pageConfig']['filename'])
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

                Id = documents[idx]['id']
                ScrapeStatus = 1
                response = requests.put(rescrape_status_url+str(Id)+"&ScrapeStatus="+str(ScrapeStatus))
                print(response.status_code)
                time.sleep(5)
                #------`---------------------------------------------------------#
            except ConnectionError as e:
                time.sleep(5)
                continue
            except Exception as e:
                Id = documents[idx]['id']
                ScrapeStatus = 2
                ErrorMessage = e.__class__.__name__
                response = requests.put(rescrape_status_url+str(Id)+"&ScrapeStatus="+str(ScrapeStatus)+"&ErrorMessage="+ErrorMessage)
                print(response.status_code)
            value = False


rabbitmq_producer = RabbitmqProducerPipe(
    publish_exchange="nlpEx",
    routing_key="nlp",
    queue_name='nlpQueue',
    host="localhost")


def delete_by_condition(documents):
    es = Elasticsearch(index=index)
    for doc in documents:
        url = doc['pageConfig']['url'].split("indianbank.in")[1]
        print("deleting the url = ",url)
        query = {
            "query": {
                "bool": {
                    "must": [
                        {
                            "match_phrase": {
                                "url": url
                            }
                        }
                    ]
                }
            }
        }
        result = es.delete_by_query(index=index, body=query)
        time.sleep(5)
        print(json.dumps(result, indent=4))

@app.route('/rescrape_all_pages', methods=['GET'])
def rescrape_all_pages():
    documents = read_config_files()
    delete_by_condition(documents)
    time.sleep(5)
    rescrape(documents)
    return "success"

# ----------------------------------------------------------------------------
@app.route('/resync_synonyms', methods=['POST'])
def resync_synonyms ():
    print("hiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii")
    ROOT_DIR = os.path.abspath(os.pardir)
    try:
        r = requests.get(url = synonyms_url) 
        data = r.json()
        print("----------> ",r)
        synonyms=[]
        for w in data:
            synonyms.append(re.sub(",","=",w))
        value="\n".join(synonyms)
        print("synonyms------>> ",value)
        file = open(ROOT_DIR+"/../qa-pipeline/1-query-parser/config_files/synonyms.txt","w+") 
        file.write(value)
        file.close() 
        return "successfully updated synonyms"
    except Exception as e:
        print("exception occurred..!!",e)
        return "failed to updated synonyms"

# ----------------------------------------------------------------------------
app.run(port=6000)
