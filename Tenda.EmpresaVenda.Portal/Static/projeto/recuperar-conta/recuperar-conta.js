Europa.Controllers.RecuperarConta = {};

$(function () {
    Europa.Controllers.RecuperarConta.CheckInputs();
    $("input:text").bind("focus change keyup blur", function (event) {
        Europa.Controllers.RecuperarConta.CheckInputs();
    });
    setTimeout(Europa.Controllers.RecuperarConta.CheckInputs, 300);
});
Europa.Controllers.RecuperarConta.CheckInputs = function () {
    if (CSS.supports("(-webkit-box-reflect:unset)") &&
        $('input:-webkit-autofill').length > 1) {
        $("#btn_login").css("background", "#ec1d24");
        $("#btn_login").removeAttr("disabled");
    } else {
        if ($("#Username").val() === "") {
            $("#btn_login").css("background", "#d1d1d1");
            $("#btn_login").attr("disabled", "disabled");
        } else {
            $("#btn_login").css("background", "#ec1d24");
            $("#btn_login").removeAttr("disabled");
        }
    }
};

Europa.Controllers.RecuperarConta.EsquecimentoSenha = function () {
    var email = $("#Username").val();
    $.get(Europa.Controllers.RecuperarConta.UrlReenviarTokenAtivacao, { Username: email }, function (res) {
        Europa.Informacao.PosAcao(res)
        if (res.Sucesso) {
            $("#btn_redirect_sucess").click();
        };
    });
};
