from configparser import ConfigParser

from typing import Optional
from fastapi import FastAPI

import uvicorn

from qa_pipeline import QAPipeline

config_file_path = '../config.ini'
config = ConfigParser()
config.read(config_file_path)

api_port = config.get("application", "api_port")

qa_pipeline = QAPipeline(config)

app = FastAPI()

@app.get("/qa")
def qa(query: str, context: Optional[str] = None):

    try:
        return qa_pipeline.search(query, context)
    except Exception as e:
        return {
            'error': e
        }

if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=int(api_port))
