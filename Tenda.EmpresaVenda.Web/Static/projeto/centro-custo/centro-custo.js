Europa.Controllers.CentroCusto = {};
Europa.Controllers.CentroCusto.Tabela = undefined;
Europa.Controllers.CentroCusto.Permissoes = undefined;

function TabelaCentroCusto($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.CentroCusto.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.CentroCusto.Tabela;
    tabelaWrapper
        .setTemplateEdit([
            '<input type="text" class="form-control" name="Codigo" id="Codigo" maxlength="6">',
            '<input type="text" class="form-control" name="Descricao" id="Descricao" maxlength="60">'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('Codigo').withTitle(Europa.i18n.Messages.Codigo).withOption('width', '20%'),
            DTColumnBuilder.newColumn('Descricao').withTitle(Europa.i18n.Messages.Descricao).withOption('width', '70%')
        ])
        .setColActions(actionsHtml, '60px')
        .setActionSave(Europa.Controllers.CentroCusto.Salvar)
        .setDefaultOptions('POST', Europa.Controllers.CentroCusto.UrlListar, Europa.Controllers.CentroCusto.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButton(Europa.Controllers.CentroCusto.Permissoes.Atualizar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButton(Europa.Controllers.CentroCusto.Permissoes.Excluir, "Excluir", "fa fa-trash", "Excluir(" + meta.row + ")", full.Situacao) +
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
        var objetoLinhaTabela = Europa.Controllers.CentroCusto.Tabela.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.Codigo, function () {
            Europa.Controllers.CentroCusto.Excluir(objetoLinhaTabela.Id);
        });
    };

    $scope.Editar = function (row) {
        Europa.Controllers.CentroCusto.Incluir = false;
        var objetoLinhaTabela = Europa.Controllers.CentroCusto.Tabela.getRowData(row);
        $scope.rowEdit(row);
        Europa.Controllers.CentroCusto.AplicarMascaras();
    };
};

DataTableApp.controller('CentroCusto', TabelaCentroCusto);

Europa.Controllers.CentroCusto.FilterParams = function () {
    var param = {};
    return param;
};

Europa.Controllers.CentroCusto.Novo = function () {
    Europa.Controllers.CentroCusto.Tabela.createRowNewData();
    Europa.Controllers.CentroCusto.AplicarMascaras();
    Europa.Controllers.CentroCusto.Incluir = true;
};
Europa.Controllers.CentroCusto.AplicarMascaras = function () {
    Europa.Mask.Apply($("#Codigo"), Europa.Mask.FORMAT_INTEIRO, true);
}
Europa.Controllers.CentroCusto.Salvar = function () {
    var obj = Europa.Controllers.CentroCusto.Tabela.getDataRowEdit();
    var url = Europa.Controllers.CentroCusto.Incluir ? Europa.Controllers.CentroCusto.UrlIncluir : Europa.Controllers.CentroCusto.UrlAlterar
    $.post(url, { model: obj }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.CentroCusto.Tabela.closeEdition();
            Europa.Controllers.CentroCusto.Tabela.reloadData();
            Europa.Controllers.CentroCusto.LimparErro();
        } else {
            Europa.Controllers.CentroCusto.AdicionarErro(resposta.Campos);
        }
    });
};

Europa.Controllers.CentroCusto.Excluir = function (id) {
    $.post(Europa.Controllers.CentroCusto.UrlExcluir, { id: id }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.CentroCusto.Tabela.reloadData();
        }
    });
};

Europa.Controllers.CentroCusto.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.CentroCusto.LimparErro = function () {
    $("[name='Descricao']").parent().removeClass("has-error");
};