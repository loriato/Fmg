Europa.Controllers.EmpresaVenda.CountDoc = 1;
Europa.Controllers.EmpresaVenda.Documentos = [];
Europa.Controllers.EmpresaVenda.ListaDatatable = [];
Europa.Controllers.EmpresaVenda.TabelaDocumentosPreCarregados = {};
Europa.Controllers.EmpresaVenda.TabelaDocumentosSalvos = {};

$(function () {
    Europa.Controllers.EmpresaVenda.MontarDropDown();
});


DataTableApp.controller('documentoEmpresaVendaTable', documentoEmpresaVendaTable);

function documentoEmpresaVendaTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

    Europa.Controllers.EmpresaVenda.TabelaDocumentosPreCarregados = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.EmpresaVenda.TabelaDocumentosPreCarregados;
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('NomeArquivo').withTitle(Europa.i18n.Messages.NomeArquivo).withOption('width', '15%'),
        DTColumnBuilder.newColumn('NomeTipoDocumento').withTitle(Europa.i18n.Messages.TipoDocumento).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Status).withOption('width', '10%').withOption('type', 'enum-format-SituacaoAprovacaoDocumento'),
    ])
        .setIdAreaHeader("documento_empresa_venda_datatable_header")
        .setColActions(actionsHtml, '5%')
        .setAutoInit(false)
        .setDefaultOptions('POST', Europa.Controllers.EmpresaVenda.UrlListarDocumentosPreCarregados, Europa.Controllers.EmpresaVenda.FilterParams);

    function actionsHtml(data, type, full, meta) {
        var div = '<div>';
        div += $scope.renderButtonDelete(Europa.i18n.Messages.Excluir, "fa fa-trash", "excluir(" + meta.row + ")");
        div += '</div>';
        return div;
    };

    $scope.renderButtonDelete = function (title, icon, onClick) {

        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .attr('style', 'margin-right:14px')
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.excluir = function (row) {
        var rowData = Europa.Controllers.EmpresaVenda.TabelaDocumentosPreCarregados.getRowData(row);

        Europa.Controllers.EmpresaVenda.ExcluirArquivoPreCarregado(rowData.Id);

    };
}

Europa.Controllers.EmpresaVenda.FilterParams = function () {
    return { lista: Europa.Controllers.EmpresaVenda.ListaDatatable };
};

Europa.Controllers.EmpresaVenda.MontarDropDown = function () {
    $.get(Europa.Controllers.EmpresaVenda.UrlListarTiposDocumentoEmpresaVenda, function (res) {
        if (res.Sucesso) {
            var lista = res.Objeto;

            if (lista.size == 0) {
                return
            }

            var options = '<option value="">Selecione...</option>';

            lista.forEach(function (obj) {
                options += '<option value="' + obj.Id + '">' + obj.Nome + '</option>';
            });

            $("#TiposDocumentoEmpresaVenda").html(options)

            return;
        }
        Europa.Informacao.PosAcao(res);
    });
}

Europa.Controllers.EmpresaVenda.PrepararUploadEmpresaVenda = function () {
    var documento = $("#documentos_empresa_venda").find("#arquivoEmpresaVenda").get(0).files[0];
    var tipoDocumento = $("#TiposDocumentoEmpresaVenda").val();

    if (documento == undefined) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.CampoObrigatorio, "Documento"));
        Europa.Informacao.Show();
        return;
    }

    if (tipoDocumento == "") {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.CampoObrigatorio, "Tipo de Documento"));
        Europa.Informacao.Show();
        return;
    }

    var maxFileSize = 16;
    var maxFileSizeInBytes = maxFileSize * 1000000;
    if (documento !== undefined && documento.size > maxFileSizeInBytes) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.ArquivoTamanhoMaximoExcedido, maxFileSize));
        Europa.Informacao.Show();
        return;
    }

    var nomeTipoDocumento = $("#TiposDocumentoEmpresaVenda option:selected").text();

    var dataAux = {
        Id: Europa.Controllers.EmpresaVenda.CountDoc,
        NomeTipoDocumento: nomeTipoDocumento,
        Situacao: 7,
        NomeArquivo: documento.name,
        IdTipoDocumento: tipoDocumento,
        File: documento
    };

    Europa.Controllers.EmpresaVenda.Documentos.push(dataAux);

    if (Europa.Controllers.EmpresaVenda.IdEmpresaVenda == undefined) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Sucesso, Europa.String.Format("Documento {0} carregado", documento.name));
        Europa.Informacao.Show();
    };

    var registro = {
        Id: Europa.Controllers.EmpresaVenda.CountDoc,
        NomeTipoDocumento: nomeTipoDocumento,
        Situacao: 7,
        NomeArquivo: documento.name,
        IdTipoDocumento: tipoDocumento
    }

    Europa.Controllers.EmpresaVenda.ListaDatatable.push(registro);

    $("#documentos_empresa_venda").find("#arquivoEmpresaVenda").val("");
    $("#TiposDocumentoEmpresaVenda").val("");

    Europa.Controllers.EmpresaVenda.CountDoc++;
        
    Europa.Controllers.EmpresaVenda.FiltrarDocumentos();
}

Europa.Controllers.EmpresaVenda.ExcluirArquivoPreCarregado = function (id) {
    Europa.Controllers.EmpresaVenda.ListaDatatable.pop(x => x.Id == id);
    Europa.Controllers.EmpresaVenda.Documentos.pop(x => x.Id == id);
    Europa.Controllers.EmpresaVenda.TabelaDocumentosPreCarregados.reloadData();
}

Europa.Controllers.EmpresaVenda.FiltrarDocumentos = function () {
    $("#documentos-pre-carregados").addClass("hidden");
    $("#documentos-salvos").addClass("hidden");

    if (Europa.Controllers.EmpresaVenda.IdEmpresaVenda != undefined) {
        Europa.Controllers.EmpresaVenda.TabelaDocumentosSalvos.reloadData();
        $("#documentos-salvos").removeClass("hidden");
        return;
    }

    $("#documentos-pre-carregados").removeClass("hidden");

    Europa.Controllers.EmpresaVenda.TabelaDocumentosPreCarregados.reloadData();
}

Europa.Controllers.EmpresaVenda.ObjectToFormData = function (formData, data, parentKey) {
    if (data && typeof data === 'object' && !(data instanceof Date) && !(data instanceof File) && !(data instanceof Blob)) {
        Object.keys(data).forEach(key => {
            Europa.Controllers.EmpresaVenda.ObjectToFormData(formData, data[key], parentKey ? `${parentKey}[${key}]` : key);
        });
    } else {
        const value = data == null ? '' : data;
        formData.append(parentKey, value);
    }
}

//Documentos Salvos
DataTableApp.controller('documentoSalvoEmpresaVendaTable', documentoSalvoEmpresaVendaTable);

function documentoSalvoEmpresaVendaTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

    Europa.Controllers.EmpresaVenda.TabelaDocumentosSalvos = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.EmpresaVenda.TabelaDocumentosSalvos;
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('NomeArquivo').withTitle(Europa.i18n.Messages.NomeArquivo).withOption('width', '15%'),
        DTColumnBuilder.newColumn('NomeTipoDocumento').withTitle(Europa.i18n.Messages.TipoDocumento).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Status).withOption('width', '10%').withOption('type', 'enum-format-SituacaoAprovacaoDocumento'),

    ])
        .setIdAreaHeader("documento_salvo_empresa_venda_datatable_header")
        .setColActions(actionsHtml, '5%')
        .setAutoInit(false)
        .setDefaultOptions('POST', Europa.Controllers.EmpresaVenda.UrlListarDocumentosEmpresaVenda, Europa.Controllers.EmpresaVenda.FiltroDocumentos);

    function actionsHtml(data, type, full, meta) {
        var div = '<div>';
        div += $scope.renderButton(Europa.Controllers.EmpresaVenda.Permissoes.Excluir, Europa.i18n.Messages.Excluir, "fa fa-trash", "Excluir(" + full.Id + ")");
        div += $scope.renderButton(Europa.Controllers.EmpresaVenda.Permissoes.Visualizar, Europa.i18n.Messages.Download, "fa fa-download", "Download(" + full.IdArquivo + ")");
        div += '</div>';
        return div;
    };

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (hasPermission == false) {
            return "";
        }

        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .attr('style', 'margin-right:5px')
            .append(icon);
        return button.prop('outerHTML');
    }

    $scope.renderButtonDelete = function (hasPermission,title, icon, onClick) {

        if (hasPermission == false) {
            return "";
        }

        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .attr('style', 'margin-right:14px')
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.Excluir = function (idDocumento) {
        Europa.Controllers.EmpresaVenda.ExcluirDocumentoEmpresaVenda(idDocumento);
    };

    $scope.Download = function(idArquivo){
        Europa.Controllers.EmpresaVenda.DownloadDocumentoEmpresaVenda(idArquivo);
    };
}

Europa.Controllers.EmpresaVenda.FiltroDocumentos = function () {
    return {
        IdEmpresaVenda: Europa.Controllers.EmpresaVenda.IdEmpresaVenda
    };
};

Europa.Controllers.EmpresaVenda.ExcluirDocumentoEmpresaVenda = function (idDocumento) {
    Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.Atencao,
        "Confirmar exclusão?", Europa.i18n.Messages.Confirmar, function () {
            $.post(Europa.Controllers.EmpresaVenda.UrlExcluirDocumentoEmpresaVenda,
                { idDocumento: idDocumento }, function (res) {
                    if (res.Sucesso) {
                        Europa.Controllers.EmpresaVenda.FiltrarDocumentos();
                        Europa.Controllers.EmpresaVenda.TabelaResponsavelAceiteRegraComissao.reloadData();
                    }

                    Europa.Informacao.PosAcao(res);
                });
        });
};

Europa.Controllers.EmpresaVenda.DownloadDocumentoEmpresaVenda = function (idArquivo) {
    location.href = Europa.Controllers.EmpresaVenda.UrlDownloadDocumentoEmpresaVenda + "?idArquivo=" + idArquivo;
};

//Empresa de venda salva
Europa.Controllers.EmpresaVenda.UploadDocumento = function () {
    var documento = $("#documentos_empresa_venda").find("#arquivoEmpresaVenda").get(0).files[0];
    var tipoDocumento = $("#TiposDocumentoEmpresaVenda").val();

    if (documento == undefined) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.CampoObrigatorio, "Documento"));
        Europa.Informacao.Show();
        return;
    }

    if (tipoDocumento == "") {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.CampoObrigatorio, "Tipo de Documento"));
        Europa.Informacao.Show();
        return;
    }

    var maxFileSize = 16;
    var maxFileSizeInBytes = maxFileSize * 1000000;
    if (documento !== undefined && documento.size > maxFileSizeInBytes) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, Europa.String.Format(Europa.i18n.Messages.ArquivoTamanhoMaximoExcedido, maxFileSize));
        Europa.Informacao.Show();
        return;
    }

    var formData = new FormData();

    formData.append("IdEmpresaVenda", Europa.Controllers.EmpresaVenda.IdEmpresaVenda);
    formData.append("File", documento);
    formData.append("IdTipoDocumento", tipoDocumento);
    formData.append("NomeArquivo", documento.name);

    $.ajax({
        type: 'POST',
        url: Europa.Controllers.EmpresaVenda.UrlUploadDocumentoEmpresaVenda,
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
        success: function (res) {
            if (res.Sucesso) {
                $("#documentos_empresa_venda").find("#arquivoEmpresaVenda").val("");
                $("#TiposDocumentoEmpresaVenda").val("");

                Europa.Controllers.EmpresaVenda.FiltrarDocumentos();
            }
            Europa.Informacao.PosAcao(res);
        }
    });
}

Europa.Controllers.EmpresaVenda.LiberarBotoes = function () {

    $("#btn_upload_documento").removeClass("hidden");
    $("#btn_adicionar_documento").addClass("hidden");

    $("#documentos-pre-carregados").addClass("hidden");
    $("#documentos-salvos").removeClass("hidden");

    if (Europa.Controllers.EmpresaVenda.IdEmpresaVenda == undefined) {
        $("#btn_adicionar_documento").removeClass("hidden");
        $("#btn_upload_documento").addClass("hidden");

        $("#documentos-pre-carregados").removeClass("hidden");
        $("#documentos-salvos").addClass("hidden");
    }
}
