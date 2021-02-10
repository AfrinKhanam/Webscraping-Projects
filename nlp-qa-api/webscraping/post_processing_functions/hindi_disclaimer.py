def hindi_disclaimer(document):
    record = document['subtitle']['elements']
    record[0]['content'].append({'text':[record[0]['text']]})
    record[0]['text'] = ''
    
    return document
