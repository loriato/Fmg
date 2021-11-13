$(function () {
});

function TableSupervisor($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.HierarquiaCoordenador.SupervisorTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.HierarquiaCoordenador.SupervisorTable;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('NomeSupervisor').withTitle(Europa.i18n.Messages.Nome).withOption("width", "100px"),
            DTColumnBuilder.newColumn('Ativo').withTitle(Europa.i18n.Messages.Ativo).withOption("width", "100px").renderWith(renderFlag)
        ])
        .setAutoInit(false)
        .setIdAreaHeader("supervisor_datatable_barra")
        .setOptionsMultiSelect('POST', Europa.Controllers.HierarquiaCoordenador.UrlListarSupervisores, Europa.Controllers.HierarquiaCoordenador.FiltroSupervisor);

    function renderFlag(data, type, full, meta) {

        if (!Europa.Controllers.HierarquiaCoordenador.Permissoes.Incluir) {
            return "";
        }

        var checkBox = '<label>';
        checkBox += '<input id=cs_' + meta.row + ' type = "checkbox" value = "' + data + '"';

        if (data) {
            checkBox += ' checked="checked"';
            checkBox += 'onchange = "Europa.Controllers.HierarquiaCoordenador.OnUnJoinCoordenadorSupervisor(' + meta.row + ')" /> ';
        } else {
            checkBox += 'onchange = "Europa.Controllers.HierarquiaCoordenador.OnJoinCoordenadorSupervisor(' + meta.row + ')" /> ';
        }

        checkBox += '</label>';
        return checkBox;
    }
};

DataTableApp.controller('SupervisorDatatable', TableSupervisor);

Europa.Controllers.HierarquiaCoordenador.FiltroSupervisor = function () {
    var params = {
        IdCoordenador: Europa.Controllers.HierarquiaCoordenador.CoordenadorId,
        NomeSupervisor: $("#nomeSupervisor").val()
    };

    return params;
};

Europa.Controllers.HierarquiaCoordenador.FiltrarSupervisor = function () {
    if (Europa.Controllers.HierarquiaCoordenador.CoordenadorId == undefined) {
        var res = {
            Sucesso: true,
            Mensagens: ["Selecione um coordenador"]
        };

        Europa.Informacao.PosAcao(res);

        return;
    }
    Europa.Controllers.HierarquiaCoordenador.SupervisorTable.reloadData(undefined, false);
};

Europa.Controllers.HierarquiaCoordenador.LimparFiltroSupervisor = function () {
    $("#nomeSupervisor").val("");
};


Europa.Controllers.HierarquiaCoordenador.OnJoinCoordenadorSupervisor = function (row) {
    var full = Europa.Controllers.HierarquiaCoordenador.SupervisorTable.getRowData(row);

    var data = {
        Id: full.IdCoordenadorSupervisor,
        Coordenador: { Id: full.IdCoordenador },
        Supervisor: { Id: full.IdSupervisor }
    };

    $.post(Europa.Controllers.HierarquiaCoordenador.UrlOnJoinCoordenadorSupervisor, { coordenadorSupervisor: data }, function (resposta) {
        if (resposta.Sucesso) {
            Europa.Controllers.HierarquiaCoordenador.FiltrarSupervisor();

            Europa.Controllers.HierarquiaCoordenador.CoordenadorTable.getRowsSelect().TipoHierarquiaCicloFinanceiro = 1;

            $("#tab_cv").prop("hidden", true)
            
        } else {
            Europa.Informacao.PosAcao(resposta);
            $("#cs_" + row).prop("checked", false);
        }
    });

};

Europa.Controllers.HierarquiaCoordenador.OnUnJoinCoordenadorSupervisor = function (row) {
    var full = Europa.Controllers.HierarquiaCoordenador.SupervisorTable.getRowData(row);

    var data = {
        Id: full.IdCoordenadorSupervisor,
        Coordenador: { Id: full.IdCoordenador },
        Supervisor: { Id: full.IdSupervisor }
    };

    $.post(Europa.Controllers.HierarquiaCoordenador.UrlOnUnJoinCoordenadorSupervisor, { coordenadorSupervisor: data }, function (resposta) {
        if (resposta.Sucesso) {
            Europa.Controllers.HierarquiaCoordenador.FiltrarSupervisor();
            console.log(resposta.Objeto)
            switch (resposta.Objeto) {
                case 1:
                    $("#tab_cv").prop("hidden", true)
                    $("#tab1").click()
                    break;
                case 2:
                    $("#tab_cs").prop("hidden", true)
                    $("#tab2").click()
                    break;
                default:
                    $("#tab_cv").prop("hidden", false)
                    $("#tab_cs").prop("hidden", false)
                    break;
            }

            Europa.Controllers.HierarquiaCoordenador.CoordenadorTable.getRowsSelect().TipoHierarquiaCicloFinanceiro = resposta.Objeto;

        } else {
            Europa.Informacao.PosAcao(resposta);
            $("#cs_" + row).prop("checked", true);
        }
    });

};