Europa.Controllers.Lead = {};


$(function () {
    $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
    $(".ng-scope").css("word-wrap", "break-word");
    Europa.Controllers.Lead.InitAutoCompletes();
    $("#filtro_situacao_pacote").val("false");
    Europa.Controllers.Lead.TreeViewPacote = new Europa.Components.TreeView("pacote_treeview");
    Europa.Mask.Telefone("#filtro_telefone", true);
});

Europa.Controllers.Lead.InitAutoCompletes = function () {

    Europa.Controllers.Lead.AutoCompleteEmpresaVenda = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("empresa_venda").Configure();

    Europa.Controllers.Lead.AutoCompletePacote = new Europa.Components.AutoCompletePacote()
        .WithTargetSuffix("pacote").Configure();

    Europa.Controllers.Lead.AutoCompletePacoteExcluir = new Europa.Components.AutoCompletePacote()
        .WithTargetSuffix("pacote_excluir").Configure();

    Europa.Controllers.Lead.AutoCompleteCorretor = new Europa.Components.AutoCompleteCorretor()
        .WithTargetSuffix("corretor").Configure();

    Europa.Controllers.Lead.ConfigurePacoteNaoLiberadoAutocomplete(Europa.Controllers.Lead.AutoCompletePacoteExcluir);

};
Europa.Controllers.Lead.ConfigurePacoteNaoLiberadoAutocomplete = function (autocompleteWrapper) {
    autocompleteWrapper.Data = function (params) {
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
                        return false;
                    },
                    column: 'liberado'
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
    autocompleteWrapper.Configure();
}


function LeadDatatable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.Lead.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.Lead.Tabela;
    self.setColumns([
        DTColumnBuilder.newColumn('Pacote').withTitle(Europa.i18n.Messages.Pacote).withOption('width', '150px'),
        DTColumnBuilder.newColumn('Liberar').withTitle(Europa.i18n.Messages.SituacaoPacote).withOption('width', '150px').renderWith(formatLiberar),
        DTColumnBuilder.newColumn('NomeEmpresaVenda').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption('width', '200px'),
        DTColumnBuilder.newColumn('CodigoPreProposta').withTitle(Europa.i18n.Messages.PreProposta).withOption('width', '200px'),
        DTColumnBuilder.newColumn('NomeLead').withTitle(Europa.i18n.Messages.Nome).withOption('width', '250px'),
        DTColumnBuilder.newColumn('Telefone1').withTitle(Europa.i18n.Messages.Telefone1).withOption('width', '150px').renderWith(Europa.String.FormatAsPhone),
        DTColumnBuilder.newColumn('Telefone2').withTitle(Europa.i18n.Messages.Telefone2).withOption('width', '150px').renderWith(Europa.String.FormatAsPhone),
        DTColumnBuilder.newColumn('Email').withTitle(Europa.i18n.Messages.Email).withOption('width', '250px'),
        DTColumnBuilder.newColumn('Logradouro').withTitle(Europa.i18n.Messages.Logradouro).withOption('width', '250px'),
        DTColumnBuilder.newColumn('Numero').withTitle(Europa.i18n.Messages.Numero).withOption('width', '150px'),
        DTColumnBuilder.newColumn('Complemento').withTitle(Europa.i18n.Messages.Complemento).withOption('width', '250px'),
        DTColumnBuilder.newColumn('Bairro').withTitle(Europa.i18n.Messages.Bairro).withOption('width', '250px'),
        DTColumnBuilder.newColumn('Cidade').withTitle(Europa.i18n.Messages.Cidade).withOption('width', '250px'),
        DTColumnBuilder.newColumn('Uf').withTitle(Europa.i18n.Messages.Estado).withOption('width', '150px'),
        DTColumnBuilder.newColumn('CEP').withTitle(Europa.i18n.Messages.CEP).withOption('width', '200px'),
        DTColumnBuilder.newColumn('NomeCorretor').withTitle(Europa.i18n.Messages.Corretor).withOption('width', '250px'),
        DTColumnBuilder.newColumn('SituacaoLead').withTitle(Europa.i18n.Messages.SituacaoLead)
            .withOption('width', '150px').withOption('type', 'enum-format-SituacaoLead'),
        DTColumnBuilder.newColumn('Desistencia').withTitle(Europa.i18n.Messages.MotivoDesistencia)
            .withOption('width', '150px').withOption('type', 'enum-format-TipoDesistencia'),
        DTColumnBuilder.newColumn('DescricaoDesistencia').withTitle(Europa.i18n.Messages.Descricao + " " + Europa.i18n.Messages.Outros).withOption('width', '300px'),
        DTColumnBuilder.newColumn('NomeCliente').withTitle(Europa.i18n.Messages.NomeIndicador).withOption('width', '250px'),
        DTColumnBuilder.newColumn('CpfCliente').withTitle(Europa.i18n.Messages.CpfIndicador).withOption('width', '250px').renderWith(Europa.String.FormatCpf),
        DTColumnBuilder.newColumn('StatusIndicacao').withTitle(Europa.i18n.Messages.StatusIndicacao).withOption('width', '250px'),
        DTColumnBuilder.newColumn('DataIndicacao').withTitle(Europa.i18n.Messages.DataIndicacao).withOption('width', '250px').renderWith(Europa.Controllers.Lead.DataNula)
       
    ])
        .setAutoInit(false)
        .setIdAreaHeader("lead_datatable_header")
        .setColActions(actionsHtml, '50px')
        .setDefaultOrder([[1, 'desc']])
        .setOptionsMultiSelect('POST', Europa.Controllers.Lead.UrlListarDatatable, Europa.Controllers.Lead.Filtro);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButton(Europa.Controllers.Lead.Permissoes.Visualizar, "Visualizar", "fa fa-eye", "detalhar(" + meta.row + ")") +
            '</div>';
    }

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (!hasPermission) {
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

    $scope.detalhar = function (row) {
        var obj = Europa.Controllers.Lead.Tabela.getRowData(row);
        Europa.Controllers.Lead.ModalDetalhar(obj);
    };
    function formatLiberar(data) {
        if (data) {
            return "Liberado";
        } 
        return "Não Liberado";
    };

};

DataTableApp.controller('Lead', LeadDatatable);

Europa.Controllers.Lead.Filtrar = function () {
    Europa.Controllers.Lead.Tabela.reloadData();
};

Europa.Controllers.Lead.DataNula = function (stringDate) {
    if (stringDate != /Date(-62135589600000)/) {
        return Europa.Date.toGeenDateFormat(stringDate);
    }
    return "";
};

Europa.Controllers.Lead.Filtro = function () {
    param = {
        IdEmpresaVenda: $("#autocomplete_empresa_venda").val(),
        SituacaoLead: $("#filtro_situacoes").val(),
        Pacote: $("#autocomplete_pacote").val(),
        IdCorretor: $("#autocomplete_corretor").val(),
        Liberar: $("#filtro_situacao_pacote").val(),
        CodigoPreProposta: $("#filtro_pre_proposta").val(),
        NomeLead: $("#filtro_lead").val(),
        Telefone: $("#filtro_telefone").val(),
        DataIndicacaoDe: $("#DataIndicacaoDeLead").val(),
        DataIndicacaoAte: $("#DataIndicacaoAteLead").val()
    };
    return param;
};

Europa.Controllers.Lead.LimparFiltro = function () {
    Europa.Controllers.Lead.AutoCompleteEmpresaVenda.Clean();
    $("#filtro_situacoes").val("");
    Europa.Controllers.Lead.AutoCompletePacote.Clean();
    Europa.Controllers.Lead.AutoCompleteCorretor.Clean();
    $("#filtro_situacao_pacote").val("");
    $("#filtro_pre_proposta").val(" ");
    $("#filtro_lead").val(" ");
    $("#filtro_telefone").val("");
    $("#DataIndicacaoDeLead").val("");
    $("#DataIndicacaoAteLead").val("");

};


Europa.Controllers.Lead.ExportarPagina = function () {

    var params = Europa.Controllers.Lead.Filtro();
    params.order = Europa.Controllers.Lead.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.Lead.Tabela.lastRequestParams.draw;
    params.pageSize = Europa.Controllers.Lead.Tabela.lastRequestParams.pageSize;
    params.start = Europa.Controllers.Lead.Tabela.lastRequestParams.start;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.Lead.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.Lead.ExportarTodos = function () {
    var dataDe = $("#DataIndicacaoDeLead").val();
    var dataAte = $("#DataIndicacaoAteLead").val();

    if (dataDe != "") {
        var dataDe = Europa.Date.AddDay(0, dataDe);
    }
    if (dataAte != "") {
        var dataAte = Europa.Date.AddDay(0, dataAte);
        var maxData = Europa.Date.AddDay(Europa.Controllers.Lead.QuantidadeDiasExportarTodos, dataDe);
    }
    if (dataDe != "" && dataAte != "") {
        if (dataAte > maxData) {
            var msgs = [];
            msgs.push(Europa.i18n.Messages.PeriodoBuscaLongo)
            var res = {
                Sucesso: false,
                Mensagens: msgs,
            };
            Europa.Informacao.PosAcao(res);
        } else {
            var params = Europa.Controllers.Lead.Filtro();
            params.order = Europa.Controllers.Lead.Tabela.lastRequestParams.order;
            params.draw = Europa.Controllers.Lead.Tabela.lastRequestParams.draw;
            var formExportar = $("#Exportar");
            formExportar.find("input").remove();
            formExportar.attr("method", "post").attr("action", Europa.Controllers.Lead.UrlExportarTodos);
            formExportar.addHiddenInputData(params);
            formExportar.submit();
        }

    } else {
        var msgs = [];
        msgs.push(Europa.i18n.Messages.InsiraPeriodoExportar)
        var res = {
            Sucesso: false,
            Mensagens: msgs,
        };
        Europa.Informacao.PosAcao(res);
    }
};

Europa.Controllers.Lead.AbriModalLiberar = function () {
    Europa.Controllers.Lead.InitTreeView();
    $("#modal_liberar_pacote").modal("show");
    $("#form_liberar_pacote").removeClass("has-error");
};

Europa.Controllers.Lead.AbriModalExportarTodos = function () {
    $("#modal_exportar_todos").modal("show");
    Europa.Controllers.Lead.InitModalExportarTodos();
};

Europa.Controllers.Lead.FecharModalExportarTodos = function () {
    $("#DataIndicacaoDeLead").val("");
    $("#DataIndicacaoAteLead").val("");
    $("#modal_exportar_todos").modal("hide");
};

Europa.Controllers.Lead.AbriModalExcluir = function () {
    Europa.Controllers.Lead.AutoCompletePacoteExcluir.Clean();
    $("#modal_excluir_pacote").modal("show");
    $("#form_excluir_pacote").removeClass("has-error");
};

Europa.Controllers.Lead.ExcluirPacote = function () {
    var pacote = $("#autocomplete_pacote_excluir").val();
    if (pacote != 0 && pacote != "" && pacote != null)
    {
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, pacote, function () {
            $.post(Europa.Controllers.Lead.UrlExcluirPacote, { pacote: pacote }, function (res) {
                if (res.Sucesso) {
                    $("#modal_excluir_pacote").modal("hide");
                    Europa.Controllers.Lead.LimparFiltro();
                    Europa.Controllers.Lead.Filtrar();
                }
                Europa.Informacao.PosAcao(res);
            });
        });
    }
    else
    {
        $("#form_excluir_pacote").addClass("has-error");
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.i18n.Messages.SelecioneRegionalAcao);
        Europa.Informacao.Show();
    }
    
}

Europa.Controllers.Lead.InitTreeView = function () {
    Europa.Controllers.Lead.TreeViewPacote
        .WithAjax("GET",
            Europa.Controllers.Lead.UrlListarPacote)
        .WithShowCheckbox(true)
        .WithRowCheck(true)
        .WithExpandIcon(false)
        .WithCollapseIcon(false)
        .WithCheckRootSiblings(true)
        .Configure();
};

Europa.Controllers.Lead.MarcarTodos = function () {
    Europa.Controllers.Lead.TreeViewPacote.CheckAllNodes();
};

Europa.Controllers.Lead.DesmarcarTodos = function () {
    Europa.Controllers.Lead.TreeViewPacote.UncheckAllNodes();
};


Europa.Controllers.Lead.LiberarPacote = function () {
    var selecionados = Europa.Controllers.Lead.TreeViewPacote.GetCheckedNodes();

    if(selecionados.length == 0) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, Europa.i18n.Messages.MsgSelecioneEmpresaVenda);
        Europa.Informacao.Show();
        return;
    }

    var pacotes = [];
    selecionados.forEach(function (item) {
        pacotes.push(item.text);
    });
    param = {
        pacote: pacotes
    };
    $.post(Europa.Controllers.Lead.UrlLiberarPacote, param, function (res) {
        if (res.Sucesso) {
            $("#modal_liberar_pacote").modal("hide");
        } else {
            $("#form_liberar_pacote").addClass("has-error");
        }
        Europa.Informacao.PosAcao(res)
    });
};

Europa.Controllers.Lead.OnChangeDataInicio = function () {
    var maxDate = Europa.Date.AddDay(Europa.Controllers.Lead.QuantidadeDiasExportarTodos,$("#DataIndicacaoDeLead").val());
    Europa.Controllers.Lead.DataIndicacaoAte = new Europa.Components.DatePicker()
        .WithTarget("#DataIndicacaoAteLead")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#DataIndicacaoDeLead").val())
        .WithMaxDate(maxDate)
        .Configure();
}

Europa.Controllers.Lead.InitModalExportarTodos = function () {
    Europa.Controllers.Lead.DataIndicacaoDe = new Europa.Components.DatePicker()
        .WithTarget("#DataIndicacaoDeLead")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(new Date())
        .Configure();

    Europa.Controllers.Lead.DataIndicacaoAte = new Europa.Components.DatePicker()
        .WithTarget("#DataIndicacaoAteLead")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#DataIndicacaoDeLead").val())
        .Configure();
};