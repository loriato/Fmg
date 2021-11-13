Europa.Controllers.ClienteModalExportar = {};

$(function () {

    Europa.Controllers.ClienteModalExportar.AutoCompleteEmpresaVenda = new Europa.Components.AutoCompleteEmpresaVendas()
    .WithTargetSuffix("empresa_venda")
    .Configure();
    
    Europa.Controllers.ClienteModalExportar.ConfigureEmpresaVendaAutocomplete(Europa.Controllers.ClienteModalExportar);
});

Europa.Controllers.ClienteModalExportar.AbrirModal = function (selectCallback) {
    $("#modal_exportar_clientes").modal("show");
    Europa.Controllers.ClienteModalExportar.InitDatePicker();
};

Europa.Controllers.ClienteModalExportar.Exportar = function () {
    if ($("#DataCriacaoDe").val() == "" || $("#DataCriacaoAte").val() == "") {
        var msgs = [];
        msgs.push(Europa.i18n.Messages.InsiraPeriodoExportar)
        var res = {
            Sucesso: false,
            Mensagens: msgs,
        };
        Europa.Informacao.PosAcao(res);
        return "";
    }
    var params = {
        Regional: $("#Regional").val(),
        idEmpresaVenda: $("#autocomplete_empresa_venda").val(),
        DataCriacaoDe: $("#DataCriacaoDe").val(),
        DataCriacaoAte: $("#DataCriacaoAte").val()
    };
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.Cliente.UrlExportar);
    formExportar.addHiddenInputData(params);
    formExportar.submit();

};

Europa.Controllers.ClienteModalExportar.InitDatePicker = function () {
    Europa.Controllers.ClienteModalExportar.DataCriacaoDe = new Europa.Components.DatePicker()
        .WithTarget("#DataCriacaoDe")
        .WithFormat("DD/MM/YYYY")
        .Configure();

    Europa.Controllers.ClienteModalExportar.DataCriacaoAte = new Europa.Components.DatePicker()
        .WithTarget("#DataCriacaoAte")
        .WithFormat("DD/MM/YYYY")
        .Configure();
};

Europa.Controllers.ClienteModalExportar.ConfigureEmpresaVendaAutocomplete = function (autocompleteWrapper) {
    autocompleteWrapper.AutoCompleteEmpresaVenda.Data = function (params) {
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
                        return $("#Regional").val();
                    },
                    column: 'regional'
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
    autocompleteWrapper.AutoCompleteEmpresaVenda.Configure();
}
Europa.Controllers.ClienteModalExportar.OnChangeDataCriacaoDe = function () {
    Europa.Controllers.ClienteModalExportar.DataCriacaoAte = new Europa.Components.DatePicker()
        .WithTarget("#DataCriacaoAte")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#DataCriacaoDe").val())
        .Configure();
}
