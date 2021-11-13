"use strict";

Europa.Controllers.DocumentacaoPreProposta = {};
Europa.Controllers.DocumentacaoPreProposta.SituacaoNaoAnexado = 1;
Europa.Controllers.DocumentacaoPreProposta.SituacaoAnexado = 2;
Europa.Controllers.DocumentacaoPreProposta.SituacaoAprovado = 3;
Europa.Controllers.DocumentacaoPreProposta.SituacaoPendente = 4;
Europa.Controllers.DocumentacaoPreProposta.SituacaoInformado = 5;
Europa.Controllers.DocumentacaoPreProposta.PropostaEmAnalise = undefined;
Europa.Controllers.DocumentacaoPreProposta.ListaProponentes = undefined;
Europa.Controllers.DocumentacaoPreProposta.ListaDocumentos = undefined;
Europa.Controllers.DocumentacaoPreProposta.ListaArquivos = undefined;
Europa.Controllers.DocumentacaoPreProposta.ListaParecer = undefined;
Europa.Controllers.DocumentacaoPreProposta.IdDocumento = undefined;
Europa.Controllers.DocumentacaoPreProposta.ExibindoInfo = false;
Europa.Controllers.DocumentacaoPreProposta.UrlAprovarDocumento = undefined;
Europa.Controllers.DocumentacaoPreProposta.UrlPendenciarDocumento = undefined;
Europa.Controllers.DocumentacaoPreProposta.UrlBaixarTodosDocumentos = undefined;
Europa.Controllers.DocumentacaoPreProposta.AlterarSicaq = {};
Europa.Controllers.GrupoCCA = {};
Europa.Controllers.DocumentacaoPreProposta.Modal = {};
Europa.Controllers.DocumentacaoPreProposta.Modal.AlterarCCA = {};
Europa.Controllers.DocumentacaoPreProposta.Permissoes = {};

$(document).ready(function () {
    // Herdando configurações já efetuadas, já que este método só é executado após a página ser carregada.
    // Se não fizer isso, sobrescrevo todas as propriedades setadas anteriormente (geralmente URLs e permissÕes)
    var self = Europa.Controllers.DocumentacaoPreProposta;

    self.Init = function () {
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
            $("#Situacao").val(Europa.i18n.Enum.Resolve("SituacaoAprovacaoDocumento", doc.Situacao));
            $("#Motivo").val(doc.Motivo);
            var parecer = self.GetParecerPorIdDocumento(self.ListaParecer, doc.Id);
            if (parecer !== undefined) {
                $("#Parecer").val(parecer.Parecer);
            } else {
                $("#Parecer").val("");
            }
            self.InitDatePicker(doc.DataExpiracao);
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

    self.AprovarDocumento = function () {
        $(".has-error").removeClass("has-error");
        var data = {
            IdDocumentoProponente: self.IdDocumento,
            DataExpiracao: $("#DataExpiracao").val(),
            Parecer: $("#Parecer").val(),
            ExisteDocumento: $("#ExisteDocumento").val()
        };
        $.post(self.UrlAprovarDocumento,
            data,
            function (res) {
                if (res.Sucesso) {
                    for (var i in self.ListaDocumentos) {
                        if (self.ListaDocumentos[i].Id == res.Objeto.Id) {
                            self.ListaDocumentos[i] = res.Objeto;
                            break;
                        }
                    }
                    if (data.Parecer != null || data.Parecer != undefined || data.Parecer != "") {
                        var parecer = {
                            DocumentoProponente: { Id: data.IdDocumentoProponente },
                            Parecer: data.Parecer
                        };
                        self.ListaParecer.push(parecer);
                    }
                    var trocouOpcao = self.SkipToNextDocument(res.Objeto.Proponente.Cliente.NomeCompleto);
                    if (!trocouOpcao) {
                        return;
                    }
                } else {
                    self.AddError(res.Campos);
                }
                Europa.Informacao.PosAcao(res);
            });
    };

    self.PendenciarDocumento = function () {
        $(".has-error").removeClass("has-error");
        var data = {
            IdDocumentoProponente: self.IdDocumento,
            DataExpiracao: $("#DataExpiracao").val(),
            Parecer: $("#Parecer").val(),
            IdPreProposta: $("#IdPreProposta").val(),
            TipoDocumento: $('#lista_documentos option[value="' + $('#lista_documentos').val() + '"]').text()
        };
        $.post(self.UrlPendenciarDocumento,
            data,
            function (res) {
                if (res.Sucesso) {
                    for (var i in self.ListaDocumentos) {
                        if (self.ListaDocumentos[i].Id == res.Objeto.Id) {
                            self.ListaDocumentos[i] = res.Objeto;
                            break;
                        }
                    }
                    if (data.Parecer != null || data.Parecer != undefined || data.Parecer != "") {
                        var parecer = {
                            DocumentoProponente: { Id: data.IdDocumentoProponente },
                            Parecer: data.Parecer
                        };
                        self.ListaParecer.push(parecer);
                    }
                    var trocouOpcao = self.SkipToNextDocument(res.Objeto.Proponente.Cliente.NomeCompleto);
                    if (!trocouOpcao) {
                        return;
                    }
                } else {
                    self.AddError(res.Campos);
                }
                Europa.Informacao.PosAcao(res);
            });
    };

    self.AtualizarViewModel = function (idPreProposta) {
        var somenteSemAnalise = $("#somente_nao_analisados").is(":checked");
        $.post(Europa.Controllers.DocumentacaoPreProposta.UrlAtualizarViewModel, { idPreProposta, somenteSemAnalise }, function (res) {
            self.ListaParecer = res.Pareceres;
            self.ListaDocumentos = res.Documentos;
            var select = document.getElementById("lista_proponentes");
            self.VerificarSituacaoDocumentos();
            $(select).trigger('change');
        });
    };

    self.BaixarTodosDocumentos = function () {
        var codigoUf = window.location.href;
        codigoUf = codigoUf.toString().split('=')[2];
        var params = { idPreProposta: $("#IdPreProposta").val(), codigoUf: codigoUf };
        var formExportar = $("#Exportar");
        $("#Exportar").find("input").remove();
        formExportar.attr("method", "post").attr("action", self.UrlBaixarTodosDocumentos);
        formExportar.addHiddenInputData(params);
        formExportar.submit();
    };

    self.SkipToNextDocument = function (nomeProponente) {
        self.AtualizarViewModel($("#IdPreProposta").val());
        var trocouOpcao = false;
        var idProponente = $("#lista_proponentes").val();
        var todosAnalisados = true;
        for (var i in self.ListaDocumentos) {
            if (self.ListaDocumentos[i].Situacao == 2 ||
                self.ListaDocumentos[i].Situacao == 5 && self.ListaDocumentos[i].Proponente.Id == $("#lista_proponentes").val()) {
                self.ListaDocumentos[i]
                $("#lista_documentos > option").each(function () {
                    var opId = this.value;
                    var idDocumento = self.ListaDocumentos[i].Id;
                    if (opId == idDocumento) {
                        $("#lista_documentos").val(opId);
                        trocouOpcao = true;
                    }
                });
                break;
            }
        }

        $("#lista_documentos").trigger('change');
        for (var i in self.ListaDocumentos) {
            if (self.ListaDocumentos[i].Situacao != 3 && self.ListaDocumentos[i].Situacao != 4 && self.ListaDocumentos[i].Proponente.Id == idProponente) {
                todosAnalisados = false;
            }
        }
        if (todosAnalisados) {
            var proponente = self.GetProponenteSelecionado();
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao,
                Europa.String.Format(Europa.i18n.Messages.TodosDocumentosDoProponenteEstaoValidados, proponente.Cliente.NomeCompleto));
            Europa.Informacao.Show();
        }
        return trocouOpcao;
    };

    self.VerificarSituacaoDocumentos = function () {
        var idProponente = $("#lista_proponentes").val();
        var somenteSemAnalise = $("#somente_nao_analisados").is(":checked");
        if (idProponente !== undefined && idProponente !== "" && somenteSemAnalise) {
            var existeDocumentoNaoAnalisado = false;
            for (var i = 0; i < self.ListaDocumentos.length; i++) {
                if (self.ListaDocumentos[i].Proponente.Id == idProponente && self.ListaDocumentos[i].Situacao != 3 && self.ListaDocumentos[i].Situacao != 4) {
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

    self.GetProponenteSelecionado = function () {
        var idProponente = $("#lista_proponentes").val();
        for (var i in self.ListaProponentes) {
            if (self.ListaProponentes[i].Id == idProponente) {
                return self.ListaProponentes[i];
            }
        }
    }

    self.CycleProponente = function (avancar) {
        var select = document.getElementById("lista_proponentes");
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
        self.VerificarSituacaoDocumentos();
        $(select).trigger('change');
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

    self.DisableAndClearFields = function () {
        $("#Situacao").val("");
        $("#Parecer").val("");
        $("#DataExpiracao").val("");
        $("#DataExpiracao").prop("disabled", true);
        $("#Parecer").prop("disabled", true);
        $("#lista_documentos").prop("disabled", true);
        $(".btn-cycle-documentos").prop("disabled", true);
        if (self.PropostaEmAnalise) {
            $("#btn_aprovar_documento").prop("disabled", true);
            $("#btn_pendenciar_documento").prop("disabled", true);
        }
        $("#ViewerContainer").html("");
        $("#link-exibir-informacoes-auditoria").hide();
    };

    self.InitDatePicker = function (dataExpiracao) {
        new Europa.Components.DatePicker()
            .WithTarget('#DataExpiracao')
            .WithFormat("DD/MM/YYYY")
            .WithMinDate(new Date())
            .WithValue(Europa.Date.toGeenDateFormat(dataExpiracao))
            .Configure();
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

    self.AddError = function (fields) {
        fields.forEach(function (key) {
            $("[name='" + key + "']").parent().addClass("has-error");
        });
    };

    // Analisar Pré Proposta
    self.IniciarAnalise = function () {
        Europa.Informacao.Hide = function () {
            location.reload(true);
        }
        Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
            Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_EmAnaliseSimplificada));
        Europa.Confirmacao.ConfirmCallback = function () {
            var idPreProposta = $("#IdPreProposta").val();
            var response = Europa.Controllers.PrePropostaWorkflow.Analisar(idPreProposta);
            Europa.Informacao.PosAcao(response);
        }
        Europa.Confirmacao.Show();
    };

    self.ExibirInformacoesDeAuditoria = function () {
        var arquivo = self.ListaArquivos[self.IdDocumento];

        if (arquivo != undefined && arquivo.Metadados != '' && arquivo.Metadados != null) {
            $("#conteudo-modal-informacoes-auditoria").text(arquivo.Metadados);
            $("#modal-informacoes-auditoria").modal("show");
        } else {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.i18n.Messages.NaoExistemMetadadosParaEsteArquivo);
            Europa.Informacao.Show();
        }
    };

    // Aprovar Pré Proposta
    self.AprovarDocumentacao = function () {
        Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
            Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_AnaliseSimplificadaAprovada));
        Europa.Confirmacao.ConfirmCallback = function () {
            var idPreProposta = $("#IdPreProposta").val();
            var response = Europa.Controllers.PrePropostaWorkflow.Aprovar(idPreProposta);
            if (response.Sucesso == true) {
                Europa.Informacao.Hide = function () {
                    location.reload(true);
                }
            }
            Europa.Informacao.PosAcao(response);
        }
        Europa.Confirmacao.Show();
    };

    // Aprovação Pré Proposta
    self.AprovarDocumentacaoFinal = function () {
        Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
            Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_AguardandoIntegracao));
        Europa.Confirmacao.ConfirmCallback = function () {
            var idPreProposta = $("#IdPreProposta").val();
            var response = Europa.Controllers.PrePropostaWorkflow.Aprovar(idPreProposta);
            if (response.Sucesso == true) {
                Europa.Informacao.Hide = function () {
                    location.reload(true);
                }
            }
            Europa.Informacao.PosAcao(response);
        }
        Europa.Confirmacao.Show();
    };

    // Pendenciar Pré Proposta
    self.PendenciarDocumentacao = function () {
        Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
            Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_DocsInsuficientesSimplificado));
        Europa.Confirmacao.ConfirmCallback = function () {
            var idPreProposta = $("#IdPreProposta").val();
            var response = Europa.Controllers.PrePropostaWorkflow.Pendenciar(idPreProposta);

            if (response.Sucesso == true) {
                Europa.Informacao.Hide = function () {
                    location.reload(true);
                }
            }

            Europa.Informacao.PosAcao(response);
        }
        Europa.Confirmacao.Show();
    };

    // Cancelar Pré Proposta
    self.CancelarPreProposta = function () {
        var codigo = $('#Codigo').val();
        Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.CancelarPreProposta, Europa.i18n.Messages.MsgCancelarPreProposta, Europa.i18n.Messages.Confirmar, function () {
            var idPreProposta = $("#IdPreProposta").val();
            var response = Europa.Controllers.PrePropostaWorkflow.Cancelar(idPreProposta);

            if (response.Sucesso == true) {
                Europa.Informacao.Hide = function () {
                    location.reload(true);
                }
            }
            Europa.Informacao.PosAcao(response);
        });
        Europa.Confirmacao.Show();
    };

    self.AlternarExibicaoInformacoes = function () {
        if (self.ExibindoInfo) {
            $('.info-proposta').each(function () { $(this).hide(); });
            $('#btn-show-hide-info').html('Ver mais informações');
        } else {
            $('.info-proposta').each(function () { $(this).fadeIn('slow'); });
            setTimeout(function () { $('#btn-show-hide-info').html('Ver menos informações'); }, 400);
        }
        self.ExibindoInfo = !self.ExibindoInfo;
    };

    // Garantindo referencia
    Europa.Controllers.DocumentacaoPreProposta = self;
    Europa.Controllers.DocumentacaoPreProposta.Init();

    if ($('#Codigo').is('[readonly="readonly"]')) {
        Europa.AddSubstituteFieldTo($('#Codigo'));
    }

    if ($('#Cliente').is('[readonly="readonly"]')) {
        Europa.AddSubstituteFieldTo($('#Cliente'));
    }

});

///SICAQ
Europa.Controllers.DocumentacaoPreProposta.AbrirModalSicaq = function () {
    var idPreProposta = $("#IdPreProposta").val();

    $("#btn_salvar_previo").addClass("hidden");
    $("#btn_salvar").removeClass("hidden");

    Europa.Controllers.DocumentacaoPreProposta.AlterarSicaq.AbrirModal(idPreProposta);

};

Europa.Controllers.DocumentacaoPreProposta.AbrirModalSicaqPrevio = function () {
    var idPreProposta = $("#IdPreProposta").val();

    $("#btn_salvar_previo").removeClass("hidden");
    $("#btn_salvar").addClass("hidden");
    ;
    Europa.Controllers.DocumentacaoPreProposta.AlterarSicaq.AbrirModalPrevio(idPreProposta);

};

////SICAQ

// Aguardando Auditoria
Europa.Controllers.DocumentacaoPreProposta.AguardandoAuditoria = function () {
    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_AguardandoAuditoria));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.AguardandoAuditoria(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

// Retorno Auditoria
Europa.Controllers.DocumentacaoPreProposta.RetornoAuditoria = function () {
    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa, "Retornar proposta da auditoria");
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.RetornoAuditoria(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

// Em Analise Completa
Europa.Controllers.DocumentacaoPreProposta.EmAnaliseCompleta = function () {
    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_EmAnaliseCompleta));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.EmAnaliseCompleta(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

//Modal Formulario
Europa.Controllers.DocumentacaoPreProposta.AbrirModalFormulario = function () {
    $("#modal-formulario").show();
}

Europa.Controllers.DocumentacaoPreProposta.BaixarFormularios = function () {
    location.href = Europa.Controllers.DocumentacaoPreProposta.UrlBaixarFormularios + "?idPreProposta=" + $("#Id").val();

}

Europa.Controllers.DocumentacaoPreProposta.AguardandoIntegracao = function () {
    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_AguardandoIntegracao));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.AguardandoIntegracao(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

Europa.Controllers.DocumentacaoPreProposta.AnaliseCompletaAprovada = function () {
    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_AnaliseCompletaAprovada));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.AnaliseCompletaAprovada(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
}

Europa.Controllers.DocumentacaoPreProposta.DocsInsuficientesSimplificado = function () {
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_DocsInsuficientesSimplificado));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#IdPreProposta").val();
        var response = Europa.Controllers.PrePropostaWorkflow.DocsInsuficientesSimplificado(idPreProposta);

        if (response.Sucesso == true) {
            Europa.Informacao.Hide = function () {
                location.reload(true);
            }
        }

        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

Europa.Controllers.DocumentacaoPreProposta.DocsInsuficientesCompleta = function () {
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_DocsInsuficientesCompleta));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#IdPreProposta").val();
        var response = Europa.Controllers.PrePropostaWorkflow.DocsInsuficientesCompleta(idPreProposta);

        if (response.Sucesso == true) {
            Europa.Informacao.Hide = function () {
                location.reload(true);
            }
        }

        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

//Trocar CCA
Europa.Controllers.DocumentacaoPreProposta.TrocarCCA = function () {
    $('#IdPreProposta').val(getUrlParameter("id"));
    Europa.Controllers.DocumentacaoPreProposta.CCAsPPR.Tabela.reloadData();
    $('#alterar_cca_modal').show();
    $.post(Europa.Controllers.DocumentacaoPreProposta.UrlListarCCA, { IdPreProposta: $('#IdPreProposta').val() }, function (data) {
        console.log(data);
        var html = '';
        html += '<option value="" selected>Selecione um CCA...</option>'
        $.each(data, function (index, obj) {
            html += '<option value="' + obj.Id + '">' + obj.Descricao + '</option>';
        });
        $("#select-novo-cca").html(html);
        $("#select-novo-cca").select2({
            trags: true
        });
    });
};

Europa.Controllers.DocumentacaoPreProposta.Modal.AlterarCCA.FecharModal = function () {
    $('#alterar_cca_modal').hide();
    $('#error-label').html("");
    $("select-novo-cca").val("").change();
};


Europa.Controllers.DocumentacaoPreProposta.Modal.AlterarCCA.Alterar = function () {
    var filtro = Europa.Controllers.DocumentacaoPreProposta.Modal.AlterarCCA.Filtro();
    if (filtro.IdCCADestino == "" || filtro.IdCCADestino == 0) {
        $('#error-label').html("É necessário selecionar um CCA de destino!");
    } else {
        $.post(Europa.Controllers.DocumentacaoPreProposta.UrlAlterarCCA, filtro, function (res) {
            if (res.Sucesso) {
                Europa.Controllers.DocumentacaoPreProposta.Modal.AlterarCCA.FecharModal();
                var abc = Europa.Messages.ShowMessages(res, Europa.i18n.Messages.Sucesso);
                $('#info-alert :button').click(function () {
                    Europa.Informacao.Hide();
                    window.location.href = "./prepropostaaguardandoanalise";
                });
               // window.location.href = "./prepropostaaguardandoanalise"
            } else {
                $('#error-label').html(res.Mensagens);
            }
        });
    }

};

Europa.Controllers.DocumentacaoPreProposta.Modal.AlterarCCA.Filtro = function () {
    return {
        IdCCADestino: $("#select-novo-cca").val(),
        IdCCAOrigem: 0,
        IdPreProposta: $('#IdPreProposta').val(),
    };
};

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return typeof sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
    return false;
};