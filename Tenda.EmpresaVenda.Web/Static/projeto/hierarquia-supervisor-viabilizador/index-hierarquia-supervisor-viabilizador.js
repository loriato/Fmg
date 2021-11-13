Europa.Controllers.HierarquiaSupervisorViabilizador = {};
Europa.Controllers.HierarquiaSupervisorViabilizador.SupervisorId = undefined;
Europa.Controllers.HierarquiaSupervisorViabilizador.SupervisorTable = undefined;
Europa.Controllers.HierarquiaSupervisorViabilizador.ViabilizadorTable = undefined;

$(function () {

})

Europa.Controllers.HierarquiaSupervisorViabilizador.ExportarSelecionados = function () {

    var params = Europa.Controllers.HierarquiaSupervisorViabilizador.FiltroViablilizador();
    params.order = Europa.Controllers.HierarquiaSupervisorViabilizador.ViabilizadorTable.lastRequestParams.order;
    params.draw = Europa.Controllers.HierarquiaSupervisorViabilizador.ViabilizadorTable.lastRequestParams.draw;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.HierarquiaSupervisorViabilizador.UrlExportarSelecionados);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.HierarquiaSupervisorViabilizador.ExportarTudo = function () {

    var params = Europa.Controllers.HierarquiaSupervisorViabilizador.FiltroViablilizador();
    params.order = Europa.Controllers.HierarquiaSupervisorViabilizador.ViabilizadorTable.lastRequestParams.order;
    params.draw = Europa.Controllers.HierarquiaSupervisorViabilizador.ViabilizadorTable.lastRequestParams.draw;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.HierarquiaSupervisorViabilizador.UrlExportarTudo);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};