Europa.Controllers.LogErro = {};
Europa.Controllers.LogErro.UrlExcluir = undefined;
Europa.Controllers.LogErro.Modal = {};
Europa.Controllers.LogErro.Modal.Horario = undefined;

$(document).ready(function () {
    Europa.Components.DatePicker.AutoApply();
    setTimeout(Europa.Controllers.LogErro.Modal.ConfigDatePicker, 300);
});

Europa.Controllers.LogErro.Modal.ConfigDatePicker = function () {

    Europa.Controllers.LogErro.Modal.Horario = new Europa.Components.DatePicker()
        .WithTarget("[name='HorarioInicio']")
        .WithFormat("DD/MM/YYYY HH:mm")
        .WithTimePicker()
        .WithTimePicker24h()
        .WithTimePickerIncrement(1)
        .Configure();

    Europa.Controllers.LogErro.Modal.Horario = new Europa.Components.DatePicker()
        .WithTarget("[name='HorarioFim']")
        .WithFormat("DD/MM/YYYY HH:mm")
        .WithTimePicker()
        .WithTimePicker24h()
        .WithTimePickerIncrement(1)
        .Configure();

    Europa.Controllers.LogErro.Modal.Horario = new Europa.Components.DatePicker()
        .WithTarget("[name='Horario']")
        .WithFormat("DD/MM/YYYY HH:mm")
        .Configure();
}

Europa.Controllers.LogErro.LimparFiltros = function () {
    $("#Type").val("");
    $("#Message").val("");
    $("#Source").val("");
    $("#Stacktrace").val("");
    $("#HorarioInicio").val("");
    $("#HorarioFim").val("");
};

Europa.Controllers.LogErro.Modal.AbrirModal = function (logWebService) {
    Europa.Controllers.LogErro.Modal.PreencherCampos(logWebService);
    $("#detalhamentoLogErro").modal("show");
};

Europa.Controllers.LogErro.Modal.CloseModal = function () {
    $("#detalhamentoLogErro").modal("hide");
};

Europa.Controllers.LogErro.Modal.PreencherCampos = function (logErro) {
    var date = Europa.Date.toFormatDate(logErro.CriadoEm, Europa.Date.FORMAT_DATE_HOUR_MINUTE_SECOND);
    $("#form_detalhar_log_erro").find("#CriadoEm").val(date);
    $("#form_detalhar_log_erro").find("#Type").val(logErro.Type);
    $("#form_detalhar_log_erro").find("#Message").val(logErro.Message);
    $("#form_detalhar_log_erro").find("#Source").val(logErro.Source);
    $("#form_detalhar_log_erro").find("#OriginClass").val(logErro.OriginClass);
    $("#form_detalhar_log_erro").find("#Caller").val(logErro.Caller);
    $("#form_detalhar_log_erro").find("#CodeLine").val(logErro.CodeLine);
    $("#form_detalhar_log_erro").find("#Target").val(logErro.Target);
    var result = "  ";
    if (logErro.Stacktrace != null && logErro.Stacktrace != undefined) {
        result = logErro.Stacktrace;
    }
    $("#form_detalhar_log_erro").find("#Stacktrace").html(document.createTextNode(result));
};

Europa.Controllers.LogErro.Modal.CopyToClipboard = function () {
    var aux = document.createElement("textarea");
    aux.value = document.getElementsByTagName("code").Stacktrace.innerText;
    document.body.appendChild(aux);
    aux.select();
    document.execCommand("copy");
    document.body.removeChild(aux);
};

Europa.Controllers.LogErro.ExcluirLogs = function () {
    var filtro = Europa.Form.SerializeJson('#form_filtro_log_erro');

    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Confirmacao, Europa.i18n.Messages.ConfirmacaoExclusaoLogs);
    Europa.Confirmacao.ConfirmCallback = function () {
        $.post(Europa.Controllers.LogErro.UrlExcluir, { filtro: filtro }, function (res) {
            if (res.Sucesso) {
                Europa.Messages.ShowMessages(res, Europa.i18n.Messages.Sucesso);
                Europa.Components.Datatable.LogErro.DataTable.reloadData();
            }
        });
    }
    Europa.Confirmacao.Show();
};