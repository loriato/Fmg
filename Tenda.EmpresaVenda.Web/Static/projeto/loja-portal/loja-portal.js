Europa.Controllers.LojaPortal = {};
Europa.Controllers.LojaPortal.Modal = {};
Europa.Controllers.LojaPortal.Tabela = {};
Europa.Controllers.LojaPortal.Permissoes = {};
Europa.Controllers.LojaPortal.IdFormularioEmpresaVenda = "#form_loja_portal";
Europa.Controllers.LojaPortal.IdFormularioEnderecoEmpresaVenda = "#form_endereco_loja_portal";
Europa.Controllers.LojaPortal.IdLojaPortal = undefined;
Europa.Controllers.LojaPortal.NomeLojaPortal = undefined;


$(function () {

});

////////////////////////////////////////////////////////////////////////////////////
// Funções Datatable
////////////////////////////////////////////////////////////////////////////////////
DataTableApp.controller('listaLojasPortal', listaLojasPortal);

function listaLojasPortal($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.LojaPortal.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.LojaPortal.Tabela;
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '20%'),
        DTColumnBuilder.newColumn('NomeComercial').withTitle(Europa.i18n.Messages.NomeComercial).withOption('width', '20%'),
        DTColumnBuilder.newColumn('PessoaContato').withTitle(Europa.i18n.Messages.PessoaContato).withOption('width', '20%'),
        DTColumnBuilder.newColumn('TelefoneContato').withTitle(Europa.i18n.Messages.Telefone).renderWith(formatTelefone).withOption('width', '20%'),
        DTColumnBuilder.newColumn('NomeCentralVenda').withTitle(Europa.i18n.Messages.CentralVendas).withOption('width', '20%'),
        DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('type', 'enum-format-Situacao').withOption('width', '7%')
    ])
        .setIdAreaHeader("datatable_header")
        .setColActions(actionsHtml, '10%')
        .setOptionsMultiSelect('POST', Europa.Controllers.LojaPortal.UrlListar, Europa.Controllers.LojaPortal.filterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButtonEdit(Europa.Controllers.LojaPortal.Permissoes.Atualizar, "Editar", "fa fa-edit", "editar(" + meta.row + ")", full.Situacao) +
            //$scope.renderButtonEdit(Europa.Controllers.LojaPortal.Permissoes.UploadFoto, Europa.i18n.Messages.UploadFoto, "fa fa-upload", "uploadFoto(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonEdit('true', Europa.i18n.Messages.MeuCadastro, "fa fa-building", "meuCadastro(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonEdit(Europa.Controllers.LojaPortal.Permissoes.Excluir, Europa.i18n.Messages.Excluir, "fa fa-trash", "excluir(" + meta.row + ")", full.Situacao) +
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

    function registrosSelecionados() {
        var itens = Europa.Controllers.LojaPortal.Tabela.getRowsSelect();
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
        var obj = Europa.Controllers.LojaPortal.Tabela.getRowData(row);
        Europa.Controllers.LojaPortal.PreencherFormularios(obj.Id);
    }

    $scope.excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.LojaPortal.Tabela.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.Nome, function () {
            $.post(Europa.Controllers.LojaPortal.UrlExcluir, { id: objetoLinhaTabela.Id }, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.LojaPortal.Tabela.reloadData();
                }
                Europa.Informacao.PosAcao(res);
                
            });
        });
    };

    $scope.uploadFoto = function (row) {
        var obj = Europa.Controllers.LojaPortal.Tabela.getRowData(row);

        Europa.Controllers.LojaPortal.Modal.AbrirModal(obj.Id);
    }

    $scope.meuCadastro = function (row) {
        var obj = Europa.Controllers.LojaPortal.Tabela.getRowData(row);
        window.location.href = Europa.Controllers.LojaPortal.UrlMeuCadastro + "/" + obj.Id;
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
                    url: Europa.Controllers.LojaPortal.UrlReativar,
                    dataType: 'json',
                    data: { idsEmpresas: idRegs },
                    type: 'POST'
                }).done(function (data) {
                    if (data.Sucesso) {
                        Europa.Controllers.LojaPortal.Filtrar();
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
                    url: Europa.Controllers.LojaPortal.UrlSuspender,
                    dataType: 'json',
                    data: { idsEmpresas: idRegs },
                    type: 'POST'
                }).done(function (data) {
                    if (data.Sucesso) {
                        Europa.Controllers.LojaPortal.Filtrar();
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
                    url: Europa.Controllers.LojaPortal.UrlCancelar,
                    dataType: 'json',
                    data: { idsEmpresas: idRegs },
                    type: 'POST'
                }).done(function (data) {
                    if (data.Sucesso) {
                        Europa.Controllers.LojaPortal.Filtrar();
                    }
                    Europa.Informacao.PosAcao(data);
                });
            });
        }
    }
};

Europa.Controllers.LojaPortal.filterParams = function () {
    var filtro = {
        Nome: $('#filtro_nome').val(),
        IdRegional: $('#autocomplete_regional').val(),
        Estado: $('#filtro_estados').val()
    }
    return filtro;
};

Europa.Controllers.LojaPortal.Filtrar = function () {
    Europa.Controllers.LojaPortal.Tabela.reloadData();
};

Europa.Controllers.LojaPortal.LimparFiltro = function () {
    $('#filtro_nome').val("");
    $('#autocomplete_regional').val(0);
    $('#filtro_estados').val("");
};

////////////////////////////////////////////////////////////////////////////////////
// Funções Edição
////////////////////////////////////////////////////////////////////////////////////
Europa.Controllers.LojaPortal.HabilitarEdicao = function () {
    Europa.Controllers.LojaPortal.HideElement("#div_visualizacao");
    Europa.Controllers.LojaPortal.HideElement("#botoes_visualizacao");
    Europa.Controllers.LojaPortal.ShowElement("#div_edicao");
    Europa.Controllers.LojaPortal.ShowElement("#botoes_edicao");
    Europa.Controllers.LojaPortal.LimparFormularios();
    Europa.Controllers.LojaPortal.AplicarMascaras();
    $("#Situacao", Europa.Controllers.LojaPortal.IdFormularioEmpresaVenda).val(1);

    Europa.Controllers.LojaPortal.IdLojaPortal = undefined;
};

Europa.Controllers.LojaPortal.PreencherFormularios = function (idLoja) {
    Europa.Controllers.LojaPortal.IdLojaPortal = idLoja;

    $.get(Europa.Controllers.LojaPortal.UrlBuscarLojaPortal, { id: idLoja }, function (res) {
        Europa.Controllers.LojaPortal.HabilitarEdicao();
        $("#div_form_loja_portal").html(res.htmlEmpresaVenda);
        $("#div_form_endereco").html(res.htmlEnderecoEmpresaVenda);
        Europa.Controllers.LojaPortal.AplicarMascaras();
        Europa.Controllers.LojaPortal.IdLojaPortal = idLoja;
    });
};

Europa.Controllers.LojaPortal.CancelarEdicao = function () {
    Europa.Controllers.LojaPortal.ShowElement("#div_visualizacao");
    Europa.Controllers.LojaPortal.ShowElement("#botoes_visualizacao");
    Europa.Controllers.LojaPortal.HideElement("#div_edicao");
    Europa.Controllers.LojaPortal.HideElement("#botoes_edicao");
    Europa.Controllers.LojaPortal.LimparFormularios();
};

Europa.Controllers.LojaPortal.LimparFormularios = function () {
    $(Europa.Controllers.LojaPortal.IdFormularioEmpresaVenda).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    Europa.Controllers.LojaPortal.AutoCompleteRegionaisLoja.ClearOptions();
    $(Europa.Controllers.LojaPortal.IdFormularioEnderecoEmpresaVenda).find(':input').not(':button, :submit, :reset, :checkbox, :radio').val('');
    Europa.Validator.ClearForm(Europa.Controllers.LojaPortal.IdFormularioEmpresaVenda);
    Europa.Validator.ClearForm(Europa.Controllers.LojaPortal.IdFormularioEnderecoEmpresaVenda);
};

Europa.Controllers.LojaPortal.Salvar = function () {
    let obj1 = Europa.Form.SerializeJson(Europa.Controllers.LojaPortal.IdFormularioEmpresaVenda);
    obj1.Situacao = $("#Situacao", Europa.Controllers.LojaPortal.IdFormularioEmpresaVenda).val();
    let obj2 = Europa.Form.SerializeJson(Europa.Controllers.LojaPortal.IdFormularioEnderecoEmpresaVenda);

    let loja = { ...obj1, ...obj2 };
    loja["idsRegionais"] = $('#autocomplete_regional_loja').val();

    var url = loja.Id == 0 || loja.Id == undefined ? Europa.Controllers.LojaPortal.UrlIncluir : Europa.Controllers.LojaPortal.UrlAtualizar;

    $.post( url, loja, function (res) {
        if (res.Sucesso) {

            Europa.Controllers.LojaPortal.IdLojaPortal = res.Objeto.IdLojaPortal;
            Europa.Controllers.LojaPortal.NomeLojaPortal = res.Objeto.NomeLojaPortal;

            Europa.Controllers.LojaPortal.CancelarEdicao();
            Europa.Controllers.LojaPortal.Filtrar();
        }
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.LojaPortal.HideElement = function (idElemento) {
    $(idElemento).css("display", "none");
};

Europa.Controllers.LojaPortal.ShowElement = function (idElemento) {
    $(idElemento).css("display", "");
};

Europa.Controllers.LojaPortal.OnChangeCepEmpresaVenda = function (input) {
    Europa.Controllers.LojaPortal.OnChangeCep(input, Europa.Controllers.LojaPortal.IdFormularioEnderecoEmpresaVenda);
};


Europa.Controllers.LojaPortal.OnChangeCep = function (input, form) {
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

Europa.Controllers.LojaPortal.AplicarMascaras = function () {
    Europa.Mask.Telefone("#TelefoneContato");
    $("#Cep", Europa.Controllers.LojaPortal.IdFormularioEnderecoEmpresaVenda).mask("00000-000");
}

////////////////////////////////////////////////////////////////////////////////////
// Funções Modal
////////////////////////////////////////////////////////////////////////////////////

Europa.Controllers.LojaPortal.Modal.AbrirModal = function (idEmpresa) {
    $("#Id", Europa.Controllers.LojaPortal.IdFormularioUploadFoto).val(idEmpresa);
    $("#modal_upload_foto").modal("show");
};

Europa.Controllers.LojaPortal.Modal.CloseModal = function () {
    $("#modal_upload_foto").modal("hide");
};

Europa.Controllers.LojaPortal.Modal.UploadFoto = function () {
    var arquivo = $("#Arquivo", Europa.Controllers.LojaPortal.IdFormularioUploadFoto).get(0).files[0];
    var idEmpresaVenda = $("#Id", Europa.Controllers.LojaPortal.IdFormularioUploadFoto).val();
    if (arquivo !== undefined && arquivo.size > 4000000) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.ArquivoTamanhoMaximoExcedido, "4"));
        Europa.Informacao.Show();
        return;
    }
    var formData = new FormData();
    formData.append("IdLojaPortal", idEmpresaVenda);
    formData.append("Foto", arquivo);

    $.ajax({
        type: 'POST',
        url: Europa.Controllers.LojaPortal.UrlUploadFoto,
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
        success: function (res) {
            if (res.Sucesso) {
                Europa.Controllers.LojaPortal.Modal.CloseModal();
            }
            Europa.Informacao.PosAcao(res);
        }
    });
};
Europa.Controllers.LojaPortal.ExportarTodos = function () {
    console.log("exportarPaTod");
    var params = Europa.Controllers.LojaPortal.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.LojaPortal.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.LojaPortal.ExportarPagina = function () {
    console.log("exportarPa");
    var params = Europa.Controllers.LojaPortal.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.LojaPortal.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};