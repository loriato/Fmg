Europa.Controllers.PrePropostaAguardandoAnalise.Tabela = {};
Europa.Controllers.PrePropostaAguardandoAnalise.ExibindoInfo = false;
Europa.Controllers.PrePropostaAguardandoAnalise.SituacaoDocumentoAvalista_Enviado = 6;
Europa.Controllers.PrePropostaAguardandoAnalise.SituacaoDocumentoAvalista_Aprovado = 3;
Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarCCA = {};

$(function () {
    Europa.Controllers.PrePropostaAguardandoAnalise.AlternarExibicaoInformacoes();

    Europa.Controllers.PrePropostaAguardandoAnalise.AutoCompleteViabilizador = new Europa.Components.AutoCompleteViabilizador()
        .WithTargetSuffix("viabilizador")
        .Configure();

    Europa.Controllers.PrePropostaAguardandoAnalise.AutoCompleteEmpresaVenda = new Europa.Components.AutoCompleteEmpresaVendasCACT()
        .WithTargetSuffix("empresa_venda")
        .Configure();

    Europa.Controllers.PrePropostaAguardandoAnalise.AutoCompleteBreveLancamento = new Europa.Components.AutoCompleteBreveLancamento()
        .WithTargetSuffix("breve_lancamento")
        .Configure();

    $("#SituacoesPreProposta").val(["17", "6"]).trigger('change');
    //$("#SituacoesPreProposta").val(["20","2"]).trigger('change');

    Europa.Controllers.PrePropostaAguardandoAnalise.Filtrar();

});


function CriarTabelaPrePropostaAguardandoAnalise($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.PrePropostaAguardandoAnalise.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.PrePropostaAguardandoAnalise.Tabela;
    self.setColumns([
        DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption('width', '100px'),
        DTColumnBuilder.newColumn('UF').withTitle(Europa.i18n.Messages.UF).withOption('width', '100px'),
        DTColumnBuilder.newColumn('CodigoPreProposta').withTitle(Europa.i18n.Messages.Codigo).withOption('width', '200px')
            .renderWith(formatLink),
        DTColumnBuilder.newColumn('SituacaoPrePropostaSuatEvs').withTitle(Europa.i18n.Messages.SituacaoPreProposta).withOption('width', '150px'),
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
        .setDefaultOptions('POST', Europa.Controllers.PrePropostaAguardandoAnalise.UrlListar, Europa.Controllers.PrePropostaAguardandoAnalise.Filtro);

    function actionsHtml(data, type, full, meta) {
        var content = "<div>";
        if (full.StatusPreProposta == 20) {
            content = content + $scope.renderButton('true', Europa.i18n.Messages.AlterarSicaq, 'fa fa-exchange', 'alterarSicaq(' + meta.row + ')');
            content = content + $scope.renderButton('true', Europa.i18n.Messages.Revisar, 'fa fa-undo', 'revisar(' + meta.row + ')');
        }
        if (full.StatusPreProposta == 2) {
            content = content + $scope.renderButton('true', Europa.i18n.Messages.AlterarSicaq, 'fa fa-exchange', 'alterarSicaqPrevio(' + meta.row + ')');
            content = content + $scope.renderButton('true', Europa.i18n.Messages.Revisar, 'fa fa-undo', 'revisar(' + meta.row + ')');
        }
        content = content + $scope.renderButtonRetornar(Europa.Controllers.PrePropostaAguardandoAnalise.Permissoes.RetornarSituacao, Europa.i18n.Messages.Retornar, 'fa fa-reply', 'retornar(' + meta.row + ')', meta.row);

        //Trocar a PPR de CCA
        content = content + $scope.renderButtonAlterarCCA(Europa.Controllers.PrePropostaAguardandoAnalise.Permissoes.TrocaCCA, "Trocar CCA da PPR", 'fa fa-table', 'trocarCCA(' + full.IdPreProposta + ')', full.StatusPreProposta, meta.row);
        //if (full.IdAvalista &&
        //    (Europa.Controllers.PrePropostaAguardandoAnalise.SituacaoDocumentoAvalista_Enviado === full.SituacaoDocumento ||
        //    Europa.Controllers.PrePropostaAguardandoAnalise.SituacaoDocumentoAvalista_Aprovado === full.SituacaoDocumento)) {
        //    content = content + "<a class='btn btn-default' title='Avalista' target='_blank' href='" + Europa.Controllers.PrePropostaAguardandoAnalise.UrlDocumentacaoAvalista + '?id=' + full.Id + "'><i class='fa fa-folder-open-o'></i></a>";
        //}

        content = content + "<a class='btn btn-default' title='Visualizar' href='" + Europa.Controllers.PrePropostaAguardandoAnalise.UrlVerDocumentacao + '?id=' + full.Id + '&codigoUf=' + Europa.Controllers.PrePropostaAguardandoAnalise.CodigoUf + "'><i class='fa fa-eye'></i></a>";

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

    $scope.alterarSicaqPrevio = function (row) {
        $("#StatusSicaqPrevio").removeAttr("disabled");
        $("#sicaq-previo").addClass("hidden")
        var objeto = Europa.Controllers.PrePropostaAguardandoAnalise.Tabela.getRowData(row);
        Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarSicaq.AbrirModalPrevio(objeto.Id);

    };

    $scope.alterarSicaq = function (row) {
        $("#StatusSicaqPrevio").attr("disabled", "disabled");
        $("#sicaq-previo").addClass("hidden")
        var objeto = Europa.Controllers.PrePropostaAguardandoAnalise.Tabela.getRowData(row);
        Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarSicaq.AbrirModal(objeto.Id);
        $("#sicaq-previo").removeClass("hidden");

    };

    $scope.revisar = function (row) {
        var objeto = Europa.Controllers.PrePropostaAguardandoAnalise.Tabela.getRowData(row);

        var situacao = objeto.StatusPreProposta == 2 ? Europa.i18n.Messages.SituacaoProposta_EmAnaliseSimplificada : Europa.i18n.Messages.SituacaoProposta_EmAnaliseCompleta;

        Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format("Deseja confirma o retorno da pré-proposta para {0}", situacao));
        Europa.Confirmacao.ConfirmCallback = function () {
            $.post(Europa.Controllers.PrePropostaAguardandoAnalise.UrlRevisarPreProposta, { idProposta: objeto.Id }, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.PrePropostaAguardandoAnalise.Tabela.reloadData();
                }
                Europa.Informacao.PosAcao(res);
            });
        }
        Europa.Confirmacao.Show();
    };

    $scope.retornar = function (row) {
        var objeto = Europa.Controllers.PrePropostaAguardandoAnalise.Tabela.getRowData(row);
        Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.RetornarSituacaoAnterior, Europa.i18n.Messages.SituacaoPreProposta));
        Europa.Confirmacao.ConfirmCallback = function () {
            $.post(Europa.Controllers.PrePropostaAguardandoAnalise.UrlRetornarSituacao, { idPreProposta: objeto.Id }, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.PrePropostaAguardandoAnalise.Tabela.reloadData();
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

    $scope.renderButtonAlterarCCA = function (hasPermission, title, icon, onClick, situacaoPreProposta) {
        if (hasPermission !== 'true') {
            return "";
        }
        if (situacaoPreProposta != 6 && situacaoPreProposta != 7 && situacaoPreProposta != 17 && situacaoPreProposta!= 18) {
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
        var objeto = Europa.Controllers.PrePropostaAguardandoAnalise.Tabela.getRowData(row);
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
            return "R$ " + value.toFixed(2).replace(".", ",").replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
        }
    }

    $scope.trocarCCA = function (IdPreProposta) {
        $('#IdPreProposta').val(IdPreProposta);
        Europa.Controllers.PrePropostaAguardandoAnalise.CCAsPPR.Tabela.reloadData();
        $('#alterar_cca_modal').show();
        $.post(Europa.Controllers.PrePropostaAguardandoAnalise.UrlListarCCA, { IdPreProposta: IdPreProposta }, function (data) {
            console.log(data);
            var html = '';
            html += '<option value="" selected>Selecione um CCA...</option>'
            $.each(data, function (index, obj) {
                html += '<option value="' + obj.Id + '">' + obj.Descricao + '</option>';
            });
            $("#select-novo-cca").html(html);
            $("#select-novo-cca").select2({
                trags: true
            });
        });

    }
}



Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarCCA.FecharModal = function () {
    $('#alterar_cca_modal').hide();
    $('#error-label').html("");
    $("select-novo-cca").val("").change();
}


Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarCCA.Alterar = function () {
    var filtro = Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarCCA.Filtro();
    if (filtro.IdCCADestino == "" || filtro.IdCCADestino == 0) {
        $('#error-label').html("É necessário selecionar um CCA de destino!");
    } else {
        $.post(Europa.Controllers.PrePropostaAguardandoAnalise.UrlAlterarCCA, filtro, function (res) {
            if (res.Sucesso) {
                Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarCCA.FecharModal();
                Europa.Messages.ShowMessages(res, Europa.i18n.Messages.Sucesso);
                Europa.Controllers.PrePropostaAguardandoAnalise.Tabela.reloadData();
            } else {
                $('#error-label').html(res.Mensagens);
            }
        });
    }
};

Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarCCA.Filtro = function () {
    return {
        IdCCADestino: $("#select-novo-cca").val(),
        IdCCAOrigem: 0,
        IdPreProposta: $('#IdPreProposta').val(),
        IdEmpresaVenda: 0
    };
};

DataTableApp.controller('PrePropostasAguardandoAnalise', CriarTabelaPrePropostaAguardandoAnalise);

Europa.Controllers.PrePropostaAguardandoAnalise.Filtro = function () {
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
        AvalistaPendente: $("#check_avalista").val() == "true"
    };
};

    Europa.Controllers.PrePropostaAguardandoAnalise.AlternarExibicaoInformacoes = function () {
        if (!Europa.Controllers.PrePropostaAguardandoAnalise.ExibindoInfo) {
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

            Europa.Controllers.PrePropostaAguardandoAnalise.InitDatePicker();
        }
        Europa.Controllers.PrePropostaAguardandoAnalise.ExibindoInfo = !Europa.Controllers.PrePropostaAguardandoAnalise.ExibindoInfo;
    };


    Europa.Controllers.PrePropostaAguardandoAnalise.ExportarPagina = function () {
        var params = Europa.Controllers.PrePropostaAguardandoAnalise.Filtro();
        params.order = Europa.Controllers.PrePropostaAguardandoAnalise.Tabela.lastRequestParams.order;
        params.draw = Europa.Controllers.PrePropostaAguardandoAnalise.Tabela.lastRequestParams.draw;
        params.pageSize = Europa.Controllers.PrePropostaAguardandoAnalise.Tabela.lastRequestParams.pageSize;
        params.start = Europa.Controllers.PrePropostaAguardandoAnalise.Tabela.lastRequestParams.start;
        var formExportar = $("#form_exportar");
        formExportar.find("input").remove();
        formExportar.attr("method", "post").attr("action", Europa.Controllers.PrePropostaAguardandoAnalise.UrlExportarPagina);
        formExportar.addHiddenInputData(params);
        formExportar.submit();
    };

    Europa.Controllers.PrePropostaAguardandoAnalise.ExportarTodos = function () {
        var params = Europa.Controllers.PrePropostaAguardandoAnalise.Filtro();
        params.order = Europa.Controllers.PrePropostaAguardandoAnalise.Tabela.lastRequestParams.order;
        params.draw = Europa.Controllers.PrePropostaAguardandoAnalise.Tabela.lastRequestParams.draw;
        var formExportar = $("#form_exportar");
        formExportar.find("input").remove();
        formExportar.attr("method", "post").attr("action", Europa.Controllers.PrePropostaAguardandoAnalise.UrlExportarTodos);
        formExportar.addHiddenInputData(params);
        formExportar.submit();
    };

    Europa.Controllers.PrePropostaAguardandoAnalise.Filtrar = function () {
        Europa.Controllers.PrePropostaAguardandoAnalise.Tabela.reloadData();
    };

    Europa.Controllers.PrePropostaAguardandoAnalise.LimparFiltro = function () {
        $("#filtro").find("input[type=text], textarea").val("");
        $("#SituacoesPreProposta").val("").trigger('change');
        $("#autocomplete_regional").val("").trigger("change");
        $("#UF").val("").trigger("change");
        $("#autocomplete_empresa_venda").val("").trigger("change");
        $("#autocomplete_breve_lancamento").val("").trigger("change");
        $("#autocomplete_viabilizador").val("").trigger("change");
        $("#check_avalista").prop("checked", false);
        $("#check_avalista").val("false");

    };

    Europa.Controllers.PrePropostaAguardandoAnalise.OnCheckAvalista = function () {
        if ($("#check_avalista").val() == "true") {
            $("#check_avalista").val("false");
        } else {
            $("#check_avalista").val("true");
        }
    };


