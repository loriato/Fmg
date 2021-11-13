Europa.Controllers.PrePropostaWorkflow = {};

$(function () {

});

Europa.Controllers.PrePropostaWorkflow.RequestAcao = function (idPreProposta, url) {
    var result = undefined;

    $.post(url, { idPreProposta: idPreProposta }, function (response) {
        result = response;
    });

    return result;
};

Europa.Controllers.PrePropostaWorkflow.AguardandoAnaliseCompleta = function () {
    var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlAguardandoAnaliseCompleta);
    return result;
}

//Reenvio para Análise
Europa.Controllers.PrePropostaWorkflow.ReenviarAnaliseCompletaAprovada = function (data) {
    var result = undefined;
    var url = Europa.Controllers.PrePropostaWorkflow.UrlReenviarAnaliseCompletaAprovada;

    $.ajax({
        type: "POST",
        url: url,
        async: false,
        data: data
    }).done(function (response) {
        result = response;
    });
    return result;
}

//Analise Simplificada Aprovada
Europa.Controllers.PrePropostaWorkflow.AnaliseSimplificadaAprovada = function (idPreProposta) {
    var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlAnaliseSimplificadaAprovada);
    
    return result;
}