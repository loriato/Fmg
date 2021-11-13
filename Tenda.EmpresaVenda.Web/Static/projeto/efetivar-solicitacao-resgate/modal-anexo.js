$(function () {

});

Europa.Controllers.EfetivarSolicitacaoResgate.AbrirModalAnexo = function () {
    $("#SolicitacaoImportExcel").modal("show");
};

Europa.Controllers.EfetivarSolicitacaoResgate.CloseModalAnexo = function () {
    $("#SolicitacaoImportExcel").modal("hide");
};

Europa.Controllers.EfetivarSolicitacaoResgate.FormatarAnexo = function (msgs) {
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

DataTableApp.controller('EfetivarSolicitacaoResgateController', ['$scope', '$http', function ($scope, $http) {
    $scope.enviandoArquivo = false;
    $scope.porcentagemCarregamento = 0;
    $scope.inputArquivo = $('#arquivo');
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
            $('#formularioUploadAnexo').ajaxSubmit({
                type: 'POST',
                url: Europa.Controllers.EfetivarSolicitacaoResgate.Url.ImportExcel,
                cache: false,
                success: function (res, status, xhr) {
                    if (res.Sucesso) {
                        Europa.Controllers.EfetivarSolicitacaoResgate.CloseModalAnexo();
                        Europa.Controllers.StatusImportacao.Show(res.Task);
                    } else {
                        Europa.Informacao.PosAcao(res);
                    }
                }
            });
        }
    }
}]);