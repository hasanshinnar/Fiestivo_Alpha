from Fiestivo.blueprints.page import create_app
# from Fiestivo.blueprints.user import create_app
from Fiestivo.extensions import mail, db, login_manager, csrf
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
if __name__ == "__main__":
    app.run(debug=True)
