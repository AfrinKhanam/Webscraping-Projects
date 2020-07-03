from building_blocks.html_to_json import HtmlToJson
from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
import json
import sys
import os
import requests 
import time
from datetime import datetime
from configparser import ConfigParser

path = "../../indian-bank-web-scraped-data/www.indianbank.in.1-Dec-2019/departments/"
config_file_path = '../../config.ini'
config = ConfigParser()
config.read(config_file_path)

def get_static_file_info():
    # url="http://localhost:7512/StaticFiles/"
    url=config['urls']['static_file_url']
    
    try:
        response=requests.get(url=url)
        data=response.json()
        print(json.dumps(data, indent=4))
        if data !=None:
            with open("./config_files/uploadedHtml.json","w+") as file:
                json.dump(data,file,indent=4)
                file.close()
    except Exception as e:
        print(e.args)

def scrape_static_file():
    uploadedHtmlPath = "./config_files/uploadedHtml.json"
    static_page_id = None
    documents = []
    try:
        with open(uploadedHtmlPath, "r") as file:
            url_list = json.loads(file.read())
            for url in url_list:
                document = url_list[url]
                document['url'] = url
                document['filename'] = path + url.split('/')[-2] + '/index.html'
                documents.append(document)

        for document in documents:

            static_page_id = document['url'].split("=")[1]

            html_to_json = HtmlToJson(document, source='web')
            html_to_json.main_title(document)
            html_to_json.get_url(document)
            html_to_json.get_document_name(document)
            html_to_json.subtitles(document)
            html_to_json.content(document)
            html_to_json.post_processing(document)
            html_to_json.frame_json(document)
            print(json.dumps(document['html_to_json'], indent=4))
            
            rabbitmq_producer = RabbitmqProducerPipe(
            publish_exchange="nlpEx",
            routing_key="nlp",
            queue_name='nlpQueue',
            host="localhost")
            
            rabbitmq_producer.publish(json.dumps(document['html_to_json']).encode())
            
            static_file_status = {"id": static_page_id,"createdOn": datetime.now(),"scrapeStatus": 1}
            update_scrape_status(static_file_status)    
            
    except Exception as e:
        static_file_status = {"id": static_page_id,"createdOn": datetime.now(),"scrapeStatus": 2}
        update_scrape_status(static_file_status) 

def update_scrape_status(params):
    try:
        requests.put(config['urls']['static_file_status_url'], data=(params))
    except Exception as e:
        print(e.args)

def main():
    #get static files configurations
    get_static_file_info()

    #scrape the static files
    scrape_static_file()
   
if __name__ == "__main__":
    while True:
        pass
        main()
        time.sleep(60)

