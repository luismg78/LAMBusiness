var Checkbox = document.getElementById('ValorPorDefault');

Checkbox.addEventListener('change', function () {
    if (Checkbox.checked) {
        Swal.fire({
            title: '¿Marcar como predeterminado?',
            text: 'Al marcar esta opción como predeterminada se eliminará su altual forma de pago predeterminada.',
            icon: 'question',
        });
    }
});