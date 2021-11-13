$(function () {
    Europa.Controllers.NovaEv.AutoCompleteLojaDisponivel = new Europa.Components.AutoCompleteLojaDisponivel()
        .WithTargetSuffix("lojas_disponiveis")
        .Configure();

    Europa.Controllers.NovaEv.AutoCompleteRegionais = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regional_empresa")
        .Configure();

    var idLoja = $("#Loja_Id").val();
    var nomeLoja = $("#Loja_Nome").val();
    Europa.Controllers.NovaEv.AutoCompleteLojaDisponivel.SetValue(idLoja, nomeLoja)
});