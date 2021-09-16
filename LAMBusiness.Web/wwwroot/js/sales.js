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

screenResize();
resetMode('saleButton');
window.addEventListener('resize', function () {
    screenResize();
});

document.getElementById('inputText').addEventListener('keydown', function (e) {
    switch (e.which) {
        case 13: //enter
            setInputText(e);
            break;
        case 27: //*
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
        case 116: //F5
            e.preventDefault();
            payMode();
            break;
    }
});

let modalElement = document.getElementById('ModalHelp');
modalElement.addEventListener('hidden.bs.modal', function (event) {
    inputText.focus();
})

function addProduct(){
    let product = document.querySelector('li.product');
    inputText.value = product.dataset.code;
    getProductByCode();
    bsOffcanvas.hide();
}
function getProduct(id, code = false) {
    addProcessWithSpinnerInList('SpinList', 'fa-search');
    var url = urlProductDetail;
    if (code) {
        url = urlProductDetailByCode;
    }
    if (id === '') {
        messageLabel.innerHTML = 'Favor de Urlrllroporcionar el código del producto';
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
    document.getElementById('total-sm').innerHTML = formatCurrency(total);
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
function saleMode() {
    resetMode('saleButton');
    cant = '1';
    inputText.setAttribute('type', 'text');
    inputText.value = '';
    inputText.dataset.input = 'codigo';
    icon.classList.remove('fa-dollar-sign', 'fa-search');
    icon.classList.add('fa-barcode');
    message.innerHTML = '';
    message.style.display = 'none';
    inputText.focus();
}
function payMode() {
    resetMode('payButton');
    inputText.setAttribute('type', 'number');
    inputText.dataset.input = 'importe';
    icon.classList.remove('fa-barcode', 'fa-search');
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
                window.location.href = urlSale;
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
    let payButton = document.getElementById('payButton');
    let saleButton = document.getElementById('saleButton');
    let searchButton = document.getElementById('searchButton');
    payButton.classList.remove('bg-success');
    saleButton.classList.remove('bg-primary');
    searchButton.classList.remove('bg-warning');
    payButton.classList.remove('text-white');
    saleButton.classList.remove('text-white');
    searchButton.classList.remove('text-white');
    payButton.classList.add('bg-white');
    saleButton.classList.add('bg-white');
    searchButton.classList.add('bg-white');
    switch (button) {
        case 'payButton':
            payButton.classList.remove('bg-white');
            payButton.classList.add('bg-success');
            payButton.classList.add('text-white');
            break;
        case 'saleButton':
            saleButton.classList.remove('bg-white');
            saleButton.classList.add('bg-primary');
            saleButton.classList.add('text-white');
            break;
        case 'searchButton':
            searchButton.classList.remove('bg-white');
            searchButton.classList.add('bg-warning');
            searchButton.classList.add('text-white');
            break;
    }
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
    icon.classList.remove('fa-barcode', 'fa-dollar-sign');
    icon.classList.add('fa-search');
    inputText.focus();
}
function searchProduct() {
    $.ajax({
        url: urlProductList ,
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
                bsOffcanvas.show();
                let myOffCanvasLabel = document.getElementById('offcanvasRightLabel');
                myOffCanvasLabel.innerHTML = "Productos";
                let myOffCanvasBody = document.getElementsByClassName('offcanvas-body');
                myOffCanvasBody[0].innerHTML = r;
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
        case 'importe':
            paySale(e);
            break;
    }
}