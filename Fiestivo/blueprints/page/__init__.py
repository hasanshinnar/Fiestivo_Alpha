# from Fiestivo.blueprints.page.routes import page
from flask import Flask

from .routes import page


def create_app():
    app = Flask(__name__, template_folder="templates")
    app.register_blueprint(page)
    return app
