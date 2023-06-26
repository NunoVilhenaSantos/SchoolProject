// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.


$(document).ready(function () {
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
});


$(document).ready(function () {
    // $('#sortableTable').DataTable();
    $('#sortableTable').DataTable({"pageLength": 25});
});


function previewImage(event) {
    const reader = new FileReader();

    reader.onload = function () {
        let preview = document.getElementById('preview');
        preview.src = reader.result;
    }

    reader.readAsDataURL(event.target.files[0]);
}


function previewImages(event) {
    let reader = new FileReader();

    reader.onload = function () {
        let previews = document.getElementsByClassName('preview');
        for (let i = 0; i < previews.length; i++) {
            previews[i].src = reader.result;
        }
    }

    reader.readAsDataURL(event.target.files[0]);
}