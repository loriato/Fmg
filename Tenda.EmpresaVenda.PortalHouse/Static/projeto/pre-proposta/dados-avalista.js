"use strict";

Europa.Controllers.PreProposta.DadosAvalista = {};
Europa.Controllers.PreProposta.DadosAvalista.UrlListarDocumentos = undefined;
Europa.Controllers.PreProposta.DadosAvalista.form = "#form_avalista";

Europa.Controllers.PreProposta.DadosAvalista.Modal = {};
Europa.Controllers.PreProposta.DadosAvalista.Modal.Id = "#documento_avalista_modal_upload";
Europa.Controllers.PreProposta.DadosAvalista.Modal.Selector = undefined;
Europa.Controllers.PreProposta.DadosAvalista.Modal.IdFormUpload = "#form_upload_documento_avalista";

Europa.Controllers.PreProposta.DadosAvalista.CurrentRowData = undefined;

$(function () {
    Europa.Controllers.PreProposta.DadosAvalista.Modal.Selector = $(Europa.Controllers.PreProposta.DadosAvalista.Modal.Id);
    $("#EnderecoAvalista_Estado").prop("disabled", true);
    Europa.Controllers.PreProposta.DadosAvalista.AplicarMascara();
});

Europa.Controllers.PreProposta.DadosAvalista.ActionAvalista = function () {

    $(".area").addClass("hidden");
    $("#area_avalista").removeClass("hidden");

    if (Europa.Controllers.PreProposta.DadosAvalista.Permissoes.Atualizar) {
        $(".avalista").prop("disabled", false);
        $(".avalista").prop("readOnly", false);
        $("#EnderecoAvalista_Estado").removeAttr("readOnly");
        $("#EnderecoAvalista_Estado").removeAttr("disabled");
        $("#btn_salvar_avalista").removeClass("hidden");
        $("#btn_enviar_documentos_avalista").removeClass("hidden");
    }

    $("#btn_imprimir_contrato").addClass("hidden");
    $("#btn_baixar_boleto").addClass("hidden"); 
    
};

////////////////////////////////////////////////////////////////////////////////////
// Funções Datatable
////////////////////////////////////////////////////////////////////////////////////
DataTableApp.controller('documentoAvalistaTable', documentoAvalistaTable);

function documentoAvalistaTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

    Europa.Controllers.PreProposta.DadosAvalista.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.PreProposta.DadosAvalista.Tabela;
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('NomeTipoDocumento').withTitle(Europa.i18n.Messages.TipoDocumento).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Anexado').withTitle(Europa.i18n.Messages.Anexado).withOption('width', '10%').renderWith(Europa.String.FormatBoolean),
        DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Status).withOption('width', '10%').withOption('type', 'enum-format-SituacaoAprovacaoDocumento'),
        DTColumnBuilder.newColumn('Parecer').withTitle(Europa.i18n.Messages.Motivo).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Situacao').withTitle("").withOption('width', '10%').renderWith(actionsHtml)

    ])
        .setAutoInit(false)
        .setIdAreaHeader("documento_avalista_datatable_header")
        .setDefaultOrder([[3, 'asc']])
        .setDefaultOptions('POST', Europa.Controllers.PreProposta.DadosAvalista.UrlListarDocumentos, Europa.Controllers.PreProposta.DadosAvalista.FilterParams);

    function actionsHtml(data, type, full, meta) {
        var div = '<div>';

        div += $scope.renderButtonUpload(data, Europa.i18n.Messages.Anexar, "fa fa-upload", "editar(" + meta.row + ")");
        div += $scope.renderButtonDelete(data, Europa.i18n.Messages.Excluir, "fa fa-trash", "excluir(" + meta.row + ")", data);        
                        
        div += '</div>';
        return div;
    }

    $scope.renderButtonUpload = function (hasPermission, title, icon, onClick) {

        if (hasPermission == 3 || hasPermission == 6) {
            return "";
        }

        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-steel')
            .attr('title', title)
            .attr('ng-click', onClick)
            .attr('style','margin-right:14px')
            .append(title)
        return button.prop('outerHTML');
    };
        
    $scope.renderButtonDelete = function (hasPermission, title, icon, onClick, full) {
        
        // Não tenho documento anexado, não tem por que excluir
        if (hasPermission == 3 || hasPermission == 6) {
            return "";
        }
        
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-steel')
            .attr('title', title)
            .attr('ng-click', onClick)
            .attr('style', 'margin-right:14px')
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.editar = function (row) {
        var rowData = Europa.Controllers.PreProposta.DadosAvalista.Tabela.getRowData(row);
        
        Europa.Controllers.PreProposta.DadosAvalista.PrepararUpload(rowData);
    };
        
    $scope.excluir = function (row) {
        var rowData = Europa.Controllers.PreProposta.DadosAvalista.Tabela.getRowData(row);
        
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, rowData.NomeTipoDocumento, function () {

            var requestContent = {
                IdPreProposta: rowData.IdPreProposta,
                IdDocumentoAvalista: rowData.Id,
                IdAvalista: rowData.IdAvalista,
                IdTipoDocumento: rowData.IdTipoDocumento
            };

            $.post(Europa.Controllers.PreProposta.DadosAvalista.UrlExcluirDocumentoAvalista, requestContent, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.PreProposta.DadosAvalista.Tabela.reloadData();   
                }
                Europa.Informacao.PosAcao(res);
            });
        });
    };
}

Europa.Controllers.PreProposta.DadosAvalista.FilterParams = function () {
    return {
        idPreProposta: $("#PreProposta_Id").val(),
        idCliente: $('#DocumentoProponente_Cliente').val(),
        situacaoDocumento: $('#DocumentoProponente_Situacao').val()
    };
};

Europa.Controllers.PreProposta.DadosAvalista.ConverterDinheiro = function (data) {
    if ($('[name="'+data+'"').val() !== undefined) {
        $('[name="' + data + '"').val($('[name="' + data + '"').replace(/\./g, ""));
        return $('[name="' + data + '"').val();
    }

    return "";
}

Europa.Controllers.PreProposta.DadosAvalista.CadastrarAvalista = function () {    

    var data = Europa.Form.SerializeJson("#form_pre_proposta");
    if (data["AvalistaDTO.Avalista.RendaDeclarada"] !== undefined) {
        data["AvalistaDTO.Avalista.RendaDeclarada"] = data["AvalistaDTO.Avalista.RendaDeclarada"].replace(/\./g, "");
    }

    if (data["AvalistaDTO.Avalista.OutrasRendas"] !== undefined) {
        data["AvalistaDTO.Avalista.OutrasRendas"] = data["AvalistaDTO.Avalista.OutrasRendas"].replace(/\./g, "");
    }

    if (data["AvalistaDTO.Avalista.ValorTotalBens"] !== undefined) {
        data["AvalistaDTO.Avalista.ValorTotalBens"] = data["AvalistaDTO.Avalista.ValorTotalBens"].replace(/\./g, "");
    }

    var dadosAvalista = {
        Id: $("#AvalistaDTO_Avalista_Id").val(),
        Nome:$("#AvalistaDTO_Avalista_Nome").val(),
        RG: $("#AvalistaDTO_Avalista_RG").val(),
        OrgaoExpedicao:$("#AvalistaDTO_Avalista_OrgaoExpedicao").val(),
        CPF:$("#AvalistaDTO_Avalista_CPF").val(),
        Profissao:$("#AvalistaDTO_Avalista_Profissao").val(),
        Empresa:$("#AvalistaDTO_Avalista_Empresa").val(),
        TempoEmpresa: $("#AvalistaDTO_Avalista_TempoEmpresa").val(),
        RendaDeclarada: data["AvalistaDTO.Avalista.RendaDeclarada"],
        OutrasRendas: data["AvalistaDTO.Avalista.OutrasRendas"],
        Banco:$("#AvalistaDTO_Avalista_Banco").val(),
        ContatoGerente:$("#AvalistaDTO_Avalista_ContatoGerente").val(),
        ValorTotalBens: data["AvalistaDTO.Avalista.ValorTotalBens"],
        PossuiImovel: $("input[name='AvalistaDTO.Avalista.PossuiImovel']:checked").val(),
        PossuiVeiculo: $("input[name='AvalistaDTO.Avalista.PossuiVeiculo']:checked").val()
    };

    var endereco = {
        Id: $("#AvalistaDTO_Endereco_Id").val(),
        Logradouro: $("#AvalistaDTO_Endereco_Logradouro").val(),
        Bairro: $("#AvalistaDTO_Endereco_Bairro").val(),
        Cep: $("#AvalistaDTO_Endereco_Cep").val(),
        Cidade: $("#AvalistaDTO_Endereco_Cidade").val(),
        Estado: $("#EnderecoAvalista_Estado").val(),
        Numero: $("#AvalistaDTO_Endereco_Numero").val(),
        Avalista: dadosAvalista
    };

    var avalista = {
        Avalista: dadosAvalista,
        Endereco: endereco,
        IdPreProposta: $("#PreProposta_Id").val()
    }

    var url = $("#AvalistaDTO_Avalista_Id").val() == 0 ? Europa.Controllers.PreProposta.DadosAvalista.UrlIncluirAvalista : Europa.Controllers.PreProposta.DadosAvalista.UrlAlterarAvalista;

    $.post(url, avalista, function (res) {
        if (res.Sucesso) {
            location.reload();
        }
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.PreProposta.DadosAvalista.PrepararUpload = function (rowData) {
    Europa.Controllers.PreProposta.DadosAvalista.CurrentRowData = rowData;
    $(Europa.Controllers.PreProposta.DadosAvalista.Modal.IdFormUpload).find("#arquivoAvalista").val("");
    Europa.Controllers.PreProposta.DadosAvalista.Modal.Show();
};

Europa.Controllers.PreProposta.DadosAvalista.Modal.Show = function () {
    Europa.Controllers.PreProposta.DadosAvalista.Modal.Selector.modal('show');
    $('#ArquivoDescricaoMotivoAvalista').removeAttr('readonly');
    $('#ArquivoDescricaoMotivoAvalista').val('');
};

Europa.Controllers.PreProposta.DadosAvalista.Modal.Hide = function () {
    Europa.Controllers.PreProposta.DadosAvalista.Modal.Selector.modal("hide");
};

Europa.Controllers.PreProposta.DadosAvalista.Upload = function () {

    var motivo = $(Europa.Controllers.PreProposta.DadosAvalista.Modal.IdFormUpload).find('#ArquivoDescricaoMotivoAvalista').get(0).value;
    var arquivoAvalista = $(Europa.Controllers.PreProposta.DadosAvalista.Modal.IdFormUpload).find("#arquivoAvalista").get(0).files[0];
    
    var maxFileSize = 16;
    var maxFileSizeInBytes = maxFileSize * 1000000;
    if (arquivoAvalista !== undefined && arquivoAvalista.size > maxFileSizeInBytes) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.ArquivoTamanhoMaximoExcedido, maxFileSize));
        Europa.Informacao.Show();
        return;
    }

    var formData = new FormData();

    formData.append("IdPreProposta", Europa.Controllers.PreProposta.DadosAvalista.CurrentRowData.IdPreProposta);
    formData.append("IdAvalista", Europa.Controllers.PreProposta.DadosAvalista.CurrentRowData.IdAvalista);
    formData.append("IdTipoDocumento", Europa.Controllers.PreProposta.DadosAvalista.CurrentRowData.IdTipoDocumento);
    formData.append("File", arquivoAvalista);
    formData.append("Motivo", motivo);
    formData.append("NomeTipoDocumento", Europa.Controllers.PreProposta.DadosAvalista.CurrentRowData.NomeTipoDocumento);
    formData.append("NomeAvalista", Europa.Controllers.PreProposta.DadosAvalista.CurrentRowData.NomeAvalista);

    $.ajax({
        type: 'POST',
        url: Europa.Controllers.PreProposta.DadosAvalista.UrlUploadDocumentoAvalista,
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
        success: function (res) {
            if (res.Sucesso) {
                Europa.Controllers.PreProposta.DadosAvalista.Tabela.reloadData();
                Europa.Controllers.PreProposta.DadosAvalista.Modal.Hide();
                Europa.Growl.SuccessFromJsonResponse(res);
            } else {
                Europa.Informacao.PosAcao(res);
            }
        }
    });
};

//Busca dados cep avalista
Europa.Controllers.PreProposta.DadosAvalista.OnChangeCepEnderecoAvalista = function (input) {
    Europa.Controllers.PreProposta.DadosAvalista.OnChangeCep(input, "AvalistaDTO_Endereco");
};

// Busca dados cep
Europa.Controllers.PreProposta.DadosAvalista.OnChangeCep = function (input, endereco) {
    var cep = $(input).val().replace(/\D/g, '');
    if (cep == "") {
        return;
    }
    var validacep = /^[0-9]{8}$/;
    if (!validacep.test(cep)) {
        return;
    }
    Europa.Components.Cep.Search(cep, function (dados) {
        $("#" + endereco + "_Logradouro", "#form_pre_proposta").val(dados.logradouroAbrev);
        $("#" + endereco + "_Bairro", "#form_pre_proposta").val(dados.bairro);
        $("#" + endereco + "_Cidade", "#form_pre_proposta").val(dados.localidade);
        $("#EnderecoAvalista_Estado", "#form_pre_proposta").val(dados.uf);
    });
};

Europa.Controllers.PreProposta.DadosAvalista.AplicarMascara = function () {
    Europa.Mask.ApplyByClass("dinheiro", Europa.Mask.FORMAT_MONEY, undefined, true);
    Europa.Mask.ApplyByClass("cep", Europa.Mask.FORMAT_CEP, true);
    Europa.Mask.ApplyByClass("inteiro", Europa.Mask.FORMAT_INTEIRO);
    Europa.Mask.Telefone("#AvalistaDTO_Avalista_ContatoGerente");
    Europa.Mask.CpfCnpj("#AvalistaDTO_Avalista_CPF");
};

Europa.Controllers.PreProposta.DadosAvalista.BaixarTodosDocumentosAvalista = function () {
    if ($("#AvalistaDTO_Avalista_Id").val() === "" || $("#AvalistaDTO_Avalista_Id").val() === undefined || $("#AvalistaDTO_Avalista_Id").val() === 0) {
        var msg = {
            Mensagens: Europa.i18n.Mensagens.MsgAvalistaNaoCadastrado
        }

        Europa.Informacao.PosAcao(msg);
        return;
    }

    var params = { IdPreProposta: $("#PreProposta_Id").val(), IdAvalista: $("#AvalistaDTO_Avalista_Id").val() };
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.PreProposta.DadosAvalista.UrlBaixarTodosDocumentosAvalista);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.PreProposta.DadosAvalista.EnviarDocumentosAvalista = function () {
    Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.EnviarDocumentosAvalista, Europa.i18n.Messages.MsgConfirmarEnvioDocumentosAvalista, Europa.i18n.Messages.Confirmar, function () {
        var params = { IdPreProposta: $("#PreProposta_Id").val(), IdAvalista: $("#AvalistaDTO_Avalista_Id").val() };
        $.post(Europa.Controllers.PreProposta.DadosAvalista.UrlEnviarDocumentosAvalista, params, function (res) {
            if (res.Sucesso) {
                Europa.Controllers.PreProposta.DadosAvalista.Tabela.reloadData();
                $(".avalista").prop("disabled", true);
                $(".avalista").prop("readOnly", true);
                $("#EnderecoAvalista_Estado").attr("readOnly");
                $("#EnderecoAvalista_Estado").attr("disabled");
                $("#btn_salvar_avalista").addClass("hidden");
                $("#btn_enviar_documentos_avalista").addClass("hidden");
            }
            Europa.Informacao.PosAcao(res);
            Europa.Controllers.PreProposta.DadosAvalista.Tabela.reloadData();
        })
    });

};
