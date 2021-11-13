Europa.Controllers.ImportExcel = {};
Europa.Controllers.ImportExcel.ModalId = "ImportExcel";
Europa.Controllers.ImportExcel.Modal = undefined;

Europa.Controllers.ImportExcel.AbrirModal = function () {
    $("#file").val("");
    Europa.Controllers.ImportExcel.Modal.modal("show");
};

Europa.Controllers.ImportExcel.CloseModal = function () {
    Europa.Controllers.ImportExcel.Modal.modal("hide");
};

$(function () {
    Europa.Controllers.ImportExcel.Modal = $("#" + Europa.Controllers.ImportExcel.ModalId);
});

Europa.Controllers.ImportExcel.Formatar = function (msgs) {
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

DataTableApp.controller('CargaController', ['$scope', '$http', function ($scope, $http) {
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
                    url: Europa.Controllers.ImportExcel.UrlSalvarArquivo,
                    cache: false,
                    success: function (res, status, xhr) {
                        if (res.Sucesso) {
                            Europa.Controllers.ImportExcel.CloseModal();
                            Europa.Controllers.StatusImportacao.Show(res.Task);
                            Europa.Controllers.ValorNominal.Filtrar();
                        } else {
                            Europa.Informacao.PosAcao(res)
                            Europa.Controllers.Lead.AdicionarErro(res.Campos);
                        }

                        
                    }
                });

            }    
    }
}]);