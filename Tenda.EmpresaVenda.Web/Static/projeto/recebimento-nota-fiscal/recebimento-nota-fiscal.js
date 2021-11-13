Europa.Controllers.RecebimentoNF = {};

Europa.Controllers.RecebimentoNF.Modal = {};
Europa.Controllers.RecebimentoNF.Modal.Id = "#avaliar_nota_fiscal";
Europa.Controllers.RecebimentoNF.CurrentRowData = undefined;
var collapsedGroups = {};

$(function () {
    $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
    $(".ng-scope").css("word-wrap", "break-word");
    $('#situacao_nota_fiscal').val(["2", "4"]).trigger('change');

    Europa.Controllers.RecebimentoNF.InitAutoComplete();
    Europa.Controllers.RecebimentoNF.InitDatePicker();

    $("#situacao_nota_fiscal").select2({
        trags: true,
        width: '100%'
    });

    $("#estado").select2({
        trags: true,
        width: '100%'
    });
});

Europa.Controllers.RecebimentoNF.InitAutoComplete = function () {
    Europa.Controllers.RecebimentoNF.AutoCompleteEmpresaVenda = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("empresa_venda")
        .Configure();
    Europa.Controllers.RecebimentoNF.AutoCompleteRegionais = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regionais")
        .Configure();
    console.log(this.param);
    Europa.Controllers.RecebimentoNF.AutoCompleteEmpresaVenda.Data = function (params) {
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
                        return $("#estado").val();
                    },
                    column: 'estado',
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
    Europa.Controllers.RecebimentoNF.AutoCompleteEmpresaVenda.Configure();
};

Europa.Controllers.RecebimentoNF.InitDatePicker = function () {
    Europa.Controllers.RecebimentoNF.DateInicio = new Europa.Components.DatePicker()
        .WithTarget("#data_inicio")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

    Europa.Controllers.RecebimentoNF.DataTermino = new Europa.Components.DatePicker()
        .WithTarget("#data_termino")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

    Europa.Controllers.RecebimentoNF.DatePrevisaoPagamentoInicio = new Europa.Components.DatePicker()
        .WithTarget("#data_previsao_pagamento_inicio")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

    Europa.Controllers.RecebimentoNF.DataPrevisaoPagamentoTermino = new Europa.Components.DatePicker()
        .WithTarget("#data_previsao_pagamento_termino")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();
};


DataTableApp.controller('RecebimentoNFDatatable', RecebimentoNFDatatable);

function RecebimentoNFDatatable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.RecebimentoNF.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.RecebimentoNF.Tabela;

    self.setColumns([
        DTColumnBuilder.newColumn('NomeFantasia').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption("width", "150px")
            .notSortable().renderWith(renderNomeFantasia),
        DTColumnBuilder.newColumn('CodigoProposta').withTitle(Europa.i18n.Messages.Proposta).withOption('width', '150px')
            .notSortable().renderWith(renderCodigoProposta),
        DTColumnBuilder.newColumn('RegraPagamento').withTitle(Europa.i18n.Messages.ParcelaPagamento)
            .withOption('width', '175px').renderWith(renderRegraPagamento).notSortable(),
        DTColumnBuilder.newColumn('DataComissao').withTitle(Europa.i18n.Messages.DataComissao).withOption("width", "150px").notSortable().renderWith(renderDataComissao),
        DTColumnBuilder.newColumn('Estado').withTitle(Europa.i18n.Messages.UF).withOption("width", "150px").notSortable().renderWith(renderEstado),
        DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption("width", "150px").notSortable().renderWith(renderRegional),
        DTColumnBuilder.newColumn('PedidoSap').withTitle(Europa.i18n.Messages.NumeroPedido).withOption("width", "175px"),
        DTColumnBuilder.newColumn('NotaFiscal').withTitle(Europa.i18n.Messages.NumeroNotaFiscalP).withOption('width', '150px').notSortable().renderWith(renderNotaFiscal),
        DTColumnBuilder.newColumn('RevisaoNF').withTitle(Europa.i18n.Messages.RevisaoNF).withOption('width', '150px').notSortable().renderWith(renderRevisaoNF),
        DTColumnBuilder.newColumn(null).withTitle("").withOption("width", "200px").notSortable().renderWith(buttonExibirNotaFiscal),
        DTColumnBuilder.newColumn(null).withTitle("").withOption("width", "200px").notSortable().renderWith(buttonNotaFiscal),
        DTColumnBuilder.newColumn('SituacaoNotaFiscal').withTitle(Europa.i18n.Messages.Situacao).withOption("width", "150px").notSortable().renderWith(renderSituacaoNF),
        DTColumnBuilder.newColumn('Motivo').withTitle(Europa.i18n.Messages.MotivoRecusa).withOption("width", "300px").notSortable().renderWith(renderMotivo),
        DTColumnBuilder.newColumn('DataPrevisaoPagamento').withTitle(Europa.i18n.Messages.PrevisaoPagamento).withOption('width', '200px').notSortable().renderWith(renderDataPrevisaoPagamento),
        DTColumnBuilder.newColumn('Pago').withTitle(Europa.i18n.Messages.Pago).withOption('width', '100px').notSortable().renderWith(renderPago)
    ])
        .setAutoInit()
        .setIdAreaHeader("nf_datatable_header")
        .setDefaultOrder([[7, 'asc']])
        .setColActions(actionsHtml, '90px')
        .setOptionsMultiSelect('POST', Europa.Controllers.RecebimentoNF.UrlListar, Europa.Controllers.RecebimentoNF.Filtro);

 
    function actionsHtml(data, type, full, meta) {
        if (full.Filhos == null || full.Filhos.length <= 1) {
            return "";
        }
        var html = '<div>'
        var button = $('<a />').addClass('btn-steel row-detail fa fa-chevron-down').attr('ng-click', 'onDetail(' + meta.row + ')');
        html += button.prop('outerHTML');
        html += '</div>';
        return html;
    }

    function buttonNotaFiscal(data, type, full, meta) {
        var rowData = full.Filhos[0];
        return '<div>' +
            $scope.renderButton("avaliarNotaFiscal(" + meta.row + ")", rowData) +
            '</div>';

    };

    function buttonExibirNotaFiscal(data, type, full, meta) {
        var rowData = full.Filhos[0];
        return '<div>' +
            $scope.renderButtonExibirNotaFiscal("exibirNotaFiscal(" + meta.row + ")", rowData) +
            '</div>';
    }

    $scope.onDetail = function (rowIdx) {
        var tabela = Europa.Controllers.RecebimentoNF.Tabela.vm.dtInstance.DataTable;
        var row = tabela.row(rowIdx);
        var tr = row.node();
        if (row.child.isShown()) {
            row.child.hide();
            $(tr).removeClass('shown');
            $(tr).find('.row-detail').removeClass('fa-chevron-up');
            $(tr).find('.row-detail').addClass('fa-chevron-down');
        }
        else {
            row.child(format(row.data())).show();
            $(tr).addClass('shown');
            $(tr).find('.row-detail').removeClass('fa-chevron-down');
            $(tr).find('.row-detail').addClass('fa-chevron-up');
        }
    };

    function format(rowData) {
        var html = ''
        rowData.Filhos.forEach(function (item) {
            html += '<tr class="row-details">' +
                '<td style="width: 90px"></td>' +
                '<td style="width: 150px">' + item.NomeFantasia + '</td>' +
                '<td style="width: 150px">' + item.CodigoProposta + '</td>' +
                '<td style="width: 175px">' + regraPagamento(item.RegraPagamento, item.TipoPagamento) + '</td>' +
                '<td style="width: 150px">' + Europa.Date.toGeenDateFormat(item.DataComissao) + '</td>' +
                '<td colspan="7" style="width: 1400px"></td>' +
                '</tr>';
        })
        return html;
    }

    $scope.renderButtonExibirNotaFiscal = function (onClick, rowData) {
        var button = $('<a />');
        if (rowData.PassoAtual == "Prop. Cancelada" || rowData.EmReversao == true) {
            button.addClass('btn btn-default')
                .append("Ver dados NF")
                .attr('disabled', 'disabled');
        }
        else {
            button.addClass('btn btn-default')
                .append("Ver dados NF")
                .attr('onClick', onClick);
        }
        return button.prop('outerHTML');
    }

    $scope.renderButton = function (onClick, row) {
        var button = $('<a />')
        var data = row.SituacaoNotaFiscal;
        if (data == 1) {
            button.addClass('btn btn-default')
                .append("Nota Fiscal Faturada")
                .attr('disabled', 'disabled');
        }
        else if (data == 4 || data == 2 || data == 6) {
            button.addClass('btn btn-default')
                .append("Avaliar Nota Fiscal")
                .attr('onClick', onClick);
        }
        else if (data == 3) {
            button.addClass('btn btn-default')
                .append("Nota Fiscal Reprovada")
                .attr('disabled', 'disabled');
        }
        else if (data == 7) {
            button.addClass('btn btn-default')
                .append("Aguardando Envio Midas")
                .attr('disabled', 'disabled');
        }
        else {
            button.addClass('btn btn-default')
                .append("Pendente de Envio")
                .attr('disabled', 'disabled');
        }
        if (row.PassoAtual == "Prop. Cancelada" || row.EmReversao == true) {
            button.addClass('btn btn-default')
                .removeAttr('onClick', onClick)
                .attr('disabled', 'disabled');
        }
        return button.prop('outerHTML');
    };

    function regraPagamento(value, tipo) {
        if (tipo == 1) {
            return value + "% Kit Completo";
        }
        if (tipo == 2) {
            return value + "% Repasse";
        }
        if (tipo == 3) {
            return value + "% Conformidade";
        }
    }

    function renderNomeFantasia(data, type, full, meta) {
        return full.Filhos[0].NomeFantasia;
    }
    function renderRevisaoNF(data, type, full, meta) {
        return full.Filhos[0].RevisaoNF;
    }
    function renderCodigoProposta(data, type, full, meta) {
        if (full.Filhos != null && full.Filhos.length > 1) {
            return "Mais de um";
        }
        return full.Filhos[0].CodigoProposta;
    }

    function renderRegraPagamento(data, type, full, meta) {
        if (full.Filhos != null && full.Filhos.length > 1) {
            return "Mais de um";
        }
        return regraPagamento(full.Filhos[0].RegraPagamento, full.Filhos[0].TipoPagamento);
    }

    function renderDataComissao(data, type, full, meta) {
        if (full.Filhos != null && full.Filhos.length > 1) {
            return "Mais de um";
        }
        var rowData = full.Filhos[0];
        return rowData.DataComissao ? Europa.Date.toGeenDateFormat(rowData.DataComissao) : " ";
    }

    function renderNotaFiscal(data, type, full, meta) {
        return full.Filhos[0].NotaFiscal;
    }

    function renderSituacaoNF(data, type, full, meta) {
        var rowData = full.Filhos[0];
        if (rowData.PassoAtual == "Prop. Cancelada" || rowData.EmReversao == true) {
            return "Distratado";
        }
        else if (rowData.SituacaoNotaFiscal != 0) {
            return Europa.i18n.Enum.Resolve("SituacaoNotaFiscal", rowData.SituacaoNotaFiscal);
        }
        else {
            return "Pendente de Envio";
        }
    }

    function renderDataPrevisaoPagamento(data, type, full, meta) {
        var rowData = full.Filhos[0];
        return rowData.DataPrevisaoPagamento ? Europa.Date.toGeenDateFormat(rowData.DataPrevisaoPagamento) : " ";
    }

    function renderPago(data, type, full, meta) {
        var rowData = full.Filhos[0];
        return Europa.String.FormatBoolean(rowData.Pago);
    }

    function renderMotivo(data, type, full, meta) {
        return full.Filhos[0].Motivo;
    }

    function renderEstado(data, type, full, meta) {
        return full.Filhos[0].Estado;
    }
    function renderRegional(data, type, full, meta) {
        console.log(full.Filhos[0]);
        return full.Filhos[0].Regional;
    }
};

function exibirNotaFiscal(row) {
    var obj = Europa.Controllers.RecebimentoNF.Tabela.getRowData(row);
    Europa.Controllers.RecebimentoNF.CurrentRowData = obj;
    var params = {
        PedidoSap: obj.PedidoSap,
        IdEmpresaVenda: obj.IdEmpresaVenda,
        IdEmpreendimento: obj.Filhos[0].IdEmpreendimento
    }

    $.post(Europa.Controllers.RecebimentoNF.UrlExibirNotaFiscal, params, function (res) {
        if (res.Sucesso) {
            $("#dados_nota_fiscal").modal("show");
            $("#nota_fiscal").html(res.Objeto);
        } else {
            Europa.Informacao.PosAcao(res);
        }
    })
}

function detalharServicoPorCliente(tgt) {
    console.log($(tgt));
    if ($(tgt).parent().next().css("display") == "none") {
        $(tgt).parent().next().css("display", "block");
        tgt.innerHTML = "";
        tgt.innerHTML = '<img class="triangle" onclick="detalharServicoPorCliente(event.target.parentNode);" src="~/../Static/images/caret-up.png">Detalhes';
    } else {
        $(tgt).parent().next().css("display", "none");
        tgt.innerHTML = "";
        tgt.innerHTML = '<img class="triangle" onclick="detalharServicoPorCliente(event.target.parentNode);" src="~/../Static/images/caret-down.png">Detalhes';

    }
}


function avaliarNotaFiscal(row) {
    var rowData = Europa.Controllers.RecebimentoNF.Tabela.getRowData(row)
    Europa.Controllers.RecebimentoNF.CurrentRowData = rowData.Filhos[0];
    $("#situacao").val(rowData.Filhos[0].SituacaoNotaFiscal);
    $("#motivo").val("");
    $('#nova_situacao').val("").trigger('change');

    if (rowData.Filhos[0].SituacaoNotaFiscal == 2 || rowData.Filhos[0].SituacaoNotaFiscal == 6) {
        $("#nova_situacao option[value=7]").hide();
    } else {
        $("#nova_situacao option[value=7]").show();
    }

    $(Europa.Controllers.RecebimentoNF.Modal.Id).modal("show");
};

Europa.Controllers.RecebimentoNF.Filtro = function () {
    param = {
        DataInicio: $("#data_inicio").val(),
        DataTermino: $("#data_termino").val(),
        DataPrevisaoPagamentoInicio: $("#data_previsao_pagamento_inicio").val(),
        DataPrevisaoPagamentoTermino: $("#data_previsao_pagamento_termino").val(),
        IdsEmpresaVenda: $("#autocomplete_empresa_venda").val(),
        Situacoes: $("#situacao_nota_fiscal").val(),
        Estado: $("#estado").val(),
        Regionais: $("#autocomplete_regionais").val(),
        NumeroPedido: $("#numero_pedido").val(),
        NumeroNotaFiscal: $("#numero_nf").val(),
    };
    return param;
};

Europa.Controllers.RecebimentoNF.LimparFiltro = function () {
    $("#data_inicio").val(" ");
    $("#data_termino").val(" ");
    $("#data_previsao_pagamento_inicio").val(" ");
    $("#data_previsao_pagamento_termino").val(" ");
    Europa.Controllers.RecebimentoNF.AutoCompleteEmpresaVenda.Clean();
    $('#situacao_nota_fiscal').val(" ").trigger('change');
    $('#estado').val(" ").trigger('change');
    Europa.Controllers.RecebimentoNF.AutoCompleteRegionais.Clean();
    $("#numero_pedido").val("");
    $("#numero_nf").val("");
    Europa.Controllers.RecebimentoNF.Filtrar();
};

Europa.Controllers.RecebimentoNF.Filtrar = function () {
    Europa.Controllers.RecebimentoNF.Tabela.reloadData();
};


Europa.Controllers.RecebimentoNF.ExportarPagina = function () {
    var params = Europa.Controllers.RecebimentoNF.Filtro();
    params.order = Europa.Controllers.RecebimentoNF.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.RecebimentoNF.Tabela.lastRequestParams.draw;
    params.pageSize = Europa.Controllers.RecebimentoNF.Tabela.lastRequestParams.pageSize;
    params.start = Europa.Controllers.RecebimentoNF.Tabela.lastRequestParams.start;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.RecebimentoNF.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};


Europa.Controllers.RecebimentoNF.ExportarTodos = function () {
    var params = Europa.Controllers.RecebimentoNF.Filtro();
    params.order = Europa.Controllers.RecebimentoNF.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.RecebimentoNF.Tabela.lastRequestParams.draw;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.RecebimentoNF.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};


Europa.Controllers.RecebimentoNF.Download = function () {
    var obj = Europa.Controllers.RecebimentoNF.CurrentRowData;
    var param = {
        idArquivo: obj.IdArquivo,
        idNotaFiscalPagamento: obj.IdNotaFiscalPagamento,
        permissaoReceber: Europa.Controllers.RecebimentoNF.Permissoes.Receber
    };
    var formDownload = $("#form_nota_fiscal");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.RecebimentoNF.UrlDownload);
    formDownload.addHiddenInputData(param);
    formDownload.submit();
    Europa.Controllers.RecebimentoNF.Tabela.reloadData();
};

Europa.Controllers.RecebimentoNF.MudarSituacao = function () {
    var obj = Europa.Controllers.RecebimentoNF.CurrentRowData;
   
    var data = {
        IdEmpresaVenda: obj.IdEmpresaVenda,
        PedidoSap: obj.PedidoSap,
        IdNotaFiscalPagamento: obj.IdNotaFiscalPagamento,
        SituacaoNotaFiscal: $("#nova_situacao").val(),
        Motivo: $("#motivo").val()
    };
    $.post(Europa.Controllers.RecebimentoNF.UrlMudarSituacao, { model: data }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.RecebimentoNF.LimparErro();
            $(Europa.Controllers.RecebimentoNF.Modal.Id).modal("hide");
            $('#nova_situacao').val("").trigger('change');
            Europa.Controllers.RecebimentoNF.Filtrar();
        } else {
            Europa.Controllers.RecebimentoNF.AdicionarErro(res.Campos);
        }
        Europa.Informacao.PosAcao(res);
    });
}



Europa.Controllers.RecebimentoNF.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.RecebimentoNF.LimparErro = function () {
    $("[name='nova_situacao']").parent().removeClass("has-error");
};


Europa.Controllers.RecebimentoNF.OnChangeNovaSituacao = function (val) {
    console.log(val);
    if (val == 3) {
        $("#motivo").removeAttr("disabled");
    } else {
        $("#motivo").val("");
        $("#motivo").attr("disabled","disabled");
    }

    if (val != "") {
        $("#btn_avaliar_nf").removeAttr("disabled");
    } else {
        $("#btn_avaliar_nf").attr('disabled', 'disabled');
    }
};


Europa.Controllers.RecebimentoNF.DownloadEmGrupo = function () {
    var select = Europa.Controllers.RecebimentoNF.Tabela.getRowsSelect();
    var idsReg = [];
    select.forEach(function (item) {
        if (item.IdNotaFiscalPagamento) {
            idsReg.push(item.IdNotaFiscalPagamento);
        }    
    });

    if (idsReg.length < 1) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.i18n.Messages.NenhumRegistroValidoSelecionado);
        Europa.Informacao.Show();
        return null;
    };
    var param = {
        idsNotaFiscalPagamento: idsReg,
        permissaoReceber: Europa.Controllers.RecebimentoNF.Permissoes.Receber
    };
    var formDownload = $("#form_nota_fiscal");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.RecebimentoNF.UrlDownloadSelecionados);
    formDownload.addHiddenInputData(param);
    formDownload.submit();
    Europa.Controllers.RecebimentoNF.Tabela.reloadData();
};

Europa.Controllers.RecebimentoNF.DownloadNFEspelho = function () {
    var obj = Europa.Controllers.RecebimentoNF.CurrentRowData;
    var params = {
        PedidoSap: obj.PedidoSap,
        IdEmpresaVenda: obj.IdEmpresaVenda,
        IdEmpreendimento: obj.Filhos[0].IdEmpreendimento
    };
    var formDownload = $("#form_nota_fiscal");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.RecebimentoNF.UrlDownloadPdf);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
};

Europa.Controllers.RecebimentoNF.AbrirModalAtualizarNumeroNF = function () {
    var selecionado = Europa.Controllers.RecebimentoNF.Tabela.getRowsSelect();
    var situcao = selecionado[0].Filhos[0].SituacaoNotaFiscal;

    if (selecionado.length != 1) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, "Selecione apenas um registro para executar essa ação.");
        Europa.Informacao.Show();
        return null;
    };
    if (situcao != 2) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, "Não é possível trocar o numero da NF nessa etapa.");
        Europa.Informacao.Show();
        return null;
    };

    $("#numero_nota_fiscal").modal("show");
};

Europa.Controllers.RecebimentoNF.AtualizarNumeroNF = function () {
    var selecionado = Europa.Controllers.RecebimentoNF.Tabela.getRowsSelect();
    var idNf = selecionado[0].IdNotaFiscalPagamento;
    var numero = $("#NovoNotaFiscal").val();

    $.post(Europa.Controllers.RecebimentoNF.UrlAtualizarNumeroNF, { idNf, numero }, function (res) {
        if (res.Sucesso) {
            Europa.Informacao.PosAcao(res);
            Europa.Controllers.RecebimentoNF.Tabela.reloadData();
            $("#NovoNotaFiscal").val("");
            $("#numero_nota_fiscal").modal("hide");
        }
    });
};