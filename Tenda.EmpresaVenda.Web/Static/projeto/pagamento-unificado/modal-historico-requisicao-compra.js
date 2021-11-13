Europa.Controllers.PagamentoUnificado.ModalHistoricoRequisicaoCompra = "#historico_requisicao_compra_sap";

$(function () {

});

function TabelaHistoricoRequisicaoCompra($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.PagamentoUnificado.TabelaHistoricoRequisicaoCompra = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.PagamentoUnificado.TabelaHistoricoRequisicaoCompra;
    tabelaWrapper
        .setColumns([
            DTColumnBuilder.newColumn('Id').withTitle("").withClass("hidden", "hidden"),
            DTColumnBuilder.newColumn('Proposta').withTitle(Europa.i18n.Messages.Proposta).withOption('width', '150px'),
            DTColumnBuilder.newColumn('EmpresaVenda').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption('width', '150px'),
            DTColumnBuilder.newColumn('TipoPagamento').withTitle(Europa.i18n.Messages.TipoPagamento).withOption('type', 'enum-format-TipoPagamento').withOption('width', '150px'),
            DTColumnBuilder.newColumn('Numero').withTitle(Europa.i18n.Messages.Numero).withOption('width', '150px'),
            DTColumnBuilder.newColumn('Texto').withTitle(Europa.i18n.Messages.Texto).withOption('width', '150px'),
            DTColumnBuilder.newColumn('Status').withTitle(Europa.i18n.Messages.Status).withOption('width', '150px'),
            DTColumnBuilder.newColumn('NomeUsuario').withTitle(Europa.i18n.Messages.CriadoPor).withOption('width', '200px')

        ])
        .setDefaultOrder([[0, 'desc']])
        .setAutoInit()
        .setDefaultOptions('POST', Europa.Controllers.PagamentoUnificado.UrlListarRequisicaoCompraSap, Europa.Controllers.PagamentoUnificado.FiltroHistoricoRequisicaoCompra);
};

DataTableApp.controller('HistoricoRequisicaoCompra', TabelaHistoricoRequisicaoCompra);

Europa.Controllers.PagamentoUnificado.FiltroHistoricoRequisicaoCompra = function () {
    var obj = Europa.Controllers.PagamentoUnificado.GetDataRow;
    var param = {};
    if (obj == undefined) {
        return param;
    }
    else {
        param = {
            IdEmpresaVenda: obj.IdEmpresaVenda,
            IdProposta: obj.IdProposta,
            TipoPagamento: obj.TipoPagamento

        };
        return param;
    }
};

Europa.Controllers.PagamentoUnificado.FiltrarHistoricoRequisicaoCompra = function () {
    Europa.Controllers.PagamentoUnificado.TabelaHistoricoRequisicaoCompra.reloadData();
};

Europa.Controllers.PagamentoUnificado.AbrirModalHistoricoRequisicaoCompra = function () {
    $(Europa.Controllers.PagamentoUnificado.ModalHistoricoRequisicaoCompra).modal("show");
};