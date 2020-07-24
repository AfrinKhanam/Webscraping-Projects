from nltk.stem import PorterStemmer
from nltk.tokenize import word_tokenize
from bs4.element import Tag
import re

class Stemmer():
    def __init__(self):
        self.ps = PorterStemmer()

    def stem(self, text):
        #------------------------------------------#
        text = text.lower()
        words_list = word_tokenize(text)
        #------------------------------------------#

        #------------------------------------------#
        stemmed_list = []
        for word in words_list:
            stemmed_list.append(self.ps.stem(word))
        #------------------------------------------#

        #------------------------------------------#
        stemmed_text = " ".join(stemmed_list)
        #------------------------------------------#

        return stemmed_text

    def w_stem(self, document):
        #------------- MAIN TITLE STEMMING -----------------------------#
        document['main_title_stem'] = self.stem(document['main_title'])
        #---------------------------------------------------------------#

        #------------- DOMAIN KEY STEMMING -----------------------------#
        self.stem_domain(document)
        #---------------------------------------------------------------#

        #------------- CLASS KEY STEMMING ------------------------------#
        self.stem_class(document)
        #---------------------------------------------------------------#

        #------------- CONTENT TEXT STEMMING ---------------------------#
        for element in document['subtitle']['elements']:
            for _, content in enumerate(element['content']):
                content['text_stem'] = []
                for idx, text in enumerate(content['text']):
                    content['text_stem'].append(self.stem(text))
        #---------------------------------------------------------------#

        self.stem_subtitle(document)
        self.stem_table(document)

        return document

    def stem_subtitle(self, document):
        #-------------- SUBTITLE TEXT STEMMING -------------------------#
        for element in document['subtitle']['elements']:
            element['text_stem'] = self.stem(element['text'])
        #---------------------------------------------------------------#

        return document

    def stem_table(self, document):
        #-------------- TABLE KEY/VALUE  STEMMING ----------------------#
        for element in document['subtitle']['elements']:
            for record in element['content']:
                if 'table' in record:
                    for row in record['table']:
                        row['value_stem'] = []

                        regex_key = re.sub(r'/', ' ', row['key'])

                        row['key_stem'] = self.stem(regex_key)

                        for value in row['value']:
                            if isinstance(value, dict) and 'table' in value:
                                row['value_stem'].append({ "table_stem": self.stem_inner_table(value) })
                            elif isinstance(value, Tag):
                                inner_text = value.get_text()
                                row['value_stem'] += [self.stem(inner_text)]
                            elif isinstance(value, str):
                                row['value_stem'] += [self.stem(value)]
                            else:
                                forced_str = str(value)
                                row['value_stem'] += [self.stem(forced_str)]
                                
        #---------------------------------------------------------------#

        return document

    def stem_inner_table(self, inner_table):
        #-------------- INNER TABLE KEY/VALUE  STEMMING ----------------------#
        table_stem = {"keys_stem": [], "values_stem": []}

        keys = inner_table['table']['keys']
        values = inner_table['table']['values']

        table_stem['keys_stem'] = [self.stem(value) for value in keys]

        for record in values:
            table_stem['values_stem'].append(
                [self.stem(value) for value in record])
        #---------------------------------------------------------------------#

        return table_stem

    def stem_domain(self, document):

        document['domain_stem'] = self.stem(document['domain'])

        return document

    def stem_class(self, document):

        document['class_stem'] = self.stem(document['class'])

        return document

    def removeSpecialCharacters(self, text):
        parsed_string = text.replace('-', ' ')
        return parsed_string


if __name__ == "__main__":
    stem = Stemmer()
    parsed_string = stem.removeSpecialCharacters('OFFICE-INDIAN BANK MUTUAL FUND')
    print(stem.stem(parsed_string))
