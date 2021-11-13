Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta = {};
Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.IdAgrupamento = {};
Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.IdSistema = {};
Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela = {};
Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Permissoes = {};
$(function () {

});

Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Novo = function () {
    Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela.createRowNewData();    
    Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.AutoCompleteStatusPreProposta = new Europa.Components.AutoCompleteStatusPreProposta()
        .WithTargetSuffix("statuspreproposta")
        .Configure();
    Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.AutoCompleteStatusPreProposta.Data = function (params) {
        var data = {
            start: 0,
            pageSize: 10,
            filter: [
                {
                    value: params.term,
                    column: this.param,
                    regex: true
                },
                {
                    value: Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.IdSistema,
                    column: 'IdSistema'
                }
            ],
            order: [
                {
                    value: "asc",
                    column: this.param
                }
            ]
        };
        if (this.paramCallback) {
            var object = this.paramCallback();
            for (var i in object) {
                data.filter.push({
                    value: object[i],
                    column: i
                })
            }
        }
        return data;
    };
    Europa.Controllers.AgrupamentoProcessoPreProposta.modoInclusao = true;
}

DataTableApp.controller('AgrupamentoSituacaoProcessoDatatable', listaStatus);

function listaStatus($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela;
    tabelaWrapper
        .setTemplateEdit([
            '<select id="autocomplete_statuspreproposta" name="autocomplete_statuspreproposta" class="select2-container form-control" ></select>'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('StatusPreProposta').withTitle(Europa.i18n.Messages.Status).withOption('width', '45%')
            /*DTColumnBuilder.newColumn('NomeAgrupamento').withTitle(Europa.i18n.Messages.Agrupamento).withOption('width', '45%')*/
        ])
        .setColActions(actionsHtml, '10%')
        .setActionSave(Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Salvar)
        .setDefaultOptions('POST', Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.UrlListar, Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.filterParams);


    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            /*$scope.renderButton(Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Permissoes.Alterar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")", full.Situacao) +*/
            $scope.renderButton(Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Permissoes.Excluir, "Excluir", "fa fa-trash", "Remover(" + meta.row + ")", full.Situacao) +
            '</div>';
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

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (hasPermission === 'true') {
            icon = $('<i/>').addClass(icon);
            var button = $('<a />')
                .addClass('btn btn-default')
                .attr('title', title)
                .attr('ng-click', onClick)
                .append(icon);
            return button.prop('outerHTML');
        } else {
            return null;
        }
    };

    $scope.Remover = function (row) {
        var objetoLinhaTabela = Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela.getRowData(row);
        console.log(Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.UrlExcluir);
        $.post(Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.UrlExcluir, { Id: objetoLinhaTabela.Id }, function (resposta) {
            Europa.Informacao.PosAcao(resposta);
            if (resposta.Sucesso) {
                Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela.reloadData();
                Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.LimparErro();
            } else {
                Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.AdicionarErro(resposta.Campos);
            }
        });
    };
};
Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Salvar = function () {
    var obj = Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela.getDataRowEdit();
    console.log(obj);
    var objIncluir = {
        IdAgrupamento : Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.IdAgrupamento,
        IdStatusPreProposta : obj.autocomplete_statuspreproposta,
        Id : obj.Id,
    };
    var url = Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Incluir ? Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.UrlIncluir : Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.UrlAlterar;
    $.post(url, { model: objIncluir }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela.closeEdition();
            Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela.reloadData();
            Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.LimparErro();
        } else {
            Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.AdicionarErro(resposta.Campos);
        }
    });
};

Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.filterParams = function () {
    var filtro = {
        StatusPreproposta: $('#filtro_Nome').val(),
        IdSituacaoPreProposta: $('#autocomplete_statuspreproposta').val(),
        IdAgrupamentoSituacao: Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.IdAgrupamento
    }
    console.log("filtros   " + Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.IdAgrupamento);
    return filtro;
};

Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Filtrar = function () {
    Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.Tabela.reloadData();
};

Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.LimparFiltro = function () {
    $('#filtro_Nome').val("");
};

Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.ExportarTodos = function () {
    console.log("exportarPaTod");
    var params = Europa.Controllers.EmpresaVenda.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.EmpresaVenda.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.ExportarPagina = function () {
    console.log("exportarPa");
    var params = Europa.Controllers.EmpresaVenda.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.EmpresaVenda.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};


Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.AgrupamentoSituacaoProcessoPreProposta.LimparErro = function () {
    $("[name='Descricao']").parent().removeClass("has-error");
};

