Europa.Controllers.PontuacaoFidelidade = {};
Europa.Controllers.PontuacaoFidelidade.Url = {};
Europa.Controllers.PontuacaoFidelidade.Progressao = [];
$(function () {
});

Europa.Controllers.PontuacaoFidelidade.OnChangeTipoPontuacaoFidelidadeFiltro = function () {
    if ($("#FiltroTipoPontuacaoFidelidade").val() == 2) {
        $(".filtro-campanha").removeClass("hidden");
    } else {
        $(".filtro-campanha").addClass("hidden");
        $("#FiltroTipoCampanhaFidelidade").val("").trigger('change');
    }
};