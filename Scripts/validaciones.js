
document.addEventListener("DOMContentLoaded", function () {
    // Solo números para num_ident
    const inputIdent = document.getElementById('num_ident');
    if (inputIdent) {
        inputIdent.addEventListener('input', function () {
            this.value = this.value.replace(/\D/g, ''); // elimina todo lo que no sea número
        });
    }

    // Solo números para tel_usu
    const inputTelefono = document.getElementById('tel_usu');
    if (inputTelefono) {
        inputTelefono.addEventListener('input', function () {
            this.value = this.value.replace(/\D/g, '');
        });
    }


    const form = document.getElementById('guardarForm');
    if (form) {
        form.addEventListener('submit', function (e) {
            e.preventDefault(); // Evita el envío tradicional

            // Validación de correo
            const correoInput = document.getElementById('correo_usu');
            const correo = correoInput.value;           
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
                        confirmButton: 'botonConfir',
                        cancelButton: 'botonCancel',

                    }
                });
                correoInput.focus();
                return; // No continúa con el envío
            }

            // Validación de contraseña
            const contrasenaInput = document.getElementById('contras_usu');
            if (contrasenaInput) {
                const contrasena = contrasenaInput.value;
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
                            confirmButton: 'botonConfir',
                            cancelButton: 'botonCancel',

                        }
                    });
                    contrasenaInput.focus();
                    return;
                }
            }

            //Validación número telefonico 
            const telefonoInput = document.getElementById('tel_usu');
            const telefono = telefonoInput.value.trim();

            const patronTelefono = /^3\d{9}$/;
            if (!patronTelefono.test(telefono)) {
                Swal.fire({
                    icon: 'error',
                    title: 'Teléfono inválido',
                    text: 'El teléfono que ingresaste no es válido.',
                    customClass: {
                        popup: 'swal2-popup',
                        title: 'swal2-title',
                        content: 'swal2-content',
                        confirmButton: 'botonConfir',
                        cancelButton: 'botonCancel',
                    }
                });
                telefonoInput.focus();
                return; // Detiene el envío
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


            // Si pasa todas las validaciones, envía el formulario con fetch
            const formData = new FormData(this);

            fetch(form.getAttribute('data-url'), {
                method: 'POST',
                body: formData
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success && data.redirectUrl) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Guardado exitoso',
                            text: data.message || 'La información fue guardada correctamente.',
                            showConfirmButton: false,
                            timer: 3000
                        }).then(() => {
                            window.location.href = data.redirectUrl;
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: data.message || 'Ocurrió un error al guardar.',
                            confirmButtonText: 'OK',
                            customClass: {
                                popup: 'swal2-popup',
                                title: 'swal2-title',
                                content: 'swal2-content',
                                confirmButton: 'botonConfir',
                                cancelButton: 'botonCancel'
                            }
                        });
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Error inesperado. Intente de nuevo más tarde.',
                        confirmButtonText: 'OK',
                        customClass: {
                            popup: 'swal2-popup',
                            title: 'swal2-title',
                            content: 'swal2-content',
                            confirmButton: 'botonConfir',
                            cancelButton: 'botonCancel'
                        }
                    });
                });
        });
    }
});

