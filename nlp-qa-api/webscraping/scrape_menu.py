import re
from typing import List

from bs4 import BeautifulSoup
from bs4.element import Tag

_RE_COMBINE_WHITESPACE = re.compile(r"\s+")
_RE_REMOVE_NON_ALPHANUMERIC = re.compile(r'[^A-Za-z0-9 ]+')

class ScrapeMenu:
    def __init__(self, es, es_index):
        self.es = es
        self.es_index = es_index

    def __clean_text__(self, text: str):
        # Remove non alphanumeric characters
        retval = _RE_REMOVE_NON_ALPHANUMERIC.sub('', text)

        # Substitute multiple whitespaces with single whitespace
        retval = _RE_COMBINE_WHITESPACE.sub(' ', retval)

        # Finally, trim the string by removing any leading / trailing whitespace characters
        retval = retval.lstrip().rstrip()

        return retval

    def __parse_menu__(self, ele, menu_items_to_ignore: List[str] = []):
        if not isinstance(ele, Tag) or ele.name != 'ul':
            return None

        menu_items = []

        for item in ele.contents:
            if not isinstance(item, Tag) or item.name != 'li':
                continue

            link_content = item.find('a')

            text = self.__clean_text__(link_content.get_text())

            if text in menu_items_to_ignore:
                continue

            menu_item = {'text': text}

            child_menu_content = item.find('ul')

            if child_menu_content is not None:
                menu_item['childItems'] = self.__parse_menu__(child_menu_content)
            else:
                menu_item['url'] = link_content['href']

            menu_items.append(menu_item)

        return menu_items

    def __tag_parents__(self, menu_items):
        def __process_children(parent):
            if 'childItems' in parent.keys() and len(parent['childItems']) > 0:
                for child in parent['childItems']:
                    if 'parents' not in parent.keys():
                        child['parents'] = [parent['text']]
                    else:
                        child['parents'] = parent['parents'] + [parent['text']]

                    __process_children(child)

        for item in menu_items:
            __process_children(item)

        return menu_items

    def scrape_scrollbar_menu(self, document, html: str):
        self.doc = document

        soup = BeautifulSoup(html, features='html5lib')

        pageConfig = document['pageConfig']

        main_content: Tag = soup.find(
            'ul', attrs={"id": pageConfig['id']})

        menu_items_to_ignore: List[str] = None

        if 'menuItemsToIgnore' in pageConfig.keys():
            menu_items_to_ignore = pageConfig['menuItemsToIgnore']

        menu_items = self.__parse_menu__(main_content, menu_items_to_ignore)

        menu_items = self.__tag_parents__(menu_items)

        return menu_items

    def index_document(self, menu):
        self.es.index(index=self.es_index, doc_type='_doc',
                      id="menu_items", body={'ib_menu': menu})

        print("menu items pushed into elastic db")
