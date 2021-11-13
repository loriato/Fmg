Europa.Controllers.TermoAceite = {}
Europa.Controllers.TermoAceite.ModalId = "modal_anexo_termo_aceite";
Europa.Controllers.TermoAceite.Modal = undefined;

$(function () {
    Europa.Controllers.TermoAceite.RenderTermoAceiteProgramaFidelidade();
    Europa.Controllers.TermoAceite.Modal = $("#" + Europa.Controllers.TermoAceite.ModalId);
});


Europa.Controllers.TermoAceite.RenderTermoAceiteProgramaFidelidade = function () {
    $.get(Europa.Controllers.TermoAceite.UrlRenderTermoAceiteProgramaFidelidade, function (res) {
        $("#secao-termo-aceite").html(res.Objeto);
    });
};


Europa.Controllers.TermoAceite.AbrirModal = function () {
    Europa.Controllers.TermoAceite.Modal.modal("show");
};

Europa.Controllers.TermoAceite.CloseModal = function () {
    Europa.Controllers.TermoAceite.Modal.modal("hide");
    $("#file").val("");
};

DataTableApp.controller('AnexoTermoAceiteController', ['$scope', '$http', function ($scope, $http) {
    $scope.enviandoArquivo = false;
    $scope.porcentagemCarregamento = 0;
    $scope.inputArquivo = $('#arquivoAnexoFile');
    $scope.taskId = "";
    $scope.taskObject = undefined;

    //Estilos
    $scope.carregamentoClass = { 'width': $scope.porcentagemCarregamento };
    $scope.EnviarArquivo = function () {
        var arquivo = $("#file")[0].files[0];
        if (arquivo != undefined && arquivo.size > 20000000) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.String.Format(Europa.i18n.Messages.TamanhoArquivoExcedido, "20MB"));
            Europa.Informacao.Show();
        } else {
            $('#form_salvar_termo_aceite').ajaxSubmit({
                type: 'POST',
                url: Europa.Controllers.TermoAceite.UrlSalvarArquivo,
                cache: false,
                success: function (res, status, xhr) {
                    if (res.Sucesso) {
                        Europa.Informacao.PosAcao(res);
                        Europa.Controllers.TermoAceite.CloseModal();
                        $("#secao-termo-aceite").html(res.Objeto);
                        Europa.Controllers.TermoAceite.Tabela.reloadData(); 
                    } else {
                        Europa.Informacao.PosAcao(res);
                    }

                }
            });
        }
    };
}]);

Europa.Controllers.TermoAceite.PreDownload = function () {
    var idArquivo = $("#IdArquivoDoubleCheck").val();
    if (idArquivo == null || idArquivo == "") {
        var res = {
            Sucesso: false,
            Mensagens: ["Não existe nenhum arquivo para download"]
        };
        Europa.Informacao.PosAcao(res);
    } else {
        Europa.Controllers.TermoAceite.Download(idArquivo)
    }
    
};

Europa.Controllers.TermoAceite.Download = function (id) {
    var params = {
        idArquivo: id
    }
    var formDownload = $("#form_download");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.TermoAceite.UrlDownload);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
}

function TabelaTermoAceite($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.TermoAceite.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.TermoAceite.Tabela;
    tabelaWrapper
        .setColumns([
            DTColumnBuilder.newColumn('Id').withTitle(Europa.i18n.Messages.Id).withOption('visible', false),
            DTColumnBuilder.newColumn('NomeDoubleCheck').withTitle(Europa.i18n.Messages.Nome).withOption('width', '45%'),
            DTColumnBuilder.newColumn('CriadoEm').withTitle(Europa.i18n.Messages.DataInicio).withOption('width', '45%').renderWith(Europa.String.FormatAsGeenDateTime)
        ])
        .setColActions(actionsHtml, '60px')
        .setDefaultOrder([[1, 'desc']])
        .setIdAreaHeader("datatable_header")
        .setDefaultOptions('POST', Europa.Controllers.TermoAceite.UrlListar, Europa.Controllers.TermoAceite.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButton(Europa.Controllers.TermoAceite.Permissoes.Visualizar, "Download", "fa fa-download", "Download(" + meta.row + ")", full.Situacao) + 
            '</div>';
    }

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (hasPermission === 'true') {
            icon = $('<i/>').addClass(icon);
            var button = $('<a />')
                .addClass('btn btn-default')
                .attr('title', title)
                .attr('ng-click', onClick)
                .append(icon);
            return button.prop('outerHTML');
        } else {
            return null;
        }
    };

    $scope.Download = function (row) {
        var objetoLinhaTabela = Europa.Controllers.TermoAceite.Tabela.getRowData(row);
        Europa.Controllers.TermoAceite.Download(objetoLinhaTabela.IdArquivoDoubleCheck);
    };
   
};

DataTableApp.controller('TermoAceite', TabelaTermoAceite);

Europa.Controllers.TermoAceite.FilterParams = function () {
    var param = {};
    return param;
};