Europa.Controllers.Empreendimento = {};
Europa.Controllers.Empreendimento.Tabela = {};
Europa.Controllers.Empreendimento.Permissoes = {};
Europa.Controllers.Empreendimento.IdFormularioEmpreendimento = "#form_empreendimento";
Europa.Controllers.Empreendimento.IdFormularioEnderecoEmpreendimento = "#form_endereco_empreendimento";
Europa.Controllers.Empreendimento.FormEmpreendimento = undefined;
Europa.Controllers.Empreendimento.ModoEdicao = false;


$(function () {

    Europa.Controllers.Empreendimento.AutoCompleteRegionais = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regional")
        .Configure();

    $("#filtro_estados").select2({
        trags: true
    });
    Europa.Controllers.Empreendimento.FiltrarTabela();
    Europa.Components.DatePicker.AutoApply();
});

////////////////////////////////////////////////////////////////////////////////////
// Funções Edição
////////////////////////////////////////////////////////////////////////////////////
Europa.Controllers.Empreendimento.HabilitarEdicao = function () {
    Europa.Controllers.Empreendimento.HideElement("#div_visualizacao");
    Europa.Controllers.Empreendimento.HideElement("#botoes_visualizacao");
    Europa.Controllers.Empreendimento.HideElement("#botoes_detalhamento");
    Europa.Controllers.Empreendimento.ShowElement("#div_edicao");
    Europa.Controllers.Empreendimento.ShowElement("#botoes_edicao");
    Europa.Controllers.Empreendimento.AplicarMascaras();
    $('#Situacao').val(1).trigger('change');
    $('#fieldset_empreendimento').removeAttr('disabled');
};

Europa.Controllers.Empreendimento.HabilitarDetalhamento = function () {
    Europa.Controllers.Empreendimento.HideElement("#botoes_edicao");
    Europa.Controllers.Empreendimento.HideElement("#div_visualizacao");
    Europa.Controllers.Empreendimento.HideElement("#botoes_visualizacao");
    Europa.Controllers.Empreendimento.ShowElement("#div_edicao");
    Europa.Controllers.Empreendimento.ShowElement("#botoes_detalhamento");
    Europa.Controllers.Empreendimento.AplicarMascaras();
    $('#Situacao').val(1).trigger('change');
    $('#fieldset_empreendimento').attr('disabled', 'disabled');
};

Europa.Controllers.Empreendimento.Incluir = function () {
    Europa.Controllers.Empreendimento.HabilitarEdicao();
    Europa.Controllers.Empreendimento.PreencherForm(0);
};

Europa.Controllers.Empreendimento.CancelarEdicao = function () {
    Europa.Controllers.Empreendimento.LimparFormularios();
    Europa.Controllers.Empreendimento.ShowElement("#div_visualizacao");
    Europa.Controllers.Empreendimento.ShowElement("#botoes_visualizacao");
    Europa.Controllers.Empreendimento.HideElement("#botoes_detalhamento");
    Europa.Controllers.Empreendimento.HideElement("#div_edicao");
    Europa.Controllers.Empreendimento.HideElement("#botoes_edicao");
    Europa.Controllers.Empreendimento.ModoEdicao = false;
};

Europa.Controllers.Empreendimento.LimparFormularios = function () {
    $(Europa.Controllers.Empreendimento.IdFormularioEmpreendimento).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('').trigger('reset');
    $(Europa.Controllers.Empreendimento.IdFormularioEnderecoEmpreendimento).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('').trigger('reset');

    Europa.Validator.ClearForm(Europa.Controllers.Empreendimento.IdFormularioEmpreendimento);
    Europa.Validator.ClearForm(Europa.Controllers.Empreendimento.IdFormularioEnderecoEmpreendimento);
};

Europa.Controllers.Empreendimento.IncorporarFormularios = function () {
    var formEmpreendimento = Europa.Form.SerializeJson(Europa.Controllers.Empreendimento.IdFormularioEmpreendimento);
    formEmpreendimento.Situacao = $("#Situacao", Europa.Controllers.Empreendimento.IdFormularioEmpreendimento).val();
    var formEnderecoEmpreendimento = Europa.Form.SerializeJson(Europa.Controllers.Empreendimento.IdFormularioEnderecoEmpreendimento);
    var result = $.extend(formEmpreendimento, formEnderecoEmpreendimento, formDadosTributarios);
    return result;
};

Europa.Controllers.Empreendimento.Salvar = function () {
    var formEmpreendimento = Europa.Form.SerializeJson(Europa.Controllers.Empreendimento.IdFormularioEmpreendimento);
    var formEnderecoEmpreendimento = Europa.Form.SerializeJson(Europa.Controllers.Empreendimento.IdFormularioEnderecoEmpreendimento);
    var urlSalvar = Europa.Controllers.Empreendimento.ModoEdicao ? Europa.Controllers.Empreendimento.UrlAlterar : Europa.Controllers.Empreendimento.UrlIncluir;

    formEmpreendimento.DisponivelCatalogo = formEmpreendimento.DisponivelCatalogo.toLowerCase() === "true";
    formEmpreendimento.Divisao = $("#Divisao").val();
    formEmpreendimento.IdSuat = $("#IdSuat").val();
    formEmpreendimento.DisponivelParaVenda = $("#DisponivelParaVenda").val() === Europa.i18n.Messages.Sim;
    formEmpreendimento.CodigoEmpresa = $("#CodigoEmpresa").val();
    formEmpreendimento.NomeEmpresa = $("#NomeEmpresa").val();
    formEmpreendimento.Regional = $("#Regional").val();
    formEmpreendimento.Divisao = $("#Divisao").val();
    formEmpreendimento.RegistroIncorporacao = $("#RegistroIncorporacao").val();
    formEmpreendimento.Mancha = $("#Mancha").val();
    formEmpreendimento.DataLancamento = $("#DataLancamento").val();
    formEmpreendimento.PrevisaoEntrega = $("#PrevisaoEntrega").val();
    formEmpreendimento.DataEntrega = $("#DataEntrega").val();
    formEmpreendimento.PriorizarRegraComissao = $('#PriorizarRegraComissao').val();
    formEmpreendimento.CNPJ = $('#CNPJ').val();
    formEmpreendimento.Nome = $('#Nome').val();

    formEnderecoEmpreendimento.Cep = $('#Cep').val();
    formEnderecoEmpreendimento.Logradouro = $('#Logradouro').val();
    formEnderecoEmpreendimento.Numero = $('#Numero').val();
    formEnderecoEmpreendimento.Complemento = $('#Complemento').val();
    formEnderecoEmpreendimento.Bairro = $('#Bairro').val();
    formEnderecoEmpreendimento.Cidade = $('#Cidade').val();
    formEnderecoEmpreendimento.Estado = $('#Estado').val();
    
    $.post(urlSalvar, { enderecoEmpreendimento: formEnderecoEmpreendimento, empreendimento: formEmpreendimento }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.Empreendimento.CancelarEdicao();
            Europa.Controllers.Empreendimento.FiltrarTabela();
        } else {
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.Empreendimento.IdFormularioEmpreendimento);
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.Empreendimento.IdFormularioEnderecoEmpreendimento);
        }

        Europa.Informacao.PosAcao(res);
    });
};


Europa.Controllers.Empreendimento.HideElement = function (idElemento) {
    $(idElemento).css("display", "none");
};

Europa.Controllers.Empreendimento.ShowElement = function (idElemento) {
    $(idElemento).css("display", "");
};

Europa.Controllers.Empreendimento.OnChangeCepEmpreendimento = function (input) {
    Europa.Controllers.Empreendimento.OnChangeCep(input, Europa.Controllers.Empreendimento.IdFormularioEnderecoEmpreendimento);
};

Europa.Controllers.Empreendimento.OnChangeCepRepresentanteLegal = function (input) {
    Europa.Controllers.Empreendimento.OnChangeCep(input, Europa.Controllers.Empreendimento.IdFormularioEnderecoEmpreendimento);
};

Europa.Controllers.Empreendimento.OnChangeCep = function (input, form) {
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

Europa.Controllers.Empreendimento.AplicarMascaras = function () {
    $("#Cep", Europa.Controllers.Empreendimento.IdFormularioEnderecoEmpreendimento).mask("00000-000");
    $("#Cep", Europa.Controllers.Empreendimento.IdFormularioEnderecoRepresentanteLegal).mask("00000-000");
};

Europa.Controllers.Empreendimento.Editar = function (idEmpreendimento) {
    Europa.Controllers.Empreendimento.HabilitarEdicao();
    Europa.Controllers.Empreendimento.PreencherForm(idEmpreendimento);
    Europa.Controllers.Empreendimento.ModoEdicao = true;
};


Europa.Controllers.Empreendimento.Detalhar = function (idEmpreendimento) {
    Europa.Controllers.Empreendimento.HabilitarDetalhamento();
    Europa.Controllers.Empreendimento.PreencherForm(idEmpreendimento);
};

Europa.Controllers.Empreendimento.PreencherForm = function (idEmpreendimento) {
    $.get(Europa.Controllers.Empreendimento.UrlBuscarEmpreendimento, { idEmpreendimento: idEmpreendimento }, function (res) {
        if (res.Sucesso) {
            $("#div_form_empreendimento").html(res.Objeto.htmlEmpreendimento);
            $("#div_form_endereco_empreendimento").html(res.Objeto.htmlEnderecoEmpreendimento);

            Europa.Controllers.Empreendimento.AplicarMascaras();

            Europa.Components.DatePicker.AutoApply();
        } else {
            Europa.Informacao.PosAcao(res);
        }
    });
};