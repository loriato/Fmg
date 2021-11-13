Europa.Controllers.TermoAceite = {};
Europa.Controllers.TermoAceite.UrlTermoAceite = undefined
Europa.Controllers.TermoAceite.CheckBox = true;

$(function () {
    Europa.Controllers.TermoAceite.UrlTermoAceite = $('#TermoAceite').val();
    Europa.Controllers.TermoAceite.ExibirTermoAceite();
});



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

Europa.Controllers.TermoAceite.FimPdf = function (o) {
    if ((o.offsetHeight + o.scrollTop + 50) > o.scrollHeight) {
        Europa.Controllers.TermoAceite.HabilitarCheckBox();
    }
};

Europa.Controllers.TermoAceite.HabilitarCheckBox = function () {
    $("#check_false").removeAttr("disabled");
    $("#check_false").css("opacity", "1");
    $("#check_false").css("cursor", "pointer");
    $("#check_false").attr("onclick", "Europa.Controllers.TermoAceite.HabilitarBtn()");
    $("#text-aceite-termos").attr("onclick", "Europa.Controllers.TermoAceite.HabilitarBtn()");
    $("#text-aceite-termos").css("opacity", "1");
};

Europa.Controllers.TermoAceite.HabilitarBtn = function () {
    if (Europa.Controllers.TermoAceite.CheckBox) {
        $("#aceite_flag").prop("checked", true);
        Europa.Controllers.TermoAceite.CheckBox = false;
    } else {
        Europa.Controllers.TermoAceite.CheckBox = true;
        $("#aceite_flag").prop("checked", false);
    }

    if ($('#aceite_flag').is(":checked")) {
        $("#check_false").css("display", "none");
        $("#check_true").css("display", "unset");
        $("#btn_aceitar").removeAttr("disabled");
        //$("#btn_aceitar").on("click", Europa.Controllers.TermoAceite.Avancar);
        $("#btn_aceitar").css("background-color", "#ec1d24");
    } else {
        $("#check_false").css("display", "unset");
        $("#check_true").css("display", "none");
        $("#btn_aceitar").prop("onclick", null).off("click");
        $("#btn_aceitar").attr("disabled", "disabled");
        $("#btn_aceitar").css("background-color", "#d1d1d1");
    }
};

Europa.Controllers.TermoAceite.Avancar = function () {
    window.location = Europa.Controllers.TermoAceite.UrlAvancar;
}