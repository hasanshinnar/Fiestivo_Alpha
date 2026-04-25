from flask import Flask, Blueprint

page = Blueprint("page", __name__, template_folder="templates", static_folder="static")


def create_app():
    app = Flask(__name__)
    from . import views

    app.register_blueprint(page)

    return app
