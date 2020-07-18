from nltk.stem      import PorterStemmer
from nltk.tokenize  import word_tokenize
import json

import re

class PreProcessing():
    def __init__(self):
        self.ps = PorterStemmer()

    # def removeSpecialCharacters(self,text):
    #     print("----------TEXT------------->",text)
    #     parsed_string=text.replace('-',' ')
    #     print("text is------>>  ",parsed_string)
    #     return parsed_string
    def removeSpecialCharacters(self,string):
        string = re.sub('(\?|@|/|#|$|\"|\'|%|\\|&|\*|\(|\)|-|\^|")', ' ', string)
        string = re.sub('\.', ' ', string)
        string = re.sub(':', ' ', string)
        string = (string.replace('[',' ')).replace(']',' ')
        return string

    def process(self,document):

        document['main_title_stem'] = self.removeSpecialCharacters(document['main_title_stem'])

        return document
        
