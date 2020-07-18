#-----------------------------------------------------------------#
import re
import json
import webscraping.utils as utils
from bs4 import BeautifulSoup
#-----------------------------------------------------------------#



class Subtitle():
    def __init__(self):
        pass



    def extract_subtitles(self, document):
        #-----------------------------------------------------------------#
        document['subtitle'] = {
            "html_tag"  : '',
            "elements"  : [{"text" : '', "content" : []}],
            "indices"   : []
        }
        #-----------------------------------------------------------------#


        #-----------------------------------------------------------------#
        subtitle_pattern = self.get_subtitle_pattern(document)

        if subtitle_pattern == None:
            print("SUBTITLE NOT FOUND")
            subtitle_pattern, dummy_subtitle_tag = self.add_dummy_subtitle(document)
        #-----------------------------------------------------------------#


        #-----------------------------------------------------------------#
        subtitle_pattern_index = None
        for idx, tag in enumerate(document['html']['main_content']['elements']):
            if re.search(subtitle_pattern, str(tag)):
                subtitle_pattern_index = idx
                break

        if subtitle_pattern_index != 0:
            print('Adding new element :::', subtitle_pattern)
            dummy_subtitle_tag = BeautifulSoup(subtitle_pattern, features="html5lib")
            document['html']['main_content']['elements'].insert(0, dummy_subtitle_tag)
        #-----------------------------------------------------------------#

        #-----------------------------------------------------------------#
        document['subtitle']['html_tag'] = subtitle_pattern

        document['subtitle']["elements"] = [{"text": tag.get_text(), "content" : []}
                for tag in document['html']['main_content']['elements']
                if re.search(subtitle_pattern, str(tag)) ]

        document['subtitle']['indices'] = self.get_subtitle_tag_indices(document)
        #-----------------------------------------------------------------#

        return document




    def get_subtitle_tag_indices(self, document):
        #-----------------------------------------------------------------#
        ele = [str(tag) for tag in document['html']['main_content']['elements']]
        subtitle_tag_indices =[idx for idx, tag in enumerate(ele) 
              if re.search(document['subtitle']['html_tag'], tag)  ]
        #-----------------------------------------------------------------#

        return subtitle_tag_indices



    def get_subtitle_pattern(self, document):
        #-----------------------------------------------------------------#
        return document['subtitle_pattern']
        #-----------------------------------------------------------------#


    def add_dummy_subtitle(self, document):
        #-----------------------------------------------------------------#
        dummy_subtitle_string = '<h5 class="dummy"></h5>'
        dummy_subtitle_tag = BeautifulSoup(dummy_subtitle_string, features="html5lib")

        for idx in range(len(document['html']['main_content']['elements'])):
            if idx % 2 == 0:
                document['html']['main_content']['elements'].insert(idx, dummy_subtitle_tag)

        subtitle_pattern = r'' + dummy_subtitle_string
        #-----------------------------------------------------------------#

        return subtitle_pattern, dummy_subtitle_tag


