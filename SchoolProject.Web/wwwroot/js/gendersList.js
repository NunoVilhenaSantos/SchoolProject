// ---------------------------------------------------------------------------------------------------------------- --->
//
// drop-down list for genders and the respective functions to populate them
//
// ---------------------------------------------------------------------------------------------------------------- --->


function getGenders() {
    // debugger;

    $.ajax({
        dataType: 'json',

        error: function (ex) {
            alert('Failed to retrieve genders. ' + ex);
            // debugger;
        },

        success: function (genders) {
            $("#GenderId").append('<option value="0">(Select a gender...)</option>');
            // debugger;

            $.each(genders, function (i, gender) {
                $("#GenderId").append('<option value="' + gender.value + '">' + gender.text + '</option>');
            });
        },

        type: 'POST',
        // url: '/Account/GetCountriesAsync'
        url: '/Genders/GetGendersListJson'
    });

    // debugger;
}

$(document).ready(function () {
    // debugger;

    // Check if the country drop-down is empty
    if ($("#GenderId option").length === 0) {
        getGenders();
    }

});