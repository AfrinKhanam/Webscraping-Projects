def hindi_vision_mission(document):
    record = document['subtitle']['elements']
    record[0]['content'].append({'text':[record[0]['text']]})
    record[0]['text'] = ''
    
    return document
