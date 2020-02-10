# importing the requests library 
import requests 
import re
from buiding_blocks.update_synonym import UpdateSynonyms
import time
  
synonyms=UpdateSynonyms()

while True:
    synonyms.fetchSynonyms()
    time.sleep(60)



