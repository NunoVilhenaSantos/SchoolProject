// ---------------------------------------------------------------------------------------------------------------- --->
//
// drop-down list for cities and countries and nationality and the respective functions to populate them
//
// ---------------------------------------------------------------------------------------------------------------- --->

var countryDataCache = {};


function getNationalities(countryId) {
    $.ajax({
        data: {countryId: countryId},
        dataType: 'json',

        error: function (ex) {
            alert('Failed to retrieve nationalities. ' + ex);
        },

        success: function (nationalities) {
            $("#NationalityId").empty();
            // $("#NationalityId").append('<option value="0">(Select a nationality...)</option>');

            $.each(nationalities, function (i, nationality) {
                $("#NationalityId").append('<option value="' + nationality.value + '">' + nationality.text + '</option>');
            });
        },

        type: 'POST',

        // Coloque o URL correto para a função que busca as nacionalidades
        url: '/Account/GetNationalitiesAsync'
    });
}


function getCities(countryId) {

    if (countryDataCache.cities && countryDataCache.cities[countryId]) {
        populateCitiesDropdown(countryDataCache.cities[countryId]);
    } else {

        $.ajax({
            data: {countryId: countryId},
            dataType: 'json',
            error: function (ex) {
                alert('Failed to retrieve cities. ' + ex);
            },
            success: function (cities) {
                countryDataCache.cities = countryDataCache.cities || {};
                countryDataCache.cities[countryId] = cities;
                populateCitiesDropdown(cities);
            },
            type: 'POST',
            url: '/Account/GetCitiesAsync'
        });

    }

}

function populateCitiesDropdown(cities) {
    $("#CityId").empty();
    $("#CityId").append('<option value="0">(Select a city...)</option>');
    $.each(cities, function (i, city) {
        $("#CityId").append('<option value="' + city.id + '">' + city.name + '</option>');
    });

}

$(document).ready(function () {

    // Trigger the change event to populate the corresponding city drop-down list
    $("#CountryId").change(function () {
        $("#CityId").empty();
        $("#NationalityId").empty();

        let countryId = $(this).val();

        getCities(countryId);
        getNationalities(countryId);
    });

    // Check if the country drop-down is empty
    if ($("#CountryId option").length === 0) {
        getCountries();
    }

});
