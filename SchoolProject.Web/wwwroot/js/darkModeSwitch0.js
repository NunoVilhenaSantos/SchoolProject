// ------------------------------------------------------------------------------------------------------------------ //
//
// Dark Mode
// https://getbootstrap.com/docs/5.0/customize/color/#dark-mode
//
// ------------------------------------------------------------------------------------------------------------------ //

function darkModeSwitching() {

    let switchInput = document.getElementById("darkModeSwitch");
    let htmlElement = document.documentElement;

    // Set initial state based on data-bs-theme attribute
    switchInput.checked = htmlElement.getAttribute("data-bs-theme") === "dark";

    // Toggle theme when switch is clicked
    switchInput.addEventListener("change", function () {
        if (this.checked) {
            htmlElement.setAttribute("data-bs-theme", "dark");
        } else {
            htmlElement.setAttribute("data-bs-theme", "light");
        }
    });

}

darkModeSwitching();