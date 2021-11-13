Europa.Controllers.Gestao = {};
Europa.Controllers.Gestao.Tabela = undefined;
Europa.Controllers.Gestao.Permissoes = undefined;

$(function () {
    $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
    $(".ng-scope").css("word-wrap", "break-word");

});

function TabelaGestao($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.Gestao.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.Gestao.Tabela;
    tabelaWrapper
        .setTemplateEdit([
            '<input type="text" class="form-control" id="DataReferencia" name="DataReferencia" datepicker="datepicker">',
            '<div name="TipoCusto"><select id="autocomplete_tipo_custo" class="form-control"></select></div>',
            '<div name="Classificacao"><select id="autocomplete_classificacao" class="form-control"></select></div>',
            '<div name="Fornecedor"><select id="autocomplete_fornecedor" class="form-control"></select></div>',
            '<input type="text" class="form-control" name="Descricao" id="Descricao" maxlength="256">',
            '<div name="EmpresaVenda"><select id="autocomplete_empresa_venda" class= "form-control"></select></div>',
            '<div name="PontoVenda"><select id="autocomplete_ponto_venda" class= "form-control"></select></div>',
            '<div name="CentroCusto"><select id="autocomplete_centro_custo" class= "form-control"></select></div>',
            '<input type="text" class="form-control" name="ValorBudgetEstimado" id="ValorBudgetEstimado">',
            '<input type="text" class="form-control" name="NumeroChamado" id="NumeroChamado" maxlength="10">',
            '<input type="text" class="form-control" id="DataCriacaoChamado" name="DataCriacaoChamado" datepicker="datepicker">',
            '<input type="text" class="form-control" id="DataFarol" name="DataFarol" datepicker="datepicker">',
            '<input type="text" class="form-control" name="NumeroRequisicaoCompra" id="NumeroRequisicaoCompra" maxlength="10">',
            '<input type="text" class="form-control" name="ValorGasto" id="ValorGasto" >',
            '<input type="text" class="form-control" name="NumeroPedido" id="NumeroPedido" maxlength="10" >',
            '<input type="text" class="form-control" name="Observacao" id="Observacao" maxlength="256" >'

        ])
        .setColumns([
            DTColumnBuilder.newColumn('DataReferencia').withTitle(Europa.i18n.Messages.Referencia).withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
            DTColumnBuilder.newColumn('TipoCusto.Descricao').withTitle(Europa.i18n.Messages.Custo).withOption('width', '150px'),
            DTColumnBuilder.newColumn('Classificacao.Descricao').withTitle(Europa.i18n.Messages.Classificacao).withOption('width', '200px'),
            DTColumnBuilder.newColumn('Fornecedor.NomeFantasia').withTitle(Europa.i18n.Messages.Fornecedor).withOption('width', '150px'),
            DTColumnBuilder.newColumn('Descricao').withTitle(Europa.i18n.Messages.DescricaoServico).withOption('width', '400px'),
            DTColumnBuilder.newColumn('EmpresaVenda.NomeFantasia').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption('width', '200px'),
            DTColumnBuilder.newColumn('PontoVenda.Nome').withTitle(Europa.i18n.Messages.PontoVenda).withOption('width', '200px'),
            DTColumnBuilder.newColumn('CentroCusto.Codigo').withTitle(Europa.i18n.Messages.CentroCusto).withOption('width', '150px'),
            DTColumnBuilder.newColumn('ValorBudgetEstimado').withTitle(Europa.i18n.Messages.BudgetEstimado).withOption('width', '150px').renderWith(renderMoney),
            DTColumnBuilder.newColumn('NumeroChamado').withTitle(Europa.i18n.Messages.Numero).withOption('width', '150px'),
            DTColumnBuilder.newColumn('DataCriacaoChamado').withTitle(Europa.i18n.Messages.DataCriacao).withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
            DTColumnBuilder.newColumn('DataFarol').withTitle(Europa.i18n.Messages.DataFarol).withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
            DTColumnBuilder.newColumn('NumeroRequisicaoCompra').withTitle(Europa.i18n.Messages.RequisicaoCompra).withOption('width', '200px'),
            DTColumnBuilder.newColumn('ValorGasto').withTitle(Europa.i18n.Messages.ValorGasto).withOption('width', '150px').renderWith(renderMoney),
            DTColumnBuilder.newColumn('NumeroPedido').withTitle(Europa.i18n.Messages.NumeroPedido).withOption('width', '150px'),
            DTColumnBuilder.newColumn('Observacao').withTitle(Europa.i18n.Messages.Observacao).withOption('width','400px')

        ])
        .setColActions(actionsHtml, '90px')
        .setIdAreaHeader("datatable_header")
        .setActionSave(Europa.Controllers.Gestao.Salvar)
        .setDefaultOptions('POST', Europa.Controllers.Gestao.UrlListar, Europa.Controllers.Gestao.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButton(Europa.Controllers.Gestao.Permissoes.Atualizar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButton(Europa.Controllers.Gestao.Permissoes.Excluir, "Excluir", "fa fa-trash", "Excluir(" + meta.row + ")", full.Situacao) +
            '</div>';
    }

    function renderMoney(data) {
        if (data) {
            var valor = "R$ ";
            valor = valor + data.toFixed(2).replace(".", ",").replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
            return valor;
        }
        return "";
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

    $scope.Excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.Gestao.Tabela.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.Descricao, function () {
            Europa.Controllers.Gestao.Excluir(objetoLinhaTabela.Id);
        });
    };

    $scope.Editar = function (row) {
        var obj = Europa.Controllers.Gestao.Tabela.getRowData(row);
        $scope.rowEdit(row);
        Europa.Controllers.Gestao.InitAutoCompletes();
        Europa.Controllers.Gestao.InitDatePickers();
        Europa.Controllers.Gestao.InitMask();
        Europa.Controllers.Gestao.AutoCompleteTipoCusto.SetValue(obj.TipoCusto.Id, obj.TipoCusto.Descricao);
        Europa.Controllers.Gestao.AutoCompleteClassificacao.SetValue(obj.Classificacao.Id, obj.Classificacao.Descricao);
        Europa.Controllers.Gestao.AutoCompleteFornecedor.SetValue(obj.Fornecedor.Id, obj.Fornecedor.NomeFantasia);
        Europa.Controllers.Gestao.AutoCompleteEmpresaVenda.SetValue(obj.EmpresaVenda.Id, obj.EmpresaVenda.NomeFantasia);
        if (obj.PontoVenda !== null) {
            Europa.Controllers.Gestao.AutoCompletePontoVenda.SetValue(obj.PontoVenda.Id, obj.PontoVenda.Nome);
        }
        Europa.Controllers.Gestao.AutoCompleteCentroCusto.SetValue(obj.CentroCusto.Id, obj.CentroCusto.Codigo);
        Europa.Controllers.Gestao.Incluir = false;
    };
};

DataTableApp.controller('Gestao', TabelaGestao);

Europa.Controllers.Gestao.InitAutoCompletes = function () {

    Europa.Controllers.Gestao.AutoCompleteTipoCusto = new Europa.Components.AutoCompleteTipoCusto()
    .WithTargetSuffix("tipo_custo").Configure();
    
    Europa.Controllers.Gestao.AutoCompleteClassificacao = new Europa.Components.AutoCompleteClassificacao()
    .WithTargetSuffix("classificacao").Configure();
    
    Europa.Controllers.Gestao.AutoCompleteFornecedor= new Europa.Components.AutoCompleteFornecedor()
    .WithTargetSuffix("fornecedor").Configure();
    
    Europa.Controllers.Gestao.AutoCompleteEmpresaVenda = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("empresa_venda").Configure();

    Europa.Controllers.Gestao.ConfigureAutoCompleteEmpresaVenda(Europa.Controllers.Gestao);
    
    Europa.Controllers.Gestao.AutoCompletePontoVenda = new Europa.Components.AutoCompletePontoVenda()
        .WithTargetSuffix("ponto_venda").Configure();

    Europa.Controllers.Gestao.ConfigureAutoCompletePontoVenda(Europa.Controllers.Gestao)
    
    Europa.Controllers.Gestao.AutoCompleteCentroCusto = new Europa.Components.AutoCompleteCentroCusto()
    .WithTargetSuffix("centro_custo").Configure();
    
};

Europa.Controllers.Gestao.ConfigureAutoCompleteEmpresaVenda = function (autocompleteWrapper) {
    $('#autocomplete_empresa_venda').on('change', function (e) {
        var idTemp = $(this).val();
        if (idTemp === null || idTemp === undefined || idTemp === "" || idTemp === 0) {
            autocompleteWrapper.AutoCompletePontoVenda.Disable();
        } else {
            autocompleteWrapper.AutoCompletePontoVenda.Enable();
        }
        autocompleteWrapper.AutoCompletePontoVenda.Clean();
    });

    autocompleteWrapper.AutoCompleteEmpresaVenda.Configure();
}

Europa.Controllers.Gestao.ConfigureAutoCompletePontoVenda = function (autocompleteWrapper) {
    autocompleteWrapper.AutoCompletePontoVenda.Data = function (params) {
        return {
            start: 0,
            pageSize: 10,
            filter: [
                {
                    value: params.term,
                    column: this.param,
                    regex: true
                },
                {
                    value: function () {
                        return $("#autocomplete_empresa_venda").val();
                    },
                    column: 'idEmpresaVenda'
                }
            ],
            order: [
                {
                    value: "asc",
                    column: this.param
                }
            ]
        };
    };

    autocompleteWrapper.AutoCompletePontoVenda.Configure();
}

Europa.Controllers.Gestao.InitDatePickers = function () {
    var DataReferencia = Europa.String.FormatAsGeenDate($("#DataReferencia").val());
    var DataCriacao = Europa.String.FormatAsGeenDate($("#DataCriacaoChamado").val());
    var DataFarol = Europa.String.FormatAsGeenDate($("#DataFarol").val());

    Europa.Components.DatePicker.AutoApply();

    Europa.Controllers.Gestao.DataReferencia = new Europa.Components.DatePicker()
        .WithTarget("#DataReferencia")
        .WithFormat("DD/MM/YYYY")
        .WithValue(DataReferencia)
        .Configure();

    Europa.Controllers.Gestao.DataCriacao = new Europa.Components.DatePicker()
        .WithTarget("#DataCriacaoChamado")
        .WithFormat("DD/MM/YYYY")
        .WithValue(DataCriacao)
        .Configure();

    Europa.Controllers.Gestao.DataFarol = new Europa.Components.DatePicker()
        .WithTarget("#DataFarol")
        .WithFormat("DD/MM/YYYY")
        .WithValue(DataFarol)
        .Configure();
};

Europa.Controllers.Gestao.InitMask = function () {
    Europa.Mask.Int($("#NumeroPedido"));
    Europa.Mask.Int($("#NumeroRequisicaoCompra"));
    Europa.Mask.Dinheiro($("#ValorBudgetEstimado"));
    Europa.Mask.Dinheiro($("#ValorGasto"));
};

Europa.Controllers.Gestao.Filtrar = function () {
    Europa.Controllers.Gestao.Tabela.reloadData();
};

Europa.Controllers.Gestao.FilterParams = function () {
    var param = {
        ReferenciaDe: $("#ReferenciaDe").val(),
        ReferenciaAte: $("#ReferenciaAte").val(),
        idTipoCusto: $("#autocomplete_filtro_tipo_custo").val(),
        idClassificacao: $("#autocomplete_filtro_classificacao").val(),
        idFornecedor: $("#autocomplete_filtro_fornecedor").val(),
        idEmpresaVenda: $("#autocomplete_filtro_empresa_venda").val(),
        idPontoVenda: $("#autocomplete_filtro_ponto_venda").val(),
        idCentroCusto: $("#autocomplete_filtro_centro_custo").val()
    };
    return param;
};

Europa.Controllers.Gestao.Novo = function () {
    Europa.Controllers.Gestao.Tabela.createRowNewData();
    Europa.Controllers.Gestao.InitAutoCompletes();
    Europa.Controllers.Gestao.InitDatePickers();
    Europa.Controllers.Gestao.InitMask();
    Europa.Controllers.Gestao.AutoCompletePontoVenda.Disable();
    Europa.Controllers.Gestao.Incluir = true;
};

Europa.Controllers.Gestao.Salvar = function () {
    var obj = Europa.Controllers.Gestao.Tabela.getDataRowEdit();
    var EmpresaVenda = {Id: Europa.Controllers.Gestao.AutoCompleteEmpresaVenda.Value()};
    var TipoCusto = {Id: Europa.Controllers.Gestao.AutoCompleteTipoCusto.Value()};
    var Classificacao = { Id: Europa.Controllers.Gestao.AutoCompleteClassificacao.Value() };
    var Fornecedor = { Id: Europa.Controllers.Gestao.AutoCompleteFornecedor.Value() };
    var CentroCusto = { Id: Europa.Controllers.Gestao.AutoCompleteCentroCusto.Value() };
    var PontoVenda = { Id: Europa.Controllers.Gestao.AutoCompletePontoVenda.Value() };

    obj.TipoCusto = TipoCusto;
    obj.Classificacao = Classificacao;
    obj.Fornecedor = Fornecedor;
    obj.EmpresaVenda = EmpresaVenda;
    obj.PontoVenda = PontoVenda;
    obj.CentroCusto = CentroCusto;
    obj.ValorBudgetEstimado = obj.ValorBudgetEstimado.replace(/\./g, '');
    obj.ValorGasto = obj.ValorGasto.replace(/\./g,'');
    var url = Europa.Controllers.Gestao.Incluir ? Europa.Controllers.Gestao.UrlIncluir : Europa.Controllers.Gestao.UrlAlterar
    $.post(url, { model: obj }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.Gestao.Tabela.closeEdition();
            Europa.Controllers.Gestao.Tabela.reloadData();
            Europa.Controllers.Gestao.LimparErro();
        } else {
            Europa.Controllers.Gestao.AdicionarErro(resposta.Campos);
        }
    });
};

Europa.Controllers.Gestao.Excluir = function (id) {
    $.post(Europa.Controllers.Gestao.UrlExcluir, { id: id }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.Gestao.Tabela.reloadData();
        }
    });
};

Europa.Controllers.Gestao.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.Gestao.LimparErro = function () {
    $("[name='Descricao']").parent().removeClass("has-error");
};


Europa.Controllers.Gestao.ExportarPagina = function () {
    var params = Europa.Controllers.Gestao.FilterParams();
    params.order = Europa.Controllers.Gestao.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.Gestao.Tabela.lastRequestParams.draw;
    params.pageSize = Europa.Controllers.Gestao.Tabela.lastRequestParams.pageSize;
    params.start = Europa.Controllers.Gestao.Tabela.lastRequestParams.start;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.Gestao.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.Gestao.ExportarTodos = function () {
    var params = Europa.Controllers.Gestao.FilterParams();
    params.order = Europa.Controllers.Gestao.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.Gestao.Tabela.lastRequestParams.draw;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.Gestao.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

