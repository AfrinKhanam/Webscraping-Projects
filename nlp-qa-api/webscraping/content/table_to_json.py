#---------------------------------------------------------------------#
from webscraping.utils import strip_tags
from webscraping.content.paragraph_to_json import parse_paragraph
from webscraping.content.list_to_json import parse_ul_to_json
#---------------------------------------------------------------------#

#-----------------------------------------------------------------------------------#
def parse_inner_table(dom):
    text = {"table" : {"keys" : [], "values" : []} }

    #--------------------------------------------------------------#
    for ele in dom.contents:


        if ele.name is not None:
            table_dom = ele.find('table')

            if table_dom:
                return parse_inner_table(table_dom)

            text['table']['keys'] = get_inner_table_keys(ele)
    #--------------------------------------------------------------#

    #--------------------------------------------------------------#
    for ele in dom.contents:
        if ele.name is not None:
            text['table']['values'] = get_inner_table_values(ele)
    #--------------------------------------------------------------#

    return text
#-----------------------------------------------------------------------------------#




#-----------------------------------------------------------------------------------#
def get_inner_table_values(ele):
    result = []

    underline = ele.find('u')
    if underline:
        #-----------------------------------------------------#
        for value in ele.contents:
            if value.name is not None: 
                result.append(value.contents[3].get_text())
        #-----------------------------------------------------#
    else:
        #--------------------------------------------------------------------------#
        record = [ record for record in ele.contents if record.name is not None]
        record = record[1:]

        for column in record:
            result.append([value.get_text() for value in column.contents if value.name is not None])
        #--------------------------------------------------------------------------#

    return result
#-----------------------------------------------------------------------------------#



#-----------------------------------------------------------------------------------#
def get_inner_table_keys(ele):
    keys = []


    underline = ele.find('u')
    if underline:
        #-----------------------------------------------------#
        for value in ele.contents:
            if value.find('u') and value.name is not None: 
                keys.append(value.contents[1].get_text())
        #-----------------------------------------------------#
    else:
        #--------------------------------------------------------------------------#
        keys = [key.get_text() for key in ele.contents[1].contents if key.name is not None]
        #--------------------------------------------------------------------------#


    return keys
#-----------------------------------------------------------------------------------#





#-----------------------------------------------------------------------------------#
def parse_table_to_json(dom):
    # ------------------------------------------------------------------------#
    table_data = []

    for child in dom.children:
        if child.name is not None:
            row_list = [ele for ele in child.contents if ele.name is not None]
            for row in row_list:

                idx = 0
                key = ''
                value = []

                for column in row:
                    if column.name is not None:
                        idx += 1
                        if idx % 2 == 0:
			
                            column = replace_tag(column)

                            for ele in column.contents:
                                if ele.name == 'ul' or ele.name == 'ol':
                                    value += parse_ul_to_json(ele)

                                elif ele.name == 'p':
                                    text = parse_paragraph(ele)
                                    if text != None: value.append(text)

                                elif ele.name == 'table':
                                    value.append(parse_inner_table(ele))

                                elif ele.name:
                                    value.append(ele.get_text())

                                elif isinstance(ele, str) and len(ele.strip()) > 0:
                                    value.append(ele)
                        else:
                            if column.find('table'):
                                key = ""
                                return parse_table_to_json(column.find('table'))

                            else: key = column.get_text()

                table_data.append({ "key" : key, "value" : value})

    return table_data
#-----------------------------------------------------------------------------------#


#---------------------------------------------------------------------#
def replace_tag(dom):
    dom = strip_tags(dom, ['span', 'strong', 'em', 'a', 'h4', 'u', 'b', 'i'])

    return dom
#---------------------------------------------------------------------#
