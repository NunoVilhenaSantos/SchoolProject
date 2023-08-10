// Please see documentation at
// https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.


<!-- --------------------------------------------------------------------------------------------------------------- -->
<!-- -->
<!-- popup de confirmação da eliminação de um item do carrinho de compras -->
<!-- -->

<!-- --------------------------------------------------------------------------------------------------------------- -->


function getProducts() {
    $.ajax({
        url: '/Orders/GetProductsAsync', type: 'POST', dataType: 'json',

        success: function (product) {
            $("#ProductId").append('<option value="0">(Select a product...)</option>');

            $.each(product, function (i, product) {
                $("#ProductId").append('<option value="' + product.id + '">' + product.name + '</option>');
            });
        },

        error: function (ex) {
            alert('Failed to retrieve products. ' + ex);
        }

    });

}


$(document).ready(function () {
    getProducts();
});