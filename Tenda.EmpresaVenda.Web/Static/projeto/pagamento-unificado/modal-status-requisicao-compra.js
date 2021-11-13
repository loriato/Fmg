$(function () {
    Europa.Controllers.PagamentoUnificado.Modal = $("#PagamentoUnificado");
});

Europa.Controllers.PagamentoUnificado.ShowStatusRequisicaoCompra = function (taskObject) {
    Europa.Controllers.PagamentoUnificado.Task = taskObject;
    $("#PagamentoUnificado_Log").html("");
    $("#PagamentoUnificado_Status").html("");
    $("#PagamentoUnificado_Download").hide();
    Europa.Controllers.PagamentoUnificado.VerificarPagamentoUnificado();
}

Europa.Controllers.PagamentoUnificado.HidePagamentoUnificado = function () {
    Europa.Controllers.PagamentoUnificado.Modal.modal("hide");
    Europa.Controllers.PagamentoUnificado.Filtrar();
};

Europa.Controllers.PagamentoUnificado.VerificarPagamentoUnificado = function () {
    Europa.Controllers.PagamentoUnificado.Modal.modal("show");
    
    $.ajax({
        url: Europa.Controllers.PagamentoUnificado.UrlVerificarTask,
        method: 'POST',
        data: { taskId: Europa.Controllers.PagamentoUnificado.Task.TaskId }
    }).done(function (response) {
        if (response.Task !== undefined) {
            Europa.Controllers.PagamentoUnificado.Task = response.Task;
        }
        Europa.Controllers.PagamentoUnificado.UpdateTaskProgress();
    });
};

Europa.Controllers.PagamentoUnificado.UpdateTaskProgress = function () {
    var task = Europa.Controllers.PagamentoUnificado.Task;

    var log = task.FullLog.replace(new RegExp('\r?\n', 'g'), '<br />');

    var status;
    if ("FINISHED" === task.Status) {
        $("#PagamentoUnificado_Download").show();
        status = "foi finalizada com sucesso.";
    } else if ("PROCESSING" === task.Status) {
        status = "está em execução.";
        setTimeout(Europa.Controllers.PagamentoUnificado.VerificarPagamentoUnificado, 3000);
    } else if ("ERROR" === task.Status) {
        status = "foi finalizada por conta de um erro fatal.";
        console.log(task.Error);
        log = task.Error.Message + "<br/>" + log;
    }
    var userStatus = task.CurrentLine + " linhas processadas de um total de " + task.TotalLines + ".<br/>A rotina " + status;

    $("#PagamentoUnificado_Status").html(userStatus);
    $("#PagamentoUnificado_Log").html(log);
};

Europa.Controllers.PagamentoUnificado.DownloadArquivo = function () {
    if (Europa.Controllers.PagamentoUnificado.Task === undefined) {
        return;
    }
    var params = { taskId: Europa.Controllers.PagamentoUnificado.Task.TaskId };
    var formExportar = $("#PagamentoUnificado_Exportar");
    formExportar.find("input").remove();
    formExportar.attr('method', 'post').attr('action', Europa.Controllers.PagamentoUnificado.UrlDownloadArquivo);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};