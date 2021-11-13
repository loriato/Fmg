$(function () {    
    setTimeout(Europa.Controllers.HierarquiaHouse.InitDatePicker,300)
});

function DatatableHierarquiaHouse($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.HierarquiaHouse.DatatableHierarquiaHouse = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.HierarquiaHouse.DatatableHierarquiaHouse;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('NomeCoordenadorHouse').withTitle(Europa.i18n.Messages.Coordenador).withOption("width", "130px"),
            DTColumnBuilder.newColumn('NomeSupervisorHouse').withTitle(Europa.i18n.Messages.Supervisor).withOption("width", "130px"),
            DTColumnBuilder.newColumn('NomeAgenteVendaHouse').withTitle(Europa.i18n.Messages.AgenteVenda).withOption("width", "150px"),
            DTColumnBuilder.newColumn('NomeHouse').withTitle(Europa.i18n.Messages.House).withOption("width", "110px"),
            DTColumnBuilder.newColumn('Inicio').withTitle(Europa.i18n.Messages.Inicio).withOption("width", "100px").renderWith(Europa.Date.toGeenDateTimeFormat),
            DTColumnBuilder.newColumn('Fim').withTitle(Europa.i18n.Messages.Fim).withOption("width", "100px").renderWith(Europa.Date.toGeenDateTimeFormat),
            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Status).withOption("width", "100px").withOption('type', 'enum-format-SituacaoHierarquiaHouse'),
            
        ])
        .setAutoInit(false)
        .setIdAreaHeader("hierarquia_house_datatable_barra")
        .setDefaultOrder([[4, 'desc'], [6, 'asc']])
        .setDefaultOptions('POST', Europa.Controllers.HierarquiaHouse.UrlListarHierarquiaHouse, Europa.Controllers.HierarquiaHouse.FiltroHierarquiaHouse);   

};

DataTableApp.controller('DatatableHierarquiaHouse', DatatableHierarquiaHouse);


Europa.Controllers.HierarquiaHouse.FiltroHierarquiaHouse = function () {
    var filtro = {
        IdHouse: $("#autocomplete_house_filtro").val(),
        PeriodoDe: $("#PeriodoDe").val(),
        PeriodoAte: $("#PeriodoAte").val(),
        Situacao: $("#situacao_hierarquia_house").val(),
        IdCoordenadorHouse: $("#autocomplete_coordenador_house").val(),
        IdSupervisorHouse: $("#autocomplete_supervisor_house").val(),
    }

    return filtro;
}

Europa.Controllers.HierarquiaHouse.LimparFiltroHierarquiaHouse = function () {
    Europa.Controllers.HierarquiaHouse.AutoCompleteLojaPortal.Clean();
    $("#PeriodoDe").val("").trigger("change");
    $("#PeriodoAte").val("").trigger("change");
    $("#situacao_hierarquia_house").val(0).trigger('change');
}

Europa.Controllers.HierarquiaHouse.OnChangePeriodoDe = function () {
    Europa.Controllers.HierarquiaHouse.PeriodoAte = new Europa.Components.DatePicker()
        .WithTarget("#PeriodoAte")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#PeriodoDe").val())
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();
}

Europa.Controllers.HierarquiaHouse.InitDatePicker = function () {
    Europa.Controllers.HierarquiaHouse.PeriodoDe = new Europa.Components.DatePicker()
        .WithTarget("#PeriodoDe")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

    Europa.Controllers.HierarquiaHouse.PeriodoAte = new Europa.Components.DatePicker()
        .WithTarget("#PeriodoAte")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();
}
