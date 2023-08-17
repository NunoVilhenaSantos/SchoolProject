// ---------------------------------------------------------------------------------------------------------------- --->
//
// Initialize Datepicker -->
//
// ---------------------------------------------------------------------------------------------------------------- --->


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