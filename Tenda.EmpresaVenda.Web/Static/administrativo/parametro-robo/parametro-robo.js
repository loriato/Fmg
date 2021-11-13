//Controller
Europa.Controllers.ParametroRobo = {};
Europa.Controllers.ParametroRobo.Tabela = {};
Europa.Controllers.ParametroRobo.TabelaExtensoes = {};
Europa.Controllers.ParametroRobo.UrlIncluir = undefined;
Europa.Controllers.ParametroRobo.UrlAtualizar = undefined;
Europa.Controllers.ParametroRobo.UrlExecutar = undefined;
Europa.Controllers.ParametroRobo.UrlListarExecucoes = undefined;
Europa.Controllers.ParametroRobo.UrlQuartzDropdown = undefined;
Europa.Controllers.ParametroRobo.UrlBuscarQuartz = undefined;
Europa.Controllers.ParametroRobo.IdQuartz = undefined;

//Eventos que serao disparados quando a pagina for carregada
$(document).ready(function () {
    Europa.Components.DatePicker.AutoApply();
    Europa.Controllers.ParametroRobo.LimparFiltros();
    setTimeout(Europa.Controllers.ParametroRobo.ConfigDatePicker, 1000);

    var id = $("#formParametroRobo").find("#Quartz_Id").val();
    if (id !== undefined && id > 0) {
        Europa.Controllers.ParametroRobo.Tabela.reloadData();
        $("#btnExecutarRobo").css("display", "inline-block");
        $("#btnExcluirLog").css("display", "inline-block");
    }


    $("#idQuartz").change(function () {
        Europa.Controllers.ParametroRobo.OnChangeQuartz();
    });
});


//
// DataTable
//
Europa.Controllers.ParametroRobo.Detalhar = function (idExecucao) {
    Europa.Components.LogExecucaoModal.AbrirModal(idExecucao);
};

Europa.Controllers.ParametroRobo.LimparFiltros = function () {
    $('#DataInicio').val("");
    $('#DataFim').val("");
};

Europa.Controllers.ParametroRobo.ConfigDatePicker = function () {
    var dataInicio = Europa.Date.Format(Europa.Date.AddDay(-7, Europa.Date.Now()), Europa.Date.FORMAT_DATE);
    Europa.Controllers.ParametroRobo.Horario1 = new Europa.Components.DatePicker()
        .WithTarget('#DataInicio')
        .WithFormat("DD/MM/YYYY HH:mm")
        .WithValue(dataInicio + " 00:00")
        .WithTimePicker()
        .WithTimePicker24h()
        .WithTimePickerIncrement(1)
        .Configure();
    Europa.Controllers.ParametroRobo.Horario2 = new Europa.Components.DatePicker()
        .WithTarget('#DataFim')
        .WithFormat("DD/MM/YYYY HH:mm")
        .WithTimePicker()
        .WithTimePicker24h()
        .WithTimePickerIncrement(1)
        .Configure();

};

Europa.Controllers.ParametroRobo.TabelaExtensoes.ParametrosFiltro = function () {
    return {
        dataInicio: $('#DataInicio').val(),
        dataFim: $('#DataFim').val(),
        idQuartz: $("#formParametroRobo").find("#Quartz_Id").val()
    };
};

Europa.Controllers.ParametroRobo.TabelaExtensoes.Filtrar = function () {
    if ($("#formParametroRobo").find("#Quartz_Id").val() > 0) {
        Europa.Controllers.ParametroRobo.Tabela.reloadData();
    }
};

DataTableApp.controller('listaExecucoes', listaExecucoes);

function listaExecucoes($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.ParametroRobo.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.ParametroRobo.Tabela;

    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('DataInicioExecucao').withTitle(Europa.i18n.Messages.Inicio).withOption("width", "20%").withOption('type', 'date-format-DD/MM/YYYY HH:mm:ss'),
        DTColumnBuilder.newColumn('DataTerminoExecucao').withTitle(Europa.i18n.Messages.Fim).withOption("width", "20%").renderWith(formatDate),
        DTColumnBuilder.newColumn('Log').withTitle(Europa.i18n.Messages.Log).withOption("width", "55%")
    ])
        .setDefaultOrder([[1, 'desc']])
        .setIdAreaHeader("datatable_barra")
        .setColActions(actionsHtml, '90px')
        .setAutoInit(false)
        .setDefaultOptions('POST', Europa.Controllers.ParametroRobo.UrlListarExecucoes, Europa.Controllers.ParametroRobo.TabelaExtensoes.ParametrosFiltro);

    function actionsHtml(data) {
        return '<button class="btn btn-default" onclick="Europa.Controllers.ParametroRobo.Detalhar(' + data.Id + ')"/>' +
            '<i class="fa fa-file-text-o"></i>' +
            '</button>';
    };

    function formatDate(date) {
        if (date) {
            return Europa.Date.toFormatDate(date, "DD/MM/YYYY HH:mm:ss");
        }
        return "";
    }
};


Europa.Controllers.ParametroRobo.GetFormData = function () {
    var data = {
        Id: $("#formParametroRobo").find("#Quartz_Id").val(),
        Nome: $("#formParametroRobo").find("#Quartz_Nome").val(),
        CaminhoCompleto: $("#formParametroRobo").find("#Quartz_CaminhoCompleto").val(),
        ServidorExecucao: $("#formParametroRobo").find("#Quartz_ServidorExecucao").val(),
        Cron: $("#formParametroRobo").find("#Quartz_Cron").val(),
        IniciarAutomaticamente: $("#formParametroRobo").find("#Quartz_IniciarAutomaticamente").val(),
        Observacoes: $("#formParametroRobo").find("#Quartz_Observacoes").val(),
        SiteExecucao: $("#formParametroRobo").find("#Quartz_SiteExecucao").val(),
        AplicacaoExecucao: $("#formParametroRobo").find("#Quartz_AplicacaoExecucao").val(),
        ForcarDesligamento: $("#formParametroRobo").find("#Quartz_ForcarDesligamento").val()
    };
    return data;
}

Europa.Controllers.ParametroRobo.SubmitForm = function () {
    var data = Europa.Controllers.ParametroRobo.GetFormData();
    $.ajax({
        type: "Post",
        url: data.Id === 0
            ? Europa.Controllers.ParametroRobo.UrlIncluir
            : Europa.Controllers.ParametroRobo.UrlAtualizar,
        data: data
    }).done(function (response) {
        if (response.Sucesso) {
            $("#formParametroRobo").find("#Quartz_Id").val(response.Objeto.Id);
            $("#btnExcluirLog").css("display", "inline-block");
            $("#btnExecutarRobo").css("display", "inline-block");
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Sucesso,
                Europa.String.Format(Europa.i18n.Messages.ParametroSalvoSucesso, $("#Quartz_Nome").val()));
            Europa.Informacao.Show();
        } else {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro,
                Europa.Controllers.ParametroRobo.FormatarMensagem(response.Mensagens));
            Europa.Informacao.Show();
        }
    });
};

Europa.Controllers.ParametroRobo.Executar = function () {
    var data = Europa.Controllers.ParametroRobo.GetFormData();
    $.post(Europa.Controllers.ParametroRobo.UrlExecutar, data, function (res) {
        Europa.Messages.ShowMessages(res);
    });
};

Europa.Controllers.ParametroRobo.FormatarMensagem = function (msgs) {
    var mensagensHtml = $('<div/>');
    if ($.isArray(msgs) && msgs.length > 0) {
        msgs.map(function (x) {
            mensagensHtml.append($('<p/>').text(x));
        });
    } else {
        mensagensHtml.append($('<p/>').text(msgs));
    }
    return mensagensHtml;
};

Europa.Controllers.ParametroRobo.ExcluirLogs = function () {
    var id = $("#formParametroRobo").find("#Quartz_Id").val();
    Europa.Components.ExcluirExecucaoModal.AbrirModal(id,
        function () {
            Europa.Controllers.ParametroRobo.Tabela.reloadData();
        });
};


Europa.Controllers.ParametroRobo.OnChangeQuartz = function () {
    var idTemp = $("#idQuartz").val();
    if (idTemp !== Europa.Controllers.ParametroRobo.IdQuartz) {
        $.ajax({
            type: "Post",
            url: Europa.Controllers.ParametroRobo.UrlBuscarQuartz,
            data: {
                idQuartz: idTemp
            }
        }).done(function (response) {
            Europa.Controllers.ParametroRobo.PreencherForm(response);
            $("#fieldsetForm").removeAttr("disabled");
            $("#btnExcluirLog").css("display", "inline-block");
            $("#btnExecutarRobo").css("display", "inline-block");
            Europa.Controllers.ParametroRobo.Tabela.reloadData();
        });
        Europa.Controllers.ParametroRobo.IdQuartz = idTemp;
    }
};

Europa.Controllers.ParametroRobo.PreencherForm = function (data) {
    $("#formParametroRobo").find("#Quartz_Id").val(data.Id);
    $("#formParametroRobo").find("#Quartz_Nome").val(data.Nome);
    $("#formParametroRobo").find("#Quartz_CaminhoCompleto").val(data.CaminhoCompleto);
    $("#formParametroRobo").find("#Quartz_ServidorExecucao").val(data.ServidorExecucao);
    $("#formParametroRobo").find("#Quartz_Cron").val(data.Cron);
    var iniciarAutomaticamente = (data.IniciarAutomaticamente + "").toLowerCase();
    $("#formParametroRobo").find("#Quartz_IniciarAutomaticamente").val(iniciarAutomaticamente).change();
    $("#formParametroRobo").find("#Quartz_SiteExecucao").val(data.SiteExecucao);
    $("#formParametroRobo").find("#Quartz_AplicacaoExecucao").val(data.AplicacaoExecucao);
    var forcarDesligamento = (data.ForcarDesligamento + "").toLowerCase();
    $("#formParametroRobo").find("#Quartz_ForcarDesligamento").val(forcarDesligamento);
    $("#formParametroRobo").find("#Quartz_Observacoes").val(data.Observacoes);
}