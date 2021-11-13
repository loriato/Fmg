Europa.Components.Datatable.Funcionalidade = {};
Europa.Components.Datatable.Funcionalidade.GetPerfilId = undefined;
Europa.Components.Datatable.Funcionalidade.PerfilId = undefined;

Europa.Components.Datatable.Funcionalidade.createDT = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Components.Datatable.Funcionalidade.DataTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);

    function createCheckbox(id, callback, disable, checked, nome) {
        var result = '<label><input type="checkbox" id="' + id +
            '" onclick="' + callback + '" value="' + nome + '"';
        if (checked) {
            result = result + ' checked="checked"';
        }
        if (nome !== undefined) {
            result = result + ' value="' + nome + '"';
        }
        if (disable) {
            result = result + ' disabled="disabled"';
        }
        result = result + '></label>';
        return result;
    }

    function formatPermitido(value, type, full) {
        var nome = full.NomeFuncionalidade;
        var id = "permitido_" + full.Id;
        var isDisable=false;
        var isChecked;
        if (value) {
            isChecked = true;
            if (Europa.Components.Datatable.Funcionalidade.permissao.Excluir === "false") {
                isDisable = true;
            }
        } else {
            isChecked = false;
            if (Europa.Components.Datatable.Funcionalidade.permissao.Atualizar === "false") {
                isDisable = true;
            }
        }
        var callback = "Europa.Components.Datatable.Funcionalidade.MudarPermissao(" + full.Id + ","+isChecked+")";
        var result = createCheckbox(id, callback, isDisable, isChecked, nome);
        return result;
    }

    function formatUF(value, type, full) {
        return full.CodigoUF + ' - ' + full.NomeUF;
    }

    function formatLogar(value, type, full) {
        var id = "logar_" + full.Id;
        var isDisable = Europa.Components.Datatable.Funcionalidade.permissao.Atualizar === "false";
        var isChecked;
        if (value) {
            isChecked = true;
        } else {
            isChecked = false;
        }
        var callback = "Europa.Components.Datatable.Funcionalidade.MudarFlagLogar(" + full.Id + "," + isChecked + ")";
        var result = createCheckbox(id, callback, isDisable, isChecked);
        return result;
    }

    Europa.Components.Datatable.Funcionalidade.DataTable
        .setIdAreaHeader("FuncionalidadeDatatable_barra")
        .setColumns([
            DTColumnBuilder.newColumn('NomeUF').withTitle(Europa.i18n.Messages.UnidadeFuncional).renderWith(formatUF),
            DTColumnBuilder.newColumn('NomeFuncionalidade').withTitle(Europa.i18n.Messages.Funcionalidade),
            DTColumnBuilder.newColumn('Permitida').withTitle(Europa.i18n.Messages.Permitida).renderWith(formatPermitido).withClass('dt-body-center').withOption("width", "80px"),
            DTColumnBuilder.newColumn('Logar').withTitle(Europa.i18n.Messages.Logar).renderWith(formatLogar).withClass('dt-body-center').withOption("width", "80px"),
        ])
        .setDefaultLength(25)
        .setAutoInit(false)
        .setOptionsSelect('POST', Europa.Components.Datatable.Funcionalidade.listAction, Europa.Components.Datatable.Funcionalidade.filterParams);
};

Europa.Components.Datatable.Funcionalidade.MudarPermissao = function(item,existe) {
    $("#permitido_" + item).attr("onclick", "Europa.Components.Datatable.Funcionalidade.MudarPermissao(" + item + "," + !existe+")");
    var nome = $("#permitido_" + item).val();
    var url;
    if (!existe) {
        url = Europa.Components.Datatable.Funcionalidade.AssociarPermissao;
    } else {
        url = Europa.Components.Datatable.Funcionalidade.RemoverPermissao;
    }
    var params = {};
    params.funcionalidade = item;
    params.perfil = Europa.Components.Datatable.Perfil.DataTable.getRowsSelect().Id;
    $.ajax({
        type: "Post",
        url: url,
        data: params
    }).done(function (response) {
        if (!response.Sucesso) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.Components.Datatable.Funcionalidade.FormatarMensagem(response.Mensagens));
            Europa.Informacao.Show();
        }
    });
}

Europa.Components.Datatable.Funcionalidade.MudarFlagLogar = function (item, existe) {
    $("#logar_" + item).attr("onclick", "Europa.Components.Datatable.Funcionalidade.MudarFlagLogar(" + item + "," + !existe + ")");
    var params = {};
    params.funcionalidade = item;
    params.logar = !existe;
    $.ajax({
        type: "Post",
        url: Europa.Components.Datatable.Funcionalidade.MudarFlagLog,
        data: params
    }).done(function (response) {
        if (!response.Sucesso) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.Components.Datatable.Funcionalidade.FormatarMensagem(response.Mensagens));
            Europa.Informacao.Show();
        }
    });
}

Europa.Components.Datatable.Funcionalidade.FormatarMensagem = function (msgs) {
    var mensagensHtml = $('<div/>');
    if ($.isArray(msgs) && msgs.length > 0) {
        msgs.map(function (x) {
            mensagensHtml.append($('<p/>').text(x));
        });
    } else {
        mensagensHtml.append($('<p/>').text(msgs));
    }
    return mensagensHtml;
}


DataTableApp.controller('FuncionalidadeDataTable', Europa.Components.Datatable.Funcionalidade.createDT);

Europa.Components.Datatable.Funcionalidade.filterParams = function () {
    var params = {};
    if (Europa.Components.Datatable.Funcionalidade.GetPerfilId !== undefined) {
        params.nome = $("#filtroNomeFunc").val();
        params.nomeUF = $("#filtroNomeUF").val();
        params.perfil = Europa.Components.Datatable.Funcionalidade.GetPerfilId();
        params.idSistema = $("#IdSistema").val();
    }
    return params;
}

$(document).ready(function() {
    Europa.Components.Datatable.Funcionalidade.GetPerfilId = function() {
        if (Europa.Components.Datatable.Funcionalidade.PerfilId !== undefined) {
            return Europa.Components.Datatable.Funcionalidade.PerfilId;
        } else if (Europa.Components.Datatable.Perfil.DataTable.getRowsSelect() !== undefined) {
            return Europa.Components.Datatable.Perfil.DataTable.getRowsSelect().Id;
        } else {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.i18n.Messages.NecessarioDefinirPerfil);
            Europa.Informacao.Show();
        }
        return undefined;
    };
    $("#IdSistema").change(function () {
        Europa.Components.Datatable.Funcionalidade.DataTable.reloadData();
    });
});

Europa.Components.Datatable.Funcionalidade.CarregarDados = function (perfilId) {
    Europa.Components.Datatable.Funcionalidade.PerfilId = perfilId;
    Europa.Components.Datatable.Funcionalidade.DataTable.reloadData();
}

Europa.Components.Datatable.Funcionalidade.LimparFiltros = function() {
    $("#filtroNomeUF").val("");
    $("#filtroNomeFunc").val("");
};