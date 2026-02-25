from .app import db
from flask_login import UserMixin
from datetime import datetime, date, time


class User(UserMixin, db.Model):
    __tablename__ = 'users'
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(150), nullable=False, unique=True)
    full_name = db.Column(db.String(200))
    email = db.Column(db.String(200), unique=True)
    password_hash = db.Column(db.String(200))
    bio = db.Column(db.Text)
    profile_picture = db.Column(db.LargeBinary)

    events = db.relationship('Event', back_populates='user', cascade='all, delete-orphan')
    reviews = db.relationship('Review', back_populates='user', cascade='all, delete-orphan')


class Category(db.Model):
    __tablename__ = 'categories'
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(150), nullable=False)

    events = db.relationship('Event', back_populates='category')


class Event(db.Model):
    __tablename__ = 'events'
    id = db.Column(db.Integer, primary_key=True)
    title = db.Column(db.String(250), nullable=False)
    description = db.Column(db.Text)
    location = db.Column(db.String(250), nullable=False)
    duration = db.Column(db.Integer, nullable=False)
    location_details = db.Column(db.Text)
    date = db.Column(db.Date, nullable=False)
    time = db.Column(db.Time, nullable=False)
    is_public = db.Column(db.Boolean, default=True)
    attendees_number = db.Column(db.Integer, default=0)
    event_picture = db.Column(db.LargeBinary)

    user_id = db.Column(db.Integer, db.ForeignKey('users.id'), nullable=False)
    user = db.relationship('User', back_populates='events')

    category_id = db.Column(db.Integer, db.ForeignKey('categories.id'))
    category = db.relationship('Category', back_populates='events')

    attends = db.relationship('Attend', back_populates='event', cascade='all, delete-orphan')
    reviews = db.relationship('Review', back_populates='event', cascade='all, delete-orphan')

    @property
    def average_rating(self):
        if not self.reviews:
            return 0
        total = sum([r.rating for r in self.reviews])
        return round(total / len(self.reviews), 2)


class Attend(db.Model):
    __tablename__ = 'attends'
    id = db.Column(db.Integer, primary_key=True)
    user_id = db.Column(db.Integer, db.ForeignKey('users.id'))
    event_id = db.Column(db.Integer, db.ForeignKey('events.id'))

    user = db.relationship('User')
    event = db.relationship('Event', back_populates='attends')


class PostOn(db.Model):
    __tablename__ = 'post_ons'
    id = db.Column(db.Integer, primary_key=True)
    user_id = db.Column(db.Integer, db.ForeignKey('users.id'))
    event_id = db.Column(db.Integer, db.ForeignKey('events.id'))

    user = db.relationship('User')
    event = db.relationship('Event')


class Review(db.Model):
    __tablename__ = 'reviews'
    id = db.Column(db.Integer, primary_key=True)
    user_id = db.Column(db.Integer, db.ForeignKey('users.id'), nullable=False)
    event_id = db.Column(db.Integer, db.ForeignKey('events.id'), nullable=False)
    review_date = db.Column(db.DateTime, default=datetime.utcnow, nullable=False)
    comment = db.Column(db.String(500), nullable=False)
    rating = db.Column(db.Float, nullable=False)

    user = db.relationship('User', back_populates='reviews')
    event = db.relationship('Event', back_populates='reviews')
