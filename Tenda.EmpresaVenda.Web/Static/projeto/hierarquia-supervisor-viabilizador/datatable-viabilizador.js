$(function () {
});

function TableViabilizador($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.HierarquiaSupervisorViabilizador.ViabilizadorTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.HierarquiaSupervisorViabilizador.ViabilizadorTable;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('NomeViabilizador').withTitle(Europa.i18n.Messages.Nome).withOption("width", "100px"),
            DTColumnBuilder.newColumn('Ativo').withTitle(Europa.i18n.Messages.Ativo).withOption("width", "100px").renderWith(renderFlag)
        ])
        .setAutoInit(false)
        .setIdAreaHeader("viabilizador_datatable_barra")
        .setOptionsMultiSelect('POST', Europa.Controllers.HierarquiaSupervisorViabilizador.UrlListarViabilizadores, Europa.Controllers.HierarquiaSupervisorViabilizador.FiltroViablilizador);

    function renderFlag(data, type, full, meta) {

        if (!Europa.Controllers.HierarquiaSupervisorViabilizador.Permissoes.Incluir) {
            return "";
        }

        var checkBox = '<label>';
        checkBox += '<input id=sv_' + meta.row + ' type = "checkbox" value = "' + data + '"';

        if (data) {
            checkBox += ' checked="checked"';
        }

        checkBox += 'onchange = "Europa.Controllers.HierarquiaSupervisorViabilizador.OnJoinSupervisorViabilizador(' + meta.row + ')" /> ';
        checkBox += '</label>';
        return checkBox;
    }      
};

DataTableApp.controller('ViabilizadorDatatable', TableViabilizador);

Europa.Controllers.HierarquiaSupervisorViabilizador.FiltroViablilizador = function () {
    var params = {
        IdSupervisor: Europa.Controllers.HierarquiaSupervisorViabilizador.SupervisorId,
        NomeViabilizador: $("#nomeViabilizador").val()
    };

    return params;
};

Europa.Controllers.HierarquiaSupervisorViabilizador.FiltrarViabilizador = function () {
    if (Europa.Controllers.HierarquiaSupervisorViabilizador.SupervisorId == undefined) {
        var res = {
            Sucesso: true,
            Mensagens: ["Selecione um surpevisor"]
        };

        Europa.Informacao.PosAcao(res);

        return;
    }
    Europa.Controllers.HierarquiaSupervisorViabilizador.ViabilizadorTable.reloadData(undefined, false);
};

Europa.Controllers.HierarquiaSupervisorViabilizador.LimparFiltroUsuario = function () {
    $("#nomeViabilizador").val("");
};

Europa.Controllers.HierarquiaSupervisorViabilizador.OnJoinSupervisorViabilizador = function (row) {

    var full = Europa.Controllers.HierarquiaSupervisorViabilizador.ViabilizadorTable.getRowData(row);

    var data = {
        Id: full.IdSupervisorViabilizador,
        Supervisor: { Id: full.IdSupervisor },
        Viabilizador: { Id: full.IdViabilizador }
    };

    $.post(Europa.Controllers.HierarquiaSupervisorViabilizador.UrlOnJoinSupervisorViabilizador, { supervisorViabilizador: data }, function (resposta) {
        if (resposta.Sucesso) {
            Europa.Controllers.HierarquiaSupervisorViabilizador.FiltrarViabilizador();
        } else {
            Europa.Informacao.PosAcao(resposta);
            $("#sv_" + row).prop("checked", false);
        }
    });

};

Europa.Controllers.HierarquiaSupervisorViabilizador.SelecionarTodosViabilizadores = function () {
    var viabilizadores = Europa.Controllers.HierarquiaSupervisorViabilizador.ViabilizadorTable.getRowsSelect();

    if (viabilizadores.length == 0) {
        var res = {
            Sucesso: false,
            Mensagens: ["Selecione um viabilizador"]
        };

        Europa.Informacao.PosAcao(res);

        return;
    }

    var lista = [];

    viabilizadores.map(function (full) {
        var data = {
            Id: full.IdSupervisorViabilizador,
            Supervisor: { Id: full.IdSupervisor },
            Viabilizador: { Id: full.IdViabilizador }
        };

        lista.push(data)
    });

    $.post(Europa.Controllers.HierarquiaSupervisorViabilizador.UrlSelecionarTodosViabilizadores,
        { lista: lista },
        function (res) {
        if (res.Sucesso) {
            Europa.Controllers.HierarquiaSupervisorViabilizador.FiltrarViabilizador();
        }
        Europa.Informacao.PosAcao(res);
    });

};