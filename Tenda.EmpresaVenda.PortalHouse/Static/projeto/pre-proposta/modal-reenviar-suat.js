Europa.Components.ModalReenviarSuat = {}

$(function () {
    $('#reenviar-suat-modal').on('shown.bs.modal', function (e) {
        Europa.Components.ModalReenviarSuat.InitAutoComplete();
    })
})

Europa.Components.ModalReenviarSuat.OpenModal = function () {
    $("#reenviar-suat-modal").modal("show");
}

Europa.Components.ModalReenviarSuat.CloseModal = function () {
    $("#reenviar-suat-modal").modal("hide");
}

Europa.Components.ModalReenviarSuat.InitAutoComplete = function () {
    Europa.Components.ModalReenviarSuat.AutoCompleteReenviarBreveLancamento = new Europa.Components.AutoCompleteBreveLancamento()
        .WithAjax(false)
        .WithTargetSuffix("reenviar_breve_lancamento")
        .WithParent("#reenviar-suat-modal")
        .Configure();
}

Europa.Components.ModalReenviarSuat.Reenviar = function () {
    let obj = {
        id: $("#PreProposta_Id").val(),
        idBreveLancamento: $("#autocomplete_reenviar_breve_lancamento").val()

    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.i18n.Messages.MsgConfirmacaoReintegracao);
    Europa.Confirmacao.ConfirmCallback = function () {
        $.post(Europa.Controllers.PreProposta.UrlReenviarSuat, obj, function (res) {
            if (res.Sucesso) {
                $("#Empreendimento_Divisao").val(res.Objeto.Divisao);
                $("#area_geral #PreProposta_BreveLancamento_Nome").val(res.Objeto.NomeBreveLancamento);
                $("#form_reenviar #PreProposta_BreveLancamento_Nome").val(res.Objeto.NomeBreveLancamento);
                $("#PreProposta_IdentificadorUnidadeSuat").val(" ");
                $("#novoEmpreendimento").html(" ");
                $("#novoEmpreendimento").append(Europa.i18n.Messages.ConsultaEstoqueEmpreendimento + " - " + res.Objeto.NomeEmpreendimento);

                Europa.Components.ModalReenviarSuat.CloseModal()
                Europa.Components.ConsultaEstoqueModalEmpreendimento.AbrirModal();
            }
            else {
                Europa.Informacao.PosAcao(res);
            }
        });

    };
    Europa.Confirmacao.Show();
}