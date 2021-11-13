Europa.Controllers.Juridico = {};

$(function () {

})

Europa.Controllers.Juridico.OpenModal = function () {
    $("#file").val("");
    $("#modal-upload-documento-juridico").show();
}

Europa.Controllers.Juridico.CloseModal = function () {
    $("#file").val("");
    $("#modal-upload-documento-juridico").hide();
}

DataTableApp.controller('JuridicoController', ['$scope', '$http', function ($scope, $http) {

    $scope.enviarArquivo = function () {
        var arquivo = $("#file")[0].files[0];

        if (arquivo == undefined) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, "Escolha um arquivo");
            Europa.Informacao.Show();

            return;
        }

        if (arquivo.size > 20000000) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, Europa.String.Format(Europa.i18n.Messages.MsgArquivoTamanhoMaximo, (arquivo.size / 1000000).toFixed(2)));
            Europa.Informacao.Show();
        } else {
            $('#formularioUploadAnexo').ajaxSubmit({
                type: 'POST',
                url: Europa.Controllers.Juridico.UrlUploadDocumentoJuridico,
                cache: false,
                success: function (res, status, xhr) {
                    if (res.Success) {
                        Europa.Controllers.Juridico.CloseModal();
                    }
                    Europa.Informacao.PosAcaoApi(res);
                }
            });
        }
    }
}]);
