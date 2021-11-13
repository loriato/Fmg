Europa.Controllers.AgrupamentoProcesso = {};
Europa.Controllers.AgrupamentoProcesso.FiltroAgrupamentoProcesso = {}
Europa.Controllers.AgrupamentoProcesso.IncluirAgrupamentoProcesso = {}
Europa.Controllers.AgrupamentoProcesso.IncluirAgrupamentoProcesso = false;

$(function () { })

Europa.Controllers.AgrupamentoProcessoPreProposta.Novo = function () {
    Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.IdAgrupamento = 0;
    Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Filtrar();
    Europa.Controllers.AgrupamentoProcesso.AgrupamentoProcessoTable.createRowNewData();
    Europa.Controllers.AgrupamentoProcessoPreProposta.AutoCompleteSistemaRow = new Europa.Components.AutoCompleteSistemas()
        .WithTargetSuffix("sistemarow")
        .Configure();
    Europa.Controllers.AgrupamentoProcessoPreProposta.modoInclusao = true;
}
function TableAgrupamentoProcesso($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.AgrupamentoProcesso.AgrupamentoProcessoTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.AgrupamentoProcesso.AgrupamentoProcessoTable;
    tabela
        .setTemplateEdit([
            '<input type="text" class="form-control" name="Nome" id="Nome">'
            /*'<select id="autocomplete_sistemarow" name="autocomplete_sistemarow" class="select2-container form-control" ></select>'*/
        ])
        .setColumns([
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '45%')
            /*DTColumnBuilder.newColumn('NomeSistema').withTitle(Europa.i18n.Messages.Sistema).withOption('width', '45%')*/
        ])
        .setColActions(actionsHtml, '15%')
        .setActionSave(Europa.Controllers.AgrupamentoProcesso.SalvarAgrupamentoProcesso)
        .setIdAreaHeader("AgrupamentoProcesso_datatable_barra")
        .setOptionsSelect('POST', Europa.Controllers.AgrupamentoProcessoPreProposta.UrlListar, Europa.Controllers.AgrupamentoProcesso.FiltroAgrupamentoProcesso);


    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButton(Europa.Controllers.AgrupamentoProcessoPreProposta.Permissoes.Alterar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButton(Europa.Controllers.AgrupamentoProcessoPreProposta.Permissoes.Excluir, "Remover", "fa fa-trash", "Remover(" + meta.row + ")", full.Situacao) +
            '</div>';
    }

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (hasPermission !== 'True' && hasPermission !== 'true') {
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

    $scope.renderButtonEdit = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 3) {
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

    $scope.onRowSelect = function (data) {
        console.log(data);
        Europa.Controllers.AgrupamentoProcessoPreProposta.IdAgrupamentoProcessoPreProposta = data.Id;
        Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.IdAgrupamento = data.Id;
        Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.IdSistema = data.IdSistemas;
        Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Filtrar();
    }

    $scope.Editar = function (row) {
        Europa.Controllers.AgrupamentoProcessoPreProposta.Incluir = false;
        $scope.rowEdit(row);
        Europa.Controllers.AgrupamentoProcessoPreProposta.AutoCompleteSistemaRow = new Europa.Components.AutoCompleteSistemas()
            .WithTargetSuffix("sistemarow")
            .Configure();
        var objetoLinhaTabela = Europa.Controllers.AgrupamentoProcesso.AgrupamentoProcessoTable.getRowData(row);
        Europa.Controllers.AgrupamentoProcessoPreProposta.AutoCompleteSistemaRow.SetValue(objetoLinhaTabela.IdSistemas, objetoLinhaTabela.NomeSistema);
    };

    $scope.Remover = function (row) {
        Europa.Confirmacao.PreAcaoV2("Remover", "Deseja remover o Agrupamento? Os status contidos no mesmo serão removidos!", Europa.i18n.Messages.Confirmar,
            function () {
                var objetoLinhaTabela = Europa.Controllers.AgrupamentoProcesso.AgrupamentoProcessoTable.getRowData(row);
                $.post(Europa.Controllers.AgrupamentoProcessoPreProposta.UrlExcluir, { Id: objetoLinhaTabela.Id }, function (resposta) {
                    if (resposta.Sucesso) {
                        Europa.Controllers.AgrupamentoProcesso.AgrupamentoProcessoTable.reloadData();
                        Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela.reloadData();
                        Europa.Controllers.AgrupamentoProcesso.LimparErro();
                    } else {
                        Europa.Controllers.AgrupamentoProcessoPreProposta.AdicionarErro(resposta.Campos);
                        Europa.Informacao.PosAcao(response);
                    }
                });
            }
        );
    };
};

DataTableApp.controller('AgrupamentoProcessoDatatable', TableAgrupamentoProcesso);

Europa.Controllers.AgrupamentoProcesso.FiltroAgrupamentoProcesso = function () {
    var filtro = {
        Nome: $('#filtro_nome').val(),
        IdSistema: $('#autocomplete_sistemas').val(),
    }
    return filtro;
};

Europa.Controllers.AgrupamentoProcesso.FiltrarAgrupamentoProcesso = function () {
    Europa.Controllers.AgrupamentoProcesso.AgrupamentoProcessoTable.reloadData();
};

Europa.Controllers.AgrupamentoProcesso.LimparFiltroAgrupamentoProcesso = function () {
    $("#filtro_Nome").val("");
};

Europa.Controllers.AgrupamentoProcesso.NovoAgrupamentoProcesso = function () {
    Europa.Controllers.AgrupamentoProcesso.AgrupamentoProcessoTable.createRowNewData();
    Europa.Controllers.AgrupamentoProcesso.IncluirAgrupamentoProcesso = true;
};

Europa.Controllers.AgrupamentoProcesso.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.AgrupamentoProcesso.LimparErro = function () {
    $("[name='Descricao']").parent().removeClass("has-error");
};

Europa.Controllers.AgrupamentoProcesso.SalvarAgrupamentoProcesso = function () {
    var obj = Europa.Controllers.AgrupamentoProcesso.AgrupamentoProcessoTable.getDataRowEdit();
    var dados = {
        Id: obj.Id,
        Nome: obj.Nome,
        IdSistemas: obj.autocomplete_sistemarow,
    };
    var url = Europa.Controllers.AgrupamentoProcessoPreProposta.Incluir ? Europa.Controllers.AgrupamentoProcessoPreProposta.UrlIncluir : Europa.Controllers.AgrupamentoProcessoPreProposta.UrlAlterar;
    $.post(url, { model: dados }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.AgrupamentoProcesso.AgrupamentoProcessoTable.closeEdition();
            Europa.Controllers.AgrupamentoProcesso.AgrupamentoProcessoTable.reloadData();
            Europa.Controllers.AgrupamentoProcesso.LimparErro();
        } else {
            Europa.Controllers.AgrupamentoProcessoPreProposta.AdicionarErro(resposta.Campos);
        }
    });
};

Europa.Controllers.AgrupamentoProcesso.ExcluirAgrupamentoProcesso = function (id) {

    $.post(Europa.Controllers.AgrupamentoProcesso.UrlExcluirAgrupamentoProcesso, { id: id }, function (resposta) {
        if (resposta.Sucesso) {
            Europa.Controllers.AgrupamentoProcesso.FiltrarAgrupamentoProcesso();
            Europa.Controllers.AgrupamentoProcesso.AgrupamentoProcessoId = 0;
            Europa.Controllers.AgrupamentoProcesso.FiltrarEmpresaVenda();
        }
        Europa.Informacao.PosAcao(resposta);
    });

};