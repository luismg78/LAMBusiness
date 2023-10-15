let dRowAlmacen = document.getElementById('dRowAlmacen');
let almacenSearchInput = document.getElementById('AlmacenSearchInput');
let almacenId = document.getElementById('AlmacenID');
let almacenNombre = document.getElementById('Nombre');
let almacenDescripcion = document.getElementById('Descripcion');
let almacenDatos = document.getElementById('AlmacenDatos');
let almacenSearch = document.getElementById('AlmacenSearch');

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

let dRowProducto = document.getElementById('dRowProducto');
let productoSearchInput = document.getElementById('ProductoSearchInput');

let productoId = document.getElementById('ProductoID');
let cantidad = document.getElementById('Cantidad');
let precioCosto = document.getElementById('PrecioCosto');
let precioVenta = document.getElementById('PrecioVenta');
let productoNombre = document.getElementById('Nombre');
let productoCodigo = document.getElementById('ProductoCodigo');
let importe = document.getElementById('Importe');
let productoDatos = document.getElementById('ProductoDatos');
let productoSearch = document.getElementById('ProductoSearch');

let dRowProveedor = document.getElementById('dRowProveedor');
let proveedorSearchInput = document.getElementById('ProveedorSearchInput');
let proveedorID = document.getElementById('ProveedorID');
let proveedorNombre = document.getElementById('ProveedorNombre');
let proveedorRFC = document.getElementById('ProveedorRFC');
let proveedorDatos = document.getElementById('ProveedorDatos');
let proveedorSearch = document.getElementById('ProveedorSearch');
let proveedoresLista = document.getElementById('ProveedoresLista');
let folio = document.getElementById('Folio');

let applyConfirm = document.getElementById('applyConfirmButton');
let modalHelp = document.getElementById('ModalHelp');
let modalHelpLabel = document.getElementById('ModalHelpLabel');

if (dRowProducto) {
    dRowProducto.addEventListener('scroll', (event) => {
        if (dRowProducto.getAttribute('data-rows-next') === "1" &&
            dRowProducto.getAttribute('data-charging') === "0" &&
            dRowProducto.getElementsByClassName('product').length % 50 === 0) {
            let scrollHeight = dRowProducto.scrollHeight;
            let scrollPosition = dRowProducto.offsetHeight + dRowProducto.scrollTop;
            if (scrollPosition >= scrollHeight) {
                skip += 50;
                addRowsNextProducto(skip);
            }
        }
    });
}
if (dRowAlmacen) {
    dRowAlmacen.addEventListener('scroll', (event) => {
        if (dRowAlmacen.getAttribute('data-rows-next') === "1" &&
            dRowAlmacen.getAttribute('data-charging') === "0" &&
            dRowAlmacen.getElementsByClassName('almacen') % 50 === 0) {
            let scrollHeight = dRowAlmacen.scrollHeight;
            let scrollPosition = dRowAlmacen.offsetHeight + dRowAlmacen.scrollTop;
            if (scrollPosition >= scrollHeight) {
                skip += 50;
                addRowsNextAlmacen(skip);
            }
        }
    });
}
if (dRowProveedor) {
    dRowProveedor.addEventListener('scroll', (event) => {
        if (dRowProveedor.getAttribute('data-rows-next') === "1" &&
            dRowProveedor.getAttribute('data-charging') === "0" &&
            dRowProveedor.getElementsByClassName('proveedor') % 50 === 0) {
            let scrollHeight = dRowProveedor.scrollHeight;
            let scrollPosition = dRowProveedor.offsetHeight + dRowProveedor.scrollTop;
            if (scrollPosition >= scrollHeight) {
                skip += 50;
                addRowsNextProveedor(skip);
            }
        }
    });
}

let itemToApply;

if (applyConfirm) {
    applyConfirm.addEventListener('click', (event) => {
        itemToApply = applyConfirm.dataset.id;
    });
}

function addAlmacen(e) {
    e.preventDefault();
    let almacen = document.querySelector('li.almacen');
    getAlmacenByName(almacen.dataset.search);
}

function addProduct(e) {
    e.preventDefault();
    let producto = document.querySelector('li.product');
    getProductoByCode(producto.dataset.search);
}

function addProveedor(e) {
    e.preventDefault();
    let proveedor = document.querySelector('li.proveedor');
    getProveedorByRFC(proveedor.dataset.search);
}

function addRowsNextAlmacen(skip, inicio = false) {
    addProcessWithSpinnerInList('SpinListAlmacen', 'fa-search');
    dRowAlmacen.setAttribute('data-charging', "1");
    var searchBy = almacenSearchInput === undefined ? "" : almacenSearchInput.value;
    if (searchBy === '') {
        dRowAlmacen.setAttribute('data-rows-next', "0");
        dRowAlmacen.setAttribute('data-charging', "0");
        removeProcessWithSpinnerInList('SpinListAlmacen', 'fa-search');
        return false;
    }
    $.ajax({
        url: urlAlmacenes,
        method: 'POST',
        datatype: 'text',
        data: {
            filtro: {
                Patron: searchBy,
                Skip: skip
            }
        },
        success: function (r) {
            if (inicio) { dRowAlmacen.innerHTML = ''; }
            if (r !== null && r.trim() !== '') {
                dRowAlmacen.innerHTML += r;
                dRowAlmacen.scrollTop -= 100;
                almacenSearchInput.focus();
            } else {
                dRowAlmacen.setAttribute('data-rows-next', "0");
            }
            dRowAlmacen.setAttribute('data-charging', "0");
            removeProcessWithSpinnerInList('SpinListAlmacen', 'fa-search');
        },
        error: function (r) {
            dRowAlmacen.setAttribute('data-charging', "0");
            removeProcessWithSpinnerInList('SpinListAlmacen', 'fa-search');
        },
        cache: false
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

function addRowsNextProducto(skip, inicio = false) {
    addProcessWithSpinnerInList('SpinList', 'fa-search');
    dRowProducto.setAttribute('data-charging', "1");
    var searchBy = productoSearchInput === undefined ? "" : productoSearchInput.value;
    if (searchBy === '') {
        dRowProducto.setAttribute('data-rows-next', "0");
        dRowProducto.setAttribute('data-charging', "0");
        removeProcessWithSpinnerInList('SpinList', 'fa-search');
        return false;
    }
    $.ajax({
        url: urlProductos,
        method: 'POST',
        datatype: 'text',
        data: {
            filtro: {
                Patron: searchBy,
                Skip: skip
            }
        },
        success: function (r) {
            if (inicio) { dRowProducto.innerHTML = ''; }
            if (r !== null && r.trim() !== '') {
                dRowProducto.innerHTML += r;
                dRowProducto.scrollTop -= 100;
                productoSearchInput.focus();
            } else {
                dRowProducto.setAttribute('data-rows-next', "0");
            }
            dRowProducto.setAttribute('data-charging', "0");
            removeProcessWithSpinnerInList('SpinList', 'fa-search');
        },
        error: function (r) {
            dRowProducto.setAttribute('data-charging', "0");
            removeProcessWithSpinnerInList('SpinList', 'fa-search');
        },
        cache: false
    });
}

function addRowsNextProveedor(skip, inicio = false) {
    addProcessWithSpinnerInList('SpinList', 'fa-search');
    dRowProveedor.setAttribute('data-charging', "1");
    var searchBy = proveedorSearchInput === undefined ? "" : proveedorSearchInput.value;
    if (searchBy === '') {
        dRowProveedor.setAttribute('data-rows-next', "0");
        dRowProveedor.setAttribute('data-charging', "0");
        removeProcessWithSpinnerInList('SpinList', 'fa-search');
        return false;
    }
    $.ajax({
        url: urlProveedores,
        method: 'POST',
        datatype: 'text',
        data: {
            filtro: {
                Patron: searchBy,
                Skip: skip
            }
        },
        success: function (r) {
            if (inicio) { dRowProveedor.innerHTML = ''; }
            if (r !== null && r.trim() !== '') {
                dRowProveedor.innerHTML += r;
                dRowProveedor.scrollTop -= 100;
                ProveedorSearchInput.focus();
            } else {
                dRowProveedor.setAttribute('data-rows-next', "0");
            }
            dRowProveedor.setAttribute('data-charging', "0");
            removeProcessWithSpinnerInList('SpinList', 'fa-search');
        },
        error: function (r) {
            dRowProveedor.setAttribute('data-charging', "0");
            removeProcessWithSpinnerInList('SpinList', 'fa-search');
        },
        cache: false
    });
}

function apply(e) {
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

function deleteButtonAlmacen(e) {
    e.preventDefault();
    almacenId.value = '';
    almacenNombre.innerHTML = '';
    almacenDescripcion.innerHTML = '';
    almacenDatos.classList.remove('d-block');
    almacenDatos.classList.add('d-none');
    almacenSearch.classList.remove('d-none');
    almacenSearch.classList.add('d-block');
    if (dRowAlmacen.children[0] !== undefined) {
        dRowAlmacen.children[0].remove();
    }
    almacenSearchInput.value = '';
    almacenSearchInput.focus();
}

function deleteButtonProducto(e) {
    e.preventDefault();
    productoId.value = '';
    cantidad.value = '';
    if (precioCosto) {
        precioCosto.value = '';
    }
    if (precioVenta) {
        precioVenta.value = '';
    }
    if (importe) {
        importe.innerHTML = '$0.00';
    }
    productoNombre.innerHTML = '';
    productoCodigo.innerHTML = '';
    productoDatos.classList.remove('d-block');
    productoDatos.classList.add('d-none');
    productoSearch.classList.remove('d-none');
    productoSearch.classList.add('d-block');
    if (dRowProducto.children[0] !== undefined) {
        dRowProducto.children[0].remove();
    }
    productoSearchInput.value = '';
    productoSearchInput.focus();
}

function deleteButtonProveedor(e) {
    e.preventDefault();
    proveedorID.value = '';
    proveedorNombre.innerHTML = '';
    proveedorRFC.innerHTML = '';
    proveedorDatos.classList.remove('d-block');
    proveedorDatos.classList.add('d-none');
    proveedorSearch.classList.remove('d-none');
    proveedorSearch.classList.add('d-block');
    if (dRowProveedor.children[0] !== undefined) {
        dRowProveedor.children[0].remove();
    }
    proveedorSearchInput.value = '';
    proveedorSearchInput.focus();
}

function deleteRegister(e) {
    var url = urlDeleteDetails;
    url = url.replace('paramId', itemToDelete);

    window.location.href = url;
}

function getAlmacen(e) {
    switch (e.keyCode) {
        case 13: //Enter
            e.preventDefault();
            if (dRowProducto.children[0] !== undefined) {
                dRowProducto.children[0].remove();
            }
            getAlmacenByName(almacenSearchInput.value);
            break;
        case 27: //ESC
            e.preventDefault();
            almacenSearchInput.value = "";
            if (dRowAlmacen.children[0] !== undefined) {
                dRowAlmacen.children[0].remove();
            }
            break;
    }
}

function getAlmacenByName(name) {
    if (name !== '') {
        addProcessWithSpinnerInList('SpinListAlmacen', 'fa-search');
        $.ajax({
            url: urlAlmacen,
            method: 'POST',
            datatype: 'text',
            data: { almacenNombre: name },
            success: function (r) {
                if (!r.error) {
                    almacenDatos.classList.remove('d-none');
                    almacenDatos.classList.add('d-block');
                    almacenId.value = r.almacenID;
                    almacenNombre.innerHTML = r.almacenNombre;
                    almacenDescripcion.innerHTML = r.almacenDescripcion;
                    almacenSearch.classList.remove('d-block');
                    almacenSearch.classList.add('d-none');
                    if (dRowAlmacen.children[0] !== undefined) {
                        dRowAlmacen.children[0].remove();
                    }
                    if (productoSearch.classList.contains('d-none')) {
                        precioVenta.focus();
                    } else {
                        productoSearchInput.focus();
                    }
                } else {
                    alert(r.message);
                }
                removeProcessWithSpinnerInList('SpinListAlmacen', 'fa-search');
            },
            error: function (r) {
                removeProcessWithSpinnerInList('SpinListAlmacen', 'fa-search');
            },
            cache: false
        });
    }
}

function getProducto(e) {
    switch (e.keyCode) {
        case 13: //Enter
            e.preventDefault();
            if (dRowAlmacen.children[0] !== undefined) {
                dRowAlmacen.children[0].remove();
            }
            getProductoByCode(productoSearchInput.value);
            break;
        case 27: //ESC
            e.preventDefault();
            productoSearchInput.value = "";
            if (dRowProducto.children[0] !== undefined) {
                dRowProducto.children[0].remove();
            }
            break;
    }
}

function getProveedor(e) {
    switch (e.keyCode) {
        case 13: //Enter
            e.preventDefault();
            getProveedorByRFC(proveedorSearchInput.value);
            break;
        case 27: //ESC
            e.preventDefault();
            proveedorSearchInput.value = "";
            if (dRowProveedor.children[0] !== undefined) {
                dRowProveedor.children[0].remove();
            }
            break;
    }
}

function getProductoByCode(code) {
    if (code !== '') {
        addProcessWithSpinnerInList('SpinList', 'fa-search');
        $.ajax({
            url: urlProducto,
            method: 'POST',
            datatype: 'text',
            data: { code: code },
            success: function (r) {
                if (!r.error) {
                    productoDatos.classList.remove('d-none');
                    productoDatos.classList.add('d-block');
                    productoId.value = r.productoID;
                    productoNombre.innerHTML = r.productoNombre;
                    productoCodigo.innerHTML = r.codigo;
                    cantidad.value = '1';
                    if (precioCosto) {
                        precioCosto.value = r.precioCosto;
                    }
                    if (precioVenta) {
                        precioVenta.value = r.precioVenta;
                    }
                    if (importe) {
                        importe.innerHTML = formatCurrency(r.precioCosto);
                    }
                    productoSearch.classList.remove('d-block');
                    productoSearch.classList.add('d-none');
                    if (dRowProducto.children[0] !== undefined) {
                        dRowProducto.children[0].remove();
                    }
                    if (precioVenta) {
                        precioVenta.focus();
                    } else {
                        cantidad.focus();
                    }
                } else {
                    alert(r.message);
                }
                removeProcessWithSpinnerInList('SpinList', 'fa-search');
            },
            error: function (r) {
                removeProcessWithSpinnerInList('SpinList', 'fa-search');
            },
            cache: false
        });
    }
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

function getProveedorByRFC(rfc) {
    if (rfc !== '') {
        addProcessWithSpinnerInList('SpinList', 'fa-search');
        $.ajax({
            url: urlProveedor,
            method: 'POST',
            datatype: 'text',
            data: { rfc: rfc },
            success: function (r) {
                if (!r.error) {
                    proveedorDatos.classList.remove('d-none');
                    proveedorDatos.classList.add('d-block');
                    proveedorID.value = r.proveedorID;
                    proveedorNombre.innerHTML = r.nombre;
                    proveedorRFC.innerHTML = r.rfc;
                    proveedorSearch.classList.remove('d-block');
                    proveedorSearch.classList.add('d-none');
                    if (dRowProveedor.children[0] !== undefined) {
                        dRowProveedor.children[0].remove();
                    }
                    folio.focus();
                } else {
                    alert(r.message);
                }
                removeProcessWithSpinnerInList('SpinList', 'fa-search');
            },
            error: function (r) {
                removeProcessWithSpinnerInList('SpinList', 'fa-search');
            },
            cache: false
        });
    }
}

function searchAlmacen(e) {
    e.preventDefault();
    if (almacenSearchInput.value !== '') {
        skip = 0;
        if (dRowProducto.children[0] !== undefined) {
            dRowProducto.children[0].remove();
        }
        addRowsNextAlmacen(skip, true);
    }
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

function searchProducto(e) {
    e.preventDefault();
    if (productoSearchInput.value !== '') {
        skip = 0;
        if (dRowAlmacen.children[0] !== undefined) {
            dRowAlmacen.children[0].remove();
        }
        addRowsNextProducto(skip, true);
    }
}

function searchProveedor(e) {
    e.preventDefault();
    if (ProveedorSearchInput.value !== '') {
        skip = 0;
        addRowsNextProveedor(skip, true);
    }
}