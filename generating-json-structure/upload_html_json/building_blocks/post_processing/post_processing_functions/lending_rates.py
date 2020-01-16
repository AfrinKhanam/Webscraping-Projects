def lending_rates(document):
    table = document['subtitle']['elements'][0]['content'][0]['table']

    for record in table:
        record['value'] += [ record['key'] ]


    return document
