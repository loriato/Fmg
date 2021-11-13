Europa.Controllers.KanbanPreProposta = {};
Europa.Controllers.KanbanPreProposta.IdCardKanbanPreProposta = null;
Europa.Controllers.KanbanPreProposta.IdAreaKanbanPreProposta = null;
Europa.Controllers.KanbanPreProposta.qtdPreproposta = null;
Europa.Controllers.KanbanPreProposta.UrlCarregaNivel2Kanban = null;
Europa.Controllers.KanbanPreProposta.UrlCarregaCardsPrePropostaKanban = null;
Europa.Controllers.KanbanPreProposta.DatatableCards = [];

$(function () {
});

Europa.Controllers.KanbanPreProposta.AbrirModalSituacaoCardKanban = function () {
    $("#modal_situacao_card_kanban").show();
}

Europa.Controllers.KanbanPreProposta.FecharModalSituacaoCardKanban = function () {
    window.location.reload(true)
}

Europa.Controllers.KanbanPreProposta.FiltroDatatableSituacaoCardKanban = function () {
    console.log($("#autocomplete_loja").val());
    var param = {
        IdCardKanbanPreProposta: Europa.Controllers.KanbanPreProposta.IdCardKanbanPreProposta,
        IdAreaKanbanPreProposta: Europa.Controllers.KanbanPreProposta.IdAreaKanbanPreProposta,
        IdEmpresasVendas: ($("#autocomplete_loja").val() == null ? null : $("#autocomplete_loja").val()),
        CodigoProposta : function () { return $("#filtro_codigo_proposta").val() },
        NomeCliente: function () { return $("#filtro_cliente").val() } ,
        IdAgentes: function () { return $("#autocomplete_agente_venda").val() } ,
        tipoStatusFiltro: function () {
                    return (($("#autocomplete_agrupamento_processo_pre_proposta").val() != undefined && $("#autocomplete_agrupamento_processo_pre_proposta").val() != null) ? $("#autocomplete_agrupamento_processo_pre_proposta").val().substr(0, 1) : "");
                },
        IdAgrupamentoProcessoPreProposta: function () { return (($("#autocomplete_agrupamento_processo_pre_proposta").val() != undefined && $("#autocomplete_agrupamento_processo_pre_proposta").val() != null) ? $("#autocomplete_agrupamento_processo_pre_proposta").val().substr(1) : "") } ,
        IdLoja: function () { return $("#autocomplete_loja").val() } ,
        IdBreveLancamento: function () { return $("#autocomplete_breve_lancamento").val() } ,
        ElaboracaoDe: function () { return $("#ElaboracaoDe").val() },
        ElaboracaoAte: function () { return $("#ElaboracaoAte").val() },
        DataEnvioDe: function () { return $("#DataEnvioDe").val() },
        DataEnvioAte: function () { return $("#DataEnvioAte").val() } ,
        DataSourceRequest: {
            order: [
                {
                    value: ($("#RadioCliente").filter(":checked").val() == "on" ? "asc" : "desc"),
                    column: ($("#RadioCliente").filter(":checked").val() == "on" ? "NomeCliente" : "Elaboracao")
                }
            ],            
            start: 0,
            pagesize: 300,
            draw: null
        }
    };
    return param;
}


Europa.Controllers.KanbanPreProposta.Limpar = function () {
    $("#autocomplete_situacao_card_kanban").val("").trigger("change");
}
//Modal Adicionar Situação ao Card

Europa.Controllers.KanbanPreProposta.CarregaNivel2Kanban = function (idAreaKanbanPreProposta) {
    var $Nivel2Kanban = $('#Nivel2Kanban'),
        url = $(this).data('url');
    $('#CardsPreProposta').replaceWith('<div id="CardsPreProposta"></div>');
    var qtd = document.getElementById("qtdPropostas");
    qtd.innerText = "0 prepropostas";
    if (idAreaKanbanPreProposta != 0 || idAreaKanbanPreProposta != undefined) {
        document.querySelectorAll('.GrayBackground').forEach(div => {
            // faz o toggle da classe, adiciona se não exitir, remove se existir
            div.classList.remove('GrayBackground');
            div.classList.remove('ButtonSelected-Card');
        })
        document.querySelectorAll('.trocacor').forEach(div => {
            // faz o toggle da classe, adiciona se não exitir, remove se existir
            div.classList.toggle('GrayBackground');
            div.classList.remove('ButtonSelected-Card');
        })
    }
    Europa.Controllers.KanbanPreProposta.IdAreaKanbanPreProposta = idAreaKanbanPreProposta;
    Europa.Controllers.KanbanPreProposta.IdCardKanbanPreProposta = 0;
    //$("#area_" + idAreaKanbanPreProposta).classList.remove("GrayBackground");
    $.get(Europa.Controllers.KanbanPreProposta.UrlCarregaNivel2Kanban + "?idAreaKanbanPreProposta=" + idAreaKanbanPreProposta, function (data) {        
        $Nivel2Kanban.replaceWith(data);
        document.querySelectorAll("#area_" + idAreaKanbanPreProposta).forEach(div => {
            // faz o toggle da classe, adiciona se não exitir, remove se existir
            div.classList.toggle('GrayBackground');
            div.classList.toggle('ButtonSelected-Card');
        })
        Europa.Controllers.KanbanPreProposta.CarregaCardPrePropostaKanban();
    });
}
Europa.Controllers.KanbanPreProposta.MudaSeta = function () {
    document.querySelectorAll('#spanSeta').forEach(div => {
        // faz o toggle da classe, adiciona se não exitir, remove se existir
        div.classList.toggle('fa-chevron-up');
        div.classList.toggle('fa-chevron-down');
        if (div.innerHTML == "Recolher filtro")
            div.innerHTML = " Expandir filtro";
        else
            div.innerHTML = " Recolher filtro";
    })
}

Europa.Controllers.KanbanPreProposta.CarregaCardPrePropostaKanban = function (idCardKanbanPreProposta) {
    var $Nivel2Kanban = $('#CardsPreProposta'),
        url = $(this).data('url');
    if (idCardKanbanPreProposta > 0) {
        Europa.Controllers.KanbanPreProposta.IdCardKanbanPreProposta = idCardKanbanPreProposta;
        document.querySelectorAll('.GrayBackground2').forEach(div => {
            // faz o toggle da classe, adiciona se não exitir, remove se existir
            div.classList.remove('GrayBackground2');
            div.classList.remove('ButtonSelected-Card');
        })
        document.querySelectorAll('.trocacorNivel2').forEach(div => {
            // faz o toggle da classe, adiciona se não exitir, remove se existir
            div.classList.toggle('GrayBackground2');
            div.classList.remove('ButtonSelected-Card');
        })
    }
    var param = Europa.Controllers.KanbanPreProposta.FiltroDatatableSituacaoCardKanban();

    $.post(Europa.Controllers.KanbanPreProposta.UrlCarregaCardsPrePropostaKanban, { request: param.Request, filtro: param }, function (data) {
        $Nivel2Kanban.replaceWith(data);
        var qtd = document.getElementById("qtdPropostas");
        qtd.innerText = Europa.Controllers.KanbanPreProposta.qtdPreproposta + " prepropostas";
        if (idCardKanbanPreProposta > 0) {
            document.querySelectorAll("#card_" + param.IdCardKanbanPreProposta).forEach(div => {
                // faz o toggle da classe, adiciona se não exitir, remove se existir
                div.classList.toggle('GrayBackground2');
                div.classList.toggle('ButtonSelected-Card');
            })
        }
        document.getElementById('Filtro_Card_Kanban').classList.remove('invisible');
        document.getElementById('Body_Card_Kanban').classList.remove('invisible');
    });
}

Europa.Controllers.KanbanPreProposta.Bloqueio = function (id) {    
    document.querySelectorAll('.CardsPreProposta').forEach(div => {
        // faz o toggle da classe, adiciona se não exitir, remove se existir
        div.classList.toggle('BlockCards');
    });
    document.getElementById('cardPreProposta_' + id).classList.remove('BlockCards');
    document.getElementById('OlhoBloqueado_' + id).classList.toggle('CardEyesRed');
    document.getElementById('OlhoBloqueado_' + id).classList.toggle('CardEyesWhite');
}