Europa.Controllers.ModalNovasMensagens = {};

$(function () {
    Europa.Controllers.ModalNovasMensagens.AbrirModal();
});

Europa.Controllers.ModalNovasMensagens.AbrirModal = function () {
    $("#modal-novas-mensagens").modal("show");
}

Europa.Controllers.ModalNovasMensagens.FecharModal = function () {
    $("#modal-novas-mensagens").modal("hide");
}