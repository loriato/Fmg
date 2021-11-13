"use strict";

Europa.Components.ConsultaEstoqueEmpreendimentoDatatable = {};
Europa.Components.ConsultaEstoqueEmpreendimentoDatatable.Tabela = {};
Europa.Components.ConsultaEstoqueEmpreendimentoDatatable.UrlListar = undefined;
Europa.Components.ConsultaEstoqueEmpreendimentoDatatable.UrlDetalharEmpreendimento = undefined;

DataTableApp.controller('estoqueEmpreendimentoTable', estoqueEmpreendimento);
function estoqueEmpreendimento($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Components.ConsultaEstoqueEmpreendimentoDatatable.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var wrapper = Europa.Components.ConsultaEstoqueEmpreendimentoDatatable.Tabela;

    wrapper.setColumns([
        DTColumnBuilder.newColumn('NomeEmpreendimento').withTitle(Europa.i18n.Messages.Empreendimento).withOption('width', '13%')
            .withOption("link", wrapper.withOptionLink(Europa.Components.ConsultaEstoqueEmpreendimentoDatatable.UrlDetalharEmpreendimento, "IdEmpreendimento")),
        DTColumnBuilder.newColumn('Divisao').withTitle(Europa.i18n.Messages.Divisao).withOption('width', '13%'),
        DTColumnBuilder.newColumn('Bairro').withTitle(Europa.i18n.Messages.Bairro).withOption('width', '13%'),
        DTColumnBuilder.newColumn('QtdeDisponivel').withTitle(Europa.i18n.Messages.QtdDispon).withOption('width', '10%'),
        DTColumnBuilder.newColumn('QtdeReservado').withTitle(Europa.i18n.Messages.QtdReserv).withOption('width', '8%'),
        DTColumnBuilder.newColumn('QtdeVendido').withTitle(Europa.i18n.Messages.QtdVendid).withOption('width', '10%'),
        DTColumnBuilder.newColumn('Caracteristicas').withTitle(Europa.i18n.Messages.Caracteristicas).withOption('width', '10%'),
        DTColumnBuilder.newColumn('QtdeUnidades').withTitle(Europa.i18n.Messages.QtdUnid).withOption('width', '10%'),
        DTColumnBuilder.newColumn('PrevisaoEntrega').withTitle(Europa.i18n.Messages.PrevisaoEntrega).withOption('width', '12%').renderWith(xgh)
    ])
        .setColActions(actionsHtml, '50px')
        .setAutoInit()
        .setOptionsSelect('POST', Europa.Components.ConsultaEstoqueEmpreendimentoDatatable.UrlListar, Europa.Components.ConsultaEstoqueEmpreendimentoDatatable.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<a class="btn btn-default" title="' + Europa.i18n.Messages.Visualizar + '" ng-click="expandEmpreendimento(\'' + meta.row + '\', \'' + $("#PreProposta_IdTorre").val() + '\')">' +
            '<i class="fa fa-arrows-alt "></i>' +
            '</a>';
    }

    function xgh(data, type, full, meta) {
        if (data === null) return "";

        var pattern = /Date\(([^)]+)\)/;
        var minutes = 360;
        var results = pattern.exec(data);

        var dt = new Date(parseFloat(results[1]) + minutes * 60000);


        return Europa.Date.toSmallDate(dt);
    }


    $scope.expandEmpreendimento = function (row, idTorre) {
        var data = Europa.Components.ConsultaEstoqueEmpreendimentoDatatable.Tabela.getRowData(row)
        Europa.Components.ConsultaEstoqueModalUnidade.AbrirModal(data.NomeEmpreendimento, data.Divisao, data.Caracteristicas, data.PrevisaoEntrega, idTorre);
    }
}

Europa.Components.ConsultaEstoqueEmpreendimentoDatatable.FilterParams = function () {
    return {
        divisao: $('#Empreendimento_Divisao').val()
    };
};

Europa.Components.ConsultaEstoqueEmpreendimentoDatatable.FiltrarTabela = function () {
    Europa.Components.ConsultaEstoqueEmpreendimentoDatatable.Tabela.reloadData();
};

Europa.Components.ConsultaEstoqueEmpreendimentoDatatable.LimparFiltro = function () {

};