Europa.Controllers.StatusContratoJunix.SinteseJunix.Incluir = true;
Europa.Controllers.StatusContratoJunix.SinteseJunix.FaseId = undefined;
Europa.Controllers.StatusContratoJunix.SinteseJunix.GetFaseId = undefined;

Europa.Controllers.StatusContratoJunix.SinteseJunix.GetFaseId = function () {
    if (Europa.Controllers.StatusContratoJunix.SinteseJunix.FaseId !== undefined) {
        return Europa.Controllers.StatusContratoJunix.SinteseJunix.FaseId;
    } else if (Europa.Controllers.StatusContratoJunix.FaseDataTable.getRowsSelect() !== undefined) {
        return Europa.Controllers.StatusContratoJunix.FaseDataTable.getRowsSelect().Id;
    } else {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.i18n.Messages.NecessarioDefinirFase);
        Europa.Informacao.Show();
    }
    return undefined;
};

$(function () {
    
});

function TabelaSintese($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.StatusContratoJunix.SinteseDataTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.StatusContratoJunix.SinteseDataTable;
    tabela
        .setTemplateEdit([            
            '<input type="text" class="form-control" name="Sintese" id="Sintese" maxlength="50">',
            '<input type="text" class="form-control" name="StatusContrato" id="StatusContrato" maxlength="50">',
            '<input type="hidden" name="FaseJunix.Id" value="" readonly="true">'
        ])
        .setColumns([             
            DTColumnBuilder.newColumn('Sintese').withTitle(Europa.i18n.Messages.Sintese).withOption("width", "45%"),
            DTColumnBuilder.newColumn('StatusContrato').withTitle(Europa.i18n.Messages.StatusContrato).withOption("width", "45%"),
            DTColumnBuilder.newColumn('FaseJunix.Id').withClass('hidden', 'hidden').withOption("width","0%")
        ])
        .setColActions(actionsHtml, '10%')
        .setAutoInit(false)
        .setActionSave(Europa.Controllers.StatusContratoJunix.SinteseJunix.Salvar)
        .setIdAreaHeader("SinteseDatatable_barra")
        .setOptionsSelect('POST', Europa.Controllers.StatusContratoJunix.listSinteses, Europa.Controllers.StatusContratoJunix.SinteseJunix.filterParams);

    function actionsHtml(data, type, full, meta) {
        var button = '<div>';
        button = button + $scope.renderButton(Europa.Controllers.StatusContratoJunix.Permissoes.Atualizar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")");
        button = button + $scope.renderButton(Europa.Controllers.StatusContratoJunix.Permissoes.Excluir, "Excluir", "fa fa-trash", "Excluir(" + meta.row + ")");
        button = button + '</div >';
        return button;
    };

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
        Europa.Controllers.StatusContratoJunix.SinteseJunix.Incluir = false;
        var objetoLinhaTabela = Europa.Controllers.StatusContratoJunix.SinteseDataTable.getRowData(row);
        $scope.rowEdit(row);
    };

    $scope.Excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.StatusContratoJunix.SinteseDataTable.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.Sintese, function () {
            Europa.Controllers.StatusContratoJunix.SinteseJunix.Excluir(objetoLinhaTabela.Id);
        });
    };
};

DataTableApp.controller('SinteseDataTable', TabelaSintese);

Europa.Controllers.StatusContratoJunix.SinteseJunix.CarregarDados = function (faseId) {
    Europa.Controllers.StatusContratoJunix.SinteseJunix.FaseId = faseId;
    Europa.Controllers.StatusContratoJunix.SinteseDataTable.closeEdition();
    Europa.Controllers.StatusContratoJunix.SinteseDataTable.reloadData();
}

Europa.Controllers.StatusContratoJunix.SinteseJunix.GetSelectedObjectsIds = function () {
    return Europa.Controllers.StatusContratoJunix.SinteseDataTable.getRowsSelect().Id;
};

Europa.Controllers.StatusContratoJunix.SinteseJunix.Filtrar = function () {
    if (Europa.Controllers.StatusContratoJunix.SinteseJunix.FaseId !== undefined) {
        Europa.Controllers.StatusContratoJunix.SinteseDataTable.reloadData();
    } else {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.i18n.Messages.NecessarioDefinirFase);
        Europa.Informacao.Show();
    }
    
};

Europa.Controllers.StatusContratoJunix.SinteseJunix.filterParams = function () {
    var params = {};
    if (Europa.Controllers.StatusContratoJunix.SinteseJunix.FaseId !== undefined) {
        params.Sintese = $("#filtroSintese").val();
        params.StatusContrato = $("#filtroStatusContrato").val();
        params.IdFase = Europa.Controllers.StatusContratoJunix.SinteseJunix.GetFaseId();
    };
    return params;
};

Europa.Controllers.StatusContratoJunix.SinteseJunix.Novo = function () {
    if (Europa.Controllers.StatusContratoJunix.SinteseJunix.FaseId !== undefined) {
        Europa.Controllers.StatusContratoJunix.SinteseDataTable.createRowNewData();
        Europa.Controllers.StatusContratoJunix.SinteseJunix.Incluir = true;
    } else {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.i18n.Messages.NecessarioDefinirFase);
        Europa.Informacao.Show();
    }
};

Europa.Controllers.StatusContratoJunix.SinteseJunix.Salvar = function () {
    var obj = Europa.Controllers.StatusContratoJunix.SinteseDataTable.getDataRowEdition();
    var fase = {
        Id: Europa.Controllers.StatusContratoJunix.SinteseJunix.GetFaseId()
    };
    obj.FaseJunix = fase;
    var url = Europa.Controllers.StatusContratoJunix.SinteseJunix.Incluir ? Europa.Controllers.StatusContratoJunix.UrlIncluirSintese : Europa.Controllers.StatusContratoJunix.UrlAlterarSintese
    $.post(url, { sintese: obj }, function (resposta) {
        Europa.Controllers.StatusContratoJunix.SinteseJunix.LimparErro();
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.StatusContratoJunix.SinteseDataTable.closeEdition();
            Europa.Controllers.StatusContratoJunix.SinteseDataTable.reloadData();            
        } else {
            Europa.Controllers.StatusContratoJunix.SinteseJunix.AdicionarErro(resposta.Campos);
        }
    });
};

Europa.Controllers.StatusContratoJunix.SinteseJunix.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.StatusContratoJunix.SinteseJunix.LimparErro = function () {
    $("[name='Sintese']").parent().removeClass("has-error");
    $("[name='StatusContrato']").parent().removeClass("has-error");
};

Europa.Controllers.StatusContratoJunix.SinteseJunix.LimparFiltro = function () {
    $('#filtroSintese').val('');
    $('#filtroStatusContrato').val('')
    Europa.Controllers.StatusContratoJunix.SinteseDataTable.reloadData();
};

Europa.Controllers.StatusContratoJunix.SinteseJunix.Excluir = function (id) {
    $.post(Europa.Controllers.StatusContratoJunix.UrlExcluirSintese, { id: id }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.StatusContratoJunix.SinteseDataTable.reloadData();
        }
    });
};