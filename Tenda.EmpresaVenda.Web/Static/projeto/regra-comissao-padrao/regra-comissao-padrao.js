Europa.Controllers.RegraComissaoPadrao = {};
Europa.Controllers.RegraComissaoPadrao.Tabela = {};
Europa.Controllers.RegraComissaoPadrao.Permissoes = {};
Europa.Controllers.RegraComissaoPadrao.IdFormContratoCorretagem = "#form_contrato_corretagem";

$(function () {
    Europa.Controllers.RegraComissaoPadrao.RenderContratoCorretagem();
});

Europa.Controllers.RegraComissaoPadrao.AbrirModal = function () {
    $("#modal_inclusao_regra_comissao").modal("show");
};

Europa.Controllers.RegraComissaoPadrao.FecharModal = function () {
    $("#modal_inclusao_regra_comissao").modal("hide");
};

Europa.Controllers.RegraComissaoPadrao.RenderContratoCorretagem = function () {
    $.get(Europa.Controllers.RegraComissaoPadrao.UrlRenderContratoCorretagem, function (res) {
        $("#secao-contrato-corretagem").html(res.Objeto);
    });   
};

Europa.Controllers.RegraComissaoPadrao.DownloadContratoCorretagem = function () {
    var formDownload = $("#form_download_contrato_corretagem");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.RegraComissaoPadrao.UrlDownloadContratoCorretagem);
    formDownload.submit();
};