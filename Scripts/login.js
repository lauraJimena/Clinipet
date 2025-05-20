//Manejar el envío del formulario con fetch y mostrar popups
document.addEventListener('DOMContentLoaded', function () {
    // Script para validar solo números en el campo "documento"
    const input = document.getElementById('documento');
    if (input) {
        input.addEventListener('input', function () {
            this.value = this.value.replace(/\D/g, ''); // Solo números
        });
    }

    // Script para la validación y envío del formulario de login
    document.getElementById('loginForm').addEventListener('submit', function (e) {
        e.preventDefault(); // Evita envío tradicional

        const formData = new FormData(this);
        console.log(formData); // Verificar los datos que se están enviando

        fetch('data - url', {
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
                            window.location.href = '@Url.Action("CambiarContraseña", "General")';
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
                    // Mostrar error con SweetAlert
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

    // Función para solicitar el cambio de contraseña
    window.solicitarCambioContrasena = function () {
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

        fetch("@Url.Action('OlvideContrasena', 'General')", {
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
    };
});
