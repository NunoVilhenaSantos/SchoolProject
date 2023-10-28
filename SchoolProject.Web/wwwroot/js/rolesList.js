// ---------------------------------------------------------------------------------------------------------------- --->
//
// drop-down list for genders and the respective functions to populate them
//
// ---------------------------------------------------------------------------------------------------------------- --->

function getRoles() {
    // debugger;

    $.ajax({
        dataType: 'json',

        error: function (ex) {
            alert('Failed to retrieve roles. ' + ex);
            // debugger;
        },

        success: function (roles) {
            $("#RoleId").append('<option value="0">(Select a role...)</option>');
            // debugger;

            $.each(roles, function (i, role) {
                $("#RoleId").append('<option value="' + role.value + '">' + role.text + '</option>');
            });
        },

        type: 'POST',
        // url: '/Account/GetCountriesAsync'
        url: '/Roles/GetRolesListJson'
    });

    // debugger;
}

$(document).ready(function () {
    // debugger;

    // Check if the country drop-down is empty
    if ($("#RoleId option").length === 0) {
        getRoles();
    }

});