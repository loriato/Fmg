// TODO MUDAR PARA NAMESPACE EUROPA

Menu = {};

function hoverTrocarImagem(){
    $('.bg-img').on('mouseover', function(){
        $('#trocar_imagem').show();
    }).on('mouseout', function(e){
        if($(e.relatedTarget).attr('id') !== 'trocar_imagem'){
            $('#trocar_imagem').hide();
        }
    });

    $('#trocar_imagem').on('mouseout', function(e){
        if(!$(e.relatedTarget).hasClass('.bg-img')){
            $('#trocar_imagem').hide();
        }
    });

    $('#trocar_imagem').click(function(){
        $('#trocar_imagem').hide();
    });
}

function abirMenu(){
    var menu = $('#nav_bar').find('.menu-expander');
    menu.animate({ left: 0 });
}

function fecharMenu(){
    var menu = $('#nav_bar').find('.menu-expander');
    menu.animate({ left: '-296px' });
}


function defineMenuActive(li){
    $('#nav_bar').find('li.active').each(function(idx, val){
        removeMenuActive($(val));
    });

    li.each(function(idx, val){
        addActive($(this));
    });
}

function removeMenuActive(li){
    li.prev().removeClass('bottom');
    li.removeClass('active');
    li.next().removeClass('top');
}

function addActive(li){
    li.prev().addClass('bottom');
    li.addClass('active');
    li.next().addClass('top');
}

function isItemMenuRootPath(li){
    return li.find('a').attr('href') === window.location.href;
}

function configureMenu() {
    $('#nav_bar .expander .icon').click(function(){
        abirMenu();
    });

    $(document).click(function(e){
        var fechar = true;
        $(e.target).parents().each(function(idx, val){
            if($(val).hasClass('expander')){
                fechar = false;
            }
        });

        if(fechar){
            fecharMenu();
        }
    });

    var selfMenu = $('.menu-expander');
    selfMenu.find('ul a').each(function (idx, val) {
        var pageActive;
        $(this).on('mouseover', function () {
            pageActive = selfMenu.find('li.active');

            var li = $(this).parent();
            defineMenuActive(li);

        }).on('mouseout', function () {
            var li = $(this);
            if(isItemMenuRootPath(li) === false) {
                removeMenuActive(li);
            }
            if(pageActive){
                defineMenuActive(pageActive);
            }
        });
    });

    selfMenu.find('li').each(function(idx, val){
        if(isItemMenuRootPath($(this))){
            addActive($(this));
            
            if($(this).parent().hasClass('sub-menu')){
                addActive($(this).parent().parent());
            }
        }
    });
}


Menu.IdFormularioUploadFoto = "#form_upload_foto";

Menu.UploadFoto = function () {
    var arquivo = $("#trocar_imagem_input", Menu.IdFormularioUploadFoto).get(0).files[0];

    if (arquivo !== undefined && arquivo.size > 4000000) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.ArquivoTamanhoMaximoExcedido, "4"));
        Europa.Informacao.Show();
        return;
    }

    var formData = new FormData();
    formData.append("IdEmpresaVenda", Menu.IdEmpresaVenda);
    formData.append("Foto", arquivo);

    $.ajax({
        type: 'POST',
        url: Menu.UrlUploadFoto,
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
        success: function (res) {
            if (res.Sucesso) {
                $("#foto_fachada").attr('src', res.Objeto)
                Europa.Growl.SuccessFromJsonResponse(res);
            } else {
                Europa.Informacao.PosAcao(res)
            }

        }
    });
};

Menu.VerificarNovasNotificacoes = function () {
    $.post(Menu.UrlVerificarNovasNotificacoes, {}, function (res) { });
};

$(document).ready(function(){
    hoverTrocarImagem();
    configureMenu();
    Menu.VerificarNovasNotificacoes();
});