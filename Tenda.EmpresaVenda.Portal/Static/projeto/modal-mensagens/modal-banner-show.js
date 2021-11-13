$("#modal-banner").modal("show");

Europa.Controllers.ModalBanner = {};

$(function () {
    Europa.Controllers.ModalBanner.AbrirModal();
});

Europa.Controllers.ModalBanner.AbrirModal = function () {
    $("#modal-banner").modal("show");
}

Europa.Controllers.ModalBanner.FecharModal = function () {
    $("#modal-banner").modal("hide");
}