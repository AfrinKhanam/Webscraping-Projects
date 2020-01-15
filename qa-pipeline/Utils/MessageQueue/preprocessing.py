class Preprocessing:

    def __init__(self):
        pass
    
    def remove_punctuation(self,query):
        # print("------------QUERY-------------",query[''])
        hypenQuery=self.remove_hyphen(query)
        return hypenQuery

    def remove_hyphen(self,query):
        query['QUERY_STRING']=query['QUERY_STRING'].replace('-',' ')
        query['PARSED_QUERY_STRING'].replace('-',' ')
        query['POTENTIAL_QUERY_LIST'].replace('-',' ')
        query['AUTO_CORRECT_QUERY'].replace('-',' ')
        query['CORRECT_QUERY'].replace('-',' ')

        print("After removing hyphen----------->> ",query['QUERY_STRING'])
        print("After removing hyphen----------->> ",query['PARSED_QUERY_STRING'])
        print("After removing hyphen----------->> ",query['POTENTIAL_QUERY_LIST'])
        print("After removing hyphen----------->> ",query['AUTO_CORRECT_QUERY'])
        print("After removing hyphen----------->> ",query['CORRECT_QUERY'])

        return query
