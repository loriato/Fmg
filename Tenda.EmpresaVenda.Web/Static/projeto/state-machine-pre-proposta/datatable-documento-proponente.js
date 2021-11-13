$(function () {

})

function DatatableDocumentoProponente($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.StateMachinePreProposta.DocumentoProponenteTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.StateMachinePreProposta.DocumentoProponenteTable;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('NomeTipoDocumento').withTitle(Europa.i18n.Messages.TipoDocumento),
            DTColumnBuilder.newColumn('ObrigatorioPortal').withTitle(Europa.i18n.Messages.ObrigatorioPortal).renderWith(renderObrigatorioPortal),
            DTColumnBuilder.newColumn('ObrigatorioHouse').withTitle(Europa.i18n.Messages.ObrigatorioLoja).renderWith(renderObrigatorioHouse),

        ])
        .setAutoInit(false)
        .setIdAreaHeader("documento_proponente_datatable_barra")
        .setDefaultOrder([[0, 'asc']])
        .setOptionsSelect('POST', Europa.Controllers.StateMachinePreProposta.UrlListarDocumentoProponenteRule, Europa.Controllers.StateMachinePreProposta.FiltroListarDocumentoProponenteRule);

    function renderObrigatorioPortal(data, type, full, meta) {
        var checkBox = '<label>';
        checkBox += '<input type = "checkbox" value = "' + data + '"';

        if (data) {
            checkBox += ' checked="checked"';
        }

        if (!Europa.Controllers.StateMachinePreProposta.Permissoes.Incluir) {
            checkBox += ' disabled ';
        }
        checkBox += 'onchange = "Europa.Controllers.StateMachinePreProposta.OnJoinTipoDocumento(' + meta.row + ', 1)" /> ';
        checkBox += '</label>';

        return checkBox;
    }

    function renderObrigatorioHouse(data, type, full, meta) {
        var checkBox = '<label>';
        checkBox += '<input type = "checkbox" value = "' + data + '"';

        if (data) {
            checkBox += ' checked="checked"';
        }

        if (!Europa.Controllers.StateMachinePreProposta.Permissoes.Incluir) {
            checkBox += ' disabled ';
        }
        checkBox += 'onchange = "Europa.Controllers.StateMachinePreProposta.OnJoinTipoDocumento(' + meta.row + ', 2)" /> ';
        checkBox += '</label>';

        return checkBox;
    }
};



DataTableApp.controller('DatatableDocumentoProponente', DatatableDocumentoProponente);

Europa.Controllers.StateMachinePreProposta.FiltroListarDocumentoProponenteRule = function () {
    var param = {
        IdRuleMachine: Europa.Controllers.StateMachinePreProposta.IdRuleMachine,
        IdTipoDocumento: $("#filtro_tipo_documento").val()
    };
    return param;
};

Europa.Controllers.StateMachinePreProposta.LimparFiltroDocumentoProponenteRule = function () {
    $("#filtro_tipo_documento").val("");
};

Europa.Controllers.StateMachinePreProposta.FiltrarDocumentoProponenteRule = function () {
    if (Europa.Controllers.StateMachinePreProposta.IdRuleMachine == undefined) {
        var res = {
            Sucesso: false,
            Mensagens: ["Selecione uma transição"]
        }
        Europa.Informacao.PosAcao(res);
        return;
    }
    Europa.Controllers.StateMachinePreProposta.DocumentoProponenteTable.reloadData(undefined,false);
}

Europa.Controllers.StateMachinePreProposta.SalvarDocumentoProponenteRule = function () {
    var obj = Europa.Controllers.StateMachinePreProposta.RuleMachineTable.getDataRowEdit();

    var url = obj.Id == 0 ? Europa.Controllers.StateMachinePreProposta.UrlIncludeRule : Europa.Controllers.StateMachinePreProposta.UrlUpdateRule

    Europa.Controllers.StateMachinePreProposta.LimparErro();

    $.post(url, { rule: obj }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.StateMachinePreProposta.RuleMachineTable.closeEdition();
            Europa.Controllers.StateMachinePreProposta.RuleMachineTable.reloadData();
            Europa.Controllers.StateMachinePreProposta.LimparErro();
        } else {
            Europa.Controllers.StateMachinePreProposta.AdicionarErro(resposta.Campos);
        }
    });
};

Europa.Controllers.StateMachinePreProposta.OnJoinTipoDocumento = function (row, tipo) {
    var obj = Europa.Controllers.StateMachinePreProposta.DocumentoProponenteTable.getRowData(row);

    var documento = {
        IdDocumentoRuleMachine: obj.IdDocumentoRuleMachine,
        IdRuleMachinePreProposta: obj.IdRuleMachinePreProposta,
        IdTipoDocumento: obj.IdTipoDocumento,
        NomeTipoDocumento: obj.NomeTipoDocumento,
        ObrigatorioPortal: tipo == 1 ? !obj.ObrigatorioPortal : obj.ObrigatorioPortal,
        ObrigatorioHouse: tipo == 2 ? !obj.ObrigatorioHouse : obj.ObrigatorioHouse
    };

    $.post(Europa.Controllers.StateMachinePreProposta.UrlOnJoinTipoDocumento, { documento: documento }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.StateMachinePreProposta.FiltrarDocumentoProponenteRule();
        }
        Europa.Informacao.PosAcao(res);
        Europa.Controllers.StateMachinePreProposta.DocumentoProponenteTable.reloadData();

    });

}