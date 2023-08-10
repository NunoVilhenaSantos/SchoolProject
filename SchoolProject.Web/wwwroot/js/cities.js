// Please see documentation at
// https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.


<!-- --------------------------------------------------------------------------------------------------------------- -->
<!-- -->
<!-- popup de confirmação da eliminação de um item do carrinho de compras -->
<!-- -->

<!-- --------------------------------------------------------------------------------------------------------------- -->


function getCountries() {
    $.ajax({
        url: '/Account/GetCountriesAsync', type: 'POST', dataType: 'json',

        success: function (country) {
            $("#CountryId").append('<option value="0">(Select a country...)</option>');

            $.each(country, function (i, country) {
                $("#CountryId").append('<option value="' + country.id + '">' + country.name + '</option>');
            });
        },

        error: function (ex) {
            alert('Failed to retrieve countries. ' + ex);
        }

    });

}


function getCities(countryId) {
    $.ajax({
        url: '/Account/GetCitiesAsync', type: 'POST', dataType: 'json', data: {countryId: countryId},

        success: function (cities) {
            $("#CityId").append('<option value="0">(Select a city...)</option>');

            $.each(cities, function (i, city) {
                $("#CityId").append('<option value="' + city.id + '">' + city.name + '</option>');
            });
        },

        error: function (ex) {
            alert('Failed to retrieve cities. ' + ex);
        }

    });

}

$(document).ready(function () {
    // Trigger the change event to populate the corresponding city dropdown list
    $("#CountryId").change(function () {
        $("#CityId").empty();

        let countryId = $(this).val();

        getCities(countryId);
    });

    // Check if the country dropdown is empty
    if ($("#CountryId option").length === 0) {
        getCountries();
    }

});