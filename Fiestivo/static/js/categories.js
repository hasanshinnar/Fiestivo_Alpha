const tabs = document.querySelectorAll(".cat-tab");
const cards = document.querySelectorAll(".event-card");
const activeTitle = document.getElementById("activeTitle");
const countLabel = document.getElementById("countLabel");
const emptyState = document.getElementById("emptyState");
const grid = document.getElementById("eventsGrid");

const catNames = {
  all: "All Events",
  football: "Football",
  mafia: "Mafia & Cards",
  food: "Hash w Nash",
  chalet: "Chalet Day",
  hiking: "Hiking",
  gaming: "Gaming Night",
  padel: "Padel",
  trip: "Road Trip",
};

tabs.forEach((tab) => {
  tab.addEventListener("click", () => {
    tabs.forEach((t) => t.classList.remove("active"));
    tab.classList.add("active");
    const cat = tab.dataset.cat;
    let visible = 0;
    cards.forEach((card) => {
      const show = cat === "all" || card.dataset.cat === cat;
      card.classList.toggle("hidden", !show);
      if (show) visible++;
    });
    activeTitle.textContent = catNames[cat] || "Events";
    countLabel.textContent = `Showing ${visible} event${visible !== 1 ? "s" : ""}`;
    emptyState.style.display = visible === 0 ? "block" : "none";
    grid.style.display = visible === 0 ? "none" : "grid";
  });
});
