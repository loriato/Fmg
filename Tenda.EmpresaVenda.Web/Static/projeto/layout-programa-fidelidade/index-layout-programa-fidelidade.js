Europa.Controllers.LayoutProgramaFidelidade = {};
Europa.Controllers.LayoutProgramaFidelidade.Id = 0;
Europa.Controllers.LayoutProgramaFidelidade.Form = "#form-layout";

$(function () {
    Europa.Controllers.LayoutProgramaFidelidade.Id = $("#IdLayoutProgramaFidelidade").val();

    Europa.Controllers.LayoutProgramaFidelidade.Init();
});

Europa.Controllers.LayoutProgramaFidelidade.Init = function () {
    if (Europa.Controllers.LayoutProgramaFidelidade.Id == 0) {
        $("#btn_novo").removeClass("hidden");
    } else {
        $("#btn_editar").removeClass("hidden");
    }
};

Europa.Controllers.LayoutProgramaFidelidade.Editar = function () {
    $("#btn_novo").addClass("hidden");
    $("#btn_editar").addClass("hidden");

    $("#btn_salvar").removeClass("hidden");
    $("#btn_cancelar").removeClass("hidden");

    $("#fieldset-form-layout", Europa.Controllers.LayoutProgramaFidelidade.Form).removeAttr("disabled");
};

Europa.Controllers.LayoutProgramaFidelidade.Novo = function () {
    $("#btn_novo").addClass("hidden");
    $("#btn_editar").addClass("hidden");

    $("#btn_salvar").removeClass("hidden");
    $("#btn_cancelar").removeClass("hidden");

    $("#IdLayoutProgramaFidelidade").val(0);
    $("#LinkParceiroExclusivo").val("");
    $("#NomeParceiroExclusivo").val("");

    $("#LinkBannerParceiroExclusivo").val("");
    $("#LinkBannerPontos").val("");

    $("#fieldset-form-layout", Europa.Controllers.LayoutProgramaFidelidade.Form).removeAttr("disabled");
};

Europa.Controllers.LayoutProgramaFidelidade.Cancelar = function () {
    location.reload();
};

Europa.Controllers.LayoutProgramaFidelidade.SalvarLayout = function () {
    var layout = {
        IdLayoutProgramaFidelidade: $("#IdLayoutProgramaFidelidade").val(),
        FileBannerParceiroExclusivo: $("#FileBannerParceiroExclusivo")[0].files[0],
        FileBannerPontos: $("#FileBannerPontos")[0].files[0],
        LinkParceiroExclusivo: $("#LinkParceiroExclusivo").val(),
        NomeParceiroExclusivo: $("#NomeParceiroExclusivo").val()
    };

    $('#form-layout').ajaxSubmit({
        type: 'POST',
        url: Europa.Controllers.LayoutProgramaFidelidade.Url.SalvarLayout,
        cache: false,
        success: function (res, status, xhr) {
            if (res.Sucesso) {
                Europa.Informacao.Hide = function () {
                    location.reload(true);
                }
            }

            Europa.Informacao.PosAcao(res);
        },
        error: function (res, status, xhr) {
            Europa.Informacao.PosAcao(res)
        }
    });
};

Europa.Controllers.LayoutProgramaFidelidade.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.LayoutProgramaFidelidade.VizualizarParceiro = function () {    

    var file = $("#FileBannerParceiroExclusivo")[0].files[0];
    var link = $("#LinkBannerParceiroExclusivo").val();

    if (file == undefined && link == "") {
        var res = {
            Sucesso: false,
            Mensagens:["Não há imagem disponível"]
        }

        Europa.Informacao.PosAcao(res);

        return;
    }

    if (file != undefined) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#banner').attr('src', e.target.result);
        }

        reader.readAsDataURL(file);
    } else {        
        $("#banner").prop("src", link);
    }   
    
    Europa.Controllers.LayoutProgramaFidelidade.AbrirModal();
};

Europa.Controllers.LayoutProgramaFidelidade.VizualizarPontos = function () {
    
    var file = $("#FileBannerPontos")[0].files[0];
    var link = $("#LinkBannerPontos").val();

    if (file == undefined && link == "") {
        var res = {
            Sucesso: false,
            Mensagens: ["Não há imagem disponível"]
        }

        Europa.Informacao.PosAcao(res);

        return;
    }

    if (file != undefined) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#banner').attr('src', e.target.result);
        }

        reader.readAsDataURL(file);
    } else {        
        $("#banner").prop("src", link);
    }  
    
    Europa.Controllers.LayoutProgramaFidelidade.AbrirModal();
};
