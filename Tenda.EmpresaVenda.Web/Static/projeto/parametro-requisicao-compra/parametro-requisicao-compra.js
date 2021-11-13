Europa.Controllers.ParametroRC = {};
Europa.Controllers.ParametroRC.Incluir = false;


function TabelaParametroRC($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.ParametroRC.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.ParametroRC.Tabela;
    tabelaWrapper
        .setTemplateEdit([
            '<input type="text" class="form-control" name="Codigo" id="Codigo" disabled="disabled">',
            '<input type="text" class="form-control" name="Valor" id="Valor" maxlength="60">',
            '<input type="text" class="form-control" name="Descricao" id="Descricao" disabled="disabled">',
            '<input type="text" class="form-control" name="Tipo" id="Tipo" disabled="disabled">'
        ])
        .setIdAreaHeader("datatable_header")
        .setColumns([
            DTColumnBuilder.newColumn("Codigo").withTitle(Europa.i18n.Messages.Codigo).withOption("width", "25%"),
            DTColumnBuilder.newColumn("Valor").withTitle(Europa.i18n.Messages.Valor).withOption("width", "25%"),
            DTColumnBuilder.newColumn("Descricao").withTitle(Europa.i18n.Messages.Descricao).withOption("width", "30%"),
            DTColumnBuilder.newColumn("Tipo").withTitle(Europa.i18n.Messages.TipoParametro).withOption("width", "12%")

        ])
        .setColActions(actionsHtml, "8%")
        .setActionSave(Europa.Controllers.ParametroRC.Salvar)
        .setDefaultOptions("POST",
            Europa.Controllers.ParametroRC.UrlListar,
            Europa.Controllers.ParametroRC.Filtro);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButton(Europa.Controllers.ParametroRC.Permissoes.Atualizar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")") +
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
            return '';
        }
    };

    $scope.Editar = function (rowNr) {
        Europa.Controllers.ParametroRC.Incluir = false;
        $scope.rowEdit(rowNr);
        
    }
};

DataTableApp.controller('ParametroRC', TabelaParametroRC);

Europa.Controllers.ParametroRC.Filtro = function () {
    var param = {
        Chave: $("#filtro-chave").val()
    };
    return param;
};


Europa.Controllers.ParametroRC.Salvar = function () {
    var obj = Europa.Controllers.ParametroRC.Tabela.getDataRowEdit();
    var url = Europa.Controllers.ParametroRC.Incluir ? Europa.Controllers.ParametroRC.UrlIncluir : Europa.Controllers.ParametroRC.UrlAtualizar
    $.post(url, { model: obj }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.ParametroRC.Tabela.closeEdition();
            Europa.Controllers.ParametroRC.Tabela.reloadData();
            Europa.Controllers.ParametroRC.LimparErro();
        } else {
            Europa.Controllers.ParametroRC.AdicionarErro(resposta.Campos);
        }
    });
};

Europa.Controllers.ParametroRC.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.ParametroRC.LimparErro = function () {
    $("[name='Valor']").parent().removeClass("has-error");
};

Europa.Controllers.ParametroRC.Filtrar = function () {
    Europa.Controllers.ParametroRC.Tabela.reloadData();
};

Europa.Controllers.ParametroRC.LimparFiltro = function () {
    $("#filtro-chave").val("");
};