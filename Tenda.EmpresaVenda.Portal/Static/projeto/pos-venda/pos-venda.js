Europa.Controllers.PosVenda = {
    Url: {
        BuscarDadosGraficos: ""
    },
    Data: undefined,
    AreaChart: undefined,
    RadialChart: undefined,
    PieChart: undefined,
    PieChartColors: ["#ff5660", "#feca34", "#79df0b", "#dddddd",],
    Filtro: {
        DataInicio: "",
        DataTermino: "",
    }
};

$(function () {

    Europa.Controllers.PosVenda.Filtro.DataInicio = $("#DataInicioBusca").val();
    Europa.Controllers.PosVenda.Filtro.DataTermino = $("#DataTerminoBusca").val();
    Europa.Controllers.PosVenda.RenderPieChart();
    Europa.Controllers.PosVenda.RenderAreaChart();
    Europa.Controllers.PosVenda.BuscarDados();   

    Europa.Controllers.PosVenda.OnChangeData("DataInicioBusca", "DataInicio");
    Europa.Controllers.PosVenda.OnChangeData("DataTerminoBusca", "DataTermino");

    Europa.Controllers.PosVenda.ResetButtonsChart();
    
});

Europa.Controllers.PosVenda.ResetButtonsChart = function () {
    Europa.Controllers.PosVenda.ChartButton1 = false;
    Europa.Controllers.PosVenda.ChartButton2 = false;
    Europa.Controllers.PosVenda.ChartButton3 = false;
};
////////////// Api Requests //////////////

Europa.Controllers.PosVenda.BuscarDados = function () {
    var filtro = {
        Inicio: Europa.Controllers.PosVenda.Filtro.DataInicio,
        Termino: Europa.Controllers.PosVenda.Filtro.DataTermino,
        SituacaoContrato: $("#situacaoContrato").val()
    };
    $.post(Europa.Controllers.PosVenda.Url.BuscarDadosGraficos, filtro, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.PosVenda.Data = res.Objeto;
            Europa.Controllers.PosVenda.UpdateView();
        } else {
            Europa.Messages.ShowMessages(res, Europa.i18n.Messages.Erro);
        }
    });
};

Europa.Controllers.PosVenda.UpdateView = function () {
    Europa.Controllers.PosVenda.UpdateChartsData();
    Europa.Controllers.PosVenda.UpdateValue("#totalKitCompleto", ".oval-kit-completo","#c3d8f0" ,Europa.Controllers.PosVenda.Data.TotalKitCompleto);
    Europa.Controllers.PosVenda.UpdateValue("#totalRepasse", ".oval-repasse","#bddeda" ,Europa.Controllers.PosVenda.Data.TotalRepasse);
    Europa.Controllers.PosVenda.UpdateValue("#totalConformidade", ".oval-conformidade","#cfd4f8" ,Europa.Controllers.PosVenda.Data.TotalConformidade);
    
    var prePropostasFinalizadas = Europa.Controllers.PosVenda.Data.PrePropostasFinalizadas;
    $(".btn-sem-docs-avalista").attr("disabled", prePropostasFinalizadas[0] === 0);
    $(".btn-docs-avalista-enviados").attr("disabled", prePropostasFinalizadas[1] === 0);
    $(".btn-tcd-assinado").attr("disabled", prePropostasFinalizadas[2] === 0);
};

Europa.Controllers.PosVenda.UpdateValue = function (idTxt,idClass,cor, valor) {
    var valorTxt = valor;
    $(idClass).css("background-color", cor);
    if (valor === undefined || valor == 0) {
        valorTxt = 0;
        $(idClass).css("background-color", "var(--duck-egg-blue)");
    } 
    $(idTxt).html(valorTxt);
}

Europa.Controllers.PosVenda.UpdatePercentage = function (idTxt, idProgress, porcentagem) {
    var porcentagemProgress = porcentagem;
    var porcentagemTxt = porcentagem + "%";
    if (porcentagem > 100) {
        porcentagemProgress = 100;
    } else if (porcentagem < 0) {
        porcentagemProgress *= -1;
    }
    if (porcentagem > 0) {
        porcentagemTxt = "+" + porcentagemTxt;
    }
    $(idTxt).html(porcentagemTxt);
    $(idProgress).attr('aria-valuenow', porcentagemTxt).css('width', porcentagemProgress + "%");
};

////////////// Charts //////////////

Europa.Controllers.PosVenda.UpdateChartsData = function () {
    var data = Europa.Controllers.PosVenda.Data;
    if (Europa.Controllers.PosVenda.PieChart) {
        var dataset = Europa.Controllers.PosVenda.PieChart.config.data.datasets[0];
        dataset.data = data.PrePropostasFinalizadas;
        Europa.Controllers.PosVenda.PieChart.update();
    }
    if (Europa.Controllers.PosVenda.PieChart) {
        var dataset = Europa.Controllers.PosVenda.AreaChart.config.data.datasets[0];
        dataset.data = data.PosVendasConformidade;
        var max = 0;
        data.PosVendasConformidade.forEach(function (value) {
            if (value > max) {
                max = value;
            }
        });
        Europa.Controllers.PosVenda.AreaChart.config.data.labels = data.DiasPosVendasConformidade;
        Europa.Controllers.PosVenda.AreaChart.config.options.scales.yAxes[0].ticks.suggestedMax = max + 2;
        Europa.Controllers.PosVenda.AreaChart.update();
    }
};

////////////// Radial/Pie Chart //////////////

Europa.Controllers.PosVenda.RenderRadialBarTeste = function () {
    var data = [{
        value: 62,
        color: '#feca34',
        title: 'Docs enviados',
        maxValue: 163,
    }, {
        value: 47,
        color: '#79df0b',
        title: 'TCD assinado',
        maxValue: 163,
    }, {
        value: 54,
        color: '#ff5660',
        title: 'Sem docs',
        maxValue: 163,
    },];
    Europa.Controllers.PosVenda.PrePropostaFinalizadaRadialChart = new Europa.Components.CircularProgress()
        .WithData(data)
        .WithTargetSuffix("posVendaPieChart")
        .WithClockwise(false)
        .WithAnimation(true)
        .WithStrokeWidth(14)
        .Configure();
};

Europa.Controllers.PosVenda.RenderPieChart = function () {
    var canvas = $("#progress_posVendaPieChart")[0];
    var labels = [Europa.i18n.Messages.SemDocs, Europa.i18n.Messages.DocsEnviados, Europa.i18n.Messages.AvalistaPreAprovadoC,];
    var colors = Europa.Controllers.PosVenda.PieChartColors;
    var unfocusedColor = "#dddddd";
    var data = [];

    Europa.Controllers.PosVenda.PieChart = new Chart(canvas, {
        type: 'PrePropostasFinalizadasDoughnutChart',
        data: {
            labels: labels,
            datasets: [
                {
                    backgroundColor: colors,
                    unfocusedColor: unfocusedColor,
                    data: data,
                },
            ]
        },
        options: {
            maintainAspectRatio: false,
            layout: {
                padding: {
                    right: 0,
                    top: 0,
                    bottom: 0,
                    left: 0,
                }
            },
            legend: {
                display: false
            },
            tooltips: {
                enabled: false,
            },
        },
    });
};

Europa.Controllers.PosVenda.UpdatePieChartSelection = function (index) {
    if (Europa.Controllers.PosVenda.PieChart) {
        if (Europa.Controllers.PosVenda.PieChart.selectedIndex === index) {
            Europa.Controllers.PosVenda.PieChart.selectedIndex = null;
            Europa.Controllers.PosVenda.PieChart.config.data.datasets[0].backgroundColor = Europa.Controllers.PosVenda.PieChartColors;
        } else {
            Europa.Controllers.PosVenda.PieChart.selectedIndex = index;
            var colors = [];
            for(var i = 0; i < Europa.Controllers.PosVenda.PieChartColors.length; i++){
                if(i === index){
                    colors.push(Europa.Controllers.PosVenda.PieChartColors[i]);
                }else{
                    colors.push("#dddddd");
                }
            }
            Europa.Controllers.PosVenda.PieChart.config.data.datasets[0].backgroundColor = colors;
        }
        Europa.Controllers.PosVenda.PieChart.update();
    }
};

////////////// Area Chart //////////////

Europa.Controllers.PosVenda.RenderAreaChart = function () {
    var canvas = $("#posVendasAreaChart")[0];

    var days = ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'];

    var labels = [];
    var data = [];

    var date = new Date();
    date.setDate(date.getDate() - 7);
    for (var i = 0; i < 10; i++) {
        var dayName = days[date.getDay()];
        labels.push(dayName);
        date.setDate(date.getDate() + 1);
    }

    var lineGradient = canvas.getContext("2d").createLinearGradient(0, 107, 0, 195);
    lineGradient.addColorStop(0, "#fad961");
    lineGradient.addColorStop(1, "#f76b1c");

    var backgroundGradient = canvas.getContext("2d").createLinearGradient(0, 107, 0, 195);
    backgroundGradient.addColorStop(0, "rgba(250,217,97,0.07)");
    backgroundGradient.addColorStop(1, "rgba(247,107,28,0.07)");

    var xAxisLineGradient = canvas.getContext("2d").createLinearGradient(0, 0, 0, 205);
    xAxisLineGradient.addColorStop(0, "rgba(255,255,255,0.09)");
    xAxisLineGradient.addColorStop(0.4, "rgba(255,215,108,0.25)");
    xAxisLineGradient.addColorStop(1, "rgba(247,107,28,0.25)");

    Europa.Controllers.PosVenda.AreaChart = new Chart(canvas, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [
                {
                    data: data,
                    lineTension: 0,
                    backgroundColor: backgroundGradient,
                    borderColor: lineGradient,
                    pointBorderColor: lineGradient,
                    pointBackgroundColor: lineGradient,
                    pointHoverBackgroundColor: lineGradient,
                    pointHoverBorderColor: lineGradient,
                    pointRadius: 4,
                    borderWidth: 4,
                    pointHoverBorderWidth: 6,
                    pointBorderWidth: 6,
                    pointHoverRadius: 6,
                    type: 'line',
                },
                {
                    data: [],
                }
            ]
        },
        options: {
            maintainAspectRatio: false,
            layout: {
                padding: {
                    right: 11,
                    top: 0,
                    bottom: 0,
                    left: 0,
                }
            },
            animation: {
                onComplete: function () {
                    if (Europa.Controllers.PosVenda.tooltipRendered) {
                        return;
                    }
                    var dataPosition = Europa.Controllers.PosVenda.AreaChart.getDatasetMeta(0).data.length - 1;
                    if (dataPosition < 0) return;
                    Europa.Controllers.PosVenda._OpenAreaChartTooltip();
                }
            },
            elements: {
                line: {
                    borderWidth: 0,
                    fill: true,
                }
            },
            legend: {
                display: false
            },
            scales: {
                xAxes: [{
                    gridLines: {
                        display: true,
                        color: xAxisLineGradient,
                        drawBorder: false,
                        zeroLineColor: "#00000000",
                    },
                    scaleLabel: {
                        display: false,
                    },
                    ticks: {
                        display: true,
                        fontColor: "#ffc400",
                        fontSize: 12,
                        lineWidth: 2,
                    },
                }],
                yAxes: [
                    {
                        gridLines: {
                            display: false,
                            color: xAxisLineGradient,
                            drawBorder: false,
                        },
                        scaleLabel: {
                            display: false,
                        },
                        ticks: {
                            display: false,
                            suggestedMax: 102,
                            suggestedMin: 0,
                        },
                    }
                ]
            },
            tooltips: {
                enabled: false,
                xPadding: 25,
                yPadding: 10,
                position: 'average',
                mode: 'single',
                custom: function (tooltipModel) {
                    var tooltipEl = $("#prepropostas-finalizadas-tooltip");

                    if (!tooltipModel.dataPoints || tooltipModel.dataPoints[0].opacity === 0) {
                        if (tooltipEl.length > 0) {
                            tooltipEl.css("opacity", "0");
                        }
                        return;
                    }

                    // Create element on first render
                    if (!tooltipEl.length) {
                        $("#areaChartWrapper")
                            .append('<div id="prepropostas-finalizadas-tooltip" class="pos-venda-chart-tooltip"></div>');
                        tooltipEl = $("#areaChartWrapper").find("#prepropostas-finalizadas-tooltip");
                    }

                    tooltipEl.html("");

                    var tooltipElBody = tooltipEl.append("<div class='tooltip-body'></div>").find(".tooltip-body");

                    function getBody(bodyItem) {
                        return bodyItem.lines;
                    }

                    var tooltipContent = tooltipElBody.append("<div class='tooltip-content'></div>").find(".tooltip-content");
                    var tooltipArrow = tooltipElBody.append("<div class='tooltip-arrow'></div>").find(".tooltip-arrow");
                    tooltipArrow.addClass("above");

                    // Set Text
                    if (tooltipModel.body) {
                        var bodyLines = tooltipModel.body.map(getBody);

                        var innerHtml = '';

                        bodyLines.forEach(function (body, i) {
                            if (i > 0) innerHtml += "<br/>";
                            innerHtml += body;
                        });

                        tooltipContent.html("");
                        tooltipContent.append(innerHtml);
                    }

                    var posX = tooltipModel.caretX - (tooltipElBody.width() / 2);
                    var posY = tooltipModel.caretY - tooltipElBody.height() - 15;

                    tooltipEl.css("opacity", 1);
                    tooltipEl.css("top", posY + 'px');
                    tooltipEl.css("left", posX + 'px');
                    tooltipEl.css("pointerEvents", 'none');
                }
            }
        }
    });
};

Europa.Controllers.PosVenda._OpenAreaChartTooltip = function () {
    var chart = Europa.Controllers.PosVenda.AreaChart;
    if (chart.tooltip._active == undefined)
        chart.tooltip._active = [];
    var activeElements = chart.tooltip._active;
    var dataPosition = Europa.Controllers.PosVenda.AreaChart.getDatasetMeta(0).data.length - 1;
    if (dataPosition < 0) return;
    var requestedElem = chart.getDatasetMeta(0).data[dataPosition];
    for (var i = 0; i < activeElements.length; i++) {
        if (requestedElem._index == activeElements[i]._index)
            return;
    }
    activeElements.push(requestedElem);
    chart.tooltip._active = activeElements;
    chart.tooltip.update(true);
    chart.draw();
    Europa.Controllers.PosVenda.tooltipRendered = true;
};

////////////// Actions //////////////

Europa.Controllers.PosVenda.OnSemDocsAvalistaClicked = function () {
    if (Europa.Controllers.PosVenda.PrePropostaFinalizadaRadialChart) {
        Europa.Controllers.PosVenda.PrePropostaFinalizadaRadialChart.SetFocusIndex(2);
    }

    if (Europa.Controllers.PosVenda.ChartButton1) {
        $("#TipoFiltroPosVenda").val(0);
        Europa.Controllers.PosVenda.ChartButton1 = false;
    }
    else {
        $("#TipoFiltroPosVenda").val(1);
        Europa.Controllers.PosVenda.ChartButton1 = true;
        Europa.Controllers.PosVenda.ChartButton2 = false;
        Europa.Controllers.PosVenda.ChartButton3 = false;
    }

    Europa.Controllers.PosVenda.UpdatePieChartSelection(0);
    Europa.Controllers.PosVenda.Tabela.reloadData();
};

Europa.Controllers.PosVenda.OnDocsAvalistaEnviadosClicked = function () {
    if (Europa.Controllers.PosVenda.PrePropostaFinalizadaRadialChart) {
        Europa.Controllers.PosVenda.PrePropostaFinalizadaRadialChart.SetFocusIndex(0);
    }

    if (Europa.Controllers.PosVenda.ChartButton2) {
        $("#TipoFiltroPosVenda").val(0);
        Europa.Controllers.PosVenda.ChartButton2 = false;
    }
    else {
        $("#TipoFiltroPosVenda").val(2);
        Europa.Controllers.PosVenda.ChartButton1 = false;
        Europa.Controllers.PosVenda.ChartButton2 = true;
        Europa.Controllers.PosVenda.ChartButton3 = false;
    }

    Europa.Controllers.PosVenda.UpdatePieChartSelection(1);
    Europa.Controllers.PosVenda.Tabela.reloadData();
};

Europa.Controllers.PosVenda.OnTCDAssinadoClicked = function () {
    if (Europa.Controllers.PosVenda.PrePropostaFinalizadaRadialChart) {
        Europa.Controllers.PosVenda.PrePropostaFinalizadaRadialChart.SetFocusIndex(1);
    }
    if (Europa.Controllers.PosVenda.ChartButton3) {
        $("#TipoFiltroPosVenda").val(0);
        Europa.Controllers.PosVenda.ChartButton3 = false;
    }
    else {
        $("#TipoFiltroPosVenda").val(3);
        Europa.Controllers.PosVenda.ChartButton1 = false;
        Europa.Controllers.PosVenda.ChartButton2 = false;
        Europa.Controllers.PosVenda.ChartButton3 = true;
    }
    Europa.Controllers.PosVenda.UpdatePieChartSelection(2);
    Europa.Controllers.PosVenda.Tabela.reloadData();
};

Europa.Controllers.PosVenda.OnToggleSelectDateDialogClicked = function () {
    if ($("#dateFilterPopup").css("display") === "block") {
        $("#dateFilterPopup").css("display", "none");
        return;
    }
    var buttonOffset = $('#buttonOpenDateFilter').offset();
    var posTop = $('#buttonOpenDateFilter').height() + 5;
    var posRight = 0;
    
    $("#dateFilterPopup").css("top", posTop + "px");
    $("#dateFilterPopup").css("right", posRight + "px");
    $("#dateFilterPopup").css("display", "block");
};

Europa.Controllers.PosVenda.OnDateFilterSelected = function () {
    Europa.Controllers.PosVenda.ResetButtonsChart();
    $("#TipoFiltroPosVenda").val("");
    var dataInicio = $("#DataInicioBusca").val();
    var dataTermino = $("#DataTerminoBusca").val();

    var parseDate = function (inputId, value) {
        var dateSplit = value.split("/");
        var date = null;
        if (dateSplit.length === 3 && value.length === 10) {
            var dateStr = dateSplit[1] + "/" + dateSplit[0] + "/" + dateSplit[2];
            date = new Date(dateStr);
            if (date == 'Invalid Date') {
                date = null;
            }
        }

        if (date === null) {
            $(inputId).addClass("filtro-data-error");
        } else {
            $(inputId).removeClass("filtro-data-error");
        }

        return date;
    };

    var dateBegin = parseDate("#filtroDataInicio", dataInicio);
    var dateEnd = parseDate("#filtroDataTermino", dataTermino);

    if (!dateBegin || !dateEnd) return;
    if (dateBegin > dateEnd) {
        $("#filtroDataTermino").addClass("filtro-data-error");
        return;
    } else {
        $("#filtroDataTermino").removeClass("filtro-data-error");
    }

    Europa.Controllers.PosVenda.Filtro.DataInicio = dataInicio;
    Europa.Controllers.PosVenda.Filtro.DataTermino = dataTermino;

    $("#dataVariacaoInicio").html(Europa.Controllers.PosVenda.Filtro.DataInicio);
    $("#dataVariacaoTermino").html(Europa.Controllers.PosVenda.Filtro.DataTermino);

    $("#dateFilterPopup").css("display", "none");
    Europa.Controllers.PosVenda.BuscarDados();

    Europa.Controllers.PosVenda.UpdatePieChartSelection(null);
    Europa.Controllers.PosVenda.UpdatePieChartSelection(null);

    $("#DataInicio").val(Europa.Controllers.PosVenda.Filtro.DataInicio);
    Europa.Controllers.PosVenda.Tabela.reloadData();
};

Europa.Controllers.PosVenda.OnChangeData = function (origem, destino) {
    $("#" + destino).val($("#" + origem).val());
}