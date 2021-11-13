Europa.Controllers.ValidarToken = {};


$(document).ready(function () {
    var ctrlDown = false,
        ctrlKey = 17,
        cmdKey = 91,
        vKey = 86,
        cKey = 67;

    $(document).keydown(function (e) {
        if (e.keyCode == ctrlKey || e.keyCode == cmdKey) ctrlDown = true;
    }).keyup(function (e) {
        if (e.keyCode == ctrlKey || e.keyCode == cmdKey) ctrlDown = false;
    });

    $(".no-copy-paste").keydown(function (e) {
        if (ctrlDown && (e.keyCode == vKey || e.keyCode == cKey)) return false;
    });

    $(document).keydown(function (e) {
        if (ctrlDown && (e.keyCode == vKey)) {
            $("#inputToken:visible").focus();
        };
    });

    $(document).keyup(function (e) {
        if (ctrlDown && (e.keyCode == vKey)) {
            var value = $("#inputToken").val();
            Europa.Controllers.ValidarToken.PreencherInputs(value);
        };
    });
});

Europa.Controllers.ValidarToken.PreencherInputs = function (valor) {
    for (var i = 1; i < 7; i++) {
        if (valor[i - 1] == " ") {
            $("#token_" + i).val("");
        } else {
            $("#token_" + i).val(valor[i - 1]);
        }
    }
    if (Europa.Controllers.ValidarToken.InputsPreenchidos()) {
        $("#btn_entrar").css("background-color", "#ec1d24");
        $("#btn_entrar").css("cursor", "pointer");
        $("#btn_entrar").on("click", Europa.Controllers.ValidarToken.Informar);
        $("#btn_entrar").removeAttr("disabled");
    }
};

Europa.Controllers.ValidarToken.InputsPreenchidos = function () {
    for (var i = 1; i < 7; i++) {
        if ($("#token_" + i).val() == "" || $("#token_" + i).val() == " ") {
            return false;
        }
    }
    return true;
};

Europa.Controllers.ValidarToken.ChangeInput = function (inp) {
    var inputId = inp.id.split('_');
    var inputPrefix = inputId[0];
    var inputNumber = inputId[1];
    var current = parseInt(inputNumber);
    var next = current + 1;
    var prev = current - 1;
    if (inp.value != "" && inp.value != " ") {
        $("#" + inputPrefix + '_' + next + ":visible").focus();
    } else {
        $("#" + inputPrefix + '_' + prev + ":visible").focus();
    }
    if (Europa.Controllers.ValidarToken.InputsPreenchidos()) {
        $("#btn_entrar").css("background-color", "#ec1d24");
        $("#btn_entrar").css("cursor", "pointer");
        $("#btn_entrar").on("click", Europa.Controllers.ValidarToken.Informar);
        $("#btn_entrar").removeAttr("disabled");
    } else {
        $("#btn_entrar").css("background-color", "#d1d1d1");
        $("#btn_entrar").attr("disabled", "disabled");
        $("#btn_entrar").css("cursor", "unset");
        $("#btn_entrar").prop("onclick", null).off("click");
    }
};

Europa.Controllers.ValidarToken.Informar = function () {
    var tokenValue = "";
    $("input[id^='token_']").each(function () {
        tokenValue += $(this).val();
    });
    tokenValue = tokenValue.toUpperCase();
    $.post(Europa.Controllers.ValidarToken.UrlVerificarToken, { token: tokenValue }, function (res) {
        if (res.Sucesso) {
            $("#token").val(tokenValue);
            $("#form_informar_token").submit();
        } else {
            Europa.Controllers.ValidarToken.InputError();
        }
    });
};

Europa.Controllers.ValidarToken.ReenviarToken = function () {
    $.post(Europa.Controllers.ValidarToken.UrlReenviarToken);
};

Europa.Controllers.ValidarToken.InputError = function () {
    for (var i = 1; i < 7; i++) {
        $("#token_" + i).addClass("input-error");
    }
    $("#msg-codigo-error").css("display","");
}