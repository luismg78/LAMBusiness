$(document).ready(function () {
    if (typeof $('#MarcaID') !== 'undefined') {
        $('#MarcaID').select2({
            language: "es",
            placeholder: 'Seleccionar marca',
            width: '100%'
        });
    }
    if (typeof $('#UnidadID') !== 'undefined') {
        $('#UnidadID').select2({
            language: "es",
            placeholder: 'Seleccionar unidad',
            minimumResultsForSearch: Infinity,
            width: '100%'
        });
    }
    if (typeof $('#TasaID') !== 'undefined') {
        $('#TasaID').select2({
            language: "es",
            placeholder: 'Seleccionar tasa',
            minimumResultsForSearch: Infinity,
            width: '100%'
        });
    }
    if (typeof $('#ProductoIDPieza') !== 'undefined') {
        $('#ProductoIDPieza').on('select2:select', function (e) {
            var data = e.params.data;
            $('#ProductoIDPieza').val(data.id).change();
            $('#CodigoPiezaNombre').val(data.text);
        });
        $('#ProductoIDPieza').select2({
            ajax: {
                url: urlObtenerProductosPiezas,
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        pattern: params.term
                    };
                },
                processResults: function (data) {
                    var results = [];
                    $.each(data, function (index, item) {
                        results.push({
                            id: item.id,
                            text: item.text
                        });
                    });

                    return { results };
                },
                cache: true
            },
            language: "es",
            placeholder: 'Teclee código o nombre del producto',
            width: '100%',
            allowClear: true,
            language: 'es',
            minimumInputLength: 3,
            minimumResultsForSearch: 50
        });
    }
    validatedUnitChange();
});

$('#UnidadID').change(function () {
    validatedUnitChange();
});

function validatedUnitChange() {
    if ($('#UnidadID').val() === '6C9C7801-D654-11E9-8B00-8CDCD47D68A1' ||
        $('#UnidadID').val() === '95B850EC-D654-11E9-8B00-8CDCD47D68A1') {
        $('#dPackage').show();
    } else {
        $('#dPackage').hide();
    }
}