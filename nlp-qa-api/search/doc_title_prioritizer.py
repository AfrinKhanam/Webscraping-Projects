import re
import numpy as np
import json
import base64
from nltk.stem import PorterStemmer


class DocTitlePrioritizer:
    def __init__(self):
        pass

    def create_regex(self, string):
        # ------------------------------------- #
        word_list = string.split()
        return word_list

    def MainTitlePrioritization(self, document):
        query_list = set(self.create_regex(document['POTENTIAL_QUERY_LIST']))

        documents = []
        docs_length = []
        sorted_docs = []

        for doc in document['ES_RESULT']['DOCUMENTS']:
            main_title_words = doc['stemmed_main_title'].split()
            main_title = set(main_title_words)

            matched_maintitle_words = query_list.intersection(main_title)
            maintitle_words_length = len(matched_maintitle_words)
            documents.append(doc)
            docs_length.append(maintitle_words_length)
            doc['matched_maintitle_words'] = list(matched_maintitle_words)

        # Create buckets
        bucket1 = []
        bucket2 = []
        bucketData1 = []
        bucketData2 = []
        bucket1_len = []
        bucket2_len = []

        if documents[0]['stemmed_main_title'].rstrip() == documents[1]['stemmed_main_title'].rstrip() and documents[0]['stemmed_main_title'].rstrip() == documents[2]['stemmed_main_title'].rstrip():
            bucketData1, bucketData2 = self.SortMainTitle(
                bucket1=None, bucket2=None, documents=documents, bucket1_len=None, bucket2_len=None, docs_length=docs_length)
        elif documents[0]['stemmed_main_title'].rstrip() == documents[1]['stemmed_main_title'].rstrip() or documents[1]['stemmed_main_title'].rstrip() == documents[0]['stemmed_main_title'].rstrip():
            bucket1.append(documents[0])
            bucket1.append(documents[1])
            bucket2.append(documents[2])
            bucket1_len.append(docs_length[0])
            bucket1_len.append(docs_length[1])
            bucket2_len.append(docs_length[2])
            bucketData1, bucketData2 = self.SortMainTitle(
                bucket1=bucket1, bucket2=bucket2, documents=None, bucket1_len=bucket1_len, bucket2_len=bucket2_len, docs_length=None)

        elif documents[0]['stemmed_main_title'] == documents[2]['stemmed_main_title']:

            bucket1.append(documents[0])
            bucket1.append(documents[2])
            bucket2.append(documents[1])
            bucket1_len.append(docs_length[0])
            bucket1_len.append(docs_length[2])
            bucket2_len.append(docs_length[1])

            bucketData1, bucketData2 = self.SortMainTitle(
                bucket1=bucket1, bucket2=bucket2, documents=None, bucket1_len=bucket1_len, bucket2_len=bucket2_len, docs_length=None)

        elif documents[1]['stemmed_main_title'] == documents[2]['stemmed_main_title'] or documents[2]['stemmed_main_title'] == documents[1]['stemmed_main_title']:
            bucket1.append(documents[1])
            bucket1.append(documents[2])
            bucket2.append(documents[0])
            bucket1_len.append(docs_length[1])
            bucket1_len.append(docs_length[2])
            bucket2_len.append(docs_length[0])

            bucketData1, bucketData2 = self.SortMainTitle(
                bucket1=bucket1, bucket2=bucket2, documents=None, bucket1_len=bucket1_len, bucket2_len=bucket2_len, docs_length=None)

        elif documents[2]['stemmed_main_title'] == documents[0]['stemmed_main_title']:
            bucket1.append(documents[2])
            bucket1.append(documents[0])
            bucket2.append(documents[1])
            bucket1_len.append(docs_length[2])
            bucket1_len.append(docs_length[0])
            bucket2_len.append(docs_length[1])

            bucketData1, bucketData2 = self.SortMainTitle(
                bucket1=bucket1, bucket2=bucket2, documents=None, bucket1_len=bucket1_len, bucket2_len=bucket2_len, docs_length=None)

        else:
            bucketData1, bucketData2 = self.SortMainTitle(
                bucket1=None, bucket2=None, documents=documents, bucket1_len=None, bucket2_len=None, docs_length=docs_length)

        self.SubtitlePrioritization(
            bucketData1, bucketData2, query_list, document)

        # Removing prepositions from sub title
    def SortMainTitle(self, bucket1, bucket2, documents, bucket1_len, bucket2_len, docs_length):
        # if docs are different
        if bucket1 != None and bucket2 != None:
            zipped = zip(bucket1, bucket1_len)
            zipped = list(zipped)
            bucketData1 = sorted(zipped, key=lambda x: x[1], reverse=True)
            bucketData2 = list(zip(bucket2, bucket2_len))

            return bucketData1, bucketData2
        else:
            # if docs are same

            # Sort docs in decreasing order based on matched length
            bucketData2 = []
            zipped = zip(documents, docs_length)
            zipped = list(zipped)
            bucketData1 = sorted(zipped, key=lambda x: x[1], reverse=True)

            return bucketData1, bucketData2

    def SortSubtitle(self):
        pass

    def SubtitlePrioritization(self, bucketData1, bucketData2, query_list, document):
        bucket1_docs = []
        bucket2_docs = []

        bucket1_subtitle_lengths = []
        bucket2_subtitle_lengths = []

        # Remove Prepositions from subtitle (bucket)
        self.remove_prepositions_from_title(bucketData1, bucketData2)

        if bucketData1 != [] and bucketData2 != []:
            documents, docs_length = zip(*bucketData1)
            for doc in documents:
                subtitle_words = set(doc['stemmed_title'].split())
                matched_subtitle_words = subtitle_words.intersection(query_list)
                matched_subtitle_length = len(matched_subtitle_words)
                bucket1_docs.append(doc)
                bucket1_subtitle_lengths.append(matched_subtitle_length)
            documents, docs_length = zip(*bucketData2)
            for doc in documents:
                subtitle_words = set(doc['stemmed_title'].split())
                matched_subtitle_words = subtitle_words.intersection(query_list)
                matched_subtitle_length = len(matched_subtitle_words)
                bucket2_docs.append(doc)
                bucket2_subtitle_lengths.append(matched_subtitle_length)
                doc['matched_subtitle_words'] = list(matched_subtitle_words)

            #  # Remove Prepositions from subtitle (bucket)
            # self.remove_prepositions_from_title(bucket1_docs,bucket2_docs)

            bucket1_zipped = zip(bucket1_docs, bucket1_subtitle_lengths)
            bucket1_zipped = list(bucket1_zipped)
            bucket1_subtitle_sorted_docs = sorted(
                bucket1_zipped, key=lambda x: x[1], reverse=True)
            doc1, length1 = zip(*bucket1_subtitle_sorted_docs)

            bucket2_zipped = zip(bucket2_docs, bucket2_subtitle_lengths)
            bucket2_zipped = list(bucket2_zipped)
            bucket2_subtitle_sorted_docs = sorted(
                bucket2_zipped, key=lambda x: x[1], reverse=True)
            doc2, length2 = zip(*bucket2_subtitle_sorted_docs)

            merged_docs = (doc1)+(doc2)

            document['ES_RESULT']['DOCUMENTS'] = merged_docs

            self.club_documents(document)

        else:
            docs = []
            subtitle_lengths = []
            documents, docs_length = zip(*bucketData1)

            # Remove Prepositions from subtitle (bucket)
            self.remove_prepositions_from_title(
                bucket1_docs=documents, bucket2_docs=None)

            for doc in documents:
                subtitle_words = set(doc['stemmed_title'].split())
                matched_subtitle_words = subtitle_words.intersection(query_list)
                matched_subtitle_length = len(matched_subtitle_words)
                docs.append(doc)
                subtitle_lengths.append(matched_subtitle_length)

            zipped = zip(docs, subtitle_lengths)
            zipped = list(zipped)
            # subtitle_sorted_docs=zipped.sort(key=lambda x: x[1],reverse=True)

            result = False
            # if all docs main title is same then do sorting else no sorting its like 3 buckets for 3 different docs
            result = all((doc['stemmed_main_title'] == (document['ES_RESULT']['DOCUMENTS']
                                                        [0]['stemmed_main_title'])) for doc in document['ES_RESULT']['DOCUMENTS'])
            if result:
                subtitle_sorted_docs = sorted(
                    zipped, key=lambda x: x[1], reverse=True)

                docs, subtitle_lengths = zip(*subtitle_sorted_docs)
                document['ES_RESULT']['DOCUMENTS'] = docs

                self.club_documents(document)

            else:

                # unzipping the docs
                docs, subtitle_lengths = zip(*zipped)
                document['ES_RESULT']['DOCUMENTS'] = docs
                self.club_documents(document)

    def club_documents(self, document):
        result = False
        result = all((doc['stemmed_main_title'].rstrip() == (document['ES_RESULT']['DOCUMENTS'][0]['stemmed_main_title'].rstrip(
        )) and doc['stemmed_title'] == (document['ES_RESULT']['DOCUMENTS'][0]['stemmed_title'])) for doc in document['ES_RESULT']['DOCUMENTS'])

        # if all docs main title and subtitle is same
        if result:
            if document['ES_RESULT']['DOCUMENTS'][0]['inner_table_keys'] != []:
                self.create_table_structure(document['ES_RESULT']['DOCUMENTS'])
            else:
                stemmed_value = "&#8226; "
                value = "&#8226; "
                for doc in document['ES_RESULT']['DOCUMENTS']:
                    stemmed_value += doc['stemmed_value']+"\n &#8226; "
                    value += doc['value']+"\n &#8226; "

                stemmed_value = stemmed_value.rstrip('\n &#8226; ')
                value = value.rstrip('\n &#8226; ')

                document['ES_RESULT']['DOCUMENTS'][0]['value'] = value
                document['ES_RESULT']['DOCUMENTS'][0]['stemmed_value'] = stemmed_value

        # if all docs main title is same but 1 different subtitle
        elif all((doc['stemmed_main_title'] == (document['ES_RESULT']['DOCUMENTS'][0]['stemmed_main_title'])) for doc in document['ES_RESULT']['DOCUMENTS']):
            element_list = [doc['stemmed_title']
                            for doc in document['ES_RESULT']['DOCUMENTS']]
            duplicates = []
            for doc in document['ES_RESULT']['DOCUMENTS']:
                if element_list.count(doc['stemmed_title']) > 1:
                    duplicates.append(doc)
            if len(duplicates) != 0:

                # check if first doc subtitle matches with duplicate list first doc subtitle
                if duplicates[0]['title'] == document['ES_RESULT']['DOCUMENTS'][0]['title']:
                    if duplicates[0]['inner_table_keys'] != []:
                        self.create_table_structure(duplicates)
                    else:
                        value = "&#8226; "
                        stemmed_value = "&#8226; "
                        for doc in duplicates:
                            stemmed_value += doc['stemmed_value']+"\n &#8226; "
                            value += doc['value']+"\n &#8226; "

                        stemmed_value = stemmed_value.rstrip('\n &#8226; ')
                        value = value.rstrip('\n &#8226; ')

                        document['ES_RESULT']['DOCUMENTS'][0]['stemmed_value'] = stemmed_value
                        document['ES_RESULT']['DOCUMENTS'][0]['value'] = value

        else:
            # if any 2 docs main title and subtitle is same
            duplicates = []
            for idx, doc in enumerate(document['ES_RESULT']['DOCUMENTS']):
                if idx == len(document['ES_RESULT']['DOCUMENTS'])-1:
                    idx = -1

                if doc['stemmed_main_title'] == document['ES_RESULT']['DOCUMENTS'][idx+1]['stemmed_main_title'] and doc['stemmed_title'] == document['ES_RESULT']['DOCUMENTS'][idx+1]['stemmed_title']:
                    duplicates.append(doc)
                    duplicates.append(
                        document['ES_RESULT']['DOCUMENTS'][idx+1])

            if len(duplicates) != 0:

                if duplicates[0]['title'] == document['ES_RESULT']['DOCUMENTS'][0]['title']:
                    if duplicates[0]['inner_table_keys'] != []:
                        self.create_table_structure(duplicates)
                    else:
                        value = "&#8226; "
                        stemmed_value = "&#8226; "
                        for doc in duplicates:
                            stemmed_value += doc['stemmed_value']+"\n &#8226; "
                            value += doc['value']+"\n &#8226; "

                        stemmed_value = stemmed_value.rstrip('\n &#8226; ')
                        value = value.rstrip('\n &#8226;')
                        document['ES_RESULT']['DOCUMENTS'][0]['stemmed_value'] = stemmed_value
                        document['ES_RESULT']['DOCUMENTS'][0]['value'] = value
            else:
                # different docs, with first has table structure
                if document['ES_RESULT']['DOCUMENTS'][0]['inner_table_keys'] != []:
                    self.create_table_structure(
                        document['ES_RESULT']['DOCUMENTS'])

                pass

    def removeWordFrequency(self, unMatched_words):
        words = []
        for u in unMatched_words:
            if u not in words:
                words.append(u)
        return words

    def create_table_structure(self, duplicates):
        inner_table_keys = []

        inner_table_values = []

        for index, doc in enumerate(duplicates):

            for key in doc['inner_table_keys']:
                inner_table_keys.append(key)

            for value in doc['inner_table_values']:
                inner_table_values.append(value)
        # inner_table_keys_length=len(inner_table_keys)
        # inner_table_values_length=len(inner_table_values)

        # for 2 same docs subtitle is same
        if len(inner_table_values) == 4:
            value = f'''{inner_table_keys[0]}---->{inner_table_keys[1]}
                                {inner_table_values[0]}---->{inner_table_values[1]}
                                {inner_table_values[2]}---->{inner_table_values[3]}'''
            duplicates[0]['value'] = value

        # for 1  docs subtitle
        elif len(inner_table_values) == 2:
            value = f'''{inner_table_keys[0]}---->{inner_table_keys[1]}
                                {inner_table_values[0]}---->{inner_table_values[1]}'''
            duplicates[0]['value'] = value

        # for 3 docs subtitle is same
        elif len(inner_table_values) == 6:
            value = f'''{inner_table_keys[0]}---->{inner_table_keys[1]}
                                {inner_table_values[0]}---->{inner_table_values[1]}
                                {inner_table_values[2]}---->{inner_table_values[3]}
                                {inner_table_values[4]}---->{inner_table_values[5]}'''
            duplicates[0]['value'] = value
        else:
            pass

    def remove_prepositions_from_title(self, bucket1_docs, bucket2_docs):
        preposition_words = [" on ", " for ", " of ",
                             " by ", " over ", " at ", " from "]
        # for different docs 2 buckets must contain data
        if bucket2_docs != None:

            for idx, doc in enumerate(bucket1_docs):
                for p in preposition_words:

                    try:
                        subtitle = doc['stemmed_title']
                        index = subtitle.index(p)
                        processed_subtitle = subtitle[:index]
                        doc['stemmed_title'] = processed_subtitle
                    except:
                        pass
                    
            for idx, doc in enumerate(bucket2_docs):
                for p in preposition_words:

                    try:
                        subtitle = doc['stemmed_title']
                        index = subtitle.index(p)
                        processed_subtitle = subtitle[:index]
                        doc['stemmed_title'] = processed_subtitle
                    except:
                        pass
        else:
            # for same docs only one bucket should have data and bucket2 is empty
            for idx, doc in enumerate(bucket1_docs):
                for p in preposition_words:

                    try:
                        subtitle = doc['stemmed_title']
                        index = subtitle.index(p)
                        processed_subtitle = subtitle[:index]
                        doc['stemmed_title'] = processed_subtitle
                    except:
                        pass
