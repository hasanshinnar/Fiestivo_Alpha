let items = document.querySelectorAll('.home_slider__item');
    let next = document.getElementById('next');
    let prev = document.getElementById('prev');
    let thumbnails = document.querySelectorAll('.home_thumbnail__item');

    // config param
    let countItem = items.length;
    let itemActive = 0;

    // event next click
    next.onclick = function() {
        itemActive = itemActive + 1;
        if (itemActive >= countItem) {
            itemActive = 0;
        }
        showSlider();
    }

    // event prev click
    prev.onclick = function() {
        itemActive = itemActive - 1;
        if (itemActive < 0) {
            itemActive = countItem - 1;
        }
        showSlider();
    }

    // auto run slider
    let refreshInterval = setInterval(() => {
        next.click();
    }, 5000)

    function showSlider() {
        // remove item active old
        let itemActiveOld = document.querySelector('.home_slider__item--active');
        let thumbnailActiveOld = document.querySelector('.home_thumbnail__item--active');
        itemActiveOld.classList.remove('home_slider__item--active');
        thumbnailActiveOld.classList.remove('home_thumbnail__item--active');

        // active new item
        items[itemActive].classList.add('home_slider__item--active');
        thumbnails[itemActive].classList.add('home_thumbnail__item--active');

        // clear auto time run slider
        clearInterval(refreshInterval);
        refreshInterval = setInterval(() => {
            next.click();
        }, 5000)
    }

    // click thumbnails
    thumbnails.forEach((thumbnail, index) => {
        thumbnail.addEventListener('mouseover', () => {
            itemActive = index;
            showSlider();
        })
    })