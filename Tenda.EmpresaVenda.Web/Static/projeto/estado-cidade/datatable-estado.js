
$(function () {

});

function DatatableEstado($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.EstadoCidade.DatatableEstado = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.EstadoCidade.DatatableEstado;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('Estado').withTitle(Europa.i18n.Messages.Estados).withOption("width", "100px"),
        ])
        .setIdAreaHeader("datatable_estado_barra")
        .setOptionsSelect('POST', Europa.Controllers.EstadoCidade.UrlListarEstados, Europa.Controllers.EstadoCidade.FiltroEstado);    

    $scope.onRowSelect = function (data) {

        if (Europa.Controllers.EstadoCidade.OnEstado == data.Estado) {
            Europa.Controllers.EstadoCidade.OnEstado = "xxxx";
            $("#btn-incluir-cidade").prop("disabled", true);
        } else {
            Europa.Controllers.EstadoCidade.OnEstado = data.Estado;
            $("#btn-incluir-cidade").prop("disabled", false)
        }        

        Europa.Controllers.EstadoCidade.FiltrarCidades();
    }

};

DataTableApp.controller('DatatableEstado', DatatableEstado);

Europa.Controllers.EstadoCidade.FiltroEstado = function () {
    var param = {
        Estado: $("#filtroEstado").val().toUpperCase()
    };
    return param;
};

Europa.Controllers.EstadoCidade.FiltrarEstados = function () {
    Europa.Controllers.EstadoCidade.DatatableEstado.reloadData(undefined, false);
    $("#btn-incluir-cidade").prop("disabled", true);
};

Europa.Controllers.EstadoCidade.LimparFiltroEstado = function () {
    $("#filtroEstado").val("");
};
