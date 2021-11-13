$(function () {
});

function TableUsuario($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.GrupoCCA.UsuarioTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.GrupoCCA.UsuarioTable;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('NomeUsuario').withTitle(Europa.i18n.Messages.Nome).withOption("width", "100px"),
            DTColumnBuilder.newColumn('Ativo').withTitle(Europa.i18n.Messages.Ativo).withOption("width", "100px").renderWith(renderFlag)

        ])
        .setAutoInit(false)
        .setIdAreaHeader("usuario_datatable_barra")
        .setOptionsSelect('POST', Europa.Controllers.GrupoCCA.UrlListarUsuario, Europa.Controllers.GrupoCCA.FiltroUsuario);

    function renderFlag(data, type, full, meta) {

        if (Europa.Controllers.GrupoCCA.Permissoes.Incluir !== 'True') {
            return "";
        }

        var checkBox = '<label>';
        checkBox += '<input id="cb_usu_' + full.IdUsuario + '_' + full.IdGrupoCCA + '" type = "checkbox" value = "' + data + '"';

        if (data) {
            checkBox += ' checked="checked"';
        }

        checkBox += 'onchange = "Europa.Controllers.GrupoCCA.OnJoinUsuario(' + meta.row + ')" /> ';
        checkBox += '</label>';
        return checkBox;
    }

};

DataTableApp.controller('UsuarioDatatable', TableUsuario);

Europa.Controllers.GrupoCCA.FiltroUsuario = function () {
    var param = {
        IdGrupoCCA: Europa.Controllers.GrupoCCA.GrupoCCAId,
        NomeUsuario: $("#filtroUsuario").val()
    };
    return param;
};

Europa.Controllers.GrupoCCA.CarregarDadosUsuario = function (grupoId) {
    Europa.Controllers.GrupoCCA.GrupoCCAId = grupoId;
    Europa.Controllers.GrupoCCA.UsuarioTable.closeEdition();
    Europa.Controllers.GrupoCCA.UsuarioTable.reloadData();
}

Europa.Controllers.GrupoCCA.FiltrarUsuario = function () {
    if (Europa.Controllers.GrupoCCA.GrupoCCAId == 0 || Europa.Controllers.GrupoCCA.GrupoCCAId == undefined) {
        var res = {
            Sucesso: false,
            Mensagens: ["Selecione um grupo CCA"]
        }
        Europa.Informacao.PosAcao(res);
        return;
    }
    Europa.Controllers.GrupoCCA.UsuarioTable.reloadData(undefined,false);
};

Europa.Controllers.GrupoCCA.LimparFiltroUsuario = function () {
    $("#filtroUsuario").val("");
};

Europa.Controllers.GrupoCCA.OnJoinUsuario = function (row) {

    var full = Europa.Controllers.GrupoCCA.UsuarioTable.getRowData(row);

    var data = {
        Id: full.IdUsuarioGrupoCCA,
        GrupoCCA: { Id: full.IdGrupoCCA },
        Usuario: { Id: full.IdUsuario }
    };

    var backup = $("#cb_usu_" + full.IdUsuario + "_" + full.IdGrupoCCA + "").val()=="true";

    $.post(Europa.Controllers.GrupoCCA.UrlAtribuirUsuarioAGrupo, { usuarioGrupoCCA: data }, function (resposta) {
        if (resposta.Sucesso) {
            Europa.Controllers.GrupoCCA.FiltrarUsuario();
        } else {
            Europa.Informacao.PosAcao(resposta);
            $("#cb_usu_" + full.IdUsuario + "_" + full.IdGrupoCCA + "").prop("checked", backup);
        }
    });

};
