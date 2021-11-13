$(function () {

    Europa.Controllers.Pagamento.InitDatePicker();
    Europa.Controllers.Pagamento.InitAutocomplete();    

    $("#autocomplete_empresa_venda").select2({
        trags: true
    });

    $("#filtro_estados").select2({
        trags: true,
        width: '100%'
    });

    Europa.Controllers.Pagamento.InitAutocomplete();  

});

Europa.Controllers.Pagamento.InitAutocomplete = function () {
    Europa.Controllers.Pagamento.AutoCompleteEmpresaVenda = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("empresa_venda")        
        .Configure();
    Europa.Controllers.Pagamento.AutocompleteRegionais = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regionais")
        .Configure();   

    Europa.Controllers.Pagamento.AutoCompleteEmpresaVenda.Data = function (params) {
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
                        return $("#filtro_regionais").val();
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
    Europa.Controllers.Pagamento.AutoCompleteEmpresaVenda.Configure();
};

Europa.Controllers.Pagamento.InitDatePicker = function () {
    Europa.Controllers.Pagamento.DateInicioVigencia = new Europa.Components.DatePicker()
        .WithTarget("#DateInicioVigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

    Europa.Controllers.Pagamento.DataTerminoVigencia = new Europa.Components.DatePicker()
        .WithTarget("#DataTerminoVigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

    Europa.Controllers.Pagamento.DataPrevisaoPagamentoInicio = new Europa.Components.DatePicker()
        .WithTarget("#data_previsao_pagamento_inicio")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate()
        .Configure();

    Europa.Controllers.Pagamento.DataPrevisaoPagamentoTermino = new Europa.Components.DatePicker()
        .WithTarget("#data_previsao_pagamento_termino")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate()
        .Configure();
};

Europa.Controllers.Pagamento.OnChangeDataTerminoVigencia = function () {
    Europa.Controllers.Pagamento.DataTerminoVigencia = new Europa.Components.DatePicker()
        .WithTarget("#DataTerminoVigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#DateInicioVigencia").val())
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

    Europa.Controllers.Pagamento.LimparTabela();
};

Europa.Controllers.Pagamento.LimparTabela = function () {
    $("#pagamentos").html("");
    Europa.Controllers.Pagamento.ListaPagamentos = [];
};

Europa.Controllers.Pagamento.LimparFiltro = function () {
    $(".filtro").val("");
    $("#autocomplete_empresa_venda").val("").trigger("change");
    $("#pagamentos").html("");
    $("#filtro_estados").val(0).trigger("change");
    $("#filtro_tipo_pagamento").val(0).trigger("change");
    $("#autocomplete_regionais").val("").trigger("change");
    $("#filtro_cliente").val("");
    $("#filtro_proposta").val("");
    $("#filtro_data_venda_inicio").val("");
    $("#filtro_data_venda_termino").val("");
    $("#filtro_data_rc_pedido_de").val("");
    $("#filtro_data_rc_pedido_ate").val("");
    $("#data_previsao_pagamento_inicio").val("");
    $("#data_previsao_pagamento_termino").val("");
    $("#filtro_pago").val(0).trigger("change");
    Europa.Controllers.Pagamento.ListaPagamentos = [];
};

Europa.Controllers.Pagamento.ValidarFiltro = function () {
    var msgs = [];
    Europa.Controllers.Pagamento.ListaPagamentos = [];
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

Europa.Controllers.Pagamento.Filtrar = function () {
    Europa.Controllers.Pagamento.ListaPagamentos = [];
    var autorizar = Europa.Controllers.Pagamento.ValidarFiltro();

    if (!autorizar) {
        return;
    }

    var filtro = Europa.Controllers.Pagamento.Filtro();

    $.post(Europa.Controllers.Pagamento.UrlListarPagamentos, filtro, function (res) {
        if (res.Sucesso) {            
            $("#pagamentos").html(res.Objeto)
            Europa.Controllers.Pagamento.Paginar();
        } else {
            Europa.Informacao.PosAcao(res);
        }
    });

};

Europa.Controllers.Pagamento.Filtro = function () {
    var filtro = {
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
    
    var request = {
        pageSize: 0,
        start:0
    };

    return {
        request: request,
        filtro: filtro
    };
};
