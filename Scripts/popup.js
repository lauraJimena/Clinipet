document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById('guardarForm');
    if (form) {
        form.addEventListener('submit', function (e) {
            e.preventDefault(); // Evita envío tradicional

            const formData = new FormData(this);

            fetch(form.getAttribute('data-url'), { // Usa el atributo data-url de la vista
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
                            confirmButtonText: 'OK'
                        });
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Error inesperado. Intente de nuevo más tarde.',
                        confirmButtonText: 'OK'
                    });
                });
        });
    }
});
