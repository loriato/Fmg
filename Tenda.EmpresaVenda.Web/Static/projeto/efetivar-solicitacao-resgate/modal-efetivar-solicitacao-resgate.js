$(function () {

});

Europa.Controllers.EfetivarSolicitacaoResgate.AbriModal = function () {
    $("#modal_aprovar_resgate").show();
};

Europa.Controllers.EfetivarSolicitacaoResgate.FecharModal = function () {
    $(".aprovar").addClass("hidden");
    $(".reprovar").addClass("hidden");

    $("#Voucher").val("");
    $("#Motivo").val("");

    $("#modal_aprovar_resgate").hide();
};

Europa.Controllers.EfetivarSolicitacaoResgate.Confirmar = function () {
    var url = Europa.Controllers.EfetivarSolicitacaoResgate.Url.ReprovarResgate;

    if (Europa.Controllers.EfetivarSolicitacaoResgate.Aprovar) {
        url = Europa.Controllers.EfetivarSolicitacaoResgate.Url.AprovarResgate;
    }

    var pontuacao = {
        IdResgate: $("#IdResgate").val(),
        Voucher: $("#Voucher").val(),
        IdEmpresaVenda: $("#IdEmpresaVenda").val(),
        Motivo: $("#Motivo").val()
    };

    $.post(url, pontuacao, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.EfetivarSolicitacaoResgate.FecharModal();
            Europa.Controllers.EfetivarSolicitacaoResgate.Filtrar();
        }
        Europa.Informacao.PosAcao(res);
    });

};