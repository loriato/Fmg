Europa.Controllers.DisponibilidadeEmpreendimento = {};
Europa.Controllers.DisponibilidadeEmpreendimento.Modal = {};

$(function () {
    Europa.Controllers.DisponibilidadeEmpreendimento.Modal.Init();
    Europa.Controllers.DisponibilidadeEmpreendimento.Modal.DesabilitarEdicao();
});

Europa.Controllers.DisponibilidadeEmpreendimento.Modal.Init = function () {
    Europa.Controllers.DisponibilidadeEmpreendimento.Modal.AutoCompleteEmpreendimento = new Europa.Components.AutoCompleteEmpreendimento()
        .WithTargetSuffix("empreendimentos")
        .Configure();
};

Europa.Controllers.DisponibilidadeEmpreendimento.Modal.Abrir = function () {
    $("#disponibilidade_empreendimento_modal").modal("show");
};

Europa.Controllers.DisponibilidadeEmpreendimento.Modal.Fechar = function () {
    $("#disponibilidade_empreendimento_modal").modal("hide");
    Europa.Controllers.DisponibilidadeEmpreendimento.Modal.LimparModal();
};

Europa.Controllers.DisponibilidadeEmpreendimento.Modal.SetarValoresEmpreendimento = function () {
    var idEmpreendimento = Europa.Controllers.DisponibilidadeEmpreendimento.Modal.AutoCompleteEmpreendimento.Value()
    if (idEmpreendimento && idEmpreendimento != 0) {
        $.get(Europa.Controllers.DisponibilidadeEmpreendimento.Modal.UrlBuscarEmpreendimento, { idEmpreendimento: idEmpreendimento }, function (result) {
            $("#empreendimento_disponibilizar_catalogo").val(result.DisponivelCatalogo ? 1 : 0);
            $("#empreendimento_disponivel_venda").val(result.DisponivelParaVenda ? 1 : 0);
            Europa.Controllers.DisponibilidadeEmpreendimento.Modal.HabilitarEdicao();
        });
    } else {
        Europa.Controllers.DisponibilidadeEmpreendimento.Modal.DesabilitarEdicao();
    }
};

Europa.Controllers.DisponibilidadeEmpreendimento.Modal.SalvarEmpreendimento = function () {

    var disponivelCatalogo = ($("#empreendimento_disponibilizar_catalogo").val() == 1)
    var disponivelParaVenda = ($("#empreendimento_disponivel_venda").val() == 1)

    var empreendimentoDTO = {
        Id: Europa.Controllers.DisponibilidadeEmpreendimento.Modal.AutoCompleteEmpreendimento.Value(),
        DisponivelCatalogo: disponivelCatalogo,
        DisponivelParaVenda: disponivelParaVenda
    };

    $.post(Europa.Controllers.DisponibilidadeEmpreendimento.Modal.UrlSalvarEmpreendimento, { empreendimentoDTO }, function (result) {
        if (result.Sucesso) {
            Europa.Controllers.DisponibilidadeEmpreendimento.Modal.Fechar()
        }
        Europa.Informacao.PosAcao(result)
    });
};

Europa.Controllers.DisponibilidadeEmpreendimento.Modal.HabilitarEdicao = function() {
    $("#empreendimento_disponibilizar_catalogo").prop('disabled', false);
    $("#empreendimento_disponivel_venda").prop('disabled', false);
};

Europa.Controllers.DisponibilidadeEmpreendimento.Modal.DesabilitarEdicao = function () {
    $("#empreendimento_disponibilizar_catalogo").val(0);
    $("#empreendimento_disponivel_venda").val(0);
    $("#empreendimento_disponibilizar_catalogo").prop('disabled', true);
    $("#empreendimento_disponivel_venda").prop('disabled', true);
};

Europa.Controllers.DisponibilidadeEmpreendimento.Modal.LimparModal = function () {
    Europa.Controllers.DisponibilidadeEmpreendimento.Modal.AutoCompleteEmpreendimento.Clean();
    Europa.Controllers.DisponibilidadeEmpreendimento.Modal.DesabilitarEdicao();
};