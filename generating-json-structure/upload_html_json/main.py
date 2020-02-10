from building_blocks.html_to_json import HtmlToJson
from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
import json
import sys
import os
import requests 
import time

path = "../../indian-bank-web-scraped-data/www.indianbank.in.1-Dec-2019/departments/"

def main():
    if os.path.exists(os.path.abspath(os.pardir)+"/upload_html_json/config_files/uploadedHtml.json"):
        os.remove(os.path.abspath(os.pardir)+"/upload_html_json/config_files/uploadedHtml.json")
        urls="http://localhost:7512/StaticFiles/GetAllStaticFileInfoAsText"
        ROOT_DIR = os.path.abspath(os.pardir)
        try:
            response=requests.get(url=urls)
            data=response.json()
            print(json.dumps(data, indent=4))
            with open(ROOT_DIR+"/upload_html_json/config_files/uploadedHtml.json","w+") as file:
                json.dump(data,file,indent=4)
        except Exception as e:
            print("exception occurred..!!",e)
        finally:
            file.close()

    uploadedHtmlPath="./config_files/uploadedHtml.json"
    documents = []
    with open(uploadedHtmlPath, "r") as file:
        url_list = json.loads(file.read())
        # print("url_list--------->",url_list,"\n")
        for url in url_list:
            # print("url--------->",url_list[url],"\n")
            document = url_list[url]
            # print("document--------->",document,"\n")

            document['url'] = url
            # print("document['url']--------->",document['url'],"\n")

            document['filename'] = path + url.split('/')[-2] + '/index.html'
            documents.append(document)

    print("----------->>",documents)

    for document in documents:
        #---------------------------------------------------------------#
        # try:
        # print('---------------------------------------------------\n')
        print("filename :: ", document['filename'])
        print("url :: ", document['url'])

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


if __name__ == "__main__":
    while True:
        main()
        time.sleep(60)

