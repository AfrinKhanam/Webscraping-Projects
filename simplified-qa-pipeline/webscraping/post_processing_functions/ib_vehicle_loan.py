def ib_vehicle_loan(document):
    inner_subtitle = document['subtitle']['elements'][0]['content'][0]['table'][0]['value'][0]

    value = document['subtitle']['elements'][0]['content'][0]['table'][0]['value']
    value.remove(inner_subtitle)

    record = document['subtitle']['elements'][0]['content'][0]['table'][0]
    record['key'] += ' : ' + inner_subtitle 


    return document
