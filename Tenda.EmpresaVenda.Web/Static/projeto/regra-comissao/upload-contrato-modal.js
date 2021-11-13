Europa.Controllers.RegraComissao.Import = {};
Europa.Controllers.RegraComissao.Import.ModalId = "modal_contrato_corretagem";
Europa.Controllers.RegraComissao.Import.Modal = undefined;

Europa.Controllers.RegraComissao.Import.AbrirModal = function () {
    Europa.Controllers.RegraComissao.Import.Modal.modal("show");
};

Europa.Controllers.RegraComissao.Import.CloseModal = function () {
    Europa.Controllers.RegraComissao.Import.Modal.modal("hide");
    $("#file").val("");
};

$(function () {
    Europa.Controllers.RegraComissao.Import.Modal = $("#" + Europa.Controllers.RegraComissao.Import.ModalId);
});

Europa.Controllers.RegraComissao.Import.Formatar = function (msgs) {
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

DataTableApp.controller('AnexoContratoCorretagemController', ['$scope', '$http', function ($scope, $http) {
    $scope.enviandoArquivo = false;
    $scope.porcentagemCarregamento = 0;
    $scope.inputArquivo = $('#arquivoAnexoFile');
    $scope.taskId = "";
    $scope.taskObject = undefined;

    //Estilos
    $scope.carregamentoClass = { 'width': $scope.porcentagemCarregamento };
    $scope.EnviarArquivoContrato = function () {
        var arquivo = $("#file")[0].files[0];
        if (arquivo != undefined && arquivo.size > 20000000) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.String.Format(Europa.i18n.Messages.TamanhoArquivoExcedido, "20MB"));
            Europa.Informacao.Show();
        } else {
            $('#form_salvar_contrato_corretagem').ajaxSubmit({
                type: 'POST',
                url: Europa.Controllers.RegraComissao.Import.UrlSalvarArquivo,
                cache: false,
                success: function (res, status, xhr) {
                    if (res.Sucesso) {
                        Europa.Informacao.PosAcao(res);
                        Europa.Controllers.RegraComissao.Import.CloseModal();
                        $("#secao-contrato-corretagem").html(res.Objeto);
                    } else {
                        Europa.Informacao.PosAcao(res);
                    }

                }
            });
        }
    };
}]);