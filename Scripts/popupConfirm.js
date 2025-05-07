document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("confirmButton").addEventListener("click", function () {
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
                confirmButton: 'botonConfir',
                cancelButton: 'botonCancel',

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
                    cancelButton: 'botonCancel',

                }
            });
        });
}