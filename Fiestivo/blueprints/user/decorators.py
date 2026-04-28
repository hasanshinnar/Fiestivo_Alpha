from functools import wraps
from flask import flash, redirect
from flask_login import current_user
from sqlalchemy.util import decorator
from wtforms.validators import url

def anonymous_required(url="/"): 
    def decorator(f):
        @wraps(f)
        def decorated_function(*args, **kwargs):
            if current_user.is_authenticated:
                return redirect(url)

            return f(*args, **kwargs)

        return decorated_function
    return decorator