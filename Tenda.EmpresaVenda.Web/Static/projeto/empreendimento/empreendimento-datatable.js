
////////////////////////////////////////////////////////////////////////////////////
// Funções Datatable
////////////////////////////////////////////////////////////////////////////////////
DataTableApp.controller('empreendimentosTable', empreendimentosTable);

function empreendimentosTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.Empreendimento.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.Empreendimento.Tabela;
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Divisao').withTitle(Europa.i18n.Messages.Divisao).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Cidade').withTitle(Europa.i18n.Messages.Cidade).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Estado').withTitle(Europa.i18n.Messages.Estado).withOption('width', '15%'),
        DTColumnBuilder.newColumn('DisponibilizarCatalogo').withTitle(Europa.i18n.Messages.DisponibilizarNoCatalogo).withOption('width', '15%').renderWith(formatBoolean),
        DTColumnBuilder.newColumn('DisponivelVenda').withTitle(Europa.i18n.Messages.DisponivelParaVenda).withOption('width', '15%').renderWith(formatBoolean)
    ])
        .setIdAreaHeader("datatable_header")
        .setColActions(actionsHtml, '10%')
        .setAutoInit()
        .setDefaultOptions('POST', Europa.Controllers.Empreendimento.UrlListar, Europa.Controllers.Empreendimento.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButtonDetail(Europa.Controllers.Empreendimento.Permissoes.Visualizar, "Visualizar", "fa fa-eye", "detalhar(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonEdit(Europa.Controllers.Empreendimento.Permissoes.Atualizar, "Editar", "fa fa-edit", "editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonDelete(Europa.Controllers.Empreendimento.Permissoes.Excluir, "Excluir", "fa fa-trash", "excluir(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonBook(Europa.Controllers.Empreendimento.Permissoes.Atualizar, "Book do Empreendimento", "fa fa-folder-open-o", "book(" + meta.row + ")") +
            '</div>';
    }

    function formatBoolean(data) {
        if(data === true){
            return Europa.i18n.Messages.Sim;
        }
        return Europa.i18n.Messages.Nao;
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
    $scope.renderButtonDetail = function (hasPermission, title, icon, onClick, situacao) {
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

    $scope.renderButtonDelete = function (hasPermission, title, icon, onClick, situacao) {
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
    $scope.renderButtonBook = function (hasPermission, title, icon, onClick, situacao) {
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

    $scope.editar = function (row) {
        var objetoLinhaTabela = Europa.Controllers.Empreendimento.Tabela.getRowData(row);
        Europa.Controllers.Empreendimento.Editar(objetoLinhaTabela.Id);
    };

    $scope.detalhar = function (row) {
        var objetoLinhaTabela = Europa.Controllers.Empreendimento.Tabela.getRowData(row);
        Europa.Controllers.Empreendimento.Detalhar(objetoLinhaTabela.Id);
    };

    $scope.excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.Empreendimento.Tabela.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.Nome, function () {
            $.post(Europa.Controllers.Empreendimento.UrlExcluir, { idEmpreendimento: objetoLinhaTabela.Id }, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.Empreendimento.Tabela.reloadData();
                    Europa.Informacao.PosAcao(res);
                } else {
                    Europa.Informacao.PosAcao(res);
                }
            });
        });
    };

    $scope.book = function (row) {
        var objetoLinhaTabela = Europa.Controllers.Empreendimento.Tabela.getRowData(row);
        Europa.Controllers.BookBreveLancamento.Modal.Show(objetoLinhaTabela.Id, undefined);
    };
}

Europa.Controllers.Empreendimento.FilterParams = function () {
    var filtro = {
        nome: $('#filtro_nome').val(),
        cidade: $('#filtro_cidade').val(),
        estados: $('#filtro_estados').val(),
        disponibilizaCatalogo: $('#filtro_disponibiliza_catalogo').val(),
        disponivelVenda: $('#filtro_disponivel_venda').val(),
        IdRegionais: $('#autocomplete_regional').val()
    };
    return filtro;
};

Europa.Controllers.Empreendimento.FiltrarTabela = function () {
    Europa.Controllers.Empreendimento.Tabela.reloadData();
};

Europa.Controllers.Empreendimento.LimparFiltro = function () {
    $('#filtro_nome').val("");
    $('#filtro_cidade').val("");
    $('#filtro_estados').val("").trigger('change');
    $('#autocomplete_regional').val("").trigger('change');

};

Europa.Controllers.Empreendimento.ExportarTodos = function () {
    var params = Europa.Controllers.Empreendimento.Tabela.lastRequestParams;
    var formExportacao = $("#formExportacao");

    $("#formExportacao").find("input").remove();
    formExportacao.attr("method", "post").attr("action", Europa.Controllers.Empreendimento.UrlExportarTodos);
    formExportacao.addHiddenInputData(params);
    formExportacao.submit();
}

Europa.Controllers.Empreendimento.ExportarPagina = function () {
    var params = Europa.Controllers.Empreendimento.Tabela.lastRequestParams;
    var formExportacao = $("#formExportacao");

    $("#formExportacao").find("input").remove();
    formExportacao.attr("method", "post").attr("action", Europa.Controllers.Empreendimento.UrlExportarPagina);
    formExportacao.addHiddenInputData(params);
    formExportacao.submit();
}