from flask import request, flash
from flask_mail import Message, Mail

mail = Mail()


def send_contact_email():
    first_name = request.form.get("First Name")
    last_name = request.form.get("Last Name")
    full_name = f"{first_name} {last_name}"
    email = request.form.get("Email")
    subject = request.form.get("Subject") or "No Subject"
    message_content = request.form.get("Message")

    msg = Message(
        subject=f"New Form: {subject} - {full_name}",
        sender="from@example.com",
        recipients=["your-email@example.com"],
    )

    msg.body = f"""
    New contact form submission:
    ---------------------------
    Name: {full_name}
    Email: {email}
    Subject: {subject}
    
    Message:
    {message_content}
    """

    try:
        mail.send(msg)
        flash("Your message has been sent successfully!", "success")
    except Exception as e:
        flash(f"Error sending message: {str(e)}", "danger")
