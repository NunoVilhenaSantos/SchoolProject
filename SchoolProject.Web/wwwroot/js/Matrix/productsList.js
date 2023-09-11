// ---------------------------------------------------------------------------------------------------------------- --->
//
// drop-down list for products
//
// ---------------------------------------------------------------------------------------------------------------- --->


function getProducts() {
    $.ajax({
        dataType: 'json', error: function (ex) {
            alert('Failed to retrieve products. ' + ex);

        }, success: function (product) {
            $("#ProductId").append('<option value="0">(Select a product...)</option>');

            $.each(product, function (i, product) {
                $("#ProductId").append('<option value="' + product.id + '">' + product.name + '</option>');
            });
        },

        type: 'POST',

        url: '/Orders/GetProductsAsync'

    });

}


$(document).ready(function () {
    getProducts();
});