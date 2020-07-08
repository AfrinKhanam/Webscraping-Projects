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
