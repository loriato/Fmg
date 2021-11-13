$(function () {
    Europa.Mask.ApplyByClass("cep", Europa.Mask.FORMAT_CEP, true);
    Europa.Mask.ApplyByClass("cpf", Europa.Mask.FORMAT_CPF, true);
    Europa.Controllers.Leads.InitAutoComplete();
});

Europa.Controllers.Leads.ModalDetalhar = function (obj) {
    Europa.Controllers.Leads.AbriModal();
    Europa.Controllers.Leads.PreencherModal(obj);
};

Europa.Controllers.Leads.AbriModal = function () {
    $("#modal_detalhar_lead").show();
};

Europa.Controllers.Leads.FecharModal = function () {
    $("#modal_detalhar_lead").hide();
    $("#btn_salvar").addClass("hidden");
    $("#Situacao").prop("disabled", true);
    $("#Desistencia").prop("disabled", true);
    $("#DescricaoDesistencia").prop("disabled", true);
    $("#Anotacoes").prop("disabled", true);
    $("#autocomplete_pre_proposta").prop("disabled", true);
};

Europa.Controllers.Leads.PreencherModal = function (obj) {

    
    $("#Id").val(obj.Id);

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


    Europa.Controllers.Leads.OnChangeSituacao();
    //Europa.Controllers.Leads.OnChangeDesistencia();
    
    if (obj.IdCorretor != 0) {
        $("#btn_salvar").removeClass("hidden");
        $("#Situacao").prop("disabled", false);
        $("#Desistencia").prop("disabled", false);
        $("#DescricaoDesistencia").prop("disabled", false);
        $("#Anotacoes").prop("disabled", false);
        $("#autocomplete_pre_proposta").prop("disabled", false);
    }
    if (obj.IdPreProposta != 0 && obj.IdPreProposta != null) {
        Europa.Controllers.Leads.AutoCompletePreProposta.SetValue(obj.IdPreProposta, obj.CodigoPreProposta);
    } else {
        $("#autocomplete_pre_proposta").val("").trigger('change');
    }
   
}

Europa.Controllers.Leads.AtualizarLeadEmpresaVenda = function () {
        
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

    $.post(Europa.Controllers.Leads.UrlAtualizarLeadEmpresaVenda, lead, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.Leads.FecharModal();
            if (Europa.Controllers.Leads.Funcao == "Diretor") {
                Europa.Controllers.Leads.FiltrarDatatableDiretor();
            } else {
                Europa.Controllers.Leads.FiltrarDatatableCorretor();
            }
        }
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.Leads.OnChangeSituacao = function () {
    var situacao = $("#Situacao").val();

    if (situacao == 2) {
        $(".desistencia").removeClass("hidden");   
        Europa.Controllers.Leads.OnChangeDesistencia();
    } else {
        $(".desistencia").addClass("hidden");
        $(".outro").addClass("hidden");
    }   
};

Europa.Controllers.Leads.OnChangeDesistencia = function () {
    var desistencia = $("#Desistencia").val();

    if (desistencia == 3) {
        $(".outro").removeClass("hidden");
        return;
    }
    $(".outro").addClass("hidden");
    
};

Europa.Controllers.Leads.InitAutoComplete = function () {
    Europa.Controllers.Leads.AutoCompletePreProposta = new Europa.Components.AutoCompletePreProposta()
        .WithTargetSuffix("pre_proposta").Configure();
};
