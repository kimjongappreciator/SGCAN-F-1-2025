import argparse
from tasks import extract_data
from celery.result import AsyncResult

def main(file_name):
    try:
        with open(file_name, 'r', encoding='utf-8') as f:
            urls = [line.strip() for line in f if line.strip()]  # Evita líneas vacías
        
        output_file = f"result_{file_name.split('.')[0]}.txt"

        with open(output_file, 'w', encoding='utf-8') as f:
            f.write("Resultados del Web Scraping\n")
            f.write("=====================================\n\n")
        
        task_ids = []

        for url in urls:
            print(f"📩 Enviando tarea para {url}...")
            task = extract_data.delay(url)
            task_ids.append(task.id)  # Guardamos el ID de la tarea

        print("⏳ Esperando resultados...")
        with open(output_file, 'w', encoding='utf-8') as f:
            for task_id in task_ids:
                result = AsyncResult(task_id)
                result_data = result.get(timeout=60)
                f.write(f"URL: {result_data['url']}\n")
                f.write(f"Título: {result_data['title']}\n")
                f.write(f"Contenido: {result_data['content']}\n")
                f.write(f"Fecha: {result_data['date']}\n")
                f.write("--------------------------------------------------------\n")
            

        print(f"✅ Proceso finalizado. Resultados en {output_file}")

    except FileNotFoundError:
        print(f"❌ Error: El archivo {file_name} no existe.")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Ejecutar web scraping desde un archivo de URLs.")
    parser.add_argument("file_name", type=str, help="Nombre del archivo de texto con las URLs")
    
    args = parser.parse_args()
    main(args.file_name)
