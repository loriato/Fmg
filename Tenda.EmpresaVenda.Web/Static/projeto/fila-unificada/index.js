Europa.Controllers.FilaUnificada = {};

$(function () {
    
});

Europa.Controllers.FilaUnificada.ExportarPagina = function () {
    var params = Europa.Controllers.FilaUnificada.Filtro();
    params.order = Europa.Controllers.FilaUnificada.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.FilaUnificada.Tabela.lastRequestParams.draw;
    params.pageSize = Europa.Controllers.FilaUnificada.Tabela.lastRequestParams.pageSize;
    params.start = Europa.Controllers.FilaUnificada.Tabela.lastRequestParams.start;
    var formExportar = $("#form_exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.FilaUnificada.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.FilaUnificada.ExportarTodos = function () {
    var params = Europa.Controllers.FilaUnificada.Filtro();
    params.order = Europa.Controllers.FilaUnificada.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.FilaUnificada.Tabela.lastRequestParams.draw;
    var formExportar = $("#form_exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.FilaUnificada.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};