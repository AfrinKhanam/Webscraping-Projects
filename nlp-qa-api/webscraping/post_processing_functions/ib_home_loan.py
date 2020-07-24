def ib_home_loan(document):
    record = document['subtitle']['elements'][0]['content'][0]['table'][13]
    record['key'] = document['subtitle']['elements'][0]['content'][0]['table'][13]['key']
    value = []
    for e in document['subtitle']['elements'][0]['content'][0]['table'][13]['value'][0]['table']['values']:
        value.append(e[0])
    record['value'] = value

    return document
