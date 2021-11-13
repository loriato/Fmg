$(function () {
    Europa.Mask.ApplyByClass("cep", Europa.Mask.FORMAT_CEP, true);
    Europa.Mask.ApplyByClass("cpf", Europa.Mask.FORMAT_CPF, true);
    Europa.Controllers.Lead.InitAutoComplete();
});

Europa.Controllers.Lead.ModalDetalhar = function (param) {
    Europa.Controllers.Lead.AbriModal();
    Europa.Controllers.Lead.PreencherModal(param);    
};

Europa.Controllers.Lead.AbriModal = function () {
    $("#modal_detalhar_lead").show();
};

Europa.Controllers.Lead.FecharModal = function () {
    $("#modal_detalhar_lead").hide();
};

Europa.Controllers.Lead.PreencherModal = function (obj) {
    console.log(obj)
    $("#Id").val(obj.Id);
    $("#EmpresaVenda_Id").val(obj.IdEmpresaVenda);

    $("#Lead_DescricaoPacote").val(obj.Pacote);
    $("#Situacao").val(obj.SituacaoLead);
    $("#Desistencia").val(obj.Desistencia);
    $("#DescricaoDesistencia").val(obj.DescricaoDesistencia);

    $("#Lead_NomeCompleto").val(obj.NomeLead);

    var tel1 = Europa.Mask.GetMaskedValue(obj.Telefone1, Europa.Mask.FORMAT_TELEFONE_9);
    var tel2 = Europa.Mask.GetMaskedValue(obj.Telefone2, Europa.Mask.FORMAT_TELEFONE_9);

    $("#Lead_Telefone1").val(tel1);
    $("#Lead_Telefone2").val(tel2);

    $("#Lead_Email").val(obj.Email);

    $("#Lead_Logradouro").val(obj.Logradouro);
    $("#Lead_Numero").val(obj.Numero);
    $("#Lead_Bairro").val(obj.Bairro);
    $("#Lead_Cidade").val(obj.Cidade);
    $("#Lead_Estado").val(obj.Uf);
    $("#Lead_Pais").val(obj.Pais);

    var cep = Europa.Mask.GetMaskedValue(obj.CEP, Europa.Mask.FORMAT_CEP);

    $("#Lead_Cep").val(cep);

    $("#Lead_Complemento").val(obj.Complemento);

    $("#Anotacoes").val(obj.Anotacoes);
    $("#Corretor_Nome").val(obj.NomeCorretor);

    $("#Lead_NomeIndicador").val(obj.NomeCliente);

    var cpf = Europa.Mask.GetMaskedValue(obj.CpfCliente, Europa.Mask.FORMAT_CPF);
    $("#Lead_CpfIndicador").val(cpf);

    Europa.Controllers.Lead.OnChangeSituacao();
    console.log(obj.IdCorretor);
    
    Europa.Controllers.Lead.ConfigurePrePropostaAutocomplete(Europa.Controllers.Lead);

    if (obj.IdPreProposta != 0 && obj.IdPreProposta != null) {
        Europa.Controllers.Lead.AutoCompletePreProposta.SetValue(obj.IdPreProposta, obj.CodigoPreProposta);
    } else {
        $("#autocomplete_pre_proposta").val("").trigger('change');
    }
}

Europa.Controllers.Lead.OnChangeSituacao = function () {
    var situacao = $("#Situacao").val();

    if (situacao == 2) {
        $(".desistencia").removeClass("hidden");
        Europa.Controllers.Lead.OnChangeDesistencia();
    } else {
        $(".desistencia").addClass("hidden");
        $(".outro").addClass("hidden");
    }
};

Europa.Controllers.Lead.OnChangeDesistencia = function () {
    var desistencia = $("#Desistencia").val();

    if (desistencia == 3) {
        $(".outro").removeClass("hidden");
        return;
    }
    $(".outro").addClass("hidden");

};

Europa.Controllers.Lead.InitAutoComplete = function () {
    Europa.Controllers.Lead.AutoCompletePreProposta = new Europa.Components.AutoCompletePreProposta()
        .WithTargetSuffix("pre_proposta").Configure();
};

Europa.Controllers.Lead.AtualizarLeadEmpresaVenda = function () {

    var situacao = $("#Situacao").val();

    if (situacao != 2) {
        $("#Desistencia").val("").trigger("change");
    }

    var desistencia = $("#Desistencia").val();

    if (desistencia != 3) {
        $("#DescricaoDesistencia").val("");
    }

    var lead = {
        Id: $("#Id").val(),
        Situacao: $("#Situacao").val(),
        Desistencia: desistencia,
        DescricaoDesistencia: $("#DescricaoDesistencia").val(),
        Anotacoes: $("#Anotacoes").val(),
        PreProposta: {
            Id: $("#autocomplete_pre_proposta").val()
        }

    };

    $.post(Europa.Controllers.Lead.UrlAtualizarLeadEmpresaVenda, lead, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.Lead.FecharModal();
            Europa.Controllers.Lead.Filtrar();
        }
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.Lead.ConfigurePrePropostaAutocomplete = function (autocompleteWrapper) {
    autocompleteWrapper.AutoCompletePreProposta.Data = function (params) {
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
                        return $("#EmpresaVenda_Id").val();
                    },
                    column: 'idEmpresaVenda'
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
    autocompleteWrapper.AutoCompletePreProposta.Configure();
}