FROM python:3.11-slim

WORKDIR /Fiestivo

ENV FLASK_APP=run.py \
       FLASK_ENV=production \
       PYTHONDONTWRITEBYTECODE=1 \
       PYTHONUNBUFFERED=1
       

COPY requirements.txt requirements.txt
RUN pip install --no-cache-dir -r requirements.txt

COPY . .

EXPOSE 5000
RUN adduser --disabled-password --gecos "" appuser
USER appuser
CMD ["python", "run.py"]