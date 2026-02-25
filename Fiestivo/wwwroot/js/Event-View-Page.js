document.addEventListener('DOMContentLoaded', function () {
    // Star rating functionality
    const ratingContainer = document.querySelector('.rating--selectable');
    if (!ratingContainer) return;

    const ratingStars = ratingContainer.querySelectorAll('i');
    const selectedRatingInput = document.getElementById('selectedRating');
    let currentRating = 0;

    function updateStarsDisplay(rating) {
        ratingStars.forEach((star, index) => {
            star.classList.toggle('fas', index < rating);
            star.classList.toggle('far', index >= rating);
        });
    }

    ratingStars.forEach(star => {
        star.addEventListener('click', function () {
            currentRating = parseInt(this.getAttribute('data-rating'));
            selectedRatingInput.value = currentRating;
            updateStarsDisplay(currentRating);
        });

        star.addEventListener('mouseover', function () {
            const hoverRating = parseInt(this.getAttribute('data-rating'));
            updateStarsDisplay(hoverRating);
        });

        star.addEventListener('mouseout', function () {
            updateStarsDisplay(currentRating);
        });
    });
    function updateStarsDisplay(rating = currentRating) {
        ratingStars.forEach((star, index) => {
            if (index < rating) {
                star.classList.remove('far');
                star.classList.add('fas');
            } else {
                star.classList.remove('fas');
                star.classList.add('far');
            }
        });
    }

    async function deleteEvent(eventId) {
        if (confirm('Are you sure you want to delete this event?')) {
            try {
                const response = await fetch(`?handler=DeleteEvent&id=${eventId}`, {
                    method: 'POST',
                    headers: {
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                    }
                });

                if (response.ok) {
                    window.location.href = '/User-Profile-Page'; // الانتقال للصفحة الرئيسية بعد الحذف
                }
            } catch (error) {
                console.error('Error:', error);
            }
        }
    }

    // Handle form submission
    const reviewForm = document.querySelector('.review-form');
    if (reviewForm) {
        reviewForm.addEventListener('submit', function (e) {
            if (currentRating < 1 || currentRating > 5) {
                e.preventDefault();
                document.querySelector('.rating-error').textContent = 'Please select a rating between 1 and 5 stars';
                return false;
            }

            // Debugging: Log the rating value before submission
            console.log('Submitting rating:', selectedRatingInput.value);
            return true;
        });
    }

    // Smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const targetId = this.getAttribute('href');
            if (targetId === '#') return;

            const targetElement = document.querySelector(targetId);
            if (targetElement) {
                targetElement.scrollIntoView({ behavior: 'smooth' });
            }
        });
    });
    document.querySelector('.review-form').addEventListener('submit', function (e) {
        const ratingInput = document.getElementById('selectedRating');
        if (!ratingInput.value || ratingInput.value === '0') {
            e.preventDefault();
            alert('Please select a rating by clicking on the stars');
            return false;
        }
        return true;
    });
});