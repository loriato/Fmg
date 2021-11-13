Europa.Controllers.CargaRelRepasseImportExcel = {};
Europa.Controllers.CargaRelRepasseImportExcel.ModalId = "CargaRelRepasseImportExcel";
Europa.Controllers.CargaRelRepasseImportExcel.Modal = undefined;

Europa.Controllers.CargaRelRepasseImportExcel.AbrirModal = function () {
    Europa.Controllers.CargaRelRepasseImportExcel.Modal.modal("show");
};

Europa.Controllers.CargaRelRepasseImportExcel.CloseModal = function () {
    Europa.Controllers.CargaRelRepasseImportExcel.Modal.modal("hide");
};

$(function () {
    Europa.Controllers.CargaRelRepasseImportExcel.Modal = $("#" + Europa.Controllers.CargaRelRepasseImportExcel.ModalId);
});

DataTableApp.controller('CargaRelRepasseController', ['$scope', '$http', function ($scope, $http) {
    $scope.enviandoArquivo = false;
    $scope.porcentagemCarregamento = 0;
    $scope.inputArquivo = $('#arquivoAnexoFile');
    $scope.taskId = "";
    $scope.taskObject = undefined;

    //Estilos
    $scope.carregamentoClass = { 'width': $scope.porcentagemCarregamento };
    $scope.enviarArquivo = function () {
        var arquivo = $("#file")[0].files[0];
        var nome = $("#file").val();
        nome = nome.slice(nome.length - 4);
        if (arquivo && nome != 'xlsx'){
           Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, Europa.String.Format(Europa.i18n.Messages.ErroFormatoArquivo,".xlsx"));
            Europa.Informacao.Show();
        }
        else if (arquivo.size > 41943040) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, Europa.String.Format(Europa.i18n.Messages.MsgArquivoTamanhoMaximo, (arquivo.size / 1048576).toFixed(2))+ "(40MB)");
            Europa.Informacao.Show();
        }
        else {
                $('#formularioUploadArquivoAnexo').ajaxSubmit({
                    type: 'POST',
                    url: Europa.Controllers.CargaRelRepasseImportExcel.UrlSalvarArquivo,
                    cache: false,
                    success: function (res, status, xhr) {
                        Europa.Controllers.CargaRelRepasseImportExcel.CloseModal();
                        Europa.Informacao.PosAcao(res)
                    },
                    error: function (res, status, xhr) {
                        Europa.Informacao.PosAcao(res)
                    }

                });
             }
    }
}]);

