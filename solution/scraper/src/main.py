from fastapi import FastAPI
from celery.result import AsyncResult
from tasks import extract_data
from pydantic import BaseModel
from typing import List, Dict

app = FastAPI()

class URLList(BaseModel):
    urls: List[str]

@app.get("/")
def read_root():
    return {"message": "Hello, FastAPI with Docker!"}

@app.get("/items/{item_id}")
def read_item(item_id: int, q: str = None):
    return {"item_id": item_id, "q": q}

@app.post("/process")
async def process_url(url_list: URLList):
     task_ids = []
    
    # Enviar las tareas a Celery
     for url in url_list.urls:
        print(f"📩 Enviando tarea para {url}...")
        task = extract_data.delay(url)
        task_ids.append(task.id)
        
     return {"message": "Tareas enviadas", "task_ids": task_ids}
    
    # Enviar las tareas a Celery
@app.get("/task/{task_id}")
async def get_task_result(task_id: str):
    task_result = AsyncResult(task_id)
    
    if not task_result.ready():
        return {"status": "PENDING", "task_id": task_id}
    
    if task_result.failed():
        return {"status": "FAILED", "task_id": task_id, "error": str(task_result.result)}
    
    return {"status": "SUCCESS", "task_id": task_id, "result": task_result.result}
    