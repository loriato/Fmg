Europa.Controllers.ValorNominal = {};

$(function () {
    Europa.Controllers.ValorNominal.InitAutoComplete();
    $("#filtro_estados").select2({
        trags: true
    });

    Europa.Components.DatePicker.AutoApply();
});

Europa.Controllers.ValorNominal.InitAutoComplete = function () {
    Europa.Controllers.ValorNominal.AutoCompleteEmpreendimento = new Europa.Components.AutoCompleteEmpreendimento()
    .WithTargetSuffix("empreendimento")
    .Configure();
    
}
DataTableApp.controller('ValorNominalEmpreendimento', TabelaValorNominalEmpreendimento);

function TabelaValorNominalEmpreendimento($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.ValorNominal.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.ValorNominal.Tabela;
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('Id').withTitle(Europa.i18n.Messages.Id).withOption('visible', false),
        DTColumnBuilder.newColumn('Divisao').withTitle(Europa.i18n.Messages.Divisao).withOption('width', '100px'),
        DTColumnBuilder.newColumn('NomeEmpreendimento').withTitle(Europa.i18n.Messages.Nome).withOption('width', '100px'),
        DTColumnBuilder.newColumn('Estado').withTitle(Europa.i18n.Messages.Estado).withOption('width', '100px'),
        DTColumnBuilder.newColumn('FaixaUmMeioDe').withTitle(Europa.i18n.Messages.FaixaUmMeioDe).withOption('width', '100px').renderWith(Europa.String.FormatMoney),
        DTColumnBuilder.newColumn('FaixaUmMeioAte').withTitle(Europa.i18n.Messages.FaixaUmMeioAte).withOption('width', '100px').renderWith(Europa.String.FormatMoney),
        DTColumnBuilder.newColumn('FaixaDoisDe').withTitle(Europa.i18n.Messages.FaixaDoisDe).withOption('width', '100px').renderWith(Europa.String.FormatMoney),
        DTColumnBuilder.newColumn('FaixaDoisAte').withTitle(Europa.i18n.Messages.FaixaDoisAte).withOption('width', '100px').renderWith(Europa.String.FormatMoney),
        DTColumnBuilder.newColumn('PNEDe').withTitle(Europa.i18n.Messages.PNEDe).withOption('width', '100px').renderWith(Europa.String.FormatMoney),
        DTColumnBuilder.newColumn('PNEAte').withTitle(Europa.i18n.Messages.PNEAte).withOption('width', '100px').renderWith(Europa.String.FormatMoney),
        DTColumnBuilder.newColumn('InicioVigencia').withTitle(Europa.i18n.Messages.InicioVigencia).withOption('width', '100px').renderWith(Europa.String.FormatAsGeenDateTime),
        DTColumnBuilder.newColumn('TerminoVigencia').withTitle(Europa.i18n.Messages.TerminoVigencia).withOption('width', '100px').renderWith(Europa.String.FormatAsGeenDateTime),
        DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('width', '100px').withOption('type', 'enum-format-SituacaoValorNominal'),

    ])
        .setIdAreaHeader("datatable_header")
        .setDefaultOrder([[0, 'desc']])
        //.setAutoInit()
        .setDefaultOptions('POST', Europa.Controllers.ValorNominal.UrlListar, Europa.Controllers.ValorNominal.FilterParams);  

}

Europa.Controllers.ValorNominal.FilterParams = function () {
    var filtro = {
        estados: $("#filtro_estados").val(),
        vigenteEm: $("#filtro_vigente_em").val(),
        idEmpreendimento: $("#autocomplete_empreendimento").val(),
        situacao: $("#filtro_situacao").val()
    };
    return filtro;
};

Europa.Controllers.ValorNominal.Filtrar = function () {
    Europa.Controllers.ValorNominal.Tabela.reloadData();
};

Europa.Controllers.ValorNominal.LimparFiltro = function () {
    $("#filtro_estados").val("").trigger('change');
    $("#filtro_vigente_em").val("");
    $("#autocomplete_empreendimento").val("").trigger("change");
    $("#filtro_situacao").val("");
}

Europa.Controllers.ValorNominal.ExportarPagina = function () {
    var params = Europa.Controllers.ValorNominal.FilterParams();
    params.order = Europa.Controllers.ValorNominal.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.ValorNominal.Tabela.lastRequestParams.draw;
    params.pageSize = Europa.Controllers.ValorNominal.Tabela.lastRequestParams.pageSize;
    params.start = Europa.Controllers.ValorNominal.Tabela.lastRequestParams.start;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.ValorNominal.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.ValorNominal.ExportarTodos = function () {
    var params = Europa.Controllers.ValorNominal.FilterParams();
    params.order = Europa.Controllers.ValorNominal.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.ValorNominal.Tabela.lastRequestParams.draw;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.ValorNominal.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};