def sb_for_central_state_govt(document):
    table_1 = document['subtitle']['elements'][0]['content'][0]['table']
    table_2 = document['subtitle']['elements'][1]['content'][0]['table']

    modify_key_value(table_1)
    modify_key_value(table_2)

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
