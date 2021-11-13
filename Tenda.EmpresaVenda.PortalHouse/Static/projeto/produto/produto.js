Europa.Controllers.Produto = {};
Europa.Controllers.Produto.Url = {};
Europa.Controllers.Produto.Selector = {};
Europa.Controllers.Produto.Template = {};
Europa.Controllers.Produto.Selector.Spinner = "#book-empreendimento-spinner";
Europa.Controllers.Produto.Selector.ListWrapper = "#book-empreendimento-target";
Europa.Controllers.Produto.Selector.ModalDetalheWrapper = "#div_breve_lancamento_info";
Europa.Controllers.Produto.Selector.ModalDetalheImages = "#div-imgs";
Europa.Controllers.Produto.Selector.ModalDetalhe = "#modal_produtos";
Europa.Controllers.Produto.Selector.ModalPlanta = "#modal_planta";
Europa.Controllers.Produto.Selector.ModalDetalheImg = "#img-zoom";
Europa.Controllers.Produto.Selector.ModalDetalheVideo = "#video-detail";
Europa.Controllers.Produto.Template.ProximaImg = 0;
Europa.Controllers.Produto.Template.VoltarImg = 0;

$(function () {
    Europa.Controllers.Produto.ListaProdutos();
})

Europa.Controllers.Produto.ListaProdutos = function () {
    $(Europa.Controllers.Produto.Selector.Spinner).show();
    $.get(Europa.Controllers.Produto.Url.Listar, null, function (res) {
        $(Europa.Controllers.Produto.Selector.ListWrapper).empty();
        $(Europa.Controllers.Produto.Selector.ListWrapper).html(res);
        $(Europa.Controllers.Produto.Selector.Spinner).hide();
    });
};

Europa.Controllers.Produto.Search = function () {
    var q = Europa.Controllers.Produto.Normalize($("#search").val());
    var tipo = Europa.Controllers.Produto.Normalize($('input[name=filtroProduto]:checked').val() + "");

    $(Europa.Controllers.Produto.Selector.ListWrapper + " .book-empreendimento-card").filter(function () {
        var nomeIdx = Europa.Controllers.Produto.Normalize($(this).data("nome"));
        var bairroIdx = Europa.Controllers.Produto.Normalize($(this).data("bairro"));
        var cidadeIdx = Europa.Controllers.Produto.Normalize($(this).data("cidade"));
        var empreendimentoVerificadoIdc = Europa.Controllers.Produto.Normalize($(this).data("verificado"));
        var match = (nomeIdx.indexOf(q) > -1 || bairroIdx.indexOf(q) > -1 || cidadeIdx.indexOf(q) > -1)
            && empreendimentoVerificadoIdc.indexOf(tipo) > -1;
        $(this).parent().toggle(match);
    });
};

Europa.Controllers.Produto.AbrirModal = function (id, isEmpreendimento) {
    Spinner.Show();
    $.post(Europa.Controllers.Produto.Url.DetalharProduto, {
        id: id,
        isEmpreendimento: isEmpreendimento
    }, function (response) {
        Spinner.Hide();
        $(Europa.Controllers.Produto.Selector.ModalDetalheWrapper).html(response);
        $(Europa.Controllers.Produto.Selector.ModalDetalhe).modal("show");
    });
};

Europa.Controllers.Produto.FecharModal = function () {
    $(Europa.Controllers.Produto.Selector.ModalDetalhe).modal("hide");
    $(Europa.Controllers.Produto.Selector.ModalDetalheWrapper).empty();
};

Europa.Controllers.Produto.SalvarBook = function (id, isEmpreendimento) {
    var data = {
        id: id,
        isEmpreendimento: isEmpreendimento
    };
    Europa.Components.DownloadFileNew(Europa.Controllers.Produto.Url.DownloadBook, data);
}

Europa.Controllers.Produto.AbrirModalPlanta = function () {
    const nome = $("#produto-nome",Europa.Controllers.Produto.Selector.ModalDetalhe).text();
    const bairro = $("#produto-bairro",Europa.Controllers.Produto.Selector.ModalDetalhe).text();
    $("#TituloNome",Europa.Controllers.Produto.Selector.ModalPlanta).text(nome);
    $("#TituloBairro",Europa.Controllers.Produto.Selector.ModalPlanta).text(bairro);
    $(Europa.Controllers.Produto.Selector.ModalDetalhe).modal("hide");
    $(Europa.Controllers.Produto.Selector.ModalPlanta).modal("show");
};

Europa.Controllers.Produto.SelecionarImg = function (type, url, nome) {
    if (type === "video") {
        var urlVideo = "https://www.youtube.com/embed/" + nome;
        $(Europa.Controllers.Produto.Selector.ModalDetalheImg).css("display", "none");
        $(Europa.Controllers.Produto.Selector.ModalDetalheImg).attr("src", url);
        $(Europa.Controllers.Produto.Selector.ModalDetalheVideo).css("display", "block");
        $(Europa.Controllers.Produto.Selector.ModalDetalheVideo).attr("src", urlVideo);
    } else {
        $(Europa.Controllers.Produto.Selector.ModalDetalheImg).css("display", "block");
        $(Europa.Controllers.Produto.Selector.ModalDetalheImg).attr("src", url);
        $(Europa.Controllers.Produto.Selector.ModalDetalheVideo).css("display", "none");
        $(Europa.Controllers.Produto.Selector.ModalDetalheVideo).removeAttr("src");
    }
};

Europa.Controllers.Produto.ProxImg = function () {
    $(Europa.Controllers.Produto.Selector.ModalDetalheImages).animate({scrollLeft: Europa.Controllers.Produto.Template.ProximaImg + 200 + "px"});
    Europa.Controllers.Produto.Template.ProximaImg = Europa.Controllers.Produto.Template.ProximaImg + 200;
    Europa.Controllers.Produto.Template.VoltarImg = Europa.Controllers.Produto.Template.VoltarImg + 200;
}

Europa.Controllers.Produto.VoltarImg = function () {
    $(Europa.Controllers.Produto.Selector.ModalDetalheImages).animate({scrollLeft: Europa.Controllers.Produto.Template.VoltarImg - 200 + "px"});
    Europa.Controllers.Produto.Template.VoltarImg = Europa.Controllers.Produto.Template.VoltarImg - 200;
    Europa.Controllers.Produto.Template.ProximaImg = Europa.Controllers.Produto.Template.ProximaImg - 200;
}
Europa.Controllers.Produto.Normalize = function (text) {
    return text.toUpperCase().normalize('NFD').replace(/[\u0300-\u036f]/g, "");
};

