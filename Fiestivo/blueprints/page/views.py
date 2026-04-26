from flask import render_template, request
from .contact import send_contact_email
from .forms import EventForm
from . import page


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


@page.route("/contact", methods=["GET", "POST"])
def contact():
    if request.method == "POST":

        send_contact_email()
    return render_template("page/contact.html")
