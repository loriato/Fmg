$(function () {
    Europa.Controllers.ResgatePontuacaoFidelidade.InitAutocomplete();
});

Europa.Controllers.ResgatePontuacaoFidelidade.InitAutocomplete = function () {
    Europa.Controllers.ResgatePontuacaoFidelidade.AutoCompleteEmpresaVenda = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("empresa_venda")
        .Configure();
};

Europa.Controllers.ResgatePontuacaoFidelidade.ValidarFiltro = function () {
    var evs = $("#autocomplete_empresa_venda").val();

    if (evs == undefined) {
        return false;
    }

    $("#tab1").click();

    return true;
};

Europa.Controllers.ResgatePontuacaoFidelidade.Filtro = function () {
    var filtro = {
        IdEmpresaVenda: $("#autocomplete_empresa_venda").val(),
        InicioVigencia: $("#InicioVigencia").val(),
        TerminoVigencia: $("#TerminoVigencia").val()
    };

    return filtro;
};

Europa.Controllers.ResgatePontuacaoFidelidade.LimparFiltro = function () {
    Europa.Controllers.ResgatePontuacaoFidelidade.AutoCompleteEmpresaVenda.Clean();
    $("#InicioVigencia").val("");
    $("#TerminoVigencia").val("");
};