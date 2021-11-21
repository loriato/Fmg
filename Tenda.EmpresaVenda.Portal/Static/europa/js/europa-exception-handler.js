$(document).ajaxError(function (e, xhr) {
    console.log(xhr);
    if (xhr.status === 403) { //forbidden
        const forbidden = JSON.parse(xhr.responseText);
        Europa.AccessDenied(forbidden.Messages);
    } else if (xhr.status === 401) { //acesso inválido
        const unauthorized = JSON.parse(xhr.responseText);
        alert(unauthorized.Messages);
        window.location.reload();
    } else if (xhr.status === 500) {
        let errorDetail = xhr.responseText;
        let resJSON = false;
        try {
            errorDetail = JSON.parse(xhr.responseText);
            resJSON = true;
        } catch (e) {
        }

        let error = 'Ocorreu um erro ao realizar essa operação. <br />Caso o problema persista entre em contato com a TI Tenda.' +
            '<a id="mostrarDetalhes" href="#" style="font-size:small">Mostrar detalhes técnicos</a>' +
            '<div id="stacktrace" style="font-size:smaller;word-break: break-word;">';

        if (resJSON) {
            error += '<div class="col-md-24" style="font-weight: bold; font-style: italic;">' + errorDetail.Messages + '</div>' +
                '<div class="col-md-24" style="overflow:auto;font-size: 12px;">' + errorDetail.Data + '</div>';
        } else {
            error += '<div class="col-md-24" style="overflow:auto;font-size: 12px;">' + errorDetail + '</div>';
        }

        error += '</div>';

        Europa.Informacao.ChangeHeaderAndContent("Erro na Requisição", error);
        Europa.Informacao.Show();


        const divStackTrace = $('#stacktrace');
        const linkMostrar = $('#mostrarDetalhes');
        divStackTrace.hide();

        linkMostrar.click(function (e) {
            e.preventDefault();

            if (divStackTrace.css('display') === 'none') {
                divStackTrace.show();
                linkMostrar.html('Ocultar detalhes técnicos');
            } else {
                divStackTrace.hide();
                linkMostrar.html('Mostrar detalhes técnicos');
            }

        });
    } else {
        console.log('Erro n tratado:');
        console.log(xhr);
    }
    Spinner.Hide();
});

Europa.AccessDenied = function (msg) {
    Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.AcessoNegado, msg);
    Europa.Informacao.Show();
};