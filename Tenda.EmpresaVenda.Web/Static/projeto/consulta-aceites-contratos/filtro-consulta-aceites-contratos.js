Europa.Controllers.ConsultaAceitesContratos.Filtrar = function () {
    Europa.Controllers.ConsultaAceitesContratos.Tabela.reloadData();
};

Europa.Controllers.ConsultaAceitesContratos.Filtro = function () {
    var param = {
        IdEmpresasVenda: $("#autocomplete_empresa_venda").val()
    };
    return param;
};