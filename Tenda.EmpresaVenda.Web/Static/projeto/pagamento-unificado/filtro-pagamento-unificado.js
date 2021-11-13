$(function () {
    

    $("#autocomplete_empresa_venda").select2({
        trags: true
    });

    $("#filtro_estados").select2({
        trags: true,
        width: '100%'
    });
   
    Europa.Controllers.PagamentoUnificado.InitAutocomplete();

    setTimeout(Europa.Controllers.PagamentoUnificado.InitDatePicker, 300); 

});

Europa.Controllers.PagamentoUnificado.InitDatePicker = function () {
    Europa.Controllers.PagamentoUnificado.DateInicioVigencia = new Europa.Components.DatePicker()
        .WithTarget("#DateInicioVigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

    Europa.Controllers.PagamentoUnificado.DataTerminoVigencia = new Europa.Components.DatePicker()
        .WithTarget("#DataTerminoVigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

};

Europa.Controllers.PagamentoUnificado.OnChangeDataTerminoVigencia = function () {
    Europa.Controllers.PagamentoUnificado.DataTerminoVigencia = new Europa.Components.DatePicker()
        .WithTarget("#DataTerminoVigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#DateInicioVigencia").val())
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();
};

Europa.Controllers.PagamentoUnificado.InitAutocomplete = function () {
    Europa.Controllers.PagamentoUnificado.AutoCompleteEmpresaVenda = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("empresa_venda")
        .Configure();

    Europa.Controllers.PagamentoUnificado.AutoCompleteRegionais = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regionais")
        .Configure();

    Europa.Controllers.PagamentoUnificado.AutoCompleteEmpresaVenda.Data = function (params) {
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
                        return $("#filtro_estados").val();
                    },
                    column: 'estado'
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
    Europa.Controllers.PagamentoUnificado.AutoCompleteEmpresaVenda.Configure();
};

Europa.Controllers.PagamentoUnificado.Filtro = function () {
    var param = {
        PeriodoDe: $("#DateInicioVigencia").val(),
        PeriodoAte: $("#DataTerminoVigencia").val(),
        IdsEmpresaVenda: $("#autocomplete_empresa_venda").val(),
        Estados: $("#filtro_estados").val(),
        Regionais: $("#autocomplete_regionais").val(),
        TipoPagamento: $("#filtro_tipo_pagamento").val(),
        NomeCliente: $("#filtro_cliente").val(),
        CodigoProposta: $("#filtro_proposta").val(),
        DataVendaDe: $("#filtro_data_venda_inicio").val(),
        DataVendaAte: $("#filtro_data_venda_termino").val(),
        StatusIntegracaoSap: $("#filtro_status_sap").val(),
        Pago: $("#filtro_pago").val(),
        DataRcPedidoSapDe: $("#filtro_data_rc_pedido_de").val(),
        DataRcPedidoSapAte: $("#filtro_data_rc_pedido_ate").val(),
        DataPrevisaoPagamentoInicio: $("#data_previsao_pagamento_inicio").val(),
        DataPrevisaoPagamentoTermino: $("#data_previsao_pagamento_termino").val()
    };
    return param;
};

Europa.Controllers.PagamentoUnificado.LimparFiltro = function () {
    $("#DateInicioVigencia").val("");
    $("#DataTerminoVigencia").val("");
    $("#autocomplete_empresa_venda").val("").trigger("change");
    $("#filtro_estados").val("").trigger("change");
    $("#autocomplete_regionais").val("").trigger("change");
    $("#filtro_cliente").val("");
    $("#filtro_data_venda_inicio").val("");
    $("#filtro_data_venda_termino").val("");
    $("#filtro_proposta").val("");
    $("#filtro_status_sap").val("").trigger("change");
    $("#filtro_pago").val(0).trigger("change");
    $("#filtro_data_rc_pedido_de").val("");
    $("#filtro_data_rc_pedido_ate").val("");
    $("#data_previsao_pagamento_inicio").val("");
    $("#data_previsao_pagamento_termino").val("");
    $("#filtro_tipo_pagamento").val(0).trigger("change");

    //Europa.Controllers.PagamentoUnificado.Filtrar();
    Europa.Controllers.PagamentoUnificado.ListaPagamentos = [];
};

Europa.Controllers.PagamentoUnificado.ValidarFiltro = function () {
    var msgs = [];
    Europa.Controllers.PagamentoUnificado.ListaPagamentos = [];
    var autorizar = true;

    if ($("#DateInicioVigencia").val() == 0 ||
        $("#DataTerminoVigencia").val() == 0) {
        msgs.push("Insira uma Data de Aptidão");
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