from fastapi import FastAPI, HTTPException
from celery.result import AsyncResult
from tasks import extract_data
from pydantic import BaseModel
from typing import List
from database import SessionLocal, ScrapingResult

app = FastAPI()

class FileName(BaseModel):
    filename: str

def read_links_from_file(filename: str) -> list[str]:
    try:
        with open(filename, "r") as file:
            links = [line.strip() for line in file if line.strip()]
        return links
    except FileNotFoundError:
        raise ValueError(f"El archivo '{filename}' no existe.")
    except Exception as e:
        raise ValueError(f"Error al leer el archivo: {e}")


@app.get("/")
def read_root():
    return {"message": "Hello, FastAPI with Docker!"}


@app.post("/process")
async def process_file(file: FileName):
    try:
        
        links = read_links_from_file(file.filename)
        print(f"📄 Enlaces leídos: {links}")  # Borrar despues

        task_ids = []
                
        for link in links:            
            task = extract_data.delay(link)
            task_ids.append(task.id)
        
        return {"message": "Tareas enviadas", "task_ids": task_ids}
    
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error interno: {e}")

@app.get("/task/{task_id}")
async def get_task_result(task_id: str):
    task_result = AsyncResult(task_id)
    
    if not task_result.ready():
        return {"status": "PENDING", "task_id": task_id}
    
    if task_result.failed():
        return {"status": "FAILED", "task_id": task_id, "error": str(task_result.result)}
    
    return {"status": "SUCCESS", "task_id": task_id, "result": task_result.result}

@app.get("/results")
async def get_results():
    db = SessionLocal()
    results = db.query(ScrapingResult).all()
    db.close()
    return results