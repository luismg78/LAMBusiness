let almacenId = $('#AlmacenID');

let dRowCorteDeCaja = document.getElementById('dRowCorteDeCaja');
let CorteDeCajaSearchInput = document.getElementById('CorteDeCajaSearchInput');
let CorteDeCajaTodosButton = document.getElementById('CorteDeCajaTodosButton');
let CorteDeCajaPendientesButton = document.getElementById('CorteDeCajaPendientesButton');

let FechaInicioInput = document.getElementById('FechaInicio');
let FechaFinInput = document.getElementById('FechaFin');

let dRowRetiro = document.getElementById('dRowRetiro');
let retiroSearchInput = document.getElementById('RetiroSearchInput');
let retiroTodosButton = document.getElementById('RetiroTodosButton');
let retiroPendientesButton = document.getElementById('RetiroPendientesButton');

let productoId = $('#ProductoID');
let cantidad = document.getElementById('Cantidad');
let precioCosto = document.getElementById('PrecioCosto');
let precioVenta = document.getElementById('PrecioVenta');
let importe = document.getElementById('Importe');

let proveedorId = $('#ProveedorID');
let folio = document.getElementById('Folio');

let applyConfirm = document.getElementById('applyConfirmButton');
let modalHelp = document.getElementById('ModalHelp');
let modalHelpLabel = document.getElementById('ModalHelpLabel');

let itemToApply;

if (applyConfirm) {
    applyConfirm.addEventListener('click', (event) => {
        itemToApply = applyConfirm.dataset.id;
    });
}
function addRowsNextCortesDeCaja(skip, todos, inicio = false) {
    addProcessWithSpinnerInList('SpinListAlmacen', 'fa-search');
    dRowCorteDeCaja.setAttribute('data-charging', "1");
    var searchBy = CorteDeCajaSearchInput === undefined ? "" : CorteDeCajaSearchInput.value;
    var date1 = FechaInicioInput === undefined ? "" : FechaInicioInput.value;
    var date2 = FechaFinInput === undefined ? "" : FechaFinInput.value;

    $.ajax({
        url: urlCortesDeCaja,
        method: 'POST',
        datatype: 'text',
        data: {
            filtro: {
                Patron: searchBy,
                Skip: skip,
                FechaInicio: date1,
                FechaFin: date2
            },
            todos: todos === '1' ? true : false
        },
        success: function (r) {
            if (inicio) { dRowCorteDeCaja.innerHTML = ''; }
            if (r !== null && r.trim() !== '') {
                dRowCorteDeCaja.innerHTML += r;
                dRowCorteDeCaja.scrollTop -= 100;
                CorteDeCajaSearchInput.focus();
            } else {
                dRowCorteDeCaja.setAttribute('data-rows-next', "0");
            }
            dRowCorteDeCaja.setAttribute('data-charging', "0");
            removeProcessWithSpinnerInList('SpinListAlmacen', 'fa-search');
        },
        error: function (r) {
            dRowCorteDeCaja.setAttribute('data-charging', "0");
            removeProcessWithSpinnerInList('SpinListAlmacen', 'fa-search');
        },
        cache: false
    });
}
function addRowsNextRetiros(skip, todos, inicio = false) {
    addProcessWithSpinnerInList('SpinListAlmacen', 'fa-search');
    dRowRetiro.setAttribute('data-charging', "1");
    var searchBy = retiroSearchInput === undefined ? "" : retiroSearchInput.value;
    var date1 = FechaInicioInput === undefined ? "" : FechaInicioInput.value;
    var date2 = FechaFinInput === undefined ? "" : FechaFinInput.value;

    $.ajax({
        url: urlRetiros,
        method: 'POST',
        datatype: 'text',
        data: {
            filtro: {
                Patron: searchBy,
                Skip: skip,
                FechaInicio: date1,
                FechaFin: date2
            },
            todos: todos === '1' ? true : false
        },
        success: function (r) {
            if (inicio) { dRowRetiro.innerHTML = ''; }
            if (r !== null && r.trim() !== '') {
                dRowRetiro.innerHTML += r;
                dRowRetiro.scrollTop -= 100;
                retiroSearchInput.focus();
            } else {
                dRowRetiro.setAttribute('data-rows-next', "0");
            }
            dRowRetiro.setAttribute('data-charging', "0");
            removeProcessWithSpinnerInList('SpinListAlmacen', 'fa-search');
        },
        error: function (r) {
            dRowRetiro.setAttribute('data-charging', "0");
            removeProcessWithSpinnerInList('SpinListAlmacen', 'fa-search');
        },
        cache: false
    });
}
function apply(e) {
    addProcess("Aplicando movimiento...");
    var url = urlApply;
    url = url.replace('paramId', itemToApply);

    window.location.href = url;
}
function changeCortesDeCaja(e, value) {
    dRowCorteDeCaja.setAttribute('data-cortesdecaja', value);
    if (e.currentTarget.id === 'CorteDeCajaTodosButton') {
        CorteDeCajaPendientesButton.classList.remove('btn-base');
        CorteDeCajaPendientesButton.classList.add('btn-outline-base');
    } else {
        CorteDeCajaTodosButton.classList.remove('btn-base');
        CorteDeCajaTodosButton.classList.add('btn-outline-base');
    }
    e.currentTarget.classList.remove('btn-outline-base');
    e.currentTarget.classList.add('btn-base');
    searchCortesDeCaja(event);
}
function changeRetiros(e, value) {
    dRowRetiro.setAttribute('data-retiros', value);
    if (e.currentTarget.id === 'RetiroTodosButton') {
        retiroPendientesButton.classList.remove('btn-base');
        retiroPendientesButton.classList.add('btn-outline-base');
    } else {
        retiroTodosButton.classList.remove('btn-base');
        retiroTodosButton.classList.add('btn-outline-base');
    }
    e.currentTarget.classList.remove('btn-outline-base');
    e.currentTarget.classList.add('btn-base');
    searchRetiros(event);
}
function deleteRegister(e) {
    var url = urlDeleteDetails;
    url = url.replace('paramId', itemToDelete);

    window.location.href = url;
}
function getProductoDetail(id) {
    addProcessWithSpinnerInList('SpinList', 'fa-search');
    $.ajax({
        url: urlProductoDetails,
        method: 'POST',
        datatype: 'text',
        data: {
            id: id,
            mostrarPrecioCosto: true
        },
        success: function (r) {
            if (r !== null && r.trim() !== '') {
                let myModal = new bootstrap.Modal(document.getElementById('ModalHelp'), {
                    keyboard: true
                })
                myModal.show(modalHelp);
                modalHelpLabel.innerHTML = 'Producto';
                modalHelp.querySelectorAll('div.modal-body')[0].innerHTML = r;
            }
            removeProcessWithSpinnerInList('SpinList', 'fa-search');
        },
        error: function (r) {
            removeProcessWithSpinnerInList('SpinList', 'fa-search');
        },
        cache: false
    });
}
function searchCortesDeCaja(e) {
    if (e.keyCode === 13 || e.type === 'click') {
        e.preventDefault();
        skip = 0;
        let todos = dRowCorteDeCaja.getAttribute('data-cortesdecaja');
        var f = document.getElementById('dfiltro');
        if (todos === '1') {
            fechaInicioLabel.innerText = FechaInicioInput.value;
            fechaFinLabel.innerText = FechaFinInput.value;
            f.classList.remove('d-none');
        } else {
            f.classList.add('d-none');
        }
        addRowsNextCortesDeCaja(skip, todos, true);
    }
}
function searchRetiros(e) {
    if (e.keyCode === 13 || e.type === 'click') {
        e.preventDefault();
        skip = 0;
        let todos = dRowRetiro.getAttribute('data-retiros');
        var f = document.getElementById('dfiltro');
        if (todos === '1') {
            fechaInicioLabel.innerText = FechaInicioInput.value;
            fechaFinLabel.innerText = FechaFinInput.value;
            f.classList.remove('d-none');
        } else {
            f.classList.add('d-none');
        }
        addRowsNextRetiros(skip, todos, true);
    }
}

//if (typeof urlAlmacenes !== 'undefined' && urlAlmacenes !== undefined && urlAlmacenes !== null) {
//    $(almacenId).on('select2:select', function (e) {
//        var data = e.params.data;
//        $(almacenId).val(data.id).change();
//    });
//    $(almacenId).select2({
//        ajax: {
//            url: urlAlmacenes,
//            dataType: 'json',
//            delay: 250,
//            data: function (params) {
//                return {
//                    Patron: params.term,
//                    Skip: 0
//                };
//            },
//            processResults: function (data) {
//                var results = [];
//                $.each(data, function (index, item) {
//                    results.push({
//                        id: item.id,
//                        text: item.text
//                    });
//                });

//                return { results };
//            },
//            cache: true
//        },
//        language: "es",
//        placeholder: 'Seleccionar almacén',
//        width: '100%',
//        allowClear: true,
//        language: 'es',
//        minimumInputLength: 3,
//        minimumResultsForSearch: 50
//    });
//}

if (typeof $(almacenId) !== 'undefined' && $(almacenId) !== undefined && $(almacenId) !== null) {
    $(almacenId).select2({
        language: "es",
        placeholder: 'Seleccionar almacén',
        width: '100%',
        allowClear: true,
        minimumResultsForSearch: Infinity,
        language: 'es'
    });
}
if (typeof urlProductos !== 'undefined' && urlProductos !== undefined && urlProductos !== null) {
    $(productoId).on('select2:select', function (e) {
        var data = e.params.data;
        $(productoId).val(data.id).trigger('change');
        $(precioCosto).val(data.precioCosto);
        $(precioVenta).val(data.precioVenta);
        if (typeof importe !== 'undefined' && importe !== null) {
            let totalImporte = 0;
            if ($(cantidad).val() > 0) {
                totalImporte = $(cantidad).val() * data.precioCosto;
                importe.innerHTML = formatCurrency(totalImporte);
            } else {
                importe.innerHTML = '$0.00';
            }
        }
        if (typeof $('#EsPaquete') !== 'undefined') {
            $('#EsPaquete').val(data.esPaquete);
        }
        if (typeof $('#cantidadEnPiezas') !== 'undefined') {
            if (data.esPaquete) {
                $('#cantidadEnPiezas').removeClass('d-none');
            } else {
                $('#cantidadEnPiezas').addClass('d-none');
            }
        }
    });
    $(productoId).select2({
        ajax: {
            url: urlProductos,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    Patron: params.term,
                    Skip: 0
                };
            },
            processResults: function (data) {
                var results = [];
                $.each(data, function (index, item) {
                    results.push({
                        id: item.id,
                        text: item.text,
                        precioCosto: item.precioCosto,
                        precioVenta: item.precioVenta,
                        esPaquete: item.esPaquete
                    });
                });

                return { results };
            },
            cache: true
        },
        language: "es",
        placeholder: 'Seleccionar producto',
        width: '100%',
        allowClear: true,
        language: 'es',
        minimumInputLength: 3,
        minimumResultsForSearch: 50
    });
}
if (typeof urlProveedores !== 'undefined' && urlProveedores !== undefined && urlProveedores !== null) {
    $(proveedorId).on('select2:select', function (e) {
        var data = e.params.data;
        $(proveedorID).val(data.id).change();
    });
    $(proveedorId).select2({
        ajax: {
            url: urlProveedores,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    Patron: params.term,
                    Skip: 0
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
        placeholder: 'Seleccionar proveedor',
        width: '100%',
        allowClear: true,
        language: 'es',
        minimumInputLength: 3,
        minimumResultsForSearch: 50
    });
}

function kardexDeMovimientos() {
    var url = `${urlKardexDeMovimientos}?productoId=${productoId.val()}&almacenId=${almacenId.val()}`
    let kardex = document.getElementById('kardexProductos');
    kardex.innerHTML = '<div class="p-5 text-center"><div class="spinner-border text-base" role="status"><span class="visually-hidden">Loading...</span></div></div>'
    fetch(url)
        .then((response) => { return response.text(); })
        .then((result) => {
            kardex.innerHTML = result;
        })
}