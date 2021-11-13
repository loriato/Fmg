Europa.Controllers.HierarquiaHouse = {};

$(function () {

});

Europa.Controllers.HierarquiaHouse.OnChangeLoja = function (idLoja) {
    $.post(Europa.Controllers.HierarquiaHouse.UrlSelecionarLoja, { idLoja: idLoja }, function (res) {
        console.log(res)
        if (res.Success) {
            location.href = Europa.Controllers.HierarquiaHouse.UrlIndexHome;
        }

        //Europa.Informacao.PosAcaoBaseResponse(res)
    });
}