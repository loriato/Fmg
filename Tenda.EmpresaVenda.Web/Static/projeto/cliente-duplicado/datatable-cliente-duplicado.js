$(function () {

});

function TableClienteDuplicado($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.ClienteDuplicado.ClienteDuplicadoTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.ClienteDuplicado.ClienteDuplicadoTable;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('NomeCliente').withTitle(Europa.i18n.Messages.Cliente),
            //DTColumnBuilder.newColumn('NomeEmpresaVenda').withTitle(Europa.i18n.Messages.Cliente),
        ])
        .setIdAreaHeader("cliente_duplicado_datatable_barra")
        .setOptionsSelect('POST', Europa.Controllers.ClienteDuplicado.UrlListarClientes, Europa.Controllers.ClienteDuplicado.FiltroClienteDuplicado);
    
    $scope.onRowSelect = function (data) {
        console.log(data.Id)
        if (Europa.Controllers.ClienteDuplicado.ClienteId == data.Id) {
            Europa.Controllers.ClienteDuplicado.ClienteId = undefined;
            Europa.Controllers.ClienteDuplicado.ClienteCpf = "---";
        } else {
            Europa.Controllers.ClienteDuplicado.ClienteId = data.Id;      
            Europa.Controllers.ClienteDuplicado.ClienteCpf = data.CPF;
        }

        Europa.Controllers.ClienteDuplicado.PrePropostaTable.reloadData(undefined, false);
    }   
};

DataTableApp.controller('ClienteDuplicadoDatatable', TableClienteDuplicado);

Europa.Controllers.ClienteDuplicado.FiltroClienteDuplicado = function () {
    var param = {
        NomeCliente: $("#nomeCliente").val()
    };
    return param;
};

Europa.Controllers.ClienteDuplicado.FiltrarClienteDuplicado = function () {
    Europa.Controllers.ClienteDuplicado.ClienteId = undefined;
    Europa.Controllers.ClienteDuplicado.ClienteCpf = "---";

    Europa.Controllers.ClienteDuplicado.ClienteDuplicadoTable.reloadData();
    Europa.Controllers.ClienteDuplicado.PrePropostaTable.reloadData(undefined, false);
};

Europa.Controllers.ClienteDuplicado.LimparFiltroClienteDuplicado = function () {
    $("#nomeCliente").val("");
};
