Europa.Controllers.FilaUnificada.ExibindoInfo = false;

$(function () {
    Europa.Controllers.FilaUnificada.InitDatePicker();

    Europa.Controllers.FilaUnificada.InitAutoComplete();

    Europa.Controllers.FilaUnificada.AlternarExibicaoInformacoes();
    
    $("#Filas").select2({
        trags: true
    });
});

Europa.Controllers.FilaUnificada.InitDatePicker = function () {
    Europa.Controllers.FilaUnificada.DataElaboracaoDe = new Europa.Components.DatePicker()
        .WithTarget("#DataElaboracaoDe")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

    Europa.Controllers.FilaUnificada.DataElaboracaoAte = new Europa.Components.DatePicker()
        .WithTarget("#DataElaboracaoAte")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

    Europa.Controllers.FilaUnificada.DataUltimoEnvioDe = new Europa.Components.DatePicker()
        .WithTarget("#DataUltimoEnvioDe")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

    Europa.Controllers.FilaUnificada.DataUltimoEnvioAte = new Europa.Components.DatePicker()
        .WithTarget("#DataUltimoEnvioAte")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();
};

Europa.Controllers.FilaUnificada.InitAutoComplete = function () {
    Europa.Controllers.FilaUnificada.AutoCompleteEmpresaVenda = new Europa.Components.AutoCompleteEmpresaVendasCACT()
        .WithTargetSuffix("empresa_venda")
        .Configure();
};

Europa.Controllers.FilaUnificada.OnChangeDataElaboracao = function () {
    Europa.Controllers.FilaUnificada.DataElaboracaoAte = new Europa.Components.DatePicker()
        .WithTarget("#DataElaboracaoAte")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#DataElaboracaoDe").val())
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();
};

Europa.Controllers.FilaUnificada.OnChangeDataUltimoEnvio = function () {
    Europa.Controllers.FilaUnificada.DataUltimoEnvioAte = new Europa.Components.DatePicker()
        .WithTarget("#DataUltimoEnvioAte")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#DataUltimoEnvioDe").val())
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();
};

Europa.Controllers.FilaUnificada.AlternarExibicaoInformacoes = function () {
    if (!Europa.Controllers.FilaUnificada.ExibindoInfo) {
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

        Europa.Controllers.FilaUnificada.InitDatePicker();
    }
    Europa.Controllers.FilaUnificada.ExibindoInfo = !Europa.Controllers.FilaUnificada.ExibindoInfo;
};

Europa.Controllers.FilaUnificada.Filtro = function () {
    return {
        Regional: $("#Regional").val(),
        CodigoProposta: $("#CodigoProposta").val(),
        DataUltimoEnvioDe: $("#DataUltimoEnvioDe").val(),
        DataUltimoEnvioAte: $("#DataUltimoEnvioAte").val(),
        Filas: $("#Filas").val(),
        NomeViabilizador: $("#NomeViabilizador").val(),
        NomeCliente: $("#NomeCliente").val(),   
        CpfCnpj: $("#CpfCnpj").val(),
        NomeEmpreendimento: $("#NomeEmpreendimento").val(),        
        IdEmpresaVendas: $("#autocomplete_empresa_venda").val(),
        DataElaboracaoDe: $("#DataElaboracaoDe").val(),
        DataElaboracaoAte: $("#DataElaboracaoAte").val(),
        AvalistaPendente: $("#check_avalista").val() == "true"
    };
};

Europa.Controllers.FilaUnificada.LimparFiltro = function () {
    $("#filtro").find("input[type=text], textarea").val("");
    $("#Filas").val("").trigger('change');
    $("#Regional").val("Todos");
    $("#autocomplete_empresa_venda").val("").trigger("change");
    $("#check_avalista").prop("checked", false);
    $("#check_avalista").val("false");
};

Europa.Controllers.FilaUnificada.OnCheckAvalista = function () {
    if ($("#check_avalista").val() == "true") {
        $("#check_avalista").val("false");
    } else {
        $("#check_avalista").val("true");
    }
};

Europa.Controllers.FilaUnificada.Filtrar = function () {
    Europa.Controllers.FilaUnificada.Tabela.reloadData();
};