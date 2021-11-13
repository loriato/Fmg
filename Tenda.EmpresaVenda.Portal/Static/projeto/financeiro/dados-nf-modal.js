Europa.Controllers.Financeiro.UrlEspelhoNF = undefined;

Europa.Controllers.Financeiro.DownloadNFEspelho = function () {
    var obj = Europa.Controllers.Financeiro.CurrentRowData;
    var params = {
        PedidoSap: obj.PedidoSap,
        IdEmpresaVenda: obj.IdEmpresaVenda,
        IdEmpreendimento: obj.Filhos[0].IdEmpreendimento
    };
    var formDownload = $("#Exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.Financeiro.UrlDownloadPdf);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
};

Europa.Controllers.Financeiro.ExibirDadosNotaFiscal = function () {
    var obj = Europa.Controllers.Financeiro.CurrentRowData;
    var params = {
        PedidoSap: obj.PedidoSap,
        IdEmpresaVenda: obj.IdEmpresaVenda,
        IdEmpreendimento: obj.Filhos[0].IdEmpreendimento
    };
    $.ajax({
        type: 'POST',
        url: Europa.Controllers.Financeiro.UrlExibirDadosNotaFiscal,
        data: params,
        async: false,
        success: function (res) {
           Europa.Controllers.Financeiro.UrlEspelhoNF = res.Objeto.Url;
           Europa.Controllers.Financeiro.Exibir();
           $("#dados_nota_fiscal").modal("show");
        }
    });
};

Europa.Controllers.Financeiro.ExibirNotaFiscal = function () {
    var obj = Europa.Controllers.Financeiro.CurrentRowData;

    var params = {
        PedidoSap: obj.PedidoSap,
        IdEmpresaVenda: obj.IdEmpresaVenda,
        IdEmpreendimento: obj.Filhos[0].IdEmpreendimento
    }

    $.post(Europa.Controllers.Financeiro.UrlExibirNotaFiscal, params, function (res) {
        if (res.Sucesso) {
            $("#dados_nota_fiscal").modal("show");
            $("#nota_fiscal").html(res.Objeto);
        } else {
            Europa.Informacao.PosAcao(res);
        }
    })
}


Europa.Controllers.Financeiro.Exibir = function () {
    var PDFJS = window['pdfjs-dist/build/pdf']
    PDFJS.GlobalWorkerOptions.workerSrc = '//mozilla.github.io/pdf.js/build/pdf.worker.js';

    var loadingTask = PDFJS.getDocument(Europa.Controllers.Financeiro.UrlEspelhoNF);
    loadingTask.promise.then(function (pdf) {
        var __TOTAL_PAGES = pdf.numPages;
        // Fetch the first page
        var pageNumber = 1;
        for (let i = 1; i <= __TOTAL_PAGES; i += 1) {
            var id = 'the-canvas' + i;
            $('#div-pdf').append("<div style='text-align: center;' ><canvas class='the-canvas' id='" + id + "' style='box-shadow: 4px 4px 50px 0 rgba(0, 0, 0, 0.25); border: 2px var(--marine-blue) solid;'></canvas></div>");
            var canvas = document.getElementById(id);
            Europa.Controllers.Financeiro.RenderPage(canvas, pdf, pageNumber++, function pageRenderingComplete() {
                if (pageNumber > pdf.numPages) {
                    return;
                }
                // Continue rendering of the next page
                Europa.Controllers.Financeiro.RenderPage(canvas, pdf, pageNumber++, pageRenderingComplete);
            });
        }
    });
    
};

Europa.Controllers.Financeiro.RenderPage = function (canvas, pdf, pageNumber, callback) {
    pdf.getPage(pageNumber).then(function (page) {
        var scale = 1.5;
        var viewport = page.getViewport({ scale: scale });
        var pageDisplayWidth = viewport.width;
        var pageDisplayHeight = viewport.height;
        var context = canvas.getContext('2d');
        canvas.width = pageDisplayWidth;
        canvas.height = pageDisplayHeight;
        var renderContext = {
            canvasContext: context,
            viewport: viewport
        };
        page.render(renderContext).promise.then(callback);
    });
}; 


function detalharServicoPorCliente(tgt) {
    console.log($(tgt));
    if ($(tgt).parent().next().css("display") == "none") {
        $(tgt).parent().next().css("display", "block");
        tgt.innerHTML = "";
        tgt.innerHTML = '<img class="triangle" style="vertical-align: middle;" onclick="detalharServicoPorCliente(event.target.parentNode);" src="~/../Static/images/caret-up.png">Detalhes';
        //$(tgt).first().addClass("upside-down")
    } else {
        $(tgt).parent().next().css("display", "none");
        //$(tgt).first().removeClass("upside-down")

        tgt.innerHTML = "";
        tgt.innerHTML = '<img class="triangle" style="vertical-align: middle;" onclick="detalharServicoPorCliente(event.target.parentNode);" src="~/../Static/images/caret-down.png">Detalhes';
    }
}