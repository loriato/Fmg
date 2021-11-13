Europa.Controllers.DocumentacaoPreProposta.CCAsPPR = {};
Europa.Controllers.DocumentacaoPreProposta.CCAsPPR.Tabela = {};
Europa.Controllers.DocumentacaoPreProposta.ListarCCAsPPR = {};


function CriarTabelaCCAsPPR($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.DocumentacaoPreProposta.CCAsPPR.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.DocumentacaoPreProposta.CCAsPPR.Tabela;
    self.setColumns([
        DTColumnBuilder.newColumn('NomeGrupoCCA').withTitle("Grupos CCA da Pré Proposta").withOption('width', '50px')
    ])
        .setAutoInit(false)
        .setIdAreaHeader("datatable_ccasppr_header")
        .setDefaultOptions('POST', Europa.Controllers.DocumentacaoPreProposta.UrlListarCCAsPPR, Europa.Controllers.DocumentacaoPreProposta.ListarCCAsPPR.Filtro);

}

DataTableApp.controller('CCAsPPR', CriarTabelaCCAsPPR);

Europa.Controllers.DocumentacaoPreProposta.ListarCCAsPPR.Filtro = function () {
    return {
        IdPreProposta: $('#IdPreProposta').val()
    };
};
