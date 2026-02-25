 document.addEventListener('DOMContentLoaded', function() {
            // Mobile Menu Toggle
            const menuToggle = document.querySelector('.banner__mobile-toggle');
            const navLinks = document.querySelector('.banner__nav');
            
            if (menuToggle) {
                menuToggle.addEventListener('click', function() {
                    navLinks.classList.toggle('banner__nav--visible');
                });
            }

            // Close Mobile Menu on Link Click
            document.querySelectorAll('.banner__nav-link').forEach(link => {
                link.addEventListener('click', function() {
                    navLinks.classList.remove('banner__nav--visible');
                });
            });

            

            // Card Animation
            document.querySelectorAll('.event-card').forEach((card, index) => {
                card.style.opacity = '0';
                card.style.transform = 'translateY(20px)';
                card.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
                
                setTimeout(() => {
                    card.style.opacity = '1';
                    card.style.transform = 'translateY(0)';
                }, index * 100);
            });
        });