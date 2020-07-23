def clean_loan(document):
    record = document['subtitle']['elements'][0]['content'][0]['table'][3]
    record['key'] = document['subtitle']['elements'][0]['content'][0]['table'][3]['key']
    record['value'] = document['subtitle']['elements'][0]['content'][0]['table'][3]['value'][0]['table']['values'][0]
    return document 

