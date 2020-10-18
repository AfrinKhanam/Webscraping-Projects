from bs4 import BeautifulSoup

class ScrapeMenu:
    def __init__(self,es,es_index):
        self.es = es
        self.scrollbar_menus =  []
        self.result = []
        self.es_index = es_index
        self.doc = dict()


    def parse_unordered_elements(self,unordered_dict,menu,element):
        for e in element.contents:
            val = self.parse_list(e)
            if val is not None:
                unordered_dict[menu].append(val)
        return unordered_dict

    def check_unorder(self,element):
        if element.name == 'ul' or element['href'] == self.doc['url']:
            return True
        return False


    def parse_list(self,elements):
        for element in elements.contents: 
            dictionary = None
            if not self.check_unorder(element):
                menu = element.get_text()
                dictionary = ({menu:element.get('href')})
            else:
                if element.name == 'a':
                    menu = element.get_text()
                    continue
                unordered_dict = {menu:[]}
                if self.result != []:
                    key = list(self.result[0].keys())[0]
                    self.result[0][key].append(unordered_dict)
                else:
                    self.result.append(unordered_dict)
                self.parse_unordered_elements(unordered_dict,menu,element)

        return dictionary

    def scrape_scrollbar_menu(self,document,html):
        self.doc = document
        soup = BeautifulSoup(html,features='html5lib')
        main_content = soup.find('ul',attrs={"id":document['pageConfig']['id']})
        for element in main_content.contents: #
            contents = self.parse_list(element)
            if contents is not None:
                self.scrollbar_menus.append(contents)
            if len(self.result) is not 0:
                self.scrollbar_menus.append(self.result[0])
            self.result = []
        return self.scrollbar_menus

    def index_document(self,menu):
        self.es.index(index=self.es_index,doc_type='_doc',id="menu_items",body={'ib_menu':menu})
        print("menu items pushed into elastic db")
