
$(function () {
    $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
    $(".ng-scope").css("word-wrap", "break-word");
});

function PagamentoUnificadoTabela($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.PagamentoUnificado.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.PagamentoUnificado.Tabela;
    self.setColumns([
        DTColumnBuilder.newColumn('NomeEmpresaVenda').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption('width', '150px'),
        DTColumnBuilder.newColumn('Estado').withTitle(Europa.i18n.Messages.UF).withOption('width', '150px'),
        DTColumnBuilder.newColumn('CentralVenda').withTitle(Europa.i18n.Messages.Central).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeEmpreendimento').withTitle(Europa.i18n.Messages.Empreendimento).withOption('width', '250px'),
        DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption('width', '150px'),
        DTColumnBuilder.newColumn('CodigoProposta').withTitle(Europa.i18n.Messages.Proposta).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeCliente').withTitle(Europa.i18n.Messages.Cliente).withOption('width', '300px'),
        DTColumnBuilder.newColumn('PassoAtual').withTitle(Europa.i18n.Messages.Situacao).withOption('width', '150px'),
        DTColumnBuilder.newColumn('DataVenda').withTitle(Europa.i18n.Messages.DataVenda).withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
        DTColumnBuilder.newColumn('StatusRepasse').withTitle(Europa.i18n.Messages.StatusRepasse).withOption('width', '150px'),
        DTColumnBuilder.newColumn('StatusConformidade').withTitle(Europa.i18n.Messages.StatusConformidade).withOption('width', '150px').renderWith(renderBool),
        DTColumnBuilder.newColumn('SituacaoNotaFiscal').withTitle(Europa.i18n.Messages.SituacaoNotaFiscal).withOption("width", "150px").withOption('type', 'enum-format-SituacaoNotaFiscal'),
        DTColumnBuilder.newColumn('FaixaUmMeio').withTitle(Europa.i18n.Messages.FaixaPagamento).withOption('width', '150px').renderWith(renderFaixa),
        DTColumnBuilder.newColumn('Tipologia').withTitle(Europa.i18n.Messages.Percentual).withOption('width', '150px').renderWith(renderPorcentagem),
        DTColumnBuilder.newColumn('RegraPagamento').withTitle(Europa.i18n.Messages.PorcentagemComissao).withOption('width', '175px').renderWith(renderRegraPagamento),
        DTColumnBuilder.newColumn('DataComissao').withTitle(Europa.i18n.Messages.DataAptidao).withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
        DTColumnBuilder.newColumn('ValorVGV').withTitle(Europa.i18n.Messages.ValorSemPremiada).withOption('width', '200px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('Tipologia').withTitle(Europa.i18n.Messages.ValorPagar).withOption('width', '150px').renderWith(renderVolorAPagar),
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
        DTColumnBuilder.newColumn('StatusIntegracaoSap').withTitle(Europa.i18n.Messages.StatusIntegracaoSap).withOption("width", "150px").renderWith(renderStatusIntegracaoSap),
    ])
        .setAutoInit(false)
        .setDefaultOrder([[7, 'asc']])
        .setColActions(actionsHtml, '100px') 
        .setIdAreaHeader("datatable_header")
        .setDefaultOptions('POST', Europa.Controllers.PagamentoUnificado.UrlListarPagamentos, Europa.Controllers.PagamentoUnificado.Filtro);

    function actionsHtml(data, type, full, meta) {
        var button = '<div>';

        //Selecionar pagamentos paa gerar requisição
        button += $scope.renderButtonGerarRC(Europa.Controllers.PagamentoUnificado.Permissoes.GerarRequisicaoCompra, 'Selecionar Pagamento', 'fa fa-check', 'SelecionarPagamento(' + meta.row + ')', full.NumeroGerado, meta.row);

        //abrir modal para visualizar requisição
        button += $scope.renderButton(Europa.Controllers.PagamentoUnificado.Permissoes.Visualizar, 'Visualizar Requisição de Compra', 'fa fa-file-text-o', 'VisualizarRequisicaoCompra(' + meta.row + ')', false);

        //abrir modal co historico das requisições
        button += $scope.renderButtonVisualizarRC(Europa.Controllers.PagamentoUnificado.Permissoes.Visualizar, 'Visualizar RC', 'fa fa-eye', 'HistoricoRequisicaoCompra(' + meta.row + ')', full.NumeroGerado);

        //excluir pagamento
        var permissaoExcluir = Europa.Controllers.PagamentoUnificado.Permissoes.Excluir && full.PedidoSap != null;
        //console.log(full.PedidoSap);
        button += $scope.renderButton(permissaoExcluir, 'Apagar Pedido', 'fa fa-trash', 'Remover(' + meta.row + ')', false);

        button += '</div>';
        
        return button;
    }

    function renderStatusIntegracaoSap(data) {
        return data ? Europa.i18n.Enum.Resolve("StatusIntegracaoSap", data) : Europa.i18n.Enum.Resolve("StatusIntegracaoSap", 0);
    };

    $scope.renderButton = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission === false || hasPermission === 'false' || hasPermission === 'False' || situacao === true) {
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

    $scope.renderButtonGerarRC = function (hasPermission, title, icon, onClick, situacao, row) {
        var full = Europa.Controllers.PagamentoUnificado.Tabela.getRowData(row);
        console.log(full);
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
    };

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
    };

    function renderPorcentagem(data,type,full,meta) {
        var porcentagem = "";

        switch (data) {
            case 1:
                porcentagem = full.ComissaoPagarPNE;
                break;
            case 2:
                porcentagem = full.ComissaoPagarUmMeio
                break;
            default:
                porcentagem = full.ComissaoPagarDois;
                break;
        }

        if (porcentagem) {
            var valor = "%";
            return valor = porcentagem.toString().replace(".", ",") + valor;
        }
        return "";
    };

    function renderMoney(data) {
        if (!data) {
            return "R$ 0,00";
        }

        return "R$ " + data.toString().replace(".", ",").replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
    };

    function renderRegra(data, type, full, meta) {
        var link = '<div>';
        if (data) {
            link = link + "<a title='Regra de Comissão' target='_blank' href='" + Europa.Controllers.PagamentoUnificado.UrlRegraComissao + '?regra=' + full.IdRegraComissao + "'>" + full.CodigoRegraComissao + "</a>";
        }
        link += '</div>';
        return link;
    };

    function renderBool(data) {
        if (data) {
            return Europa.i18n.Messages.Sim;
        }
        return Europa.i18n.Messages.Nao;
    };

    function renderEmReversao(data, type, full, meta) {
        if (data) {
            return Europa.i18n.Messages.Cancelado;
        }
        if (full.PassoAtual == "Prop. Cancelada") {
            return Europa.i18n.Messages.Cancelado;
        }
        return " ";
    };

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
    };

    function renderVolorAPagar(data, type, full, meta) {

        var valor = 0;

        switch (data) {
            case 1:
                valor = full.ValorVGV * full.ComissaoPagarPNE / 100;
                break;
            case 2:
                valor = full.ValorVGV * full.ComissaoPagarUmMeio / 100;
                break;
            default:
                valor = full.ValorVGV * full.ComissaoPagarDois / 100;
                break;
        }

        return "R$ " + Europa.String.FormatMoney(valor);
    };    

    function getValorAPagar(data, type, full, meta) {
        var valor = 0;

        switch (data) {
            case 1:
                valor = full.ValorVGV * full.ComissaoPagarPNE / 100;
                break;
            case 2:
                valor = full.ValorVGV * full.ComissaoPagarUmMeio / 100;
                break;
            default:
                valor = full.ValorVGV * full.ComissaoPagarDois / 100;
                break;
        }

        return valor;
    };

    $scope.SelecionarPagamento = function (idxRow) {
        //selecionando a linha
        var tabela = Europa.Controllers.PagamentoUnificado.Tabela.vm.dtInstance.DataTable;
        var row = tabela.row(idxRow);
        var tr = row.node();
        Europa.Controllers.PagamentoUnificado.GetRow = tr;

        //selecionando pagamento
        var obj = Europa.Controllers.PagamentoUnificado.Tabela.getRowData(idxRow);

        var valorAPagar = getValorAPagar(obj.Tipologia, null, obj, null);
        obj.ValorAPagar = valorAPagar.toString().replace('.', ',');

        obj.ValorVGV = obj.ValorVGV.toString().replace('.', ',');

        Europa.Controllers.PagamentoUnificado.GetDataRow = obj;

        var lista = Europa.Controllers.PagamentoUnificado.ListaPagamentos;

        if (Europa.Controllers.PagamentoUnificado.ListaPagamentos.filter(x => (x.Id == obj.Id)).length != 0) {
            $(Europa.Controllers.PagamentoUnificado.GetRow).removeClass('selected');
            Europa.Controllers.PagamentoUnificado.ListaPagamentos.pop(obj);
        } else {
            Europa.Controllers.PagamentoUnificado.ListaPagamentos.push(obj);
            $(Europa.Controllers.PagamentoUnificado.GetRow).addClass('selected');
        }

    };

    $scope.VisualizarRequisicaoCompra = function (idxRow) {

        var tabela = Europa.Controllers.PagamentoUnificado.Tabela.vm.dtInstance.DataTable;
        var row = tabela.row(idxRow);
        var tr = row.node();
        Europa.Controllers.PagamentoUnificado.GetRow = tr;


        var obj = Europa.Controllers.PagamentoUnificado.Tabela.getRowData(idxRow);

        var valorAPagar = getValorAPagar(obj.Tipologia, null, obj, null);
        obj.ValorAPagar = valorAPagar;

        Europa.Controllers.PagamentoUnificado.GetDataRow = obj;

        Europa.Controllers.PagamentoUnificado.AbrirModalGerarRequisicao(obj);
        
    };

    $scope.HistoricoRequisicaoCompra = function (row) {
        var obj = Europa.Controllers.PagamentoUnificado.Tabela.getRowData(row);
        Europa.Controllers.PagamentoUnificado.GetDataRow = obj;
        Europa.Controllers.PagamentoUnificado.AbrirModalHistoricoRequisicaoCompra();
        Europa.Controllers.PagamentoUnificado.FiltrarHistoricoRequisicaoCompra();
    };

    $scope.Remover = function (row) {
        var full = Europa.Controllers.PagamentoUnificado.Tabela.getRowData(row);

        var obj = {
            idPagamento: full.IdPagamento
        };

        Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.Confirmacao,
            "Deseja realmente excluir o registro {0}?".replace("{0}", full.PedidoSap),
            "",
            function () {
                $.post(Europa.Controllers.PagamentoUnificado.UrlRemoverPagamento, obj, function (res) {
                    if (res.Sucesso) {
                        Europa.Controllers.PagamentoUnificado.Filtrar();
                    }
                    Europa.Informacao.PosAcao(res);
                });
            }
        );
    };
            
}

DataTableApp.controller('TabelaPagamentoUnificado', PagamentoUnificadoTabela);

Europa.Controllers.PagamentoUnificado.Filtrar = function () {
    var autorizar = Europa.Controllers.PagamentoUnificado.ValidarFiltro();
    if (!autorizar) {
        return;
    }

    Europa.Controllers.PagamentoUnificado.Tabela.reloadData();
};

Europa.Controllers.PagamentoUnificado.GerarRequisicaoCompraEmLote = function () {
    if (Europa.Controllers.PagamentoUnificado.ListaPagamentos.length < 1) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.i18n.Messages.NenhumRegistroValidoSelecionado);
        Europa.Informacao.Show();
        return null;
    };

    $("#gerar-requisicao-compra").prop("disabled", true);

    var pagamentos = {
        pagamentos: Europa.Controllers.PagamentoUnificado.ListaPagamentos
    };

    $.post(Europa.Controllers.PagamentoUnificado.UrlGerarRequisicaoCompraEmLote, pagamentos, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.PagamentoUnificado.ShowStatusRequisicaoCompra(res.Task);

            Europa.Controllers.PagamentoUnificado.ListaPagamentos = [];
            $("#gerar-requisicao-compra").prop("disabled", false);
            $("#PagamentoUnificado_Download").addClass("hidden");
        } else {
            Europa.Informacao.PosAcao(res);
        }

    });
};

Europa.Controllers.PagamentoUnificado.ExportarPagina = function () {
    var autorizar = Europa.Controllers.PagamentoUnificado.ValidarFiltro();
    if (!autorizar) {
        return;
    }

    Europa.Controllers.PagamentoUnificado.Filtrar();

    var params = Europa.Controllers.PagamentoUnificado.Tabela.lastRequestParams;
    
    var formExportar = $("#form_exportar");
    $("#form_exportar").find("input").remove();
   
    formExportar.attr("method", "post").attr("action", Europa.Controllers.PagamentoUnificado.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.PagamentoUnificado.ExportarTodos = function () {
    var autorizar = Europa.Controllers.PagamentoUnificado.ValidarFiltro();
    if (!autorizar) {
        return;
    }

    Europa.Controllers.PagamentoUnificado.Filtrar();

    var params = Europa.Controllers.PagamentoUnificado.Tabela.lastRequestParams;

    var formExportar = $("#form_exportar");
    $("#form_exportar").find("input").remove();

    formExportar.attr("method", "post").attr("action", Europa.Controllers.PagamentoUnificado.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};



