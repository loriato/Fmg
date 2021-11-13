$(function () {
});

function TableUsuario($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.LojaPortal.UsuarioTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.LojaPortal.UsuarioTable;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('NomeUsuario').withTitle(Europa.i18n.Messages.Nome).withOption("width", "100px"),
            DTColumnBuilder.newColumn('Ativo').withTitle(Europa.i18n.Messages.Ativo).withOption("width", "100px").renderWith(renderFlag)

        ])
        .setAutoInit(false)
        .setIdAreaHeader("usuario_datatable_barra")
        .setOptionsSelect('POST', Europa.Controllers.LojaPortal.UrlListarUsuario, Europa.Controllers.LojaPortal.FiltroUsuario);

    function renderFlag(data, type, full, meta) {
        if (Europa.Controllers.LojaPortal.Permissoes.Incluir !== 'True') {
            return "";
        }

        var checkBox = '<label>';
        checkBox += '<input type = "checkbox" value = "' + data + '"';

        if (data) {
            checkBox += ' checked="checked"';
        }

        checkBox += 'onchange = "Europa.Controllers.LojaPortal.OnJoinUsuario(' + meta.row + ')" /> ';
        checkBox += '</label>';
        return checkBox;
    }

};

DataTableApp.controller('UsuarioDatatable', TableUsuario);

Europa.Controllers.LojaPortal.FiltroUsuario = function () {
    var param = {
        IdLojaPortal: Europa.Controllers.LojaPortal.LojaPortalId,
        NomeUsuario: $("#filtroUsuario").val()
    };
    return param;
};

Europa.Controllers.LojaPortal.CarregarDadosUsuario = function (grupoId) {
    Europa.Controllers.LojaPortal.LojaPortalId = grupoId;
    Europa.Controllers.LojaPortal.UsuarioTable.closeEdition();
    Europa.Controllers.LojaPortal.UsuarioTable.reloadData();
}

Europa.Controllers.LojaPortal.FiltrarUsuario = function () {
    if (Europa.Controllers.LojaPortal.LojaPortalId == 0 || Europa.Controllers.LojaPortal.LojaPortalId == undefined) {
        var res = {
            Sucesso: false,
            Mensagens: ["Selecione uma Loja."]
        }
        Europa.Informacao.PosAcao(res);
        return;
    }
    Europa.Controllers.LojaPortal.UsuarioTable.reloadData(undefined, false);
};

Europa.Controllers.LojaPortal.LimparFiltroUsuario = function () {
    $("#filtroUsuario").val("");
};

Europa.Controllers.LojaPortal.OnJoinUsuario = function (row) {

    var full = Europa.Controllers.LojaPortal.UsuarioTable.getRowData(row);

    var data = {
        IdLoja: full.IdLoja,
        IdUsuario: full.IdUsuario,
        NomeUsuario: full.NomeUsuario,
        Estado: full.Estado,
        Email: full.Email
    };

    $.post(Europa.Controllers.LojaPortal.UrlAtribuirUsuarioALoja, data, function (resposta) {
        if (resposta.Sucesso) {
            Europa.Controllers.LojaPortal.FiltrarUsuario();
        } else {
            let input = $("input[type=checkbox").eq(row);
            input.prop("checked", !input.prop("checked"));
        }
        Europa.Informacao.PosAcao(resposta);
    });

};
