Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta = {};
Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela = {};
Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Permissoes = {};
$(function () {
   
});

DataTableApp.controller('AgrupamentoSituacaoProcessoDatatable', listaStatus);

function listaStatus($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela;
    tabelaWrapper
        .setTemplateEdit([
            '<input type="text" class="form-control" name="StatusPadrao" id="StatusPadrao" disabled>',
            '<input type="text" class="form-control" name="StatusPortalHouse" id="StatusPortalHouse">'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('Sistema.Nome').withTitle(Europa.i18n.Messages.TransferenciaStatusPreProposta_StatusPadrao).withOption('width', '45%'),
            DTColumnBuilder.newColumn('Sistema.Nome').withTitle(Europa.i18n.Messages.TransferenciaStatusPreProposta_StatusPortalHouse).withOption('width', '45%')
        ])
        .setColActions(actionsHtml, '10%')
        .setActionSave(Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Salvar)
        .setDefaultOptions('POST', Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.UrlListar, Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.filterParams);


    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButton(Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Permissoes.Alterar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButton(Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Permissoes.Excluir, "Excluir", "fa fa-trash", "Remover(" + meta.row + ")", full.Situacao) +
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
        Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Incluir = false;
        var objetoLinhaTabela = Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela.getRowData(row);
        $scope.rowEdit(row);
    };
};
Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Salvar = function () {    
    var obj = Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela.getDataRowEdit();
    var url = Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Incluir ? Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.UrlIncluir : Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.UrlAlterar;
    $.post(url, { model: obj }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela.closeEdition();
            Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela.reloadData();
            Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.LimparErro();
        } else {
            Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.AdicionarErro(resposta.Campos);
        }
    });
};

Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.filterParams = function () {
    var filtro = {        
        StatusPreProposta: $('#filtro_Nome').val(),
        IdAgrupamentoSituacao: Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.IdAgrupamento
    }
    console.log(filtro);
    return filtro;
};

Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Filtrar = function () {
    Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela.reloadData();
};

Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.LimparFiltro = function () {
    $('#filtro_Nome').val("");
};

Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.ExportarTodos = function () {
    console.log("exportarPaTod");
    var params = Europa.Controllers.EmpresaVenda.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.EmpresaVenda.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.ExportarPagina = function () {
    console.log("exportarPa");
    var params = Europa.Controllers.EmpresaVenda.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.EmpresaVenda.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};


Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.LimparErro = function () {
    $("[name='Descricao']").parent().removeClass("has-error");
};

