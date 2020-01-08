def attach_email_subtitle(document):
    if document['subtitle']['elements'][0]['text'] == '':
        document['subtitle']['elements'][0]['text'] = 'EMAIL ADDRESS'


    return document
