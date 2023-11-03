$(document).ready(function () {
    $("form").submit(function (event) {
        event.preventDefault(); // Previne o envio padrão do formulário

        // Função para verificar se um campo requerido está vazio
        function isRequiredFieldEmpty(field) {
            return field === null || field === undefined || field === "";
        }

        // Serializa os campos requeridos do formulário em um objeto JSON
        var formData = {
            StudentId: @ViewData["StudentId"],
            StudentIdGuid: "@ViewData["StudentIdGuid"]",
            Enrollments: []
        };

        var requiredFields = $(".form-control[required]");

        if (requiredFields.length > 0) {
            var formIsValid = true;

            requiredFields.each(function () {
                var name = $(this).attr("name");
                var value = $(this).val();

                if (isRequiredFieldEmpty(value)) {
                    formIsValid = false;
                    // Adicione código aqui para lidar com campos vazios, se necessário
                    // Por exemplo, exiba uma mensagem de erro para o usuário
                    console.log("Campo requerido vazio: " + name);
                } else {
                    // Verifica se o campo é um campo de matrícula e o processa
                    if (name.startsWith("Enrollments")) {
                        var enrollmentProp = name.split(".")[1];
                        if (!formData.Enrollments[enrollmentProp]) {
                            formData.Enrollments[enrollmentProp] = [];
                        }
                        formData.Enrollments[enrollmentProp].push(value);
                    } else {
                        formData[name] = value;
                    }
                }
            });

            if (!formIsValid) {
                // O formulário não é válido, não envie os dados
                console.log("Formulário inválido. Corrija os campos vazios.");
                return;
            }
        }

        // Faça uma solicitação AJAX para enviar os dados para o método GradesAssignment
        $.ajax({
            type: "POST",
            url: "@Url.Action("GradesAssignment", "Enrollments")",
            data: JSON.stringify(formData), // Envie os dados como JSON
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                // Manipule a resposta do servidor, se necessário
                console.log("Dados enviados com sucesso!");
            },
            error: function () {
                // Manipule erros se a solicitação falhar
                console.log("Erro ao enviar os dados.");
            }
        });
    });
});
