Europa.Controllers.DocumentacaoAvalista = {};
Europa.Controllers.DocumentacaoAvalista.SituacaoNaoAnexado = 1;
Europa.Controllers.DocumentacaoAvalista.SituacaoAnexado = 2;
Europa.Controllers.DocumentacaoAvalista.SituacaoAprovado = 3;
Europa.Controllers.DocumentacaoAvalista.SituacaoPendente = 4;
Europa.Controllers.DocumentacaoAvalista.SituacaoInformado = 5;
Europa.Controllers.DocumentacaoAvalista.SituacaoEnviado = 6;
Europa.Controllers.DocumentacaoAvalista.PropostaEmAnalise = undefined;
Europa.Controllers.DocumentacaoAvalista.ListaProponentes = undefined;
Europa.Controllers.DocumentacaoAvalista.ListaDocumentos = undefined;
Europa.Controllers.DocumentacaoAvalista.ListaArquivos = undefined;
Europa.Controllers.DocumentacaoAvalista.ListaParecer = undefined;
Europa.Controllers.DocumentacaoAvalista.IdDocumento = undefined;
Europa.Controllers.DocumentacaoAvalista.ExibindoInfo = false;
Europa.Controllers.DocumentacaoAvalista.UrlAprovarDocumento = undefined;
Europa.Controllers.DocumentacaoAvalista.UrlPendenciarDocumento = undefined;
Europa.Controllers.DocumentacaoAvalista.UrlBaixarTodosDocumentos = undefined;

$(function () {
    Europa.Controllers.DocumentacaoAvalista.AplicarMascara();
    Europa.Controllers.DocumentacaoAvalista.Init();
});

Europa.Controllers.DocumentacaoAvalista.Init = function () {
    $("#btn_pendenciar_documento").prop("disabled", true);
    $("#btn_aprovar_documento").prop("disabled", true);
    $("#Parecer").prop("disabled", true);
    $("#btn_aprovar_documentacao").prop("disabled", true);
    $("#btn_pendenciar_documentacao").prop("disabled", true);
}

Europa.Controllers.DocumentacaoAvalista.AplicarMascara = function () {
    Europa.Mask.ApplyByClass("dinheiro", Europa.Mask.FORMAT_MONEY, undefined, true);
    Europa.Mask.ApplyByClass("cep", Europa.Mask.FORMAT_CEP, true);
    Europa.Mask.ApplyByClass("inteiro", Europa.Mask.FORMAT_INTEIRO);
    Europa.Mask.Telefone("#ContatoGerente");
    Europa.Mask.CpfCnpj("#CPF");
};

Europa.Controllers.DocumentacaoAvalista.VerificarSituacaoDocumentos = function () {
    var idAvalista = $("#lista_avalistas").val();
    var somenteSemAnalise = $("#somente_nao_analisados").is(":checked");
    if (idAvalista !== undefined && idAvalista !== "" && somenteSemAnalise) {        
        var existeDocumentoNaoAnalisado = false;
        for (var i = 0; i < Europa.Controllers.DocumentacaoAvalista.ListaDocumentos.length; i++) {
            if (Europa.Controllers.DocumentacaoAvalista.ListaDocumentos[i].Avalista.Id == idAvalista && Europa.Controllers.DocumentacaoAvalista.ListaDocumentos[i].Situacao != 3 && Europa.Controllers.DocumentacaoAvalista.ListaDocumentos[i].Situacao != 4) {
                existeDocumentoNaoAnalisado = true;
            }
        }
        if (!existeDocumentoNaoAnalisado) {
            $(".dadosDocumentos").addClass("hidden");
            $("#todos_documentos_aprovados").removeClass("hidden");
        }
        else {
            $(".dadosDocumentos").removeClass("hidden");
            $("#todos_documentos_aprovados").addClass("hidden");
        }
    }
    else {
        $(".dadosDocumentos").removeClass("hidden");
        $("#todos_documentos_aprovados").addClass("hidden");
    }
}

Europa.Controllers.DocumentacaoAvalista.AtualizarViewModel = function (idPreProposta) {
    var somenteSemAnalise = $("#somente_nao_analisados").is(":checked");
    $.post(Europa.Controllers.DocumentacaoAvalista.UrlAtualizarViewModel, { idPreProposta, somenteSemAnalise }, function (res) {
        Europa.Controllers.DocumentacaoAvalista.ListaParecer = res.Pareceres;
        Europa.Controllers.DocumentacaoAvalista.ListaDocumentos = res.Documentos;
        var select = document.getElementById("lista_avalistas");
        Europa.Controllers.DocumentacaoAvalista.VerificarSituacaoDocumentos();
        $(select).trigger('change');
    });
};

Europa.Controllers.DocumentacaoAvalista.CycleAvalista = function (avancar) {
    var select = document.getElementById("lista_avalistas");
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
    Europa.Controllers.DocumentacaoAvalista.VerificarSituacaoDocumentos();
    $(select).trigger('change');
};

Europa.Controllers.DocumentacaoAvalista.SelecionarAvalista = function (select) {
    
    $(".has-error").removeClass("has-error");
    var selectDocumentos = document.getElementById("lista_documentos");
    $(selectDocumentos).empty();
    var idAvalista = select.value;
    if (idAvalista) {
        Europa.Controllers.DocumentacaoAvalista.ListaDocumentos.forEach(function (x) {
            if (x.Avalista.Id == idAvalista) {
                selectDocumentos.options[selectDocumentos.options.length] = new Option(x.TipoDocumento.Nome, x.Id);
            }
        });
    } else {
        $("#btn_pendenciar_documento").prop("disabled", true);
        $("#btn_aprovar_documento").prop("disabled", true);
        $("#btn_aprovar_documentacao").prop("disabled", true);
        $("#btn_pendenciar_documentacao").prop("disabled", true);
    }
    $(selectDocumentos).trigger('change');
};

Europa.Controllers.DocumentacaoAvalista.AprovarDocumento = function () {
    $(".has-error").removeClass("has-error");
    var data = {
        IdDocumentoAvalista: Europa.Controllers.DocumentacaoAvalista.IdDocumento,
        Parecer: $("#Parecer").val(),
        ExisteDocumento: $("#ExisteDocumento").val()
    };
    $.post(Europa.Controllers.DocumentacaoAvalista.UrlAprovarDocumento,
        data,
        function (res) {
            if (res.Sucesso) {
                for (var i in Europa.Controllers.DocumentacaoAvalista.ListaDocumentos) {
                    if (Europa.Controllers.DocumentacaoAvalista.ListaDocumentos[i].Id == res.Objeto.Id) {
                        Europa.Controllers.DocumentacaoAvalista.ListaDocumentos[i] = res.Objeto;
                        break;
                    }
                }
                if (data.Parecer != null || data.Parecer != undefined || data.Parecer != "") {
                    var parecer = {
                        DocumentoAvalista: { Id: data.IdDocumentoAvalista },
                        Parecer: data.Parecer
                    };
                    Europa.Controllers.DocumentacaoAvalista.ListaParecer.push(parecer);
                }
                var trocouOpcao = Europa.Controllers.DocumentacaoAvalista.SkipToNextDocument(res.Objeto.NomeTipoDocumento);
                if (!trocouOpcao) {
                    return;
                }
            } else {
                Europa.Controllers.DocumentacaoAvalista.AddError(res.Campos);
            }
            Europa.Informacao.PosAcao(res);
        });
};

Europa.Controllers.DocumentacaoAvalista.SkipToNextDocument = function (nomeAvalista) {
    Europa.Controllers.DocumentacaoAvalista.AtualizarViewModel($("#IdPreProposta").val());
    var trocouOpcao = false;
    var idAvalista = $("#lista_avalistas").val();
    var todosAnalisados = true;
    for (var i in Europa.Controllers.DocumentacaoAvalista.ListaDocumentos) {
        if (Europa.Controllers.DocumentacaoAvalista.ListaDocumentos[i].Situacao == 2 ||
            Europa.Controllers.DocumentacaoAvalista.ListaDocumentos[i].Situacao == 5 && Europa.Controllers.DocumentacaoAvalista.ListaDocumentos[i].Avalista.Id == $("#lista_avalistas").val()) {
            Europa.Controllers.DocumentacaoAvalista.ListaDocumentos[i]
            $("#lista_documentos > option").each(function () {
                var opId = this.value;
                var idDocumento = Europa.Controllers.DocumentacaoAvalista.ListaDocumentos[i].Id;
                if (opId == idDocumento) {
                    $("#lista_documentos").val(opId);
                    trocouOpcao = true;
                }
            });
            break;
        }
    }

    $("#lista_documentos").trigger('change');
    for (var i in Europa.Controllers.DocumentacaoAvalista.ListaDocumentos) {
        if (Europa.Controllers.DocumentacaoAvalista.ListaDocumentos[i].Situacao != 3 && Europa.Controllers.DocumentacaoAvalista.ListaDocumentos[i].Situacao != 4 && Europa.Controllers.DocumentacaoAvalista.ListaDocumentos[i].Avalista.Id == idAvalista) {
            todosAnalisados = false;
        }
    }
    if (todosAnalisados) {
        var avalista = Europa.Controllers.DocumentacaoAvalista.GetAvalistaSelecionado();
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao,
            Europa.String.Format(Europa.i18n.Messages.TodosDocumentosDoAvalistaEstaoValidados, avalista.Nome));
        Europa.Informacao.Show();
    }
    return trocouOpcao;
};

Europa.Controllers.DocumentacaoAvalista.GetAvalistaSelecionado = function () {
    var idAvalista = $("#lista_avalistas").val();
    for (var i in Europa.Controllers.DocumentacaoAvalista.ListaAvalistas) {
        if (Europa.Controllers.DocumentacaoAvalista.ListaAvalistas[i].Id == idAvalista) {
            return Europa.Controllers.DocumentacaoAvalista.ListaAvalistas[i];
        }
    }
}

Europa.Controllers.DocumentacaoAvalista.AddError = function (fields) {
    fields.forEach(function (key) {
        $("[name='" + key + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.DocumentacaoAvalista.CycleDocumento = function (avancar) {
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

Europa.Controllers.DocumentacaoAvalista.GetArrayItemById = function (arr, id) {
    for (var i = 0, iLen = arr.length; i < iLen; i++) {
        if (arr[i].Id == id) {
            return arr[i];
        }
    }
};

Europa.Controllers.DocumentacaoAvalista.GetParecerPorIdDocumento = function (listaParecer, idDocumento) {
    for (var i = 0; i < listaParecer.length; i++) {
        if (listaParecer[i].DocumentoAvalista.Id == idDocumento) {
            return listaParecer[i];
        }
    }
};

Europa.Controllers.DocumentacaoAvalista.DisableAndClearFields = function () {
    $("#Situacao").val("");
    $("#Parecer").val("");
    $("#Parecer").prop("disabled", true);
    $("#lista_documentos").prop("disabled", true);
    $(".btn-cycle-documentos").prop("disabled", true);
    if (Europa.Controllers.DocumentacaoAvalista.PropostaEmAnalise) {
        $("#btn_aprovar_documento").prop("disabled", true);
        $("#btn_pendenciar_documento").prop("disabled", true);
    }
    $("#ViewerContainer").html("");
    $("#link-exibir-informacoes-auditoria").hide();
};

Europa.Controllers.DocumentacaoAvalista.EnableFields = function (doc) {
    $("#lista_documentos").prop("disabled", false);
    $(".btn-cycle-documentos").prop("disabled", false);
    $("#link-exibir-informacoes-auditoria").show();
    if (doc.Situacao != Europa.Controllers.DocumentacaoAvalista.SituacaoAprovado) {
        $("#Parecer").prop("disabled", false);
    }

    switch (doc.Situacao) {
        case Europa.Controllers.DocumentacaoAvalista.SituacaoAprovado:
        case Europa.Controllers.DocumentacaoAvalista.SituacaoPendente:
        case Europa.Controllers.DocumentacaoAvalista.SituacaoAnexado:
        case Europa.Controllers.DocumentacaoAvalista.SituacaoInformado:
        case Europa.Controllers.DocumentacaoAvalista.SituacaoNaoAnexado:
            $("#btn_pendenciar_documento").prop("disabled", true);
            $("#btn_aprovar_documento").prop("disabled", true);
            $("#Parecer").prop("disabled", true);
            $("#btn_aprovar_documentacao").prop("disabled", true);
            $("#btn_pendenciar_documentacao").prop("disabled", true);
            break;

        case Europa.Controllers.DocumentacaoAvalista.SituacaoEnviado:
            $("#btn_pendenciar_documento").prop("disabled", false);
            $("#btn_aprovar_documento").prop("disabled", false);
            $("#Parecer").prop("disabled", false);
            $("#btn_aprovar_documentacao").prop("disabled", false);
            $("#btn_pendenciar_documentacao").prop("disabled", false);
            break;
    }
};

Europa.Controllers.DocumentacaoAvalista.DisableAndClearFields = function () {
    $("#Situacao").val("");
    $("#Parecer").val("");
    $("#Parecer").prop("disabled", true);
    $("#lista_documentos").prop("disabled", true);
    $(".btn-cycle-documentos").prop("disabled", true);
    if (Europa.Controllers.DocumentacaoAvalista.PropostaEmAnalise) {
        $("#btn_aprovar_documento").prop("disabled", true);
        $("#btn_pendenciar_documento").prop("disabled", true);
    }
    $("#ViewerContainer").html("");
    $("#link-exibir-informacoes-auditoria").hide();
};

Europa.Controllers.DocumentacaoAvalista.SelecionarDocumento = function (select) {
    $(".has-error").removeClass("has-error");
    var idDocumento = select.value;
    if (idDocumento) {
        Europa.Controllers.DocumentacaoAvalista.IdDocumento = idDocumento;
        var doc = Europa.Controllers.DocumentacaoAvalista.GetArrayItemById(Europa.Controllers.DocumentacaoAvalista.ListaDocumentos, idDocumento);
        $("#Situacao").val(Europa.i18n.Enum.Resolve("SituacaoAprovacaoDocumentoAvalista", doc.Situacao));
        $("#Motivo").val(doc.Motivo);
        var parecer = Europa.Controllers.DocumentacaoAvalista.GetParecerPorIdDocumento(Europa.Controllers.DocumentacaoAvalista.ListaParecer, doc.Id);
        if (parecer !== undefined) {
            $("#Parecer").val(parecer.Parecer);
        } else {
            $("#Parecer").val("");
        }
        Europa.Controllers.DocumentacaoAvalista.EnableFields(doc);
        var arquivo = Europa.Controllers.DocumentacaoAvalista.ListaArquivos[idDocumento];
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
        Europa.Controllers.DocumentacaoAvalista.DisableAndClearFields();
        Europa.Controllers.DocumentacaoAvalista.IdDocumento = undefined;
    }
};

Europa.Controllers.DocumentacaoAvalista.PendenciarDocumento = function () {
    $(".has-error").removeClass("has-error");
    var data = {
        IdDocumentoAvalista: Europa.Controllers.DocumentacaoAvalista.IdDocumento,
        Parecer: $("#Parecer").val(),
        IdPreProposta: $("#IdPreProposta").val(),
        TipoDocumento: $('#lista_documentos option[value="' + $('#lista_documentos').val() + '"]').text()
    };
    $.post(Europa.Controllers.DocumentacaoAvalista.UrlPendenciarDocumento,
        data,
        function (res) {
            if (res.Sucesso) {
                for (var i in Europa.Controllers.DocumentacaoAvalista.ListaDocumentos) {
                    if (Europa.Controllers.DocumentacaoAvalista.ListaDocumentos[i].Id == res.Objeto.Id) {
                        Europa.Controllers.DocumentacaoAvalista.ListaDocumentos[i] = res.Objeto;
                        break;
                    }
                }
                if (data.Parecer != null || data.Parecer != undefined || data.Parecer != "") {
                    var parecer = {
                        DocumentoAvalista: { Id: data.IdDocumentoAvalista },
                        Parecer: data.Parecer
                    };
                    Europa.Controllers.DocumentacaoAvalista.ListaParecer.push(parecer);
                }
                var trocouOpcao = Europa.Controllers.DocumentacaoAvalista.SkipToNextDocument(res.Objeto.NomeTipoDocumento);
                if (!trocouOpcao) {
                    return;
                }
            } else {
                Europa.Controllers.DocumentacaoAvalista.AddError(res.Campos);
            }
            Europa.Informacao.PosAcao(res);
        });
};

Europa.Controllers.DocumentacaoAvalista.BaixarTodosDocumentos = function () {
    if ($("#IdAvalista").val() === "" || $("#IdAvalista").val() === undefined || $("#IdAvalista").val() === 0) {
        var msg = {
            Mensagens: Europa.i18n.Mensagens.MsgAvalistaNaoCadastrado
        }

        Europa.Informacao.PosAcao(msg);
        return;
    }   
            
    var params = {
        IdPreProposta: $("#IdPreProposta").val(), IdAvalista: $("#IdAvalista").val(), NomeAvalista: $("#NomeAvalista").val()
    };

    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.DocumentacaoAvalista.UrlBaixarTodosDocumentos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

// Aprovar Avalista
Europa.Controllers.DocumentacaoAvalista.AprovarDocumentacao = function () {
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarEtapa2, Europa.i18n.Messages.PreAprovarAvalista));
    Europa.Confirmacao.ConfirmCallback = function () {
        var documento = {
            IdPreProposta: $("#IdPreProposta").val(),
            IdAvalista:$("#IdAvalista").val()
        }

        $.post(Europa.Controllers.DocumentacaoAvalista.UrlAprovarAvalista, documento, function (res) {
            Europa.Informacao.PosAcao(res)
        });
    }
    Europa.Confirmacao.Show();
};

// Pendenciar Avalista
Europa.Controllers.DocumentacaoAvalista.PendenciarDocumentacao = function () {
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarEtapa2, Europa.i18n.Messages.PendenciaAvalista));
    Europa.Confirmacao.ConfirmCallback = function () {
        var documento = {
            IdPreProposta: $("#IdPreProposta").val(),
            IdAvalista: $("#IdAvalista").val(),
            Motivo: $("#Motivo").val()
        }

        $.post(Europa.Controllers.DocumentacaoAvalista.UrlPendenciarAvalista, documento, function (res) {
            Europa.Informacao.PosAcao(res)
        });
    }
    Europa.Confirmacao.Show();
};