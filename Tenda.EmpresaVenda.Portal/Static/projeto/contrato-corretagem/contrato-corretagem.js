Europa.Controllers.ContratoCorretagem = {};
Europa.Controllers.ContratoCorretagem.Url = undefined;
Europa.Controllers.ContratoCorretagem.CheckBox = true;


Europa.Controllers.ContratoCorretagem.ExibirContrato = function () {
    var pdfjsLib = window['pdfjs-dist/build/pdf'];
    pdfjsLib.GlobalWorkerOptions.workerSrc = '//mozilla.github.io/pdf.js/build/pdf.worker.js';
    var loadingTask = pdfjsLib.getDocument(Europa.Controllers.ContratoCorretagem.Url);
    loadingTask.promise.then(function (pdf) {
        var __TOTAL_PAGES = pdf.numPages;
        // Fetch the first page
        var pageNumber = 1;
        for (let i = 1; i <= __TOTAL_PAGES; i += 1) {
            var id = 'the-canvas' + i;
            $('#div-pdf').append("<div style='text-align: center;' ><canvas class='the-canvas' id='" + id + "'></canvas></div>");
            var canvas = document.getElementById(id);
            Europa.Controllers.ContratoCorretagem.RenderPage(canvas, pdf, pageNumber++, function pageRenderingComplete() {
                if (pageNumber > pdf.numPages) {
                    return;
                }
                // Continue rendering of the next page
                Europa.Controllers.ContratoCorretagem.RenderPage(canvas, pdf, pageNumber++, pageRenderingComplete);
            });
        }
    });
};


Europa.Controllers.ContratoCorretagem.CarregarContrato = function () {
    var pdfjsLib = window['pdfjs-dist/build/pdf'];
    pdfjsLib.GlobalWorkerOptions.workerSrc = '//mozilla.github.io/pdf.js/build/pdf.worker.js';
    var loadingTask = pdfjsLib.getDocument(Europa.Controllers.ContratoCorretagem.Url);
    loadingTask.promise.then(function (pdf) {
        var __TOTAL_PAGES = pdf.numPages;
        // Fetch the first page
        var pageNumber = 1;
        for (let i = 1; i <= __TOTAL_PAGES; i += 1) {
            var id = 'the-canvas' + i;
            $('#div-pdf').append("<div style='text-align: center;' ><canvas class='the-canvas' id='" + id + "'></canvas></div>");
            var canvas = document.getElementById(id);
            Europa.Controllers.ContratoCorretagem.RenderPage(canvas, pdf, pageNumber++, function pageRenderingComplete() {
                if (pageNumber > pdf.numPages) {
                    return;
                }
                // Continue rendering of the next page
                Europa.Controllers.ContratoCorretagem.RenderPage(canvas, pdf, pageNumber++, pageRenderingComplete);
            });
        }
    });
};

Europa.Controllers.ContratoCorretagem.RenderPage = function (canvas, pdf, pageNumber, callback) {
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

Europa.Controllers.ContratoCorretagem.CheckBoxConfig = function (opt) {
    if (opt == 1) {
        console.log("op1");
        $("#check_false").css("display", "none");
        $("#check_true").css("display", "unset");
    } else {
        $("#check_false").css("display", "unset");
        $("#check_true").css("display", "none");
    }
};

$(document).ready(function () {
    $("#exibir_pdf").attr("src", Europa.Controllers.ContratoCorretagem.Url);
});

Europa.Controllers.ContratoCorretagem.FimPdf = function (o) {
    if ((o.offsetHeight + o.scrollTop + 50) > o.scrollHeight) {
        Europa.Controllers.ContratoCorretagem.HabilitarCheckBox();
    }
};

Europa.Controllers.ContratoCorretagem.HabilitarCheckBox = function () {
    $("#check_false").removeAttr("disabled");
    $("#check_false").css("opacity", "1");
    $("#check_false").css("cursor", "pointer");
    $("#check_false").attr("onclick", "Europa.Controllers.ContratoCorretagem.HabilitarBtn()");
    $("#text-aceite-termos").attr("onclick", "Europa.Controllers.ContratoCorretagem.HabilitarBtn()");
    $("#text-aceite-termos").css("opacity", "1");
};

Europa.Controllers.ContratoCorretagem.HabilitarBtn = function () {
    if (Europa.Controllers.ContratoCorretagem.CheckBox) {
        $("#aceite_flag").prop("checked", true);
        Europa.Controllers.ContratoCorretagem.CheckBox = false;
    } else {
        Europa.Controllers.ContratoCorretagem.CheckBox = true;
        $("#aceite_flag").prop("checked", false);
    }
    if ($('#aceite_flag').is(":checked")) {
        $("#check_false").css("display", "none");
        $("#check_true").css("display", "unset");
        $("#btn_aceitar").removeAttr("disabled");
        $("#btn_aceitar").on("click", Europa.Controllers.ContratoCorretagem.Avancar);
        $("#btn_aceitar").css("background-color", "#ec1d24");
    } else {
        $("#check_false").css("display", "unset");
        $("#check_true").css("display", "none");
        $("#btn_aceitar").prop("onclick", null).off("click");
        $("#btn_aceitar").attr("disabled", "disabled");
        $("#btn_aceitar").css("background-color", "#d1d1d1");
    }
};

Europa.Controllers.ContratoCorretagem.Avancar = function () {
    $('#form_aceitar').submit();
};


Europa.Controllers.ContratoCorretagem.Download = function () {
    Europa.Components.DownloadFile(Europa.Controllers.ContratoCorretagem.UrlDownloadContrato);
}