$(function () {
    Europa.Controllers.EmpresaVenda.AutoCompleteLojaDisponivel = new Europa.Components.AutoCompleteLojaDisponivel()
        .WithTargetSuffix("lojas_disponiveis")
        .Configure();

    var idLoja = $("#Loja_Id").val();
    var nomeLoja = $("#Loja_Nome").val();
    Europa.Controllers.EmpresaVenda.AutoCompleteLojaDisponivel.SetValue(idLoja, nomeLoja)

    Europa.Controllers.EmpresaVenda.AutoCompleteRegionalEmpresa = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regional_empresa")
        .Configure();

    var idEmpresaVenda = $("#Id").val();
    $.get(Europa.Controllers.EmpresaVenda.urlBuscarRegionaisEmpresa, { idEmpresaVenda: idEmpresaVenda}, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.EmpresaVenda.AutoCompleteRegionalEmpresa.SetMultipleValues(res.Objeto.Regionais, "Id", "Nome");
        } else {
            Europa.Informacao.PosAcao(res);
        }
    });    
});

Europa.Controllers.EmpresaVenda.AbrirModalConfig = function () {
    $('#modal-configuracoes').modal('show');
}
Europa.Controllers.EmpresaVenda.FecharModalConfig = function () {
    $('#modal-configuracoes').modal('hide');
}
Europa.Controllers.EmpresaVenda.SalvarModal = function () {
    $('#modal-configuracoes').modal('hide');
    Europa.Controllers.EmpresaVenda.Salvar();
}