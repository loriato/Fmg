Europa.Controllers.DocumentoFormulario = {};
Europa.Controllers.DocumentoFormulario.TabelaDocumentos = undefined;
Europa.Controllers.DocumentoFormulario.IdPreProposta = 0;

$(function () {
    
})

//Datatable
DataTableApp.controller('DocumentoFormularioDatatable', documentoFormularioDatatable);

function documentoFormularioDatatable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

    Europa.Controllers.DocumentoFormulario.TabelaDocumentos = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.DocumentoFormulario.TabelaDocumentos;
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('NomeDocumento').withTitle(Europa.i18n.Messages.Nome).withOption('width', '10%'),
        DTColumnBuilder.newColumn('NomeResponsavel').withTitle(Europa.i18n.Messages.Viabilizador).withOption('width', '10%'),

    ])
        .setColActions(actionsHtml, '2%')
        .setDefaultOrder([[1, 'asc']])
        .setDefaultOptions('POST', Europa.Controllers.DocumentoFormulario.UrlListarDocumentos, Europa.Controllers.DocumentoFormulario.FilterParams);

    function actionsHtml(data, type, full, meta) {
        var button = "<div>";
        button += tabelaWrapper.renderButton(full.SituacaoPreProposta == 18 || full.SituacaoPreProposta == 14, Europa.i18n.Messages.Excluir, "fa fa-trash", "Excluir(" + meta.row + ")");
        button += tabelaWrapper.renderButton(full.IdArquivo, Europa.i18n.Messages.Download, "fa fa-download", "Download(" + full.IdArquivo + ")");
        return button + "</div>";
    }

    $scope.Excluir = function (row) {
        var objeto = Europa.Controllers.DocumentoFormulario.TabelaDocumentos.getRowData(row);

        Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.Excluir,
            "Tem certeza que deseja excluir o documento " + objeto.NomeDocumento + "?",
            "Excluir"
            , function () {
                Europa.Controllers.DocumentoFormulario.ExcluirDocumento(objeto)
            });
        
    };

    $scope.Download = function (idArquivo) {
        Europa.Controllers.DocumentoFormulario.DownloadDocumentoFormulario(idArquivo);
    }
}

Europa.Controllers.DocumentoFormulario.FilterParams = function () {
    var idPreProposta = $("#IdPreProposta").val();
    if (idPreProposta == null) {
        idPreProposta = $("#PreProposta_Id").val();
    }
    return {
        IdPreProposta: idPreProposta
    };
};

Europa.Controllers.DocumentoFormulario.FiltrarTabela = function () {
    Europa.Controllers.DocumentoFormulario.TabelaDocumentos.reloadData();
};

Europa.Controllers.DocumentoFormulario.LimparFiltro = function () {
    
}

Europa.Controllers.DocumentoFormulario.CloseModal = function () {
    $("#file").val("");
    $("#modal-formulario").hide()
}

Europa.Controllers.DocumentoFormulario.EnviarArquivo = function () {
    var documento = $("#formulario_upload_documento").find("#file").get(0).files[0];

    if (documento == undefined) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.CampoObrigatorio, "Documento"));
        Europa.Informacao.Show();
        return;
    }

    var maxFileSize = 16;
    var maxFileSizeInBytes = maxFileSize * 1000000;
    if (documento !== undefined && documento.size > maxFileSizeInBytes) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.ArquivoTamanhoMaximoExcedido, maxFileSize));
        Europa.Informacao.Show();
        return;
    }

    var idPreProposta = $("#IdPreProposta").val();

    if (idPreProposta == null) {
        idPreProposta = $("#PreProposta_Id").val();
    }

    var formData = new FormData();

    formData.append("Formulario", documento);
    formData.append("IdPreProposta", idPreProposta);
    formData.append("NomeDocumento", documento.name);

    $.ajax({
        type: 'POST',
        url: Europa.Controllers.DocumentoFormulario.UrlUploadDocumento,
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
        success: function (res) {
            if (res.Sucesso) {
                $("#file").val("");
                console.log("hello there");
                Europa.Controllers.DocumentoFormulario.FiltrarTabela();
            }
            Europa.Informacao.PosAcao(res);
        }
    });

}

Europa.Controllers.DocumentoFormulario.ExcluirDocumento = function (formulario) {

    $.post(Europa.Controllers.DocumentoFormulario.UrlExcluirDocumento, { formulario: formulario }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.DocumentoFormulario.FiltrarTabela();
        }

        Europa.Informacao.PosAcao(res);
    });
}

Europa.Controllers.DocumentoFormulario.DownloadDocumentoFormulario = function (idArquivo) {
    location.href = Europa.Controllers.DocumentoFormulario.UrlDownloadDocumentoFormulario += "?idArquivo=" + idArquivo;
}