Europa.Controllers.PontoVenda = {};
Europa.Controllers.PontoVenda.Tabela = {};
Europa.Controllers.PontoVenda.Inclur = false;

$(function () {
    $("#filtro_situacoes").select2({
        trags: true
    });
    $('#filtro_situacoes').val(1).trigger('change');
    Europa.Controllers.PontoVenda.InitAutoCompleteEmpresaVendaFiltro();
    Europa.Controllers.PontoVenda.Tabela.reloadData();

    Europa.Controllers.PontoVenda.AutoCompleteViabilizadorFiltro = new Europa.Components.AutoCompleteViabilizador()
        .WithTargetSuffix("filtro_viabilizador").Configure();
});

Europa.Controllers.PontoVenda.LimparFiltro = function () {
    $("#filtro_nome").val("");
    $('#filtro_situacoes').val(1).trigger('change');
    $("#filtro_iniciativa").val("");
    Europa.Controllers.PontoVenda.AutoCompleteEmpresaVendaInclusaoFiltro.Clean();
    Europa.Controllers.PontoVenda.AutoCompleteViabilizadorFiltro.Clean();
};



Europa.Controllers.PontoVenda.InitAutoCompleteEmpresaVendaFiltro = function () {
    Europa.Controllers.PontoVenda.AutoCompleteEmpresaVendaInclusaoFiltro = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("filtro_empresa_venda").Configure();
};
