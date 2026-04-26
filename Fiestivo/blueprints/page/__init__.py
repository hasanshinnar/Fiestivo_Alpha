from flask import Flask, Blueprint
from .contact import mail
import os
from dotenv import load_dotenv

env_path = os.path.join(os.path.dirname(__file__), "..", "config", ".env")
load_dotenv(dotenv_path=env_path)

page = Blueprint("page", __name__, template_folder="templates", static_folder="static")
app = Flask(__name__)
app.config["MAIL_SERVER"] = "sandbox.smtp.mailtrap.io"
app.config["MAIL_PORT"] = 2525
app.config["MAIL_USERNAME"] = os.getenv("MAIL_USERNAME")
app.config["MAIL_PASSWORD"] = os.getenv("MAIL_PASSWORD")
app.config["MAIL_USE_TLS"] = True
app.config["MAIL_USE_SSL"] = False
mail.init_app(app)


def create_app():
    from . import views

    app.register_blueprint(page)

    return app
