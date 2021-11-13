"use strict";
Europa.Controllers.DefinirSenha = {};
Europa.Controllers.DefinirSenha.UrlDefinirSenha = undefined;
Europa.Controllers.DefinirSenha.NovaSenhaVisivel = false;
Europa.Controllers.DefinirSenha.ConfirmeSenhaVisivel = false;


$(function () {
    Europa.Controllers.DefinirSenha.CheckInputs()
});


Europa.Controllers.DefinirSenha.CheckInputs = function () {
    Europa.Controllers.DefinirSenha.HabilitarEyeNovaSenha();
    Europa.Controllers.DefinirSenha.HabilitarEyeConfirmeSenha();
    if (CSS.supports("(-webkit-box-reflect:unset)") &&
        $('input:-webkit-autofill').length > 1) {
        $("#btn_login").css("background", "#ec1d24");
        $("#btn_login").removeAttr("disabled");
    } else {
        if (!$("#nova_senha").val() || !$("#confirme_senha").val()) {
            $("#btn_login").css("background", "#d1d1d1");
            $("#btn_login").attr("disabled", "disabled");
        } else {
            $("#btn_login").css("background", "#ec1d24");
            $("#btn_login").removeAttr("disabled");
        }
    }
};

Europa.Controllers.DefinirSenha.HabilitarEyeNovaSenha = function () {
    if ($("#nova_senha").val() == "") {
        $("#nova_senha_normal_eye").css("display", "unset");
        $("#nova_senha_black_eye").css("display", "none");
    } else {
        $("#nova_senha_black_eye").css("display", "unset");
        $("#nova_senha_normal_eye").css("display", "none");
    }
};

Europa.Controllers.DefinirSenha.HabilitarEyeConfirmeSenha = function () {
    if ($("#confirme_senha").val() == "") {
        $("#confirme_senha_normal_eye").css("display", "unset");
        $("#confirme_senha_black_eye").css("display", "none");
    } else {
        $("#confirme_senha_black_eye").css("display", "unset");
        $("#confirme_senha_normal_eye").css("display", "none");
    }
};

Europa.Controllers.DefinirSenha.MostrarNovaSenha = function () {
    if (!Europa.Controllers.DefinirSenha.NovaSenhaVisivel) {
        $("#nova_senha").removeAttr("type");
        Europa.Controllers.DefinirSenha.NovaSenhaVisivel = true;
    } else {
        Europa.Controllers.DefinirSenha.NovaSenhaVisivel = false;
        $("#nova_senha").attr("type", "password");
    }
};

Europa.Controllers.DefinirSenha.MostrarConfirmeSenha = function () {
    if (!Europa.Controllers.DefinirSenha.ConfirmeSenhaVisivel) {
        $("#confirme_senha").removeAttr("type");
        Europa.Controllers.DefinirSenha.ConfirmeSenhaVisivel = true;
    } else {
        Europa.Controllers.DefinirSenha.ConfirmeSenhaVisivel = false;
        $("#confirme_senha").attr("type", "password");
    }
};

Europa.Controllers.DefinirSenha.Definir = function () {
    var corretorDTO = {
        Id: $("#Id").val(),
        Senha: $("#nova_senha").val(),
        ConfirmacaoSenha: $("#confirme_senha").val()
    }; 
    $.post(Europa.Controllers.DefinirSenha.UrlDefinirSenha, corretorDTO, function (res) {
        Europa.Controllers.DefinirSenha.ValidarCampos();
        Europa.Controllers.DefinirSenha.LimparMensagemErro();
        if (res.Sucesso) {
            window.location = Europa.Controllers.DefinirSenha.UrlSucesso;
        } else {
            Europa.Controllers.DefinirSenha.InvalidarCampos(res.Campos);
            Europa.Controllers.DefinirSenha.ExibirMensagemErro(res.Mensagens);
        }
    });
};

Europa.Controllers.DefinirSenha.LimparMensagemErro = function () {
    $("#div_error").text("");
}

Europa.Controllers.DefinirSenha.ExibirMensagemErro = function (mensagens) {
    Europa.Controllers.DefinirSenha.LimparMensagemErro()
    mensagens.forEach(function (mensagem) {
        $("#div_error").append('<br/>' + mensagem);
    });
}

Europa.Controllers.DefinirSenha.ValidarCampos = function (fields) {
    $("form#form_alteracao_senha :input").each(function () {
        var input = $(this);
        input.removeClass('errorBorder');
    });
}

Europa.Controllers.DefinirSenha.InvalidarCampos = function (fields) {
    fields.forEach(function (field) {
        $("input[name*=" + field + "]").addClass('errorBorder');
    });
}

Europa.Controllers.DefinirSenha.ParameterValidator = function (parameterName, parameter) {
    if (parameter === undefined) {
        alert('Parameter ' + parameterName + ' is required');
    }
};

Europa.Controllers.DefinirSenha.ConsultaPreProposta = function () {
    window.location = Europa.Controllers.DefinirSenha.UrlConsultaPreProposta;
};

Europa.Controllers.DefinirSenha.Login = function () {
    window.location = Europa.Controllers.DefinirSenha.UrlLogin;
};
