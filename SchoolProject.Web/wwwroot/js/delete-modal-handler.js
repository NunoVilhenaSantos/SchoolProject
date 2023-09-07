// delete-delete-modal-handler.js

function handleDeleteButtonModal(event) {
    // debugger;

    let button = $(event.relatedTarget);
    console.log('button:', button);
    console.log('button data:', button.data);

    const deleteAction = button.data('del-action');
    const deleteClass = button.data('del-class');
    const deleteController = button.data('del-controller');
    const deleteItemId = button.data('del-item-id');
    const deleteItemName = button.data('del-item-name');

    console.log('deleteAction:', deleteAction);
    console.log('deleteClass:', deleteClass);
    console.log('deleteController:', deleteController);
    console.log('deleteItemId:', deleteItemId);
    console.log('deleteItemName:', deleteItemName);

    // Construa o URL de exclusão com base no controlador, na ação e no item.Id
    const deleteUrl = '/' + deleteController + '/' + deleteAction + '/' + deleteItemId;
    console.log('deleteUrl:', deleteUrl);

    $('#deleteClassNamePlaceholder').text('Delete ' + deleteClass);
    $('#deleteItemNamePlaceholder').text('Do you want to delete the ' + deleteItemName);


    // Event handler for deleting the country
    $("#deleteButtonOk").click(function () {
        window.location.href = deleteUrl;
        debugger;
    });

}


// Event handler for setting the ID and deleting an object when the delete modal is shown
$("#deleteButton").on("show.bs.modal", handleDeleteButtonModal);
