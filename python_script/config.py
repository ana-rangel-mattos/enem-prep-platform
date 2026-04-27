from dotenv import load_dotenv
import os

load_dotenv()

HOST = os.getenv("DB_HOST")
DATABASE = os.getenv("DB_DATABASE")
USER = os.getenv("DB_USER")
PASSWORD = os.getenv("DB_PASSWORD")
PORT = os.getenv("DB_PORT")
