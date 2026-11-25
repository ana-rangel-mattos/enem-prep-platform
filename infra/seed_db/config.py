from dotenv import load_dotenv
import os

load_dotenv()

HOST = os.getenv("DB_HOST")
DATABASE = os.getenv("DB_DATABASE")
USER = os.getenv("DB_USER")
PASSWORD = os.getenv("DB_PASSWORD")
PORT = os.getenv("DB_PORT")

LANGUAGES_ID = os.getenv("DB_LANGUAGES_ID")
NATURAL_SCIENCES_ID = os.getenv("DB_NATURAL_SCIENCES_ID")
SOCIAL_SCIENCES_ID = os.getenv("DB_SOCIAL_SCIENCES_ID")
MATH_ID = os.getenv("DB_MATH_ID")

POSTED_BY_ID = os.getenv("DB_POSTED_BY_ID")

