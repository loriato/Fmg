Europa.Controllers.PoliticaPrivacidade = {};

$(function () {
    console.log(Europa.Controllers.PoliticaPrivacidade.UrlPoliticaPrivacidade)
});

Europa.Controllers.PoliticaPrivacidade.CarregarPoliticaPrivacidade = function () {
    var pdfjsLib = window['pdfjs-dist/build/pdf'];
    pdfjsLib.GlobalWorkerOptions.workerSrc = '//mozilla.github.io/pdf.js/build/pdf.worker.js';
    var loadingTask = pdfjsLib.getDocument(Europa.Controllers.PoliticaPrivacidade.UrlPoliticaPrivacidade);
    loadingTask.promise.then(function (pdf) {
        var __TOTAL_PAGES = pdf.numPages;
        // Fetch the first page
        var pageNumber = 1;
        for (let i = 1; i <= __TOTAL_PAGES; i += 1) {
            var id = 'the-canvas' + i;
            $('#div-pdf').append("<div style='text-align: center;' ><canvas class='the-canvas' id='" + id + "'></canvas></div>");
            var canvas = document.getElementById(id);
            Europa.Controllers.PoliticaPrivacidade.RenderPage(canvas, pdf, pageNumber++, function pageRenderingComplete() {
                if (pageNumber > pdf.numPages) {
                    return;
                }
                // Continue rendering of the next page
                Europa.Controllers.PoliticaPrivacidade.RenderPage(canvas, pdf, pageNumber++, pageRenderingComplete);
            });
        }
    });
};

Europa.Controllers.PoliticaPrivacidade.RenderPage = function (canvas, pdf, pageNumber, callback) {
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