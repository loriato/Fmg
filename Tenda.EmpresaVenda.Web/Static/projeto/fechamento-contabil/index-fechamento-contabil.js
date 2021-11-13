Europa.Controllers.FechamentoContabil = {};

$(function () {

});

Europa.Controllers.FechamentoContabil.OnChangeTerminoFechamento = function () {
    Europa.Controllers.FechamentoContabil.InicioFechamento = new Europa.Components.DatePicker()
        .WithTarget("#TerminoFechamento")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#InicioFechamento").val())
        .Configure();
}

Europa.Controllers.FechamentoContabil.InitDatepicker = function () {
    Europa.Controllers.FechamentoContabil.InicioFechamento = new Europa.Components.DatePicker()
        .WithTarget("#InicioFechamento")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate(new Date())
        .Configure();

    Europa.Controllers.FechamentoContabil.TerminoFechamento = new Europa.Components.DatePicker()
        .WithTarget("#TerminoFechamento")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate(new Date())
        .Configure();
}

