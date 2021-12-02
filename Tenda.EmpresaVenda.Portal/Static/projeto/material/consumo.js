Europa.Controllers.Material.Consumo = {};
Europa.Controllers.Material.Consumo.DataTable = {};

Europa.Controllers.Material.Consumo.Tabela = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Controllers.Material.Consumo.DataTable = dataTableWrapper;

    dataTableWrapper
        .setTemplateEdit([
            '<input id="Nome" name="Nome" class="form-control"maxlength="128" ></input > ',
            '<input id="Lote" name="Lote" class="form-control" maxlength="20"></input>',
            '<input id="Total" name="Total" class="form-control" type="number"></input>',
            '<input id="Validade" name="Validade" class="form-control" datepicker="datepicker"></input>'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '25%'),
            DTColumnBuilder.newColumn('Lote').withTitle('Lote').withOption('width', '15%'),
            DTColumnBuilder.newColumn('Total').withTitle(Europa.i18n.Messages.Total).withOption('width', '15%'),
            DTColumnBuilder.newColumn('Validade').withTitle('Validade').withOption("type", "date-format-DD/MM/YYYY").withOption('width', '25%')
        ])
        .setActionSave(Europa.Controllers.Material.Consumo.Salvar)
        .setColActions(actionsHtml, '10%')
        .setDefaultOptions('POST', Europa.Controllers.Material.Consumo.UrlListar, Europa.Controllers.Material.Consumo.Params);
    function actionsHtml(data, type, full, meta) {
        var html = '<div>';
        html = html + $scope.renderButton("Editar", "fa fa-edit", "Editar(" + meta.row + ")");
        if (full.Total > 0) {
            html = html + $scope.renderButton("Pedido", "fas fa-shopping-basket", "Pedido(" + meta.row + ")");
        }
        html = html + '</div>';
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
        var objetoLinhaTabela = Europa.Controllers.Material.Consumo.DataTable.getRowData(row);
        var date = Europa.String.FormatAsGeenDate($("#Validade").val());
        
        Europa.Controllers.Material.Consumo.DataVencimento = new Europa.Components.DatePicker()
            .WithTarget("#Validade")
            .WithFormat("DD/MM/YYYY")
            .WithValue(date)
            .Configure();
        Europa.Controllers.Material.Consumo.modoInclusao = false;
    };
    $scope.Pedido = function (row) {
        var obj = Europa.Controllers.Material.Consumo.DataTable.getRowData(row);
        $("#Material").val(obj.Nome);
        $("#IdMaterial").val(obj.Id);
        Europa.Controllers.Material.AbrirModalPedido();
        Europa.Controllers.Material.modoCautela = false;
    }
};

DataTableApp.controller('ConsumoDataTable', Europa.Controllers.Material.Consumo.Tabela);

Europa.Controllers.Material.Consumo.Params = function () {
    var param = Europa.Form.SerializeJson(Europa.Controllers.Material.FormFiltro);
    return param;
};

Europa.Controllers.Material.Consumo.Novo = function () {
    Europa.Controllers.Material.Consumo.DataTable.createRowNewData();
    Europa.Components.DatePicker.AutoApply();
    Europa.Controllers.Material.Consumo.modoInclusao = true;
};

Europa.Controllers.Material.Consumo.Salvar = function () {
    var obj = Europa.Controllers.Material.Consumo.DataTable.getDataRowEdit();
    var url = Europa.Controllers.Material.Consumo.modoInclusao ? Europa.Controllers.Material.Consumo.UrlIncluir : Europa.Controllers.Material.Consumo.UrlAlterar

    $.post(url, obj, function (resposta) {
        if (resposta.Success) {
            Europa.Controllers.Material.Filtrar();
            Europa.Controllers.Material.Consumo.DataTable.closeEdition();
        } else {
           
        }
        Europa.Informacao.PosAcaoBaseResponse(resposta);
    });
};