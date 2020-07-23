from configparser import ConfigParser

from typing import Optional
from fastapi import BackgroundTasks, HTTPException, Response, FastAPI

import requests
import os
import re

# Disable the annoying "Unverified HTTPS request is being made" warning
requests.packages.urllib3.disable_warnings()

import uvicorn

from search.qa_pipeline import QAPipeline
from webscraping.web_scraping_pipeline import WebScrapingPipeline

config_file_path = '../config.ini'
config = ConfigParser()
config.read(config_file_path)

api_port = config.get("application", "api_port")

http_proxy = config.get("proxies", "http")
https_proxy = config.get("proxies", "https")

proxies = None

if http_proxy is not None and https_proxy is not None:
    proxies = {
        "http": http_proxy,
        "https": https_proxy
    }

qa_pipeline = QAPipeline(config)

fetch_scraping_config_url = config.get("urls", "rescrape_all_pages_url")
scraping_status_url = config.get("urls", "rescrape_status_url")
es_host = config.get("elastic_search_credentials", "host")
es_port = config.getint("elastic_search_credentials", "port")
es_index = config.get("elastic_search_credentials", "index")
fetch_static_scraping_config_url = config.get("urls", "static_file_url")
static_scraping_url = config.get("urls", "static_file_status_url")
synonyms_url = config.get("urls", "synonyms_url")
on_scraping_completed_url = config.get("urls", "on_scraping_completed_url")
on_static_file_scraping_completed_url = config.get("urls", "on_static_file_scraping_completed_url")


app = FastAPI()

@app.get("/qa")
def qa(query: str, context: Optional[str] = None):

    try:
        return qa_pipeline.search(query, context)
    except Exception as e:
        raise HTTPException(status_code=500, detail=e.args)

__scraping_in_progress = False

def __scrape_all_pages__():
    global __scraping_in_progress

    __scraping_in_progress = True

    try:
        web_scraping_pipeline = WebScrapingPipeline(fetch_scraping_config_url, scraping_status_url, es_host, es_port, es_index, proxies)
        
        web_scraping_pipeline.scrape_all_pages()

        requests.post(on_scraping_completed_url)

    except Exception as e:
        print(f"Scraping error: {repr(e)}")

        requests.post(on_scraping_completed_url, repr(e))

    __scraping_in_progress = False

def __scrape_page__(page_config):
    global __scraping_in_progress

    __scraping_in_progress = True

    try:
        web_scraping_pipeline = WebScrapingPipeline(fetch_scraping_config_url, scraping_status_url, es_host, es_port, es_index, proxies)
        
        web_scraping_pipeline.scrape_page(page_config)
    except Exception as e:
        print(f"Scraping error: {e.args}")

    __scraping_in_progress = False

def __scrape_all_static_pages__():
    global __scraping_in_progress

    __scraping_in_progress = True

    try:
        web_scraping_pipeline = WebScrapingPipeline(fetch_static_scraping_config_url, static_scraping_url, es_host, es_port, es_index, proxies)
        
        web_scraping_pipeline.scrape_static_page()

        requests.post(on_static_file_scraping_completed_url)
    except Exception as e:
        print(f"Scraping error: {e.args}")

        requests.post(on_static_file_scraping_completed_url, repr(e))


    __scraping_in_progress = False

@app.get("/scrape_all_pages")
def scrape_all_pages(background_tasks: BackgroundTasks):
    global __scraping_in_progress

    if __scraping_in_progress:
        raise HTTPException(status_code=406, detail="A previously queued scraping operation is already in progress")

    try:
        background_tasks.add_task(__scrape_all_pages__)

        return {
            "status": "success",
            "detail": "Scraping Task Queued Successfully."
        }
    except Exception as e:
        raise HTTPException(status_code=500, detail=e.args)

@app.get("/scrape_static_pages")
def scrape_static_page(background_tasks: BackgroundTasks):
    global __scraping_in_progress
    
    if __scraping_in_progress:
        raise HTTPException(status_code=406, detail="A previously queued scraping operation is already in progress")

    try:
        background_tasks.add_task(__scrape_all_static_pages__)

        return {
            "status": "success",
            "detail": "Scraping Task Queued Successfully."
        }
    except Exception as e:
        raise HTTPException(status_code=500, detail=e.args)


@app.get("/scrape_page")
def scrape_page(page_config, background_tasks: BackgroundTasks):
    global __scraping_in_progress
    
    if __scraping_in_progress:
        raise HTTPException(status_code=406, detail="A previously queued scraping operation is already in progress")

    try:
        background_tasks.add_task(__scrape_page__, page_config=page_config)

        return {
            "status": "success",
            "detail": "Scraping Task Queued Successfully."
        }
    except Exception as e:
        raise HTTPException(status_code=500, detail=e.args)

def __sync_synonyms__():
    try:
        root_directory = os.path.abspath(os.pardir)
        print(root_directory+"/simplified-qa-pipeline/config_files/synonyms.txt")
        response = requests.get(url=synonyms_url)
        data = response.json()
        synonyms = []
        for synonym in data:
            synonyms.append(re.sub(",", "=", synonym))
        synonyms = "\n".join(synonyms)
        file = open(
            root_directory+"/simplified-qa-pipeline/config_files/synonyms.txt", "w+")
        file.write(synonyms)
        print("Added synonyms successfully..!!")
        file.close()
    except Exception as e:
        print(f"Synonyms Syncing error: {e.args}")

@app.get('/resync_synonyms')
def resync_synonyms(background_tasks: BackgroundTasks):
    try:
        background_tasks.add_task(__sync_synonyms__)
        return {
                "status": "success",
                "detail": "Syncing Synonyms Successfully.."
            }
    except Exception as e:
        raise HTTPException(status_code=500, detail=e.args)

if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=int(api_port))
