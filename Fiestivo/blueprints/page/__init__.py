import os
from flask import Flask, Blueprint
from flask_login import LoginManager
from dotenv import load_dotenv
from .database import User, db
from .contact import mail

login_manager = LoginManager()
page = Blueprint("page", __name__, template_folder="templates", static_folder="static")
app = Flask(__name__)


def create_app():
    env_path = os.path.join(os.path.dirname(__file__), "..", "..", "config", ".env")
    load_dotenv(dotenv_path=env_path)
    app.config.from_object("config.settings")
    app.config.update(
        MAIL_SERVER="sandbox.smtp.mailtrap.io",
        MAIL_PORT=2525,
        MAIL_USERNAME=os.getenv("MAIL_USERNAME"),
        MAIL_PASSWORD=os.getenv("MAIL_PASSWORD"),
        MAIL_USE_TLS=True,
        MAIL_USE_SSL=False,
    )

    db.init_app(app)
    mail.init_app(app)
    login_manager.init_app(app)
    login_manager.login_view = "page.login"

    @login_manager.user_loader
    def load_user(user_id):
        return User.query.get(int(user_id))

    from . import views

    app.register_blueprint(page)

    return app
