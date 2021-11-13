Europa.Controllers.Corretor = {};
Europa.Controllers.Corretor.Tabela = {};
Europa.Controllers.Corretor.Permissoes = {};
Europa.Controllers.Corretor.IdFormularioCorretor = "#form_corretor";
Europa.Controllers.Corretor.FormCorretor = undefined;
Europa.Controllers.Corretor.ModoEdicao = false;


$(function () {
    $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
    $(".ng-scope").css("word-wrap", "break-word");

    $("#filtro_situacoes").select2({
        trags: true
    });
    $('#filtro_situacoes').val(1).trigger('change');
    Europa.Controllers.Corretor.FiltrarTabela();
});

////////////////////////////////////////////////////////////////////////////////////
// Funções Edição
////////////////////////////////////////////////////////////////////////////////////
Europa.Controllers.Corretor.HabilitarEdicao = function (isCreate) {
    Europa.Controllers.Corretor.HideElement("#div_visualizacao");
    Europa.Controllers.Corretor.HideElement("#header_listagem");
    Europa.Controllers.Corretor.HideElement("#header_detalhamento");
    Europa.Controllers.Corretor.ShowElement("#div_edicao");
    Europa.Controllers.Corretor.ShowElement("#header_edicao");
    Europa.Controllers.Corretor.ShowElement("#password_hint");
    Europa.Controllers.Corretor.HideElement("#cadastro_subtitle");
    Europa.Controllers.Corretor.HideElement("#detail_buttons");
    Europa.Controllers.Corretor.HideElement("#title-detalhe");
    Europa.Controllers.Corretor.HideElement("#title-listagem");
    Europa.Controllers.Corretor.AplicarMascaras();
    $('#Situacao').val(1).trigger('change');
    Europa.Controllers.Corretor.InitDatepicker();
    $('#fieldset_corretor').removeAttr('disabled');
    Europa.Controllers.Corretor.HideElement("#detail_buttons");

    if(isCreate){
        Europa.Controllers.Corretor.ShowElement("#title-create");
        Europa.Controllers.Corretor.HideElement("#title-edit");
        Europa.Controllers.Corretor.HideElement("#botao_atualizar");
        Europa.Controllers.Corretor.ShowElement("#botao_cadastrar");
    }else{
        Europa.Controllers.Corretor.HideElement("#title-create");
        Europa.Controllers.Corretor.ShowElement("#title-edit");
        Europa.Controllers.Corretor.ShowElement("#botao_atualizar");
        Europa.Controllers.Corretor.HideElement("#botao_cadastrar");
    }

    Europa.Controllers.Corretor.AutoCompletePerfilPortal.Enable();
    Europa.Controllers.Corretor.AutoCompletePerfilPortal.SetMultipleValues([], "Id", "Nome");

};

Europa.Controllers.Corretor.HabilitarDetalhamento = function () {
    Europa.Controllers.Corretor.HideElement("#header_edicao");
    Europa.Controllers.Corretor.HideElement("#div_visualizacao");
    Europa.Controllers.Corretor.HideElement("#header_listagem");
    Europa.Controllers.Corretor.ShowElement("#div_edicao");
    Europa.Controllers.Corretor.ShowElement("#header_detalhamento");
    Europa.Controllers.Corretor.HideElement("#cadastro_subtitle");
    Europa.Controllers.Corretor.HideElement("#password_hint");
    Europa.Controllers.Corretor.ShowElement("#detail_buttons");
    Europa.Controllers.Corretor.HideElement("#title-edit");
    Europa.Controllers.Corretor.ShowElement("#title-detalhe");
    Europa.Controllers.Corretor.HideElement("#title-listagem");
    Europa.Controllers.Corretor.AplicarMascaras();
    $('#Situacao').val(1).trigger('change');
    Europa.Controllers.Corretor.InitDatepicker();
    $('#fieldset_corretor').attr('disabled', 'disabled');
};

Europa.Controllers.Corretor.Incluir = function () {
    Europa.Controllers.Corretor.ModoEdicao = true;
    Europa.Controllers.Corretor.HabilitarEdicao(true);
    Europa.Controllers.Corretor.ShowElement("#cadastro_subtitle");  
    $(Europa.Controllers.Corretor.IdFormularioCorretor).find(":input#Usuario_Id").val(0);

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
    Europa.Controllers.Corretor.ShowElement("#header_listagem");
    Europa.Controllers.Corretor.HideElement("#header_detalhamento");
    Europa.Controllers.Corretor.HideElement("#div_edicao");
    Europa.Controllers.Corretor.HideElement("#header_edicao");
    Europa.Controllers.Corretor.HideElement("#cadastro_subtitle");
    Europa.Controllers.Corretor.HideElement("#password_hint");
    Europa.Controllers.Corretor.HideElement("#detail_buttons");
    Europa.Controllers.Corretor.HideElement("#title-detalhe");
    Europa.Controllers.Corretor.ShowElement("#title-listagem");
    Europa.Controllers.Corretor.HideElement("#title-edit");
    Europa.Controllers.Corretor.HideElement("#title-create");
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
    var urlSalvar = Europa.Controllers.Corretor.ModoEdicao && formCorretor.Id > 0 ? Europa.Controllers.Corretor.UrlAlterar : Europa.Controllers.Corretor.UrlIncluir;
    if(formCorretor.Id > 0){
        Europa.Confirmacao.PreAcaoV2("Deseja alterar os dados desse usuário?"
            ,""
            , "Alterar"
            , function () {
                Europa.Controllers.Corretor.RequisitarSalvar(urlSalvar, formCorretor);
            });
    } else {

        var corretor = {
            NovoCorretor: formCorretor,
            Perfis: Europa.Controllers.Corretor.AutoCompletePerfilPortal.Value(),
            IdSistema: Europa.Controllers.Corretor.EmpresaVendaPortal.Id
        };

        Europa.Controllers.Corretor.RequisitarSalvar(urlSalvar, corretor);
    }
};

Europa.Controllers.Corretor.RequisitarSalvar = function(url, form){
    $.post(url, { corretor: form }, function (res) {
        $('[name="Perfil"]').removeClass('has-error');
        if (res.Sucesso) {
            Europa.Controllers.Corretor.CancelarEdicao();
            Europa.Controllers.Corretor.FiltrarTabela();
        } else {
            if (res.Campos.includes(Europa.i18n.Messages.Perfil)) {
                $('[name="' + Europa.i18n.Messages.Perfil +'"]').addClass('has-error');
            }
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.Corretor.IdFormularioCorretor);
        }

        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.Corretor.Excluir = function(id) {
    Europa.Confirmacao.PreAcaoV2("Tem certeza que deseja excluir esse corretor?"
        , "Essa ação não pode ser desfeita."
        , "Excluir"
        , function () {
            if(id === null || id === undefined) {
                id = $(Europa.Controllers.Corretor.IdFormularioCorretor).find("#Id").val();
            }
            $.post(Europa.Controllers.Corretor.UrlExcluir, { idCorretor: id }, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.Corretor.Tabela.reloadData();
                    Europa.Informacao.PosAcao(res);
                } else {
                    Europa.Informacao.PosAcao(res);
                }
            });
        });
}


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
    if(idCorretor === undefined || idCorretor === null){
        idCorretor = $(Europa.Controllers.Corretor.IdFormularioCorretor).find("#Id").val();
    }
    Europa.Controllers.Corretor.HabilitarEdicao(false);
    Europa.Controllers.Corretor.PreencherForm(idCorretor);
    Europa.Controllers.Corretor.ModoEdicao = true;
    Europa.Controllers.Corretor.PreencherPerfil(idCorretor);
};


Europa.Controllers.Corretor.Detalhar = function (idCorretor) {
    Europa.Controllers.Corretor.HabilitarDetalhamento();
    Europa.Controllers.Corretor.PreencherForm(idCorretor);
    Europa.Controllers.Corretor.PreencherPerfil(idCorretor);
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