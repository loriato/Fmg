////////////////////////////////////////////////////////////////////////////////////
// Funções Datatable
////////////////////////////////////////////////////////////////////////////////////
DataTableApp.controller('pontoVendaTable', pontoVendaTable);

function pontoVendaTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.PontoVenda.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.PontoVenda.Tabela;
    tabelaWrapper
        .setIdAreaHeader("datatable_header")
        .setTemplateEdit([
            '<input type="text" class="form-control" id="Nome" name="Nome" value="" maxlength="128">',
            '<div name="EmpresaVenda"><select id="autocomplete_empresa_venda" class="form-control"></select></div>',
            '<select class="form-control" id="IniciativaTenda" name="IniciativaTenda">' +
            '<option value="true">' + Europa.i18n.Messages.Sim + '</option>' +
            '<option value="false">' + Europa.i18n.Messages.Nao + '</option>' +
            '</select>',
            '<select class="form-control" id="Situacao" name="Situacao">' +
            '<option value="1">Ativo</option>' +
            '<option value="2">Suspenso</option>' +
            '<option value="3">Cancelado</option>' +
            '</select>',
            '<div name="Viabilizador"><select id="autocomplete_viabilizador" class="form-control"></select></div>'
        ])
        .setColumns([
            //DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '15%'),
            DTColumnBuilder.newColumn('NomePontoVenda').withTitle(Europa.i18n.Messages.Nome).withOption('width', '15%'),
            //DTColumnBuilder.newColumn('EmpresaVenda.NomeFantasia').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption('width', '15%'),
            DTColumnBuilder.newColumn('NomeFantasia').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption('width', '15%'),
            DTColumnBuilder.newColumn('IniciativaTenda').withTitle(Europa.i18n.Messages.GerenciadoTenda).withOption('width', '10%').renderWith(formatBoolean),
            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('width', '10%').withOption('type', 'enum-format-Situacao'),
            //DTColumnBuilder.newColumn('Viabilizador.Nome').withTitle(Europa.i18n.Messages.Viabilizador).withOption('width', '15%')
            DTColumnBuilder.newColumn('NomeViabilizador').withTitle(Europa.i18n.Messages.Viabilizador).withOption('width', '15%')
        ])
        .setColActions(actionsHtml, '10%')
        .setActionSave(Europa.Controllers.PontoVenda.PreSalvar)
        .setAutoInit()
        .setOptionsMultiSelect('POST', Europa.Controllers.PontoVenda.UrlListar, Europa.Controllers.PontoVenda.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButton(Europa.Controllers.PontoVenda.Permissoes.Atualizar, "Editar", "fa fa-edit", "editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButton(Europa.Controllers.PontoVenda.Permissoes.Excluir, "Excluir", "fa fa-trash", "excluir(" + meta.row + ")", full.Situacao) +
            '</div>';
    }

    function formatBoolean(data) {
        if (data === true) {
            return Europa.i18n.Messages.Sim;
        }
        return Europa.i18n.Messages.Nao;
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
    };

    $scope.editar = function (row) {
        $scope.rowEdit(row);
        var obj = Europa.Controllers.PontoVenda.Tabela.getRowData(row);
        Europa.Controllers.PontoVenda.Inclur = false;
        Europa.Controllers.PontoVenda.InitAutoCompleteEmpresaVenda();
        Europa.Controllers.PontoVenda.InitAutoCompleteViabilizador();
        $("#IniciativaTenda").val(obj.IniciativaTenda.toString());
        Europa.Controllers.PontoVenda.AutoCompleteEmpresaVendaInclusao.SetValue(obj.IdEmpresaVenda, obj.NomeFantasia);
        if(obj.Viabilizador !== null){
            Europa.Controllers.PontoVenda.AutoCompleteViabilizadorInclusao.SetValue(obj.IdViabilizador, obj.NomeViabilizador);
        }
    };

    $scope.excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.PontoVenda.Tabela.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.NomePontoVenda, function () {
            $.post(Europa.Controllers.PontoVenda.UrlExcluir, { id: objetoLinhaTabela.Id }, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.PontoVenda.Tabela.reloadData();
                    Europa.Informacao.PosAcao(res);
                } else {
                    Europa.Informacao.PosAcao(res);
                }
            });
        });
    };

}

Europa.Controllers.PontoVenda.FilterParams = function () {
    var filtro = {
        Nome: $("#filtro_nome").val(),
        Situacao: $("#filtro_situacoes").val(),
        IniciativaTenda: $("#filtro_iniciativa").val(),
        idEmpresaVenda: $("#autocomplete_filtro_empresa_venda").val(),
        IdViabilizador: $("#autocomplete_filtro_viabilizador").val()
    };
    return filtro;
};

Europa.Controllers.PontoVenda.FiltrarTabela = function () {
    Europa.Controllers.PontoVenda.Tabela.reloadData();
};

Europa.Controllers.PontoVenda.InitAutoCompleteEmpresaVenda = function () {
    Europa.Controllers.PontoVenda.AutoCompleteEmpresaVendaInclusao = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("empresa_venda").Configure();
};

Europa.Controllers.PontoVenda.InitAutoCompleteViabilizador = function () {
    Europa.Controllers.PontoVenda.AutoCompleteViabilizadorInclusao = new Europa.Components.AutoCompleteViabilizador()
        .WithTargetSuffix("viabilizador").Configure();
};


Europa.Controllers.PontoVenda.Novo = function () {
    Europa.Controllers.PontoVenda.Tabela.createRowNewData();
    $("#Situacao").attr("disabled","disabled");
    Europa.Controllers.PontoVenda.InitAutoCompleteEmpresaVenda();
    Europa.Controllers.PontoVenda.InitAutoCompleteViabilizador();
    Europa.Controllers.PontoVenda.Inclur = true;
};


Europa.Controllers.PontoVenda.PreSalvar = function () {
    var objetoLinhaTabela = Europa.Controllers.PontoVenda.Tabela.getDataRowEdit();
    var EmpresaVenda = {
        Id: Europa.Controllers.PontoVenda.AutoCompleteEmpresaVendaInclusao.Value()
    };
    var Viabilizador = {
        Id: Europa.Controllers.PontoVenda.AutoCompleteViabilizadorInclusao.Value()
    };
    objetoLinhaTabela.Nome = $("#Nome").val();
    objetoLinhaTabela.EmpresaVenda = EmpresaVenda;
    objetoLinhaTabela.Viabilizador = Viabilizador;
    Europa.Controllers.PontoVenda.Salvar(objetoLinhaTabela);
}


Europa.Controllers.PontoVenda.Salvar = function (obj) {
    var url = Europa.Controllers.PontoVenda.Inclur ? Europa.Controllers.PontoVenda.UrlIncluir : Europa.Controllers.PontoVenda.UrlAlterar;
    $(".has-error").removeClass("has-error");
    Europa.Validator.ClearForm("#form_pontovenda");
    $.post(url, {model: obj}, function (res) {
            if (res.Sucesso) {
                Europa.Controllers.PontoVenda.Tabela.closeEdition();
                Europa.Controllers.PontoVenda.Tabela.reloadData();
            } else {
                Europa.Controllers.PontoVenda.AddError(res.Campos);
            }
            Europa.Informacao.PosAcao(res);
        });
}


Europa.Controllers.PontoVenda.AddError = function (fields) {
    fields.forEach(function (key) {
        $("[name='" + key + "']").parent().addClass("has-error");
    });
}



Europa.Controllers.PontoVenda.Ativar = function () {
    Europa.Controllers.PontoVenda.PreAlterarSituacao(Europa.Controllers.PontoVenda.UrlReativar, 1);
};

Europa.Controllers.PontoVenda.Suspender = function () {
    Europa.Controllers.PontoVenda.PreAlterarSituacao(Europa.Controllers.PontoVenda.UrlSuspender, 2);
};

Europa.Controllers.PontoVenda.Cancelar = function () {
    Europa.Controllers.PontoVenda.PreAlterarSituacao(Europa.Controllers.PontoVenda.UrlCancelar, 3);
};

Europa.Controllers.PontoVenda.PreAlterarSituacao = function (url, situacao) {
    var objetosSelecionados = Europa.Controllers.PontoVenda.Tabela.getRowsSelect();
    var ids = [];
    objetosSelecionados.forEach(function (item) {
        ids.push(item.Id);
    });

    if (objetosSelecionados != null && objetosSelecionados != undefined && objetosSelecionados.length != 0) {
        Europa.Confirmacao.PreAcaoMulti(Europa.Controllers.PontoVenda.RenderizaSituacaoMensagem(situacao), function () {
            Europa.Controllers.PontoVenda.AlterarSituacao(url, ids);
        });
    } else {
        Europa.Informacao.ChangeHeaderAndContent(
            Europa.i18n.Messages.Erro,
            Europa.i18n.Messages.NenhumRegistroSelecionando
        );
        Europa.Informacao.Show();
    }
}

Europa.Controllers.PontoVenda.RenderizaSituacaoMensagem = function (value) {
    switch (value) {
        case 1:
            return Europa.i18n.Messages.Ativar;
        case 2:
            return Europa.i18n.Messages.Suspender;
        case 3:
            return Europa.i18n.Messages.Cancelar;
    }
};

Europa.Controllers.PontoVenda.AlterarSituacao = function (url, ids) {
    $.post(url, { ids: ids }, function (res) {
        Europa.Controllers.PontoVenda.Tabela.reloadData();
        Europa.Informacao.PosAcao(res);
    }).fail(function () {
        console.log("Ocorreu algum problema.");
    });
}

Europa.Controllers.PontoVenda.ExportarPagina = function () {
    var params = Europa.Controllers.PontoVenda.FilterParams();
    params.order = Europa.Controllers.PontoVenda.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.PontoVenda.Tabela.lastRequestParams.draw;
    params.pageSize = Europa.Controllers.PontoVenda.Tabela.lastRequestParams.pageSize;
    params.start = Europa.Controllers.PontoVenda.Tabela.lastRequestParams.start;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.PontoVenda.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.PontoVenda.ExportarTodos = function () {
    var params = Europa.Controllers.PontoVenda.FilterParams();
    params.order = Europa.Controllers.PontoVenda.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.PontoVenda.Tabela.lastRequestParams.draw;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.PontoVenda.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};