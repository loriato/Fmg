Europa.Components.Modal.FormularioUsuario = {};

//#region properties
Europa.Components.Modal.FormularioUsuario.Selecionar = undefined;
Europa.Components.Modal.FormularioUsuario.SelectorForm = undefined;
Europa.Components.Modal.FormularioUsuario.SelectorModal = undefined;
Europa.Components.Modal.FormularioUsuario.Datatable = undefined;
Europa.Components.Modal.FormularioUsuario.SelectorModalTitle = undefined;
Europa.Components.Modal.FormularioUsuario.AutoCompletePerfil = undefined;
Europa.Components.Modal.FormularioUsuario.AutoCompleteArea = undefined;
Europa.Components.Modal.FormularioUsuario.AutoCompleteRegional = undefined;
Europa.Components.Modal.FormularioUsuario.CallbackAction = undefined;
Europa.Components.Modal.FormularioUsuario.GetPerfis = undefined;


//#endregion

$(document).ready(function () {
    Europa.Components.Modal.FormularioUsuario.SelectorModal = $("#usuarioModal");
    Europa.Components.Modal.FormularioUsuario.SelectorModalTitle = $("#usuarioModal_titulo");
    Europa.Components.Modal.FormularioUsuario.SelectorForm = $("#usuarioModal_formulario");
    Europa.Components.Modal.FormularioUsuario.AutoCompletePerfil = new Europa.Components.AutoCompletePerfil()
        .WithTargetSuffix("perfil_usuarioModal_formulario")
        .Configure();
    Europa.Components.Modal.FormularioUsuario.AutoCompleteArea = new Europa.Components.AutoCompleteAreaNegocio()
        .WithTargetSuffix("area_usuarioModal_formulario")
        .Configure();
    Europa.Components.Modal.FormularioUsuario.AutoCompleteRegional = new Europa.Components.AutoCompleteRegional()
        .WithTargetSuffix("regional_usuarioModal_formulario")
        .Configure();
});

Europa.Components.Modal.FormularioUsuario.EditOrCreate = function (id) {
    Europa.Components.Modal.FormularioUsuario.SetAsView(false);
    if (id !== undefined) {
        var model = Europa.Components.Modal.FormularioUsuario.GetModel(id);
        Europa.Components.Modal.FormularioUsuario.FillForm(model);
        Europa.Components.Modal.FormularioUsuario.SelectorModalTitle.text(Europa.i18n.Messages.AlterarUsuario);
        Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Login").prop("readonly", true);
        Europa.Components.Modal.FormularioUsuario.Datatable.reloadData();
    } else {
        Europa.Components.Modal.FormularioUsuario.Clear();
        Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Login").prop("readonly", false);
        Europa.Components.Modal.FormularioUsuario.SelectorModalTitle.text(Europa.i18n.Messages.IncluirUsuario);
    }
};

Europa.Components.Modal.FormularioUsuario.View = function (id) {
    var model = Europa.Components.Modal.FormularioUsuario.GetModel(id);
    Europa.Components.Modal.FormularioUsuario.FillForm(model);
    Europa.Components.Modal.FormularioUsuario.SetAsView();
    Europa.Components.Modal.FormularioUsuario.AbrirModal();
};

Europa.Components.Modal.FormularioUsuario.GetModel = function (id) {
    var model;
    $.ajax({
        type: "Get",
        url: Europa.Components.Modal.FormularioUsuario.GetUsuario,
        async: false,
        data: {
            id: id
        }
    }).done(function (response) {
        model = response;
    });
    return model;
};

Europa.Components.Modal.FormularioUsuario.AbrirModal = function (callback) {
    Europa.Components.Modal.FormularioUsuario.SelectorModal.modal("show");
    Europa.Components.Modal.FormularioUsuario.CallbackAction = callback;
};

Europa.Components.Modal.FormularioUsuario.MainAction = function () {
    Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#PerfilId")
        .val(Europa.Components.Modal.FormularioUsuario.AutoCompletePerfil.Value());
    Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Area_Id")
        .val(Europa.Components.Modal.FormularioUsuario.AutoCompleteArea.Value());
    Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Regional_Id").val(Europa.Components.Modal.FormularioUsuario.AutoCompleteRegional.Value());
    Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Situacao").val();
    var model = Europa.Form.SerializeJson("#usuarioModal_formulario");
    Europa.Components.Modal.FormularioUsuario.CallbackAction(model);
};

Europa.Components.Modal.FormularioUsuario.CloseModal = function () {
    Europa.Components.Modal.FormularioUsuario.SelectorModal.modal("hide");
};

Europa.Components.Modal.FormularioUsuario.SetAsView = function (view) {
    if (view === undefined || view) {
        Europa.Components.Modal.FormularioUsuario.SelectorModalTitle.text(Europa.i18n.Messages.VisualizarUsuario);
        Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Nome").prop("readonly", true);
        Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Email").prop("readonly", true);
        Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Login").prop("readonly", true);
        Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#usuarioModal_formulario_btn_perfil")
            .prop("disabled", true);
        Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#usuarioModal_formulario_btn_area")
            .prop("disabled", true);
        $("#usuarioModal_btnDesistir").html("<i class='fa fa-times'></i> " + Europa.i18n.Messages.Fechar);
        $("#usuarioModal_btnSelecionar").addClass("hidden");
        Europa.Components.Modal.FormularioUsuario.AutoCompletePerfil.Disable();
        Europa.Components.Modal.FormularioUsuario.AutoCompleteArea.Disable();
    } else {
        Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Nome").prop("readonly", false);
        Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Email").prop("readonly", false);
        Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#usuarioModal_formulario_btn_perfil")
            .prop("disabled", false);
        Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#usuarioModal_formulario_btn_area")
            .prop("disabled", false);
        $("#usuarioModal_btnSelecionar").removeClass("hidden");
        $("#usuarioModal_btnDesistir").html("<i class='fa fa-ban'></i> " + Europa.i18n.Messages.Desistir);
        Europa.Components.Modal.FormularioUsuario.AutoCompletePerfil.Enable();
        Europa.Components.Modal.FormularioUsuario.AutoCompleteArea.Enable();
    }
};

Europa.Components.Modal.FormularioUsuario.Clear = function () {
    Europa.Components.Modal.FormularioUsuario.SelectorForm.trigger("reset");
    Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Id").val("0");
    Europa.Components.Modal.FormularioUsuario.AutoCompletePerfil.Clean();
    Europa.Components.Modal.FormularioUsuario.AutoCompleteArea.Clean();
};

Europa.Components.Modal.FormularioUsuario.FormatarMensagem = function (msgs) {
    var mensagensHtml = $('<div/>');
    if ($.isArray(msgs) && msgs.length > 0) {
        msgs.map(function (x) {
            mensagensHtml.append($('<p/>').text(x));
        });
    } else {
        mensagensHtml.append($('<p/>').text(msgs));
    }
    return mensagensHtml;
};

Europa.Components.Modal.FormularioUsuario.createDT = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Components.Modal.FormularioUsuario.Datatable = dataTableWrapper;

    dataTableWrapper.setIdAreaHeader("usuario_datable_header_perfil")
        .setColumns([
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '50%'),
            DTColumnBuilder.newColumn('CriadoEm').withTitle(Europa.i18n.Messages.DataCriacao).renderWith(dataTableWrapper.renderDateSmall).withOption('width', '30%'),
            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).renderWith(situacaoRender)
        ])
        .setColActions(actionsHtml, '50px')
        .setAutoInit(false)
        .setOptionsSelect('POST', Europa.Components.Modal.FormularioUsuario.GetPerfis, Europa.Components.Modal.FormularioUsuario.Filter);

    $scope.excluir = function (row) {
        var data = {
            idUsuario: Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Id").val(),
            idPerfil: row
        };

        $.post(Europa.Components.Modal.FormularioUsuario.UrlExcluirPerfil,
            data,
            function (res) {
                if (res.Sucesso) {
                    Europa.Components.Modal.FormularioUsuario.Datatable.reloadData();
                    Europa.Components.Datatable.Usuario.DataTable.reloadData();
                }
                Europa.Messages.ShowMessages(res);
            });
    }

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            dataTableWrapper.renderButton(Europa.Components.Modal.FormularioUsuario.PermExcluir, Europa.i18n.Messages.Excluir, "fa fa-trash", "excluir(" + full.Id + ")") +
            '</div>';
    }
    function situacaoRender(data, type, full, meta) {
        return Europa.i18n.Enum.Resolve("Situacao", data);
    }
};

DataTableApp.controller('PerfisDataTable', Europa.Components.Modal.FormularioUsuario.createDT);

Europa.Components.Modal.FormularioUsuario.Filter = function () {
    if (Europa.Components.Modal.FormularioUsuario.SelectorForm) {
        return { idUsuario: Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Id").val() };
    }
    return 0;
};

Europa.Components.Modal.FormularioUsuario.FillForm = function (model) {
    Europa.Components.Modal.FormularioUsuario.SelectorForm.trigger("reset");
    Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Id").val(model.Id);
    Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Nome").val(model.Nome);
    Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Login").val(model.Login);
    Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Situacao").val(Europa.i18n.Enum.Resolve("Situacao", model.Situacao));
    Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Email").val(model.Email)
};

Europa.Components.Modal.FormularioUsuario.AbrirModalPerfil = function () {
    Europa.Components.ModalPerfil
        .WithSelectFuncion(Europa.Components.Modal.FormularioUsuario.ActionSelecionarPerfil)
        .Show();
};

Europa.Components.Modal.FormularioUsuario.ActionSelecionarPerfil = function (perfil) {
    var data = {
        idUsuario: Europa.Components.Modal.FormularioUsuario.SelectorForm.find("#Usuario_Id").val(),
        idPerfil: perfil.Id
    };
    $.post(Europa.Components.Modal.FormularioUsuario.UrlIncluirPerfil,
        data,
        function (res) {
            if (res.Sucesso) {
                Europa.Components.Modal.FormularioUsuario.Datatable.reloadData();
                Europa.Components.Datatable.Usuario.DataTable.reloadData();
            }
            Europa.Messages.ShowMessages(res);
        });
};

Europa.Components.Modal.FormularioUsuario.AbrirModalAreas = function () {
    Europa.Components.AreaNegocioModal.AbrirModal(Europa.Components.Modal.FormularioUsuario.ActionSelecionarAreas);
};

Europa.Components.Modal.FormularioUsuario.ActionSelecionarAreas = function (area) {
    Europa.Components.Modal.FormularioUsuario.AutoCompleteArea.SetValue(area.Id, area.Nome);
}