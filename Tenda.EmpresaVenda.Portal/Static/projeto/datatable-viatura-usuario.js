
Europa.Components.ViaturaUsuario.Tabela = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Components.ViaturaUsuario.DataTable = dataTableWrapper;

    dataTableWrapper
        .setColumns([
            DTColumnBuilder.newColumn('NomeUsuario').withTitle(Europa.i18n.Messages.Nome).withOption('width', '15%'),
            DTColumnBuilder.newColumn('Modelo').withTitle(Europa.i18n.Messages.Modelo).withOption('width', '20%'),
            DTColumnBuilder.newColumn('Placa').withTitle(Europa.i18n.Messages.Placa).withOption('width', '20%'),
            DTColumnBuilder.newColumn('DataPedido').withTitle('Data Pedido').withOption('width', '20%').withOption("type", "date-format-DD/MM/YYYY hh:mm:ss"),
            DTColumnBuilder.newColumn('DataEntrega').withTitle('Data Entrega').withOption('width', '20%').withOption("type", "date-format-DD/MM/YYYY hh:mm:ss"),
            DTColumnBuilder.newColumn('QuilometragemAntigo').withTitle('Quilometragem Antigo').withOption('width', '15%'),
            DTColumnBuilder.newColumn('QuilometragemNovo').withTitle('Quilometragem Novo').withOption('width', '15%'),

        ])
        .setDefaultOrder([3, "desc"])
        .setDefaultOptions('POST', Europa.Components.ViaturaUsuario.UrlListar, Europa.Components.ViaturaUsuario.Params);


};

DataTableApp.controller('ViaturaUsuarioDataTable', Europa.Components.ViaturaUsuario.Tabela);

