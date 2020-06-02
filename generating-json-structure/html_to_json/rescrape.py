from building_blocks.html_to_json import HtmlToJson
from building_blocks.MessageQueue.rabbitmq_pipe import RabbitmqProducerPipe
import json
import sys
import flask
from flask import request, jsonify
import os

app = flask.Flask(__name__)
app.config["DEBUG"] = True


documents = []


rabbitmq_producer = RabbitmqProducerPipe(
    publish_exchange="nlpEx",
    routing_key="nlp",
    queue_name='nlpQueue',
    host="localhost")
path = '/home/ashutosh/Desktop/WorkSpace/IndianBank-KBOT(1)/generating-json-structure/html_to_json/config_files/rescrape_pages/rescrape_url.json'
json_files = []

# ROOT_DIR = os.path.dirname(os.path.abspath(__file__)) # This is your Project Root
CONFIG_PATH = './config_files/rescrape_pages/rescrape_url.json'


@app.route('/', methods=['POST'])
def rescrape_urls():
    try:
        document = json.dumps(request.json, indent=4)
        with open(path, 'w+') as file:
            file.write(document)
            file.close()
            json_files.append(CONFIG_PATH)
            documents = read_json()
            # main()

        return 'html page scraped successfully..!!'

    except Exception as e:
        return 'exception-->>'+str(e.args)
# ----------------------------------------------------------- #


def read_json():

    with open(json_files[0], "r") as file:
        url_list = json.loads(file.read())
        for url in url_list:
            document = url_list[url]

            document['url'] = url

            document['filename'] = 'rescraping/index.html'
            documents.append(document)
        file.close()
        return documents


# ----------------------------------------------------------- #

def main():

    for document in documents:
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

        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        print(json.dumps(document['html_to_json'], indent=4))
        #---------------------------------------------------------------#

        #---------------------------------------------------------------#
        rabbitmq_producer.publish(json.dumps(
            document['html_to_json']).encode())
        #---------------------------------------------------------------#

        print('---------------------------------------------------\n\n')
    return 'document'

if __name__ == "__main__":
    app.run(port=8000)
    # main()
