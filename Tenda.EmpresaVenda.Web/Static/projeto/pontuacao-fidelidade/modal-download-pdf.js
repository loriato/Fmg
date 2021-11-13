Europa.Controllers.ModalDownloadPdfPontuacaoFidelidade = {
    Url: {
        ListarEmpresasVendas: "",
        DownloadPdfPontuacaoFidelidade: "",
        DownloadPdfPontuacaoFidelidadeEvs: ""
    },
};

$(function () {
});

Europa.Controllers.ModalDownloadPdfPontuacaoFidelidade.Abrir = function (idPontuacaoFidelidade) {
    Europa.Controllers.ModalDownloadPdfPontuacaoFidelidade.IdPontuacaoFidelidade = idPontuacaoFidelidade;
    $.get(Europa.Controllers.ModalDownloadPdfPontuacaoFidelidade.Url.ListarEmpresasVendas, { idPontuacaoFidelidade: idPontuacaoFidelidade }, function (res) {
        var html = "<option value='0'>" + Europa.i18n.Messages.Todos + "</option>";
        for (var i = 0; i < res.length; i++) {
            var item = res[i];
            html = html + "<option value='" + item.Id + "'>" + item.NomeFantasia + "</option>"
        }
        $("#empresa_vendas_select", "#modalDownloadPdfPontuacaoFidelidade").html(html);
        $("#modalDownloadPdfPontuacaoFidelidade").show();
    });
};

Europa.Controllers.ModalDownloadPdfPontuacaoFidelidade.Fechar = function () {
    $("#empresa_vendas_select", "#modalDownloadPdfPontuacaoFidelidade").html("");
    $("#modalDownloadPdfPontuacaoFidelidade").hide();
};

Europa.Controllers.ModalDownloadPdfPontuacaoFidelidade.Confirmar = function () {
    var selecionado = parseInt($("#empresa_vendas_select", "#modalDownloadPdfPontuacaoFidelidade").val());
    if (selecionado === 0) {
        location.href = Europa.Controllers.ModalDownloadPdfPontuacaoFidelidade.Url.DownloadPdfPontuacaoFidelidade + "?idPontuacaoFidelidade="
            + Europa.Controllers.ModalDownloadPdfPontuacaoFidelidade.IdPontuacaoFidelidade;
    } else {
        location.href = Europa.Controllers.ModalDownloadPdfPontuacaoFidelidade.Url.DownloadPdfPontuacaoFidelidadeEvs + "?idPontuacaoFidelidade="
            + Europa.Controllers.ModalDownloadPdfPontuacaoFidelidade.IdPontuacaoFidelidade + "&idEmpresaVenda=" + selecionado;
    }
};