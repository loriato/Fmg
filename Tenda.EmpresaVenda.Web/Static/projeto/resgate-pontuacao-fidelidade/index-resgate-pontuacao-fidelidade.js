Europa.Controllers.ResgatePontuacaoFidelidade = {};

$(function () {
    $("#tab1").click();
});

Europa.Controllers.ResgatePontuacaoFidelidade.PreencherTotais = function () {
    var idEmpresaVenda = $("#autocomplete_empresa_venda").val();

    $.post(Europa.Controllers.ResgatePontuacaoFidelidade.Url.Totais, { idEmpresaVenda: idEmpresaVenda }, function (res) {
        if (res.Sucesso) {
            $("#PontuacaoResgatada").val(res.Objeto.PontuacaoResgatada);
            $("#PontuacaoSolicitada").val(res.Objeto.PontuacaoSolicitada);
            $("#PontuacaoDisponivel").val(res.Objeto.PontuacaoDisponivel);
            $("#PontuacaoIndisponivel").val(res.Objeto.PontuacaoIndisponivel);
            $("#PontuacaoTotal").val(res.Objeto.PontuacaoTotal);

            $("#PontuacaoDisponivel", "#form_solicitar_resgate").val(res.Objeto.PontuacaoDisponivel);

            if (res.Objeto.PontuacaoDisponivel > 0) {
                $("#btn_solicitar_resgate").removeClass("hidden");
            }
        }
    });
};