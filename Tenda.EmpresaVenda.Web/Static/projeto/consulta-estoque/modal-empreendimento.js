"use strict";


Europa.Components.ConsultaEstoqueModalEmpreendimento = {};
Europa.Components.ConsultaEstoqueModalEmpreendimento.IdModal = "#disponibilidadeEmpreendimento";

Europa.Components.ConsultaEstoqueModalEmpreendimento.AbrirModal = function () {
    
    $.post(Europa.Controllers.PreProposta.UrlValidarEmpreendimento, {
        idPreProposta: $('#PreProposta_Id').val()
    }, function (res) {
        if (res.Sucesso) {
            $(Europa.Components.ConsultaEstoqueModalEmpreendimento.IdModal).modal("show");
            Europa.Components.ConsultaEstoqueEmpreendimentoDatatable.FiltrarTabela();
        } else {
            Europa.Informacao.PosAcao(res);
        }
    });    
};

Europa.Components.ConsultaEstoqueModalEmpreendimento.CloseModal = function () {
    $(Europa.Components.ConsultaEstoqueModalEmpreendimento.IdModal).modal("hide");
};
