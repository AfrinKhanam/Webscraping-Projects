def hindi_agriculture_godowns(document):
    record = document['subtitle']['elements']
    contents = record[0]['text'].split("\n")
    contents = [e for e in contents if e != ""]
    # subtitles = [e for idx, e in enumerate(contents) if idx % 2 == 0]
    # values = [e for idx, e in enumerate(contents) if idx % 2 != 0]

    # print(subtitles)
    # print(values)

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
    # print(document['subtitle']['elements'])

    return document
