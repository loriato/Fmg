"use strict";
"use strict";

Europa.Controllers.PreProposta.DocumentoProponente = {};
Europa.Controllers.PreProposta.DocumentoProponente.SituacaoNaoAnexado = 1;
Europa.Controllers.PreProposta.DocumentoProponente.SituacaoAprovado = 3;
Europa.Controllers.PreProposta.DocumentoProponente.SituacaoFluxoEnviado = "Fluxo Enviado";
Europa.Controllers.PreProposta.DocumentoProponente.EmAnaliseSimplificada = "Em Análise Simplificada";
Europa.Controllers.PreProposta.DocumentoProponente.EmAnaliseCompleta = "Em Análise Completa";
Europa.Controllers.PreProposta.DocumentoProponente.IdPreProposta = undefined;
Europa.Controllers.PreProposta.DocumentoProponente.CurrentRowData = undefined;

Europa.Controllers.PreProposta.DocumentoProponente.UrlListar = undefined;
Europa.Controllers.PreProposta.DocumentoProponente.UrlUpload = "";
Europa.Controllers.PreProposta.DocumentoProponente.UrlExcluir = undefined;

Europa.Controllers.PreProposta.DocumentoProponente.DropDownSituacaoAprovacaoDocumento = undefined;

Europa.Controllers.PreProposta.DocumentoProponente.Modal = {};
Europa.Controllers.PreProposta.DocumentoProponente.Modal.Id = "#documento_proponente_modal_upload";
Europa.Controllers.PreProposta.DocumentoProponente.Modal.Selector = undefined;
Europa.Controllers.PreProposta.DocumentoProponente.Modal.IdFormUpload = "#form_upload_documento_proponente";

Europa.Controllers.PreProposta.DocumentoProponente.Init = function () {
    // Herdando configurações já efetuadas, já que este método só é executado após a página ser carregada.
    // Se não fizer isso, sobrescrevo todas as propriedades setadas anteriormente (geralmente URLs e permissÕes)
    var self = Europa.Controllers.PreProposta.DocumentoProponente;

    self.Modal.Selector = $(self.Modal.Id);

    $('#DocumentoProponente_Situacao').removeAttr('readonly');
    $('#DocumentoProponente_Cliente').removeAttr('readonly');

    self.Configure = function (idPreProposta) {
        self.IdPreProposta = idPreProposta;
        self.Reload();

        setTimeout(
            function () {
                $('#DocumentoProponente_Situacao').removeAttr('readonly');
                $('#DocumentoProponente_Cliente').removeAttr('readonly');
            }, 1000);
    };

    self.Reset = function () {
        self.IdPreProposta = undefined;
    };

    self.ReconstruirInformacoes = function () {
        // Chamar o render, recarregar o grid.
        var requestContent = {
            idPreProposta: self.IdPreProposta
        };
        $.post(Europa.Controllers.PreProposta.UrlProponenteDropDown, requestContent, function (response) {
            $('#dropdown_documento_proponente_cliente').html(response);
        });
        self.FiltrarTabela();
    };

    self.Reload = function () {
        self.FiltrarTabela();
    };

    self.Modal.Show = function () {
        self.Modal.Selector.modal('show');
        $('#ArquivoDescricaoMotivo').removeAttr('readonly');
        $('#ArquivoDescricaoMotivo').val('');
    };

    self.Modal.Hide = function () {
        self.Modal.Selector.modal("hide");
    };

    self.PrepararUpload = function (rowData) {
        self.CurrentRowData = rowData;
        $(self.Modal.IdFormUpload).find("#arquivo").val("");
        self.Modal.Show();
    };

    self.Upload = function () {

        var motivo = $(self.Modal.IdFormUpload).find('#ArquivoDescricaoMotivo').get(0).value;
        var arquivo = $(self.Modal.IdFormUpload).find("#arquivo").get(0).files[0];

        var maxFileSize = 16;
        var maxFileSizeInBytes = maxFileSize * 1000000;
        if (arquivo !== undefined && arquivo.size > maxFileSizeInBytes) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.ArquivoTamanhoMaximoExcedido, maxFileSize));
            Europa.Informacao.Show();
            return;
        }
        var formData = new FormData();
        formData.append("IdPreProposta", self.CurrentRowData.IdPreProposta);
        formData.append("IdProponente", self.CurrentRowData.IdProponente);
        formData.append("IdTipoDocumento", self.CurrentRowData.IdTipoDocumento);
        formData.append("File", arquivo);
        formData.append("Motivo", motivo);

        $.ajax({
            type: 'POST',
            url: self.UrlUpload,
            data: formData,
            contentType: false,
            cache: false,
            processData: false,
            success: function (res) {
                if (res.Sucesso) {
                    self.Tabela.reloadData();
                    self.Modal.Hide();
                    Europa.Growl.SuccessFromJsonResponse(res);
                    location.reload(true);
                } else {
                    Europa.Informacao.PosAcao(res);
                }
            }
        });
    };

    self.PodeAnexar = function (data) {
        var situacaoPreProposta = $("#SituacaoPrepropostaSuatEvs").val();
        
        if (Europa.Controllers.PreProposta.DocumentoProponente.SituacaoFluxoEnviado == situacaoPreProposta) {
            if (data.NomeTipoDocumento === Europa.i18n.Messages.Documento_ANEXO01) {
                return true;
            }

            if (data.NomeTipoDocumento === Europa.i18n.Messages.Documento_ANEXO07) {
                return true;
            }

            if (data.NomeTipoDocumento === Europa.i18n.Messages.Documento_ANEXO12) {
                return true;
            }
        }

        return false;
    };
};


////////////////////////////////////////////////////////////////////////////////////
// Funções Datatable
////////////////////////////////////////////////////////////////////////////////////
DataTableApp.controller('documentoProponentesTable', documentoProponentesTable);

function documentoProponentesTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

    Europa.Controllers.PreProposta.DocumentoProponente.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.PreProposta.DocumentoProponente.Tabela;
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('Id').withTitle(Europa.i18n.Messages.NomeCliente).withOption('visible', false),
        DTColumnBuilder.newColumn('NomeCliente').withTitle(Europa.i18n.Messages.NomeCliente).withOption('width', '20%')
            .withOption("link", tabelaWrapper.withOptionLink(Europa.Components.DetailAction.Cliente, "IdCliente")),
        DTColumnBuilder.newColumn('NomeTipoDocumento').withTitle(Europa.i18n.Messages.TipoDocumento).withOption('width', '25%'),
        DTColumnBuilder.newColumn('Anexado').withTitle(Europa.i18n.Messages.Anexado + "?").withOption('width', '10%').renderWith(Europa.String.FormatBoolean),
        DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('width', '10%').withOption('type', 'enum-format-SituacaoAprovacaoDocumento'),
        DTColumnBuilder.newColumn('Motivo').withTitle(Europa.i18n.Messages.Motivo).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Parecer').withTitle(Europa.i18n.Messages.Parecer).withOption('width', '15%')
    ])
        .setIdAreaHeader("documento_proponente_datatable_header")
        .setColActions(actionsHtml, '50px')
        .setDefaultOrder([[3, 'asc']])
        .setAutoInit()
        .setDefaultOptions('POST', Europa.Controllers.PreProposta.DocumentoProponente.UrlListar, Europa.Controllers.PreProposta.DocumentoProponente.FilterParams);

    function actionsHtml(data, type, full, meta) {
        var situacaoPreProposta = $("#SituacaoPrepropostaSuatEvs").val();
        var div = '<div>';


        if (Europa.Controllers.PreProposta.DocumentoProponente.EmAnaliseSimplificada != situacaoPreProposta && Europa.Controllers.PreProposta.DocumentoProponente.EmAnaliseCompleta != situacaoPreProposta) {
            if (full.Anexado) {
                div += $scope.renderButtonDelete(Europa.Controllers.PreProposta.DocumentoProponente.Permissoes.Excluir, "Excluir", "fa fa-trash", "excluir(" + meta.row + ")", data);
            }

            var permissao = (full.Situacao != 3);
            
            if (permissao) {
                div += $scope.renderButtonUpload(Europa.Controllers.PreProposta.DocumentoProponente.Permissoes.Atualizar, "Anexar Arquivo", "fa fa-upload", "editar(" + meta.row + ")", data);
            }
        }

        div += '</div>';
        return div;
    }

    $scope.renderButtonUpload = function (hasPermission, title, icon, onClick, data) {
        if (hasPermission !== 'true') {
            return "";
        }

        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };
    
    $scope.renderButtonDelete = function (hasPermission, title, icon, onClick, full) {
        if (hasPermission !== 'true') {
            return "";
        }
        // Só não posso alterar quando situação for aprovado
        var situacoesProibirModificacao = [Europa.Controllers.PreProposta.DocumentoProponente.SituacaoAprovado];
        if (situacoesProibirModificacao.includes(full.Situacao)) {
            return "";
        }
        // Não tenho documento anexado, não tem por que excluir
        if (full.Anexado === false || full.Anexado === 'false') {
            return "";
        }

        if (!Europa.Controllers.PreProposta.PodeManterAssociacoes()) {
            return "";
        }

        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.editar = function (row) {
        var rowData = Europa.Controllers.PreProposta.DocumentoProponente.Tabela.getRowData(row);
        Europa.Controllers.PreProposta.DocumentoProponente.PrepararUpload(rowData);
    };

    $scope.naoanexar = function (row) {
        var rowData = Europa.Controllers.PreProposta.DocumentoProponente.Tabela.getRowData(row);
        Europa.Controllers.PreProposta.DocumentoProponente.NegativarAnexo(rowData);
    };

    $scope.excluir = function (row) {
        var rowData = Europa.Controllers.PreProposta.DocumentoProponente.Tabela.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, rowData.NomeTipoDocumento, function () {

            var requestContent = {
                idPreProposta: rowData.IdPreProposta,
                id: rowData.Id
            };

            $.post(Europa.Controllers.PreProposta.DocumentoProponente.UrlExcluir, requestContent, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.PreProposta.DocumentoProponente.Reload();
                    Europa.Informacao.PosAcao(res);
                } else {
                    Europa.Informacao.PosAcao(res);
                }
            });
        });
    };

}


Europa.Controllers.PreProposta.DocumentoProponente.FilterParams = function () {
    return {
        idPreProposta: Europa.Controllers.PreProposta.DocumentoProponente.IdPreProposta,
        idCliente: $('#DocumentoProponente_Cliente').val(),
        situacaoDocumento: $('#DocumentoProponente_Situacao').val()
    };
};

Europa.Controllers.PreProposta.DocumentoProponente.FiltrarTabela = function () {
    $('#DocumentoProponente_Situacao').removeAttr('readonly');
    $('#DocumentoProponente_Cliente').removeAttr('readonly');
    Europa.Controllers.PreProposta.DocumentoProponente.Tabela.reloadData();
};

Europa.Controllers.PreProposta.DocumentoProponente.LimparFiltro = function () {
};

Europa.Controllers.PreProposta.DocumentoProponente.OnChangeMotivo = function () {
    var idMotivo = $("#IdMotivo").val();
    if (idMotivo === '0') {
        $("#DescricaoMotivo").removeAttr("disabled");
    }
    else {
        $("#DescricaoMotivo").attr("disabled", "disabled");
    }
};
