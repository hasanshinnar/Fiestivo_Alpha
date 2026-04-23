from flask import Flask
from .views import page


def create_app():
    import os

    template_dir = os.path.abspath("Fiestivo/templates")
    app = Flask(__name__, template_folder=template_dir)
    app.config["SECRET_KEY"] = "your-secret-key"
    app.register_blueprint(page)
    return app
