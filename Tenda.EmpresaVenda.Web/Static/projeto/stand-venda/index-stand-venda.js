Europa.Controllers.StandVenda = {};
Europa.Controllers.StandVenda.StandVendaTable = undefined;
Europa.Controllers.StandVenda.StandEmpresaVendaTable = undefined;
//Europa.Controllers.StandVenda.IdPontoVenda = undefined;
//Europa.Controllers.StandVenda.Regional = undefined;
//Europa.Controllers.StandVenda.IdStandVenda = 0;

$(function () {
    Europa.Controllers.StandVenda.AutoCompleteRegionalFiltro = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regional")
        .WithPlaceholder("Regional")
        .Configure();

    $("#filtro_estados").select2({
        trags: true,
        placeholder: "UF"
    });
    $("#filtro_estados").val(0).trigger("change")
});