def hindi_general_managers(document):
    records = document['subtitle']['elements']

    doc = []

    for record in records:
        for ele in record['content']:
            if ele['dom'].name != None:
                val = (ele['dom'].find('span'))
                contents = val.contents
                for idx,i in enumerate(contents):
                    if i.name == 'br':
                        del contents[idx]

                contents[1] = "\n".join(contents[1:len(contents)])
                # print(contents )
            # for idx,val in enumerate(contents):
            doc.append({"text":contents[1],"content":[{"text":[contents[0]]}]})
                # del document['subtitle']['elements']['indices']

    # print(doc)
    document['subtitle']['elements'] = doc
    # print(document)

    return document