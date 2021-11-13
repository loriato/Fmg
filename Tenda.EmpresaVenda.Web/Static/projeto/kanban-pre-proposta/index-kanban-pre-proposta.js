Europa.Controllers.KanbanPreProposta = {};
Europa.Controllers.KanbanPreProposta.IdCardKanbanPreProposta = null;
Europa.Controllers.KanbanPreProposta.DatatableCards = [];

$(function () {
    Europa.Controllers.KanbanPreProposta.AutoCompleteSituacaoKanbanPreProposta = new Europa.Components.AutoCompleteSituacaoKanbanPreProposta()
        .WithTargetSuffix("situacao_card_kanban")
        .Configure();
});


Europa.Controllers.KanbanPreProposta.OnChangeColorPicker = function () {
    var corA = $("#Cor", "#modal_area_kanban").val();
    $("#preview", "#modal_area_kanban").css("background", corA);

    var corE = $("#Cor", "#modal_card_kanban").val();
    $("#preview", "#modal_card_kanban").css("background", corE);
}

Europa.Controllers.KanbanPreProposta.OnChangeTituloArea = function () {
    $("#Descricao", "#modal_area_kanban").keyup(function (x) {
        var titulo = $("#Descricao", "#modal_area_kanban").val();
        $("#preview", "#modal_area_kanban").html(titulo);
    });

    $("#Descricao", "#modal_card_kanban").keyup(function (x) {
        var titulo = $("#Descricao", "#modal_card_kanban").val();
        $("#preview", "#modal_card_kanban").html(titulo);
    });

    var titulo = $("#Descricao", "#modal_card_kanban").val();
    $("#preview", "#modal_card_kanban").html(titulo);

    var titulo = $("#Descricao", "#modal_area_kanban").val();
    $("#preview", "#modal_area_kanban").html(titulo);
}

//Área Kanban

//Área Kanban


//Modal Adicionar Situação ao Card

Europa.Controllers.KanbanPreProposta.AbrirModalSituacaoCardKanban = function () {
    $("#modal_situacao_card_kanban").show();
}

Europa.Controllers.KanbanPreProposta.FecharModalSituacaoCardKanban = function () {
    window.location.reload(true)
}

function DatatableSituacaoCardKanban($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.KanbanPreProposta.DatatableSituacaoCardKanban = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.KanbanPreProposta.DatatableSituacaoCardKanban;
    self.setColumns([
        DTColumnBuilder.newColumn('StatusPortalHouse').withTitle("Status Portal House").withOption('width', '80%'),
        DTColumnBuilder.newColumn('Cor').withTitle("Cor").withOption('width', '10%').renderWith(renderColor),
    ])
        .setAutoInit(false)
        //.setDefaultOrder([[7, 'asc']])
        .setColActions(actionsHtml, '100px')
        .setDefaultOptions('POST', Europa.Controllers.KanbanPreProposta.UrlListarSituacaoCardKanban, Europa.Controllers.KanbanPreProposta.FiltroDatatableSituacaoCardKanban);

    function actionsHtml(data, type, full, meta) {
        var button = '<div>';

        button += self.renderButton(Europa.Controllers.KanbanPreProposta.Permissao.Excluir, "Remover", "fa fa-trash", 'Remover(' + meta.row + ')', false);

        button += '</div>';

        return button;
    }

    function renderColor(data) {
        return '<div style="background-color:' + data +';min-width:inherit;min-height:20px"></div>'
    }
        
    $scope.Remover = function (row) {
        var full = Europa.Controllers.KanbanPreProposta.DatatableSituacaoCardKanban.getRowData(row);
        Europa.Controllers.KanbanPreProposta.RemoverSituacaoCardKanban(full);
    };

}

DataTableApp.controller('DatatableSituacaoCardKanban', DatatableSituacaoCardKanban);

Europa.Controllers.KanbanPreProposta.FiltroDatatableSituacaoCardKanban = function () {
    var param = {
        IdCardKanbanPreProposta: Europa.Controllers.KanbanPreProposta.IdCardKanbanPreProposta
    };

    return param;
}

Europa.Controllers.KanbanPreProposta.FiltrarDatatableSituacaoCardKanban = function () {
    Europa.Controllers.KanbanPreProposta.DatatableSituacaoCardKanban.reloadData();
};

Europa.Controllers.KanbanPreProposta.NovaSituacaoCardKanban = function (idCardKanban) {
    console.log(idCardKanban)
    Europa.Controllers.KanbanPreProposta.IdCardKanbanPreProposta = idCardKanban;
    Europa.Controllers.KanbanPreProposta.AbrirModalSituacaoCardKanban();
    Europa.Controllers.KanbanPreProposta.FiltroDatatableSituacaoCardKanban()
    Europa.Controllers.KanbanPreProposta.FiltrarDatatableSituacaoCardKanban();
}

Europa.Controllers.KanbanPreProposta.AdicionarSituacaoCardKanban = function () {

    var rgb = document.getElementById("card_" + Europa.Controllers.KanbanPreProposta.IdCardKanbanPreProposta).style.backgroundColor;

    var data = {
        IdStatusPreProposta: $("#autocomplete_situacao_card_kanban").val(),
        IdCardKanbanPreProposta: Europa.Controllers.KanbanPreProposta.IdCardKanbanPreProposta,
        Cor: rgb
    }

    $.post(Europa.Controllers.KanbanPreProposta.UrlAdicionarSituacaoCardKanban, data, function (res) {
        if (res.Success) {
            $("#autocomplete_situacao_card_kanban").val("").trigger("change");
            Europa.Controllers.KanbanPreProposta.FiltrarDatatableSituacaoCardKanban(rgb);
        }
        Europa.Informacao.PosAcaoApi(res);
    });
}

Europa.Controllers.KanbanPreProposta.RemoverSituacaoCardKanban = function (full) {
    console.log(full)
    Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.Excluir, "Deseja remover a situação " + full.StatusPortalHouse+"?", Europa.i18n.Messages.Confirmar,
        function () {
            $.post(Europa.Controllers.KanbanPreProposta.UrlRemoverSituacaoCardKanban, { IdCardKanbanSituacaoPreProposta: full.Id }, function (res) {
                if (res.Success) {
                    Europa.Controllers.KanbanPreProposta.FiltrarDatatableSituacaoCardKanban();
                    Europa.Controllers.KanbanPreProposta.DatatableCards.forEach(function (datatable) { datatable.Datatable.reloadData(); });
                }
                Europa.Informacao.PosAcaoApi(res);
            });
        }
    );
    Europa.Confirmacao.Show();
}

Europa.Controllers.KanbanPreProposta.Limpar = function () {
    $("#autocomplete_situacao_card_kanban").val("").trigger("change");
}
//Modal Adicionar Situação ao Card
