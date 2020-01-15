import requests 
import re
import pathlib
import os
class UpdateSynonyms:
    def __init__(self):
        # api-endpoint 
        self.url="http://localhost:7512/Synonyms/GetAllWordsCsv"
        ROOT_DIR = os.path.abspath(os.pardir)
        # self.path=pathlib.Path(__file__)._make_child_relpath
        print("--> ",ROOT_DIR+"/1-query-parser/config_files/synonyms.txt")


        # sending get request and saving the response as response object 
        try:

            self.r = requests.get(url = self.url) 
             # extracting data in json format 
            self.data = self.r.json()

            synonyms=[]
            for w in self.data:
                print(re.sub(",","=",w))
                synonyms.append(re.sub(",","=",w))

            value="\n".join(synonyms)
            file = open(ROOT_DIR+"/1-query-parser/config_files/synonyms.txt","w") 
            file.write(value)
            file.close() 
            print(file)
        except:
            print("exception occurred..!!")


       



