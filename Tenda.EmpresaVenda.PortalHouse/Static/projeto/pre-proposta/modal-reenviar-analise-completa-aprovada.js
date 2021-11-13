Europa.Components.ModalReenviarAnaliseCompletaAprovada = {}

$(function () {
    $("#justificativa").keyup(function (e) {
        if ($(this).val().length > 0) {
            $('#btn_confirmar_envio_analise').enable(true);
        }
        else {
            $('#btn_confirmar_envio_analise').enable(false);
        }
    });
})

Europa.Components.ModalReenviarAnaliseCompletaAprovada.OpenModal = function () {
    $("#reenviar-analise-completa-aprovada-modal").modal("show");
}

Europa.Components.ModalReenviarAnaliseCompletaAprovada.ReenviarWorkflow = function () {
    var data = {
            idPreProposta: $("#PreProposta_Id").val(),
            justificativa: $('#justificativa').val()
    }

    var response = Europa.Controllers.PrePropostaWorkflow.ReenviarAnaliseCompletaAprovada(data);

    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Informacao.PosAcao(response);
}