Europa.Controllers.ModalDownloadPdfRegraComissao = {
    Url: {
        ListarEmpresasVendas: "",
        DownloadPdfRegraComissao: "",
        DownloadPdfRegraComissaoEvs: ""
    },
};

Europa.Controllers.ModalDownloadPdfRegraComissao.Abrir = function (idRegraComissao) {
    Europa.Controllers.ModalDownloadPdfRegraComissao.IdRegraComissao = idRegraComissao;
    $.get(Europa.Controllers.ModalDownloadPdfRegraComissao.Url.ListarEmpresasVendas, {idRegraComissao: Europa.Controllers.ModalDownloadPdfRegraComissao.IdRegraComissao}, function (res) {
        var html = "<option value='0'>" + Europa.i18n.Messages.Todos + "</option>";
        for(var i = 0; i < res.length; i++){
            var item = res[i];
            html = html + "<option value='" + item.Id + "'>" + item.NomeFantasia + "</option>"
        }
        $("#empresa_vendas_select", "#modalDownloadPdfRegraComissao").html(html);
        $("#modalDownloadPdfRegraComissao").show();
    });
};

Europa.Controllers.ModalDownloadPdfRegraComissao.Fechar = function () {
    $("#empresa_vendas_select", "#modalDownloadPdfRegraComissao").html("");
    $("#modalDownloadPdfRegraComissao").hide();
};

Europa.Controllers.ModalDownloadPdfRegraComissao.Confirmar = function () {
    var selecionado = parseInt($("#empresa_vendas_select", "#modalDownloadPdfRegraComissao").val());
    if(selecionado === 0){
        location.href = Europa.Controllers.ModalDownloadPdfRegraComissao.Url.DownloadPdfRegraComissao + "?idRegra="
            + Europa.Controllers.ModalDownloadPdfRegraComissao.IdRegraComissao;
    }else{
        location.href = Europa.Controllers.ModalDownloadPdfRegraComissao.Url.DownloadPdfRegraComissaoEvs + "?idRegra="
            + Europa.Controllers.ModalDownloadPdfRegraComissao.IdRegraComissao + "&idEmpresaVenda=" + selecionado;
    }
};