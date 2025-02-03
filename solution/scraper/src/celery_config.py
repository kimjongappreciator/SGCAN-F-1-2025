from celery import Celery

app = Celery(
    'mi_proyecto',  
    broker='redis://host.docker.internal:9050/0',  
    backend='redis://host.docker.internal:9050/0', 
    include=['tasks']  
)


app.conf.update(
    result_expires=3600,  
)