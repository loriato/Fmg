Europa.Controllers.Loja.StatusImportacao = {};
Europa.Controllers.Loja.StatusImportacao.ModalId = "StatusImportacao";
Europa.Controllers.Loja.StatusImportacao.Modal = undefined;
Europa.Controllers.Loja.StatusImportacao.Task = undefined;

Europa.Controllers.Loja.StatusImportacao.Show = function(taskObject) {
    Europa.Controllers.Loja.StatusImportacao.Task = taskObject;
    $("#StatusImportacao_Log").html("");
    $("#StatusImportacao_Status").html("");
    $("#StatusImportacao_Download").hide();
    Europa.Controllers.Loja.StatusImportacao.VerificarStatusImportacao();
};

Europa.Controllers.Loja.StatusImportacao.Hide = function() {
    Europa.Controllers.Loja.StatusImportacao.Modal.modal("hide");
};

$(function() {
    Europa.Controllers.Loja.StatusImportacao.Modal = $("#" + Europa.Controllers.Loja.StatusImportacao.ModalId);
});

Europa.Controllers.Loja.StatusImportacao.VerificarStatusImportacao = function () {
    Europa.Controllers.Loja.StatusImportacao.Modal.modal("show");
    $.ajax({
        url: Europa.Controllers.Loja.StatusImportacao.UrlVerificarTask,
        method: 'POST',
        data: { taskId: Europa.Controllers.Loja.StatusImportacao.Task.TaskId }
    }).done(function (response) {
        if (response.Task !== undefined) {
            Europa.Controllers.Loja.StatusImportacao.Task = response.Task;
        }
        Europa.Controllers.Loja.StatusImportacao.UpdateTaskProgress();
    });
};

Europa.Controllers.Loja.StatusImportacao.UpdateTaskProgress = function () {
    var task = Europa.Controllers.Loja.StatusImportacao.Task;

    var log = task.FullLog.replace(new RegExp('\r?\n', 'g'), '<br />');

    var status;
    if ("FINISHED" === task.Status) {
        $("#StatusImportacao_Download").show();
        status = "foi finalizada com sucesso.";
        Europa.Controllers.Loja.Tabela.reloadData();
    } else if ("PROCESSING" === task.Status) {
        status = "está em execução.";
        setTimeout(Europa.Controllers.Loja.StatusImportacao.VerificarStatusImportacao, 3000);
    } else if ("ERROR" === task.Status) {
        status = "foi finalizada por conta de um erro fatal.";
        log = task.Error.Message + "<br/>" + log;
    }
    var userStatus = task.CurrentLine + " linhas processadas de um total de " + task.TotalLines + ".<br/>A rotina " + status;

    $("#StatusImportacao_Status").html(userStatus);
    $("#StatusImportacao_Log").html(log);
};

Europa.Controllers.Loja.StatusImportacao.CalculateSpeed = function() {
    var task = Europa.Controllers.Loja.StatusImportacao.Task;

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

Europa.Controllers.Loja.StatusImportacao.DownloadArquivo = function () {
    if (Europa.Controllers.Loja.StatusImportacao.Task === undefined) {
        return;
    }
    var params = { taskId: Europa.Controllers.Loja.StatusImportacao.Task.TaskId };
    var formExportar = $("#StatusImportacao_Exportar");
    formExportar.find("input").remove();
    formExportar.attr('method', 'post').attr('action', Europa.Controllers.Loja.StatusImportacao.UrlDownloadArquivo);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};