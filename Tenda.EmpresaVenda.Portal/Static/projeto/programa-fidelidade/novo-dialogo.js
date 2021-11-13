//Diálogo de Informação
Europa.NovaInformacao = {};
Europa.NovaInformacao.Attr = {};
Europa.NovaInformacao.Attr.Modal = "#nova-info-alert";
Europa.NovaInformacao.Attr.Header = "#nova-info-header";
Europa.NovaInformacao.Attr.Body = "#nova-info-body";
Europa.NovaInformacao.ConfirmCallback = undefined;

Europa.NovaInformacao.Clear = function () {
    $(Europa.NovaInformacao.Attr.Header).html("Informação");
    $(Europa.NovaInformacao.Attr.Body).html("A DEFINIR");
};

Europa.NovaInformacao.ChangeHeader = function (value) {
    $(Europa.NovaInformacao.Attr.Header).html(value);
    if (value === null || value === undefined) {
        $(Europa.NovaInformacao.Attr.Header).css("display", "none");
    } else {
        $(Europa.NovaInformacao.Attr.Header).css("display", "block");
    }
};

Europa.NovaInformacao.ChangeContent = function (value) {    
    $(Europa.NovaInformacao.Attr.Body).html(value);
    if (value === null || value === undefined) {
        $(Europa.NovaInformacao.Attr.Body).css("display", "none");
    } else {
        $(Europa.NovaInformacao.Attr.Body).css("display", "block");
    }
};

Europa.NovaInformacao.ChangeHeaderAndContent = function (headerValue, contentValue) {    
    Europa.NovaInformacao.ChangeHeader(headerValue);
    Europa.NovaInformacao.ChangeContent(contentValue);
};

Europa.NovaInformacao.Show = function () {
    $(Europa.NovaInformacao.Attr.Header).css('display', 'block');
    $(Europa.NovaInformacao.Attr.Body).removeClass('sucesso');
    $(Europa.NovaInformacao.Attr.Modal).show();
};

Europa.NovaInformacao.ShowAsSuccess = function () {
    //$(Europa.NovaInformacao.Attr.Header).css('display', 'none');
    $(Europa.NovaInformacao.Attr.Body).addClass('sucesso');
    $(Europa.NovaInformacao.Attr.Modal).show();
};

Europa.NovaInformacao.Hide = function () {
    $(Europa.NovaInformacao.Attr.Modal).hide();
};

Europa.NovaInformacao.PosAcao = function (title, res) {    
    if (res && res.Mensagens && res.Mensagens.length > 0) {
        if (res.Sucesso) {
            Europa.NovaInformacao.ChangeHeaderAndContent(title, res.Mensagens.join("<br/>"));
            Europa.NovaInformacao.ShowAsSuccess();
        } else {
            Europa.NovaInformacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, res.Mensagens.join("<br/>"));
            Europa.NovaInformacao.Show();
        }
    }
}

//Diálogo Confirmação
Europa.NovaConfirmacao = {};
Europa.NovaConfirmacao.Attr = {};
Europa.NovaConfirmacao.Attr.Id = "#nova-confirm-alert";
Europa.NovaConfirmacao.Attr.Header = "#nova-confirm-header";
Europa.NovaConfirmacao.Attr.Body = "#nova-confirm-body";
Europa.NovaConfirmacao.Attr.PositiveButton = "#nova-confirm-positive-button";
Europa.NovaConfirmacao.ConfirmCallback = undefined;
Europa.NovaConfirmacao.CancelCallback = undefined;

Europa.NovaConfirmacao.Clear = function () {
    $(Europa.NovaConfirmacao.Attr.Header).html("Atenção");
    Europa.NovaConfirmacao.Attr.SuccessCallback = undefined;
    Europa.NovaConfirmacao.Attr.ErrorCallback = undefined;
};

Europa.NovaConfirmacao.ChangeHeader = function (value) {
    $(Europa.NovaConfirmacao.Attr.Header).html(value);
    if (value === null || value === undefined) {
        $(Europa.NovaConfirmacao.Attr.Header).css("display", "none");
    } else {
        $(Europa.NovaConfirmacao.Attr.Header).css("display", "block");
    }
};

Europa.NovaConfirmacao.ChangeContent = function (value) {
    $(Europa.NovaConfirmacao.Attr.Body).html(value);
    if (value === null || value === undefined) {
        $(Europa.NovaConfirmacao.Attr.Body).css("display", "none");
    } else {
        $(Europa.NovaConfirmacao.Attr.Body).css("display", "block");
    }
};

Europa.NovaConfirmacao.ChangeConfirmText = function (value) {
    $(Europa.NovaConfirmacao.Attr.PositiveButton).html(value);
    if (value === null || value === undefined) {
        $(Europa.NovaConfirmacao.Attr.PositiveButton).css("display", "none");
    } else {
        $(Europa.NovaConfirmacao.Attr.PositiveButton).css("display", "block");
    }
};

Europa.NovaConfirmacao.ChangeHeaderAndContent = function (headerValue, contentValue) {
    Europa.NovaConfirmacao.ChangeHeader(headerValue);
    Europa.NovaConfirmacao.ChangeContent(contentValue);
};

Europa.NovaConfirmacao.Confirm = function () {
    if (Europa.NovaConfirmacao.ConfirmCallback != undefined) {
        Europa.NovaConfirmacao.ConfirmCallback();
    }
    Europa.NovaConfirmacao.Hide();
};

//FIX-ME: Não validei esta função pois não está sendo utilizada
Europa.NovaConfirmacao.Cancel = function () {
    if (Europa.NovaConfirmacao.CancelCallback != undefined) {
        Europa.NovaConfirmacao.CancelCallback();
    }
    Europa.NovaConfirmacao.Hide();
};

Europa.NovaConfirmacao.Show = function () {
    $(Europa.NovaConfirmacao.Attr.Id).show();
};

Europa.NovaConfirmacao.Hide = function () {
    $(Europa.NovaConfirmacao.Attr.Id).hide();
};

Europa.NovaConfirmacao.PreAcao = function (titulo, mensagem, textoConfirmacao, callback) {
    Europa.NovaConfirmacao.ChangeHeader(titulo);
    Europa.NovaConfirmacao.ChangeContent(mensagem);
    Europa.NovaConfirmacao.ChangeConfirmText(textoConfirmacao);
    Europa.NovaConfirmacao.ConfirmCallback = callback;
    Europa.NovaConfirmacao.Show();
};