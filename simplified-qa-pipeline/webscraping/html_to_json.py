#-----------------------------------------------------------------#
from urllib.request import urlopen
from bs4 import BeautifulSoup
from webscraping.subtitles import Subtitle
from webscraping.content.content import extract_content
from webscraping.post_processing import PostProcessing
import json
import requests
#-----------------------------------------------------------------#


class HtmlToJson(Subtitle, PostProcessing):
    #-----------------------------------------------------------------#
    def __init__(self, html):
        PostProcessing.__init__(self)

        self.dom = BeautifulSoup(html, features="html5lib")
    #-----------------------------------------------------------------#

    #-----------------------------------------------------------------#

    def main_title(self, document):
        main_title = self.dom.find(document['html']['main_title']['tag'], attrs={
                                   "class": document['html']['main_title']['class']})

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
            document["main_title"] = "general managers"
            document["domain"] = "about us"
            document["class"] = "general managers"
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
        return document
    #-----------------------------------------------------------------#
