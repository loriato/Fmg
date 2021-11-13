Europa.Controllers.AceitesBanners = {};

$(function () {
    Europa.Controllers.AceitesBanners.InitAutoComplete();
});

Europa.Controllers.AceitesBanners.InitAutoComplete = function () {
    Europa.Controllers.AceitesBanners.AutoCompleteTituloBanner = new Europa.Components.AutoCompleteTituloBanners()
        .WithTargetSuffix("titulo_banner")
        .Configure();
}


function AceitesBannersTabela($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.AceitesBanners.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.AceitesBanners.Tabela;
    self.setColumns([
        DTColumnBuilder.newColumn('TituloBanner').withTitle(Europa.i18n.Messages.TituloBanner).withOption('width', '150px'),
        DTColumnBuilder.newColumn('DataAceite').withTitle(Europa.i18n.Messages.DataAceite).withOption('width', '80px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('NomeCorretor').withTitle(Europa.i18n.Messages.NomeCorretor).withOption('width', '100px'),
        DTColumnBuilder.newColumn('EmailCorretor').withTitle(Europa.i18n.Messages.EmailCorretor).withOption('width', '100px'),

    ])
        .setAutoInit()
        .setIdAreaHeader("datatable_header")
        .setOptionsMultiSelect('POST', Europa.Controllers.AceitesBanners.UrlListar, Europa.Controllers.AceitesBanners.Filtro);

}

DataTableApp.controller('AceitesBanners', AceitesBannersTabela);

Europa.Controllers.AceitesBanners.Filtrar = function () {
    Europa.Controllers.AceitesBanners.Tabela.reloadData();
};

Europa.Controllers.AceitesBanners.Filtro = function () {
    var param = {
        IdBanner: $("#autocomplete_titulo_banner").val(),
    };
    return param;
};

Europa.Controllers.AceitesBanners.ExportarPagina = function () {

    var params = {
        IdBanner: $("#autocomplete_titulo_banner").val(),
    };
    params.order = Europa.Controllers.AceitesBanners.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.AceitesBanners.Tabela.lastRequestParams.draw;
    params.pageSize = Europa.Controllers.AceitesBanners.Tabela.lastRequestParams.pageSize;
    params.start = Europa.Controllers.AceitesBanners.Tabela.lastRequestParams.start;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.AceitesBanners.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.AceitesBanners.ExportarTodos = function () {


    var params = {
        IdBanner: $("#autocomplete_titulo_banner").val(),
    };
    params.order = Europa.Controllers.AceitesBanners.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.AceitesBanners.Tabela.lastRequestParams.draw;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.AceitesBanners.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();

};