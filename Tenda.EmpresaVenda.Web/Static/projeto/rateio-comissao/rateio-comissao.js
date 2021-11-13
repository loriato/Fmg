Europa.Controllers.RateioComissao = {};
Europa.Controllers.RateioComissao.Tabela = undefined;
Europa.Controllers.RateioComissao.Permissoes = undefined;
Europa.Controllers.RateioComissao.Modal = "#modal_rateio_comissao";
Europa.Controllers.RateioComissao.IdFormRateioComissao = "#form_rateio_comissao"

$(function () {
    Europa.Controllers.RateioComissao.InitAutoComplete();

    Europa.Components.DatePicker.AutoApply();
})

Europa.Controllers.RateioComissao.InitAutoComplete = function () {

    Europa.Controllers.RateioComissao.AutoCompleteEmpresaVendaContratanteFiltro = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("contratante_filtro")
        .Configure();

    Europa.Controllers.RateioComissao.AutoCompleteEmpresaVendaContratadaFiltro = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("contratada_filtro")
        .Configure();

    Europa.Controllers.RateioComissao.AutoCompleteEmpreendimentoFiltro = new Europa.Components.AutoCompleteEmpreendimento()
        .WithTargetSuffix("empreendimento_filtro")
        .Configure();
    Europa.Controllers.RateioComissao.AutoCompleteEmpresaVendaContratante = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("contratante")
        .Configure();

    Europa.Controllers.RateioComissao.AutoCompleteEmpresaVendaContratada = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("contratada")
        .Configure();

    Europa.Controllers.RateioComissao.AutoCompleteEmpreendimento = new Europa.Components.AutoCompleteEmpreendimento()
        .WithTargetSuffix("empreendimento")
        .Configure();
};



function TabelaRateioComissao($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.RateioComissao.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.RateioComissao.Tabela;
    tabelaWrapper
        .setColumns([
            DTColumnBuilder.newColumn('Id').withTitle(Europa.i18n.Messages.Id).withOption('visible', false),
            DTColumnBuilder.newColumn('NomeContratante').withTitle(Europa.i18n.Messages.ContratanteCentral).withOption('width', '100px'),
            DTColumnBuilder.newColumn('NomeContratada').withTitle(Europa.i18n.Messages.ContratadaForcaVendas).withOption('width', '100px'),
            DTColumnBuilder.newColumn('NomeEmpreendimento').withTitle(Europa.i18n.Messages.Empreendimento).withOption('width', '100px').renderWith(renderNomeEmpreendimento),
            DTColumnBuilder.newColumn('InicioVigencia').withTitle(Europa.i18n.Messages.InicioVigencia).withOption('width', '100px').renderWith(Europa.String.FormatAsGeenDate),
            DTColumnBuilder.newColumn('TerminoVigencia').withTitle(Europa.i18n.Messages.TerminoVigencia).withOption('width', '100px').renderWith(Europa.String.FormatAsGeenDate),
            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('type', 'enum-format-SituacaoRateioComissao').withOption('width', '100px')
            
        ])
        .setColActions(actionsHtml, '60px')
        .setDefaultOrder([[1, 'desc']])
        .setIdAreaHeader("datatable_header")
        .setActionSave(Europa.Controllers.RateioComissao.Salvar)
        .setDefaultOptions('POST', Europa.Controllers.RateioComissao.UrlListar, Europa.Controllers.RateioComissao.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButtonRascunho(Europa.Controllers.RateioComissao.Permissoes.Ativar, "Ativar", "fa fa-check", "Ativar(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonFinalizar(Europa.Controllers.RateioComissao.Permissoes.Finalizar, "Finalizar", "fa fa-close", "Finalizar(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonRascunho(Europa.Controllers.RateioComissao.Permissoes.Excluir, "Excluir", "fa fa-trash", "Excluir(" + meta.row + ")", full.Situacao) +
            '</div>';
    }

    function renderNomeEmpreendimento(data) {
        if (data == null) {
            return 'TODOS';
        }
        return data;
    }
    $scope.renderButtonFinalizar = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission === 'true' && situacao == 1) {
            icon = $('<i/>').addClass(icon);
            var button = $('<a />')
                .addClass('btn btn-default')
                .attr('title', title)
                .attr('ng-click', onClick)
                .append(icon);
            return button.prop('outerHTML');
        } else {
            return " ";
        }
    };
    $scope.renderButtonRascunho = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission === 'true' && situacao == 3) {
            icon = $('<i/>').addClass(icon);
            var button = $('<a />')
                .addClass('btn btn-default')
                .attr('title', title)
                .attr('ng-click', onClick)
                .append(icon);
            return button.prop('outerHTML');
        } else {
            return " ";
        }
    };
    $scope.Ativar = function (row) {
        var objetoLinhaTabela = Europa.Controllers.RateioComissao.Tabela.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Ativar, "do rateio de comissão", function () {
            Europa.Controllers.RateioComissao.Ativar(objetoLinhaTabela.Id);
        });
    };

    $scope.Finalizar = function (row) {
        var objetoLinhaTabela = Europa.Controllers.RateioComissao.Tabela.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Finalizar, "do rateio de comissão", function () {
            Europa.Controllers.RateioComissao.Finalizar(objetoLinhaTabela.Id);
        });
    };
    $scope.Excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.RateioComissao.Tabela.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, "do rateio de comissão", function () {
            Europa.Controllers.RateioComissao.Excluir(objetoLinhaTabela.Id);
        });
    };
};

DataTableApp.controller('RateioComissao', TabelaRateioComissao);

Europa.Controllers.RateioComissao.Filtrar = function () {
    Europa.Controllers.RateioComissao.Tabela.reloadData();
}

Europa.Controllers.RateioComissao.FilterParams = function () {
    var param = {
        IdContratante: $("#autocomplete_contratante_filtro").val(),
        IdContratada: $("#autocomplete_contratada_filtro").val(),
        IdEmpreendimento: $("#autocomplete_empreendimento_filtro").val(),
        VigenteEm: $("#vigente_em").val()
    };
    return param;
};

Europa.Controllers.RateioComissao.Novo = function () {
    $(Europa.Controllers.RateioComissao.Modal).modal("show");
    Europa.Controllers.RateioComissao.LimparModal(); 
    Europa.Controllers.RateioComissao.LimparErro(); 

};

Europa.Controllers.RateioComissao.Salvar = function () {
    var validar = Europa.Controllers.RateioComissao.Validar();
    if (!validar) {
        return;
    }

    var contratante = {
        Id: Europa.Controllers.RateioComissao.AutoCompleteEmpresaVendaContratante.Value()
    }
    var contratada = {
        Id: Europa.Controllers.RateioComissao.AutoCompleteEmpresaVendaContratada.Value()
    }
    
    var empreendimento = {
        Id: Europa.Controllers.RateioComissao.AutoCompleteEmpreendimento.Value()
    }

    var obj = {
        Contratante: contratante,
        Contratada: contratada,
        Empreendimento: empreendimento
    };
    var url = Europa.Controllers.RateioComissao.UrlIncluir;
    $.post(url, { model: obj }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.RateioComissao.Tabela.reloadData();
            Europa.Controllers.RateioComissao.LimparErro();
            $(Europa.Controllers.RateioComissao.Modal).modal("hide");
        } else {
            Europa.Controllers.RateioComissao.AdicionarErro(resposta.Campos);
        }
    });
};
Europa.Controllers.RateioComissao.Ativar = function (id) {
    $.post(Europa.Controllers.RateioComissao.UrlAtivar, { id: id }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.RateioComissao.Tabela.reloadData();
        }
    });
};

Europa.Controllers.RateioComissao.Finalizar = function (id) {
    $.post(Europa.Controllers.RateioComissao.UrlFinalizar, { id: id }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.RateioComissao.Tabela.reloadData();
        }
    });
};

Europa.Controllers.RateioComissao.Excluir = function (id) {
    $.post(Europa.Controllers.RateioComissao.UrlExcluir, { id: id }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.RateioComissao.Tabela.reloadData();
        }
    });
};
Europa.Controllers.RateioComissao.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.RateioComissao.LimparErro = function () {
    $(".has-error").removeClass("has-error");
};

Europa.Controllers.RateioComissao.LimparModal = function () {
    Europa.Controllers.RateioComissao.AutoCompleteEmpresaVendaContratante.Clean();
    Europa.Controllers.RateioComissao.AutoCompleteEmpresaVendaContratada.Clean();
    Europa.Controllers.RateioComissao.AutoCompleteEmpreendimento.Clean();

    var $radios = $('input:radio[name=radio-select]')
    $radios.filter('[value=Todos]').prop('checked', true);
    Europa.Controllers.RateioComissao.OnChangeTodos()
    
};

Europa.Controllers.RateioComissao.LimparFiltro = function () {
    Europa.Controllers.RateioComissao.AutoCompleteEmpresaVendaContratanteFiltro.Clean();
    Europa.Controllers.RateioComissao.AutoCompleteEmpresaVendaContratadaFiltro.Clean();
    Europa.Controllers.RateioComissao.AutoCompleteEmpreendimentoFiltro.Clean();
    $("#vigente_em").val(" ");
};

Europa.Controllers.RateioComissao.OnChangeTodos = function () {
    $("#autocomplete_empreendimento").attr("disabled", "disabled");
    $("#autocomplete_empreendimento").val("").trigger('change');
};

Europa.Controllers.RateioComissao.OnChangeSelecionar = function () {
    $("#autocomplete_empreendimento").removeAttr("disabled");
};

Europa.Controllers.RateioComissao.Validar = function () {
    var radio = $('input[name=radio-select]:checked').val(); 
    var empreendimento = Europa.Controllers.RateioComissao.AutoCompleteEmpreendimento.Value();
    if (radio == 'Selecionar')
    if (empreendimento == undefined || empreendimento == "" || empreendimento == null) {
        var res = {
            Sucesso: false,
            Mensagens: ["Selecione um Empreendimento"]
        }
        Europa.Informacao.PosAcao(res);
        return false;
    }
    return true;
};
