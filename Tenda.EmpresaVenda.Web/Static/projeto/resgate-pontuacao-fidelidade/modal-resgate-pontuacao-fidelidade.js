$(function () {
    
});

Europa.Controllers.ResgatePontuacaoFidelidade.AbriModal = function () {
    $("#modal_solicitar_resgate").show();
};

Europa.Controllers.ResgatePontuacaoFidelidade.FecharModal = function () {
    $("#modal_solicitar_resgate").hide();
};

Europa.Controllers.ResgatePontuacaoFidelidade.SolicitarResgate = function () {
    var data = {
        IdEmpresaVenda: $("#autocomplete_empresa_venda").val(),
        PontuacaoSolicitada: $("#PontuacaoSolicitada", "#form_solicitar_resgate").val(),
        PontuacaoDisponivel: $("#PontuacaoDisponivel").val()
    };

    $.post(Europa.Controllers.ResgatePontuacaoFidelidade.Url.SolicitarResgate, data, function (res) {
        if (res.Sucesso) {
            $("#PontuacaoSolicitada", "#form_solicitar_resgate").val(0);
            Europa.Controllers.ResgatePontuacaoFidelidade.FecharModal();
            Europa.Controllers.ResgatePontuacaoFidelidade.Filtrar();
        }

        Europa.Informacao.PosAcao(res);
    });
};
