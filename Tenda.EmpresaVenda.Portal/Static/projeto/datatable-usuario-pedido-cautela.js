
Europa.Components.UsuarioPedidoCautela.Tabela = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Components.UsuarioPedidoCautela.DataTable = dataTableWrapper;

    dataTableWrapper
        .setColumns([
            DTColumnBuilder.newColumn('NomeUsuario').withTitle(Europa.i18n.Messages.Nome).withOption('width', '15%'),
            DTColumnBuilder.newColumn('Cautela').withTitle(Europa.i18n.Messages.Modelo).withOption('width', '20%'),
            DTColumnBuilder.newColumn('Quantidade').withTitle(Europa.i18n.Messages.Placa).withOption('width', '20%'),
            DTColumnBuilder.newColumn('DataPedido').withTitle('Data Pedido').withOption('width', '20%').withOption("type", "date-format-DD/MM/YYYY hh:mm:ss"),
            DTColumnBuilder.newColumn('DataDevolucao').withTitle('Data Entrega').withOption('width', '20%').withOption("type", "date-format-DD/MM/YYYY hh:mm:ss"),

        ])
        .setDefaultOrder([3, "desc"])
        .setDefaultOptions('POST', Europa.Components.UsuarioPedidoCautela.UrlListar, Europa.Components.UsuarioPedidoCautela.Params);


};

DataTableApp.controller('UsuarioPedidoCautelaDataTable', Europa.Components.UsuarioPedidoCautela.Tabela);

