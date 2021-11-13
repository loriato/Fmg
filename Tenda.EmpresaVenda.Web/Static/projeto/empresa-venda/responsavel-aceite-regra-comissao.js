Europa.Controllers.EmpresaVenda.CountDoc = 1;
Europa.Controllers.EmpresaVenda.Documentos = [];
Europa.Controllers.EmpresaVenda.ListaDatatable = [];
Europa.Controllers.EmpresaVenda.TabelaResponsavelAceiteRegraComissao = {};
Europa.Controllers.EmpresaVenda.IdResponsavelAceiteRegraComissao = {};

$(function () {

});

Europa.Controllers.EmpresaVenda.InitAutoCompletes = function () {
    Europa.Controllers.EmpresaVenda.AutoCompleteCorretorEmpresaVenda = new Europa.Components.AutoCompleteCorretorEmpresaVenda()
        .WithTargetSuffix("corretor").Configure();

    Europa.Controllers.EmpresaVenda.ConfigureAutoCompleteCorretorEmpresaVenda(Europa.Controllers.EmpresaVenda);
}

Europa.Controllers.EmpresaVenda.ConfigureAutoCompleteCorretorEmpresaVenda = function (autocompleteWrapper) {
    autocompleteWrapper.AutoCompleteCorretorEmpresaVenda.Data = function (params) {
        return {
            start: 0,
            pageSize: 10,
            filter: [
                {
                    value: params.term,
                    column: this.param,
                    regex: true
                },
                {
                    value: function () {
                        return Europa.Controllers.EmpresaVenda.IdEmpresaVenda;
                    },
                    column: 'idEmpresaVenda'
                }
            ],
            order: [
                {
                    value: "asc",
                    column: this.param
                }
            ]
        };
    };

    autocompleteWrapper.AutoCompleteCorretorEmpresaVenda.Configure();
}

DataTableApp.controller('responsavelAceiteRegraComissao', aceiteRegraComissaoTable);

function aceiteRegraComissaoTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.EmpresaVenda.TabelaResponsavelAceiteRegraComissao = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.EmpresaVenda.TabelaResponsavelAceiteRegraComissao;
    tabelaWrapper.setTemplateEdit([
        '<div name="NomeCorretor"><select id="autocomplete_corretor" class= "form-control"></select></div>'
    ]).setColumns([
        DTColumnBuilder.newColumn('NomeCorretor').withTitle(Europa.i18n.Messages.NomeCorretor).withOption('width', '60%'),
        DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Status).withOption('width', '20%').withOption('type', 'enum-format-Situacao'),
    ])
        .setColActions(actionsHtml, '10%')
        .setIdAreaHeader("responsavel_aceite_regra_comissao_header")
        .setAutoInit(false)
        .setActionSave(Europa.Controllers.EmpresaVenda.SalvarResponsavelAceiteRegraComissao)
        .setDefaultOptions('POST', Europa.Controllers.EmpresaVenda.UrlListarResponsavelAceiteRegraComissao, Europa.Controllers.EmpresaVenda.FilterResponsavelParams);

    function actionsHtml(data, type, full, meta) {
        var div = '<div>';
        div += $scope.renderButtonEdit(Europa.i18n.Messages.Suspender, "fa fa-minus-circle", "Suspender(" + meta.row + ")");
        div += '</div>';
        return div;
    };

    $scope.renderButtonEdit = function (title, icon, onClick) {

        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .attr('style', 'margin-right:14px')
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.Suspender = function (row) {
        var obj = Europa.Controllers.EmpresaVenda.TabelaResponsavelAceiteRegraComissao.getRowData(row);
        console.log(obj);
        var model = {
            IdResponsavelRegraComissao: obj.Id,
        };

        $.post(Europa.Controllers.EmpresaVenda.UrlSuspenderResponsavelAceiteRegraComissao, model, function (resposta) {
            Europa.Informacao.PosAcao(resposta);
            if (resposta.Sucesso) {
                Europa.Controllers.EmpresaVenda.TabelaResponsavelAceiteRegraComissao.reloadData();
            }
        });
    };
}


Europa.Controllers.EmpresaVenda.FilterResponsavelParams = function () {

    return {
        IdEmpresaVenda: Europa.Controllers.EmpresaVenda.IdEmpresaVenda
    };

};

Europa.Controllers.EmpresaVenda.NovoResponsavelAceiteRegraComissao = function () {
    Europa.Controllers.EmpresaVenda.TabelaResponsavelAceiteRegraComissao.createRowNewData();
    Europa.Controllers.EmpresaVenda.InitAutoCompletes();
};

Europa.Controllers.EmpresaVenda.SalvarResponsavelAceiteRegraComissao = function () {
    if (Europa.Controllers.EmpresaVenda.IdEmpresaVenda == undefined) {
        return "";
    }

    var model = {
        IdCorretor: $("#autocomplete_corretor").val(),
        IdEmpresaVenda: Europa.Controllers.EmpresaVenda.IdEmpresaVenda
    };

    $.post(Europa.Controllers.EmpresaVenda.UrlIncluirResponsavelAceiteRegraComissao, model, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.EmpresaVenda.TabelaResponsavelAceiteRegraComissao.closeEdition();
            Europa.Controllers.EmpresaVenda.TabelaResponsavelAceiteRegraComissao.reloadData();
        }
    });
};