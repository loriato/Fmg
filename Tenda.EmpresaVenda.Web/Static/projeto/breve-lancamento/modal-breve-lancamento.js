Europa.Controllers.BreveLancamento.TabelaPrioridade = {};

function PrioridadeTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.BreveLancamento.TabelaPrioridade = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.BreveLancamento.TabelaPrioridade;
    tabela
        .setTemplateEdit([
            '<input type="text" class="form-control" id="Nome" name="Nome" readonly="true">',
            '<input type="number" class="form-control" id="Sequencia" name="Sequencia" min="1">'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption("width", "100px"),
            DTColumnBuilder.newColumn('Sequencia').withTitle(Europa.i18n.Messages.Sequencia).withOption("width","100px")

        ])
        .setActionSave(Europa.Controllers.BreveLancamento.SalvarPrioridade)
        .setColActions(actionsHtml, '60px')
        .setIdAreaHeader("modal-prioridade-titulo")
        .setAutoInit(false)
        .setOptionsSelect('POST', Europa.Controllers.BreveLancamento.UrlListarPrioridade, Europa.Controllers.BreveLancamento.FilterParamsPrioridade);

    function actionsHtml(data, type, full, meta) {
        var button = '<div>';
        button += Europa.Controllers.BreveLancamento.TabelaPrioridade.renderButton(Europa.Controllers.BreveLancamento.Permissoes.Atualizar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")");
        button += '</div >';
        return button;
    };

    $scope.Editar = function (row) {
        $scope.rowEdit(row);
        var obj = Europa.Controllers.BreveLancamento.TabelaPrioridade.getRowData(row);
        
    };
};

DataTableApp.controller('PrioridadeDataTable', PrioridadeTable);

Europa.Controllers.BreveLancamento.FiltrarPrioridade = function () {
    Europa.Controllers.BreveLancamento.TabelaPrioridade.reloadData();

};

Europa.Controllers.BreveLancamento.FilterParamsPrioridade = function () {
    var filtro = {
        nome: $('#filtro_nome_modal').val()
    };
    return filtro;
};

Europa.Controllers.BreveLancamento.LimparFiltroPrioridade = function () {
    $('#filtro_nome_modal').val("");
};

Europa.Controllers.BreveLancamento.AbrirModal = function () {
    Europa.Controllers.BreveLancamento.LimparFiltroPrioridade();
    $("#modal-prioridade").modal("show");
    Europa.Controllers.BreveLancamento.TabelaPrioridade.reloadData();
};

Europa.Controllers.BreveLancamento.SalvarPrioridade = function () {
    var obj = Europa.Controllers.BreveLancamento.TabelaPrioridade.getDataRowEdit();
    var url = Europa.Controllers.BreveLancamento.UrlSalvarPrioridade;
    $.post(url, { breveLancamento: obj}, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.BreveLancamento.TabelaPrioridade.closeEdition();
            Europa.Controllers.BreveLancamento.TabelaPrioridade.reloadData();
        } else {
            Europa.Controllers.BreveLancamento.AddError(res.Campos);
        }
        Europa.Informacao.PosAcao(res);
    });
}

Europa.Controllers.BreveLancamento.AddError = function (fields) {
    fields.forEach(function (key) {
        $("[name='" + key + "']").parent().addClass("has-error");
    });
}