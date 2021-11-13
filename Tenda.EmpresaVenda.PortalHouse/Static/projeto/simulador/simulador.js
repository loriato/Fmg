Europa.Controllers.Simulador = {};
Europa.Controllers.Simulador.UrlMatrizOferta = "";
Europa.Controllers.Simulador.UrlSimulador = "";
Europa.Controllers.Simulador.IdPreProposta = 0;
Europa.Controllers.Simulador.TabelaDetalhamentoFinanceiro = {};
Europa.Controllers.Simulador.TabelaResultadoSimulacao = {};
Europa.Controllers.Simulador.TabelaItbiEmolumento = {};
Europa.Controllers.Simulador.ModoInclusao = true;
Europa.Controllers.Simulador.IsTelaCliente = false;

$(function () {

});

//Simulador
Europa.Controllers.Simulador.AddError = function (fields) {
    fields.forEach(function (key) {
        $("[name='" + key + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.Simulador.PodeManterAssociacoes = function () {
    var value = $('#PodeManterAssociacoes').val();
    return 'true' === value || 'True' === value || '1' === value || 1 === value;
};

Europa.Controllers.Simulador.AbrirDialogo = function () {
    $("#dialogo_simulador").modal("show");
    setTimeout(function () { $(".modal-backdrop").remove() }, 300);
    $("#modal-simulacao").css("z-index", 999);
    $("#btn_matriz_oferta").prop("disabled", false)
};

Europa.Controllers.Simulador.FecharDialogo = function () {
    $("#dialogo_simulador").modal("hide");
    setTimeout(function () { $(".modal-backdrop").remove() }, 300);
    $("#select_simulador").val(0).trigger('change');
    $("#modal-simulacao").css("z-index", 1000)
    $("#btn_matriz_oferta").prop("disabled", false)
    if (!this.IsTelaCliente) {
        Europa.Controllers.Simulador.AplicarSimulacaoAtual();
    }
};

//Matriz de Oferta
Europa.Controllers.Simulador.AbrirMatrizOferta = function () {
    $("#iframe_simulador").remove();
    $("#btn_matriz_oferta").prop("disabled", true)
    var parametro = {
        IdPreProposta: $("#PreProposta_Id").val()
    };

    Europa.Controllers.Simulador.IdPreProposta = $("#PreProposta_Id").val()

    $.post(Europa.Controllers.Simulador.UrlBuscarUrlMatrizOferta, parametro, function (res) {
        console.log(res)
        if (res.Sucesso) {
            $("#div-frame-simulador").append('<iframe id="iframe_simulador" src="' + res.Objeto + '" frameborder="0" scrolling="yes" style="overflow:hidden;height:100%;width:100%"></iframe>')
            Europa.Controllers.Simulador.UrlMatrizOferta = res.Objeto;
            Europa.Controllers.Simulador.AbrirDialogo();
        }
        else {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, res.Mensagens.join("<br/>"));
            Europa.Informacao.Show();
            $("#btn_matriz_oferta").prop("disabled", false)
        }
    });
        
};

Europa.Controllers.Simulador.AbrirMatrizOfertaCliente = function () {
    $("#iframe_simulador").remove();
    $("#btn_matriz_oferta").prop("disabled", true)
    var parametro = {
        IdCliente: $("#IdCliente").val()
    };

    Europa.Controllers.Simulador.IdPreProposta = $("#PreProposta_Id").val()

    $.post(Europa.Controllers.Simulador.UrlBuscarUrlMatrizOferta, parametro, function (res) {
        console.log(res)
        if (res.Sucesso) {
            $("#div-frame-simulador").append('<iframe id="iframe_simulador" src="' + res.Objeto + '" frameborder="0" scrolling="yes" style="overflow:hidden;height:100%;width:100%"></iframe>')
            Europa.Controllers.Simulador.UrlMatrizOferta = res.Objeto;
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

Europa.Controllers.Simulador.FecharMenu = function () {

}

// Simulador
Europa.Controllers.Simulador.AbrirSimulador = function () {
    $("#iframe_simulador").remove();
    var parametro = {
        IdPreProposta: $("#PreProposta_Id").val()
    };

    Europa.Controllers.Simulador.IdPreProposta = $("#PreProposta_Id").val()

    $.post(Europa.Controllers.Simulador.UrlBuscarUrlSimulador, parametro, function (res) {
        console.log(res)
        if (res.Sucesso) {
            $("#div-frame-simulador").append('<iframe id="iframe_simulador" src="' + res.Objeto + '" frameborder="0" scrolling="yes" style="overflow:hidden;height:100%;width:100%"></iframe>')
            Europa.Controllers.Simulador.UrlSimulador = res.Objeto;
            Europa.Controllers.Simulador.AbrirDialogo();
        }
        else {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, res.Mensagens.join("<br/>"));
            Europa.Informacao.Show();
        }
    });
}

Europa.Controllers.Simulador.OnChangeSelect = function () {
    var select = parseInt($("#select_simulador").val());
    
    switch (select) {
        case 0:
            break;
        case 1:
            Europa.Controllers.Simulador.AbrirMatrizOferta();
            break;
        case 2:
            Europa.Controllers.Simulador.AbrirSimulador();
            break;
        default:
            Europa.Controllers.Simulador.FecharDialogo();
            break;
    };
}

Europa.Controllers.Simulador.AplicarSimulacaoAtual = function () {
    var parametro = {
        IdProposta: $("#PreProposta_Id").val()
    };

    $.post(Europa.Controllers.Simulador.UrlAplicarSimulacaoAtual, parametro, function (res) {
        console.log(res)
        if (res.Sucesso) {
            Europa.Informacao.Hide = function () {
                location.reload(true);
            }
        }
        Europa.Informacao.PosAcao(res)        
    })
}