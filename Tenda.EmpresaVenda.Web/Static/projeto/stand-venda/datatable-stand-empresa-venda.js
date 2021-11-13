$(function () {

});

function TableStandEmpresaVenda($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.StandVenda.StandEmpresaVendaTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.StandVenda.StandEmpresaVendaTable;
    tabela
        .setColumns([
            //DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption("width", "40px"),
            DTColumnBuilder.newColumn('NomeEmpresaVenda').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption("width", "40px"),
            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).renderWith(renderSituacao).withOption("width", "40px"),
        ])
        .setIdAreaHeader("standEmpresaVenda_datatable_barra")
        //.setDefaultOrder([[1, 'asc'], [2, 'asc']])
        .setAutoInit(false)
        .setOptionsSelect('POST', Europa.Controllers.StandVenda.UrlListarDatatableStandVendaEmpresaVenda, Europa.Controllers.StandVenda.FiltroStandEmpresaVenda);

    $scope.onRowSelect = function (data) {

    }

    function renderSituacao(data,type,full,meta) {
        var checkBox = '<label>';
        checkBox += '<input type = "checkbox" value = "' + data + '"';

        if (full.Situacao==1) {
            checkBox += ' checked="checked"';
        }

        checkBox += 'onchange = "Europa.Controllers.StandVenda.OnJoinStandEmpresaVenda(' + meta.row + ')" /> ';
        checkBox += '</label>';
        return checkBox;
    }
};

DataTableApp.controller('StandEmpresaVendaDatatable', TableStandEmpresaVenda);

Europa.Controllers.StandVenda.FiltroStandEmpresaVenda = function () {
    

    var param = {
        Nome: $("#filtro_nome_empresa_venda").val(),
        Estado: Europa.Controllers.StandVenda.Estado,
        IdRegional: Europa.Controllers.StandVenda.IdRegional,
        IdStandVenda: Europa.Controllers.StandVenda.IdStandVenda
    };
    return param;
};

Europa.Controllers.StandVenda.FiltrarStandEmpresaVenda = function () {

    if (Europa.Controllers.StandVenda.IdStandVenda == undefined) {
        var res = {
            Sucesso: false,
            Mensagens: ["Selecione um Stand"]
        }
        Europa.Informacao.PosAcao(res);
        return;
    }

    Europa.Controllers.StandVenda.StandEmpresaVendaTable.reloadData(undefined,false);
};

Europa.Controllers.StandVenda.LimparFiltroStandEmpresaVenda = function () {
    $("#filtro_nome_empresa_venda").val("")
};

Europa.Controllers.StandVenda.OnJoinStandEmpresaVenda = function (row) {

    console.log(JSON.stringify(obj));
    var obj = Europa.Controllers.StandVenda.StandEmpresaVendaTable.getRowData(row);

    var url = Europa.Controllers.StandVenda.UrlOnJoinStandEmpresaVenda;

    $.post(url, obj, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.StandVenda.StandEmpresaVendaTable.reloadData(undefined,false);
        }

        Europa.Informacao.PosAcao(res);
    });
};

