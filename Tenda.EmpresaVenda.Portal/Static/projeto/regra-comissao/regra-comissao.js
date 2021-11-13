Europa.Controllers.RegraComissao = {};
Europa.Controllers.RegraComissao.Url = undefined;
Europa.Controllers.RegraComissao.CheckBox = true;
Europa.Controllers.RegraComissao.CheckCampanha = undefined;
Europa.Controllers.RegraComissao.Tipo = undefined;
Europa.Controllers.RegraComissao.DoubleCheckBox = true;

$(document).ready(function () {
    var select = $('#IdRegraAceiteMaisRecente');

    if (select === undefined) { return; }
    $('#IdRegraAceiteMaisRecente').on('change', function () {
        var params = {
            IdRegraComissaoEvs: $('#IdRegraAceiteMaisRecente').children("option:selected").val()
        };
        $.ajax({
            type: 'POST',
            url: Europa.Controllers.RegraComissao.UrlArquivo,
            data: params,
            beforeSend: function () {
                Spinner.Show();
            },
            error: function () {
                Spinner.Hide();
            },
            success: function (result, text, response) {
                Europa.Controllers.RegraComissao.Url = result.RegraComissao;
                Europa.Controllers.RegraComissao.ExibirRegraComissao();
                Spinner.Hide();
            }
        });
    });

    Europa.Controllers.RegraComissao.CheckCampanha = $("#camp_check");
    Europa.Controllers.RegraComissao.Tipo = $("#RegraComissaoEvs_Tipo").val();

    if (Europa.Controllers.RegraComissao.Tipo == "Campanha") {
        Europa.Controllers.RegraComissao.CheckCampanha.removeClass("hidden");
        $("#camp_aceite_flag").css("display", "none");
        Europa.Controllers.RegraComissao.TextAceiteCampanha();
    };

});

Europa.Controllers.RegraComissao.ExibirRegraComissao = function () {
    $('#div-pdf').html("");
    var pdfjsLib = window['pdfjs-dist/build/pdf'];
    pdfjsLib.GlobalWorkerOptions.workerSrc = '//mozilla.github.io/pdf.js/build/pdf.worker.js';
    var loadingTask = pdfjsLib.getDocument(Europa.Controllers.RegraComissao.Url);
    loadingTask.promise.then(function (pdf) {
        var __TOTAL_PAGES = pdf.numPages;
        // Fetch the first page
        var pageNumber = 1;
        for (let i = 1; i <= __TOTAL_PAGES; i += 1) {
            var id = 'the-canvas' + i;
            $('#div-pdf').append("<div style='text-align: center;' ><canvas class='the-canvas' id='" + id + "'></canvas></div>");
            var canvas = document.getElementById(id);
            Europa.Controllers.RegraComissao.RenderPage(canvas, pdf, pageNumber++, function pageRenderingComplete() {
                if (pageNumber > pdf.numPages) {
                    return;
                }
                // Continue rendering of the next page
                Europa.Controllers.RegraComissao.RenderPage(canvas, pdf, pageNumber++, pageRenderingComplete);
            });
        }
    });
};


Europa.Controllers.RegraComissao.CarregarRegraComissao = function () {
    var pdfjsLib = window['pdfjs-dist/build/pdf'];
    pdfjsLib.GlobalWorkerOptions.workerSrc = '//mozilla.github.io/pdf.js/build/pdf.worker.js';
    var loadingTask = pdfjsLib.getDocument(Europa.Controllers.RegraComissao.Url);
    loadingTask.promise.then(function (pdf) {
        var __TOTAL_PAGES = pdf.numPages;
        // Fetch the first page
        var pageNumber = 1;
        for (let i = 1; i <= __TOTAL_PAGES; i += 1) {
            var id = 'the-canvas' + i;
            $('#div-pdf').append("<div style='text-align: center;' ><canvas class='the-canvas' id='" + id + "'></canvas></div>");
            var canvas = document.getElementById(id);
            Europa.Controllers.RegraComissao.RenderPage(canvas, pdf, pageNumber++, function pageRenderingComplete() {
                if (pageNumber > pdf.numPages) {
                    return;
                }
                // Continue rendering of the next page
                Europa.Controllers.RegraComissao.RenderPage(canvas, pdf, pageNumber++, pageRenderingComplete);
            });
        }
    });
};

Europa.Controllers.RegraComissao.FimPdf = function (o) {
    if ((o.offsetHeight + o.scrollTop + 50) > o.scrollHeight) {
        Europa.Controllers.RegraComissao.HabilitarCheckBox();
    }
};

Europa.Controllers.RegraComissao.RenderPage = function (canvas, pdf, pageNumber, callback) {
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

Europa.Controllers.RegraComissao.HabilitarBtn = function () {
    
    if (Europa.Controllers.RegraComissao.CheckBox) {
        $("#aceite_flag").prop("checked", true);
        Europa.Controllers.RegraComissao.CheckBox = false;
    } else {
        Europa.Controllers.RegraComissao.CheckBox = true;
        $("#aceite_flag").prop("checked", false);
        Europa.Controllers.RegraComissao.DesabilitarAceite();
    }

    if ($('#aceite_flag').is(":checked")) {
        $("#check_false").css("display", "none");
        $("#check_true").css("display", "unset");
        Europa.Controllers.RegraComissao.HabilitarDoubleCheckBox();
    } else {
        Europa.Controllers.RegraComissao.DesabilitarAceite();
        $("#check_false").css("display", "unset");
        $("#check_true").css("display", "none");
        $("#btn_aceitar").prop("onclick", null).off("click");
        $("#btn_aceitar").attr("disabled", "disabled");
        $("#btn_aceitar").css("background-color", "#d1d1d1");
    }
};

Europa.Controllers.RegraComissao.DesabilitarAceite = function () {
    $("#camp_check_false").attr("disabled", "disabled");
    $("#camp_check_false").removeAttr("onclick");
    $("#camp_check_true").css("display", "none");
    $("#camp_check_false").css("display", "unset");
    $("#camp_check_false").css("opacity", "0.5");
    $("#camp-text-aceite-termos").removeAttr("onclick");
    $("#camp-text-aceite-termos").css("opacity", "0.5");
};

Europa.Controllers.RegraComissao.HabilitarAceite = function () {
    if (Europa.Controllers.RegraComissao.DoubleCheckBox) {
        $("#camp_aceite_flag").prop("checked", true);
        Europa.Controllers.RegraComissao.DoubleCheckBox = false;
    } else {
        Europa.Controllers.RegraComissao.DoubleCheckBox = true;
        $("#camp_aceite_flag").prop("checked", false);
    }

    if ($('#camp_aceite_flag').is(":checked")) {
        $("#camp_check_false").css("display", "none");
        $("#camp_check_true").css("display", "unset");
        $("#btn_aceitar").removeAttr("disabled");
        $("#btn_aceitar").on("click", Europa.Controllers.RegraComissao.Avancar);
        $("#btn_aceitar").css("background-color", "#ec1d24");
    } else {
        $("#camp_check_false").css("display", "unset");
        $("#camp_check_true").css("display", "none");
        $("#btn_aceitar").prop("onclick", null).off("click");
        $("#btn_aceitar").attr("disabled", "disabled");
        $("#btn_aceitar").css("background-color", "#d1d1d1");
    }
};

Europa.Controllers.RegraComissao.HabilitarCheckBox = function () {
    $("#check_false").removeAttr("disabled");
    $("#check_false").css("opacity", "1");
    $("#check_false").css("cursor", "pointer");
    $("#check_false").attr("onclick", "Europa.Controllers.RegraComissao.HabilitarBtn()");
    $("#text-aceite-termos").attr("onclick", "Europa.Controllers.RegraComissao.HabilitarBtn()");
    $("#text-aceite-termos").css("opacity", "1");
};

Europa.Controllers.RegraComissao.HabilitarDoubleCheckBox = function () {
    if (Europa.Controllers.RegraComissao.Tipo == "Campanha") {
        $("#camp_check_false").removeAttr("disabled");
        $("#camp_check_false").css("opacity", "1");
        $("#camp_check_false").css("cursor", "pointer");
        $("#camp_check_false").attr("onclick", "Europa.Controllers.RegraComissao.HabilitarAceite()");
        $("#camp-text-aceite-termos").attr("onclick", "Europa.Controllers.RegraComissao.HabilitarAceite()");
        $("#camp-text-aceite-termos").css("opacity", "1");       

    } else {
        $("#btn_aceitar").removeAttr("disabled");
        $("#btn_aceitar").on("click", Europa.Controllers.RegraComissao.Avancar);
        $("#btn_aceitar").css("background-color", "#ec1d24");
    }
};

Europa.Controllers.RegraComissao.TextAceiteCampanha = function () {
    var descricao = $("#RegraComissaoEvs_Descricao").val();
    var termino = $("#RegraComissaoEvs_TerminoVigencia").val();

    $.get(Europa.Controllers.RegraComissao.UrlRegraEvSuspensa, function (res) {
        if (res.Sucesso) {
            var msg = 'Importante! a Campanha "{0}" tem tempo pré - determinado e estará em vigor até o dia "{1}" após está data a tabela de comissão anterior "{2}" voltará a ser a tabela vigente.'
            msg = msg.replace("{0}", descricao);
            msg = msg.replace("{1}", termino);
            msg = msg.replace("{2}", res.Objeto);
            //console.log(msg)
        }
    });
};

Europa.Controllers.RegraComissao.Avancar = function () {    
    $('#form_aceitar').submit();
};

Europa.Controllers.RegraComissao.Download = function () {
    var params = {
        IdRegraComissaoEvs: $('#IdRegraAceiteMaisRecente').children("option:selected").val()
    };
    Europa.Components.DownloadFile(Europa.Controllers.RegraComissao.UrlDownloadRegraComissao, params);
};