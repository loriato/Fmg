$(function () {
    Europa.Controllers.Gestao.InitAutoCompletesFiltro();
    Europa.Controllers.Gestao.AutoCompletePontoVendaFiltro.Disable();
});


Europa.Controllers.Gestao.InitAutoCompletesFiltro = function() {
    Europa.Controllers.Gestao.AutoCompleteTipoCustoFiltro = new Europa.Components.AutoCompleteTipoCusto()
        .WithTargetSuffix("filtro_tipo_custo").Configure();

    Europa.Controllers.Gestao.AutoCompleteClassificacaoFiltro = new Europa.Components.AutoCompleteClassificacao()
        .WithTargetSuffix("filtro_classificacao").Configure();

    Europa.Controllers.Gestao.AutoCompleteFornecedorFiltro = new Europa.Components.AutoCompleteFornecedor()
        .WithTargetSuffix("filtro_fornecedor").Configure();

    Europa.Controllers.Gestao.AutoCompleteEmpresaVendaFiltro = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("filtro_empresa_venda").Configure();

    Europa.Controllers.Gestao.ConfigureAutoCompleteEmpresaVendaFiltro(Europa.Controllers.Gestao);

    Europa.Controllers.Gestao.AutoCompletePontoVendaFiltro = new Europa.Components.AutoCompletePontoVenda()
        .WithTargetSuffix("filtro_ponto_venda").Configure();

    Europa.Controllers.Gestao.ConfigureAutoCompletePontoVendaFiltro(Europa.Controllers.Gestao)

    Europa.Controllers.Gestao.AutoCompleteCentroCustoFiltro = new Europa.Components.AutoCompleteCentroCusto()
        .WithTargetSuffix("filtro_centro_custo").Configure();
};

Europa.Controllers.Gestao.ConfigureAutoCompleteEmpresaVendaFiltro = function (autocompleteWrapper) {
    $('#autocomplete_filtro_empresa_venda').on('change', function (e) {
        var idTemp = $(this).val();
        if (idTemp === null || idTemp === undefined || idTemp === "" || idTemp === 0) {
            autocompleteWrapper.AutoCompletePontoVendaFiltro.Disable();
        } else {
            autocompleteWrapper.AutoCompletePontoVendaFiltro.Enable();
        }
        autocompleteWrapper.AutoCompletePontoVendaFiltro.Clean();
    });

    autocompleteWrapper.AutoCompleteEmpresaVendaFiltro.Configure();
}

Europa.Controllers.Gestao.ConfigureAutoCompletePontoVendaFiltro = function (autocompleteWrapper) {
    autocompleteWrapper.AutoCompletePontoVendaFiltro.Data = function (params) {
        return {
            start: 0,
            pageSize: 10,
            filter: [
                {
                    value: params.term,
                        column: this.param,
                            regex: true
                },
                {
                    value: function () {
                        return $("#autocomplete_filtro_empresa_venda").val();
                    },
                    column: 'idEmpresaVenda'
                }
            ],
            order: [
                {
                    value: "asc",
                    column: this.param
                }
            ]
        };
    };

    autocompleteWrapper.AutoCompletePontoVendaFiltro.Configure();
}
//{
//    value: params.term,
//        column: this.param,
//            regex: true
//},
//{
//    value: function () {
//        return $("#autocomplete_empresa_venda").val();
//    },
//    column: 'idEmpresaVenda'
//}
//            ],

Europa.Controllers.Gestao.LimparFiltro = function () {
    $("#ReferenciaDe").val("");
    $("#ReferenciaAte").val("");
    Europa.Controllers.Gestao.AutoCompleteTipoCustoFiltro.Clean();
    Europa.Controllers.Gestao.AutoCompleteClassificacaoFiltro.Clean();
    Europa.Controllers.Gestao.AutoCompleteFornecedorFiltro.Clean();
    Europa.Controllers.Gestao.AutoCompleteEmpresaVendaFiltro.Clean();
    Europa.Controllers.Gestao.AutoCompletePontoVendaFiltro.Clean();
    Europa.Controllers.Gestao.AutoCompleteCentroCustoFiltro.Clean();
    Europa.Controllers.Gestao.AutoCompletePontoVendaFiltro.Disable();
};
