// Event handler for setting the ID and deleting a country when the delete modal is shown
// Please see documentation at
// https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.


<!-- --------------------------------------------------------------------------------------------------------------- -->
<!-- -->
<!-- popups para as paginas dos paises -->
<!-- -->

<!-- --------------------------------------------------------------------------------------------------------------- -->


function handleDeleteCountryModal(event) {
    let button = $(event.relatedTarget);
    let id = button.closest("td").attr("id");

    // Event handler for deleting the country
    $("#deleteCountryOk").click(function () {
        window.location.href = '/Countries/Delete/' + id;
        debugger;
    });
}

function handleDeleteCityModal(event) {
    let button = $(event.relatedTarget);
    let id = button.closest("td").attr("id");

    // Event handler for deleting the city
    $("#deleteCityOk").click(function () {
        window.location.href = '/Countries/DeleteCity/' + id;
        debugger;
    });
}

// Event handler for setting the ID and deleting a country when the delete modal is shown
$("#deleteCountry").on("show.bs.modal", handleDeleteCountryModal);

// Event handler for setting the ID and deleting a city when the delete modal is shown
$("#deleteCity").on("show.bs.modal", handleDeleteCityModal);