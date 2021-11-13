
$(function () {

});

function TableAvalista($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.EstadoAvalista.AvalistaTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.EstadoAvalista.AvalistaTable;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('NomeAvalista').withTitle(Europa.i18n.Messages.Nome).withOption("width", "100px"),
            DTColumnBuilder.newColumn('Ativo').withTitle(Europa.i18n.Messages.Ativo).withOption("width", "100px").renderWith(renderFlag)

        ])
        .setAutoInit(false)
        .setIdAreaHeader("datatable_avalista_barra")
        .setOptionsSelect('POST', Europa.Controllers.EstadoAvalista.UrlListarAvalista, Europa.Controllers.EstadoAvalista.FiltroAvalista);

    function renderFlag(data, type, full, meta) {

        //if (Europa.Controllers.EstadoAvalista.Permissoes.Incluir !== 'True') {
        //    return "";
        //}

        var checkBox = '<label>';
        checkBox += '<input type = "checkbox" value = "' + data + '"';

        if (data) {
            checkBox += ' checked="checked"';
        }

        checkBox += 'onchange = "Europa.Controllers.EstadoAvalista.OnJoinAvalista(' + meta.row + ')" /> ';
        checkBox += '</label>';
        return checkBox;
    }

};


DataTableApp.controller('DatatableAvalista', TableAvalista);

Europa.Controllers.EstadoAvalista.FiltroAvalista = function () {
    var param = {
        NomeAvalista: $("#filtroAvalista").val(),
        Estado: Europa.Controllers.EstadoAvalista.EstadoSelecionado,
    };

    return param;
};

Europa.Controllers.EstadoAvalista.FiltrarAvalistas = function () {
    Europa.Controllers.EstadoAvalista.AvalistaTable.reloadData();
};

Europa.Controllers.EstadoAvalista.LimparFiltroAvalista = function () {
    $("#filtroAvalista").val("");
};

Europa.Controllers.EstadoAvalista.OnJoinAvalista = function (row) {

    var full = Europa.Controllers.EstadoAvalista.AvalistaTable.getRowData(row);
    var data = {
        Id: full.Id,
        Estado: Europa.Controllers.EstadoAvalista.EstadoSelecionado,
        IdAvalista: full.IdAvalista,
        NomeAvalista: full.NomeAvalista,
        Ativo: full.Ativo,
        IdEstadoAvalista: full.IdEstadoAvalista
    };

    $.post(Europa.Controllers.EstadoAvalista.UrlOnJoin, { filtro: data }, function (resposta) {
        if (resposta.Sucesso) {
            Europa.Controllers.EstadoAvalista.FiltrarAvalistas();
        } else {
            Europa.Informacao.PosAcao(resposta);
        }
    });

};
