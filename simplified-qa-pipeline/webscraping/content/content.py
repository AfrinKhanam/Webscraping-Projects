#---------------------------------------------------------------------#
import json
import re
import sys

from bs4 import BeautifulSoup

import webscraping.utils as utils
from webscraping.content.table_to_json import parse_table_to_json
from webscraping.content.list_to_json import parse_ul_to_json
#---------------------------------------------------------------------#

#---------------------------------------------------------------------#
def extract_content(document):
    get_dom(document)
    get_text(document)

    return document
#---------------------------------------------------------------------#

#---------------------------------------------------------------------#
def get_dom(document):

    #--------------------------------------------------------------------------#
    res = utils.list_split(document['html']['main_content']['elements'], document['subtitle']['indices'])
    if len(res) != len(document['subtitle']['elements']):
        sys.exit("FATAL ERROR :: [len(res) != len(document['subtitle']['elements'] [{}]]".format(__file__) )
    #--------------------------------------------------------------------------#

    #--------------------------------------------------------------------------#
    for x in range(len(res)):
        for idx in range(len(res[x])):
            if re.search( document['subtitle']['html_tag'], str(res[x][idx]) ) is None: 
                document['subtitle']['elements'][x]['content'].append({ "dom" : (res[x][idx]) } )
    #--------------------------------------------------------------------------#

    return document
#---------------------------------------------------------------------#




#---------------------------------------------------------------------#
def get_text(document):

    #------------------------------------------------------------#
    for element in  document['subtitle']['elements']:
        for content_item in element['content']:
            dom = content_item['dom']

            if dom.name == 'ul' or dom.name == 'ol':
                content_item['text'] = parse_ul_to_json(dom)

            elif dom.name == 'p' or dom.name == 'h5' or dom.name == 'h3':
                content_item['text'] = [dom.get_text()]

            elif dom.name == 'em':
                content_item['text'] = [dom.get_text()]

            elif dom.name == 'table':
                content_item['text'] = ''
                content_item['table'] = parse_table_to_json(dom)

            elif dom.name == 'div' and re.search(r'class="col-sm-3.*', str(dom)):
                content_item['text'] = [dom.get_text()]

            else:
                content_item['text'] = ''
    #-----------------------------------------------------------#

    return document
#---------------------------------------------------------------------#
