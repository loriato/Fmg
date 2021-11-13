"use strict";

Europa.Controllers.PreProposta.ItbiEmolumento = {};
Europa.Controllers.PreProposta.ItbiEmolumento.IdPreProposta = undefined;
Europa.Controllers.PreProposta.ItbiEmolumento.UrlListar = undefined;
Europa.Controllers.PreProposta.ItbiEmolumento.UrlIncluir = undefined;
Europa.Controllers.PreProposta.ItbiEmolumento.UrlAlterar = undefined;
Europa.Controllers.PreProposta.ItbiEmolumento.UrlExcluir = undefined;
Europa.Controllers.PreProposta.ItbiEmolumento.DropDownTipoParcela = undefined;

Europa.Controllers.PreProposta.ItbiEmolumento.Init = function () {
    // Herdando configurações já efetuadas, já que este método só é executado após a página ser carregada.
    // Se não fizer isso, sobrescrevo todas as propriedades setadas anteriormente (geralmente URLs e permissÕes)
    var self = Europa.Controllers.PreProposta.ItbiEmolumento;

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
                    idPreProposta: Europa.Controllers.PreProposta.ItbiEmolumento.IdPreProposta,
                    idPlanoPagamento: objetoLinhaTabela.Id
                };

                $.post(Europa.Controllers.PreProposta.ItbiEmolumento.UrlExcluir, requestContent, function (res) {
                    if (res.Sucesso) {
                        Europa.Controllers.PreProposta.ItbiEmolumento.Reload();
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
        $("#area_itbi_emolumento .has-error").removeClass("has-error");
        // Europa.Validator.ClearForm("#form_pontovenda");
        $.post(url, { idPreProposta: self.IdPreProposta, planoPagamento: obj }, function (res) {
            if (res.Sucesso) {
                self.Tabela.closeEdition();
                self.Reload();
                // FIXME: achar uma forma melhor de notificar os demais grids
                // O caminho é avisar o pai (pré-proposta) de determinada ação, e o PAI toma as ações necessárias
                Europa.Controllers.PreProposta.RefreshTotalFinanceiro();
                self.modoInclusao = true;
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

    Europa.Controllers.PreProposta.ItbiEmolumento = self;
};



////////////////////////////////////////////////////////////////////////////////////
// Funções Datatable
////////////////////////////////////////////////////////////////////////////////////
DataTableApp.controller('itbiEmolumentoTable', itbiEmolumentoTable);

function itbiEmolumentoTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.PreProposta.ItbiEmolumento.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.PreProposta.ItbiEmolumento.Tabela;
    tabelaWrapper.
        setColumns([
            DTColumnBuilder.newColumn('TipoParcela').withTitle(Europa.i18n.Messages.TipoParcela).withOption('width', '20%').withOption('type', 'enum-format-TipoParcela'),
            DTColumnBuilder.newColumn('NumeroParcelas').withTitle(Europa.i18n.Messages.NumeroParcelas).withOption('width', '15%'),
            DTColumnBuilder.newColumn('ValorParcela').withTitle(Europa.i18n.Messages.ValorParcela).withOption('width', '20%').renderWith(renderMoney).withClass('dt-body-right'),
            DTColumnBuilder.newColumn('Total').withTitle(Europa.i18n.Messages.Total).withOption('width', '20%').renderWith(renderMoney).withClass('dt-body-right'),
            DTColumnBuilder.newColumn('DataVencimento').withTitle(Europa.i18n.Messages.DataVencimento).withOption('width', '15%').withOption("type", "date-format-DD/MM/YYYY")
        ])
        .setTemplateEdit([
            Europa.Controllers.PreProposta.ItbiEmolumento.DropDownTipoParcela,
            '<input type="text" class="form-control" id="ItbiEmolumentoNumeroParcelasEdit" name="NumeroParcelas" maxlength="3" onblur="Europa.Controllers.PreProposta.ItbiEmolumento.AtualizarValorTotal()">',
            '<input type="text" class="form-control" id="ItbiEmolumentoValorParcelaEdit" name="ValorParcela" maxlength="10" style="text-align:right" onblur="Europa.Controllers.PreProposta.ItbiEmolumento.AtualizarValorTotal()">',
            '<input type="text" class="form-control" id="ItbiEmolumentoTotalEdit" name="Total" readonly="readonly" style="text-align:right">',
            '<input type="text" class="form-control" id="ItbiEmolumentoDataVencimentoEdit" name="DataVencimento" datepicker="datepicker">'
        ])
        .setActionSave(Europa.Controllers.PreProposta.ItbiEmolumento.PreSalvar)
        .setIdAreaHeader("itbi_emolumento_datatable_header")
        .setColActions(actionsHtml, '50px')
        .setAutoInit()
        .setDefaultOptions('POST', Europa.Controllers.PreProposta.ItbiEmolumento.UrlListar, Europa.Controllers.PreProposta.ItbiEmolumento.FilterParams);

    function actionsHtml(data, type, full, meta) {
        if (Europa.Controllers.PreProposta.Permissoes.DetalhamentoFinanceiroManual) {
            return '<div>' +
                $scope.renderButtonEdit(Europa.Controllers.PreProposta.ItbiEmolumento.Permissoes.Atualizar, "Editar", "fa fa-edit", "editar(" + meta.row + ")", full.Situacao) +
                $scope.renderButtonDelete(Europa.Controllers.PreProposta.ItbiEmolumento.Permissoes.Excluir, "Excluir", "fa fa-trash", "excluir(" + meta.row + ")", full.Situacao) +
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
            .addClass('btn btn-steel')
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
            .addClass('btn btn-steel')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.editar = function (row) {
        $scope.rowEdit(row);
        Europa.Controllers.PreProposta.ItbiEmolumento.AplicarMascarasEDatePicker();
        Europa.Controllers.PreProposta.ItbiEmolumento.modoInclusao = false;
    };

    $scope.excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.PreProposta.ItbiEmolumento.Tabela.getRowData(row);
        Europa.Controllers.PreProposta.ItbiEmolumento.Excluir(objetoLinhaTabela);
    };

    function renderMoney(data) {
        return Europa.String.FormatMoney(data);
    }
}

Europa.Controllers.PreProposta.ItbiEmolumento.FilterParams = function () {
    return {
        idPreProposta: Europa.Controllers.PreProposta.ItbiEmolumento.IdPreProposta
    };
};

Europa.Controllers.PreProposta.ItbiEmolumento.PreSalvar = function () {
    var objetoLinhaTabela = Europa.Controllers.PreProposta.ItbiEmolumento.Tabela.getDataRowEdit();

    objetoLinhaTabela.ValorParcela = objetoLinhaTabela.ValorParcela.replace(/\./g, '');
    objetoLinhaTabela.Total = objetoLinhaTabela.Total.replace(/\./g, '');

    Europa.Controllers.PreProposta.ItbiEmolumento.Salvar(objetoLinhaTabela);
};

Europa.Controllers.PreProposta.ItbiEmolumento.AplicarMascarasEDatePicker = function () {
    var valorParcela = $("#ItbiEmolumentoValorParcelaEdit").val();
    valorParcela = valorParcela.replace(/\./g, '').replace(',', '.');
    var valorParcelaDecimal = parseFloat(Math.round(valorParcela * 100) / 100).toFixed(2);
    Europa.Mask.Dinheiro("#ItbiEmolumentoValorParcelaEdit");
    $("#ItbiEmolumentoValorParcelaEdit").val(valorParcelaDecimal.replace('.', ','));
    Europa.Controllers.PreProposta.ItbiEmolumento.AtualizarValorTotal();
    $("#ItbiEmolumentoNumeroParcelasEdit").mask("000");

    var date = Europa.String.FormatAsGeenDate($("#ItbiEmolumentoDataVencimentoEdit").val());
    Europa.Components.DatePicker.AutoApply();
    Europa.Controllers.PreProposta.ItbiEmolumento.DataVencimento = new Europa.Components.DatePicker()
        .WithTarget("#ItbiEmolumentoDataVencimentoEdit")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate(Europa.Date.Now())
        .WithValue(date)
        .Configure();
};

Europa.Controllers.PreProposta.ItbiEmolumento.AtualizarValorTotal = function () {
    var valorParcela = $("#ItbiEmolumentoValorParcelaEdit").val();
    valorParcela = valorParcela.replace(/\./g, '').replace(',', '.');
    var numeroParcelas = $("#ItbiEmolumentoNumeroParcelasEdit").val();
    var valorTotal = valorParcela * numeroParcelas;
    var valorTotalDecimal = parseFloat(Math.round(valorTotal * 100) / 100).toFixed(2);
    $("#ItbiEmolumentoTotalEdit").val(valorTotalDecimal.replace('.', ','));
    Europa.Mask.Dinheiro("#ItbiEmolumentoTotalEdit");
};