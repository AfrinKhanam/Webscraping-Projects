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
