from nltk.stem      import PorterStemmer
from nltk.tokenize  import word_tokenize
import json

import re

class PreProcessing():
    def __init__(self):
        self.ps = PorterStemmer()

    def stem(self, text):
        #------------------------------------------#
        text = text.lower()
        words_list = word_tokenize(text)
        #------------------------------------------#

        #------------------------------------------#
        stemmed_list = []
        for word in words_list:
            stemmed_list.append(self.ps.stem(word))
        #------------------------------------------#

        #------------------------------------------#
        stemmed_text = " ".join(stemmed_list)
        #------------------------------------------#

        return stemmed_text

    def removeSpecialCharacters(self,text):
        parsed_string=text.replace('-',' ')
        print("text is------>>  ",parsed_string)
        return parsed_string


    def postProcessing(self,document):

        # print("doc name is ----------->> ",document['document_name'])
        # print("title name is ----------->> ",document['main_title'])
        # print("elements name is ----------->> ",document['subtitle']['elements'])
         #------------- MAIN TITLE Unnecessary Symbols removal -----------------------------#
        document['main_title'] = self.removeSpecialCharacters(document['main_title'])

        for element in document['subtitle']['elements']:

            for _,content in enumerate(element['content']):

                for idx,text in enumerate(content['text']):
                    val=[]
                    val.append(text)
                    print(val)
                    # document['subtitle']['elements']['content']=[self.removeSpecialCharacters(text)]
                    #print(document['subtitle']['elements'][0]['content'])
                    document['subtitle']['elements'][0]['content'][0]['text'] = val
                    # contentData=document['subtitle']['elements']['content']
                    # print("idx------------->",idx)
                    # print("content is--------------------------",self.removeSpecialCharacters(text))
