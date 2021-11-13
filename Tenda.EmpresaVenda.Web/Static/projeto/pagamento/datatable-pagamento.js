Europa.Controllers.Pagamento.IdProposta = 0;

$(function () {
    
});

function Datatable($scope, $attrs, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    
    var idEmpresaVenda = $attrs.id;
    const tabelaWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    tabelaWrapper
        .setTemplateEdit([
            '<input type="text" class="form-control" name="NomeLoja" id="NomeLoja" disabled="disabled">',
            '<input type="text" class="form-control" name="NomeEmpreendimento" id="NomeEmpreendimento" disabled="disabled">',
            '<input type="text" class="form-control" name="CodigoProposta" id="CodigoProposta" disabled="disabled">',
            '<input type="text" class="form-control" name="NomeCliente" id="NomeCliente" disabled="disabled">',
            '<input type="text" class="form-control" name="PassoAtual" id="PassoAtual" disabled="disabled">',
            '<input type="text" class="form-control" name="DataVenda" id="DataVenda" disabled="disabled">',
            '<input type="text" class="form-control" name="FaixaUmMeio" id="FaixaUmMeio" disabled="disabled">',
            '<input type="text" class="form-control" name="Percentual" id="Percentual" disabled="disabled">',
            '<input type="text" class="form-control" name="RegraPagamento" id="RegraPagamento" disabled="disabled">',
            '<input type="text" class="form-control" name="DataComissao" id="DataComissao" disabled="disabled">',
            '<input type="text" class="form-control" name="ValorSemPremiada" id="ValorSemPremiada" disabled="disabled">',
            '<input type="text" class="form-control" name="ValorAPagar" id="ValorAPagar" disabled="disabled">',
            '<input type="text" class="form-control" name="CodigoRegraComissao" id="CodigoRegraComissao" disabled="disabled">',
            '<input type="text" class="form-control" name="EmReversao" id="EmReversao" disabled="disabled">',
            '<input type="text" class="form-control" name="Pago" id="Pago" disabled="disabled">',
            '<input type="text" class="form-control" name="PedidoSap" id="PedidoSap">',
            '<input type="text" class="form-control hidden" name="IdPagamento" id="IdPagamento">',
            '<input type="text" class="form-control hidden" name="IdProposta" id="IdProposta">',
            '<input type="text" class="form-control hidden" name="TipoPagamento" id="TipoPagamento">',
            '<input type="text" class="form-control hidden" name="IdEmpresaVenda" id="IdEmpresaVenda">',
            '<input type="text" class="form-control hidden" name="Valor" id="Valor">'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('NomeLoja').withTitle(Europa.i18n.Messages.Central).withOption('width', '150px'),
            DTColumnBuilder.newColumn('Estado').withTitle(Europa.i18n.Messages.UF).withOption('width', '150px'),
            DTColumnBuilder.newColumn('NomeEmpreendimento').withTitle(Europa.i18n.Messages.Empreendimento).withOption('width', '150px'),
            DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption('width', '150px'),
            DTColumnBuilder.newColumn('CodigoProposta').withTitle(Europa.i18n.Messages.Proposta).withOption('width', '150px'),
            DTColumnBuilder.newColumn('NomeCliente').withTitle(Europa.i18n.Messages.Cliente).withOption('width', '200px'),
            DTColumnBuilder.newColumn('PassoAtual').withTitle(Europa.i18n.Messages.Situacao).withOption('width', '150px'),
            DTColumnBuilder.newColumn('DataVenda').withTitle(Europa.i18n.Messages.DataVenda).withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
            DTColumnBuilder.newColumn('StatusRepasse').withTitle(Europa.i18n.Messages.StatusRepasse).withOption('width', '150px'),
            DTColumnBuilder.newColumn('StatusConformidade').withTitle(Europa.i18n.Messages.StatusConformidade).withOption('width', '150px').renderWith(renderBool),
            DTColumnBuilder.newColumn('SituacaoNotaFiscal').withTitle(Europa.i18n.Messages.SituacaoNotaFiscal).withOption("width", "150px").renderWith(renderSituacaoNF),
            DTColumnBuilder.newColumn('FaixaUmMeio').withTitle(Europa.i18n.Messages.FaixaPagamento).withOption('width', '150px').renderWith(renderFaixa),
            DTColumnBuilder.newColumn('Percentual').withTitle(Europa.i18n.Messages.Percentual).withOption('width', '150px').renderWith(renderPorcentagem),
            DTColumnBuilder.newColumn('RegraPagamento').withTitle(Europa.i18n.Messages.PorcentagemComissao).withOption('width', '175px').renderWith(renderRegraPagamento),
            DTColumnBuilder.newColumn('DataComissao').withTitle(Europa.i18n.Messages.DataAptidao).withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
            DTColumnBuilder.newColumn('ValorSemPremiada').withTitle(Europa.i18n.Messages.ValorSemPremiada).withOption('width', '200px').renderWith(renderMoney),
            DTColumnBuilder.newColumn('ValorAPagar').withTitle(Europa.i18n.Messages.ValorPagar).withOption('width', '150px').renderWith(renderMoney),
            DTColumnBuilder.newColumn('CodigoRegraComissao').withTitle(Europa.i18n.Messages.CodigoRegra).withOption('width', '150px').renderWith(renderRegra),
            DTColumnBuilder.newColumn('EmReversao').withTitle(Europa.i18n.Messages.EmReversao).withOption('width', '150px').renderWith(renderBool),
            DTColumnBuilder.newColumn('ReciboCompra').withTitle(Europa.i18n.Messages.RC).withOption('width', '150px'),
            DTColumnBuilder.newColumn('DataRequisicaoCompra').withTitle(Europa.i18n.Messages.DataRC).withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
            //DTColumnBuilder.newColumn('ChamadoPedido').withTitle(Europa.i18n.Messages.ChamadoPedido).withOption('width', '150px'),
            DTColumnBuilder.newColumn('PedidoSap').withTitle(Europa.i18n.Messages.NumeroPedido).withOption('width', '150px'),
            DTColumnBuilder.newColumn('DataPedidoSap').withTitle(Europa.i18n.Messages.DataPedidoSap).withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
            DTColumnBuilder.newColumn('MIGO').withTitle(Europa.i18n.Messages.MIGO).withOption('width', '150px'),
            DTColumnBuilder.newColumn('MIRO').withTitle(Europa.i18n.Messages.MIRO).withOption('width', '150px'),
            DTColumnBuilder.newColumn('MIR7').withTitle(Europa.i18n.Messages.MIR7).withOption('width', '150px'),
            DTColumnBuilder.newColumn('NotaFiscal').withTitle(Europa.i18n.Messages.NotaFiscal).withOption('width', '150px'),
            DTColumnBuilder.newColumn('ChamadoPagamento').withTitle(Europa.i18n.Messages.ChamadoPgto).withOption('width', '150px'),
            DTColumnBuilder.newColumn('DataPrevisaoPagamento').withTitle(Europa.i18n.Messages.PrevisaoPgto).withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
            DTColumnBuilder.newColumn('Pago').withTitle(Europa.i18n.Messages.Pagamento).withOption('width', '150px').renderWith(renderBool),
            DTColumnBuilder.newColumn('EmReversao').withTitle(Europa.i18n.Messages.SituacaoPagamento).withOption("width", "150px").renderWith(renderEmReversao),
            DTColumnBuilder.newColumn('StatusIntegracaoSap').withTitle(Europa.i18n.Messages.StatusIntegracaoSap).withOption("width", "150px").renderWith(renderStatusIntegracaoSap), //withOption('type', 'enum-format-StatusIntegracaoSap'),
            DTColumnBuilder.newColumn('IdPagamento').withTitle("").withClass("hidden", "hidden"),
            DTColumnBuilder.newColumn('IdProposta').withTitle("").withClass("hidden", "hidden"),
            DTColumnBuilder.newColumn('TipoPagamento').withTitle("").withClass("hidden", "hidden"),
            DTColumnBuilder.newColumn('IdEmpresaVenda').withTitle("").withClass("hidden", "hidden"),
            DTColumnBuilder.newColumn('').withTitle("").withClass("hidden", "hidden"),

        ])
        .setColActions(actionsHtml, '100px') 
        .setIdAreaHeader("datatable_header_" + idEmpresaVenda)
        .setActionSave(SalvarPagamento)
        .setDefaultOptions('POST', Europa.Controllers.Pagamento.UrlListarTabela, filtrar);   

    function filtrar() {
        var form = {
            PeriodoDe: $("#DateInicioVigencia").val(),
            PeriodoAte: $("#DataTerminoVigencia").val(),
            IdEmpresaVenda: idEmpresaVenda,
            Regional: $("#filtro_regionais").val(),
            Estados: $("#filtro_estados").val(),
            TipoPagamento: $("#filtro_tipo_pagamento").val(),
            NomeCliente: $("#filtro_cliente").val(),
            CodigoProposta: $("#filtro_proposta").val(),
            DataVendaDe: $("#filtro_data_venda_inicio").val(),
            DataVendaAte: $("#filtro_data_venda_termino").val(),
            StatusIntegracaoSap: $("#filtro_status_sap").val(),
            Pago: $("#filtro_pago").val(),
            DataRcPedidoSapDe: $("#filtro_data_rc_pedido_de").val(),
            DataRcPedidoSapAte: $("#filtro_data_rc_pedido_ate").val(),
            DataRcPedidoSapDe: $("#filtro_data_rc_pedido_de").val(),
            DataRcPedidoSapAte: $("#filtro_data_rc_pedido_ate").val(),
            DataPrevisaoPagamentoInicio: $("#data_previsao_pagamento_inicio").val(),
            DataPrevisaoPagamentoTermino: $("#data_previsao_pagamento_termino").val(),
        };
        return form;
    }

    function actionsHtml(data, type, full, meta) {
        var button = '<div>';
                //selecionar pagamentos para gerar requisição em lote
                button += $scope.renderButtonGerarRC(Europa.Controllers.Pagamento.Permissoes.GerarRequisicaoCompra, 'Selecionar Pagamento', 'fa fa-check', 'SelecionarPagamento(' + meta.row + ')', full.NumeroGerado, meta.row);

                //abrir modal para visualizar requisição
                button += $scope.renderButton(Europa.Controllers.Pagamento.Permissoes.Visualizar, 'Visualizar Requisição de Compra', 'fa fa-file-text-o', 'GerarRequisicaoCompra(' + meta.row + ')', false);

                button += $scope.renderButtonVisualizarRC(Europa.Controllers.Pagamento.Permissoes.Visualizar, 'Visualizar RC', 'fa fa-eye', 'VisualizarRequisicaoCompra(' + meta.row + ')', full.NumeroGerado);

                var permissaoExcluir = Europa.Controllers.Pagamento.Permissoes.Excluir == "true" && full.PedidoSap != null;
        console.log(full.PedidoSap);

                button += $scope.renderButton(permissaoExcluir, 'Apagar Pedido', 'fa fa-trash', 'Remover(' + meta.row + ')', false);
            
        

        button += '</div>';
        return button;
    }

    function renderStatusIntegracaoSap(data) {
        return data ? Europa.i18n.Enum.Resolve("StatusIntegracaoSap", data) : Europa.i18n.Enum.Resolve("StatusIntegracaoSap", 0);
    }

    $scope.renderButton = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission === false || hasPermission === 'false' || hasPermission === 'False' || situacao === true) {
            return "";
        }
        icon = $('<i/>').addClass(icon);

        var button = $('<a />')
            .addClass('btn btn-default gerar-rc')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        

        return button.prop('outerHTML');
    }

    $scope.renderButtonGerarRC = function (hasPermission, title, icon, onClick, situacao, row) {
        var full = tabelaWrapper.getRowData(row);
        if (hasPermission === false || hasPermission === 'false' || hasPermission === 'False' || situacao === true || full.EmReversao == true || full.PassoAtual == "Prop. Cancelada") {
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

    $scope.renderButtonVisualizarRC = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission === false || hasPermission === 'false' || hasPermission === 'False' || situacao === false) {
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

    function renderFaixa(data, type, full, meta) {
        switch (full.Tipologia) {
            case 1:
                return "PNE";
                break;
            case 2:
                return "F 1,5";
                break;
            case 3:
                return "F 2,0";
                break;
        }

        if (data) {
            return "F 1,5";
        }

        return "F 2,0";
    }
    
    function renderPorcentagem(data) {
        if (data) {
            var valor = "%";
            return valor = data.toString().replace(".",",") + valor;
        }
        return "";
    }

    function renderMoney(data) {
        if (data) {
            return "R$ " + Europa.String.FormatMoney(data);
        }
        return "";
    }

    function renderRegra(data, type, full, meta) {
        var link = '<div>';
        if (data) {
            link = link + "<a title='Regra de Comissão' target='_blank' href='" + Europa.Controllers.Pagamento.UrlRegraComissao + '?regra=' + full.IdRegraComissao + "'>" + full.CodigoRegraComissao + "</a>";
        }
        link += '</div>';
        return link;
    }

    function renderBool(data) {
        if (data) {
            return Europa.i18n.Messages.Sim;
        }
        return Europa.i18n.Messages.Nao;
    }
    function renderEmReversao(data, type, full, meta) {
        if (data) {
            return Europa.i18n.Messages.Cancelado;
        }
        if (full.PassoAtual == "Prop. Cancelada") {
            return Europa.i18n.Messages.Cancelado;
        }
        return " ";
    }

    function renderRegraPagamento(data, type, full, meta) {
        if (full.TipoPagamento == 1) {
            return data + "% Kit Completo";
        }
        if (full.TipoPagamento == 2) {
            return data + "% Repasse";
        }
        if (full.TipoPagamento == 3) {
            return data + "% Conformidade";
        }
    }    

    $scope.PreencherPagamento = function (row) {
        var obj = tabelaWrapper.getRowData(row);
        $scope.rowEdit(row);

        $("#DataVenda").val(Europa.Date.toGeenDateFormat(obj.DataVenda));
        $("#FaixaUmMeio").val(renderFaixa(obj.FaixaUmMeio));
        $("#Percentual").val(renderPorcentagem(obj.Percentual));
        $("#RegraPagamento").val(renderRegraPagamento(obj.RegraPagamento,null,obj,null));
        $("#DataComissao").val(Europa.Date.toGeenDateFormat(obj.DataComissao));
        $("#ValorSemPremiada").val(renderMoney(obj.ValorSemPremiada));
        $("#ValorAPagar").val(renderMoney(obj.ValorAPagar));
        $("#CodigoRegraComissao").val(obj.CodigoRegraComissao);
        $("#EmReversao").val(renderBool( obj.EmReversao));
        $("#Pago").val(renderBool( obj.Pago));
        $("#DataPedido").val(Europa.Date.toGeenDateFormat(obj.DataPedido));
        $("#PedidoSap").val(obj.PedidoSap);

        $("#PedidoSap").focus();
        $("#PedidoSap").setCursorPosition(0);

        $("#Valor").val(obj.ValorAPagar);
    };

    function SalvarPagamento() {
        var valor = $("#Valor").val().toString().replace(".",",");
        
        var obj = {
            Id: $("#IdPagamento").val(),
            Proposta: { Id: $("#IdProposta").val() },
            TipoPagamento: $("#TipoPagamento").val(),
            ValorPagamento: valor,
            PedidoSap: $("#PedidoSap").val(),
            EmpresaVenda: { Id: $("#IdEmpresaVenda").val() }
        };

        $.post(Europa.Controllers.Pagamento.UrlSalvarPagamento, obj, function (res) {
            if (res.Sucesso) {
                tabelaWrapper.reloadData();
                Europa.Controllers.Pagamento.DesenharTabelas();
            }
            Europa.Informacao.PosAcao(res);
        });
    };

    $scope.Remover = function (row) {
        
        var full = tabelaWrapper.getRowData(row);

        console.log(full)

        var obj = {
            idPagamento: full.IdPagamento
        };

        Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.Confirmacao,
            "Deseja realmente excluir o registro {0}?".replace("{0}", full.PedidoSap),
            "",
            function () {
                $.post(Europa.Controllers.Pagamento.UrlRemoverPagamento, obj, function (res) {
                    if (res.Sucesso) {
                        tabelaWrapper.reloadData();
                    }
                    Europa.Informacao.PosAcao(res);
                });
            }
        );  
    };

    $scope.SelecionarPagamento = function (idxRow) {
        var tabela = tabelaWrapper.vm.dtInstance.DataTable;
        var row = tabela.row(idxRow);
        var tr = row.node();
        Europa.Controllers.Pagamento.GetRow = tr;

        var obj = tabelaWrapper.getRowData(idxRow);
        obj.ValorAPagar = obj.ValorAPagar.toString().replace('.', ',');
        obj.ValorSemPremiada = obj.ValorSemPremiada.toString().replace('.', ',');
        
        Europa.Controllers.Pagamento.GetDataRow = obj;

        if (Europa.Controllers.Pagamento.ListaPagamentos.filter(x => (x.Id == obj.Id)).length != 0) {
            $(Europa.Controllers.Pagamento.GetRow).removeClass('selected');
            Europa.Controllers.Pagamento.ListaPagamentos.pop(obj);
        } else {
            Europa.Controllers.Pagamento.ListaPagamentos.push(obj);
            $(Europa.Controllers.Pagamento.GetRow).addClass('selected');
        }
        
    }

    $scope.GerarRequisicaoCompra = function (idxRow) {
        var tabela = tabelaWrapper.vm.dtInstance.DataTable;
        var row = tabela.row(idxRow);
        var tr = row.node();
        Europa.Controllers.Pagamento.GetRow = tr;

       
        var obj = tabelaWrapper.getRowData(idxRow);
        Europa.Controllers.Pagamento.GetDataRow = obj;
        Europa.Controllers.Pagamento.AbrirModalGerarRequisicao(obj);
    };
    $scope.VisualizarRequisicaoCompra = function (row) {
        var obj = tabelaWrapper.getRowData(row);
        Europa.Controllers.Pagamento.GetDataRow = obj;
        Europa.Controllers.RC.AbrirModalVisualizarRequisicao();
        Europa.Controllers.RC.Filtro();
    }

    function renderSituacaoNF(data, type, full, meta) {
        if (full.PassoAtual == "Prop. Cancelada" || full.EmReversao == true) {
            return "Distratado";
        }
        return data ? Europa.i18n.Enum.Resolve("SituacaoNotaFiscal", data) : "Pendente de Envio";

    }
};

DataTableApp.controller('Datatable', Datatable);

Europa.Controllers.Pagamento.DesenharTabelas = function () {
    $("#pagamentos").html("");
    $("#StatusImportacao_Download").removeClass("hidden");
    $("#gerar-requisicao-compra").prop("disabled", false);
    Europa.Controllers.Pagamento.ListaPagamentoRC = [];
    Europa.Controllers.Pagamento.ListaPagamentos = [];
    var autorizar = Europa.Controllers.Pagamento.ValidarFiltro();
    if (!autorizar) {
        return;
    }

    var filtro = Europa.Controllers.Pagamento.Filtro();
    $.post(Europa.Controllers.Pagamento.UrlDesenharTabelas, filtro, function (res) {
        if (res.Sucesso) {
            EuropaCompileAngularControllers("#pagamentos", res.Objeto, function () {
                $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
                $(".ng-scope").css("word-wrap", "break-word");
            });
        }
    });
};

$.fn.setCursorPosition = function (pos) {
    this.each(function (index, elem) {
        if (elem.setSelectionRange) {
            elem.setSelectionRange(pos, pos);
        } else if (elem.createTextRange) {
            var range = elem.createTextRange();
            range.collapse(true);
            range.moveEnd('character', pos);
            range.moveStart('character', pos);
            range.select();
        }
    });
    return this;
};
