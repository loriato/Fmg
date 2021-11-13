Europa.Controllers.ValorNominal = {}

$(function () {
    $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
    $(".ng-scope").css("word-wrap", "break-word");
});

DataTableApp.controller('ValorNominal', ValorNominalTabela);

function ValorNominalTabela($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.ValorNominal.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.ValorNominal.Tabela;

    self.setColumns([
        DTColumnBuilder.newColumn('NomeEmpreendimento').withTitle(Europa.i18n.Messages.Empreendimento).withOption('width', '200px'),
        DTColumnBuilder.newColumn('InicioVigencia').withTitle(Europa.i18n.Messages.AtualizadoEm).withOption('width', '200px').renderWith(Europa.String.FormatAsGeenDateTime),
        DTColumnBuilder.newColumn('FaixaUmMeioDe').withTitle(Europa.i18n.Messages.ValorFaixaUmMeioDe).withOption('width', '175px').renderWith(renderRetanguloRed),
        DTColumnBuilder.newColumn('FaixaUmMeioAte').withTitle(Europa.i18n.Messages.ValorFaixaUmMeioAte).withOption('width', '175px').renderWith(renderRetanguloRed),
        DTColumnBuilder.newColumn('FaixaDoisDe').withTitle(Europa.i18n.Messages.ValorFaixaDoisDe).withOption('width', '175px').renderWith(renderRetanguloPink),
        DTColumnBuilder.newColumn('FaixaDoisAte').withTitle(Europa.i18n.Messages.ValorFaixaDoisAte).withOption('width', '175px').renderWith(renderRetanguloPink),
        DTColumnBuilder.newColumn('PNEDe').withTitle(Europa.i18n.Messages.ValorPNEDe).withOption('width', '175px').renderWith(renderRetanguloOrange),
        DTColumnBuilder.newColumn('PNEAte').withTitle(Europa.i18n.Messages.ValorPNEAte).withOption('width', '175px').renderWith(renderRetanguloOrange)

    ])
        .setIdAreaHeader("valor_nominal_datatable_header")
        .setDefaultOptions('POST', Europa.Controllers.ValorNominal.UrlListar, Europa.Controllers.ValorNominal.Filtro);

    function renderRetanguloRed(data) {
        return '<div><label class="retangulo-red">' + Europa.String.FormatMoney(data) + '</label></div>';
    }
    function renderRetanguloPink(data) {
        return '<div><label class="retangulo-pink">' + Europa.String.FormatMoney(data) + '</label></div>';
    }
    function renderRetanguloOrange(data) {
        return '<div><label class="retangulo-orange">' + Europa.String.FormatMoney(data) + '</label></div>';
    }
};

Europa.Controllers.ValorNominal.Filtro = function () {
    var param = {
        nome: $("#empreendimento_filtro").val()
    };
    return param;
}
Europa.Controllers.ValorNominal.Filtrar = function () {
    Europa.Controllers.ValorNominal.Tabela.reloadData();
};

Europa.Controllers.ValorNominal.ExportarPagina = function () {
    var params = Europa.Controllers.ValorNominal.Filtro();
    params.order = Europa.Controllers.ValorNominal.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.ValorNominal.Tabela.lastRequestParams.draw;
    params.pageSize = Europa.Controllers.ValorNominal.Tabela.lastRequestParams.pageSize;
    params.start = Europa.Controllers.ValorNominal.Tabela.lastRequestParams.start;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.ValorNominal.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.ValorNominal.ExportarTodos = function () {
    var params = Europa.Controllers.ValorNominal.Filtro();
    params.order = Europa.Controllers.ValorNominal.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.ValorNominal.Tabela.lastRequestParams.draw;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.ValorNominal.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};