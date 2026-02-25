from flask import current_app as app, render_template, redirect, url_for, request, flash
from flask_login import login_user, logout_user, login_required, current_user
from werkzeug.security import generate_password_hash, check_password_hash
from .app import db, login_manager
from .models import User, Event, Category, Review, Attend


@app.route('/')
def index():
    events = Event.query.order_by(Event.date.asc(), Event.time.asc()).all()
    return render_template('index.html', events=events)


@app.route('/events')
def events():
    events = Event.query.all()
    return render_template('events.html', events=events)


@app.route('/event/<int:event_id>')
def event_view(event_id):
    event = Event.query.get_or_404(event_id)
    return render_template('event_view.html', event=event)


@app.route('/categories')
def categories():
    cats = Category.query.all()
    return render_template('categories.html', categories=cats)


@app.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST':
        username = request.form['username']
        password = request.form['password']
        user = User.query.filter_by(username=username).first()
        if user and check_password_hash(user.password_hash or '', password):
            login_user(user)
            return redirect(url_for('index'))
        flash('Invalid credentials', 'danger')
    return render_template('login.html')


@app.route('/signup', methods=['GET', 'POST'])
def signup():
    if request.method == 'POST':
        username = request.form['username']
        email = request.form['email']
        password = request.form['password']
        if User.query.filter((User.username==username)|(User.email==email)).first():
            flash('User exists', 'warning')
            return redirect(url_for('signup'))
        u = User(username=username, email=email, password_hash=generate_password_hash(password))
        db.session.add(u)
        db.session.commit()
        login_user(u)
        return redirect(url_for('index'))
    return render_template('signup.html')


@app.route('/logout')
@login_required
def logout():
    logout_user()
    return redirect(url_for('index'))


@app.route('/profile/<int:user_id>')
def profile(user_id):
    user = User.query.get_or_404(user_id)
    return render_template('user_profile.html', user=user)


@app.route('/create-event', methods=['GET', 'POST'])
@login_required
def create_event():
    if request.method == 'POST':
        title = request.form.get('title')
        location = request.form.get('location')
        date = request.form.get('date')
        time_val = request.form.get('time')
        duration = int(request.form.get('duration') or 1)
        # minimal parsing; in a real app validate and sanitize
        from datetime import datetime
        event = Event(
            title=title,
            location=location,
            date=datetime.strptime(date, '%Y-%m-%d').date(),
            time=datetime.strptime(time_val, '%H:%M').time(),
            duration=duration,
            user_id=current_user.id
        )
        db.session.add(event)
        db.session.commit()
        return redirect(url_for('event_view', event_id=event.id))
    cats = Category.query.all()
    return render_template('create_event.html', categories=cats)


@login_manager.user_loader
def load_user(user_id):
    return User.query.get(int(user_id))
