Europa.Controllers.Conecta = {};
Europa.Controllers.Conecta.UrlKanban = "";

var frm = "";
$(function () {
});

$("#frame-conecta").ready(function () {

})

Europa.Controllers.Conecta.adequarLayout = function () {
    $('#iframe_conecta').contents().find('body').find('#logout').remove();
    $('#iframe_conecta').contents().find('body').find('#btn-back').remove();
    
}

Europa.Controllers.Conecta.AbrirDialogo = function () {
    $("#dialogo_conecta").modal('show')
    setTimeout(function () {
        $(".modal-backdrop").remove();
    }, 300);
    $("#dialogo_conecta").focus();


};


Europa.Controllers.Conecta.FecharDialogo = function () {
    $("#dialogo_conecta").modal('hide')
    setTimeout(function () { $(".modal-backdrop").remove() }, 300);
};

Europa.Controllers.Conecta.AbrirKanban = function () {
    
    $("#iframe_conecta").remove();

    $.get(Europa.Controllers.Conecta.BuscarUrlKanban, function (res) {
        console.log(res)
        if (res.Sucesso) {
            Europa.Controllers.Conecta.UrlKanban = res.Objeto;            
            $("#div-frame-conecta").append('<iframe id="iframe_conecta" onload="Europa.Controllers.Conecta.adequarLayout();" src="' + res.Objeto + '" frameborder="0" scrolling="yes" style="overflow:hidden;height:100%;width:100%"></iframe>')
            Europa.Controllers.Conecta.AbrirDialogo();
        } else {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, res.Mensagens.join("<br/>"));
            Europa.Informacao.Show();
        }
    });
}
