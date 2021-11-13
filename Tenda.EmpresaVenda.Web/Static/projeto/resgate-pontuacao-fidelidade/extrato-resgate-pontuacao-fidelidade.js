$(function () {
    
});

function extratoTabela($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.ResgatePontuacaoFidelidade.DatatableExtrato = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.ResgatePontuacaoFidelidade.DatatableExtrato;
    self.setColumns([
        DTColumnBuilder.newColumn('Codigo').withTitle(Europa.i18n.Messages.Codigo).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Pontuacao').withTitle(Europa.i18n.Messages.Pontos).withOption('width', '15%'),
        DTColumnBuilder.newColumn('SituacaoResgate').withTitle(Europa.i18n.Messages.Situacao).withOption('type', 'enum-format-SituacaoResgate').withOption('width', '15%'),
        DTColumnBuilder.newColumn('Motivo').withTitle(Europa.i18n.Messages.Motivo).withOption('width', '15%'),
        DTColumnBuilder.newColumn('DataResgate').withTitle(Europa.i18n.Messages.DataSolicitacao).withOption('width', '15%').renderWith(Europa.Date.toGeenDateFormat),
        DTColumnBuilder.newColumn('Voucher').withTitle(Europa.i18n.Messages.Voucher).withOption('width', '15%'),
        DTColumnBuilder.newColumn('DataLiberacao').withTitle(Europa.i18n.Messages.DataLiberacao).withOption('width', '50px').renderWith(Europa.Date.toGeenDateFormat),
    ])
        .setAutoInit(false)
        .setDefaultOrder([[3, 'asc']])
        .setIdAreaHeader("extrato_datatable_barra")
        .setDefaultOptions('POST', Europa.Controllers.ResgatePontuacaoFidelidade.Url.ListarResgate, Europa.Controllers.ResgatePontuacaoFidelidade.Filtro);

    function actionsHtml(data, type, full, meta) {
        var content = "<div>";

        content = content + "</div>";
        return content;
    };
}

DataTableApp.controller('ExtratoDatatable', extratoTabela);

Europa.Controllers.ResgatePontuacaoFidelidade.Filtrar = function () {
    var autorizar = Europa.Controllers.ResgatePontuacaoFidelidade.ValidarFiltro();
    var msgs = [];

    $("#btn_solicitar_resgate").addClass("hidden");

    if (!autorizar) {

        msgs.push("Campo [Empresa de Vendas] Obrigatório");

        var res = {
            Sucesso: false,
            Mensagens: msgs
        };

        Europa.Informacao.PosAcao(res);
        return;
    }

    Europa.Controllers.ResgatePontuacaoFidelidade.DatatableResgatePontuacaoFidelidade.reloadData();
    Europa.Controllers.ResgatePontuacaoFidelidade.DatatableExtrato.reloadData();

    Europa.Controllers.ResgatePontuacaoFidelidade.PreencherTotais();
};

Europa.Controllers.ResgatePontuacaoFidelidade.ExportarTodosExtrato = function () {

    var autorizar = Europa.Controllers.ResgatePontuacaoFidelidade.ValidarFiltro();
    var msgs = [];

    $("#btn_solicitar_resgate").addClass("hidden");

    if (!autorizar) {

        msgs.push("Campo [Empresa de Vendas] Obrigatório");

        var res = {
            Sucesso: false,
            Mensagens: msgs
        };

        Europa.Informacao.PosAcao(res);
        return;
    }

    var params = Europa.Controllers.ResgatePontuacaoFidelidade.DatatableExtrato.lastRequestParams;
    var formDownload = $("#Exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.ResgatePontuacaoFidelidade.Url.ExportarTodosExtrato);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
}

Europa.Controllers.ResgatePontuacaoFidelidade.ExportarPaginaExtrato = function () {

    var autorizar = Europa.Controllers.ResgatePontuacaoFidelidade.ValidarFiltro();
    var msgs = [];

    $("#btn_solicitar_resgate").addClass("hidden");

    if (!autorizar) {

        msgs.push("Campo [Empresa de Vendas] Obrigatório");

        var res = {
            Sucesso: false,
            Mensagens: msgs
        };

        Europa.Informacao.PosAcao(res);
        return;
    }

    var params = Europa.Controllers.ResgatePontuacaoFidelidade.DatatableExtrato.lastRequestParams;
    var formDownload = $("#Exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.ResgatePontuacaoFidelidade.Url.ExportarPaginaExtrato);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
}