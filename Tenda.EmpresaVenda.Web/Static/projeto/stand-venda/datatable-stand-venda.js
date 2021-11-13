Europa.Controllers.StandVenda.IncluirStandVenda = false;

$(function () {
})

function TableStandVenda($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.StandVenda.StandVendaTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.StandVenda.StandVendaTable;
    tabela
        .setTemplateEdit([
            '<select id="autocomplete_regionaledit" name="RegionalEdit" class="select2-container form-control" value="" style="width:100% !important;"></select>',
            Europa.Controllers.StandVenda.DropDownEstado,
            '<input type="text" class="form-control" name="Nome" id="Nome" maxlength="50">'         
        ])
        .setColumns([
            DTColumnBuilder.newColumn('NovoRegional').withTitle(Europa.i18n.Messages.Regional).withOption("width", "40px"),
            DTColumnBuilder.newColumn('Estado').withTitle(Europa.i18n.Messages.UF).withOption("width", "40px"),
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption("width", "150px"),
        ])
        .setColActions(actionsHtml, '50px')
        .setActionSave(Europa.Controllers.StandVenda.PreSalvarStandVenda)
        .setIdAreaHeader("standVenda_datatable_barra")
        .setDefaultOrder([[3, 'asc']])
        .setOptionsSelect('POST', Europa.Controllers.StandVenda.UrlListarDatatableStandVenda, Europa.Controllers.StandVenda.FiltroStandVenda);

    function actionsHtml(data, type, full, meta) {
        var button = '<div>';
        //button += $scope.renderButton(Europa.Controllers.StandVenda.Permissoes.Atualizar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")");
        button += $scope.renderButton(Europa.Controllers.StandVenda.Permissoes.Excluir, "Excluir", "fa fa-trash", "Excluir(" + meta.row + ")");
        button += '</div >';
        return button;
    }

    $scope.onRowSelect = function (data) {
        Europa.Controllers.StandVenda.OnClickStandVenda(data);
    }

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (!hasPermission) {
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
        Europa.Controllers.StandVenda.IncluirStandVenda = false;
        $scope.rowEdit(row);

        var obj = Europa.Controllers.StandVenda.StandVendaTable.getRowData(row)

        Europa.Controllers.StandVenda.IdPontoVenda = obj.IdPontoVenda;
    };

    $scope.Excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.StandVenda.StandVendaTable.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.Nome, function () {
            Europa.Controllers.StandVenda.ExcluirStandVenda(objetoLinhaTabela.Id);
        });
    };
};

DataTableApp.controller('StandVendaDatatable', TableStandVenda);

Europa.Controllers.StandVenda.FiltroStandVenda = function () {
    var param = {
        Nome: $("#filtro_nome").val(),
        Estado: $("#filtro_estados").val(),
        IdRegional: $("#autocomplete_regional").val()
    };
    return param;
};

Europa.Controllers.StandVenda.FiltrarStandVenda = function () {
    Europa.Controllers.StandVenda.StandVendaTable.reloadData();
    Europa.Controllers.StandVenda.StandEmpresaVendaTable.reloadData();
};

Europa.Controllers.StandVenda.LimparFiltroStandVenda = function () {
    $("#filtro_nome").val("");
    $("#filtro_estados").val(0).trigger("change");
    $("#autocomplete_regional").val(0).trigger("change");
};

Europa.Controllers.StandVenda.NovoStandVenda = function () {

    Europa.Controllers.StandVenda.StandVendaTable.createRowNewData();
    Europa.Controllers.StandVenda.IncluirStandVenda = true;
    Europa.Controllers.StandVenda.IdStandVenda = undefined;
    Europa.Controllers.StandVenda.AutoCompleteRegionalFiltro = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regionaledit")
        .Configure();
    Europa.Controllers.StandVenda.StandEmpresaVendaTable.reloadData();
};

Europa.Controllers.StandVenda.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.StandVenda.LimparErro = function () {
    $("[name='Nome']").parent().removeClass("has-error");
    $("[name='Regional']").parent().removeClass("has-error");
};

Europa.Controllers.StandVenda.PreSalvarStandVenda = function () {
    Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.Atencao,
        "Após inclusão não será permitida a edição da Regional, UF e Nome selecionados. Deseja realmente continuar?",
        "",
        function () {
            Europa.Controllers.StandVenda.SalvarStandVenda();
        });
}

Europa.Controllers.StandVenda.SalvarStandVenda = function () {
    var obj = Europa.Controllers.StandVenda.StandVendaTable.getDataRowEdit();
    var stand = {
        Id: obj.Id,
        Nome: obj.Nome,
        Regional: obj.Estado,
        Estado: obj.Estado,
        IdRegional: $('#autocomplete_regionaledit').val()
    }

    var url = Europa.Controllers.StandVenda.UrlIncluirStandVenda;

    Europa.Controllers.StandVenda.LimparErro();

    $.post(url, stand, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.StandVenda.StandVendaTable.closeEdition();
            Europa.Controllers.StandVenda.StandVendaTable.reloadData();
        }

        Europa.Controllers.StandVenda.AdicionarErro(res.Campos);
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.StandVenda.ExcluirStandVenda = function (idStandVenda) {
    $.post(Europa.Controllers.StandVenda.UrlExcluirStandVenda, { idStandVenda: idStandVenda }, function (resposta) {
        
        if (resposta.Sucesso) {
            Europa.Controllers.StandVenda.IdStandVenda = undefined;
            Europa.Controllers.StandVenda.Regional = undefined;         

            Europa.Controllers.StandVenda.FiltrarStandVenda();
            
        }
        Europa.Informacao.PosAcao(resposta);

    });

};

Europa.Controllers.StandVenda.OnClickStandVenda = function (stand) {
    Europa.Controllers.StandVenda.StandVendaTable.closeEdition();

    if (Europa.Controllers.StandVenda.IdStandVenda == stand.Id) {
        Europa.Controllers.StandVenda.IdStandVenda = undefined;
        Europa.Controllers.StandVenda.Regional = undefined;

        Europa.Controllers.StandVenda.StandEmpresaVendaTable.reloadData();
        return;
    }

    Europa.Controllers.StandVenda.IdStandVenda = stand.Id;
    Europa.Controllers.StandVenda.Estado = stand.Estado;
    Europa.Controllers.StandVenda.IdRegional = stand.IdRegional;
    Europa.Controllers.StandVenda.FiltrarStandEmpresaVenda();
}