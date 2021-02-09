def hindi_nodal_officers(document):
    record = document['subtitle']['elements']
    record[0]['content'].append({'text':[record[0]['text']]})
    record[0]['text'] = ''
    
    return document
