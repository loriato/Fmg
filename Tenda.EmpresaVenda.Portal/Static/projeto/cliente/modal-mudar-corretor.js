Europa.Components.CorretorModal = {}
Europa.Components.CorretorModal.Selectors = {
    IdFormularioCliente: "#form_cliente"
}

Europa.Components.CorretorModal.OpenModal = function () {
    Europa.Components.CorretorModal.InitModal();
    $("#modal-mudar-corretor").modal("show");
}

Europa.Components.CorretorModal.CloseModal = function () {
    $("#modal-mudar-corretor").modal("hide");
}

Europa.Components.CorretorModal.InitAutoComplete = function () {
    Europa.Components.CorretorModal.AutoCompleteCorretor = new Europa.Components.AutoCompleteProprietarioEmpresaVenda()
        .WithTargetSuffix("corretor")
        .Configure();
}

Europa.Components.CorretorModal.InitModal = function () {
    Europa.Components.CorretorModal.InitAutoComplete();

    let nomeCorretor = $("#Cliente_NomeCorretor", Europa.Components.CorretorModal.Selectors.IdFormularioCliente).val();
    $("#NomeCorretorAtual").val(nomeCorretor);
}

Europa.Components.CorretorModal.Salvar = function () {
    let obj = {
        idCliente: $("#Cliente_Id", Europa.Components.CorretorModal.Selectors.IdFormularioCliente).val(),
        idCorretorAtual: $("#Cliente_Corretor", Europa.Components.CorretorModal.Selectors.IdFormularioCliente).val(),
        idNovoCorretor: $("#autocomplete_corretor").val()
    }

    $.post(Europa.Components.CorretorModal.UrlMudarCorretor, obj, function (res) {
        console.log(res)
        if (res.Sucesso) {
            Europa.Components.CorretorModal.CloseModal();
            Europa.Informacao.Hide = function () {
                location.reload();
            }
        }
        Europa.Informacao.PosAcao(res);
    })
}