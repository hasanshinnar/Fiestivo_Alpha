from flask import Blueprint, render_template
from .forms import EventForm
import os

# template_dir = os.path.abspath("Fiestivo/templates")
page = Blueprint("page", __name__, template_folder="templates")


@page.route("/")
def home_page():
    return render_template("page/homepage.html")


@page.route("/login")
def login():
    return render_template("page/login.html")


@page.route("/CreateEvent")
def create_event():
    form = EventForm()
    return render_template("page/create.html", forms=form)
