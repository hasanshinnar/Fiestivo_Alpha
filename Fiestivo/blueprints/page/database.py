import os
from flask_sqlalchemy import SQLAlchemy
from dotenv import load_dotenv
from config.settings import Debug_mode
from flask_login import UserMixin

Debug = Debug_mode()
env_path = os.path.join(os.path.dirname(__file__), "..", "config", ".env")
load_dotenv(dotenv_path=env_path)
db = SQLAlchemy()


class Users(db.Model, UserMixin):
    __tablename__ = "users"
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(100), unique=True, nullable=False)  # Add this!
    email = db.Column(db.String(255), unique=True, nullable=False)
    password = db.Column(db.String(255), nullable=False)


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
