Europa.Controllers.RegraComissao = {};
Europa.Controllers.RegraComissao.Tabela = {};
Europa.Controllers.RegraComissao.Permissoes = {};
Europa.Controllers.RegraComissao.IdFormContratoCorretagem = "#form_contrato_corretagem";

$(function () {
    Europa.Controllers.RegraComissao.RenderContratoCorretagem();
});

Europa.Controllers.RegraComissao.AbrirModal = function () {
    $("#modal_inclusao_regra_comissao").modal("show");
};

Europa.Controllers.RegraComissao.FecharModal = function () {
    $("#modal_inclusao_regra_comissao").modal("hide");
};

Europa.Controllers.RegraComissao.RenderContratoCorretagem = function () {
    $.get(Europa.Controllers.RegraComissao.UrlRenderContratoCorretagem, function (res) {
        $("#secao-contrato-corretagem").html(res.Objeto);
    });   
};

Europa.Controllers.RegraComissao.DownloadContratoCorretagem = function () {
    var formDownload = $("#form_download_contrato_corretagem");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.RegraComissao.UrlDownloadContratoCorretagem);
    formDownload.submit();
};