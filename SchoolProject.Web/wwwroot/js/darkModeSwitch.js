// ------------------------------------------------------------------------------------------------------------------ //
//
// Dark Mode
// https://getbootstrap.com/docs/5.0/customize/color/#dark-mode
//
// ------------------------------------------------------------------------------------------------------------------ //

function darkModeSwitching() {

    let switchInput1 = document.getElementById("darkModeSwitch1");
    let switchInput2 = document.getElementById("darkModeSwitch2");
    let htmlElement = document.documentElement;

    // Synchronize initial states
    switchInput1.checked = switchInput2.checked = htmlElement.getAttribute("data-bs-theme") === "dark";

    // Toggle theme when either switch is clicked
    switchInput1.addEventListener("change", function () {
        htmlElement.setAttribute("data-bs-theme", this.checked ? "dark" : "light");
        switchInput2.checked = this.checked;
    });

    switchInput2.addEventListener("change", function () {
        htmlElement.setAttribute("data-bs-theme", this.checked ? "dark" : "light");
        switchInput1.checked = this.checked;
    });

}

darkModeSwitching();

// ------------------------------------------------------------------------------------------------------------------ //