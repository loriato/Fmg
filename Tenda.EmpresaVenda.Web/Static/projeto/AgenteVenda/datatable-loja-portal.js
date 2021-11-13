Europa.Controllers.LojaPortal.IncluirLojaPortal = false;

$(function () { })

function TableLojaPortal($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.LojaPortal.LojaPortalTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.LojaPortal.LojaPortalTable;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome)
        ])
        .setIdAreaHeader("LojaPortal_datatable_header")
        .setOptionsSelect('POST', Europa.Controllers.LojaPortal.UrlListarLojasPortal, Europa.Controllers.LojaPortal.FiltroLojaPortal);

    $scope.onRowSelect = function (data) {
        if (Europa.Controllers.LojaPortal.LojaPortalTable.getDataRowEdit().Id === undefined || Europa.Controllers.LojaPortal.LojaPortalTable.getDataRowEdit().Descricao === undefined) {
            if (Europa.Controllers.LojaPortal.LojaPortalId !== undefined) {
                if (Europa.Controllers.LojaPortal.LojaPortalId !== data.Id) {
                    Europa.Controllers.LojaPortal.CarregarDadosUsuario(data.Id);
                } else {
                    Europa.Controllers.LojaPortal.LojaPortalId = undefined;

                    Europa.Controllers.LojaPortal.UsuarioTable.closeEdition();
                    Europa.Controllers.LojaPortal.UsuarioTable.reloadData();
                }
            } else {
                Europa.Controllers.LojaPortal.CarregarDadosUsuario(data.Id);
            }
        }
    }
};

DataTableApp.controller('LojaPortalDatatable', TableLojaPortal);

Europa.Controllers.LojaPortal.FiltroLojaPortal = function () {
    var param = {
        Nome: $("#filtroLoja").val()
    };
    return param;
};

Europa.Controllers.LojaPortal.FiltrarLojaPortal = function () {
    Europa.Controllers.LojaPortal.LojaPortalTable.reloadData();
};

Europa.Controllers.LojaPortal.LimparFiltroLojaPortal = function () {
    $("#filtroLoja").val("");
};
