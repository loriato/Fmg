Europa.Controllers.Loja = {};
Europa.Controllers.Loja.Tabela = {};
Europa.Controllers.Loja.Permissoes = {};

$(function () {

    Europa.Controllers.Loja.AutoCompleteRegionais = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regional")
        .Configure();

    $("#filtro_situacao").select2({
        trags: true
    });
    $('#filtro_situacao').val(1).trigger('change');
    Europa.Controllers.Loja.FiltrarTabela();
});
