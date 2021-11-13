Europa.Components.Datatable.LogEntidade = {};
Europa.Components.Datatable.LogEntidade.createDT = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

    Europa.Components.Datatable.LogEntidade.DataTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);

    Europa.Components.Datatable.LogEntidade.DataTable
        .setIdAreaHeader("LogEntidadeDatatable_barra")
        .setColumns([
            DTColumnBuilder.newColumn('Entidade').withTitle(Europa.i18n.Messages.Entidade).withOption('width', '25%'),
            DTColumnBuilder.newColumn('ChavePrimaria').withTitle(Europa.i18n.Messages.ChavePrimaria).withOption('width', '10%'),
            DTColumnBuilder.newColumn('NomeUsuarioCriacao').withTitle(Europa.i18n.Messages.CriadoPor).withOption('width', '15%'),
            DTColumnBuilder.newColumn('CriadoEm').withTitle(Europa.i18n.Messages.CriadoEm).withOption("type", "date-format-DD/MM/YYYY HH:mm:ss").withOption('width', '15%'),
            DTColumnBuilder.newColumn('NomeUsuarioAtualizacao').withTitle(Europa.i18n.Messages.AtualizadoPor).withOption('width', '15%'),
            DTColumnBuilder.newColumn('AtualizadoEm').withTitle(Europa.i18n.Messages.AtualizadoEm).withOption("type", "date-format-DD/MM/YYYY HH:mm:ss").withOption('width', '15%')
        ])
        .setColActions(actionsHtml, '50px')
        .setDefaultOrder([[5, 'desc']])
        .setAutoInit(false)
        .setOptionsSelect('POST', Europa.Components.Datatable.LogEntidade.listAction, Europa.Components.Datatable.LogEntidade.filterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>'
            + Europa.Components.Datatable.LogEntidade.DataTable.renderButton(true, Europa.i18n.Messages.Detalhar, 'fa fa-eye', 'detalharLog(' + meta.row + ')')
            + '</div>';
    };

    $scope.detalharLog = function(id) {
        Europa.Components.Datatable.LogEntidade.DetalharLog(id);
    };
};

DataTableApp.controller('LogEntidadeDataTable', Europa.Components.Datatable.LogEntidade.createDT);


Europa.Components.Datatable.LogEntidade.filterParams = function() {
    var params = {
        DataInicio: $("#DataInicio").val(),
        DataFim: $("#DataFim").val(),
        Entidade: $("#form_filtro_log_entidade").find("#Entidade").val(),
        ChavePrimaria: $("#form_filtro_log_entidade").find("#ChavePrimariaFiltro").val(),
        IdUsuarioCriador: Europa.Controllers.LogEntidade.AutoCompleteUsuarioCriador !== undefined
            ? Europa.Controllers.LogEntidade.AutoCompleteUsuarioCriador.Value()
            : "",
        IdUsuarioAtualizacao: Europa.Controllers.LogEntidade.AutoCompleteUsuarioAtualizacao !== undefined
            ? Europa.Controllers.LogEntidade.AutoCompleteUsuarioAtualizacao.Value()
            : ""

    };
    console.log(params);
    return params;
};

Europa.Components.Datatable.LogEntidade.Filtrar = function() {
    Europa.Components.Datatable.LogEntidade.DataTable.reloadData();
};

Europa.Components.Datatable.LogEntidade.ExportarDatatable = function () {
    var params = Europa.Components.Datatable.LogEntidade.DataTable.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Components.Datatable.LogEntidade.UrlExportarDatatable);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Components.Datatable.LogEntidade.ExportarPagina = function () {
    var params = Europa.Components.Datatable.LogEntidade.DataTable.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Components.Datatable.LogEntidade.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};


Europa.Components.Datatable.LogEntidade.DetalharLog = function(id) {
    var data = Europa.Components.Datatable.LogEntidade.DataTable.getRowData(id);
    Europa.Controllers.LogEntidade.Modal.AbrirModal(data);
};
