from Fiestivo.blueprints.page import create_app, db
# from Fiestivo.blueprints.user import create_user
from Fiestivo.extensions import mail,login_manager, csrf
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
    app.run(debug=True)
