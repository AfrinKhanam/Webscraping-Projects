def hindi_multicity_cheque_facility(document):
    record = document['subtitle']['elements']
    contents = record[0]['text'].split("\n\n\n\n")
    print(contents)
    contents = [e for e in contents if e != ""]

    record[0]['text'] = ""
    record[0]['content'] = [{
        "text": "",
        "table": [

        ]
    }]
    for idx, ele in enumerate(contents):
        if idx % 2 == 0:
            record[0]['content'][0]['table'].append({"key":ele})

        else:
            length = len(record[0]['content'][0]['table'])

            record[0]['content'][0]['table'][length-1]["value"] = [ele]
    del record[0]['content'][0]['table'][3]
    return document