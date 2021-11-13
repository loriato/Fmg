Europa.Controllers.PerfilModal = {};

$(document).ready(function () {

});

Europa.Controllers.PerfilModal.AbrirModal = function () {
    $("#Nome").val('');
    $("#edicao_perfil").modal("show");
};

Europa.Controllers.PerfilModal.CloseModal = function () {
    $("#edicao_perfil").modal("hide");
};

Europa.Controllers.PerfilModal.Salvar = function () {
    var data = { nome: $("#Nome").val() }
    $.post(Europa.Controllers.PerfilModal.UrlSalvar, data, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.PerfilModal.CloseModal();
            if (Europa.Components.Datatable.Perfil.DataTable != undefined) {
                Europa.Components.Datatable.Perfil.DataTable.reloadData();
            }
        }
        Europa.Controllers.PerfilModal.ShowMessages(res);
    });
};

Europa.Controllers.PerfilModal.ShowMessages = function (res) {
    if (res.Mensagens && res.Mensagens.length) {
        ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, Europa.Messages.Format(res.Mensagens));
        Show();
    }
};