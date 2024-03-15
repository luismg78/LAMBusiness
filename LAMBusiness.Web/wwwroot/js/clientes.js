$(document).ready(function () {
    $('#EstadoID').select2({
        language: "es",
        placeholder: 'Seleccionar Estado',
        minimumResultsForSearch: Infinity,
        width: '100%'
    });

    $('#EstadoID').on('select2:select', function (e) {
        var data = e.params.data;
        StateChange();
    });

    $('#MunicipioID').select2({
        language: "es",
        placeholder: 'Seleccionar Municipio',
        minimumResultsForSearch: Infinity,
        width: '100%'
    });

    function StateChange() {
        addProcessWithSpinner('Cargando municipios...');
        $.ajax({
            url: urlMunicipios,
            method: 'POST',
            datatype: 'text',
            data: {
                id: $('#EstadoID').val()
            },
            success: function (r) {
                $('#MunicipioID').empty();
                if (r !== null && r.trim() !== '') {
                    $('#MunicipioID').append(r);
                }
                removeProcessWithSpinner();
            },
            error: function (r) {
                removeProcessWithSpinner();
            },
            cache: false
        });
    }
});