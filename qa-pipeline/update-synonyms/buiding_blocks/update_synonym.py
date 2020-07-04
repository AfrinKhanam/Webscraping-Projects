import requests 
import re
import pathlib
import os
from configparser import ConfigParser

class UpdateSynonyms:
    def __init__(self):
        # api-endpoint 
        config_file_path = '../../config.ini'
        config = ConfigParser()
        config.read(config_file_path)
        self.url=config['urls']['synonyms_url']
        self.ROOT_DIR = os.path.abspath(os.pardir)
    
    def fetchSynonyms(self):

        try:
            self.r = requests.get(url = self.url) 
             # extracting data in json format 
            self.data = self.r.json()

            synonyms=[]
            for w in self.data:
                # print(re.sub(",","=",w))
                synonyms.append(re.sub(",","=",w))

            value="\n".join(synonyms)
            print("synonyms------>> ",value)
            file = open(self.ROOT_DIR+"/1-query-parser/config_files/synonyms.txt","w+") 
            file.write(value)
            file.close() 
            # print(file)
        except Exception as e:
            print("exception occurred..!!",e)