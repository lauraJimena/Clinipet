// Restringir la entrada de caracteres, permitiendo solo números
document.addEventListener('DOMContentLoaded', function () {
    const input = document.getElementById('documento');
    if (input) {
        input.addEventListener('input', function () {
            this.value = this.value.replace(/\D/g, '');
        });
    }

    // Script para manejar el envío del formulario con fetch y mostrar popups
    const form = document.getElementById('loginForm');
    if (form) {
        form.addEventListener('submit', function (e) {
            e.preventDefault();

            const formData = new FormData(this);

            fetch('/General/Login', {
                method: 'POST',
                body: formData
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        if (data.changePassword) {
                            Swal.fire({
                                icon: 'info',
                                title: 'Actualizar contraseña',
                                text: data.message,
                                confirmButtonText: 'Continuar',
                                customClass: {
                                    popup: 'swal2-popup',
                                    title: 'swal2-title',
                                    content: 'swal2-content',
                                    confirmButton: 'boton'
                                }
                            }).then(() => {
                                window.location.href = '/General/CambiarContraseña';
                            });
                        } else if (data.redirectUrl) {
                            Swal.fire({
                                icon: 'success',
                                title: 'Inicio de sesión exitoso',
                                text: data.message,
                                showConfirmButton: false,
                                timer: 1500,
                                customClass: {
                                    popup: 'swal2-popup',
                                    title: 'swal2-title',
                                    content: 'swal2-content',
                                    confirmButton: 'boton'
                                }
                            }).then(() => {
                                window.location.href = data.redirectUrl;
                            });
                        }
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: data.message,
                            confirmButtonText: 'Intentar de nuevo',
                            customClass: {
                                popup: 'swal2-popup',
                                title: 'swal2-title',
                                text: 'swal2-text',
                                content: 'swal2-content',
                                confirmButton: 'boton'
                            }
                        });
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Ocurrió un error inesperado. Intenta de nuevo más tarde.',
                        confirmButtonText: 'Aceptar',
                        customClass: {
                            popup: 'swal2-popup',
                            title: 'swal2-title',
                            content: 'swal2-content',
                            icon: 'swal2-icon',
                            confirmButton: 'boton'
                        }
                    });
                });
        });
    }
});
//Mostrar Contraseña

        function verContras(idInput) {
        const input = document.getElementById(idInput);
        if (input.type === 'password') {
            input.type = 'text';
        } else {
            input.type = 'password';
        }
    }


// Mostrar mensaje de envío de correo para recuperación de contraseña
function solicitarCambioContrasena() {
    let numIdentidad = document.getElementById("documento").value;

    if (!numIdentidad) {
        Swal.fire({
            title: "Error",
            text: "Ingresa tu número de identidad antes de solicitar la recuperación.",
            icon: "error",
            customClass: {
                popup: 'swal2-popup',
                title: 'swal2-title',
                content: 'swal2-content',
                confirmButton: 'boton'
            }
        });
        return;
    }

    fetch("/General/OlvideContrasena", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ num_ident: numIdentidad })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                Swal.fire({
                    title: "Correo enviado 📩",
                    text: "A tu correo fue enviado un mensaje para restablecer tu contraseña.",
                    icon: "success",
                    customClass: {
                        popup: 'swal2-popup',
                        title: 'swal2-title',
                        content: 'swal2-content',
                        confirmButton: 'boton'
                    }
                });
            } else {
                Swal.fire({
                    title: "Error",
                    text: data.message,
                    icon: "error",
                    customClass: {
                        popup: 'swal2-popup',
                        title: 'swal2-title',
                        content: 'swal2-content',
                        confirmButton: 'boton'
                    }
                });
            }
        })
        .catch(error => {
            console.error("Error:", error);
            Swal.fire({
                title: "Error",
                text: "Hubo un problema en el envío.",
                icon: "error",
                customClass: {
                    popup: 'swal2-popup',
                    title: 'swal2-title',
                    content: 'swal2-content',
                    confirmButton: 'boton'
                }
            });
        });
}
