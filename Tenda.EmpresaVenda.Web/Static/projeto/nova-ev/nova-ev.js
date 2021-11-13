Europa.Controllers.NovaEv = {};
Europa.Controllers.NovaEv.Tabela = {};
Europa.Controllers.NovaEv.TabelaDocumentos = {};
Europa.Controllers.NovaEv.Permissoes = {};
Europa.Controllers.NovaEv.Modal = {};
Europa.Controllers.NovaEv.IdFormularioEmpresaVenda = "#form_empresa_venda";
Europa.Controllers.NovaEv.IdFormularioEnderecoEmpresaVenda = "#form_endereco_empresa_venda";
Europa.Controllers.NovaEv.IdFormularioDadosTributarios = "#form_dados_tributarios";
Europa.Controllers.NovaEv.IdFormularioRepresentanteLegal = "#form_dados_representante_legal";
Europa.Controllers.NovaEv.IdFormularioUploadFoto = "#form_upload_foto";
Europa.Controllers.NovaEv.IdFormularioResponsavelTecnico = "#form_resp_tecnico";
Europa.Controllers.NovaEv.IdFormularioEnderecoCorretor = "#form_endereco_corretor";
Europa.Controllers.NovaEv.IdEmpresaVenda = 0;


$(function () {

});


////////////////////////////////////////////////////////////////////////////////////
// Funções Datatable
////////////////////////////////////////////////////////////////////////////////////
DataTableApp.controller('listaEmpresas', listaEmpresas);

function listaEmpresas($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.NovaEv.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.NovaEv.Tabela;
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('NomeFantasia').withTitle(Europa.i18n.Messages.NomeFantasia).withOption('width', '15%'),
        DTColumnBuilder.newColumn('RazaoSocial').withTitle(Europa.i18n.Messages.RazaoSocial).withOption('width', '15%'),
        DTColumnBuilder.newColumn('CNPJ').withTitle(Europa.i18n.Messages.Cnpj).renderWith(formatCNPJ).withOption('width', '7%'),
        DTColumnBuilder.newColumn('NomeLoja').withTitle(Europa.i18n.Messages.CentralVendas).withOption('width', '10%'),
        DTColumnBuilder.newColumn('CreciJuridico').withTitle(Europa.i18n.Messages.CreciJuridico).withOption('width', '10%'),
        DTColumnBuilder.newColumn('NomeCorretor').withTitle(Europa.i18n.Messages.RepresentanteLegal).withOption('width', '13%'),
        DTColumnBuilder.newColumn('Telefone').withTitle(Europa.i18n.Messages.Telefone).renderWith(formatTelefone).withOption('width', '7%'),
        DTColumnBuilder.newColumn('Email').withTitle(Europa.i18n.Messages.Email).withOption('width', '10%'),
        DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('type', 'enum-format-Situacao').withOption('width', '7%')
    ])
        .setIdAreaHeader("datatable_header")
        .setColActions(actionsHtml, '10%')
        .setDefaultOptions('POST', Europa.Controllers.NovaEv.UrlListar, Europa.Controllers.NovaEv.filterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButtonEdit(Europa.Controllers.NovaEv.Permissoes.Atualizar, "Editar", "fa fa-edit", "editar(" + meta.row + ")") +            
            '</div>';
    }

    function formatCNPJ(data, type, full) {
        return Europa.Mask.GetMaskedValue(data, Europa.Mask.FORMAT_CNPJ);
    }

    function formatTelefone(data, type, full) {
        return Europa.String.FormatTelefone(data);
    }

    $scope.renderButtonEdit = function (hasPermission, title, icon, onClick) {
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    }
    $scope.editar = function (row) {
        var obj = Europa.Controllers.NovaEv.Tabela.getRowData(row);
        Europa.Controllers.NovaEv.PreencherFormularios(obj.Id, obj.IdCorretor);
    }
};

Europa.Controllers.NovaEv.filterParams = function () {
    var filtro = {
        empresaVenda: $('#filtro_empresa_venda').val(),
        cnpj: $('#filtro_cnpj').val(),
        creci: $('#filtro_creci').val(),
        situacoes: $('#filtro_situacoes').val()
    }
    return filtro;
};

Europa.Controllers.NovaEv.Filtrar = function () {
    Europa.Controllers.NovaEv.Tabela.reloadData();
};

Europa.Controllers.NovaEv.LimparFiltro = function () {
    $('#filtro_empresa_venda').val("");
    $('#filtro_cnpj').val("");
    $('#filtro_creci').val("");
    //$('#filtro_situacoes').val(1).trigger('change');
};

////////////////////////////////////////////////////////////////////////////////////
// Funções Edição
////////////////////////////////////////////////////////////////////////////////////
Europa.Controllers.NovaEv.HabilitarEdicao = function () {
    Europa.Controllers.NovaEv.HideElement("#div_visualizacao");
    Europa.Controllers.NovaEv.HideElement("#botoes_visualizacao");
    Europa.Controllers.NovaEv.ShowElement("#div_edicao");
    Europa.Controllers.NovaEv.ShowElement("#botoes_edicao");
    Europa.Controllers.NovaEv.LimparFormularios();
    Europa.Controllers.NovaEv.AplicarMascaras();
    Europa.Controllers.NovaEv.InitDatepicker();
    
};

Europa.Controllers.NovaEv.PreencherFormularios = function (idEmpresaVenda, idCorretor) {
    Europa.Controllers.NovaEv.IdEmpresaVenda = idEmpresaVenda;
    Europa.Controllers.NovaEv.FiltrarDocumentos();

    $.get(Europa.Controllers.NovaEv.UrlBuscarEmpresaVenda, { idEmpresaVenda: idEmpresaVenda, idCorretor: idCorretor }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.NovaEv.HabilitarEdicao();
            $("#div_form_empresa_venda").html(res.Objeto.htmlEmpresaVenda);
            $("#div_form_endereco").html(res.Objeto.htmlEnderecoEmpresaVenda);
            $("#div_form_dados_tributarios").html(res.Objeto.htmlDadosTributarios);
            $("#div_form_representante_legal").html(res.Objeto.htmlRepresentanteLegal);
            $("#div_form_responsavel_tecnico").html(res.Objeto.htmlResponsavelTecnico);
            Europa.Controllers.NovaEv.AplicarMascaras();
            Europa.Components.DatePicker.AutoApply();
        } else {
            Europa.Informacao.PosAcao(res);
            return;
        }
    });
};

Europa.Controllers.NovaEv.CancelarEdicao = function () {
    Europa.Controllers.NovaEv.ShowElement("#div_visualizacao");
    Europa.Controllers.NovaEv.ShowElement("#botoes_visualizacao");
    Europa.Controllers.NovaEv.HideElement("#div_edicao");
    Europa.Controllers.NovaEv.HideElement("#botoes_edicao");
    Europa.Controllers.NovaEv.LimparFormularios();
};

Europa.Controllers.NovaEv.LimparFormularios = function () {
    Europa.Controllers.NovaEv.AutoCompleteLojaDisponivel.Clean();
    $(Europa.Controllers.NovaEv.IdFormularioEmpresaVenda).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    $(Europa.Controllers.NovaEv.IdFormularioEnderecoEmpresaVenda).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    $(Europa.Controllers.NovaEv.IdFormularioDadosTributarios).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    $(Europa.Controllers.NovaEv.IdFormularioRepresentanteLegal).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    $("#LucroPresumido", Europa.Controllers.NovaEv.IdFormularioDadosTributarios).val("000");
    $("#LucroReal", Europa.Controllers.NovaEv.IdFormularioDadosTributarios).val("000");
    Europa.Validator.ClearForm(Europa.Controllers.NovaEv.IdFormularioEmpresaVenda);
    Europa.Validator.ClearForm(Europa.Controllers.NovaEv.IdFormularioEnderecoEmpresaVenda);
    Europa.Validator.ClearForm(Europa.Controllers.NovaEv.IdFormularioDadosTributarios);
    Europa.Validator.ClearForm(Europa.Controllers.NovaEv.IdFormularioRepresentanteLegal);
};

Europa.Controllers.NovaEv.IncorporarFormularios = function () {
    var formEmpresaVenda = Europa.Form.SerializeJson(Europa.Controllers.NovaEv.IdFormularioEmpresaVenda);
    formEmpresaVenda.Situacao = $("#Situacao", Europa.Controllers.NovaEv.IdFormularioEmpresaVenda).val();
    var formEnderecoEmpresaVenda = Europa.Form.SerializeJson(Europa.Controllers.NovaEv.IdFormularioEnderecoEmpresaVenda);
    var formDadosTributarios = Europa.Form.SerializeJson(Europa.Controllers.NovaEv.IdFormularioDadosTributarios);
    var formResponsavelTecnico = Europa.Form.SerializeJson(Europa.Controllers.NovaEv.IdFormularioResponsavelTecnico);
    var result = $.extend(formEmpresaVenda, formEnderecoEmpresaVenda, formDadosTributarios, formResponsavelTecnico);
    return result;
};

Europa.Controllers.NovaEv.IncorporarFormulariosCorretor = function () {
    var formCorretor = Europa.Form.SerializeJson(Europa.Controllers.NovaEv.IdFormularioRepresentanteLegal);
    var formEnderecoCorretor = Europa.Form.SerializeJson(Europa.Controllers.NovaEv.IdFormularioEnderecoCorretor);
    var result = $.extend(formCorretor, formEnderecoCorretor);
    return result;
};

Europa.Controllers.NovaEv.Salvar = function () {
    var objeto = Europa.Controllers.NovaEv.IncorporarFormularios();
    objeto.LucroPresumido = objeto.LucroPresumido.replace('.', '');
    objeto.LucroReal = objeto.LucroReal.replace('.', '');
    var formCorretor = Europa.Controllers.NovaEv.IncorporarFormulariosCorretor();
    var idsRegionais = $('#autocomplete_regional_empresa').val();

    var url = objeto.Id == 0 || objeto.Id == undefined ? Europa.Controllers.NovaEv.UrlIncluir : Europa.Controllers.NovaEv.UrlAtualizar;
    $.post(url, { empresaVenda: objeto, corretor: formCorretor, idsRegionais: idsRegionais}, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.NovaEv.CancelarEdicao();
            Europa.Controllers.NovaEv.Filtrar();
        } else {
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.NovaEv.IdFormularioEmpresaVenda);
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.NovaEv.IdFormularioEnderecoEmpresaVenda);
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.NovaEv.IdFormularioDadosTributarios);
            Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.NovaEv.IdFormularioRepresentanteLegal);
        }
    }).done(function (res) {
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.NovaEv.HideElement = function (idElemento) {
    $(idElemento).css("display", "none");
};

Europa.Controllers.NovaEv.ShowElement = function (idElemento) {
    $(idElemento).css("display", "");
};

Europa.Controllers.NovaEv.OnChangeCepEmpresaVenda = function (input) {
    Europa.Controllers.NovaEv.OnChangeCep(input, Europa.Controllers.NovaEv.IdFormularioEnderecoEmpresaVenda);
};

Europa.Controllers.NovaEv.OnChangeCepCorretor = function (input) {
    Europa.Controllers.NovaEv.OnChangeCep(input, Europa.Controllers.NovaEv.IdFormularioEnderecoCorretor);
};

Europa.Controllers.NovaEv.OnChangeCep = function (input, form) {
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

Europa.Controllers.NovaEv.AplicarMascaras = function () {
    console.log("aplica mask");
    Europa.Mask.Apply($("#CNPJ", Europa.Controllers.NovaEv.IdFormularioEmpresaVenda), Europa.Mask.FORMAT_CNPJ, true);
    Europa.Mask.Dinheiro($("#LucroPresumido", Europa.Controllers.NovaEv.IdFormularioDadosTributarios));
    Europa.Mask.Dinheiro($("#LucroReal", Europa.Controllers.NovaEv.IdFormularioDadosTributarios));
    Europa.Mask.Apply($("#CNPJ", Europa.Controllers.NovaEv.IdFormularioRepresentanteLegal), Europa.Mask.FORMAT_CNPJ, true);
    Europa.Mask.Apply($("#CPF", Europa.Controllers.NovaEv.IdFormularioRepresentanteLegal), Europa.Mask.FORMAT_CPF, true);
    Europa.Mask.Telefone("#Telefone");
    $("#Cep", Europa.Controllers.NovaEv.IdFormularioEnderecoEmpresaVenda).mask("00000-000");
    $("#Cep", Europa.Controllers.NovaEv.IdFormularioEnderecoCorretor).mask("00000-000");
}

Europa.Controllers.NovaEv.InitDatepicker = function (value) {
    Europa.Controllers.NovaEv.DataNascimento = new Europa.Components.DatePicker()
        .WithTarget("#DataNascimento", Europa.Controllers.NovaEv.IdFormularioRepresentanteLegal)
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now())
        .WithValue(value)
        .Configure();
};

Europa.Controllers.NovaEv.AbrirModal = function () {
    $("[name='Motivo']").parent().removeClass("has-error");
    $("#Motivo").val("");
    $("#modal_reprovar").modal("show");
};

Europa.Controllers.NovaEv.ReprovarEv = function () {
    var idEmpresaVenda = $("#Id").val();
    var motivo = $("#Motivo").val();
    $.post(Europa.Controllers.NovaEv.UrlReprovarEv, { idEmpresaVenda: idEmpresaVenda,motivo: motivo }, function (res) {
        if (res.Sucesso) {
            $("#modal_reprovar").modal("hide");
            Europa.Controllers.NovaEv.CancelarEdicao();
            Europa.Controllers.NovaEv.Tabela.reloadData();
        } else {
            Europa.Controllers.NovaEv.AdicionarErro(res.Campos);
        }
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.NovaEv.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};
