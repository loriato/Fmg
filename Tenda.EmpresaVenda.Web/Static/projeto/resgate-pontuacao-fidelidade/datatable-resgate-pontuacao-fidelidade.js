$(function () {
    
});

function pontuacaoFidelidadeTabela($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.ResgatePontuacaoFidelidade.DatatableResgatePontuacaoFidelidade = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.ResgatePontuacaoFidelidade.DatatableResgatePontuacaoFidelidade;
    self.setColumns([
        DTColumnBuilder.newColumn('CodigoProposta').withTitle(Europa.i18n.Messages.Proposta).withOption('width', '15%'),
        DTColumnBuilder.newColumn('NomeEmpreendimento').withTitle(Europa.i18n.Messages.Empreendimento).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Pontuacao').withTitle(Europa.i18n.Messages.Pontuacao).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Codigo').withTitle(Europa.i18n.Messages.Codigo).withOption('width', '15%').renderWith(renderCodigo),
        DTColumnBuilder.newColumn('DataPontuacao').withTitle(Europa.i18n.Messages.DataPontuacao).withOption('width', '15%').renderWith(Europa.Date.toGeenDateFormat),
        DTColumnBuilder.newColumn('SituacaoPontuacao').withTitle(Europa.i18n.Messages.Situacao).withOption('type', 'enum-format-SituacaoPontuacao').withOption('width', '15%'),

    ])
        .setAutoInit(false)
        .setDefaultOrder([[0, 'asc']])
        .setIdAreaHeader("pontuacao_datatable_barra")
        .setDefaultOptions('POST', Europa.Controllers.ResgatePontuacaoFidelidade.Url.ListarPontuacao, Europa.Controllers.ResgatePontuacaoFidelidade.Filtro);

    function actionsHtml(data, type, full, meta) {
        var content = "<div>";
        
        content = content + "</div>";
        return content;
    };    

    function renderCodigo(data, type, full, meta) {
        var link = '<div>';
        if (data) {
            link = link + "<a title='Pontuação Fidelidade' target='_blank' href='" + Europa.Controllers.ResgatePontuacaoFidelidade.Url.MatrizPontuacaoFidelidade + '?idPontuacaoFidelidade=' + full.IdPontuacaoFidelidade + "'>" + full.Codigo + "</a>";
        }
        link += '</div>';
        return link;
    }
}

DataTableApp.controller('PontuacaoDatatable', pontuacaoFidelidadeTabela);

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

Europa.Controllers.ResgatePontuacaoFidelidade.ExportarTodosPontuacao = function () {

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

    var params = Europa.Controllers.ResgatePontuacaoFidelidade.DatatableResgatePontuacaoFidelidade.lastRequestParams;
    var formDownload = $("#Exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.ResgatePontuacaoFidelidade.Url.ExportarTodosPontuacao);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
}

Europa.Controllers.ResgatePontuacaoFidelidade.ExportarPaginaPontuacao = function () {

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

    var params = Europa.Controllers.ResgatePontuacaoFidelidade.DatatableResgatePontuacaoFidelidade.lastRequestParams;
    var formDownload = $("#Exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.ResgatePontuacaoFidelidade.Url.ExportarPaginaPontuacao);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
}