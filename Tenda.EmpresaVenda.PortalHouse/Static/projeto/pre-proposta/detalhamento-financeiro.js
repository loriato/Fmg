"use strict";

Europa.Controllers.PreProposta.DetalhamentoFinanceiro = {};
Europa.Controllers.PreProposta.DetalhamentoFinanceiro.IdPreProposta = undefined;
Europa.Controllers.PreProposta.DetalhamentoFinanceiro.UrlListar = undefined;
Europa.Controllers.PreProposta.DetalhamentoFinanceiro.UrlIncluir = undefined;
Europa.Controllers.PreProposta.DetalhamentoFinanceiro.UrlAlterar = undefined;
Europa.Controllers.PreProposta.DetalhamentoFinanceiro.UrlExcluir = undefined;
Europa.Controllers.PreProposta.DetalhamentoFinanceiro.DropDownTipoParcela = undefined;

Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Init = function () {
    // Herdando configurações já efetuadas, já que este método só é executado após a página ser carregada.
    // Se não fizer isso, sobrescrevo todas as propriedades setadas anteriormente (geralmente URLs e permissÕes)
    var self = Europa.Controllers.PreProposta.DetalhamentoFinanceiro;

    self.modoInclusao = true;

    self.Configure = function (idPreProposta) {
        self.IdPreProposta = idPreProposta;
        self.Reload();
    };

    self.Reset = function () {
        self.IdPreProposta = undefined;
    };

    self.Reload = function () {
        self.Tabela.reloadData();
    };

    self.Novo = function () {
        self.modoInclusao = true;
        self.Tabela.createRowNewData();
        self.AplicarMascarasEDatePicker();
    };

    self.Editar = function () {

    };

    self.Excluir = function (objetoLinhaTabela) {
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, Europa.i18n.Enum.Resolve("TipoParcela", objetoLinhaTabela.TipoParcela),
            function () {
                var requestContent = {
                    idPreProposta: Europa.Controllers.PreProposta.DetalhamentoFinanceiro.IdPreProposta,
                    idPlanoPagamento: objetoLinhaTabela.Id
                };
                Spinner.Show();
                $.post(Europa.Controllers.PreProposta.DetalhamentoFinanceiro.UrlExcluir, requestContent, function (res) {
                    Spinner.Hide();
                    if (res.Sucesso) {
                        Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Reload();
                        Europa.Informacao.PosAcao(res);

                        // FIXME: achar uma forma melhor de notificar os demais grids
                        // O caminho é avisar o pai (pré-proposta) de determinada ação, e o PAI toma as ações necessárias
                        Europa.Controllers.PreProposta.RefreshTotalFinanceiro();
                    } else {
                        Europa.Informacao.PosAcao(res);
                    }
                });
            });
    };

    self.Salvar = function (obj) {
        var url = self.modoInclusao ? self.UrlIncluir : self.UrlAlterar;
        $("#area_detalhamento_financeiro .has-error").removeClass("has-error");
        // Europa.Validator.ClearForm("#form_pontovenda");
        Spinner.Show();
        $.post(url, { idPreProposta: self.IdPreProposta, planoPagamento: obj }, function (res) {
            Spinner.Hide();
            if (res.Sucesso) {
                self.Tabela.closeEdition();
                self.Reload();
                self.modoInclusao = true;

                // FIXME: achar uma forma melhor de notificar os demais grids
                // O caminho é avisar o pai (pré-proposta) de determinada ação, e o PAI toma as ações necessárias
                Europa.Controllers.PreProposta.RefreshTotalFinanceiro();
            } else {
                self.AddError(res.Campos);
            }
            Europa.Informacao.PosAcao(res);
        });
    };

    self.AddError = function (fields) {
        fields.forEach(function (key) {
            $("[name='" + key + "']").parent().addClass("has-error");
        });
    };
    Europa.Controllers.PreProposta.DetalhamentoFinanceiro = self;
};



////////////////////////////////////////////////////////////////////////////////////
// Funções Datatable
////////////////////////////////////////////////////////////////////////////////////
DataTableApp.controller('detalhamentoFinanceiroTable', detalhamentoFinanceiroTable);

function detalhamentoFinanceiroTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Tabela;
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
            '<input type="text" class="form-control form-control-sm" id="DetalhamentoFinanceiroNumeroParcelasEdit" name="NumeroParcelas" maxlength="3" onblur="Europa.Controllers.PreProposta.DetalhamentoFinanceiro.AtualizarValorTotal()">',
            '<input type="text" class="form-control form-control-sm dinheiro" id="DetalhamentoFinanceiroValorParcelaEdit" name="ValorParcela" maxlength="10" style="text-align:right" onblur="Europa.Controllers.PreProposta.DetalhamentoFinanceiro.AtualizarValorTotal()">',
            '<input type="text" class="form-control form-control-sm dinheiro" id="DetalhamentoFinanceiroTotalEdit" name="Total" readonly="readonly" style="text-align:right">',
            '<input type="text" class="form-control form-control-sm" id="DetalhamentoFinanceiroDataVencimentoEdit" name="DataVencimento" datepicker="datepicker">'
        ])
        .setActionSave(Europa.Controllers.PreProposta.DetalhamentoFinanceiro.PreSalvar)
        .setIdAreaHeader("detalhamento_financeiro_datatable_header")
        .setColActions(actionsHtml, '50px')
        .setAutoInit(true)
        .setDefaultOptions('POST', Europa.Controllers.PreProposta.DetalhamentoFinanceiro.UrlListar, Europa.Controllers.PreProposta.DetalhamentoFinanceiro.FilterParams);

    function actionsHtml(data, type, full, meta) {
        if (Europa.Controllers.PreProposta.Permissoes.DetalhamentoFinanceiroManual) {
            return '<div>' +
                $scope.renderButtonEdit(Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Permissoes.Atualizar, "Editar", "fas fa-pen", "editar(" + meta.row + ")", full.Situacao) +
                $scope.renderButtonDelete(Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Permissoes.Excluir, "Excluir", "fas fa-trash", "excluir(" + meta.row + ")", full.Situacao) +
                '</div>';
        }

        return "";
    }

    $scope.renderButtonEdit = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 3 || !Europa.Controllers.PreProposta.PodeManterAssociacoes()) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn color-blue')
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
            .addClass('btn color-blue')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.editar = function (row) {
        $scope.rowEdit(row);
        $("#DetalhamentoFinanceiroValorParcelaEdit").mask("##.###,99", { reverse: true })

        var objeto = Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Tabela.getRowData(row);
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

Europa.Controllers.PreProposta.DetalhamentoFinanceiro.FilterParams = function () {
    return {
        idPreProposta: Europa.Controllers.PreProposta.DetalhamentoFinanceiro.IdPreProposta || 0
    };
};

Europa.Controllers.PreProposta.DetalhamentoFinanceiro.PreSalvar = function () {
    var objetoLinhaTabela = Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Tabela.getDataRowEdit();
    Europa.Controllers.PreProposta.DetalhamentoFinanceiro.AtualizarValorTotal();
    objetoLinhaTabela.ValorParcela = objetoLinhaTabela.ValorParcela.replace(/\./g, '');
    objetoLinhaTabela.Total = objetoLinhaTabela.Total.replace(/\./g, '');

    Europa.Controllers.PreProposta.DetalhamentoFinanceiro.Salvar(objetoLinhaTabela);
};

Europa.Controllers.PreProposta.DetalhamentoFinanceiro.AplicarMascarasEDatePicker = function () {

    Europa.String.FormatAsMoney($('#DetalhamentoFinanceiroValorParcelaEdit').val());
    Europa.Mask.ApplyByClass("dinheiro", Europa.Mask.FORMAT_MONEY, undefined, true);

    Europa.Controllers.PreProposta.DetalhamentoFinanceiro.AtualizarValorTotal();
    $("#DetalhamentoFinanceiroNumeroParcelasEdit").mask("000");

    var date = Europa.String.FormatAsGeenDate($("#DetalhamentoFinanceiroDataVencimentoEdit").val());
    Europa.Components.DatePicker.AutoApply();
    Europa.Controllers.PreProposta.DetalhamentoFinanceiro.DataVencimento = new Europa.Components.DatePicker()
        .WithTarget("#DetalhamentoFinanceiroDataVencimentoEdit")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate(Europa.Date.Now())
        .WithValue(date)
        .Configure();
};

Europa.Controllers.PreProposta.DetalhamentoFinanceiro.AtualizarValorTotal = function () {
    var valorParcela = $("#DetalhamentoFinanceiroValorParcelaEdit").val();
    valorParcela = valorParcela.replace(/\./g, '').replace(',', '.');
    var numeroParcelas = $("#DetalhamentoFinanceiroNumeroParcelasEdit").val();
    var valorTotal = valorParcela * numeroParcelas;
    var valorTotalDecimal = parseFloat(Math.round(valorTotal * 100) / 100).toFixed(2);
    $("#DetalhamentoFinanceiroTotalEdit").val(valorTotalDecimal.replace('.', ','));
    Europa.Mask.Dinheiro("#DetalhamentoFinanceiroTotalEdit");
};