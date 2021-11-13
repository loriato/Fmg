$(function () {
    
});

Europa.Controllers.CadastroPerfil.createDT = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.CadastroPerfil.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Controllers.CadastroPerfil.Tabela
        .setIdAreaHeader("perfil_datatable_header")
        .setColActions(actionsHtml, "10%")
        .setTemplateEdit([
            '<input type="text" class="form-control" name="Nome" value="">',
            '<input type="text" class="form-control" name="ResponsavelCriacao.Nome" value="" readonly="true">',
            '<select class="form-control" name="Situacao" readonly="true" disabled>' +
            '<option value="1">' + Europa.i18n.Messages.Situacao_Ativo + '</option>' +
            '<option value="2">' + Europa.i18n.Messages.Situacao_Suspenso + '</option>' +
            '<option value="3">' + Europa.i18n.Messages.Situacao_Cancelado + '</option>' +
            '</select>',
            '<select class="form-control" id="ExibePortal" name="ExibePortal">' +
            '<option value="true">' + Europa.i18n.Messages.Sim + '</option>' +
            '<option value="false">' + Europa.i18n.Messages.Nao + '</option>' +
            '</select>',
            '<input type="hidden" name="ResponsavelCriacao.Id" value="" readonly="true">'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '30%'),
            DTColumnBuilder.newColumn('ResponsavelCriacao.Nome').withTitle(Europa.i18n.Messages.Responsavel).withOption('width', '30%'),
            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).renderWith(Europa.Controllers.CadastroPerfil.RenderizaSituacao).withOption('width', '20%'),
            DTColumnBuilder.newColumn('ExibePortal').withTitle(Europa.i18n.Messages.ExibePortal).withOption('width', '10%').renderWith(renderBoolean),
            DTColumnBuilder.newColumn('ResponsavelCriacao.Id').withClass('hidden', 'hidden')            
        ])
        .setActionSave(Europa.Controllers.CadastroPerfil.PreSalvar)
        .setOptionsSelect('POST', Europa.Controllers.CadastroPerfil.UrlListar, Europa.Controllers.CadastroPerfil.filterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButton('true', "Editar", "fa fa-edit", "Editar(" + meta.row + ")") +
            '</div>';
    }

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (hasPermission !== 'true') {
            return "";
        }

        icon = $('<i/>').addClass(icon);

        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);

        return button.prop('outerHTML');
    }

    $scope.onDoubleClickOnRow = function (row) {
        if (row != undefined) {
            $scope.rowEdit(row);
        }
    }

    function renderBoolean(data) {
        if (data === true) {
            return Europa.i18n.Messages.Sim;
        }

        return Europa.i18n.Messages.Nao;
    }

    $scope.Editar = function (row) {
        $scope.rowEdit(row);
        var obj = Europa.Controllers.CadastroPerfil.Tabela.getRowData(row);
        Europa.Controllers.CadastroPerfil.modoInclusao = false;
        $("#ExibePortal").val(obj.ExibePortal.toString());
    }
};

Europa.Controllers.CadastroPerfil.GetSelectedObjectsIds = function () {
    return Europa.Components.Datatable.CadastroPerfil.DataTable.getRowsSelect().Id;
};

DataTableApp.controller('TabelaPerfil', Europa.Controllers.CadastroPerfil.createDT);


Europa.Controllers.CadastroPerfil.Filtrar = function () {
    Europa.Controllers.CadastroPerfil.Tabela.reloadData();
};

Europa.Controllers.CadastroPerfil.filterParams = function () {
    var params = {
        nome: $("#FiltroNomePerfil").val(),
        buscaSuspensos: true
    };
    return params;
};