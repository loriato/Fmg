Europa.Controllers.FilaUnificada.TabelaOperador = undefined;

$(function () {
    
});

function TabelaFilaUnificadaOperador($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.FilaUnificada.TabelaOperador = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.FilaUnificada.TabelaOperador;
    self.setColumns([
        DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption('width', '100px').notSortable(),
        DTColumnBuilder.newColumn('DataStatus').withTitle(Europa.i18n.Messages.DataHoraUltimoEnvio).withOption('width', '175px').renderWith(Europa.Date.toGeenDateTimeFormat).notSortable(),
        DTColumnBuilder.newColumn('DataElaboracao').withTitle(Europa.i18n.Messages.DataElaboracao).withOption('width', '175px').renderWith(Europa.Date.toGeenDateTimeFormat).notSortable(),
        DTColumnBuilder.newColumn('NomeCliente').withTitle(Europa.i18n.Messages.Cliente).withOption('width', '300px')
            .renderWith(formatLinkCliente).notSortable(),
        DTColumnBuilder.newColumn('CpfCnpjCliente').withTitle(Europa.i18n.Messages.Cpf).withOption('width', '150px').renderWith(Europa.String.FormatCpf).notSortable(),
        DTColumnBuilder.newColumn('NomeEmpreendimento').withTitle(Europa.i18n.Messages.Empreendimento).withOption('width', '200px')
            .renderWith(formatLinkEmpreendimento).notSortable(),
        DTColumnBuilder.newColumn('NomeTorre').withTitle(Europa.i18n.Messages.Torre).withOption('width', '100px')
            .renderWith(formatLinkTorre).notSortable(),
        DTColumnBuilder.newColumn('NomeUnidade').withTitle(Europa.i18n.Messages.Unidade).withOption('width', '200px')
            .renderWith(formatLinkUnidade).notSortable(),
        DTColumnBuilder.newColumn('').withClass('hidden', 'hidden').notSortable()
    ])
        .setIdAreaHeader("datatable_operador_header")
        .setColActions(actionsHtml, '100px')
        .setDefaultOrder([[8, 'asc']])
        .setDefaultOptions('POST', Europa.Controllers.FilaUnificada.UrlListar, Europa.Controllers.FilaUnificada.FiltroOperador);

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
    };

    $scope.alterarSicaq = function (row) {
        var objeto = Europa.Controllers.FilaUnificada.Tabela.getRowData(row);
        Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarSicaq.AbrirModal(objeto.Id);
    };

    function actionsHtml(data, meta, full, type) {
        var content = "<div>";
        if (full.StatusPreProposta == 2) {
            content = content + $scope.renderButton('true', Europa.i18n.Messages.AlterarSicaq, 'fa fa-exchange', 'alterarSicaq(' + meta.row + ')');
            content = content + $scope.renderButton('true', Europa.i18n.Messages.Revisar, 'fa fa-undo', 'revisar(' + meta.row + ')');
        }

        if (full.IdAvalista) {
            content = content + "<a class='btn btn-default' title='Avalista' href='" + Europa.Controllers.FilaUnificada.UrlDocumentacaoAvalista + '?id=' + full.IdProposta + "' target=_blank><i class='fa fa-folder-open-o'></i></a>";
        }

        if (full.Origem == 1) {
            content = content + '<a class="btn btn-default" title="Visualizar" href="' + Europa.Controllers.FilaUnificada.UrlVerDocumentacao + '?id=' + full.IdProposta + '&codigoUf=REL01" target=_blank><i class="fa fa-eye"></i></a>';
        }

        if (full.Origem == 2) {
            content = content + '<a class="btn btn-default" title="Visualizar" href="' + Europa.Components.DetailAction.Proposta + '?id=' + full.IdProposta + '" target=_blank><i class="fa fa-eye"></i></a>';
        }

        content = content + "</div>";
        return content;
    };

    function formatLinkCliente(data, meta, full, type) {
        if (full.Origem == 1) {
            return '<a href="' + Europa.Components.DetailAction.Cliente + '?id=' + full.IdCliente + '" target=_blank>' + full.NomeCliente + '</a>';
        }

        if (full.Origem == 2) {
            return '<a href="' + Europa.Components.DetailAction.ClienteSUAT + '?id=' + full.IdCliente + '" target=_blank>' + full.NomeCliente + '</a>';
        }
    };

    function formatLinkEmpreendimento(data, meta, full, type) {
        if (full.Origem == 1) {
            return full.NomeEmpreendimento;
        }

        if (full.Origem == 2) {
            return '<a href="' + Europa.Components.DetailAction.EmpreendimentoSUAT + '?id=' + full.IdEmpreendimento + '" target=_blank>' + full.NomeEmpreendimento + '</a>';
        }
    };

    function formatLinkTorre(data, meta, full, type) {
        if (full.NomeTorre) {
            return '<a href="' + Europa.Components.DetailAction.TorreSUAT + '?id=' + full.IdTorre + '" target=_blank>' + full.NomeTorre + '</a>';
        }
        return "";
    };

    function formatLinkUnidade(data, meta, full, type) {
        if (full.NomeUnidade) {
            return '<a href="' + Europa.Components.DetailAction.Unidade + '?id=' + full.IdUnidade + '" target=_blank>' + full.NomeUnidade + '</a>';
        }
        return "";
    };
}

DataTableApp.controller('FilaUnificadaDatatableOperador', TabelaFilaUnificadaOperador);

Europa.Controllers.FilaUnificada.FiltroOperador = function () {
    var param = {
        IsOperador: true
    };

    return param;
};

Europa.Controllers.FilaUnificada.ExportarPaginaOperador = function () {
    var params = Europa.Controllers.FilaUnificada.TabelaOperador.lastRequestParams;
    var formExportar = $("#form_exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.FilaUnificada.UrlExportarPaginaOperador);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.FilaUnificada.ExportarTodosOperador = function () {
    var params = Europa.Controllers.FilaUnificada.TabelaOperador.lastRequestParams;
    var formExportar = $("#form_exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.FilaUnificada.UrlExportarTodosOperador);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.FilaUnificada.FiltrarOperador = function () {
    Europa.Controllers.FilaUnificada.TabelaOperador.reloadData();
};