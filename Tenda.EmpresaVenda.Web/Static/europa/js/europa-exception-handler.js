$(document).ajaxError(function (e, xhr) {
    if (xhr.status == 403) {
        alert("Sua sessão não está  ativa. Você será redirecionado para a página de inicial no sistema!");
        window.location = '~/'
    }
    else if (xhr.status == 500) {
        var errorDetail = JSON.parse(xhr.responseText);

        var error = 'Ocorreu um erro ao realizar essa operação. <br />Caso o problema persista entre em contato com a TI Tenda.' +
            '<a id="mostrarDetalhes" href="#" style="font-size:small">Mostrar detalhes técnicos</a>' +
            '<div id="stacktrace" style="font-size:smaller">' +
            '<div class="col-md-24" style="font-weight: bold; font-style: italic;">' + errorDetail.Message + '</div>'+
            '<div class="col-md-24" style="overflow:auto;font-size: 12px;">' + errorDetail.StackTrace + '</div>'+
            '</div>';

        Europa.Informacao.ChangeHeaderAndContent("Erro Na Requisição", error);
        Europa.Informacao.Show();


        var divStackTrace = $('#stacktrace');
        var linkMostrar = $('#mostrarDetalhes');
        divStackTrace.hide();

        linkMostrar.click(function (e) {
            e.preventDefault();

            if (divStackTrace.css('display') == 'none') {
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
    console.log(msg);
    Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.AcessoNegado, msg);
    Europa.Informacao.Show();
};