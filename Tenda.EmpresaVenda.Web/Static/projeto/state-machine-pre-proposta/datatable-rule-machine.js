
$(function () {

})

function DatatableRuleMachine($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.StateMachinePreProposta.RuleMachineTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.StateMachinePreProposta.RuleMachineTable;
    tabela
        .setTemplateEdit([
            Europa.Controllers.StateMachinePreProposta.DropDownOrigem,
            Europa.Controllers.StateMachinePreProposta.DropDownDestino
        ])
        .setColumns([
            DTColumnBuilder.newColumn('Origem').withTitle(Europa.i18n.Messages.Origem).withOption('type', 'enum-format-SituacaoProposta'),
            DTColumnBuilder.newColumn('Destino').withTitle(Europa.i18n.Messages.Destino).withOption('type', 'enum-format-SituacaoProposta'),
        ])
        .setColActions(actionsHtml, '100px')
        .setActionSave(Europa.Controllers.StateMachinePreProposta.SalvarRuleMachinePreProposta)
        .setIdAreaHeader("rule_machine_datatable_barra")
        .setOptionsSelect('POST', Europa.Controllers.StateMachinePreProposta.UrlListarRuleMachinePreProposta, Europa.Controllers.StateMachinePreProposta.FiltroRuleMachine);

    function actionsHtml(data, type, full, meta) {
        var button = '<div>';
        button += $scope.renderButton(Europa.Controllers.StateMachinePreProposta.Permissoes.Atualizar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")");
        button += $scope.renderButton(Europa.Controllers.StateMachinePreProposta.Permissoes.Excluir, "Excluir", "fa fa-trash", "Excluir(" + meta.row + ")");
        button += '</div >';
        return button;
    }

    $scope.renderButton = function (hasPermission, title, icon, onClick) {

        if (hasPermission == false) {
            return "";
        }

        icon = $('<i/>').addClass(icon);

        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);

        return button.prop('outerHTML');
    }

    $scope.Editar = function (row) {
        $scope.rowEdit(row);
    };

    $scope.onRowSelect = function (data) {
        if (Europa.Controllers.StateMachinePreProposta.IdRuleMachine == data.Id) {
            Europa.Controllers.StateMachinePreProposta.IdRuleMachine = undefined;
        } else {
            Europa.Controllers.StateMachinePreProposta.IdRuleMachine = data.Id;
        }
        
        Europa.Controllers.StateMachinePreProposta.DocumentoProponenteTable.reloadData();
    }
    
    $scope.Excluir = function (row) {
        var objeto = Europa.Controllers.StateMachinePreProposta.RuleMachineTable.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir,
            Europa.String.Format("Transição {0} para {1}", Europa.i18n.Enum.Resolve("SituacaoProposta", objeto.Origem), Europa.i18n.Enum.Resolve("SituacaoProposta", objeto.Destino)),
            function () {
                Europa.Controllers.StateMachinePreProposta.ExcluirRule(objeto.Id);
        });
    };
};

DataTableApp.controller('DatatableRuleMachine', DatatableRuleMachine);

Europa.Controllers.StateMachinePreProposta.FiltroRuleMachine = function () {
    var param = {
        Origem: $("#filtro_origem").val(),
        Destino: $("#filtro_destino").val()
    };
    return param;
};

Europa.Controllers.StateMachinePreProposta.LimparFiltroRuleMachine = function () {
    $("#filtro_origem").val("");
    $("#filtro_destino").val("");
};

Europa.Controllers.StateMachinePreProposta.FiltrarRuleMachine = function () {
    Europa.Controllers.StateMachinePreProposta.IdRuleMachine = undefined;

    Europa.Controllers.StateMachinePreProposta.RuleMachineTable.reloadData();
    Europa.Controllers.StateMachinePreProposta.DocumentoProponenteTable.reloadData();
}

Europa.Controllers.StateMachinePreProposta.NewRule = function () {
    Europa.Controllers.StateMachinePreProposta.RuleMachineTable.createRowNewData();
}

Europa.Controllers.StateMachinePreProposta.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.StateMachinePreProposta.LimparErro = function () {
    $("[name='Origem']").parent().removeClass("has-error");
    $("[name='Destino']").parent().removeClass("has-error");
};

Europa.Controllers.StateMachinePreProposta.SalvarRuleMachinePreProposta = function () {
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

Europa.Controllers.StateMachinePreProposta.ExcluirRule = function (idRuleMachine) {
    $.post(Europa.Controllers.StateMachinePreProposta.UrlExcluirRule, { idRuleMachine: idRuleMachine }, function (resposta) {

        if (resposta.Sucesso) {
            Europa.Controllers.StateMachinePreProposta.IdRuleMachine = undefined;

            Europa.Controllers.StateMachinePreProposta.FiltrarRuleMachine();
            Europa.Controllers.StateMachinePreProposta.DocumentoProponenteTable.reloadData();

        }
        Europa.Informacao.PosAcao(resposta);

    });

};
