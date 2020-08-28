def hindi_capital_gains(document):
    record = document['subtitle']['elements']
    contents = record[0]['text'].split("\n")
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

    return document
