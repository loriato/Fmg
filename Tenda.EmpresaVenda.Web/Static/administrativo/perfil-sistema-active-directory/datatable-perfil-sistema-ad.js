Europa.Controllers.PerfilSistemaGrupoAD.createDT = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.PerfilSistemaGrupoAD.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Controllers.PerfilSistemaGrupoAD.Tabela
        .setIdAreaHeader("perfilSistemaGrupoAD_datatable_header")
        .setTemplateEdit([
            '<select id="autocomplete_perfil" name="NomePerfil" class="select2-container form-control"></select>',
            '<input type="text" id="GrupoActiveDirectory" class="form-control" name="GrupoActiveDirectory" style="text-transform: uppercase;">'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('NomePerfil').withTitle(Europa.i18n.Messages.Perfil).withOption('width', '46%'),
            DTColumnBuilder.newColumn('GrupoActiveDirectory').withTitle(Europa.i18n.Messages.GrupoAd).withOption('width', '46%')
        ])
        .setColActions(actionsHtml, '40px')
        .setActionSave(Europa.Controllers.PerfilSistemaGrupoAD.PreSalvar)
        .setDefaultOptions('POST', Europa.Controllers.PerfilSistemaGrupoAD.UrlListar);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButton(Europa.Controllers.PerfilSistemaGrupoAD.Permissoes.Alterar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")") +
            $scope.renderButton(Europa.Controllers.PerfilSistemaGrupoAD.Permissoes.Excluir, "Excluir", "fa fa-trash", "Excluir(" + meta.row + ")") +
            '</div>';
    }

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (hasPermission != "true") {
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

    $scope.Editar = function (row) {
        Europa.Controllers.PerfilSistemaGrupoAD.modoInclusao = false;
        $scope.rowEdit(row);
        var objetoLinhaTabela = Europa.Controllers.PerfilSistemaGrupoAD.Tabela.getRowData(row);
        Europa.Controllers.PerfilSistemaGrupoAD.InicializarAutoCompletes();
        Europa.Controllers.PerfilSistemaGrupoAD.AutoCompletePerfil.SetValue(objetoLinhaTabela.IdPerfil, objetoLinhaTabela.NomePerfil);
    }

    $scope.Excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.PerfilSistemaGrupoAD.Tabela.getRowData(row);
        Europa.Controllers.PerfilSistemaGrupoAD.PreDeletar(objetoLinhaTabela);
    }
};

DataTableApp.controller('TabelaPerfilSistemaGrupoAD', Europa.Controllers.PerfilSistemaGrupoAD.createDT);