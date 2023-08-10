// Please see documentation at
// https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.


// Initialize Datepicker
function initializeDatepicker() {
    const userLang = navigator.language || navigator.languages[0];

    $('.date').datepicker({
        format: "dd/mm/yyyy", language: userLang, autoclose: true, todayHighlight: true
    });

    $('#datePicker').datepicker({
        format: "dd/mm/yyyy", language: userLang, autoclose: true, todayHighlight: true
    });
}


$(document).ready(function () {
    initializeDatepicker();
});