"use strict";
"use strict";

Europa.Controllers.PreProposta.DocumentoProponente = {};
Europa.Controllers.PreProposta.DocumentoProponente.SituacaoNaoAnexado = 1;
Europa.Controllers.PreProposta.DocumentoProponente.SituacaoAprovado = 3;
Europa.Controllers.PreProposta.DocumentoProponente.EmAnaliseSimplificada = "Em Análise Simplificada";
Europa.Controllers.PreProposta.DocumentoProponente.EmAnaliseCompleta = "Em Análise Completa";
Europa.Controllers.PreProposta.DocumentoProponente.EmAnaliseSimplificada
Europa.Controllers.PreProposta.DocumentoProponente.IdPreProposta = undefined;
Europa.Controllers.PreProposta.DocumentoProponente.CurrentRowData = undefined;
Europa.Controllers.PreProposta.DocumentoProponente.CurrentRow = undefined;

Europa.Controllers.PreProposta.DocumentoProponente.UrlListar = undefined;
Europa.Controllers.PreProposta.DocumentoProponente.UrlUpload = undefined;
Europa.Controllers.PreProposta.DocumentoProponente.UrlExcluir = undefined;

Europa.Controllers.PreProposta.DocumentoProponente.DropDownSituacaoAprovacaoDocumento = undefined;

Europa.Controllers.PreProposta.DocumentoProponente.Modal = {};
Europa.Controllers.PreProposta.DocumentoProponente.Modal.Id = "#documento_proponente_modal_upload";
Europa.Controllers.PreProposta.DocumentoProponente.Modal.Selector = undefined;
Europa.Controllers.PreProposta.DocumentoProponente.Modal.IdFormUpload = "#form_upload_documento_proponente";

Europa.Controllers.PreProposta.DocumentoProponente.IdDocumento = undefined;
Europa.Controllers.PreProposta.DocumentoProponente.ListaDocumentos = undefined;
Europa.Controllers.PreProposta.DocumentoProponente.ListaArquivos = undefined;

Europa.Controllers.PreProposta.DocumentoProponente.DocsAdicionados = [];

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

    self.PrepararUpload = function (rowData, row) {
        self.CurrentRowData = rowData;
        self.CurrentRow = row;
        $(self.Modal.IdFormUpload).find("#arquivo").val("");
        self.Modal.Show();
    };

    self.NegativarAnexo = function (rowData) {
        self.CurrentRowData = rowData;
        $("#IdMotivo").removeAttr("readonly");
        $("#DescricaoMotivo").removeAttr("readonly");
        $("#CampoDocumento").val(rowData.NomeTipoDocumento);
        $("#CampoProponente").val(rowData.NomeCliente);
        $("#documento_proponente_nao_anexar").modal('show');
    };

    self.SalvarNegativarAnexo = function () {
        var url = Europa.Controllers.PreProposta.UrlNegativarAnexo;
        var documento = self.CurrentRowData;
        var idMotivo = $("#IdMotivo").val();
        var DescricaoMotivo = $("#DescricaoMotivo").val();
        $.post(url, { documento, idMotivo, DescricaoMotivo }, function (res) {
            if (res.Sucesso) {
                $("#documento_proponente_nao_anexar").modal('hide');
                Europa.Controllers.PreProposta.DocumentoProponente.Tabela.reloadData();
                self.FecharModalNegativarAnexo();
                self.CurrentRowData = undefined;
                $("#DescricaoMotivo").attr("disabled", "disabled");
                $("#IdMotivo").val("")
            }
            Europa.Informacao.PosAcao(res);
        });
    }

    self.Modal.Show = function () {
        self.Modal.Selector.modal('show');
        $('#ArquivoDescricaoMotivo').removeAttr('readonly');
        $('#ArquivoDescricaoMotivo').val('');
    };

    self.Modal.Hide = function () {
        self.Modal.Selector.modal("hide");
    };

    self.FecharModalNegativarAnexo = function () {
        $("#IdMotivo").val("");
        $("#DescricaoMotivo").val("");
        $("#DescricaoMotivo").attr("disabled", "disabled");
        $("#documento_proponente_nao_anexar").modal('hide');
    };

    self.PodeAnexarDocumento = function (situacaoDocumento) {
        var situacaoPermiteAnexar = Europa.Controllers.PreProposta.DocumentoProponente.SituacaoAprovado != situacaoDocumento;
        return situacaoPermiteAnexar; //&& Europa.Controllers.PreProposta.PodeManterAssociacoes();
    }

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
                    Europa.Controllers.PreProposta.DocumentoProponente.DocsAdicionados.push(self.CurrentRow);
                } else {
                    Europa.Informacao.PosAcao(res);
                }
            }
        });
    };

    self.SelecionarProponente = function (select) {
        $(".has-error").removeClass("has-error");
        var selectDocumentos = document.getElementById("lista_documentos");
        $(selectDocumentos).empty();
        var idProponente = select.value;
        if (idProponente) {
            self.ListaDocumentos.forEach(function (x) {
                // noinspection EqualityComparisonWithCoercionJS
                if (x.Proponente.Id == idProponente) {
                    selectDocumentos.options[selectDocumentos.options.length] = new Option(x.TipoDocumento.Nome, x.Id);
                }
            });
        }
        $(selectDocumentos).trigger('change');
    };

    self.SelecionarDocumento = function (select) {
        $(".has-error").removeClass("has-error");
        var idDocumento = select.value;
        if (idDocumento) {
            self.IdDocumento = idDocumento;
            var doc = self.GetArrayItemById(self.ListaDocumentos, idDocumento);
            self.EnableFields(doc);
            var arquivo = self.ListaArquivos[idDocumento];
            var existeDocumento = arquivo == undefined ? false : true;
            $("#ExisteDocumento").val(existeDocumento);
            if (arquivo) {
                var urlArquivo = basePath.replace("http:", "");
                urlArquivo = urlArquivo.replace(":80", "");
                urlArquivo = urlArquivo + "/Static/pdfjs/web/Viewer.html?file=" + arquivo.Url;
                var stringIframe = "<iframe src='" + urlArquivo + "' style='border: 1px solid #ccc; width: 100%; height: 100%;'></iframe>";
                $("#ViewerContainer").html(stringIframe);
            } else {
                $("#ViewerContainer").html("<label class='text-center' style='width: 100%'>" + Europa.i18n.Messages.DocumentoSelecionadoNaoPossuiArquivoAnexado + "</label>");
            }
        } else {
            self.DisableAndClearFields();
            self.IdDocumento = undefined;
        }
    };

    self.CycleDocumento = function (avancar) {
        var select = document.getElementById("lista_documentos");
        var curIndex = select.selectedIndex;
        var length = select.length;
        if (avancar) {
            if (curIndex + 1 >= length) {
                select.selectedIndex = 0;
            } else {
                select.selectedIndex = curIndex + 1;
            }
        } else {
            if (curIndex <= 0) {
                select.selectedIndex = select.length - 1;
            } else {
                select.selectedIndex = curIndex - 1;
            }
        }
        $(select).trigger('change');
    };

    self.EnableFields = function (doc) {
        $("#lista_documentos").prop("disabled", false);
        $(".btn-cycle-documentos").prop("disabled", false);
        $("#link-exibir-informacoes-auditoria").show();
        if (self.PropostaEmAnalise) {
            if (doc.Situacao != Europa.Controllers.DocumentacaoPreProposta.SituacaoAprovado) {
                $("#Parecer").prop("disabled", false);
                $("#DataExpiracao").prop("disabled", false);
            }

            switch (doc.Situacao) {
                case Europa.Controllers.DocumentacaoPreProposta.SituacaoAprovado:
                case Europa.Controllers.DocumentacaoPreProposta.SituacaoPendente:
                    $("#btn_pendenciar_documento").prop("disabled", false);
                    $("#btn_aprovar_documento").prop("disabled", false);
                    $("#Parecer").prop("disabled", false);
                    $("#DataExpiracao").prop("disabled", false);
                    break;
                case Europa.Controllers.DocumentacaoPreProposta.SituacaoAnexado:
                case Europa.Controllers.DocumentacaoPreProposta.SituacaoInformado:
                    $("#btn_pendenciar_documento").prop("disabled", false);
                    $("#btn_aprovar_documento").prop("disabled", false);
                    break;
                case Europa.Controllers.DocumentacaoPreProposta.SituacaoNaoAnexado:
                    $("#btn_pendenciar_documento").prop("disabled", true);
                    $("#btn_aprovar_documento").prop("disabled", false);
                    break;
            }
        }
    };


    self.GetArrayItemById = function (arr, id) {
        for (var i = 0, iLen = arr.length; i < iLen; i++) {
            if (arr[i].Id == id) {
                return arr[i];
            }
        }
    };

    self.GetParecerPorIdDocumento = function (listaParecer, idDocumento) {
        for (var i = 0; i < listaParecer.length; i++) {
            if (listaParecer[i].DocumentoProponente.Id == idDocumento) {
                return listaParecer[i];
            }
        }
    };

    self.DisableAndClearFields = function () {
        $("#lista_documentos").prop("disabled", true);
        $("#ViewerContainer").html("");
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
        DTColumnBuilder.newColumn('NomeCliente').withTitle(Europa.i18n.Messages.NomeCliente).withOption('width', '15%')
            .withOption("link", tabelaWrapper.withOptionLink(Europa.Components.DetailAction.Cliente, "IdCliente")),
        DTColumnBuilder.newColumn('NomeTipoDocumento').withTitle(Europa.i18n.Messages.TipoDocumento).withOption('width', '28%'),
        DTColumnBuilder.newColumn('Anexado').withTitle(Europa.i18n.Messages.Anexado + "?").withOption('width', '10%').renderWith(Europa.String.FormatBoolean),
        DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('width', '10%').withOption('type', 'enum-format-SituacaoAprovacaoDocumento'),
        DTColumnBuilder.newColumn('Motivo').withTitle(Europa.i18n.Messages.Motivo).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Parecer').withTitle(Europa.i18n.Messages.Parecer).withOption('width', '15%')
    ])
        .setIdAreaHeader("documento_proponente_datatable_header")
        .setColActions(actionsHtml, '25px')
        .setDefaultOrder([[3, 'asc']])
        .setAutoInit()
        .setDefaultOptions('POST', Europa.Controllers.PreProposta.DocumentoProponente.UrlListar, Europa.Controllers.PreProposta.DocumentoProponente.FilterParams);

    function actionsHtml(data, type, full, meta) {
        var situacaoPreProposta = $("#SituacaoPrepropostaSuatEvs").val();

        var div = '<div>';
        if (Europa.Controllers.PreProposta.DocumentoProponente.EmAnaliseSimplificada != situacaoPreProposta && Europa.Controllers.PreProposta.DocumentoProponente.EmAnaliseCompleta != situacaoPreProposta) {
            if (data.Situacao !== Europa.Controllers.PreProposta.DocumentoProponente.SituacaoAprovado) {
                div += $scope.renderButtonUpload(Europa.Controllers.PreProposta.DocumentoProponente.Permissoes.Atualizar, "Anexar Arquivo", "fa fa-upload", "editar(" + meta.row + ")", data);
                div += $scope.renderButtonNaoAnexar(Europa.Controllers.PreProposta.DocumentoProponente.Permissoes.NaoAnexar, "Não Anexar", "fa fa-edit", "naoanexar(" + meta.row + ")", full);
                div += $scope.renderButtonDelete(Europa.Controllers.PreProposta.DocumentoProponente.Permissoes.Excluir, "Excluir", "fa fa-trash", "excluir(" + meta.row + ")", data);

            }
        }
        div += $scope.renderButtonDownload("Download Arquivo", "fa fa-download", "download(" + meta.row + ")", data);
        div += $scope.renderButtonVisualizar("Visualizar Documento", "fa fa-eye", "visualizarDoc(" + meta.row + ")", data, meta.row);
        div += '</div>';
        return div;
    }

    $scope.renderButtonVisualizar = function (title, icon, onClick, data, row) {
        if (data.Situacao != 2 || Europa.Controllers.PreProposta.DocumentoProponente.DocsAdicionados.includes(row)) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-steel')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.renderButtonDownload = function (title, icon, onClick, data, row) {
        if (data.Situacao != 2) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-steel')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.renderButtonUpload = function (hasPermission, title, icon, onClick, data) {
        if (!Europa.Controllers.PreProposta.DocumentoProponente.PodeAnexarDocumento(data.Situacao)) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-steel')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.renderButtonNaoAnexar = function (hasPermission, title, icon, onClick, data) {
        if (!Europa.Controllers.PreProposta.PodeManterAssociacoes(data.Situacao)) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-steel')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.renderButtonDelete = function (hasPermission, title, icon, onClick, full) {
        // Só não posso alterar quando situação for aprovado
        var situacoesProibirModificacao = [Europa.Controllers.PreProposta.DocumentoProponente.SituacaoAprovado];
        if (situacoesProibirModificacao.includes(full.Situacao)) {
            return "";
        }
        // Não tenho documento anexado, não tem por que excluir
        if (full.Anexado === false || full.Anexado === 'false') {
            return "";
        }

        //if (!Europa.Controllers.PreProposta.PodeManterAssociacoes()) {
        //    return "";
        //}

        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-steel')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.editar = function (row) {
        var rowData = Europa.Controllers.PreProposta.DocumentoProponente.Tabela.getRowData(row);
        Europa.Controllers.PreProposta.DocumentoProponente.PrepararUpload(rowData, row);
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

    $scope.download = function (row) {
        var rowData = Europa.Controllers.PreProposta.DocumentoProponente.Tabela.getRowData(row);

        var params = {
            idPreProposta: $("#PreProposta_Id").val(),
            idDocumento: rowData.Id
        };
        var formExportar = $("#Exportar");
        $("#Exportar").find("input").remove();
        formExportar.attr("method", "post").attr("action", Europa.Controllers.PreProposta.UrlBaixarDocumento);
        formExportar.addHiddenInputData(params);
        formExportar.submit();
    };

    $scope.visualizarDoc = function (row) {
        $("#modal_visualizar_docs").modal('show');
        var rowData = Europa.Controllers.PreProposta.DocumentoProponente.Tabela.getRowData(row);
        var select = document.getElementById("lista_documentos");
        select.value = rowData.Id;
        $(select).trigger('change');
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
