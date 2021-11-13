Europa.Controllers.StatusImportacao.Tabela = {};

$(function () {
    Europa.Components.DatePicker.AutoApply();
    Europa.Controllers.StatusImportacao.LimparBusca();

    setTimeout(Europa.Controllers.StatusImportacao.ConfigDatePicker, 1000);

    $("#filtro_origem").select2({
        trags: true,
         width: '100%'
    });

    $("#filtro_situacao").select2({
        trags: true,
        width: '100%'
    });
    setTimeout(Europa.Controllers.StatusImportacao.BuscarSituacao, 6000);
})

function listaImportacoes($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.StatusImportacao.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.StatusImportacao.Tabela;

    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('NomeArquivo').withTitle(Europa.i18n.Messages.NomeArquivo).withOption("width", "100px"),
        DTColumnBuilder.newColumn('NomeUsuario').withTitle(Europa.i18n.Messages.CriadoPor).withOption("width", "100px"),
        DTColumnBuilder.newColumn('CriadoEm').withTitle(Europa.i18n.Messages.CriadoEm).withOption("width", "100px").withOption('type', 'date-format-DD/MM/YYYY HH:mm:ss'),
        DTColumnBuilder.newColumn('Origem').withTitle(Europa.i18n.Messages.Origem).withOption('type', 'enum-format-TipoOrigem').withOption('width', '50px'),
        DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('type', 'enum-format-SituacaoArquivo').withOption('width', '100px'),
        DTColumnBuilder.newColumn('DataInicio').withTitle(Europa.i18n.Messages.Inicio).withOption("width", "100px").withOption('type', 'date-format-DD/MM/YYYY HH:mm:ss'),
        DTColumnBuilder.newColumn('DataFim').withTitle(Europa.i18n.Messages.Fim).withOption("width", "100px").renderWith(formatDate),
        DTColumnBuilder.newColumn('RegistroAtual').withTitle(Europa.i18n.Messages.RegistroAtual).withOption("width", "80px"),
        DTColumnBuilder.newColumn('TotalRegistros').withTitle(Europa.i18n.Messages.TotalRegistros).withOption("width","80px")
    ])
        .setDefaultOrder([[3, 'desc']])
        .setIdAreaHeader("datatable_barra")
        .setColActions(actionsHtml, '60px')
        .setDefaultOptions('POST', Europa.Controllers.StatusImportacao.UrlListarImportacoes, Europa.Controllers.StatusImportacao.ParametrosFiltro);

    function actionsHtml(data, type, full, meta) {
        var button = '<div>';
        if (data.IdExecucao !== 0) {
            button = button + $scope.renderButton('true', "Detalhar", "fa fa-file-text-o", "Detalhar(" + data.IdExecucao + ")");
        }
        if (data.IdArquivo !== 0) {
            button = button + $scope.renderButton('true', "Download", "fa fa-download", "Download(" + data.IdArquivo + ")");
        }
        return button;

    };

    function formatDate(date) {
        if (date) {
            return Europa.Date.toFormatDate(date, "DD/MM/YYYY HH:mm:ss");
        }
        return "";
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

    $scope.Detalhar = function (idExecucao) {
        Europa.Controllers.StatusImportacao.Detalhar(idExecucao);
    };

    $scope.Download = function (idArquivo) {
        Europa.Controllers.StatusImportacao.Download(idArquivo);
    };

};

DataTableApp.controller('listaImportacoes', listaImportacoes);

Europa.Controllers.StatusImportacao.Detalhar = function (idExecucao) {
    Europa.Components.LogExecucaoModal.AbrirModal(idExecucao);
};

Europa.Controllers.StatusImportacao.Download = function (idArquivo) {
    location.href = Europa.Controllers.StatusImportacao.UrlDownloadArquivo + "?idArquivo=" + idArquivo;
}

Europa.Controllers.StatusImportacao.ParametrosFiltro = function () {
    return {
        DataInicio: $('#DataInicio').val(),
        DataFim: $('#DataFim').val(),
        DataCriacaoInicio: $('#DataCriacaoInicio').val(),
        DataCriacaoFim: $('#DataCriacaoFim').val(),
        NomeArquivo: $('#filtro_nomeArquivo').val(),
        Origem: $('#filtro_origem').val(),
        Situacao: $('#filtro_situacao').val(),
        CriadoPor: $('#filtro_criadoPor').val()

    };
};

Europa.Controllers.StatusImportacao.Filtrar = function () {
    Europa.Controllers.StatusImportacao.Tabela.reloadData();
}

Europa.Controllers.StatusImportacao.LimparFiltros = function () {
    $('#DataInicio').val("");
    $('#DataFim').val("");
    $('#DataCriacaoInicio').val("");
    $('#DataCriacaoFim').val("");
    $('#filtro_nomeArquivo').val("");
    $('#filtro_origem').val("").trigger("change");
    $('#filtro_situacao').val("").trigger("change");
    $('#filtro_criadoPor').val("");
};

Europa.Controllers.StatusImportacao.ConfigDatePicker = function () {
    new Europa.Components.DatePicker()
        .WithTarget('#filtroDataInicio')
        .WithParentEl("#modalLogsExecucao")
        .WithFormat("DD/MM/YYYY")
        .WithOpens("right")
        .WithParentEl("#modalLogsExecucao")
        .Configure();

    new Europa.Components.DatePicker()
        .WithTarget('#filtroDataFim')
        .WithParentEl("#modalLogsExecucao")
        .WithFormat("DD/MM/YYYY")
        .WithOpens("right")
        .Configure();

};

Europa.Controllers.StatusImportacao.LimparBusca = function () {
    $("#filtroDataInicio").val("");
    $("#filtroDataFim").val("");
    $("#filtroTipo").val("");
    $("#filtroLog").val("");
};

Europa.Controllers.StatusImportacao.ExportarTodos = function () {
    var params = Europa.Controllers.StatusImportacao.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.StatusImportacao.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.StatusImportacao.ExportarPagina = function () {
    var params = Europa.Controllers.StatusImportacao.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.StatusImportacao.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.StatusImportacao.Executar = function () {
    $.post(Europa.Controllers.StatusImportacao.UrlIntegrarRepasseJunix, function (res) {
        Europa.Messages.ShowMessages(res);
        if (res.Sucesso) {
          setTimeout(Europa.Controllers.StatusImportacao.BuscarSituacao, 5000);
        }
       
    });
};

Europa.Controllers.StatusImportacao.BuscarSituacao = function () {
    $.ajax({
        url: Europa.Controllers.StatusImportacao.UrlBuscarSituacao,
        type: 'GET',
        async: false,
        success: function (res) {
            console.log(res);
            if (res == 'True') {
                Europa.Controllers.StatusImportacao.Tabela.reloadData();
                var time = setTimeout(Europa.Controllers.StatusImportacao.BuscarSituacao, 5000);
            } else {
                Europa.Controllers.StatusImportacao.Tabela.reloadData();
            }
        }
   });
}