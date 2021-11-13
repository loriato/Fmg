Europa.Controllers.CadastroPerfil = {};

Europa.Controllers.CadastroPerfil.Tabela = {};
Europa.Controllers.CadastroPerfil.Form = {};
Europa.Controllers.CadastroPerfil.UrlListar;
Europa.Controllers.CadastroPerfil.UrlIncluir;
Europa.Controllers.CadastroPerfil.UrlAlterar;
Europa.Controllers.CadastroPerfil.UrlSuspender;
Europa.Controllers.CadastroPerfil.UrlReativar;
Europa.Controllers.CadastroPerfil.UrlCancelar;
Europa.Controllers.CadastroPerfil.Mensagem = {};
Europa.Controllers.CadastroPerfil.modoInclusao = undefined;

Europa.Controllers.CadastroPerfil.Novo = function () {
    Europa.Controllers.CadastroPerfil.Tabela.createRowNewData();
    Europa.Controllers.CadastroPerfil.modoInclusao = true;
}

Europa.Controllers.CadastroPerfil.PreSalvar = function () {
    var objetoLinhaTabela = Europa.Controllers.CadastroPerfil.Tabela.getDataRowEdit();
    var url = Europa.Controllers.CadastroPerfil.modoInclusao ? Europa.Controllers.CadastroPerfil.UrlIncluir : Europa.Controllers.CadastroPerfil.UrlAlterar;

    Europa.Controllers.CadastroPerfil.Form.CamposVazios(objetoLinhaTabela, function (camposVazios) {
        if (camposVazios.length > 0) {
            camposVazios.map(function (campo, i) {
                if (campo != 'ResponsavelCriacao') {
                    camposVazios[i] = Europa.String.Format(Europa.i18n.Messages.CampoObrigatorio, campo);
                }
            });

            if (!(camposVazios.length == 1 && camposVazios[0] == "ResponsavelCriacao")) {
                Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.SalvarDados, Europa.Controllers.CadastroPerfil.Mensagem.Formatar(camposVazios));
                Europa.Informacao.Show();
            } else {
                Europa.Controllers.CadastroPerfil.Salvar(objetoLinhaTabela, url);
            }
        } else {
            Europa.Controllers.CadastroPerfil.Salvar(objetoLinhaTabela, url);
        }
    });
}

Europa.Controllers.CadastroPerfil.Salvar = function (obj, url) {
    $.post(url, $.param(obj), function (res) {
        Europa.Controllers.CadastroPerfil.PosSalvar(res, Europa.i18n.Messages.SalvarDados);
    });
};

Europa.Controllers.CadastroPerfil.PosSalvar = function (res, titulo) {
    if (res.Sucesso) {
        Europa.Controllers.CadastroPerfil.Tabela.closeEdition();
        Europa.Controllers.CadastroPerfil.Tabela.reloadData();
        if (Europa.Controllers.CadastroPerfil.modoInclusao) {
            Europa.Informacao.ChangeHeaderAndContent(titulo, Europa.i18n.Messages.PerfilSalvo);
            Europa.Controllers.CadastroPerfil.modoInclusao = false;
        } else {
            Europa.Informacao.ChangeHeaderAndContent(titulo, Europa.i18n.Messages.PerfilAlterado);
        }

        Europa.Informacao.Show();
    } else {
        Europa.Informacao.ChangeHeaderAndContent(titulo, Europa.Controllers.CadastroPerfil.Mensagem.Formatar(res.Mensagens));
        Europa.Informacao.Show();
    }
}

Europa.Controllers.CadastroPerfil.RenderizaSituacao = function (value) {
    switch (value) {
        case 1:
            return Europa.i18n.Messages.Situacao_Ativo;
        case 2:
            return Europa.i18n.Messages.Situacao_Suspenso;
        case 3:
            return Europa.i18n.Messages.Situacao_Cancelado;
    }
};

Europa.Controllers.CadastroPerfil.RenderizaSituacaoMensagem = function (value) {
    switch (value) {
        case 1:
            return Europa.i18n.Messages.Ativar;
        case 2:
            return Europa.i18n.Messages.Suspender;
        case 3:
            return Europa.i18n.Messages.Cancelar;
    }
};

Europa.Controllers.CadastroPerfil.PreAlterarSituacao = function (url, situacao) {
    var objetoSelecionado = Europa.Controllers.CadastroPerfil.Tabela.getRowsSelect();

    if (objetoSelecionado != null || objetoSelecionado != undefined) {
        if (objetoSelecionado.Situacao == situacao) {
            Europa.Informacao.ChangeHeaderAndContent(
                Europa.i18n.Messages.Erro,
                Europa.String.Format(Europa.i18n.Messages.PerfilSituacaoInalterada,
                    objetoSelecionado.Nome,
                    Europa.Controllers.CadastroPerfil.RenderizaSituacao(objetoSelecionado.Situacao))
            );
            Europa.Informacao.Show();

            return;
        }
        Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.AlterarSituacao, Europa.String.Format(Europa.i18n.Messages.ConfirmacaoAlterarSituacao,
            Europa.Controllers.CadastroPerfil.RenderizaSituacaoMensagem(situacao)));
        Europa.Confirmacao.ConfirmCallback = function () {
            Europa.Controllers.CadastroPerfil.AlterarSituacao(url, objetoSelecionado.Id, situacao);
        }
        Europa.Confirmacao.Show();
    } else {
        Europa.Informacao.ChangeHeaderAndContent(
            Europa.i18n.Messages.Erro,
            Europa.i18n.Messages.NenhumRegistroSelecionando
        );
        Europa.Informacao.Show();
    }
}

Europa.Controllers.CadastroPerfil.AlterarSituacao = function (url, id, situacao) {
    $.post(url, { id: id, situacao: situacao }, function (res) {
        Europa.Controllers.CadastroPerfil.PosAlterarSituacao(res);
    }).fail(function () {
        console.log("Ocorreu algum problema.");
    });
}

Europa.Controllers.CadastroPerfil.PosAlterarSituacao = function (res) {
    Europa.Controllers.CadastroPerfil.Tabela.reloadData();

    if (res.Sucesso) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.AlterarSituacao, Europa.i18n.Messages.SituacaoPerfilAlterada);
    } else {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.AlterarSituacao, Europa.Controllers.CadastroPerfil.Mensagem.Formatar(res.Mensagens));
    }
    Europa.Informacao.Show();
};

Europa.Controllers.CadastroPerfil.Suspender = function () {
    Europa.Controllers.CadastroPerfil.PreAlterarSituacao(Europa.Controllers.CadastroPerfil.UrlSuspender, 2);
};

Europa.Controllers.CadastroPerfil.Ativar = function () {
    Europa.Controllers.CadastroPerfil.PreAlterarSituacao(Europa.Controllers.CadastroPerfil.UrlReativar, 1);
};

Europa.Controllers.CadastroPerfil.Cancelar = function () {
    Europa.Controllers.CadastroPerfil.PreAlterarSituacao(Europa.Controllers.CadastroPerfil.UrlCancelar, 3);
};

Europa.Controllers.CadastroPerfil.Form.CamposVazios = function (obj, callback) {
    var camposVazios = [];
    var existeCamposVazios = false;

    for (var key in obj) {
        if (obj.hasOwnProperty(key)) {
            var value = obj[key];
            if (value.length === 0 && key != "ResponsavelCriacao.Id" && key != "ResponsavelCriacao.Nome") {
                camposVazios.push(key);
                existeCamposVazios = true;
            }
        }
    }

    if (callback !== undefined) {
        callback(camposVazios);
    }

    return existeCamposVazios;
}

Europa.Controllers.CadastroPerfil.Mensagem.Formatar = function (msgs) {
    var mensagensHtml = $('<div/>');
    if ($.isArray(msgs) && msgs.length > 0) {
        msgs.map(function (x) {
            mensagensHtml.append($('<p/>').text(x));
        });
    } else {
        mensagensHtml.append($('<p/>').text(msgs));
    }
    return mensagensHtml;
}