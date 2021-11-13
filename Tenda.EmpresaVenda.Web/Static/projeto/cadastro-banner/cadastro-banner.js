Europa.Controllers.CadastroBanner = {};
Europa.Controllers.CadastroBanner.Tabela = {};
Europa.Controllers.CadastroBanner.Url = false;
Europa.Controllers.CadastroBanner.IdBanner = 0;

$(function () {
    Europa.Controllers.CadastroBanner.ConfigDatePicker();
    $("#filtro_estados").select2({
        trags: true
    });
    Europa.Controllers.CadastroBanner.AutoCompleteRegional = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regional")
        .Configure();
});

Europa.Controllers.CadastroBanner.LimparFiltro = function () {
    $("#DescricaoBanner").val("");
    $("#TipoBannerForm").val(0);
    $("#SituacaoBannerFiltro").val(0);
    $("#DataInicioVigencia").val("");
    $("#DataFimVigencia").val("");
    $("#filtro_estados").val(0);
    $("#autocomplete_regional").val(0);
    $("#DiretorBannerFiltro").prop("checked", false);
    $("#ExibicaoBannerFiltro").prop("checked", false);

    Europa.Controllers.CadastroBanner.ConfigDatePicker();
};

Europa.Controllers.CadastroBanner.ConfigDatePicker = function () {

    Europa.Controllers.CadastroBanner.DatePicker1 = new Europa.Components.DatePicker()
        .WithTarget("[name='DataInicioVigencia']")
        .WithFormat("DD/MM/YYYY")
        .Configure();

    Europa.Controllers.CadastroBanner.DatePicker2 = new Europa.Components.DatePicker()
        .WithTarget("[name='DataFimVigencia']")
        .WithFormat("DD/MM/YYYY")
        .Configure();

    Europa.Controllers.CadastroBanner.DataInicio = new Europa.Components.DatePicker()
        .WithTarget("#InicioVigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate(Europa.Date.Now())
        .Configure();

    Europa.Controllers.CadastroBanner.DataFim = new Europa.Components.DatePicker()
        .WithTarget("#FimVigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate(Europa.Date.Now())
        .Configure();

}

Europa.Controllers.CadastroBanner.OnChangeDataInicio = function () {
    Europa.Controllers.CadastroBanner.DataFim = new Europa.Components.DatePicker()
        .WithTarget("#FimVigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#InicioVigencia").val())
        .Configure();
};

Europa.Controllers.CadastroBanner.OnChangeDataInicioFiltro = function () {
    Europa.Controllers.CadastroBanner.DatePicker2 = new Europa.Components.DatePicker()
        .WithTarget("[name='DataFimVigencia']")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#DataInicioVigencia").val())
        .Configure();
};


