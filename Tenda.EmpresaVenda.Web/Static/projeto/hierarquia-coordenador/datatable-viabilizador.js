$(function () {
});

function TableViabilizador($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.HierarquiaCoordenador.ViabilizadorTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.HierarquiaCoordenador.ViabilizadorTable;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('NomeViabilizador').withTitle(Europa.i18n.Messages.Nome).withOption("width", "100px"),
            DTColumnBuilder.newColumn('Ativo').withTitle(Europa.i18n.Messages.Ativo).withOption("width", "100px").renderWith(renderFlag)
        ])
        .setAutoInit(false)
        .setIdAreaHeader("viabilizador_datatable_barra")
        .setOptionsMultiSelect('POST', Europa.Controllers.HierarquiaCoordenador.UrlListarViabilizadores, Europa.Controllers.HierarquiaCoordenador.FiltroViabilizador);

    function renderFlag(data, type, full, meta) {

        if (!Europa.Controllers.HierarquiaCoordenador.Permissoes.Incluir) {
            return "";
        }

        var checkBox = '<label>';
        checkBox += '<input id=cv_' + meta.row + ' type = "checkbox" value = "' + data + '"';

        if (data) {
            checkBox += ' checked="checked"';
            checkBox += 'onchange = "Europa.Controllers.HierarquiaCoordenador.OnUnJoinCoordenadorViabilizador(' + meta.row + ')" /> ';
        } else {
            checkBox += 'onchange = "Europa.Controllers.HierarquiaCoordenador.OnJoinCoordenadorViabilizador(' + meta.row + ')" /> ';
        }

        checkBox += '</label>';
        return checkBox;
    }
};

DataTableApp.controller('ViabilizadorDatatable', TableViabilizador);

Europa.Controllers.HierarquiaCoordenador.FiltroViabilizador = function () {
    var params = {
        IdCoordenador: Europa.Controllers.HierarquiaCoordenador.CoordenadorId,
        NomeViabilizador: $("#nomeViabilizador").val()
    };

    return params;
};

Europa.Controllers.HierarquiaCoordenador.FiltrarViabilizador = function () {
    if (Europa.Controllers.HierarquiaCoordenador.CoordenadorId == undefined) {
        var res = {
            Sucesso: true,
            Mensagens: ["Selecione um surpevisor"]
        };

        Europa.Informacao.PosAcao(res);

        return;
    }
    Europa.Controllers.HierarquiaCoordenador.ViabilizadorTable.reloadData(undefined, false);
};

Europa.Controllers.HierarquiaCoordenador.LimparFiltroViabilizador = function () {
    $("#nomeViabilizador").val("");
};


Europa.Controllers.HierarquiaCoordenador.OnJoinCoordenadorViabilizador = function (row) {
    var full = Europa.Controllers.HierarquiaCoordenador.ViabilizadorTable.getRowData(row);

    var data = {
        Id: full.IdCoordenadorViabilizador,
        Coordenador: { Id: full.IdCoordenador },
        Viabilizador: { Id: full.IdViabilizador }
    };

    $.post(Europa.Controllers.HierarquiaCoordenador.UrlOnJoinCoordenadorViabilizador, { coordenadorViabilizador: data }, function (resposta) {
        if (resposta.Sucesso) {
            Europa.Controllers.HierarquiaCoordenador.FiltrarViabilizador();

            Europa.Controllers.HierarquiaCoordenador.CoordenadorTable.getRowsSelect().TipoHierarquiaCicloFinanceiro = 2;

            $("#tab_cs").prop("hidden", true)
        } else {
            Europa.Informacao.PosAcao(resposta);
            $("#cv_" + row).prop("checked", false);
        }
    });

};

Europa.Controllers.HierarquiaCoordenador.OnUnJoinCoordenadorViabilizador = function (row) {
    var full = Europa.Controllers.HierarquiaCoordenador.ViabilizadorTable.getRowData(row);

    var data = {
        Id: full.IdCoordenadorViabilizador,
        Coordenador: { Id: full.IdCoordenador },
        Viabilizador: { Id: full.IdViabilizador }
    };

    $.post(Europa.Controllers.HierarquiaCoordenador.UrlOnUnJoinCoordenadorViabilizador, { coordenadorViabilizador: data }, function (resposta) {
        if (resposta.Sucesso) {
            Europa.Controllers.HierarquiaCoordenador.FiltrarViabilizador();
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
            $("#cv_" + row).prop("checked", true);
        }
    });

};