// ---------------------------------------------------------------------------------------------------------------- --->
//
// drop-down list for cities and countries and nationality and the respective functions to populate them
//
// ---------------------------------------------------------------------------------------------------------------- --->


function getBookEditions(bookId) {
    debugger;

    $.ajax({
        data: {bookId: bookId}, dataType: 'json',

        error: function (ex) {
            alert('Failed to retrieve book. ' + ex);
            debugger;
        },

        success: function (bookEditions) {
            $("#BookEditionId").append('<option value="0">(Select a Book Edition...)</option>');
            debugger;

            // $.each(cities, function (i, city) {
            //     $("#CityId").append('<option value="' + city.id + '">' + city.name + '</option>');
            // });

            $.each(bookEditions, function (i, bookEdition) {
                $("#BookEditionId").append('<option value="' + bookEdition.value + '">' + bookEdition.text + '</option>');
            });
        },

        type: 'POST',
        url: '/Reservations/GetBookEditionsList'
    });

}

function getBooks() {
    //debugger;

    $.ajax({
        dataType: 'json',

        error: function (ex) {
            alert('Failed to retrieve Books. ' + ex);
            // debugger;
        },

        success: function (books) {
            $("#BookId").append('<option value="0">(Select a Book...)</option>');
            // debugger;

            $.each(books, function (i, book) {
                $("#BookId").append('<option value="' + book.value + '">' + book.text + '</option>');
            });
        },

        type: 'POST',
        // url: '/Account/GetCountriesAsync'
        url: '/Reservations/GetBooksList'
    });


    // debugger;
}

$(document).ready(function () {
    debugger;

    // Trigger the change event to populate the corresponding city drop-down list
    $("#BookId").change(function () {
        $("#BookEditionId").empty();
        //$("#NationalityId").empty();

        let bookId = $(this).val();

        getBookEditions(bookId);

        debugger;
        /*getNationalities(countryId);*/
    });

    // Check if the country drop-down is empty
    if ($("#BookId option").length === 0) {
        getBooks();
    }

});