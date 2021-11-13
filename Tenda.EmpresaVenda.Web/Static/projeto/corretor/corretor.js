Europa.Controllers.Corretor = {};
Europa.Controllers.Corretor.Tabela = {};
Europa.Controllers.Corretor.Permissoes = {};
Europa.Controllers.Corretor.IdFormularioCorretor = "#form_corretor";
Europa.Controllers.Corretor.FormCorretor = undefined;
Europa.Controllers.Corretor.ModoEdicao = false;


$(function () {
    $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
    $(".ng-scope").css("word-wrap", "break-word");


    Europa.Controllers.Corretor.AutoCompleteRegionais = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regional")
        .Configure();

    $("#filtro_situacoes").select2({
        trags: true
    });
    $('#filtro_situacoes').val(1).trigger('change');

    $("#filtro_funcao").select2({
        trags: true
    });
    $("#filtro_estados").select2({
        trags: true
    });
    Europa.Controllers.Corretor.FiltrarTabela();
});

////////////////////////////////////////////////////////////////////////////////////
// Funções Edição
////////////////////////////////////////////////////////////////////////////////////
Europa.Controllers.Corretor.HabilitarEdicao = function () {
    Europa.Controllers.Corretor.HideElement("#div_visualizacao");
    Europa.Controllers.Corretor.HideElement("#botoes_visualizacao");
    Europa.Controllers.Corretor.HideElement("#botoes_detalhamento");
    Europa.Controllers.Corretor.ShowElement("#div_edicao");
    Europa.Controllers.Corretor.ShowElement("#botoes_edicao");
    Europa.Controllers.Corretor.AplicarMascaras();
    $('#Situacao').val(1).trigger('change');
    Europa.Controllers.Corretor.InitDatepicker();
    $('#fieldset_corretor').removeAttr('disabled');

    Europa.Controllers.Corretor.AutoCompletePerfilPortal.Enable();
    Europa.Controllers.Corretor.AutoCompletePerfilPortal.SetMultipleValues([], "Id", "Nome");
};

Europa.Controllers.Corretor.HabilitarDetalhamento = function () {
    Europa.Controllers.Corretor.HideElement("#botoes_edicao");
    Europa.Controllers.Corretor.HideElement("#div_visualizacao");
    Europa.Controllers.Corretor.HideElement("#botoes_visualizacao");
    Europa.Controllers.Corretor.ShowElement("#div_edicao");
    Europa.Controllers.Corretor.ShowElement("#botoes_detalhamento");
    Europa.Controllers.Corretor.AplicarMascaras();
    $('#Situacao').val(1).trigger('change');
    Europa.Controllers.Corretor.InitDatepicker();
    $('#fieldset_corretor').attr('disabled', 'disabled');
};

Europa.Controllers.Corretor.Incluir = function () {
    Europa.Controllers.Corretor.ModoEdicao = true;
    Europa.Controllers.Corretor.HabilitarEdicao();

    setTimeout(
        function () {
            Europa.Components.DatePicker.AutoApply();
            $("#DataNascimento").val(null);
            $("#DataCredenciamento").val(null);
        }, 300);

    setTimeout(
        function () {
            Europa.Controllers.Corretor.InitDatepicker();
        }, 300);
};

Europa.Controllers.Corretor.CancelarEdicao = function () {
    Europa.Controllers.Corretor.LimparFormularios();
    Europa.Controllers.Corretor.ShowElement("#div_visualizacao");
    Europa.Controllers.Corretor.ShowElement("#botoes_visualizacao");
    Europa.Controllers.Corretor.HideElement("#botoes_detalhamento");
    Europa.Controllers.Corretor.HideElement("#div_edicao");
    Europa.Controllers.Corretor.HideElement("#botoes_edicao");
    Europa.Controllers.Corretor.ModoEdicao = false;
    Europa.Controllers.Corretor.FiltrarTabela();
    Europa.Controllers.Corretor.AutoCompletePerfilPortal.SetMultipleValues([], "Id", "Nome");
};

Europa.Controllers.Corretor.LimparFormularios = function () {
    $(Europa.Controllers.Corretor.IdFormularioCorretor).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('').trigger('reset');

    Europa.Validator.ClearForm(Europa.Controllers.Corretor.IdFormularioCorretor);
};

Europa.Controllers.Corretor.IncorporarFormularios = function () {
    var formCorretor = Europa.Form.SerializeJson(Europa.Controllers.Corretor.IdFormularioCorretor);
    formCorretor.Situacao = $("#Situacao", Europa.Controllers.Corretor.IdFormularioCorretor).val();
    var result = $.extend(formCorretor, formDadosTributarios);
    return result;
};

Europa.Controllers.Corretor.Salvar = function () {
    var formCorretor = Europa.Form.SerializeJson(Europa.Controllers.Corretor.IdFormularioCorretor);

    var urlSalvar = formCorretor.Id > 0 ? Europa.Controllers.Corretor.UrlAlterar : Europa.Controllers.Corretor.UrlIncluir;

    $.post(urlSalvar, { corretor: formCorretor }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.Corretor.CancelarEdicao();
            Europa.Controllers.Corretor.FiltrarTabela();
        } else {
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.Corretor.IdFormularioCorretor);
        }

        Europa.Informacao.PosAcao(res);
    });
};


Europa.Controllers.Corretor.HideElement = function (idElemento) {
    $(idElemento).css("display", "none");
};

Europa.Controllers.Corretor.ShowElement = function (idElemento) {
    $(idElemento).css("display", "");
};


Europa.Controllers.Corretor.AplicarMascaras = function () {
    Europa.Mask.Apply($("#CNPJ", Europa.Controllers.Corretor.IdFormularioCorretor), Europa.Mask.FORMAT_CNPJ, true);
    Europa.Mask.Apply($("#CPF", Europa.Controllers.Corretor.IdFormularioCorretor), Europa.Mask.FORMAT_CPF, true);
    Europa.Mask.Telefone("#Telefone");
    $("#Cep", Europa.Controllers.Corretor.IdFormularioEnderecoRepresentanteLegal).mask("00000-000");
};

Europa.Controllers.Corretor.InitDatepicker = function (dataNascimento, dataCredenciamento) {
    Europa.Controllers.Corretor.DataNascimento = new Europa.Components.DatePicker()
        .WithTarget("#DataNascimento", Europa.Controllers.Corretor.IdFormularioCorretor)
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .WithValue(dataNascimento)
        .Configure();

    Europa.Controllers.Corretor.DataCredenciamento = new Europa.Components.DatePicker()
        .WithTarget("#DataCredenciamento", Europa.Controllers.Corretor.IdFormularioCorretor)
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .WithValue(dataCredenciamento)
        .Configure();

};


Europa.Controllers.Corretor.Editar = function (idCorretor) {
    Europa.Controllers.Corretor.HabilitarEdicao();
    Europa.Controllers.Corretor.PreencherForm(idCorretor);
    Europa.Controllers.Corretor.ModoEdicao = true;
};


Europa.Controllers.Corretor.Detalhar = function (idCorretor) {
    Europa.Controllers.Corretor.HabilitarDetalhamento();
    Europa.Controllers.Corretor.PreencherForm(idCorretor);
};

Europa.Controllers.Corretor.PreencherForm = function (idCorretor) {
    $.get(Europa.Controllers.Corretor.UrlBuscarCorretor, { idCorretor: idCorretor }, function (res) {
        if (res.Sucesso) {
            $("#div_form_corretor").html(res.Objeto.htmlCorretor);

            Europa.Controllers.Corretor.AplicarMascaras();

            var dtNascimento = $("#DataNascimento").val();
            var dtCredenciamento = $("#DataCredenciamento").val();
            setTimeout(function () {
                Europa.Controllers.Corretor.InitDatepicker(dtNascimento, dtCredenciamento);
            }, 200);

            Europa.Components.DatePicker.AutoApply();
        } else {
            Europa.Informacao.PosAcao(res);
        }
    });
};