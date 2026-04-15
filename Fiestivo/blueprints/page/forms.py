from flask_wtf import FlaskForm
from wtforms import (
    StringField,
    IntegerField,
    DateField,
    TimeField,
    SubmitField,
    SelectField,
)
from wtforms.validators import DataRequired, NumberRange


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
