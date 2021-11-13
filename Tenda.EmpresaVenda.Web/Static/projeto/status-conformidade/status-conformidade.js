Europa.Controllers.StatusConformidade = {};
Europa.Controllers.StatusConformidade.Tabela = undefined;
Europa.Controllers.StatusConformidade.Permissoes = {};

function TabelaConformidadeEvs($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.StatusConformidade.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.StatusConformidade.Tabela;
    tabelaWrapper
        .setTemplateEdit([
            '<input type="text" class="form-control" name="DescricaoJunix" id="DescricaoJunix" maxlength="50">',
            '<input type="text" class="form-control" name="DescricaoEvs" id="DescricaoEvs" maxlength="50">'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('DescricaoJunix').withTitle(Europa.i18n.Messages.DescricaoJunix).withOption('width', '45%'),
            DTColumnBuilder.newColumn('DescricaoEvs').withTitle(Europa.i18n.Messages.DescricaoEvs).withOption('width', '45%')
        ])
        .setColActions(actionsHtml, '60px')
        .setActionSave(Europa.Controllers.StatusConformidade.Salvar)
        .setIdAreaHeader("datatable_header")
        .setDefaultOptions('POST', Europa.Controllers.StatusConformidade.UrlListar, Europa.Controllers.StatusConformidade.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButton(Europa.Controllers.StatusConformidade.Permissoes.Atualizar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButton(Europa.Controllers.StatusConformidade.Permissoes.Excluir, "Excluir", "fa fa-trash", "Excluir(" + meta.row + ")", full.Situacao) +
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
        var objetoLinhaTabela = Europa.Controllers.StatusConformidade.Tabela.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.DescricaoJunix + " - " + objetoLinhaTabela.DescricaoEvs, function () {
            Europa.Controllers.StatusConformidade.Excluir(objetoLinhaTabela.Id);
        });
    };

    $scope.Editar = function (row) {
        Europa.Controllers.StatusConformidade.Inclur = false;
        var objetoLinhaTabela = Europa.Controllers.StatusConformidade.Tabela.getRowData(row);
        $scope.rowEdit(row);
    };
};

DataTableApp.controller('StatusConformidade', TabelaConformidadeEvs);



Europa.Controllers.StatusConformidade.Filter = function () {
    Europa.Controllers.StatusConformidade.Tabela.reloadData();
};

Europa.Controllers.StatusConformidade.FilterParams = function () {
    var param = {
        statusConformidadeEvs: $('#descricaoConformidadeEvs').val()
    };
    return param;
};
Europa.Controllers.StatusConformidade.LimparFilter = function () {
    $('#descricaoConformidadeEvs').val('');
    Europa.Controllers.StatusConformidade.Tabela.reloadData();
};


Europa.Controllers.StatusConformidade.NovoStatusConformidadeEvs = function () {
    Europa.Controllers.StatusConformidade.Tabela.createRowNewData();
    Europa.Controllers.StatusConformidade.Inclur = true;
};

Europa.Controllers.StatusConformidade.Salvar = function () {
    var obj = Europa.Controllers.StatusConformidade.Tabela.getDataRowEdit();
    var url = Europa.Controllers.StatusConformidade.Inclur ? Europa.Controllers.StatusConformidade.UrlIncluir : Europa.Controllers.StatusConformidade.UrlAlterar
    $.post(url, { model: obj }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.StatusConformidade.Tabela.closeEdition();
            Europa.Controllers.StatusConformidade.Tabela.reloadData();
            Europa.Controllers.StatusConformidade.LimparErro();
        } else {
            Europa.Controllers.StatusConformidade.AdicionarErro(resposta.Campos);
        }
    });
};

Europa.Controllers.StatusConformidade.Excluir = function (id) {
    $.post(Europa.Controllers.StatusConformidade.UrlExcluir, { id: id }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.StatusConformidade.Tabela.reloadData();
        }
    });
};

Europa.Controllers.StatusConformidade.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.StatusConformidade.LimparErro = function () {
    $("[name='DescricaoJunix']").parent().removeClass("has-error");
    $("[name='DescricaoEvs']").parent().removeClass("has-error");
};