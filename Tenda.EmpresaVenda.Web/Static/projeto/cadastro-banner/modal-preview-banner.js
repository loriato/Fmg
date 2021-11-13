$(function () {
});

Europa.Controllers.CadastroBanner.Preview = function (idBanner) {
    var link = $.get(Europa.Controllers.CadastroBanner.UrlVisualizarBanner, { id: idBanner }, function (res) {
        if (res) {
            console.log(res)
            $("#banner").prop("src", res);
            $("#modal-banner-portal-ev").show();
        }
    });
};

Europa.Controllers.CadastroBanner.FecharModal = function () {
    $("#modal-banner-portal-ev").hide();
};
