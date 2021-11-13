Europa.Controllers.ResgateProgramaFidelidade = {};

$(function () {
    $("#myRange")[0].max = Europa.Controllers.ResgateProgramaFidelidade.PontosDisponiveis;

    if ($("#myRange")[0].max == $("#myRange").val()) {
        location.href = Europa.Controllers.ResgateProgramaFidelidade.Url.Index;
    }
});

Europa.Controllers.ResgateProgramaFidelidade.OnInput = function (on, target) {
    $(target).val($(on).val());
    var valor = $("#myRange").val();

    if ($(on).val() == ""||$(on).val() == "0") {
        $(target).val(0);

        $(".card-pontos").removeClass("col-md-12");
        $(".card-pontos").addClass("col-md-24");
        $(".info-resgate").addClass("hidden");
        $(".Rectangle").removeClass("pull-right");

        Europa.Controllers.ResgateProgramaFidelidade.OnChangePontoDisponivel(valor);

        return;
    }       

    $(".card-pontos").addClass("col-md-12");
    $(".Rectangle").addClass("pull-right");
    $(".card-pontos").removeClass("col-md-24");
    $(".info-resgate").removeClass("hidden");
    
    Europa.Controllers.ResgateProgramaFidelidade.OnChangePontoDisponivel(valor);
    if (valor == 1) {
        valor = valor + " Ponto";
    } else {
        valor = valor + " Pontos";
    }

    $(".pontos").html(valor);
}

Europa.Controllers.ResgateProgramaFidelidade.OnChangePontoDisponivel = function (valor) {
    var pontoDisponivel = Europa.Controllers.ResgateProgramaFidelidade.PontosDisponiveis;

    pontoDisponivel -= valor;

    $(".-pontos").html(pontoDisponivel)
}

Europa.Controllers.ResgateProgramaFidelidade.ResgatarPontos = function () {
    var pontos = $("#myRange").val();

    Europa.NovaConfirmacao.PreAcao("Confirma a solicitação de resgate dos pontos?",
        "Essa ação não pode ser cancelada",
        "Confirmar", function () {
            var data = {
                PontuacaoSolicitada: pontos
            };
            
            $.post(Europa.Controllers.ResgateProgramaFidelidade.Url.SolicitarResgate, data, function (res) {
                
                if (res.Sucesso) {
                    Europa.NovaInformacao.Hide = function () {
                        location.href = Europa.Controllers.ResgateProgramaFidelidade.Url.Index;
                    }           
                }

                Europa.NovaInformacao.PosAcao(Europa.i18n.Messages.MsgSolicitacaoResgateSucesso, res);

            });
        });
};