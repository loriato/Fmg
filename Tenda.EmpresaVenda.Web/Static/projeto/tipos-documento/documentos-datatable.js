DataTableApp.controller('TiposDocumento', TiposDocumentoDatatable);

function TiposDocumentoDatatable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.TiposDocumento.DataTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Controllers.TiposDocumento.DataTable
        .setIdAreaHeader("documentos_datatable_barra")
        .setTemplateEdit([
            '<input type="text" class="form-control" id="Nome" name="Nome"  maxlength="256">',
            '<select class="form-control" id="VisivelPortal" name="VisivelPortal">' +
            '<option value="true">' + Europa.i18n.Messages.Sim + '</option>' +
            '<option value="false">' + Europa.i18n.Messages.Nao + '</option>' +
            '</select>',
            '<select class="form-control" id="VisivelLoja" name="VisivelLoja">' +
            '<option value="true">' + Europa.i18n.Messages.Sim + '</option>' +
            '<option value="false">' + Europa.i18n.Messages.Nao + '</option>' +
            '</select>',
        ])
        .setColumns([
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '50%'),
            DTColumnBuilder.newColumn('VisivelPortal').withTitle(Europa.i18n.Messages.VisivelPortal).withOption('width', '10%').renderWith(renderBoolean),
            DTColumnBuilder.newColumn('VisivelLoja').withTitle(Europa.i18n.Messages.VisivelLoja).withOption('width', '10%').renderWith(renderBoolean),

        ])
        .setColActions(actionsHtml, '5%')
        .setIdAreaHeader("datatable_header")
        .setAutoInit(true)
        .setActionSave(Europa.Controllers.TiposDocumento.ConfirmarSalvar)
        .setDefaultOptions('POST', Europa.Controllers.TiposDocumento.UrlListar, Europa.Controllers.TiposDocumento.FilterParams);



    function actionsHtml(data, type, full, meta) {
        var canExclude = false;
        //alert(JSON.stringify(full));
        if (Europa.Controllers.TiposDocumento.Permissoes.Excluir) {
            if (full.DocumentoUsado && full.DocumentoObrigatorio) {
                canExclude = true;
            }
        }
        return '<div>' +
            //$scope.renderButton(Europa.Controllers.CadastroBanner.Permissoes.Atualizar, "Editar", "fa fa-edit", "editar(" + meta.row + ")") +
            //$scope.renderButton(editar, "Upload", "fa fa-upload", "upload(" + meta.row + ")") +
            $scope.renderButton(canExclude, "Excluir", "fa fa-trash", "excluir(" + meta.row + ")") +
            $scope.renderButtonEdit(Europa.Controllers.TiposDocumento.Permissoes.Alterar, "Editar", "fa fa-edit", "editar(" + meta.row + ")", full.Situacao) +

            '</div>';
    }

    $scope.renderButtonEdit = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 3) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (hasPermission === false) {
            return "";
        }

        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.editar = function (row) {
        Europa.Controllers.TiposDocumento.ModoEdicao = true;
        $scope.rowEdit(row);
        var objetoLinhaTabela = Europa.Controllers.TiposDocumento.DataTable.getRowData();
        Europa.Controllers.TiposDocumento.HabilitarEdicao(objetoLinhaTabela);
    };

    $scope.excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.TiposDocumento.DataTable.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.Nome, function () {
            $.post(Europa.Controllers.TiposDocumento.UrlExcluir, { id: objetoLinhaTabela.Id }, function (res) {
                if (res.Success) {
                    Europa.Controllers.TiposDocumento.DataTable.reloadData();
                }
                Europa.Informacao.PosAcaoApi(res);
            });
        });
    }
    function renderBoolean(data) {
        if (data === true) {
            return Europa.i18n.Messages.Sim;
        }

        return Europa.i18n.Messages.Nao;
    };

}

Europa.Controllers.TiposDocumento.FilterParams = function () {
    var filtro = {
        Nome: $('#nome_filtro').val(),
    };
    return filtro;
};

Europa.Controllers.TiposDocumento.FiltrarTabela = function () {
    Europa.Controllers.TiposDocumento.DataTable.reloadData();
};

Europa.Controllers.TiposDocumento.LimparFiltro = function () {
    $('#nome_filtro').val("");
};

Europa.Controllers.TiposDocumento.Novo = function () {
    Europa.Controllers.TiposDocumento.ModoEdicao = false;
    Europa.Controllers.TiposDocumento.DataTable.createRowNewData();
};

Europa.Controllers.TiposDocumento.ConfirmarSalvar = function () {
    if (Europa.Controllers.TiposDocumento.ModoEdicao) {
        Europa.Controllers.TiposDocumento.Salvar();
    } else {
        Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Confirmacao,Europa.i18n.Messages.ConfirmarNovoTipoDocumento);
        Europa.Confirmacao.ConfirmCallback = Europa.Controllers.TiposDocumento.Salvar;
        Europa.Confirmacao.Show();
    }
};

Europa.Controllers.TiposDocumento.Salvar = function () {
    var objetoLinhaTabela = Europa.Controllers.TiposDocumento.DataTable.getDataRowEdit();
    var obj = Europa.Controllers.TiposDocumento.PreencherFiltro(objetoLinhaTabela);
    var url = Europa.Controllers.TiposDocumento.ModoEdicao ? Europa.Controllers.TiposDocumento.UrlAlterar : Europa.Controllers.TiposDocumento.UrlIncluir
    Europa.Controllers.TiposDocumento.ModoEdicao = false;
    $.post(url, obj, function (res) {
            Europa.Controllers.TiposDocumento.DataTable.closeEdition();
            Europa.Controllers.TiposDocumento.DataTable.reloadData();
        Europa.Informacao.PosAcaoApi(res);
    });
};


Europa.Controllers.TiposDocumento.HabilitarEdicao = function (objetoLinhaTabela) {
    $('#Nome').attr('disabled', 'disabled');
    $('#VisivelPortal').val(JSON.stringify(objetoLinhaTabela.VisivelPortal)).trigger('change');
    $('#VisivelLoja').val(JSON.stringify(objetoLinhaTabela.VisivelLoja)).trigger('change');
};

Europa.Controllers.TiposDocumento.PreencherFiltro = function (objetoLinhaTabela) {
    var obj = {
        Id: objetoLinhaTabela.Id,
        Situacao: 1,
        Nome: objetoLinhaTabela.Nome,
        VisivelPortal: objetoLinhaTabela.VisivelPortal,
        VisivelLoja: objetoLinhaTabela.VisivelLoja,
        Obrigatorio: objetoLinhaTabela.Obrigatorio,
        ObrigatorioLoja: objetoLinhaTabela.ObrigatorioLoja
    };
    return obj;
}


//Europa.Controllers.Loja.ExportarTodos = function () {
//    var params = Europa.Controllers.Loja.Tabela.lastRequestParams;
//    var formExportar = $("#Exportar");
//    $("#Exportar").find("input").remove();
//    formExportar.attr("method", "post").attr("action", Europa.Controllers.Loja.UrlExportarTodos);
//    formExportar.addHiddenInputData(params);
//    formExportar.submit();
//};

//Europa.Controllers.Loja.ExportarPagina = function () {
//    var params = Europa.Controllers.Loja.Tabela.lastRequestParams;
//    var formExportar = $("#Exportar");
//    $("#Exportar").find("input").remove();
//    formExportar.attr("method", "post").attr("action", Europa.Controllers.Loja.UrlExportarPagina);
//    formExportar.addHiddenInputData(params);
//    formExportar.submit();
//};
