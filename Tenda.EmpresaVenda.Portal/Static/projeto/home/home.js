Europa.Controllers.Home = {};

$(function () {
    Europa.Controllers.Home.Esconder();
});
Europa.Components.ViaturaUsuario.Params = function () {

    return {};
};

Europa.Components.UsuarioPedidoCautela.Params = function () {
    return {};
};

Europa.Components.UsuarioPedidoConsumo.Params = function () {
    return {};
};
Europa.Controllers.Home.Listar = function () {
    Europa.Controllers.Material.Consumo.DataTable.reloadData();
    Europa.Controllers.Material.Cautela.DataTable.reloadData();
    Europa.Components.ViaturaUsuario.DataTable.reloadData();
};
Europa.Controllers.Home.Esconder = function () {
    $("#cautelas").hide();
    $("#viaturas").hide();
    $("#consumos").hide();
};

Europa.Controllers.Home.Cautela = function () {
    $("#cautelas").show();
    $("#viaturas").hide();
    $("#consumos").hide();
    Europa.Controllers.Home.Listar();
};

Europa.Controllers.Home.Consumo = function () {
    $("#cautelas").hide();
    $("#viaturas").hide();
    $("#consumos").show();
    Europa.Controllers.Home.Listar();
};


Europa.Controllers.Home.Viatura = function () {
    $("#cautelas").hide();
    $("#viaturas").show();
    $("#consumos").hide();
    Europa.Controllers.Home.Listar();
};

Europa.Controllers.Material = {};
Europa.Controllers.Material.Cautela = {};
Europa.Controllers.Material.Cautela.DataTable = {};
Europa.Controllers.Material.Cautela.Tabela = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Controllers.Material.Cautela.DataTable = dataTableWrapper;

    dataTableWrapper
        .setColumns([
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '30%'),
            DTColumnBuilder.newColumn('Marca').withTitle(Europa.i18n.Messages.Marca).withOption('width', '30%'),
            DTColumnBuilder.newColumn('Total').withTitle(Europa.i18n.Messages.Total).withOption('width', '30%')
        ])
        
        .setDefaultOptions('POST', Europa.Controllers.Material.Cautela.UrlListar, Europa.Controllers.Material.Cautela.Params);
  

};

DataTableApp.controller('CautelaDataTable', Europa.Controllers.Material.Cautela.Tabela);

Europa.Controllers.Material.Cautela.Params = function () {
    var param = {};
    return param;
};

Europa.Controllers.Material.Consumo = {};
Europa.Controllers.Material.Consumo.DataTable = {};
Europa.Controllers.Material.Consumo.Tabela = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Controllers.Material.Consumo.DataTable = dataTableWrapper;

    dataTableWrapper
        .setColumns([
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '25%'),
            DTColumnBuilder.newColumn('Lote').withTitle('Lote').withOption('width', '15%'),
            DTColumnBuilder.newColumn('Total').withTitle(Europa.i18n.Messages.Total).withOption('width', '15%'),
            DTColumnBuilder.newColumn('Validade').withTitle('Validade').withOption("type", "date-format-DD/MM/YYYY").withOption('width', '25%')
        ])
        .setDefaultOptions('POST', Europa.Controllers.Material.Consumo.UrlListar, Europa.Controllers.Material.Consumo.Params);
    
};

DataTableApp.controller('ConsumoDataTable', Europa.Controllers.Material.Consumo.Tabela);

Europa.Controllers.Material.Consumo.Params = function () {
    var param = {};
    return param;
};