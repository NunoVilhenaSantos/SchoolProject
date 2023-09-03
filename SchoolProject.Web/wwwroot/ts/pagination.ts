// pagination.ts


// Obtém o valor da variável controllerName definida no Razor
// Remove this duplicate declaration
// const controllerName = (window as any).controllerName as string;


//function setupDropdownChangeEvent(): void {

//    // Obtain the value of the variable controllerName defined in Razor
//    const controllerName = (window as any).controllerName as string;


//    function handleDropdownChange(): void {
//        const selectedPageSize = parseInt((document.getElementById('pageSizeSelect') as HTMLSelectElement).value, 10);
//        const selectedSortProperty = (document.getElementById('sortProperty') as HTMLSelectElement).value;
//        const selectedSortOrder = (document.getElementById('sortOrder') as HTMLSelectElement).value;

//        // Construct the URL
//        const url = `/${controllerName}/IndexCards1?pageNumber=1&pageSize=${selectedPageSize}&sortOrder=${selectedSortOrder}&sortProperty=${selectedSortProperty}`;

//        window.location.href = url;
//    }


//    document.addEventListener('DOMContentLoaded', () => {
//        const pageSizeSelect = document.getElementById('pageSizeSelect');
//        const sortPropertySelect = document.getElementById('sortProperty');
//        const sortOrderSelect = document.getElementById('sortOrder');

//        if (pageSizeSelect && sortPropertySelect && sortOrderSelect) {
//            pageSizeSelect.addEventListener('change', handleDropdownChange);
//            sortPropertySelect.addEventListener('change', handleDropdownChange);
//            sortOrderSelect.addEventListener('change', handleDropdownChange);
//        }
//    });

//}


// Call the setupDropdownChangeEvent function to initialize the event handling
//setupDropdownChangeEvent();

