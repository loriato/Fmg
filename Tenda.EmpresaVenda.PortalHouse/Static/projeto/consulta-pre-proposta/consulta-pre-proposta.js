"use strict";

Europa.Controllers.ConsultaPreProposta = {};
Europa.Controllers.ConsultaPreProposta.Table = {};

$(function () {
    Europa.Components.DatePicker.AutoApply();
    setTimeout(Europa.Controllers.ConsultaPreProposta.ConfigDatePicker, 300);

    Europa.Controllers.ConsultaPreProposta.AutoCompleteLoja = new Europa.Components.AutoCompleteLoja()
        .WithTargetSuffix("loja")
        .Configure();

    Europa.Controllers.ConsultaPreProposta.AutoCompleteAgenteVenda = new Europa.Components.AutoCompleteAgenteVendaHouseConsulta()
        .WithTargetSuffix("agente_venda")
        .Configure();

    Europa.Controllers.ConsultaPreProposta.AutoCompleteBreveLancamento = new Europa.Components.AutoCompleteBreveLancamentoRegional()
            .WithTargetSuffix("breve_lancamento")
        .Configure();

    Europa.Controllers.ConsultaPreProposta.AutoCompleteAgrupamentoProcessoPreProposta = new Europa.Components.AgrupamentoProcessoPreProposta()
        .WithTargetSuffix("agrupamento_processo_pre_proposta")
        .Configure();

});



Europa.Controllers.ConsultaPreProposta.ConfigDatePicker = function () {
    Europa.Controllers.ConsultaPreProposta.ElaboracaoDe = new Europa.Components.DatePicker()
        .WithTarget("#ElaboracaoDe")
        .WithFormat("DD/MM/YYYY")
        .Configure();

    Europa.Controllers.ConsultaPreProposta.ElaboracaoAte = new Europa.Components.DatePicker()
        .WithTarget("#ElaboracaoAte")
        .WithFormat("DD/MM/YYYY")
        .Configure();

    $("#situacao_pre_proposta").select2({
        trags: true,
        width: '100%'
    });

};



////////////////////////////////////////////////////////////////////////////////////
// Funções Datatable
////////////////////////////////////////////////////////////////////////////////////

$(function () {
    $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
    $(".ng-scope").css("word-wrap", "break-word");
});

DataTableApp.controller('consultaPrePropostaTable', consultaPrePropostaTable);

function consultaPrePropostaTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.ConsultaPreProposta.Table = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.ConsultaPreProposta.Table;
    self.setColumns([
        DTColumnBuilder.newColumn('Codigo').withTitle(Europa.i18n.Messages.Codigo).withOption('width', '150px')
            .withOption("link", self.withOptionLink(Europa.Components.DetailAction.PreProposta, "Id")),
        DTColumnBuilder.newColumn('NomeCliente').withTitle(Europa.i18n.Messages.Cliente).withOption('width', '250px')
            .withOption("link", self.withOptionLink(Europa.Components.DetailAction.Cliente, "IdCliente")),
        DTColumnBuilder.newColumn('SituacaoPrePropostaSuatEvs').withTitle(Europa.i18n.Messages.Status).withOption('width', '150px').renderWith(Europa.Controllers.ConsultaPreProposta.FormatEnum),
        DTColumnBuilder.newColumn('NumeroDocumentosPendentes').withTitle(Europa.i18n.Messages.NumeroDocumentosPendentes).withOption('width', '220px'),
        DTColumnBuilder.newColumn('MotivoParecer').withTitle(Europa.i18n.Messages.ParecerTenda).withOption('width', '600px'),
        DTColumnBuilder.newColumn('MotivoPendencia').withTitle(Europa.i18n.Messages.MotivoPendencia).withOption('width', '600px'),
        DTColumnBuilder.newColumn('NomePontoVenda').withTitle(Europa.i18n.Messages.PontoVenda).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeCorretor').withTitle(Europa.i18n.Messages.Corretor).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeViabilizador').withTitle(Europa.i18n.Messages.Viabilizador).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeElaborador').withTitle(Europa.i18n.Messages.Elaborador).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeBreveLancamento').withTitle(Europa.i18n.Messages.Produto).withOption('width', '150px'),
        DTColumnBuilder.newColumn('Elaboracao').withTitle(Europa.i18n.Messages.DataElaboracao).withOption('width', '150px').renderWith(Europa.Controllers.ConsultaPreProposta.Table.renderDateSmall),
        DTColumnBuilder.newColumn('DataEnvio').withTitle(Europa.i18n.Messages.DataUltimoEnvio).withOption('width', '150px').renderWith(Europa.Controllers.ConsultaPreProposta.Table.renderDateSmall),
        DTColumnBuilder.newColumn('NomeAssistenteAnalise').withTitle(Europa.i18n.Messages.AssistenteAnalise).withOption('width', '200px'),
        DTColumnBuilder.newColumn('TipoRenda').withTitle(Europa.i18n.Messages.TipoRenda).withOption('width', '150px').withOption('type', 'enum-format-TipoRenda'),
        DTColumnBuilder.newColumn('RendaApurada').withTitle(Europa.i18n.Messages.RendaFamiliar).withOption('width', '150px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('FgtsApurado').withTitle(Europa.i18n.Messages.FGTSApurado).withOption('width', '150px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('Entrada').withTitle(Europa.i18n.Messages.Entrada).withOption('width', '150px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('PreChaves').withTitle(Europa.i18n.Messages.PreChaves).withOption('width', '150px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('PreChavesIntermediaria').withTitle(Europa.i18n.Messages.PreChavesIntermediaria).withOption('width', '250px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('Fgts').withTitle(Europa.i18n.Messages.FGTS).withOption('width', '150px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('Subsidio').withTitle(Europa.i18n.Messages.Subsidio).withOption('width', '150px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('Financiamento').withTitle(Europa.i18n.Messages.Financiamento).withOption('width', '150px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('PosChaves').withTitle(Europa.i18n.Messages.PosChaves).withOption('width', '150px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('StatusSicaq').withTitle(Europa.i18n.Messages.StatusSicaq).withOption('width', '150px').withOption('type', 'enum-format-StatusSicaq'),
        DTColumnBuilder.newColumn('NomeAnalistaSicaq').withTitle(Europa.i18n.Messages.AnalistaSicaq).withOption('width', '150px'),
        DTColumnBuilder.newColumn('DataSicaq').withTitle(Europa.i18n.Messages.DataHoraSicaq).withOption('width', '150px').withOption("type", "date-format-DD/MM/YYYY HH:mm:ss"),
        DTColumnBuilder.newColumn('ParcelaAprovada').withTitle(Europa.i18n.Messages.ParcelaAprovadaDoSICAQ).withOption('width', '250px').renderWith(renderValorParcela),
        DTColumnBuilder.newColumn('OrigemCliente').withTitle(Europa.i18n.Messages.OrigemCliente).withOption('width', '175px').withOption('type', 'enum-format-TipoOrigemCliente')
    ])
        .setIdAreaHeader("consulta_pre_proposta_datatable_header")
        .setDefaultOrder([[0, 'desc']])
        .setAutoInit(false)
        .setDefaultOptions('POST', Europa.Controllers.ConsultaPreProposta.UrlListar, Europa.Controllers.ConsultaPreProposta.FilterParams);

    function renderMoney(data) {
        if (data) {
            var value = "R$ ";
            value = value + data.toFixed(2).replace(".", ",").replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
            return value;
        }
        return "";
    };

    function renderValorParcela(data, type, full, meta) {
        if (full.DataSicaq) {
            var value = full.ParcelaAprovada;
            if (value === undefined || value === '' || value === null) {
                return "";
            }
            return "R$ " + value.toFixed(2).replace(".", ",").replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
        }
    }


}

Europa.Controllers.ConsultaPreProposta.FormatEnum = function (data, meta, full, type) {
    console.log(((full.SituacaoPrePropostaHouse == "" || full.SituacaoPrePropostaHouse == null) ? data : full.SituacaoPrePropostaHouse));
    if (data) {
        switch (data) {
            case "Aguardando Auditoria":
                return Europa.i18n.Messages.SituacaoProposta_EmAnalise;
            default:
                return ((full.SituacaoPrePropostaHouse == "" || full.SituacaoPrePropostaHouse == null) ? data : full.SituacaoPrePropostaHouse);
        }
    }
    return ((full.SituacaoPrePropostaHouse == "" || full.SituacaoPrePropostaHouse == null) ? data : full.SituacaoPrePropostaHouse);
}

Europa.Controllers.ConsultaPreProposta.FilterParams = function () {
    if ($("#autocomplete_agrupamento_processo_pre_proposta").val() != undefined && $("#autocomplete_agrupamento_processo_pre_proposta").val() !=null) {
        console.log("autocomplete_agrupamento_processo_pre_proposta: " + $("#autocomplete_agrupamento_processo_pre_proposta").val().substr(1));
        console.log("tipoStatusFiltro: " + $("#autocomplete_agrupamento_processo_pre_proposta").val().substr(0, 1));
    }
    return {
        idEmpresaVenda: $("#autocomplete_loja").val(),
        idCorretor: $("#autocomplete_agente_venda").val(),
        idBreveLancamento: $("#autocomplete_breve_lancamento").val(),
        nomeCliente: $("#filtro_cliente").val(),
        elaboracaoDe: $("#ElaboracaoDe").val(),
        elaboracaoAte: $("#ElaboracaoAte").val(),
        DataEnvioDe: $("#DataEnvioDe").val(),
        DataEnvioAte: $("#DataEnvioAte").val(),
        CodigoProposta: $("#filtro_codigo_proposta").val(),
        situacoes: $("#situacao_pre_proposta").val(),
        tipoStatusFiltro: (($("#autocomplete_agrupamento_processo_pre_proposta").val() != undefined && $("#autocomplete_agrupamento_processo_pre_proposta").val() != null)?$("#autocomplete_agrupamento_processo_pre_proposta").val().substr(0, 1):""),
        IdAgrupamentoProcessoPreProposta: (($("#autocomplete_agrupamento_processo_pre_proposta").val() != undefined && $("#autocomplete_agrupamento_processo_pre_proposta").val() != null) ? $("#autocomplete_agrupamento_processo_pre_proposta").val().substr(1) : "")
    };
};

Europa.Controllers.ConsultaPreProposta.Filtrar = function () {
    Europa.Controllers.ConsultaPreProposta.Table.reloadData();
};

Europa.Controllers.ConsultaPreProposta.LimparFiltro = function () {
    $('#filtro_cliente').val("");
    $("#autocomplete_agente_venda").val("").trigger("change");
    $("#autocomplete_agrupamento_processo_pre_proposta").val("").trigger("change");
    $("#autocomplete_loja").val("").trigger("change");
    $("#autocomplete_breve_lancamento").val("").trigger("change");
    $("#ElaboracaoDe").val("");
    $("#ElaboracaoAte").val("");
    $("#DataEnvioDe").val("");
    $("#DataEnvioAte").val("");
    $("#filtro_codigo_proposta").val("");
    $("#situacao_pre_proposta").val("").trigger("change");
};


Europa.Controllers.ConsultaPreProposta.ExportarPagina = function () {
    Europa.Components.DownloadFile(Europa.Controllers.ConsultaPreProposta.UrlExportarPagina,
        Europa.Controllers.ConsultaPreProposta.Table.lastRequestParams);
};

Europa.Controllers.ConsultaPreProposta.ExportarTodos = function () {
    Europa.Components.DownloadFile(Europa.Controllers.ConsultaPreProposta.UrlExportarTodos,
        Europa.Controllers.ConsultaPreProposta.Table.lastRequestParams);
};