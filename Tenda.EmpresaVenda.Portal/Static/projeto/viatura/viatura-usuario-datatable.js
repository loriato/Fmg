Europa.Controllers.ViaturaUsuario = {};
Europa.Controllers.ViaturaUsuario.FormFiltro = "#form-viatura"

Europa.Controllers.ViaturaUsuario.Tabela = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Controllers.ViaturaUsuario.DataTable = dataTableWrapper;

    dataTableWrapper
        .setColumns([
            DTColumnBuilder.newColumn('Usuario.Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '15%'),
            DTColumnBuilder.newColumn('Viatura.Modelo').withTitle(Europa.i18n.Messages.Modelo).withOption('width', '20%'),
            DTColumnBuilder.newColumn('Viatura.Placa').withTitle(Europa.i18n.Messages.Placa).withOption('width', '20%'),
            DTColumnBuilder.newColumn('Pedido').withTitle('Data Pedido').withOption('width', '20%').withOption("type", "date-format-DD/MM/YYYY hh:mm:ss"),
            DTColumnBuilder.newColumn('Entrega').withTitle('Data Entrega').withOption('width', '20%').withOption("type", "date-format-DD/MM/YYYY hh:mm:ss"),
            DTColumnBuilder.newColumn('QuilometragemAntigo').withTitle('Quilometragem Antigo').withOption('width', '15%'),
            DTColumnBuilder.newColumn('QuilometragemNovo').withTitle('Quilometragem Novo').withOption('width', '15%'),

        ])
        .setDefaultOrder([3, "desc"])
        .setDefaultOptions('POST', Europa.Controllers.ViaturaUsuario.UrlListar, Europa.Controllers.ViaturaUsuario.Params);


};

DataTableApp.controller('ViaturaUsuarioDataTable', Europa.Controllers.ViaturaUsuario.Tabela);

Europa.Controllers.ViaturaUsuario.Params = function () {
    var param = Europa.Form.SerializeJson(Europa.Controllers.ViaturaUsuario.FormFiltro);
    return param;
}