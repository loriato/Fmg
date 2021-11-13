Europa.Components.Datatable.Usuario = {};
Europa.Components.Datatable.Usuario.IsModal = false;
Europa.Components.Datatable.Usuario.DataTable = {};
Europa.Components.Datatable.Usuario.listAction = "";

$(function () {
    $("#IdSistema").change(function () {
        Europa.Components.Datatable.Usuario.DataTable.reloadData();
    })
});

Europa.Components.Datatable.Usuario.createDT = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Components.Datatable.Usuario.DataTable = dataTableWrapper;

    function formatSitu(value) {
        var result = Europa.i18n.Enum.Resolve('Situacao', value);
        return result;
    }

    dataTableWrapper
        .setIdAreaHeader("UsuarioDatatable_barra")
        .setColumns([
            DTColumnBuilder.newColumn('NomeUsuario').withTitle(Europa.i18n.Messages.Nome).withOption('width', '150px'),
            DTColumnBuilder.newColumn('Email').withTitle(Europa.i18n.Messages.Email).withOption('width', '150px'),
            DTColumnBuilder.newColumn('Perfis').withTitle(Europa.i18n.Messages.Perfil).withOption('width', '200px'),
            DTColumnBuilder.newColumn('Login').withTitle(Europa.i18n.Messages.Login).withOption('width', '100px'),
            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).renderWith(formatSitu).withOption('width', '50px')
        ])
        .setColActions(Europa.Components.Datatable.Usuario.ActionsHtml, "50px")
        .setOptionsMultiSelect('POST', Europa.Components.Datatable.Usuario.listAction, Europa.Components.Datatable.Usuario.filterParams);

    $scope.edit = function (rowNr) {
        var id = Europa.Components.Datatable.Usuario.DataTable.getRowData(rowNr).Id;
        Europa.Components.Datatable.Usuario.Edit(id);
    }
};

Europa.Components.Datatable.Usuario.AtivarRegistrosSelecionados = function () {
    Europa.Components.Datatable.Usuario.PreAlterarSituacao(1);
}

Europa.Components.Datatable.Usuario.SuspenderRegistrosSelecionados = function () {
    Europa.Components.Datatable.Usuario.PreAlterarSituacao(2);
}

Europa.Components.Datatable.Usuario.CancelarRegistrosSelecionados = function () {
    Europa.Components.Datatable.Usuario.PreAlterarSituacao(3);
}

Europa.Components.Datatable.Usuario.GetSelectedObjectsIds = function () {
    var objetosSelecionados = Europa.Components.Datatable.Usuario.DataTable.getRowsSelect();
    var ids = [];
    objetosSelecionados.map(function (x) {
        ids.push(x.Id);
    });
    return ids;
};

Europa.Components.Datatable.Usuario.PreAlterarSituacao = function (action) {
    var ids = Europa.Components.Datatable.Usuario.GetSelectedObjectsIds();
    var situ = Europa.i18n.Enum.Resolve('SituacaoUsuario', action);
    if (ids.length > 0) {
        Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AlterarSituacao,
            Europa.String.Format(Europa.i18n.Messages.AlterarSituacaoPara, situ));
        Europa.Confirmacao.ConfirmCallback = function () {
            Europa.Components.Datatable.Usuario.AlterarSituacao(action, ids);
        }
        Europa.Confirmacao.Show();
    } else {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.AlterarSituacao, Europa.i18n.Messages.SelecioneRegistro);
        Europa.Informacao.Show();
    }
};

Europa.Components.Datatable.Usuario.AlterarSituacao = function (action, ids) {
    var url = "";
    if (action == 1) {
        url = Europa.Components.Datatable.Usuario.ActionAtivar;
    }
    if (action == 2) {
        url = Europa.Components.Datatable.Usuario.ActionSuspender;
    }
    if (action == 3) {
        url = Europa.Components.Datatable.Usuario.ActionCancelar;
    }
    $.ajax({
        type: "Post",
        traditional: true,
        url: url,
        data: { situ: action, ids: ids }
    }).done(function (response) {
        if (response.Sucesso) {
            Europa.Components.Datatable.Usuario.DataTable.reloadData();
        }
    });
};

//#region Modal de Filtro
Europa.Components.Datatable.Usuario.AbrirModalFiltro = function () {
    Europa.Components.Modal.FiltroUsuario.AbrirModal(Europa.Components.Datatable.Usuario.SelectModalFiltro);
}

Europa.Components.Datatable.Usuario.SelectModalFiltro = function () {
    Europa.Components.Datatable.Usuario.DataTable.reloadData();
}

//#endregion

//#region Modal de Formulário De Usuário
Europa.Components.Datatable.Usuario.Edit = function (id) {
    Europa.Components.Modal.FormularioUsuario.EditOrCreate(id);
    Europa.Components.Datatable.Usuario.AbrirModalFormularioUsuario();
}

Europa.Components.Datatable.Usuario.AbrirModalFormularioUsuario = function () {
    Europa.Components.Modal.FormularioUsuario.AbrirModal(Europa.Components.Datatable.Usuario.SelectModalFormularioUsuario);
}

Europa.Components.Datatable.Usuario.SelectModalFormularioUsuario = function (model) {
    Europa.Components.Datatable.Usuario.Salvar(model);
}
//#endregion

DataTableApp.controller('UsuarioDataTable', Europa.Components.Datatable.Usuario.createDT);

Europa.Components.Datatable.Usuario.Salvar = function (model) {
    var idValue;
    if (model["Usuario.Id"] === undefined) {
        var result = model.split("&");
        var result2 = result[0].split("=");
        idValue = result2[1];
    } else {
        idValue = Number(model["Usuario.Id"]);
    }
    var isCreating = true;
    if (idValue !== "") {
        isCreating = false;
    }

    $.ajax({
        type: "Post",
        url: Europa.Components.Datatable.Usuario.ActionSalvar,
        data: { model, isFullUserUpdate: true }
    }).done(function (response) {
        if (response.Sucesso) {
            if (isCreating) {
                Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Sucesso, Europa.String.Format(Europa.i18n.Messages.UsuarioCriadoSucesso, response.Objeto.Login));
            } else {
                Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Sucesso, Europa.String.Format(Europa.i18n.Messages.UsuarioAlteradoSucesso, response.Objeto.Login));
            }
            Europa.Informacao.Show();
            Europa.Components.Datatable.Usuario.DataTable.reloadData();
            Europa.Components.Modal.FormularioUsuario.CloseModal();
        } else {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.Components.Modal.FormularioUsuario.FormatarMensagem(response.Mensagens));
            Europa.Informacao.Show();
        }
    });
}

Europa.Components.Datatable.Usuario.filterParams = function () {
    var filtroForm = $("#formFiltroUsuario");
    var params = {};

    if (Europa.Components.Datatable.Usuario.IsModal) {
        params.tipos = [];
        if (document.getElementById("Ativos").checked) {
            params.tipos[params.tipos.length] = ("Ativo");
        }
        if (document.getElementById("Suspensos").checked) {
            params.tipos[params.tipos.length] = ("Suspenso");
        }
        if (filtroForm.find("#Nome").val() != null && filtroForm.find("#Nome").val() != "") {
            params.name = filtroForm.find("#Nome").val();
        }
        if (filtroForm.find("#Login").val() != null && filtroForm.find("#Login").val() != "") {
            params.login = filtroForm.find("#Login").val();
        }
        if (filtroForm.find("#Email").val() != null && filtroForm.find("#Email").val() != "") {
            params.email = filtroForm.find("#Email").val();
        }
        params.perfil = Europa.Components.Modal.FiltroUsuario.AutoCompletePerfil.Value();
        params.area = Europa.Components.Modal.FiltroUsuario.AutoCompleteArea.Value();
    } else {
        params.nameOrEmail = $("#filtroNomeEmail").val();
    }
    params.idSistema = $("#IdSistema").val();

    return params;
}



Europa.Components.Datatable.Usuario.Filtrar = function () {
    Europa.Components.Datatable.Usuario.IsModal = false;
    Europa.Components.Datatable.Usuario.DataTable.reloadData();
}

Europa.Components.Datatable.Usuario.FiltrarModal = function () {
    Europa.Components.Datatable.Usuario.IsModal = true;
    Europa.Components.Datatable.Usuario.DataTable.reloadData();
    Europa.Components.Modal.FiltroUsuario.CloseModal();
}


Europa.Components.Datatable.Usuario.ActionsHtml = function (data, type, full, meta) {
    var edit = '<button class="btn btn-default" ng-click="edit(' + meta.row + ')">' + '<i class="fa fa-pencil-square-o"></i>' + '</button>';
    var buttons = "";
    if (Europa.Components.Datatable.Usuario.permissao.atualizar === "true") {
        buttons = buttons + edit;
    }
    return buttons;
}

Europa.Components.Datatable.Usuario.ExportarUsuariosAtivos = function () {
    var params = Europa.Components.Datatable.Usuario.filterParams();
    params.order = Europa.Components.Datatable.Usuario.DataTable.lastRequestParams.order;
    params.draw = Europa.Components.Datatable.Usuario.DataTable.lastRequestParams.draw;
    var formExportacao = $("#formExportacaoUsuario");
    $("#formExportacaoUsuario").find("input").remove();
    formExportacao.attr("method", "post").attr("action", Europa.Components.Datatable.Usuario.urlExportarUsuariosAtivos);
    formExportacao.addHiddenInputData(params);
    formExportacao.submit();
};