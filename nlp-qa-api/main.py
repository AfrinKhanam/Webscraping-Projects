import json
import re
import requests

# Disable the annoying "Unverified HTTPS request is being made" warning
requests.packages.urllib3.disable_warnings()

from configparser import ConfigParser

from typing import Optional
from fastapi import Request, BackgroundTasks, HTTPException, FastAPI

import uvicorn

from common.utils import get_error_details
from search.qa_pipeline import QAPipeline
from webscraping.web_scraping_pipeline import WebScrapingPipeline
from webscraping.scrape_menu import ScrapeMenu

config_file_path = './config.ini'
config = ConfigParser()
config.read(config_file_path)

api_port = config.get("application", "api_port")

http_proxy = config.get("proxies", "http")
https_proxy = config.get("proxies", "https")

proxies = { "http": http_proxy, "https": https_proxy } if (http_proxy and https_proxy) else None

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

scrape_menu_url = config.get("scrape_menu","url")
scrape_menu_id = config.get("scrape_menu","id")

app = FastAPI()

@app.get("/qa")
def qa(query: str, context: Optional[str] = None):

    try:
        return qa_pipeline.search(query, context)
    except Exception:
        raise HTTPException(status_code=500, detail=get_error_details())

__scraping_in_progress = False

def __scrape_all_pages__():
    global __scraping_in_progress

    __scraping_in_progress = True

    try:
        web_scraping_pipeline = WebScrapingPipeline(fetch_scraping_config_url, scraping_status_url, es_host, es_port, es_index, proxies)
        
        web_scraping_pipeline.scrape_all_pages()

        requests.post(on_scraping_completed_url)

        print(f"Scraping completed")

    except Exception:
        err_msg = get_error_details()

        print(f"Scraping error: {err_msg}")

        requests.post(on_scraping_completed_url, err_msg)

    __scraping_in_progress = False

def __scrape_page__(json_config):
    global __scraping_in_progress

    __scraping_in_progress = True

    try:
        web_scraping_pipeline = WebScrapingPipeline(fetch_scraping_config_url, scraping_status_url, es_host, es_port, es_index, proxies)
        
        web_scraping_pipeline.scrape_page(json_config)

        print(f"Scraping completed")
    except Exception:
        print(f"Scraping error: {get_error_details()}")

    __scraping_in_progress = False

def __scrape_all_static_pages__():
    global __scraping_in_progress

    __scraping_in_progress = True

    try:
        web_scraping_pipeline = WebScrapingPipeline(fetch_static_scraping_config_url, static_scraping_url, es_host, es_port, es_index, proxies)
        
        web_scraping_pipeline.scrape_static_page()

        requests.post(on_static_file_scraping_completed_url)

        print(f"Scraping completed")
    except Exception:
        err_msg = get_error_details()

        print(f"Scraping error: {err_msg}")

        requests.post(on_static_file_scraping_completed_url, err_msg)

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
    except Exception:
        raise HTTPException(status_code=500, detail=get_error_details())

@app.get('/get_ib_menu')
def scrape_menu():
    try:
        ib_menu = WebScrapingPipeline(fetch_scraping_config_url, scraping_status_url, es_host, es_port, es_index, proxies).__fetch_page_from_es__(field='_id',value='menu_items')
        return ib_menu['hits']['hits'][0]['_source']['ib_menu']
    except Exception:
        raise HTTPException(status_code=500, detail=get_error_details())

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
    except Exception:
        raise HTTPException(status_code=500, detail=get_error_details())


@app.post("/scrape_page")
async def scrape_page(request: Request, background_tasks: BackgroundTasks):
    global __scraping_in_progress
    
    if __scraping_in_progress:
        raise HTTPException(status_code=406, detail="A previously queued scraping operation is already in progress")

    try:
        json_config = await request.json()

        background_tasks.add_task(__scrape_page__, json_config=json_config)

        return {
            "status": "success",
            "detail": "Scraping Task Queued Successfully."
        }
    except Exception:
        raise HTTPException(status_code=500, detail=get_error_details())


def __sync_synonyms__():
    try:
        global qa_pipeline
        response = requests.get(url=synonyms_url)

        data = response.json()

        synonyms = "\n".join([re.sub(",", "=", synonym) for synonym in data])

        with open("./config_files/synonyms.txt", "w+") as f:
            f.write(synonyms)
        qa_pipeline = QAPipeline(config)

        print("Added synonyms successfully..!!")

    except Exception:
        print(f"Synonyms Syncing error: {get_error_details()}")

@app.get('/resync_synonyms')
def resync_synonyms(background_tasks: BackgroundTasks):
    try:
        background_tasks.add_task(__sync_synonyms__)
        return {
                "status": "success",
                "detail": "Syncing Synonyms Successfully.."
            }
    except Exception:
        raise HTTPException(status_code=500, detail=get_error_details())

if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=int(api_port))
