from flask import Flask, render_template, redirect, url_for , request
import psycopg2     

app = Flask(__name__)

conn = psycopg2.connect(
    host="localhost",
    database="Fiestivo ",
    user="Hasan",
    password="medo"           
)
cursor = conn.cursor()   

cursor.execute('''CREATE TABLE IF NOT EXISTS events (
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

@app.route('/CreateEvent', methods=['POST'])
def create_event_post():
    title          = request.form.get('title')
    event_type     = request.form.get('event_type')
    date           = request.form.get('date')
    time           = request.form.get('time')
    venue          = request.form.get('venue')
    area           = request.form.get('area')
    total_capacity = int(request.form.get('total_capacity'))
    confirmed_count= int(request.form.get('confirmed_count'))

    spots_open = total_capacity - confirmed_count  
    data = cursor.execute("INSERT INTO events (title, event_type, date, time, venue, area, total_capacity, confirmed_count, spots_open) VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s)",
                (title, event_type, date, time, venue, area, total_capacity, confirmed_count, spots_open))
    conn.commit()
    return(redirect(url_for('home_page')))
    cursor.close()
    conn.close()    

@app.route('/')
def home_page():
    return render_template('homepage.html')

@app.route('/CreateEvent', methods=['GET' , 'POST']) 
def create_event():
    return render_template('creat.html')

@app.route('/login')
def login():
    return render_template('login.html')



if __name__ == "__main__":
    app.run(debug=True)
