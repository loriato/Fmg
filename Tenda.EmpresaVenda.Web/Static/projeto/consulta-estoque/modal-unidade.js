"use strict";

Europa.Components.ConsultaEstoqueModalUnidade = {};
Europa.Components.ConsultaEstoqueModalUnidade.IdModal = "#disponibilidadeUnidade";

Europa.Components.ConsultaEstoqueModalUnidade.AbrirModal = function (nomeEmpreendimento, divisao, caracteristicas, previsaoEntrega, idTorre) {
    $(Europa.Components.ConsultaEstoqueModalUnidade.IdModal).modal("show");
    //console.log(previsaoEntrega);
    var date = Europa.Date.toFormatDate.Utc(previsaoEntrega, Europa.Date.FORMAT_DATE);

    Europa.Components.ConsultaEstoqueUnidadeDatatable.EmpreendimentoParams = {
        divisao: divisao,
        caracteristicas: caracteristicas,
        previsaoEntrega: date,
        idTorre: idTorre
    };
    $("#modal_unidade_empreendimento").val(nomeEmpreendimento);
    $("#modal_unidade_caracteristicas").val(caracteristicas);
    $("#modal_unidade_previsao_entrega").val(date);
    Europa.Components.ConsultaEstoqueUnidadeDatatable.FiltrarTabela();
};

Europa.Components.ConsultaEstoqueModalUnidade.CloseModal = function () {
    $(Europa.Components.ConsultaEstoqueModalUnidade.IdModal).modal("hide");
};

Europa.Components.ConsultaEstoqueModalUnidade.PreSelecionar = function () {
    var data = Europa.Components.ConsultaEstoqueUnidadeDatatable.Tabela.getRowsSelect();

    if (data === null || data === undefined || data.length <= 0) {
        Europa.Informacao.Clear();
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.i18n.Messages.NecessarioSelecionarUnidade);
        Europa.Informacao.Show();
        return;
    }
    var businessKey = data.NomeEmpreendimento + " - " + data.NomeTorre + " - " + data.NomeUnidade + " - " + data.Caracteristicas;

    Europa.Confirmacao.PreAcao(Europa.i18n.Messages.EnviarParaIntegracao, businessKey, function () {
        Europa.Components.ConsultaEstoqueModalUnidade.Selecionar(data);
    });
};

Europa.Components.ConsultaEstoqueModalUnidade.Selecionar = function (data) {
    if (Europa.Components.ConsultaEstoqueUnidadeDatatable.Tabela.getRowsSelect()) {

        var identificador = data.NomeEmpreendimento + " - " + data.NomeTorre + " - " + data.NomeUnidade + " - " + data.IdSapUnidade;
        var idPreProposta = $('#PreProposta_Id').val();
        var requestBody = {
            idPreProposta: idPreProposta,
            idUnidadeSuat: data.IdUnidade,
            identificadorUnidadeSuat: identificador,
            idTorre: data.IdTorre,
            nomeTorre: data.NomeTorre
        };
        $.post(Europa.Controllers.PreProposta.UrlDefinirUnidadePreProposta, requestBody, function (res) {
            if (res.Sucesso) {
                Europa.Components.ConsultaEstoqueModalUnidade.CloseModal();
                Europa.Components.ConsultaEstoqueModalEmpreendimento.CloseModal();
                Europa.Components.ConsultaEstoqueModalUnidade.Integrar();
            } else {
                Europa.Informacao.PosAcao(res);
            }
        });
    }
};


Europa.Components.ConsultaEstoqueModalUnidade.Integrar = function () {
    var idPreProposta = $('#PreProposta_Id').val();
    var requestBody = {
        idPreProposta: idPreProposta
    };

    Europa.Components.LogIntegracaoModal.AbrirModal();
    $.post(Europa.Controllers.PreProposta.UrlIntegrarPreProposta, requestBody, function (res) {
        if (res.Sucesso) {
            Europa.Components.LogIntegracaoModal.Tabela.reloadData();
            Europa.Informacao.Hide = function () {
                location.reload(true);
            };
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, Europa.i18n.Messages.IntegracaoPropostaSucesso);
            Europa.Informacao.Show();
        } else {
            Europa.Informacao.Hide = function () {
                location.reload(true);
            };
            Europa.Informacao.PosAcao(res);
        }
    });
};