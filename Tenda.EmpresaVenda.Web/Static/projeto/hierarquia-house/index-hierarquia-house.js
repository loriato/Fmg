Europa.Controllers.HierarquiaHouse = {};
Europa.Controllers.HierarquiaHouse.DatatableSupervisorHouse = {};
Europa.Controllers.HierarquiaHouse.DatatableAgenteVendaHouse = {};
Europa.Controllers.HierarquiaHouse.DatatableHierarquiaHouse = {};

Europa.Controllers.HierarquiaHouse.IdSupervisorHouse = -1;
Europa.Controllers.HierarquiaHouse.NomeSupervisorHouse = -1;

Europa.Controllers.HierarquiaHouse.AutoCompleteAgenteVendaHouse = undefined;

Europa.Controllers.HierarquiaHouse.IdAgenteVenda = 0;

Europa.Controllers.HierarquiaHouse.AgenteVendaEdit = {};

Europa.Controllers.HierarquiaHouse.PeriodoDe = undefined;

Europa.Controllers.HierarquiaHouse.IdCoordenadorHouse = -1;

$(function () {
    Europa.Controllers.HierarquiaHouse.InitAutoComplete();

    //Europa.Controllers.HierarquiaHouse.ConfigureAutoCompleteAgenteVendaHouse(Europa.Controllers.HierarquiaHouse);
    Europa.Controllers.HierarquiaHouse.ConfigureAutoCompleteHouse(Europa.Controllers.HierarquiaHouse);
    //Europa.Controllers.HierarquiaHouse.OnChangeCoordenadorHouse(Europa.Controllers.HierarquiaHouse);
    setTimeout(Europa.Controllers.HierarquiaHouse.OnChangeSupervisorHouse, 300);

});

Europa.Controllers.HierarquiaHouse.InitAutoComplete = function () {
    //Europa.Controllers.HierarquiaHouse.AutoCompleteAgenteVendaHouse = new Europa.Components.AutoCompleteAgenteVendaHouse()
    //    .WithTargetSuffix("agente_venda_house")
    //    .Configure();

    Europa.Controllers.HierarquiaHouse.AutoCompleteSupervisorHouse = new Europa.Components.AutoCompleteSupervisorHouse()
        .WithTargetSuffix("supervisor_house")
        .Configure();

    Europa.Controllers.HierarquiaHouse.AutoCompleteHouse = new Europa.Components.AutoCompleteLojaPortal()
        .WithTargetSuffix("house")
        .Configure();

    Europa.Controllers.HierarquiaHouse.AutoCompleteLojaPortal = new Europa.Components.AutoCompleteLojaPortal()
        .WithTargetSuffix("house_filtro")
        .Configure();

    Europa.Controllers.HierarquiaHouse.AutoCompleteCoordenadorHouse = new Europa.Components.AutoCompleteCoordenadorHouse()
        .WithTargetSuffix("coordenador_house")
        .Configure();    
}

Europa.Controllers.HierarquiaHouse.ConfigureAutoCompleteAgenteVendaHouse = function (autocompleteWrapper) {
    autocompleteWrapper.AutoCompleteAgenteVendaHouse.Data = function (params) {
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
                        return Europa.Controllers.HierarquiaHouse.IdSupervisorHouse;
                    },
                    column: 'idSupervisorHouse'
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

    autocompleteWrapper.AutoCompleteAgenteVendaHouse.Configure();
}

Europa.Controllers.HierarquiaHouse.ConfigureAutoCompleteSupervisorHouse = function (autocompleteWrapper) {
    autocompleteWrapper.AutoCompleteSupervisorHouse.Data = function (params) {
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
                        return $("#autocomplete_coordenador_house").val();
                    },
                    column: 'IdCoordenadorHouse'
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

    autocompleteWrapper.AutoCompleteSupervisorHouse.Configure();
}

Europa.Controllers.HierarquiaHouse.ConfigureAutoCompleteHouse = function (autocompleteWrapper) {
    autocompleteWrapper.AutoCompleteHouse.Data = function (params) {
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
                        return Europa.Controllers.HierarquiaHouse.IdSupervisorHouse;
                    },
                    column: 'idSupervisorHouse'
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

    autocompleteWrapper.AutoCompleteHouse.Configure();
}

Europa.Controllers.HierarquiaHouse.FiltrarDatatableHierarquiaHouse = function () {
    Europa.Controllers.HierarquiaHouse.DatatableHierarquiaHouse.reloadData();
}

Europa.Controllers.HierarquiaHouse.OnChangeSupervisorHouse = function () {
    if ($("#IdCoordenadorHouse").val() > 0) {
        Europa.Controllers.HierarquiaHouse.AutoCompleteCoordenadorHouse.SetValue($("#IdCoordenadorHouse").val(), $("#NomeCoordenadorHouse").val());
        Europa.Controllers.HierarquiaHouse.IdCoordenadorHouse = $("#IdCoordenadorHouse").val();

        Europa.Controllers.HierarquiaHouse.FiltrarDatatableSupervisorHouse();
    }
}

Europa.Controllers.HierarquiaHouse.OnChangeCoordenadorHouse = function () {
    Europa.Controllers.HierarquiaHouse.ConfigureAutoCompleteSupervisorHouse(Europa.Controllers.HierarquiaHouse);
    Europa.Controllers.HierarquiaHouse.FiltrarDatatableSupervisorHouse();
}