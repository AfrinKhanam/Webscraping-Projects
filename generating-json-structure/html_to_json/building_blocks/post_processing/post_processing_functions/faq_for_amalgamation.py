import json
def faq_for_amalgamation(document):
    # json.dumps(document,indent=4)
    table = document['subtitle']['elements'][0]['content'][0]['table']

    for record in table:
        record['value'] += [ record['key'] ]


    return document
