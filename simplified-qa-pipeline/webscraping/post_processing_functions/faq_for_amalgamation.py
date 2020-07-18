import json
def faq_for_amalgamation(document):
    #print("*********************",document)
    table = document['subtitle']['elements'][0]['content'][0]['table']

    for record in table:
        key = record['key']
        value = record['value']
        record['key'] = value[0] #for tables having 3 columns
        record['value'] = [key]

    #print("************************",document)
    return document

