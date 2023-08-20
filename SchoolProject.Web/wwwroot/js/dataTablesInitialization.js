// ------------------------------------------------------------------------------------------------------------------ //
//
// Data tables initialization.
//
// ------------------------------------------------------------------------------------------------------------------ //


//
// <table class="table">
//
// to
//
// <table class="table table-hover" sortable="True" id="sortableTable">
//


$(document).ready(function () {

    $('#sortableTable').DataTable({
        // pageLength: 25,
        pagingType: 'full_numbers'
    });

    $('#exampleTableMulti-column-ordering').DataTable({
        columnDefs: [
            {
                targets: [0],
                orderData: [0, 1]
            },
            {
                targets: [1],
                orderData: [1, 0]
            },
            {
                targets: [4],
                orderData: [4, 0]
            }
        ]
    });

    $('#exampleTableHiddenColumns').DataTable({
        columnDefs: [
            {
                target: 2,
                visible: false,
                searchable: false
            },
            {
                target: 3,
                visible: false
            }
        ]
    });

});

