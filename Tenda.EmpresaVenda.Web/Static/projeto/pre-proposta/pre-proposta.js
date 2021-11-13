"use strict";

Europa.Controllers.PreProposta = {};
Europa.Controllers.PreProposta.IdPreProposta = {};
Europa.Controllers.PreProposta.Dto = {};

// Injetado via outros JS
Europa.Controllers.PreProposta.Proponente = {};
Europa.Controllers.PreProposta.DetalhamentoFinanceiro = {};
Europa.Controllers.PreProposta.ItbiEmolumento = {};
Europa.Controllers.PreProposta.DocumentoProponente = {};

Europa.Controllers.PreProposta.AutoCompleteBreveLancamento = {};

$(document).ready(function () {
    // Herdando configurações já efetuadas, já que este método só é executado após a página ser carregada.
    // Se não fizer isso, sobrescrevo todas as propriedades setadas anteriormente (geralmente URLs e permissÕes)
    var self = Europa.Controllers.PreProposta;

    // Colocar todas as informações no contexto JS
    Europa.Controllers.PreProposta.Dto = null;

    Europa.Controllers.PreProposta.DocumentoProponente.Init();
    Europa.Controllers.PreProposta.Proponente.Init();
    Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Init();
    Europa.Controllers.PreProposta.ItbiEmolumento.Init();
    Europa.Controllers.PreProposta.EnableModalIndicacao();

    self.IdPreProposta = $('#PreProposta_Id').val();
    self.Init = function () {
        self.DocumentoProponente.Configure(self.IdPreProposta);
        self.Proponente.Configure(self.IdPreProposta);
        self.DetalhamentoFinanceiro.Configure(self.IdPreProposta);
        self.ItbiEmolumento.Configure(self.IdPreProposta);
    };
    self.SelecionarProposta = function () {
        Europa.Controllers.PreProposta.Proponente.Configure(self.IdPreProposta);
        Europa.Controllers.PreProposta.DocumentoProponente.Configure(self.IdPreProposta);
        Europa.Controllers.PreProposta.ItbiEmolumento.Configure(self.IdPreProposta);
        Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Configure(self.IdPreProposta);
    };

    self.BaixarTodosDocumentos = function () {
        var params = { idPreProposta: $("#PreProposta_Id").val() };
        var formExportar = $("#Exportar");
        $("#Exportar").find("input").remove();
        formExportar.attr("method", "post").attr("action", self.UrlBaixarTodosDocumentos);
        formExportar.addHiddenInputData(params);
        formExportar.submit();
    };

    self.BaixarBoleto = function () {
        var params = {
            idProposta: $("#PreProposta_IdSuat").val(),
            idPreProposta: $("#PreProposta_Id").val()
        };

        $.post(Europa.Controllers.PreProposta.UrlPreBaixarBoleto, params, function (response) {
            if (!response.Sucesso) {
                Europa.Informacao.PosAcao(response);
            } else {
                var formExportar = $("#Exportar");
                $("#Exportar").find("input").remove();
                formExportar.attr("method", "post").attr("action", self.UrlBaixarBoleto);
                formExportar.addHiddenInputData(params);
                formExportar.submit();
            }
        });
    };

    self.BaixarContrato = function () {
        var params = {
            idProposta: $("#PreProposta_IdSuat").val(),
            idPreProposta: $("#PreProposta_Id").val()
        };

        $.post(Europa.Controllers.PreProposta.UrlPreBaixarContrato, params, function (response) {
            if (!response.Sucesso) {
                Europa.Informacao.PosAcao(response);
            } else {
                var formExportar = $("#Exportar");
                $("#Exportar").find("input").remove();
                formExportar.attr("method", "post").attr("action", self.UrlBaixarContrato);
                formExportar.addHiddenInputData(params);
                formExportar.submit();
            }
        });
    };

    // Cancelar Pré-Proposta
    self.Cancelar= function () {
        Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.CancelarPreProposta, Europa.i18n.Messages.MsgCancelarPreProposta, Europa.i18n.Messages.Confirmar,
            function () {
                var idPreProposta = $("#PreProposta_Id").val();
                var response = Europa.Controllers.PrePropostaWorkflow.Cancelar(idPreProposta);

                if (response.Sucesso == true) {
                    Europa.Informacao.Hide = function () {
                        location.reload(true);
                    }
                }
                Europa.Informacao.PosAcao(response);
            }
        );
        Europa.Confirmacao.Show();
    };


    //Aprovar Pré-Proposta
    self.Aprovar = function () {
        var codigo = $('#PreProposta_Codigo').val();
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Aprovar, codigo,
            function () {
                var idPreProposta = $("#PreProposta_Id").val();
                var response = Europa.Controllers.PrePropostaWorkflow.Aprovar(idPreProposta);

                if (response.Sucesso == true) {
                    Europa.Informacao.Hide = function () {
                        location.reload(true);
                    }
                }
                Europa.Informacao.PosAcao(response);
            });
        Europa.Confirmacao.Show();
    };

    // Retroceder Pré Proposta
    self.Retroceder = function () {
        Europa.Informacao.Hide = function () {
            location.reload(true);
        }
        Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa, Europa.i18n.Messages.MsgConfirmacaoRetrocederPreProposta);
        Europa.Confirmacao.ConfirmCallback = function () {
            var idPreProposta = $("#PreProposta_Id").val();
            var response = Europa.Controllers.PrePropostaWorkflow.Retroceder(idPreProposta);

            if (response.Sucesso == true) {
                Europa.Informacao.Hide = function () {
                    location.reload(true);
                }
            }

            Europa.Informacao.PosAcao(response);
        }
        Europa.Confirmacao.Show();
    };

    // Finalizar Pré Proposta
    self.Finalizar = function () {
        return;
        Europa.Informacao.Hide = function () {
            location.reload(true);
        }
        Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
            Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_AguardandoIntegracao));
        Europa.Confirmacao.ConfirmCallback = function () {
            var idPreProposta = $("#PreProposta_Id").val();
            var response = Europa.Controllers.PrePropostaWorkflow.Finalizar(idPreProposta);
            Europa.Informacao.PosAcao(response);
        }
        Europa.Confirmacao.Show();
    };

    //Aguardar Fluxo
    self.AguardarFluxo = function () {
        Europa.Informacao.Hide = function () {
            location.reload(true);
        };
        Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa, Europa.i18n.Messages.MsgConfirmacaoRetrocederPreProposta);
        Europa.Confirmacao.ConfirmCallback = function () {
            var idPreProposta = $("#PreProposta_Id").val();
            var response = Europa.Controllers.PrePropostaWorkflow.AguardarFluxo(idPreProposta);
            Europa.Informacao.PosAcao(response);
        };
        Europa.Confirmacao.Show();
    };


    // Garantindo referencia    
    Europa.Controllers.PreProposta = self;
    Europa.Controllers.PreProposta.Init();

    Europa.Controllers.PreProposta.InitComponents();
    $('#autocomplete_breve_lancamento').attr('readonly', 'readonly');

    if ($('#PreProposta_Cliente_NomeCompleto').is('[readonly="readonly"]')) {
        Europa.AddSubstituteFieldTo($('#PreProposta_Cliente_NomeCompleto'));
    }

    //status suat evs
    Europa.Controllers.PreProposta.StatusSuatEvs();

    Europa.Controllers.PreProposta.ConfigureBreveLancamentoAutocomplete(Europa.Controllers.PreProposta);
    Europa.Controllers.PreProposta.ConfigureTorreAutocomplete(Europa.Controllers.PreProposta);

    //$("#autocomplete_breve_lancamento").val($("#PreProposta_BreveLancamento_Id").val())

    Europa.Controllers.PreProposta.AutoCompleteTorre.SetValue($("#PreProposta_IdTorre").val(), $("#PreProposta_NomeTorre").val());

});

Europa.Controllers.PreProposta.AplicarMascaras = function () {
    Europa.Mask.ApplyByClass("dinheiro", Europa.Mask.FORMAT_MONEY, undefined, true);
    Europa.Mask.CpfCnpj("#PreProposta_CpfIndicador");
}

Europa.Controllers.PreProposta.PodeManterAssociacoes = function () {
    var value = $('#PodeManterAssociacoes').val();
    return 'true' === value || 'True' === value || '1' === value || 1 === value; 
};

Europa.Controllers.PreProposta.InitComponents = function () {
    // Inicializa o autocomplete de Breve Lançamento
    Europa.Controllers.PreProposta.InitAutocomplete();
    // Força preenchimento do campo de Ponto de Venda
    Europa.Controllers.PreProposta.ChangePontoVenda();
    // Força preenchimento do campo de Breve Lançamento
    Europa.Controllers.PreProposta.ChangeBreveLancamento();

    $("#PreProposta_PontoVenda").val($("#PreProposta_PontoVenda_Id").val());

    Europa.Controllers.PreProposta.TratarHabilitacaoCampos();

    Europa.Controllers.PreProposta.AplicarMascaras();
};

Europa.Controllers.PreProposta.InitAutocomplete = function () {
    Europa.Controllers.PreProposta.AutoCompleteBreveLancamento = new Europa.Components.AutoCompleteBreveLancamentoListarPorRegionalDisponiveisParaCatalogoNoEstado()
        .WithTargetSuffix("breve_lancamento")
        .Configure();

    Europa.Controllers.PreProposta.AutoCompleteCorretor = new Europa.Components.AutoCompleteCorretorEmpresaVenda()
        .WithTargetSuffix("corretor")
        .Configure();

    Europa.Controllers.PreProposta.AutoCompleteReenviarBreveLancamento = new Europa.Components.AutoCompleteBreveLancamento()
        .WithAjax(false)
        .WithTargetSuffix("reenviar_breve_lancamento")
        .Configure();

    Europa.Controllers.PreProposta.AutoCompleteTorre = new Europa.Components.AutoCompleteTorre()
        .WithTargetSuffix("torre")
        .WithParamName("NomeTorre")
        .Configure();

};

Europa.Controllers.PreProposta.SelectCliente = function (data) {
    $.ajax({
        url: Europa.Controllers.PreProposta.UrlBuscarCliente,
        method: 'POST',
        data: { idCliente: data }
    })
    .done(function (response) {
        if (response.Sucesso) {
            var cliente = response.Objeto;
            $("[name='PreProposta.Cliente.NomeCompleto']").val(cliente.NomeCompleto);
            $("[name='PreProposta.Cliente.Id']").val(cliente.Id);
        } else {
            Europa.Informacao.PosAcao(response);
        }
    });
};

Europa.Controllers.PreProposta.RefreshTotalFinanceiro = function () {
    var requestContent = {
        idPreProposta: $('#PreProposta_Id').val()
    };
    $.post(Europa.Controllers.PreProposta.UrlTotalFinanceiro, requestContent, function (response) {
        $('#area_totais').html(response);
        Europa.Controllers.PreProposta.AplicarMascaras();
    });
};

Europa.Controllers.PreProposta.ChangePontoVenda = function () {
    $("[name='PreProposta.PontoVenda.Id']").val($("#PreProposta_PontoVenda").val());
};

Europa.Controllers.PreProposta.ChangeBreveLancamento = function () {
    var id = $("#PreProposta_BreveLancamento_Id").val();
    var text = $("#PreProposta_BreveLancamento_Nome").val();
    if (id != undefined && id > 0) {
        Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.SetValue(id, text)
    }
};

Europa.Controllers.PreProposta.TratarHabilitacaoCampos = function () {
    // Habilita a edição caso seja uma nova proposta
    $('#fieldset_pre_proposta :input').each(function (index, element) {
        $(element).attr("readonly", "readonly");
    });
    $('#fieldset_pre_proposta :radio').each(function (index, element) {
        $(element).attr("disabled", "disabled");
    });
    $('#PreProposta_SituacaoProposta').attr("style", "pointer-events: none;");
    $('#PreProposta_PontoVenda').attr("style", "pointer-events: none;");
    $('#PreProposta_OrigemCliente').attr("style", "pointer-events: none;");
    $('#btn_select_cliente').attr("disabled", "disabled");
    Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Disable();
    Europa.Controllers.PreProposta.AutoCompleteCorretor.Disable();
    if (Europa.Controllers.PreProposta.IdPreProposta == undefined || Europa.Controllers.PreProposta.IdPreProposta == 0) {
        $('#form_pre_proposta input[type="radio"]').each(function () {
            $(this).prop('checked', false);
        });
    }
};

Europa.Controllers.PreProposta.AbrirModalHistorico = function () {
    $("#modal-historico-preproposta").modal('show');
    Europa.Controllers.PreProposta.Historico.Filtrar();
};

Europa.Controllers.PreProposta.StatusSuatEvs = function () {
    var idPreProposta = $("#PreProposta_Id").val();
    $.ajax({
        type: 'POST',
        url: Europa.Controllers.PreProposta.UrlStatusSuatEvs,
        data: { idPreProposta: idPreProposta },
        async: false
    }).done(function (res) {
        $("#SituacaoPropostaEvs").val(res);
    });

};

Europa.Controllers.PreProposta.AbrirModalReintegrarPrePropostaConfirma = function () {
    $("#btn_reenviar_suat").val("true");
    var fatorSocial = $("#fator-social-confirma-modal").val();
    var faixaUmMeio = $("#faixa-um-meio-confirma").val();

    if (fatorSocial == 'True') {
        $("#fator-social-confirma-modal").val(Europa.i18n.Messages.Sim);
    }
    if (fatorSocial == 'False') {
        $("#fator-social-confirma-modal").val(Europa.i18n.Messages.Nao);
    }
    if (faixaUmMeio == 'True') {
        $("#faixa-um-meio-confirma").val(Europa.i18n.Messages.Sim);
    }
    if (faixaUmMeio == 'False') {
        $("#faixa-um-meio-confirma").val(Europa.i18n.Messages.Nao);
    }

    $("#fator_social_confirma_modal").modal("show");
};

Europa.Controllers.PreProposta.AbrirModalIntegrarPrePropostaConfirma = function () {
    var fatorSocial = $("#fator-social-confirma-modal").val();
    var faixaUmMeio = $("#faixa-um-meio-confirma").val();

    if (fatorSocial == 'True') {
        $("#fator-social-confirma-modal").val(Europa.i18n.Messages.Sim);
    }
    if (fatorSocial == 'False') {
        $("#fator-social-confirma-modal").val(Europa.i18n.Messages.Nao);
    }
    if (faixaUmMeio == 'True') {
        $("#faixa-um-meio-confirma").val(Europa.i18n.Messages.Sim);
    }
    if (faixaUmMeio == 'False') {
        $("#faixa-um-meio-confirma").val(Europa.i18n.Messages.Nao);
    }

    $("#fator_social_confirma_modal").modal("show");
};

Europa.Controllers.PreProposta.AbrirModalReenviarSuat = function () {
    var bnt = $("#btn_reenviar_suat").val();
    var prePropostaIdUnidadeSuat = $("#PreProposta_IdUnidadeSuat").val();
    $("#fator_social_confirma_modal").modal("hide");

    if (bnt == "true") {
        $("#modal-reenviar").modal("show");
    } else {
        if (prePropostaIdUnidadeSuat == 0 || prePropostaIdUnidadeSuat == undefined || prePropostaIdUnidadeSuat == null) {
            Europa.Components.ConsultaEstoqueModalEmpreendimento.AbrirModal();
        } else {
            Europa.Components.ConsultaEstoqueModalUnidade.Integrar();
        }
    }
};

Europa.Controllers.PreProposta.FecharModalReenviarSuat = function () {
    $("#modal-reenviar").modal("hide");
};

Europa.Controllers.PreProposta.ReenviarSuat = function () {
    var idPreProposta = $("#PreProposta_Id").val();
    var idBreveLancamento = $("#autocomplete_reenviar_breve_lancamento").val();
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.i18n.Messages.MsgConfirmacaoReintegracao);
    Europa.Confirmacao.ConfirmCallback = function () {
        $.post(Europa.Controllers.PreProposta.UrlReenviarSuat, { idPreProposta: idPreProposta, idBreveLancamento: idBreveLancamento }, function (res) {
            if (res.Sucesso) {
                $("#Empreendimento_Divisao").val(res.Objeto.Divisao);
                $("#area_geral #PreProposta_BreveLancamento_Nome").val(res.Objeto.NomeBreveLancamento);
                $("#form_reenviar #PreProposta_BreveLancamento_Nome").val(res.Objeto.NomeBreveLancamento);
                $("#PreProposta_IdentificadorUnidadeSuat").val(" ");
                $("#novoEmpreendimento").html(" ");
                $("#novoEmpreendimento").append(Europa.i18n.Messages.ConsultaEstoqueEmpreendimento +" - "+ res.Objeto.NomeEmpreendimento);
                Europa.Controllers.PreProposta.FecharModalReenviarSuat();
                Europa.Components.ConsultaEstoqueModalEmpreendimento.AbrirModal();
            }
            else {
                Europa.Informacao.PosAcao(res);
            }
        });

    };
    Europa.Confirmacao.Show();
};

Europa.Controllers.PreProposta.AbrirModalFatorSocial = function () {
    var fatorSocial = $("#PreProposta_FatorSocial").val();
    var faixaUmMeio = $("#PreProposta_FaixaUmMeio_Modal").val();
    if (fatorSocial == 'True') {
        fatorSocial = Europa.i18n.Messages.Sim;
    }
    if (fatorSocial == 'False') {
        fatorSocial = Europa.i18n.Messages.Nao;
    }
    if (faixaUmMeio == 'True') {
        $("#PreProposta_FaixaUmMeio_Modal").val(Europa.i18n.Messages.Sim);;
    }
    if (faixaUmMeio == 'False') {
        $("#PreProposta_FaixaUmMeio_Modal").val(Europa.i18n.Messages.Nao);
    }

    $("#PreProposta_FatorSocial").val(fatorSocial);
    $('#fator_social_modal').modal('show');

};

Europa.Controllers.PreProposta.MudarFatorSocial = function () {
    var param = {
        fatorSocial: $("#novo_fator_social").val(),
        idPreProposta: $("#PreProposta_Id").val()
    };
    $.post(Europa.Controllers.PreProposta.UrlMudarFatorSocial, param, function (res) {
        if (res.Sucesso) {
            $('#fator_social_modal').modal('hide');
            $("#novo_fator_social").val(" ")
            Europa.Informacao.Hide = function () {
                location.reload();
            };
        }
        Europa.Informacao.PosAcao(res);

    });
};

Europa.Controllers.PreProposta.Simulador = function () {
    var idPreProposta = $("#PreProposta_Id").val();
    $.post(Europa.Controllers.PreProposta.UrlMontarUrlSimulador,
        { idPreProposta: idPreProposta }, function (res) {
            if (res.Sucesso) {
                console.log(res.Objeto)
                window.open(res.Objeto, "_blank");
                return;
            } 
            Europa.Informacao.PosAcao(res);
        })
    

    return;
};

Europa.Controllers.PreProposta.ResultadosSimulador = function () {
    $("#modal-simulacao").modal("show");
    setTimeout(function () { $(".modal-backdrop").remove() }, 300);
};

Europa.Controllers.PreProposta.SalvarBreveLancamento = function () {
    var idPreProposta = $("#PreProposta_Id").val();
    var idBreveLancamento = Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Value();
    var idTorre = Europa.Controllers.PreProposta.AutoCompleteTorre.Value();
    var nomeTorre = Europa.Controllers.PreProposta.AutoCompleteTorre.Text();
    var observacaoTorre = $("#PreProposta_ObservacaoTorre").val();

    var data = {
        IdPreProposta: idPreProposta,
        IdBreveLancamento: idBreveLancamento,
        IdTorre: idTorre,
        ObservacaoTorre: observacaoTorre,
        NomeTorre:nomeTorre
    };

    var result = undefined;
    $.ajax({
        type: "POST",
        url: Europa.Controllers.PreProposta.UrlSalvarBreveLancamento,
        async: false,
        data: data
    }).done(function (response) {
        result = response;
    });
    return result;
};

//Workflow
//Aguardando Análise Completa
Europa.Controllers.PreProposta.AguardandoAnaliseCompleta = function () {

    var response = Europa.Controllers.PreProposta.SalvarBreveLancamento()

    if (!response.Sucesso) {
        Europa.Informacao.PosAcao(response);
        console.log("DEU RUIM")
        return
    }

    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_AguardandoAnaliseCompleta));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#PreProposta_Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.AguardandoAnaliseCompleta(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

//Workflow
//Aguardando Integração
Europa.Controllers.PreProposta.AguardandoIntegracao = function () {

    var response = Europa.Controllers.PreProposta.SalvarBreveLancamento()

    if (!response.Sucesso) {
        Europa.Informacao.PosAcao(response);
        console.log("DEU RUIM")
        return
    }

    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_AguardandoIntegracao));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#PreProposta_Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.AguardandoIntegracao(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

//Em Análise Completa
Europa.Controllers.PreProposta.EmAnaliseCompleta = function () {

    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_EmAnaliseCompleta));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#PreProposta_Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.EmAnaliseCompleta(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

//Aguardando Auditoria
Europa.Controllers.PreProposta.AguardandoAuditoria = function () {

    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_AguardandoAuditoria));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#PreProposta_Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.AguardandoAuditoria(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

//Retorno Auditoria CACT
Europa.Controllers.PreProposta.RetornoAuditoria = function () {

    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        "Retorno de Auditoria");
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#PreProposta_Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.RetornoAuditoria(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

//Fluxo Enviado
Europa.Controllers.PreProposta.FluxoEnviado = function () {

    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_FluxoEnviado));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#PreProposta_Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.FluxoEnviado(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

//Docs Insuficientes
Europa.Controllers.PreProposta.DocsInsuficientes = function () {

    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_DocsInsuficientes));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#PreProposta_Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.DocsInsuficientes(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

Europa.Controllers.PreProposta.AnaliseCompletaAprovada = function () {
    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_AnaliseCompletaAprovada));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#PreProposta_Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.AnaliseCompletaAprovada(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
}

Europa.Controllers.PreProposta.ReenviarAnaliseCompletaAprovada = function () {
    Europa.Informacao.Hide = function () {
        location.reload(true);
    }

    var content = '';
    content += '<div>';
    content += Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoRetrocederPreProposta2, Europa.i18n.Messages.SituacaoProposta_AnaliseCompletaAprovada);
    content += '</div>';
    content += '</br>';
    content += '<div>';
    content += '<label>Justificativa: </label>';
    content += '</br>';
    content += '<textarea id="justificativa-textarea" cols="40" rows="5"></textarea>';
    content += '</div>';

    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.DevolverParaAnalise, content);

    $('#confirm-alert-body-btn-primary').enable(false);
    $('#justificativa-textarea').on('input', function (e) {
        if ($('#justificativa-textarea').val() == '') {
            $('#confirm-alert-body-btn-primary').enable(false);
        }
        else {
            $('#confirm-alert-body-btn-primary').enable(true);
        }
    });

    Europa.Confirmacao.ConfirmCallback = function () {
            var data = {
                idPreProposta: $("#PreProposta_Id").val(),
                Justificativa: $('#justificativa-textarea').val()
            }

            var response = Europa.Controllers.PrePropostaWorkflow.ReenviarAnaliseCompletaAprovada(data);
            Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
}

Europa.Controllers.PreProposta.AnaliseSimplificadaAprovada = function () {
    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_AnaliseSimplificadaAprovada));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#PreProposta_Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.AnaliseSimplificadaAprovada(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
}

Europa.Controllers.PreProposta.AbrirModalFormulario = function () {
    $("#modal-formulario").show();
};

Europa.Controllers.PreProposta.BaixarFormularios = function () {
    location.href = Europa.Controllers.PreProposta.UrlBaixarFormularios + "?idPreProposta=" + $("#PreProposta_Id").val();

};

Europa.Controllers.PreProposta.ConfigureBreveLancamentoAutocomplete = function (autocompleteWrapper) {
    autocompleteWrapper.AutoCompleteBreveLancamento.Data = function (params) {
        return {
            start: 0,
            pageSize: 10,
            filter: [
                {
                    value: params.term,
                    column: this.param,
                    regex: true
                },
                {
                    value: function () {
                        return $("#regional").val();
                    },
                    column: 'regional'
                }
            ],
            order: [
                {
                    value: "asc",
                    column: this.param
                }
            ]
        };
    };

    $('#autocomplete_breve_lancamento').on('change', function (e) {
        var idLanc = autocompleteWrapper.AutoCompleteBreveLancamento.Value();
        if (idLanc > 0 && idLanc !== undefined) {
            $("#autocomplete_torre").removeAttr("disabled");
            autocompleteWrapper.AutoCompleteTorre.Clean();
            //autocompleteWrapper.ConfigureTorreAutocomplete(autocompleteWrapper);
        }
        else {
            autocompleteWrapper.AutoCompleteTorre.Clean();
            $("#autocomplete_torre").attr("disabled", "disabled");
        }
    });
    autocompleteWrapper.AutoCompleteBreveLancamento.Configure();

    if ($("#PreProposta_SituacaoProposta").val() == 14) {
        autocompleteWrapper.AutoCompleteBreveLancamento.Enable();
    }

    autocompleteWrapper.ChangeBreveLancamento();  

};

Europa.Controllers.PreProposta.ConfigureTorreAutocomplete = function (autocompleteWrapper) {
    autocompleteWrapper.AutoCompleteTorre.Data = function (params) {
        return {
            start: 0,
            pageSize: 10,
            filter: [
                {
                    value: params.term,
                    column: this.param,
                    regex: true
                },
                {
                    value: function () {
                        return $("#autocomplete_breve_lancamento").val();
                    },
                    column: 'idBreveLancamento'
                }
            ],
            order: [
                {
                    value: "asc",
                    column: this.param
                }
            ]
        };
    };
    autocompleteWrapper.AutoCompleteTorre.Configure();

    autocompleteWrapper.ConfigTorreAuto();

    if ($("#PreProposta_SituacaoProposta").val() == 14) {
        autocompleteWrapper.AutoCompleteBreveLancamento.Enable();
        $("#PreProposta_ObservacaoTorre").removeAttr("readOnly");
    }
};

Europa.Controllers.PreProposta.ConfigTorreAuto = function () {
    if ($("#PreProposta_SituacaoProposta").val() != 14) {
        Europa.Controllers.PreProposta.AutoCompleteTorre.Disable();
    }

    var id = $("#PreProposta_IdTorre").val();
    var text = $("#PreProposta_NomeTorre").val();
    if (id != undefined && id > 0) {
        Europa.Controllers.PreProposta.AutoCompleteTorre.SetValue(id,text);
    }
}
//Modal Formulario
Europa.Controllers.PreProposta.AbrirModalFormulario = function () {
    $("#modal-formulario").show();
}
Europa.Controllers.PreProposta.AbrirModalOrigemIndicacao = function () {
    $("#modal_origem_indicacao").modal("show");
};
Europa.Controllers.PreProposta.EnableModalIndicacao = function () {
    var origemCliente = $("#PreProposta_OrigemCliente").val();
    if (origemCliente == 3) {
        $("#span_origem_cliente").prop('disabled', false);
    } else {
        $("#span_origem_cliente").prop('disabled', true);
    }
};