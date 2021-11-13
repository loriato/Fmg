Europa.Controllers.PosVenda.Tabela = {};

$(function () {

    Europa.Controllers.PosVenda.AutoCompleteCorretor = new Europa.Components.AutoCompleteCorretorEmpresaVenda()
        .WithTargetSuffix("corretor")
        .Configure();

    Europa.Controllers.PosVenda.AutoCompleteProduto = new Europa.Components.AutoCompleteBreveLancamentoRegional()
        .WithTargetSuffix("produto")
        .Configure();

    Europa.Controllers.PosVenda.AutoCompletePontoVenda = new Europa.Components.AutoCompletePontoVendaEmpresaVenda()
        .WithTargetSuffix("ponto_venda")
        .Configure();

});

DataTableApp.controller('consultaPosVendaDatatable', consultaPosVendaDatatable);

function consultaPosVendaDatatable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.PosVenda.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.PosVenda.Tabela;
    self.setColumns([
        DTColumnBuilder.newColumn('CodigoPreProposta').withTitle(Europa.i18n.Messages.CodigoPreProposta).withOption('width', '150px')
            .withOption("link", self.withOptionLink(Europa.Components.DetailAction.PreProposta, "IdPreProposta")),
        DTColumnBuilder.newColumn('CodigoProposta').withTitle(Europa.i18n.Messages.CodigoProposta).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeClientePreProposta').withTitle(Europa.i18n.Messages.Cliente).withOption('width', '150px').renderWith(renderCliente),
        DTColumnBuilder.newColumn('SituacaoPreProposta').withTitle(Europa.i18n.Messages.Situacao).withOption('width', '100px').renderWith(textBold),
        DTColumnBuilder.newColumn('NomeCorretor').withTitle(Europa.i18n.Messages.Corretor).withOption('width', '150px'),
        //DTColumnBuilder.newColumn('StatusConformidade').withTitle(Europa.i18n.Messages.StatusConformidade).withOption('width', '100px').renderWith(textBold),
        DTColumnBuilder.newColumn('StatusConformidade').withTitle(Europa.i18n.Messages.SituacaoContrato).withOption('width', '100px').renderWith(renderSituacaoContrato),
        DTColumnBuilder.newColumn('SituacaoDocAvalista').withTitle("").withOption('width', '100px').renderWith(renderFarol)
    ])
        .setIdAreaHeader("consulta_pos_venda_datatable_header")     
        .setDefaultOrder([[1, 'desc']])
        .setDefaultOptions('POST', Europa.Controllers.PosVenda.UrlListarPosVenda, Europa.Controllers.PosVenda.Filtro);

    function renderFarol(data, type, full, meta) {
        var saida = '<div class="Rectangle-9 col-md-3">';
        if (data == 6) {
            saida += '<div class="Oval-3-Copy-2 col-md-1"></div>';
            saida += '<div class="Oval-3-Copy-3 col-md-1" style="background-color:#feca34"></div>';
            saida += '<div class="Oval-3-Copy-2 col-md-1"></div>';
            saida += '</div>';
            return saida;
        } 
        if (data == 3) {
            saida += '<div class="Oval-3-Copy-2 col-md-1"></div>';
            saida += '<div class="Oval-3-Copy-2 col-md-1"></div>';
            saida += '<div class="Oval-3-Copy-3 col-md-1" style="background-color:#79df0b"></div>';
            saida += '</div>';
            return saida;
        }
        if (data == 1 || full.PosChaves > 0 || full.PreChaves > 0 && full.PreChaves <= 100 && data != 6 && data != 3) {
            saida += '<div class="Oval-3-Copy-3 col-md-1" style="background-color:#ff5660"></div>';
            saida += '<div class="Oval-3-Copy-2 col-md-1"></div>';
            saida += '<div class="Oval-3-Copy-2 col-md-1"></div>';
            saida += '</div>';
            return saida;
        }
        else {
            saida += '<div class="Oval-3-Copy-2 col-md-1"></div>';
            saida += '<div class="Oval-3-Copy-2 col-md-1"></div>';
            saida += '<div class="Oval-3-Copy-2 col-md-1"></div>';
        }
        
        saida += '</div>';

        return saida;
    };

    function textBold(data) {
        if (data) {
            return '<b>' + data + '</b>';
        }

        return "";
    }

    function renderCliente(data, type, full, meta) {
        var link = '<div>';
        if (data) {
            link = link + "<a title='Cliente' href='" + Europa.Controllers.PosVenda.UrlCliente + '?id=' + full.IdCliente + "'>" + full.NomeClientePreProposta + "</a>";
        } else {
            link += full.NomeClienteProposta;
        }
        link += '</div>';
        return link;
    }

    function renderSituacaoContrato(data, type, full, meta){
        if (full.DataRepasse != null && full.DataConformidade != null) {
            return '<b>' + Europa.i18n.Enum.Resolve("SituacaoContrato", 1) +
                "/" +
                Europa.i18n.Enum.Resolve("SituacaoContrato", 2) +
                '</b>';
        }
        if (full.DataRepasse!=null) {
            return '<b>' + Europa.i18n.Enum.Resolve("SituacaoContrato", 1) + '</b>';
        }
        if (full.DataConformidade!=null) {
            return '<b>' + Europa.i18n.Enum.Resolve("SituacaoContrato", 2) + '</b>';
        }
        return '';
    }
    
}

Europa.Controllers.PosVenda.Filtro = function () {
    var param = {
        CodigoPreProposta: $("#filtro_codigo_pre_proposta").val(),
        CodigoProposta: $("#filtro_codigo_proposta").val(),
        NomeCliente: $("#filtro_cliente").val(),
        IdCorretor: $("#autocomplete_corretor").val(),
        IdProduto: $("#autocomplete_produto").val(),
        IdPontoVenda: $("#autocomplete_ponto_venda").val(),
        IdSituacaoPreProposta: $("#situacao_pre_proposta").val(),
        StatusConformidade: $("#filtro_status_conformidade").val(),
        Inicio: $("#DataInicioBusca").val(),
        Termino: $("#DataTerminoBusca").val(),
        SituacaoContrato: $("#situacaoContrato").val(),
        TipoFiltroPosVenda: $("#TipoFiltroPosVenda").val()
    }
    
    return param;
}

Europa.Controllers.PosVenda.LimparFiltro = function () {
    $("#filtro_codigo_pre_proposta").val("");
    $("#filtro_codigo_proposta").val("");
    $('#filtro_cliente').val("");
    $("#autocomplete_corretor").val("").trigger("change");
    $("#autocomplete_produto").val("").trigger("change");
    $("#autocomplete_ponto_venda").val("").trigger("change");
    $("#situacao_pre_proposta").val("").trigger("change");
    $("#filtro_status_conformidade").val("");
    $("#situacaoContrato").val("");
    $("#TipoFiltroPosVenda").val("");
};

Europa.Controllers.PosVenda.Filtrar = function () {
    Europa.Controllers.PosVenda.UpdatePieChartSelection($("#TipoFiltroPosVenda").val() - 1);
    Europa.Controllers.PosVenda.ResetButtonsChart();
    Europa.Controllers.PosVenda.Tabela.reloadData();
}

Europa.Controllers.PosVenda.ExportarPagina = function () {
    var params = Europa.Controllers.PosVenda.Filtro();
    params.order = Europa.Controllers.PosVenda.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.PosVenda.Tabela.lastRequestParams.draw;
    params.pageSize = Europa.Controllers.PosVenda.Tabela.lastRequestParams.pageSize;
    params.start = Europa.Controllers.PosVenda.Tabela.lastRequestParams.start;
    var formExportar = $("#form_exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.PosVenda.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.PosVenda.ExportarTodos = function () {
    var params = Europa.Controllers.PosVenda.Filtro();
    params.order = Europa.Controllers.PosVenda.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.PosVenda.Tabela.lastRequestParams.draw;
    var formExportar = $("#form_exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.PosVenda.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};