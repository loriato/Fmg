Europa.Components.ModalFatorSocialConfirma = {}
Europa.Components.ModalFatorSocialConfirma.Selectors = {
    IsReintegracao: undefined
}

Europa.Components.ModalFatorSocialConfirma.OpenModal = function () {
    $("#fator_social_confirma_modal").modal("show");
}

Europa.Components.ModalFatorSocialConfirma.CloseModal = function () {
    $("#fator_social_confirma_modal").modal("hide");
}

Europa.Components.ModalFatorSocialConfirma.AbrirModalReenviarSuat = function () {
    let prePropostaIdUnidadeSuat = $("#PreProposta_IdUnidadeSuat").val();

    Europa.Components.ModalFatorSocialConfirma.CloseModal();

    if (this.Selectors.IsReintegracao) {
        Europa.Components.ModalReenviarSuat.OpenModal();
    } else {
        if (prePropostaIdUnidadeSuat == 0 || prePropostaIdUnidadeSuat == undefined || prePropostaIdUnidadeSuat == null) {
            Europa.Components.ConsultaEstoqueModalEmpreendimento.AbrirModal();
        } else {
            Europa.Components.ConsultaEstoqueModalUnidade.Integrar();
        }
    }
}