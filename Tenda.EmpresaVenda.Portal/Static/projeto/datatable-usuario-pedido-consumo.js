
Europa.Components.UsuarioPedidoConsumo.Tabela = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Components.UsuarioPedidoConsumo.DataTable = dataTableWrapper;

    dataTableWrapper
        .setColumns([
            DTColumnBuilder.newColumn('NomeUsuario').withTitle(Europa.i18n.Messages.Nome).withOption('width', '15%'),
            DTColumnBuilder.newColumn('Consumo').withTitle(Europa.i18n.Messages.Modelo).withOption('width', '20%'),
            DTColumnBuilder.newColumn('Quantidade').withTitle(Europa.i18n.Messages.Placa).withOption('width', '20%'),
            DTColumnBuilder.newColumn('DataPedido').withTitle('Data Pedido').withOption('width', '20%').withOption("type", "date-format-DD/MM/YYYY hh:mm:ss")
        ])
        .setDefaultOrder([3, "desc"])
        .setDefaultOptions('POST', Europa.Components.UsuarioPedidoConsumo.UrlListar, Europa.Components.UsuarioPedidoConsumo.Params);


};

DataTableApp.controller('UsuarioPedidoConsumoDataTable', Europa.Components.UsuarioPedidoConsumo.Tabela);

