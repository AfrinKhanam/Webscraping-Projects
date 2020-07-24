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
