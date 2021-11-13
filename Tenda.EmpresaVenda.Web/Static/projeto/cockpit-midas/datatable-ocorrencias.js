$(function () {

});

function TableOcorrencias($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.CockpitMidas.TableOcorrencias = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.CockpitMidas.TableOcorrencias;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('IdOcorrencia').withTitle(Europa.i18n.Messages.Ocorrencia).withOption("width", "100px"),
            DTColumnBuilder.newColumn('CNPJTomador').withTitle(Europa.i18n.Messages.CNPJTomador).withOption("width", "140px").renderWith(formatCNPJ),
            DTColumnBuilder.newColumn('CNPJPrestador').withTitle(Europa.i18n.Messages.CNPJPrestador).withOption("width", "140px").renderWith(formatCNPJ),
            DTColumnBuilder.newColumn('NfeNumber').withTitle(Europa.i18n.Messages.NotaFiscal).withOption("width", "100px"),
            DTColumnBuilder.newColumn('Match').withTitle(Europa.i18n.Messages.Match).withOption("width", "100px").renderWith(Europa.String.FormatBoolean),
        ])
        .setAutoInit(true)
        .setIdAreaHeader("ocorrencias_datatable_header")
        .setOptionsSelect('POST', Europa.Controllers.CockpitMidas.UrlListarOcorrencias, Europa.Controllers.CockpitMidas.FiltroOcorrencias);

    function formatCNPJ(data) {
        if (data != null) {
            return Europa.Mask.GetMaskedValue(data, Europa.Mask.FORMAT_CNPJ);
        }
        return "";
    }
    
};

DataTableApp.controller('ocorrenciasTable', TableOcorrencias);

Europa.Controllers.CockpitMidas.FiltroOcorrencias = function () {

    var match = null;

    if ($('#filtro_match').val() == 1) {
        match = true;
    }
    if ($('#filtro_match').val() == 2) {
        match = false;
    }

    var param = {
        Ocorrencia: $('#filtro_ocorrencia').val(),
        Match: match,
        CNPJTomador: $('#filtro_cnpj_tomador').val(),
        CNPJPrestador: $('#filtro_cnpj_prestador').val(),
        IdsEmpresaVenda: $("#autocomplete_empresa_venda").val(),
        NumeroNotaFiscal: $("#numero_nf").val(),
        Estado: $("#estado").val(),
        PreProposta: $('#numero_preproposta').val(),
        NumeroPedido: $("#numero_pedido").val(),
        DataPrevisaoPagamentoInicio: $("#data_previsao_pagamento_inicio").val(),
        DataPrevisaoPagamentoTermino: $("#data_previsao_pagamento_termino").val()
    };
    return param;
};

Europa.Controllers.CockpitMidas.FiltrarTabelaOcorrencias = function () {
    Europa.Controllers.CockpitMidas.TableOcorrencias.reloadData();
}

Europa.Controllers.CockpitMidas.ExportarTodosOcorrencias = function () {
    var params = Europa.Controllers.CockpitMidas.FiltroOcorrencias();
    params.order = Europa.Controllers.CockpitMidas.TableOcorrencias.lastRequestParams.order;
    params.draw = Europa.Controllers.CockpitMidas.TableOcorrencias.lastRequestParams.draw;
    var formExportar = $("#ExportarOcorrencias");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.CockpitMidas.UrlExportarTodosOcorrencias);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.CockpitMidas.ExportarPaginaOcorrencias = function () {
    var params = Europa.Controllers.CockpitMidas.FiltroOcorrencias();
    params.order = Europa.Controllers.CockpitMidas.TableOcorrencias.lastRequestParams.order;
    params.draw = Europa.Controllers.CockpitMidas.TableOcorrencias.lastRequestParams.draw;
    params.pageSize = Europa.Controllers.CockpitMidas.TableOcorrencias.lastRequestParams.pageSize;
    params.start = Europa.Controllers.CockpitMidas.TableOcorrencias.lastRequestParams.start;
    var formExportar = $("#ExportarOcorrencias");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.CockpitMidas.UrlExportarPaginaOcorrencias);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

