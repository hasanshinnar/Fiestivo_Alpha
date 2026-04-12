  const slides = document.querySelectorAll(".slide");
  const thumbs = document.querySelectorAll(".thumb");
  const dots = document.querySelectorAll(".dot");
  let current = 0;
  let timer;

  function goTo(i) {
    slides[current].classList.remove("active");
    thumbs[current].classList.remove("active");
    dots[current].classList.remove("active");
    current = i;
    slides[current].classList.add("active");
    thumbs[current].classList.add("active");
    dots[current].classList.add("active");
  }

  function autoplay() {
    timer = setInterval(() => goTo((current + 1) % slides.length), 4000);
  }

  thumbs.forEach((t) => {
    t.addEventListener("click", () => {
      clearInterval(timer);
      goTo(+t.dataset.index);
      autoplay();
    });
  });
  dots.forEach((d) => {
    d.addEventListener("click", () => {
      clearInterval(timer);
      goTo(+d.dataset.index);
      autoplay();
    });
  });

  autoplay();