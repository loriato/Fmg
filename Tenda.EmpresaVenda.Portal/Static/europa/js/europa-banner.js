Europa.Controllers.BannerPortalEV = {};
Europa.Controllers.BannerPortalEV.Banners = [];

$(function () {    
    setTimeout(function () { Europa.Controllers.BannerPortalEV.ShowBanners() }, 100);
    setTimeout(function () { $("#slide-banner").carousel("pause");},100);
});

Europa.Controllers.BannerPortalEV.ShowBanners = function () {
    $.post(Europa.Controllers.BannerPortalEV.Url.ShowBanners, {}, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.BannerPortalEV.Banners = res.Objeto;
            res.Objeto.forEach(function (b, index) {
                //banner a ser exibido
                var banner = '<div class="item" id="' + index + '"><img src="' + b.Url + '" alt="teste" style="max-width:1300px;"></div>';

                if (b.Link) {
                    banner = '<div class="item" id="' + index + '"><a href="http://' + b.Link + '" target="_blank"><img src="' + b.Url + '" alt="teste" style="max-width:1300px;"></a></div>';
                }
                $("#banners").append(banner);

                $("#"+index).val(b.IdBanner)

            });
            $("#0").addClass("active")
            $("#modal-banner-portal-ev").show();
            $("#slide-banner").carousel("pause");

            $("#btn-aceite").removeClass("hidden");
            $("#btn-aceite").text("Fechar");

            if (res.Objeto[0].Tipo == 1) {
                $("#btn-aceite").text("Aceitar");
            }

            if (Europa.Controllers.BannerPortalEV.Banners[Europa.Controllers.BannerPortalEV.Banners.length - 1].IdBanner ==
                res.Objeto[0].IdBanner) {
                $("#btn-aceite").text("Fechar");
                if (res.Objeto[0].Tipo == 1) {
                    $("#btn-aceite").text("Aceitar");
                }
            }

            $("#Tipo").val(res.Objeto[0].Tipo);

        } else {
            $("#modal-banner-portal-ev").hide();
        }           
    });
}

Europa.Controllers.BannerPortalEV.VerificarBanner = function () {
    var idBanner = $("#banners div.item.active").val();

    $.post(Europa.Controllers.BannerPortalEV.Url.VerificarBanner, { idBanner: idBanner }, function (res) {
        if (res.Sucesso) { 
            $("#btn-aceite").text("Fechar");
            $("#btn-aceite").removeClass("hidden");

            var index = $("#banners div.item.active").attr("id");

            if (Europa.Controllers.BannerPortalEV.Banners[Europa.Controllers.BannerPortalEV.Banners.length - 1].IdBanner ==
                Europa.Controllers.BannerPortalEV.Banners[index].IdBanner) {
                Europa.Controllers.BannerPortalEV.FecharModal();
                return;
            } 

            index++;

            $(".proximo").click();

            var banner = Europa.Controllers.BannerPortalEV.Banners[index];

            if (banner.Tipo == 1) {
                $("#btn-aceite").text("Aceitar");
            }
            //console.log(banner)
            //if (Europa.Controllers.BannerPortalEV.Banners[Europa.Controllers.BannerPortalEV.Banners.length - 1].IdBanner ==
            //    banner.IdBanner) {
            //    $("#btn-aceite").text("Fechar");
               
            //    if (banner.Tipo == 1) {
            //        $("#btn-aceite").text("Aceitar");
            //    }                
            //}

        } else {
            Europa.NovaInformacao.PosAcao("Erro",res);
        }
    });
}

Europa.Controllers.BannerPortalEV.FecharModal = function () {
    $.post(Europa.Controllers.BannerPortalEV.Url.FecharBanner, {}, function (res) {
        if (res.Sucesso) {
            $("#modal-banner-portal-ev").hide();
        } else {
            console.log(res)
            Europa.NovaInformacao.PosAcao(Europa.i18n.Messages.Informacao, res);
        }
    });    
}
