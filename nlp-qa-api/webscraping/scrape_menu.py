import requests
from bs4 import BeautifulSoup
import json
from configparser import ConfigParser


scrollbar_menus = []
result = []


def unordered_elements(unordered_dict,menu,element):
    for e in element.contents:
        val = parse_list(e)
        if val is not None:
            unordered_dict[menu].append(val)
    return unordered_dict

def check_unorder(element):
    if element.name == 'ul' or element['href'] == 'https://indianbank.in/#':
        return True
    return False


def parse_list(elements):
    global result
    for element in elements.contents: 
        dictionary = None
        if not check_unorder(element):
            menu = element.get_text()
            dictionary = ({menu:element.get('href')})
        else:
            if element.name == 'a':
                menu = element.get_text()
                continue
            unordered_dict = {menu:[]}
            if result != []:
                key = list(result[0].keys())[0]
                result[0][key].append(unordered_dict)
            else:
                result.append(unordered_dict)
            unordered_elements(unordered_dict,menu,element)

    return dictionary

def scrape_scrollbar_menu(url,id):
    try:

        response = requests.get(url,verify=False)

        html = response.content

        soup = BeautifulSoup(html,features='html5lib')

        main_content = soup.find('ul',attrs={"id":id})

        global result

        for element in main_content.contents: #
            contents = parse_list(element)
            if contents is not None:
                scrollbar_menus.append(contents)
            if len(result) is not 0:
                scrollbar_menus.append(result[0])
            result = []
        return scrollbar_menus
    except (requests.exceptions.ConnectionError, ConnectionResetError):
        print("exc")
    except Exception as e:
        print(e.args)