import os
from dotenv import load_dotenv

load_dotenv()


def Debug_mode():
    Debug = True
    return Debug


# Database Configuration
db_uri = f'postgresql://{os.getenv("DB_USER")}:{os.getenv("DB_PASSWORD")}@{os.getenv("DB_HOST")}/{os.getenv("DB_NAME")}'
SQLALCHEMY_DATABASE_URI = db_uri
SECRET_KEY = os.getenv("DB_SECRET_KEY")
SQLALCHEMY_TRACK_MODIFICATIONS = False

SQLALCHEMY_ENGINE_OPTIONS = {
    "pool_size": 10,
    "pool_recycle": 3600,
    "pool_pre_ping": True,
    "max_overflow": 20,
}


# User
SEED_ADMIN_EMAIL = os.getenv("SEED_ADMIN_EMAIL")
SEED_ADMIN_PASSWORD = os.getenv("SEED_ADMIN_PASSWORD")
REMEMBER_COOKIE_DURATION = 60 * 60 * 24 * 7  # 7 days
