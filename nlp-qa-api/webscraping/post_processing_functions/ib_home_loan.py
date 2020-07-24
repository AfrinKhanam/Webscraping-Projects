# def ib_home_loan(document):
#     inner_subtitle_1 = document['subtitle']['elements'][0]['content'][0]['table'][0]['value'][0]
#     inner_subtitle_2 = document['subtitle']['elements'][0]['content'][0]['table'][0]['value'][2]

#     value = document['subtitle']['elements'][0]['content'][0]['table'][0]['value']
#     value.remove(inner_subtitle_1)
#     value.remove(inner_subtitle_2)

#     value[0] = inner_subtitle_1 + ' : ' + value[0]
#     value[1] = inner_subtitle_2 + ' : ' + value[1]

#     del document['subtitle']['elements'][0]['content'][0]['table'][1]['value'][0]
#     del document['subtitle']['elements'][0]['content'][0]['table'][3]['value'][0]


#     return document
def ib_home_loan(document):
    record = document['subtitle']['elements'][0]['content'][0]['table'][13]
    record['key'] = document['subtitle']['elements'][0]['content'][0]['table'][13]['key']
    value = []
    for e in document['subtitle']['elements'][0]['content'][0]['table'][13]['value'][0]['table']['values']:
        value.append(e[0])
    record['value'] = value

    return document
