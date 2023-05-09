var cant = '1';
let ventaId = document.getElementById('VentaNoAplicadaID');
let inputText = document.getElementById('inputText');
let icon = document.getElementById('icon');
let message = document.getElementById('messageLabel');

let myOffcanvas = document.getElementById('offcanvasRight');
let bsOffcanvas = new bootstrap.Offcanvas(myOffcanvas);
myOffcanvas.addEventListener('hidden.bs.offcanvas', function () {
    inputText.focus();
});

let modalHelp = document.getElementById('ModalHelp');
modalHelp.addEventListener('hidden.bs.modal', function (event) {
    window.location.href = urlSale;
})

screenResize();
resetMode('saleButton');
window.addEventListener('resize', function () {
    screenResize();
});

document.getElementById('inputText').addEventListener('keydown', function (e) {
    let key = e.which;
    let isShift = !!e.shiftKey;
    if (isShift) {
        switch (key) {
            case 16:
                break;
            case 56: //*
            case 187:
                if (isNaN(inputText.value) || parseFloat(inputText.value) === 0) {
                    cant = '1';
                    message.innerHTML = 'Cantidad incorrecta';
                    message.style.display = 'block';
                } else {
                    message.style.display = 'none';
                    cant = inputText.value;
                }
                inputText.value = '';
                e.cancelable = true;
                e.preventDefault();
                break;
            default:
                alert(key);
                break;
        }
    } else {
        switch (key) {
            case 13: //enter
                setInputText(e);
                break;
            case 27: //ESC            
                saleMode();
                break;
            case 106: //*
                if (isNaN(inputText.value) || parseFloat(inputText.value) === 0) {
                    cant = '1';
                    message.innerHTML = 'Cantidad incorrecta';
                    message.style.display = 'block';
                } else {
                    message.style.display = 'none';
                    cant = inputText.value;
                }
                inputText.value = '';
                e.cancelable = true;
                e.preventDefault();
                break;
            case 112: //F1
                e.preventDefault();
                searchMode();
                break;
            case 114: //F3    
                e.preventDefault();
                cancelSaleMode();
                break;
            case 115: //F4
                e.preventDefault();
                getItBackSaleMode();
                break;
            case 116: //F5
                e.preventDefault();
                payMode();
                break;
            case 118: //F7 retiro de efectivo
                withdrawCashMode(event);
                break;
            case 119: //F8
                e.preventDefault();
                closeSalesMode();
                break;
        }
    }
});

let modalElement = document.getElementById('ModalHelp');
modalElement.addEventListener('hidden.bs.modal', function (event) {
    inputText.focus();
})

let offCanvasBody = document.getElementsByClassName('offcanvas-body');
if (offCanvasBody) {
    offCanvasBody[0].addEventListener('scroll', (event) => {
        var dRowProducto = document.getElementById('dRowProducto');
        if (dRowProducto.getAttribute('data-rows-next') === "1" &&
            dRowProducto.getAttribute('data-charging') === "0" &&
            dRowProducto.getElementsByClassName('product').length % 50 === 0) {
            let scrollHeight = offCanvasBody[0].scrollHeight;
            let scrollPosition = offCanvasBody[0].scrollTop + offCanvasBody[0].offsetHeight;
            if (scrollPosition >= scrollHeight) {
                skip += 50;
                addRowsNextProducto(skip);
            }
        }
    });
}

function addRowsNextProducto(skip, inicio = false) {
    addProcessWithSpinnerInList('SpinList', 'fa-search');
    dRowProducto.setAttribute('data-charging', "1");
    $.ajax({
        url: urlProductList,
        method: 'POST',
        datatype: 'text',
        data: {
            filtro: {
                Patron: inputText.value,
                Skip: skip
            }
        },
        success: function (r) {
            if (inicio) { dRowProducto.innerHTML = ''; }
            if (r !== null && r.trim() !== '') {
                dRowProducto.children[0].innerHTML += r;
                dRowProducto.scrollTop -= 100;
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
function addProduct(e) {
    let product = e.currentTarget;
    inputText.value = product.dataset.search;
    getProductByCode();
    bsOffcanvas.hide();
}
function addSale(e) {
    let sale = e.currentTarget;
    window.location.href = urlSale + '?id=' + sale.dataset.id;
}
function cancelSale(e) {
    e.preventDefault();
    addProcessWithSpinner('Cancelando venta...');
    $.ajax({
        url: urlCancelSale,
        method: 'POST',
        datatype: 'text',
        data: { id: ventaId.value },
        success: function (result) {
            if (result.error !== undefined && result.error !== null && result.error) {
                if (result.reiniciar) {
                    window.location.href = urlMovement;
                } else {
                    message.innerHTML = result.estatus;
                    message.style.display = 'block';
                    removeProcessWithSpinner();
                    bsOffcanvas.hide();
                    inputText.focus();
                }
            } else {
                window.location.href = urlSale;
            }
        },
        error: function (r) {
            alert(r);
            removeProcessWithSpinner();
        },
        cache: false
    });
}
function cancelSaleMode() {
    if (!validateSale()) { return; }
    resetMode('cancelSaleButton');
    let myOffCanvasLabel = document.getElementById('offcanvasRightLabel');
    myOffCanvasLabel.innerHTML = "Cancelar Venta";
    let myOffCanvasBody = document.getElementsByClassName('offcanvas-body');
    let html = '<div class="p-3">';
    html += '<p class="text-muted">Presione el botón cancelar si desea anular la venta actual e iniciar una nueva venta.</p>';
    html += '<button class="btn btn-danger mb-3" onclick="cancelSale(event);">Cancelar venta</button>';
    html += '<p class="text-muted">Presione el botón guardar si desea almacenar temporalmente la venta actual e iniciar una nueva venta.</p>';
    html += '<button class="btn btn-warning mb-3" onclick="saveSale(event);">Guardar venta</button>';
    html += '</div>';
    myOffCanvasBody[0].innerHTML = html;
    bsOffcanvas.show();
    inputText.focus();
}
function closeSalesMode() {
    resetMode('closeSalesButton');
    inputText.setAttribute('type', 'password');
    inputText.dataset.input = 'corteCaja';
    icon.classList.remove('fa-barcode', 'fa-search', 'fa-dollar-sign', 'fa-cash-register');
    icon.classList.add('fa-file-invoice-dollar');
    inputText.focus();
}
function getItBackSale(e) {
    e.preventDefault();
}
function getItBackSaleMode() {
    $.ajax({
        url: urlGetItBackSale,
        method: 'POST',
        success: function (result) {
            if (result.error !== undefined && result.error !== null && result.error) {
                if (result.reiniciar) {
                    window.location.href = urlMovement;
                } else {
                    message.innerHTML = result.estatus;
                    message.style.display = 'block';
                }
            } else {
                resetMode('getItBackSaleButton');
                let myOffCanvasLabel = document.getElementById('offcanvasRightLabel');
                myOffCanvasLabel.innerHTML = "Ventas pendientes de aplicar";
                let myOffCanvasBody = document.getElementsByClassName('offcanvas-body');
                myOffCanvasBody[0].innerHTML = result;
                bsOffcanvas.show();
            }
        },
        error: function (r) {
            inputText.value = '';
            message.innerHTML = 'Código inexistente...';
            message.style.display = 'block';
        },
        cache: false
    });
}
function getProduct(id, code = false) {
    addProcessWithSpinnerInList('SpinList', 'fa-search');
    var url = urlProductDetail;
    if (code) {
        url = urlProductDetailByCode;
    }
    if (id === '') {
        messageLabel.innerHTML = 'Favor de proporcionar el código del producto';
        messageLabel.style.display = 'block';
        return false;
    }
    $.ajax({
        url: url,
        method: 'POST',
        datatype: 'text',
        data: { id: id },
        success: function (r) {
            if (r !== null && r.trim() !== '') {
                let myModal = new bootstrap.Modal(document.getElementById('ModalHelp'), {
                    keyboard: true
                })
                myModal.show(modalElement);
                let modalLabel = document.getElementById('ModalHelpLabel');
                let modalBody = document.getElementsByClassName('modal-body');
                modalLabel.innerHTML = 'Producto';
                modalBody[0].innerHTML = r;
                message.innerHTML = '';
                message.style.display = 'none';
            } else {
                message.innerHTML = 'Código inexistente...';
                message.style.display = 'block';
            }
            inputText.value = '';
            removeProcessWithSpinnerInList('SpinList', 'fa-search');
        },
        error: function (r) {
            removeProcessWithSpinnerInList('SpinList', 'fa-search');
        },
        cache: false
    });
}
function getProductByCode() {
    let searchProductBool = false;
    $.ajax({
        url: urlProductByCode,
        method: 'POST',
        datatype: 'text',
        data: {
            id: ventaId.value,
            codigo: inputText.value,
            cantidad: cant
        },
        success: function (result) {
            if (result.error !== undefined && result.error !== null && result.error) {
                if (result.buscarProducto) {
                    searchProductBool = true;
                } else if (result.reiniciar) {
                    window.location.href = urlMovement;
                } else {
                    message.innerHTML = result.estatus;
                    message.style.display = 'block';
                }
            } else {
                var elementNode = document.createElement('div');
                elementNode.classList.add('bg-white', 'list-group-item', 'list-group-item-action')
                elementNode.innerHTML = result;
                var box = document.getElementById('datos');
                box.children[0].appendChild(elementNode);
                box.scrollTop = box.scrollHeight;
                cant = '1';
                inputText.value = '';
                message.innerHTML = '';
                message.style.display = 'none';
                getTotalSum();
            }
        },
        error: function (r) {
            inputText.value = '';
            message.innerHTML = 'Código inexistente...';
            message.style.display = 'block';
        },
        cache: false,
        async: false
    });
    if (searchProductBool) {
        searchProduct();
    }
}
function getTotalSum() {
    var total = 0;
    let importe = document.getElementsByClassName('saleAmount');
    for (let i = 0; i < importe.length; i++) {
        total += parseFloat(importe[i].dataset.importe);
    }
    document.getElementById('total').innerHTML = formatCurrency(total);
    document.getElementById('total-sm').innerHTML = 'Total ' + formatCurrency(total);
}
function offcanvas(e) {
    bsOffcanvas.show();
}
function makeSale(e) {
    e.preventDefault();
    cant = cant === '' ? '1' : cant;
    if (inputText.value === '') {
        message.innerHTML = 'Favor de proporcionar el código del producto';
        message.style.display = 'block';
        return false;
    }
    getProductByCode();
}
function payMode() {
    if (!validateSale()) { return; }
    resetMode('payButton');
    inputText.setAttribute('type', 'number');
    inputText.dataset.input = 'importe';
    icon.classList.remove('fa-barcode', 'fa-search', 'fa-cash-register', 'fa-file-invoice-dollar');
    icon.classList.add('fa-dollar-sign');
    inputText.focus();
}
function paySale(e) {
    e.preventDefault();
    if (inputText.value === '') {
        message.innerHTML = 'Favor de proporcionar el importe.';
        message.style.display = 'block';
        return false;
    }
    $.ajax({
        url: urlSetSale,
        method: 'POST',
        datatype: 'text',
        data: {
            id: ventaId.value,
            importe: parseFloat(inputText.value)
        },
        success: function (result) {
            if (result.error !== undefined && result.error !== null && result.error) {
                if (result.reiniciar) {
                    window.location.href = urlMovement;
                } else {
                    message.innerHTML = result.estatus;
                    message.style.display = 'block';
                }
            } else {
                resetSale(result);
            }
        },
        error: function (r) {
            inputText.value = '';
            message.innerHTML = 'Venta no realizada...';
            message.style.display = 'block';
        },
        cache: false
    });
}
function resetMode(button) {
    inputText.value = '';
    message.innerHTML = '';
    message.style.display = 'none';
    let elementSelectedButton = document.querySelector('.selected');
    elementSelectedButton.classList.remove('bg-base', 'selected', 'text-white');
    elementSelectedButton.classList.add('bg-white');
    let elementButton = document.getElementById(button);
    elementButton.classList.remove('bg-white');
    elementButton.classList.add('bg-base', 'selected', 'text-white');
    inputText.setAttribute('placeholder', 'Contraseña del usuario.');
    switch (button) {
        case 'closeSalesButton':
            inputText.setAttribute('placeholder', 'Contraseña del usuario.');
            break;
        case 'payButton':
            inputText.setAttribute('placeholder', 'Importe');
            break;
        case 'cancelSaleButton':
        case 'saleButton':
        case 'getItBackSaleButton':
            inputText.setAttribute('placeholder', 'Código del producto.');
            break;
        case 'searchButton':
            inputText.setAttribute('placeholder', 'Código o descripción del producto.');
            break;
        case 'withdrawCashButton':
            inputText.setAttribute('placeholder', 'Cantidad * Denominación.');
            break;
    }
}
function resetSale(r) {
    let modalHelpLabel = document.getElementById('ModalHelpLabel');
    let myModal = new bootstrap.Modal(document.getElementById('ModalHelp'), {
        keyboard: true
    })
    myModal.show(modalHelp);
    modalHelpLabel.innerHTML = 'Ventas';
    modalHelp.querySelectorAll('div.modal-dialog')[0].classList.remove('modal-lg');
    modalHelp.querySelectorAll('div.modal-dialog')[0].classList.add('modal-md');
    modalHelp.querySelectorAll('div.modal-body')[0].innerHTML = r;
}
function saveSale(e) {
    e.preventDefault();
    addProcessWithSpinner('Guardando venta...');
    $.ajax({
        url: urlSaveSale,
        method: 'POST',
        datatype: 'text',
        data: { id: ventaId.value },
        success: function (result) {
            if (result.error !== undefined && result.error !== null && result.error) {
                if (result.reiniciar) {
                    window.location.href = urlMovement;
                } else {
                    message.innerHTML = result.estatus;
                    message.style.display = 'block';
                    removeProcessWithSpinner();
                    bsOffcanvas.hide();
                    inputText.focus();
                }
            } else {
                window.location.href = urlSale;
            }
        },
        error: function (r) {
            alert(r);
            removeProcessWithSpinner();
        },
        cache: false
    });
}
function saleMode() {
    if (inputText.dataset.input === 'retiroEfectivo') {
        var box = document.getElementById('datos');
        if (box.children[0].children !== undefined && box.children[0].children.length > 0) {
            message.innerHTML = 'Opción no aprobada, retiro de efectivo en proceso.'
            message.style.display = 'block';
            return false;
        }
    }
    resetMode('saleButton');
    cant = '1';
    inputText.setAttribute('type', 'text');
    inputText.value = '';
    inputText.dataset.input = 'codigo';
    icon.classList.remove('fa-dollar-sign', 'fa-search', 'fa-cash-register', 'fa-file-invoice-dollar');
    icon.classList.add('fa-barcode');
    message.innerHTML = '';
    message.style.display = 'none';
    inputText.focus();
}
function screenResize() {
    var height = $(window).height();
    var heightH = $('header').height();
    var heightF = $('#footer').height();

    $('section').height(height - heightH - heightF - 1);
}
function searchMode() {
    resetMode('searchButton');
    inputText.setAttribute('type', 'text');
    inputText.dataset.input = 'buscar';
    icon.classList.remove('fa-barcode', 'fa-dollar-sign', 'fa-cash-register', 'fa-file-invoice-dollar');
    icon.classList.add('fa-search');
    inputText.focus();
}
function searchProduct() {
    $.ajax({
        url: urlProductList,
        method: 'POST',
        datatype: 'text',
        data: {
            filtro: {
                Patron: inputText.value,
                Skip: 0
            },
            mostrarPrecioVenta: true
        },
        success: function (r) {
            if (r !== null && r.trim() !== '') {
                var elementNode = document.createElement('div');
                elementNode.id = 'dRowProducto';
                elementNode.setAttribute('data-charging', '0');
                elementNode.setAttribute('data-rows-next', '1');
                elementNode.innerHTML = r;
                let myOffCanvasLabel = document.getElementById('offcanvasRightLabel');
                myOffCanvasLabel.innerHTML = "Productos";
                let myOffCanvasBody = document.getElementsByClassName('offcanvas-body');
                myOffCanvasBody[0].innerHTML = '';
                myOffCanvasBody[0].appendChild(elementNode);
                bsOffcanvas.show();
            } else {
                message.innerHTML = 'Código inexistente...';
                message.style.display = 'block';
            }
            inputText.value = '';
            removeProcessWithSpinnerInList('SpinList', 'fa-search');
        },
        error: function (r) {
            removeProcessWithSpinnerInList('SpinList', 'fa-search');
        },
        cache: false
    });
}
function setInputText(e) {
    e.preventDefault();
    switch (inputText.dataset.input) {
        case 'buscar':
            getProduct(inputText.value, true);
            break;
        case 'codigo':
            makeSale(e);
            break;
        case 'corteCaja':
            //closeSale(e);
            alert('Corte de caja');
            break;
        case 'importe':
            paySale(e);
            break;
        case 'retiroEfectivo':
            withdrawCashSale();
            break;
    }
}
function validateSale(e) {
    var box = document.getElementById('datos');
    if (box.children[0].children !== undefined && box.children[0].children.length > 0) {
        message.innerHTML = "";
        message.style.display = 'none';
        return true;
    }
    message.innerHTML = "No hay registros de ventas para procesar";
    message.style.display = 'block';
    return false;
}
function withdrawCashMode(e) {
    e.preventDefault();
    if (inputText.dataset.input === 'codigo') {
        var box = document.getElementById('datos');
        if (box.children[0].children !== undefined && box.children[0].children.length > 0) {
            message.innerHTML = 'Opción no aprobada, venta en proceso.'
            message.style.display = 'block';
            return false;
        }
    }
    resetMode('withdrawCashButton');
    inputText.setAttribute('type', 'number');
    inputText.dataset.input = 'retiroEfectivo';
    icon.classList.remove('fa-barcode', 'fa-search', 'fa-dollar-sign', 'fa-file-invoice-dollar');
    icon.classList.add('fa-cash-register');
    inputText.focus();
}
function withdrawCashRemove(e) {
    e.currentTarget.parentNode.parentNode.parentNode.remove();
    getTotalSum();
}
function withdrawCashSale() {
    if (inputText.value === '' || cant <= 0 || inputText.value <= 0) {
        message.innerHTML = 'Cantidad incorrecta.';
        message.style.display = 'block';
    } else {
        var elementNode = document.createElement('div');
        elementNode.classList.add('bg-white', 'list-group-item', 'list-group-item-action')
        let importe = cant * inputText.value;
        var html = '<div class="row p-3">';
        html += '<div class="col-6"><h4>' + cant + ' x ' + formatCurrency(inputText.value) + '</h4></div>';
        html += '<div class="col-5 text-end saleAmount" data-importe="' + importe + '"><h4>' + formatCurrency(importe) + '</h4></div>';
        html += '<div class="col-1 text-end"><a href="#" class="fas fa-times text-danger" onclick="withdrawCashRemove(event);"></a></div>';
        html += '</div>';
        elementNode.innerHTML = html;
        var box = document.getElementById('datos');
        box.children[0].appendChild(elementNode);
        box.scrollTop = box.scrollHeight;
        message.innerHTML = '';
        message.style.display = 'none';
        getTotalSum();
    }
    cant = '1';
    inputText.value = '';
}
