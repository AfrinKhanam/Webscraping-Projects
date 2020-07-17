#-----------------------------------------------------------------#
from urllib.request import urlopen
from bs4 import BeautifulSoup
from building_blocks.subtitle.subtitles import Subtitle
from building_blocks.content.content import extract_content
from building_blocks.post_processing.post_processing import PostProcessing
import json
import requests
#-----------------------------------------------------------------#


class HtmlToJson(Subtitle, PostProcessing):
    #-----------------------------------------------------------------#
    def __init__(self, document, source):
        PostProcessing.__init__(self)
        if source is 'local':
            with open(document['filename']) as file:
                html = file.read()
        elif source is 'web':
            print("000000000000000000000000000000000000000000000")
            print(requests)
            response = requests.get(document['url'], verify=False)
            print("***********************************")
            html = response.content
            # print(html)
        else:
            raise('INVALID SOURCE IS DEFINED')

        self.dom = BeautifulSoup(html, features="html5lib")
    #-----------------------------------------------------------------#

    #-----------------------------------------------------------------#

    def main_title(self, document):
        main_title = self.dom.find(document['html']['main_title']['tag'], attrs={
                                   "class": document['html']['main_title']['class']})
        print("main title is----> ", main_title)

        document['html']['main_title']['text'] = main_title.get_text()
        return document
    #-----------------------------------------------------------------#

    #-----------------------------------------------------------------#

    def get_document_name(self, document):
        document['document_name'] = document['filename'].split('/')[-2]
    #-----------------------------------------------------------------#

    #-----------------------------------------------------------------#

    def get_url(self, document):
        # url = self.dom.find("link",{"rel":"alternate"})['href']
        # url = url.split("feed")[0]
        # url = url.replace("../../../", "https://www.indianbank.in/")
        # document['url'] = url

        document['url'] = document['url']
        return document
    #-----------------------------------------------------------------#

    #-----------------------------------------------------------------#
    def generate_json_for_general_managers(self,document):
        document['subtitle'] = {
            "html_tag": '',
            "elements": []
        }
        try:
            main_title = self.dom.find(document['html']['main_title']['tag'], attrs={
                                   "class": document['html']['main_title']['class']})
            document["main_title"] = main_title.get_text()
            document["domain"] = main_title.get_text()
            document["class"] = main_title.get_text()
            document["document_name"] = document['document_name'] = document['filename'].split('/')[-2]

            contents = []
            main_content = self.dom.find("tbody")
            for row in main_content:
                if len(row) != 1:  # eleminating junk row value from table
                    for idx, col in enumerate(row):
                        if len(col) != 1:
                            contents.append(col.contents)
            for idx,content in enumerate(contents):
                if len(content) != 0:
                    content_obj = {"text":content[2], "content":[{"text":[content[0]]}]}
                    document['subtitle']['elements'].insert(idx,content_obj)
            # print("000000000",json.dumps(document,indent=4))
        except Exception as e:
            print("exception occurred..!!",e.args)

    def subtitles(self, document):
        try:
            #-----------------------------------------------------------#
            main_content = self.dom.find(document['html']['main_content']['tag'], attrs={"class" : document['html']['main_content']['class']})

            #if main_content.find('div', attrs={"class" : "table-responsive"}):
                #main_content = main_content.find('div', attrs={"class" : "table-responsive"})
            #elif main_content.find('div', attrs={"class" : "table-wraper"}):
                #main_content = main_content.find('div', attrs={"class" : "table-wraper"})
            #-----------------------------------------------------------#

            #-----------------------------------------------------------#
            main_content_ele = []
            for element in main_content.contents:
                if element.name is not None:
                    main_content_ele.append(element)

            document['html']['main_content']['elements'] = main_content_ele

            self.extract_subtitles(document)
            #-----------------------------------------------------------#

            return document
        except Exception as e:
            print("exception occurred at subtitles ", e)
    #-----------------------------------------------------------------#

    #-----------------------------------------------------------------#

    def content(self, document):
        extract_content(document)
    #-----------------------------------------------------------------#

    #-----------------------------------------------------------------#

    def frame_json(self, document):
        #-----------------------------------------------------------#
        del document['subtitle']['indices']
        for idx in range(len(document['subtitle']['elements'])):
            for ele in document['subtitle']['elements'][idx]['content']:
                del ele['dom']
        #-----------------------------------------------------------#

        #-----------------------------------------------------------#
        document['html_to_json'] = {
            "document_name": document['document_name'],
            "url": document["url"],
            "domain": document['page_hierarchy']['domain'],
            "class": document['page_hierarchy']['class'],
            "sub_class": "",
            "main_title": document['html']["main_title"]["text"],
            "subtitle": document["subtitle"]
        }
        #-----------------------------------------------------------#
        # print(document)
        return document
    #-----------------------------------------------------------------#
