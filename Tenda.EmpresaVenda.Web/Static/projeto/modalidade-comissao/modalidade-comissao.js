Europa.Controllers.ModalidadeComissao = {};

$(function () {
    $("#filtro_estados").select2({
        trags: true
    });
});

DataTableApp.controller('Empreendimento', TabelaEmpreendimento);

function TabelaEmpreendimento($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.ModalidadeComissao.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.ModalidadeComissao.Tabela;
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Divisao').withTitle(Europa.i18n.Messages.Divisao).withOption('width', '15%'),
        DTColumnBuilder.newColumn('ModalidadeComissao').withTitle(Europa.i18n.Messages.ModalidadeComissao).withOption('width', '15%').withOption('type','enum-format-TipoModalidadeComissao'),
        DTColumnBuilder.newColumn('Estado').withTitle(Europa.i18n.Messages.Estado).withOption('width', '15%'),
        DTColumnBuilder.newColumn('DisponibilizarCatalogo').withTitle(Europa.i18n.Messages.DisponibilizarNoCatalogo).withOption('width', '15%').renderWith(formatBoolean),
        DTColumnBuilder.newColumn('DisponivelVenda').withTitle(Europa.i18n.Messages.DisponivelParaVenda).withOption('width', '15%').renderWith(formatBoolean)
    ])
        .setIdAreaHeader("datatable_header")
        .setAutoInit()
        .setOptionsMultiSelect('POST', Europa.Controllers.ModalidadeComissao.UrlListar, Europa.Controllers.ModalidadeComissao.FilterParams);

    function formatBoolean(data) {
        if (data === true) {
            return Europa.i18n.Messages.Sim;
        }
        return Europa.i18n.Messages.Nao;
    }

}

Europa.Controllers.ModalidadeComissao.FilterParams = function () {
    var filtro = {
        nome: $('#filtro_nome').val(),
        estados: $('#filtro_estados').val(),
        disponibilizaCatalogo: $('#filtro_disponibiliza_catalogo').val(),
        disponivelVenda: $('#filtro_disponivel_venda').val(),
        modalidadeComissao: $('#filtro_modalidade_comissao').val()
    };
    return filtro;
};
Europa.Controllers.ModalidadeComissao.Limpar = function () {
    $('#filtro_nome').val("");
    $('#filtro_estados').val("").trigger('change');
    $('#filtro_disponibiliza_catalogo').val(0);
    $('#filtro_disponivel_venda').val(0);
    $('#filtro_modalidade_comissao').val("").trigger('change');
}

Europa.Controllers.ModalidadeComissao.Filtrar = function () {
    Europa.Controllers.ModalidadeComissao.Tabela.reloadData();
}

Europa.Controllers.ModalidadeComissao.Validar = function () {
    var objetosSelecionados = Europa.Controllers.ModalidadeComissao.Tabela.getRowsSelect();
    var autorizar = true;
    if (objetosSelecionados == null || objetosSelecionados == undefined || objetosSelecionados.length == 0) {
        Europa.Informacao.ChangeHeaderAndContent(
            Europa.i18n.Messages.Erro,
            Europa.i18n.Messages.NenhumRegistroSelecionando);

        autorizar = false;

        Europa.Informacao.Show();
    }

    return autorizar;
};

Europa.Controllers.ModalidadeComissao.Fixa = function () {
    Europa.Controllers.ModalidadeComissao.PreAlterarTipo(Europa.Controllers.ModalidadeComissao.UrlFixa);
};

Europa.Controllers.ModalidadeComissao.Nominal = function () {
    Europa.Controllers.ModalidadeComissao.PreAlterarTipo(Europa.Controllers.ModalidadeComissao.UrlNominal);
};


Europa.Controllers.ModalidadeComissao.PreAlterarTipo = function (url) {
    var autorizar = Europa.Controllers.ModalidadeComissao.Validar();

    if (!autorizar) {
        return;
    }

    var objetosSelecionados = Europa.Controllers.ModalidadeComissao.Tabela.getRowsSelect();
    var Ids = [];
    objetosSelecionados.forEach(function (item) {
        Ids.push(item.Id);
    });
    Europa.Confirmacao.PreAcaoMulti(Europa.i18n.Messages.Alterar, function () {
        $.post(url, { Ids: Ids }, function (res) {
            if (res.Sucesso) {
                Europa.Controllers.ModalidadeComissao.Filtrar();
            }
            Europa.Informacao.PosAcao(res);
        });
    });
}