from flask_wtf import FlaskForm
from .database import User
from . import app
from wtforms import (
    StringField,
    IntegerField,
    DateField,
    TimeField,
    SubmitField,
    SelectField,
    PasswordField,
)
from wtforms.validators import (
    DataRequired,
    NumberRange,
    InputRequired,
    Length,
    ValidationError,
)
from flask import Flask
from flask_mail import Mail


class EventForm(FlaskForm):
    title = StringField(label="Event Title", validators=[DataRequired()])

    event_type = SelectField(
        label="Event Type",
        validators=[DataRequired()],
        choices=[
            ("", "Choose category"),
            ("football", "⚽ Football"),
            ("padel", "🎾 Padel"),
            ("basketball", "🏀 Basketball"),
            ("mafia", "🃏 Mafia / Card Games"),
            ("dinner", "🍽️ Hash w Nash (Lunch/Dinner)"),
            ("chalet", "🏖️ Chalet / Pool Day"),
            ("gaming", "🎮 Gaming Night (PlayStation)"),
            ("hiking", "🏔️ Hiking / Nature Trip"),
            ("roadtrip", "🚗 Road Trip"),
            ("bowling", "🎳 Bowling Night"),
            ("paintball", "🎯 Paintball / Laser Tag"),
            ("cinema", "🎬 Cinema Outing"),
            ("other", "Other"),
        ],
    )

    date = DateField(label="Event Date", validators=[DataRequired()])
    time = TimeField(label="Start Time", validators=[DataRequired()])
    venue = StringField(label="Venue", validators=[DataRequired()])
    area = StringField(label="Area", validators=[DataRequired()])
    total_capacity = IntegerField(
        label="Total Capacity", validators=[DataRequired(), NumberRange(min=2, max=100)]
    )
    confirmed_count = IntegerField(
        label="Confirmed Count", validators=[NumberRange(min=0)]
    )
    submit = SubmitField(label="Create Event")


class ContactForm(FlaskForm):
    mail = Mail(app)
    first_name = StringField(label="First Name", validators=[DataRequired()])
    last_name = StringField(label="Last Name", validators=[DataRequired()])
    email = StringField(label="Email", validators=[DataRequired()])
    subject = SelectField(
        label="Subject",
        validators=[DataRequired()],
        choices=[
            ("", "Choose topic"),
            ("Report a bug", "Report a bug"),
            ("Feature request", "Feature request"),
            ("Partnership / collaboration", "Partnership / collaboration"),
            ("Account issue", "Account issue"),
            ("Something else", "Something else"),
        ],
    )
    message = StringField(label="Message", validators=[DataRequired()])
    submit = SubmitField(label="Send Message")


class LoginForm(FlaskForm):
    email = StringField(label="Email", validators=[DataRequired()])
    username = StringField(label="Username", validators=[DataRequired()])
    password = PasswordField(label="Password", validators=[DataRequired()])
    submit = SubmitField(label="Login")


class RegistrationForm(FlaskForm):
    full_name = StringField(
        label="Full Name",
        validators=[DataRequired(), Length(min=2, max=50)],
        render_kw={"placeholder": "Full Name"},
    )
    username = StringField(
        label="Username",
        validators=[DataRequired(), Length(min=2, max=20)],
        render_kw={"placeholder": "Username"},
    )
    email = StringField(
        label="Email", validators=[DataRequired()], render_kw={"placeholder": "Email"}
    )
    password = PasswordField(
        label="Password",
        validators=[
            DataRequired(),
            Length(min=6, max=20),
        ],
        render_kw={"placeholder": "Password"},
    )
    submit = SubmitField(label="Create Account")

    def validate_email(self, email):
        existing_user = User.query.filter_by(email=email.data).first()
        if existing_user:
            raise ValidationError(
                "This email is already registered. Please choose a different one."
            )

    def validate_username(self, username):
        existing_user = User.query.filter_by(username=username.data).first()
        if existing_user:
            raise ValidationError(
                "This username is already taken. Please choose a different one."
            )
