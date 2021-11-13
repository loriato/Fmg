Europa.Controllers.Simulador = {};
Europa.Controllers.Simulador.TabelaDetalhamentoFinanceiro = {};
Europa.Controllers.Simulador.TabelaResultadoSimulacao = {};
Europa.Controllers.Simulador.TabelaItbiEmolumento = {};
Europa.Controllers.Simulador.ModoInclusao = true;

$(function () {

});

Europa.Controllers.Simulador.AddError = function (fields) {
    fields.forEach(function (key) {
        $("[name='" + key + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.Simulador.PodeManterAssociacoes = function () {
    var value = $('#PodeManterAssociacoes').val();
    return 'true' === value || 'True' === value || '1' === value || 1 === value;
};