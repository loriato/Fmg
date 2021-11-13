$(function () {

});

function propostaFaturadaTabela($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.PropostaFaturada.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.PropostaFaturada.Tabela;
    self.setColumns([
        DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.UF).withOption('width', '5%'),
        DTColumnBuilder.newColumn('CodigoProposta').withTitle(Europa.i18n.Messages.Proposta).withOption('width', '75%'),
        DTColumnBuilder.newColumn('Faturado').withTitle(Europa.i18n.Messages.Faturado).withOption('width', '10%').renderWith(renderBool),
        DTColumnBuilder.newColumn('DataFaturado').withTitle(Europa.i18n.Messages.DataFaturamento).withOption('width', '10%').renderWith(Europa.Date.toGeenDateFormat),
        
    ])
        .setIdAreaHeader("datatable_header")
        .setDefaultOrder([[1, 'asc']])
        .setDefaultOptions('POST', Europa.Controllers.PropostaFaturada.UrlListar, Europa.Controllers.PropostaFaturada.Filtro);
    
    function renderBool(data) {
        if (data) {
            return Europa.i18n.Messages.Sim;
        }
        return Europa.i18n.Messages.Nao;
    }

}

DataTableApp.controller('TabelaPropostaFaturada', propostaFaturadaTabela);

Europa.Controllers.PropostaFaturada.Filtrar = function () {
    Europa.Controllers.PropostaFaturada.Tabela.reloadData();
};

Europa.Controllers.PropostaFaturada.Filtro = function () {
    var param = {
        CodigoProposta: $("#filtro_codigo_proposta").val(),
        Estado: $("#filtro_estados").val(),
        DataFaturadoDe: $("#filtro_data_faturamento_de").val(),
        DataFaturadoAte: $("#filtro_data_faturamento_ate").val(),
        Faturado:1
    };
    return param;
};

Europa.Controllers.PropostaFaturada.LimparFiltro = function () {
    $("#filtro_codigo_proposta").val("");
    $("#filtro_faturado").val(0).trigger("change");
    $("#filtro_regional").val("").trigger("change");
    $("#filtro_data_faturamento_de").val("");
    $("#filtro_data_faturamento_ate").val("");
    $("#filtro_estados").val(0).trigger("change");

    Europa.Controllers.PropostaFaturada.Filtrar();
};

Europa.Controllers.PropostaFaturada.ExportarTodos = function () {
    var params = Europa.Controllers.PropostaFaturada.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.PropostaFaturada.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.PropostaFaturada.ExportarPagina = function () {
    var params = Europa.Controllers.PropostaFaturada.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.PropostaFaturada.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};