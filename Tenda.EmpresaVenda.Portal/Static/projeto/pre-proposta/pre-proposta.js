"use strict";

Europa.Controllers.PreProposta = {};
Europa.Controllers.PreProposta.IdPreProposta = {};
Europa.Controllers.PreProposta.Dto = {};

// Injetado via outros JS
Europa.Controllers.PreProposta.Proponente = {};
Europa.Controllers.PreProposta.DetalhamentoFinanceiro = {};
Europa.Controllers.PreProposta.ItbiEmolumento = {};
Europa.Controllers.PreProposta.DocumentoProponente = {};


var enviarPreProposta = false;

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

    // Garantindo referencia    
    Europa.Controllers.PreProposta = self;
    Europa.Controllers.PreProposta.Init();

    Europa.Controllers.PreProposta.InitComponents();
    Europa.Controllers.PreProposta.EnableModalIndicacao();

    if (!Europa.Controllers.PreProposta.PodeManterAssociacoes()) {
        $('#autocomplete_breve_lancamento').attr('readOnly', 'readOnly');
        $('#autocomplete_torre').attr('readOnly', 'readOnly');
    }

    if (Europa.Controllers.PreProposta.IdEmpre != 0) {
        Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.SetSelected(Europa.Controllers.PreProposta.IdEmpre, Europa.Controllers.PreProposta.NomeEmpre);
    }

    var IdPontoVenda = $("#PreProposta_PontoVenda_Id").val();
    if (IdPontoVenda) {
        $("#PreProposta_PontoVenda option[value='" + IdPontoVenda + "']").prop('selected', true);
    }

    //var IdRegiao = $("#PreProposta_Regiao_Id").val();
    //if (IdRegiao) {
    //    $("#PreProposta_Regiao option[value='" + IdRegiao + "']").prop('selected', true);
    //}

    if ($('#PreProposta_Cliente_NomeCompleto').is('[readonly="readonly"]')) {
        Europa.AddSubstituteFieldTo($('#PreProposta_Cliente_NomeCompleto'));
    }

    Europa.NavbarScrollControl("tabMenu", "block_content_page");
    $("#tabMenu li").click(function (e) {
        e.preventDefault();
        Europa.OnTabChange(this, "tabMenu", "block_content_page");
    });

    Europa.Controllers.PreProposta.ConfigureBreveLancamentoAutocomplete(Europa.Controllers.PreProposta);
    Europa.Controllers.PreProposta.ConfigureCloneBreveLancamentoAutocomplete(Europa.Controllers.PreProposta);

    setTimeout(Europa.Controllers.PreProposta.ConfigTorreAuto, 500);
    //status suat evs
    Europa.Controllers.PreProposta.StatusSuatEvs();

    Europa.Controllers.PreProposta.BloquearCampos();

    setTimeout(Europa.Controllers.PreProposta.ChangeBreveLancamento, 500)
    setTimeout(Europa.Controllers.PreProposta.InitBreveLancamento, 100)
    setTimeout(Europa.Controllers.PreProposta.InitTorre, 300)

    setTimeout(Europa.Controllers.PreProposta.ScreenBehaviors, 100)

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
    // Força preenchimento do campo de Breve Lançamento
    //Europa.Controllers.PreProposta.ChangeBreveLancamento();
    if ($("#PreProposta_PontoVenda_Id").val() > 0) {
        Europa.Controllers.PreProposta.ChangePontoVenda();
    }

    if ($("#PreProposta_Regiao_Id").val() > 0) {
        //Europa.Controllers.PreProposta.ChangeRegiao();
    }
    Europa.Controllers.PreProposta.TratarHabilitacaoCampos();

    Europa.Controllers.PreProposta.AplicarMascaras();
    if ($("#SituacaoPrepropostaSuatEvs").val() == 'Aguardando Auditoria') {
        Europa.Controllers.PreProposta.MascaraSituacao();
    }

    if ($("#PreProposta_BreveLancamento_Id").val() > 0) {
        //Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.SetValue($("#PreProposta_BreveLancamento_Id").val(), $("#PreProposta_BreveLancamento_Nome").val());

    } else {
        Europa.Controllers.PreProposta.InitAutocomplete();
    }


};

Europa.Controllers.PreProposta.MascaraSituacao = function () {
    $("#SituacaoPrepropostaSuatEvs").val("Em Análise");
};

Europa.Controllers.PreProposta.InitAutocomplete = function () {
    //Europa.Controllers.PreProposta.AutoCompleteBreveLancamento = new Europa.Components.AutoCompleteBreveLancamentoRegionalSemEmpreendimento()
    //    .WithAjax(false)
    //    .WithTargetSuffix("breve_lancamento")
    //    .Configure();

    Europa.Controllers.PreProposta.AutoCompleteCloneBreveLancamento = new Europa.Components.AutoCompleteBreveLancamentoRegional()
        .WithAjax(false)
        .WithTargetSuffix("clone_breve_lancamento")
        .Configure();

    Europa.Controllers.PreProposta.AutoCompleteCorretor = new Europa.Components.AutoCompleteCorretorEmpresaVenda()
        .WithTargetSuffix("corretor")
        .Configure();

    Europa.Controllers.PreProposta.AutoCompleteTorre = new Europa.Components.AutoCompleteTorre()
        .WithTargetSuffix("torre")
        .WithParamName("NomeTorre")
        .Configure();

    Europa.Controllers.PreProposta.AutoCompleteCloneTorre = new Europa.Components.AutoCompleteTorre()
        .WithTargetSuffix("clone_torre")
        .WithParamName("NomeTorre")
        .Configure();

    //Europa.Controllers.PreProposta.AutoCompleteBreveLancamentoReal = new Europa.Components.AutoCompleteBreveLancamentoRegionalSemEmpreendimento()
    //    .WithTargetSuffix("breve_lancamento_real")
    //    .Configure();

    Europa.Controllers.PreProposta.AutoCompleteBreveLancamento = new Europa.Components.AutoCompleteBreveLancamentoRegional()
        .WithTargetSuffix("breve_lancamento")
        .Configure();

};

Europa.Controllers.PreProposta.ConfigTorreAuto = function () {
    Europa.Controllers.PreProposta.AutoCompleteTorre.SetValue($("#PreProposta_IdTorre").val(), $("#PreProposta_NomeTorre").val());
}

Europa.Controllers.PreProposta.ConfigureBreveLancamentoAutocomplete = function (autocompleteWrapper) {
    $("#autocomplete_breve_lancamento").on("change", function (e) {
        var idLanc = autocompleteWrapper.AutoCompleteBreveLancamento.Value();

        if (idLanc > 0) {
            $("#autocomplete_torre").removeAttr("disabled");
            autocompleteWrapper.AutoCompleteTorre.Clean();
            autocompleteWrapper.ConfigureTorreAutocomplete(autocompleteWrapper);
        }
        else {
            autocompleteWrapper.AutoCompleteTorre.Clean();
            $("#autocomplete_torre").attr("readOnly", "readOnly");
        }

    });
    autocompleteWrapper.AutoCompleteBreveLancamento.Configure();
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
};

Europa.Controllers.PreProposta.ConfigureCloneBreveLancamentoAutocomplete = function (autocompleteWrapper) {
    $("#autocomplete_clone_breve_lancamento").on("change", function (e) {
        var idLanc = autocompleteWrapper.AutoCompleteCloneBreveLancamento.Value();
        if (idLanc > 0 && idLanc !== undefined) {
            $("#autocomplete_clone_torre").removeAttr("disabled");
            autocompleteWrapper.AutoCompleteCloneTorre.Clean();
            autocompleteWrapper.ConfigureCloneTorreAutocomplete(autocompleteWrapper);
        }
        else {
            autocompleteWrapper.AutoCompleteCloneTorre.Clean();
            $("#autocomplete_clone_torre").attr("disabled", "disabled");
        }

    });
    autocompleteWrapper.AutoCompleteCloneBreveLancamento.Configure();
};

Europa.Controllers.PreProposta.ConfigureCloneTorreAutocomplete = function (autocompleteWrapper) {
    autocompleteWrapper.AutoCompleteCloneTorre.Data = function (params) {
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
                        return $("#autocomplete_clone_breve_lancamento").val();
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
    autocompleteWrapper.AutoCompleteCloneTorre.Configure();
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
    if ($("#PreProposta_PontoVenda").val() > 0) {
        $("[name='PreProposta.PontoVenda.Id']").val($("#PreProposta_PontoVenda").val());
    }
};

//Europa.Controllers.PreProposta.ChangeRegiao = function () {
//    if ($("#PreProposta_Regiao").val() > 0) {
//        $("[name='PreProposta.Regiao.Id']").val($("#PreProposta_Regiao").val());
//    }
//};

Europa.Controllers.PreProposta.ChangeBreveLancamento = function () {

    var id = $("#PreProposta_BreveLancamento_Id").val();
    var text = $("#PreProposta_BreveLancamento_Nome").val();
    if (id != undefined && id > 0) {
        Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.SetSelected(id, text);

        var emp = $("#PreProposta_BreveLancamento_Empreendimento_Id").val() == "";
        if (emp) {
            Europa.Controllers.PreProposta.ConfigureTorreAutocomplete(Europa.Controllers.PreProposta);
            Europa.Controllers.PreProposta.AutoCompleteTorre.Enable();
        }
    }

};

Europa.Controllers.PreProposta.ChangeCloneBreveLancamento = function () {
    var id = $("#Clone_BreveLancamento_Id", "#form_clone_pre_proposta").val();
    var text = $("#Clone_BreveLancamento_Nome", "#form_clone_pre_proposta").val();
    if (id != undefined && id > 0) {
        Europa.Controllers.PreProposta.AutoCompleteCloneBreveLancamento.SetSelected(id, text);
    }
};

Europa.Controllers.PreProposta.SalvarPreProposta = function () {
    var data = {};
    data = Europa.Form.SerializeJson("#form_pre_proposta");

    if (Europa.Controllers.PreProposta.AutoCompleteTorre.Value() != undefined && Europa.Controllers.PreProposta.AutoCompleteTorre.Value() != 0) {
        data["PreProposta.IdTorre"] = Europa.Controllers.PreProposta.AutoCompleteTorre.Value();
    }

    if (Europa.Controllers.PreProposta.AutoCompleteTorre.Text() != "") {
        data["PreProposta.NomeTorre"] = Europa.Controllers.PreProposta.AutoCompleteTorre.Text();
    }

    var parcelaSolicitada = $("#PreProposta_ParcelaSolicitada").val() == "" || $("#PreProposta_ParcelaSolicitada").val() == undefined ? 0 : $("#PreProposta_ParcelaSolicitada").val().replace('.', '');

    data["PreProposta.ParcelaSolicitada"] = parcelaSolicitada;
    delete data["PreProposta.PontoVenda"];

    if (Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Value() != null) {
        data["PreProposta.BreveLancamento.Id"] = Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Value();
        data["PreProposta.BreveLancamento.Nome"] = Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Text();
    } else if ($("#PreProposta_BreveLancamento_Id").val() > 0) {

    } else if (Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Value() == null) {

        delete data["PreProposta.BreveLancamento.Empreendimento.Id"]
        delete data["PreProposta.BreveLancamento.Id"]
        delete data["PreProposta.BreveLancamento.Nome"]
        delete data["EnderecoBreveLancamento"]
    }

    //if (data["PreProposta.Regiao"] == "") {
    //    data["PreProposta.Regiao.Id"] = 0;
    //}
    //delete data["PreProposta.Regiao"];

    //data["PreProposta.IsBreveLancamento"] = data["PreProposta.IsBreveLancamento"] == 'True';
    data["PreProposta.FaixaEv"] = data["PreProposta.FaixaEv"] == 'True';


    //console.log(data)

    var url = $('#PreProposta_Id').val() > 0 ? Europa.Controllers.PreProposta.UrlAlterarPreProposta : Europa.Controllers.PreProposta.UrlIncluirPreProposta;
    $.ajax({
        url: url,
        method: 'POST',
        data: { dto: data }
    })
        .done(function (res) {
            if (res.Sucesso) {
                $("#area_geral").html(res.Objeto.htmlGeral);
                Europa.Controllers.PreProposta.AplicarMascaras();
                $("#PreProposta_PontoVenda option[value='" + $("#PreProposta_PontoVenda_Id").val() + "']").prop('selected', true);

                Europa.Informacao.Hide = function () {
                    Europa.Controllers.PreProposta.Desistir();
                };
            } else {
                Europa.Validator.InvalidateList(res.Campos, "#form_pre_proposta");
            }
            Europa.Informacao.PosAcao(res);
        });
};

Europa.Controllers.PreProposta.CancelarPreProposta = function () {
    var data = {
        idPreProposta: $('#PreProposta_Id').val()
    };

    Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.CancelarPreProposta, Europa.i18n.Messages.MsgCancelarPreProposta, Europa.i18n.Messages.Confirmar, function () {
        $.post(Europa.Controllers.PreProposta.UrlCancelarPreProposta, data, function (res) {
            if (res.Sucesso) {
                Europa.Informacao.Hide = function () {
                    Europa.Controllers.PreProposta.Desistir();
                };
            }
            Europa.Informacao.PosAcao(res);
        });
    });
};

Europa.Controllers.PreProposta.RevisarPreProposta = function () {
    var data = {
        idPreProposta: $('#PreProposta_Id').val()
    };

    var codigo = $('#PreProposta_Codigo').val();
    Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Revisar, codigo,
        function () {
            $.post(Europa.Controllers.PreProposta.UrlRevisarPreProposta, data, function (res) {
                if (res.Sucesso) {
                    Europa.Informacao.Hide = function () {
                        Europa.Controllers.PreProposta.Desistir();
                    };
                }
                Europa.Informacao.PosAcao(res);

            });
        });
};
Europa.Controllers.PreProposta.PreEnviarPreProposta = function () {
    var codigo = $('#PreProposta_Codigo').val();
    Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Enviar, codigo,
        function () {
            //flag para diferenciar os botoes "enviar" e "indicaçao" na hora de salvar/cancelar indicação
            enviarPreProposta = true;
            Europa.Controllers.Indicacao.AbrirModal();
        });
};

Europa.Controllers.PreProposta.EnviarPreProposta = function () {
    var data = {};
    data = Europa.Form.SerializeJson("#form_pre_proposta");

    var parcelaSolicitada = $("#PreProposta_ParcelaSolicitada").val() == "" || $("#PreProposta_ParcelaSolicitada").val() == undefined ? 0 : $("#PreProposta_ParcelaSolicitada").val().replace('.', '');

    var data = {
        preProposta: {
            id: $('#PreProposta_Id').val(),
            ParcelaSolicitada: parcelaSolicitada,
            IdTorre: Europa.Controllers.PreProposta.AutoCompleteTorre.Value(),
            NomeTorre: Europa.Controllers.PreProposta.AutoCompleteTorre.Text(),
            ObservacaoTorre: $("#PreProposta_ObservacaoTorre").val(),
            //Regiao: { Id: $("#PreProposta_Regiao").val() },
            FaixaEv: data["PreProposta.FaixaEv"] == 'True',
            //IsBreveLancamento: data["PreProposta.IsBreveLancamento"] == 'True'
        },
        IdBreveLancamento: Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Value()
    };

    if (Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Value() != undefined) {
        data.IdBreveLancamento = Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Value()
    } //else if (Europa.Controllers.PreProposta.AutoCompleteBreveLancamentoReal.Value() != undefined) {
    //  data.IdBreveLancamento = Europa.Controllers.PreProposta.AutoCompleteBreveLancamentoReal.Value()
    //}
    //console.log(data)

    $.post(Europa.Controllers.PreProposta.UrlEnviarPreProposta, data, function (res) {
        if (res.Sucesso) {
            Europa.Informacao.Hide = function () {
                Europa.Controllers.PreProposta.Desistir();
            };
        } else {
            Europa.Informacao.Hide = function () {
                $(Europa.Informacao.Attr.Modal).modal("hide");
            }
        }
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.PreProposta.RetornarPreProposta = function () {
    var data = {};
    data = Europa.Form.SerializeJson("#form_pre_proposta");

    var parcelaSolicitada = $("#PreProposta_ParcelaSolicitada").val() == "" || $("#PreProposta_ParcelaSolicitada").val() == undefined ? 0 : $("#PreProposta_ParcelaSolicitada").val().replace('.', '');

    var data = {
        preProposta: {
            id: $('#PreProposta_Id').val(),
            ParcelaSolicitada: parcelaSolicitada,
            IdTorre: Europa.Controllers.PreProposta.AutoCompleteTorre.Value() == undefined ? -1 : Europa.Controllers.PreProposta.AutoCompleteTorre.Value(),
            NomeTorre: Europa.Controllers.PreProposta.AutoCompleteTorre.Text() == "" ? "TORRE INEXISTENTE" : Europa.Controllers.PreProposta.AutoCompleteTorre.Text(),
            ObservacaoTorre: $("#PreProposta_ObservacaoTorre").val(),
            //Regiao: { Id: $("#PreProposta_Regiao").val() },
            FaixaEv: data["PreProposta.FaixaEv"] == 'True'
        },
        IdBreveLancamento: Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Value()
    };

    if (Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Value() != undefined) {
        data.IdBreveLancamento = Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Value()
    } //else if (Europa.Controllers.PreProposta.AutoCompleteBreveLancamentoReal.Value() != undefined) {
    //  data.IdBreveLancamento = Europa.Controllers.PreProposta.AutoCompleteBreveLancamentoReal.Value()
    //}

    var codigo = $('#PreProposta_Codigo').val();
    Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Enviar, codigo,
        function () {
            $.post(Europa.Controllers.PreProposta.UrlRetornarPreProposta, data, function (res) {
                if (res.Sucesso) {
                    Europa.Informacao.Hide = function () {
                        Europa.Controllers.PreProposta.Desistir();
                    };
                }
                Europa.Informacao.PosAcao(res);
            });
        });
};

Europa.Controllers.PreProposta.AbrirAlterarCorretor = function () {
    var corretorAtual = $('#PreProposta_Corretor_Nome').val();
    $('#alterar_corretor_atual').val(corretorAtual);
    $("#autocomplete_corretor").val("").trigger("change");
    $('#alterar_corretor_modal').modal('show');
    $("#area_corretor").removeClass("has-error");
};

Europa.Controllers.PreProposta.AlterarCorretor = function () {
    var data = {
        idPreProposta: $('#PreProposta_Id').val(),
        idCorretor: $("#autocomplete_corretor").val()
    };
    var codigo = $('#PreProposta_Codigo').val();
    Europa.Confirmacao.PreAcao(Europa.i18n.Messages.AlterarCorretor, codigo,
        function () {
            $.post(Europa.Controllers.PreProposta.UrlAlterarCorretor, data, function (res) {
                if (res.Sucesso) {
                    $("#area_corretor").removeClass("has-error");
                    Europa.Informacao.Hide = function () {
                        Europa.Controllers.PreProposta.Desistir();

                    };
                }
                else {
                    //Europa.Validator.InvalidateList(res.Campos, "#form_upload_documento_proponente");
                    $("#area_corretor").addClass("has-error");
                }
                Europa.Informacao.PosAcao(res);
            });
        });
};

Europa.Controllers.PreProposta.AbrirModalClonarPreProposta = function () {
    $('#clonar_pre_proposta_modal').modal('show');
    var breveLancamentoAtual = Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Text();
    var nomeTorreAtual = Europa.Controllers.PreProposta.AutoCompleteTorre.Text();
    var obsTorreAtual = $("#PreProposta_ObservacaoTorre").val();
    $('#breve_lancamento_atual').val(breveLancamentoAtual);
    $("#autocomplete_clone_breve_lancamento").val("").trigger("change");
    $("#torre_atual").val(nomeTorreAtual);
    Europa.Controllers.PreProposta.InitAutocomplete();
    $("#observacao_torre").val(obsTorreAtual);
};

Europa.Controllers.PreProposta.AbrirModalObservacoes = function () {
    $('#modal_observacoes').modal('show');
};

Europa.Controllers.PreProposta.ClonarPreProposta = function () {
    var data = {
        idPreProposta: $('#PreProposta_Id').val(),
        idBreveLancamento: Europa.Controllers.PreProposta.AutoCompleteCloneBreveLancamento.Value(),
        idTorre: Europa.Controllers.PreProposta.AutoCompleteCloneTorre.Value(),
        obsTorre: $("#observacao_torre_clone").val(),
        nomeTorre: Europa.Controllers.PreProposta.AutoCompleteCloneTorre.Text()
    };
    var codigo = $('#PreProposta_Codigo').val();
    Europa.Confirmacao.PreAcao(Europa.i18n.Messages.ClonarPreProposta, codigo,
        function () {
            $.post(Europa.Controllers.PreProposta.UrlClonarPreProposta, data, function (res) {
                if (res.Sucesso) {
                    Europa.Informacao.Hide = function () {
                        Europa.Controllers.PreProposta.Desistir(res.Objeto.Id);
                    };
                }
                Europa.Informacao.PosAcao(res);
            });
        });
};

Europa.Controllers.PreProposta.Desistir = function (idClone) {
    var idPreProposta = $('#PreProposta_Id').val();
    if (idClone != null) {
        idPreProposta = idClone;
    }

    var basePath = location.href.split("/Index")[0];
    var basePath = basePath.split("/?idCliente")[0];
    if (idPreProposta == undefined || idPreProposta == null)
        location.href = basePath;
    location.href = basePath + "/Index/" + idPreProposta;
};

Europa.Controllers.PreProposta.TratarHabilitacaoCampos = function () {
    // Habilita a edição caso seja uma nova proposta
    if (Europa.Controllers.PreProposta.IdPreProposta == undefined || Europa.Controllers.PreProposta.IdPreProposta == 0) {
        $("#fieldset_pre_proposta").removeAttr("disabled");
        $("#PreProposta_ObservacaoTorre").removeAttr("disabled").removeAttr("readonly");
        $('#form_pre_proposta input[type="radio"]').each(function () {
            $(this).prop('checked', false);
        });
        Europa.Controllers.PreProposta.OnSelectBreveLancamento(1);
    } else {
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
        if ($("#PreProposta_SituacaoProposta").val() == 1) {
            Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Enable();
            Europa.Controllers.PreProposta.AutoCompleteTorre.Enable();
            $("#PreProposta_ObservacaoTorre").removeAttr("disabled").removeAttr("readonly");
            $("#PreProposta_ClienteCotista_Nao").removeAttr("disabled").removeAttr("readonly");
            $("#PreProposta_ClienteCotista_Sim").removeAttr("disabled").removeAttr("readonly");
            $("#PreProposta_FatorSocial_Nao").removeAttr("disabled").removeAttr("readonly");
            $("#PreProposta_FatorSocial_Sim").removeAttr("disabled").removeAttr("readonly");
            $('#PreProposta_ParcelaSolicitada').removeAttr("disabled").removeAttr("readonly");
            //$('#PreProposta_Regiao').removeAttr("disabled").removeAttr("readonly");
            //$('#PreProposta_IsBreveLancamento_Sim').removeAttr("disabled").removeAttr("readonly");
            //$('#PreProposta_IsBreveLancamento_Nao').removeAttr("disabled").removeAttr("readonly");
            $('#PreProposta_FaixaEv_Sim').removeAttr("disabled").removeAttr("readonly");
            $('#PreProposta_FaixaEv_Nao').removeAttr("disabled").removeAttr("readonly");
        } else {
            Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Disable();
            Europa.Controllers.PreProposta.AutoCompleteTorre.Disable();
            $("#PreProposta_ClienteCotista_Nao").attr("disabled", "disabled").attr("readonly", "readonly");
            $("#PreProposta_ClienteCotista_Sim").attr("disabled", "disabled").attr("readonly", "readonly");
            $("#PreProposta_FatorSocial_Nao").attr("disabled", "disabled").attr("readonly", "readonly");
            $("#PreProposta_FatorSocial_Sim").attr("disabled", "disabled").attr("readonly", "readonly");
            //$('#PreProposta_Regiao').attr("style", "pointer-events: none;");
        }
        if ($("#PreProposta_SituacaoProposta").val() == 8) {
            Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Enable();
            Europa.Controllers.PreProposta.AutoCompleteTorre.Enable();
            $("#PreProposta_ObservacaoTorre").removeAttr("disabled").removeAttr("readonly");
            $('#PreProposta_ParcelaSolicitada').removeAttr("disabled").removeAttr("readonly");
            //$('#PreProposta_Regiao').removeAttr("disabled").removeAttr("readonly");
            //$('#PreProposta_IsBreveLancamento_Sim').removeAttr("disabled").removeAttr("readonly");
            //$('#PreProposta_IsBreveLancamento_Nao').removeAttr("disabled").removeAttr("readonly");
            $('#PreProposta_FaixaEv_Sim').removeAttr("disabled").removeAttr("readonly");
            $('#PreProposta_FaixaEv_Nao').removeAttr("disabled").removeAttr("readonly");

            Europa.Controllers.PreProposta.OnSelectBreveLancamento(1);
        }

    }


    if ($("#SituacaoAnterior").val() == 18) {
        $('#PreProposta_ParcelaSolicitada').attr("disabled", "disabled").attr("readonly", "readonly");
    }

    if ($("#PreProposta_SituacaoProposta").val() == 12) {
        $('#PreProposta_FaixaEv_Sim').removeAttr("disabled").removeAttr("readonly");
        $('#PreProposta_FaixaEv_Nao').removeAttr("disabled").removeAttr("readonly");
    }

    //var isBreveLancamento = $('#PreProposta_IsBreveLancamento').val() == 'True';

    //if (isBreveLancamento) {
    //    $("#PreProposta_IsBreveLancamento_Sim").click();
    //}

    //Europa.Controllers.PreProposta.OnSelectBreveLancamento(isBreveLancamento);

    //$('#PreProposta_IsBreveLancamento_Sim').prop("disabled", true);
    //$('#PreProposta_IsBreveLancamento_Nao').prop("disabled", true);

};

//Não utilizado
Europa.Controllers.PreProposta.PreencherTotais = function () {
    var request = {
        idPreProposta: $('#PreProposta_Id').val()
    };

    if (request.idPreProposta === undefined || request.idPreProposta == 0 || request.idPreProposta == "0") { return; }

    $.ajax({
        url: Europa.Controllers.PreProposta.UrlPreencherTotais,
        method: 'POST',
        data: request
    })
        .done(function (response) {
            $('#PreProposta_TotalDetalhamentoFinanceiro').val(response.TotalDetalhamentoFinanceiro);
            $('#PreProposta_TotalItbiEmolumento').val(response.TotalItbiEmolumento);
            $('#PreProposta_Valor').val(response.Total);
            Europa.Controllers.PreProposta.AplicarMascaras();
        });
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

Europa.Controllers.PreProposta.OnChangeBar = function (data) {
    $('#tabMenu').on('click', function (event) {
        let elementoClicado = $(event.target);

        if (elementoClicado[0].hash !== "#area_avalista") {
            $("#area_avalista").addClass("hidden");
            $(".area").removeClass("hidden");
            $(elementoClicado[0].hash).addClass("active");
            $("#tabAvalista").removeClass("active");
            $(".avalista").attr("readOnly", "readOnly");
            $("#btn_salvar_avalista").addClass("hidden");
            $("#btn_enviar_documentos_avalista").addClass("hidden");
            $("#btn_imprimir_contrato").removeClass("hidden");
            $("#btn_baixar_boleto").removeClass("hidden");
        } else {
            $("#btn_salvar_avalista").removeClass("hidden");
            $("#btn_enviar_documentos_avalista").removeClass("hidden");
            $("#btn_imprimir_contrato").addClass("hidden");
            $("#btn_baixar_boleto").addClass("hidden");
        }
    });
};

Europa.Controllers.PreProposta.BaixarFormularios = function () {
    location.href = Europa.Controllers.PreProposta.UrlBaixarFormularios + "?idPreProposta=" + $("#PreProposta_Id").val();

};

//Aguardando Análise Completa
Europa.Controllers.PreProposta.AguardandoAnaliseCompleta = function () {

    var data = {
        preProposta: {
            id: $('#PreProposta_Id').val(),
            BreveLancamento: {
                Id: $("#PreProposta_BreveLancamento_Id").val()
            },
            IdTorre: $("#PreProposta_IdTorre").val(),
            ObservacaoTorre: $("#PreProposta_ObservacaoTorre").val(),
            NomeTorre: $("#PreProposta_NomeTorre").val()
        }
    };

    var codigo = $('#PreProposta_Codigo').val();
    Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Enviar, codigo,
        function () {
            $.post(Europa.Controllers.PreProposta.UrlAguardandoAnaliseCompleta, data, function (res) {
                if (res.Sucesso) {
                    Europa.Informacao.Hide = function () {
                        Europa.Controllers.PreProposta.Desistir();
                    };
                }
                Europa.Informacao.PosAcao(res);
            });
        });
};

Europa.Controllers.PreProposta.BloquearCampos = function () {
    var situacao = [12, 8, 19]

    if (situacao.includes(parseInt($("#PreProposta_SituacaoProposta").val()))) {
        Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Enable();
        Europa.Controllers.PreProposta.AutoCompleteTorre.Enable();
        $("#PreProposta_ObservacaoTorre").prop("disabled", false);
        $("#PreProposta_ObservacaoTorre").prop("readOnly", false)

        //Europa.Controllers.PreProposta.ConfigureCloneTorreAutocomplete(Europa.Controllers.PreProposta)
    }

}

Europa.Controllers.PreProposta.OnSelectBreveLancamento = function (data) {
    var situacao = $("#PreProposta_SituacaoProposta").val();

    if (data && (situacao == 1 || situacao == 8 || situacao == undefined)) {
        $('#autocomplete_breve_lancamento').removeAttr("disabled").removeAttr("readonly");
    } else {
        $('#autocomplete_breve_lancamento').attr("readOnly", "readOnly");
        $('#autocomplete_breve_lancamento').attr("disabled", "disabled");
        Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.Clean();
    }
};

Europa.Controllers.PreProposta.InitBreveLancamento = function () {

    var id = $("#PreProposta_BreveLancamento_Id").val();
    var text = $("#PreProposta_BreveLancamento_Nome").val();

    if (id != undefined && id > 0) {
        Europa.Controllers.PreProposta.AutoCompleteBreveLancamento.SetValue(id, text);
    }
};

Europa.Controllers.PreProposta.InitTorre = function () {
    var breveLancamento = $("#PreProposta_BreveLancamento_Id").val();
    var torre = $("#PreProposta_IdTorre").val();

    if (breveLancamento != undefined && breveLancamento > 0 && torre == 0) {

        //Europa.Controllers.PreProposta.AutoCompleteTorre.Enable();
        Europa.Controllers.PreProposta.ConfigureTorreAutocomplete(Europa.Controllers.PreProposta);
    }
};

Europa.Controllers.PreProposta.ScreenBehaviors = function () {
    var situacao = $("#PreProposta_SituacaoProposta").val();
    switch (situacao) {
        case "1":
            Europa.Controllers.PreProposta.OnSelectBreveLancamento(1);
            break;
    }
};

Europa.Controllers.PreProposta.AbrirModalCompartilharLink = function () {
    Europa.Controllers.PreProposta.LimparModalCompartilharLink();
    Europa.Mask.Telefone('#TelefoneIndicado', false);
    $("#modal-compartilhar-link").modal("show");
};


Europa.Controllers.PreProposta.CompartilharLinkIndicador = function () {
    var param = {
        id: $("#PreProposta_Cliente_Id").val(),
        nome: $("#NomeIndicado").val(),
        telefone: $("#TelefoneIndicado").val()
    };
    var url = undefined;
    var destinatario = $("#Destinatario").val();
    if (destinatario == 1) {
        url = Europa.Controllers.PreProposta.UrlEnviarWhatsBotmakerIndicador;
    } else if (destinatario == 2) {
        url = Europa.Controllers.PreProposta.UrlEnviarWhatsBotmakerIndicado;
    } else {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, "O atributo Destinatário é obrigatório");
        Europa.Informacao.Show();
    }

    $.post(url, param, function (res) {
        if (res.Success) {
            $("#modal-compartilhar-link").modal("hide");
        } else {
            Europa.Util.HandleFeedbackFields(res, "#form-compartilhar-indicado");
        }
        Europa.Util.HandleResponseMessages(res);
    });
};

Europa.Controllers.PreProposta.OnChangeDestinatario = function () {
    var destinatario = $("#Destinatario").val();

    if (destinatario == 2) {
        $(".indicado").removeClass("hidden");
        return;
    }
    $(".indicado").addClass("hidden");
};
Europa.Controllers.PreProposta.LimparModalCompartilharLink = function () {
    Europa.Util.RemoveFeedbackFields("#form-compartilhar-indicado");
    $("#Destinatario").val(null).trigger('change');
    $("#NomeIndicado").val("");
    $("#TelefoneIndicado").val("");
};

Europa.Controllers.PreProposta.OnChangeIndicacao = function () {
    var origemCliente = $("#PreProposta_OrigemCliente").val();
    if (origemCliente == 3) {
        Europa.Controllers.PreProposta.AbrirModalOrigemIndicacao();
        $("#span_origem_cliente").prop('disabled', false);
    } else {
        $("#span_origem_cliente").prop('disabled', true);
    }
   
};
Europa.Controllers.PreProposta.EnableModalIndicacao = function () {
    var origemCliente = $("#PreProposta_OrigemCliente").val();
    if (origemCliente == 3) {
        $("#span_origem_cliente").prop('disabled', false);
    } else {
        $("#span_origem_cliente").prop('disabled', true);
    }
};

Europa.Controllers.PreProposta.AbrirModalOrigemIndicacao = function () {
    $("#modal_origem_indicacao").modal("show");
};
Europa.Controllers.PreProposta.ValidarOrigemIndicacao = function () {
    var nomeIndicador = $("#PreProposta_NomeIndicador").val();
    var cpfIndicador = $("#PreProposta_CpfIndicador").val();
    var validarOrigemIndicacao = $("#validarOrigemIndicacao").val();
    var cpfValid = Europa.Validator.ValidCpf(cpfIndicador);
    if (validarOrigemIndicacao == 'True' && (nomeIndicador == "" || cpfIndicador == "")) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, "Favor preencher todos os campos.");
        Europa.Informacao.Show();
    }
    else if (cpfIndicador != "" && !cpfValid) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, "Favor inserir um cpf váldo.");
        Europa.Informacao.Show();
    }
    else {
        Europa.Controllers.PreProposta.FecharModalOrigemIndicacao();
    }

};
Europa.Controllers.PreProposta.CancelarModalOrigemIndicacao = function () {
    Europa.Controllers.PreProposta.LimparModalOrigemIndicacao();
    $('#PreProposta_OrigemCliente').val("").trigger("change");
};
Europa.Controllers.PreProposta.LimparModalOrigemIndicacao = function () {
    $("#PreProposta_CpfIndicador").val("");
    $("#PreProposta_NomeIndicador").val("");
    Europa.Controllers.PreProposta.FecharModalOrigemIndicacao();
};
Europa.Controllers.PreProposta.FecharModalOrigemIndicacao = function () {
    $("#modal_origem_indicacao").modal("hide");
};