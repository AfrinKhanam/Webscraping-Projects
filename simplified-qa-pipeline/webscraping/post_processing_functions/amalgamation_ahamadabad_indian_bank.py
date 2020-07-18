import json
def amalgamation_ahamadabad_indian_bank(document):
    #print("*********************",document)
    table_1 = document['subtitle']['elements'][0]['content'][1]['table']
    table_2 = document['subtitle']['elements'][0]['content'][4]['table']

    modify_key_value(table_1)
    modify_key_value(table_2)

    #print("************************",document)
    return document
def modify_key_value(table):
    for record in table:
        key = record['key']
        value = record['value']
        if len(value)==0:
            record['key'] = "" #for tables having 3 columns
            record['value'] = [key]
        else:
            record['key'] = value[0]
            record['value'] = [key]

