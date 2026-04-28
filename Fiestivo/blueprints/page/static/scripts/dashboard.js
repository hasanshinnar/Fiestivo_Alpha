const tabs = document.querySelectorAll(".tab-btn");
const panels = document.querySelectorAll(".tab-panel");

tabs.forEach((btn) => {
  btn.addEventListener("click", () => {
    tabs.forEach((t) => t.classList.remove("active"));
    panels.forEach((p) => p.classList.remove("active"));
    btn.classList.add("active");
    document.getElementById("tab-" + btn.dataset.tab).classList.add("active");
  });
});

// Follow toggle
document.querySelectorAll(".friend-follow").forEach((btn) => {
  btn.addEventListener("click", () => {
    btn.classList.toggle("following");
    btn.textContent = btn.classList.contains("following")
      ? "Following"
      : "Follow";
  });
});

// Interest tag toggle
document.querySelectorAll(".tag").forEach((tag) => {
  tag.addEventListener("click", () => tag.classList.toggle("active"));
});

// Edit profile button
document.querySelector(".btn-edit-profile").addEventListener("click", () => {
  alert("Edit profile modal would open here.");
});
