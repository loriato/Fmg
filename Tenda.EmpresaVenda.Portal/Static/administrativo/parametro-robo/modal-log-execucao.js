Europa.Components.LogExecucaoModal = {};
Europa.Components.LogExecucaoModal.DataTable = {};
Europa.Components.LogExecucaoModal.TabelaExtensoes = {};
Europa.Components.LogExecucaoModal.ActionSelecionar = undefined;
Europa.Components.LogExecucaoModal.Index = {};
var parametrosBusca = {};

//Eventos que serao disparados quando a pagina for carregada
$(document).ready(function () {

    Europa.Components.DatePicker.AutoApply();
    Europa.Components.LogExecucaoModal.LimparBusca();
    setTimeout(Europa.Components.LogExecucaoModal.ConfigDatePicker, 1000);

    var id = $('#Execucao_Id').val();
    if (id !== undefined && id >= 0) {
        parametrosBusca.idExecucao = id;
    }

});

Europa.Components.LogExecucaoModal.ConfigDatePicker = function() {
    new Europa.Components.DatePicker()
        .WithTarget('#filtroDataInicio')
        .WithParentEl("#modalLogsExecucao")
        .WithFormat("DD/MM/YYYY")
        .WithOpens("right")
        .WithParentEl("#modalLogsExecucao")
        .Configure();

   new Europa.Components.DatePicker()
        .WithTarget('#filtroDataFim')
        .WithParentEl("#modalLogsExecucao")
        .WithFormat("DD/MM/YYYY")
        .WithOpens("right")
        .Configure();

};

Europa.Components.LogExecucaoModal.AbrirModal = function(idExecucao) {
    Europa.Components.LogExecucaoModal.BuscarExecucaoParaDetalhar(idExecucao);
};

Europa.Components.LogExecucaoModal.CloseModal = function() {
    $("#modalLogsExecucao").modal("hide");
};

Europa.Components.LogExecucaoModal.GetRows = function() {
    return Europa.Components.LogExecucaoModal.DataTable.getRowsSelect();
};

Europa.Components.LogExecucaoModal.BuscarExecucaoParaDetalhar = function(idExecucao) {
    $.ajax({
        url: Europa.Components.LogExecucaoModal.buscarExecucao,
        dataType: 'json',
        data: { idExecucao: idExecucao }
    }).done(function(execucao) {
        if (execucao != null) {
            execucao.DataInicioExecucao = Europa.Date
                .toFormatLongDate(execucao.DataInicioExecucao, Europa.Date.FORMAT_DATE_HOUR_MINUTE_SECOND);
            execucao.DataFimExecucao = Europa.Date.toFormatLongDate(execucao.DataFimExecucao, "DD/MM/YYYY HH:mm:ss");
            $('#nome').val(execucao.Quartz.Nome);
            $('#dataInicioDialogo').val(execucao.DataInicioExecucao);
            $('#dataFimDialogo').val(execucao.DataFimExecucao);
            $('#Execucao_Id').val(execucao.Id);
            $("#modalLogsExecucao").modal("show");
            Europa.Components.LogExecucaoModal.TabelaExtensoes.Filtrar();
        }
    });
};


DataTableApp.controller('listLogs', listLogs);

Europa.Components.LogExecucaoModal.LimparBusca = function() {
    $("#filtroDataInicio").val("");
    $("#filtroDataFim").val("");
    $("#filtroTipo").val("");
    $("#filtroLog").val("");
};

Europa.Components.LogExecucaoModal.TabelaExtensoes.ParametrosFiltro = function () {
    return parametrosBusca;
};

Europa.Components.LogExecucaoModal.TabelaExtensoes.Filtrar = function() {
    parametrosBusca = {
        dataInicio: $("#filtroDataInicio").val(),
        dataFim: $("#filtroDataFim").val(),
        tipo: $("#filtroTipo").val(),
        log: $("#filtroLog").val(),
        idExecucao: $('#Execucao_Id').val()
    };
    if (parametrosBusca.idExecucao !== undefined && parametrosBusca.idExecucao >= 0) {
        Europa.Components.LogExecucaoModal.DataTable.reloadData();
    }
};



function listLogs($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Components.LogExecucaoModal.DataTable = dataTableWrapper;

    dataTableWrapper.setIdAreaHeader("barra_datatable_modal_log_execucao")
        .setColumns([
        DTColumnBuilder.newColumn('CriadoEm').withTitle(Europa.i18n.Messages.Momento).withOption("width", "15%").withOption('type', 'date-format-DD/MM/YYYY HH:mm:ss'),
        DTColumnBuilder.newColumn('Tipo').withTitle(Europa.i18n.Messages.Tipo).withOption("width", "15%").withOption('type', 'enum-format-TipoLog'),
        DTColumnBuilder.newColumn('Log').withTitle(Europa.i18n.Messages.Log).withOption("width", "70%")
        ])
        .setAutoInit(false)
        .setOptionsSelect('POST', Europa.Components.LogExecucaoModal.listAction, Europa.Components.LogExecucaoModal.TabelaExtensoes.ParametrosFiltro);
};
