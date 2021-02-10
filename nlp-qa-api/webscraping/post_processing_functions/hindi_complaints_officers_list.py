def hindi_complaints_officers_list(document):
    record = document['subtitle']['elements']
    contents = record[0]['content']
    contents = [e for e in contents if e != ""]

    for i,e in enumerate(contents):
        document['subtitle']['elements'].append({'text':e['text'],'content':[{"text":[e['dom'].text]}]})

   
    del document['subtitle']['elements'][0]
    return document
