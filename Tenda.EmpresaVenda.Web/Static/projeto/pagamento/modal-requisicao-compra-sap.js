Europa.Controllers.RC = {};
Europa.Controllers.RC.ModalRCSap = "#requisicao_compra_sap";

function TabelaRequisicaoCompra($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.RC.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.RC.Tabela;
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
        .setDefaultOptions('POST', Europa.Controllers.RC.UrlListar, Europa.Controllers.RC.FilterParams);
};

DataTableApp.controller('RequisicaoCompra', TabelaRequisicaoCompra);

Europa.Controllers.RC.FilterParams = function () {
    var obj = Europa.Controllers.Pagamento.GetDataRow;
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

Europa.Controllers.RC.Filtro = function () {
    Europa.Controllers.RC.Tabela.reloadData();
};

Europa.Controllers.RC.AbrirModalVisualizarRequisicao = function () {
    $(Europa.Controllers.RC.ModalRCSap).modal("show");
};