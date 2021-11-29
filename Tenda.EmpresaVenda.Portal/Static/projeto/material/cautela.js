Europa.Controllers.Material.Cautela = {};
Europa.Controllers.Material.Cautela.DataTable = {};


Europa.Controllers.Material.Cautela.Tabela = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Controllers.Material.Cautela.DataTable = dataTableWrapper;

    dataTableWrapper
        .setTemplateEdit([
            '<input id="Nome" name="Nome" class="form-control" ></input > ',
            '<input id="Marca" name="Marca" class="form-control"></input>',
            '<input id="Total" name="Total" class="form-control" ></input>'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '30%'),
            DTColumnBuilder.newColumn('Marca').withTitle(Europa.i18n.Messages.Marca).withOption('width', '30%'),
            DTColumnBuilder.newColumn('Total').withTitle(Europa.i18n.Messages.Total).withOption('width', '30%')
        ])
        .setActionSave(Europa.Controllers.Material.Cautela.Salvar)
        .setColActions(actionsHtml, '10%')
        .setDefaultOptions('POST', Europa.Controllers.Material.Cautela.UrlListar, Europa.Controllers.Material.Cautela.Params);
    function actionsHtml(data, type, full, meta) {
        var html = '<div>';
        html = html + $scope.renderButton("Editar", "fa fa-edit", "Editar(" + meta.row + ")");
        if (full.Total > 0) {
            html = html + $scope.renderButton("Pedido", "fas fa-shopping-basket", "Pedido(" + meta.row + ")");
        }
        html = html+ '</div>';
        return html
    }

    $scope.renderButton = function (title, icon, onClick) {
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn-actions')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');

    };
    $scope.Editar = function (row) {
        $scope.rowEdit(row);
        var objetoLinhaTabela = Europa.Controllers.Material.Cautela.DataTable.getRowData(row);
        Europa.Controllers.Material.Cautela.modoInclusao = false;
        console.log(objetoLinhaTabela);
    };
    $scope.Pedido = function (row) {
        var obj = Europa.Controllers.Material.Cautela.DataTable.getRowData(row);
        $("#Material").val(obj.Nome);
        $("#IdMaterial").val(obj.Id);
        Europa.Controllers.Material.AbrirModalPedido();
        Europa.Controllers.Material.modoCautela = true;
    }
};

DataTableApp.controller('CautelaDataTable', Europa.Controllers.Material.Cautela.Tabela);

Europa.Controllers.Material.Cautela.Params = function () {
    var param = Europa.Form.SerializeJson(Europa.Controllers.Material.FormFiltro);
    return param;
};

Europa.Controllers.Material.Cautela.Novo = function () {
    Europa.Controllers.Material.Cautela.DataTable.createRowNewData();
    Europa.Controllers.Material.Cautela.modoInclusao = true;
};
Europa.Controllers.Material.Cautela.Salvar = function () {
    var obj = Europa.Controllers.Material.Cautela.DataTable.getDataRowEdit();
    var url = Europa.Controllers.Material.Cautela.modoInclusao ? Europa.Controllers.Material.Cautela.UrlIncluir : Europa.Controllers.Material.Cautela.UrlAlterar
    $.post(url, obj, function (resposta) {
        if (resposta.Success) {
            Europa.Controllers.Material.Filtrar();
            Europa.Controllers.Material.Cautela.DataTable.closeEdition();
        } else {

        }
        Europa.Informacao.PosAcaoBaseResponse(resposta);
    });
};