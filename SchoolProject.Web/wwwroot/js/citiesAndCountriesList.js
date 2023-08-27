// ---------------------------------------------------------------------------------------------------------------- --->
//
// drop-down list for cities and countries and nationality and the respective functions to populate them
//
// ---------------------------------------------------------------------------------------------------------------- --->


function getCities(countryId) {
    // debugger;

    $.ajax({
        data: {countryId: countryId}, dataType: 'json',

        error: function (ex) {
            alert('Failed to retrieve cities. ' + ex);
            debugger;
        },

        success: function (cities) {
            $("#CityId").append('<option value="0">(Select a city...)</option>');
            // debugger;

            // $.each(cities, function (i, city) {
            //     $("#CityId").append('<option value="' + city.id + '">' + city.name + '</option>');
            // });

            $.each(cities, function (i, city) {
                $("#CityId").append('<option value="' + city.value + '">' + city.text + '</option>');
            });
        },

        type: 'POST',
        url: '/Account/GetCitiesAsync'
    });

}

function getNationalities(countryId) {
    // debugger;

    $.ajax({
        data: {countryId: countryId}, dataType: 'json',

        error: function (ex) {
            alert('Failed to retrieve nationalities. ' + ex);
            debugger;
        },

        success: function (nationalities) {
            $("#NationalityId").empty();
            // $("#NationalityId").append('<option value="0">(Select a nationality...)</option>');

            $.each(nationalities, function (i, nationality) {
                $("#NationalityId").append('<option value="' + nationality.value + '">' + nationality.text + '</option>');
            });
        },

        // Coloque o URL correto para a função que busca as nacionalidades
        type: 'POST',
        url: '/Account/GetNationalitiesAsync'
    });
}

function getCountries() {
    // debugger;

    $.ajax({
        dataType: 'json',

        error: function (ex) {
            alert('Failed to retrieve countries. ' + ex);
            debugger;
        },

        success: function (countries) {
            $("#CountryId").append('<option value="0">(Select a country...)</option>');
            // debugger;

            $.each(countries, function (i, country) {
                $("#CountryId").append('<option value="' + country.value + '">' + country.text + '</option>');
            });
        },

        type: 'POST',
        // url: '/Account/GetCountriesAsync'
        url: '/Account/GetCountriesWithNationalitiesAsync'
    });

    // debugger;
}

$(document).ready(function () {
    // debugger;

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