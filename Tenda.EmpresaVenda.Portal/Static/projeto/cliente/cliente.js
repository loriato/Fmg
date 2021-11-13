Europa.Controllers.Cliente = {};
Europa.Controllers.Cliente.UrlIndex = undefined;
Europa.Controllers.Cliente.UrlIncluirCliente = undefined;
Europa.Controllers.Cliente.UrlAtualizarCliente = undefined;
Europa.Controllers.Cliente.UrlValidarDadosVenda = undefined;
Europa.Controllers.Cliente.UrlCadastrarProposta = undefined;
Europa.Controllers.Cliente.Form = "#form_cliente";
Europa.Controllers.Cliente.HeaderNovo = "#cliente_header_novo";
Europa.Controllers.Cliente.HeaderInfo = "#cliente_header_info";
Europa.Controllers.Cliente.FieldsetContent = undefined;

$(function () {
    Europa.NavbarScrollControl("tabMenu", "block_content_page");
    $("#tabMenu li").click(function (e) {
        e.preventDefault();
        Europa.OnTabChange(this, "tabMenu", "block_content_page");
    });

    Europa.Components.DatePicker.AutoApply();
    Europa.Controllers.Cliente.AplicarMascaras();
    Europa.Controllers.Cliente.MontarHeader();
    Europa.Controllers.Cliente.EventHandler();
    Europa.Controllers.Cliente.InitAutoCompletes();
    setTimeout(Europa.Controllers.Cliente.DisableAutoComplete, 100);
    Europa.Controllers.Cliente.ConfigureRadioButtons();
});

// Monta e mostra o header com as informações do usuário (se existir)
Europa.Controllers.Cliente.MontarHeader = function () {
    var val = Europa.Controllers.Cliente.GetId();
    if (val > 0) {
        $(Europa.Controllers.Cliente.HeaderNovo).hide();
        $(Europa.Controllers.Cliente.HeaderInfo).show();
        $("#cliente_header_email").empty();
        $("#cliente_header_nome").empty();

        var nome = $("#Cliente_NomeCompleto", Europa.Controllers.Cliente.Form).val();
        var email = $("#Cliente_Email", Europa.Controllers.Cliente.Form).val();
        var telefone = $("#Cliente_TelefoneResidencial", Europa.Controllers.Cliente.Form).val();
        var telefoneFormatado = Europa.String.FormatTelefone(telefone);
        $("#cliente_header_nome").html(nome);
        
        if (email && email != "") {
            $("#cliente_header_email").html(email);
        }
        if (telefoneFormatado && telefoneFormatado != "") {
            if (email && email != "")
                telefoneFormatado = "&nbsp;|&nbsp;" + telefoneFormatado;
            $("#cliente_header_telefone").html(telefoneFormatado);
        }
        $("#titlebar-name-target").css("padding-top", "0")
        $("#titlebar-name-target").css("padding-bottom", "0")
        $("#titlebar-name-target").css("padding-left", "3")
    } else {
        $(Europa.Controllers.Cliente.HeaderNovo).show();
        $(Europa.Controllers.Cliente.HeaderInfo).hide();
    }
};

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
    if ($("#Cliente_EstadoCivil").val() == 2) {
        $("#btn_select_cliente").prop("disabled", "");
        $("#Cliente_RegimeBens").prop("disabled", "");
    } else {
        $("#btn_select_cliente").prop("disabled", "disabled");
        $("#Cliente_RegimeBens").prop("disabled", "disabled");
        $("#Cliente_RegimeBens").val("");
        $("[name='Familiar.Cliente2.NomeCompleto']").val("");
        $("[name='Familiar.Cliente2.Id']").val("");
    }
}

Europa.Controllers.Cliente.SelectConjuge = function (data) {
    $.post(Europa.Controllers.Cliente.UrlBuscarCliente, { idCliente: data },
    function (response){
        if (response.Sucesso) {
            var cliente = response.Objeto;
            $("[name='Familiar.Cliente1.Id']").val($("#Cliente_Id").val());
            $("[name='Familiar.Cliente2.NomeCompleto']").val(cliente.NomeCompleto);
            $("[name='Familiar.Cliente2.Id']").val(cliente.Id);
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
        $("#Cliente_Profissao_Id").val(Europa.Controllers.Cliente.AutoCompleteProfissaoCliente.Value());
        $("#Cliente_Profissao_Nome").val(Europa.Controllers.Cliente.AutoCompleteProfissaoCliente.Text());
    })

    Europa.Controllers.Cliente.AutoCompleteBanco = new Europa.Components.AutoCompleteBanco()
        .WithAjax(false)
        .WithTargetSuffix("banco")
        .Configure();

    $("#autocomplete_banco").change(function () {
        $("#Cliente_Banco_Id").val(Europa.Controllers.Cliente.AutoCompleteBanco.Value());
    })

    Europa.Controllers.Cliente.PreencheAutoComplete();
    $(".daterangepicker.dropdown-menu").css("z-index", 10050);
    Europa.AutoCompleteFix();

};

// Habilita os autocompletes
Europa.Controllers.Cliente.EnableAutoComplete = function () {
    Europa.Controllers.Cliente.AutoCompleteProfissaoCliente.Enable();
    Europa.Controllers.Cliente.AutoCompleteBanco.Enable();
};

// Desabilita os autocompletes
Europa.Controllers.Cliente.DisableAutoComplete = function () {
    Europa.Controllers.Cliente.AutoCompleteProfissaoCliente.Disable();
    Europa.Controllers.Cliente.AutoCompleteBanco.Disable();
};

// Preenche os autocompletes com o valor do model passado
Europa.Controllers.Cliente.PreencheAutoComplete = function () {
    if ($("#Cliente_Profissao_Id", Europa.Controllers.Cliente.Form).val() > 0) {
        Europa.Controllers.Cliente.AutoCompleteProfissaoCliente
            .SetValue($("#Cliente_Profissao_Id", Europa.Controllers.Cliente.Form).val(),
                $("#Cliente_Profissao_Nome", Europa.Controllers.Cliente.Form).val());
    }
    var idBanco = $("#Cliente_Banco_Id", Europa.Controllers.Cliente.Form).val();
    if (idBanco > 0) {
        Europa.Controllers.Cliente.AutoCompleteBanco.SetSelected(idBanco);
    }
};

// Habilitar edição
Europa.Controllers.Cliente.HabilitarEdicao = function () {
    $("#div_buttons_edicao").show();
    $("#div_buttons_visualizacao").hide();
    $("#btn_validar").show();
    Europa.Controllers.Cliente.MontarHeader();
    $("#fieldset_formulario", Europa.Controllers.Cliente.Form).removeAttr("disabled");
    Europa.Controllers.Cliente.AplicarMascaras();
    Europa.Controllers.Cliente.AutoCompleteProfissaoCliente.Enable();
    Europa.Controllers.Cliente.OnChangePossuiContaBanco();
    Europa.HideSubstituteFields();
    $("#btn_select_cliente").prop("disabled", "disabled");
    $("#Cliente_RegimeBens").prop("disabled", "disabled");
    $("#Cliente_TempoDeEmpresa").attr("readonly", true);
};

// Desabilitar edição
Europa.Controllers.Cliente.DesabilitarEdicao = function (isSaveSucess) {
    if (Europa.Controllers.Cliente.FieldsetContent != undefined && (isSaveSucess == undefined || isSaveSucess == false)) {
        $("#fieldset_formulario").html(Europa.Controllers.Cliente.FieldsetContent);
        $("#div_profissao span").remove();
        $("#div_banco span").remove();
        Europa.Controllers.Cliente.InitAutoCompletes();
        Europa.Controllers.Cliente.PreencheAutoComplete();
    }
    $("#div_buttons_edicao").hide();
    $("#div_buttons_visualizacao").show();
    $("#btn_validar").hide();
    $("#fieldset_formulario", Europa.Controllers.Cliente.Form).attr("disabled", "disabled");
    Europa.Controllers.Cliente.MontarHeader();
    Europa.Controllers.Cliente.AplicarMascaras();
    Europa.Controllers.Cliente.DisableAutoComplete();
    Europa.ShowSubstituteFields();
};

// Retorna id do cliente
Europa.Controllers.Cliente.GetId = function () {
    return $("#Cliente_Id", Europa.Controllers.Cliente.Form).val();
};

// Verifica o tipo de residência e habilita o campo Valor Aluguel, em caso de residência alugada
Europa.Controllers.Cliente.TipoResidenciaOnChange = function () {
    var val = $("#Cliente_TipoResidencia", Europa.Controllers.Cliente.Form).val();
    if (val == 2) {
        $("#Cliente_ValorAluguel", Europa.Controllers.Cliente.Form).removeAttr("disabled");
    } else {
        $("#Cliente_ValorAluguel", Europa.Controllers.Cliente.Form).attr("disabled", "disabled");
        $("#Cliente_ValorAluguel", Europa.Controllers.Cliente.Form).val("");
    }
};

// Verifica o tipo de renda e habilita/desabilita os campos corretos (Renda Formal e Informal)
Europa.Controllers.Cliente.TipoRendaOnChange = function () {
    var val = $("#Cliente_TipoRenda", Europa.Controllers.Cliente.Form).val();
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
    $("#Cliente_" + renda, Europa.Controllers.Cliente.Form).removeAttr("disabled");
    $("#Cliente_Origem" + renda, Europa.Controllers.Cliente.Form).removeAttr("disabled");
};

// Desabilita e limpa os campos de renda
Europa.Controllers.Cliente.TipoRendaDisableRenda = function (renda) {
    if (!Europa.Controllers.Cliente.IsView) {
        $("#Cliente_" + renda, Europa.Controllers.Cliente.Form).attr("disabled", "disabled");
        $("#Cliente_" + renda, Europa.Controllers.Cliente.Form).val("");
        $("#Cliente_Origem" + renda, Europa.Controllers.Cliente.Form).attr("disabled", "disabled");
        $("#Cliente_Origem" + renda, Europa.Controllers.Cliente.Form).val("");
        Europa.Controllers.Cliente.RendaOnchange();
    }
};

// Recalcula a renda mensal
Europa.Controllers.Cliente.RendaOnchange = function () {
    var rf = $("#Cliente_RendaFormal", Europa.Controllers.Cliente.Form).cleanVal();
    var ri = $("#Cliente_RendaInformal", Europa.Controllers.Cliente.Form).cleanVal();
    var rm = 0;
    rm = rf != undefined && rf.trim().length == 0 ? 0 : parseFloat(rf);
    rm += ri != undefined && ri.trim().length == 0 ? 0 : parseFloat(ri);
    var selector = $("#Cliente_RendaMensal", Europa.Controllers.Cliente.Form);
    selector.val(selector.masked(rm));
};

Europa.Controllers.Cliente.ConfigureRadioButtons = function () {
    Europa.Controllers.Cliente.OnChangePossuiVeiculo();
    Europa.Controllers.Cliente.OnChangeVeiculoFinanciado();
    Europa.Controllers.Cliente.OnChangePossuiContaBanco();
    Europa.Controllers.Cliente.OnChangePossuiComprometimentoFinanceiro();
    Europa.Controllers.Cliente.OnChangePossuiCartaoCredito();
};

// Habilita/desabilita campos referente ao valor do veículo
Europa.Controllers.Cliente.OnChangePossuiVeiculo = function () {
    var val = $("input[name='Cliente.PossuiVeiculo']:checked").val();
    if (val == 'False' || val == undefined) {
        $("#Cliente_ValorVeiculo").attr("disabled", true);
        $("input[name='Cliente.VeiculoFinanciado']").attr('disabled', true);
        $("#Cliente_ValorVeiculo").val(0);
        $("input[name='Cliente.VeiculoFinanciado']").prop("checked", false);
        Europa.Controllers.Cliente.OnChangeVeiculoFinanciado();
    }
    else {
        $("#Cliente_ValorVeiculo").removeAttr("disabled");
        $("input[name='Cliente.VeiculoFinanciado']").removeAttr("disabled");
    }
};

// Habilita/desabilita campos referente ao financiamento do veículo
Europa.Controllers.Cliente.OnChangeVeiculoFinanciado = function () {
    var val = $("input[name='Cliente.VeiculoFinanciado']:checked").val();
    if (val == 'False' || val == undefined) {
        $("#Cliente_ValorUltimaParcelaFinanciamentoVeiculo").attr("disabled", true);
        $("#Cliente_DataUltimaParcelaFinanciamentoPaga").attr("disabled", true);
        $("#btn_data_financiamento").attr("disabled", true);

        $("#Cliente_ValorUltimaParcelaFinanciamentoVeiculo").val(0);
        $("#Cliente_DataUltimaParcelaFinanciamentoPaga").val("");
    }
    else {
        $("#Cliente_ValorUltimaParcelaFinanciamentoVeiculo").removeAttr("disabled");
        $("#Cliente_DataUltimaParcelaFinanciamentoPaga").removeAttr("disabled");
        $("#btn_data_financiamento").removeAttr("disabled");
    }
};

// Habilita/desabilita campos referente à conta bancária
Europa.Controllers.Cliente.OnChangePossuiContaBanco = function () {
    var val = $("input[name='Cliente.PossuiContaBanco']:checked").val();
    if (val == 'False' || val == undefined) {
        Europa.Controllers.Cliente.AutoCompleteBanco.Disable();
        Europa.Controllers.Cliente.AutoCompleteBanco.Clean();
        $("#Cliente_LimiteChequeEspecial").attr("disabled", true);
        $("#Cliente_LimiteChequeEspecial").val(0);
    }
    else {
        Europa.Controllers.Cliente.AutoCompleteBanco.Enable();
        $("#Cliente_LimiteChequeEspecial").removeAttr("disabled");
    }
};

// Habilita/desabilita campos referente ao comprometimento financeiro
Europa.Controllers.Cliente.OnChangePossuiComprometimentoFinanceiro = function () {
    var val = $("input[name='Cliente.PossuiComprometimentoFinanceiro']:checked").val();
    if (val == 'False' || val == undefined) {
        $("#Cliente_ValorComprometimentoFinanceiro").attr("disabled", true);
        $("#Cliente_PrestacoesVencer").attr("disabled", true);
        $("#Cliente_DataUltimaPrestacaoPaga").attr("disabled", true);
        $("#btn_data_prestacao").attr("disabled", true);
        $("#Cliente_ValorComprometimentoFinanceiro").val(0);
        $("#Cliente_PrestacoesVencer").val(0);
        $("#Cliente_ValorComprometimentoFinanceiro").val("");
        $("#Cliente_DataUltimaPrestacaoPaga").val("");
    }
    else {
        $("#Cliente_ValorComprometimentoFinanceiro").removeAttr("disabled");
        $("#Cliente_PrestacoesVencer").removeAttr("disabled");
        $("#Cliente_DataUltimaPrestacaoPaga").removeAttr("disabled");
        $("#btn_data_prestacao").removeAttr("disabled");
    }
};

// Habilita/desabilita campos referente à conta bancária
Europa.Controllers.Cliente.OnChangePossuiCartaoCredito = function () {
    var val = $("input[name='Cliente.PossuiCartaoCredito']:checked").val();
    if (val == 'False' || val == undefined) {
        $("#Cliente_BandeiraCartaoCredito").attr("disabled", true);
        $("#Cliente_LimiteCartaoCredito").attr("disabled", true);
        $("#Cliente_BandeiraCartaoCredito").val("");
        $("#Cliente_LimiteCartaoCredito").val(0);
    }
    else {
        $("#Cliente_BandeiraCartaoCredito").removeAttr("disabled");
        $("#Cliente_LimiteCartaoCredito").removeAttr("disabled");
    }
};
// Buscar tempo de empresa

Europa.Controllers.Cliente.OnChangeTempoEmpresa = function (input) {
   
    var data = $(input).val();

    var dd1 = data.slice(0,2);
    var mm1 = data.slice(3,5);
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
    $("#Cliente_TempoDeEmpresa").val(numberOfMonths)
}

 //Busca dados cep Cliente
Europa.Controllers.Cliente.OnChangeCepEnderecoCliente = function (input) {
    Europa.Controllers.Cliente.OnChangeCep(input, "EnderecoCliente");
};

// Busca dados cep Empresa
Europa.Controllers.Cliente.OnChangeCepEnderecoEmpresa = function (input) {
    Europa.Controllers.Cliente.OnChangeCep(input, "EnderecoEmpresa");
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
        $("#" + endereco + "_Logradouro", Europa.Controllers.Cliente.Form).val(dados.logradouroAbrev);
        $("#" + endereco + "_Bairro", Europa.Controllers.Cliente.Form).val(dados.bairro);
        $("#" + endereco + "_Cidade", Europa.Controllers.Cliente.Form).val(dados.localidade);
        $("#" + endereco + "_Estado", Europa.Controllers.Cliente.Form).val(dados.uf);
    });
};

// Retorna os dados do formulário
Europa.Controllers.Cliente.GetFormData = function () {
    var data = Europa.Form.SerializeJson(Europa.Controllers.Cliente.Form);
    if (data["ValorFGTS"] !== undefined) {
        data["ValorFGTS"] = data["ValorFGTS"].replace(/\./g, "");
    }
    if (data["Cliente.RendaMensal"] !== undefined) {
        data["Cliente.RendaMensal"] = data["Cliente.RendaMensal"].replace(/\./g, "");
    }
    if (data["Cliente.RendaInformal"] !== undefined) {
        data["Cliente.RendaInformal"] = data["Cliente.RendaInformal"].replace(/\./g, "");
    }
    if (data["Cliente.FGTS"] !== undefined) {
        data["Cliente.FGTS"] = data["Cliente.FGTS"].replace(/\./g, "");
    }
    if (data["Cliente.RendaFormal"] !== undefined) {
        data["Cliente.RendaFormal"] = data["Cliente.RendaFormal"].replace(/\./g, "");
    }
    if (data["Cliente.ValorAluguel"] !== undefined) {
        data["Cliente.ValorAluguel"] = data["Cliente.ValorAluguel"].replace(/\./g, "");
    }
    if (data["Cliente.ValorFinanciamento"] !== undefined) {
        data["Cliente.ValorFinanciamento"] = data["Cliente.ValorFinanciamento"].replace(/\./g, "");
    }
    if (data["Cliente.ValorVeiculo"] !== undefined) {
        data["Cliente.ValorVeiculo"] = data["Cliente.ValorVeiculo"].replace(/\./g, "");
    }
    if (data["Cliente.ValorUltimaParcelaFinanciamentoVeiculo"] !== undefined) {
        data["Cliente.ValorUltimaParcelaFinanciamentoVeiculo"] = data["Cliente.ValorUltimaParcelaFinanciamentoVeiculo"].replace(/\./g, "");
    }
    if (data["Cliente.LimiteChequeEspecial"] !== undefined) {
        data["Cliente.LimiteChequeEspecial"] = data["Cliente.LimiteChequeEspecial"].replace(/\./g, "");
    }
    if (data["Cliente.ValorComprometimentoFinanceiro"] !== undefined) {
        data["Cliente.ValorComprometimentoFinanceiro"] = data["Cliente.ValorComprometimentoFinanceiro"].replace(/\./g, "");
    }
    if (data["Cliente.LimiteCartaoCredito"] !== undefined) {
        data["Cliente.LimiteCartaoCredito"] = data["Cliente.LimiteCartaoCredito"].replace(/\./g, "");
    }
    if (data["EnderecoCliente.Logradouro"] !== undefined) {
       data["EnderecoCliente.Logradouro"] = $("#EnderecoCliente_Logradouro").val().slice(0, 24);
    }
    if (data["EnderecoEmpresa.Logradouro"] !== undefined) {
        data["EnderecoEmpresa.Logradouro"] = $("#EnderecoEmpresa_Logradouro").val().slice(0, 24);
    }
    return data;
};



// Calcula a idade baseado na data de nascimento
Europa.Controllers.Cliente.CalcIdade = function () {
    $("#Idade", Europa.Controllers.Cliente.Form).val(Europa.Date.GetAge($("#Cliente_DataNascimento").val()));
};

// Limpa o formulário
Europa.Controllers.Cliente.LimparFormulario = function () {
    $(Europa.Controllers.Cliente.Form).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    Europa.Controllers.Cliente.AutoCompleteProfissaoCliente.Clean();
}

// Aplica máscaras nos campos
Europa.Controllers.Cliente.AplicarMascaras = function () {
    Europa.Mask.ApplyByClass("cep", Europa.Mask.FORMAT_CEP, true)
    Europa.Mask.ApplyByClass("inteiro", Europa.Mask.FORMAT_INTEIRO);
    Europa.Mask.ApplyByClass("dinheiro", Europa.Mask.FORMAT_MONEY, undefined, true);
    Europa.Mask.Telefone("#Cliente_TelefoneResidencial");
    Europa.Mask.Telefone("#Cliente_TelefoneComercial");
    Europa.Mask.Telefone("#Cliente_TelefonePrimeiraReferencia");
    Europa.Mask.Telefone("#Cliente_TelefoneSegundaReferencia");
    Europa.Mask.CpfCnpj("#Cliente_CpfCnpj");

    Europa.Controllers.Cliente.InitDatePickers();
};

// Inicializa os datepickers
Europa.Controllers.Cliente.InitDatePickers = function () {
    Europa.Controllers.Cliente.DataNascimento = new Europa.Components.DatePicker()
        .WithTarget("#Cliente_DataNascimento")
        .WithParentEl("#form_cliente")
        .WithValue($("#Cliente_DataNascimento").val())
        .WithMaxDate(Europa.Date.Now())
        .Configure();
    Europa.Mask.Apply("#Cliente_DataNascimento", Europa.Mask.FORMAT_DATE);

    Europa.Controllers.Cliente.DataEmissao = new Europa.Components.DatePicker()
        .WithTarget("#Cliente_DataEmissao")
        .WithParentEl("#form_cliente")
        .WithValue($("#Cliente_DataEmissao").val())
        .WithMaxDate(Europa.Date.Now())
        .Configure();
    Europa.Mask.Apply("#Cliente_DataEmissao", Europa.Mask.FORMAT_DATE);

    Europa.Controllers.Cliente.DataAdmissao = new Europa.Components.DatePicker()
        .WithTarget("#Cliente_DataAdmissao")
        .WithParentEl("#form_cliente")
        .WithValue($("#Cliente_DataAdmissao").val())
        .WithMaxDate(Europa.Date.Now())
        .Configure();
    Europa.Mask.Apply("#Cliente_DataAdmissao", Europa.Mask.FORMAT_DATE);

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
    Europa.Controllers.Cliente.TipoResidenciaOnChange();
    Europa.Controllers.Cliente.TipoRendaOnChange();
};

// Habilitar edição limpando o formulário
Europa.Controllers.Cliente.Novo = function () {
    Europa.Controllers.Cliente.FieldsetContent = $("#fieldset_formulario").html();
    Europa.Controllers.Cliente.LimparFormulario();
    $("#Cliente_Nacionalidade").val("BR");
    Europa.Controllers.Cliente.EventHandler();
    Europa.Controllers.Cliente.HabilitarEdicao();
};

// Habilitar edição limpando o formulário
Europa.Controllers.Cliente.Editar = function () {
    Europa.Controllers.Cliente.HabilitarEdicao();
    Europa.Controllers.Cliente.HabilitarConjuge();
};

// Salvar dados
Europa.Controllers.Cliente.Salvar = function () {
    var objeto = Europa.Controllers.Cliente.GetFormData();
    var idCliente = Europa.Controllers.Cliente.GetId();
    var url = idCliente == 0 || idCliente == undefined ? Europa.Controllers.Cliente.UrlIncluirCliente : Europa.Controllers.Cliente.UrlAtualizarCliente;
    $.post(url, { dto: objeto }, function (res) {
        if (!res.Sucesso) {
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.Cliente.Form);
            if ($.inArray("Cliente.Banco", res.Campos) > 0) {
                $("#div_banco").addClass("has-error");
            } else {
                $("#div_banco").removeClass("has-error");
            }
        }
    }).done(function (res) {
        if (res.Sucesso) {
            $("#div_conteudo").html(res.Objeto);
            Europa.Controllers.Cliente.DesabilitarEdicao(true);
            Europa.Controllers.Cliente.InitAutoCompletes();
            Europa.Controllers.Cliente.DisableAutoComplete();
            Europa.Controllers.Cliente.AplicarMascaras();
            if (idCliente == null || idCliente == 0) {
                $("#modal_cadastrar_proposta").modal("show");
            } else {
                Europa.Informacao.PosAcao(res);
            }
            
        } else {
            Europa.Informacao.PosAcao(res);
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
    $.post(Europa.Controllers.Cliente.UrlValidarDadosIntegracao, { dto: objeto }, function (res) {
        if (!res.Sucesso) {
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.Cliente.Form);
        }
    }).done(function (res) {
        if (res.Sucesso) {
            Europa.Controllers.Cliente.DesabilitarEdicao(true);
            Europa.Controllers.Cliente.InitAutoCompletes();
            Europa.Controllers.Cliente.DisableAutoComplete();
            Europa.Controllers.Cliente.AplicarMascaras();
            $("#Cliente_Id").val(res.Objeto.Cliente.Id);
            if (idCliente == null || idCliente == 0) {
                $("#modal_cadastrar_proposta").modal("show");
            } else {
                Europa.Informacao.PosAcao(res);
            }
        }
        else {
            Europa.Informacao.PosAcao(res);
        }
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

    var basePath = location.href.split("/cliente")[0];
    var idCliente = $("#Cliente_Id").val();
    if (idCliente == undefined || idCliente == null) {
        location.href = basePath + "/preproposta";
    }
        

    location.href = basePath + "/preproposta/?idCliente=" + idCliente;    
}