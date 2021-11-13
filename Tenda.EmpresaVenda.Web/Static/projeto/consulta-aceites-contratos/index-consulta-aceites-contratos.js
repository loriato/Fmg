Europa.Controllers.ConsultaAceitesContratos = {};
Europa.Controllers.ConsultaAceitesContratos.Filtrar = {};
Europa.Controllers.ConsultaAceitesContratos.Permissoes = {};
var collapsedGroups = {};

$(function () {
    $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
    $(".ng-scope").css("word-wrap", "break-word");
    $("#autocomplete_empresa_venda").select2({
        placeholder: 'Selecione uma empresa de venda',
        width: '100%',
        multiple: true,
        scrollAfterSelect: true
    });
    Europa.Controllers.ConsultaAceitesContratos.InitAutoComplete();


});

Europa.Controllers.ConsultaAceitesContratos.InitAutoComplete = function () {
    Europa.Controllers.ConsultaAceitesContratos.AutoCompleteEmpresaVenda = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("empresa_venda")
        .Configure();

    Europa.Controllers.ConsultaAceitesContratos.AutoCompleteEmpresaVenda.Data = function (params) {
        return {
            start: 0,
            pageSize: 10,
            filter: [
                {
                    value: params.term,
                    column: this.param,
                    regex: true
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
    Europa.Controllers.ConsultaAceitesContratos.AutoCompleteEmpresaVenda.Configure();
};


Europa.Controllers.ConsultaAceitesContratos.ExportarPagina = function () {
    var params = Europa.Controllers.ConsultaAceitesContratos.Filtro();
    params.order = Europa.Controllers.ConsultaAceitesContratos.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.ConsultaAceitesContratos.Tabela.lastRequestParams.draw;
    params.pageSize = Europa.Controllers.ConsultaAceitesContratos.Tabela.lastRequestParams.pageSize;
    params.start = Europa.Controllers.ConsultaAceitesContratos.Tabela.lastRequestParams.start;
    var formExportar = $("#form_exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.ConsultaAceitesContratos.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};


Europa.Controllers.ConsultaAceitesContratos.ExportarTodos = function () {
    var params = Europa.Controllers.ConsultaAceitesContratos.Filtro();
    params.order = Europa.Controllers.ConsultaAceitesContratos.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.ConsultaAceitesContratos.Tabela.lastRequestParams.draw;
    var formExportar = $("#form_exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.ConsultaAceitesContratos.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};





Europa.Controllers.ConsultaAceitesContratos.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.ConsultaAceitesContratos.LimparErro = function () {
    $("[name='nova_situacao']").parent().removeClass("has-error");
};


