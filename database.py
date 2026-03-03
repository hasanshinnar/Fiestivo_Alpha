import psycopg2
from dotenv import load_dotenv
import os

load_dotenv()


def insert_event(data):
    with psycopg2.connect(
        host=os.getenv("DB_HOST"),
        database=os.getenv("DB_NAME"),
        user=os.getenv("DB_USER"),
        password=os.getenv("DB_PASSWORD"),
    ) as conn:
        with conn.cursor() as cur:
            cur.execute("""CREATE TABLE IF NOT EXISTS events (
    id SERIAL PRIMARY KEY, 
    title VARCHAR(255) NOT NULL,
    event_type VARCHAR(100) NOT NULL,
    date DATE NOT NULL,
    time TIME NOT NULL,
    venue VARCHAR(255) ,
    area VARCHAR(100),
    total_capacity INTEGER NOT NULL,
    confirmed_count INTEGER ,
    spots_open INTEGER 
       );""")
            cur.execute(
                """
                INSERT INTO events 
                (title, event_type, date, time, venue, area, total_capacity, confirmed_count, spots_open)
                VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s)
            """,
                data,
            )
