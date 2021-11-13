Europa.Controllers.AgrupamentoProcessoPreProposta = {};
Europa.Controllers.AgrupamentoProcessoPreProposta.IdAgrupamentoProcessoPreProposta = {};
Europa.Controllers.AgrupamentoProcessoPreProposta.Tabela = {};
Europa.Controllers.AgrupamentoProcessoPreProposta.Permissoes = {};
Europa.Components.Datatable.AgrupamentoProcessoPreProposta = {};
Europa.Components.Datatable.AgrupamentoProcessoPreProposta.DataTable = {};
$(function () {
    Europa.Controllers.AgrupamentoProcessoPreProposta.AutoCompleteSistemas = new Europa.Components.AutoCompleteSistemas()
        .WithTargetSuffix("sistemas")
        .Configure();
});
