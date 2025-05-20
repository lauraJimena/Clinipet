

document.addEventListener("DOMContentLoaded", function () {

    const confirmButton = document.getElementById("confirmButton");

    confirmButton.addEventListener("click", function (event) {
        event.preventDefault(); // Evita envío automático

        const checkboxes = document.querySelectorAll('input[name="id_mascotas"]:checked');

        if (checkboxes.length === 0) {
            Swal.fire({
                icon: 'warning',
                title: 'Debe seleccionar una mascota',
                text: 'Por favor seleccione al menos una mascota para continuar.',
                customClass: {
                    popup: 'swal2-popup',
                    title: 'swal2-title',
                    content: 'swal2-content',
                    confirmButton: 'boton botonConfir',
                    cancelButton: 'boton botonCancel'
                }
            });
            return;
        }
        Swal.fire({
            title: "¿Estás seguro?",
            text: "Confirma que deseas agendar la cita.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Sí, confirmar",
            cancelButtonText: "Cancelar",
            customClass: {
                popup: 'swal2-popup',
                title: 'swal2-title',
                content: 'swal2-content',
                confirmButton: 'boton botonConfir',
                cancelButton: 'boton botonCancel'
            }
        }).then((result) => {
            if (result.isConfirmed) {
                enviarFormulario();
            }
        });
    });
});

function enviarFormulario() {
    const form = document.getElementById("confirmForm");
    const formData = new FormData(form);

    fetch(form.action, {
        method: "POST",
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            if (data.success && data.redirectUrl) {
                Swal.fire({
                    icon: "success",
                    title: "Cita confirmada",
                    text: data.message || "Tu cita ha sido agendada exitosamente.",
                    showConfirmButton: false,
                    timer: 3000,
                    customClass: {
                        popup: 'swal2-popup',
                        title: 'swal2-title',
                        content: 'swal2-content',
                        confirmButton: 'botonConfir'
                    }
                }).then(() => {
                    window.location.href = data.redirectUrl;
                });
            } else {
                Swal.fire({
                    icon: "error",
                    title: "Error",
                    text: data.message || "Ocurrió un error al confirmar la cita.",
                    confirmButtonText: "OK",
                    customClass: {
                        popup: 'swal2-popup',
                        title: 'swal2-title',
                        content: 'swal2-content',
                        confirmButton: 'botonConfir'
                    }
                });
            }
        })
        .catch(error => {
            console.error("Error:", error);
            Swal.fire({
                icon: "error",
                title: "Error",
                text: "Error inesperado. Intenta de nuevo más tarde.",
                confirmButtonText: "OK",
                 customClass: {
                    popup: 'swal2-popup',
                    title: 'swal2-title',
                    content: 'swal2-content',
                    confirmButton: 'botonConfir',
                    cancelButton: 'botonCancel'

                }
            });
        });
}