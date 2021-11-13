Europa.Controllers.PropostaFaturada = {};
Europa.Controllers.PropostaFaturada.Tabela = {};

$(function () {
    $("#filtro_estados").select2({
        trags: true
    });
    $("#filtro_estados").val(0).trigger("change");
});

