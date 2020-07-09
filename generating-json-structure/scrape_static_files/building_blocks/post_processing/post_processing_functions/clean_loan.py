def clean_loan(document):
    record = document['subtitle']['elements'][0]['content'][0]['table'][2]['value'][0]['table']['values']

    record[2].append(record[1][2])

    return document
