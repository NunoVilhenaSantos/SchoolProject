// ---------------------------------------------------------------------------------------------------------------- --->
//
// drop-down list for cities and countries and nationality and the respective functions to populate them
//
// ---------------------------------------------------------------------------------------------------------------- --->


// Código para obter e popular as dropdowns
$(document).ready(function () {
    const countryDataCache = {
        cities: {},
        nationalities: {},
        countries: {}
    }, $cityDropdown = $("#CityId"), $nationalityDropdown = $("#NationalityId");


    const cityDropdownExists = $cityDropdown.length > 0;
    const nationalityDropdownExists = $nationalityDropdown.length > 0;


    function handleAjaxError(ex) {
        console.error('An error occurred:', ex);
        // Pode adicionar lógica para mostrar mensagens de erro ao utilizador de forma amigável
    }


    function populateDropdown($dropdown, items, defaultText) {
        $dropdown.empty();
        $dropdown.append('<option value="0">' + defaultText + '</option>');
        items.forEach(item => {
            $dropdown.append('<option value="' + item.value + '">' + item.text + '</option>');
        });

    }


    function populateCitiesDropdown(cities) {
        if (cityDropdownExists) {
            populateDropdown($cityDropdown, cities, 'Select a city...');
        }
    }


    // Função para preencher a lista suspensa de países
    function populateCountriesDropdown(countries) {
        $("#CountryId").empty();
        $("#CountryId").append('<option value="0">(Select a country...)</option>');
        countries.forEach(function (country) {
            $("#CountryId").append('<option value="' + country.id + '">' + country.name + '</option>');
        });
    }


    function loadCitiesAndNationalities(countryId) {
        loadCitiesFromServer(countryId);
        loadNationalitiesFromServer(countryId);
    }


    function loadCitiesFromServer(countryId) {
        if (countryDataCache.cities[countryId]) {
            populateCitiesDropdown(countryDataCache.cities[countryId]);
        } else {
            $.ajax({
                data: {countryId: countryId},
                dataType: 'json',
                error: handleAjaxError,
                success: function (cities) {
                    countryDataCache.cities[countryId] = cities;
                    populateCitiesDropdown(cities);
                },
                type: 'POST',
                url: '/Account/GetCitiesAsync'
            });
        }
    }

    function loadNationalitiesFromServer(countryId) {
        if (nationalityDropdownExists) {
            if (countryDataCache.nationalities[countryId]) {
                populateDropdown($nationalityDropdown, countryDataCache.nationalities[countryId], 'Select a nationality...');
            } else {
                $.ajax({
                    data: {countryId: countryId},
                    dataType: 'json',
                    error: handleAjaxError,
                    success: function (nationalities) {
                        countryDataCache.nationalities[countryId] = nationalities;
                        populateDropdown($nationalityDropdown, nationalities, 'Select a nationality...');
                    },
                    type: 'POST',
                    url: '/Account/GetNationalitiesAsync'
                });
            }
        } else {
            // ...
        }
    }


    $("#CountryId").change(function () {
        if (cityDropdownExists) {
            $cityDropdown.empty();
        }

        if (nationalityDropdownExists) {
            $nationalityDropdown.empty();
        }

        let countryId = $(this).val();

        loadCitiesAndNationalities(countryId);
    });

    if ($("#CountryId option").length === 0) {
        getCountriesFromServer();
    }

    // Função para obter os países
    function getCountriesFromServer() {
        $.ajax({
            dataType: 'json',

            error: function (ex) {
                alert('Failed to retrieve countries. ' + ex);
            },

            success: function (countries) {
                populateCountriesDropdown(countries);
            },

            type: 'POST',
            url: '/Account/GetCountriesWithNationalitesAsync'
        });

    }


});
