
    document.addEventListener("DOMContentLoaded", function () {
        // Solo números para num_ident
        const inputIdent = document.getElementById('num_ident');
    if (inputIdent) {
        inputIdent.addEventListener('input', function () {
            this.value = this.value.replace(/\D/g, '');
        });
        }

    // Solo números para tel_usu
    const inputTelefono = document.getElementById('tel_usu');
    if (inputTelefono) {
        inputTelefono.addEventListener('input', function () {
            this.value = this.value.replace(/\D/g, '');
        });
        }

    // Manejo del formulario
    const form = document.getElementById('registroForm');
    if (form) {
        form.addEventListener('submit', function (e) {
            e.preventDefault();

            const correoInput = document.getElementById('correo_usu');
            const contrasenaInput = document.getElementById('contras_usu');
            const telefonoInput = document.getElementById('tel_usu');

            // Validación correo
            const correo = correoInput.value.trim();
            const patronCorreo = /^[a-zA-Z0-9._%+-]{3,}@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
            if (!patronCorreo.test(correo)) {
                Swal.fire({
                    icon: 'error',
                    title: 'Correo inválido',
                    text: 'Por favor ingresa un correo válido (ejemplo: nombre@dominio.com)',
                    customClass: {
                        popup: 'swal2-popup',
                        title: 'swal2-title',
                        content: 'swal2-content',
                        confirmButton: 'botonConfir'
                    }
                });
                correoInput.focus();
                return;
            }

            // Validación contraseña
            const contrasena = contrasenaInput.value.trim();
            const patronContrasena = /^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{7,}$/;
            if (!patronContrasena.test(contrasena)) {
                Swal.fire({
                    icon: 'error',
                    title: 'Contraseña inválida',
                    html: 'Debe tener al menos 7 caracteres, una letra mayúscula, un número y un carácter especial.',
                    customClass: {
                        popup: 'swal2-popup',
                        title: 'swal2-title',
                        content: 'swal2-content',
                        confirmButton: 'botonConfir'
                    }
                });
                contrasenaInput.focus();
                return;
            }

            // Validación teléfono
            const telefono = telefonoInput.value.trim();
            const patronTelefono = /^3\d{9}$/;
            if (!patronTelefono.test(telefono)) {
                Swal.fire({
                    icon: 'error',
                    title: 'Teléfono inválido',
                    text: 'El teléfono que ingresaste no es válido. Debe comenzar con 3 y tener 10 dígitos.',
                    customClass: {
                        popup: 'swal2-popup',
                        title: 'swal2-title',
                        content: 'swal2-content',
                        confirmButton: 'boton botonConfir'
                    }
                });
                telefonoInput.focus();
                return;
            }
            //Validación de número de documento 
            const tipoDoc = document.getElementById('id_tipo_ident').value;
            const identInput = document.getElementById('num_ident');
            const numeroDocumento = identInput.value.trim();

            let regex;
            let mensajeError;

            if (tipoDoc === "1") {
                // Cédula: entre 7 y 10 dígitos numéricos
                regex = /^\d{7,10}$/;
                mensajeError = 'La cédula debe tener entre 7 y 10 dígitos numéricos.';
            } else if (tipoDoc === "2") {
                // Pasaporte: alfanumérico entre 7 y 15 caracteres
                regex = /^[a-zA-Z0-9]{7,15}$/;
                mensajeError = 'La cédula de extrajeria debe tener entre 7 y 15 caracteres alfanuméricos.';
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Tipo de documento no seleccionado',
                    text: 'Por favor, seleccione el tipo de documento.',
                    customClass: {
                        popup: 'swal2-popup',
                        title: 'swal2-title',
                        content: 'swal2-content',
                        confirmButton: 'botonConfir',
                        cancelButton: 'botonCancel',
                    }
                });
                return;
            }

            if (!regex.test(numeroDocumento)) {
                Swal.fire({
                    icon: 'error',
                    title: 'Número de documento inválido',
                    text: mensajeError,
                    customClass: {
                        popup: 'swal2-popup',
                        title: 'swal2-title',
                        content: 'swal2-content',
                        confirmButton: 'botonConfir',
                        cancelButton: 'botonCancel',
                    }
                });
                identInput.focus();
                return;
            }


            // Si pasa todas las validaciones, envía con fetch
            const formData = new FormData(form);

            fetch(form.getAttribute('data-url'), {
                method: 'POST',
                body: formData
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Registro exitoso',
                            text: data.message,
                            showConfirmButton: false,
                            timer: 1800,
                            customClass: {
                                popup: 'swal2-popup',
                                title: 'swal2-title',
                                content: 'swal2-content'
                            }
                        }).then(() => {
                            const redirectUrl = form.getAttribute('data-redirect');
                            window.location.href = redirectUrl;
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: data.message,
                            confirmButtonText: 'Intentar de nuevo',
                            customClass: {
                                popup: 'swal2-popup',
                                title: 'swal2-title',
                                content: 'swal2-content',
                                confirmButton: 'boton botonConfir'
                            }
                        });
                    }
                })
                .catch(error => {
                    console.error("Error:", error);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Ocurrió un error inesperado. Intenta de nuevo más tarde.',
                        confirmButtonText: 'Aceptar',
                         customClass: {
                                popup: 'swal2-popup',
                                title: 'swal2-title',
                                content: 'swal2-content',
                                confirmButton: 'botonConfir'
                            }
                    });
                });
        });
        }
    });
