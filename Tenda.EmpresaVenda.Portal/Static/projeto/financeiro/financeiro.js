Europa.Controllers.Financeiro = {};
Europa.Controllers.Financeiro.Tabela = {};

Europa.Controllers.Financeiro.Modal = {};
Europa.Controllers.Financeiro.Modal.Id = "#documento_nota_fiscal_upload";
Europa.Controllers.Financeiro.Modal.Selector = undefined;
Europa.Controllers.Financeiro.Modal.IdFormUpload = "#form_upload_nota_fiscal";
Europa.Controllers.Financeiro.CurrentRowData = undefined;
var collapsedGroups = {};

$(function () {
    $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
    $(".ng-scope").css("word-wrap", "break-word");
    Europa.Controllers.Financeiro.InitMask();
    //Europa.Controllers.Financeiro.ExibirNotaFiscal()
    $("#situacao_nota_fiscal").val(0);

    var table = $('#DataTables_Table_0').DataTable();

    table.on('draw', function () {
        Europa.Controllers.Financeiro.Destaque();
    });

    setTimeout(function () { Europa.Controllers.Financeiro.ConfigDatePicker() },1000);
});




Europa.Controllers.Financeiro.ConfigDatePicker = function () {
    var de = new Date();
    Europa.Components.DatePicker.AutoApply();

    Europa.Controllers.Financeiro.DataComissaoAte = new Europa.Components.DatePicker()
        .WithTarget("#DataComissaoAte")
        .WithFormat("DD/MM/YYYY")
        .WithValue(de)
        .Configure();
    de.setMonth(de.getMonth() - 1);
    Europa.Controllers.Financeiro.DataComissaoDe = new Europa.Components.DatePicker()
        .WithTarget("#DataComissaoDe")
        .WithFormat("DD/MM/YYYY")
        .WithValue(de)
        .Configure();

    Europa.Controllers.Financeiro.DataPrevisaoAte = new Europa.Components.DatePicker()
        .WithTarget("#data_previsao_pagamento_inicio")
        .WithFormat("DD/MM/YYYY")
        .Configure();
    Europa.Controllers.Financeiro.DataPrevisaoDe = new Europa.Components.DatePicker()
        .WithTarget("#data_previsao_pagamento_termino")
        .WithFormat("DD/MM/YYYY")
        .Configure();

    Europa.Controllers.Financeiro.Tabela.reloadData();
};

Europa.Controllers.Financeiro.InitMask = function () {
    Europa.Mask.Int($("#NotaFiscal"));
}
DataTableApp.controller('FinanceiroDatatable', FinanceiroDatatable);

function FinanceiroDatatable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.Financeiro.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.Financeiro.Tabela;

    self.setColumns([
        DTColumnBuilder.newColumn('CodigoProposta').withTitle(Europa.i18n.Messages.Proposta).withOption('width', '150px').notSortable().renderWith(renderCodigoProposta),
        DTColumnBuilder.newColumn('NomeCliente').withTitle(Europa.i18n.Messages.Cliente).withOption('width', '250px').notSortable().renderWith(renderNomeCliente),
        DTColumnBuilder.newColumn('RegraPagamento').withTitle(Europa.i18n.Messages.ParcelaPagamento)
            .withOption('width', '175px').notSortable().renderWith(renderRegraPagamento),
        DTColumnBuilder.newColumn('PedidoSap').withTitle(Europa.i18n.Messages.NumeroPedido).withOption("width", "175px"),
        DTColumnBuilder.newColumn('NotaFiscal').withTitle(Europa.i18n.Messages.NotaFiscal).withOption('width', '200px').notSortable().renderWith(renderNotaFiscal),
        DTColumnBuilder.newColumn('').withTitle("").withOption("width", "200px").notSortable().renderWith(buttonNotaFiscal),
        DTColumnBuilder.newColumn('RevisaoNF').withTitle(Europa.i18n.Messages.RevisaoNF).withOption("width", "150px").notSortable().renderWith(renderRevisaoNF),
        DTColumnBuilder.newColumn('SituacaoNotaFiscal').withTitle(Europa.i18n.Messages.Situacao).withOption("width", "150px").notSortable().renderWith(renderSituacaoNF),
        DTColumnBuilder.newColumn('Motivo').withTitle(Europa.i18n.Messages.MotivoRecusa).withOption("width", "300px").notSortable().renderWith(renderMotivo),
        DTColumnBuilder.newColumn('DataPrevisaoPagamento').withTitle(Europa.i18n.Messages.PrevisaoPagamento).withOption('width', '200px').notSortable().renderWith(renderDataPrevisaoPagamento),
        DTColumnBuilder.newColumn('Pago').withTitle(Europa.i18n.Messages.Pago).withOption('width', '100px').notSortable().renderWith(renderPago),
    ])
        .setAutoInit()
        .setDefaultOrder([[4, 'asc']])
        .setColActions(actionsHtml, '90px')
        .setOptionsSelect('POST', Europa.Controllers.Financeiro.UrlListar, Europa.Controllers.Financeiro.Filtro);

    function actionsHtml(data, type, full, meta) {
        var html = '<div>'
        html += $scope.renderButtonHistorico(true, "", "fa fa-history", 'Europa.Controllers.Financeiro.AbrirModalHistorico(' + meta.row + ')');
        if (full.Filhos == null || full.Filhos.length <= 1) {

        } else {
            var button = $('<a />').addClass('btn-steel row-detail fa fa-chevron-down').attr('ng-click', 'onDetail(' + meta.row + ')');
            html += button.prop('outerHTML');
        }
        
        html += '</div>';
        return html;
    }
    function actionsHtml1(name, collapsed) {
        var html = '<div>'
        var button;
        if (collapsed) {
            button = $('<a />').addClass('btn-steel row-detail fa fa-chevron-up').attr('data-name', name);
        } else {
            button = $('<a />').addClass('btn-steel row-detail fa fa-chevron-down').attr('data-name', name);
        }

        html += button.prop('outerHTML');
        html += '</div>';
        return html;
    }

    $scope.renderButtonHistorico = function (hasPermission, title, icon, onClick) {
        if (!hasPermission) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-blue')
            .attr('title', title)
            .attr('onClick', onClick)
            .attr('style' , "margin: 5px")
            .append(icon);
        return button.prop('outerHTML');
    };


    function renderCodigoProposta(data, type, full, meta) {
        if (full.Filhos != null && full.Filhos.length > 1) {
            return "Mais de um";
        }
        return full.Filhos[0].CodigoProposta;
    }

    function renderNomeCliente(data, type, full, meta) {
        if (full.Filhos != null && full.Filhos.length > 1) {
            return "Mais de um";
        }
        return full.Filhos[0].NomeCliente;
    }

    function renderRegraPagamento(data, type, full, meta) {
        if (full.Filhos != null && full.Filhos.length > 1) {
            return "Mais de um";
        }
        return regraPagamento(full.Filhos[0].RegraPagamento, full.Filhos[0].TipoPagamento);
    }

    function renderNotaFiscal(data, type, full, meta) {
        var obj = full.Filhos[0];
        if (!obj.SituacaoNotaFiscal || obj.SituacaoNotaFiscal == 3) {
            return '<div>' +
                $scope.renderButtonEspelhoNF("downloadNFEspelho(" + meta.row + ")", obj.SituacaoNotaFiscal) +
                '</div>';
        } else {
            return obj.NotaFiscal;
        }
        
    }

    function renderSituacaoNF(data, type, full, meta) {
        var rowData = full.Filhos[0];
        if (rowData.EmReversao || rowData.PassoAtual == "Prop. Cancelada") {
            return "Distratado";
        }
        return rowData.SituacaoNotaFiscal ? Europa.i18n.Enum.Resolve("SituacaoNotaFiscal", rowData.SituacaoNotaFiscal) : "Pendente de Envio";

    }

    function renderDataPrevisaoPagamento(data, type, full, meta) {
        var rowData = full.Filhos[0];
        return rowData.DataPrevisaoPagamento ? Europa.Date.toGeenDateFormat(rowData.DataPrevisaoPagamento) : " ";
    }

    function renderRevisaoNF(data, type, full, meta) {
        var rowData = full.Filhos[0];
        return rowData.RevisaoNF;
    }
    function renderPago(data, type, full, meta) {
        var rowData = full.Filhos[0];
        return Europa.String.FormatBoolean(rowData.Pago);
    }

    function renderMotivo(data, type, full, meta) {
        return full.Filhos[0].Motivo;
    }

    $scope.onDetail = function (rowIdx) {
        var tabela = Europa.Controllers.Financeiro.Tabela.vm.dtInstance.DataTable;
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
    }

    function format(rowData) {
        var html = ''
        rowData.Filhos.forEach(function (item) {
            html += '<tr class="row-details">' +
                '<td style="width: 60px"></td>' +
                '<td style="width: 150px">' + item.CodigoProposta + '</td>' +
                '<td style="width: 350px">' + item.NomeCliente + '</td>' +
                '<td style="width: 175px">' + regraPagamento(item.RegraPagamento, item.TipoPagamento) + '</td>' +
                '<td colspan="8" style="width: 1275px"></td>' +
                '</tr>';
        })
        return html;
    }



    function buttonNotaFiscal(data, type, full, meta) {
        var rowData = full.Filhos[0];
        return '<div>' +
            $scope.renderButton("uploadNotaFiscal(" + meta.row + ")", rowData.SituacaoNotaFiscal, rowData.EmReversao, rowData.PassoAtual) +
            '</div>';

    };

    $scope.renderButton = function (onClick, data, emReversao, passoAtual) {
        var button = $('<a />');
        switch (data) {
            case 1:
                if (Europa.Controllers.Financeiro.EmFechamentoContabil) {
                    button.addClass('btn btn-em-fechamento-contabil')
                        .attr('disabled', 'disabled');
                } else {
                    button.addClass('btn btn-substituir')
                        .attr('disabled', 'disabled');
                }
                button.append("Nota fiscal faturada");
                break;
            case 2:
                if (Europa.Controllers.Financeiro.EmFechamentoContabil) {
                    button.addClass('btn btn-em-fechamento-contabil')
                        .attr('disabled', 'disabled');
                } else {
                    button.addClass('btn btn-substituir')
                        .attr('onClick', onClick);
                }
                button.append("Substituir nota fiscal")
                break;
            case 3:
                if (Europa.Controllers.Financeiro.EmFechamentoContabil) {
                    button.addClass('btn btn-em-fechamento-contabil-recusado')
                        .attr('disabled', 'disabled');
                } else {
                    button.addClass('btn btn-recusado')
                        .attr('onClick', onClick);
                }
                button.append("Nota recusada");
                break;
            case 4:                
                if (Europa.Controllers.Financeiro.EmFechamentoContabil) {
                    button.addClass('btn btn-em-fechamento-contabil')
                    .attr('disabled', 'disabled');
                } else {
                    button.addClass('btn btn-substituir')
                    .attr('disabled', 'disabled');
                }
                button.append("Nota fiscal em análise");
                break;
            case 6:
                if (Europa.Controllers.Financeiro.EmFechamentoContabil) {
                    button.addClass('btn btn-em-fechamento-contabil')
                        .attr('disabled', 'disabled');
                } else {
                    button.addClass('btn btn-substituir')
                        .attr('disabled', 'disabled');
                }
                button.append("Nota fiscal faturada");
                break;
            case 7:
                if (Europa.Controllers.Financeiro.EmFechamentoContabil) {
                    button.addClass('btn btn-em-fechamento-contabil')
                        .attr('disabled', 'disabled');
                } else {
                    button.addClass('btn btn-substituir')
                        .attr('disabled', 'disabled');
                }
                button.append("Nota fiscal em análise");
                break;

            default:
                if (Europa.Controllers.Financeiro.EmFechamentoContabil) {
                    button.addClass('btn btn-em-fechamento-contabil')
                        .attr('disabled', 'disabled');
                } else {
                    button.addClass('btn btn-anexar')
                        .attr('onClick', onClick);
                }
                button.append("Anexar nota fiscal");
                button.addClass("button-destaque");
                break;

        }

        if (emReversao == true || passoAtual == "Prop. Cancelada") {
            button.removeClass('btn btn-anexar');
            button.removeClass('btn btn-recusado')
                .removeAttr('onClick', onClick);

            button.addClass('btn btn-substituir')
                .attr('disabled', 'disabled');
            button.text("Distratado");

        }
        return button.prop('outerHTML');
    };

    $scope.renderButtonEspelhoNF = function (onClick, data) {
        var button = $('<a />');
        if (data == 0) {
            button.addClass('btn btn-purple-pendente')
                .append("Ver dados NF")
                .attr('onClick', onClick);
        }
        else {
            button.addClass('btn btn-purple')
                .append("Ver dados NF")
                .attr('onClick', onClick);
        }
        return button.prop('outerHTML');
    }

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

    
};

function downloadNFEspelho(row) {
    var rowData = Europa.Controllers.Financeiro.Tabela.getRowData(row)
    Europa.Controllers.Financeiro.CurrentRowData = rowData;
    //Europa.Controllers.Financeiro.ExibirDadosNotaFiscal();
    Europa.Controllers.Financeiro.ExibirNotaFiscal();
};

function uploadNotaFiscal(row) {
    var rowData = Europa.Controllers.Financeiro.Tabela.getRowData(row)
    Europa.Controllers.Financeiro.CurrentRowData = rowData.Filhos[0];
    $("#NotaFiscal").val("");
    $("#arquivo").val("");
    $(Europa.Controllers.Financeiro.Modal.Id).modal("show");
};

Europa.Controllers.Financeiro.Filtro = function () {
    param = {
        DataInicio: $("#DataComissaoDe").val(),
        DataTermino: $("#DataComissaoAte").val(),
        DataPrevisaoPagamentoInicio: $("#data_previsao_pagamento_inicio").val(),
        DataPrevisaoPagamentoTermino: $("#data_previsao_pagamento_termino").val(),
        Situacao: $("#situacao_nota_fiscal").val(),
        Faturado:1
    };
    return param;
};

Europa.Controllers.Financeiro.Filtrar = function () {

    if (Europa.Controllers.Financeiro.ValidarFiltro()) {
        Europa.Controllers.Financeiro.Tabela.reloadData();
        Europa.Controllers.Financeiro.BuscarTotal();
    }
};

Europa.Controllers.Financeiro.ValidarFiltro = function () {
    var msgs = [];

    var autorizar = true;

    if ($("#DataComissaoDe").val() == 0 ||
        $("#DataComissaoAte").val() == 0) {
        msgs.push("Insira um período");
        autorizar = false;
    }


    if (!autorizar) {
        var res = {
            Sucesso: false,
            Mensagens: msgs
        };

        Europa.Informacao.PosAcao(res);

        return false;
    }

    return true;
};
Europa.Controllers.Financeiro.Upload = function () {

    var obj = Europa.Controllers.Financeiro.CurrentRowData;
    var notaFiscal = $("#NotaFiscal").val();
    var arquivo = $(Europa.Controllers.Financeiro.Modal.IdFormUpload).find("#arquivo").get(0).files[0];

    var maxFileSize = 16;
    var maxFileSizeInBytes = maxFileSize * 1000000;
    if (arquivo !== undefined && arquivo.size > maxFileSizeInBytes) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.ArquivoTamanhoMaximoExcedido, maxFileSize));
        Europa.Informacao.Show();
        return;
    }
    var formData = new FormData();
    formData.append("IdPagamento", obj.IdPagamento);
    formData.append("IdNotaFiscalPagamento", obj.IdNotaFiscalPagamento);
    formData.append("PedidoSap", obj.PedidoSap);
    formData.append("File", arquivo);
    formData.append("NotaFiscal", notaFiscal);
    formData.append("DataPedidoSap", obj.DataPedidoSap);
    $.ajax({
        type: 'POST',
        url: Europa.Controllers.Financeiro.UrlUpload,
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
        success: function (res) {
            if (res.Sucesso) {
                Europa.Controllers.Financeiro.Tabela.reloadData();
                $(Europa.Controllers.Financeiro.Modal.Id).modal("hide");
                Europa.Growl.SuccessFromJsonResponse(res);
            } else {
                Europa.Informacao.PosAcao(res);
            }
        }
    });
};


Europa.Controllers.Financeiro.ExportarPagina = function () {
    if (Europa.Controllers.Financeiro.ValidarFiltro()) {
        var params = Europa.Controllers.Financeiro.Filtro();
        params.order = Europa.Controllers.Financeiro.Tabela.lastRequestParams.order;
        params.draw = Europa.Controllers.Financeiro.Tabela.lastRequestParams.draw;
        params.pageSize = Europa.Controllers.Financeiro.Tabela.lastRequestParams.pageSize;
        params.start = Europa.Controllers.Financeiro.Tabela.lastRequestParams.start;
        var formExportar = $("#Exportar");
        formExportar.find("input").remove();
        formExportar.attr("method", "post").attr("action", Europa.Controllers.Financeiro.UrlExportarPagina);
        formExportar.addHiddenInputData(params);
        formExportar.submit();
    }
};


Europa.Controllers.Financeiro.ExportarTodos = function () {
    if (Europa.Controllers.Financeiro.ValidarFiltro()) {
        var params = Europa.Controllers.Financeiro.Filtro();
        params.order = Europa.Controllers.Financeiro.Tabela.lastRequestParams.order;
        params.draw = Europa.Controllers.Financeiro.Tabela.lastRequestParams.draw;
        var formExportar = $("#Exportar");
        formExportar.find("input").remove();
        formExportar.attr("method", "post").attr("action", Europa.Controllers.Financeiro.UrlExportarTodos);
        formExportar.addHiddenInputData(params);
        formExportar.submit();
    }
};

Europa.Controllers.Financeiro.BuscarTotal = function () {
    var param = Europa.Controllers.Financeiro.Filtro();
    $.post(Europa.Controllers.Financeiro.UrlBuscarTotal, param, function (res) {
        var total = res.Total.replace("R$", "R$ ");
        $("#total").val(total);
    });
};

Europa.Controllers.Financeiro.Destaque = function () {
    $('body tr td:contains("Anexar nota fiscal")').parent().css({ 'background-color': '#fdf2d9' });
    $('body tr td:contains("Nota recusada")').parent().css({ 'background-color': '#fbd9df' });
    $('body tr td:contains("Nota recusada")').parent().addClass("nota-recusada");
};

Europa.Controllers.Financeiro.InitFiltro = function () {

    $('#DataComissaoAte').val("20/01/2021");
    $('#DataComissaoDe').val("20/01/2021");
};