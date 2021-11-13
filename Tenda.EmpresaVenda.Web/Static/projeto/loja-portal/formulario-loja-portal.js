$(function () {

    Europa.Controllers.LojaPortal.AutoCompleteCentralVenda = new Europa.Components.AutoCompleteLojaDisponivel()
        .WithTargetSuffix("central_vendas")
        .Configure();

    Europa.Controllers.LojaPortal.AutoCompleteRegionaisLoja = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regional_loja")
        .Configure();

    Europa.Controllers.LojaPortal.AutoCompleteRegionais = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regional")
        .Configure();

    $('#filtro_estados').select2({
        trags: true
    });

    var idCentralVenda = $("#IdCentralVenda").val();
    var nomeCentralVenda = $("#NomeCentralVenda").val();
    if (idCentralVenda > 0) {
        Europa.Controllers.LojaPortal.AutoCompleteCentralVenda.SetValue(idCentralVenda, nomeCentralVenda);
    }
    var idLoja = $("#Id").val();
    $.get(Europa.Controllers.LojaPortal.urlBuscarRegionaisLoja, { idEmpresaVenda: idLoja }, function (res) {
        if (res.Sucesso) {
            console.log("hit")
            Europa.Controllers.LojaPortal.AutoCompleteRegionaisLoja.SetMultipleValues(res.Objeto.Regionais, "Id", "Nome");
        } else {
            Europa.Informacao.PosAcao(res);
        }
    });    
})