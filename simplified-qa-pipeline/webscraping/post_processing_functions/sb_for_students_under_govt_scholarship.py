import json

def sb_for_students_under_govt_scholarship(document):
    table = document['subtitle']['elements'][0]['content'][0]['table']
    subtitles = ['PRODUCT','TARGET CUSTOMERS','KYC REQUIREMENT','NATURE OF OPERATION',
                 'AVERAGE MONTHLY MINIMUM BALANCE & CHARGES FOR NON MAINTENANCE OF MIN BAL','SPECIAL FEATURE']
                 
    for idx,subtitle in enumerate(subtitles):
        key = table[idx]['key']
        table[idx]['key'] = subtitle
        table[idx]['value'] = ['&#8226; ' + "".join(key) + '&#8226; ' + "".join(table[idx]['value'])]

    return document
