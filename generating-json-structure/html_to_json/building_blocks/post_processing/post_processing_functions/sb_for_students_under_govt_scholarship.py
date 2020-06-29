import json
def sb_for_students_under_govt_scholarship(document):
    #print("*********************",document)

    table = document['subtitle']['elements'][0]['content'][0]['table']
    subtitles = ['PRODUCT','TARGET CUSTOMERS','KYC REQUIREMENT','NATURE OF OPERATION','AVERAGE MONTHLY MINIMUM BALANCE & CHARGES FOR NON MAINTENANCE OF MIN BAL','SPECIAL FEATURE']
    for idx,subtitle in enumerate(subtitles):
        table[idx]['key'] = subtitle + '-' + table[idx]['key']
        table[idx]['value'] = subtitle + '-' + table[idx]['value'][0]

        #for record in table:
        
            #key = record['key']
            #value = record['value']
            #record['key'] = key
            #print(record['key'])
            #record['value'] = value

    print("************************",table)
    return document

