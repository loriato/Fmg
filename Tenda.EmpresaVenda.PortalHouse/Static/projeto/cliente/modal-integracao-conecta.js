Europa.Controllers.Cliente.CloseModalMensagemConecta = {};
$(function () {
    $("#btn-integrar-cliente-conecta").prop("hidden", true);
    $("#InformacoesGeraisDto_TelefoneLead", "#modal_integracao_conecta").prop("disabled", true);

    Europa.Mask.ApplyByClass("telefone", Europa.Mask.FORMAT_TELEFONE_9)

});

Europa.Controllers.Cliente.AbrirModalIntegracaoConecta = function () {
    $("#modal_integracao_conecta").modal("show");
}

Europa.Controllers.Cliente.OnSelectTerceiro = function () {
    $("#InformacoesGeraisDto_TelefoneLead", "#modal_integracao_conecta").prop("disabled", true);
    if ($("#Cliente_terceiro_sim:checked").val() == "True") {        
        $("#InformacoesGeraisDto_TelefoneLead", "#modal_integracao_conecta").prop("disabled", false);
        $("#btn-integrar-cliente-conecta").prop("hidden", true);
        document.getElementById('busca-lead').classList.remove('invisible');
        document.getElementById('result-pesquisa-lead').classList.remove('invisible');
        if (Europa.Controllers.Cliente.NovoCliente) {
            $("#btn-salvar-cliente-conecta").prop("hidden", true);
        }
        document.getElementById('btn-integrar-cliente-conecta2').classList.add('btn-bloq');
    }
    if ($("#Cliente_terceiro_nao:checked").val()=="False") {
        $("#btn-integrar-cliente-conecta").prop("hidden", false)
        $("#InformacoesGeraisDto_NomeLead", "#modal_integracao_conecta").val("");
        $("#InformacoesGeraisDto_Uuid").val("");
        $("#InformacoesGeraisDto_TelefoneLead", "#modal_integracao_conecta").val("");
        document.getElementById('busca-lead').classList.add('invisible');
        document.getElementById('result-pesquisa-lead').classList.add('invisible');
        if (Europa.Controllers.Cliente.NovoCliente) {
            $("#btn-salvar-cliente-conecta").prop("hidden", false);
            $("#btn-integrar-cliente-conecta").prop("hidden", true);
        }
        document.getElementById('btn-integrar-cliente-conecta2').classList.remove('btn-bloq');
    }
}
Europa.Controllers.Cliente.OnSelectLeadTerceiro = function (uui,nome,telefone) {
    document.getElementById('btn-integrar-cliente-conecta3').classList.remove('btn-bloq');
    $("#InformacoesGeraisDto_UuidLead").val(uui);
    $("#InformacoesGeraisDto_NomeLead", "#modal_integracao_conecta").val(nome);
    $("#InformacoesGeraisDto_TelefoneLead", "#modal_integracao_conecta").val(telefone);
}
Europa.Controllers.Cliente.BuscarLeadConecta = function () {
    var data = {        
        Nome: $("#InformacoesGeraisDto_NomeCompleto", "#modal_integracao_conecta").val(),
        DataSourceRequest: {
            start: 0,
            pageSize: 15,
        }
    }
    console.log(data);
    if ($("#Cliente_terceiro_nao:checked").val() == "False" || $("#Cliente_terceiro_sim:checked").val() == undefined) {
        return;
    }
    $.post(Europa.Controllers.Cliente.UrlBuscarLeadConecta, data, function (res) {
        if (res!=null) {
            $('#result-pesquisa-lead').replaceWith(res);
        } else {
            $('#result-pesquisa-lead').replaceWith('<div id="result-pesquisa-lead"></div>');
        }
    })
}
Europa.Controllers.Cliente.CloseModalMensagemConecta = function (error) {
    $("#Cliente_terceiro_sim:checked").val(false);
    $("#Cliente_terceiro_nao:checked").val(false);
    $("modal_integracao_conecta").find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    if (error)
        $("#modal_mensagem_conecta").modal("hide");
    else {
        let basePath = location.href.split("/Index")[0];
        let novoId = Europa.Controllers.Cliente.GetId();
        location.href = basePath + "/Index/" + novoId;
    }
    
}
Europa.Controllers.Cliente.AbrirModalMensagemConecta = function () {
    $("#modal_mensagem_conecta").modal("show");
}
Europa.Controllers.Cliente.IntegrarClienteConecta = function () {
    if (Europa.Controllers.Cliente.NovoCliente) {
        Europa.Controllers.Cliente.Salvar(true);
        return
    }       


    $("#btn-integrar-cliente-conecta").prop("disabled", true);

    var clienteDto = Europa.Controllers.Cliente.GetFormData();

    clienteDto["IdCliente"] = $("#IdCliente").val();
    clienteDto["Uuid"] = $("#InformacoesGeraisDto_UuidLead").val();
    clienteDto["NomeLead"] = $("#InformacoesGeraisDto_NomeLead", "#modal_integracao_conecta").val();
    clienteDto["TelefoneLead"] = $("#InformacoesGeraisDto_TelefoneLead", "#modal_integracao_conecta").val();

    var url = $("#Cliente_terceiro_sim:checked").val() == "True" ? Europa.Controllers.Cliente.UrlIntegrarClienteConecta : Europa.Controllers.Cliente.UrlCriarLeadConecta;
    $.post(url, clienteDto, function (res) {
        console.log(res)
        //Europa.Informacao.PosAcaoBaseResponse(res);
        Europa.Controllers.Cliente.MensagemError = false;
        if (res.Success) {
            $("#mensagem_conecta_texto").text("Dados do comprador salvos com sucesso!");
            Europa.Controllers.Cliente.AbrirModalMensagemConecta();
            Europa.Informacao.Hide = function () {
                location.reload(true);
            }
        }
        else {
            Europa.Informacao.PosAcaoBaseResponse(res);
        }

        $("#btn-integrar-cliente-conecta").prop("disabled", false);
    })

}

Europa.Controllers.Cliente.SalvarClienteLead = function () {
    Europa.Controllers.Cliente.AbrirModalIntegracaoConecta();

    if (Europa.Controllers.Cliente.NovoCliente) {
        $("#btn-fechar-integrar-cliente-conecta").prop("hidden", true);
        $("#btn-integrar-cliente-conecta").prop("hidden", true);
        $("#btn-salvar-cliente-conecta").prop("hidden", true);
    }
}

Europa.Controllers.Cliente.VincularLead = function () {
    var objeto = Europa.Controllers.Cliente.GetFormData();
    console.log(objeto["InformacoesGeraisDto.NomeCompleto"]);
    if (objeto["InformacoesGeraisDto.NomeCompleto"] == null || objeto["InformacoesGeraisDto.NomeCompleto"] == "") {
        var msg = {
            Mensagens: [
                "Dados do cliente não preenchido",
            ]
        }
        Europa.Informacao.PosAcao(msg);
        return;
    }
    else {
        $("#Cliente_terceiro_sim:checked").prop("checked", false);
        $("#Cliente_terceiro_nao:checked").prop("checked", false);
        $("#Cliente_NomeLead", "#modal_integracao_conecta").val("");
        $("#Cliente_NomeLead", "#modal_integracao_conecta").prop("disabled", true);
        $("#Cliente_TelefoneLead", "#modal_integracao_conecta").val("");
        $("#Cliente_TelefoneLead", "#modal_integracao_conecta").prop("disabled", true);
        $("#btn-integrar-cliente-conecta", "#modal_integracao_conecta").prop("hidden", true);
        document.getElementById('btn-integrar-cliente-conecta2').classList.add('btn-bloq');
        $('#result-pesquisa-lead').replaceWith('<div id="result-pesquisa-lead"></div>');
        document.getElementById('busca-lead').classList.add('invisible');
//        document.getElementById('btn-integrar-cliente-conecta3').classList.add('btn-bloq');        
        
        Europa.Controllers.Cliente.AbrirModalIntegracaoConecta();
    }
}

