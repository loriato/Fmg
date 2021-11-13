
$(function () {

});

function DatatableEstado($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.EstadoAvalista.DatatableEstado = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.EstadoAvalista.DatatableEstado;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('Estado').withTitle(Europa.i18n.Messages.Estados).withOption("width", "100px"),
        ])
        .setIdAreaHeader("datatable_estado_barra")
        .setOptionsSelect('POST', Europa.Controllers.EstadoAvalista.UrlListarEstados, Europa.Controllers.EstadoAvalista.FiltroEstado);    

    $scope.onRowSelect = function (data) {
        Europa.Controllers.EstadoAvalista.EstadoSelecionado = data.Estado;
        Europa.Controllers.EstadoAvalista.AvalistaTable.reloadData();
    }

};

DataTableApp.controller('DatatableEstado', DatatableEstado);

Europa.Controllers.EstadoAvalista.FiltroEstado = function () {
    var param = {
        Estado: $("#filtroEstado").val().toUpperCase()
    };
    return param;
};

Europa.Controllers.EstadoAvalista.FiltrarEstados = function () {
    Europa.Controllers.EstadoAvalista.DatatableEstado.reloadData(undefined, false);
    $("#btn-incluir-cidade").prop("disabled", true);
};

Europa.Controllers.EstadoAvalista.LimparFiltroEstado = function () {
    $("#filtroEstado").val("");
};
