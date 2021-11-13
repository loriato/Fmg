Europa.Controllers.EmpresaVenda = {};
Europa.Controllers.EmpresaVenda.Modal = {};
Europa.Controllers.EmpresaVenda.Tabela = {};
Europa.Controllers.EmpresaVenda.Permissoes = {};
Europa.Controllers.EmpresaVenda.IdFormularioEmpresaVenda = "#form_empresa_venda";
Europa.Controllers.EmpresaVenda.IdFormularioEnderecoEmpresaVenda = "#form_endereco_empresa_venda";
Europa.Controllers.EmpresaVenda.IdFormularioDadosTributarios = "#form_dados_tributarios";
Europa.Controllers.EmpresaVenda.IdFormularioRepresentanteLegal = "#form_dados_representante_legal";
Europa.Controllers.EmpresaVenda.IdFormularioUploadFoto = "#form_upload_foto"; 
Europa.Controllers.EmpresaVenda.IdFormularioResponsavelTecnico = "#form_resp_tecnico";
Europa.Controllers.EmpresaVenda.IdFormularioEnderecoCorretor = "#form_endereco_corretor";
Europa.Controllers.EmpresaVenda.FormEmpresaVenda = undefined;
Europa.Controllers.EmpresaVenda.FormRepresentanteLegal = undefined;
Europa.Controllers.EmpresaVenda.FormEnderecoRepresentanteLegal = undefined;
Europa.Controllers.EmpresaVenda.IdEmpresaVenda = undefined;
Europa.Controllers.EmpresaVenda.NomeEmpresaVenda = undefined;


$(function () {
    $("#filtro_situacoes").select2({
        trags: true
    });
    $("#filtro_estados").select2({
        trags: true
    });
    $('#filtro_situacoes').val(1).trigger('change');
    Europa.Controllers.EmpresaVenda.Filtrar();
    Europa.Controllers.EmpresaVenda.AutoCompleteRegionais = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regionais")
        .Configure();
    
});

////////////////////////////////////////////////////////////////////////////////////
// Funções Datatable
////////////////////////////////////////////////////////////////////////////////////
DataTableApp.controller('listaEmpresas', listaEmpresas);

function listaEmpresas($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.EmpresaVenda.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.EmpresaVenda.Tabela;
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
    .setAutoInit()
    .setColActions(actionsHtml, '10%')
    .setOptionsMultiSelect('POST', Europa.Controllers.EmpresaVenda.UrlListar, Europa.Controllers.EmpresaVenda.filterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButtonEdit(Europa.Controllers.EmpresaVenda.Permissoes.Atualizar, "Editar", "fa fa-edit", "editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonEdit(Europa.Controllers.EmpresaVenda.Permissoes.UploadFoto, Europa.i18n.Messages.UploadFoto, "fa fa-upload", "uploadFoto(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonEdit('true', Europa.i18n.Messages.MeuCadastro, "fa fa-building", "meuCadastro(" + meta.row + ")", full.Situacao) +
            '</div>';
    }

    function formatCNPJ(data, type, full) {
        return Europa.Mask.GetMaskedValue(data, Europa.Mask.FORMAT_CNPJ);
    }

    function formatTelefone(data, type, full) {
        return Europa.String.FormatTelefone(data);
    }

    $scope.renderButtonEdit = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 3) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    }

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (hasPermission === 'true') {
            icon = $('<i/>').addClass(icon);
            var button = $('<a />')
                .addClass('btn btn-default')
                .attr('title', title)
                .attr('ng-click', onClick)
                .append(icon);
            return button.prop('outerHTML');
        } else {
            return null;
        }
    }

    function registrosSelecionados() {
        var itens = Europa.Controllers.EmpresaVenda.Tabela.getRowsSelect();
        var registros = [];

        itens.forEach(function (item) {
            registros.push(item);
        });

        if (registros.length < 1) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.i18n.Messages.MsgNenhumRegistroSelecionado);
            Europa.Informacao.Show();
            return null;
        }
        return registros;
    }

    $scope.editar = function (row) {
        var obj = Europa.Controllers.EmpresaVenda.Tabela.getRowData(row);
        if (obj.CorretorVisualizarClientes) {
            $("#CorretorVisualizarClientes").val("True");
        }
        Europa.Controllers.EmpresaVenda.PreencherFormularios(obj.Id, obj.IdCorretor);
    }

    $scope.uploadFoto = function (row) {
        var obj = Europa.Controllers.EmpresaVenda.Tabela.getRowData(row);

        Europa.Controllers.EmpresaVenda.Modal.AbrirModal(obj.Id);
    }

    $scope.meuCadastro = function (row) {
        var obj = Europa.Controllers.EmpresaVenda.Tabela.getRowData(row);
        window.location.href = Europa.Controllers.EmpresaVenda.UrlMeuCadastro + "/" + obj.Id;
    }

    $scope.reativarItens = function () {
        var registros = registrosSelecionados();
        if (registros !== null) {
            Europa.Confirmacao.PreAcaoMulti(Europa.i18n.Messages.Reativar, function () {
                var idRegs = [];
                registros.forEach(function (item) {
                    idRegs.push(item.Id);
                });
                $.ajax({
                    url: Europa.Controllers.EmpresaVenda.UrlReativar,
                    dataType: 'json',
                    data: { idsEmpresas: idRegs },
                    type: 'POST'
                }).done(function (data) {
                    if (data.Sucesso) {
                        Europa.Controllers.EmpresaVenda.Filtrar();
                    }
                    Europa.Informacao.PosAcao(data);
                });
            });
        }
    }

    $scope.suspenderItens = function () {
        var registros = registrosSelecionados();
        if (registros !== null) {
            Europa.Confirmacao.PreAcaoMulti(Europa.i18n.Messages.Suspender, function () {
                var idRegs = [];
                registros.forEach(function (item) {
                    idRegs.push(item.Id);
                });
                $.ajax({
                    url: Europa.Controllers.EmpresaVenda.UrlSuspender,
                    dataType: 'json',
                    data: { idsEmpresas: idRegs },
                    type: 'POST'
                }).done(function (data) {
                    if (data.Sucesso) {
                        Europa.Controllers.EmpresaVenda.Filtrar();
                    }
                    Europa.Informacao.PosAcao(data);
                });
            });
        }
    }

    $scope.cancelarItens = function () {
        var registros = registrosSelecionados();
        if (registros !== null) {
            Europa.Confirmacao.PreAcaoMulti(Europa.i18n.Messages.Cancelar, function () {
                var idRegs = [];
                registros.forEach(function (item) {
                    idRegs.push(item.Id);
                });
                $.ajax({
                    url: Europa.Controllers.EmpresaVenda.UrlCancelar,
                    dataType: 'json',
                    data: { idsEmpresas: idRegs },
                    type: 'POST'
                }).done(function (data) {
                    if (data.Sucesso) {
                        Europa.Controllers.EmpresaVenda.Filtrar();
                    }
                    Europa.Informacao.PosAcao(data);
                });
            });
        }
    }
};

Europa.Controllers.EmpresaVenda.filterParams = function () {

    var filtro = {
        empresaVenda: $('#filtro_empresa_venda').val(),
        cnpj: $('#filtro_cnpj').val(),
        creci: $('#filtro_creci').val(),
        situacoes: $('#filtro_situacoes').val(),
        regionais: $('#autocomplete_regionais').val(),
        estados: $('#filtro_estados').val(),
    }
    return filtro;
};

Europa.Controllers.EmpresaVenda.Filtrar = function () {
    Europa.Controllers.EmpresaVenda.Tabela.reloadData();
};

Europa.Controllers.EmpresaVenda.LimparFiltro = function () {
    $('#filtro_empresa_venda').val("");
    $('#filtro_cnpj').val("");
    $('#filtro_creci').val("");
    $('#filtro_estados').val("").trigger('change');
    $('#filtro_situacoes').val(1).trigger('change');
    $('#autocomplete_regionais').val("").trigger('change');
};

////////////////////////////////////////////////////////////////////////////////////
// Funções Edição
////////////////////////////////////////////////////////////////////////////////////
Europa.Controllers.EmpresaVenda.HabilitarEdicao = function () {
    Europa.Controllers.EmpresaVenda.HideElement("#div_visualizacao");
    Europa.Controllers.EmpresaVenda.HideElement("#botoes_visualizacao");
    Europa.Controllers.EmpresaVenda.HideElement("#painel_responsavel_aceite_regra_comissao");
    Europa.Controllers.EmpresaVenda.ShowElement("#div_edicao");
    Europa.Controllers.EmpresaVenda.ShowElement("#botoes_edicao");
    Europa.Controllers.EmpresaVenda.LimparFormularios();
    Europa.Controllers.EmpresaVenda.AplicarMascaras();
    Europa.Controllers.EmpresaVenda.InitDatepicker();
    $("#Situacao", Europa.Controllers.EmpresaVenda.IdFormularioEmpresaVenda).val(1);

    Europa.Controllers.EmpresaVenda.Documentos = [];
    Europa.Controllers.EmpresaVenda.ListaDatatable = [];


    Europa.Controllers.EmpresaVenda.TabelaDocumentosPreCarregados.reloadData();
    Europa.Controllers.EmpresaVenda.IdEmpresaVenda = undefined;

    Europa.Controllers.EmpresaVenda.FiltrarDocumentos();

    Europa.Controllers.EmpresaVenda.LiberarBotoes();
};

Europa.Controllers.EmpresaVenda.PreencherFormularios = function (idEmpresaVenda, idCorretor) {
    Europa.Controllers.EmpresaVenda.IdEmpresaVenda = idEmpresaVenda;
    Europa.Controllers.EmpresaVenda.FiltrarDocumentos();

    $.get(Europa.Controllers.EmpresaVenda.UrlBuscarEmpresaVenda, { idEmpresaVenda: idEmpresaVenda, idCorretor: idCorretor }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.EmpresaVenda.HabilitarEdicao();
            $("#div_form_empresa_venda").html(res.Objeto.htmlEmpresaVenda);            
            $("#div_form_endereco").html(res.Objeto.htmlEnderecoEmpresaVenda);
            $("#div_form_dados_tributarios").html(res.Objeto.htmlDadosTributarios);
            $("#div_form_representante_legal").html(res.Objeto.htmlRepresentanteLegal);
            $("#div_form_responsavel_tecnico").html(res.Objeto.htmlResponsavelTecnico);
            Europa.Controllers.EmpresaVenda.AplicarMascaras();
            Europa.Components.DatePicker.AutoApply();
            Europa.Controllers.EmpresaVenda.IdEmpresaVenda = idEmpresaVenda;
            //verifica se pode cadastrar responsavel pelo aceite regra comissao
            if (res.Objeto.podeCadastrarResponsavel) {
                Europa.Controllers.EmpresaVenda.ShowElement("#painel_responsavel_aceite_regra_comissao");
            }
            else {
                Europa.Controllers.EmpresaVenda.HideElement("#painel_responsavel_aceite_regra_comissao");
            }

            Europa.Controllers.EmpresaVenda.LiberarBotoes();
            Europa.Controllers.EmpresaVenda.TabelaResponsavelAceiteRegraComissao.reloadData();
        } else {
            Europa.Informacao.PosAcao(res);
        }
    });
};

Europa.Controllers.EmpresaVenda.CancelarEdicao = function () {
    Europa.Controllers.EmpresaVenda.ShowElement("#div_visualizacao");
    Europa.Controllers.EmpresaVenda.ShowElement("#botoes_visualizacao");
    Europa.Controllers.EmpresaVenda.HideElement("#div_edicao");
    Europa.Controllers.EmpresaVenda.HideElement("#botoes_edicao");
    Europa.Controllers.EmpresaVenda.LimparFormularios();
};

Europa.Controllers.EmpresaVenda.LimparFormularios = function () {
    Europa.Controllers.EmpresaVenda.AutoCompleteLojaDisponivel.Clean();
    $(Europa.Controllers.EmpresaVenda.IdFormularioEmpresaVenda).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    $(Europa.Controllers.EmpresaVenda.IdFormularioEnderecoEmpresaVenda).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    $(Europa.Controllers.EmpresaVenda.IdFormularioDadosTributarios).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    $(Europa.Controllers.EmpresaVenda.IdFormularioRepresentanteLegal).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    $(Europa.Controllers.EmpresaVenda.IdFormularioEnderecoCorretor).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    $(Europa.Controllers.EmpresaVenda.IdFormularioResponsavelTecnico).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    $("#LucroPresumido", Europa.Controllers.EmpresaVenda.IdFormularioDadosTributarios).val("000");
    $("#LucroReal", Europa.Controllers.EmpresaVenda.IdFormularioDadosTributarios).val("000");
    Europa.Validator.ClearForm(Europa.Controllers.EmpresaVenda.IdFormularioEmpresaVenda);
    Europa.Validator.ClearForm(Europa.Controllers.EmpresaVenda.IdFormularioEnderecoEmpresaVenda);
    Europa.Validator.ClearForm(Europa.Controllers.EmpresaVenda.IdFormularioDadosTributarios);
    Europa.Validator.ClearForm(Europa.Controllers.EmpresaVenda.IdFormularioRepresentanteLegal);
    Europa.Validator.ClearForm(Europa.Controllers.EmpresaVenda.IdFormularioEnderecoCorretor);
    Europa.Validator.ClearForm(Europa.Controllers.EmpresaVenda.IdFormularioResponsavelTecnico);
};

Europa.Controllers.EmpresaVenda.IncorporarFormularios = function () {
    var formEmpresaVenda = Europa.Form.SerializeJson(Europa.Controllers.EmpresaVenda.IdFormularioEmpresaVenda);
    formEmpresaVenda.CorretorVisualizarClientes = $("#CorretorVisualizarClientes").val();
    formEmpresaVenda.Situacao = $("#Situacao", Europa.Controllers.EmpresaVenda.IdFormularioEmpresaVenda).val();
    
    var formEnderecoEmpresaVenda = Europa.Form.SerializeJson(Europa.Controllers.EmpresaVenda.IdFormularioEnderecoEmpresaVenda);
    var formDadosTributarios = Europa.Form.SerializeJson(Europa.Controllers.EmpresaVenda.IdFormularioDadosTributarios);
    var formResponsavelTecnico = Europa.Form.SerializeJson(Europa.Controllers.EmpresaVenda.IdFormularioResponsavelTecnico);
    var result = $.extend(formEmpresaVenda, formEnderecoEmpresaVenda, formDadosTributarios, formResponsavelTecnico);
    return result;
};

Europa.Controllers.EmpresaVenda.IncorporarFormulariosCorretor = function () {
    var formCorretor = Europa.Form.SerializeJson(Europa.Controllers.EmpresaVenda.IdFormularioRepresentanteLegal);
    var formEnderecoCorretor = Europa.Form.SerializeJson(Europa.Controllers.EmpresaVenda.IdFormularioEnderecoCorretor);
    var result = $.extend(formCorretor, formEnderecoCorretor);
    return result;
};

Europa.Controllers.EmpresaVenda.Salvar = function () {
    var objeto = Europa.Controllers.EmpresaVenda.IncorporarFormularios();
    objeto.LucroPresumido = objeto.LucroPresumido.replace('.', '');
    objeto.LucroReal = objeto.LucroReal.replace('.', '');
    var formCorretor = Europa.Controllers.EmpresaVenda.IncorporarFormulariosCorretor();

    var url = objeto.Id == 0 || objeto.Id == undefined ? Europa.Controllers.EmpresaVenda.UrlIncluir : Europa.Controllers.EmpresaVenda.UrlAtualizar;

    var data = {};
    data.EmpresaVenda = objeto;
    data.Corretor = formCorretor;
    data.Documentos = Europa.Controllers.EmpresaVenda.Documentos;
    data.idsRegionais = $("#autocomplete_regional_empresa").val();

    var formData = new FormData();

    Europa.Controllers.EmpresaVenda.ObjectToFormData(formData, data);

    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'json',
        contentType: false,
        data: formData,
        cache: false,
        processData: false,
        success: function (res) {
            if (res.Sucesso) {
                //res.Mensagens.forEach(console.log)

                Europa.Controllers.EmpresaVenda.IdEmpresaVenda = res.Objeto.IdEmpresaVenda;
                Europa.Controllers.EmpresaVenda.NomeEmpresaVenda = res.Objeto.NomeEmpresaVenda;

                Europa.Controllers.EmpresaVenda.CancelarEdicao();
                Europa.Controllers.EmpresaVenda.Filtrar();
            } else {
                Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.EmpresaVenda.IdFormularioEmpresaVenda);
                Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.EmpresaVenda.IdFormularioEnderecoEmpresaVenda);
                Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.EmpresaVenda.IdFormularioDadosTributarios);
                Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.EmpresaVenda.IdFormularioRepresentanteLegal);
            }
            Europa.Informacao.PosAcao(res);
        }
    });
};

Europa.Controllers.EmpresaVenda.HideElement = function (idElemento) {
    $(idElemento).css("display", "none");
};

Europa.Controllers.EmpresaVenda.ShowElement = function (idElemento) {
    $(idElemento).css("display", "");
};

Europa.Controllers.EmpresaVenda.OnChangeCepEmpresaVenda = function (input) {
    Europa.Controllers.EmpresaVenda.OnChangeCep(input, Europa.Controllers.EmpresaVenda.IdFormularioEnderecoEmpresaVenda);
};

Europa.Controllers.EmpresaVenda.OnChangeCepCorretor = function (input) {
    Europa.Controllers.EmpresaVenda.OnChangeCep(input, Europa.Controllers.EmpresaVenda.IdFormularioEnderecoCorretor);
};

Europa.Controllers.EmpresaVenda.OnChangeCep = function (input, form) {
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

Europa.Controllers.EmpresaVenda.AplicarMascaras = function () {
    Europa.Mask.Apply($("#CNPJ", Europa.Controllers.EmpresaVenda.IdFormularioEmpresaVenda), Europa.Mask.FORMAT_CNPJ, true);
    Europa.Mask.Dinheiro($("#LucroPresumido", Europa.Controllers.EmpresaVenda.IdFormularioDadosTributarios));
    Europa.Mask.Dinheiro($("#LucroReal", Europa.Controllers.EmpresaVenda.IdFormularioDadosTributarios));
    Europa.Mask.Apply($("#CNPJ", Europa.Controllers.EmpresaVenda.IdFormularioRepresentanteLegal), Europa.Mask.FORMAT_CNPJ, true);
    Europa.Mask.Apply($("#CPF", Europa.Controllers.EmpresaVenda.IdFormularioRepresentanteLegal), Europa.Mask.FORMAT_CPF, true);
    Europa.Mask.Telefone("#Telefone");
    $("#Cep", Europa.Controllers.EmpresaVenda.IdFormularioEnderecoEmpresaVenda).mask("00000-000");
    $("#Cep", Europa.Controllers.EmpresaVenda.IdFormularioEnderecoCorretor).mask("00000-000");
}

Europa.Controllers.EmpresaVenda.InitDatepicker = function (value) {
    Europa.Controllers.EmpresaVenda.DataNascimento = new Europa.Components.DatePicker()
        .WithTarget("#DataNascimento", Europa.Controllers.EmpresaVenda.IdFormularioRepresentanteLegal)
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now())
        .WithValue(value)
        .Configure();
};

////////////////////////////////////////////////////////////////////////////////////
// Funções Modal
////////////////////////////////////////////////////////////////////////////////////

Europa.Controllers.EmpresaVenda.Modal.AbrirModal = function (idEmpresa) {
    $("#Id", Europa.Controllers.EmpresaVenda.IdFormularioUploadFoto).val(idEmpresa);
    $("#modal_upload_foto").modal("show");
};

Europa.Controllers.EmpresaVenda.Modal.CloseModal = function () {
    $("#modal_upload_foto").modal("hide");
};

Europa.Controllers.EmpresaVenda.Modal.UploadFoto = function () {
    var arquivo = $("#Arquivo", Europa.Controllers.EmpresaVenda.IdFormularioUploadFoto).get(0).files[0];
    var idEmpresaVenda = $("#Id", Europa.Controllers.EmpresaVenda.IdFormularioUploadFoto).val();
    if (arquivo !== undefined && arquivo.size > 4000000) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.ArquivoTamanhoMaximoExcedido, "4"));
        Europa.Informacao.Show();
        return;
    }
    var formData = new FormData();
    formData.append("IdEmpresaVenda", idEmpresaVenda);
    formData.append("Foto", arquivo);

    $.ajax({
        type: 'POST',
        url: Europa.Controllers.EmpresaVenda.UrlUploadFoto,
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
        success: function (res) {
            if (res.Sucesso) {
                Europa.Controllers.EmpresaVenda.Modal.CloseModal();
            }
            Europa.Informacao.PosAcao(res);
        }
    });
};
Europa.Controllers.EmpresaVenda.ExportarTodos = function () {
    console.log("exportarPaTod");
    var params = Europa.Controllers.EmpresaVenda.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.EmpresaVenda.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.EmpresaVenda.ExportarPagina = function () {
    console.log("exportarPa");
    var params = Europa.Controllers.EmpresaVenda.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.EmpresaVenda.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};