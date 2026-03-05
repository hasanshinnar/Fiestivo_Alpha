from flask import render_template, redirect, url_for, request
from Fiestivo import app
from Fiestivo import database
from Fiestivo.forms import EventForm


@app.route("/")
def home_page():
    return render_template("homepage.html")


@app.route("/login")
def login():
    return render_template("login.html")


@app.route("/CreateEvent")
def create_event():
    form = EventForm()

    return render_template("creat.html", forms=form)
