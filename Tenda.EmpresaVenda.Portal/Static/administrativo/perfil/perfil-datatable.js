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
            '<input type="hidden" name="ResponsavelCriacao.Id" value="" readonly="true">'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '40%'),
            DTColumnBuilder.newColumn('ResponsavelCriacao.Nome').withTitle(Europa.i18n.Messages.Responsavel).withOption('width', '30%'),
            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).renderWith(Europa.Controllers.CadastroPerfil.RenderizaSituacao).withOption('width', '20%'),
            DTColumnBuilder.newColumn('ResponsavelCriacao.Id').withClass('hidden', 'hidden')
        ])
        .setActionSave(Europa.Controllers.CadastroPerfil.PreSalvar)
        .setOptionsSelect('POST', Europa.Controllers.CadastroPerfil.UrlListar, Europa.Controllers.CadastroPerfil.filterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButton('true', "Editar", "fa fa-edit", "rowEdit(" + meta.row + ")") +
            '</div>';
    }

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (hasPermission !== 'true') {
            return "";
        }

        icon = $('<i/>').addClass(icon);

        var button = $('<a />')
            .addClass('btn btn-steel')
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