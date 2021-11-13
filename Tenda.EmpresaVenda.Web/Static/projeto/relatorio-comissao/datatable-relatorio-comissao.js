Europa.Controllers.RelatorioComissao = {};
Europa.Controllers.RelatorioComissao.ExibindoInfo = false;


$(function () {
    $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
    $(".ng-scope").css("word-wrap", "break-word");

    Europa.Controllers.RelatorioComissao.AlternarExibicaoInformacoes();
    $("#situacao_pre_proposta").select2({
        trags: true,
        width: '100%'
    });

    $("#filtro_estado").select2({
        trags: true,
        width: '100%'
    });

    Europa.Controllers.RelatorioComissao.AutoCompleteRegionais = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regionais")
        .Configure();

    Europa.Controllers.RelatorioComissao.AutoCompleteEmpreendimento = new Europa.Components.AutoCompleteEmpreendimento()
        .WithTargetSuffix("empreendimento")
        .Configure();

    Europa.Controllers.RelatorioComissao.AutoCompleteEmpresaVenda = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("empresa_venda")
        .Configure();

    Europa.Controllers.RelatorioComissao.AutoCompletePontoVenda = new Europa.Components.AutoCompletePontoVenda()
        .WithTargetSuffix("ponto_venda")
        .Configure();

    Europa.Controllers.RelatorioComissao.AutoCompleteCorretor = new Europa.Components.AutoCompleteCorretor()
        .WithTargetSuffix("corretor")
        .Configure();

    Europa.Controllers.RelatorioComissao.AutoCompleteBreveLancamento = new Europa.Components.AutoCompleteBreveLancamento()
        .WithTargetSuffix("breve_lancamento")
        .Configure();

    Europa.Controllers.RelatorioComissao.AutoCompleteViabilizador = new Europa.Components.AutoCompleteViabilizador()
        .WithTargetSuffix("viabilizador")
        .Configure();

    Europa.Controllers.RelatorioComissao.AutoCompletePontoVenda.Disable();
});

Europa.Controllers.RelatorioComissao.OnChangeEmpresaFiltro = function () {
    var idEmp = Europa.Controllers.RelatorioComissao.AutoCompleteEmpresaVenda.Value();

    if (!idEmp) {
        Europa.Controllers.RelatorioComissao.AutoCompletePontoVenda.Disable();
        Europa.Controllers.RelatorioComissao.AutoCompletePontoVenda.Clean();
        $("#autocomplete_empresa_venda").val("");
        return;
    }

    Europa.Controllers.RelatorioComissao.AutoCompletePontoVenda.Enable();

    Europa.Controllers.RelatorioComissao.AutoCompletePontoVenda = new Europa.Components.AutoCompletePontoVenda()
        .WithTargetSuffix("ponto_venda");
    Europa.Controllers.RelatorioComissao.AutoCompletePontoVenda
        .Data = function (params) {
            var data = {
                start: 0,
                pageSize: 10,
                filter: [
                    {
                        value: params.term,
                        column: this.param,
                        regex: true
                    },
                    {
                        value: $("#autocomplete_empresa_venda").val(),
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
            if (this.paramCallback) {
                var object = this.paramCallback();
                for (var i in object) {
                    data.filter.push({
                        value: object[i],
                        column: i
                    })
                }
            }
            return data;
        };
    Europa.Controllers.RelatorioComissao.AutoCompletePontoVenda.Configure();
};

Europa.Controllers.RelatorioComissao.AlternarExibicaoInformacoes = function () {
    if (!Europa.Controllers.RelatorioComissao.ExibindoInfo) {
        $('.mais-filtros').each(function () {
            $(this).hide();
        });
        $('#info-hide-filtro').html(Europa.i18n.Messages.MaisFiltros);
    } else {
        $('.mais-filtros').each(function () {
            $(this).fadeIn('slow');
        });
        setTimeout(function () {
            $('#info-hide-filtro').html(Europa.i18n.Messages.MenosFiltros);
        }, 300);
    }
    Europa.Controllers.RelatorioComissao.ExibindoInfo = !Europa.Controllers.RelatorioComissao.ExibindoInfo;
};

function relatorioComissaoTabela($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.RelatorioComissao.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.RelatorioComissao.Tabela;
    self.setColumns([
        DTColumnBuilder.newColumn('Id').withTitle(Europa.i18n.Messages.Id).withClass("hidden", "hidden"),
        DTColumnBuilder.newColumn('Divisao').withTitle(Europa.i18n.Messages.Divisao).withOption('width', '100px'),
        DTColumnBuilder.newColumn('NomeEmpreendimento').withTitle(Europa.i18n.Messages.Empreendimento).withOption('width', '200px'),
        DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption('width', '100px'),
        DTColumnBuilder.newColumn('Estado').withTitle(Europa.i18n.Messages.UF).withOption('width', '100px'),
        DTColumnBuilder.newColumn('EmpresaDeVenda').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption('width', '200px'),
        DTColumnBuilder.newColumn('CentralVenda').withTitle(Europa.i18n.Messages.Central).withOption('width', '200px'),
        DTColumnBuilder.newColumn('NomePontoVenda').withTitle(Europa.i18n.Messages.PontoVenda).withOption('width', '200px'),
        DTColumnBuilder.newColumn('NomeCliente').withTitle(Europa.i18n.Messages.Cliente).withOption('width', '200px'),
        DTColumnBuilder.newColumn('DataSicaq').withTitle(Europa.i18n.Messages.DataSicaqValidade).withOption('width', '150px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('DataSicaqPreproposta').withTitle(Europa.i18n.Messages.DataSicaq).withOption('width', '150px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('StatusContrato').withTitle(Europa.i18n.Messages.StatusContrato).withOption('width', '200px'),
        DTColumnBuilder.newColumn('EmReversao').withTitle(Europa.i18n.Messages.Reversao).withOption('width', '200px').renderWith(renderBool),
        DTColumnBuilder.newColumn('StatusRepasse').withTitle(Europa.i18n.Messages.StatusRepasse).withOption('width', '200px'),
        DTColumnBuilder.newColumn('DataRepasse').withTitle(Europa.i18n.Messages.DataRepasse).withOption('width', '200px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('StatusConformidade').withTitle(Europa.i18n.Messages.StatusConformidade).withOption('width', '200px'),
        DTColumnBuilder.newColumn('DataConformidade').withTitle(Europa.i18n.Messages.DataConformidade).withOption('width', '200px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('PassoAtual').withTitle(Europa.i18n.Messages.StatusPRO).withOption('width', '200px'),
        DTColumnBuilder.newColumn('DataKitCompleto').withTitle(Europa.i18n.Messages.DataKitCompleto).withOption('width', '200px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('CodigoRegra').withTitle(Europa.i18n.Messages.CodigoRegra).withOption('width', '150px').renderWith(renderRegra),
        //DTColumnBuilder.newColumn('FaixaUmMeio').withTitle(Europa.i18n.Messages.ComissaoAcordadoUmMeio).withOption('width', '200px').renderWith(renderPorcentagem),
        //DTColumnBuilder.newColumn('FaixaDois').withTitle(Europa.i18n.Messages.ComissaoAcordadoDois).withOption('width', '200px').renderWith(renderPorcentagem),
        DTColumnBuilder.newColumn('Modalidade').withTitle(Europa.i18n.Messages.Comissao).withOption('width', '200px').renderWith(renderComissao),
        DTColumnBuilder.newColumn('Tipologia').withTitle(Europa.i18n.Messages.Faixa).withOption('width', '200px').renderWith(renderFaixa),
        DTColumnBuilder.newColumn('RegraPagamento').withTitle(Europa.i18n.Messages.RegraPagamento).withOption('width', '200px').renderWith(renderRegraPagamento),
        DTColumnBuilder.newColumn('Tipologia').withTitle(Europa.i18n.Messages.ComissaoPagar).withOption('width', '200px').renderWith(renderComissaoAPagar),
        //DTColumnBuilder.newColumn('ComissaoPagarUmMeio').withTitle(Europa.i18n.Messages.ComissaoPagarUmMeio).withOption('width', '200px').renderWith(renderPorcentagem),
        //DTColumnBuilder.newColumn('ComissaoPagarDois').withTitle(Europa.i18n.Messages.ComissaoPagarDois).withOption('width', '200px').renderWith(renderPorcentagem),
        DTColumnBuilder.newColumn('CodigoFornecedor').withTitle(Europa.i18n.Messages.CodigoFornecedor).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeFornecedor').withTitle(Europa.i18n.Messages.Fornecedor).withOption('width', '200px'),
        DTColumnBuilder.newColumn('NomeEmpresaVenda').withTitle(Europa.i18n.Messages.Empresa).withOption('width', '200px'),
        DTColumnBuilder.newColumn('DescricaoTorre').withTitle(Europa.i18n.Messages.Bloco).withOption('width', '100px'),
        DTColumnBuilder.newColumn('DescricaoUnidade').withTitle(Europa.i18n.Messages.Unidade).withOption('width', '100px'),
        DTColumnBuilder.newColumn('DataVenda').withTitle(Europa.i18n.Messages.DataVenda).withOption('width', '150px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('ValorVGV').withTitle(Europa.i18n.Messages.ValorSemPremiada).withOption('width', '200px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('CodigoPreProposta').withTitle(Europa.i18n.Messages.PreProposta).withOption('width', '200px'),
        DTColumnBuilder.newColumn('CodigoProposta').withTitle(Europa.i18n.Messages.Proposta).withOption('width', '200px'),
        DTColumnBuilder.newColumn('DescricaoTipologia').withTitle(Europa.i18n.Messages.Tipologia).withOption('width', '200px'),
        DTColumnBuilder.newColumn('DataRegistro').withTitle(Europa.i18n.Messages.DataRegistro).withOption('width', '200px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('Pago').withTitle(Europa.i18n.Messages.Pago).withOption('width', '200px').renderWith(renderBool),
        DTColumnBuilder.newColumn('DataPagamento').withTitle(Europa.i18n.Messages.DataPedido).withOption('width', '200px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('Faturado').withTitle(Europa.i18n.Messages.Faturado).withOption('width', '175px').renderWith(renderBool),
        DTColumnBuilder.newColumn('Observacao').withTitle(Europa.i18n.Messages.Observacao).withOption('width', '200px'),

    ])
    .setAutoInit(false)
    .setIdAreaHeader("datatable_header")
        .setDefaultOptions('POST', Europa.Controllers.RelatorioComissao.UrlListar, Europa.Controllers.RelatorioComissao.Filtro);

    function renderMoney(data) {
        if (data) {
            var valor = "R$ ";
            valor = valor + data.toFixed(2).replace(".", ",").replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
            return valor;
        }
        return "";
    }

    function renderPorcentagem(data) {
        if (data) {
            var valor = "%";
            return valor = data + valor;
        }
        return "";  
    }

    function renderRegra(data, type, full, meta) {
        var link = '<div>';
        if (data) {
            link = link + "<a title='Regra de Comissão' target='_blank' href='" + Europa.Controllers.RelatorioComissao.UrlRegraComissao + '?regra=' + full.IdRegraComissao + "'>" + full.CodigoRegra + "</a>";
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

    function renderRegraPagamento(data, type, full, meta) {
        //console.log(full)
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

    function renderFaixa(data, type, full, meta) {
        if (data > 0) {
            return Europa.i18n.Enum.Resolve("Tipologia", data);
        }

        if (full.FlagFaixaUmMeio) {
            return Europa.i18n.Enum.Resolve("Tipologia", 2);
        }

        return Europa.i18n.Enum.Resolve("Tipologia", 3);
    }

    function renderComissao(data, type, full, meta) {
        
        if (full.Modalidade == 1) {
            if (full.FlagFaixaUmMeio) {
                return full.FaixaUmMeio + "%";
            }
            return full.FaixaDois + "%";
        }
        return full.Faixa + "%";
    }

    function renderComissaoAPagar(data, type, full, meta) {
        if (data > 0) {
            switch (data) {
                case 1:
                    return full.ComissaoPagarPNE + "%";
                    break;
                case 2:
                    return full.ComissaoPagarUmMeio + "%";
                    break;
                case 3:
                    return full.ComissaoPagarDois + "%";
                    break;
            }            
        }

        if (full.FlagFaixaUmMeio) {
            return full.ComissaoPagarUmMeio + "%";
        }
        return full.ComissaoPagarDois + "%";
    }
}

DataTableApp.controller('TabelaComissao', relatorioComissaoTabela);

Europa.Controllers.RelatorioComissao.Filtrar = function () {
    Europa.Controllers.RelatorioComissao.Tabela.reloadData();
};

Europa.Controllers.RelatorioComissao.Filtro = function () {
    var param = {
        IdEmpreendimento: $("#autocomplete_empreendimento").val(),
        CodigoPreProposta: $("#filtro_codigo_preproposta").val(),          
        CodigoProposta: $("#filtro_codigo_proposta").val(),
        Estados: $("#filtro_estado").val(),
        Regionais: $("#autocomplete_regionais").val(),
        NomeFornecedor: $("#filtro_nome_fornecedor").val(),
        CodigoFornecedor: $("#filtro_codigo_fornecedor").val(),
        DataVendaDe: $("#filtro_data_venda_de").val(),
        DataVendaAte: $("#filtro_data_venda_ate").val(),
        NomeCliente: $("#filtro_cliente").val(),
        StatusContrato: $("#filtro_status_contrato").val(),
        IdEmpresaVenda: $("#autocomplete_empresa_venda").val(),
        PontosVenda: $("#autocomplete_ponto_venda").val(),
        Faturado: $("#filtro_faturado").val()
    };
    return param;
};

Europa.Controllers.RelatorioComissao.LimparFiltro = function () {
    $("#autocomplete_empreendimento").val("").trigger("change");
    $("#autocomplete_ponto_venda").val("").trigger("change");
    $("#autocomplete_empresa_venda").val("").trigger("change");
    $("#filtro_codigo_preproposta").val("");
    $("#filtro_codigo_proposta").val("");
    $("#filtro_estado").val("").trigger("change");
    $("#autocomplete_regionais").val("").trigger("change");
    $("#filtro_nome_fornecedor").val("");
    $("#filtro_codigo_fornecedor").val("");
    $("#filtro_data_venda_de").val("");
    $("#filtro_data_venda_ate").val("");
    $("#filtro_cliente").val("");
    $("#filtro_status_contrato").val("");
    $("#filtro_faturado").val(0).trigger("change");
    Europa.Controllers.RelatorioComissao.Filtrar();
};

Europa.Controllers.RelatorioComissao.ExportarTodos = function () {
    var params = Europa.Controllers.RelatorioComissao.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.RelatorioComissao.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.RelatorioComissao.ExportarPagina = function () {
    var params = Europa.Controllers.RelatorioComissao.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.RelatorioComissao.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.RelatorioComissao.ExportarRelatorioVendaUnificado = function () {
    var params = Europa.Controllers.RelatorioComissao.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.RelatorioComissao.UrlExportarRelatorioVendaUnificadoTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};