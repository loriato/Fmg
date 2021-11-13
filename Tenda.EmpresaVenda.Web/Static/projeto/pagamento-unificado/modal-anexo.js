Europa.Controllers.PagamentoUnificado.ModalId = "PagamentoImportExcel";

$(function () {

});

Europa.Controllers.PagamentoUnificado.AbrirModalImportExcel = function () {
    $('#file').val("");
    $("#PagamentoImportExcel").modal("show")
};

Europa.Controllers.PagamentoUnificado.CloseModalImportExcel = function () {
    $('#file').val("");
    $("#PagamentoImportExcel").modal("hide");
};

Europa.Controllers.PagamentoUnificado.Formatar = function (msgs) {
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

DataTableApp.controller('PagamentoUnificadoController', ['$scope', '$http', function ($scope, $http) {
    $scope.enviandoArquivo = false;
    $scope.porcentagemCarregamento = 0;
    $scope.inputArquivo = $('#arquivoAnexoFile');
    $scope.taskId = "";
    $scope.taskObject = undefined;

    //Estilos
    $scope.carregamentoClass = { 'width': $scope.porcentagemCarregamento };
    $scope.enviarArquivo = function () {
        var arquivo = $("#file")[0].files[0];

        if (arquivo == undefined) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.CampoObrigatorio, "Arquivo"));
            Europa.Informacao.Show();
            return;
        }

        if (arquivo.size > 20000000) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, Europa.String.Format(Europa.i18n.Messages.MsgArquivoTamanhoMaximo, (arquivo.size / 1000000).toFixed(2)));
            Europa.Informacao.Show();
            return;
        } 

        $('#formularioUploadArquivoAnexo').ajaxSubmit({
            type: 'POST',
            url: Europa.Controllers.PagamentoUnificado.UrlSalvarArquivo,
            cache: false,
            success: function (res, status, xhr) {
                if (res.Sucesso) {
                    Europa.Controllers.PagamentoUnificado.CloseModalImportExcel();
                    Europa.Controllers.PagamentoUnificado.ShowStatusRequisicaoCompra(res.Task);
                } else {
                    Europa.Informacao.PosAcao(res);
                }
            }
        });        
    }
}]);