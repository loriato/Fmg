$(function () {
    
});

//Simulador

Europa.Controllers.Simulador.AbrirDialogo = function () {
    $("#dialogo_simulador").modal("show");
    setTimeout(function () { $(".modal-backdrop").remove() }, 300);
    $("#modal-simulacao").css("z-index", 999);
    setTimeout(function () { $("#dialogo_simulador").css("padding-left", "0px") },200);

};

Europa.Controllers.Simulador.FecharDialogo = function () {
    $("#dialogo_simulador").modal("hide");
    setTimeout(function () { $(".modal-backdrop").remove() }, 300);
    $("#select_simulador").val(0).trigger('change');
    $("#modal-simulacao").css("z-index", 1000);

    Europa.Controllers.Simulador.AplicarSimulacaoAtual();
};

Europa.Controllers.Simulador.AbrirSimulador = function () {
    $("#iframe_simulador").remove();
    
    var parametro = {
        IdPreProposta: $("#PreProposta_Id").val()
    };
    console.log(Europa.Controllers.Simulador.IdPreProposta)
    Europa.Controllers.Simulador.IdPreProposta = $("#PreProposta_Id").val()
    //return
    $.post(Europa.Controllers.Simulador.UrlBuscarUrlSimulador, parametro, function (res) {
        console.log(res)
        if (res.Sucesso) {
            $("#div-frame-simulador").append('<iframe id="iframe_simulador" src="' + res.Objeto + '" frameborder="0" scrolling="true" style="height:500px"></iframe>')
            Europa.Controllers.Simulador.UrlSimulador = res.Objeto;
            Europa.Controllers.Simulador.AbrirDialogo();
            
        }
        else {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, res.Mensagens.join("<br/>"));
            Europa.Informacao.Show();
        }
    });
}

Europa.Controllers.Simulador.MontarUrlSimuladorMenu = function () {
    $("#iframe_simulador").remove();

    $.get(Europa.Controllers.Simulador.UrlMontarUrlSimuladorMenu, function (res) {
        console.log(res)
        if (res.Sucesso) {
            $("#div-frame-simulador").append('<iframe id="iframe_simulador" src="' + res.Objeto + '" frameborder="0" scrolling="yes" style="height:500px"></iframe>')
            Europa.Controllers.Simulador.UrlSimulador = res.Objeto;
            Europa.Controllers.Simulador.AbrirDialogo();

        }
        else {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, res.Mensagens.join("<br/>"));
            Europa.Informacao.Show();
        }
    });
}

Europa.Controllers.Simulador.CloseMenu = function () {
    $(".menu-expander").css("overflow-y", "auto");
    $(".menu-expander").css("left", "-296px");
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

    })
}



////Matriz de Oferta

Europa.Controllers.Simulador.MontarUrlMatrizOfertaMenu = function () {
    $("#iframe_simulador").remove();

    $.get(Europa.Controllers.Simulador.UrlMontarUrlMatrizOfertaMenu, function (res) {
        console.log(res)
        if (res.Sucesso) {
            $("#div-frame-simulador").append('<iframe id="iframe_simulador" src="' + res.Objeto + '" frameborder="0" scrolling="yes" style="height:500px"></iframe>')
            Europa.Controllers.Simulador.UrlSimulador = res.Objeto;
            Europa.Controllers.Simulador.AbrirDialogo();

        }
        else {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, res.Mensagens.join("<br/>"));
            Europa.Informacao.Show();
        }
    });
}

Europa.Controllers.Simulador.AbrirMatrizOfertaCliente = function () {
    $("#iframe_simulador").remove();
    $("#btn_matriz_oferta").prop("disabled", true)
    var parametro = {
        IdCliente: $("#Cliente_Id").val(),
        OrigemSimulacao:3
    };

    Europa.Controllers.Simulador.IdPreProposta = $("#PreProposta_Id").val()

    $.post(Europa.Controllers.Simulador.UrlMatrizOfertaCliente, parametro, function (res) {
        console.log(res)
        if (res.Success) {
            $("#div-frame-simulador").append('<iframe id="iframe_simulador" src="' + res.Data + '" frameborder="0" scrolling="yes" style="overflow:hidden;min-height:600px;width:100%"></iframe>')
            Europa.Controllers.Simulador.UrlMatrizOferta = res.Data;
            Europa.Controllers.Simulador.AbrirDialogo();
            Europa.Controllers.Simulador.IsTelaCliente = true;
        }
        else {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, res.Mensagens.join("<br/>"));
            Europa.Informacao.Show();
            $("#btn_matriz_oferta").prop("disabled", false)
        }
    });
};

Europa.Controllers.Simulador.AbrirMatrizOferta = function () {
    $("#iframe_simulador").remove();
    $("#btn_matriz_oferta").prop("disabled", true)
    var parametro = {
        IdPreProposta: $("#PreProposta_Id").val()
    };

    Europa.Controllers.Simulador.IdPreProposta = $("#PreProposta_Id").val()

    $.post(Europa.Controllers.Simulador.UrlBuscarUrlMatrizOferta, parametro, function (res) {
        console.log(res)
        if (res.Success) {
            $("#div-frame-simulador").append('<iframe id="iframe_simulador" src="' + res.Data + '" frameborder="0" scrolling="yes" style="overflow:hidden;min-height:600px;width:100%"></iframe>')
            Europa.Controllers.Simulador.UrlMatrizOferta = res.Data;
            Europa.Controllers.Simulador.AbrirDialogo();
        }
        else {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, res.Messages.join("<br/>"));
            Europa.Informacao.Show();
            $("#btn_matriz_oferta").prop("disabled", false)
        }
    });

};

