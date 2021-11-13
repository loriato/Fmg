Europa.Controllers.PrePropostaWorkflow = {};
Europa.Controllers.PrePropostaWorkflow.UrlRedirecionar = undefined;
Europa.Controllers.PrePropostaWorkflow.UrlBuscarPorCodigo = undefined;
Europa.Controllers.PrePropostaWorkflow.UrlEnviarAuditoria = undefined;
Europa.Controllers.PrePropostaWorkflow.UrlRetornoAuditoria = undefined;

$(document).ready(function () {
    Europa.Controllers.PrePropostaWorkflow.Filtrar(); $("#TelefoneCorretor").mask("(00) 00000-0000");
    $("#titlebar-name-target").addClass("col-md-8");
});

Europa.Controllers.PrePropostaWorkflow.Buscar = function (codigo) {
    $.ajax({
        type: "GET",
        url: Europa.Controllers.PrePropostaWorkflow.UrlBuscarPorCodigo,
        async: false,
        data: {
            codigo: codigo
        }
    }).done(function (response) {
        if (response && response.Sucesso) {
            var codigoPreProposta = response.Objeto;
            var basePath = Europa.Controllers.PrePropostaWorkflow.UrlRedirecionar.split("/Index")[0];
            if (codigoPreProposta == undefined || codigoPreProposta == null)
                location.href = basePath;

            location.href = basePath + "/Index/" + codigoPreProposta;
        } else if (response.Sucesso === false) {
            Europa.Informacao.PosAcao(response);
        }
    });
};

Europa.Controllers.PrePropostaWorkflow.SalvarPreProposta = function () {
    var id = $("#IdPreProposta").val();
    var obs = $("#Observacao").val();
    $.post(Europa.Controllers.PrePropostaWorkflow.UrlSalvar, { idPreProposta: id, observacao : obs }, function (res) {
        Europa.Informacao.PosAcao(res);
    });
};

// Salvar Pré Proposta
Europa.Controllers.PrePropostaWorkflow.Salvar = function (idPreProposta) {
    var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlSalvar);
    return result;
};

// Enviar Pré Proposta
Europa.Controllers.PrePropostaWorkflow.Enviar = function (idPreProposta) {
    var result =  Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlEnviar);
    return result;
};

// Reenviar Pré Proposta
Europa.Controllers.PrePropostaWorkflow.Reenviar = function (idPreProposta) {
    var result =  Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlReenviar);
    return result;
};

// Revisar Pré Proposta
Europa.Controllers.PrePropostaWorkflow.Revisar = function (idPreProposta) {
    var result =  Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlRevisar);
    return result;
};

// Analisar Pré Proposta
Europa.Controllers.PrePropostaWorkflow.Analisar = function (idPreProposta) {
    var result =  Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlAnalisar);
    return result;
};

// Aprovar Pré Proposta
Europa.Controllers.PrePropostaWorkflow.Aprovar = function (idPreProposta) {
    var result =  Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlAprovar);
    return result;
};

// Pendenciar Pré Proposta
Europa.Controllers.PrePropostaWorkflow.Pendenciar = function (idPreProposta) {
    var result =  Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlPendenciar);
    return result;
};

// DocsInsuficientesSimplificado
Europa.Controllers.PrePropostaWorkflow.DocsInsuficientesSimplificado = function (idPreProposta) {
    var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlDocsInsuficientesSimplificado);
    return result;
};

// DocsInsuficientesCompleta
Europa.Controllers.PrePropostaWorkflow.DocsInsuficientesCompleta = function (idPreProposta) {
    var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlDocsInsuficientesCompleta);
    return result;
};

// Finalizar Pré Proposta
Europa.Controllers.PrePropostaWorkflow.Finalizar = function (idPreProposta) {
    var result =  Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlFinalizar);
    return result;
};

// AguardandoAnaliseCompleta
Europa.Controllers.PrePropostaWorkflow.AguardandoAnaliseCompleta = function (idPreProposta) {
    var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlAguardandoAnaliseCompleta);
    return result;
}

//Em Análise Completa
Europa.Controllers.PrePropostaWorkflow.EmAnaliseCompleta = function (idPreProposta) {
    var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlEmAnaliseCompleta);
    return result;
}

//Aguardando Auditoria
Europa.Controllers.PrePropostaWorkflow.AguardandoAuditoria = function (idPreProposta) {
    var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlAguardandoAuditoria);
    return result;
}

//FluxoEnviado
Europa.Controllers.PrePropostaWorkflow.FluxoEnviado = function (idPreProposta) {
    var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlFluxoEnviado);
    return result;
}

//RetornoAuditoria
Europa.Controllers.PrePropostaWorkflow.RetornoAuditoria = function (idPreProposta) {
    var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlRetornoAuditoria);
    return result;
}

//Docsinsuficientes
Europa.Controllers.PrePropostaWorkflow.DocsInsuficientes = function (idPreProposta) {
    var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlDocsInsuficientes);
    return result;
}

// Cancelar Pré Proposta
Europa.Controllers.PrePropostaWorkflow.Cancelar = function (idPreProposta) {
    var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlCancelar);
    return result;
};

// Retroceder Pré Proposta
Europa.Controllers.PrePropostaWorkflow.Retroceder = function (idPreProposta) {
    var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlRetroceder);
    return result;
};

//Aguardar Fluxo
Europa.Controllers.PrePropostaWorkflow.AguardarFluxo = function (idPreProposta) {
    var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlAguardarFluxo);
    return result;
};

//Aguardando Integração
Europa.Controllers.PrePropostaWorkflow.AguardandoIntegracao = function (idPreProposta) {
    var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlAguardandoIntegracao);
    return result;
}

//Análise Completa Aprovada
Europa.Controllers.PrePropostaWorkflow.AnaliseCompletaAprovada = function (idPreProposta) {
    var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlAnaliseCompletaAprovada);
    return result;
}

//Reenvio para Análise
Europa.Controllers.PrePropostaWorkflow.ReenviarAnaliseCompletaAprovada = function (data) {
    var result = undefined;
    var url = Europa.Controllers.PrePropostaWorkflow.UrlReenviarAnaliseCompletaAprovada;
    $.ajax({
        type: "POST",
        url: url,
        async: false,
        data: data
    }).done(function (response) {
        result = response;
    });
    return result;
}
//Analise Simplificada Aprovada
Europa.Controllers.PrePropostaWorkflow.AnaliseSimplificadaAprovada = function (idPreProposta) {
    var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlAnaliseSimplificadaAprovada);
    return result;
}

Europa.Controllers.PrePropostaWorkflow.DesassociarUnidade = function (idPreProposta) {
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.i18n.Messages.ConfirmaDesassociacaoUnidade);
    Europa.Confirmacao.ConfirmCallback = function () {
        var result = Europa.Controllers.PrePropostaWorkflow.RequestAcao(idPreProposta, Europa.Controllers.PrePropostaWorkflow.UrlDesassociarUnidade);
        Europa.Informacao.PosAcao(result)
        if (result.Sucesso) {
            $("#botao_desassociar_unidade").hide();
        }
    }
    Europa.Confirmacao.Show();
};

// Função que faz a requisição para o controller
Europa.Controllers.PrePropostaWorkflow.RequestAcao = function (idPreProposta, url) {
    var result = undefined;
    $.ajax({
        type: "POST",
        url: url,
        async: false,
        data: {
            idPreProposta: idPreProposta
        }
    }).done(function (response) {
        result = response;
    });
    return result;
};


DataTableApp.controller('historicoPrePropostaTable', historicoPrePropostaTable);

function historicoPrePropostaTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.PrePropostaWorkflow.Table = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.PrePropostaWorkflow.Table;
    self.setColumns([
        DTColumnBuilder.newColumn('CodigoPreProposta').withTitle(Europa.i18n.Messages.Codigo).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeResponsavelInicio').withTitle(Europa.i18n.Messages.Responsavel).withOption('width', '150px'),
        DTColumnBuilder.newColumn('SituacaoInicio').withTitle(Europa.i18n.Messages.Situacao).withOption('width', '150px').withOption('type', 'enum-format-SituacaoProposta'),
        DTColumnBuilder.newColumn('Inicio').withTitle(Europa.i18n.Messages.Inicio).withOption('width', '150px').withOption("type", "date-format-DD/MM/YYYY HH:mm:ss"),
        DTColumnBuilder.newColumn('NomeResponsavelTermino').withTitle(Europa.i18n.Messages.Responsavel).withOption('width', '150px'),
        DTColumnBuilder.newColumn('SituacaoTermino').withTitle(Europa.i18n.Messages.Situacao).withOption('width', '150px').withOption('type', 'enum-format-SituacaoProposta'),
        DTColumnBuilder.newColumn('Termino').withTitle(Europa.i18n.Messages.Termino).withOption('width', '150px').withOption("type", "date-format-DD/MM/YYYY HH:mm:ss"),
        DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('width', '150px').withOption('type', 'enum-format-Situacao')
    ])
        .setIdAreaHeader("historico_pre_proposta_datatable_header")
        .setDefaultOrder([[3, 'desc']])
        .setAutoInit(true)
        .setDefaultOptions('POST', Europa.Controllers.PrePropostaWorkflow.UrlHistoricos, Europa.Controllers.PrePropostaWorkflow.FilterParams);
};

Europa.Controllers.PrePropostaWorkflow.FilterParams = function () {
    return {
        idPreProposta: $('#Id').val()
    };
};

Europa.Controllers.PrePropostaWorkflow.Filtrar = function () {
    // O JS está sendo utilizado em outros locais, só que tais locais não possuem a referencia a essa tabela
    if (Europa.Controllers.PrePropostaWorkflow.Table == undefined) { return; }
    Europa.Controllers.PrePropostaWorkflow.Table.reloadData();
};

Europa.Controllers.PrePropostaWorkflow.LimparFiltro = function () {
};

Europa.Controllers.PrePropostaWorkflow.AnaliseAuditoria = function (opt) {

    switch (opt) {
        case 1:
            Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoEnviarAuditoria));
            break;
        case 2:
            Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoRetornoAuditoria));
            break;
        default:
            Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.MsgErrorAuditoria));
    };

    var id = $("#IdPreProposta").val();

    Europa.Confirmacao.ConfirmCallback = function () {
        $.post(Europa.Controllers.PrePropostaWorkflow.UrlAnaliseAuditoria, { idPreProposta: id, botao: opt }, function (res) {
            Europa.Informacao.PosAcao(res);
        });
    };

    Europa.Confirmacao.Show();

    $('#info-alert').click(function () { window.location.reload() });
};