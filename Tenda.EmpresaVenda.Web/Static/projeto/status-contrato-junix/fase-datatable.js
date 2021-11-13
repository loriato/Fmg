Europa.Controllers.StatusContratoJunix.FaseJunix.Incluir = undefined;
$(function () {
});

function TabelaFase ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.StatusContratoJunix.FaseDataTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.StatusContratoJunix.FaseDataTable;
    tabela
        .setTemplateEdit([
            '<input type="text" class="form-control" name="Fase" id="Fase" maxlength="50">'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('Fase').withTitle(Europa.i18n.Messages.FaseJunix)
        ])
        .setColActions(actionsHtml, '100px')
        .setActionSave(Europa.Controllers.StatusContratoJunix.FaseJunix.Salvar)
        .setIdAreaHeader("FaseDatatable_barra")
        .setOptionsSelect('POST', Europa.Controllers.StatusContratoJunix.listFases, Europa.Controllers.StatusContratoJunix.FaseJunix.filterParams);

    function actionsHtml(data, type, full, meta) {
        var button = '<div>';
        button = button + $scope.renderButton(Europa.Controllers.StatusContratoJunix.Permissoes.Atualizar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")");
        button = button + $scope.renderButton(Europa.Controllers.StatusContratoJunix.Permissoes.Excluir, "Excluir", "fa fa-trash", "Excluir(" + meta.row + ")");
        button = button + '</div >';
        return button;
    }

    $scope.onRowSelect = function (data) {
        if (Europa.Controllers.StatusContratoJunix.FaseDataTable.getDataRowEdit().Id === undefined || Europa.Controllers.StatusContratoJunix.FaseDataTable.getDataRowEdit().Fase === undefined) {
            if (Europa.Controllers.StatusContratoJunix.SinteseJunix.FaseId !== undefined) {
                if (Europa.Controllers.StatusContratoJunix.SinteseJunix.FaseId !== data.Id) {
                    Europa.Controllers.StatusContratoJunix.SinteseJunix.CarregarDados(data.Id);
                } else {
                    Europa.Controllers.StatusContratoJunix.SinteseJunix.FaseId = undefined;
                    Europa.Controllers.StatusContratoJunix.SinteseDataTable.closeEdition();
                    Europa.Controllers.StatusContratoJunix.SinteseDataTable.reloadData();
                }
            } else {
                Europa.Controllers.StatusContratoJunix.SinteseJunix.CarregarDados(data.Id);
            }
        }
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
    
    $scope.Editar = function (row) {
        Europa.Controllers.StatusContratoJunix.FaseJunix.Incluir = false;
        var objetoLinhaTabela = Europa.Controllers.StatusContratoJunix.FaseDataTable.getRowData(row);
        $scope.rowEdit(row);
    };

    $scope.Excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.StatusContratoJunix.FaseDataTable.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.Fase, function () {
            Europa.Controllers.StatusContratoJunix.FaseJunix.Excluir(objetoLinhaTabela.Id);
        });
    };
};

DataTableApp.controller('FaseDataTable', TabelaFase);

Europa.Controllers.StatusContratoJunix.FaseJunix.GetSelectedObjectsIds = function () {
    return Europa.Controllers.StatusContratoJunix.FaseDataTable.getRowsSelect().Id;
};

Europa.Controllers.StatusContratoJunix.FaseJunix.Filtrar = function () {
    Europa.Controllers.StatusContratoJunix.FaseDataTable.reloadData();
};

Europa.Controllers.StatusContratoJunix.FaseJunix.filterParams = function () {
    var params = {
        Fase: $("#filtroFases").val()
    };
    return params;
};

Europa.Controllers.StatusContratoJunix.FaseJunix.Novo = function () {
    Europa.Controllers.StatusContratoJunix.FaseDataTable.createRowNewData();
    Europa.Controllers.StatusContratoJunix.FaseJunix.Incluir = true;
};

Europa.Controllers.StatusContratoJunix.FaseJunix.Salvar = function () {    
    var obj = Europa.Controllers.StatusContratoJunix.FaseDataTable.getDataRowEdit();
    var url = Europa.Controllers.StatusContratoJunix.FaseJunix.Incluir ? Europa.Controllers.StatusContratoJunix.UrlIncluirFase : Europa.Controllers.StatusContratoJunix.UrlAlterarFase
   
    $.post(url, { fase: obj }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.StatusContratoJunix.FaseDataTable.closeEdition();
            Europa.Controllers.StatusContratoJunix.FaseDataTable.reloadData();
            Europa.Controllers.StatusContratoJunix.FaseJunix.LimparErro();
        } else {
            
            Europa.Controllers.StatusContratoJunix.FaseJunix.AdicionarErro(resposta.Campos);
        }
    });
};

Europa.Controllers.StatusContratoJunix.FaseJunix.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.StatusContratoJunix.FaseJunix.LimparErro = function () {
    $("[name='Fase']").parent().removeClass("has-error");
};

Europa.Controllers.StatusContratoJunix.FaseJunix.LimparFiltro = function () {
    $('#filtroFases').val('');
    Europa.Controllers.StatusContratoJunix.FaseDataTable.reloadData();
};

Europa.Controllers.StatusContratoJunix.FaseJunix.Excluir = function (id) {
    $.post(Europa.Controllers.StatusContratoJunix.UrlExcluirFase, { id: id }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.StatusContratoJunix.FaseDataTable.reloadData();
        }
    });
};