import sqlalchemy, psycopg2, os
from flask_sqlalchemy import SQLAlchemy
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from sqlalchemy_utils import database_exists, create_database
from dotenv import load_dotenv
from config.settings import Debug_mode, SQLALCHEMY_DATABASE_URI
from flask_login import UserMixin
from wtforms.validators import URL, url

Debug = Debug_mode()

db_uri = SQLALCHEMY_DATABASE_URI

engine = create_engine(db_uri)
if not database_exists(engine.url):
    create_database(engine.url)
db = SQLAlchemy()


class User(db.Model, UserMixin):
    __tablename__ = "users"

    id = db.Column(db.Integer, primary_key=True)
    full_name = db.Column(db.String(255), nullable=False)
    username = db.Column(db.String(100), unique=True, nullable=False)
    email = db.Column(db.String(255), unique=True, nullable=False)
    password = db.Column(db.String(255), nullable=False)

    @staticmethod
    def create_new(data_tuple):
        """Creates a user from a tuple: (name, username, email, password)"""
        new_user = User(
            full_name=data_tuple[0],
            username=data_tuple[1],
            email=data_tuple[2],
            password=data_tuple[3],
        )
        try:
            db.session.add(new_user)
            db.session.commit()
            return new_user
        except Exception as e:
            db.session.rollback()
            print(f"Error inserting user: {e}")
            return None


class Events(db.Model):

    __tablename__ = "events"
    id = db.Column(db.Integer, primary_key=True)
    title = db.Column(db.String(255), nullable=False)
    event_type = db.Column(db.String(100), nullable=False)
    date = db.Column(db.Date, nullable=False)
    time = db.Column(db.Time, nullable=False)
    venue = db.Column(db.String(255))
    area = db.Column(db.String(100))
    total_capacity = db.Column(db.Integer, nullable=False)
    confirmed_count = db.Column(db.Integer, default=0)
    spots_open = db.Column(db.Integer, nullable=False)

    def insert_event(data_tuple):
        new_event = Events(
            title=data_tuple[0],
            event_type=data_tuple[1],
            date=data_tuple[2],
            time=data_tuple[3],
            venue=data_tuple[4],
            area=data_tuple[5],
            total_capacity=data_tuple[6],
            confirmed_count=data_tuple[7],
            spots_open=data_tuple[8],
        )
        try:
            db.session.add(new_event)
            db.session.commit()
        except Exception as e:
            db.session.rollback()
            print(f"Error inserting event:", e)
