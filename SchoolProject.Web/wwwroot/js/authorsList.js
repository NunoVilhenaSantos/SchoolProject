// ---------------------------------------------------------------------------------------------------------------- --->
//
// drop-down list for cities and countries and nationality and the respective functions to populate them
//
// ---------------------------------------------------------------------------------------------------------------- --->

function getAuthors() {
    // debugger;

    $.ajax({
        dataType: 'json',

        error: function (ex) {
            alert('Failed to retrieve the authors list. ' + ex);
            // debugger;
        },

        success: function (authors) {
            // $("#AuthorId").append('<option value="0">(Select a author...)</option>');
            // debugger;

            $.each(authors, function (i, author) {
                $("#AuthorId").append('<option value="' + author.value + '">' + author.text + '</option>');
            });
        },

        type: 'POST',
        url: '/Authors/GetAuthorsListJson'
        //url: '/Account/GetCountriesWithNationalitiesAsync'
    });

    // debugger;
}

$(document).ready(function () {
    // debugger;

    // Trigger the change event to populate the corresponding city drop-down list
    $("#AuthorId").change(function () {
        // $("#CityId").empty();
        // $("#NationalityId").empty();

        let authorId = $(this).val();

        // getCities(subscriptionId);
        // getNationalities(countryId);
    });

    // Check if the country drop-down is empty
    if ($("#AuthorId option").length === 0) {
        getAuthors();
    }

});