
DataTableApp.controller('CadastroBanner', cadastroBannerTable);

function cadastroBannerTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

    Europa.Controllers.CadastroBanner.DataTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);

    Europa.Controllers.CadastroBanner.DataTable
        .setIdAreaHeader("CadastroBannerDatatable_barra")
        .setTemplateEdit([
            '<input type="text" class="form-control" id="Descricao" name="Descricao" value="" maxlength="128">',
            Europa.Controllers.CadastroBanner.DropDownSituacao,
            '<input type="text" datepicker="datepicker" class="form-control" id="InicioVigencia" name="InicioVigencia" onchange="Europa.Controllers.CadastroBanner.OnChangeDataInicio()">',
            '<input type="text"  datepicker="datepicker" class="form-control" id="FimVigencia" name="FimVigencia">',
            '<input type="text" class="form-control" id="NomeArquivo" name="NomeArquivo" value="" maxlength="128" disabled>',
            '<input type="text" class="form-control" id="Link" name="Link" value="" maxlength="256">',
            '<select id="autocomplete_regionaledit" name="Regional" class="select2-container form-control" value="" style="width:150px;"></select>',
            Europa.Controllers.CadastroBanner.DropDownEstado,
            Europa.Controllers.CadastroBanner.DropDownTipo,
            '<select class="form-control" id="Exibicao" name="Exibicao">' +
            '<option value="true">' + Europa.i18n.Messages.Sim + '</option>' +
            '<option value="false">' + Europa.i18n.Messages.Nao + '</option>' +
            '</select>',
            '<select class="form-control" id="Diretor" name="Diretor">' +
            '<option value="true">' + Europa.i18n.Messages.Sim + '</option>' +
            '<option value="false">' + Europa.i18n.Messages.Nao + '</option>' +
            '</select>',
        ])
        .setColumns([
            DTColumnBuilder.newColumn('Descricao').withTitle(Europa.i18n.Messages.Descricao).withOption('width', '150px'),
            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('width', '50px').withOption('type', 'enum-format-SituacaoBanner'),
            DTColumnBuilder.newColumn('InicioVigencia').withTitle(Europa.i18n.Messages.InicioVigencia).renderWith(Europa.Date.toGeenDateFormat).withOption('width', '50px'),
            DTColumnBuilder.newColumn('FimVigencia').withTitle(Europa.i18n.Messages.FimVigencia).renderWith(renderData).withOption('width', '50px'),
            DTColumnBuilder.newColumn('NomeArquivo').withTitle(Europa.i18n.Messages.Arquivo).withOption('width', '100px'),
            DTColumnBuilder.newColumn('Link').withTitle(Europa.i18n.Messages.Link).withOption('width', '150px'),
            DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption('width', '60px'),
            DTColumnBuilder.newColumn('Estado').withTitle(Europa.i18n.Messages.UF).withOption('width', '60px').renderWith(renderEstado),
            DTColumnBuilder.newColumn('Tipo').withTitle(Europa.i18n.Messages.Tipo).withOption('width', '50px').withOption('type', 'enum-format-TipoBanner'),
            DTColumnBuilder.newColumn('Exibicao').withTitle(Europa.i18n.Messages.ExibicaoUnica).withOption('width', '50px').renderWith(renderBoolean),
            DTColumnBuilder.newColumn('Diretor').withTitle(Europa.i18n.Messages.Diretor).withOption('width', '50px').renderWith(renderBoolean),
            DTColumnBuilder.newColumn('IdArquivo').withClass("hidden", "hidden"),
        ])
        .setColActions(actionsHtml, '100px')
        .setDefaultOrder([[1, 'desc']])
        .setActionSave(Europa.Controllers.CadastroBanner.PreSalvar)
        .setAutoInit(true)
        .setOptionsSelect('POST', Europa.Controllers.CadastroBanner.UrlListar, Europa.Controllers.CadastroBanner.Filtro);

    function actionsHtml(data, type, full, meta) {
        var visualizar = Europa.Controllers.CadastroBanner.Permissoes.Visualizar && full.IdArquivo > 0;
        var excluir = Europa.Controllers.CadastroBanner.Permissoes.Excluir && !full.Visualizado;
        var editar = Europa.Controllers.CadastroBanner.Permissoes.Atualizar && !full.Visualizado;

        return '<div>' +
            $scope.renderButton(visualizar, "Exibir", 'fa fa-eye', 'mostrarBanner(' + meta.row + ')') +
            $scope.renderButton(Europa.Controllers.CadastroBanner.Permissoes.Atualizar, "Editar", "fa fa-edit", "editar(" + meta.row + ")") +
            $scope.renderButton(excluir, "Excluir", "fa fa-trash", "excluir(" + meta.row + ")") +
            $scope.renderButton(editar, "Upload", "fa fa-upload", "upload(" + meta.row + ")") +
            '</div>';
    }

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (hasPermission === false) {
            return "";
        }

        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    function renderBoolean(data) {
        if (data === true) {
            return Europa.i18n.Messages.Sim;
        }

        return Europa.i18n.Messages.Nao;
    }

    function renderEstado(data) {
        if (data) {
            return data;
        }
        return Europa.i18n.Messages.Todas;
    }

    $scope.mostrarBanner = function (row) {
        var obj = Europa.Controllers.CadastroBanner.DataTable.getRowData(row);
        Europa.Controllers.CadastroBanner.Preview(obj.Id);

        $("#btn-fechar").removeClass("hidden");
        $("#btn-aceitar").addClass("hidden");

        if (obj.Tipo == 1) {
            $("#btn-aceitar").removeClass("hidden");
            $("#btn-fechar").addClass("hidden");
        }
    };

    $scope.editar = function (row) {
        Europa.Controllers.CadastroBanner.Url = false;
        $scope.rowEdit(row);

        var obj = Europa.Controllers.CadastroBanner.DataTable.getRowData(row);


        if (!obj.Estado) {
            $("#filtro_estados").val("");
        } else {
            $("#filtro_estados").val(obj.Estado);
        }

        $("#Exibicao").val(obj.Exibicao.toString());
        $("#Diretor").val(obj.Diretor.toString());

        $("#IdArquivo").val(obj.IdArquivo);

        $("#Visualizado").val(obj.Visualizado);

        if (obj.Visualizado) {
            $("#Tipo").prop("disabled", "disabled");
            $("#Exibicao").prop("disabled", "disabled");
            $("#Diretor").prop("disabled", "disabled");
        }

        var date = Europa.String.FormatAsGeenDate($("#InicioVigencia").val());
        var date2 = Europa.String.FormatAsGeenDate($("#FimVigencia").val());

        Europa.Components.DatePicker.AutoApply();

        Europa.Controllers.CadastroBanner.DataInicio = new Europa.Components.DatePicker()
            .WithTarget("#InicioVigencia")
            .WithFormat("DD/MM/YYYY")
            .WithMinDate(Europa.Date.Now())
            .WithValue(date)
            .Configure();

        Europa.Controllers.CadastroBanner.DataInicio = new Europa.Components.DatePicker()
            .WithTarget("#FimVigencia")
            .WithFormat("DD/MM/YYYY")
            .WithValue(date2)
            .Configure();

        Europa.Controllers.CadastroBanner.OnChangeTipo();

        if ($("#Situacao").val() == 1) {
            $("[name='InicioVigencia']").prop("disabled", true);
            $("[name='FimVigencia']").prop("disabled", true);
        }

        Europa.Controllers.CadastroBanner.AutoCompleteRegionalEdit = new Europa.Components.AutoCompleteRegionais()
            .WithTargetSuffix("regionaledit")
            .Configure();

        Europa.Controllers.CadastroBanner.AutoCompleteRegionalEdit.SetValue(obj.IdRegional, obj.Regional);


        
    };

    $scope.upload = function (row) {
        $("#IdArquivo").val(0);
        var obj = Europa.Controllers.CadastroBanner.DataTable.getRowData(row);
        Europa.Controllers.CadastroBanner.IdBanner = obj.Id;
        $("#modal_cadastrar_banner").show();
        $("#fileid").get(0).value = "";
    };

    $scope.excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.CadastroBanner.DataTable.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.Descricao, function () {
            $.post(Europa.Controllers.CadastroBanner.UrlExcluir, { id: objetoLinhaTabela.Id }, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.CadastroBanner.DataTable.reloadData();                    
                }
                Europa.Informacao.PosAcao(res);
            });
        });
    };

    function renderData(data) {        
        if (data) {
            return Europa.Date.toGeenDateFormat(data);
        }
        return "";
    }
        
};

Europa.Controllers.CadastroBanner.Filtro = function () {
    var params = {
        Descricao: $("#DescricaoBanner").val(),
        Situacao: $("#SituacaoBannerFiltro").val(),
        Tipo: $("#TipoBannerForm").val(),
        InicioVigencia: $("#DataInicioVigencia").val(),
        FimVigencia: $("#DataFimVigencia").val(),
        Estado: $("#filtro_estados").val(),
        IdRegional: $('#autocomplete_regional').val(),
        Diretor: $("#DiretorBannerFiltro").is(":checked") ? true : null,
        Exibicao: $("#ExibicaoBannerFiltro").is(":checked") ? true : null
    };
    
    return params;
};

Europa.Controllers.CadastroBanner.Filtrar = function () {
    Europa.Controllers.CadastroBanner.DataTable.reloadData();
};


Europa.Controllers.CadastroBanner.PreSalvar = function () {
    var objetoLinhaTabela = Europa.Controllers.CadastroBanner.DataTable.getDataRowEdit();

    objetoLinhaTabela.Arquivo = {
        Id: $("#IdArquivo").val()
    };

    objetoLinhaTabela.Visualizado = $("#Visualizado").val();

    if (objetoLinhaTabela.Exibicao === "true") {
        objetoLinhaTabela.Exibicao = true
    }
    if (objetoLinhaTabela.Exibicao === "false") {
        objetoLinhaTabela.Exibicao = false
    }
    if (objetoLinhaTabela.Diretor === "true") {
        objetoLinhaTabela.Diretor = true
    }
    if (objetoLinhaTabela.Diretor === "false") {
        objetoLinhaTabela.Diretor = false
    }

    objetoLinhaTabela.Estado = $('#filtro_estados_edit').val();
    objetoLinhaTabela.Regional = { Id: $('#autocomplete_regionaledit').val() };

    Europa.Controllers.CadastroBanner.Salvar(objetoLinhaTabela);
}

Europa.Controllers.CadastroBanner.Salvar = function (obj) {
    var url = Europa.Controllers.CadastroBanner.Url ? Europa.Controllers.CadastroBanner.UrlIncluir : Europa.Controllers.CadastroBanner.UrlAlterar;
    Europa.Validator.ClearForm("#form_fitro_banner");
    $.post(url, { model: obj }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.CadastroBanner.DataTable.closeEdition();
            Europa.Controllers.CadastroBanner.DataTable.reloadData();
        }
        Europa.Informacao.PosAcao(res);
    });
}

Europa.Controllers.CadastroBanner.Novo = function () {
    $("#fileid").get(0).value = "";
    $("#IdArquivo").val(0);
    $("#Visualizado").val(false);
    Europa.Controllers.CadastroBanner.DataTable.createRowNewData();
    $("#Arquivo").attr("disabled", "disabled");
    $("#Situacao").val(2);
    $("#Situacao").attr("disabled", "disabled");
    $("#Exibicao").prop("disabled", true);
    Europa.Controllers.CadastroBanner.AutoCompleteRegionalEdit = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regionaledit")
        .Configure();
    Europa.Controllers.CadastroBanner.Url = true;
    Europa.Controllers.CadastroBanner.ConfigDatePicker();
};

Europa.Controllers.CadastroBanner.SalvarBanner = function () {
    var fileUpload = $("#fileid").get(0);

    var files = fileUpload.files;

    if (files.length < 1) {
        var res = {
            Sucesso: false,
            Mensagens:["Insira um arquivo"]
        }

        Europa.Informacao.PosAcao(res);

        return;
    }
    
    var fileData = new FormData();

    fileData.append('file', files[0]);
    var id = Europa.Controllers.CadastroBanner.IdBanner;
    fileData.append('id', id);

    $.ajax({
        url: Europa.Controllers.CadastroBanner.UrlUpload,
        data: fileData,
        type: 'POST',
        processData: false,
        contentType: false,
        success: function (res) {
            Europa.Controllers.CadastroBanner.FecharModal();
            Europa.Controllers.CadastroBanner.DataTable.reloadData();
            Europa.Informacao.PosAcao(res);
            $('#modal_cadastrar_banner').hide()
        }
    });
};

Europa.Controllers.CadastroBanner.FecharModal = function () {
    $("#modal_cadastrar_banner").hide();
};

Europa.Controllers.CadastroBanner.FecharModalView = function () {
    $("#modal_visualizar_imagem").hide();
};

Europa.Controllers.CadastroBanner.OnChangeTipo = function () {
    var tipo = $("#Tipo").val();
    $("#Exibicao").prop("disabled", false);
    if (tipo == 1) {
        $("#Exibicao").val("true");
        $("#Exibicao").prop("disabled", true);
    }
};