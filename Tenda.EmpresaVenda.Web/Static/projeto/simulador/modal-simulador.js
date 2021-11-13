$(function () {

})

Europa.Controllers.Simulador.AbrirModalSimulador = function () {
    $("#modal-simulacao").modal("show");
    setTimeout(function () { $(".modal-backdrop").remove() }, 300);
};
Europa.Controllers.Simulador.FecharModalSimulador = function () {
    $("#modal-simulacao").modal("hide");
    setTimeout(function () { $(".modal-backdrop").remove() }, 300);
};

//Detalhamento Financeiro
DataTableApp.controller('DetalhamentoFinanceiroModalSimulador', DetalhamentoFinanceiroModalSimulador);

function DetalhamentoFinanceiroModalSimulador($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.Simulador.TabelaDetalhamentoFinanceiro = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.Simulador.TabelaDetalhamentoFinanceiro;
    tabelaWrapper.
        setColumns([
            DTColumnBuilder.newColumn('TipoParcela').withTitle(Europa.i18n.Messages.TipoParcela).withOption('width', '20%').withOption('type', 'enum-format-TipoParcela'),
            DTColumnBuilder.newColumn('NumeroParcelas').withTitle(Europa.i18n.Messages.NumeroParcelas).withOption('width', '15%'),
            DTColumnBuilder.newColumn('ValorParcela').withTitle(Europa.i18n.Messages.ValorParcela).withOption('width', '20%').renderWith(renderMoney).withClass('dt-body-right'),
            DTColumnBuilder.newColumn('Total').withTitle(Europa.i18n.Messages.Total).withOption('width', '15%').renderWith(renderMoney).withClass('dt-body-right'),
            DTColumnBuilder.newColumn('DataVencimento').withTitle(Europa.i18n.Messages.DataVencimento).withOption('width', '15%').withOption("type", "date-format-DD/MM/YYYY")
        ])
        .setTemplateEdit([
            Europa.Controllers.Simulador.DropDownTipoParcelaDetalhamentoFinanceiro,
            '<input type="text" class="form-control" id="DetalhamentoFinanceiroNumeroParcelasEdit" name="NumeroParcelas" maxlength="3" onblur="Europa.Controllers.PreProposta.DetalhamentoFinanceiro.AtualizarValorTotal()">',
            '<input type="text" class="form-control dinheiro" id="DetalhamentoFinanceiroValorParcelaEdit" name="ValorParcela" maxlength="10" style="text-align:right" onblur="Europa.Controllers.PreProposta.DetalhamentoFinanceiro.AtualizarValorTotal()">',
            '<input type="text" class="form-control dinheiro" id="DetalhamentoFinanceiroTotalEdit" name="Total" readonly="readonly" style="text-align:right">',
            '<input type="text" class="form-control" id="DetalhamentoFinanceiroDataVencimentoEdit" name="DataVencimento" datepicker="datepicker">'
        ])
        .setActionSave(Europa.Controllers.Simulador.PreSalvarDetalhamentoFinanceiro)
        .setIdAreaHeader("detalhamento_financeiro_datatable_modal_header")
        .setColActions(actionsHtml, '70px')
        .setDefaultOptions('POST', Europa.Controllers.Simulador.UrlListarDetalhamentoFinanceiro, Europa.Controllers.Simulador.FiltroDetalhamentoFinanceiro);

    function actionsHtml(data, type, full, meta) {
        var div = '<div>';
        if (Europa.Controllers.Simulador.Permissoes.DetalhamentoFinanceiroManual) {
            div += $scope.renderButtonEdit(Europa.Controllers.Simulador.PermissoesDetalhamentoFinanceiro.Atualizar, "Editar", "fa fa-edit", "editar(" + meta.row + ")", full.Situacao);
            div += $scope.renderButtonDelete(Europa.Controllers.Simulador.PermissoesDetalhamentoFinanceiro.Excluir, "Excluir", "fa fa-trash", "excluir(" + meta.row + ")", full.Situacao);
        }
        return div+='</div>';
    }

    $scope.renderButtonEdit = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 3 || !Europa.Controllers.Simulador.PodeManterAssociacoes()) {
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
        if (hasPermission !== 'true' || situacao === 3 || !Europa.Controllers.Simulador.PodeManterAssociacoes()) {
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

        var objeto = Europa.Controllers.Simulador.TabelaDetalhamentoFinanceiro.getRowData(row);
        var valorParcela = objeto.ValorParcela.toString().replace('.', ',');
        $("#DetalhamentoFinanceiroValorParcelaEdit").val(valorParcela);

        Europa.Controllers.Simulador.AtualizarValorTotalDetalhamentoFinanceiro();

        var date = Europa.String.FormatAsGeenDate($("#DetalhamentoFinanceiroDataVencimentoEdit").val());
        Europa.Components.DatePicker.AutoApply();
        Europa.Controllers.Simulador.DataVencimentoDetalhamentoFinanceiro = new Europa.Components.DatePicker()
            .WithTarget("#DetalhamentoFinanceiroDataVencimentoEdit")
            .WithFormat("DD/MM/YYYY")
            .WithMinDate(Europa.Date.Now())
            .WithValue(date)
            .Configure();

        Europa.Controllers.Simulador.ModoInclusao = false;
    };

    $scope.excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.Simulador.TabelaDetalhamentoFinanceiro.getRowData(row);
        Europa.Controllers.Simulador.ExcluirDetalhamentoFinanceiro(objetoLinhaTabela);
    };

    function renderMoney(data) {
        return Europa.String.FormatMoney(data);
    }
}

Europa.Controllers.Simulador.FiltroDetalhamentoFinanceiro = function () {
    return {
        idPreProposta: $("#PreProposta_Id").val()
    };
};

Europa.Controllers.Simulador.NovoDetalhamentoFinanceiro = function () {
    Europa.Controllers.Simulador.ModoInclusao = true;
    Europa.Controllers.Simulador.TabelaDetalhamentoFinanceiro.createRowNewData();
    Europa.Controllers.Simulador.AplicarMascarasEDatePickerDetalhamentoFinanceiro();
};

Europa.Controllers.Simulador.AplicarMascarasEDatePickerDetalhamentoFinanceiro = function () {

    Europa.String.FormatAsMoney($('#DetalhamentoFinanceiroValorParcelaEdit').val());
    Europa.Mask.ApplyByClass("dinheiro", Europa.Mask.FORMAT_MONEY, undefined, true);

    Europa.Controllers.Simulador.AtualizarValorTotalDetalhamentoFinanceiro();
    $("#DetalhamentoFinanceiroNumeroParcelasEdit").mask("000");

    var date = Europa.String.FormatAsGeenDate($("#DetalhamentoFinanceiroDataVencimentoEdit").val());
    Europa.Components.DatePicker.AutoApply();
    Europa.Controllers.Simulador.DataVencimentoDetalhamentoFinanceiro = new Europa.Components.DatePicker()
        .WithTarget("#DetalhamentoFinanceiroDataVencimentoEdit")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate(Europa.Date.Now())
        .WithValue(date)
        .Configure();
};

Europa.Controllers.Simulador.AtualizarValorTotalDetalhamentoFinanceiro = function () {
    var valorParcela = $("#DetalhamentoFinanceiroValorParcelaEdit").val();
    valorParcela = valorParcela.replace(/\./g, '').replace(',', '.');
    var numeroParcelas = $("#DetalhamentoFinanceiroNumeroParcelasEdit").val();
    var valorTotal = valorParcela * numeroParcelas;
    var valorTotalDecimal = parseFloat(Math.round(valorTotal * 100) / 100).toFixed(2);
    $("#DetalhamentoFinanceiroTotalEdit").val(valorTotalDecimal.replace('.', ','));
    Europa.Mask.Dinheiro("#DetalhamentoFinanceiroTotalEdit");
};

Europa.Controllers.Simulador.PreSalvarDetalhamentoFinanceiro = function () {
    var objetoLinhaTabela = Europa.Controllers.Simulador.TabelaDetalhamentoFinanceiro.getDataRowEdit();
    Europa.Controllers.Simulador.AtualizarValorTotalDetalhamentoFinanceiro();
    objetoLinhaTabela.ValorParcela = objetoLinhaTabela.ValorParcela.replace(/\./g, '');
    objetoLinhaTabela.Total = objetoLinhaTabela.Total.replace(/\./g, '');

    Europa.Controllers.Simulador.SalvarDetalhamentoFinanceiro(objetoLinhaTabela);
};

Europa.Controllers.Simulador.SalvarDetalhamentoFinanceiro = function (obj) {
    var idPreProposta = $("#PreProposta_Id").val();
    var url = Europa.Controllers.Simulador.ModoInclusao ?
        Europa.Controllers.Simulador.UrlIncluirDetalhamentoFinanceiro : Europa.Controllers.Simulador.UrlAlterarDetalhamentoFinanceiro;
    $("#area_detalhamento_financeiro .has-error").removeClass("has-error");

    $.post(url, { idPreProposta: idPreProposta, planoPagamento: obj }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.Simulador.TabelaDetalhamentoFinanceiro.closeEdition();
            Europa.Controllers.Simulador.TabelaDetalhamentoFinanceiro.reloadData();
            Europa.Controllers.Simulador.ModoInclusao = true;

            Europa.Controllers.Simulador.AtualizarTotalFinanceiro();

        } else {
            Europa.Controllers.Simulador.AddError(res.Campos);
        }
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.Simulador.ExcluirDetalhamentoFinanceiro = function (objetoLinhaTabela) {
    Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, Europa.i18n.Enum.Resolve("TipoParcela", objetoLinhaTabela.TipoParcela),
        function () {
            var requestContent = {
                idPreProposta: $("#PreProposta_Id").val(),
                idPlanoPagamento: objetoLinhaTabela.Id
            };

            $.post(Europa.Controllers.Simulador.UrlExcluirDetalhamentoFinanceiro, requestContent, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.Simulador.TabelaDetalhamentoFinanceiro.reloadData();

                    Europa.Controllers.Simulador.AtualizarTotalFinanceiro();
                }
                Europa.Informacao.PosAcao(res);
            });
        });
};

//Itbi e emolumentos
DataTableApp.controller('ItbiEmolumentoModalSimulador', itbiEmolumentoModalSimulador);

function itbiEmolumentoModalSimulador($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.Simulador.TabelaItbiEmolumento = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.Simulador.TabelaItbiEmolumento;
    tabelaWrapper.
        setColumns([
            DTColumnBuilder.newColumn('TipoParcela').withTitle(Europa.i18n.Messages.TipoParcela).withOption('width', '20%').withOption('type', 'enum-format-TipoParcela'),
            DTColumnBuilder.newColumn('NumeroParcelas').withTitle(Europa.i18n.Messages.NumeroParcelas).withOption('width', '15%'),
            DTColumnBuilder.newColumn('ValorParcela').withTitle(Europa.i18n.Messages.ValorParcela).withOption('width', '20%').renderWith(renderMoney).withClass('dt-body-right'),
            DTColumnBuilder.newColumn('Total').withTitle(Europa.i18n.Messages.Total).withOption('width', '20%').renderWith(renderMoney).withClass('dt-body-right'),
            DTColumnBuilder.newColumn('DataVencimento').withTitle(Europa.i18n.Messages.DataVencimento).withOption('width', '15%').withOption("type", "date-format-DD/MM/YYYY")
        ])
        .setTemplateEdit([
            Europa.Controllers.Simulador.DropDownTipoParcelaItbiEmolumento,
            '<input type="text" class="form-control" id="ItbiEmolumentoNumeroParcelasEdit" name="NumeroParcelas" maxlength="3" onblur="Europa.Controllers.PreProposta.ItbiEmolumento.AtualizarValorTotal()">',
            '<input type="text" class="form-control" id="ItbiEmolumentoValorParcelaEdit" name="ValorParcela" maxlength="10" style="text-align:right" onblur="Europa.Controllers.PreProposta.ItbiEmolumento.AtualizarValorTotal()">',
            '<input type="text" class="form-control" id="ItbiEmolumentoTotalEdit" name="Total" readonly="readonly" style="text-align:right">',
            '<input type="text" class="form-control" id="ItbiEmolumentoDataVencimentoEdit" name="DataVencimento" datepicker="datepicker">'
        ])
        .setColActions(actionsHtml, '70px')
        .setActionSave(Europa.Controllers.Simulador.PreSalvarItbiEmolumento)
        .setIdAreaHeader("itbi_emolumento_datatable_modal_header")
        .setDefaultOptions('POST', Europa.Controllers.Simulador.UrlListarItbiEmolumento, Europa.Controllers.Simulador.FiltroItbiEmolumento);

    function actionsHtml(data, type, full, meta) {
        var div = '<div>';
        if (Europa.Controllers.Simulador.Permissoes.DetalhamentoFinanceiroManual) {
            div += $scope.renderButtonEdit(Europa.Controllers.Simulador.PermissoesItbiEmolumento.Atualizar, "Editar", "fa fa-edit", "editar(" + meta.row + ")", full.Situacao);
            div += $scope.renderButtonDelete(Europa.Controllers.Simulador.PermissoesItbiEmolumento.Excluir, "Excluir", "fa fa-trash", "excluir(" + meta.row + ")", full.Situacao);
        }
        return div += '</div>';
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
        Europa.Controllers.Simulador.AplicarMascarasEDatePickerItbiEmolumento();
        Europa.Controllers.Simulador.ModoInclusao = false;
    };

    $scope.excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.Simulador.TabelaItbiEmolumento.getRowData(row);
        Europa.Controllers.Simulador.ExcluirItbiEmolumento(objetoLinhaTabela);
    };

    function renderMoney(data) {
        return Europa.String.FormatMoney(data);
    }
}

Europa.Controllers.Simulador.FiltroItbiEmolumento = function () {
    return {
        idPreProposta: $("#PreProposta_Id").val()
    };
};

Europa.Controllers.Simulador.NovoItbiEmolumento = function () {
    Europa.Controllers.Simulador.ModoInclusao = true;
    Europa.Controllers.Simulador.TabelaItbiEmolumento.createRowNewData();
    Europa.Controllers.Simulador.AplicarMascarasEDatePickerItbiEmolumento();
};

Europa.Controllers.Simulador.PreSalvarItbiEmolumento = function () {
    var objetoLinhaTabela = Europa.Controllers.Simulador.TabelaItbiEmolumento.getDataRowEdit();

    objetoLinhaTabela.ValorParcela = objetoLinhaTabela.ValorParcela.replace(/\./g, '');
    objetoLinhaTabela.Total = objetoLinhaTabela.Total.replace(/\./g, '');

    Europa.Controllers.Simulador.SalvarItbiEmolumento(objetoLinhaTabela);
};

Europa.Controllers.Simulador.AplicarMascarasEDatePickerItbiEmolumento = function () {
    var valorParcela = $("#ItbiEmolumentoValorParcelaEdit").val();
    valorParcela = valorParcela.replace(/\./g, '').replace(',', '.');
    var valorParcelaDecimal = parseFloat(Math.round(valorParcela * 100) / 100).toFixed(2);
    Europa.Mask.Dinheiro("#ItbiEmolumentoValorParcelaEdit");
    $("#ItbiEmolumentoValorParcelaEdit").val(valorParcelaDecimal.replace('.', ','));
    Europa.Controllers.Simulador.AtualizarValorTotalItbiEmolumento();
    $("#ItbiEmolumentoNumeroParcelasEdit").mask("000");

    var date = Europa.String.FormatAsGeenDate($("#ItbiEmolumentoDataVencimentoEdit").val());
    Europa.Components.DatePicker.AutoApply();
    Europa.Controllers.Simulador.DataVencimentoItbiEmolumento = new Europa.Components.DatePicker()
        .WithTarget("#ItbiEmolumentoDataVencimentoEdit")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate(Europa.Date.Now())
        .WithValue(date)
        .Configure();
};

Europa.Controllers.Simulador.AtualizarValorTotalItbiEmolumento = function () {
    var valorParcela = $("#ItbiEmolumentoValorParcelaEdit").val();
    valorParcela = valorParcela.replace(/\./g, '').replace(',', '.');
    var numeroParcelas = $("#ItbiEmolumentoNumeroParcelasEdit").val();
    var valorTotal = valorParcela * numeroParcelas;
    var valorTotalDecimal = parseFloat(Math.round(valorTotal * 100) / 100).toFixed(2);
    $("#ItbiEmolumentoTotalEdit").val(valorTotalDecimal.replace('.', ','));
    Europa.Mask.Dinheiro("#ItbiEmolumentoTotalEdit");
};

Europa.Controllers.Simulador.SalvarItbiEmolumento = function (obj) {
    var idPreProposta = $("#PreProposta_Id").val();

    var url = Europa.Controllers.Simulador.ModoInclusao ?
        Europa.Controllers.Simulador.UrlIncluirItbiEmolumento : Europa.Controllers.Simulador.UrlAlterarItbiEmolumento;
    $("#area_itbi_emolumento .has-error").removeClass("has-error");

    $.post(url, { idPreProposta: idPreProposta, planoPagamento: obj }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.Simulador.TabelaItbiEmolumento.closeEdition();
            Europa.Controllers.Simulador.TabelaItbiEmolumento.reloadData();

            Europa.Controllers.Simulador.AtualizarTotalFinanceiro();

        } else {
            Europa.Controllers.Simulador.AddError(res.Campos);
        }
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.Simulador.ExcluirItbiEmolumento = function (objetoLinhaTabela) {
    Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, Europa.i18n.Enum.Resolve("TipoParcela", objetoLinhaTabela.TipoParcela),
        function () {
            var requestContent = {
                idPreProposta: $("#PreProposta_Id").val(),
                idPlanoPagamento: objetoLinhaTabela.Id
            };

            $.post(Europa.Controllers.Simulador.UrlExcluirItbiEmolumento, requestContent, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.Simulador.TabelaItbiEmolumento.reloadData();

                    Europa.Controllers.Simulador.AtualizarTotalFinanceiro();
                }
                Europa.Informacao.PosAcao(res);
            });
        });
};

//Simulador
DataTableApp.controller('ResultadoSimulacaoModalTable', resultadoSimulacaoModalTable);

function resultadoSimulacaoModalTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.Simulador.TabelaResultadoSimulacao = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.Simulador.TabelaResultadoSimulacao;
    tabelaWrapper.
        setColumns([
            DTColumnBuilder.newColumn('TipoParcela').withTitle(Europa.i18n.Messages.TipoParcela).withOption('width', '20%').withOption('type', 'enum-format-TipoParcela'),
            DTColumnBuilder.newColumn('NumeroParcelas').withTitle(Europa.i18n.Messages.NumeroParcelas).withOption('width', '15%'),
            DTColumnBuilder.newColumn('ValorParcela').withTitle(Europa.i18n.Messages.ValorParcela).withOption('width', '20%').renderWith(renderMoney).withClass('dt-body-right'),
            DTColumnBuilder.newColumn('Total').withTitle(Europa.i18n.Messages.Total).withOption('width', '20%').renderWith(renderMoney).withClass('dt-body-right'),
            DTColumnBuilder.newColumn('DataVencimento').withTitle(Europa.i18n.Messages.DataVencimento).withOption('width', '15%').renderWith(Europa.Date.toGeenDateFormat)// withOption("type", "date-format-DD/MM/YYYY")
        ])
        .setAutoInit(false)
        .setIdAreaHeader("resultado_simulacao_datatable_modal_header")
        .setDefaultOptions('POST', Europa.Controllers.Simulador.UrlDetalhamentoFinanceiroBySimulador, Europa.Controllers.Simulador.FiltroResultadoSimulacao);

    function renderMoney(data) {
        return Europa.String.FormatMoney(data);
    }

}

Europa.Controllers.Simulador.FiltroResultadoSimulacao = function () {
    var simulacoes = $("#simulacoes").val();

    if (simulacoes == "") {
        return;
    }

    var codigo = simulacoes.split('-')

    var parametro = {
        Codigo: codigo[0],
        Digito: codigo[1],
        CodigoPreProposta: $("#PreProposta_Codigo").val(),
        IdProposta: $("#PreProposta_Id").val()
    };

    return parametro;
}

//Ações
Europa.Controllers.Simulador.SimularPreProposta = function () {
    var idPreProposta = $("#PreProposta_Id").val();
    $.post(Europa.Controllers.Simulador.UrlMontarUrlSimulador,
        { idPreProposta: idPreProposta }, function (res) {
            console.log(res)
            if (res.Sucesso) {
                window.open(res.Objeto, "_blank");
                return;
            }
            Europa.Informacao.PosAcao(res);
        });
}

Europa.Controllers.Simulador.AtualizarSimulacoes = function () {
    var preProposta = $("#PreProposta_Codigo").val();
    $.post(Europa.Controllers.Simulador.UrlAtualizarResultadosSimulacao,
        { preProposta: preProposta }, function (res) {
            if (res.Sucesso) {
                var lista = res.Objeto;

                var options = '<option value="">Selecione...</option>';

                lista.forEach(function (obj) {
                    options += '<option value="' + obj.Codigo + "-" + obj.Digito + '">' + Europa.Date.toGeenDateFormat(obj.CriadoEm) + ' - ' + obj.Codigo + '-' + obj.Digito + ' - ' + obj.Produto + '</option>';
                });

                $("#simulacoes").html(options)
            }

            Europa.Informacao.PosAcao(res)
        });
}

Europa.Controllers.Simulador.OnChangeSimulacao = function () {
    if ($('#simulacoes').val() != "") {
        $("#btn-aplicar-simulacao").removeClass("hidden");
    } else {
        $("#btn-aplicar-simulacao").addClass("hidden")
    }
    Europa.Controllers.Simulador.TabelaResultadoSimulacao.reloadData();

};

Europa.Controllers.Simulador.AplicarDetalhamentoFinanceiro = function () {
    var parametro = Europa.Controllers.Simulador.FiltroResultadoSimulacao();
    parametro.Codigo = parseInt(parametro.Codigo);
    parametro.Digito = parseInt(parametro.Digito);

    $.post(Europa.Controllers.Simulador.UrlAplicarDetalhamentoFinanceiro, {
        parametro: Europa.Controllers.Simulador.FiltroResultadoSimulacao()
    }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.Simulador.TabelaDetalhamentoFinanceiro.reloadData();
            Europa.Controllers.Simulador.TabelaItbiEmolumento.reloadData();

            Europa.Controllers.Simulador.AtualizarTotalFinanceiro();

            Europa.Controllers.Simulador.FecharModalSimulador = function () {
                $("#modal-simulacao").modal("hide");
                location.reload();
            }
        }

        Europa.Informacao.PosAcao(res);
    });
}

Europa.Controllers.Simulador.AtualizarTotalFinanceiro = function () {

    var requestContent = {
        idPreProposta: $("#PreProposta_Id").val()
    };
    $.post(Europa.Controllers.Simulador.UrlAtualizarTotalFinanceiro, requestContent, function (res) {
        if (!res.Sucesso) {
            Europa.Informacao.PosAcao(res);
        }
    });
};