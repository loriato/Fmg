Europa.Components.ExcluirExecucaoModal = {};
Europa.Components.ExcluirExecucaoModal.UrlExcluir = undefined;
Europa.Components.ExcluirExecucaoModal.Form = "#form_excluir_log";
Europa.Components.ExcluirExecucaoModal.Callback = undefined;

$(document).ready(function () {
    Europa.Components.DatePicker.AutoApply();
});

Europa.Components.ExcluirExecucaoModal.AbrirModal = function (idquartz, callback) {
    Europa.Components.ExcluirExecucaoModal.Callback = callback;
    $("#IdExcluirQuartz", Europa.Components.ExcluirExecucaoModal.Form).val(idquartz);
    $("#DataExcluirDe", Europa.Components.ExcluirExecucaoModal.Form).val("");
    $("#DataExcluirAte", Europa.Components.ExcluirExecucaoModal.Form).val("");
    $("#dialog_excluir_log").modal("show");
};

Europa.Components.ExcluirExecucaoModal.CloseModal = function () {
    $("#dialog_excluir_log").modal("hide");
};
Europa.Components.ExcluirExecucaoModal.Excluir = function () {
    var data = {
        id: $("#IdExcluirQuartz", Europa.Components.ExcluirExecucaoModal.Form).val(),
        de: $("#DataExcluirDe", Europa.Components.ExcluirExecucaoModal.Form).val(),
        ate: $("#DataExcluirAte", Europa.Components.ExcluirExecucaoModal.Form).val()
    };
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoExcluiLog, data.de,data.ate));
    Europa.Confirmacao.ConfirmCallback = function () {
        $.post(Europa.Components.ExcluirExecucaoModal.UrlExcluir,
       data,
       function (res) {
           if (res.Sucesso) {
               Europa.Components.ExcluirExecucaoModal.CloseModal();
               if (Europa.Components.ExcluirExecucaoModal.Callback) {
                   Europa.Components.ExcluirExecucaoModal.Callback();
               }
           }
           Europa.Messages.ShowMessages(res);
       });
    }
    Europa.Confirmacao.Show();
   
};
