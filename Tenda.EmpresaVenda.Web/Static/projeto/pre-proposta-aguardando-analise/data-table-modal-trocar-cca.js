Europa.Controllers.PrePropostaAguardandoAnalise.CCAsPPR = {};
Europa.Controllers.PrePropostaAguardandoAnalise.CCAsPPR.Tabela = {};
Europa.Controllers.PrePropostaAguardandoAnalise.ListarCCAsPPR = {};


function CriarTabelaCCAsPPR($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.PrePropostaAguardandoAnalise.CCAsPPR.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.PrePropostaAguardandoAnalise.CCAsPPR.Tabela;
    self.setColumns([
        DTColumnBuilder.newColumn('NomeGrupoCCA').withTitle("Grupos CCA da Pré Proposta").withOption('width', '50px')
    ])
        .setAutoInit(false)
        .setIdAreaHeader("datatable_ccasppr_header")
        .setDefaultOptions('POST', Europa.Controllers.PrePropostaAguardandoAnalise.UrlListarCCAsPPR, Europa.Controllers.PrePropostaAguardandoAnalise.ListarCCAsPPR.Filtro);

}

DataTableApp.controller('CCAsPPR', CriarTabelaCCAsPPR);

Europa.Controllers.PrePropostaAguardandoAnalise.ListarCCAsPPR.Filtro = function () {
    return {
        IdPreProposta: $('#IdPreProposta').val()
    };
};
