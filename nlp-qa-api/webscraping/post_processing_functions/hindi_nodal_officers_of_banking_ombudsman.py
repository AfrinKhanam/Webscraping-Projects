def hindi_nodal_officers_of_banking_ombudsman(document):
    record = document['subtitle']['elements']
    record[0]['content'].append({'text':[record[0]['text']]})
    record[0]['text'] = ''
    del record[0]['content'][0]
    return document
