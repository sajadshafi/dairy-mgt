const themeSwitch = document.getElementById("theme-switch");
const themeIconDark = document.getElementById("theme-icon-dark");
const themeIconLight = document.getElementById("theme-icon-light");
const navbar = document.getElementById("navbar");
const sidebar = document.getElementById("sidebar");

const darkMode = localStorage.getItem('dark-mode', false);

if (darkMode) {
   document.body.classList.add("dark-mode");
   navbar.classList.add("navbar-dark");
   sidebar.classList.add("sidebar-dark-primary");
   themeIconLight.style.display = "none";
   themeIconDark.style.display = "block";
}
else {
   sidebar.classList.add("sidebar-light-primary");
   navbar.classList.add("navbar-light");
   themeIconDark.style.display = "none";
   themeIconLight.style.display = "block";
}


themeSwitch.addEventListener('click', () => {
   if (document.body.classList.contains("dark-mode")) {

      document.body.classList.remove("dark-mode");

      //sidebar
      sidebar.classList.remove("sidebar-dark-primary");
      sidebar.classList.add("sidebar-light-primary");

      //navbar
      navbar.classList.remove("navbar-dark");
      navbar.classList.add("navbar-light");

      themeIconDark.style.display = "none";
      themeIconLight.style.display = "block";

      localStorage.removeItem("dark-mode");
   } else {

      document.body.classList.add("dark-mode");

      //navbar
      navbar.classList.remove("navbar-light");
      navbar.classList.add("navbar-dark");

      //sidebar
      sidebar.classList.remove("sidebar-light-primary");
      sidebar.classList.add("sidebar-dark-primary");

      themeIconLight.style.display = "none";
      themeIconDark.style.display = "block";

      localStorage.setItem("dark-mode", true);
   }
});