﻿Europa.Controllers.AdiantamentoEmergencial = {};

$(function () {
    $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
    $(".ng-scope").css("word-wrap", "break-word");
    Europa.Controllers.AdiantamentoEmergencial.InitAutoComplete();
     
});

Europa.Controllers.AdiantamentoEmergencial.InitAutoComplete = function () {
    Europa.Controllers.AdiantamentoEmergencial.AutoCompleteEmpresaVendas = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("empresa_venda")
        .Configure();
}


function AdiantamentoEmergencialTabela($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.AdiantamentoEmergencial.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.AdiantamentoEmergencial.Tabela;
    self.setColumns([
        DTColumnBuilder.newColumn('Id').withTitle(Europa.i18n.Messages.Id).withClass("hidden", "hidden"),
        DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption('width', '100px'),
        DTColumnBuilder.newColumn('Divisao').withTitle(Europa.i18n.Messages.Divisao).withOption('width', '100px'),
        DTColumnBuilder.newColumn('NomeEmpreendimento').withTitle(Europa.i18n.Messages.Empreendimento).withOption('width', '200px'),
        DTColumnBuilder.newColumn('EmpresaDeVenda').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption('width', '200px'),
        DTColumnBuilder.newColumn('CentralVenda').withTitle(Europa.i18n.Messages.Central).withOption('width', '200px'),
        DTColumnBuilder.newColumn('NomeCliente').withTitle(Europa.i18n.Messages.Cliente).withOption('width', '200px'),
        DTColumnBuilder.newColumn('DataSicaq').withTitle(Europa.i18n.Messages.DataSicaq).withOption('width', '150px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('StatusContrato').withTitle(Europa.i18n.Messages.StatusContrato).withOption('width', '200px'),
        DTColumnBuilder.newColumn('EmReversao').withTitle(Europa.i18n.Messages.Reversao).withOption('width', '200px').renderWith(renderBool),
        DTColumnBuilder.newColumn('StatusRepasse').withTitle(Europa.i18n.Messages.StatusRepasse).withOption('width', '200px'),
        DTColumnBuilder.newColumn('DataRepasse').withTitle(Europa.i18n.Messages.DataRepasse).withOption('width', '200px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('StatusConformidade').withTitle(Europa.i18n.Messages.StatusConformidade).withOption('width', '200px'),
        DTColumnBuilder.newColumn('DataConformidade').withTitle(Europa.i18n.Messages.DataConformidade).withOption('width', '200px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('PassoAtual').withTitle(Europa.i18n.Messages.StatusPRO).withOption('width', '200px'),
        DTColumnBuilder.newColumn('DataKitCompleto').withTitle(Europa.i18n.Messages.DataKitCompleto).withOption('width', '200px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('CodigoRegra').withTitle(Europa.i18n.Messages.CodigoRegra).withOption('width', '150px').renderWith(renderRegra),
        DTColumnBuilder.newColumn('FaixaUmMeio').withTitle(Europa.i18n.Messages.ComissaoAcordadoUmMeio).withOption('width', '200px').renderWith(renderPorcentagem),
        DTColumnBuilder.newColumn('FaixaDois').withTitle(Europa.i18n.Messages.ComissaoAcordadoDois).withOption('width', '200px').renderWith(renderPorcentagem),
        DTColumnBuilder.newColumn('RegraPagamento').withTitle(Europa.i18n.Messages.RegraPagamento).withOption('width', '200px').renderWith(renderRegraPagamento),
        DTColumnBuilder.newColumn('ComissaoPagarUmMeio').withTitle(Europa.i18n.Messages.ComissaoPagarUmMeio).withOption('width', '200px').renderWith(renderPorcentagem),
        DTColumnBuilder.newColumn('ComissaoPagarDois').withTitle(Europa.i18n.Messages.ComissaoPagarDois).withOption('width', '200px').renderWith(renderPorcentagem),
        DTColumnBuilder.newColumn('CodigoFornecedor').withTitle(Europa.i18n.Messages.CodigoFornecedor).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeFornecedor').withTitle(Europa.i18n.Messages.Fornecedor).withOption('width', '200px'),
        DTColumnBuilder.newColumn('NomeEmpresaVenda').withTitle(Europa.i18n.Messages.Empresa).withOption('width', '200px'),
        DTColumnBuilder.newColumn('DescricaoTorre').withTitle(Europa.i18n.Messages.Bloco).withOption('width', '100px'),
        DTColumnBuilder.newColumn('DescricaoUnidade').withTitle(Europa.i18n.Messages.Unidade).withOption('width', '100px'),
        DTColumnBuilder.newColumn('DataVenda').withTitle(Europa.i18n.Messages.DataVenda).withOption('width', '150px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('ValorVGV').withTitle(Europa.i18n.Messages.ValorSemPremiada).withOption('width', '200px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('CodigoPreProposta').withTitle(Europa.i18n.Messages.PreProposta).withOption('width', '200px'),
        DTColumnBuilder.newColumn('CodigoProposta').withTitle(Europa.i18n.Messages.Proposta).withOption('width', '200px'),
        DTColumnBuilder.newColumn('DescricaoTipologia').withTitle(Europa.i18n.Messages.Tipologia).withOption('width', '200px'),
        DTColumnBuilder.newColumn('DataRegistro').withTitle(Europa.i18n.Messages.DataRegistro).withOption('width', '200px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('Pago').withTitle(Europa.i18n.Messages.Pago).withOption('width', '200px').renderWith(renderBool),
        DTColumnBuilder.newColumn('DataPagamento').withTitle(Europa.i18n.Messages.DataPedido).withOption('width', '200px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('Faturado').withTitle(Europa.i18n.Messages.Faturado).withOption('width', '200px').renderWith(renderBool),
        DTColumnBuilder.newColumn('Observacao').withTitle(Europa.i18n.Messages.Observacao).withOption('width', '200px'),

    ])
        .setAutoInit(false)
        .setIdAreaHeader("datatable_header")
        .setOptionsMultiSelect('POST', Europa.Controllers.AdiantamentoEmergencial.UrlListar, Europa.Controllers.AdiantamentoEmergencial.Filtro);

    function renderMoney(data) {
        if (data) {
            var valor = "R$ ";
            valor = valor + data.toFixed(2).replace(".", ",").replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
            return valor;
        }
        return "";
    }

    function renderPorcentagem(data) {
        if (data) {
            var valor = "%";
            return valor = data + valor;
        }
        return "";
    }

    function renderRegra(data, type, full, meta) {
        var link = '<div>';
        if (data) {
            link = link + "<a title='Regra de Comissão' target='_blank' href='" + Europa.Controllers.AdiantamentoEmergencial.UrlRegraComissao + '?regra=' + full.IdRegraComissao + "'>" + full.CodigoRegra + "</a>";
        }
        link += '</div>';
        return link;
    }

    function renderBool(data) {
        if (data) {
            return Europa.i18n.Messages.Sim;
        }
        return Europa.i18n.Messages.Nao;
    }

    function renderRegraPagamento(data, type, full, meta) {
        if (full.TipoPagamento == 1) {
            return data + "% Kit Completo";
        }
        if (full.TipoPagamento == 2) {
            return data + "% Repasse";
        }
        if (full.TipoPagamento == 3) {
            return data + "% Conformidade";
        }
    }
}

DataTableApp.controller('AdiantamentoEmergencial', AdiantamentoEmergencialTabela);

Europa.Controllers.AdiantamentoEmergencial.Filtrar = function () {
    Europa.Controllers.AdiantamentoEmergencial.Tabela.reloadData();
};

Europa.Controllers.AdiantamentoEmergencial.Filtro = function () {
    var param = {
        DataVendaDe: $("#filtro_data_venda_de").val(),
        DataVendaAte: $("#filtro_data_venda_ate").val(),
        IdsEmpresaVenda: $("#autocomplete_empresa_venda").val(),
        TipoPagamento: $("#filtro_tipo_pagamento").val(),
        TipoEmpresaVenda:1
    };
    return param;
};

Europa.Controllers.AdiantamentoEmergencial.LimparFiltro = function () {
    $("#autocomplete_empresa_venda").val("").trigger("change");
    $("#filtro_data_venda_de").val("");
    $("#filtro_tipo_pagamento").val("");
    $("#filtro_data_venda_ate").val("");
};


Europa.Controllers.AdiantamentoEmergencial.Validar = function () {
    var objetosSelecionados = Europa.Controllers.AdiantamentoEmergencial.Tabela.getRowsSelect();
    var autorizar = true;
    if (objetosSelecionados == null || objetosSelecionados == undefined || objetosSelecionados.length == 0) {
        Europa.Informacao.ChangeHeaderAndContent(
            Europa.i18n.Messages.Erro,
            Europa.i18n.Messages.NenhumRegistroSelecionando);

        autorizar = false;

        Europa.Informacao.Show();
    }

    return autorizar;
};

Europa.Controllers.AdiantamentoEmergencial.Solicitar = function () {
    var autorizar = Europa.Controllers.AdiantamentoEmergencial.Validar();

    if (!autorizar) {
        return;
    }
    
    var objetosSelecionados = Europa.Controllers.AdiantamentoEmergencial.Tabela.getRowsSelect();
    var Objeto = [];
    objetosSelecionados.forEach(function (item) {
        var novoObjeto = {
            TipoPagamento: item.TipoPagamento,
            IdProposta: item.IdProposta
        };
        Objeto.push(novoObjeto);
    });
    var url = Europa.Controllers.AdiantamentoEmergencial.UrlSolicitarAdiantamento;
    $.post(url, { model: Objeto }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.AdiantamentoEmergencial.Filtrar();
        }
        Europa.Informacao.PosAcao(res);
    });
};