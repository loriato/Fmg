$(function () {
    
});

Europa.Controllers.HierarquiaHouse.AbrirModalSelecionarSupervisor = function () {
    $("#modal_selecionar_supervisor").show();
}

Europa.Controllers.HierarquiaHouse.FecharModalSelecionarSupervisor = function () {
    Europa.Controllers.HierarquiaHouse.AutoCompleteSupervisorHouse.Clean();
    $("#modal_selecionar_supervisor").hide();
    Europa.Controllers.HierarquiaHouse.FiltrarDatatableSupervisorHouse();
}

Europa.Controllers.HierarquiaHouse.SalvarCoordenadorSupervisorHouse = function () {
    var full = Europa.Controllers.HierarquiaHouse.DatatableSupervisorHouse.getRowsSelect();

    var hierarquiaHouseDto = {
        IdCoordenadorSupervisorHouse: full.IdCoordenadorSupervisorHouse,
        IdSupervisorHouse: full.IdSupervisorHouse,
        IdCoordenadorHouse: full.IdCoordenadorHouse,
        NomeSupervisorHouse: full.NomeSupervisorHouse,
        NomeCoordenadorHouse: full.NomeCoordenadorHouse,
        IdSupervisorHouseNovo: Europa.Controllers.HierarquiaHouse.AutoCompleteSupervisorHouse.Value(),
        Ativo: full.Ativo
    }

    var backup = $('#cb_sup_' + full.IdCoordenadorHouse + '_' + full.IdSupervisorHouse + "").val() == "true";

    $.post(Europa.Controllers.HierarquiaHouse.UrlOnJoinCoordenadorSupervisorHouse, hierarquiaHouseDto, function (res) {
        console.log(res);

        if (res.Success) {
            console.log("Ok")
            Europa.Controllers.HierarquiaHouse.IdSupervisorHouse = -1;
            Europa.Controllers.HierarquiaHouse.FiltrarDatatableSupervisorHouse();
            Europa.Controllers.HierarquiaHouse.FecharModalSelecionarSupervisor();
        } else {
            $('#cb_sup_' + full.IdCoordenadorHouse + '_' + full.IdSupervisorHouse + "").prop("checked", backup);
        }

        Europa.Informacao.PosAcaoApi(res)

    });
}
