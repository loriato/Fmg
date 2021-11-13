"use strict";


Europa.Components.ConsultaEstoqueUnidadeDatatable = {};
Europa.Components.ConsultaEstoqueUnidadeDatatable.Tabela = {};
Europa.Components.ConsultaEstoqueUnidadeDatatable.EmpreendimentoParams = {};
Europa.Components.ConsultaEstoqueUnidadeDatatable.UrlListar = undefined;

DataTableApp.controller('estoqueUnidadeTable', estoqueUnidade);
function estoqueUnidade($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Components.ConsultaEstoqueUnidadeDatatable.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var wrapper = Europa.Components.ConsultaEstoqueUnidadeDatatable.Tabela;

    wrapper.setColumns([
        DTColumnBuilder.newColumn('NomeTorre').withTitle(Europa.i18n.Messages.Torre).withOption('width', '15%')
            .withOption("link", wrapper.withOptionLink(Europa.Controllers.PreProposta.UrlDetalharTorre, "IdTorre", "IdSapTorre")),
        DTColumnBuilder.newColumn('NomeUnidade').withTitle(Europa.i18n.Messages.Unidade).withOption('width', '10%')
            .withOption("link", wrapper.withOptionLink(Europa.Controllers.PreProposta.UrlDetalharUnidade, "IdUnidade", "IdSapUnidade")),
        DTColumnBuilder.newColumn('Metragem').withTitle(Europa.i18n.Messages.Metragem).withOption('width', '10%').withClass('dt-body-center').renderWith(formatMetragem).withClass('dt-body-right'),
        DTColumnBuilder.newColumn('Caracteristicas').withTitle(Europa.i18n.Messages.Caracteristicas),
        DTColumnBuilder.newColumn('Andar').withTitle(Europa.i18n.Messages.Andar).withOption('width', '10%').withClass('dt-body-center')
    ])
        .setAutoInit()
        .setOptionsSelect('POST', Europa.Components.ConsultaEstoqueUnidadeDatatable.UrlListar, Europa.Components.ConsultaEstoqueUnidadeDatatable.FilterParams);

    function formatMetragem(data) {
        return data.toFixed(2).replace(".", ",");
    }
}

Europa.Components.ConsultaEstoqueUnidadeDatatable.FilterParams = function () {
    return Europa.Components.ConsultaEstoqueUnidadeDatatable.EmpreendimentoParams;
};

Europa.Components.ConsultaEstoqueUnidadeDatatable.FiltrarTabela = function () {
    Europa.Components.ConsultaEstoqueUnidadeDatatable.Tabela.reloadData();
};