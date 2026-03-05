from flask import Flask, render_template, redirect, url_for, request

app = Flask(__name__)
app.config["SECRET_KEY"] = "2f3c1ea789e0b9ea645e6d92"
from Fiestivo import database
from Fiestivo import forms
from Fiestivo import routes
