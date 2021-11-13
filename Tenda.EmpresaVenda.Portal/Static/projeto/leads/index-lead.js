Europa.Controllers.Leads = {};

$(function () {
});

Europa.Controllers.Leads.ExportarPaginaDiretor = function () {

    var autorizar = Europa.Controllers.Leads.ValidarFiltroDiretor();

    if (!autorizar) {
        return;
    }

    Europa.Controllers.Leads.FiltrarDatatableDiretor();

    var params = Europa.Controllers.Leads.TabelaDiretor.lastRequestParams;

    var formDownload = $("#form_exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.Leads.UrlExportarPaginaDiretor);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
};

Europa.Controllers.Leads.ExportarTodosDiretor = function () {

    var autorizar = Europa.Controllers.Leads.ValidarFiltroDiretor();

    if (!autorizar) {
        return;
    }

    Europa.Controllers.Leads.FiltrarDatatableDiretor();

    var params = Europa.Controllers.Leads.TabelaDiretor.lastRequestParams;

    var formDownload = $("#form_exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.Leads.UrlExportarTodosDiretor);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
};

Europa.Controllers.Leads.ExportarPaginaCorretor = function () {

    Europa.Controllers.Leads.FiltrarDatatableCorretor();

    var params = Europa.Controllers.Leads.TabelaCorretor.lastRequestParams;

    var formDownload = $("#form_exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.Leads.UrlExportarPaginaCorretor);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
};

Europa.Controllers.Leads.ExportarTodosCorretor = function () {

    Europa.Controllers.Leads.FiltrarDatatableCorretor();

    var params = Europa.Controllers.Leads.TabelaCorretor.lastRequestParams;

    var formDownload = $("#form_exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.Leads.UrlExportarTodosCorretor);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
};