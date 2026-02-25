function toggleDrawer(drawerId) {
    const drawerContent = document.getElementById(drawerId);
    const headerIcon = drawerContent.previousElementSibling.querySelector('.material-icons');

    drawerContent.classList.toggle('User_Profile_drawer__content--closed');
    headerIcon.classList.toggle('material-icons--rotate-180');
}

document.addEventListener('DOMContentLoaded', function () {
    // Initialize carousels
    document.querySelectorAll('.User_Profile_carousel__container').forEach(carousel => {
        carousel.dataset.position = 0;
        carousel.style.transform = 'translateX(0)';
    });

    // Profile picture preview
    const profilePictureInput = document.getElementById('profilePicture');
    if (profilePictureInput) {
        profilePictureInput.addEventListener('change', function (e) {
            const file = e.target.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (event) {
                    document.getElementById('profilePicturePreview').src = event.target.result;
                };
                reader.readAsDataURL(file);
            }
        });
    }

    // Event picture preview
    const eventPictureInput = document.getElementById('eventPicture');
    if (eventPictureInput) {
        eventPictureInput.addEventListener('change', function (e) {
            const file = e.target.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (event) {
                    document.getElementById('eventPicturePreview').src = event.target.result;
                };
                reader.readAsDataURL(file);
            }
        });
    }

    // Privacy switch initialization
    const privacySwitch = document.getElementById('eventPrivacy');
    if (privacySwitch) {
        privacySwitch.addEventListener('change', updatePrivacyLabels);
    }
});

function nextSlide(carouselId) {
    const carousel = document.getElementById(carouselId);
    const cards = carousel.querySelectorAll('.User_Profile_event-card');
    if (cards.length === 0) return;

    const cardWidth = cards[0].offsetWidth + 20;
    const visibleCards = Math.floor(carousel.offsetWidth / cardWidth);
    const maxPosition = -(cardWidth * (cards.length - visibleCards));

    let currentPosition = parseInt(carousel.dataset.position) || 0;
    currentPosition -= cardWidth;

    if (currentPosition < maxPosition) currentPosition = maxPosition;

    carousel.style.transform = `translateX(${currentPosition}px)`;
    carousel.dataset.position = currentPosition;
}

function prevSlide(carouselId) {
    const carousel = document.getElementById(carouselId);
    const cards = carousel.querySelectorAll('.User_Profile_event-card');
    if (cards.length === 0) return;

    const cardWidth = cards[0].offsetWidth + 20;

    let currentPosition = parseInt(carousel.dataset.position) || 0;
    currentPosition += cardWidth;

    if (currentPosition > 0) currentPosition = 0;

    carousel.style.transform = `translateX(${currentPosition}px)`;
    carousel.dataset.position = currentPosition;
}

async function showGuestsModal(eventId, eventName) {
    const modal = document.getElementById('guestsModal');
    const modalTitle = document.getElementById('modalEventTitle');
    const guestList = document.getElementById('guestList');

    modalTitle.textContent = `${eventName} - Guests`;
    guestList.innerHTML = 'Loading...';

    try {
        const eventResponse = await fetch(`/api/events/${eventId}`);
        const eventData = await eventResponse.json();
        const maxAttendees = eventData.attendees_Number;

        const guestsResponse = await fetch(`/api/events/${eventId}/guests`);
        const guests = await guestsResponse.json();

        guestList.innerHTML = `
            <div style="margin-bottom: 15px; font-weight: 500;">
                Attendees: ${guests.length}/${maxAttendees}
            </div>
            <ul style="list-style: none; padding: 0;">
                ${guests.map(guest => `
                    <li class="User_Profile_guest-list__item">
                        <span>${guest.fullName}</span>
                        <button class="User_Profile_guest-list__remove-btn" 
                                onclick="removeGuest('${eventId}', '${guest.userId}', this)">
                            Remove
                        </button>
                    </li>
                `).join('')}
            </ul>
        `;

        modal.style.display = 'flex';
    } catch (error) {
        console.error('Error:', error);
        guestList.innerHTML = 'Error loading guests';
    }
}

async function removeGuest(eventId, userId, buttonElement) {
    if (!confirm('Are you sure you want to remove this guest?')) return;

    try {
        const response = await fetch(`/api/events/${eventId}/remove-guest/${userId}`, {
            method: 'POST',
            headers: {
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        });

        if (response.ok) {
            buttonElement.closest('.User_Profile_guest-list__item').remove();
            const counter = document.querySelector('#guestList > div');
            if (counter) {
                const [current, max] = counter.textContent.match(/\d+/g);
                counter.textContent = `Attendees: ${parseInt(current) - 1}/${max}`;
            }
        } else {
            const error = await response.text();
            alert(`Error: ${error}`);
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Error removing guest');
    }
}

function showEditModal(eventId, eventName) {
    const modal = document.getElementById('editEventModal');
    const form = document.getElementById('editEventForm');
    form.dataset.eventId = eventId;

    fetch(`/api/events/${eventId}`)
        .then(response => response.json())
        .then(eventData => {
            document.getElementById('eventName').value = eventData.event_Title;
            document.getElementById('eventType').value = eventData.category_ID;
            document.getElementById('eventDate').value = eventData.event_Date.split('T')[0];
            document.getElementById('startTime').value = eventData.event_time.substring(0, 5);
            document.getElementById('duration').value = eventData.event_Duration;
            document.getElementById('expectedAttendance').value = eventData.attendees_Number;
            document.getElementById('eventLocation').value = eventData.event_Location;
            document.getElementById('locationDetails').value = eventData.event_Location_Details;
            document.getElementById('eventDescription').value = eventData.event_Discription;
            document.getElementById('eventPrivacy').checked = eventData.isPublic;

            // Set event picture preview
            const eventPicturePreview = document.getElementById('eventPicturePreview');
            if (eventData.event_Picture) {
                eventPicturePreview.src = `data:image/jpeg;base64,${eventData.event_Picture}`;
            } else {
                eventPicturePreview.src = '/img/User-Profile-Page-imgs/default-event-image.jpg';
            }

            updatePrivacyLabels();
            modal.style.display = 'flex';
        })
        .catch(error => {
            console.error('Error fetching event data:', error);
            alert('Failed to load event data');
        });
}

function showEditProfileModal() {
    const modal = document.getElementById('editProfileModal');
    modal.style.display = 'flex';

    const passwordDrawerContent = document.getElementById('change-password');
    const passwordDrawerHeaderIcon = passwordDrawerContent.previousElementSibling.querySelector('.material-icons');

    if (!passwordDrawerContent.classList.contains('User_Profile_drawer__content--closed')) {
        passwordDrawerContent.classList.add('User_Profile_drawer__content--closed');
        passwordDrawerHeaderIcon.classList.add('material-icons--rotate-180');
    }
}

function updatePrivacyLabels() {
    const isPublic = document.getElementById('eventPrivacy').checked;
    document.getElementById('privateLabel').classList.toggle('User_Profile_switch__label--active', !isPublic);
    document.getElementById('publicLabel').classList.toggle('User_Profile_switch__label--active', isPublic);
}

async function saveEventChanges() {
    const form = document.getElementById('editEventForm');
    const eventId = form.dataset.eventId;
    const formData = new FormData();

    // Add form fields
    formData.append('Event_ID', eventId);
    formData.append('Event_Title', document.getElementById('eventName').value);
    formData.append('Category_ID', document.getElementById('eventType').value);
    formData.append('Event_Date', document.getElementById('eventDate').value);
    formData.append('Event_time', document.getElementById('startTime').value + ':00');
    formData.append('Event_Duration', document.getElementById('duration').value);
    formData.append('Attendees_Number', document.getElementById('expectedAttendance').value);
    formData.append('Event_Location', document.getElementById('eventLocation').value);
    formData.append('Event_Location_Details', document.getElementById('locationDetails').value);
    formData.append('Event_Discription', document.getElementById('eventDescription').value);
    formData.append('IsPublic', document.getElementById('eventPrivacy').checked);

    // Add event picture if selected
    const eventPictureInput = document.getElementById('eventPicture');
    if (eventPictureInput.files.length > 0) {
        formData.append('EventPicture', eventPictureInput.files[0]);
    }

    try {
        const response = await fetch('/User-Profile-Page?handler=EditEvent', {
            method: 'POST',
            headers: {
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: formData
        });

        if (response.ok) {
            alert('Event updated successfully!');
            closeModal();
            window.location.reload();
        } else {
            const error = await response.text();
            alert(`Error: ${error}`);
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Failed to save changes');
    }
}

async function cancelBookingFromProfile(eventId) {
    if (confirm('Are you sure you want to cancel this booking?')) {
        try {
            const response = await fetch(`/Event-View-Page/${eventId}?handler=CancelBooking`, {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                }
            });

            if (response.ok) {
                location.reload();
            }
        } catch (error) {
            console.error('Error:', error);
        }
    }
}

async function deleteEventFromProfile(eventId) {
    if (!confirm('Are you sure you want to delete this event?')) return;

    try {
        const response = await fetch(`/User-Profile-Page?handler=DeleteEvent&id=${eventId}`, {
            method: 'POST',
            headers: {
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value,
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            window.location.reload();
        } else {
            const error = await response.text();
            alert(`Error: ${error}`);
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Error deleting event');
    }
}

function closeModal() {
    document.getElementById('guestsModal').style.display = 'none';
    document.getElementById('editEventModal').style.display = 'none';
    document.getElementById('editProfileModal').style.display = 'none';
}

window.addEventListener('click', function (event) {
    const modals = document.querySelectorAll('.User_Profile_modal');
    modals.forEach(modal => {
        if (event.target === modal) {
            closeModal();
        }
    });
});

document.addEventListener('DOMContentLoaded', function () {
    const editProfileForm = document.getElementById('editProfileForm');
    if (editProfileForm) {
        editProfileForm.addEventListener('submit', function (event) {
            event.preventDefault();
            saveProfileChanges(event);
        });
    }
});

async function saveProfileChanges(event) {
    event.preventDefault();
    const form = document.getElementById('editProfileForm');

    if (!validatePasswordChange()) return;

    const formData = new FormData(form);
    try {
        const response = await fetch(form.action, {
            method: 'POST',
            headers: {
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: formData
        });

        if (response.ok) {
            window.location.reload();
        } else {
            const error = await response.text();
            alert(`Error: ${error}`);
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Error saving profile changes');
    }
}

function validatePasswordChange() {
    const currentPassword = document.getElementById('currentPassword').value;
    const newPassword = document.getElementById('newPassword').value;
    const confirmPassword = document.getElementById('confirmPassword').value;

    if (!currentPassword && !newPassword && !confirmPassword) return true;

    if (!currentPassword) {
        alert('Please enter your current password');
        return false;
    }
    if (!newPassword) {
        alert('Please enter a new password');
        return false;
    }
    if (newPassword !== confirmPassword) {
        alert('New passwords do not match');
        return false;
    }

    return true;
}