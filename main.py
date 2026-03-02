from flask import Flask, render_template 
from flask_sqlalchemy import SQLAlchemy 
app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///fiestivo.db'
db = SQLAlchemy(app)

class item(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(80), nullable=False)
    description = db.Column(db.String(200), nullable=False)
    

@app.route('/')
def home_page():
   return render_template('homepage.html')
@app.route('/CreateEvent')
def create_event():
    return render_template('creat.html')

@app.route('/login')
def login():
    return render_template('login.html')

if __name__ == "__main__":
    app.run(debug=True)
