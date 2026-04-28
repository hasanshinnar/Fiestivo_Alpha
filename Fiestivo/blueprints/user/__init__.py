from flask import Flask, Blueprint
from .contact import mail
import os
from dotenv import load_dotenv

env_path = os.path.join(os.path.dirname(__file__), "..", "config", ".env")
load_dotenv(dotenv_path=env_path)

user = Blueprint("user", __name__, template_folder="templates", static_folder="static")
app = Flask(__name__)
mail.init_app(app)


def create_user():
    from . import views

    app.register_blueprint(user)

    return app
