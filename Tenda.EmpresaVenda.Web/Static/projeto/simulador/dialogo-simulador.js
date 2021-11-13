$(function () {

});

Europa.Controllers.Simulador.AbrirDialogo = function () {
    $("#dialogo_simulador").modal("show");
    setTimeout(function () { $(".modal-backdrop").remove() }, 300);
    $("#modal-simulacao").css("z-index", 999);
    setTimeout(function () { $("#dialogo_simulador").css("padding-left", "0px") }, 200);

};

Europa.Controllers.Simulador.FecharDialogo = function () {
    $("#dialogo_simulador").modal("hide");
    setTimeout(function () { $(".modal-backdrop").remove() }, 300);
    $("#select_simulador").val(0).trigger('change');
    $("#modal-simulacao").css("z-index", 10050);

    Europa.Controllers.Simulador.AplicarSimulacaoAtual();
};

Europa.Controllers.Simulador.AbrirSimulador = function () {
    $("#iframe_simulador").remove();

    var parametro = {
        IdPreProposta: $("#PreProposta_Id").val()
    };
    
    //return
    $.post(Europa.Controllers.Simulador.UrlMontarUrlSimulador, parametro, function (res) {
        console.log(res)
        if (res.Sucesso) {
            $("#div-frame-simulador").append('<iframe id="iframe_simulador" src="' + res.Objeto + '" frameborder="0" scrolling="yes" style="min-height:510px;min-width: -webkit-fill-available;"></iframe>')
            Europa.Controllers.Simulador.UrlSimulador = res.Objeto;
            Europa.Controllers.Simulador.AbrirDialogo();

        }
        else {
            Europa.Informacao.PosAcao(res);
        }
    });
}

Europa.Controllers.Simulador.AplicarSimulacaoAtual = function () {
    var parametro = {
        IdProposta: $("#PreProposta_Id").val()
    };

    if ($("#PreProposta_Id").val() == undefined) {
        return;
    }

    $.post(Europa.Controllers.Simulador.UrlAplicarSimulacaoAtual, parametro, function (res) {
        if (res.Sucesso) {
            Europa.Informacao.PosAcao(res);

            Europa.Informacao.Hide = function () {
                location.reload(true);
            }
        }
    });
}

Europa.Controllers.Simulador.AbrirMatrizOferta = function () {
    $("#iframe_simulador").remove();

    var IdPreProposta= $("#PreProposta_Id").val();

    //return
    $.get(Europa.Controllers.Simulador.UrlBuscarUrlMatrizOferta + "?IdPreProposta=" + IdPreProposta,  function (res) {
        console.log(res)
        if (res.Sucesso) {
            $("#div-frame-simulador").append('<iframe id="iframe_simulador" src="' + res.Objeto + '" frameborder="0" scrolling="yes" style="min-height:510px;min-width: -webkit-fill-available;"></iframe>')
            Europa.Controllers.Simulador.UrlSimulador = res.Objeto;
            Europa.Controllers.Simulador.AbrirDialogo();

        }
        else {
            Europa.Informacao.PosAcao(res);
        }
    });
}