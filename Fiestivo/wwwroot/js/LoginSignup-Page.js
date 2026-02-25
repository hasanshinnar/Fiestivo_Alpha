document.addEventListener('DOMContentLoaded', function() {
            // Form switching functionality
            const loginForm = document.querySelector('.fiestivo-login__form-box--login');
            const registerForm = document.querySelector('.fiestivo-login__form-box--register');
            const signUpLink = document.querySelector('.signUpLink');
            const signInLink = document.querySelector('.signInLink');
            const formContainer = document.querySelector('.fiestivo-login__form-container');

            function switchForms(fromForm, toForm) {
                fromForm.style.animation = 'slideOut 0.3s ease-out forwards'; // Faster slideOut
                setTimeout(() => {
                    fromForm.style.display = 'none';
                    toForm.style.display = 'flex';
                    toForm.style.animation = 'slideIn 0.3s ease-out forwards'; // Faster slideIn
                    formContainer.style.height = toForm.scrollHeight + 40 + 'px';
                }, 300); // Keep the timeout consistent with the animation duration
    }
    document.querySelector('.fiestivo-login__form-box--register form').addEventListener('submit', function (e) {
        const emailInput = document.getElementById('email');
        const email = emailInput.value.trim();

        // Simple email regex validation
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (!emailRegex.test(email)) {
            e.preventDefault();
            alert('الرجاء إدخال بريد إلكتروني صحيح');
            emailInput.focus();
        }
    });

            // Add animation keyframes
            const style = document.createElement('style');
            style.textContent = `
                @keyframes slideOut {
                    from { opacity: 1; transform: translateX(0); }
                    to { opacity: 0; transform: translateX(-20px); }
                }
                @keyframes slideIn {
                    from { opacity: 0; transform: translateX(20px); }
                    to { opacity: 1; transform: translateX(0); }
                }
            `;
            document.head.appendChild(style);

            // Form switch event listeners
            signUpLink.addEventListener('click', function(e) {
                e.preventDefault();
                switchForms(loginForm, registerForm);
            });

            signInLink.addEventListener('click', function(e) {
                e.preventDefault();
                switchForms(registerForm, loginForm);
            });

            // Set initial container height
            formContainer.style.height = loginForm.scrollHeight + 40 + 'px';

            // Mobile menu toggle
            const menuToggle = document.querySelector('.fiestivo-login__mobile-menu-toggle');
            const navLinks = document.querySelector('.fiestivo-login__nav-links');

            if (menuToggle && navLinks) {
                menuToggle.addEventListener('click', function() {
                    navLinks.classList.toggle('fiestivo-login__nav-links--show');
                });

                // Close menu when clicking on links
                document.querySelectorAll('.fiestivo-login__nav-links a').forEach(link => {
                    link.addEventListener('click', function() {
                        navLinks.classList.remove('fiestivo-login__nav-links--show');
                    });
                });
            }


            // Search functionality
            const searchBtn = document.querySelector('.fiestivo-login__search-btn');
            const searchInput = document.querySelector('.fiestivo-login__search-input');

            if (searchBtn && searchInput) {
                searchBtn.addEventListener('click', function() {
                    if (searchInput.value.trim() !== '') {
                        alert('Searching for: ' + searchInput.value);
                    }
                });

                searchInput.addEventListener('keypress', function(e) {
                    if (e.key === 'Enter' && searchInput.value.trim() !== '') {
                        alert('Searching for: ' + searchInput.value);
                    }
                });
            }
        });