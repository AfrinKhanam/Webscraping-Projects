def ib_vehicle_loan(document):
    record = document['subtitle']['elements'][0]['content'][0]['table'] 
    record[0]['key'] =  record[0]['key']
    record[0]['value'] = record[0]['value']
    record[0]['value'].append(record[4]['key'])

    record[1]['key'] = record[1]['key'] +" "+ record[1]['value'][0]
    record[1]['value'] = [record[2]['key'] +" " +"".join(record[2]['value'])]
    record[1]['value'].insert(1, record[3]['key'] +" " +"".join(record[3]['value']))

    record[6]['value'] = [" ".join(record[6]['value'])]
    
    for i in range(7,16):
        del record[i]

    return document
