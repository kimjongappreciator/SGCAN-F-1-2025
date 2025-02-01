from celery import Celery

# Configura Celery
app = Celery(
    'mi_proyecto',  # Nombre del módulo
    broker='redis://localhost:9050/0',  # Broker (Redis en este caso)
    backend='redis://localhost:9050/0',  # Backend para resultados
    include=['tasks']  # Importa el módulo que contiene las tareas
)

# Configuración adicional (opcional)
app.conf.update(
    result_expires=3600,  # Tiempo de expiración de los resultados
)