const loginPanel = document.getElementById("loginPanel");
const registerPanel = document.getElementById("registerPanel");
const toRegister = document.getElementById("toRegister");
const toLogin = document.getElementById("toLogin");

toRegister.addEventListener("click", (e) => {
  e.preventDefault();
  loginPanel.classList.add("hidden");
  registerPanel.classList.add("visible");
});
toLogin.addEventListener("click", (e) => {
  e.preventDefault();
  registerPanel.classList.remove("visible");
  loginPanel.classList.remove("hidden");
});

// Password toggles
document.getElementById("toggleLogin").addEventListener("click", () => {
  const p = document.getElementById("loginPass");
  p.type = p.type === "password" ? "text" : "password";
});
document.getElementById("toggleReg").addEventListener("click", () => {
  const p = document.getElementById("regPass");
  p.type = p.type === "password" ? "text" : "password";
});
