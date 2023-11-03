// function saveEnrollment(button) {
//     // Get the enrollment GUID.
//     let enrollmentGuid = button.parentElement.parentElement.querySelector('input[name="item.EnrollmentIdGuid"]').value;
//
//     // Get the grade and absences values.
//     let grade = button.parentElement.parentElement.querySelector('input[name="item.Grade"]').value;
//     let absences = button.parentElement.parentElement.querySelector('input[name="item.Absences"]').value;
//
//     // Construct the data object to send in the request body.
//     let data = {
//         grade: grade,
//         absences: absences
//     };
//
//     // Update the enrollment in the database.
//     let xhr = new XMLHttpRequest();
//     xhr.open("POST", "/Enrollments/SaveEnrollments/" + enrollmentGuid);
//     xhr.setRequestHeader("Content-Type", "application/json");
//     xhr.send(JSON.stringify(data));
//
//     xhr.onload = function () {
//         if (xhr.status === 200) {
//             // The enrollment was updated successfully.
//         } else {
//             // An error occurred while updating the enrollment.
//         }
//     };
//
//     // Check if the method is being called.
//     console.log("O método saveEnrollment() foi chamado!");
// }


// Function to save enrollment
function saveEnrollment(button) {
    // Get the enrollment GUID.
    let enrollmentGuid = button.parentElement.parentElement.querySelector('input[name="item.EnrollmentIdGuid"]').value;

    // Get the grade and absences values.
    let grade = button.parentElement.parentElement.querySelector('input[name="item.Grade"]').value;
    let absences = button.parentElement.parentElement.querySelector('input[name="item.Absences"]').value;

    // Construct the data object to send in the request body.
    let data = {
        grade: parseFloat(grade),  // Convert to a number if necessary
        absences: parseInt(absences),  // Convert to an integer if necessary
    };

    debugger;

    // Update the enrollment in the database.
    fetch(`/Enrollments/SaveEnrollments/${enrollmentGuid}`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data),

    })

        .then(response => {
            if (response.status === 200) {
                // The enrollment was updated successfully.
                console.log("Data saved successfully.");
            } else {
                // An error occurred while updating the enrollment.
                console.error("Error updating data.");
            }
        })

        .catch(error => {
            console.error("Request error: " + error);
        });
}

// Add a click event listener to the button
const saveButton = document.getElementById("saveButton");

if (saveButton) {
    saveButton.addEventListener("click", function () {
        saveEnrollment(this);
    });
}
