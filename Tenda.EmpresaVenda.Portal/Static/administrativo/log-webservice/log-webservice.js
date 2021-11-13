Europa.Controllers.LogWebService = {};
Europa.Controllers.LogWebService.UrlExcluir = undefined;
Europa.Controllers.LogWebService.Modal = {};
Europa.Controllers.LogWebService.Modal.Horario = undefined;

$(document).ready(function () {
    Europa.Components.DatePicker.AutoApply();
    setTimeout(Europa.Controllers.LogWebService.Modal.ConfigDatePicker, 300);
});

Europa.Controllers.LogWebService.Modal.ConfigDatePicker = function () {
    
    Europa.Controllers.LogWebService.Modal.Horario1 = new Europa.Components.DatePicker()
        .WithTarget("[name='HorarioInicio']")
        .WithFormat("DD/MM/YYYY HH:mm")
        .WithTimePicker()
        .WithTimePicker24h()
        .WithParentEl("#form_filtro_log_webservice")
        .WithTimePickerIncrement(1)
        .Configure();

    Europa.Controllers.LogWebService.Modal.Horario2 = new Europa.Components.DatePicker()
        .WithTarget("[name='HorarioFim']")
        .WithFormat("DD/MM/YYYY HH:mm")
        .WithTimePicker()
        .WithTimePicker24h()
        .WithParentEl("#form_filtro_log_webservice")
        .WithTimePickerIncrement(1)
        .Configure();

    Europa.Controllers.LogWebService.Modal.Horario3 = new Europa.Components.DatePicker()
        .WithTarget("[name='Horario']")
        .WithFormat("DD/MM/YYYY HH:mm")
        .WithParentEl("#detalhamentoLogWebService")
        .Configure();
}

Europa.Controllers.LogWebService.LimparFiltros = function() {
    $("#Endpoint").val("");
    $("#Action").val("");
    $("#Content").val("");
    $("#HorarioInicio").val("");
    $("#HorarioFim").val("");
    $("#Stage").val("");
};

Europa.Controllers.LogWebService.Modal.AbrirModal = function (logWebService) {
    Europa.Controllers.LogWebService.Modal.PreencherCampos(logWebService);
    $("#detalhamentoLogWebService").modal("show");
};

Europa.Controllers.LogWebService.Modal.CloseModal = function() {
    $("#detalhamentoLogWebService").modal("hide");
};

Europa.Controllers.LogWebService.Modal.PreencherCampos = function (logWebService) {
    var date = Europa.Date.toFormatDate(logWebService.CriadoEm, Europa.Date.FORMAT_DATE_HOUR_MINUTE_SECOND);
    $("#form_detalhar_log_webservice").find("#CriadoEm").val(date);
    $("#form_detalhar_log_webservice").find("#Endpoint").val(logWebService.Endpoint);
    $("#form_detalhar_log_webservice").find("#Action").val(logWebService.Action);
    $("#form_detalhar_log_webservice").find("#Stage").val(Europa.Controllers.LogWebService.Modal.ResolveEnum(logWebService.Stage));
    var temp = logWebService.Content;
    temp = Europa.Controllers.LogWebService.Modal.syntaxHighlight(temp);
    $("#form_detalhar_log_webservice").find("#Content").html(document.createTextNode(temp));
};

Europa.Controllers.LogWebService.Modal.ResolveEnum = function (data) {
    if (data == 1) {
        return "Antes de Serializar";
    } else if (data == 2) {
        return "Depois de Serializar";
    } else if (data == 4) {
        return "Antes de Deserializar";
    } else if (data == 8) {
        return "Depois de Deserializar";
    }
}

Europa.Controllers.LogWebService.Modal.CopyToClipboard = function () {
    var aux = document.createElement("textarea");
    aux.value = document.getElementsByTagName("code").Content.innerText;
    document.body.appendChild(aux);
    aux.select();
    document.execCommand("copy");

    document.body.removeChild(aux);
};

Europa.Controllers.LogWebService.Modal.syntaxHighlight = function(xml) {
        var formatted = '';
        var reg = /(>)(<)(\/*)/g;
        xml = xml.replace(reg, '$1\r\n$2$3');
        var pad = 0;
        jQuery.each(xml.split('\r\n'), function (index, node) {
            var indent = 0;
            if (node.match(/.+<\/\w[^>]*>$/)) {
                indent = 0;
            } else if (node.match(/^<\/\w/)) {
                if (pad != 0) {
                    pad -= 1;
                }
            } else if (node.match(/^<\w[^>]*[^\/]>.*$/)) {
                indent = 1;
            } else {
                indent = 0;
            }
            var padding = '';
            for (var i = 0; i < pad; i++) {
                padding += '  ';
            }
            formatted += padding + node + '\r\n';
            pad += indent;
        });
    return formatted;
};

Europa.Controllers.LogWebService.ExcluirLogs = function () {
    var filtro = {
        Endpoint: $("#form_filtro_log_webservice").find("#Endpoint").val(),
        Operacao: $("#form_filtro_log_webservice").find("#Action").val(),
        Conteudo: $("#form_filtro_log_webservice").find("#Content").val(),
        HorarioInicio: $("#form_filtro_log_webservice").find("#HorarioInicio").val(),
        HorarioFim: $("#form_filtro_log_webservice").find("#HorarioFim").val(),
        Stage: $("#form_filtro_log_webservice").find("#Stage").val()
    };

    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Confirmacao, Europa.i18n.Messages.ConfirmacaoExclusaoLogs);
    Europa.Confirmacao.ConfirmCallback = function () {
        $.post(Europa.Controllers.LogWebService.UrlExcluir, { filtro: filtro }, function (res) {
            if (res.Sucesso) {
                Europa.Messages.ShowMessages(res, Europa.i18n.Messages.Sucesso);
                Europa.Components.Datatable.LogWebService.DataTable.reloadData();
            }
        });
    }
    Europa.Confirmacao.Show();
};