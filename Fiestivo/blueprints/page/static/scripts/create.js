document.querySelectorAll(".toggle").forEach((toggle) => {
  toggle.addEventListener("click", function () {
    const hiddenInput = this.nextElementSibling;
    if (this.classList.contains("on")) {
      hiddenInput.value = "on";
    } else {
      hiddenInput.value = "off";
    }
  });
});
