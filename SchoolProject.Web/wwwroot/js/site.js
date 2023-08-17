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

    // <table class="table table-striped table-hover" id="sortableTable" style="width:100%">

    $('#sortableTable').DataTable({
        // pageLength: 25,
        pagingType: 'full_numbers'
    });

    $('#exampleTableMulti-column-ordering').DataTable({
        columnDefs: [
            {
                targets: [0],
                orderData: [0, 1]
            },
            {
                targets: [1],
                orderData: [1, 0]
            },
            {
                targets: [4],
                orderData: [4, 0]
            }
        ]
    });

    $('#exampleTableHiddenColumns').DataTable({
        columnDefs: [
            {
                target: 2,
                visible: false,
                searchable: false
            },
            {
                target: 3,
                visible: false
            }
        ]
    });

});


function previewImage(event) {
    const reader = new FileReader();

    reader.onload = function () {
        let preview = document.getElementById('preview');
        preview.src = reader.result;
    };

    reader.readAsDataURL(event.target.files[0]);
}


function previewImages(event) {
    let reader = new FileReader();

    reader.onload = function () {
        let previews = document.getElementsByClassName('preview');
        for (let i = 0; i < previews.length; i++) {
            previews[i].src = reader.result;
        }
    };

    reader.readAsDataURL(event.target.files[0]);
}
