Europa.Controllers.CockpitMidas = {};
Europa.Controllers.CockpitMidas.TableOcorrencias = {};

$(function () {

})

Europa.Controllers.CockpitMidas.Filtrar = function () {
    Europa.Controllers.CockpitMidas.FiltrarTabelaNotas();
    Europa.Controllers.CockpitMidas.FiltrarTabelaOcorrencias();
}

Europa.Controllers.CockpitMidas.LimparFiltro = function () {
    $('#filtro_ocorrencia').val('');
    $('#filtro_match').val(0).trigger('change'); 
    $("#data_previsao_pagamento_inicio").val('');
    $("#data_previsao_pagamento_termino").val('');
    $("#autocomplete_empresa_venda").val(0).trigger('change');
    $("#situacao_nota_fiscal").val('').trigger('change');
    $("#estado").val(0).trigger('change');
    $("#numero_pedido").val('');
    $("#numero_nf").val('');
    $('#numero_preproposta').val('');
    $('#filtro_cnpj_tomador').val('');
    $('#filtro_cnpj_prestador').val('');
    $('#filtro_comissionavel').val(0).trigger('change');
}

