$(function () {
    Europa.Controllers.Leads.AutoCompletePacoteCorretor = new Europa.Components.AutoCompletePacoteCorretor()
        .WithTargetSuffix("pacote_corretor")
        .Configure();
    Europa.Mask.Telefone("#filtro_telefone_corretor", true);
    Europa.Controllers.Leads.PacoteCorretor();
});

Europa.Controllers.Leads.PacoteCorretor = function () {
    $.get(Europa.Components.ActionAutoCompletePacoteCorretor, function (res) {
        var options = '<option value=""></option>';
        res.records.forEach(function (elem) {
            if (elem.Nome == Europa.Controllers.Leads.Pacote) {
                options += '<option value="' + elem.Nome + '" selected>' + elem.Nome + '</option>';
            } else {
                options += '<option value="' + elem.Nome + '">' + elem.Nome + '</option>';
            }
        });
        $("#pacote_corretor").html(options);
    });
};

Europa.Controllers.Leads.LimparFiltroCorretor = function () {
    $("#filtro_situacoes_corretor").val("").trigger("change");
    $("#pacote_corretor").val("").trigger("change");
    $("#filtro_lead_corretor").val(" ");
    $("#filtro_telefone_corretor").val(" ");
};

Europa.Controllers.Leads.FiltroCorretor = function () {

    var filtro = {
        SituacaoLead: $("#filtro_situacoes_corretor").val(),
        Pacote: $("#pacote_corretor").val(),
        IdCorretor: Europa.Controllers.Leads.IdCorretor,
        NomeCorretor: Europa.Controllers.Leads.NomeCorretor,
        NomeLead: $("#filtro_lead_corretor").val(),
        Telefone: $("#filtro_telefone_corretor").val()
    };

    return filtro;
};