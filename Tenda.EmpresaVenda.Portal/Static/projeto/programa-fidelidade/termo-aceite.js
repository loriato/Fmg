Europa.Controllers.TermoAceite = {}
Europa.Controllers.TermoAceite.UrlTermoAceite = undefined

$(function () {
    Europa.Controllers.TermoAceite.UrlTermoAceite = $('#TermoAceite').val();
    Europa.Controllers.TermoAceite.ExibirTermoAceite();
});


Europa.Controllers.TermoAceite.Download = function () {
    Europa.Components.DownloadFile(Europa.Controllers.TermoAceite.UrlDownloadTermoAceite);
};

Europa.Controllers.TermoAceite.ExibirTermoAceite = function () {
    $('#div-pdf').html("");
    var pdfjsLib = window['pdfjs-dist/build/pdf'];
    pdfjsLib.GlobalWorkerOptions.workerSrc = '//mozilla.github.io/pdf.js/build/pdf.worker.js';
    var loadingTask = pdfjsLib.getDocument(Europa.Controllers.TermoAceite.UrlTermoAceite);
    loadingTask.promise.then(function (pdf) {
        var __TOTAL_PAGES = pdf.numPages;
        // Fetch the first page
        var pageNumber = 1;
        for (let i = 1; i <= __TOTAL_PAGES; i += 1) {
            var id = 'the-canvas' + i;
            $('#div-pdf').append("<div style='text-align: center;' ><canvas class='the-canvas' id='" + id + "'></canvas></div>");
            var canvas = document.getElementById(id);
            Europa.Controllers.TermoAceite.RenderPage(canvas, pdf, pageNumber++, function pageRenderingComplete() {
                if (pageNumber > pdf.numPages) {
                    return;
                }
                // Continue rendering of the next page
                Europa.Controllers.TermoAceite.RenderPage(canvas, pdf, pageNumber++, pageRenderingComplete);
            });
        }
    });
};

Europa.Controllers.TermoAceite.RenderPage = function (canvas, pdf, pageNumber, callback) {
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