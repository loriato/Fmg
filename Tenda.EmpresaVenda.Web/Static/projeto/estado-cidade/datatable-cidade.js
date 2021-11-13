
$(function () {

});

function DatatableCidade($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.EstadoCidade.DatatableCidade = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.EstadoCidade.DatatableCidade;
    tabela
        .setTemplateEdit([
            '<input type="text" class="form-control" name="Cidade" id="Cidade" maxlength="50">'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('Cidade').withTitle(Europa.i18n.Messages.Cidades).withOption("width", "100px"),
        ])
        .setColActions(actionsHtml, '10px')
        .setActionSave(Europa.Controllers.EstadoCidade.SalvarCidade)
        .setIdAreaHeader("datatable_cidade_barra")
        .setOptionsSelect('POST', Europa.Controllers.EstadoCidade.UrlListarCidades, Europa.Controllers.EstadoCidade.FiltroCidade);

    function actionsHtml(data, type, full, meta) {
        var button = '<div>';
        button += tabela.renderButton(Europa.Controllers.EstadoCidade.Permissoes.Atualizar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")");
        button += tabela.renderButton(Europa.Controllers.EstadoCidade.Permissoes.Excluir, "Excluir", "fa fa-trash", "Excluir(" + meta.row + ")");
        button += '</div >';
        return button;
    }

    $scope.Editar = function (row) {
        $scope.rowEdit(row);
    }

    $scope.Excluir = function (row) {
        var obj = Europa.Controllers.EstadoCidade.DatatableCidade.getRowData(row);

        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, obj.Cidade, function () {
            Europa.Controllers.EstadoCidade.ExcluirCidade(obj);
        });
        
    }

};

DataTableApp.controller('DatatableCidade', DatatableCidade);

Europa.Controllers.EstadoCidade.FiltroCidade = function () {
    var param = {
        Estado: Europa.Controllers.EstadoCidade.OnEstado,
        Cidade: $("#filtroCidade").val()
    };

    return param;
};

Europa.Controllers.EstadoCidade.FiltrarCidades = function () {
    Europa.Controllers.EstadoCidade.DatatableCidade.closeEdition();
    Europa.Controllers.EstadoCidade.DatatableCidade.reloadData(undefined, false);
};

Europa.Controllers.EstadoCidade.LimparFiltroCidade = function () {
    $("#filtroCidade").val("");
};

Europa.Controllers.EstadoCidade.IncluirCidade = function () {
    Europa.Controllers.EstadoCidade.DatatableCidade.createRowNewData();
};

Europa.Controllers.EstadoCidade.SalvarCidade = function () {
    var obj = Europa.Controllers.EstadoCidade.DatatableCidade.getDataRowEdit();
    obj.Estado = Europa.Controllers.EstadoCidade.OnEstado;

    var url = obj.Id == 0 ? Europa.Controllers.EstadoCidade.UrlIncluirCidade : Europa.Controllers.EstadoCidade.UrlAtualizarCidade
    
    $.post(url, { estadoCidade: obj }, function (res) {
        Europa.Informacao.PosAcao(res);
        if (res.Sucesso) {
            Europa.Controllers.EstadoCidade.DatatableCidade.closeEdition();
            Europa.Controllers.EstadoCidade.DatatableCidade.reloadData();
            Europa.Controllers.EstadoCidade.LimparErro();
        } else {
            Europa.Controllers.EstadoCidade.AdicionarErro(res.Campos);
        }
    });
};

Europa.Controllers.EstadoCidade.ExcluirCidade = function (obj) {
    $.post(Europa.Controllers.EstadoCidade.UrlExcluirCidade, { estadoCidade: obj }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.EstadoCidade.FiltrarCidades();
        }
        Europa.Informacao.PosAcao(res);
    });
}