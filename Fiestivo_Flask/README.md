# Fiestivo (Flask)

This is a minimal Flask port scaffold of the original ASP.NET Fiestivo project.

Quick start

1. Create a virtualenv and install requirements:

```bash
python -m venv .venv
.venv\Scripts\activate    # Windows
pip install -r Fiestivo_Flask/requirements.txt
```

2. Run the app:

```bash
set FLASK_APP=Fiestivo_Flask.app:create_app
flask run
```

This scaffold includes models, basic routes, and sample templates. It is intended as a starting point — further conversion of pages and full feature parity will require iterative work.
