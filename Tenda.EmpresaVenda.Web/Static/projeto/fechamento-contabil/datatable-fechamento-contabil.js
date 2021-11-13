

$(function () {

});

DataTableApp.controller('fechamentoContabilTable', fechamentoContabilTable);

function fechamentoContabilTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

    Europa.Controllers.FechamentoContabil.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.FechamentoContabil.Tabela;
    tabelaWrapper.setTemplateEdit([
        '<input type="text" id="Descricao" class="form-control" name="Descricao" value="">',
        '<input type="text" id="InicioFechamento" class="form-control" name="InicioFechamento" value="" onchange="Europa.Controllers.FechamentoContabil.OnChangeTerminoFechamento()">',
        '<input type="text" id="TerminoFechamento" class="form-control" name="TerminoFechamento" value="">',
        '<input type="text" id="QuantidadeDiasLembrete" class="form-control" name="QuantidadeDiasLembrete" value="">',
    ])
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('Descricao').withTitle(Europa.i18n.Messages.Descricao).withOption('width', '10%'),
        DTColumnBuilder.newColumn('InicioFechamento').withTitle(Europa.i18n.Messages.InicioFechamento).withOption('width', '15%').renderWith(Europa.Date.toGeenDateFormat),
        DTColumnBuilder.newColumn('TerminoFechamento').withTitle(Europa.i18n.Messages.TerminoFechamento).withOption('width', '15%').renderWith(Europa.Date.toGeenDateFormat),
        DTColumnBuilder.newColumn('QuantidadeDiasLembrete').withTitle(Europa.i18n.Messages.QuantidadeDiasLembrete).withOption('width', '5%'),
    ])
        .setActionSave(Europa.Controllers.FechamentoContabil.SalvarFechamentoContabil)
        .setIdAreaHeader("fechamento_contabil_datatable_header")
        .setColActions(actionsHtml, '2%')
        .setDefaultOptions('POST', Europa.Controllers.FechamentoContabil.UrlListarFechamentoContabil, Europa.Controllers.FechamentoContabil.FilterParams);

    function actionsHtml(data, type, full, meta) {
        var button = '<div>';
        if (!full.EmReversao) {
            button += $scope.renderButtonNotify('Reenviar notificações', 'fa fa-envelope ', 'ReenviarNoficacao(' + meta.row + ')', full.InicioFechamento);
            button += '</div>';
            return button;
        }
    }

    $scope.renderButtonNotify = function (title, icon, onClick, data) {

        var inicio = Europa.Date.toFormatDate(data, Europa.Date.FORMAT_DATE);
        var array = inicio.split("/");
        var novoInicio = (array[2] + "-" + array[1] + "-" + array[0]);

        var novaData = Date.parse(novoInicio);

        var hoje = new Date();

        if (hoje > novaData) {
            return "";
        }
        
        icon = $('<i/>').addClass(icon);

        var button = $('<a />')
            .addClass('btn btn-default gerar-rc')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);


        return button.prop('outerHTML');
    }

    $scope.Excluir = function (row) {
        var objeto = Europa.Controllers.FechamentoContabil.Tabela.getRowData(row);

        var fechamento = {
            IdFechamento: objeto.Id,
            Descricao:objeto.Descricao
        }

        Europa.Controllers.FechamentoContabil.ExcluirFechamentoContabil(fechamento);
        
    };

    $scope.ReenviarNoficacao = function (row) {
        var fechamento = Europa.Controllers.FechamentoContabil.Tabela.getRowData(row);
        fechamento.InicioFechamento = Europa.Date.toFormatDate(fechamento.InicioFechamento, Europa.Date.FORMAT_DATE);
        fechamento.TerminoFechamento = Europa.Date.toFormatDate(fechamento.TerminoFechamento, Europa.Date.FORMAT_DATE);
        $.post(Europa.Controllers.FechamentoContabil.UrlEnviarNotificacaoEmail, { fechamento }, function (res) {
            Europa.Informacao.PosAcao(res);
        });
    };
}

Europa.Controllers.FechamentoContabil.FilterParams = function () {
    return {
        InicioFechamento: $("#InicioFechamentoFiltro").val(),
        TerminoFechamento: $("#TerminoFechamentoFitlro").val()
    };
};

Europa.Controllers.FechamentoContabil.FiltrarTabela = function () {
    Europa.Controllers.FechamentoContabil.Tabela.reloadData();
};

Europa.Controllers.FechamentoContabil.LimparFiltro = function () {
    $("#InicioFechamentoFiltro").val("");
    $("#TerminoFechamentoFitlro").val("");
}

Europa.Controllers.FechamentoContabil.Adicionar = function () {
    Europa.Controllers.FechamentoContabil.Tabela.createRowNewData();
    Europa.Controllers.FechamentoContabil.InitDatepicker();
};

Europa.Controllers.FechamentoContabil.FechamentoContabilDto = function () {
    return {
        Descricao: $("#Descricao").val(),
        InicioFechamento: $("#InicioFechamento").val(),
        TerminoFechamento: $("#TerminoFechamento").val(),
        QuantidadeDiasLembrete: $("#QuantidadeDiasLembrete").val(),
    }
};

Europa.Controllers.FechamentoContabil.SalvarFechamentoContabil = function () {
    var fechamentoDto = Europa.Controllers.FechamentoContabil.FechamentoContabilDto();

    var url = Europa.Controllers.FechamentoContabil.UrlInserirFechamentoContabil;

    $(".has-error").removeClass("has-error");

    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Confirmacao, "Período de fechamento contábil não pode ser alterado ou excluído. Confirma inclusão?");
    Europa.Confirmacao.ConfirmCallback = function () {
        $.post(url, { fechamentoDto: fechamentoDto }, function (res) {

            if (res.Sucesso) {
                Europa.Controllers.FechamentoContabil.Tabela.closeEdition();
                Europa.Controllers.FechamentoContabil.FiltrarTabela();
                Europa.Informacao.Hide = function () {
                    $(Europa.Informacao.Attr.Modal).modal("hide");
                    Europa.Controllers.FechamentoContabil.EnviarNotificacao(res.Objeto);
                } 
            } else {
                Europa.Controllers.FechamentoContabil.AddError(res.Campos);
            }

            Europa.Informacao.PosAcao(res);

        });
    }
    Europa.Confirmacao.Show();
};

Europa.Controllers.FechamentoContabil.ExcluirFechamentoContabil = function (fechamentoDto) {

    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Confirmacao, "Esta ação não pode ser desfeita.Deseja continuar ?");
    Europa.Confirmacao.ConfirmCallback = function () {
        $.post(Europa.Controllers.FechamentoContabil.UrlExcluirFechamentoContabil,
            { fechamentoDto: fechamentoDto }, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.FechamentoContabil.FiltrarTabela();
                }
                Europa.Informacao.PosAcao(res);
            });
    }
    Europa.Confirmacao.Show();
}

Europa.Controllers.FechamentoContabil.AddError = function (campos) {

    if (campos == undefined) {
        return;
    }

    campos.forEach(function (key) {
        $("[name='" + key + "']").parent().addClass("has-error");
    })
}

Europa.Controllers.FechamentoContabil.EnviarNotificacao = function (fechamento) {
    fechamento.InicioFechamento = Europa.Date.toFormatDate(fechamento.InicioFechamento, Europa.Date.FORMAT_DATE);
    fechamento.TerminoFechamento = Europa.Date.toFormatDate(fechamento.TerminoFechamento, Europa.Date.FORMAT_DATE);
    $.post(Europa.Controllers.FechamentoContabil.UrlEnviarNotificacaoEmail, { fechamento }, function (res) {
        if (res.Sucesso) {
            Europa.Informacao.PosAcao(res);
            Europa.Informacao.Hide = function () {
                $(Europa.Informacao.Attr.Modal).modal("hide");
            } 
        }
    });
}

Europa.Controllers.FechamentoContabil.FecharModal = function () {
    $(Europa.Informacao.Attr.Modal).modal("hide");
}
