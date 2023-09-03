// pagination.js

$(document).ready(() => {
    const controllerName = document.body.getAttribute('data-controller-name');

    $('#pageSizeSelect, #sortProperty, #sortOrder').change(() => {
        const selectedPageSize = parseInt($('#pageSizeSelect').val()); // Parse as an integer
        const selectedSortProperty = $('#sortProperty').val();
        const selectedSortOrder = $('#sortOrder').val();


        // Redirect to the first page with the selected parameters
        // Construct the URL
        const url = `/${controllerName}/IndexCards1?pageNumber=1&pageSize=${selectedPageSize}&sortOrder=${selectedSortOrder}&sortProperty=${selectedSortProperty}`;

        window.location.href = url;

    });

});
