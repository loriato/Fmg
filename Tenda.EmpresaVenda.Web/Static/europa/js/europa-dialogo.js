'use strict'

//Spinner
var Spinner = {};
Spinner.Show = function () {
    $("#spinner").modal("show");
};
Spinner.Hide = function () {
    $("#spinner").modal("hide");
};

//Diálogo de Informação
Europa.Confirmacao = {};
Europa.Confirmacao.Attr = {};
Europa.Confirmacao.Attr.Id = "#confirm-alert";
Europa.Confirmacao.Attr.Header = "#confirm-alert-header";
Europa.Confirmacao.Attr.Body = "#confirm-alert-body";
Europa.Confirmacao.ConfirmCallback = undefined;
Europa.Confirmacao.CancelCallback = undefined;

Europa.Confirmacao.Clear = function () {
    $(Europa.Confirmacao.Attr.Header).html("Atenção");
    Europa.Confirmacao.Attr.SuccessCallback = undefined;
    Europa.Confirmacao.Attr.ErrorCallback = undefined;
};
Europa.Confirmacao.ChangeHeader = function (value) {
    $(Europa.Confirmacao.Attr.Header).html(value);
};
Europa.Confirmacao.ChangeContent = function (value) {
    $(Europa.Confirmacao.Attr.Body).html(value);
};
Europa.Confirmacao.ChangeHeaderAndContent = function (headerValue, contentValue) {
    $(Europa.Confirmacao.Attr.Header).html(headerValue);
    $(Europa.Confirmacao.Attr.Body).html(contentValue);
};
Europa.Confirmacao.Confirm = function () {
    if (Europa.Confirmacao.ConfirmCallback != undefined) {
        Europa.Confirmacao.ConfirmCallback();
    }
    Europa.Confirmacao.Hide();
};
Europa.Confirmacao.Cancel = function () {
    if (Europa.Confirmacao.CancelCallback != undefined) {
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
Europa.Informacao.Attr.Modal = "#info-alert";
Europa.Informacao.Attr.Header = "#info-alert-header";
Europa.Informacao.Attr.Body = "#info-alert-body";
Europa.Informacao.ConfirmCallback = undefined;

Europa.Informacao.Clear = function () {
    $(Europa.Informacao.Attr.Header).html("Informação");
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

//teste

//TODO voltar na tela de msg

Europa.Confirmacao.PreAcao = function (acao, chavecandidata, callback) {
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
    if (mensagem.length < 1) Europa.Confirmacao.Confirm();
};

Europa.Confirmacao.ChangeConfirmText = function (value) {
    $(Europa.Confirmacao.Attr.PositiveButton).html(value);
    if (value === null || value === undefined) {
        $(Europa.Confirmacao.Attr.PositiveButton).css("display", "none");
    } else {
        $(Europa.Confirmacao.Attr.PositiveButton).css("display", "block");
    }
};

Europa.Confirmacao.PreAcaoMulti = function (acao, callback) {
    Europa.Confirmacao.ChangeHeader(Europa.i18n.Messages.Confirmacao);
    Europa.Confirmacao.ChangeContent(Europa.String.Format(Europa.i18n.Messages.ConfirmacaoAlterarSituacao, acao.toLowerCase()));
    Europa.Confirmacao.ConfirmCallback = callback;
    Europa.Confirmacao.Show();
};

Europa.Informacao.PosAcao = function (res) {
    if (res && res.Mensagens && res.Mensagens.length > 0) {
        if (res.Sucesso) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Sucesso, res.Mensagens.join("<br/>"));
        }
        else {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, res.Mensagens.join("<br/>"));
        }
        Europa.Informacao.Show();
    }
};


//fim teste



/*
Diálogo de Sucesso (Estilo Growl)
** EXEMPLO DE USO **
var sucesso = new Europa.Notification()
                        .Content("O registro X foi adicionado com sucesso.")
                        .WithIcon('fa fa-check')
                        .WithDismissDelay(false)
                        .Show();
*/
Europa.Growl = {};
Europa.Growl.Success = function (message) {
    new Europa.Notification()
        .Success(message);
};
Europa.Growl.SuccessFromJsonResponse = function (jsonResponse) {
    if (jsonResponse && jsonResponse.Mensagens && jsonResponse.Mensagens.length > 0) {
        new Europa.Notification()
            .Success(jsonResponse.Mensagens.join("<br/>"));
    }
};

Europa.Notification = function (message) {
    if (message != undefined) {
        this.Body = message;
    } else {
        this.Body = "Default Message";
    }
    this.Delay = 10000;
    this.ShowDismissArea = true;
    this.OffsetX = 5;
    this.OffsetY = 50;
    this.Icon = 'fa fa-exclamation-triangle';
    this.Target = '_blank';
    this.Type = "info";
    return this;
};

Europa.Notification.prototype.Success = function (message) {
    return this.SetTypeAttributesAndShow('fa fa-check', 'success', message);
};

Europa.Notification.prototype.Fail = function (message) {
    return this.SetTypeAttributesAndShow('fa fa-times', 'danger', message);
};

Europa.Notification.prototype.Warning = function (message) {
    return this.SetTypeAttributesAndShow('fa fa-warn', 'warning', message);
};

Europa.Notification.prototype.SetTypeAttributesAndShow = function (icon, type, message) {
    this.SetTypeAttributes(icon, type);
    return this.Show(message);
};

Europa.Notification.prototype.SetTypeAttributes = function (icon, type) {
    this.Icon = icon;
    this.Type = type;
};

Europa.Notification.prototype.WithType = function (type) {
    this.Type = type;
};

Europa.Notification.prototype.WithDismissDelay = function (value) {
    this.Delay = value;
    return this;
};

Europa.Notification.prototype.WithDismiss = function (value) {
    this.ShowDismissArea = value;
    return this;
};

Europa.Notification.prototype.Offset = function (x, y) {
    this.OffsetX = x;
    this.OffsetY = y;
    return this;
};

Europa.Notification.prototype.OffsetX = function (x) {
    this.OffsetX = x;
    return this;
};

Europa.Notification.prototype.OffsetY = function (y) {
    this.OffsetY = y;
    return this;
};

Europa.Notification.prototype.Content = function (value) {
    this.Body = value;
    return this;
};

Europa.Notification.prototype.WithIcon = function (icon) {
    this.Icon = icon;
    return this;
};

Europa.Notification.prototype.Show = function (message) {
    if (message !== undefined) {
        this.Body = message;
    }

    var template = '<div data-notify="container" class="col-md-6 alert alert-{0}" role="alert" style="padding: 12px;">' +
        '<button type="button" aria-hidden="true" class="close" data-notify="dismiss" style="margin-top:-5px;">×</button> ' +
        '<div>' +
        '<span data-notify="icon">&nbsp</span><span data-notify="message">{2}</span> ' +
        '</div>';

    if (this.ShowDismissArea === true || this.ShowDismissArea === 1) {
        template += '<div class="col-md-24" style="height:20px;margin-top: 5px;">' +
            '<div class="col-md-9" style="padding-top: 3px"> ' +
            'Fechando em <span id="europa_notification"> </span>s' +
            '</div>' +
            '<div class="col-md-15"> ' +
            '<div class="progress" data-notify="progressbar" style="margin-top: 1px; height: 15px;">' +
            '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;">' +
            '</div> ' +
            '</div>' +
            '</div>';
    }

    template += '<div>' +
        '<a href="{3}" target="{4}" data-notify="url"></a> ' +
        '</div>' +
        '</div>';


    this.notification = $.notify({
        // options
        icon: this.Icon,
        message: this.Body,
        target: this.Target
    }, {
            // settings
            element: 'body',
            type: this.Type,
            allow_dismiss: true,
            newest_on_top: false,
            showProgressbar: true,
            placement: {
                from: "top",
                align: "right"
            },
            offset: { x: this.OffsetX, y: this.OffsetY },
            spacing: 10,
            z_index: 10051,
            delay: this.Delay,
            timer: 10,
            url_target: '_blank',
            animate: {
                enter: 'animated fadeInDown',
                exit: 'animated fadeOutUp'
            },
            icon_type: 'class',
            template: template
        });
    this.notification.update({ onShow: Europa.Notification.Countdown(this.notification.$ele.find("#europa_notification"), this.Delay) });
    return this;
};

Europa.Notification.Countdown = function (elem, countdown) {
    if (countdown >= 0) {
        elem.html(countdown / 1000);
        setTimeout(function () { Europa.Notification.Countdown(elem, countdown - 1000); }, 1000);
    }
};

Europa.Informacao.PosAcaoApi = function (res) {
    if (res && res.Fields && res.Fields.length > 0) {
        res.Fields.forEach(function (x) { res.Messages.push(x.Value) })
    }

    if (res && res.Messages && res.Messages.length > 0) {
        if (res.Success) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Sucesso, res.Messages.join("<br/>"));
        }
        else {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, res.Messages.join("<br/>"));
        }
        Europa.Informacao.Show();
    }
};
