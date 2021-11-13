Europa.Components.LogIntegracaoModal = {};
Europa.Components.LogIntegracaoModal.Tabela = {};

$(document).ready(function () {
});

Europa.Components.LogIntegracaoModal.AbrirModal = function () {
    $("#log-integracao-modal").modal("show");
    Europa.Components.LogIntegracaoModal.Tabela.reloadData();
}

Europa.Components.LogIntegracaoModal.FecharModal = function () {
    $("#log-integracao-modal").modal("hide");
}


DataTableApp.controller('logIntegracao', logIntegracaoTable);

function logIntegracaoTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Components.LogIntegracaoModal.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Components.LogIntegracaoModal.Tabela;
    tabelaWrapper
        .setColumns([
            DTColumnBuilder.newColumn('CriadoEm').withTitle(Europa.i18n.Messages.CriadoEm).withOption('width', '15%').withOption("type", "date-format-DD/MM/YYYY HH:mm:ss"),
            DTColumnBuilder.newColumn('Codigo').withTitle(Europa.i18n.Messages.Proposta).withOption('width', '15%'),
            DTColumnBuilder.newColumn('PreProposta').withTitle(Europa.i18n.Messages.PreProposta).withOption('width', '15%'),
            DTColumnBuilder.newColumn('Mensagem').withTitle(Europa.i18n.Messages.Mensagem).withOption('width', '55%')
        ])
        .setDefaultOrder([[0, "desc"]])
        .setAutoInit(false)
        .setOptionsSelect('POST', Europa.Components.LogIntegracaoModal.UrlListar, Europa.Components.LogIntegracaoModal.FilterParams);
}

Europa.Components.LogIntegracaoModal.FilterParams = function () {
    var params = {
        codigoPreProposta: $("#PreProposta_Codigo").val()
    };
    return params;
}