def ca_for_state_central_govt(document):
    #print("*********************",document)
    #print("hiiiiiiiiiiiii")
    table_1 = document['subtitle']['elements'][0]['content'][0]['table']
    table_2 = document['subtitle']['elements'][1]['content'][1]['table']

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

    #print("000000000000000000000000",table)

