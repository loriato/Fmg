Europa.Controllers.LeadEmMassaImportExcel = {};
Europa.Controllers.LeadEmMassaImportExcel.ModalId = "LeadEmMassaImportExcel";
Europa.Controllers.LeadEmMassaImportExcel.Modal = undefined;

Europa.Controllers.LeadEmMassaImportExcel.AbrirModal = function () {
    Europa.Controllers.LeadEmMassaImportExcel.LimparCampo();
    Europa.Controllers.LeadEmMassaImportExcel.Modal.modal("show");
};

Europa.Controllers.LeadEmMassaImportExcel.CloseModal = function () {
    Europa.Controllers.LeadEmMassaImportExcel.Modal.modal("hide");
};

$(function () {
    Europa.Controllers.LeadEmMassaImportExcel.Modal = $("#" + Europa.Controllers.LeadEmMassaImportExcel.ModalId);
});

Europa.Controllers.LeadEmMassaImportExcel.Formatar = function (msgs) {
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

DataTableApp.controller('CargaLeadEmMassaController', ['$scope', '$http', function ($scope, $http) {
    $scope.enviandoArquivo = false;
    $scope.porcentagemCarregamento = 0;
    $scope.inputArquivo = $('#arquivoAnexoFile');
    $scope.taskId = "";
    $scope.taskObject = undefined;

    //Estilos
    $scope.carregamentoClass = { 'width': $scope.porcentagemCarregamento };
    $scope.enviarArquivo = function () {
        var arquivo = $("#arquivo")[0].files[0];
        if (arquivo.size > 20000000) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, Europa.String.Format(Europa.i18n.Messages.MsgArquivoTamanhoMaximo, (arquivo.size / 1000000).toFixed(2)));
            Europa.Informacao.Show();
        } else {
            $('#formularioUploadArquivoAnexoEmMassa').ajaxSubmit({
                type: 'POST',
                url: Europa.Controllers.LeadEmMassaImportExcel.UrlSalvarArquivo,
                cache: false,
                success: function (res, status, xhr) {
                        Europa.Controllers.LeadEmMassaImportExcel.CloseModal();
                        Europa.Controllers.StatusImportacao.Show(res.Task);
                }
            });

        }
    }
}]);



Europa.Controllers.LeadEmMassaImportExcel.LimparCampo = function () {
    $("#arquivo").val("");
};