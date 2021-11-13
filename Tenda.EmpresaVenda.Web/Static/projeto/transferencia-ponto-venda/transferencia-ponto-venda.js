﻿Europa.Controllers.PontoVenda = {};
Europa.Controllers.PontoVenda.Tabela = {};

$(function () {
    Europa.Controllers.PontoVenda.InitAutoCompletes();
    Europa.Controllers.PontoVenda.ConfigureAutoCompleteNovoViabilizador(Europa.Controllers.PontoVenda);

    $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
    $(".ng-scope").css("word-wrap", "break-word");
});


Europa.Controllers.PontoVenda.InitAutoCompletes = function () {

    Europa.Controllers.PontoVenda.AutoCompleteViabilizador = new Europa.Components.AutoCompleteViabilizador()
        .WithTargetSuffix("viabilizador").Configure();

    Europa.Controllers.PontoVenda.AutoCompleteNovoViabilizador = new Europa.Components.AutoCompleteViabilizador()
        .WithTargetSuffix("novo_viabilizador").Configure();

    Europa.Controllers.PontoVenda.AutoPontoVenda = new Europa.Components.AutoCompletePontoVenda()
        .WithTargetSuffix("ponto_venda")
        .Configure();
};


function PrePropostaTabela($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.PontoVenda.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.PontoVenda.Tabela;
    self.setColumns([
        DTColumnBuilder.newColumn('NomePontoVenda').withTitle(Europa.i18n.Messages.PontoVenda).withOption('width', '150px'),
        DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption('width', '150px'),
        DTColumnBuilder.newColumn('Codigo').withTitle(Europa.i18n.Messages.Codigo).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeCliente').withTitle(Europa.i18n.Messages.Cliente).withOption('width', '250px'),
        DTColumnBuilder.newColumn('CpfCnpj').withTitle(Europa.i18n.Messages.CpfCnpj).withOption('width', '150px').renderWith(Europa.String.FormatCpf),
        DTColumnBuilder.newColumn('SituacaoPrePropostaSuatEvs').withTitle(Europa.i18n.Messages.SituacaoPreProposta).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeEmpresaVenda').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeCorretor').withTitle(Europa.i18n.Messages.Corretor).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeViabilizador').withTitle(Europa.i18n.Messages.Viabilizador).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeElaborador').withTitle(Europa.i18n.Messages.Elaborador).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeBreveLancamento').withTitle(Europa.i18n.Messages.Produto).withOption('width', '150px'),
        DTColumnBuilder.newColumn('Elaboracao').withTitle(Europa.i18n.Messages.DataElaboracao).withOption('width', '150px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('DataEnvio').withTitle(Europa.i18n.Messages.DataUltimoEnvio).withOption('width', '150px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('NomeAssistenteAnalise').withTitle(Europa.i18n.Messages.AssistenteAnalise).withOption('width', '150px'),
        DTColumnBuilder.newColumn('TipoRenda').withTitle(Europa.i18n.Messages.TipoRenda).withOption('width', '150px').withOption('type', 'enum-format-TipoRenda'),
        DTColumnBuilder.newColumn('RendaApurada').withTitle(Europa.i18n.Messages.RendaFamiliar).withOption('width', '150px').renderWith(Europa.String.FormatDinheiro),
        DTColumnBuilder.newColumn('FgtsApurado').withTitle(Europa.i18n.Messages.FGTSApurado).withOption('width', '150px').renderWith(Europa.String.FormatDinheiro),
        DTColumnBuilder.newColumn('Entrada').withTitle(Europa.i18n.Messages.Entrada).withOption('width', '150px').renderWith(Europa.String.FormatDinheiro),
        DTColumnBuilder.newColumn('PreChaves').withTitle(Europa.i18n.Messages.PreChaves).withOption('width', '150px').renderWith(Europa.String.FormatDinheiro),
        DTColumnBuilder.newColumn('PreChavesIntermediaria').withTitle(Europa.i18n.Messages.PreChavesIntermediaria).withOption('width', '250px').renderWith(Europa.String.FormatDinheiro),
        DTColumnBuilder.newColumn('Fgts').withTitle(Europa.i18n.Messages.FGTS).withOption('width', '150px').renderWith(Europa.String.FormatDinheiro),
        DTColumnBuilder.newColumn('Subsidio').withTitle(Europa.i18n.Messages.Subsidio).withOption('width', '150px').renderWith(Europa.String.FormatDinheiro),
        DTColumnBuilder.newColumn('Financiamento').withTitle(Europa.i18n.Messages.Financiamento).withOption('width', '150px').renderWith(Europa.String.FormatDinheiro),
        DTColumnBuilder.newColumn('PosChaves').withTitle(Europa.i18n.Messages.PosChaves).withOption('width', '150px').renderWith(Europa.String.FormatDinheiro),
        DTColumnBuilder.newColumn('StatusSicaq').withTitle(Europa.i18n.Messages.StatusSicaq).withOption('width', '150px').withOption('type', 'enum-format-StatusSicaq'),
        DTColumnBuilder.newColumn('NomeAnalistaSicaq').withTitle(Europa.i18n.Messages.AnalistaSicaq).withOption('width', '150px'),
        DTColumnBuilder.newColumn('DataSicaq').withTitle(Europa.i18n.Messages.DataHoraSicaq).withOption('width', '150px').withOption("type", "date-format-DD/MM/YYYY HH:mm:ss"),
        DTColumnBuilder.newColumn('ParcelaAprovada').withTitle(Europa.i18n.Messages.ParcelaAprovadaDoSICAQ).withOption('width', '250px').renderWith(Europa.String.FormatDinheiro)
    ])
        .setAutoInit(false)
        .setDefaultOptions('POST', Europa.Controllers.PontoVenda.UrlListar, Europa.Controllers.PontoVenda.FilterParams);
}

DataTableApp.controller('PreProposta', PrePropostaTabela);

Europa.Controllers.PontoVenda.FilterParams = function () {
    param = {
        idViabilizador: $("#autocomplete_viabilizador").val(),
        idEmpresaVenda: $("#autocomplete_empresa_venda").val(),
        idPontoVenda: $("#autocomplete_ponto_venda").val(),
    }
    return param;
};

Europa.Controllers.PontoVenda.LimparFiltro = function () {
    $("#autocomplete_viabilizador").val("").trigger("change");
    $("#autocomplete_novo_viabilizador").val("").trigger("change");
    $("#autocomplete_ponto_venda").val("").trigger("change");
};

Europa.Controllers.PontoVenda.Filtrar = function () {
    Europa.Controllers.PontoVenda.Tabela.reloadData();
    Europa.Controllers.PontoVenda.AutoCompleteNovoViabilizador.Clean();
};

function registrosSelecionados() {
    var itens = Europa.Controllers.PontoVenda.Tabela.getRowsSelect();
    var registros = [];

    itens.forEach(function (item) {
        registros.push(item);
    });
    if ($("#autocomplete_viabilizador").val() == null || $("#autocomplete_novo_viabilizador").val() == null) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.i18n.Messages.NenhumViabilizadorSelecionado);
        Europa.Informacao.Show();
        return null;
    }
    else if (registros.length < 1) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.i18n.Messages.NenhumRegistroSelecionando);
        Europa.Informacao.Show();
        return null;
    }
    return registros;
}

//Europa.Controllers.PontoVenda.TransferirSelecionados = function () {
//    var registros = registrosSelecionados();
//    if (registros !== null) {
//        Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.Confirmacao, Europa.i18n.Messages.MensagemConfirmacaoTransferir," ", function () {
//            var idRegs = [];
//            registros.forEach(function (item) {
//                idRegs.push(item.Id);
//            });
//            var idViabilizador = $("#autocomplete_novo_viabilizador").val();
//            $.ajax({
//                    url: Europa.Controllers.Carteira.UrlTransferirSelecionados,
//                    dataType: 'json',
//                data: { idsPreProposta: idRegs, idViabilizador: idViabilizador},
//                    type: 'POST'
//             }).done(function (data) {
//                    if (data.Sucesso) {
//                        Europa.Controllers.Carteira.Filtrar();
//                    }
//                    Europa.Informacao.PosAcao(data);
//            });
//        });
//    }   
//};

Europa.Controllers.PontoVenda.TransferirTodos = function () {
    var idViabilizador = $("#autocomplete_viabilizador").val();
    var idNovoViabilizador = $("#autocomplete_novo_viabilizador").val();
    var idPontoVenda = $("#autocomplete_ponto_venda").val();
    console.log(idPontoVenda);
    if (idNovoViabilizador !== null && idViabilizador !== null) {
        Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.Confirmacao, Europa.i18n.Messages.MensagemConfirmacaoTransferirTodos," ", function () {
            $.ajax({
                url: Europa.Controllers.PontoVenda.UrlTransferirTodos,
                dataType: 'json',
                data: { idViabilizador: idViabilizador, idNovoViabilizador: idNovoViabilizador, idPontoVenda: idPontoVenda},
                type: 'POST'
            }).done(function (data) {
                if (data.Sucesso) {
                    Europa.Controllers.PontoVenda.Filtrar();
                }
                Europa.Informacao.PosAcao(data);
            });
        });
    } else {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.i18n.Messages.NenhumViabilizadorSelecionado);
        Europa.Informacao.Show();
        return null;
    }
};

Europa.Controllers.PontoVenda.ConfigureAutoCompleteNovoViabilizador = function (autocompleteWrapper) {
    autocompleteWrapper.AutoCompleteNovoViabilizador.Data = function (params) {
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
                        return $("#autocomplete_viabilizador").val();
                    },
                    column: 'exceto_viabilizador'
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

    autocompleteWrapper.AutoCompleteNovoViabilizador.Configure();
}



