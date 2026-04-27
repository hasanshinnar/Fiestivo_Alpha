from flask import render_template, request, redirect, url_for, flash
from flask_login import login_user
from flask_bcrypt import Bcrypt
from wtforms import form
from .contact import send_contact_email
from .database import Users, db
from .forms import EventForm, LoginForm, RegistrationForm
from . import page
from werkzeug.security import generate_password_hash, check_password_hash


@page.route("/")
def home_page():
    return render_template("page/homepage.html")


@page.route("/browse+events")
def browse_events():
    return render_template("page/browse_events.html")


@page.route("/login", methods=["GET", "POST"])
def login():
    login_form = LoginForm()
    reg_form = RegistrationForm()
    error_message = None

    if request.method == "POST":
        action = request.form.get("form_type")

        # --- REGISTRATION LOGIC ---
        if action == "register" and reg_form.validate_on_submit():
            existing_user = Users.query.filter_by(email=reg_form.Email.data).first()
            if existing_user:
                flash("Email already registered.")
            elif reg_form.validate_on_submit():
                hashed_pw = (
                    Bcrypt()
                    .generate_password_hash(reg_form.password.data)
                    .decode("utf-8")
                )
                new_user = Users(
                    email=reg_form.Email.data,
                    password=hashed_pw,
                )

                db.session.add(new_user)
                db.session.commit()
                flash("Registration successful! Please login.")
                return redirect(url_for("page.login"))

        # --- LOGIN LOGIC ---
        elif action == "login" and login_form.validate_on_submit():
            user = Users.query.filter_by(email=login_form.username.data).first()

            if user and check_password_hash(user.password, login_form.password.data):
                login_user(user)
                return redirect(url_for("page.dashboard"))
            else:
                flash("Invalid credentials.")

    return render_template(
        "page/login.html", error=error_message, login_form=login_form, reg_form=reg_form
    )


@page.route("/CreateEvent")
def create_event():
    form = EventForm()
    return render_template("page/create.html", forms=form)


@page.route("/contact", methods=["GET", "POST"])
def contact():
    if request.method == "POST":

        send_contact_email()
    return render_template("page/contact.html")
