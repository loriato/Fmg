$(function () {
});

function TableCoordenador($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.HierarquiaCoordenador.CoordenadorTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.HierarquiaCoordenador.CoordenadorTable;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('NomeCoordenador').withTitle(Europa.i18n.Messages.Nome).withOption("width", "100px")

        ])
        .setIdAreaHeader("coordenador_datatable_barra")
        .setOptionsSelect('POST', Europa.Controllers.HierarquiaCoordenador.UrlListarCoordenadores, Europa.Controllers.HierarquiaCoordenador.FiltroCoordenador);

    $scope.onRowSelect = function (data) {
        $("#tab_cv").prop("hidden", false)
        $("#tab_cs").prop("hidden", false)

        if (Europa.Controllers.HierarquiaCoordenador.CoordenadorId == data.Id) {
            Europa.Controllers.HierarquiaCoordenador.CoordenadorId = undefined;
            $("#btn-opcoes").prop("disabled", true)
        } else {
            Europa.Controllers.HierarquiaCoordenador.CoordenadorId = data.Id;
            $("#btn-opcoes").prop("disabled", false)

            switch (data.TipoHierarquiaCicloFinanceiro) {
                case 1: 
                    $("#tab_cv").prop("hidden", true)
                    $("#tab1").click()
                    break;
                case 2:
                    $("#tab_cs").prop("hidden", true)
                    $("#tab2").click()
                    break;
                default:
                    break;
            }
        }

        Europa.Controllers.HierarquiaCoordenador.SupervisorTable.reloadData(undefined, false);
        Europa.Controllers.HierarquiaCoordenador.ViabilizadorTable.reloadData(undefined, false);

    }
};

DataTableApp.controller('CoordenadorDatatable', TableCoordenador);

Europa.Controllers.HierarquiaCoordenador.FiltroCoordenador = function () {
    var params = {
        NomeCoordenador: $("#nomeCoordenador").val()
    };

    return params;
};

Europa.Controllers.HierarquiaCoordenador.FiltrarCoordenador = function () {
    Europa.Controllers.HierarquiaCoordenador.CoordenadorId = undefined;
    Europa.Controllers.HierarquiaCoordenador.CoordenadorTable.reloadData();
    Europa.Controllers.HierarquiaCoordenador.ViabilizadorTable.reloadData();
};

Europa.Controllers.HierarquiaCoordenador.LimparFiltroCoordenador = function () {
    $("#nomeCoordenador").val("");
};
