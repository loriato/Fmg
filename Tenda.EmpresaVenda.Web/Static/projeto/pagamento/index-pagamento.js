Europa.Controllers.Pagamento = {};
Europa.Controllers.Pagamento.PedidoSap = "";
Europa.Controllers.Pagamento.Records = [];
Europa.Controllers.Pagamento.Tabelas = [];
Europa.Controllers.Pagamento.ListaPagamentos = [];

$(function () {
    
});

Europa.Controllers.Pagamento.Validar = function () {
    var autorizar = Europa.Controllers.Pagamento.ValidarFiltro();

    if (!autorizar) {
        return;
    }

    var params = Europa.Controllers.Pagamento.Filtro();

    $.post(Europa.Controllers.Pagamento.UrlValidarFiltro, params, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.Pagamento.ExportarPagina();
        } else {
            Europa.Informacao.PosAcao(res);
        }
    });
}

Europa.Controllers.Pagamento.ExportarPagina = function () {

    var autorizar = Europa.Controllers.Pagamento.ValidarFiltro();

    if (!autorizar) {
        return;
    }

    var params = Europa.Controllers.Pagamento.Filtro();    
        
    var formDownload = $("#form_exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.Pagamento.UrlExportarPagina);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
};

Europa.Controllers.Pagamento.ExportarTodos = function () {

    var autorizar = Europa.Controllers.Pagamento.ValidarFiltro();

    if (!autorizar) {
        return;
    }

    var params = Europa.Controllers.Pagamento.Filtro();
    var formDownload = $("#form_exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.Pagamento.UrlExportarTodos);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
};

Europa.Controllers.Pagamento.ExportarTodasEvs = function () {

    var msgs = [];

    var autorizar = true;

    if ($("#DateInicioVigencia").val() == 0 ||
        $("#DataTerminoVigencia").val() == 0) {
        msgs.push("Insira um período");
        autorizar = false;
    }
    
    if (!autorizar) {
        var res = {
            Sucesso: false,
            Mensagens: msgs
        };

        Europa.Informacao.PosAcao(res);

        return ;
    }

    var params = Europa.Controllers.Pagamento.Filtro();
    var formDownload = $("#form_exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.Pagamento.UrlExportarTodasEvs);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
};
