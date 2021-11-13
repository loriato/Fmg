Europa.Controllers.ParametroSistema = {};
Europa.Controllers.ParametroSistema.UrlListarFluxoVendas = undefined;

$(document).ready(function () {
    Europa.Controllers.ParametroSistema.AutoCompletePerfil = new Europa.Components
        .AutoCompletePerfil()
        .WithTargetSuffix("perfil")
        .Configure();

    Europa.Controllers.ParametroSistema.AutoCompletePerfil.SetValue($("#PerfilInicial_Id").val(), $("#PerfilInicial_Nome").val());

    $(".select2").css('z-index', 30);
});

Europa.Controllers.ParametroSistema.AbrirAnexos = function () {
    Europa.Controllers.ServicoGerenciamentoAnexo.AbrirDominioAnexo($("#Id").val(), Europa.Controllers.ServicoGerenciamentoAnexo.Dominio.ParametrosSistema, false);
};

Europa.Controllers.ParametroSistema.ActionSelecionar = function (item) {
    Europa.Controllers.ParametroSistema.AutoCompletePerfil.SetValue(item.Id, item.Nome);
}

Europa.Controllers.ParametroSistema.AbrirModal = function () {
    Europa.Components.ModalPerfil
        .WithSelectFuncion(Europa.Controllers.ParametroSistema.ActionSelecionar)
        .Show();
}

Europa.Controllers.ParametroSistema.SubmitForm = function () {
    $('#PerfilInicial_Id').val(Europa.Controllers.ParametroSistema.AutoCompletePerfil.Value());

    var data = Europa.Form.SerializeJson("#formParametroSistema");
    $.ajax({
        type: "Post",
        url: Europa.Controllers.ParametroSistema.UrlSalvar,
        data: data
    }).done(function (response) {
        if (response.Sucesso) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Sucesso, Europa.String.Format(Europa.i18n.Messages.ParametroSalvoSucesso, $("#Sistema_Nome").val()));
            Europa.Informacao.ShowAsSuccess();
        } else {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.Controllers.ParametroSistema.FormatarMensagem(response.Mensagens));
            Europa.Informacao.Show();
        }
    });
}

Europa.Controllers.ParametroSistema.FormatarMensagem = function (msgs) {
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