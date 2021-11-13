"use strict";


Europa.Components.ConsultaEstoqueModalEmpreendimento = {};

Europa.Components.ConsultaEstoqueModalEmpreendimento.AbrirModal = function () {

    $.post(Europa.Controllers.PreProposta.UrlValidarEmpreendimento, {
        idPreProposta: $('#PreProposta_Id').val()
    }, function (res) {
            if (res.Sucesso) {
            $("#disponibilidade-empreendimento-modal").modal("show");
            Europa.Components.ConsultaEstoqueEmpreendimentoDatatable.FiltrarTabela();
        } else {
            Europa.Informacao.PosAcao(res);
        }
    });
};

Europa.Components.ConsultaEstoqueModalEmpreendimento.CloseModal = function () {
    $("#disponibilidade-empreendimento-modal").modal("hide");
};