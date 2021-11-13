Europa.Controllers.PropostaFaturada.ModalId = "PropostaFaturada";
Europa.Controllers.PropostaFaturada.Modal = undefined;
Europa.Controllers.PropostaFaturada.Task = undefined;

$(function () {
});

Europa.Controllers.PropostaFaturada.ShowStatusImportacao = function (taskObject) {
    
    Europa.Controllers.PropostaFaturada.Task = taskObject;
    $("#PropostaFaturada_Log").html("");
    $("#PropostaFaturada_Status").html("");
    $("#PropostaFaturada_Download").hide();
    Europa.Controllers.PropostaFaturada.VerificarPropostaFaturada();
};

Europa.Controllers.PropostaFaturada.Hide = function () {
    $("#PropostaFaturada").hide();
};

Europa.Controllers.PropostaFaturada.VerificarPropostaFaturada = function () {
    
    $("#PropostaFaturada").show();
    $.ajax({
        url: Europa.Controllers.PropostaFaturada.UrlVerificarTask,
        method: 'POST',
        data: { taskId: Europa.Controllers.PropostaFaturada.Task.TaskId }
    }).done(function (response) {
        if (response.Task !== undefined) {
            Europa.Controllers.PropostaFaturada.Task = response.Task;
        }
        Europa.Controllers.PropostaFaturada.UpdateTaskProgress();
    });
};

Europa.Controllers.PropostaFaturada.UpdateTaskProgress = function () {
    var task = Europa.Controllers.PropostaFaturada.Task;

    var log = task.FullLog.replace(new RegExp('\r?\n', 'g'), '<br />');

    var status;
    if ("FINISHED" === task.Status) {
        $("#PropostaFaturada_Download").show();
        status = "foi finalizada com sucesso.";
    } else if ("PROCESSING" === task.Status) {
        status = "está em execução.";
        setTimeout(Europa.Controllers.PropostaFaturada.VerificarPropostaFaturada, 3000);
    } else if ("ERROR" === task.Status) {
        status = "foi finalizada por conta de um erro fatal.";
        console.log(task.Error);
        log = task.Error.Message + "<br/>" + log;
    }
    var userStatus = task.CurrentLine + " linhas processadas de um total de " + task.TotalLines + ".<br/>A rotina " + status;

    $("#PropostaFaturada_Status").html(userStatus);
    $("#PropostaFaturada_Log").html(log);
};

Europa.Controllers.PropostaFaturada.CalculateSpeed = function () {
    var task = Europa.Controllers.PropostaFaturada.Task;

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

Europa.Controllers.PropostaFaturada.DownloadArquivo = function () {
    if (Europa.Controllers.PropostaFaturada.Task === undefined) {
        return;
    }
    var params = { taskId: Europa.Controllers.PropostaFaturada.Task.TaskId };
    var formExportar = $("#PropostaFaturada_Exportar");
    formExportar.find("input").remove();
    formExportar.attr('method', 'post').attr('action', Europa.Controllers.PropostaFaturada.UrlDownloadArquivo);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};