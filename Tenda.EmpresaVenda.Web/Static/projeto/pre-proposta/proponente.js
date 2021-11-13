"use strict";

Europa.Controllers.PreProposta.Proponente = {};
Europa.Controllers.PreProposta.Proponente.IdPreProposta = undefined;
Europa.Controllers.PreProposta.Proponente.IdClienteEdit = undefined;
Europa.Controllers.PreProposta.Proponente.UrlListar = undefined;
Europa.Controllers.PreProposta.Proponente.UrlIncluir = undefined;
Europa.Controllers.PreProposta.Proponente.UrlAlterar = undefined;
Europa.Controllers.PreProposta.Proponente.UrlExcluir = undefined;
Europa.Controllers.PreProposta.Proponente.CurrentEditData = undefined;
Europa.Controllers.PreProposta.Proponente.PreSalvar = undefined;

Europa.Controllers.PreProposta.Proponente.Init = function () {
    // Herdando configurações já efetuadas, já que este método só é executado após a página ser carregada.
    // Se não fizer isso, sobrescrevo todas as propriedades setadas anteriormente (geralmente URLs e permissÕes)
    var self = Europa.Controllers.PreProposta.Proponente;

    self.modoInclusao = true;

    self.Configure = function (idPreProposta) {
        self.IdPreProposta = idPreProposta;
        self.Reload();
    };

    self.Reset = function () {
        self.IdPreProposta = undefined;
    };

    self.Reload = function () {
        self.Tabela.reloadData();
    };

    self.CallbackSelectCliente = function (idCliente) {
        $.ajax({
            url: self.UrlIncluir,
            method: 'POST',
            data: {
                idPreProposta: self.IdPreProposta,
                idCliente: idCliente,
                proponente: {
                    Participacao: 1
                }
            }
        })
        .done(function (response) {
            if (response.Sucesso) {
                self.Tabela.setCallBackReload(function () {
                    var objetoLinhaTabela = undefined;
                    for (var i = 0; i < 10; i++) {
                        var objLinha = Europa.Controllers.PreProposta.Proponente.Tabela.getRowData(i);
                        if (objLinha && objLinha.Id == response.Objeto.idProponente) {
                            objetoLinhaTabela = Europa.Controllers.PreProposta.Proponente.Tabela.getRowData(i);
                            Europa.Controllers.PreProposta.Proponente.Tabela.rowEdit(i);
                            break;
                        }
                    }
                    self.CurrentEditData = objetoLinhaTabela;
                    $('#ProponenteEditCpfCnpjCliente').val(Europa.Controllers.PreProposta.Proponente.FormatDocument(objetoLinhaTabela.CpfCnpjCliente));
                    $('#ProponenteEditTitular').val(Europa.String.FormatBoolean(objetoLinhaTabela.Titular));
                    $('#ProponenteEditCelular').val(Europa.String.FormatAsPhone(objetoLinhaTabela.Celular));
                    $('#ProponenteEditResidencial').val(Europa.String.FormatAsPhone(objetoLinhaTabela.Residencial));
                    $('#ProponenteEditParticipacao').mask("000");

                    self.IdClienteEdit = objetoLinhaTabela.IdCliente;;

                    self.Tabela.setCallBackReload(undefined);
                });
                self.Tabela.reloadData(true);
            } else {
                Europa.Informacao.PosAcao(response);
                setTimeout(Europa.Components.ClienteModal.AbrirModal.bind(null, self.CallbackSelectCliente), 100);
            }
        });
    };

    self.AddError = function (fields) {
        fields.forEach(function (key) {
            $("[name='" + key + "']").parent().addClass("has-error");
        });
    };

    /**
     * @return {string}
     */
    self.FormatDocument = function (data) {
        return data ? Europa.Mask.GetMaskedValue(data, Europa.Mask.Behavior.CpfCnpj) : "";
    };

    Europa.Controllers.PreProposta.Proponente = self;
};

////////////////////////////////////////////////////////////////////////////////////
// Funções Datatable
////////////////////////////////////////////////////////////////////////////////////
DataTableApp.controller('proponentesTable', proponentesTable);

function proponentesTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

    Europa.Controllers.PreProposta.Proponente.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.PreProposta.Proponente.Tabela;
    tabelaWrapper.
        setColumns([
            DTColumnBuilder.newColumn('NomeCliente').withTitle(Europa.i18n.Messages.Nome).withOption('width', '20%')
                .withOption("link", tabelaWrapper.withOptionLink(Europa.Components.DetailAction.Cliente, "IdCliente")),
            DTColumnBuilder.newColumn('CpfCnpjCliente').withTitle(Europa.i18n.Messages.CpfCnpj).withOption('width', '15%').renderWith(formatDocument),
            DTColumnBuilder.newColumn('Titular').withTitle(Europa.i18n.Messages.Titular).withOption('width', '10%').renderWith(Europa.String.FormatBoolean),
            DTColumnBuilder.newColumn('Participacao').withTitle(Europa.i18n.Messages.Participacao).withOption('width', '10%'),
            DTColumnBuilder.newColumn('Email').withTitle(Europa.i18n.Messages.Email).withOption('width', '15%'),
            DTColumnBuilder.newColumn('Celular').withTitle(Europa.i18n.Messages.Celular).withOption('width', '10%').renderWith(Europa.String.FormatAsPhone),
            DTColumnBuilder.newColumn('Residencial').withTitle(Europa.i18n.Messages.Residencial).withOption('width', '10%').renderWith(Europa.String.FormatAsPhone)
        ])
        .setTemplateEdit([
            '<input type="text" class="form-control" id="ProponenteEditNomeCliente" name="NomeCliente" readonly="true">',
            '<input type="text" class="form-control" id="ProponenteEditCpfCnpjCliente" name="CpfCnpjCliente" readonly="true">',
            '<input type="text" class="form-control" id="ProponenteEditTitular" name="Titular" readonly="true">',
            '<input type="text" class="form-control" id="ProponenteEditParticipacao" name="Participacao">',
            '<input type="text" class="form-control" id="ProponenteEditEmail" name="Email" readonly="true">',
            '<input type="text" class="form-control" id="ProponenteEditCelular" name="Celular" readonly="true">',
            '<input type="text" class="form-control" id="ProponenteEditResidencial" name="Residencial" readonly="true">'
        ])
        .setActionSave(Europa.Controllers.PreProposta.Proponente.PreSalvar)
        .setIdAreaHeader("proponente_datatable_header")
        .setAutoInit()
        .setDefaultOrder([[3, 'desc'], [1, 'asc']])
        .setDefaultOptions('POST', Europa.Controllers.PreProposta.Proponente.UrlListar, Europa.Controllers.PreProposta.Proponente.FilterParams);

    function formatDocument(data) {
        return data? Europa.Mask.GetMaskedValue(data, Europa.Mask.Behavior.CpfCnpj) : "";
    }
}

Europa.Controllers.PreProposta.Proponente.FilterParams = function () {
    return {
        idPreProposta: Europa.Controllers.PreProposta.Proponente.IdPreProposta
    };
};
