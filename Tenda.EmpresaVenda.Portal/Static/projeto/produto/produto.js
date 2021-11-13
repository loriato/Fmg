"use strict";

Europa.Controllers.Produto = {};
Europa.Controllers.Produto.UrlListar = undefined;
Europa.Controllers.Produto.ListaProdutos = undefined;
Europa.Controllers.Produto.Book = {};
Europa.Controllers.Produto.Book.Id = "#book-empreendimento-target";
Europa.Controllers.Produto.Book.Selector = undefined;
Europa.Controllers.Produto.Book.Vars = {};
Europa.Controllers.Produto.Spinner = {};
Europa.Controllers.Produto.Spinner.Id = "#book-empreendimento-spinner";
Europa.Controllers.Produto.Spinner.Selector = undefined;
Europa.Controllers.Produto.Template = {};
Europa.Controllers.Produto.Template.Id = "#book-empreendimento-card-template";
Europa.Controllers.Produto.Template.Html = undefined;
Europa.Controllers.Produto.Template.ProximaImg = 0;
Europa.Controllers.Produto.Template.VoltarImg = 0;

$(document).ready(function () {
    Europa.NavbarScrollControl("tabMenu", "block_content_page");
    $("#tabMenu li").click(function (e) {
        e.preventDefault();
        Europa.OnTabChange(this, "tabMenu", "block_content_page");
    });




    // Herdando configurações já efetuadas, já que este método só é executado após a página ser carregada.
    // Se não fizer isso, sobrescrevo todas as propriedades setadas anteriormente (geralmente URLs e permissÕes)
    var self = Europa.Controllers.Produto;

    self.Book.Selector = $(self.Book.Id);
    self.Spinner.Selector = $(self.Spinner.Id);
    self.Template.Html = $(self.Template.Id).html();
    self.Template.ImgTemplate = '<img class="img-responsive img-book-slides image-{id}-{idx}" onclick="{download-action}" src="{src}">';

    self.Init = function () {
        self.Data = undefined;
        self.ListaProdutos = undefined;
    };

    self.GetData = function (objs) {
        if (objs == undefined) {
            $.get(self.UrlListar, function (response) {
                if (self.ListaProdutos == undefined) {
                    self.ListaProdutos = response;
                }
                if (objs != undefined) {
                    self.Data = objs;
                } else {
                    self.Data = response;
                }
                // Clear target places
                self.Book.Selector.html("");

                var prefix = "book-";
                var elementIndex = 0;

                self.Spinner.Selector.hide();

                // Dictionary to control SlideShow of Images
                self.Book.Vars.SlideControl = [];


                self.Data.forEach(function (entry) {
                    elementIndex++;

                    entry.Id = prefix + elementIndex;
                    // Build Search Indexes
                    // Improvements: Create Phonetic Index!
                    entry.NomeIdx = entry.Nome.toUpperCase().normalize('NFD').replace(/[\u0300-\u036f]/g, "");
                    entry.Endereco.BairroIdx = entry.Endereco.Bairro.toUpperCase().normalize('NFD').replace(/[\u0300-\u036f]/g, "");
                    entry.Endereco.CidadeIdx = entry.Endereco.Cidade.toUpperCase().normalize('NFD').replace(/[\u0300-\u036f]/g, "");
                    entry.Endereco.EstadoIdx = entry.Endereco.Estado.toUpperCase().normalize('NFD').replace(/[\u0300-\u036f]/g, "");
                    entry.VerificarEmpreendimentoIdx = (entry.VerificarEmpreendimento + "").toUpperCase().normalize('NFD').replace(/[\u0300-\u036f]/g, "");

                    // To Write Html Data
                    entry.Html = self.WriteItem(entry);

                    self.Book.Selector.append(entry.Html);

                    // Definindo que a imagem a ser exibida do book é a primeira
                    self.Book.Vars.SlideControl.push({
                        key: entry.Id,
                        current: 1,
                        min: 1,
                        max: entry.Book.length
                    });

                    $(".image-" + entry.Id + "-1", "#" + entry.Id).css("display", "block");
                });
            });
        } else {
            self.Data = objs;
            // Clear target places
            self.Book.Selector.html("");

            var prefix = "book-";
            var elementIndex = 0;

            self.Spinner.Selector.hide();

            // Dictionary to control SlideShow of Images
            self.Book.Vars.SlideControl = [];
            self.Data.forEach(function (entry) {
                elementIndex++;

                entry.Id = prefix + elementIndex;
                // Build Search Indexes
                // Improvements: Create Phonetic Index!
                entry.NomeIdx = entry.Nome.toUpperCase().normalize('NFD').replace(/[\u0300-\u036f]/g, "");
                entry.Endereco.BairroIdx = entry.Endereco.Bairro.toUpperCase().normalize('NFD').replace(/[\u0300-\u036f]/g, "");
                entry.Endereco.CidadeIdx = entry.Endereco.Cidade.toUpperCase().normalize('NFD').replace(/[\u0300-\u036f]/g, "");
                entry.Endereco.EstadoIdx = entry.Endereco.Estado.toUpperCase().normalize('NFD').replace(/[\u0300-\u036f]/g, "");
                entry.VerificarEmpreendimentoIdx = (entry.VerificarEmpreendimento + "").toUpperCase().normalize('NFD').replace(/[\u0300-\u036f]/g, "");

                // To Write Html Data
                entry.Html = self.WriteItem(entry);

                self.Book.Selector.append(entry.Html);

                // Definindo que a imagem a ser exibida do book é a primeira
                self.Book.Vars.SlideControl.push({
                    key: entry.Id,
                    current: 1,
                    min: 1,
                    max: entry.Book.length
                });

                $(".image-" + entry.Id + "-1", "#" + entry.Id).css("display", "block");
            });

        }


    };

    self.Search = function (q) {
        self.Spinner.Selector.show();
        self.Book.Selector.hide();

        q = $("#search").val().toUpperCase().normalize('NFD').replace(/[\u0300-\u036f]/g, "");
        var fi = ($('input[name=filtroProduto]:checked').val() + "").toUpperCase().normalize('NFD').replace(/[\u0300-\u036f]/g, "");
        var results = self.ListaProdutos.filter(reg => (reg.NomeIdx.includes(q)
            || reg.Endereco.BairroIdx.includes(q)
            || reg.Endereco.CidadeIdx.includes(q))
            && reg.VerificarEmpreendimentoIdx.includes(fi));
        self.GetData(results);
        // FIXME: Deveria pegar apenas o delta para desabilitar
        //self.Data.forEach(function (entry) {
        //    $('#' + entry.Id).remove('display', 'none');
        //});

        //results.forEach(function (entry) {
        //    $('#' + entry.Id).css('display', 'block');
        //});

        setTimeout(function () {
            self.Spinner.Selector.hide();
            self.Book.Selector.show();
        }, 200);
    };

    // Write Item only once, keeping a ID reference on self.Data
    // Every search only change css:display property to none|block
    self.WriteItem = function (entry) {
        // Value or Empty All
        entry.Endereco.Logradouro = self.ValueOrEmpty(entry.Endereco.Logradouro);
        entry.Endereco.Numero = self.ValueOrEmpty(entry.Endereco.Numero);
        entry.Endereco.Bairro = self.ValueOrEmpty(entry.Endereco.Bairro);
        entry.Endereco.Cidade = self.ValueOrEmpty(entry.Endereco.Cidade);
        entry.Endereco.Estado = self.ValueOrEmpty(entry.Endereco.Estado);

        var separator = ", ";
        var lastSeparator = " - ";
        var endereco = entry.Endereco.Logradouro;
        if (entry.Endereco.Logradouro !== "") {
            endereco += separator;
        }
        endereco += entry.Endereco.Numero;
        if (entry.Endereco.Numero !== "") {
            endereco;
        }
        //endereco += entry.Endereco.Bairro;
        //if (entry.Endereco.Bairro !== "") { endereco += separator; }
        //endereco += entry.Endereco.Cidade;
        //if (entry.Endereco.Cidade !== "") { endereco += lastSeparator; }
        //endereco += entry.Endereco.Estado;

        // HMN: Removendo já que só vamos exibir a imagem principal
        // FIXME: Apagar quando a proposta de IV for aprovada
        //var imagens = "";   
        //if (self.HasValue(entry.Book)) {
        //    var index = 1;
        //    entry.Book.forEach(function (bookEntry) {
        //        var image = self.Template.ImgTemplate
        //            .replace("{id}", entry.Id)
        //            .replace("{idx}", index);
        //        if (bookEntry.ContentType.toLowerCase().includes("image")) {
        //            image = image.replace("{src}", bookEntry.Url)
        //                .replace("{download-action}", "");
        //        } else {
        //            image = image.replace("{src}", bookEntry.UrlThumbnail)
        //                .replace("{download-action}", "Europa.Controllers.Produto.Download('" + bookEntry.Url + "')");
        //        }
        //        imagens += image;
        //        index++;
        //    });
        //}

        var imagem = "";
        if (self.HasValue(entry.ImagemPrincipal)) {
            var image = self.Template.ImgTemplate
                .replace("{id}", entry.Id)
                .replace("{idx}", 0);
            imagem = image.replace("{src}", entry.ImagemPrincipal.Url).replace("{download-action}", "");
        }

        return self.Template.Html
            .replace("{id}", entry.Id)
            .replace("{id}", entry.Id)
            .replace("{nome}", entry.Nome)
            .replace("{nome}", entry.Nome)
            .replace("{verificarEmpreendimento}", entry.VerificarEmpreendimento + "" == 'true' ? "" : "hidden")
            .replace("{verificarEmpreendimento}", entry.VerificarEmpreendimento + "" == 'true' ? "hidden" : "")
            .replace("{endereco}", endereco)
            .replace("{imagens}", imagem)
            .replace("{idBreveLancamento}", entry.IdBreveLancamento)
            .replace("{verificarEmpreendimento}", entry.VerificarEmpreendimento)
            .replace("{idEmpreendimento}", entry.IdEmpreendimento)
            .replace("{verificarEmpreendimento}", entry.VerificarEmpreendimento + "" == 'true' ? "" : "hidden")
            .replace("{verificarEmpreendimento}", entry.VerificarEmpreendimento)
            .replace("{idBreveLancamento}", entry.IdBreveLancamento)
            .replace("{verificarEmpreendimento}", entry.VerificarEmpreendimento + "" == 'true' ? "hidden" : "")
            .replace("{informacoes}", self.ValueOrEmpty(entry.Informacoes))
            .replace("{bairro}", entry.Endereco.Bairro)
            .replace("{cidade}", entry.Endereco.Cidade);
    };

    self.Download = function (url) {
        window.open(url, '_blank');
    };

    self.ValueOrEmpty = function (value) {
        if (!self.HasValue(value)) {
            return "";
        }
        return value.trim();
    };

    self.HasValue = function (value) {
        return value !== undefined && value !== "null" && value !== null;
    };


    // SlideShow Control Functions
    self.NextImage = function (element) {
        var bookId = element.parent().parent().attr('id');
        var slideItem = self.Book.Vars.SlideControl.find(reg => reg.key === bookId);

        // Next item
        var next = slideItem.current + 1;
        if (next > slideItem.max) {
            next = slideItem.min;
        }
        slideItem.current = next;

        // Put all to display none
        element.parent().children('img').each(function () {
            $(this).css("display", "none");
        });
        // Show previous image
        $(".image-" + bookId + "-" + next, "#" + bookId).css("display", "block");
    };

    self.PreviousImage = function (element) {
        var bookId = element.parent().parent().attr('id');
        var slideItem = self.Book.Vars.SlideControl.find(reg => reg.key === bookId);

        var previous = slideItem.current - 1;
        if (previous < slideItem.min) {
            previous = slideItem.max;
        }
        slideItem.current = previous;

        // Put all to display none
        element.parent().children('img').each(function () {
            $(this).css("display", "none");
        });
        // Show previous image
        $(".image-" + bookId + "-" + previous, "#" + bookId).css("display", "block");
    };

    // Garantindo referencia    
    Europa.Controllers.Produto = self;

    Europa.Controllers.Produto.GetData();
});

Europa.Controllers.Produto.AbrirPreProposta = function (id) {
    $.post(Europa.Controllers.Produto.UrlPreProposta, {idEmpre: id}, function (response) {
    });
}

Europa.Controllers.Produto.AbrirModal = function (id, empre) {
    $.post(Europa.Controllers.Produto.UrlAbrirModalInfo, {idBl: id, empreendimento: empre}, function (response) {
        if (response.Sucesso) {
            $("#div_breve_lancamento_info").html(response.Objeto);
            $("#modal_produtos").modal("show");
            if (empre) {
                $("#btnInfoSimu").css("display", "none");
            } else {
                $("#btnInfoSimu").css("display", "unset");
            }
        }
    });
};
Europa.Controllers.Produto.AbrirModalPlanta = function () {
    var nome = $("#myModalLabel").text();
    var bairro = $("#ModalBairro").text();
    $("#TituloNome").text(nome);
    $("#TituloBairro").text(bairro);
    $("#modal_produtos").modal("hide");
    $("#modal_planta").modal("show");
};

Europa.Controllers.Produto.VoltarModalPlanta = function (id, empre) {
    $("#modal_planta").modal("hide");
    $("#modal_produtos").modal("show");
};

Europa.Controllers.Produto.FecharModal = function (id, empre) {
    $("#video-detail-breve-lancamento").removeAttr("src");
    $("#video-detail-empreendimento").removeAttr("src");
    $("#modal_produtos").modal("hide");
};

Europa.Controllers.Produto.SalvarBook = function (id, empre) {
    $.post(Europa.Controllers.Produto.UrlVerificarArquivo, {
        idBl: id,
        tpArquivo: false,
        empre: empre
    }, function (response) {
        if (response.Sucesso) {
            var formExportar = $("#form_exportar");
            formExportar.find("input").remove();
            formExportar.attr("method", "get").attr("action", Europa.Controllers.Produto.UrlSalvarBook);
            formExportar.addHiddenInputData({idBl: id, empre: empre});
            formExportar.submit();
        } else {
            Europa.Informacao.PosAcao(response);
        }
    });
}

Europa.Controllers.Produto.DownloadSimulador = function (idBl) {
    $.post(Europa.Controllers.Produto.UrlVerificarArquivo, {
        idBl: idBl,
        tpArquivo: true,
        empre: false
    }, function (response) {
        if (response.Sucesso) {
            var formExportar = $("#form_exportar");
            formExportar.find("input").remove();
            formExportar.attr("method", "post").attr("action", Europa.Controllers.Produto.UrlSalvarSimulador);
            formExportar.addHiddenInputData({idBl: idBl});
            formExportar.submit();
        } else {
            Europa.Informacao.PosAcao(response);
        }
    });
};

Europa.Controllers.Produto.SelecionarImgBreveLancamento = function (type, url, nome) {
    if (type === "video") {
        var urlVideo = "https://www.youtube.com/embed/" + nome;
        $("#img-zoom-breve-lancamento").css("display", "none");
        $("#img-zoom-breve-lancamento").attr("src", url);
        $("#video-detail-breve-lancamento").css("display", "block");
        $("#video-detail-breve-lancamento").attr("src", urlVideo);
    } else {
        $("#img-zoom-breve-lancamento").css("display", "block");
        $("#img-zoom-breve-lancamento").attr("src", url);
        $("#video-detail-breve-lancamento").css("display", "none");
        $("#video-detail-breve-lancamento").removeAttr("src");
    }
};

Europa.Controllers.Produto.SelecionarImgEmpreendimento = function (type, url, nome) {
    if (type === "video") {
        var urlVideo = "https://www.youtube.com/embed/" + nome;
        $("#img-zoom-empreendimento").css("display", "none");
        $("#img-zoom-empreendimento").attr("src", url);
        $("#video-detail-empreendimento").css("display", "block");
        $("#video-detail-empreendimento").attr("src", urlVideo);
    } else {
        $("#img-zoom-empreendimento").css("display", "block");
        $("#img-zoom-empreendimento").attr("src", url);
        $("#video-detail-empreendimento").css("display", "none");
        $("#video-detail-empreendimento").removeAttr("src");
    }
};

Europa.Controllers.Produto.ProxImg = function () {
    $("#div-imgs").animate({scrollLeft: Europa.Controllers.Produto.Template.ProximaImg + 200 + "px"});
    Europa.Controllers.Produto.Template.ProximaImg = Europa.Controllers.Produto.Template.ProximaImg + 200;
    Europa.Controllers.Produto.Template.VoltarImg = Europa.Controllers.Produto.Template.VoltarImg + 200;
}

Europa.Controllers.Produto.VoltarImg = function () {
    $("#div-imgs").animate({scrollLeft: Europa.Controllers.Produto.Template.VoltarImg - 200 + "px"});
    Europa.Controllers.Produto.Template.VoltarImg = Europa.Controllers.Produto.Template.VoltarImg - 200;
    Europa.Controllers.Produto.Template.ProximaImg = Europa.Controllers.Produto.Template.ProximaImg - 200;
}

Europa.Controllers.Produto.TabelaValorNominal = function (id) {
    window.open(Europa.Controllers.Produto.UrlTabelaNominal + '?id=' + id, '_blank');
}