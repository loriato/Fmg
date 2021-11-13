Europa.Controllers.ProgramaFidelidade = {};

$(function () {
    $("#corpo-conteudo-layout").css('padding-left', '64px');
});

Europa.Controllers.ProgramaFidelidade.DownloadPdf = function () {
    //location.href = Europa.Controllers.ProgramaFidelidade.UrlDownloadPdf;
    //var obj = Europa.Controllers.RecebimentoNF.CurrentRowData;
    var formDownload = $("#form_pdf");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.ProgramaFidelidade.UrlDownloadPdf);
    formDownload.submit();
}