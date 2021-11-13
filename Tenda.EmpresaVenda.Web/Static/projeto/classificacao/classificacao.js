Europa.Controllers.Classificacao = {};
Europa.Controllers.Classificacao.Tabela = undefined;
Europa.Controllers.Classificacao.Permissoes = undefined;

function TabelaClassificacao($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.Classificacao.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.Classificacao.Tabela;
    tabelaWrapper
        .setTemplateEdit([
            '<input type="text" class="form-control" name="Descricao" id="Descricao" maxlength="50">'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('Descricao').withTitle(Europa.i18n.Messages.Descricao).withOption('width', '90%')
        ])
        .setColActions(actionsHtml, '60px')
        .setActionSave(Europa.Controllers.Classificacao.Salvar)
        .setDefaultOptions('POST', Europa.Controllers.Classificacao.UrlListar, Europa.Controllers.Classificacao.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButton(Europa.Controllers.Classificacao.Permissoes.Atualizar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButton(Europa.Controllers.Classificacao.Permissoes.Excluir, "Excluir", "fa fa-trash", "Excluir(" + meta.row + ")", full.Situacao) +
            '</div>';
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

    $scope.Excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.Classificacao.Tabela.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.Descricao, function () {
            Europa.Controllers.Classificacao.Excluir(objetoLinhaTabela.Id);
        });
    };

    $scope.Editar = function (row) {
        Europa.Controllers.Classificacao.Incluir = false;
        var objetoLinhaTabela = Europa.Controllers.Classificacao.Tabela.getRowData(row);
        $scope.rowEdit(row);
    };
};

DataTableApp.controller('Classificacao', TabelaClassificacao);

Europa.Controllers.Classificacao.FilterParams = function () {
    var param = {};
    return param;
};

Europa.Controllers.Classificacao.Novo = function () {
    Europa.Controllers.Classificacao.Tabela.createRowNewData();
    Europa.Controllers.Classificacao.Incluir = true;
};

Europa.Controllers.Classificacao.Salvar = function () {
    var obj = Europa.Controllers.Classificacao.Tabela.getDataRowEdit();
    var url = Europa.Controllers.Classificacao.Incluir ? Europa.Controllers.Classificacao.UrlIncluir : Europa.Controllers.Classificacao.UrlAlterar
    $.post(url, { model: obj }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.Classificacao.Tabela.closeEdition();
            Europa.Controllers.Classificacao.Tabela.reloadData();
            Europa.Controllers.Classificacao.LimparErro();
        } else {
            Europa.Controllers.Classificacao.AdicionarErro(resposta.Campos);
        }
    });
};

Europa.Controllers.Classificacao.Excluir = function (id) {
    $.post(Europa.Controllers.Classificacao.UrlExcluir, { id: id }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.Classificacao.Tabela.reloadData();
        }
    });
};

Europa.Controllers.Classificacao.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.Classificacao.LimparErro = function () {
    $("[name='Descricao']").parent().removeClass("has-error");
};