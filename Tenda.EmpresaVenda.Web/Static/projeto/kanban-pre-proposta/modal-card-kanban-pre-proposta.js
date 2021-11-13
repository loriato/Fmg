$(function () {
})

Europa.Controllers.KanbanPreProposta.AbrirModalCardKanban = function (idArea) {

    $.get(Europa.Controllers.KanbanPreProposta.UrlFormularioCardKanbanPreProposta + "?idCardKanbanPreProposta=0&idAreaKanbanPreProposta=" + idArea, function (res) {

        if (res) {
            $("#div-card-kanban").html(res);

            Europa.Controllers.KanbanPreProposta.OnChangeColorPicker();
            Europa.Controllers.KanbanPreProposta.OnChangeTituloArea();

            $("#modal_card_kanban").show()
        }
    });

    Europa.Controllers.KanbanPreProposta.OnChangeColorPicker();
    Europa.Controllers.KanbanPreProposta.OnChangeTituloArea();

    $("#modal_card_kanban").show();
}

Europa.Controllers.KanbanPreProposta.FecharModalCardKanban = function () {
    $("#modal_card_kanban").hide();
}

Europa.Controllers.KanbanPreProposta.AdicionarCardKanban = function (idArea) {

    Europa.Controllers.KanbanPreProposta.AbrirModalCardKanban(idArea);
    $("#IdAreaKanban", "#modal_card_kanban").val(idArea);
}

Europa.Controllers.KanbanPreProposta.SalvarCardKanbanPreProposta = function () {
    var data = {};
    data = Europa.Form.SerializeJson("#form_card_kanban");
    console.log(data)

    $.post(Europa.Controllers.KanbanPreProposta.UrlSalvarCardKanbanPreProposta, data, function (res) {
        if (res.Success) {
            Europa.Informacao.Hide = function () {
                location.reload(true);
            }
        }
        Europa.Informacao.PosAcaoApi(res);
    });
}

Europa.Controllers.KanbanPreProposta.EditarCardKanbanPreProposta = function (idCardKanban) {
    $.get(Europa.Controllers.KanbanPreProposta.UrlFormularioCardKanbanPreProposta + "?idCardKanbanPreProposta=" + idCardKanban, function (res) {

        if (res) {
            $("#div-card-kanban").html(res);

            Europa.Controllers.KanbanPreProposta.OnChangeColorPicker();
            Europa.Controllers.KanbanPreProposta.OnChangeTituloArea();

            $("#modal_card_kanban").show();
        }
    });
};

function Datatable($scope, $attrs, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

    var idCardKanban = $attrs.id;
    var tabelaWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);

    Europa.Controllers.KanbanPreProposta.DatatableCards.push({ IdCardKanban: parseInt(idCardKanban), Datatable: tabelaWrapper });

    tabelaWrapper
        .setColumns([
            DTColumnBuilder.newColumn('StatusPortalHouse').withTitle("Status Portal House").withOption('width', '90%'),            
        ])
        .setIdAreaHeader("datatable_header_" + idCardKanban)
        .setDefaultOptions('POST', Europa.Controllers.KanbanPreProposta.UrlListarSituacaoCardKanban, filtrar);

    function filtrar() {
        var form = {
            IdCardKanbanPreProposta: idCardKanban
        };
        return form;
    }    
};

DataTableApp.controller('Datatable', Datatable);

Europa.Controllers.KanbanPreProposta.RemoverCardKanbanPreProposta = function (idCardKanban) {
    Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.Excluir, "Ao remover este card, toda sua configuração também será removida", Europa.i18n.Messages.Confirmar,
        function () {

            $.post(Europa.Controllers.KanbanPreProposta.UrlRemoverCardKanbanPreProposta, { idCardKanbanPreProposta: idCardKanban }, function (res) {
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
}