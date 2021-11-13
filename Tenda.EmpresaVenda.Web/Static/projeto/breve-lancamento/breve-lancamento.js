Europa.Controllers.BreveLancamento = {};
Europa.Controllers.BreveLancamento.Tabela = {};
Europa.Controllers.BreveLancamento.Permissoes = {};
Europa.Controllers.BreveLancamento.IdFormularioBreveLancamento = "#form_breve_lancamento";
Europa.Controllers.BreveLancamento.IdFormularioEnderecoBreveLancamento = "#form_endereco_breve_lancamento";
Europa.Controllers.BreveLancamento.FormBreveLancamento = undefined;
Europa.Controllers.BreveLancamento.ModoEdicao = false;


$(function () {

    $("#filtro_estados").select2({
        trags: true
    });
    Europa.Controllers.BreveLancamento.InitAutocomplete();
    Europa.Controllers.BreveLancamento.FiltrarTabela();
});

Europa.Controllers.BreveLancamento.InitAutocomplete = function () {

    Europa.Controllers.BreveLancamento.AutoCompleteEmpreendimentoFiltro = new Europa.Components.AutoCompleteEmpreendimento()
        .WithTargetSuffix("empreendimento_filtro")
        .Configure();
    Europa.Controllers.BreveLancamento.AutoCompleteRegionalFiltro = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regional_filtro")
        .Configure();
};

////////////////////////////////////////////////////////////////////////////////////
// Funções Edição
////////////////////////////////////////////////////////////////////////////////////
Europa.Controllers.BreveLancamento.HabilitarEdicao = function () {
    Europa.Controllers.BreveLancamento.HideElement("#div_visualizacao");
    Europa.Controllers.BreveLancamento.HideElement("#botoes_visualizacao");
    Europa.Controllers.BreveLancamento.HideElement("#botoes_detalhamento");
    Europa.Controllers.BreveLancamento.ShowElement("#div_edicao");
    Europa.Controllers.BreveLancamento.ShowElement("#botoes_edicao");
    Europa.Controllers.BreveLancamento.AplicarMascaras();
    $('#Situacao').val(1).trigger('change');
    $('#fieldset_breve_lancamento').removeAttr('disabled');

};

Europa.Controllers.BreveLancamento.HabilitarDetalhamento = function () {
    Europa.Controllers.BreveLancamento.HideElement("#botoes_edicao");
    Europa.Controllers.BreveLancamento.HideElement("#div_visualizacao");
    Europa.Controllers.BreveLancamento.HideElement("#botoes_visualizacao");
    Europa.Controllers.BreveLancamento.ShowElement("#div_edicao");
    Europa.Controllers.BreveLancamento.ShowElement("#botoes_detalhamento");
    Europa.Controllers.BreveLancamento.AplicarMascaras();
    $('#Situacao').val(1).trigger('change');
    $('#fieldset_breve_lancamento').attr('disabled', 'disabled');

};

Europa.Controllers.BreveLancamento.Incluir = function () {
    Europa.Controllers.BreveLancamento.HabilitarEdicao();
    Europa.Controllers.BreveLancamento.PreencherForm(0);
};

Europa.Controllers.BreveLancamento.CancelarEdicao = function () {
    Europa.Controllers.BreveLancamento.LimparFormularios();
    Europa.Controllers.BreveLancamento.ShowElement("#div_visualizacao");
    Europa.Controllers.BreveLancamento.ShowElement("#botoes_visualizacao");
    Europa.Controllers.BreveLancamento.HideElement("#botoes_detalhamento");
    Europa.Controllers.BreveLancamento.HideElement("#div_edicao");
    Europa.Controllers.BreveLancamento.HideElement("#botoes_edicao");
    Europa.Controllers.BreveLancamento.ModoEdicao = false;
};

Europa.Controllers.BreveLancamento.LimparFormularios = function () {
    $(Europa.Controllers.BreveLancamento.IdFormularioBreveLancamento).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('').trigger('reset');
    $(Europa.Controllers.BreveLancamento.IdFormularioEnderecoBreveLancamento).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('').trigger('reset');

    Europa.Validator.ClearForm(Europa.Controllers.BreveLancamento.IdFormularioBreveLancamento);
    Europa.Validator.ClearForm(Europa.Controllers.BreveLancamento.IdFormularioEnderecoBreveLancamento);
};

Europa.Controllers.BreveLancamento.IncorporarFormularios = function () {
    var formBreveLancamento = Europa.Form.SerializeJson(Europa.Controllers.BreveLancamento.IdFormularioBreveLancamento);
    formBreveLancamento.Situacao = $("#Situacao", Europa.Controllers.BreveLancamento.IdFormularioBreveLancamento).val();
    var formEnderecoBreveLancamento = Europa.Form.SerializeJson(Europa.Controllers.BreveLancamento.IdFormularioEnderecoBreveLancamento);
    var result = $.extend(formBreveLancamento, formEnderecoBreveLancamento, formDadosTributarios);
    return result;
};

Europa.Controllers.BreveLancamento.Salvar = function () {

    var formBreveLancamento = Europa.Form.SerializeJson(Europa.Controllers.BreveLancamento.IdFormularioBreveLancamento);
    var formEnderecoBreveLancamento = Europa.Form.SerializeJson(Europa.Controllers.BreveLancamento.IdFormularioEnderecoBreveLancamento);
    var urlSalvar = Europa.Controllers.BreveLancamento.ModoEdicao ? Europa.Controllers.BreveLancamento.UrlAlterar : Europa.Controllers.BreveLancamento.UrlIncluir;

    formBreveLancamento.DisponivelCatalogo = formBreveLancamento.DisponivelCatalogo.toLowerCase() === "true";
    formBreveLancamento.Empreendimento = {
        Id: $(Europa.Controllers.BreveLancamento.FormBreveLancamento).find("#Empreendimento_Id").val(),
        Nome: $(Europa.Controllers.BreveLancamento.FormBreveLancamento).find("#Empreendimento_Nome").val()
    };

    formBreveLancamento["Regional"] = { Id: $('#autocomplete_regionais').val() };

    console.log(formBreveLancamento);

    $.post(urlSalvar, { enderecoBreveLancamento: formEnderecoBreveLancamento, breveLancamento: formBreveLancamento }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.BreveLancamento.CancelarEdicao();
            Europa.Controllers.BreveLancamento.FiltrarTabela();
        } else {
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.BreveLancamento.IdFormularioBreveLancamento);
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.BreveLancamento.IdFormularioEnderecoBreveLancamento);
        }

        Europa.Informacao.PosAcao(res);
    });
};


Europa.Controllers.BreveLancamento.HideElement = function (idElemento) {
    $(idElemento).css("display", "none");
};

Europa.Controllers.BreveLancamento.ShowElement = function (idElemento) {
    $(idElemento).css("display", "");
};

Europa.Controllers.BreveLancamento.OnChangeCepBreveLancamento = function (input) {
    Europa.Controllers.BreveLancamento.OnChangeCep(input, Europa.Controllers.BreveLancamento.IdFormularioEnderecoBreveLancamento);
};

Europa.Controllers.BreveLancamento.OnChangeCepRepresentanteLegal = function (input) {
    Europa.Controllers.BreveLancamento.OnChangeCep(input, Europa.Controllers.BreveLancamento.IdFormularioEnderecoBreveLancamento);
};

Europa.Controllers.BreveLancamento.OnChangeCep = function (input, form) {
    var cep = $(input).val().replace(/\D/g, '');
    if (cep == "") {
        return;
    }
    var validacep = /^[0-9]{8}$/;
    if (!validacep.test(cep)) {
        return;
    }
    Europa.Components.Cep.Search(cep, function (dados) {
        $("#Logradouro", form).val(dados.logradouroAbrev);
        $("#Bairro", form).val(dados.bairro);
        $("#Cidade", form).val(dados.localidade);
        $("#Estado", form).val(dados.uf);
    });
};

Europa.Controllers.BreveLancamento.AplicarMascaras = function () {
    $("#Cep", Europa.Controllers.BreveLancamento.IdFormularioEnderecoBreveLancamento).mask("00000-000");
    $("#Cep", Europa.Controllers.BreveLancamento.IdFormularioEnderecoRepresentanteLegal).mask("00000-000");
};

Europa.Controllers.BreveLancamento.Editar = function (idBreveLancamento) {
    Europa.Controllers.BreveLancamento.HabilitarEdicao();
    Europa.Controllers.BreveLancamento.PreencherForm(idBreveLancamento);
    Europa.Controllers.BreveLancamento.ModoEdicao = true;
};


Europa.Controllers.BreveLancamento.Detalhar = function (idBreveLancamento) {
    Europa.Controllers.BreveLancamento.HabilitarDetalhamento();
    Europa.Controllers.BreveLancamento.VisualizarForm(idBreveLancamento);
};

Europa.Controllers.BreveLancamento.PreencherForm = function (idBreveLancamento) {
    $.get(Europa.Controllers.BreveLancamento.UrlBuscarBreveLancamento, { idBreveLancamento: idBreveLancamento }, function (res) {
        console.log(res);
        if (res.Sucesso) {
            $("#div_form_breve_lancamento").html(res.Objeto.htmlBreveLancamento);
            $("#div_form_endereco_breve_lancamento").html(res.Objeto.htmlEnderecoBreveLancamento);
            Europa.Controllers.BreveLancamento.AutoCompleteRegional = new Europa.Components.AutoCompleteRegionais()
                .WithTargetSuffix("regionais")
                .Configure();
            if ($("#regional_id").val() > 0) {
                Europa.Controllers.BreveLancamento.AutoCompleteRegional.SetValue($("#regional_id").val(), $("#regional_nome").val())
            }
            Europa.Controllers.BreveLancamento.AplicarMascaras();
            Europa.Components.DatePicker.AutoApply();
        } else {
            Europa.Informacao.PosAcao(res);
        }
    });
};



Europa.Controllers.BreveLancamento.VisualizarForm = function (idBreveLancamento) {
    $.get(Europa.Controllers.BreveLancamento.UrlBuscarBreveLancamento, { idBreveLancamento: idBreveLancamento }, function (res) {
        console.log(res);
        if (res.Sucesso) {
            $("#div_form_breve_lancamento").html(res.Objeto.htmlBreveLancamento);
            $("#div_form_endereco_breve_lancamento").html(res.Objeto.htmlEnderecoBreveLancamento);
            Europa.Controllers.BreveLancamento.AutoCompleteRegional = new Europa.Components.AutoCompleteRegionais()
                .WithTargetSuffix("regionais")
                .Configure();
            if ($("#regional_id").val() > 0) {
                Europa.Controllers.BreveLancamento.AutoCompleteRegional.SetValue($("#regional_id").val(), $("#regional_nome").val())
            }
            Europa.Controllers.BreveLancamento.AutoCompleteRegional.Disable();
            Europa.Controllers.BreveLancamento.AplicarMascaras();
            Europa.Components.DatePicker.AutoApply();
        } else {
            Europa.Informacao.PosAcao(res);
        }
    });
};