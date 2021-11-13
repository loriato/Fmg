"use strict";

Europa.Controllers.BookBreveLancamento = {};
Europa.Controllers.BookBreveLancamento.Datatable = {};

Europa.Controllers.BookBreveLancamento.UrlListar = undefined;
Europa.Controllers.BookBreveLancamento.UrlExcluir = undefined;
Europa.Controllers.BookBreveLancamento.UrlUpload = undefined;

Europa.Controllers.BookBreveLancamento.IdFormUpload = "#form_upload";

Europa.Controllers.BookBreveLancamento.Modal = {};
Europa.Controllers.BookBreveLancamento.Modal.Id = "#modal_book_empreeendimento";
Europa.Controllers.BookBreveLancamento.Modal.Selector = undefined;
Europa.Controllers.BookBreveLancamento.Modal.Vars = {};
Europa.Controllers.BookBreveLancamento.Modal.Vars.IdBreveLancamento = undefined;
Europa.Controllers.BookBreveLancamento.Modal.Vars.CallbackFuncion = undefined;


$(document).ready(function () {
    Europa.Controllers.BookBreveLancamento.Modal.Selector = $(Europa.Controllers.BookBreveLancamento.Modal.Id);
});


Europa.Controllers.BookBreveLancamento.Modal.Show = function (idBreveLancamento, callbackfuncion) {
    if (idBreveLancamento === undefined) {
        alert('Parameter idBreveLancamento is required');
    }
    // Back to default state
    Europa.Controllers.BookBreveLancamento.Modal.Init();

    Europa.Controllers.BookBreveLancamento.Modal.Vars.IdBreveLancamento = idBreveLancamento;
    Europa.Controllers.BookBreveLancamento.Modal.Vars.CallbackFuncion = callbackfuncion;

    Europa.Controllers.BookBreveLancamento.Datatable.reloadData();

    Europa.Controllers.BookBreveLancamento.Modal.Selector.modal("show");
};

Europa.Controllers.BookBreveLancamento.Modal.Init = function () {
    Europa.Controllers.BookBreveLancamento.Modal.Vars.IdBreveLancamento = undefined;
    Europa.Controllers.BookBreveLancamento.Modal.Vars.CallbackFuncion = undefined;
};

Europa.Controllers.BookBreveLancamento.Modal.Hide = function () {
    Europa.Controllers.BookBreveLancamento.Modal.Selector.modal("hide");
};

/// DataTable ------------------------------------------------------------------------------------------
Europa.Controllers.BookBreveLancamento.FilterParams = function () {
    var filtro = {
        idBreveLancamento: Europa.Controllers.BookBreveLancamento.Modal.Vars.IdBreveLancamento
    };
    return filtro;
};


DataTableApp.controller('bookBreveLancamentoTable', bookBreveLancamentoTable);

function bookBreveLancamentoTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.BookBreveLancamento.Datatable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Controllers.BookBreveLancamento.Datatable.setColumns([
        DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '25%'),
        DTColumnBuilder.newColumn('UrlThumbnail').withTitle(Europa.i18n.Messages.Imagem).notSortable().withOption('width', '30%').renderWith(RenderStaticResourceThumbs),
    ])
        .setIdAreaHeader("book_breve_lancamento_datatable_header")
        .setAutoInit(false)
        .setColActions(actionsHtml, '10%')
        .setOptionsSelect('POST', Europa.Controllers.BookBreveLancamento.UrlListar, Europa.Controllers.BookBreveLancamento.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButtonDetail(true, "detalhar(" + meta.row + ")") +
            $scope.renderButtonDelete(true, "excluir(" + meta.row + ")") +
            '</div>';
    }

    function RenderStaticResourceThumbs(data) {
        return "<img src='" + data + "' style='max-width:128px;max-heigth:128px;'></img>";
    }

    $scope.renderButtonDetail = function (hasPermission, onClick) {
        return $scope.renderButton(hasPermission, "Detalhar", "fa fa-eye", onClick);
    };
    $scope.renderButtonDelete = function (hasPermission, onClick) {
        return $scope.renderButton(hasPermission, "Excluir", "fa fa-trash", onClick);
    };
    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (hasPermission === 'true' || hasPermission === true) {
            icon = $('<i/>').addClass(icon);
            var button = $('<a />')
                .addClass('btn btn-default')
                .attr('title', title)
                .attr('ng-click', onClick)
                .append(icon);
            return button.prop('outerHTML');
        } else {
            return null;
        }
    };

    $scope.detalhar = function (row) {
        var objetoLinhaTabela = Europa.Controllers.BookBreveLancamento.Datatable.getRowData(row);
        if(objetoLinhaTabela.ContentType === "video"){
            window.open("https://www.youtube.com/embed/" + objetoLinhaTabela.Nome + "?autoplay=1");
        }else{
            window.open(objetoLinhaTabela.Url);
        }
    };

    $scope.excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.BookBreveLancamento.Datatable.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.Nome, function () {
            $.post(Europa.Controllers.BookBreveLancamento.UrlExcluir, {idAssociacao: objetoLinhaTabela.Id}, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.BookBreveLancamento.Datatable.reloadData();
                    Europa.Growl.SuccessFromJsonResponse(res);
                } else {
                    Europa.Informacao.PosAcao(res);
                }
            });
        });
    };
};

Europa.Controllers.BookBreveLancamento.Processar = function (columns = 8) {
    if (Europa.Controllers.BookBreveLancamento.Datatable.vm.dtInstance.DataTable === undefined) {
        return;
    }
};

Europa.Controllers.BookBreveLancamento.SimpleDivTemplate = function (id) {
    return "<div align='center' id=" + id + "></div>";
};

Europa.Controllers.BookBreveLancamento.DivTemplate = function (id) {
    return "<div class='outer' align='center' id=" + id + "></div>";
};

Europa.Controllers.BookBreveLancamento.DivImageGrid = function (id) {
    return Europa.Controllers.BookBreveLancamento.DivTemplate("image_grid_" + id);
};

Europa.Controllers.BookBreveLancamento.OnBookTypeChanged = function () {
    var selected = $("#tipo_book_select").val();
    
    if(selected === "0"){
        $("#video_upload_fields").css("display", "none");
        $("#image_upload_fields").css("display", "block");
    }else{
        $("#video_upload_fields").css("display", "block");
        $("#image_upload_fields").css("display", "none");
    }
};


Europa.Controllers.BookBreveLancamento.Upload = function () {
    var tipo = $("#tipo_book_select").val();
    
    var url = tipo === "0" ? Europa.Controllers.BookBreveLancamento.UrlUpload : Europa.Controllers.BookBreveLancamento.UrlVideoUpload;

    var formData = new FormData();
    
    if(tipo === "0"){
        var arquivo = $("#Arquivo", Europa.Controllers.BookBreveLancamento.IdFormUpload).get(0).files[0];

        var maxFileSize = 16;
        var maxFileSizeInBytes = maxFileSize * 1000000;
        if (arquivo !== undefined && arquivo.size > maxFileSizeInBytes) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.ArquivoTamanhoMaximoExcedido, maxFileSize));
            Europa.Informacao.Show();
            return;
        }
        
        formData.append("File", arquivo);
    }else{
        formData.append("YoutubeVideoCode", $("#VideoUrl").val());
    }
    
    formData.append("TargetId", Europa.Controllers.BookBreveLancamento.Modal.Vars.IdBreveLancamento);
    $.ajax({
        type: 'POST',
        url: url,
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
        success: function (res) {
            if (res.Sucesso) {
                Europa.Controllers.BookBreveLancamento.Datatable.reloadData();
                Europa.Growl.SuccessFromJsonResponse(res);
                $("#VideoUrl").val("");
            } else {
                Europa.Informacao.PosAcao(res);
            }
        }
    });
};


/// Implementação Experimental de Grid --------------------------------------------------------------------
Europa.Controllers.BookBreveLancamento.Grid = {};
Europa.Controllers.BookBreveLancamento.Grid.Data = {};
Europa.Controllers.BookBreveLancamento.Grid.MockData = function () {
    return Europa.Controllers.BookBreveLancamento.Datatable.vm.dtInstance.DataTable.rows().data();
};
Europa.Controllers.BookBreveLancamento.Grid.Write = function () {
    var data = Europa.Controllers.BookBreveLancamento.Grid.MockData();

    var template = $('#template_grid_content').html();

    var colNumber = 8;
    var numberOfColsOnEuropaBootstrap = 24;

    var mdSize = numberOfColsOnEuropaBootstrap / colNumber;

    var index = 0;
    var total = data.length;

    // Iniciando como primeira linha
    var allTemplate = "<div class='col-md-24'>";

    for (index = 0; index < total; index++) {

        // Break line
        if (index % colNumber === 0) {
            allTemplate += "</div><div class='col-md-24'>";
        }

        allTemplate += (Europa.Controllers.BookBreveLancamento.Grid.WriteItem(template, mdSize, data[index]));

    }
    allTemplate += "</div>";

    console.log(allTemplate);

    $('#template_target').html(allTemplate);
};


Europa.Controllers.BookBreveLancamento.Grid.WriteItem = function (template, mdSize, model) {
    var maxTextSize = 25;
    var minBreak = 12;
    var sizeOfBreak = 5;

    if (model.Nome.length > maxTextSize) {
        var lengthDiff = model.Nome.length - minBreak + sizeOfBreak;
        var firstPartOfNewName = model.Nome.substring(0, minBreak);
        var lastPartOfNewName = model.Nome.substring(lengthDiff);
        model.Nome = firstPartOfNewName + "....." + lastPartOfNewName;
    }

    var item = template.replace("{img-src}", model.UrlThumbnail)
        .replace("{text}", model.Nome)
        .replace("{size}", mdSize);

    return item;
};