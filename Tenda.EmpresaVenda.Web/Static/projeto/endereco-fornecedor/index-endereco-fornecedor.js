Europa.Controllers.EnderecoFornecedor = {};
Europa.Controllers.EnderecoFornecedor.Tabela = undefined;

$(function () {
    $("#filtro_estados").select2({
        trags: true,
        width: '100%'
    });
});

Europa.Controllers.EnderecoFornecedor.FilterParams = function () {
    return {
        CodigoFornecedor: $("#CodigoFornecedor").val(),
        Estados: $('#filtro_estados').val()
    };
};

Europa.Controllers.EnderecoFornecedor.LimparFiltro = function () {
    $('#filtro_estados').val(0).trigger('change');    
    $("#CodigoFornecedor").val("");
};

DataTableApp.controller('enderecoFornecedorTable', enderecoFornecedorTable);

function enderecoFornecedorTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.EnderecoFornecedor.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.EnderecoFornecedor.Tabela;
    self.setColumns([
        DTColumnBuilder.newColumn('Estado').withTitle(Europa.i18n.Messages.UF).withOption('width', '20px'),
        DTColumnBuilder.newColumn('CodigoFornecedor').withTitle(Europa.i18n.Messages.CodigoFornecedor).withOption('width', '100px'),
        DTColumnBuilder.newColumn('RazaoSocial').withTitle(Europa.i18n.Messages.RazaoSocial).withOption('width', '150px'),
        DTColumnBuilder.newColumn('Cnpj').withTitle(Europa.i18n.Messages.Cnpj).withOption('width', '150px').renderWith(Europa.String.FormatCnpj),
        DTColumnBuilder.newColumn('Logradouro').withTitle(Europa.i18n.Messages.Endereco).withOption('width', '150px').renderWith(renderEndereco),

    ])
        .setIdAreaHeader("endereco_fornecedor_datatable_header")
        .setDefaultOrder([[0, 'asc']])
        .setDefaultOptions('POST', Europa.Controllers.EnderecoFornecedor.UrlListarEnderecoFornecedor, Europa.Controllers.EnderecoFornecedor.FilterParams);

    function renderEndereco(data, type, full, meta) {
        var endereco = full.Logradouro + ", " +
            full.Numero + ", " +
            full.Bairro + ", " +
            "CEP "+Europa.String.FormatCep(full.Cep);

        return endereco;
    }
}

Europa.Controllers.EnderecoFornecedor.FiltrarTabela = function () {
    Europa.Controllers.EnderecoFornecedor.Tabela.reloadData();
}