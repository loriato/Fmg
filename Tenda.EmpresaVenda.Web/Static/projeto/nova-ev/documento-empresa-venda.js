$(function () {
});

DataTableApp.controller('documentoEmpresaVendaTable', documentoEmpresaVendaTable);

function documentoEmpresaVendaTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.NovaEv.TabelaDocumentos = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.NovaEv.TabelaDocumentos;
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('NomeArquivo').withTitle(Europa.i18n.Messages.NomeFantasia).withOption('width', '15%'),
        DTColumnBuilder.newColumn('NomeTipoDocumento').withTitle(Europa.i18n.Messages.NomeFantasia).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Status).withOption('width', '10%').withOption('type', 'enum-format-SituacaoAprovacaoDocumento'),

    ])
        .setIdAreaHeader("documento_empresa_venda_datatable_header")
        .setColActions(actionsHtml, '5%')
        .setAutoInit(false)
        .setDefaultOptions('POST', Europa.Controllers.NovaEv.UrlListarDocumentosEmpresaVenda, Europa.Controllers.NovaEv.FilterParams);

    function actionsHtml(data, type, full, meta) {
        var botoes = '<div>';

        botoes += $scope.renderButton(Europa.Controllers.NovaEv.Permissoes.Download, "Download", "fa fa-download", "Download(" + full.IdArquivo + ")");

        return botoes+='</div>';
    }

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    }

    $scope.Download = function (idArquivo) {
        location.href = Europa.Controllers.NovaEv.UrlDownloadDocumentoEmpresaVenda + "?idArquivo=" + idArquivo;
    }
};

Europa.Controllers.NovaEv.FilterParams = function () {
    return {
        IdEmpresaVenda: Europa.Controllers.NovaEv.IdEmpresaVenda
    };
};

Europa.Controllers.NovaEv.FiltrarDocumentos = function () {
    Europa.Controllers.NovaEv.TabelaDocumentos.reloadData();
}

