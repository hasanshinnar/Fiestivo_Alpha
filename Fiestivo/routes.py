from flask import render_template, redirect, url_for, request
from Fiestivo import app
from Fiestivo import database


@app.route("/")
def home_page():
    return render_template("homepage.html")


@app.route("/login")
def login():
    return render_template("login.html")


@app.route("/CreateEvent", methods=["GET", "POST"])
def create_event():
    if request.method == "POST":
        title = request.form.get("title")
        event_type = request.form.get("event_type")
        date = request.form.get("date")
        time = request.form.get("time")
        venue = request.form.get("venue")
        area = request.form.get("area")
        total_capacity = int(request.form.get("total_capacity"))
        confirmed_count = int(request.form.get("confirmed_count") or 0)

        spots_open = total_capacity - confirmed_count

        database.insert_event(
            (
                title,
                event_type,
                date,
                time,
                venue,
                area,
                total_capacity,
                confirmed_count,
                spots_open,
            )
        )

        return redirect(url_for("home_page"))

    return render_template("creat.html")
