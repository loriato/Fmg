Europa.Controllers.TranslateStatusPreProposta = {};
Europa.Controllers.TranslateStatusPreProposta.Tabela = {};
Europa.Controllers.TranslateStatusPreProposta.Permissoes = {};
$(function () {
   
});

DataTableApp.controller('TranslateStatusDatatable', listaStatus);

function listaStatus($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.TranslateStatusPreProposta.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.TranslateStatusPreProposta.Tabela;
    tabelaWrapper
        .setTemplateEdit([
            '<input type="text" class="form-control" name="StatusPadrao" id="StatusPadrao" disabled>',
            '<input type="text" class="form-control" name="StatusPortalHouse" id="StatusPortalHouse">'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('StatusPadrao').withTitle(Europa.i18n.Messages.TransferenciaStatusPreProposta_StatusPadrao).withOption('width', '45%'),
            DTColumnBuilder.newColumn('StatusPortalHouse').withTitle(Europa.i18n.Messages.TransferenciaStatusPreProposta_StatusPortalHouse).withOption('width', '45%')
        ])
        .setColActions(actionsHtml, '10%')
        .setActionSave(Europa.Controllers.TranslateStatusPreProposta.Salvar)
        .setDefaultOptions('POST', Europa.Controllers.TranslateStatusPreProposta.UrlListar, Europa.Controllers.TranslateStatusPreProposta.filterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButton(Europa.Controllers.TranslateStatusPreProposta.Permissoes.Alterar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")", full.Situacao) +
            //$scope.renderButton(Europa.Controllers.TranslateStatusPreProposta.Permissoes.Excluir, "Excluir", "fa fa-trash", "Excluir(" + meta.row + ")", full.Situacao) +
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
    }

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (hasPermission === 'true') {
            icon = $('<i/>').addClass(icon);
            var button = $('<a />')
                .addClass('btn btn-default')
                .attr('title', title)
                .attr('ng-click', onClick)
                .append(icon);
            return button.prop('outerHTML');
        } else {
            return null;
        }
    };

    $scope.Editar = function (row) {
        Europa.Controllers.TranslateStatusPreProposta.Incluir = false;
        var objetoLinhaTabela = Europa.Controllers.TranslateStatusPreProposta.Tabela.getRowData(row);
        $scope.rowEdit(row);
    };
};
Europa.Controllers.TranslateStatusPreProposta.Salvar = function () {
    console.log("teste");
    var obj = Europa.Controllers.TranslateStatusPreProposta.Tabela.getDataRowEdit();
    var url = Europa.Controllers.TranslateStatusPreProposta.Incluir ? Europa.Controllers.TranslateStatusPreProposta.UrlIncluir : Europa.Controllers.TranslateStatusPreProposta.UrlAlterar;
    $.post(url, { model: obj }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.TranslateStatusPreProposta.Tabela.closeEdition();
            Europa.Controllers.TranslateStatusPreProposta.Tabela.reloadData();
            Europa.Controllers.TranslateStatusPreProposta.LimparErro();
        } else {
            Europa.Controllers.TranslateStatusPreProposta.AdicionarErro(resposta.Campos);
        }
    });
};

Europa.Controllers.TranslateStatusPreProposta.filterParams = function () {
    console.log("Teste");
    var filtro = {
        StatusPadrao: $('#filtro_Nome').val(),
    }
    console.log(filtro);
    return filtro;
};

Europa.Controllers.TranslateStatusPreProposta.Filtrar = function () {
    Europa.Controllers.TranslateStatusPreProposta.Tabela.reloadData();
};

Europa.Controllers.TranslateStatusPreProposta.LimparFiltro = function () {
    $('#filtro_Nome').val("");
};

Europa.Controllers.TranslateStatusPreProposta.ExportarTodos = function () {
    console.log("exportarPaTod");
    var params = Europa.Controllers.EmpresaVenda.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.EmpresaVenda.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.TranslateStatusPreProposta.ExportarPagina = function () {
    console.log("exportarPa");
    var params = Europa.Controllers.EmpresaVenda.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.EmpresaVenda.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};


Europa.Controllers.TranslateStatusPreProposta.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.TranslateStatusPreProposta.LimparErro = function () {
    $("[name='Descricao']").parent().removeClass("has-error");
};