Europa.Controllers.PagamentoImportExcel = {};
Europa.Controllers.PagamentoImportExcel.ModalId = "PagamentoImportExcel";
Europa.Controllers.PagamentoImportExcel.Modal = undefined;

Europa.Controllers.PagamentoImportExcel.AbrirModal = function () {
    $('#file').val("");
    Europa.Controllers.PagamentoImportExcel.Modal.modal("show");
};

Europa.Controllers.PagamentoImportExcel.CloseModal = function () {
    Europa.Controllers.PagamentoImportExcel.Modal.modal("hide");
};

$(function () {
    Europa.Controllers.PagamentoImportExcel.Modal = $("#" + Europa.Controllers.PagamentoImportExcel.ModalId);
});

Europa.Controllers.PagamentoImportExcel.Formatar = function (msgs) {
    var mensagensHtml = $('<div/>');
    if ($.isArray(msgs) && msgs.length > 0) {
        msgs.map(function (x) {
            mensagensHtml.append($('<p/>').text(x));
        });
    } else {
        mensagensHtml.append($('<p/>').text(msgs));
    }
    return mensagensHtml;
};

DataTableApp.controller('PagamentoController', ['$scope', '$http', function ($scope, $http) {
    $scope.enviandoArquivo = false;
    $scope.porcentagemCarregamento = 0;
    $scope.inputArquivo = $('#arquivoAnexoFile');
    $scope.taskId = "";
    $scope.taskObject = undefined;

    //Estilos
    $scope.carregamentoClass = { 'width': $scope.porcentagemCarregamento };
    $scope.enviarArquivo = function () {
        var arquivo = $("#file")[0].files[0];
        if (arquivo.size > 20000000) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, Europa.String.Format(Europa.i18n.Messages.MsgArquivoTamanhoMaximo, (arquivo.size / 1000000).toFixed(2)));
            Europa.Informacao.Show();
        } else {
            $('#formularioUploadArquivoAnexo').ajaxSubmit({
                type: 'POST',
                url: Europa.Controllers.PagamentoImportExcel.UrlSalvarArquivo,
                cache: false,
                success: function (res, status, xhr) {
                    if (res.Sucesso) {
                        Europa.Controllers.PagamentoImportExcel.CloseModal();
                        Europa.Controllers.StatusImportacao.Show(res.Task);
                    } else {
                        Europa.Informacao.PosAcao(res);
                    }

                    
                }
            });

        }
    }
}]);

