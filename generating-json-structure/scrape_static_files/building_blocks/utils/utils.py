from bs4 import BeautifulSoup, NavigableString
import re

#--------------------------------------------------------#
def list_split(input_list, index_list):
    res = [input_list[i : j] for i, j in zip([0] +
          index_list, index_list + [None])]

    if len(res[0]) == 0:
        del res[0]

    return  res
#--------------------------------------------------------#



#--------------------------------------------------------#
def strip_tags(dom, invalid_tags):

    for tag in invalid_tags:
        #print("dom :::: ", dom)
        tag = dom.find(tag)
        if tag:
            tag_name = dom.name
            print(tag_name)

            dom = re.sub(r'(<strong>|</strong>)', ' ', str(dom))
            dom = re.sub(r'(<span.*">|</span>)', ' ', str(dom))
            dom = re.sub(r'(<a .*">|</a>)', ' ', str(dom))
            dom = re.sub(r'(<h4>|</h4>)', ' ', str(dom))
            dom = re.sub(r'(<u>|</u>)', ' ', str(dom))
            dom = re.sub(r'(<b>|</b>)', ' ', str(dom))

            if tag_name == 'td':
                dom = '<table>' + dom  + '</table>'
                soup = BeautifulSoup(dom, features='html5lib')
                dom = soup.find(tag_name)

                #print('RESULT AFTER TAG STRIP :: ', dom)

    return dom
#--------------------------------------------------------#
