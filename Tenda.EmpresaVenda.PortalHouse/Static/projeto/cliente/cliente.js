Europa.Controllers.Cliente = {};
Europa.Controllers.Cliente.UrlIndex = undefined;
Europa.Controllers.Cliente.UrlIncluirCliente = undefined;
Europa.Controllers.Cliente.UrlAtualizarCliente = undefined;
Europa.Controllers.Cliente.UrlValidarDadosVenda = undefined;
Europa.Controllers.Cliente.UrlCadastrarProposta = undefined;
Europa.Controllers.Cliente.Form = "#form_cliente";
Europa.Controllers.Cliente.FieldsetContent = undefined;
Europa.Controllers.Cliente.NovoCliente = true;
Europa.Controllers.Cliente.MensagemError = true;

$(function () {
    Europa.NavbarScrollControl("tabMenu", "block_content_page");
    $("#tabMenu li").click(function (e) {
        e.preventDefault();
        Europa.OnTabChange(this, "tabMenu", "block_content_page");
    });

    Europa.Components.DatePicker.AutoApply();
    Europa.Controllers.Cliente.AplicarMascaras();
    Europa.Controllers.Cliente.EventHandler();
    Europa.Controllers.Cliente.InitAutoCompletes();
    setTimeout(Europa.Controllers.Cliente.DisableAutoComplete, 100);

    Europa.Controllers.Cliente.NovoCliente = $("#Cliente_Id").val() == "" || $("#Cliente_Id").val() == "0"
});

Europa.Controllers.Cliente.AbrirModalCadastroPPR = function () {
    $("#modal_cadastrar_proposta").modal("show");
}

Europa.Controllers.Cliente.FecharModalCadastroPPR = function () {
    let basePath = location.href.split("/Index")[0];
    let novoId = Europa.Controllers.Cliente.GetId();
    location.href = basePath + "/Index/" + novoId;
}

// Permite cópia dos valores dos campos
Europa.Controllers.Cliente.AllowFieldCopy = function () {
    var val = Europa.Controllers.Cliente.GetId();
    if (val > 0) {
        Europa.AddSubstituteFields();
    } else {
        Europa.RemoveSubstituteFields();
    }
};

Europa.Controllers.Cliente.HabilitarConjuge = function () {
    if ($("#DadosPessoaisDto_EstadoCivil").val() == 2) {
        $("#btn_select_cliente").prop("disabled", "");
        $("#DadosPessoaisDto_RegimeBens").prop("disabled", "");
    } else {
        $("#btn_select_cliente").prop("disabled", "disabled");
        $("#DadosPessoaisDto_RegimeBens").prop("disabled", "disabled");
        $("#DadosPessoaisDto_RegimeBens").val("");
        $("[name='Model.DadosPessoaisDto.FamiliarDto.Cliente2.Nome']").val("");
        $("[name='Model.DadosPessoaisDto.FamiliarDto?.Cliente2.Id']").val("");
    }
}

Europa.Controllers.Cliente.SelectConjuge = function (data) {
    
    $.post(Europa.Controllers.Cliente.UrlBuscarCliente, { idCliente: data },
        function (response) {
            if (response.Sucesso) {
                var cliente = response.Objeto;
                $("[name='DadosPessoaisDto.FamiliarDto.Cliente1.Id']").val($("#IdCliente").val());
                $("[name='DadosPessoaisDto.FamiliarDto.Cliente2.Nome']").val(cliente.NomeCompleto);
                $("[name='Model.DadosPessoaisDto.FamiliarDto?.Cliente2.Nome']").val(cliente.NomeCompleto);
                $("[name='DadosPessoaisDto.FamiliarDto.Cliente2.Id']").val(cliente.Id);
            } else {
                Europa.Informacao.PosAcao(response);
            }
        });
};

// Inicializa os autocompletes
Europa.Controllers.Cliente.InitAutoCompletes = function () {
    Europa.Controllers.Cliente.AutoCompleteProfissaoCliente = new Europa.Components.AutoCompleteProfissao()
        .WithTargetSuffix("profissao_cliente")
        .Configure();

    $("#autocomplete_profissao_cliente").change(function () {
        $("#DadosPessoaisDto_Profissao_Id").val(Europa.Controllers.Cliente.AutoCompleteProfissaoCliente.Value());
        $("#DadosPessoaisDto_Profissao_Nome").val(Europa.Controllers.Cliente.AutoCompleteProfissaoCliente.Text());
    })

    Europa.Controllers.Cliente.PreencheAutoComplete();
    $(".daterangepicker.dropdown-menu").css("z-index", 10050);
    Europa.AutoCompleteFix();

};

// Habilita os autocompletes
Europa.Controllers.Cliente.EnableAutoComplete = function () {
    Europa.Controllers.Cliente.AutoCompleteProfissaoCliente.Enable();
};

// Desabilita os autocompletes
Europa.Controllers.Cliente.DisableAutoComplete = function () {
    Europa.Controllers.Cliente.AutoCompleteProfissaoCliente.Disable();
};

// Preenche os autocompletes com o valor do model passado
Europa.Controllers.Cliente.PreencheAutoComplete = function () {
    if ($("#DadosPessoaisDto_Profissao_Id", Europa.Controllers.Cliente.Form).val() > 0) {
        Europa.Controllers.Cliente.AutoCompleteProfissaoCliente
            .SetValue($("#DadosPessoaisDto_Profissao_Id", Europa.Controllers.Cliente.Form).val(),
                $("#DadosPessoaisDto_Profissao_Nome", Europa.Controllers.Cliente.Form).val());
    }
};

// Habilitar edição
Europa.Controllers.Cliente.HabilitarEdicao = function () {
    $("#div_buttons_edicao").show();
    $("#div_buttons_visualizacao").hide();
    $("#btn_validar").show();
    $("#fieldset_formulario", Europa.Controllers.Cliente.Form).removeAttr("disabled");
    Europa.Controllers.Cliente.AplicarMascaras();
    Europa.Controllers.Cliente.AutoCompleteProfissaoCliente.Enable();
    Europa.HideSubstituteFields();
    $("#btn_select_cliente").prop("disabled", "disabled");
    $("#DadosPessoaisDto_RegimeBens").prop("disabled", "disabled");
    $("#DadosProfissionaisDto_TempoDeEmpresa").attr("readonly", true);
    $("#btn_vincular_cliente_lead").prop("hidden",false)
};

// Desabilitar edição
Europa.Controllers.Cliente.DesabilitarEdicao = function (isSaveSucess) {
    if (Europa.Controllers.Cliente.FieldsetContent != undefined && (isSaveSucess == undefined || isSaveSucess == false)) {
        $("#fieldset_formulario").html(Europa.Controllers.Cliente.FieldsetContent);
        $("#div_profissao span").remove();
        Europa.Controllers.Cliente.InitAutoCompletes();
        Europa.Controllers.Cliente.PreencheAutoComplete();
    }
    $("#div_buttons_edicao").hide();
    $("#div_buttons_visualizacao").show();
    $("#btn_validar").hide();
    $("#fieldset_formulario", Europa.Controllers.Cliente.Form).attr("disabled", "disabled");
    Europa.Controllers.Cliente.AplicarMascaras();
    Europa.Controllers.Cliente.InitAutoCompletes();
    Europa.Controllers.Cliente.DisableAutoComplete();
    Europa.ShowSubstituteFields();
};

// Retorna id do cliente
Europa.Controllers.Cliente.GetId = function () {
    return $("#IdCliente", Europa.Controllers.Cliente.Form).val();
};

// Verifica o tipo de renda e habilita/desabilita os campos corretos (Renda Formal e Informal)
Europa.Controllers.Cliente.TipoRendaOnChange = function () {
    var val = $("#DadosFinanceirosDto_TipoRenda", Europa.Controllers.Cliente.Form).val();
    if (val == 1) {
        Europa.Controllers.Cliente.TipoRendaEnableRenda("RendaInformal");
        Europa.Controllers.Cliente.TipoRendaDisableRenda("RendaFormal");
    } else if (val == 2) {
        Europa.Controllers.Cliente.TipoRendaEnableRenda("RendaFormal");
        Europa.Controllers.Cliente.TipoRendaDisableRenda("RendaInformal");
    } else if (val == 3) {
        Europa.Controllers.Cliente.TipoRendaEnableRenda("RendaFormal");
        Europa.Controllers.Cliente.TipoRendaEnableRenda("RendaInformal");
    } else {
        Europa.Controllers.Cliente.TipoRendaDisableRenda("RendaFormal");
        Europa.Controllers.Cliente.TipoRendaDisableRenda("RendaInformal");
    }
};

// Habilita os campos de renda
Europa.Controllers.Cliente.TipoRendaEnableRenda = function (renda) {
    $("#DadosFinanceirosDto_" + renda, Europa.Controllers.Cliente.Form).removeAttr("disabled");
    $("#Cliente_Origem" + renda, Europa.Controllers.Cliente.Form).removeAttr("disabled");
};

// Desabilita e limpa os campos de renda
Europa.Controllers.Cliente.TipoRendaDisableRenda = function (renda) {
    if (!Europa.Controllers.Cliente.IsView) {
        $("#DadosFinanceirosDto_" + renda, Europa.Controllers.Cliente.Form).attr("disabled", "disabled");
        $("#DadosFinanceirosDto_" + renda, Europa.Controllers.Cliente.Form).val("");
        Europa.Controllers.Cliente.RendaOnchange();
    }
};

// Recalcula a renda mensal
Europa.Controllers.Cliente.RendaOnchange = function () {
    var rf = $("#DadosFinanceirosDto_RendaFormal", Europa.Controllers.Cliente.Form).cleanVal();
    var ri = $("#DadosFinanceirosDto_RendaInformal", Europa.Controllers.Cliente.Form).cleanVal();
    var rm = 0;
    rm = rf != undefined && rf.trim().length == 0 ? 0 : parseFloat(rf);
    rm += ri != undefined && ri.trim().length == 0 ? 0 : parseFloat(ri);
    var selector = $("#DadosFinanceirosDto_RendaMensal", Europa.Controllers.Cliente.Form);
    selector.val(selector.masked(rm));
};

// Buscar tempo de empresa

Europa.Controllers.Cliente.OnChangeTempoEmpresa = function (input) {

    var data = $(input).val();

    var dd1 = data.slice(0, 2);
    var mm1 = data.slice(3, 5);
    var yyyy1 = data.slice(6, 10);

    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();

    today = mm + '/' + dd + '/' + yyyy;
    var numberOfMonths = (yyyy - yyyy1) * 12 + (mm - mm1);
    if (dd1 > dd) {
        numberOfMonths -= 1;
    }
    $("#DadosProfissionaisDto_TempoDeEmpresa").val(numberOfMonths)
}

//Busca dados cep Cliente
Europa.Controllers.Cliente.OnChangeCepEnderecoCliente = function (input) {
    Europa.Controllers.Cliente.OnChangeCep(input, "EnderecoClienteDto");
};

// Busca dados cep Empresa
Europa.Controllers.Cliente.OnChangeCepEnderecoEmpresa = function (input) {
    Europa.Controllers.Cliente.OnChangeCep(input, "DadosProfissionaisDto_EnderecoEmpresaDto");
};

// Busca dados cep
Europa.Controllers.Cliente.OnChangeCep = function (input, endereco) {
    var cep = $(input).val().replace(/\D/g, '');

    if (cep == "") {
        return;
    }
    var validacep = /^[0-9]{8}$/;
    if (!validacep.test(cep)) {
        return;
    }
    Europa.Components.Cep.Search(cep, function (dados) {
        $("#" + endereco + "_Logradouro", Europa.Controllers.Cliente.Form).val(dados.Objeto.logradouroAbrev);
        $("#" + endereco + "_Bairro", Europa.Controllers.Cliente.Form).val(dados.Objeto.bairro);
        $("#" + endereco + "_Cidade", Europa.Controllers.Cliente.Form).val(dados.Objeto.localidade);
        $("#" + endereco + "_Estado", Europa.Controllers.Cliente.Form).val(dados.Objeto.uf);
    });
};

// Retorna os dados do formulário
Europa.Controllers.Cliente.GetFormData = function () {
    var data = Europa.Form.SerializeJson(Europa.Controllers.Cliente.Form);
    console.log(data);
    if (data["DadosFinanceirosDto.RendaFormal"] !== undefined) {
        data["DadosFinanceirosDto.RendaFormal"] = data["DadosFinanceirosDto.RendaFormal"].replace(/\./g, "");
    }
    if (data["DadosFinanceirosDto.RendaInformal"] !== undefined) {
        data["DadosFinanceirosDto.RendaInformal"] = data["DadosFinanceirosDto.RendaInformal"].replace(/\./g, "");
    }
    if (data["DadosFinanceirosDto.RendaMensal"] !== undefined) {
        data["DadosFinanceirosDto.RendaMensal"] = data["DadosFinanceirosDto.RendaMensal"].replace(/\./g, "");
    }
    if (data["DadosFinanceirosDto.FGTS"] !== undefined) {
        data["DadosFinanceirosDto.FGTS"] = data["DadosFinanceirosDto.FGTS"].replace(/\./g, "");
    }
    data["InformacoesGeraisDto.NomeCompleto"] = $("#InformacoesGeraisDto_NomeCompleto").val();
    console.log($("#InformacoesGeraisDto_NomeCompleto").val());

    return data;
};



// Calcula a idade baseado na data de nascimento
Europa.Controllers.Cliente.CalcIdade = function () {
    $("#Idade", Europa.Controllers.Cliente.Form).val(Europa.Date.GetAge($("#DadosPessoaisDto_DataNascimento").val()));
};

// Limpa o formulário
Europa.Controllers.Cliente.LimparFormulario = function () {
    $(Europa.Controllers.Cliente.Form).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    $("modal_integracao_conecta").find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    Europa.Controllers.Cliente.AutoCompleteProfissaoCliente = new Europa.Components.AutoCompleteProfissao()
        .WithTargetSuffix("profissao_cliente")
        .Configure();
}

// Aplica máscaras nos campos
Europa.Controllers.Cliente.AplicarMascaras = function () {
    Europa.Mask.ApplyByClass("cep", Europa.Mask.FORMAT_CEP, true)
    Europa.Mask.ApplyByClass("inteiro", Europa.Mask.FORMAT_INTEIRO);
    Europa.Mask.ApplyByClass("dinheiro", Europa.Mask.FORMAT_MONEY, undefined, true);
    Europa.Mask.Telefone("#InformacoesGeraisDto_TelefoneResidencial");
    Europa.Mask.Telefone("#InformacoesGeraisDto_TelefoneComercial");
    Europa.Mask.Telefone("#ReferenciasDto_TelefonePrimeiraReferencia");
    Europa.Mask.Telefone("#ReferenciasDto_TelefoneSegundaReferencia");
    Europa.Mask.CpfCnpj("#InformacoesGeraisDto_CpfCnpj");

    Europa.Mask.ApplyByClass("telefone", Europa.Mask.FORMAT_TELEFONE_9);

    Europa.Mask.Telefone("#InformacoesGeraisDto_TelefoneLead");
    Europa.Controllers.Cliente.InitDatePickers();
};

// Inicializa os datepickers
Europa.Controllers.Cliente.InitDatePickers = function () {
    Europa.Controllers.Cliente.DataNascimento = new Europa.Components.DatePicker()
        .WithTarget("#DadosPessoaisDto_DataNascimento")
        .WithParentEl("#form_cliente")
        .WithValue($("#DadosPessoaisDto_DataNascimento").val())
        .WithMaxDate(Europa.Date.Now())
        .Configure();
    Europa.Mask.Apply("#DadosPessoaisDto_DataNascimento", Europa.Mask.FORMAT_DATE);

    Europa.Controllers.Cliente.DataEmissao = new Europa.Components.DatePicker()
        .WithTarget("#DadosPessoaisDto_DataEmissao")
        .WithParentEl("#form_cliente")
        .WithValue($("#DadosPessoaisDto_DataEmissao").val())
        .WithMaxDate(Europa.Date.Now())
        .Configure();
    Europa.Mask.Apply("#DadosPessoaisDto_DataEmissao", Europa.Mask.FORMAT_DATE);

    Europa.Controllers.Cliente.DataAdmissao = new Europa.Components.DatePicker()
        .WithTarget("#DadosProfissionaisDto_DataAdmissao")
        .WithParentEl("#form_cliente")
        .WithValue($("#DadosProfissionaisDto_DataAdmissao").val())
        .WithMaxDate(Europa.Date.Now())
        .Configure();
    Europa.Mask.Apply("#DadosProfissionaisDto_DataAdmissao", Europa.Mask.FORMAT_DATE);

    Europa.Controllers.Cliente.DataUltimaParcelaFinanciamentoPaga = new Europa.Components.DatePicker()
        .WithTarget("#Cliente_DataUltimaParcelaFinanciamentoPaga")
        .WithParentEl("#form_cliente")
        .WithValue($("#Cliente_DataUltimaParcelaFinanciamentoPaga").val())
        .WithMaxDate(Europa.Date.Now())
        .Configure();
    Europa.Mask.Apply("#Cliente_DataUltimaParcelaFinanciamentoPaga", Europa.Mask.FORMAT_DATE);

    Europa.Controllers.Cliente.DataUltimaPrestacaoPaga = new Europa.Components.DatePicker()
        .WithTarget("#Cliente_DataUltimaPrestacaoPaga")
        .WithParentEl("#form_cliente")
        .WithValue($("#Cliente_DataUltimaPrestacaoPaga").val())
        .WithMaxDate(Europa.Date.Now())
        .Configure();
    Europa.Mask.Apply("#Cliente_DataUltimaPrestacaoPaga", Europa.Mask.FORMAT_DATE);

    Europa.Controllers.Cliente.UltimaParcelaFinanciamento = new Europa.Components.DatePicker()
        .WithTarget("#Cliente_UltimaParcelaFinanciamento")
        .WithParentEl("#form_cliente")
        .WithValue($("#Cliente_UltimaParcelaFinanciamento").val())
        .Configure();
    Europa.Mask.Apply("#Cliente_UltimaParcelaFinanciamento", Europa.Mask.FORMAT_DATE);

    Europa.Controllers.Cliente.CalcIdade();
};

// Simula eventos de Tipo Residência e TipoRenda
Europa.Controllers.Cliente.EventHandler = function () {
    Europa.Controllers.Cliente.TipoRendaOnChange();
};

// Habilitar edição limpando o formulário
Europa.Controllers.Cliente.Novo = function () {

    Europa.Controllers.Cliente.FieldsetContent = $("#fieldset_formulario").html();
    Europa.Controllers.Cliente.LimparFormulario();
    $("#DadosPessoaisDto_Nacionalidade").val("BR");
    Europa.Controllers.Cliente.EventHandler();
    Europa.Controllers.Cliente.HabilitarEdicao();
    Europa.Controllers.Cliente.NovoCliente = true;

    Europa.Controllers.Cliente.ChangeButtons();
    
};

// Habilitar edição limpando o formulário
Europa.Controllers.Cliente.Editar = function () {
    Europa.Controllers.Cliente.HabilitarEdicao();
    Europa.Controllers.Cliente.HabilitarConjuge();
    Europa.Controllers.Cliente.NovoCliente = false;

    Europa.Controllers.Cliente.ChangeButtons();
};

// Salvar dados
Europa.Controllers.Cliente.Salvar = function (integracao) {

    $("#btn-salvar-cliente-conecta").prop("disabled", true);

    let idProfissao = Europa.Controllers.Cliente.AutoCompleteProfissaoCliente.Value();
    let nomeProfissao = Europa.Controllers.Cliente.AutoCompleteProfissaoCliente.Text();
    var objeto = Europa.Controllers.Cliente.GetFormData();
    var idCliente = Europa.Controllers.Cliente.GetId();

    console.log("Salvar 1");
    //console.log(objeto)
    var url = idCliente == 0 || idCliente == undefined ? Europa.Controllers.Cliente.UrlIncluirCliente : Europa.Controllers.Cliente.UrlAtualizarCliente;

    objeto["NovoCliente"] = Europa.Controllers.Cliente.NovoCliente;
    if (Europa.Controllers.Cliente.NovoCliente) {
        objeto["InformacoesGeraisDto.UuidLead"] = $("#InformacoesGeraisDto_UuidLead").val();
        objeto["InformacoesGeraisDto.NomeLead"] = $("#InformacoesGeraisDto_NomeLead", "#modal_integracao_conecta").val();
        objeto["InformacoesGeraisDto.TelefoneLead"] = $("#InformacoesGeraisDto_TelefoneLead", "#modal_integracao_conecta").val();
    } else {
        objeto["InformacoesGeraisDto.NomeLead"] = $("#InformacoesGeraisDto_NomeLead").val();
        objeto["InformacoesGeraisDto.TelefoneLead"] = $("#InformacoesGeraisDto_TelefoneLead").val();
    }
    if (integracao && $("#Cliente_terceiro_sim:checked").val() == "False") {
        objeto["InformacoesGeraisDto.NomeLead"] = $("#InformacoesGeraisDto_NomeCompleto").val();
        objeto["InformacoesGeraisDto.TelefoneLead"] = $("#InformacoesGeraisDto_TelefoneResidencial").val();
    }

    objeto["InformacoesGeraisDto.NomeCompleto"] = $("#InformacoesGeraisDto_NomeCompleto").val();

    $.post(url, { model: objeto }, function (res) {
        if (res.Sucesso) {

            //console.log(res)

            $("#div_conteudo").html(res.Objeto);
            Europa.Controllers.Cliente.DesabilitarEdicao(true);
            Europa.Controllers.Cliente.InitAutoCompletes();
            Europa.Controllers.Cliente.DisableAutoComplete();
            Europa.Controllers.Cliente.AplicarMascaras();
            Europa.Controllers.Cliente.MensagemError = false;
            if (integracao && $("#Cliente_terceiro_sim:checked").val() == "True")
                Europa.Controllers.Cliente.MensagemError = true;
            if (idCliente == null || idCliente == 0) {
                if (integracao) {
                    $("#mensagem_conecta_texto").text("Integração realizada com sucesso!");                    
                    Europa.Controllers.Cliente.AbrirModalMensagemConecta();
                }
                //Europa.Controllers.Cliente.AbrirModalCadastroPPR();
            } else {
                if (integracao) {
                    $("#mensagem_conecta_texto").text("Integração realizada com sucesso!");
                    Europa.Controllers.Cliente.AbrirModalMensagemConecta();
                }
                else
                    Europa.Informacao.PosAcao(res);
            }
            if (idProfissao == null || idProfissao == 0) {
                Europa.Controllers.Cliente.AutoCompleteProfissaoCliente.Clean();
            } else {
                Europa.Controllers.Cliente.AutoCompleteProfissaoCliente
                    .SetValue(idProfissao, nomeProfissao);
            }
        } else {
            if (integracao) {
                $("#mensagem_conecta_texto").text(res.Mensagens);
                Europa.Controllers.Cliente.MensagemError = true;
                Europa.Controllers.Cliente.AbrirModalMensagemConecta();
            }
            else
                Europa.Informacao.PosAcao(res);
            $("#btn-salvar-cliente-conecta").prop("disabled", false);
        } 
    });
};

// Validar dados de venda
Europa.Controllers.Cliente.ValidarDadosVenda = function () {
    var objeto = Europa.Controllers.Cliente.GetFormData();
    $.post(Europa.Controllers.Cliente.UrlValidarDadosVenda, { dto: objeto }, function (res) {
        if (!res.Sucesso) {
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.Cliente.Form);
        }
    }).done(function (res) {
        Europa.Informacao.PosAcao(res);
    });
};


// Validar dados de venda via integracao
Europa.Controllers.Cliente.ValidarDadosIntegracao = function () {
    var objeto = Europa.Controllers.Cliente.GetFormData();
    var idCliente = Europa.Controllers.Cliente.GetId();

    objeto["NovoCliente"] = Europa.Controllers.Cliente.GetId() == "";
    console.log(objeto);
    $.post(Europa.Controllers.Cliente.UrlValidarDadosIntegracao, { dto: objeto }, function (res) {
        console.log(res)
        if (res.Success) {
            Europa.Informacao.Hide = function () {
                location.href = Europa.Controllers.Cliente.UrlIndex.split('Index')[0] + 'Index/' + res.Data;
            }
            
        }
        Europa.Informacao.PosAcaoBaseResponse(res);
    });
};

// Seleciona o Cliente
Europa.Controllers.Cliente.SelectCliente = function (idCliente, nomeCliente) {
    var basePath = location.href.split("/Index")[0];
    if (idCliente == undefined || idCliente == null)
        location.href = basePath;

    location.href = basePath + "/Index/" + idCliente;
};


Europa.Controllers.Cliente.CadastrarProposta = function () {
    var basePath = location.href.split("/Cliente")[0];
    var idCliente = $("#IdCliente").val();
    if (idCliente == undefined || idCliente == null) {
        location.href = basePath + "/preproposta";
    }

    location.href = basePath + "/preproposta/?idCliente=" + idCliente;
}

Europa.Controllers.Cliente.ChangeButtons = function () {
    $("#btn_salvar_cliente").prop("hidden", false);
    $("#btn_salvar_cliente_lead").prop("hidden", true);
    if (Europa.Controllers.Cliente.NovoCliente) {
        $("#btn_vincular_cliente_lead").prop("hidden", true);
        $("#btn_salvar_cliente").prop("hidden", true);
        $("#btn_salvar_cliente_lead").prop("hidden", false);
    }
}