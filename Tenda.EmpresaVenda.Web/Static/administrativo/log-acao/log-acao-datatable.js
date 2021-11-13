Europa.Components.Datatable.LogAcao = {};
Europa.Components.Datatable.LogAcao.createDT = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

    Europa.Components.Datatable.LogAcao.DataTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);

    Europa.Components.Datatable.LogAcao.DataTable
        .setIdAreaHeader("LogAcaoDatatable_barra")
        .setColumns([
            DTColumnBuilder.newColumn('CriadoEm').withTitle(Europa.i18n.Messages.Momento).withOption("type", "date-format-DD/MM/YYYY HH:mm:ss").withOption('width', '10%'),
            DTColumnBuilder.newColumn('NomeSistema').withTitle(Europa.i18n.Messages.Sistema).withOption('width', '10%'),
            DTColumnBuilder.newColumn('NomeUnidadeFuncional').withTitle(Europa.i18n.Messages.UnidadeFuncional).withOption('width', '20%'),
            DTColumnBuilder.newColumn('NomeFuncionalidade').withTitle(Europa.i18n.Messages.Funcionalidade).withOption('width', '20%'),
            DTColumnBuilder.newColumn('NomeUsuario').withTitle(Europa.i18n.Messages.Usuario).withOption('width', '10%'),
            DTColumnBuilder.newColumn('IdAcesso').withTitle(Europa.i18n.Messages.Acesso).withOption('width', '10%'),
            DTColumnBuilder.newColumn('NomePerfil').withTitle(Europa.i18n.Messages.Perfil).withOption('width', '13%')
        ])
        .setColActions(actionsHtml, '7%')
        .setDefaultOrder([[5, 'desc']])
        .setAutoInit(false)
        .setOptionsSelect('POST', Europa.Components.Datatable.LogAcao.listAction, Europa.Components.Datatable.LogAcao.filterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>'
            + Europa.Components.Datatable.LogAcao.DataTable.renderButton(true, Europa.i18n.Messages.Detalhar, 'fa fa-eye', 'detalharLog(' + meta.row + ')')
            + '</div>';
    };
    $scope.detalharLog = function(id) {
        Europa.Components.Datatable.LogAcao.DetalharLog(id);
    }
};

DataTableApp.controller('LogAcaoDataTable', Europa.Components.Datatable.LogAcao.createDT);


Europa.Components.Datatable.LogAcao.filterParams = function() {
    var params = {
        DataInicio: $("#form_filtro_log_acao").find("#DataInicio").val(),
        DataFim: $("#form_filtro_log_acao").find("#DataFim").val(),
        IdUsuario: Europa.Controllers.LogAcao.AutoCompleteUsuario !== undefined
            ? Europa.Controllers.LogAcao.AutoCompleteUsuario.Value()
            : "",
        Sistema: $("#form_filtro_log_acao").find("#Sistema").val(),
        IdFuncionalidade: Europa.Controllers.LogAcao.AutoCompleteFuncionalidade !== undefined
            ? Europa.Controllers.LogAcao.AutoCompleteFuncionalidade.Value()
            : "",
        IdUnidadeFuncional: Europa.Controllers.LogAcao.AutoCompleteUnidadeFuncional !== undefined
            ? Europa.Controllers.LogAcao.AutoCompleteUnidadeFuncional.Value()
            : ""
    };
    return params;
};

Europa.Components.Datatable.LogAcao.Filtrar = function() {
    Europa.Components.Datatable.LogAcao.DataTable.reloadData();
};

Europa.Components.Datatable.LogAcao.ExportarDatatable = function () {
    var params = Europa.Components.Datatable.LogAcao.DataTable.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Components.Datatable.LogAcao.UrlExportarDatatable);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Components.Datatable.LogAcao.ExportarPagina = function () {
    var params = Europa.Components.Datatable.LogAcao.DataTable.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Components.Datatable.LogAcao.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};


Europa.Components.Datatable.LogAcao.DetalharLog = function (id) {
    var data = Europa.Components.Datatable.LogAcao.DataTable.getRowData(id);
    Europa.Controllers.LogAcao.Modal.AbrirModal(data);
};
