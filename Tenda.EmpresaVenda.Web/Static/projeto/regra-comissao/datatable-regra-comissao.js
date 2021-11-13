Europa.Controllers.RegraComissao.IdFormFiltro = "#form_filtro";
Europa.Controllers.RegraComissao.IdFormModal = "#form_inclusao";

$(function () {
    $("#filtro_tipo_regra_comissao", Europa.Controllers.RegraComissao.IdFormFiltro).select2({
        trags: true,
        width: '100%'
    });

    $("#filtro_situacoes", Europa.Controllers.RegraComissao.IdFormFiltro).select2({
        trags: true,
        width: '100%',
        value: 1
    });
    $("#filtro_regional").select2();

    $("#Regional").select2();

    Europa.Controllers.RegraComissao.AutoCompleteEmpresaVenda = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("filtro_empresa_venda").Configure();

    Europa.Components.DatePicker.AutoApply();
    Europa.Controllers.RegraComissao.LimparFiltro();
});


DataTableApp.controller('RegraComissaoTable', RegraComissaoTable);

function RegraComissaoTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.RegraComissao.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.RegraComissao.Tabela;
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
            DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption('width', '100px'),
            DTColumnBuilder.newColumn('NomeEmpresaVenda').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption('width', '200px'),
            DTColumnBuilder.newColumn('Cnpj').withTitle(Europa.i18n.Messages.Cnpj).renderWith(formatCNPJ).withOption('width', '200px'),
            DTColumnBuilder.newColumn('Codigo').withTitle(Europa.i18n.Messages.CodigoRegra).withOption('width', '150px'),
            DTColumnBuilder.newColumn('Descricao').withTitle(Europa.i18n.Messages.Descricao).withOption('width', '250px'),
            DTColumnBuilder.newColumn('InicioVigencia').withTitle(Europa.i18n.Messages.InicioVigencia).withOption("type", "date-format-DD/MM/YYYY HH:mm:ss").withOption('width', '150px'),
            DTColumnBuilder.newColumn('TerminoVigencia').withTitle(Europa.i18n.Messages.TerminoVigencia).withOption("type", "date-format-DD/MM/YYYY HH:mm:ss").withOption('width', '150px'),
            DTColumnBuilder.newColumn('DataAceite').withTitle(Europa.i18n.Messages.DataAceite).withOption("type", "date-format-DD/MM/YYYY HH:mm:ss").withOption('width', '150px'),
            DTColumnBuilder.newColumn('Aprovador').withTitle(Europa.i18n.Messages.Aprovador).withOption('width', '250px'),
            DTColumnBuilder.newColumn('TipoRegraComissao').withTitle(Europa.i18n.Messages.Tipo).withOption('type', 'enum-format-TipoRegraComissao').withOption('width', '150px'),
            DTColumnBuilder.newColumn('SituacaoEvs').withTitle(Europa.i18n.Messages.Situacao).withOption('type', 'enum-format-SituacaoRegraComissao').withOption('width', '150px')
        ])
        .setColActions(actionsHtml, '100px')
        .setIdAreaHeader("datatable_header")
        .setAutoInit(false)
        .setActionSave(Europa.Controllers.RegraComissao.SalvarEdicao)
        .setDefaultOptions('POST', Europa.Controllers.RegraComissao.UrlListar, Europa.Controllers.RegraComissao.FilterParams);

    function actionsHtml(data, type, full, meta) {
        var html = '<div>' + Europa.Controllers.RegraComissao.Tabela.renderButton(Europa.Controllers.RegraComissao.Permissoes.Incluir, Europa.i18n.Messages.IncluirNovoAPartir, "fa fa-plus", "NovoPorReferencia(" + meta.row + ")");
        if (full.SituacaoRc !== 0) {
            html = html + Europa.Controllers.RegraComissao.Tabela.renderButton(Europa.Controllers.RegraComissao.Permissoes.Visualizar, Europa.i18n.Messages.Visualizar, "fa fa-eye", "Editar(" + meta.row + ")");
        }

        if (full.SituacaoRc === 0) {
            html = html + Europa.Controllers.RegraComissao.Tabela.renderButton(Europa.Controllers.RegraComissao.Permissoes.Atualizar, Europa.i18n.Messages.Editar, "fa fa-edit", "Editar(" + meta.row + ")");
        }
        if (full.SituacaoRc === 0 && full.IdArquivo !== 0) {
            html = html + Europa.Controllers.RegraComissao.Tabela.renderButton(Europa.Controllers.RegraComissao.Permissoes.Liberar, Europa.i18n.Messages.Liberar, "fa fa-unlock", "Liberar(" + meta.row + ")");
        }
        if (full.SituacaoRc !== 0) {
            html = html + Europa.Controllers.RegraComissao.Tabela.renderButton(Europa.Controllers.RegraComissao.Permissoes.Detalhar, "Detalhar " + Europa.i18n.Messages.Aceites, "fa fa-check", "Detalhar(" + data.IdRegraComissao + ")");
        }
        if (full.IdArquivo !== 0) {
            html = html + Europa.Controllers.RegraComissao.Tabela.renderButton(Europa.Controllers.RegraComissao.Permissoes.DownloadRegraComissao, Europa.i18n.Messages.Download, "fa fa-download", "Download(" + data.IdRegraComissao + "," + data.IdEmpresaVenda + ",'" + data.NomeEmpresaVenda +"')");
        }
        if (full.SituacaoRc === 0) {
            html = html + Europa.Controllers.RegraComissao.Tabela.renderButton(Europa.Controllers.RegraComissao.Permissoes.Excluir, Europa.i18n.Messages.Excluir, "fa fa-trash", "Excluir(" + meta.row + ")");
        }
        html += '</div>';
        
        return html;
    }

    $scope.Editar = function (row) {
        /*$scope.rowEdit(row);
        var obj = Europa.Controllers.RegraComissao.Tabela.getRowData(row);
        $("#Situacao").val(Europa.i18n.Enum.Resolve("SituacaoRegraComissao", obj.Situacao));
        $("#InicioVigencia").val(Europa.Date.Format(Europa.Date.Parse(obj.InicioVigencia), 'DD/MM/YYYY HH:mm:ss'));
        $("#TerminoVigencia").val(Europa.Date.Format(Europa.Date.Parse(obj.TerminoVigencia), 'DD/MM/YYYY HH:mm:ss'));*/
        var obj = Europa.Controllers.RegraComissao.Tabela.getRowData(row);

        //location.href = Europa.Controllers.RegraComissao.UrlMatriz + "?regra=" + obj.IdRegraComissao + "&IdEmpresaVenda=" + obj.IdEmpresaVenda;
        var url = Europa.Controllers.RegraComissao.UrlMatriz + "?regra=" + obj.IdRegraComissao + "&IdEmpresaVenda=" + obj.IdEmpresaVenda;
        window.open(url, "_blank");
    };

    $scope.VisualizarTodos = function (row) {
        var obj = Europa.Controllers.RegraComissao.Tabela.getRowData(row);
        var url = Europa.Controllers.RegraComissao.UrlMatriz + "?regra=" + obj.IdRegraComissao;
        window.open(url, "_blank");
    }

    $scope.NovoPorReferencia = function (row) {
        var obj = Europa.Controllers.RegraComissao.Tabela.getRowData(row);
        Europa.Controllers.ModalNovaRegraComissao.Abrir({ Id: obj.IdRegraComissao, Regional: obj.Regional});
    };

    $scope.Liberar = function (row) {
        var obj = Europa.Controllers.RegraComissao.Tabela.getRowData(row);
        Europa.Controllers.RegraComissao.Liberar(obj);
    };

    $scope.Detalhar = function (idRegra) {
        window.location.href = Europa.Controllers.RegraComissao.UrlDetalhar + '/' + idRegra;
    };

    $scope.Download = function (idRegra, idEv, nomeFantasia) {
        Europa.Controllers.ModalDownloadPdfRegraComissao.AbrirEv(idRegra,idEv,nomeFantasia);
    };

    function formatCNPJ(data, type, full) {
        return Europa.Mask.GetMaskedValue(data, Europa.Mask.FORMAT_CNPJ);
    }

    $scope.Excluir = function (row) {
        var objetoLinha = Europa.Controllers.RegraComissao.Tabela.getRowData(row);
        Europa.Confirmacao.PreAcaoV2(
            Europa.i18n.Messages.Excluir,
            String.format(Europa.i18n.Messages.ExcluirRegraComissao, objetoLinha.Descricao),
            Europa.i18n.Messages.Confirmar,
            function () {
                Europa.Controllers.RegraComissao.ExcluirRegraComissao(objetoLinha.IdRegraComissao);
            });
    };

    function renderEnum(data) {
        console.log(data)
        return Europa.i18n.Enum.Resolve("SituacaoRegraComissao", data);
        
        return "";
    }
};

Europa.Controllers.RegraComissao.ExcluirRegraComissao = function (idRegra) {
    $.post(Europa.Controllers.RegraComissao.UrlExcluir, { idRegra: idRegra }, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.RegraComissao.Tabela.reloadData();
        }
        Europa.Informacao.PosAcao(res);
    });
};


Europa.Controllers.RegraComissao.FilterParams = function () {
    var filtro = {
        Descricao: $('#filtro_descricao', Europa.Controllers.RegraComissao.IdFormFiltro).val(),
        Situacoes: $('#filtro_situacoes', Europa.Controllers.RegraComissao.IdFormFiltro).val(),
        Regionais: $('#filtro_regional', Europa.Controllers.RegraComissao.IdFormFiltro).val(),
        VigenteEm: $('#VigenteEm', Europa.Controllers.RegraComissao.IdFormFiltro).val(),
        IdEmpresaVenda: $('#autocomplete_filtro_empresa_venda', Europa.Controllers.RegraComissao.IdFormFiltro).val(),
        Tipos: $('#filtro_tipo_regra_comissao', Europa.Controllers.RegraComissao.IdFormFiltro).val(),
        CodigoRegra: $('#filtro_codigo_regra', Europa.Controllers.RegraComissao.IdFormFiltro).val()
    };
    return filtro;
};

Europa.Controllers.RegraComissao.ValidarFiltro = function () {
    if (Europa.Controllers.RegraComissao.FilterParams().Regionais.length === 0 || Europa.Controllers.RegraComissao.FilterParams().Situacoes.length === 0) {
        var res = {
            Sucesso: false,
            Mensagens: ["Filtros Regional e Situação são obrigatórios"]
        };
        Europa.Informacao.PosAcao(res);
        return false;
    }
    return true;
};
Europa.Controllers.RegraComissao.Filtrar = function () {
    if (Europa.Controllers.RegraComissao.ValidarFiltro()){
        Europa.Controllers.RegraComissao.Tabela.reloadData();
    }
};

Europa.Controllers.RegraComissao.LimparFiltro = function () {
    $('#filtro_descricao', Europa.Controllers.RegraComissao.IdFormFiltro).val("");
    $('#filtro_situacoes', Europa.Controllers.RegraComissao.IdFormFiltro).val("").trigger('change');
    $('#filtro_regional', Europa.Controllers.RegraComissao.IdFormFiltro).val("").trigger('change');
    $('#VigenteEm', Europa.Controllers.RegraComissao.IdFormFiltro).val("");
    Europa.Controllers.RegraComissao.AutoCompleteEmpresaVenda.Clean();
    $('#filtro_tipo_regra_comissao', Europa.Controllers.RegraComissao.IdFormFiltro).val("").trigger('change');
};

Europa.Controllers.RegraComissao.Incluir = function () {
    Europa.Controllers.RegraComissao.AbrirModal();
};

Europa.Controllers.RegraComissao.SalvarInclusao = function () {
    Europa.Validator.ClearForm(Europa.Controllers.RegraComissao.IdFormModal);
    var arquivo = $("#Arquivo", Europa.Controllers.RegraComissao.IdFormModal).get(0).files[0];
    var formData = new FormData();
    formData.append("Descricao", $("#Descricao", Europa.Controllers.RegraComissao.IdFormModal).val());
    formData.append("Arquivo", arquivo);
    formData.append("Regional", $("#Regional").val());

    $.ajax({
        type: 'POST',
        url: Europa.Controllers.RegraComissao.UrlIncluir,
        data: formData,
        contentType: false,
        processData: false,
        success: function (res) {
            if (res.Sucesso) {
                Europa.Controllers.RegraComissao.Tabela.reloadData();
                Europa.Controllers.RegraComissao.FecharModal();
            } else {
                Europa.Validator.InvalidateList(res.Campos, Europa.Controllers.RegraComissao.IdFormModal);
            }
            Europa.Informacao.PosAcao(res);
        }
    });
};

Europa.Controllers.RegraComissao.Editar = function (row) {
    Europa.Controllers.RegraComissao.Tabela.rowEdit(row);
};

Europa.Controllers.RegraComissao.SalvarEdicao = function () {
    Europa.Controllers.RegraComissao.LimparErro();
    var obj = Europa.Controllers.RegraComissao.Tabela.getDataRowEdit();
    $.post(Europa.Controllers.RegraComissao.UrlAtualizar, {model: obj}, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.RegraComissao.Tabela.closeEdition();
            Europa.Controllers.RegraComissao.Tabela.reloadData();
        } else {
            if (res.Campos !== undefined) {
                Europa.Controllers.RegraComissao.AdicionarErro(res.Campos);
            }
        }
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.RegraComissao.Liberar = function (objeto) {
    Europa.Confirmacao.ChangeHeader(Europa.i18n.Messages.Confirmacao);
    Europa.Confirmacao.ChangeContent(Europa.String.Format(Europa.i18n.Messages.MsgConfirmacaoLiberacaoRegraComissao, objeto.Descricao));
    Europa.Confirmacao.ConfirmCallback = function () {
        $.post(Europa.Controllers.RegraComissao.UrlLiberar, { idRegra: objeto.IdRegraComissao }, function (res) {
            if (res.Sucesso) {
                Europa.Controllers.RegraComissao.Tabela.reloadData();
            }
            Europa.Informacao.PosAcao(res);
        });
    };
    Europa.Confirmacao.Show();
};

Europa.Controllers.RegraComissao.DownloadRegraComissao = function (idRegra) {
    var formDownload = $("#form_download_regra_comissao");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.RegraComissao.UrlDownloadRegraComissao);
    formDownload.addHiddenInputData({idRegra: idRegra});
    formDownload.submit();
};

Europa.Controllers.RegraComissao.AbrirModal = function () {
    $("#Descricao", Europa.Controllers.RegraComissao.IdFormModal).val("");
    $("#Arquivo", Europa.Controllers.RegraComissao.IdFormModal).val("");
    $('#Regional').val("").trigger('change');
    $("#modal_inclusao_regra_comissao").modal("show");
};

Europa.Controllers.RegraComissao.FecharModal = function () {
    $("#modal_inclusao_regra_comissao").modal("hide");
};

Europa.Controllers.RegraComissao.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.RegraComissao.LimparErro = function () {
    $("[name='Descricao']").parent().removeClass("has-error");
    $("[name='Regional']").parent().removeClass("has-error");
};

Europa.Controllers.RegraComissao.ExportarTodos = function () {
    if (!Europa.Controllers.RegraComissao.ValidarFiltro()) {
        return;
    }

    var params = Europa.Controllers.RegraComissao.Tabela.lastRequestParams;
    var formDownload = $("#Exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.RegraComissao.UrlExportarTodos);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
}

Europa.Controllers.RegraComissao.ExportarPagina = function () {
    if (!Europa.Controllers.RegraComissao.ValidarFiltro()) {
        return;
    }

    var params = Europa.Controllers.RegraComissao.Tabela.lastRequestParams;
    var formDownload = $("#Exportar");
    formDownload.find("input").remove();
    formDownload.attr("method", "post").attr("action", Europa.Controllers.RegraComissao.UrlExportarPagina);
    formDownload.addHiddenInputData(params);
    formDownload.submit();
}