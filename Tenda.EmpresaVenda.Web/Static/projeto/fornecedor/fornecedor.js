Europa.Controllers.Fornecedor = {};
Europa.Controllers.Fornecedor.Tabela = undefined;
Europa.Controllers.Fornecedor.Permissoes = undefined;

$(function () {
    Europa.Controllers.Fornecedor.AplicarMascaras();
});
function TabelaFornecedor($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.Fornecedor.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.Fornecedor.Tabela;
    tabelaWrapper
        .setTemplateEdit([
            '<input type="text" class="form-control" name="NomeFantasia" id="NomeFantasia" maxlength="60">',
            '<input type="text" class="form-control" name="RazaoSocial" id="RazaoSocial" maxlength="60">',
            '<input type="text" class="form-control" name="CNPJ" id="CNPJ">'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('NomeFantasia').withTitle(Europa.i18n.Messages.NomeFantasia).withOption('width', '35%'),
            DTColumnBuilder.newColumn('RazaoSocial').withTitle(Europa.i18n.Messages.RazaoSocial).withOption('width', '35%'),
            DTColumnBuilder.newColumn('CNPJ').withTitle(Europa.i18n.Messages.Cnpj).renderWith(formatCNPJ).withOption('width', '20%')
        ])
        .setColActions(actionsHtml, '60px')
        .setActionSave(Europa.Controllers.Fornecedor.Salvar)
        .setDefaultOptions('POST', Europa.Controllers.Fornecedor.UrlListar, Europa.Controllers.Fornecedor.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButton(Europa.Controllers.Fornecedor.Permissoes.Atualizar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButton(Europa.Controllers.Fornecedor.Permissoes.Excluir, "Excluir", "fa fa-trash", "Excluir(" + meta.row + ")", full.Situacao) +
            '</div>';
    }
    function formatCNPJ(data, type, full) {
        return Europa.Mask.GetMaskedValue(data, Europa.Mask.FORMAT_CNPJ);
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
        var objetoLinhaTabela = Europa.Controllers.Fornecedor.Tabela.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.NomeFantasia, function () {
            Europa.Controllers.Fornecedor.Excluir(objetoLinhaTabela.Id);
        });
    };

    $scope.Editar = function (row) {
        Europa.Controllers.Fornecedor.Incluir = false;
        var objetoLinhaTabela = Europa.Controllers.Fornecedor.Tabela.getRowData(row);
        $scope.rowEdit(row);
        Europa.Controllers.Fornecedor.AplicarMascaras();
    };
};

DataTableApp.controller('Fornecedor', TabelaFornecedor);

Europa.Controllers.Fornecedor.AplicarMascaras = function () {
    Europa.Mask.Apply($("#CNPJ"), Europa.Mask.FORMAT_CNPJ, true);
}

Europa.Controllers.Fornecedor.FilterParams = function () {
    var param = {};
    return param;
};

Europa.Controllers.Fornecedor.Novo = function () {
    Europa.Controllers.Fornecedor.Tabela.createRowNewData();
    Europa.Controllers.Fornecedor.AplicarMascaras();
    Europa.Controllers.Fornecedor.Incluir = true;
};

Europa.Controllers.Fornecedor.Salvar = function () {
    var obj = Europa.Controllers.Fornecedor.Tabela.getDataRowEdit();
    var url = Europa.Controllers.Fornecedor.Incluir ? Europa.Controllers.Fornecedor.UrlIncluir : Europa.Controllers.Fornecedor.UrlAlterar
    $.post(url, { model: obj }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.Fornecedor.Tabela.closeEdition();
            Europa.Controllers.Fornecedor.Tabela.reloadData();
            Europa.Controllers.Fornecedor.LimparErro();
        } else {
            Europa.Controllers.Fornecedor.AdicionarErro(resposta.Campos);
        }
    });
};

Europa.Controllers.Fornecedor.Excluir = function (id) {
    $.post(Europa.Controllers.Fornecedor.UrlExcluir, { id: id }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.Fornecedor.Tabela.reloadData();
        }
    });
};

Europa.Controllers.Fornecedor.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.Fornecedor.LimparErro = function () {
    $("[name='Descricao']").parent().removeClass("has-error");
};