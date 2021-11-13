Europa.Controllers.EmpreendimentoImportExcel = {};
Europa.Controllers.EmpreendimentoImportExcel.ModalId = "EmpreendimentoImportExcel";
Europa.Controllers.EmpreendimentoImportExcel.Modal = undefined;

Europa.Controllers.EmpreendimentoImportExcel.AbrirModal = function() {
    Europa.Controllers.EmpreendimentoImportExcel.Modal.modal("show");
};

Europa.Controllers.EmpreendimentoImportExcel.CloseModal = function() {
    Europa.Controllers.EmpreendimentoImportExcel.Modal.modal("hide");
};

$(function() {
    Europa.Controllers.EmpreendimentoImportExcel.Modal = $("#" + Europa.Controllers.EmpreendimentoImportExcel.ModalId);
});

Europa.Controllers.EmpreendimentoImportExcel.Formatar = function(msgs) {
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
    $scope.enviarArquivo = function () {
        var arquivo = $("#file")[0].files[0];
        if (arquivo.size > 20000000) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, Europa.String.Format(Europa.i18n.Messages.MsgArquivoTamanhoMaximo, (arquivo.size / 1000000).toFixed(2)));
            Europa.Informacao.Show();
        } else {
            $('#formularioUploadArquivoAnexo').ajaxSubmit({
                type: 'POST',
                url: Europa.Controllers.EmpreendimentoImportExcel.UrlSalvarArquivo,
                cache: false,
                success: function (res, status, xhr) {
                    Europa.Controllers.EmpreendimentoImportExcel.CloseModal();
                    Europa.Controllers.StatusImportacao.Show(res.Task);
                }
            });
        }
    }
}]);

Europa.Controllers.Empreendimento.IntegrarEmpreendimento = function () {
    $.post(Europa.Controllers.Empreendimento.UrlIntegrarEmpreendimento, function (res) {
        Europa.Informacao.PosAcao(res);
    });
};
