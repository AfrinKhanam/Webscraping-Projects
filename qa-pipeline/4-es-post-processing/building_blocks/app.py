import re
text="quantum of loan"
preposition_words=["on","for","of","by","over","at","from"]
for p in preposition_words:
    try:

        result=text.index(p)
        text=text[:result]
        print(text)
    except:
        pass







   
