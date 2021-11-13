Europa.Controllers.StatusSuatEvs = {};
Europa.Controllers.StatusSuatEvs.Tabela = undefined;
Europa.Controllers.StatusSuatEvs.Permissoes = {};

function TabelaSuatEvs($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.StatusSuatEvs.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.StatusSuatEvs.Tabela;
    tabelaWrapper
        .setTemplateEdit([
            '<input type="text" class="form-control" name="DescricaoSuat" id="DescricaoSuat" maxlength="50">',
            '<input type="text" class="form-control" name="DescricaoEvs" id="DescricaoEvs" maxlength="50">'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('DescricaoSuat').withTitle(Europa.i18n.Messages.DescricaoSuat).withOption('width', '45%'),
            DTColumnBuilder.newColumn('DescricaoEvs').withTitle(Europa.i18n.Messages.DescricaoEvs).withOption('width', '45%')
        ])
        .setColActions(actionsHtml, '60px')
        .setActionSave(Europa.Controllers.StatusSuatEvs.Salvar)
        .setIdAreaHeader("datatable_header")
        .setDefaultOptions('POST', Europa.Controllers.StatusSuatEvs.UrlListar, Europa.Controllers.StatusSuatEvs.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButton(Europa.Controllers.StatusSuatEvs.Permissoes.Atualizar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButton(Europa.Controllers.StatusSuatEvs.Permissoes.Excluir, "Excluir", "fa fa-trash", "Excluir(" + meta.row + ")", full.Situacao) +
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
        var objetoLinhaTabela = Europa.Controllers.StatusSuatEvs.Tabela.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.DescricaoSuat + " - " + objetoLinhaTabela.DescricaoEvs, function () {
            Europa.Controllers.StatusSuatEvs.Excluir(objetoLinhaTabela.Id);
        });
    };

    $scope.Editar = function (row) {
        Europa.Controllers.StatusSuatEvs.Inclur = false;
        var objetoLinhaTabela = Europa.Controllers.StatusSuatEvs.Tabela.getRowData(row);
        $scope.rowEdit(row);
    };
};

DataTableApp.controller('StatusSuatEvs', TabelaSuatEvs);



Europa.Controllers.StatusSuatEvs.Filter = function () {
    Europa.Controllers.StatusSuatEvs.Tabela.reloadData();
};

Europa.Controllers.StatusSuatEvs.FilterParams = function () {
    var param = {
        statusSuatEvs: $('#descricaoSuatEvs').val()
    };
    return param;
};
Europa.Controllers.StatusSuatEvs.LimparFilter = function () {
    $('#descricaoSuatEvs').val('');
    Europa.Controllers.StatusSuatEvs.Tabela.reloadData();
};


Europa.Controllers.StatusSuatEvs.NovoStatusSuatEvs = function () {
    Europa.Controllers.StatusSuatEvs.Tabela.createRowNewData();
    Europa.Controllers.StatusSuatEvs.Inclur = true;
};

Europa.Controllers.StatusSuatEvs.Salvar = function () {
    var obj = Europa.Controllers.StatusSuatEvs.Tabela.getDataRowEdit();
    var url = Europa.Controllers.StatusSuatEvs.Inclur ? Europa.Controllers.StatusSuatEvs.UrlIncluir : Europa.Controllers.StatusSuatEvs.UrlAlterar
    $.post(url, { model: obj }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.StatusSuatEvs.Tabela.closeEdition();
            Europa.Controllers.StatusSuatEvs.Tabela.reloadData();
            Europa.Controllers.StatusSuatEvs.LimparErro();
        } else {
            Europa.Controllers.StatusSuatEvs.AdicionarErro(resposta.Campos);
        }
    });
};

Europa.Controllers.StatusSuatEvs.Excluir = function (id) {
    $.post(Europa.Controllers.StatusSuatEvs.UrlExcluir, { id: id }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.StatusSuatEvs.Tabela.reloadData();
        }
    });
};

Europa.Controllers.StatusSuatEvs.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.StatusSuatEvs.LimparErro = function () {
    $("[name='DescricaoSuat']").parent().removeClass("has-error");
    $("[name='DescricaoEvs']").parent().removeClass("has-error");
};