
$(function () {
    Europa.Controllers.HierarquiaHouse.AbrirModal();
});

Europa.Controllers.HierarquiaHouse.AbrirModal = function () {
    $("#modal_selecionar_loja").show()
}

Europa.Controllers.HierarquiaHouse.SelecionarLoja = function () {

    var idLoja = $("#loja").val();

    $.post(Europa.Controllers.HierarquiaHouse.UrlSelecionarLoja, { idLoja: idLoja }, function (res) {
        console.log(res)
        if (res.Success) {
            location.href = Europa.Controllers.HierarquiaHouse.UrlIndexHome;
        }        

        Europa.Informacao.PosAcaoBaseResponse(res)
    });
}

