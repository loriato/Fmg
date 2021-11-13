$(function () {

});

Europa.Controllers.PropostaFaturada.AbrirModalAnexo = function () {
    $("#file").val("");
    $("#SolicitacaoImportExcel").modal("show");
};

Europa.Controllers.PropostaFaturada.CloseModalAnexo = function () {
    $("#file").val("");
    $("#SolicitacaoImportExcel").modal("hide");
};

Europa.Controllers.PropostaFaturada.FormatarAnexo = function (msgs) {
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

DataTableApp.controller('PropostaFaturadaController', ['$scope', '$http', function ($scope, $http) {
    $scope.enviandoArquivo = false;
    $scope.porcentagemCarregamento = 0;
    $scope.inputArquivo = $('#arquivo');
    $scope.taskId = "";
    $scope.taskObject = undefined;

    //Estilos
    $scope.carregamentoClass = { 'width': $scope.porcentagemCarregamento };
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
                url: Europa.Controllers.PropostaFaturada.UrlImportExcel,
                cache: false,
                success: function (res, status, xhr) {
                    if (res.Sucesso) {
                        Europa.Controllers.PropostaFaturada.CloseModalAnexo();
                        Europa.Controllers.PropostaFaturada.ShowStatusImportacao(res.Task);
                    } else {
                        Europa.Informacao.PosAcao(res);
                    }
                }
            });
        }
    }
}]);