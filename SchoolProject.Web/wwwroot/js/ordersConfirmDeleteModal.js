// ---------------------------------------------------------------------------------------------------------------- --->
//
//  popup de confirmação da eliminação de um item do carrinho de compras -->
//
// ---------------------------------------------------------------------------------------------------------------- --->


// Event handler for confirming the order
$("#confirmOrderOk").click(function () {
    window.location.href = "/Orders/ConfirmOrder";
    debugger;
});


// Event handler for setting the ID when the delete modal is shown
$("#deleteOrderItem").on("show.bs.modal", function (event) {

    let button = $(event.relatedTarget);
    let id = button.closest("td").attr("id");
    debugger;

    // Event handler for deleting an item from the shopping list
    $("#deleteItemOk").click(function () {
        window.location.href = "/Orders/DeleteItem/" + id;
        debugger;
    });

});