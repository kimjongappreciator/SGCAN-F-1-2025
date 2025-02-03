#!/bin/bash
cd src
python -m celery -A celery_config worker --loglevel=info --pool=solo &

uvicorn main:app --host 0.0.0.0 --port 8000