//Spinner
Spinner = {Inner: {}};
Spinner.Show = function () {
    $("#modal-spinner").modal("show");
};
Spinner.Hide = function () {
    setTimeout(function () { $("#modal-spinner").modal("hide"); }, 500);
};

Spinner.Inner.Show = function (element) {
    let spinner = '<div class="d-flex justify-content-center loader m-3">' +
        '<div class="spinner-grow" role="status">' +
        '<span class="sr-only">' + Europa.i18n.Messages.Carregando + '</span>' +
        '</div>' +
        '<div class="spinner-grow" role="status">' +
        '</div>' +
        '<div class="spinner-grow" role="status">' +
        '</div>' +
        '</div>';
    $(element).html(spinner);
};

//Diálogo de Confirmação
Europa.Confirmacao = {};
Europa.Confirmacao.Attr = {};
Europa.Confirmacao.Attr.Id = "#modal-confirm-alert";
Europa.Confirmacao.Attr.Header = "#confirm-alert-header";
Europa.Confirmacao.Attr.Body = "#confirm-alert-body";
Europa.Confirmacao.ConfirmCallback = undefined;
Europa.Confirmacao.CancelCallback = undefined;

Europa.Confirmacao.Clear = function () {
    $(Europa.Confirmacao.Attr.Header).html(Europa.i18n.Messages.Atencao);
    Europa.Confirmacao.Attr.SuccessCallback = undefined;
    Europa.Confirmacao.Attr.ErrorCallback = undefined;
};
Europa.Confirmacao.ChangeHeader = function (value) {
    $(Europa.Confirmacao.Attr.Header).html(value);
};
Europa.Confirmacao.ChangeContent = function (value) {
    $(Europa.Confirmacao.Attr.Body).html(value);
};
Europa.Confirmacao.ChangeConfirmText = function (value) {
    $(Europa.Confirmacao.Attr.PositiveButton).html(value);
    if (value === null || value === undefined) {
        $(Europa.Confirmacao.Attr.PositiveButton).css("display", "none");
    } else {
        $(Europa.Confirmacao.Attr.PositiveButton).css("display", "block");
    }
};
Europa.Confirmacao.ChangeHeaderAndContent = function (headerValue, contentValue) {
    $(Europa.Confirmacao.Attr.Header).html(headerValue);
    $(Europa.Confirmacao.Attr.Body).html(contentValue);
};
Europa.Confirmacao.Confirm = function () {
    if (Europa.Confirmacao.ConfirmCallback !== undefined) {
        Europa.Confirmacao.ConfirmCallback();
    }
    Europa.Confirmacao.Hide();
};
Europa.Confirmacao.Cancel = function () {
    if (Europa.Confirmacao.CancelCallback !== undefined) {
        Europa.Confirmacao.CancelCallback();
    }
    Europa.Confirmacao.Hide();
};
Europa.Confirmacao.Show = function () {
    $(Europa.Confirmacao.Attr.Id).modal("show");
};
Europa.Confirmacao.Hide = function () {
    $(Europa.Confirmacao.Attr.Id).modal("hide");
};

//Diálogo de Informação
Europa.Informacao = {};
Europa.Informacao.Attr = {};
Europa.Informacao.Attr.Modal = "#modal-info-alert";
Europa.Informacao.Attr.Header = "#info-alert-header";
Europa.Informacao.Attr.Body = "#info-alert-body";
Europa.Informacao.ConfirmCallback = undefined;

Europa.Informacao.Clear = function () {
    $(Europa.Informacao.Attr.Header).html(Europa.i18n.Messages.Informacao);
    $(Europa.Informacao.Attr.Body).html("A DEFINIR");
};
Europa.Informacao.ChangeHeader = function (value) {
    $(Europa.Informacao.Attr.Header).html(value);
};
Europa.Informacao.ChangeContent = function (value) {
    $(Europa.Informacao.Attr.Body).html(value);
};
Europa.Informacao.ChangeHeaderAndContent = function (headerValue, contentValue) {
    $(Europa.Informacao.Attr.Header).html(headerValue);
    $(Europa.Informacao.Attr.Body).html(contentValue);
};
Europa.Informacao.Show = function () {
    $(Europa.Informacao.Attr.Modal).modal("show");
};
Europa.Informacao.Hide = function () {
    $(Europa.Informacao.Attr.Modal).modal("hide");
};

Europa.Confirmacao.PreAcao = function (acao, chavecandidata, callback) {
    Europa.Confirmacao.ChangeConfirmText(Europa.i18n.Messages.Confirmar);
    Europa.Confirmacao.ChangeHeader(Europa.i18n.Messages.Confirmacao);
    Europa.Confirmacao.ChangeContent(Europa.String.Format(Europa.i18n.Messages.ConfirmacaoAcaoRegistro, acao.toLowerCase(), chavecandidata));
    Europa.Confirmacao.ConfirmCallback = callback;
    Europa.Confirmacao.Show();
};

Europa.Confirmacao.PreAcaoV2 = function (titulo, mensagem, textoConfirmacao, callback) {
    Europa.Confirmacao.ChangeHeader(titulo);
    Europa.Confirmacao.ChangeContent(mensagem);
    Europa.Confirmacao.ChangeConfirmText(textoConfirmacao);
    Europa.Confirmacao.ConfirmCallback = callback;
    Europa.Confirmacao.Show();
};

Europa.Confirmacao.PreAcaoMulti = function (acao, callback) {
    Europa.Confirmacao.ChangeConfirmText(Europa.i18n.Messages.Confirmar);
    Europa.Confirmacao.ChangeHeader(Europa.i18n.Messages.Confirmacao);
    Europa.Confirmacao.ChangeContent(Europa.String.Format(Europa.i18n.Messages.ConfirmacaoAlterarSituacao, acao.toLowerCase()));
    Europa.Confirmacao.ConfirmCallback = callback;
    Europa.Confirmacao.Show();
};

Europa.Informacao.PosAcao = function (res) {
    if (res && res.Mensagens && res.Mensagens.length > 0) {
        if (res.Sucesso) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Sucesso, res.Mensagens.join("<br/>"));
            Europa.Informacao.Show();
        }
        else {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, res.Mensagens.join("<br/>"));
            Europa.Informacao.Show();
        }
    }
};

Europa.Informacao.PosAcaoBaseResponse = function (res) {
    if (res.Success) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Sucesso, res.Messages.join("<br/>"));
        Europa.Informacao.Show();
    }
    else {
        if (res.Messages.length > 0) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, res.Messages.join("<br/>"));
        } else {
            var msg = [];

            res.Fields.forEach(function (f) {
                msg.push(f.Value)
            })

            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, msg.join("</br>"));
        }

        Europa.Informacao.Show();
    }
};

