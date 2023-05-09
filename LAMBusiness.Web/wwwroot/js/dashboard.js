
function AreaCharts(subtitle) {
    addProcessWithSpinner('Generando estadísticas de movimientos...');
    let datos = [];
    //let datos = [
    //    {
    //        name: "Total Venta 2021",
    //        data: [45.52, 52.3, 38, 45, 19, 23, 2, 56.42, 84.68, 12.57, 96.45, 11]
    //    },
    //    {
    //        name: "Total Venta 2022",
    //        data: [15.2, 12.3, 78, 36, 85, 42, 12, 74.42, 10.68, 86.58, 10.45, 10]
    //    }
    //];
    $.ajax({
        url: urlMovement,
        method: 'POST',
        datatype: 'text',
        success: function (result) {
            if (result.error !== undefined && result.error !== null) {
                if (result.error) {
                    window.location.href = urlMenu;
                } else {
                    datos = result.datos;
                    var options = {
                        chart: {
                            background: '#fff',
                            height: 350,
                            type: "area"
                        },
                        dataLabels: {
                            enabled: false
                        },
                        series: datos,
                        grid: {
                            show: true,
                            strokeDashArray: 0,
                            xaxis: {
                                lines: {
                                    show: true
                                }
                            },
                            yaxis: {
                                lines: {
                                    show: true
                                }
                            }
                        },
                        stroke: {
                            show: true,
                            curve: 'smooth',
                            lineCap: 'round',
                            width: 3
                        },
                        title: {
                            text: 'Estadísticas de Ventas'
                        },
                        subtitle: {
                            text: subtitle,
                            style: {
                                color: '#808080'
                            }
                        },
                        fill: {
                            type: "gradient",
                            gradient: {
                                shadeIntensity: 1,
                                opacityFrom: 0.7,
                                opacityTo: 0.9,
                                stops: [0, 90, 100]
                            }
                        },
                        yaxis: {
                            labels: {
                                formatter: function (value) {
                                    return formatCurrency(value);
                                }
                            },
                        },
                        xaxis: {
                            categories: [
                                "Ene",
                                "Feb",
                                "Mar",
                                "Abr",
                                "May",
                                "Jun",
                                "Jul",
                                "Ago",
                                "Sep",
                                "Oct",
                                "Nov",
                                "Dic"
                            ]
                        }
                    };

                    var chart = new ApexCharts(document.querySelector("#chart"), options);

                    chart.render();
                    removeProcessWithSpinner();
                }
            } else {
                window.location.href = urlMenu;
            }
        },
        error: function (r) {
            alert(r);
            removeProcessWithSpinner();
        },
        cache: false
    });    
}