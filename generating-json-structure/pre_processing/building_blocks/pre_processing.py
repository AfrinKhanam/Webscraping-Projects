from nltk.stem      import PorterStemmer
from nltk.tokenize  import word_tokenize
import json

import re

class PreProcessing():
    def __init__(self):
        self.ps = PorterStemmer()

    def removeSpecialCharacters(self,text):
        print("----------TEXT------------->",text)
        parsed_string=text.replace('-',' ')
        print("text is------>>  ",parsed_string)
        return parsed_string


    def postProcessing(self,document):

        # print("doc name is ----------->> ",document['document_name'])
        # print("title name is ----------->> ",document['main_title'])
        # print("elements name is ----------->> ",document['subtitle']['elements'])
         #------------- MAIN TITLE Unnecessary Symbols removal -----------------------------#
        document['main_title'] = self.removeSpecialCharacters(document['main_title'])
        
        # val=[]
        # for element in document['subtitle']['elements']:

        #     for _,content in enumerate(element['content']):

        #         for idx,text in enumerate(content['text']):
                    
        #             val.append(text)
        #             print("value is--->>",val)
        #             # document['subtitle']['elements']['content']=[self.removeSpecialCharacters(text)]
        #             #print(document['subtitle']['elements'][0]['content'])
        # document['subtitle']['elements'][0]['content'][0]['text'] = val
                    # contentData=document['subtitle']['elements']['content']
                    # print("idx------------->",idx)
                    # print("content is--------------------------",self.removeSpecialCharacters(text))
