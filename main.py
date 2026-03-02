from flask import Flask, render_template 
from flask_sqlalchemy import SQLAlchemy 
import database

app = Flask(__name__)

@app.route('/')
def home_page():
   return render_template('homepage.html')
@app.route('/CreateEvent', methods=['GET'])
def create_event():
    return render_template('creat.html')

@app.route('/login')
def login():
    return render_template('login.html')

if __name__ == "__main__":
    app.run(debug=True)
