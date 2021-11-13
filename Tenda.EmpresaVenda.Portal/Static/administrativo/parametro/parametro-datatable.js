Europa.Controllers.Parametro = {};
Europa.Controllers.Parametro.Form = {};
Europa.Controllers.Parametro.urlIncluir = undefined;
Europa.Controllers.Parametro.urlAlterar = undefined;
Europa.Controllers.Parametro.urlListar = undefined;
Europa.Controllers.Parametro.urlRemover = undefined;
Europa.Controllers.Parametro.urlSelecionar = undefined;
Europa.Controllers.Parametro.CheckPerms = undefined;
Europa.Controllers.Parametro.DropDownTipoParametro = undefined;
Europa.Controllers.Parametro.Autocompletes = {};
Europa.Controllers.Parametro.urlBuscarMidiaContato = undefined;
Europa.Controllers.Parametro.urlBuscarPerfil = undefined;
Europa.Controllers.Parametro.urlBuscarMeioComunicacao = undefined;
Europa.Controllers.Parametro.urlBuscarUsuarioPortal = undefined;
Europa.Controllers.Parametro.urlBuscarTemplateMensagem = undefined;

DataTableApp.controller("listaParametro", listaParametro);

var modoInclusao = false;
var parametrosBusca = {};
var valorParametroAtual = undefined;

Europa.Controllers.Parametro.OnChangeTipoParametro = function () {

    var op = $("#TipoParametro").val();
    if (op == "1") {
        $("#div_valor_parametro").html('<textarea id="valor_id" name="Valor" type="text" class="form-control" maxlength="2000" rows="4">');
        $("#valor_id").val(valorParametroAtual);
        valorParametroAtual = null;
        return;
    }
    if (op == "7") {
        var html = '<select id="valor_id" name="Valor" class="form-control">' +
                        '<option value="true" selected="selected">True</option>' +
                        '<option value="false">False</option>' +
                    '</select>';
        
        $("#div_valor_parametro").html(html);
        if (!modoInclusao) {
            $("#valor_id").val(valorParametroAtual);
        }
        valorParametroAtual = null;
        return;
    }

    var idAutocomplete = "";
    var autocomplete = undefined;
    var url = "";

    switch (op) {
        case "2":
            idAutocomplete = "autocomplete_midiaContato";
            url = Europa.Controllers.Parametro.urlBuscarMidiaContato;
            break;
        case "3":
            idAutocomplete = "autocomplete_perfil";
            url = Europa.Controllers.Parametro.urlBuscarPerfil;
            break;
        case "4":
            idAutocomplete = "autocomplete_tabulacaoAtendimento";
            url = Europa.Controllers.Parametro.urlBuscarTabulacaoAtendimento;
            break;
        case "5":
            idAutocomplete = "autocomplete_meioComunicacao";
            url = Europa.Controllers.Parametro.urlBuscarMeioComunicacao;
            break;
        case "6":
            idAutocomplete = "autocomplete_usuarioPortal";
            url = Europa.Controllers.Parametro.urlBuscarUsuarioPortal;
            break;
        case "8":
            idAutocomplete = "autocomplete_templateMensagem";
            url = Europa.Controllers.Parametro.urlBuscarTemplateMensagem;
            break;
    }
    $("#div_valor_parametro").html('<select id="' + idAutocomplete + '" name="Valor" class="select2-container form-control"></select>');
    Europa.Controllers.Parametro.InitAutocompletes();

    $.ajax({
        type: "GET",
        url: url,
        data: { id: valorParametroAtual },
        dataType: 'json',
        success: function (res) {
            switch (op) {
                case "2":
                    Europa.Controllers.Parametro.Autocompletes.AutoCompleteMidiaContato.SetValue(res.Id, res.Nome);
                    break;
                case "3":
                    Europa.Controllers.Parametro.Autocompletes.AutoCompletePerfil.SetValue(res.Id, res.Nome);
                    break;
                case "4":
                    Europa.Controllers.Parametro.Autocompletes.AutoCompleteTabulacaoAtendimento.SetValue(res.Id, res.Nome);
                    break;
                case "5":
                    Europa.Controllers.Parametro.Autocompletes.AutoCompleteMeioComunicacaoSemMidia.SetValue(res.Id, res.Nome);
                    break;
                case "6":
                    Europa.Controllers.Parametro.Autocompletes.AutoCompleteUsuarioPortal.SetValue(res.Id, res.Nome);
                    break;
                case "8":
                    Europa.Controllers.Parametro.Autocompletes.AutoCompleteTemplateMensagem.SetValue(res.Id, res.Nome);
                    break;
            }
        }
    });
    valorParametroAtual = null;
};

Europa.Controllers.Parametro.InitAutocompletes = function () {
    Europa.Controllers.Parametro.Autocompletes.AutoCompleteMidiaContato = new Europa.Components.AutoCompleteMidiaContato()
        .WithTargetSuffix("midiaContato")
        .Configure();
    Europa.Controllers.Parametro.Autocompletes.AutoCompletePerfil = new Europa.Components.AutoCompletePerfil()
        .WithTargetSuffix("perfil")
        .Configure();
    Europa.Controllers.Parametro.Autocompletes.AutoCompleteTabulacaoAtendimento = new Europa.Components.AutoCompleteTabulacaoAtendimento()
        .WithTargetSuffix("tabulacaoAtendimento")
        .Configure();
    Europa.Controllers.Parametro.Autocompletes.AutoCompleteMeioComunicacaoSemMidia = new Europa.Components.AutoCompleteMeioComunicacaoSemMidia()
        .WithTargetSuffix("meioComunicacao")
        .Configure();
    Europa.Controllers.Parametro.Autocompletes.AutoCompleteUsuarioPortal = new Europa.Components.AutoCompleteUsuario()
        .WithTargetSuffix("usuarioPortal")
        .Configure();
    Europa.Controllers.Parametro.Autocompletes.AutoCompleteTemplateMensagem = new Europa.Components.AutoCompleteTemplateMensagem()
        .WithTargetSuffix("templateMensagem")
        .Configure();
};

Europa.Controllers.Parametro.Novo = function () {
    Europa.Controllers.Parametro.Tabela.createRowNewData();
    Europa.Controllers.Parametro.RenderSelecionar();
    modoInclusao = true;
    valorParametroAtual = null;
    Europa.Controllers.Parametro.OnChangeTipoParametro();
};

Europa.Controllers.Parametro.RenderSelecionar = function () {
    $.get(Europa.Controllers.Parametro.urlSelecionar, function (result) {
        $("#div_sistema_id").html(result);
    });
};

Europa.Controllers.Parametro.RenderSelecionarComValue = function (id, isNew) {
    $.get(Europa.Controllers.Parametro.urlSelecionar, function (result) {
        if (isNew) {
            $("#div_sistema_id", ".newRow").html(result);
        } else {
            $("#div_sistema_id", "tbody", "#datatable_parametros_id").html(result);
        }
        var inputsSistema = $("[name='Sistema']", "#div_sistema_id");
        $(inputsSistema).val(id);
    });
};

Europa.Controllers.Parametro.PreSalvar = function () {
    var objetoLinhaTabela = Europa.Controllers.Parametro.Tabela.getDataRowEdit();
    var inputsSistema = $("[name='Sistema']", "#datatable_parametros_id");
    idSistema = inputsSistema.val();
    if (idSistema == undefined || idSistema == null || idSistema == 0) {
        inputsSistema = $("[name='Sistema']", "#div_sistema_id");
        idSistema = inputsSistema.val();
    }
    var url = modoInclusao ? Europa.Controllers.Parametro.urlIncluir : Europa.Controllers.Parametro.urlAlterar;

    Europa.Controllers.Parametro.Salvar(objetoLinhaTabela, url, idSistema);

};

Europa.Controllers.Parametro.Salvar = function (obj, url, idSistema) {
    $.post(url, { parametro: obj, idSistema: idSistema }, function (res) {
        Europa.Controllers.Parametro.PosSalvar(res);
    });
};

Europa.Controllers.Parametro.PosSalvar = function (res) {
    if (res.Sucesso) {
        var msg = "";
        var chaveCandidata = res.Objeto.Sistema.Nome + " - " + res.Objeto.Chave;

        if (modoInclusao) {
            msg = Europa.i18n.Messages.RegistroSucesso.replace("{0}", chaveCandidata).replace("{1}", Europa.i18n.Messages.Incluido);
        }
        else {
            msg = Europa.i18n.Messages.RegistroSucesso.replace("{0}", chaveCandidata).replace("{1}", Europa.i18n.Messages.Alterado);
        }
        Europa.Controllers.Parametro.Tabela.closeEdition();
        Europa.Controllers.Parametro.Tabela.reloadData();
        modoInclusao = false;
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Sucesso, msg);
        Europa.Informacao.ShowAsSuccess();
    }
    else {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, res.Mensagens.join("<br\>"));
        Europa.Informacao.Show();
    }

};

Europa.Controllers.Parametro.Excluir = function (id) {
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Excluir, Europa.i18n.Messages.ConfirmacaoExcluirParametro);
    Europa.Confirmacao.ConfirmCallback = function () {

        $.ajax({
            url: Europa.Controllers.Parametro.urlRemover,
            type: "POST",
            data: { id: id }
        }).done(function (res) {
            if (res.Sucesso) {
                Europa.Controllers.Parametro.Tabela.reloadData();
            }
            else {
                Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, res.Mensagens.join("<br\>"));
                Europa.Informacao.Show();
            }
        });
    }

    Europa.Confirmacao.Show();
};

Europa.Controllers.Parametro.Filtrar = function () {
    var inputsSistema = $("[name='Sistema']");
    var sistema = inputsSistema[0];
    var idSistema = $(sistema).val();
    parametrosBusca = {
        chave: $("#filtro-chave").val(),
        idSistema: idSistema
    };
    Europa.Controllers.Parametro.Tabela.reloadData();
};

Europa.Controllers.Parametro.ParametrosFiltro = function () {
    return parametrosBusca;
};

Europa.Controllers.Parametro.LimparFiltro = function () {
    $("#filtro-chave").val("");
    var inputsSistema = $("[name='Sistema']");
    var sistema = inputsSistema[0];
    $(sistema).val("");
};

Europa.Controllers.Parametro.ActionsHtml = function (data, type, full, meta) {
    if (Europa.Controllers.Parametro.CheckPerms == "True") {
        return '<div class="actions-datatable-parametro">' +
        '<button class="btn btn-default"' +
        'ng-click=' +
        '"edit(' + meta.row + ')"' +
        '>' +
        '<i class="fa fa-edit"></i>' +
        '</button>' +
        '<button class="btn btn-default"' +
        'onclick=' +
        '"Europa.Controllers.Parametro.Excluir(' + data.Id + ')"' + '>' +
        '<i class="fa fa-trash"></i>' +
        '</button>' +
        '</div>';
    }
    return "";
};

function listaParametro($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.Parametro.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.Parametro.Tabela;
    tabelaWrapper
        .setTemplateEdit([
            '<div id="div_sistema_id"></div>',
            '<input name="Chave" type="text" class="form-control" maxlength="256">',
            Europa.Controllers.Parametro.DropDownTipoParametro,
            '<div id="div_valor_parametro"></div>',
            ,
            '<textarea name="Descricao" type="text" class="form-control" maxlength="255" rows="4">'
        ])
        .setIdAreaHeader("datatable_parametro_header")
        .setColumns([
            DTColumnBuilder.newColumn("Sistema.Nome").withTitle(Europa.i18n.Messages.Sistema).withOption("width", "15%"),
            DTColumnBuilder.newColumn("Chave").withTitle(Europa.i18n.Messages.Chave).withOption("width", "15%"),
            DTColumnBuilder.newColumn("TipoParametro").withTitle(Europa.i18n.Messages.TipoParametro).withOption('type', 'enum-format-TipoParametro').withOption("width", "14%"),
            DTColumnBuilder.newColumn("Valor").withTitle(Europa.i18n.Messages.Valor).withOption("width", "14%"),
            DTColumnBuilder.newColumn("Detalhe").withTitle(Europa.i18n.Messages.Detalhe).notSortable().withOption("width", "14%"),
            DTColumnBuilder.newColumn("Descricao").withTitle(Europa.i18n.Messages.Descricao).withOption("width", "15%")

        ])
        .setColActions(Europa.Controllers.Parametro.ActionsHtml, "8%")
        .setActionSave(Europa.Controllers.Parametro.PreSalvar)
        .setDefaultOptions("GET",
            Europa.Controllers.Parametro.urlListar,
            Europa.Controllers.Parametro.ParametrosFiltro);


    $scope.edit = function (rowNr) {
        $scope.rowEdit(rowNr);
        var model = tabelaWrapper.getRowData(rowNr);
        valorParametroAtual = model.Valor;
        Europa.Controllers.Parametro.OnChangeTipoParametro();
        Europa.Controllers.Parametro.RenderSelecionarComValue(model.Sistema.Id);
    }
};

Europa.Controllers.Parametro.ExportarPagina = function () {
    var params = Europa.Controllers.Parametro.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.Parametro.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.Parametro.ExportarTodos = function () {
    var params = Europa.Controllers.Parametro.Tabela.lastRequestParams;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.Parametro.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};