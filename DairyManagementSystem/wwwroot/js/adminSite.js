const themeSwitch = document.getElementById("theme-switch");
const themeIconDark = document.getElementById("theme-icon-dark");
const themeIconLight = document.getElementById("theme-icon-light");

const darkMode = localStorage.getItem('dark-mode', false);

if (darkMode) {
   document.body.classList.add("dark-mode");
   themeIconLight.style.display = "none";
   themeIconDark.style.display = "block";
}
else {
   themeIconDark.style.display = "none";
   themeIconLight.style.display = "block";
}


themeSwitch.addEventListener('click', () => {
   if (document.body.classList.contains("dark-mode")) {

      document.body.classList.remove("dark-mode");

      themeIconDark.style.display = "none";
      themeIconLight.style.display = "block";

      localStorage.removeItem("dark-mode");
   } else {

      document.body.classList.add("dark-mode");

      themeIconLight.style.display = "none";
      themeIconDark.style.display = "block";

      localStorage.setItem("dark-mode", true);
   }
});