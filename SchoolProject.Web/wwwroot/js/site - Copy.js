// Please see documentation at
// https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.


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


// ------------------------------------------------------------------------------------------------------------------ //
//
// Preview Image and preview Images
//
// ------------------------------------------------------------------------------------------------------------------ //

function previewImage(event) {
    let reader = new FileReader();

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


<!-- --------------------------------------------------------------------------------------------------------------- -->
<!-- -->
<!-- popup de confirmação da eliminação de um item do carrinho de compras -->
<!-- -->
<!-- --------------------------------------------------------------------------------------------------------------- -->


// Event handler for confirming the order
$("#confirmOrderOk").click(function () {
    window.location.href = "/Orders/ConfirmOrder";
    debugger;
});

// Event handler for deleting an item from the shopping list
$("#deleteItemOk").click(function () {
    window.location.href = "/Orders/DeleteItem/" + id;
    debugger;
});

// Event handler for setting the ID when the delete modal is shown
$("#deleteStaticBackdrop").on("show.bs.modal", function (event) {
    let button = $(event.relatedTarget);
    id = button.closest("td").attr("id");
    debugger;
});


<!-- --------------------------------------------------------------------------------------------------------------- -->
<!--  -->
<!-- popup geral para datas -->
<!--  -->
<!-- --------------------------------------------------------------------------------------------------------------- -->

$(function () {

    let userLang = navigator.language || navigator.languages[0];

    $('.date').datepicker(
        {
            format: "dd/mm/yyyy",
            language: userLang,
            autoclose: true,
            todayHighlight: true
        }
    );

    $('#datePicker').datepicker(
        {
            format: "dd/mm/yyyy",
            language: userLang,
            autoclose: true,
            todayHighlight: true
        });

});


<!-- --------------------------------------------------------------------------------------------------------------- -->
<!-- TODO: fix this -->
<!-- popup geral para erros BD -->
<!-- está com erros ainda não funciona -->
<!-- --------------------------------------------------------------------------------------------------------------- -->

$(function () {
    // debugger;

    let error = $("#error").val();

    if (error === "True") {
        $('#errorStaticBackdrop').modal('show');
    }
});


// ------------------------------------------------------------------------------------------------------------------ //
//
// Document Ready functions
//
// ------------------------------------------------------------------------------------------------------------------ //


$(document).ready(function () {
    let id = -1;
    // debugger;
    darkModeSwitching();
});