Europa.Controllers.RegraComissaoPadrao.IdFormFiltro = "#form_filtro";
Europa.Controllers.RegraComissaoPadrao.IdFormModal = "#form_inclusao";

$(function () {
    $("#filtro_situacoes", Europa.Controllers.RegraComissaoPadrao.IdFormFiltro).select2({
        trags: true,
        width: '100%',
        value: 1
    });
    $("#filtro_regional").select2();

    $("#Regional").select2();

    Europa.Components.DatePicker.AutoApply();
    Europa.Controllers.RegraComissaoPadrao.LimparFiltro();
    Europa.Controllers.RegraComissaoPadrao.Filtrar();
});


DataTableApp.controller('RegraComissaoPadraoTable', RegraComissaoPadraoTable);

function RegraComissaoPadraoTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.RegraComissaoPadrao.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.RegraComissaoPadrao.Tabela;
    tabelaWrapper
        .setTemplateEdit([
            '<input type="text" class="form-control" name="Id" id="Id" style="visibility:hidden">',
            '<input type="text" class="form-control" name="Descricao" id="Descricao" maxlength="255">',
            '<input type="text" class="form-control" name="Regional" id="Regional" disabled="disabled">',
            '<input type="text" class="form-control" name="Situacao" id="Situacao" disabled="disabled">',
            '<input type="text" class="form-control" name="InicioVigencia" id="InicioVigencia" disabled="disabled">',
            '<input type="text" class="form-control" name="TerminoVigencia" id="TerminoVigencia" disabled="disabled">'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('Id').withTitle(Europa.i18n.Messages.Id).withOption('visible', false),
            DTColumnBuilder.newColumn('Descricao').withTitle(Europa.i18n.Messages.Descricao).withOption('width', '36%'),
            DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption('width', '10%'),
            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('type', 'enum-format-SituacaoRegraComissao').withOption('width', '15%'),
            DTColumnBuilder.newColumn('InicioVigencia').withTitle(Europa.i18n.Messages.InicioVigencia).withOption("type", "date-format-DD/MM/YYYY HH:mm:ss").withOption('width', '15%'),
            DTColumnBuilder.newColumn('TerminoVigencia').withTitle(Europa.i18n.Messages.TerminoVigencia).withOption("type", "date-format-DD/MM/YYYY HH:mm:ss").withOption('width', '15%')
        ])
        .setColActions(actionsHtml, '60px')
        .setIdAreaHeader("datatable_header")
        .setAutoInit()
        .setActionSave(Europa.Controllers.RegraComissaoPadrao.SalvarEdicao)
        .setDefaultOptions('POST', Europa.Controllers.RegraComissaoPadrao.UrlListar, Europa.Controllers.RegraComissaoPadrao.FilterParams);

    function actionsHtml(data, type, full, meta) {
        var html = '<div>' + Europa.Controllers.RegraComissaoPadrao.Tabela.renderButton(Europa.Controllers.RegraComissaoPadrao.Permissoes.Incluir, Europa.i18n.Messages.IncluirNovoAPartir, "fa fa-plus", "NovoPorReferencia(" + meta.row + ")")
            + Europa.Controllers.RegraComissaoPadrao.Tabela.renderButton(Europa.Controllers.RegraComissaoPadrao.Permissoes.Atualizar, Europa.i18n.Messages.Editar, "fa fa-edit", "Editar(" + meta.row + ")");
        if (full.Situacao === 0 && full.IdArquivo !== 0) {
            html = html + Europa.Controllers.RegraComissaoPadrao.Tabela.renderButton(Europa.Controllers.RegraComissaoPadrao.Permissoes.Liberar, Europa.i18n.Messages.Liberar, "fa fa-unlock", "Liberar(" + meta.row + ")");
        }
        if (full.Situacao !== 0) {
            html = html + Europa.Controllers.RegraComissaoPadrao.Tabela.renderButton(Europa.Controllers.RegraComissaoPadrao.Permissoes.Visualizar, Europa.i18n.Messages.Visualizar, "fa fa-eye", "Detalhar(" + data.Id + ")");
        }
        if (full.IdArquivo !== 0) {
            html = html + Europa.Controllers.RegraComissaoPadrao.Tabela.renderButton(Europa.Controllers.RegraComissaoPadrao.Permissoes.DownloadRegraComissaoPadrao, Europa.i18n.Messages.Download, "fa fa-download", "Download(" + data.Id + ")");
        }
        html += '</div>';
        
        return html;
    }

    $scope.Editar = function (row) {
        /*$scope.rowEdit(row);
        var obj = Europa.Controllers.RegraComissaoPadrao.Tabela.getRowData(row);
        $("#Situacao").val(Europa.i18n.Enum.Resolve("SituacaoRegraComissaoPadrao", obj.Situacao));
        $("#InicioVigencia").val(Europa.Date.Format(Europa.Date.Parse(obj.InicioVigencia), 'DD/MM/YYYY HH:mm:ss'));
        $("#TerminoVigencia").val(Europa.Date.Format(Europa.Date.Parse(obj.TerminoVigencia), 'DD/MM/YYYY HH:mm:ss'));*/
        var obj = Europa.Controllers.RegraComissaoPadrao.Tabela.getRowData(row);
        location.href = Europa.Controllers.RegraComissaoPadrao.UrlMatriz + "?regra=" + obj.Id;
    };

    $scope.NovoPorReferencia = function (row) {
        var obj = Europa.Controllers.RegraComissaoPadrao.Tabela.getRowData(row);
        Europa.Controllers.ModalNovaRegraComissaoPadrao.Abrir({Id: obj.Id, Regional: obj.Regional});
    };

    $scope.Liberar = function (row) {
        var obj = Europa.Controllers.RegraComissaoPadrao.Tabela.getRowData(row);
        Europa.Controllers.RegraComissaoPadrao.Liberar(obj);
    };

    $scope.Detalhar = function (idRegra) {
        location.href = Europa.Controllers.RegraComissaoPadrao.UrlMatriz + "?regra=" + idRegra;
    };

    $scope.Download = function (idRegra) {
        location.href = Europa.Controllers.RegraComissaoPadrao.UrlDownloadPdf + "?idRegra=" + idRegra;
    };
}


Europa.Controllers.RegraComissaoPadrao.FilterParams = function () {
    var filtro = {
        Descricao: $('#filtro_descricao', Europa.Controllers.RegraComissaoPadrao.IdFormFiltro).val(),
        Situacoes: $('#filtro_situacoes', Europa.Controllers.RegraComissaoPadrao.IdFormFiltro).val(),
        Regionais: $('#filtro_regional', Europa.Controllers.RegraComissaoPadrao.IdFormFiltro).val(),
        VigenteEm: $('#VigenteEm', Europa.Controllers.RegraComissaoPadrao.IdFormFiltro).val()
    };
    return filtro;
};

Europa.Controllers.RegraComissaoPadrao.Filtrar = function () {
    Europa.Controllers.RegraComissaoPadrao.Tabela.reloadData();
};

Europa.Controllers.RegraComissaoPadrao.LimparFiltro = function () {
    $('#filtro_descricao', Europa.Controllers.RegraComissaoPadrao.IdFormFiltro).val("");
    $('#filtro_situacoes', Europa.Controllers.RegraComissaoPadrao.IdFormFiltro).val("").trigger('change');
    $('#filtro_regional', Europa.Controllers.RegraComissaoPadrao.IdFormFiltro).val("").trigger('change');
    $('#VigenteEm', Europa.Controllers.RegraComissaoPadrao.IdFormFiltro).val("");
};

Europa.Controllers.RegraComissaoPadrao.Incluir = function () {
    Europa.Controllers.RegraComissaoPadrao.AbrirModal();
};

Europa.Controllers.RegraComissaoPadrao.SalvarInclusao = function () {
    Europa.Validator.ClearForm(Europa.Controllers.RegraComissaoPadrao.IdFormModal);
    var arquivo = $("#Arquivo", Europa.Controllers.RegraComissaoPadrao.IdFormModal).get(0).files[0];
    var formData = new FormData();
    formData.append("Descricao", $("#Descricao", Europa.Controllers.RegraComissaoPadrao.IdFormModal).val());
    formData.append("Arquivo", arquivo);
    formData.append("Regional", $("#Regional").val());

    $.ajax({
        type: 'POST',
        url: Europa.Controllers.RegraComissaoPadrao.UrlIncluir,
        data: formData,
        contentType: false,
        processData: false,
        success: function (res) {
            if (res.Sucesso) {
                Europa.Controllers.RegraComissaoPadrao.Tabela.reloadData();
                Europa.Controllers.RegraComissaoPadrao.FecharModal();
            } else {
                Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.RegraComissaoPadrao.IdFormModal);
            }
            Europa.Informacao.PosAcao(res);
        }
    });
};

Europa.Controllers.RegraComissaoPadrao.Editar = function (row) {
    Europa.Controllers.RegraComissaoPadrao.Tabela.rowEdit(row);
};

Europa.Controllers.RegraComissaoPadrao.SalvarEdicao = function () {
    Europa.Controllers.RegraComissaoPadrao.LimparErro();
    var obj = Europa.Controllers.RegraComissaoPadrao.Tabela.getDataRowEdit();
    $.post(Europa.Controllers.RegraComissaoPadrao.UrlAtualizar, {model: obj}, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.RegraComissaoPadrao.Tabela.closeEdition();
            Europa.Controllers.RegraComissaoPadrao.Tabela.reloadData();
        } else {
            if (res.Campos !== undefined) {
                Europa.Controllers.RegraComissaoPadrao.AdicionarErro(res.Campos);
            }
        }
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.RegraComissaoPadrao.Liberar = function (objeto) {
    Europa.Confirmacao.ChangeHeader(Europa.i18n.Messages.Confirmacao);
    Europa.Confirmacao.ChangeContent(Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoLiberacaoRegraComissaoPadrao, objeto.Descricao));
    Europa.Confirmacao.ConfirmCallback = function () {
        $.post(Europa.Controllers.RegraComissaoPadrao.UrlLiberar, {idRegra: objeto.Id}, function (res) {
            if (res.Sucesso) {
                Europa.Controllers.RegraComissaoPadrao.Tabela.reloadData();
            }
            Europa.Informacao.PosAcao(res);
        });
    };
    Europa.Confirmacao.Show();
};

Europa.Controllers.RegraComissaoPadrao.DownloadRegraComissaoPadrao = function (idRegra) {
    var formDownload = $("#form_download_regra_comissao");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.RegraComissaoPadrao.UrlDownloadRegraComissaoPadrao);
    formDownload.addHiddenInputData({idRegra: idRegra});
    formDownload.submit();
};

Europa.Controllers.RegraComissaoPadrao.AbrirModal = function () {
    $("#Descricao", Europa.Controllers.RegraComissaoPadrao.IdFormModal).val("");
    $("#Arquivo", Europa.Controllers.RegraComissaoPadrao.IdFormModal).val("");
    $('#Regional').val("").trigger('change');
    $("#modal_inclusao_regra_comissao").modal("show");
};

Europa.Controllers.RegraComissaoPadrao.FecharModal = function () {
    $("#modal_inclusao_regra_comissao").modal("hide");
};

Europa.Controllers.RegraComissaoPadrao.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.RegraComissaoPadrao.LimparErro = function () {
    $("[name='Descricao']").parent().removeClass("has-error");
    $("[name='Regional']").parent().removeClass("has-error");
};