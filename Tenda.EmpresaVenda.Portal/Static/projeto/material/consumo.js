Europa.Controllers.Material.Consumo = {};
Europa.Controllers.Material.Consumo.DataTable = {};

Europa.Controllers.Material.Consumo.Tabela = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Controllers.Material.Consumo.DataTable = dataTableWrapper;

    dataTableWrapper
        .setColumns([
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '25%'),
            DTColumnBuilder.newColumn('Lote').withTitle('Lote').withOption('width', '25%'),
            DTColumnBuilder.newColumn('Total').withTitle(Europa.i18n.Messages.Total).withOption('width', '25%'),
            DTColumnBuilder.newColumn('Validade').withTitle('Validade').withOption("type", "date-format-DD/MM/YYYY").withOption('width', '25%')
        ])
        .setDefaultOptions('POST', Europa.Controllers.Material.Consumo.UrlListar, Europa.Controllers.Material.Consumo.Params);

};

DataTableApp.controller('ConsumoDataTable', Europa.Controllers.Material.Consumo.Tabela);

Europa.Controllers.Material.Consumo.Params = function () {
    var param = Europa.Form.SerializeJson(Europa.Controllers.Material.FormFiltro);
    return param;
};