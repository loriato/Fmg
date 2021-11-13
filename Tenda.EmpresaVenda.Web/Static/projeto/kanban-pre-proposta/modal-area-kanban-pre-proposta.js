$(function () {

});

Europa.Controllers.KanbanPreProposta.AbrirModalAreaKanban = function () {

    $.get(Europa.Controllers.KanbanPreProposta.UrlFormularioAreaKanbanPreProposta + "?idAreaKanbanPreProposta=0", function (res) {
        
        if (res) {
            $("#div-area-kanban").html(res);

            Europa.Controllers.KanbanPreProposta.OnChangeColorPicker();
            Europa.Controllers.KanbanPreProposta.OnChangeTituloArea();

            $("#modal_area_kanban").show()
        }        
    });    
}

Europa.Controllers.KanbanPreProposta.FecharModalAreaKanban = function () {
    $("#modal_area_kanban").hide()
}

Europa.Controllers.KanbanPreProposta.SalvarAreaKanbanPreProposta = function () {
    var data = {};
    data = Europa.Form.SerializeJson("#form_area_kanban");
    console.log(data);

    $.post(Europa.Controllers.KanbanPreProposta.UrlSalvarAreaKanbanPreProposta, data, function (res) {
        if (res.Success) {
            Europa.Informacao.Hide = function () {
                location.reload(true);
            }
        }
        Europa.Informacao.PosAcaoApi(res);
    });
}

Europa.Controllers.KanbanPreProposta.EditarAreaKanban = function (idArea) {
    $.get(Europa.Controllers.KanbanPreProposta.UrlFormularioAreaKanbanPreProposta + "?idAreaKanbanPreProposta=" + idArea, function (res) {

        if (res) {
            $("#div-area-kanban").html(res);

            Europa.Controllers.KanbanPreProposta.OnChangeColorPicker();
            Europa.Controllers.KanbanPreProposta.OnChangeTituloArea();

            $("#modal_area_kanban").show()
        }
    });
};

Europa.Controllers.KanbanPreProposta.ExcluirAreaKanban = function (idArea) {
    Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.Excluir, "Ao remover esta área, toda sua configuração também será removida", Europa.i18n.Messages.Confirmar,
        function () {

            $.post(Europa.Controllers.KanbanPreProposta.UrlExcluirAreaKanbanPreProposta, { idAreaKanbanPreProposta: idArea }, function (res) {
                if (res.Success) {
                    Europa.Informacao.Hide = function () {
                        location.reload(true)
                    }
                }
                Europa.Informacao.PosAcaoApi(res);
            });
        }
    );
    Europa.Confirmacao.Show();
};