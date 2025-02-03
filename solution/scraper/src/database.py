from sqlalchemy import create_engine, Column, Integer, String, Text, DateTime
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker

#conexión a pg
DATABASE_URL = "postgresql+psycopg2://f1_sgcan:f1_sgcan123456@host.docker.internal:5432/f1_database"

engine = create_engine(DATABASE_URL)

SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

Base = declarative_base()

class ScrapingResult(Base):
    __tablename__ = "scraping_results"

    id = Column(Integer, primary_key=True, index=True)
    fileId = Column(String, nullable=True)
    task_id = Column(String, nullable=True)    
    url = Column(String, nullable=False)
    title = Column(String, nullable=True)
    content = Column(Text, nullable=True)
    date = Column(String, nullable=True)

Base.metadata.create_all(bind=engine)