from sqlalchemy import create_engine, Column, Integer, String, Text, DateTime
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker

# Configura la conexión a PostgreSQL
DATABASE_URL = "postgresql+psycopg2://postgres:admin@localhost:9040/sgca"

engine = create_engine(DATABASE_URL)

SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

Base = declarative_base()

class ScrapingResult(Base):
    __tablename__ = "scraping_results"

    id = Column(Integer, primary_key=True, index=True)
    task_id = Column(String, nullable=True)    
    url = Column(String, nullable=False)
    title = Column(String, nullable=True)
    content = Column(Text, nullable=True)
    date = Column(String, nullable=True)

# Crea las tablas en la base de datos (si no existen)
Base.metadata.create_all(bind=engine)