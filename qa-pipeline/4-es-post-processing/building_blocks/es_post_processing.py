import re
import numpy as np
import json
import base64
from nltk.stem import PorterStemmer             

class ESPostProcessing:
    def __init__(self):
      pass



    def create_regex(self, string):
        # ------------------------------------- #
        word_list = string.split()
        return word_list

    def MainTitlePrioritization(self,document):
        query_list=set(self.create_regex(document['POTENTIAL_QUERY_LIST']))
        print("query list is-->",query_list)

        documents=[]
        docs_length=[]
        sorted_docs=[]

        for doc in document['ES_RESULT']['DOCUMENTS']:
            print("main title is--> ",doc['stemmed_main_title'])
            main_title_words=doc['stemmed_main_title'].split()
            main_title=set(main_title_words)
            print("main title set is--> ",main_title)

            matched_maintitle_words=query_list.intersection(main_title)
            print("matched main titles--->",matched_maintitle_words)
            maintitle_words_length=len(matched_maintitle_words)
            documents.append(doc)
            docs_length.append(maintitle_words_length)
            doc['matched_maintitle_words']=list(matched_maintitle_words)
        # Create buckets 
        bucket1=[]
        bucket2=[]
        bucketData1=[]
        bucketData2=[]
        bucket1_len=[]
        bucket2_len=[]

        if documents[0]['stemmed_main_title']==documents[1]['stemmed_main_title'] and documents[0]['stemmed_main_title']==documents[2]['stemmed_main_title']:
            print("IF ALL DOCS ARE SAME")
            bucketData1,bucketData2=self.SortMainTitle(bucket1=None,bucket2=None,documents=documents,bucket1_len=None,bucket2_len=None,docs_length=docs_length)
        elif documents[0]['stemmed_main_title']==documents[1]['stemmed_main_title'] or documents[1]['stemmed_main_title']==documents[0]['stemmed_main_title']:
            bucket1.append(documents[0])
            bucket1.append(documents[1])
            bucket2.append(documents[2])
            bucket1_len.append(docs_length[0])
            bucket1_len.append(docs_length[1])
            bucket2_len.append(docs_length[2])
            print("IF 1ST AND 2ND DOCS ARE SAME",documents[0]['stemmed_main_title'],"--->",documents[1]['stemmed_main_title'])
            bucketData1,bucketData2=self.SortMainTitle(bucket1=bucket1,bucket2=bucket2,documents=None,bucket1_len=bucket1_len,bucket2_len=bucket2_len,docs_length=None)
            
        elif documents[0]['stemmed_main_title']==documents[2]['stemmed_main_title']:

            bucket1.append(documents[0])
            bucket1.append(documents[2])
            bucket2.append(documents[1])
            bucket1_len.append(docs_length[0])
            bucket1_len.append(docs_length[2])
            bucket2_len.append(docs_length[1])
            print("IF 1ST AND 3RD DOCS ARE SAME",documents[0]['stemmed_main_title'],"--->",documents[2]['stemmed_main_title'])

            bucketData1,bucketData2=self.SortMainTitle(bucket1=bucket1,bucket2=bucket2,documents=None,bucket1_len=bucket1_len,bucket2_len=bucket2_len,docs_length=None)
        elif documents[1]['stemmed_main_title']==documents[2]['stemmed_main_title'] or documents[2]['stemmed_main_title']==documents[1]['stemmed_main_title']:
            bucket1.append(documents[1])
            bucket1.append(documents[2])
            bucket2.append(documents[0])
            bucket1_len.append(docs_length[1])
            bucket1_len.append(docs_length[2])
            bucket2_len.append(docs_length[0])
            print(json.dumps(bucket1,indent=4))
            print(json.dumps(bucket2,indent=4))

            print("IF 2ND AND 3RD DOCS ARE SAME",documents[1]['stemmed_main_title'],documents[2]['stemmed_main_title'])

            bucketData1,bucketData2=self.SortMainTitle(bucket1=bucket1,bucket2=bucket2,documents=None,bucket1_len=bucket1_len,bucket2_len=bucket2_len,docs_length=None)

            

        elif documents[2]['stemmed_main_title']==documents[0]['stemmed_main_title']:
            bucket1.append(documents[2])
            bucket1.append(documents[0])
            bucket2.append(documents[1])
            bucket1_len.append(docs_length[2])
            bucket1_len.append(docs_length[0])
            bucket2_len.append(docs_length[1])
            print("IF 3RD AND 1ST DOCS ARE SAME",documents[2]['stemmed_main_title'],"--->",documents[0]['stemmed_main_title'])

            bucketData1,bucketData2=self.SortMainTitle(bucket1=bucket1,bucket2=bucket2,documents=None,bucket1_len=bucket1_len,bucket2_len=bucket2_len,docs_length=None)

        else:
            print("IF ALL DOCS ARE DIFFERENT")
            bucketData1,bucketData2=self.SortMainTitle(bucket1=None,bucket2=None,documents=documents,bucket1_len=None,bucket2_len=None,docs_length=docs_length)

        print("bucketData1---> ",json.dumps(bucketData1,indent=4))
        print("bucketData2---> ",json.dumps(bucketData2,indent=4))

        self.SubtitlePrioritization(bucketData1,bucketData2,query_list,document)

        # Removing prepositions from sub title
    def SortMainTitle(self,bucket1,bucket2,documents,bucket1_len,bucket2_len,docs_length):
        # if docs are different
        if bucket1 !=None and bucket2!=None:
            zipped=zip(bucket1,bucket1_len)
            zipped=list(zipped)
            bucketData1=sorted(zipped,key=lambda x: x[1], reverse=True)
            bucketData2=list(zip(bucket2,bucket2_len))
            print("--------------------SORTING BASED ON MAIN TITLE--(DIFFERENT DOCS)-----------------")
            print(json.dumps(bucketData1,indent=4))
            print(json.dumps(bucketData2,indent=4))

            print("---------------------END OF SORTING BASED ON MAIN TITLE------(DIFFERENT DOCS)--------")
            return bucketData1,bucketData2
        else:
        # if docs are same

        # Sort docs in decreasing order based on matched length
            bucketData2=[]
            zipped=zip(documents,docs_length)
            zipped=list(zipped)
            bucketData1=sorted(zipped,key=lambda x: x[1], reverse=True)
            print("--------------------SORTING BASED ON MAIN TITLE-----(SAME DOCS)--------------")
            print(json.dumps(bucketData1,indent=4))
            print("---------------------END OF SORTING BASED ON MAIN TITLE------(SAME DOCS)--------")
            return bucketData1,bucketData2
            
    def SortSubtitle(self):
        pass

    def SubtitlePrioritization(self,bucketData1,bucketData2,query_list,document):
        bucket1_docs=[]
        bucket2_docs=[]

        bucket1_subtitle_lengths=[]
        bucket2_subtitle_lengths=[]

        # Remove Prepositions from subtitle (bucket)
        self.remove_prepositions_from_title(bucketData1,bucketData2)


        if bucketData1!=[] and bucketData2!=[]:
            documents,docs_length=zip(*bucketData1)
            # print(json.dumps(documents,indent=4))
            for doc in documents:
                subtitle_words=set(doc['stemmed_title'].split())
                print("subtitle words is--->>",subtitle_words)
                matched_subtitle_words=subtitle_words.intersection(query_list)
                print("matched subtitle words---> ",matched_subtitle_words)
                matched_subtitle_length=len(matched_subtitle_words)
                bucket1_docs.append(doc)
                bucket1_subtitle_lengths.append(matched_subtitle_length)
                doc['matched_subtitle_words']=list(matched_subtitle_words)
            documents,docs_length=zip(*bucketData2)
            for doc in documents:
                subtitle_words=set(doc['stemmed_title'].split())
                print("subtitle words is--->>",subtitle_words)
                matched_subtitle_words=subtitle_words.intersection(query_list)
                print("matched subtitle words---> ",matched_subtitle_words)
                matched_subtitle_length=len(matched_subtitle_words)
                bucket2_docs.append(doc)
                bucket2_subtitle_lengths.append(matched_subtitle_length)
                doc['matched_subtitle_words']=list(matched_subtitle_words)

            #  # Remove Prepositions from subtitle (bucket)
            # self.remove_prepositions_from_title(bucket1_docs,bucket2_docs)

            bucket1_zipped=zip(bucket1_docs,bucket1_subtitle_lengths)
            bucket1_zipped=list(bucket1_zipped)
            bucket1_subtitle_sorted_docs=sorted(bucket1_zipped,key=lambda x: x[1], reverse=True)
            doc1,length1=zip(*bucket1_subtitle_sorted_docs)

            bucket2_zipped=zip(bucket2_docs,bucket2_subtitle_lengths)
            bucket2_zipped=list(bucket2_zipped)
            bucket2_subtitle_sorted_docs=sorted(bucket2_zipped,key=lambda x: x[1], reverse=True)
            doc2,length2=zip(*bucket2_subtitle_sorted_docs)

            print("------------START OF SUBTITLE SORTING--------(2 SAME DOCS)----------------------") 
            merged_docs=(doc1)+(doc2)      
            
            document['ES_RESULT']['DOCUMENTS']=merged_docs
            print(json.dumps(merged_docs,indent=4))
            # print("DOC1--->> ",json.dumps(doc1,indent=4))
            # print("DOC2--->> ",json.dumps(doc2,indent=4))
            
            print("------------END OF SUBTITLE SORTING--------(2 SAME DOCS)----------------------") 
            self.club_documents(document)
            
        else:
        # print(type(zip(sortedDocs)))
            docs=[]
            subtitle_lengths=[]
            documents,docs_length=zip(*bucketData1)

            # Remove Prepositions from subtitle (bucket)
            self.remove_prepositions_from_title(bucket1_docs=documents,bucket2_docs=None)

            for doc in documents:
                subtitle_words=set(doc['stemmed_title'].split())
                print("subtitle words is--->>",subtitle_words)
                matched_subtitle_words=subtitle_words.intersection(query_list)
                print("matched subtitle words---> ",matched_subtitle_words)
                matched_subtitle_length=len(matched_subtitle_words)
                docs.append(doc)
                subtitle_lengths.append(matched_subtitle_length)
                doc['matched_subtitle_words']=list(matched_subtitle_words)

            zipped=zip(docs,subtitle_lengths)
            zipped=list(zipped)
            # print("zipped---> ",json.dumps(zipped,indent=4))
            # subtitle_sorted_docs=zipped.sort(key=lambda x: x[1],reverse=True)

            result=False
            # if all docs main title is same then do sorting else no sorting its like 3 buckets for 3 different docs
            result=all((doc['stemmed_main_title']== (document['ES_RESULT']['DOCUMENTS'][0]['stemmed_main_title'])) for doc in document['ES_RESULT']['DOCUMENTS'])
            if result:
                subtitle_sorted_docs=sorted(zipped,key=lambda x: x[1], reverse=True)

                docs,subtitle_lengths=zip(*subtitle_sorted_docs)
                print("------------START OF SUBTITLE SORTING--------(SAME DOCS OR DIFFERENT DOCS)----------------------") 
                document['ES_RESULT']['DOCUMENTS']=docs
                print(json.dumps(docs,indent=4))
                print("------------START OF SUBTITLE SORTING--------(SAME DOCS OR DIFFERENT DOCS)----------------------") 
                self.club_documents(document)
                
            else:

                # subtitle_sorted_docs=sorted(zipped,key=lambda x: x[1], reverse=True)
                # print("-----------------SORTING BASED ON SUB TITLE---------------------------")
                # print(json.dumps(subtitle_sorted_docs,indent=4))
                # print("-----------------END OF SORTING BASED ON SUB TITLE---------------------------")
                # unzipping the docs
                docs,subtitle_lengths=zip(*zipped)
                print("------------START OF SUBTITLE SORTING--------(SAME DOCS OR DIFFERENT DOCS)----------------------") 
                document['ES_RESULT']['DOCUMENTS']=docs
                print(json.dumps(docs,indent=4))
                print("------------START OF SUBTITLE SORTING--------(SAME DOCS OR DIFFERENT DOCS)----------------------") 
                self.club_documents(document)

            # print("-----------------SORTING BASED ON SUB TITLE---------------------------")
        # print(json.dumps(docs,indent=4))
        # print("-----------------END OF SORTING BASED ON SUB TITLE---------------------------")
    def club_documents(self,document):
        result=False
        result=all((doc['stemmed_main_title']== (document['ES_RESULT']['DOCUMENTS'][0]['stemmed_main_title']) and doc['stemmed_title']== (document['ES_RESULT']['DOCUMENTS'][0]['stemmed_title']) ) for doc in document['ES_RESULT']['DOCUMENTS'])
        # if all docs main title and subtitle is same
        if result:
            stemmed_value=""
            value=""
            for doc in document['ES_RESULT']['DOCUMENTS']:
                stemmed_value+= doc['stemmed_value']+"\n"
                value+= doc['value']+"\n"
            
            document['ES_RESULT']['DOCUMENTS'][0]['value']=value
            document['ES_RESULT']['DOCUMENTS'][0]['stemmed_value']=stemmed_value
            print("-------------if all docs main title and subtitle is same-----------------")
            print("value is--->",value)

            print("stemmed value is-->",stemmed_value)
            print("After Merging the same docs--->",json.dumps(document,indent=4))
            print("-------------if all docs main title and subtitle is same----(end)-------------")

        # if all docs main title is same but 1 different subtitle
        elif all((doc['stemmed_main_title']== (document['ES_RESULT']['DOCUMENTS'][0]['stemmed_main_title']) ) for doc in document['ES_RESULT']['DOCUMENTS']):
            element_list=[doc['stemmed_title'] for doc in document['ES_RESULT']['DOCUMENTS']]
            print(element_list)
            duplicates=[]
            for doc in document['ES_RESULT']['DOCUMENTS']:
                if element_list.count(doc['stemmed_title'])>1:
                    duplicates.append(doc)
            print("meow--> \n ",json.dumps(duplicates,indent=4))
            print("end of meow")
            if len(duplicates)!=0:
                print("duplicates are present---->>")
                if duplicates[0]['title']==document['ES_RESULT']['DOCUMENTS'][0]['title']:
                    value=""
                    stemmed_value=""
                    for doc in duplicates:
                        stemmed_value+=doc['stemmed_value']+"\n"
                        value+=doc['value']+"\n"

                    document['ES_RESULT']['DOCUMENTS'][0]['stemmed_value']=stemmed_value
                    document['ES_RESULT']['DOCUMENTS'][0]['value']=value
                    print("----------if all docs main title is same but 1 different subtitle............")
                    print(document['ES_RESULT']['DOCUMENTS'][0]['value'])
                    print("----------if all docs main title is same but 1 different subtitle......(end)......")

        else:
            # if any 2 docs main title and subtitle is same
            duplicates=[]
            for idx,doc in enumerate(document['ES_RESULT']['DOCUMENTS']):
                print("doc--->",doc['main_title'])
                if idx==len(document['ES_RESULT']['DOCUMENTS'])-1:
                    idx=-1
                    print("entering if statement after reaching end of the list",idx)
                    
                if doc['stemmed_main_title']==document['ES_RESULT']['DOCUMENTS'][idx+1]['stemmed_main_title'] and doc['stemmed_title']==document['ES_RESULT']['DOCUMENTS'][idx+1]['stemmed_title']:
                    print("appending to the list",doc['main_title'],"----",document['ES_RESULT']['DOCUMENTS'][idx+1]['main_title'])
                    duplicates.append(doc)
                    duplicates.append(document['ES_RESULT']['DOCUMENTS'][idx+1])
            print("duplicates--->",duplicates)
            if len(duplicates)!=0:
                print("duplicates are present...")
                if duplicates[0]['title']==document['ES_RESULT']['DOCUMENTS'][0]['title']:
                    value=""
                    stemmed_value=""
                    for doc in duplicates:
                        stemmed_value+=doc['stemmed_value']+"\n"
                        value+=doc['value']+"\n"

                    document['ES_RESULT']['DOCUMENTS'][0]['stemmed_value']=stemmed_value
                    document['ES_RESULT']['DOCUMENTS'][0]['value']=value
                    print("CLUBBING 2 SAME DOCS OF SAME MAIN TITLE AND SUBTITLE--------->> ")
                    print(document['ES_RESULT']['DOCUMENTS'][0]['value'])
                    print("CLUBBING 2 SAME DOCS OF SAME MAIN TITLE AND SUBTITLE------(END)--->> ")
            else:
                pass


              
            

                

            # print(element_list)
            pass

        # print(json.dumps(document,indent=4))

    def removeWordFrequency(self,unMatched_words):
        words=[]
        for u in unMatched_words:
            if u not in words:
                words.append(u)
        return words


    
    def remove_prepositions_from_title(self,bucket1_docs,bucket2_docs):

        # for idx,doc in enumerate(documents):
        #     subtitle_words=set(doc['stemmed_title'].split())
        #     prepositions=set([" on "," for "," of "," by "," over "," at "," from "])
        #     # prepositions=set(["of"])

        #     processed_subtitle_words=(subtitle_words - prepositions)
        #     print("actual subtitle is--> ",doc['stemmed_title'])
        #     print("Processed subtitle words is--->> ",processed_subtitle_words)
        #     processed_subtitle=" ".join(str(e) for e in processed_subtitle_words)
        #     print("Processed subtitle is--->> ",processed_subtitle)
        #     doc['stemmed_title']=processed_subtitle


            # print("subtitle is---> ",subtitle)
        preposition_words=[" on "," for "," of "," by "," over "," at "," from "]
        # for different docs 2 buckets must contain data
        if bucket2_docs!=None:

            for idx,doc in enumerate(bucket1_docs):
                for p in preposition_words:

                    try:
                        subtitle=doc['stemmed_title']
                        index=subtitle.index(p)
                        print("index of the preposition found in subtitle is------> ",index)
                        processed_subtitle=subtitle[:index]
                        print("processed subtitle is----------> ",processed_subtitle)
                        doc['stemmed_title']=processed_subtitle
                        print("processed stemmed_title is-----> ",doc['stemmed_title'])
                    except:
                        print("inside except block------->>",p)
                        pass
            for idx,doc in enumerate(bucket2_docs):
                for p in preposition_words:

                    try:
                        subtitle=doc['stemmed_title']
                        index=subtitle.index(p)
                        print("index of the preposition found in subtitle is------> ",index)
                        processed_subtitle=subtitle[:index]
                        print("processed subtitle is----------> ",processed_subtitle)
                        doc['stemmed_title']=processed_subtitle
                        print("processed stemmed_title is-----> ",doc['stemmed_title'])
                    except:
                        print("inside except block------->>",p)
                        pass
        else:
            # for same docs only one bucket should have data and bucket2 is empty
            for idx,doc in enumerate(bucket1_docs):
                for p in preposition_words:

                    try:
                        subtitle=doc['stemmed_title']
                        index=subtitle.index(p)
                        print("index of the preposition found in subtitle is------> ",index)
                        processed_subtitle=subtitle[:index]
                        print("processed subtitle is----------> ",processed_subtitle)
                        doc['stemmed_title']=processed_subtitle
                        print("processed stemmed_title is-----> ",doc['stemmed_title'])
                    except:
                        print("inside except block------->>",p)
                        pass
            
            
