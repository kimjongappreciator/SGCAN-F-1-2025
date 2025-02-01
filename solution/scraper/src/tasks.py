import requests
from bs4 import BeautifulSoup
from celery_config import app
from database import SessionLocal, ScrapingResult

@app.task(bind=True)
def extract_data(self, url):    
    try:
        response = requests.get(url, timeout=10)
        response.raise_for_status()
        soup = BeautifulSoup(response.text, 'html.parser')

        title = [t.get_text(strip=True) for t in soup.find_all('h1', class_='mb-4 d-sm-block d-none fs-3')]
        content = [p.get_text(strip=True) for p in soup.find_all('p', class_='')]
        date = [d.get_text(strip=True) for d in soup.find_all('time')]

        result = {
            "url": url,
            "title": title[0] if title else "No encontrado",
            "content": content[0] if content else "No encontrado",
            "date": date[0] if date else "No encontrado"
        }
        
        db = SessionLocal()
        db_result = ScrapingResult(
            url=result["url"],
            title=result["title"],
            content=result["content"],
            date=result["date"],
            task_id=self.request.id  # Guarda el task_id
        )
        db.add(db_result)
        db.commit()
        db.refresh(db_result)
        db.close()

        print(f"✅ Procesado: {url}")
        return result
    
    except requests.exceptions.RequestException as e:
        print(f"❌ Error en {url}: {e}")
        return {"url": url, "error": str(e)}