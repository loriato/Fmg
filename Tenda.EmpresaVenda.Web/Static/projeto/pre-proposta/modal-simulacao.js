Europa.Controllers.PreProposta.ModalSimulacao = {};
Europa.Controllers.PreProposta.ModalSimulacao.Inclusao = false;

$(function () {
    Europa.Controllers.PreProposta.ModalSimulacao.LimparResultado();
})

//PRÉ-PROPOSTA
DataTableApp.controller('detalhamentoFinanceiroModalTable', detalhamentoFinanceiroModalTable);

function detalhamentoFinanceiroModalTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.PreProposta.ModalSimulacao.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.PreProposta.ModalSimulacao.Tabela;
    tabelaWrapper.
        setColumns([
            DTColumnBuilder.newColumn('TipoParcela').withTitle(Europa.i18n.Messages.TipoParcela).withOption('width', '20%').withOption('type', 'enum-format-TipoParcela'),
            DTColumnBuilder.newColumn('NumeroParcelas').withTitle(Europa.i18n.Messages.NumeroParcelas).withOption('width', '15%'),
            DTColumnBuilder.newColumn('ValorParcela').withTitle(Europa.i18n.Messages.ValorParcela).withOption('width', '20%').renderWith(renderMoney).withClass('dt-body-right'),
            DTColumnBuilder.newColumn('Total').withTitle(Europa.i18n.Messages.Total).withOption('width', '20%').renderWith(renderMoney).withClass('dt-body-right'),
            DTColumnBuilder.newColumn('DataVencimento').withTitle(Europa.i18n.Messages.DataVencimento).withOption('width', '15%').withOption("type", "date-format-DD/MM/YYYY")
        ])
        .setTemplateEdit([
            Europa.Controllers.PreProposta.DetalhamentoFinanceiro.DropDownTipoParcela,
            '<input type="text" class="form-control" id="DetalhamentoFinanceiroNumeroParcelasEdit" name="NumeroParcelas" maxlength="3" onblur="Europa.Controllers.PreProposta.DetalhamentoFinanceiro.AtualizarValorTotal()">',
            '<input type="text" class="form-control" id="DetalhamentoFinanceiroValorParcelaEdit" name="ValorParcela" maxlength="10" style="text-align:right" onblur="Europa.Controllers.PreProposta.DetalhamentoFinanceiro.AtualizarValorTotal()">',
            '<input type="text" class="form-control dinheiro" id="DetalhamentoFinanceiroTotalEdit" name="Total" readonly="readonly" style="text-align:right">',
            '<input type="text" class="form-control" id="DetalhamentoFinanceiroDataVencimentoEdit" name="DataVencimento" datepicker="datepicker">'
        ])
        .setActionSave(Europa.Controllers.PreProposta.ModalSimulacao.PreSalvar)
        .setIdAreaHeader("detalhamento_financeiro_datatable_modal_header")
        .setColActions(actionsHtml, '50px')
        .setDefaultOptions('POST', Europa.Controllers.PreProposta.DetalhamentoFinanceiro.UrlListar, Europa.Controllers.PreProposta.ModalSimulacao.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButtonEdit(Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Permissoes.Atualizar, "Editar", "fa fa-edit", "editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonDelete(Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Permissoes.Excluir, "Excluir", "fa fa-trash", "excluir(" + meta.row + ")", full.Situacao) +
            '</div>';
    }

    $scope.renderButtonEdit = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 3 || !Europa.Controllers.PreProposta.PodeManterAssociacoes()) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.renderButtonDelete = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 3 || !Europa.Controllers.PreProposta.PodeManterAssociacoes()) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.editar = function (row) {
        $scope.rowEdit(row);

        $("#DetalhamentoFinanceiroValorParcelaEdit").mask("##.###,99", { reverse: true })
                
        var objeto = Europa.Controllers.PreProposta.ModalSimulacao.Tabela.getRowData(row);
        var valorParcela = objeto.ValorParcela.toString().replace('.', ',');
        $("#DetalhamentoFinanceiroValorParcelaEdit").val(valorParcela);
        
        Europa.Controllers.PreProposta.DetalhamentoFinanceiro.AtualizarValorTotal();

        var date = Europa.String.FormatAsGeenDate($("#DetalhamentoFinanceiroDataVencimentoEdit").val());
        Europa.Components.DatePicker.AutoApply();
        Europa.Controllers.PreProposta.DetalhamentoFinanceiro.DataVencimento = new Europa.Components.DatePicker()
            .WithTarget("#DetalhamentoFinanceiroDataVencimentoEdit")
            .WithFormat("DD/MM/YYYY")
            .WithMinDate(Europa.Date.Now())
            .WithValue(date)
            .Configure();

        Europa.Controllers.PreProposta.DetalhamentoFinanceiro.modoInclusao = false;
    };

    $scope.excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Tabela.getRowData(row);
        Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Excluir(objetoLinhaTabela);
    };

    function renderMoney(data) {
        return Europa.String.FormatMoney(data);
    }
}

Europa.Controllers.PreProposta.ModalSimulacao.FilterParams = function () {
    return {
        idPreProposta: $("#PreProposta_Id").val()
    };
};

Europa.Controllers.PreProposta.ModalSimulacao.PreSalvar = function () {
    var objetoLinhaTabela = Europa.Controllers.PreProposta.ModalSimulacao.Tabela.getDataRowEdit();

    objetoLinhaTabela.ValorParcela = objetoLinhaTabela.ValorParcela.replace(/\./g, '');
    objetoLinhaTabela.Total = objetoLinhaTabela.Total.replace(/\./g, '');
    
    Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Salvar(objetoLinhaTabela);
};

Europa.Controllers.PreProposta.ModalSimulacao.Novo = function () {
    Europa.Controllers.PreProposta.ModalSimulacao.Inclusao = true;
    Europa.Controllers.PreProposta.ModalSimulacao.Tabela.createRowNewData();
    Europa.Controllers.PreProposta.DetalhamentoFinanceiro.AplicarMascarasEDatePicker();
};

//SIMULADOR
Europa.Controllers.PreProposta.ModalSimulacao.OnChangeSimulacao = function () {

    Europa.Controllers.PreProposta.ModalSimulacao.TabelaResultadoSimulacao.reloadData();

    return
    Europa.Controllers.PreProposta.ModalSimulacao.LimparResultado();

    var simulacoes = $("#simulacoes").val();

    if (simulacoes == "") {        
        return;
    }

    var codigo = simulacoes.split('-')

    var parametro = {
        Codigo: codigo[0],
        Digito: codigo[1]
    };

    $.post(Europa.Controllers.PreProposta.UrlResultadoSimulacao, { parametro: parametro }, function (res) {
        if (res.Sucesso) {
            var obj = res.Objeto;
            Europa.Controllers.PreProposta.ModalSimulacao.PreencherResultados(obj);
        } else {
            Europa.Informacao.PosAcao(res);
        } 
    });
};

Europa.Controllers.PreProposta.ModalSimulacao.PreencherResultados = function (obj) {

    if (obj == null) return;

    $("#SimuladorDto_CotaFinanciamento").val(Europa.String.FormatAsMoney(obj.CotaFinanciamento.toString()))
    $("#SimuladorDto_Subsidio").val(Europa.String.FormatAsMoney(obj.Subsidio.toString()))
    $("#SimuladorDto_FinanciamentoPreAprovado").val(Europa.String.FormatAsMoney(obj.FinanciamentoPreAprovado.toString()))
    $("#SimuladorDto_TaxaJuros").val(obj.TaxaJuros)
    $("#SimuladorDto_PrazoAmortizacao").val(obj.PrazoAmortizacao + " meses")
    $("#SimuladorDto_PrazoObraCef").val(obj.PrazoObraCef + " meses")
    $("#SimuladorDto_PrimeiraParcelaFinanciamento").val(Europa.String.FormatAsMoney(obj.PrimeiraParcelaFinanciamento.toString()))

    $("#SimuladorDto_ValorEntrada").val(Europa.String.FormatAsMoney(obj.ValorEntrada.toString()))
    $("#SimuladorDto_ValorFgts").val(Europa.String.FormatAsMoney(obj.ValorFgts.toString()))

    $("#SimuladorDto_ValorTotalPreChaves").val(Europa.String.FormatAsMoney(obj.ValorTotalPreChaves.toString()))
    $("#SimuladorDto_TotalPreChavesIntermediaria").val(Europa.String.FormatAsMoney(obj.TotalPreChavesIntermediaria.toString()))
    $("#SimuladorDto_ValorTotalAprovadoCliente").val(Europa.String.FormatAsMoney(obj.ValorTotalAprovadoCliente.toString()))

    $("#SimuladorDto_QuantidadeParcelaAprovadaPreChaves").val(obj.QuantidadeParcelaAprovadaPreChaves)
    $("#SimuladorDto_ValorParcelaAprovadaPreChaves").val(Europa.String.FormatAsMoney(obj.ValorParcelaAprovadaPreChaves.toString()))
    $("#SimuladorDto_ValorTotalAprovadoPreChaves").val(Europa.String.FormatAsMoney(obj.ValorTotalAprovadoPreChaves.toString()))

    $("#SimuladorDto_QuantidadeParcelaNegociadaPreChaves").val(obj.QuantidadeParcelaNegociadaPreChaves)
    $("#SimuladorDto_ValorParcelaNegociadaPreChaves").val(Europa.String.FormatAsMoney(obj.ValorParcelaNegociadaPreChaves.toString()))
    $("#SimuladorDto_ValorTotalNegociadoPreChaves").val(Europa.String.FormatAsMoney(obj.ValorTotalNegociadoPreChaves.toString()))

    $("#SimuladorDto_ValorTotalIntermediaria").val(Europa.String.FormatAsMoney(obj.ValorTotalIntermediaria.toString()))
    $("#SimuladorDto_DataVencimentoPre").val(Europa.Date.toGeenDateFormat(obj.DataVencimentoPre))

    $("#SimuladorDto_ValorPrimeiraIntermediaria").val(Europa.String.FormatAsMoney(obj.ValorPrimeiraIntermediaria.toString()))
    $("#SimuladorDto_DataPrimeiraIntermediaria").val(Europa.Date.toGeenDateFormat(obj.DataPrimeiraIntermediaria))
    $("[name='SimuladorDto.TotalPreChavesIntermediaria']").val(Europa.String.FormatAsMoney(obj.TotalPreChavesIntermediaria.toString()))

    $("#SimuladorDto_ValorSegundaIntermediaria").val(Europa.String.FormatAsMoney(obj.ValorSegundaIntermediaria.toString()))
    $("#SimuladorDto_DataSegundaIntermediaria").val(Europa.Date.toGeenDateFormat(obj.DataSegundaIntermediaria))
    var totalIntermediaria = obj.TotalPreChavesIntermediaria + obj.TotalNegociadoIntermediaria;
    $("#totalIntermediaria").val(Europa.String.FormatAsMoney(totalIntermediaria.toString()))

    $("#SimuladorDto_InicioPagamentoPosChaves").val(Europa.Date.toGeenDateFormat(obj.InicioPagamentoPosChaves))

    $("#SimuladorDto_QuantidadeParcelaAprovadaPosChaves").val(obj.QuantidadeParcelaAprovadaPosChaves)
    $("#SimuladorDto_ValorParcelaAprovadaPosChaves").val(Europa.String.FormatAsMoney(obj.ValorParcelaAprovadaPosChaves.toString()))
    $("#SimuladorDto_ValorTotalAprovadoPosChaves").val(Europa.String.FormatAsMoney(obj.ValorTotalAprovadoPosChaves.toString()))

    $("#SimuladorDto_QuantidadeParcelaNegociadaPosChaves").val(obj.QuantidadeParcelaNegociadaPosChaves)
    $("#SimuladorDto_ValorParcelaNegociadaPosChaves").val(Europa.String.FormatAsMoney(obj.ValorParcelaNegociadaPosChaves.toString()))
    $("#SimuladorDto_ValorTotalNegociadoPosChaves").val(Europa.String.FormatAsMoney(obj.ValorTotalNegociadoPosChaves.toString()))

    $("#SimuladorDto_ValorEmolumento").val(Europa.String.FormatAsMoney(obj.ValorEmolumento.toString()))
    $("#SimuladorDto_ValorItbi").val(Europa.String.FormatAsMoney(obj.ValorItbi.toString()))
    $("#SimuladorDto_ValorTotal").val(Europa.String.FormatAsMoney(obj.ValorTotal.toString()))
};

Europa.Controllers.PreProposta.ModalSimulacao.LimparResultado = function () {
    $("#SimuladorDto_CotaFinanciamento").val("")
    $("#SimuladorDto_Subsidio").val("")
    $("#SimuladorDto_FinanciamentoPreAprovado").val("")
    $("#SimuladorDto_TaxaJuros").val("")
    $("#SimuladorDto_PrazoAmortizacao").val("")
    $("#SimuladorDto_PrazoObraCef").val("")
    $("#SimuladorDto_PrimeiraParcelaFinanciamento").val("")

    $("#SimuladorDto_ValorEntrada").val("")
    $("#SimuladorDto_ValorFgts").val("")

    $("#SimuladorDto_ValorTotalPreChaves").val("")
    $("#SimuladorDto_TotalPreChavesIntermediaria").val("")
    $("#SimuladorDto_ValorTotalAprovadoCliente").val("")

    $("#SimuladorDto_QuantidadeParcelaAprovadaPreChaves").val("")
    $("#SimuladorDto_ValorParcelaAprovadaPreChaves").val("")
    $("#SimuladorDto_ValorTotalAprovadoPreChaves").val("")

    $("#SimuladorDto_QuantidadeParcelaNegociadaPreChaves").val("")
    $("#SimuladorDto_ValorParcelaNegociadaPreChaves").val("")
    $("#SimuladorDto_ValorTotalNegociadoPreChaves").val("")

    $("#SimuladorDto_ValorTotalIntermediaria").val("")
    $("#SimuladorDto_DataVencimentoPre").val("")

    $("#SimuladorDto_ValorPrimeiraIntermediaria").val("")
    $("#SimuladorDto_DataPrimeiraIntermediaria").val("")
    $("[name='SimuladorDto.TotalPreChavesIntermediaria']").val("")

    $("#SimuladorDto_ValorSegundaIntermediaria").val("")
    $("#SimuladorDto_DataSegundaIntermediaria").val("")
    $("#totalIntermediaria").val("")

    $("#SimuladorDto_InicioPagamentoPosChaves").val("")

    $("#SimuladorDto_QuantidadeParcelaAprovadaPosChaves").val("")
    $("#SimuladorDto_ValorParcelaAprovadaPosChaves").val("")
    $("#SimuladorDto_ValorTotalAprovadoPosChaves").val("")

    $("#SimuladorDto_QuantidadeParcelaNegociadaPosChaves").val("")
    $("#SimuladorDto_ValorParcelaNegociadaPosChaves").val("")
    $("#SimuladorDto_ValorTotalNegociadoPosChaves").val("")

    $("#SimuladorDto_ValorEmolumento").val("")
    $("#SimuladorDto_ValorItbi").val("")
    $("#SimuladorDto_ValorTotal").val("")
}

Europa.Controllers.PreProposta.ModalSimulacao.AtualizarSimulacoes = function () {
    Europa.Controllers.PreProposta.ModalSimulacao.LimparResultado();

    var preProposta = $("#PreProposta_Codigo").val();
    $.post(Europa.Controllers.PreProposta.UrlAtualizarResultadosSimulacao,
        { preProposta: preProposta }, function (res) {
            if (res.Sucesso) {
                var lista = res.Objeto;

                var options = '<option value="">Selecione...</option>';

                lista.forEach(function (obj) {
                    options += '<option value="' + obj.Codigo + "-" + obj.Digito + '">' + obj.Codigo + "-" + obj.Digito + '</option>';
                });

                $("#simulacoes").html(options)
            }
            Europa.Informacao.PosAcao(res)
        });
}

//Simulação
DataTableApp.controller('ResultadoSimulacaoModalTable', resultadoSimulacaoModalTable);

function resultadoSimulacaoModalTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.PreProposta.ModalSimulacao.TabelaResultadoSimulacao = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.PreProposta.ModalSimulacao.TabelaResultadoSimulacao;
    tabelaWrapper.
        setColumns([
            DTColumnBuilder.newColumn('TipoParcela').withTitle(Europa.i18n.Messages.TipoParcela).withOption('width', '20%').withOption('type', 'enum-format-TipoParcela'),
            DTColumnBuilder.newColumn('NumeroParcelas').withTitle(Europa.i18n.Messages.NumeroParcelas).withOption('width', '15%'),
            DTColumnBuilder.newColumn('ValorParcela').withTitle(Europa.i18n.Messages.ValorParcela).withOption('width', '20%').renderWith(renderMoney).withClass('dt-body-right'),
            DTColumnBuilder.newColumn('Total').withTitle(Europa.i18n.Messages.Total).withOption('width', '20%').renderWith(renderMoney).withClass('dt-body-right'),
            DTColumnBuilder.newColumn('DataVencimento').withTitle(Europa.i18n.Messages.DataVencimento).withOption('width', '15%').withOption("type", "date-format-DD/MM/YYYY")
        ])
        .setAutoInit(false)
        .setIdAreaHeader("resultado_simulacao_datatable_modal_header")
        .setDefaultOptions('POST', Europa.Controllers.PreProposta.UrlDetalhamentoFinanceiroBySimulador, Europa.Controllers.PreProposta.ModalSimulacao.FiltroResiltadoSimulacao);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButtonEdit(Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Permissoes.Atualizar, "Editar", "fa fa-edit", "editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonDelete(Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Permissoes.Excluir, "Excluir", "fa fa-trash", "excluir(" + meta.row + ")", full.Situacao) +
            '</div>';
    }

    $scope.renderButtonEdit = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 3 || !Europa.Controllers.PreProposta.PodeManterAssociacoes()) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.renderButtonDelete = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 3 || !Europa.Controllers.PreProposta.PodeManterAssociacoes()) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.editar = function (row) {
        $scope.rowEdit(row);

        $("#DetalhamentoFinanceiroValorParcelaEdit").mask("##.###,99", { reverse: true })

        var objeto = Europa.Controllers.PreProposta.ModalSimulacao.Tabela.getRowData(row);
        var valorParcela = objeto.ValorParcela.toString().replace('.', ',');
        $("#DetalhamentoFinanceiroValorParcelaEdit").val(valorParcela);

        Europa.Controllers.PreProposta.DetalhamentoFinanceiro.AtualizarValorTotal();

        var date = Europa.String.FormatAsGeenDate($("#DetalhamentoFinanceiroDataVencimentoEdit").val());
        Europa.Components.DatePicker.AutoApply();
        Europa.Controllers.PreProposta.DetalhamentoFinanceiro.DataVencimento = new Europa.Components.DatePicker()
            .WithTarget("#DetalhamentoFinanceiroDataVencimentoEdit")
            .WithFormat("DD/MM/YYYY")
            .WithMinDate(Europa.Date.Now())
            .WithValue(date)
            .Configure();

        Europa.Controllers.PreProposta.DetalhamentoFinanceiro.modoInclusao = false;
    };

    $scope.excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Tabela.getRowData(row);
        Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Excluir(objetoLinhaTabela);
    };

    function renderMoney(data) {
        return Europa.String.FormatMoney(data);
    }
}

Europa.Controllers.PreProposta.ModalSimulacao.FiltroResiltadoSimulacao = function () {
    var simulacoes = $("#simulacoes").val();

    if (simulacoes == "") {
        return;
    }

    var codigo = simulacoes.split('-')

    var parametro = {
        Codigo: codigo[0],
        Digito: codigo[1]
    };

    return parametro;
}
