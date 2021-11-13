Europa.Controllers.Login = {};
Europa.Controllers.Login.StatusPassword = 0;


$(document).ready(function () {
    Europa.Controllers.Login.CheckInputs();
    $("input:text").bind("focus change keyup blur", function (event) {
        Europa.Controllers.Login.CheckInputs();
    });
    setTimeout(Europa.Controllers.Login.CheckInputs, 300);
});

Europa.Controllers.Login.CheckInputs = function () {
    Europa.Controllers.Login.HabilitarEye();
    if (CSS.supports("(-webkit-box-reflect:unset)") &&
        $('input:-webkit-autofill').length > 1) {
        $("#btn_login").css("background", "#ec1d24");
        $("#btn_login").removeAttr("disabled");
    } else {
        if ($("#Password").val() === "" || $("#Username").val() === "") {
            $("#btn_login").css("background", "#d1d1d1");
            $("#btn_login").attr("disabled", "disabled");
        } else {
            $("#btn_login").css("background", "#ec1d24");
            $("#btn_login").removeAttr("disabled");
        }
    }
};

Europa.Controllers.Login.HabilitarEye = function () {
    if ($("#Password").val() == "") {
        $("#normal_eye").css("display", "unset");
        $("#black_eye").css("display", "none");
    } else {
        $("#black_eye").css("display", "unset");
        $("#normal_eye").css("display", "none");
    }
};

Europa.Controllers.Login.ShowPassword = function () {
    if (Europa.Controllers.Login.StatusPassword == 0) {
        $("#Password").removeAttr("type");
        Europa.Controllers.Login.StatusPassword = 1;
    } else {
        Europa.Controllers.Login.StatusPassword = 0;
        $("#Password").attr("type", "password");
    }
};
Europa.Controllers.Login.RecuperarConta = function () {
    window.location = Europa.Controllers.Login.UrlRecuperarConta;
};

Europa.Controllers.Login.PreCadastroEv = function () {
    window.location = Europa.Controllers.Login.UrlPreCadastro;
}