Europa.Controllers.StatusImportacao = {};
Europa.Controllers.StatusImportacao.ModalId = "StatusImportacao";
Europa.Controllers.StatusImportacao.Modal = undefined;
Europa.Controllers.StatusImportacao.Task = undefined;

Europa.Controllers.StatusImportacao.Show = function (taskObject) {
    Europa.Controllers.StatusImportacao.Task = taskObject;
    $("#StatusImportacao_Log").html("");
    $("#StatusImportacao_Status").html("");
    $("#StatusImportacao_Download").hide();
    Europa.Controllers.StatusImportacao.VerificarStatusImportacao();
};

Europa.Controllers.StatusImportacao.Hide = function () {
    Europa.Controllers.StatusImportacao.Modal.modal("hide");
};

$(function () {
    Europa.Controllers.StatusImportacao.Modal = $("#" + Europa.Controllers.StatusImportacao.ModalId);
});

Europa.Controllers.StatusImportacao.VerificarStatusImportacao = function () {
    Europa.Controllers.StatusImportacao.Modal.modal("show");
    $.ajax({
        url: Europa.Controllers.StatusImportacao.UrlVerificarTask,
        method: 'POST',
        data: { taskId: Europa.Controllers.StatusImportacao.Task.TaskId }
    }).done(function (response) {
        if (response.Task !== undefined) {
            Europa.Controllers.StatusImportacao.Task = response.Task;
        }
        Europa.Controllers.StatusImportacao.UpdateTaskProgress();
    });
};

Europa.Controllers.StatusImportacao.UpdateTaskProgress = function () {
    var task = Europa.Controllers.StatusImportacao.Task;

    var log = task.FullLog.replace(new RegExp('\r?\n', 'g'), '<br />');

    var status;
    if ("FINISHED" === task.Status) {
        $("#StatusImportacao_Download").show();
        status = "foi finalizada com sucesso.";
    } else if ("PROCESSING" === task.Status) {
        status = "está em execução.";
        setTimeout(Europa.Controllers.StatusImportacao.VerificarStatusImportacao, 3000);
    } else if ("ERROR" === task.Status) {
        status = "foi finalizada por conta de um erro fatal.";
        console.log(task.Error);
        log = task.Error.Message + "<br/>" + log;
    }
    var userStatus = task.CurrentLine + " linhas processadas de um total de " + task.TotalLines + ".<br/>A rotina " + status;

    $("#StatusImportacao_Status").html(userStatus);
    $("#StatusImportacao_Log").html(log);
};

Europa.Controllers.StatusImportacao.CalculateSpeed = function () {
    var task = Europa.Controllers.StatusImportacao.Task;

    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(task.LastUpdate);
    var lastUpdate = new Date(parseFloat(results[1]));

    results = pattern.exec(task.Start);
    var start = new Date(parseFloat(results[1]));

    var miliseconds = lastUpdate.getTime() - start.getTime();
    var seconds = miliseconds / 1000;

    var speed = task.CurrentLine / seconds;

    return speed;
}

Europa.Controllers.StatusImportacao.DownloadArquivo = function () {
    if (Europa.Controllers.StatusImportacao.Task === undefined) {
        return;
    }
    var params = { taskId: Europa.Controllers.StatusImportacao.Task.TaskId };
    var formExportar = $("#StatusImportacao_Exportar");
    formExportar.find("input").remove();
    formExportar.attr('method', 'post').attr('action', Europa.Controllers.StatusImportacao.UrlDownloadArquivo);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};