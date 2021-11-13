$(function () {

    Europa.Controllers.PontuacaoFidelidade.AutoCompleteEmpreendimento = new Europa.Components.AutoCompleteEmpreendimento()
        .WithTargetSuffix("empreendimento")
        .Configure();

    Europa.Controllers.PontuacaoFidelidade.AutoCompleteEmpresaVenda = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("empresa_venda")
        .Configure();

    Europa.Components.DatePicker.AutoApply();
})

function pontuacaoFidelidadeTabela($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.PontuacaoFidelidade.DatatablePontuacaoFidelidade = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.PontuacaoFidelidade.DatatablePontuacaoFidelidade;
    self.setColumns([
        DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption('width', '40px'),
        DTColumnBuilder.newColumn('NomeEmpresaVenda').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption('width', '100px'),
        DTColumnBuilder.newColumn('NomeEmpreendimento').withTitle(Europa.i18n.Messages.Empreendimento).withOption('width', '100px'),
        DTColumnBuilder.newColumn('Descricao').withTitle(Europa.i18n.Messages.Descricao).withOption('width', '100px'),
        DTColumnBuilder.newColumn('Codigo').withTitle(Europa.i18n.Messages.Codigo).withOption('width', '100px').renderWith(renderCodigo),
        DTColumnBuilder.newColumn('InicioVigencia').withTitle(Europa.i18n.Messages.InicioVigencia).withOption('width', '100px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('TerminoVigencia').withTitle(Europa.i18n.Messages.TerminoVigencia).withOption('width', '100px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('TipoPontuacaoFidelidade').withTitle(Europa.i18n.Messages.TipoPontuacaoFidelidade).withOption('type', 'enum-format-TipoPontuacaoFidelidade').withOption('width', '100px'),
        DTColumnBuilder.newColumn('TipoCampanhaFidelidade').withTitle(Europa.i18n.Messages.TipoCampanhaFidelidade).withOption('type', 'enum-format-TipoCampanhaFidelidade').withOption('width', '100px'),
        DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('type', 'enum-format-SituacaoPontuacaoFidelidade').withOption('width', '100px'),

    ])
        .setAutoInit(false)
        .setDefaultOrder([[4, 'asc']])
        .setColActions(actionsHtml, '5%')
        .setIdAreaHeader("datatable_header")
        .setDefaultOptions('POST', Europa.Controllers.PontuacaoFidelidade.Url.ListarDatatablePontuacaoFidelidade, Europa.Controllers.PontuacaoFidelidade.Filtro);

    function actionsHtml(data, type, full, meta) {
        var content = "<div>";

        content = content + Europa.Controllers.PontuacaoFidelidade.DatatablePontuacaoFidelidade.renderButton(Europa.Controllers.PontuacaoFidelidade.Permissoes.Detalhar, Europa.i18n.Messages.Detalhar, "fa fa-eye", "Detalhar(" + data.IdPontuacaoFidelidade + ")");

        if (full.Situacao == 0) {
            content = content + Europa.Controllers.PontuacaoFidelidade.DatatablePontuacaoFidelidade.renderButton(Europa.Controllers.PontuacaoFidelidade.Permissoes.Liberar, Europa.i18n.Messages.Liberar, "fa fa-unlock", "Liberar(" + data.IdPontuacaoFidelidade + ")");
            content = content + Europa.Controllers.PontuacaoFidelidade.DatatablePontuacaoFidelidade.renderButton(Europa.Controllers.PontuacaoFidelidade.Permissoes.Excluir, Europa.i18n.Messages.Excluir, "fa fa-trash", "Excluir(" + data.IdPontuacaoFidelidade + ")");
        }

        if (full.IdPontuacaoFidelidade) {
            content = content + Europa.Controllers.PontuacaoFidelidade.DatatablePontuacaoFidelidade.renderButton(Europa.Controllers.PontuacaoFidelidade.Permissoes.Download, Europa.i18n.Messages.Download, "fa fa-download", "Download(" + data.IdPontuacaoFidelidade + "," + full.IdEmpresaVenda + ")");
        }

        content = content + "</div>";
        return content;
    };

    $scope.Detalhar = function (data) {
        window.open(Europa.Controllers.PontuacaoFidelidade.Url.MatrizPontuacaoFidelidade + "?idPontuacaoFidelidade=" + data,'_blank')
        //location.href = Europa.Controllers.PontuacaoFidelidade.Url.MatrizPontuacaoFidelidade + "?idPontuacaoFidelidade=" + data;
    }

    $scope.Liberar = function (idPontuacaoFidelidade) {        
        Europa.Controllers.PontuacaoFidelidade.Liberar(idPontuacaoFidelidade);
    }

    $scope.Excluir = function (idPontuacaoFidelidade) {
        Europa.Controllers.PontuacaoFidelidade.Excluir(idPontuacaoFidelidade);
    }

    $scope.Download = function (idPontuacaoFidelidade, idEmpresaVenda) {
        location.href = Europa.Controllers.PontuacaoFidelidade.Url.DownloadPdfPontuacaoFidelidadeEvs + "?idPontuacaoFidelidade="
            + idPontuacaoFidelidade + "&idEmpresaVenda=" + idEmpresaVenda;
    }

    function renderCodigo(data, type, full, meta) {
        var link = '<div>';
        if (data) {
            link = link + "<a title='Pontuação Fidelidade' target='_blank' href='" + Europa.Controllers.PontuacaoFidelidade.Url.MatrizPontuacaoFidelidade + '?idPontuacaoFidelidade=' + full.IdPontuacaoFidelidade + "'>" + full.Codigo + "</a>";
        }
        link += '</div>';
        return link;
    }

}

DataTableApp.controller('TabelaPontuacao', pontuacaoFidelidadeTabela);

Europa.Controllers.PontuacaoFidelidade.Filtrar = function () {
    Europa.Controllers.PontuacaoFidelidade.DatatablePontuacaoFidelidade.reloadData();
};

Europa.Controllers.PontuacaoFidelidade.Filtro = function () {
    var param = {
        Regional: $("#FiltroRegional").val(),
        Descricao: $("#Descricao").val(),
        IdEmpreendimentos: $("#autocomplete_empreendimento").val(),
        VigenteEm: $("#VigenteEm").val(),
        TipoPontuacaoFidelidade: $("#FiltroTipoPontuacaoFidelidade").val(),
        TipoCampanhaFidelidade: $("#FiltroTipoCampanhaFidelidade").val(),
        Situacao: $("#Situacao").val(),
        IdsEmpresaVenda: [],
        Codigo:$("#Codigo").val()
    };

    if ($("#autocomplete_empresa_venda").val() != null) {
        param.IdsEmpresaVenda.push($("#autocomplete_empresa_venda").val());
    }

    return param;
};

Europa.Controllers.PontuacaoFidelidade.LimparFiltro = function () {
    $("#FiltroRegional").val("").trigger("change");
    $("#Descricao").val("");
    $("#autocomplete_empreendimento").val("").trigger("change");
    $("#autocomplete_empresa_venda").val("").trigger("change");
    $("#VigenteEm").val("");
    $("#FiltroTipoPontuacaoFidelidade").val("").trigger("change");
    $("#FiltroTipoCampanhaFidelidade").val("").trigger("change");
    $("#Situacao").val("").trigger("change");
    $("#Codigo").val("");
    
    Europa.Controllers.PontuacaoFidelidade.Filtrar();
};

Europa.Controllers.PontuacaoFidelidade.Liberar = function (idPontuacaoFidelidade) {
    $.post(Europa.Controllers.PontuacaoFidelidade.Url.Liberar, { idPontuacaoFidelidade: idPontuacaoFidelidade }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.PontuacaoFidelidade.DatatablePontuacaoFidelidade.reloadData();
        }
            
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.PontuacaoFidelidade.Excluir = function (idPontuacaoFidelidade) {
    $.post(Europa.Controllers.PontuacaoFidelidade.Url.Excluir, { idPontuacaoFidelidade: idPontuacaoFidelidade }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.PontuacaoFidelidade.DatatablePontuacaoFidelidade.reloadData();
        }

        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.PontuacaoFidelidade.ExportarTodos = function () {

    var params = Europa.Controllers.PontuacaoFidelidade.DatatablePontuacaoFidelidade.lastRequestParams;
    var formDownload = $("#Exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.PontuacaoFidelidade.Url.ExportarTodos);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
}

Europa.Controllers.PontuacaoFidelidade.ExportarPagina = function () {

    var params = Europa.Controllers.PontuacaoFidelidade.DatatablePontuacaoFidelidade.lastRequestParams;
    var formDownload = $("#Exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.PontuacaoFidelidade.Url.ExportarPagina);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
}