Europa.Controllers.HierarquiaCoordenador = {};

$(function () {

});


Europa.Controllers.HierarquiaCoordenador.ExportarSelecionadosSupervisor = function () {

    var params = Europa.Controllers.HierarquiaCoordenador.FiltroSupervisor();
    params.order = Europa.Controllers.HierarquiaCoordenador.SupervisorTable.lastRequestParams.order;
    params.draw = Europa.Controllers.HierarquiaCoordenador.SupervisorTable.lastRequestParams.draw;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.HierarquiaCoordenador.UrlExportarSelecionadosSupervisor);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.HierarquiaCoordenador.ExportarSelecionadosViabilizador = function () {

    var params = Europa.Controllers.HierarquiaCoordenador.FiltroViabilizador();
    params.order = Europa.Controllers.HierarquiaCoordenador.ViabilizadorTable.lastRequestParams.order;
    params.draw = Europa.Controllers.HierarquiaCoordenador.ViabilizadorTable.lastRequestParams.draw;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.HierarquiaCoordenador.UrlExportarSelecionadosViabilizador);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.HierarquiaCoordenador.ExportarTodos = function () {

    var params = Europa.Controllers.HierarquiaCoordenador.FiltroViabilizador();
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.HierarquiaCoordenador.UrlExportarTudo);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};