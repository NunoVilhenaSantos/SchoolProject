// ---------------------------------------------------------------------------------------------------------------- --->
//
// dropdown list for cities and countries
//
// ---------------------------------------------------------------------------------------------------------------- --->

function getCountries() {
    $.ajax({

        dataType: 'json', error: function (ex) {
            alert('Failed to retrieve countries. ' + ex);

        }, success: function (country) {
            $("#CountryId").append('<option value="0">(Select a country...)</option>');

            $.each(country, function (i, country) {
                $("#CountryId").append('<option value="' + country.id + '">' + country.name + '</option>');
            });
        },

        type: 'POST',

        url: '/Account/GetCountriesAsync'

    });

}

function getCities(countryId) {
    $.ajax({

        data: {countryId: countryId}, dataType: 'json', error: function (ex) {
            alert('Failed to retrieve cities. ' + ex);

        }, success: function (cities) {
            $("#CityId").append('<option value="0">(Select a city...)</option>');

            $.each(cities, function (i, city) {
                $("#CityId").append('<option value="' + city.id + '">' + city.name + '</option>');
            });
        },

        type: 'POST',

        url: '/Account/GetCitiesAsync'

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