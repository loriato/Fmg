$(function () {

});

function TablePreProposta($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.ClienteDuplicado.PrePropostaTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.ClienteDuplicado.PrePropostaTable;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('Codigo').withTitle(Europa.i18n.Messages.PreProposta).withOption("width", "100px")
                .withOption("link", tabela.withOptionLink(Europa.Components.DetailAction.PreProposta, "Id")),
            DTColumnBuilder.newColumn('NomeEmpresaVenda').withTitle(Europa.i18n.Messages.EmpresaVenda),
            DTColumnBuilder.newColumn('NomeCliente').withTitle(Europa.i18n.Messages.Cliente).withOption("width", "175px"),
            DTColumnBuilder.newColumn('NomeViabilizador').withTitle(Europa.i18n.Messages.Viabilizador),
            DTColumnBuilder.newColumn('SituacaoPrePropostaPortalHouse').withTitle(Europa.i18n.Messages.SituacaoPreProposta),
            DTColumnBuilder.newColumn('Elaboracao').withTitle(Europa.i18n.Messages.DataElaboracao).withOption("type", "date-format-DD/MM/YYYY HH:mm:ss"),
        ])
        .setAutoInit(false)
        .setColActions(actionsHtml, '100px')
        .setIdAreaHeader("pre_proposta_datatable_barra")
        .setOptionsSelect('POST', Europa.Controllers.ClienteDuplicado.UrlListarPreProposta, Europa.Controllers.ClienteDuplicado.FiltroPreProposta);

    function actionsHtml(data, type, full, meta) {
        var button = '<div>';
        var situacoes = [1, 2, 7, 8, 11, 12, 14, 15, 18, 19];

        if (situacoes.includes(full.StatusAnalise) && Europa.Controllers.ClienteDuplicado.Permissoes.Cancelar) {
            button += tabela.renderButton(Europa.Controllers.ClienteDuplicado.Permissoes.Cancelar, "Cancelar", "fa fa-ban", "Cancelar(" + meta.row + ")");
        }

        button += '</div >';

        return button;
    }

    $scope.Cancelar = function (row) {
        var data = Europa.Controllers.ClienteDuplicado.PrePropostaTable.getRowData(row);
        Europa.Controllers.ClienteDuplicado.Cancelar(data);
    }

    function renderCodigo(data, type, full, meta) {
        return Europa.Components.DetailAction.PreProposta
    }
};

DataTableApp.controller('PrePropostaDatatable', TablePreProposta);

Europa.Controllers.ClienteDuplicado.FiltroPreProposta = function () {
    var param = {
        NomeCliente: $("#nomeCliente").val(),
        CPF: Europa.Controllers.ClienteDuplicado.ClienteCpf
    };
    return param;
};

Europa.Controllers.ClienteDuplicado.FiltrarPreProposta = function () {
    if (Europa.Controllers.ClienteDuplicado.ClienteId == undefined) {
        var res = {
            Sucesso: true,
            Mensagens: ["Selecione um cliente"]
        };

        Europa.Informacao.PosAcao(res);

        return;
    }

    Europa.Controllers.ClienteDuplicado.PrePropostaTable.reloadData();
};

Europa.Controllers.ClienteDuplicado.LimparFiltroPreProposta = function () {
    $("#preProposta").val("");
};

Europa.Controllers.ClienteDuplicado.Cancelar = function (data) {
    Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.CancelarPreProposta, Europa.i18n.Messages.MsgCancelarPreProposta, Europa.i18n.Messages.Confirmar,
        function () {
            var response = Europa.Controllers.PrePropostaWorkflow.Cancelar(data.Id);

            if (response.Sucesso == true) {
                Europa.Controllers.ClienteDuplicado.PrePropostaTable.reloadData(undefined,false);
            }
            Europa.Informacao.PosAcao(response);
        }
    );
    Europa.Confirmacao.Show();
}