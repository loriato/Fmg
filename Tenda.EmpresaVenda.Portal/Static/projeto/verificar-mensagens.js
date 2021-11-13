"use strict";

Europa.Components.MessageAfterLogin = {};
Europa.Components.MessageAfterLogin.Data = [];

$(document).ready(function () {
    Europa.Components.MessageAfterLogin.Init();

    Europa.Components.MessageAfterLogin.CheckMessages();
});


Europa.Components.MessageAfterLogin.Init = function () {
    Europa.Components.MessageAfterLogin.Data.push({ code: "0001", message: Europa.i18n.Messages.RegrasComissaoParaAprovacao });
};

Europa.Components.MessageAfterLogin.CheckMessages = function () {
    var url = new URL(window.location.href);
    var code = url.searchParams.get("code");

    if (code !== undefined) {
        var values = Europa.Components.MessageAfterLogin.Data.find(reg => reg.code === code);
        if (values !== undefined) {
            $("#regra-comissao-novas-regras").modal('show');
        }
    }
};