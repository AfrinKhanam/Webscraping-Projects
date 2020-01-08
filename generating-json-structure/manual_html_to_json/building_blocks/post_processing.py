def post_processing(document):

    if document['document_name'] == 'ib-vehicle-loan':
        ib_vehicle_loan(document)

    elif document['document_name'] == 'ib-home-loan':
        ib_home_loan(document)

    elif document['document_name'] == 'ib-doctor-plus':
        ib_doctor_plus(document)

    elif document['document_name'] == 'ind-sme-e-vaahan':
        ind_sme_e_vaahan(document)

    elif document['document_name'] == 'lending-rates':
        lending_rates(document)

    elif document['document_name'] == 'ind-mortgage':
        ind_mortgage(document)

    elif document['document_name'] == 'ib-clean-loan-to-salaried-class':
        clean_loan(document)

    return document

def clean_loan(document):
    record = document['subtitle']['elements'][0]['content'][0]['table'][2]['value'][0]['table']['values']

    record[2].append(record[1][2])

    return document


def ind_mortgage(document):
    #-----------------------------------------------------------------------#
    record = document['subtitle']['elements'][0]['content'][0]['table'][0]

    inner_subtitle_1 = record['value'][0]
    inner_subtitle_2 = record['value'][5]

    record['value'].remove(inner_subtitle_1)
    record['value'].remove(inner_subtitle_2)

    record['value'][0] = inner_subtitle_1 + record['value'][0]
    record['value'][1] = inner_subtitle_1 + record['value'][1]
    record['value'][2] = inner_subtitle_1 + record['value'][2]
    record['value'][3] = inner_subtitle_1 + record['value'][3]


    record['value'][4] = inner_subtitle_2 + record['value'][4]
    #-----------------------------------------------------------------------#


    #-----------------------------------------------------------------------#
    record = document['subtitle']['elements'][0]['content'][0]['table'][2]

    inner_subtitle_1 = record['value'][0]
    inner_subtitle_2 = record['value'][3]
    inner_subtitle_3 = record['value'][7]

    record['value'].remove(inner_subtitle_1)
    record['value'].remove(inner_subtitle_2)
    record['value'].remove(inner_subtitle_3)

    record['value'][0] = inner_subtitle_1 + '\n' + record['value'][0]
    record['value'][1] = inner_subtitle_1 + '\n' +  record['value'][1]


    record['value'][2] = inner_subtitle_2 + '\n' + record['value'][2]
    record['value'][3] = inner_subtitle_2 + '\n' + record['value'][3]

    record['value'][5] = inner_subtitle_3 + '\n ' + record['value'][5]
    record['value'][6] = inner_subtitle_3 + '\n ' + record['value'][6]
    #-----------------------------------------------------------------------#


    return document


def lending_rates(document):
    table = document['subtitle']['elements'][0]['content'][0]['table']

    for record in table:
        record['value'] += [ record['key'] ]


    return document


def ind_sme_e_vaahan(document):
    record = document['subtitle']['elements'][0]['content'][0]['table'][6]

    inner_subtitle_1 = record['value'][0]
    inner_subtitle_2 = record['value'][2]
    inner_subtitle_3 = record['value'][7]

    record['value'].remove(inner_subtitle_1)
    record['value'].remove(inner_subtitle_2)
    record['value'].remove(inner_subtitle_3)


    record['value'][0] = inner_subtitle_1 + record['value'][0]
    
    record['value'][1] = inner_subtitle_2 + record['value'][1]
    record['value'][2] = inner_subtitle_2 + record['value'][2]
    record['value'][3] = inner_subtitle_2 + record['value'][3]
    record['value'][4] = inner_subtitle_2 + record['value'][4]

    record['value'][5] = inner_subtitle_3 + record['value'][5]

    return document


def ib_doctor_plus(document):
    record = document['subtitle']['elements'][0]['content'][1]['table'][4]

    inner_subtitle_1 = record['value'][0]
    inner_subtitle_2 = record['value'][3]

    record['value'].remove(inner_subtitle_1)
    record['value'].remove(inner_subtitle_2)

    record['value'][0] = inner_subtitle_1 + record['value'][0]
    record['value'][1] = inner_subtitle_1 + record['value'][1]

    record['value'][2] = inner_subtitle_2 + record['value'][2]


    record = document['subtitle']['elements'][0]['content'][1]['table'][6]

    inner_subtitle_1 = record['value'][0]
    inner_subtitle_2 = record['value'][4]

    record['value'].remove(inner_subtitle_1)
    record['value'].remove(inner_subtitle_2)

    record['value'][0] = inner_subtitle_1 + record['value'][0]
    record['value'][1] = inner_subtitle_1 + record['value'][1]
    record['value'][2] = inner_subtitle_1 + record['value'][2]

    record['value'][3] = inner_subtitle_2 + record['value'][3]
    record['value'][4] = inner_subtitle_2 + record['value'][4]
    record['value'][5] = inner_subtitle_2 + record['value'][5]

    return document



def ib_vehicle_loan(document):
    inner_subtitle = document['subtitle']['elements'][0]['content'][0]['table'][0]['value'][0]

    value = document['subtitle']['elements'][0]['content'][0]['table'][0]['value']
    value.remove(inner_subtitle)

    record = document['subtitle']['elements'][0]['content'][0]['table'][0]
    record['key'] += ' : ' + inner_subtitle 


    return document

def ib_home_loan(document):
    inner_subtitle_1 = document['subtitle']['elements'][0]['content'][0]['table'][0]['value'][0]
    inner_subtitle_2 = document['subtitle']['elements'][0]['content'][0]['table'][0]['value'][2]

    value = document['subtitle']['elements'][0]['content'][0]['table'][0]['value']
    value.remove(inner_subtitle_1)
    value.remove(inner_subtitle_2)

    value[0] = inner_subtitle_1 + ' : ' + value[0]
    value[1] = inner_subtitle_2 + ' : ' + value[1]

    del document['subtitle']['elements'][0]['content'][0]['table'][1]['value'][0]
    del document['subtitle']['elements'][0]['content'][0]['table'][3]['value'][0]


    return document
