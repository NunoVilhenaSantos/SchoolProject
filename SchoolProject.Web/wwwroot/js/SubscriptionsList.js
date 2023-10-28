// ---------------------------------------------------------------------------------------------------------------- --->
//
// drop-down list for cities and countries and nationality and the respective functions to populate them
//
// ---------------------------------------------------------------------------------------------------------------- --->

function getSubscriptions() {
    // debugger;

    $.ajax({
        dataType: 'json',

        error: function (ex) {
            alert('Failed to retrieve subscriptions. ' + ex);
            // debugger;
        },

        success: function (subscriptions) {
            // $("#SubscriptionId").append('<option value="0">(Select a subscription...)</option>');
            // debugger;

            $.each(subscriptions, function (i, subscription) {
                $("#SubscriptionId").append('<option value="' + subscription.value + '">' + subscription.text + '</option>');
            });
        },

        type: 'POST',
        url: '/Subscriptions/GetSubscriptionsJson'
        //url: '/Account/GetCountriesWithNationalitiesAsync'
    });

    // debugger;
}

$(document).ready(function () {
    // debugger;

    // Trigger the change event to populate the corresponding city drop-down list
    $("#SubscriptionId").change(function () {
        // $("#CityId").empty();
        // $("#NationalityId").empty();

        let subscriptionId = $(this).val();

        // getCities(subscriptionId);
        // getNationalities(subscriptionId);
    });

    // Check if the country drop-down is empty
    if ($("#SubscriptionId option").length === 0) {
        getSubscriptions();
    }

});