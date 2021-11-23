Europa.Controllers.Material.Cautela = {};
Europa.Controllers.Material.Cautela.DataTable = {};


Europa.Controllers.Material.Cautela.Tabela = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Controllers.Material.Cautela.DataTable = dataTableWrapper;

    dataTableWrapper
        .setColumns([
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '33%'),
            DTColumnBuilder.newColumn('Marca').withTitle(Europa.i18n.Messages.Marca).withOption('width', '33%'),
            DTColumnBuilder.newColumn('Total').withTitle(Europa.i18n.Messages.Total).withOption('width', '33%')
        ])
        .setDefaultOptions('POST', Europa.Controllers.Material.Cautela.UrlListar, Europa.Controllers.Material.Cautela.Params);

};

DataTableApp.controller('CautelaDataTable', Europa.Controllers.Material.Cautela.Tabela);

Europa.Controllers.Material.Cautela.Params = function () {
    var param = Europa.Form.SerializeJson(Europa.Controllers.Material.FormFiltro);
    return param;
};