"use strict";

Europa.Controllers.ConsultaEstoque = {};
Europa.Controllers.ConsultaEstoque.EstoqueEmpreendimento = {};
Europa.Controllers.ConsultaEstoque.EstoqueEmpreendimento.Tabela = {};
Europa.Controllers.ConsultaEstoque.EstoqueUnidade = {};
Europa.Controllers.ConsultaEstoque.EstoqueUnidade.Tabela = {};

////////////////////////////////////////////////////////////////////////////////////
// Datatable Estoque Empreendimento
////////////////////////////////////////////////////////////////////////////////////
DataTableApp.controller('estoqueEmpreendimentoTable', estoqueEmpreendimento);

function estoqueEmpreendimento($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.ConsultaEstoque.EstoqueEmpreendimento.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var wrapper = Europa.Controllers.ConsultaEstoque.EstoqueEmpreendimento.Tabela;

    wrapper.setColumns([
        DTColumnBuilder.newColumn('NomeEmpreendimento').withTitle(Europa.i18n.Messages.Empreendimento).withOption('width', '13%'),
        DTColumnBuilder.newColumn('Divisao').withTitle(Europa.i18n.Messages.Divisao).withOption('width', '13%'),
        DTColumnBuilder.newColumn('Bairro').withTitle(Europa.i18n.Messages.Bairro).withOption('width', '13%'),
        DTColumnBuilder.newColumn('QtdeDisponivel').withTitle(Europa.i18n.Messages.QtdDisponiveis).withOption('width', '10%'),
        DTColumnBuilder.newColumn('QtdeReservado').withTitle(Europa.i18n.Messages.QtdReservadas).withOption('width', '8%'),
        DTColumnBuilder.newColumn('QtdeVendido').withTitle(Europa.i18n.Messages.QtdVendidas).withOption('width', '10%'),
        DTColumnBuilder.newColumn('Caracteristicas').withTitle(Europa.i18n.Messages.Caracteristicas).withOption('width', '10%'),
        DTColumnBuilder.newColumn('QtdeUnidades').withTitle(Europa.i18n.Messages.QtdUnidades).withOption('width', '10%'),
        DTColumnBuilder.newColumn('PrevisaoEntrega').withTitle(Europa.i18n.Messages.PrevisaoEntrega).withOption('width', '12%').withOption("type", "date-format-DD/MM/YYYY")
    ])
    .setIdAreaHeader("datatable_estoque_empreendimento_header")
    .setColActions(actionsHtml, '10%')
    .setAutoInit()
    .setOptionsMultiSelect('POST', Europa.Controllers.ConsultaEstoque.UrlConsultaEmpreendimento, Europa.Controllers.ConsultaEstoque.EstoqueEmpreendimento.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            '</div>';
    }
}

Europa.Controllers.ConsultaEstoque.EstoqueEmpreendimento.FilterParams = function () {
    var filtro = {
        nome: $('#filtro_nome').val(),
        cpfCnpjCreci: $('#filtro_cpf_cnpj_creci').val(),
        situacao: $('#filtro_situacoes').val()
    };
    return filtro;
};

Europa.Controllers.ConsultaEstoque.EstoqueEmpreendimento.FiltrarTabela = function () {
    Europa.Controllers.ConsultaEstoque.EstoqueEmpreendimento.Tabela.reloadData();
};

Europa.Controllers.ConsultaEstoque.EstoqueEmpreendimento.LimparFiltro = function () {

};




////////////////////////////////////////////////////////////////////////////////////
// Datatable Estoque Unidade
////////////////////////////////////////////////////////////////////////////////////
DataTableApp.controller('estoqueUnidadeTable', estoqueUnidade);

function estoqueUnidade($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.ConsultaEstoque.EstoqueUnidade.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var wrapper = Europa.Controllers.ConsultaEstoque.EstoqueUnidade.Tabela;

    wrapper.setColumns([
        DTColumnBuilder.newColumn('NomeTorre').withTitle(Europa.i18n.Messages.Torre).withOption('width', '13%'),
        DTColumnBuilder.newColumn('NomeUnidade').withTitle(Europa.i18n.Messages.Unidade).withOption('width', '13%'),
        DTColumnBuilder.newColumn('Metragem').withTitle(Europa.i18n.Messages.Metragem).withOption('width', '13%'),
        DTColumnBuilder.newColumn('Andar').withTitle(Europa.i18n.Messages.Andar).withOption('width', '10%'),
    ])
        .setIdAreaHeader("datatable_estoque_unidade_header")
        .setColActions(actionsHtml, '10%')
        .setAutoInit()
        .setOptionsMultiSelect('POST', Europa.Controllers.ConsultaEstoque.UrlConsultaUnidade, Europa.Controllers.ConsultaEstoque.EstoqueUnidade.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            '</div>';
    }
}

Europa.Controllers.ConsultaEstoque.EstoqueUnidade.FilterParams = function () {
    var filtro = {
        nome: $('#filtro_nome').val(),
        cpfCnpjCreci: $('#filtro_cpf_cnpj_creci').val(),
        situacao: $('#filtro_situacoes').val()
    };
    return filtro;
};

Europa.Controllers.ConsultaEstoque.EstoqueUnidade.FiltrarTabela = function () {
    Europa.Controllers.ConsultaEstoque.EstoqueUnidade.Tabela.reloadData();
};

Europa.Controllers.ConsultaEstoque.EstoqueUnidade.LimparFiltro = function () {

};