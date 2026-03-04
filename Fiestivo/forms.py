from flask_wtf import FlaskForm
from wtforms import StringField, IntegerField, DateField, TimeField, SubmitField


class EventForm(FlaskForm):
    title = StringField(label="Event Title")
    event_type = StringField(label="Event Type")
    date = DateField(label="Event Date")
    time = TimeField(label="Start Time")
    venue = StringField(label="Venue")
    area = StringField(label="Area")
    total_capacity = IntegerField(label="Total Capacity")
    confirmed_count = IntegerField(label="Confirmed Count")
    submit = SubmitField(label="Create Event")
