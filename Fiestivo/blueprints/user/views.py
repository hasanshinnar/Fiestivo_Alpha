from blueprints.page.forms import LoginForm
from . import user
from flask import redirect, render_template, request, url_for , flash , safe_next_url
from flask_login import login_required, logout_user
from .contact import send_contact_email
from .forms import EventForm
from page.database import User , db 
from .decorators import anonymous_required
from flask_login import login_user


@user.route("/")
def home_page():
    return render_template("user/homepage.html")

@user.route('/login', methods=['GET', 'POST'])
@anonymous_required()
def login():
    form = LoginForm() 

    if form.validate_on_submit():
        u = User.find_by_identity(form.identity.data)

        if u and u.authenticated(password=form.password.data):
            
            if login_user(u, remember=form.remember_me.data):
                u.update_activity_tracking(request.remote_addr)
                db.session.commit() 
                next_url = request.args.get('next')
                if next_url and next_url.startswith('/'):
                    return redirect(next_url)

                return redirect(url_for('user.home_page'))
            else:
                flash('This account has been disabled.', 'error')
        else:
            flash('Invalid identity or password.', 'error')

    return render_template('user/login.html', form=form)


@user.route("/logout")
@login_required
def logout():
    logout_user()
    return redirect(url_for("page.home_page"))


@user.route("/user")
def user_page():
    return render_template("user/user.html")


@user.route("/CreateEvent")
def create_event():
    form = EventForm()
    return render_template("user/create.html", forms=form)


@user.route("/contact", methods=["GET", "POST"])
def contact():
    if request.method == "POST":

        send_contact_email()
    return render_template("user/contact.html")
