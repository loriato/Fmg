Europa.Controllers.ModalMensagens = {};


$(function () {
    $(document).on('click', function (e) {
        var img = e.target.closest('#exibir-msg');
        var interior = e.target.closest('#conteudo-msg-modal');
        if (img || interior) { } else {
            Europa.Controllers.ModalMensagens.FecharModal();
        }
    });
});

Europa.Controllers.ModalMensagens.AbrirModal = function () {
    $("#spinner").css("display", "none");
    $("#modal-mensagens").modal("show");
    $("#img-mensagem").css({ "display": "unset", "position": "fixed" });
    $("#modal-mensagens").animate({ top: "-16px" });
    $("#img-mensagem").animate({ marginTop: "25px" });
    $(".modal-backdrop.in").css("opacity", "0");
    setTimeout(function () { Europa.Controllers.ModalMensagens.CarregarLista() }, 400);
}

Europa.Controllers.ModalMensagens.CarregarLista = function () {
    $.post(Europa.Controllers.ModalMensagens.UrlListar, null, function (res) {
        $("#solicitacao_exibir").html(res);
        $("#quant_noti").addClass("hide");
        $("#spinner").css("display", "unset");
    });
}

Europa.Controllers.ModalMensagens.FecharModal = function () {
    //$("#quant_noti").css("display", "unset");
    $("#modal-mensagens").animate({ top: "-470px" });
    $("#img-mensagem").animate({ marginTop: "-600px" });
    setTimeout(function () { Europa.Controllers.ModalMensagens.AnimateModal() }, 400);
}

Europa.Controllers.ModalMensagens.AnimateModal = function () {
    $(".modal-backdrop.in").css("opacity", "0.5");
    $("#modal-mensagens").modal("hide");
}