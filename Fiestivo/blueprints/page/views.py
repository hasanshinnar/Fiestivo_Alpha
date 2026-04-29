import bcrypt
from flask import render_template, request, redirect, url_for, flash
from flask_login import login_required, login_user
from flask_bcrypt import Bcrypt
from .contact import send_contact_email
from .database import User
from .forms import EventForm, LoginForm, RegistrationForm
from . import page

bcrypt = Bcrypt()


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
    existing_email = User.query.filter_by(email=reg_form.email.data).first()
    existing_username = User.query.filter_by(username=reg_form.username.data).first()

    if request.method == "POST":
        action = request.form.get("form_type")

        # --- REGISTRATION LOGIC ---
        if action == "register" and reg_form.validate_on_submit():
            existing_username = User.query.filter_by(
                username=reg_form.username.data
            ).first()

            if existing_email:
                flash("Email already registered.")
            elif existing_username:
                flash("Username already taken.")
            else:
                hashed_pw = bcrypt.generate_password_hash(
                    reg_form.password.data
                ).decode("utf-8")

                new_user = User.create_new(
                    (
                        reg_form.full_name.data,
                        reg_form.username.data,
                        reg_form.email.data,
                        hashed_pw,
                    )
                )

                if new_user:
                    flash("Registration successful! Please login.")
                    return redirect(url_for("page.login"))
                else:
                    flash("Something went wrong with the database.")

        # --- LOGIN LOGIC ---
        elif action == "login" and login_form.validate_on_submit():
            user = User.query.filter_by(email=login_form.username.data).first()

            if user and bcrypt.check_password_hash(
                user.password, login_form.password.data
            ):
                login_user(user)
                return redirect(url_for(f"/{User.query.first().username}"))
            else:
                flash("Invalid credentials.")

    return render_template("page/login.html", login_form=login_form, reg_form=reg_form)


@page.route(f"/{User.query.first().username}")
@login_required
def dashboard():
    return render_template("page/dashboard.html")


@page.route("/CreateEvent")
def create_event():
    form = EventForm()
    return render_template("page/create.html", forms=form)


@page.route("/contact", methods=["GET", "POST"])
def contact():
    if request.method == "POST":

        send_contact_email()
    return render_template("page/contact.html")
