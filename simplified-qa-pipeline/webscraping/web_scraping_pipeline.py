import json
import sys
import time
from traceback import format_exc
import requests

# Disable the annoying "Unverified HTTPS request is being made" warning
requests.packages.urllib3.disable_warnings()

from elasticsearch import Elasticsearch

from webscraping.html_to_json import HtmlToJson
from webscraping.stemming import Stemmer
from webscraping.pre_processing import PreProcessing
from webscraping.elastic import Elastic

db_sleep_time = 2

class WebScrapingPipeline:

    def __init__(self, fetch_scraping_config_url, scraping_status_url, es_host, es_port, es_index, proxies=None):
        self.__es_index = es_index
        self.__fetch_scraping_config_url = fetch_scraping_config_url
        self.__scraping_status_url = scraping_status_url

        self.__stemmer = Stemmer()
        self.__pre_processor = PreProcessing()

        self.__elastic = Elastic(es_host, es_port, es_index, '_doc')

        self.__proxies = proxies

    def scrape_all_pages(self):
        self.__drop_database__()
        time.sleep(db_sleep_time)
        self.__rescrape_all_pages__()

    def scrape_page(self, page_config):
        self.__delete_page_from_es_index__()
        time.sleep(db_sleep_time)
        self.__rescrape_page__(page_config)

    def __drop_database__(self):
        try:
            es = Elasticsearch(index=self.__es_index)
            es.indices.delete(index=self.__es_index)
        except Exception as e:
            print("No database found..!!", self.get_error_details(e))

    def __delete_page_from_es_index__(self, page_config):
        # TODO: Delete an individual url's documents from Elasticsearch index
        pass

    def __get_scraping_configuration__(self):
        documents = []

        response = requests.get(url=self.__fetch_scraping_config_url)

        json_configurations = response.json()

        # TODO: Cleanup this hardcoded path!!!
        path = "../../indian-bank-web-scraped-data/www.indianbank.in.1-Dec-2019/departments/"

        if json_configurations != None:
            # ----------------------------------------------------------- #
            for json_config in json_configurations:
                url_list = json.loads(json_config['pageConfig'])
                if "document_name" not in url_list:
                    for url in url_list:
                        document = url_list[url]
                        document['url'] = url
                        document['filename'] = path + url.split('/')[-2] + '/index.html'
                        json_config['pageConfig'] = document
                        documents.append(json_config)
                else:
                    document = url_list
                    json_config['pageConfig'] = document

                    documents.append(json_config)

            return documents

        return None

    def __rescrape_all_pages__(self):
        documents = self.__get_scraping_configuration__()

        for idx in range(len(documents)):
            self.__rescrape_page__(documents[idx])

    def __rescrape_page__(self, document):
        doc_id = document['id']
        doc_url = document['pageConfig']['url']

        print(f"Scraping {doc_url}...")

        error_message = None
        value = 1

        while value <= 5:
            try:
                document = self.__generate_json_structure__(document)

                document = self.__stemmer.w_stem(document)

                document = self.__pre_processor.process(document)

                self.__elastic.generate_individual_document(document)

                self.__elastic.index_document(document)

                scrape_status = 1

                response = requests.put(f"{self.__scraping_status_url}{doc_id}&ScrapeStatus={scrape_status}")

                print(f"Success: {doc_url}")

                time.sleep(5)
                break
                #----------------------------------------------------------------#

            except ConnectionError as e:
                value += 1
                time.sleep(5)
                continue

            except Exception as e:

                error_message = f"Scraping Error: {self.get_error_details(e)}"

                break

        if value > 5:
            error_message = f"Scraping Error: Page not reachable. Max retries reached."

        if error_message is not None:
            print(f"{error_message}: {doc_url}")

            scrape_status = 2
            response = requests.put(f"{self.__scraping_status_url}{doc_id}&ScrapeStatus={scrape_status}&ErrorMessage={error_message}")


    def __generate_json_structure__(self, document):

        response = requests.get(document['url'], verify=False, proxies=self.__proxies)

        html = response.content

        html_to_json = HtmlToJson(html)

        manager_pages = ['/departments/general-managers/', '/departments/general-managers/']

        if any([u for u in manager_pages if document['url'].lower().endswith(u)]):
            html_to_json.generate_json_for_general_managers(document)

            return document

        document = document['pageConfig']

        #---------------------------------------------------------------#
        html_to_json.main_title(document)
        html_to_json.get_url(document)
        html_to_json.get_document_name(document)
        html_to_json.subtitles(document)
        html_to_json.content(document)
        html_to_json.post_processing(document)
        html_to_json.frame_json(document)

        return document['html_to_json']

    def get_error_details(self, err):
        return {
            error: repr(err),
            stacktrace: format_exc()
        }