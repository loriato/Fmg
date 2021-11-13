Europa.Controllers.RelatorioAcessos = {};

Europa.Controllers.RelatorioAcessos.Filtro = function () {
    var param = {
        DataInicio: $("#DataInicio").val(),
        DataFim: $("#DataFim").val()
    };

    return param;
};
Europa.Controllers.RelatorioAcessos.Exportar = function () {
    if ($("#DataInicio").val() == 0 ||
        $("#DataFim").val() == 0) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, "Insira um Período");
        Europa.Informacao.Show();
        return null;
    };
    var formExportar = $("#Exportar");
    var param = Europa.Controllers.RelatorioAcessos.Filtro();
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.RelatorioAcessos.UrlExportar);
    formExportar.addHiddenInputData(param);
    formExportar.submit();
};