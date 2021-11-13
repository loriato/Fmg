Europa.Controllers.HistoricoRegraComissao.Tabela = undefined;

$(function () {

});

DataTableApp.controller("HistoricoRegraComissao", HistoricoRegraComissao);

function HistoricoRegraComissao($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.HistoricoRegraComissao.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.HistoricoRegraComissao.Tabela;
    tabelaWrapper
        .setColumns([
            DTColumnBuilder.newColumn('Descricao').withTitle(Europa.i18n.Messages.Descricao).withOption('width', '100px'),
            DTColumnBuilder.newColumn('TipoRegraComissao').withTitle(Europa.i18n.Messages.TipoRegraComissao).withOption('width', '100px').withOption('type', 'enum-format-TipoRegraComissao'),
            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('width', '100px').withOption('type', 'enum-format-SituacaoRegraComissao'),
            DTColumnBuilder.newColumn('EmpresaVenda').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption('width', '100px'),
            DTColumnBuilder.newColumn('ResponsavelInicio').withTitle(Europa.i18n.Messages.ResponsavelInicio).withOption('width', '100px'),            
            DTColumnBuilder.newColumn('DataInicio').withTitle(Europa.i18n.Messages.DataInicio).withOption('width', '100px').withOption("type", "date-format-DD/MM/YYYY"),
            DTColumnBuilder.newColumn('ResponsavelTermino').withTitle(Europa.i18n.Messages.ResponsavelTermino).withOption('width', '100px'),
            DTColumnBuilder.newColumn('DataTermino').withTitle(Europa.i18n.Messages.DataTermino).withOption('width', '100px').withOption("type", "date-format-DD/MM/YYYY")
            //DTColumnBuilder.newColumn('Status').withTitle(Europa.i18n.Messages.Status).withOption('width', '100px').withOption('type', 'enum-format-Situacao')
        ])
        .setIdAreaHeader("datatable_header")
        .setDefaultOptions('POST', Europa.Controllers.HistoricoRegraComissao.UrlListarHistorico, Europa.Controllers.HistoricoRegraComissao.Filtro);    
};

Europa.Controllers.HistoricoRegraComissao.Filtro = function () {
    var filtro = {
        Descricao: ""
    };

    return filtro;
};

Europa.Controllers.HistoricoRegraComissao.Filtrar = function () {
    Europa.Controllers.HistoricoRegraComissao.Tabela.reloadData();
};
