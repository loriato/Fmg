Europa.Controllers.Loja.Import = {};
Europa.Controllers.Loja.Import.ModalId = "lojas_import";
Europa.Controllers.Loja.Import.Modal = undefined;

Europa.Controllers.Loja.Import.AbrirModal = function() {
    Europa.Controllers.Loja.Import.Modal.modal("show");
};

Europa.Controllers.Loja.Import.CloseModal = function() {
    Europa.Controllers.Loja.Import.Modal.modal("hide");
};

$(function() {
    Europa.Controllers.Loja.Import.Modal = $("#" + Europa.Controllers.Loja.Import.ModalId);
});

Europa.Controllers.Loja.Import.Formatar = function(msgs) {
    var mensagensHtml = $('<div/>');
    if ($.isArray(msgs) && msgs.length > 0) {
        msgs.map(function(x) {
            mensagensHtml.append($('<p/>').text(x));
        });
    } else {
        mensagensHtml.append($('<p/>').text(msgs));
    }
    return mensagensHtml;
};

DataTableApp.controller('AnexoPrecoRasoController', ['$scope', '$http', function($scope, $http) {
    $scope.enviandoArquivo = false;
    $scope.porcentagemCarregamento = 0;
    $scope.inputArquivo = $('#arquivoAnexoFile');
    $scope.taskId = "";
    $scope.taskObject = undefined;

    //Estilos
    $scope.carregamentoClass = { 'width': $scope.porcentagemCarregamento };
    $scope.enviarExcelLojas = function () {
        var arquivo = $("#file")[0].files[0];
        if (arquivo != undefined && arquivo.size > 20000000) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.String.Format(Europa.i18n.Messages.TamanhoArquivoExcedido, "20MB"));
            Europa.Informacao.Show();
        } else {
            $('#formularioUploadArquivoAnexo').ajaxSubmit({
                type: 'POST',
                url: Europa.Controllers.Loja.Import.UrlSalvarArquivo,
                cache: false,
                success: function (res, status, xhr) {
                    if (res.Sucesso) {
                        Europa.Controllers.Loja.Import.CloseModal();
                        Europa.Controllers.Loja.StatusImportacao.Show(res.Task);
                    } else {
                        Europa.Informacao.PosAcao(res);
                    }

                }
            });
        }
    }
}]);