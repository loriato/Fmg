Europa.Controllers.PreProposta = {};
Europa.Controllers.PreProposta.UrlRedirecionar = undefined;
Europa.Controllers.PreProposta.UrlBuscarPorCodigo = undefined;



Europa.Controllers.PreProposta.Buscar = function () {
    var codigo = $("#filtro_codigo").val();
    if (codigo === '' || codigo === undefined) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, Europa.i18n.Messages.MsgInformeCodigoPreProposta);
        Europa.Informacao.Show();
        return;
    }
    Europa.Controllers.PrePropostaWorkflow.Buscar(codigo);
}

// Salvar Pré Proposta
Europa.Controllers.PreProposta.Salvar = function () {
    //var response = Europa.Controllers.PrePropostaWorkflow.Salvar(idPreProposta);
};

// Enviar Pré Proposta
Europa.Controllers.PreProposta.Enviar = function () {
    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_AguardandoAnalise));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.Enviar(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

// Reenviar Pré Proposta
Europa.Controllers.PreProposta.Reenviar = function () {
    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_Retorno));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.Reenviar(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

// Revisar Pré Proposta
Europa.Controllers.PreProposta.Revisar = function () {
    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_EmAnalise));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.Revisar(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

// Analisar Pré Proposta
Europa.Controllers.PreProposta.Analisar = function () {
    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_EmAnalise));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.Analisar(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

// Aprovar Pré Proposta
Europa.Controllers.PreProposta.Aprovar = function () {
    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_Aprovada));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.Aprovar(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

// Pendenciar Pré Proposta
Europa.Controllers.PreProposta.Pendenciar = function () {
    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_DocsInsuficientes));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.Pendenciar(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

// Finalizar Pré Proposta
Europa.Controllers.PreProposta.Finalizar = function () {
    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_AguardandoIntegracao));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.Finalizar(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

// Cancelar Pré Proposta
Europa.Controllers.PreProposta.Cancelar = function () {
    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa,
        Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoAvancarPreProposta, Europa.i18n.Messages.SituacaoProposta_Cancelada));
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.Cancelar(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};

// Retroceder Pré Proposta
Europa.Controllers.PreProposta.Retroceder = function () {
    Europa.Informacao.Hide = function () {
        location.reload(true);
    }
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AvancarEtapa, Europa.i18n.Messages.MsgConfirmacaoRetrocederPreProposta);
    Europa.Confirmacao.ConfirmCallback = function () {
        var idPreProposta = $("#Id").val();
        var response = Europa.Controllers.PrePropostaWorkflow.Retroceder(idPreProposta);
        Europa.Informacao.PosAcao(response);
    }
    Europa.Confirmacao.Show();
};