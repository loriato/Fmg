$(function () {
    $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
    $(".ng-scope").css("word-wrap", "break-word");
    $('#situacao_nota_fiscal').val(["2", "4"]).trigger('change');



    Europa.Controllers.CockpitMidas.InitAutoComplete();
    Europa.Controllers.CockpitMidas.InitDatePicker();

    $("#situacao_nota_fiscal").select2({
        trags: true,
        width: '100%'
    });

    $("#estado").select2({
        trags: true,
        width: '100%'
    });

    $("#filtro_match").prepend("<option value='' selected='selected'>Todos</option>");


    $("#filtro_match").select2({
        trags: true,
        width: '100%',
        minimumResultsForSearch: -1
    });


});

Europa.Controllers.CockpitMidas.InitDatePicker = function () {
    Europa.Controllers.CockpitMidas.DateInicio = new Europa.Components.DatePicker()
        .WithTarget("#data_inicio")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

    Europa.Controllers.CockpitMidas.DataTermino = new Europa.Components.DatePicker()
        .WithTarget("#data_termino")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

    Europa.Controllers.CockpitMidas.DatePrevisaoPagamentoInicio = new Europa.Components.DatePicker()
        .WithTarget("#data_previsao_pagamento_inicio")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

    Europa.Controllers.CockpitMidas.DataPrevisaoPagamentoTermino = new Europa.Components.DatePicker()
        .WithTarget("#data_previsao_pagamento_termino")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();
};


Europa.Controllers.CockpitMidas.InitAutoComplete = function () {
    Europa.Controllers.CockpitMidas.AutoCompleteEmpresaVenda = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("empresa_venda")
        .Configure();
    Europa.Controllers.CockpitMidas.AutoCompleteRegionais = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regionais")
        .Configure();
    console.log(this.param);
    Europa.Controllers.CockpitMidas.AutoCompleteEmpresaVenda.Data = function (params) {
        return {
            start: 0,
            pageSize: 10,
            filter: [
                {
                    value: params.term,
                    column: this.param,
                    regex: true
                },
                {
                    value: function () {
                        return $("#estado").val();
                    },
                    column: 'estado',
                }
            ],
            order: [
                {
                    value: "asc",
                    column: this.param
                }
            ]
        };
    };
    Europa.Controllers.CockpitMidas.AutoCompleteEmpresaVenda.Configure();
};


function TableNotas($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.CockpitMidas.TableNotas = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.CockpitMidas.TableNotas;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('IdOcorrencia').withTitle(Europa.i18n.Messages.Ocorrencia + " Midas").withOption("width", "100px").renderWith(renderOcurrence),
            DTColumnBuilder.newColumn('NFeMidas').withTitle(Europa.i18n.Messages.NumeroNotaFiscalP + " Midas").withOption("width", "100px"),
            DTColumnBuilder.newColumn('NotaFiscal').withTitle(Europa.i18n.Messages.NumeroNotaFiscalP + " Portal").withOption('width', '150px'),
            DTColumnBuilder.newColumn('Match').withTitle(Europa.i18n.Messages.Match).withOption("width", "100px").renderWith(Europa.String.FormatBoolean),
            DTColumnBuilder.newColumn('NomeFantasia').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption("width", "150px"),
            DTColumnBuilder.newColumn('CodigoPreProposta').withTitle(Europa.i18n.Messages.Proposta).withOption('width', '150px'),
            DTColumnBuilder.newColumn('Estado').withTitle(Europa.i18n.Messages.UF).withOption("width", "150px"),
            DTColumnBuilder.newColumn('PedidoSap').withTitle(Europa.i18n.Messages.NumeroPedido).withOption("width", "175px"),
            DTColumnBuilder.newColumn('SituacaoNotaFiscal').withTitle(Europa.i18n.Messages.Situacao).withOption("width", "150px").notSortable().renderWith(renderSituacaoNF),
            DTColumnBuilder.newColumn('Motivo').withTitle(Europa.i18n.Messages.MotivoRecusa).withOption("width", "300px"),
            DTColumnBuilder.newColumn('DataPrevisaoPagamento').withTitle(Europa.i18n.Messages.PrevisaoPagamento).withOption('width', '200px').notSortable().renderWith(Europa.Date.toGeenDateFormat),
            DTColumnBuilder.newColumn('Pago').withTitle(Europa.i18n.Messages.Pago).withOption('width', '100px').notSortable().renderWith(Europa.String.FormatBoolean)
        ])
        .setAutoInit(true)
        .setIdAreaHeader("notas_datatable_header")
        .setOptionsSelect('POST', Europa.Controllers.CockpitMidas.UrlListarNotas, Europa.Controllers.CockpitMidas.Filtro);

    function renderOcurrence(data, type, full, meta) {

        if (data == 0) {
            return "";
        }
        return data;
    }       


    function regraPagamento(value, tipo) {
        if (tipo == 1) {
            return value + "% Kit Completo";
        }
        if (tipo == 2) {
            return value + "% Repasse";
        }
        if (tipo == 3) {
            return value + "% Conformidade";
        }
    }


    function renderSituacaoNF(data, type, full, meta) {

        if (full.PassoAtual == "Prop. Cancelada" || full.EmReversao == true) {
            return "Distratado";
        }
        else if (full.SituacaoNotaFiscal != 0) {
            return Europa.i18n.Enum.Resolve("SituacaoNotaFiscal", full.SituacaoNotaFiscal);
        }
        else {
            return "Pendente de Envio";
        }
    }



};

DataTableApp.controller('notasTable', TableNotas);

Europa.Controllers.CockpitMidas.Filtro = function () {

    var match = null;

    if ($('#filtro_match').val() == 1) {
        match = true;
    }
    if ($('#filtro_match').val() == 2) {
        match = false;
    }

    param = {
        Ocorrencia: $('#filtro_ocorrencia').val(),
        Match: match,
        DataInicio: $("#data_inicio").val(),
        DataTermino: $("#data_termino").val(),
        DataPrevisaoPagamentoInicio: $("#data_previsao_pagamento_inicio").val(),
        DataPrevisaoPagamentoTermino: $("#data_previsao_pagamento_termino").val(),
        IdsEmpresaVenda: $("#autocomplete_empresa_venda").val(),
        Situacoes: $("#situacao_nota_fiscal").val(),
        Estado: $("#estado").val(),
        Regionais: $("#autocomplete_regionais").val(),
        NumeroPedido: $("#numero_pedido").val(),
        NumeroNotaFiscal: $("#numero_nf").val(),
        PreProposta: $('#numero_preproposta').val(),
        CNPJPrestador: $('#filtro_cnpj_prestador').val(),
        CNPJTomador: $('#filtro_cnpj_tomador').val()

    };
    return param;
};

Europa.Controllers.CockpitMidas.FiltrarTabelaNotas = function () {
    Europa.Controllers.CockpitMidas.TableNotas.reloadData();
}

Europa.Controllers.CockpitMidas.ExportarTodosNotas = function () {
    var params = Europa.Controllers.CockpitMidas.Filtro();
    params.order = Europa.Controllers.CockpitMidas.TableNotas.lastRequestParams.order;
    params.draw = Europa.Controllers.CockpitMidas.TableNotas.lastRequestParams.draw;
    var formExportar = $("#ExportarNotas");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.CockpitMidas.UrlExportarTodosNotas);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.CockpitMidas.ExportarPaginaNotas = function () {
    var params = Europa.Controllers.CockpitMidas.FiltroOcorrencias();
    params.order = Europa.Controllers.CockpitMidas.TableNotas.lastRequestParams.order;
    params.draw = Europa.Controllers.CockpitMidas.TableNotas.lastRequestParams.draw;
    params.pageSize = Europa.Controllers.CockpitMidas.TableNotas.lastRequestParams.pageSize;
    params.start = Europa.Controllers.CockpitMidas.TableNotas.lastRequestParams.start;
    var formExportar = $("#ExportarNotas");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.CockpitMidas.UrlExportarPaginaNotas);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};
