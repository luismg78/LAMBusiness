function addProcess(text, tiempo = 0, fondo = 'bg-transparent-dark') {
    if ($("#loader > div")[0] === undefined) {
        $("#loader").removeClass('d-none');
        var html = "" +
            '<div class="bg-fixed ' + fondo + ' bg-disabled w-100">' +
            '	<div class="mx-auto position-relative text-center w-50" style="top:10rem;">' +
            '		<div class="loader">' +
            '           <div class="item-1"></div>' +
            '           <div class="item-2"></div>' +
            '           <div class="item-3"></div>' +
            '           <div class="item-4"></div>' +
            '           <div class="item-5"></div>' +
            '       </div>' +
            '		<h6 id="ProcesoText" class="mt-5 text-white">' + text + '</h6>' +
            '	</div>' +
            '</div>';
        $("#loader").append(html);
        if (tiempo > 0) {
            setTimeout(function () {
                removeProcess();
            }, tiempo);
        }
    }
    $("#inputDisabled").focus();
}

function addProcessWithSpinner(text) {
    if ($(".process-with-spinner")[0] === undefined) {
        var html = '';
        html += '<div class="process-with-spinner">';
        html += '    <div class="spinner"></div>';
        html += '    <div class="ml-3">' + text + '</div>';
        html += '</div>';
        $('body').append(html);
    }
}

function addProcessWithSpinnerInList(obj, icon) {
    obj = "#" + obj;
    if ($(obj)[0] !== undefined) {
        $(obj + " > i").removeClass(icon);
        $(obj + " > i").addClass('spinnerInList');
    }
}

function addButtonWithSpinner(text, container, buttonDisabled, time = 0) {
    if ($(container)[0] !== undefined && $('#button-with-spinner')[0] === undefined) {
        var html = '';
        html += '<a id="button-with-spinner" class="btn btn-secondary text-white">';
        html += '   <div class="row align-items-center">';
        html += '       <div class="col-1"><div class="spinner"></div></div>';
        html += '       <div class="col-10">'+text+'</div>';
        html += '   </div>';
        html += '</a>';
        $(buttonDisabled).hide();
        $(container).append(html);
        if (time > 0) {
            setTimeout(function () {
                $(buttonDisabled).show();
                $('#button-with-spinner').remove();
            }, time);
        }
        $('#button-with-spinner').focus();
    }
}

function btnProcess(text, button, tiempo = 0) {
    if ($(".btn-process > div")[0] === undefined) {
        if (text !== '') {
            var html = "" +
                '<div class="">' +
                '	<h6 id="ProcesoText" class="text-dark">' + text + '</h6>' +
                '</div>';
            $(".btn-process").append(html);
        }
        $('#' + button).addClass('invisible');
        if (tiempo > 0) {
            setTimeout(function () {
                $(".btn-process > div").remove();
                $('#' + button).removeClass('invisible');
            }, tiempo);
        }
    }
    $("#inputDisabled").focus();
}

function formatCurrency(numero) {
    var valorNegativo = false;
    if (numero < 0) {
        valorNegativo = true;
        numero = Math.abs(numero);
    }
    return (valorNegativo ? '-$' : '$') +
        parseFloat(numero, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,').toString();
}

function removeProcess() {
    $("#loader").addClass('d-none');
    $("#loader > div").remove();
}

function removeProcessWithSpinner() {
    $('.process-with-spinner').remove();
}

function removeProcessWithSpinnerInList(obj, icon) {
    obj = "#" + obj;
    if ($(obj)[0] !== undefined) {
        $(obj + " > i").removeClass('spinnerInList');
        $(obj + " > i").addClass(icon);
    }
}

function removeButtonWithSpinner(buttonEnabled) {
    if ($('#button-with-spinner')[0] !== undefined) {
        $(buttonEnabled).show();
        $('#button-with-spinner').remove();
    }
}

function positionTopScreen(e) {
    $('html, body').animate({ scrollTop: 0 }, 300);
    e.preventDefault();
}


//cursor
function positionCursorWithArrowKey(e) {
    switch (e.keyCode) {
        case 38:
            var $div = $('div.list-registers ul li');
            var $selected = $('div.list-registers ul li.selected');
            $selected.removeClass('selected').prev().addClass('selected');
            var band = true;
            if ($selected.prev().length === 0) {
                if ($selected.index() === 0 && $selected.prev().index() === -1) {
                    e.currentTarget.value = input;
                    band = false;
                } else {
                    $div.eq(-1).addClass('selected')
                }
            }
            if (band) {
                var obj = $('div.list-registers ul li.selected')
                e.currentTarget.value = obj[0].dataset.search;
                obj[0].scrollIntoView(false);
                $(e.currentTarget).focus();
            }
            break;
        case 40:
            var $div = $('div.list-registers ul li');
            var $selected = $('div.list-registers ul li.selected');
            $selected.removeClass('selected').next().addClass('selected');
            var band = true;
            if ($selected.next().length === 0) {
                if ($selected.index() === $div.length - 1 && $selected.next().index() === -1) {
                    e.currentTarget.value = input;
                    band = false;
                } else {
                    $div.eq(0).addClass('selected')
                }
            }
            if (band) {
                var obj = $('div.list-registers ul li.selected')
                e.currentTarget.value = obj[0].dataset.search;
                obj[0].scrollIntoView(false);
                $(e.currentTarget).focus();
            }
            break;
        default:
            if (e.keyCode !== 13) {
                input = e.currentTarget.value;
                $('#dRow > div').remove();
            }
            break;
    }
}

$(document).on('mouseout', 'div.list-registers', function () {
    $('div.list-registers ul li.selected').removeClass('selected');
});

$(document).on('mouseover', 'div.list-registers ul li', function (e) {
    var $selected = $('div.list-registers ul li.selected'), $div = $(e.currentTarget);
    $selected.removeClass('selected');
    $div.eq(-1).addClass('selected');
});

//movimiento

function multiplyCantByPriceWhenChangeValue(precio) {
    var _precio = '#' + precio;
    var importe = parseFloat($('#Cantidad').val()) * parseFloat($(_precio).val());
    if (importe.toString() === 'NaN') {
        $('#Importe')[0].innerText = '$0.00';
    } else {
        $('#Importe')[0].innerText = formatCurrency(importe);
    }
}