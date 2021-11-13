$(function () {
    //Europa.Controllers.Leads.AutoCompletePacoteDiretor = new Europa.Components.AutoCompletePacoteDiretor()
    //    .WithTargetSuffix("pacote_diretor")
    //    .Configure();

    Europa.Controllers.Leads.AutoCompleteCorretorFiltro = new Europa.Components.AutoCompleteCorretorEmpresaVenda()
        .WithTargetSuffix("corretor_filtro")
        .Configure();

    Europa.Mask.Telefone("#filtro_telefone", true);
    Europa.Controllers.Leads.PacoteDiretor();
});

Europa.Controllers.Leads.PacoteDiretor = function () {
    $.get(Europa.Components.ActionAutoCompletePacoteDiretor, function (res) {
        var options = '<option value=""></option>';
        res.records.forEach(function (elem) {
            if (elem.Nome == Europa.Controllers.Leads.Pacote) {
                options += '<option value="' + elem.Nome + '" selected>' + elem.Nome + '</option>';
            } else {
                options += '<option value="' + elem.Nome + '">' + elem.Nome + '</option>';
            }
        });
        $("#pacote_diretor").html(options);
    });
};

Europa.Controllers.Leads.FiltroDiretor = function () {

    var filtro = {
        SituacaoLead: $("#filtro_situacoes").val(),
        Pacote: $("#pacote_diretor").val(),
        IdCorretor: $("#autocomplete_corretor_filtro").val(),
        NomeLead: $("#filtro_lead").val(),
        Telefone: $("#filtro_telefone").val()
    };

    return filtro;
};

Europa.Controllers.Leads.LimparFiltroDiretor = function () {
    $("#filtro_situacoes").val("").trigger("change");
    $("#pacote_diretor").val("").trigger("change");
    $("#autocomplete_corretor_filtro").val("").trigger("change");
    $(".selecionar").addClass("hidden");
    $(".corretor").addClass("hidden");
    Europa.Controllers.Leads.DeselecionarRadio();
    Europa.Controllers.Leads.TabelaDiretor.scope.deselectAllRows();
    $(".selectAllDt").attr("disabled", "disabled");
    $(".deselectAllDt").attr("disabled", "disabled");
    Europa.Controllers.Leads.TabelaDiretor.clearData()
    $("#filtro_lead").val(" ");
    $("#filtro_telefone").val(" ");
};

Europa.Controllers.Leads.ValidarFiltroDiretor = function () {
    var pacote = $("#pacote_diretor").val();

    if (pacote == null || pacote == undefined || pacote.Length < 1 || pacote == "") {
        var res = {
            Sucesso: false,
            Mensagens: [Europa.i18n.Messages.CampoObrigatorio.replace("{0}", Europa.i18n.Messages.Pacote)]
        };

        Europa.Informacao.PosAcao(res);

        return false;
    }
    $(".selecionar").removeClass("hidden");
    return true;
};

Europa.Controllers.Leads.AbrirPacote = function () {
    $("#filtro_situacoes").val("").trigger("change");
    $("#autocomplete_corretor_filtro").val("").trigger("change");

    var autorizar = Europa.Controllers.Leads.ValidarFiltroDiretor();

    if (!autorizar) {
        return;
    }
    
    Europa.Controllers.Leads.FiltrarDatatableDiretor();
};
