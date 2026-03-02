from flask import Flask, redirect, url_for , request
from main import app
import psycopg2         
conn = psycopg2.connect(
    host="localhost",
    database="Fiestivo",
    user="postgres",
    password="hasan"           
)
cur = conn.cursor()   
cur.execute('''CREATE TABLE IF NOT EXISTS events (
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
);''')
conn.commit()

@app.route('/CreateEvent', methods=['POST'])
def create_event():
    title          = request.form.get('title')
    event_type     = request.form.get('event_type')
    date           = request.form.get('date')
    time           = request.form.get('time')
    venue          = request.form.get('venue')
    area           = request.form.get('area')
    total_capacity = int(request.form.get('total_capacity'))
    confirmed_count= int(request.form.get('confirmed_count'))

    spots_open = total_capacity - confirmed_count  
    data = cur.execute("INSERT INTO events (title, event_type, date, time, venue, area, total_capacity, confirmed_count, spots_open) VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s)",
                (title, event_type, date, time, venue, area, total_capacity, confirmed_count, spots_open))
    return(redirect(url_for('home_page')))

 

cur.close()
conn.close()