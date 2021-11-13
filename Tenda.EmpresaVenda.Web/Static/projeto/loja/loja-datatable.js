DataTableApp.controller('LojasTable', LojasTable);

function LojasTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.Loja.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.Loja.Tabela;
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '15%'),
        DTColumnBuilder.newColumn('NomeFantasia').withTitle(Europa.i18n.Messages.NomeFantasia).withOption('width', '15%'),
        DTColumnBuilder.newColumn('SapId').withTitle(Europa.i18n.Messages.IdSap).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption('width', '15%').renderWith(renderRegional),
        DTColumnBuilder.newColumn('DataIntegracao').withTitle(Europa.i18n.Messages.DataIntegracao).withOption('width', '15%').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('width', '15%').withOption('type', 'enum-format-Situacao')

    ])
        .setIdAreaHeader("datatable_header")
        .setAutoInit()
        .setDefaultOptions('POST', Europa.Controllers.Loja.UrlListar, Europa.Controllers.Loja.FilterParams);
}



function renderRegional(data, type, full, meta) {
    return JSON.stringify(full.Regional.Nome).replace('"', '').replace('"','');
    //return meta.Regional.Nome;
}

Europa.Controllers.Loja.FilterParams = function () {
    var filtro = {
        Nome: $('#filtro_nome').val(),
        SapId: $('#filtro_id_sap').val(),
        IdsSituacoes: $('#filtro_situacao').val(),
        DataIntegracaoDe: $('#data_integracao_de').val(),
        DataIntegracaoAte: $('#data_integracao_ate').val(),
        Regional: $('#autocomplete_regional').val()
    };
    return filtro;
};

Europa.Controllers.Loja.FiltrarTabela = function () {
    Europa.Controllers.Loja.Tabela.reloadData();
};

Europa.Controllers.Loja.LimparFiltro = function () {
    $('#filtro_nome').val("");
    $('#filtro_id_sap').val("");
    $('#autocomplete_regional').val("");
    $('#data_integracao_de').val("");
    $('#data_integracao_ate').val("");
    $('#filtro_situacao').val(1).trigger('change');
};

Europa.Controllers.Loja.ExportarTodos = function () {
    var params = Europa.Controllers.Loja.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.Loja.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.Loja.ExportarPagina = function () {
    var params = Europa.Controllers.Loja.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.Loja.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.Loja.IntegrarLoja = function () {
    $.post(Europa.Controllers.Loja.UrlIntegrarCentralImobiliariaSuat, function (res) {
        Europa.Informacao.Hide = function () {
            location.reload(true);
        };
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, res.Mensagens.join("<br/>"));
        Europa.Informacao.Show();
    });
};