Europa.Controllers.Viatura = {};
Europa.Controllers.Viatura.IdModal = "#modal-viatura";
Europa.Controllers.Viatura.Form = "#form-viatura";
Europa.Controllers.Viatura.FormFiltro = "#form-filtro";
Europa.Controllers.Viatura.DataTable = {};


$(function () {
    Europa.Controllers.Viatura.InitMask();
    //Europa.Controllers.Viatura.InitAutoComplete();
});

Europa.Controllers.Viatura.AbrirModalIncluir = function () {
    $(Europa.Controllers.Viatura.IdModal).modal('show');
};

Europa.Controllers.Viatura.FecharModalIncluir = function () {
    $(Europa.Controllers.Viatura.IdModal).modal('hide');
};

Europa.Controllers.Viatura.InitMask = function () {
    Europa.Mask.Int("#Quilometragem");
    Europa.Mask.Int("#Renavam");
};



Europa.Controllers.Viatura.Tabela = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Controllers.Viatura.DataTable = dataTableWrapper;

    dataTableWrapper
        .setColumns([
            DTColumnBuilder.newColumn('Placa').withTitle(Europa.i18n.Messages.Placa).withOption('width', '15%'),
            DTColumnBuilder.newColumn('Modelo').withTitle(Europa.i18n.Messages.Modelo).withOption('width', '20%'),
            DTColumnBuilder.newColumn('Renavam').withTitle(Europa.i18n.Messages.Renavam).withOption('width', '20%'),
            DTColumnBuilder.newColumn('TipoCombustivel').withTitle(Europa.i18n.Messages.TipoCombustivel).withOption('type', 'enum-format-TipoCombustivel').withOption('width', '15%'),
            DTColumnBuilder.newColumn('Quilometragem').withTitle('Quilometragem').withOption('width', '15%'),
            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('type', 'enum-format-SituacaoViatura').withOption('width', '20%'),


        ])
        .setColActions(actionsHtml, '10%')
        .setDefaultOptions('POST', Europa.Controllers.Viatura.UrlListar, Europa.Controllers.Viatura.Params);


    function actionsHtml(data, type, full, meta) {
        var html = '<div>' + $scope.renderButton("Visualizar", "fas fa-eye", "Visualizar(" + data.Id + ")");
        if (full.Situacao === 1) {
            html = html + $scope.renderButton("Alocar", "fas fa-exchange-alt", "Alocar(" + meta.row + ")");
        }
        if (full.Situacao === 2) {
            html = html + $scope.renderButton("Desalocar", "fas fa-exchange-alt", "Desalocar(" + meta.row + ")");
        }
        html = html +'</div>';

        return html;
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

    $scope.Visualizar = function (id) {
        window.location = Europa.Controllers.Viatura.UrlVisualizar + '/' + id;
    };

    $scope.Alocar = function (row) {
        var obj = Europa.Controllers.Viatura.DataTable.getRowData(row);
        var viatura = obj.Modelo + ' - ' + obj.Placa;
        $("#Viatura").val(viatura);
        $("#IdViatura").val(obj.Id);
        Europa.Controllers.Alocar.AbrirModal();
    }
    $scope.Desalocar = function (row) {
        var obj = Europa.Controllers.Viatura.DataTable.getRowData(row);
        var viatura = obj.Modelo + ' - ' + obj.Placa;
        $("#ViaturaDesalocar").val(viatura);
        $("#IdViaturaDesalocar").val(obj.Id);
        Europa.Controllers.Desalocar.AbrirModal();
    }
};

DataTableApp.controller('ViaturaDataTable', Europa.Controllers.Viatura.Tabela);

Europa.Controllers.Viatura.Params = function () {
    var param = Europa.Form.SerializeJson(Europa.Controllers.Viatura.FormFiltro);
    return param;
}

Europa.Controllers.Viatura.Salvar = function () {
    var param = Europa.Form.SerializeJson(Europa.Controllers.Viatura.Form);
    $.post(Europa.Controllers.Viatura.UrlSalvar, param, function (res) {
        if (res.Success) {
            Europa.Controllers.Viatura.Filtrar();
            Europa.Controllers.Viatura.FecharModalIncluir();
        } else {
        }
        Europa.Informacao.PosAcaoBaseResponse(res);
    });
}

Europa.Controllers.Viatura.Filtrar = function () {
    Europa.Controllers.Viatura.DataTable.reloadData();
};