Europa.Controllers.LeadImportExcel = {};
Europa.Controllers.LeadImportExcel.ModalId = "LeadImportExcel";
Europa.Controllers.LeadImportExcel.Modal = undefined;

Europa.Controllers.LeadImportExcel.AbrirModal = function () {
    var selecionados = Europa.Controllers.Lead.TreeViewEv.GetCheckedNodes();
    if (selecionados.length > 0) {
        Europa.Controllers.Lead.LimparCampo();
        Europa.Controllers.LeadImportExcel.Modal.modal("show");
    }
    else {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.i18n.Messages.NenhumRegistroSelecionando);
        Europa.Informacao.Show();
    }
};

Europa.Controllers.LeadImportExcel.CloseModal = function () {
    Europa.Controllers.Lead.LimparErro();
    Europa.Controllers.Lead.LimparCampo();
    Europa.Controllers.LeadImportExcel.Modal.modal("hide");
};

$(function () {
    Europa.Controllers.LeadImportExcel.Modal = $("#" + Europa.Controllers.LeadImportExcel.ModalId);
});

Europa.Controllers.LeadImportExcel.Formatar = function (msgs) {
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

DataTableApp.controller('CargaLeadController', ['$scope', '$http', function ($scope, $http) {
    $scope.enviandoArquivo = false;
    $scope.porcentagemCarregamento = 0;
    $scope.inputArquivo = $('#arquivoAnexoFile');
    $scope.taskId = "";
    $scope.taskObject = undefined;

    //Estilos
    $scope.carregamentoClass = { 'width': $scope.porcentagemCarregamento };
    $scope.enviarArquivo = function () {
            var selecionados = Europa.Controllers.Lead.TreeViewEv.GetCheckedNodes();
            var idRegs = [];
            selecionados.forEach(function (item) {
                idRegs.push(item.id);
            });
            var arquivo = $("#file")[0].files[0];
            if (arquivo.size > 20000000) {
                Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, Europa.String.Format(Europa.i18n.Messages.MsgArquivoTamanhoMaximo, (arquivo.size / 1000000).toFixed(2)));
                Europa.Informacao.Show();
            } else {
                $('#formularioUploadArquivoAnexo').ajaxSubmit({
                    type: 'POST',
                    url: Europa.Controllers.LeadImportExcel.UrlSalvarArquivo,
                    cache: false,
                    data: {
                        idsEmpresaVenda: idRegs,
                        pacote: $("#Pacote").val()
                    },
                    success: function (res, status, xhr) {
                        if (res.Sucesso) {
                            Europa.Controllers.LeadImportExcel.CloseModal();
                            Europa.Controllers.StatusImportacao.Show(res.Task);
                            Europa.Controllers.Lead.LimparErro();
                            Europa.Controllers.Lead.LimparCampo();
                            Europa.Controllers.Lead.AtualizarTreeViewEv();
                        } else {
                            Europa.Informacao.PosAcao(res)
                            Europa.Controllers.Lead.AdicionarErro(res.Campos);
                        }

                        
                    }
                });

            }    
    }
}]);


Europa.Controllers.Lead.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.Lead.LimparErro = function () {
    $("[name='Pacote']").parent().removeClass("has-error");

};

Europa.Controllers.Lead.LimparCampo = function () {
    $("#Pacote").val("");
    $("#file").val("");
};