$(function () {
    Europa.Controllers.EfetivarSolicitacaoResgate.InitAutoComplete();
});

Europa.Controllers.EfetivarSolicitacaoResgate.InitAutoComplete = function () {
    Europa.Controllers.EfetivarSolicitacaoResgate.AutoCompleteEmpresaVendas = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("empresa_venda")
        .Configure();
}

Europa.Controllers.EfetivarSolicitacaoResgate.Filtro = function () {
    var filtro = {
        IdsEmpresaVenda: $("#autocomplete_empresa_venda").val(),
        SituacaoResgate: $("#SituacaoResgate").val(),
        PeriodoDe: $("#PeriodoDe").val(),
        PeriodoAte: $("#PeriodoAte").val()
    };

    return filtro;
};

Europa.Controllers.EfetivarSolicitacaoResgate.LimparFiltro = function () {
    Europa.Controllers.EfetivarSolicitacaoResgate.AutoCompleteEmpresaVendas.Clean();
    $("#SituacaoResgate").val("").trigger("change");
    $("#PeriodoDe").val("");
    $("#PeriodoAte").val("");
};

function solicitacaoTabela($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.EfetivarSolicitacaoResgate.DatatableSolicitacao = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.EfetivarSolicitacaoResgate.DatatableSolicitacao;
    self.setColumns([
        DTColumnBuilder.newColumn('Codigo').withTitle(Europa.i18n.Messages.Codigo).withOption('width', '50px'),
        DTColumnBuilder.newColumn('NomeEmpresaVenda').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeSolicitadoPor').withTitle(Europa.i18n.Messages.Solicitante).withOption('width', '150px'),
        DTColumnBuilder.newColumn('Pontuacao').withTitle(Europa.i18n.Messages.Pontos).withOption('width', '50px'),
        DTColumnBuilder.newColumn('SituacaoResgate').withTitle(Europa.i18n.Messages.Situacao).withOption('type', 'enum-format-SituacaoResgate').withOption('width', '50px'),
        DTColumnBuilder.newColumn('Motivo').withTitle(Europa.i18n.Messages.Motivo).withOption('width', '200px'),
        DTColumnBuilder.newColumn('DataResgate').withTitle(Europa.i18n.Messages.DataSolicitacao).withOption('width', '50px').renderWith(Europa.Date.toGeenDateFormat),
        DTColumnBuilder.newColumn('Voucher').withTitle(Europa.i18n.Messages.Voucher).withOption('width', '50px'),
        DTColumnBuilder.newColumn('DataLiberacao').withTitle(Europa.i18n.Messages.DataLiberacao).withOption('width', '50px').renderWith(Europa.Date.toGeenDateFormat),

    ])
        .setAutoInit(false)
        .setColActions(actionsHtml,'5%')
        .setDefaultOrder([[1, 'asc']])
        .setIdAreaHeader("solicitacao_datatable_barra")
        .setDefaultOptions('POST', Europa.Controllers.EfetivarSolicitacaoResgate.Url.ListarResgate, Europa.Controllers.EfetivarSolicitacaoResgate.Filtro);

    function actionsHtml(data, type, full, meta) {
        var content = "<div>";

        if (full.SituacaoResgate == 0) {
            content += Europa.Controllers.EfetivarSolicitacaoResgate.DatatableSolicitacao.renderButton(Europa.Controllers.EfetivarSolicitacaoResgate.Permissao.Aprovar, Europa.i18n.Messages.Liberar, "fa fa-unlock", "Liberar(" + meta.row + ")");
            //content += Europa.Controllers.EfetivarSolicitacaoResgate.DatatableSolicitacao.renderButton(Europa.Controllers.EfetivarSolicitacaoResgate.Permissao.Reprovar, Europa.i18n.Messages.Reprovar, "fa fa-close", "Reprovar(" + meta.row + ")");
        }

        content = content + "</div>";
        return content;
    };

    $scope.Liberar = function (row) {
        var obj = Europa.Controllers.EfetivarSolicitacaoResgate.DatatableSolicitacao.getRowData(row);
        
        Europa.Controllers.EfetivarSolicitacaoResgate.Aprovar = true;
        $(".aprovar").removeClass("hidden");
        $("#myModalLabel").html("Liberar Resgate");
        $("#IdResgate").val(obj.Id);
        $("#IdEmpresaVenda").val(obj.IdEmpresaVenda);
        Europa.Controllers.EfetivarSolicitacaoResgate.AbriModal();
    };

    $scope.Reprovar = function (row) {
        var obj = Europa.Controllers.EfetivarSolicitacaoResgate.DatatableSolicitacao.getRowData(row);

        Europa.Controllers.EfetivarSolicitacaoResgate.Aprovar = false;
        $(".reprovar").removeClass("hidden");
        $("#myModalLabel").html("Reprovar Resgate");
        $("#IdResgate").val(obj.Id);
        $("#IdEmpresaVenda").val(obj.IdEmpresaVenda);
        Europa.Controllers.EfetivarSolicitacaoResgate.AbriModal();
    };
}

DataTableApp.controller('SolicitacaoDatatable', solicitacaoTabela);

Europa.Controllers.EfetivarSolicitacaoResgate.Filtrar = function () {
    Europa.Controllers.EfetivarSolicitacaoResgate.DatatableSolicitacao.reloadData();
}


Europa.Controllers.EfetivarSolicitacaoResgate.ExportarTodos = function () {

    var params = Europa.Controllers.EfetivarSolicitacaoResgate.DatatableSolicitacao.lastRequestParams;
    var formDownload = $("#Exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.EfetivarSolicitacaoResgate.Url.ExportarTodos);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
}

Europa.Controllers.EfetivarSolicitacaoResgate.ExportarPagina = function () {

    var params = Europa.Controllers.EfetivarSolicitacaoResgate.DatatableSolicitacao.lastRequestParams;
    var formDownload = $("#Exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.EfetivarSolicitacaoResgate.Url.ExportarPagina);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
}