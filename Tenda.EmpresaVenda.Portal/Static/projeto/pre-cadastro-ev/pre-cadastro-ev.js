Europa.Controllers.PreCadastroEv = {};
Europa.Controllers.PreCadastroEv.IdFormularioDadosCadastrais = "#form_dados_cadastrais";
Europa.Controllers.PreCadastroEv.IdFormularioRepresentanteLegal = "#form_representante_legal";
Europa.Controllers.PreCadastroEv.IdFormularioResponsavelTecnico = "#form_resp_tecnico";
Europa.Controllers.PreCadastroEv.IdFormularioDadosTributarios = "#form_dados_tributarios";
Europa.Controllers.PreCadastroEv.IdFormularioEnderecoCorretor = "#form_endereco_corretor";
Europa.Controllers.PreCadastroEv.IdEmpresaVenda = undefined;
Europa.Controllers.PreCadastroEv.NomeEmpresaVenda = undefined;


$(function () {
    Europa.NavbarScrollControl("tabMenu", "block_content_page");
    $("#tabMenu li").click(function (e) {
        e.preventDefault();
        Europa.OnTabChange(this, "tabMenu", "block_content_page");
    });

    Europa.Controllers.PreCadastroEv.AplicarMascaras();
    Europa.Controllers.PreCadastroEv.InitDatepicker();
});



Europa.Controllers.PreCadastroEv.AplicarMascaras = function () {
    Europa.Mask.Apply($("#EmpresaVenda_CNPJ", Europa.Controllers.PreCadastroEv.IdFormularioDadosCadastrais), Europa.Mask.FORMAT_CNPJ, true);
    Europa.Mask.Dinheiro($("#EmpresaVenda_LucroPresumido", Europa.Controllers.PreCadastroEv.IdFormularioDadosTributarios));
    Europa.Mask.Dinheiro($("#EmpresaVenda_LucroReal", Europa.Controllers.PreCadastroEv.IdFormularioDadosTributarios));
    Europa.Mask.Apply($("#Corretor_CPF", Europa.Controllers.PreCadastroEv.IdFormularioRepresentanteLegal), Europa.Mask.FORMAT_CPF, true);
    Europa.Mask.Telefone("#Corretor_Telefone");
    $('#Corretor_Email').mask("A", {
        translation: {
            "A": { pattern: /[\w@\-.+]/, recursive: true }
        }
    });
    $("#EmpresaVenda_Cep", Europa.Controllers.PreCadastroEv.IdFormularioDadosCadastrais).mask("00000-000");
    $("#Corretor_Cep", Europa.Controllers.PreCadastroEv.IdFormularioEnderecoCorretor).mask("00000-000");
}
Europa.Controllers.PreCadastroEv.InitDatepicker = function (value) {
    Europa.Controllers.PreCadastroEv.DataNascimento = new Europa.Components.DatePicker()
        .WithTarget("#Corretor_DataNascimento", Europa.Controllers.PreCadastroEv.IdFormularioRepresentanteLegal)
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now())
        .WithValue(value)
        .Configure();
};

Europa.Controllers.PreCadastroEv.LimparFormularios = function () {
    $(Europa.Controllers.PreCadastroEv.IdFormularioDadosCadastrais).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    $(Europa.Controllers.PreCadastroEv.IdFormularioDadosTributarios).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    $(Europa.Controllers.PreCadastroEv.IdFormularioRepresentanteLegal).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    $(Europa.Controllers.PreCadastroEv.IdFormularioEnderecoCorretor).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    $(Europa.Controllers.PreCadastroEv.IdFormularioResponsavelTecnico).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    $("#EmpresaVenda_LucroPresumido", Europa.Controllers.PreCadastroEv.IdFormularioDadosTributarios).val("0,00");
    $("#EmpresaVenda_LucroReal", Europa.Controllers.PreCadastroEv.IdFormularioDadosTributarios).val("0,00");
    Europa.Validator.ClearForm(Europa.Controllers.PreCadastroEv.IdFormularioDadosCadastrais);
    Europa.Validator.ClearForm(Europa.Controllers.PreCadastroEv.IdFormularioResponsavelTecnico);
    Europa.Validator.ClearForm(Europa.Controllers.PreCadastroEv.IdFormularioDadosTributarios);
    Europa.Validator.ClearForm(Europa.Controllers.PreCadastroEv.IdFormularioRepresentanteLegal);
    Europa.Validator.ClearForm(Europa.Controllers.PreCadastroEv.IdFormularioEnderecoCorretor);

    Europa.Controllers.PreCadastroEv.LimparDocumentos();
    
};
Europa.Controllers.PreCadastroEv.OnChangeCepEmpresaVenda = function (input) {
    Europa.Controllers.PreCadastroEv.OnChangeCep(input, Europa.Controllers.PreCadastroEv.IdFormularioDadosCadastrais);
};

Europa.Controllers.PreCadastroEv.OnChangeCepCorretor = function (input) {
    Europa.Controllers.PreCadastroEv.OnChangeCepCorretor(input, Europa.Controllers.PreCadastroEv.IdFormularioEnderecoCorretor);
};

Europa.Controllers.PreCadastroEv.OnChangeCep = function (input, form) {
    var cep = $(input).val().replace(/\D/g, '');
    if (cep == "") {
        return;
    }
    var validacep = /^[0-9]{8}$/;
    if (!validacep.test(cep)) {
        return;
    }
    Europa.Controllers.PreCadastroEv.Search(cep, function (dados) {
        $("#EmpresaVenda_Logradouro", form).val(dados.logradouroAbrev);
        $("#EmpresaVenda_Bairro", form).val(dados.bairro);
        $("#EmpresaVenda_Cidade", form).val(dados.localidade);
        $("#EmpresaVenda_Estado", form).val(dados.uf);
    });
};

Europa.Controllers.PreCadastroEv.OnChangeCepCorretor = function (input, form) {
    var cep = $(input).val().replace(/\D/g, '');
    if (cep == "") {
        return;
    }
    var validacep = /^[0-9]{8}$/;
    if (!validacep.test(cep)) {
        return;
    }
    Europa.Controllers.PreCadastroEv.Search(cep, function (dados) {
        $("#Corretor_Logradouro", form).val(dados.logradouroAbrev);
        $("#Corretor_Bairro", form).val(dados.bairro);
        $("#Corretor_Cidade", form).val(dados.localidade);
        $("#Corretor_Estado", form).val(dados.uf);
    });
};
Europa.Controllers.PreCadastroEv.Search = function (cep, callback) {
    $.post(Europa.Controllers.PreCadastroEv.UrlFor,{ cep: cep }, function (dados) {
        if (dados.Sucesso != undefined && dados.Sucesso == false) {
            Europa.Informacao.ChangeHeaderAndContent('Atenção', dados.Objeto);
            Europa.Informacao.Show();
        } else {
            callback(dados);
        }
    });
}

Europa.Controllers.PreCadastroEv.IncorporarFormularios = function () {
    var formEmpresaVenda = Europa.Form.SerializeJson(Europa.Controllers.PreCadastroEv.IdFormularioDadosCadastrais);
    var formDadosTributarios = Europa.Form.SerializeJson(Europa.Controllers.PreCadastroEv.IdFormularioDadosTributarios);
    var formResponsavelTecnico = Europa.Form.SerializeJson(Europa.Controllers.PreCadastroEv.IdFormularioResponsavelTecnico);
    var formCorretor = Europa.Form.SerializeJson(Europa.Controllers.PreCadastroEv.IdFormularioRepresentanteLegal);
    var formEnderecoCorretor = Europa.Form.SerializeJson(Europa.Controllers.PreCadastroEv.IdFormularioEnderecoCorretor);
    var result = $.extend(formEmpresaVenda, formDadosTributarios, formResponsavelTecnico, formCorretor, formEnderecoCorretor);
    return result;
};

Europa.Controllers.PreCadastroEv.Salvar = function () {
    
    var objeto = Europa.Controllers.PreCadastroEv.IncorporarFormularios();

    objeto['EmpresaVenda.LucroReal'] = $("#EmpresaVenda_LucroReal").val().replace('.', '');
    objeto['EmpresaVenda.LucroPresumido'] = $("#EmpresaVenda_LucroPresumido").val().replace('.', '');
    
    var url = Europa.Controllers.PreCadastroEv.UrlIncluir;

    $.post(url, { preCadastroDTO: objeto }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.PreCadastroEv.IdEmpresaVenda = res.Objeto.IdEmpresaVenda;
            Europa.Controllers.PreCadastroEv.NomeEmpresaVenda = res.Objeto.NomeEmpresaVenda;

            Europa.Controllers.PreCadastroEv.UploadDocumentosEmpresaVenda();            

            Europa.Controllers.PreCadastroEv.LimparFormularios();
        } else {
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.PreCadastroEv.IdFormularioDadosCadastrais);
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.PreCadastroEv.IdFormularioDadosTributarios);
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.PreCadastroEv.IdFormularioRepresentanteLegal);
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.PreCadastroEv.IdFormularioEnderecoCorretor);
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.PreCadastroEv.IdFormularioResponsavelTecnico);
        }
        Europa.Informacao.PosAcao(res);
    })
    //).done(function (res) {
    //    Europa.Informacao.PosAcao(res);
    //});
};