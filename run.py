from Fiestivo.blueprints.page import create_app, db
import os, logging
from logging.handlers import RotatingFileHandler

# from Fiestivo.blueprints.user import create_user
from Fiestivo.extensions import mail, login_manager, csrf
from dotenv import load_dotenv

""" def extensions(app):
    mail.init_app(app)
    db.init_app(app)
    login_manager.init_app(app)
    csrf.init_app(app)
    return None
 """

load_dotenv()
app = create_app()
with app.app_context():
    db.create_all()
    print("Tables created successfully!")
if __name__ == "__main__":
    if not app.debug:
        file_handler = RotatingFileHandler(
            "logs/fiestivo.log", maxBytes=10240, backupCount=10
        )
        file_handler.setFormatter(
            logging.Formatter(
                "%(asctime)s %(levelname)s: %(message)s [in %(pathname)s:%(lineno)d]"
            )
        )
        file_handler.setLevel(logging.INFO)
        app.logger.addHandler(file_handler)
        app.logger.setLevel(logging.INFO)
        app.logger.info("Fiestivo startup")
    else:
        app.run(
            debug=os.getenv("FLASK_ENV") != "production",
            host="0.0.0.0",
            port=int(os.getenv("PORT", 5000)),
        )
