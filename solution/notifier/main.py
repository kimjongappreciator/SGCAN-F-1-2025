from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
import smtplib
from email.mime.text import MIMEText
from email.mime.multipart import MIMEMultipart

app = FastAPI()

SMTP_SERVER = "smtp.gmail.com"
SMTP_PORT = 587
SENDER_EMAIL = "XXXXXXX"
SENDER_PASSWORD = "XXXXXXX"

class EmailRequest(BaseModel):
    to_email: str
    subject: str
    body: str


@app.post("/notify")
async def send_email(request: EmailRequest):
    try:        
        message = MIMEMultipart()
        message["From"] = SENDER_EMAIL
        message["To"] = request.to_email
        message["Subject"] = request.subject
        message.attach(MIMEText(request.body, "plain"))
        
        with smtplib.SMTP(SMTP_SERVER, SMTP_PORT) as server:
            server.starttls()
            server.login(SENDER_EMAIL, SENDER_PASSWORD)
            server.sendmail(SENDER_EMAIL, request.to_email, message.as_string())

        return {"message": "Correo enviado exitosamente"}

    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error al enviar el correo: {e}")