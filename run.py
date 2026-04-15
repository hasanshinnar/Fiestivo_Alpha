from flask import Flask
import os
from dotenv import load_dotenv
from Fiestivo.blueprints.page import page

load_dotenv()
app = Flask(__name__, template_folder="templates")
app.config["SECRET_KEY"] = os.getenv("SECRET_KEY")
app.register_blueprint(page)
if __name__ == "__main__":
    app.run(debug=True)
