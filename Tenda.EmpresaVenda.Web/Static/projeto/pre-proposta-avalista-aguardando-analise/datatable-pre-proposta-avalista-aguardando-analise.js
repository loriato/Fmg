Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Tabela = {};
Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.ExibindoInfo = false;
Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.SituacaoDocumentoAvalista_Enviado = 6;
Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.SituacaoDocumentoAvalista_Aprovado = 3;


$(function () {
    Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.AlternarExibicaoInformacoes();

    Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.AutoCompleteViabilizador = new Europa.Components.AutoCompleteViabilizador()
        .WithTargetSuffix("viabilizador")
        .Configure();

    Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.AutoCompleteEmpresaVenda = new Europa.Components.AutoCompleteEmpresaVendasCACT()
        .WithTargetSuffix("empresa_venda")
        .Configure();

    Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.AutoCompleteBreveLancamento = new Europa.Components.AutoCompleteBreveLancamento()
        .WithTargetSuffix("breve_lancamento")
        .Configure();


    Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.AutoCompleteRegionais = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regional")
        .Configure();

    Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Filtrar();

    $("#situacao_avalista").select2({
        trags: true,
        width: '100%'
    });

    $("#UF").select2({
        trags: true,
        width: '100%'
    });

});


function CriarTabelaPrePropostaAvalistaAguardandoAnalise($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Tabela;
    self.setColumns([
        DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption('width', '100px'),
        DTColumnBuilder.newColumn('UF').withTitle(Europa.i18n.Messages.UF).withOption('width', '100px'),
        DTColumnBuilder.newColumn('CodigoPreProposta').withTitle(Europa.i18n.Messages.Codigo).withOption('width', '200px')
            .renderWith(formatLink),
        DTColumnBuilder.newColumn('SituacaoPrePropostaSuatEvs').withTitle(Europa.i18n.Messages.SituacaoPreProposta).withOption('width', '150px'),
        DTColumnBuilder.newColumn('SituacaoAvalista').withTitle(Europa.i18n.Messages.SituacaoAvalista).withOption('width', '150px').withOption('type', 'enum-format-SituacaoAvalista'),
        DTColumnBuilder.newColumn('DataHoraUltimoEnvio').withTitle(Europa.i18n.Messages.DataHoraUltimoEnvio).withOption('width', '150px').renderWith(Europa.Date.toGeenDateTimeFormat), 
        DTColumnBuilder.newColumn('ProponenteUm').withTitle(Europa.i18n.Messages.ProponenteUm).withOption('width', '200px')
            .withOption("link", self.withOptionLink(Europa.Components.DetailAction.Cliente, "IdProponenteUm")),
        DTColumnBuilder.newColumn('CpfProponenteUm').withTitle(Europa.i18n.Messages.CpfProponenteUm).withOption('width', '150px').renderWith(Europa.String.FormatCpf),
        DTColumnBuilder.newColumn('ProponenteDois').withTitle(Europa.i18n.Messages.ProponenteDois).withOption('width', '250px')
            .withOption("link", self.withOptionLink(Europa.Components.DetailAction.Cliente, "IdProponenteDois")),
        DTColumnBuilder.newColumn('CpfProponenteDois').withTitle(Europa.i18n.Messages.CpfProponenteDois).withOption('width', '150px').renderWith(Europa.String.FormatCpf),
        DTColumnBuilder.newColumn('BreveLancamento').withTitle(Europa.i18n.Messages.Produto).withOption('width', '150px'),
        DTColumnBuilder.newColumn('EmpresaVenda').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeElaborador').withTitle(Europa.i18n.Messages.Elaborador).withOption('width', '150px'),
        DTColumnBuilder.newColumn('Corretor').withTitle(Europa.i18n.Messages.Corretor).withOption('width', '150px'),
        DTColumnBuilder.newColumn('AgenteViabilizador').withTitle(Europa.i18n.Messages.Viabilizador).withOption('width', '250px'),
        DTColumnBuilder.newColumn('DataElaboracao').withTitle(Europa.i18n.Messages.DataDaElaboracao).withOption('width', '150px').withOption("type", "date-format-DD/MM/YYYY"),
        DTColumnBuilder.newColumn('HoraElaboracao').withTitle(Europa.i18n.Messages.HoraDaElaboracao).withOption('width', '150px').renderWith(Europa.Date.toGeenTimeFormat),
        
        DTColumnBuilder.newColumn('DataInicioPrimeiraAnalise').withTitle(Europa.i18n.Messages.DataDaPrimeiraAnalise).withOption('width', '150px').withOption("type", "date-format-DD/MM/YYYY"),
        DTColumnBuilder.newColumn('HoraInicioPrimeiraAnalise').withTitle(Europa.i18n.Messages.HoraDaPrimeiraAnalise).withOption('width', '150px').renderWith(Europa.Date.toGeenTimeFormat),
        DTColumnBuilder.newColumn('HoraFimPrimeiraAnalise').withTitle(Europa.i18n.Messages.FimDaPrimeiraAnalise).withOption('width', '150px').renderWith(Europa.Date.toGeenTimeFormat),
        DTColumnBuilder.newColumn('NomeUsuarioPrimeiraAnalise').withTitle(Europa.i18n.Messages.AnalistaPrimeiraAnalise).withOption('width', '250px'),
        DTColumnBuilder.newColumn('ContadorPrimeiraAnalise').withTitle(Europa.i18n.Messages.ContadorPrimeiraAnalise).withOption('width', '150px').renderWith(formatarContador),

        DTColumnBuilder.newColumn('DataInicioUltimaAnalise').withTitle(Europa.i18n.Messages.DataDaUltimaAnalise).withOption('width', '150px').withOption("type", "date-format-DD/MM/YYYY"),
        DTColumnBuilder.newColumn('HoraInicioUltimaAnalise').withTitle(Europa.i18n.Messages.HoraDaUltimaAnalise).withOption('width', '150px').renderWith(Europa.Date.toGeenTimeFormat),
        DTColumnBuilder.newColumn('HoraFimUltimaAnalise').withTitle(Europa.i18n.Messages.FimDaUltimaAnalise).withOption('width', '150px').renderWith(Europa.Date.toGeenTimeFormat),
        DTColumnBuilder.newColumn('NomeUsuarioUltimaAnalise').withTitle(Europa.i18n.Messages.AnalistaUltimaAnalise).withOption('width', '250px'),
        DTColumnBuilder.newColumn('Contador').withTitle(Europa.i18n.Messages.ContadorUltimaAnalise).withOption('width', '150px').renderWith(formatarContador),
        DTColumnBuilder.newColumn('ContadorTotal').withTitle(Europa.i18n.Messages.ContadorTotal).withOption('width', '150px').renderWith(formatarContador),
        DTColumnBuilder.newColumn('QuantidadeAnalise').withTitle(Europa.i18n.Messages.QuantidadeAnalise).withOption('width', '150px'),
        DTColumnBuilder.newColumn('PropostasAnteriores').withTitle(Europa.i18n.Messages.HouvePropostasAnteriores).withOption('width', '200px').renderWith(Europa.String.FormatBoolean),
        DTColumnBuilder.newColumn('NumeroPropostasAnteriores').withTitle(Europa.i18n.Messages.NumeroPropostasAnteriores).withOption('width', '200px'),
        DTColumnBuilder.newColumn('MotivoPendencia').withTitle(Europa.i18n.Messages.MotivoPendencia).withOption('width', '250px'),
        // Não exibido na tela, mas exportado no Excel. Pedido ref. http://jira.construtoratenda.com/browse/EVS-109
        //DTColumnBuilder.newColumn('MotivoParecer').withTitle(Europa.i18n.Messages.Parecer).withOption('width', '250px'),
        DTColumnBuilder.newColumn('StatusSicaq').withTitle(Europa.i18n.Messages.StatusSicaq).withOption('width', '150px').withOption('type', 'enum-format-StatusSicaq'),
        DTColumnBuilder.newColumn('DataSicaq').withTitle(Europa.i18n.Messages.DataSicaq).withOption('width', '150px').withOption("type", "date-format-DD/MM/YYYY"),
        DTColumnBuilder.newColumn('HoraSicaq').withTitle(Europa.i18n.Messages.HoraSicaq).withOption('width', '150px').renderWith(Europa.Date.toGeenTimeFormat),
        DTColumnBuilder.newColumn('NomeAnalistaSicaq').withTitle(Europa.i18n.Messages.AnalistaSicaq).withOption('width', '250px'),
        DTColumnBuilder.newColumn('ParcelaAprovada').withTitle(Europa.i18n.Messages.ParcelaAprovadaDoSICAQ).withOption('width', '250px').renderWith(renderValorParcela),
        DTColumnBuilder.newColumn('Observacao').withTitle(Europa.i18n.Messages.Observacao).withOption('width', '250px')
    ])
        .setAutoInit(false)
    .setIdAreaHeader("datatable_header")
    .setColActions(actionsHtml, '110px')
    .setDefaultOrder([[2, 'desc']])
        .setDefaultOptions('POST', Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.UrlListar, Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Filtro);

    function actionsHtml(data, type, full, meta) {
        var content = "<div>";
        if (full.IdAvalista) {
            content = content + "<a class='btn btn-default' title='Avalista' target='_blank' href='" + Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.UrlDocumentacaoAvalista + '?id=' + full.Id + "'><i class='fa fa-eye'></i></a>";
        }
        content = content + "</div>";
        return content;
    };

    // O MomentJS ainda não dá suporte a formatação de duração no formato HHH:mm
    function formatarContador(input) {
        var horas = Math.floor(input / 60);
        var minutos = Math.floor(input - (horas * 60));
        minutos = minutos.toString().length == 1 ? "0" + minutos : minutos;

        switch (horas.toString().length) {
            case 1:
                return "00" + horas + ":" + minutos;
            case 2:
                return "0" + horas + ":" + minutos;
            case 3:
                return horas + ":" + minutos;
        }
    }

    function formatarParcelaAprpvada(input) {
        console.log(input);
        var teste = parseFloat(input.toFixed(10));
        return input.concat('0.00');
    }
    
    $scope.alterarSicaq = function (row) {
        var objeto = Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Tabela.getRowData(row);
        Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Modal.AlterarSicaq.AbrirModal(objeto.Id);      
    };

    $scope.revisar = function (row) {
        var objeto = Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Tabela.getRowData(row);
        Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format("Deseja confirma o retorno da pré-proposta para {0}", Europa.i18n.Messages.SituacaoProposta_EmAnaliseSimplificada));
        Europa.Confirmacao.ConfirmCallback = function () {
            $.post(Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.UrlRevisarPreProposta, { idProposta: objeto.Id }, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Tabela.reloadData();
                }
                Europa.Informacao.PosAcao(res);
            });
        }
        Europa.Confirmacao.Show();
    };

    $scope.retornar = function (row) {
        var objeto = Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Tabela.getRowData(row);
        Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.RetornarSituacaoAnterior, Europa.i18n.Messages.SituacaoPreProposta));
        Europa.Confirmacao.ConfirmCallback = function () {
            $.post(Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.UrlRetornarSituacao, { idPreProposta: objeto.Id }, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Tabela.reloadData();
                }
                Europa.Informacao.PosAcao(res);
            });
        }
        Europa.Confirmacao.Show();
    };

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (hasPermission !== 'true') {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.renderButtonRetornar = function (hasPermission, title, icon, onClick, row) {
        if (hasPermission !== 'true') {
            return "";
        }
        var objeto = Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Tabela.getRowData(row);
        if (objeto.StatusPreProposta != "7" && objeto.StatusPreProposta != "18") {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    function formatLink(data, meta, full, type) {
        return '<a href="' + Europa.Components.DetailAction.PreProposta + '?id=' + full.IdPreProposta + '" target=_blank>' + full.CodigoPreProposta + '</a>';
    };

    function renderValorParcela(data, type, full, meta) {
        if (full.DataSicaq) {
            var value = full.ParcelaAprovada;
            if (value === undefined || value === '' || value === null) {
                return "";
            }
            return "R$ "+value.toFixed(2).replace(".", ",").replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
        }
    }

    function situacaoAvalistaRender(data) {
        if (data == 1) {
            return  "Não Anexado";
        }
        else if (data == 2) {
            return "Anexado";
        }
        else if (data == 3) {
            return "Avalista Aprovado";
        }
        else if (data == 4) {
            return "Avalista Reprovado";
        }
        else if (data == 5) {
            return "Informado";
        }
        if (data == 6) {
            return "Aguardando Análise";
        }
        return "Pré-Carregado"
    }

}

DataTableApp.controller('PrePropostasAguardandoAnalise', CriarTabelaPrePropostaAvalistaAguardandoAnalise);

Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Filtro = function () {
    return {
        Regional: $("#autocomplete_regional").val(),
        UF: $("#UF").val(),
        CodigoPreProposta: $("#CodigoPreProposta").val(),
        SituacoesPreProposta: $("#SituacoesPreProposta").val(),
        IdBreveLancamento: $("#autocomplete_breve_lancamento").val(),
        IdEmpresaVenda: $("#autocomplete_empresa_venda").val(),
        DataElaboracaoDe: $("#DataElaboracaoDe").val(),
        DataElaboracaoAte: $("#DataElaboracaoAte").val(),
        Cpf: $("#CpfProponente").val(),
        Cliente: $("#Cliente").val(),
        DataUltimoEnvioDe: $("#DataUltimoEnvioDe").val(),
        DataUltimoEnvioAte: $("#DataUltimoEnvioAte").val(),
        IdViabilizador: $("#autocomplete_viabilizador").val(),
        SituacoesAvalista: $("#situacao_avalista").val(),
        Regionais: $("#regionais").val()
    };
};

Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.AlternarExibicaoInformacoes = function () {
    if (!Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.ExibindoInfo) {
        $('.mais-filtros').each(function () {
            $(this).hide();
        });
        $('#info-hide-filtro').html(Europa.i18n.Messages.MaisFiltros);
    } else {
        $('.mais-filtros').each(function () {
            $(this).fadeIn('slow');
        });
        setTimeout(function () {
            $('#info-hide-filtro').html(Europa.i18n.Messages.MenosFiltros);
        }, 300);

        Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.InitDatePicker();
    }
    Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.ExibindoInfo = !Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.ExibindoInfo;
};


Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.ExportarPagina = function () {
    var params = Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Filtro();
    params.order = Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Tabela.lastRequestParams.draw;
    params.pageSize = Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Tabela.lastRequestParams.pageSize;
    params.start = Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Tabela.lastRequestParams.start;
    var formExportar = $("#form_exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.ExportarTodos = function () {
    var params = Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Filtro();
    params.order = Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Tabela.lastRequestParams.draw;
    var formExportar = $("#form_exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Filtrar = function () {
    Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Tabela.reloadData();
};

Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.LimparFiltro = function () {
    $("#filtro").find("input[type=text], textarea").val("");
    $("#SituacoesPreProposta").val("").trigger('change');
    $("#autocomplete_regional").val("").trigger("change");
    $("#autocomplete_empresa_venda").val("").trigger("change");
    $("#autocomplete_breve_lancamento").val("").trigger("change");
    $("#autocomplete_viabilizador").val("").trigger("change");
    $("#check_avalista").prop("checked", false);
    $("#check_avalista").val("false");
    $("#situacao_avalista").val("").trigger('change');
    $("#UF").val("").trigger('change');

};

Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.OnCheckAvalista = function () {
    if ($("#check_avalista").val() == "true") {
        $("#check_avalista").val("false");
    } else {
        $("#check_avalista").val("true");
    }
}