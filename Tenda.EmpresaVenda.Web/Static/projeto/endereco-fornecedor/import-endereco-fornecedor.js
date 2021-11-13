$(function () {
})

Europa.Controllers.EnderecoFornecedor.OpenModal = function () {
    $("#file").val("");
    $("#modal-import").show();
};

Europa.Controllers.EnderecoFornecedor.CloseModal = function () {
    $("#file").val("");
    $("#modal-import").hide();
}

Europa.Controllers.EnderecoFornecedor.UploadEnderecoFornecedor = function () {
    var file = $("#file")[0].files[0];

    var nome = $("#file").val();
    nome = nome.slice(nome.length - 4);

    if (file == undefined) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.CampoObrigatorio, "Arquivo"));
        Europa.Informacao.Show();
        return;
    }

    if (file && nome != 'xlsx') {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, Europa.String.Format(Europa.i18n.Messages.ErroFormatoArquivo, ".xlsx"));
        Europa.Informacao.Show();
        return;
    }
    else if (file.size > 41943040) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, Europa.String.Format(Europa.i18n.Messages.MsgArquivoTamanhoMaximo, (arquivo.size / 1048576).toFixed(2)) + "(40MB)");
        Europa.Informacao.Show();
        return;
    }

    $('#formularioUploadArquivoAnexo').ajaxSubmit({
        type: 'POST',
        url: Europa.Controllers.EnderecoFornecedor.UrUploadEnderecoFornecedor,
        cache: false,
        success: function (res, status, xhr) {
            if (res.Sucesso) {
                Europa.Controllers.EnderecoFornecedor.CloseModal();
            } else {
                window.location.href = res.Objeto.TargetFilePath;
            }
            Europa.Controllers.EnderecoFornecedor.CloseModal();
            Europa.Controllers.EnderecoFornecedor.FiltrarTabela();            
            Europa.Informacao.PosAcao(res);
        },
        error: function (res, status, xhr) {
            Europa.Informacao.PosAcao(res);
        }

    });
}