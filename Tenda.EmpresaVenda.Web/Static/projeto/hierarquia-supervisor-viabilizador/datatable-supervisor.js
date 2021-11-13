$(function () {
});

function TableSupervisor($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.HierarquiaSupervisorViabilizador.SupervisorTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.HierarquiaSupervisorViabilizador.SupervisorTable;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('NomeSupervisor').withTitle(Europa.i18n.Messages.Nome).withOption("width", "100px")

        ])
        .setIdAreaHeader("supervisor_datatable_barra")
        .setOptionsSelect('POST', Europa.Controllers.HierarquiaSupervisorViabilizador.UrlListarSupervisores, Europa.Controllers.HierarquiaSupervisorViabilizador.FiltroSupervisor);

    $scope.onRowSelect = function (data) {
        if (Europa.Controllers.HierarquiaSupervisorViabilizador.SupervisorId == data.Id) {
            Europa.Controllers.HierarquiaSupervisorViabilizador.SupervisorId = undefined;
            $("#btn-opcoes").prop("disabled", true)
        } else {
            Europa.Controllers.HierarquiaSupervisorViabilizador.SupervisorId = data.Id;
            $("#btn-opcoes").prop("disabled", false)
        }

        Europa.Controllers.HierarquiaSupervisorViabilizador.ViabilizadorTable.reloadData(undefined, false);

    }
};

DataTableApp.controller('SupervisorDatatable', TableSupervisor);

Europa.Controllers.HierarquiaSupervisorViabilizador.FiltroSupervisor = function () {
    var params = {
        NomeSupervisor: $("#nomeSupervisor").val()
    };

    return params;
};

Europa.Controllers.HierarquiaSupervisorViabilizador.FiltrarSupervisor = function () {
    Europa.Controllers.HierarquiaSupervisorViabilizador.SupervisorId = undefined;
    Europa.Controllers.HierarquiaSupervisorViabilizador.SupervisorTable.reloadData();
    Europa.Controllers.HierarquiaSupervisorViabilizador.ViabilizadorTable.reloadData();
};

Europa.Controllers.HierarquiaSupervisorViabilizador.LimparFiltroSupervisor = function () {
    $("#nomeSupervisor").val("");
};
