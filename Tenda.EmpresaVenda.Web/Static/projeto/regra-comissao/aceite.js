Europa.Controllers.RegraComissao = {};
Europa.Controllers.RegraComissao.Aceite = {};
Europa.Controllers.RegraComissao.Aceite.Tabela = {};
Europa.Controllers.RegraComissao.Aceite.IdForm = "#form_detalhamento_regra_comissao";

DataTableApp.controller('AceiteDatatable', AceiteDatatable);

function AceiteDatatable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.RegraComissao.Aceite.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.RegraComissao.Aceite.Tabela;
    tabelaWrapper
        .setColumns([
            DTColumnBuilder.newColumn('NomeEmpresaVenda').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption('width', '25%'),
            DTColumnBuilder.newColumn('CnpjEmpresaVenda').withTitle(Europa.i18n.Messages.Cnpj).renderWith(formatCNPJ).withOption('width', '20%'),
            DTColumnBuilder.newColumn('InicioVigenciaRegraComissao').withTitle(Europa.i18n.Messages.InicioVigencia).withOption("type", "date-format-DD/MM/YYYY HH:mm:ss").withOption('width', '10%'),
            DTColumnBuilder.newColumn('TerminoVigenciaRegraComissao').withTitle(Europa.i18n.Messages.TerminoVigencia).withOption("type", "date-format-DD/MM/YYYY HH:mm:ss").withOption('width', '10%'),
            DTColumnBuilder.newColumn('DataAceite').withTitle(Europa.i18n.Messages.DataAceite).withOption("type", "date-format-DD/MM/YYYY HH:mm:ss").withOption('width', '10%'),
            DTColumnBuilder.newColumn('NomeAprovador').withTitle(Europa.i18n.Messages.Aprovador).withOption('width', '25%')
        ])
        .setColActions(actionsHtml, '60px')
        .setIdAreaHeader("datatable_header")
        .setDefaultOptions('POST', Europa.Controllers.RegraComissao.Aceite.UrlListar, Europa.Controllers.RegraComissao.Aceite.FilterParams);

    function actionsHtml(data, type, full, meta) {
        var html = '<div>';

        if (full.IdArquivo !== 0) {
            html = html + Europa.Controllers.RegraComissao.Aceite.Tabela.renderButton(Europa.Controllers.RegraComissao.Aceite.PermDownloadRegraComissaoEvs, Europa.i18n.Messages.Download, "fa fa-download", "Download(" + data.Id + ")");
        }
        html += '</div>';
        return html;
    }

    function formatCNPJ(data, type, full) {
        return Europa.Mask.GetMaskedValue(data, Europa.Mask.FORMAT_CNPJ);
    }

    $scope.Download = function (idRegra) {
        location.href = Europa.Controllers.RegraComissao.Aceite.UrlDownloadPdfRegraComissaoEvs + "?idRegra=" + idRegra;
    };
}


Europa.Controllers.RegraComissao.Aceite.FilterParams = function () {
    return {
        idRegra: $("#Id", Europa.Controllers.RegraComissao.Aceite.IdForm).val()
    };
};

Europa.Controllers.RegraComissao.Aceite.ExportarTodos = function () {
    var params = Europa.Controllers.RegraComissao.Aceite.Tabela.lastRequestParams;
    params.idRegraComissao = $('#form_detalhamento_regra_comissao').find('#Id').val();
    var formExportar = $("#exportar");
    $("#exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.RegraComissao.Aceite.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.RegraComissao.Aceite.ExportarPagina = function () {
    var params = Europa.Controllers.RegraComissao.Aceite.Tabela.lastRequestParams;
    params.idRegraComissao = $('#form_detalhamento_regra_comissao').find('#Id').val();
    var formExportar = $("#exportar");
    $("#exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.RegraComissao.Aceite.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};