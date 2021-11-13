Europa.Controllers.TipoCusto = {};
Europa.Controllers.TipoCusto.Tabela = undefined;
Europa.Controllers.TipoCusto.Permissoes = undefined;

function TabelaTipoCusto($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.TipoCusto.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.TipoCusto.Tabela;
    tabelaWrapper
        .setTemplateEdit([
            '<input type="text" class="form-control" name="Descricao" id="Descricao" maxlength="60">'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('Descricao').withTitle(Europa.i18n.Messages.Descricao).withOption('width', '90%')
        ])
        .setColActions(actionsHtml, '60px')
        .setActionSave(Europa.Controllers.TipoCusto.Salvar)
        .setDefaultOptions('POST', Europa.Controllers.TipoCusto.UrlListar, Europa.Controllers.TipoCusto.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButton(Europa.Controllers.TipoCusto.Permissoes.Atualizar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButton(Europa.Controllers.TipoCusto.Permissoes.Excluir, "Excluir", "fa fa-trash", "Excluir(" + meta.row + ")", full.Situacao) +
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
        var objetoLinhaTabela = Europa.Controllers.TipoCusto.Tabela.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.Descricao, function () {
            Europa.Controllers.TipoCusto.Excluir(objetoLinhaTabela.Id);
        });
    };

    $scope.Editar = function (row) {
        Europa.Controllers.TipoCusto.Incluir = false;
        var objetoLinhaTabela = Europa.Controllers.TipoCusto.Tabela.getRowData(row);
        $scope.rowEdit(row);
    };
};

DataTableApp.controller('TipoCusto', TabelaTipoCusto);

Europa.Controllers.TipoCusto.FilterParams = function () {
    var param = { };
    return param;
};

Europa.Controllers.TipoCusto.Novo = function () {
    Europa.Controllers.TipoCusto.Tabela.createRowNewData();
    Europa.Controllers.TipoCusto.Incluir = true;
};

Europa.Controllers.TipoCusto.Salvar = function () {
    var obj = Europa.Controllers.TipoCusto.Tabela.getDataRowEdit();
    var url = Europa.Controllers.TipoCusto.Incluir ? Europa.Controllers.TipoCusto.UrlIncluir : Europa.Controllers.TipoCusto.UrlAlterar
    $.post(url, { model: obj }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.TipoCusto.Tabela.closeEdition();
            Europa.Controllers.TipoCusto.Tabela.reloadData();
            Europa.Controllers.TipoCusto.LimparErro();
        } else {
            Europa.Controllers.TipoCusto.AdicionarErro(resposta.Campos);
        }
    });
};

Europa.Controllers.TipoCusto.Excluir = function (id) {
    $.post(Europa.Controllers.TipoCusto.UrlExcluir, { id: id }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.TipoCusto.Tabela.reloadData();
        }
    });
};

Europa.Controllers.TipoCusto.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.TipoCusto.LimparErro = function () {
    $("[name='Descricao']").parent().removeClass("has-error");
};