Europa.Controllers.FilaEmail = {};
Europa.Controllers.FilaEmail.Tabela = {};

$(function () {
    $("#Situacoes").select2({
        trags: true,
        width: '100%'
    });

});

DataTableApp.controller('filaEmailTable', filaEmailTable);

function filaEmailTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

    Europa.Controllers.FilaEmail.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.FilaEmail.Tabela;
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('Destinatario').withTitle(Europa.i18n.Messages.Destinatario).withOption('width', '20%'),
        DTColumnBuilder.newColumn('Titulo').withTitle(Europa.i18n.Messages.Titulo).withOption('width', '30%'),
        DTColumnBuilder.newColumn('SituacaoEnvio').withTitle(Europa.i18n.Messages.SituacaoEnvio).withOption('width', '15%').withOption('type', 'enum-format-SituacaoEnvioFila'),
        DTColumnBuilder.newColumn('NumeroTentativas').withTitle(Europa.i18n.Messages.NumeroTentativas).withOption('width', '15%'),
        DTColumnBuilder.newColumn('DataEnvio').withTitle(Europa.i18n.Messages.DataEnvio).withOption('width', '10%').renderWith(Europa.Date.toGeenDateFormat),
    ])
        .setColActions(actionsHtml, '5%')
        .setDefaultOptions('POST', Europa.Controllers.FilaEmail.UrlListarFilaEmail, Europa.Controllers.FilaEmail.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return "";        
    }

}

Europa.Controllers.FilaEmail.FilterParams = function () {
    return {
        Destinatario: $("#Destinatario").val(),
        Situacoes: $("#Situacoes").val(),
        PeriodoDe: $("#PeriodoDe").val(),
        PeriodoAte: $("#PeriodoAte").val()
    };
};

Europa.Controllers.FilaEmail.FiltrarTabela = function () {
    Europa.Controllers.FilaEmail.Tabela.reloadData();
};

Europa.Controllers.FilaEmail.LimparFiltro = function () {
    $("#Destinatario").val("");
    $("#Situacoes").val("").trigger("change");
    $("#PeriodoDe").val("");
    $("#PeriodoAte").val("");
}

Europa.Controllers.FilaEmail.OnChangePeriodo = function () {
    Europa.Controllers.FilaEmail.PeriodoDe = new Europa.Components.DatePicker()
        .WithTarget("#PeriodoAte")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#PeriodoDe").val())
        .Configure();
}

