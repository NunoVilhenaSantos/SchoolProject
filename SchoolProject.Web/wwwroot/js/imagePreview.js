// ------------------------------------------------------------------------------------------------------------------ //
//
// image preview
//
// ------------------------------------------------------------------------------------------------------------------ //


function previewImage(event) {
    const reader = new FileReader();

    reader.onload = function () {
        let preview = document.getElementById('preview');
        preview.src = reader.result;
    };

    reader.readAsDataURL(event.target.files[0]);
}


function previewImages(event) {
    let reader = new FileReader();

    reader.onload = function () {
        let previews = document.getElementsByClassName('preview');
        for (let i = 0; i < previews.length; i++) {
            previews[i].src = reader.result;
        }
    };

    reader.readAsDataURL(event.target.files[0]);
}
