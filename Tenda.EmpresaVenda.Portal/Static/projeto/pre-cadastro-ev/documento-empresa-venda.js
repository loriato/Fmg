Europa.Controllers.PreCadastroEv.UrlListarDocumentos = undefined;
Europa.Controllers.PreCadastroEv.Documentos = [];
Europa.Controllers.PreCadastroEv.ViewDocumentos = [];
Europa.Controllers.PreCadastroEv.ListaDatatable = [];
Europa.Controllers.PreCadastroEv.CountDoc = 1;

$(function () {
    Europa.Controllers.PreCadastroEv.MontarDropDown();
});

//Datatable

DataTableApp.controller('documentoEmpresaVendaTable', documentoEmpresaVendaTable);

function documentoEmpresaVendaTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

    Europa.Controllers.PreCadastroEv.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.PreCadastroEv.Tabela;
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('NomeArquivo').withTitle(Europa.i18n.Messages.NomeArquivo).withOption('width', '15%'),
        DTColumnBuilder.newColumn('NomeTipoDocumento').withTitle(Europa.i18n.Messages.TipoDocumento).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Status).withOption('width', '10%').withOption('type', 'enum-format-SituacaoAprovacaoDocumento'),
        DTColumnBuilder.newColumn('Situacao').withTitle("").withOption('width', '10%').renderWith(actionsHtml)

    ])
        .setIdAreaHeader("documento_empresa_venda_datatable_header")
        .setDefaultOrder([[3, 'asc']])
        .setDefaultOptions('POST', Europa.Controllers.PreCadastroEv.UrlListarDocumentos, Europa.Controllers.PreCadastroEv.FilterParams);

    function actionsHtml(data, type, full, meta) {
        var div = '<div>';
        div += $scope.renderButtonDelete(Europa.i18n.Messages.Excluir, "fa fa-trash", "excluir(" + meta.row + ")");
        div += '</div>';
        return div;
    };    

    $scope.renderButtonDelete = function (title, icon, onClick) {

        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-steel')
            .attr('title', title)
            .attr('ng-click', onClick)
            .attr('style', 'margin-right:14px')
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.excluir = function (row) {
        var rowData = Europa.Controllers.PreCadastroEv.Tabela.getRowData(row);

        Europa.Controllers.PreCadastroEv.ExcluirArquivoPreCarregado(rowData.Id);

    };
}

Europa.Controllers.PreCadastroEv.FilterParams = function () {
    return { lista: Europa.Controllers.PreCadastroEv.ListaDatatable };
};

Europa.Controllers.PreCadastroEv.ExcluirArquivoPreCarregado = function (id) {
    Europa.Controllers.PreCadastroEv.ListaDatatable.pop(x => x.Id == id);
    Europa.Controllers.PreCadastroEv.Documentos.pop(x => x.Id == id);
    Europa.Controllers.PreCadastroEv.Tabela.reloadData();
}
//Datatable

Europa.Controllers.PreCadastroEv.MontarDropDown = function () {
    $.get(Europa.Controllers.PreCadastroEv.UrlListarTiposDocumentoEmpresaVenda, function (res) {
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

Europa.Controllers.PreCadastroEv.PrepararUploadEmpresaVenda = function () {
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
        Id: Europa.Controllers.PreCadastroEv.CountDoc,
        NomeTipoDocumento: nomeTipoDocumento,
        Situacao: 7,
        NomeArquivo: documento.name,
        IdTipoDocumento: tipoDocumento,
        File: documento
    };

    Europa.Controllers.PreCadastroEv.Documentos.push(dataAux);    

    Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Sucesso, Europa.String.Format("Documento {0} carregado", documento.name));
    Europa.Informacao.Show();

    var registro = {
        Id: Europa.Controllers.PreCadastroEv.CountDoc,
        NomeTipoDocumento: nomeTipoDocumento,
        Situacao: 7,
        NomeArquivo: documento.name,
        IdTipoDocumento: tipoDocumento
    }

    Europa.Controllers.PreCadastroEv.ListaDatatable.push(registro);
    Europa.Controllers.PreCadastroEv.Tabela.reloadData();

    $("#documentos_empresa_venda").find("#arquivoEmpresaVenda").val("");
    $("#TiposDocumentoEmpresaVenda").val("");

    Europa.Controllers.PreCadastroEv.CountDoc++;
}

Europa.Controllers.PreCadastroEv.LimparDocumentos = function () {
    $("#documentos_empresa_venda").find("#arquivoEmpresaVenda").val("");
    $("#TiposDocumentoEmpresaVenda").val("");

    Europa.Controllers.PreCadastroEv.Documentos = [];
    Europa.Controllers.PreCadastroEv.ListaDatatable = [];

    Europa.Controllers.PreCadastroEv.Tabela.reloadData();
}

Europa.Controllers.PreCadastroEv.UploadDocumentosEmpresaVenda = function () {

    Europa.Controllers.PreCadastroEv.Documentos.forEach(Europa.Controllers.PreCadastroEv.UploadDocumentoEmpresaVenda);

}

Europa.Controllers.PreCadastroEv.UploadDocumentoEmpresaVenda = function (documento, idx, list) {

    var formData = new FormData();

    formData.append("IdEmpresaVenda", Europa.Controllers.PreCadastroEv.IdEmpresaVenda);
    formData.append("NomeEmpresaVenda", Europa.Controllers.PreCadastroEv.NomeEmpresaVenda);
    formData.append("File", documento.File);
    formData.append("IdTipoDocumento", documento.IdTipoDocumento);
    formData.append("NomeTipoDocumento", documento.NomeTipoDocumento);
    formData.append("NomeArquivo", documento.NomeArquivo);

    $.ajax({
        type: 'POST',
        url: Europa.Controllers.PreCadastroEv.UrlUploadDocumentoEmpresaVenda,
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
        success: function (res) {
            if (res.Sucesso) {
                
            } else {
                Europa.Informacao.PosAcao(res);
            }
        }
    });
}