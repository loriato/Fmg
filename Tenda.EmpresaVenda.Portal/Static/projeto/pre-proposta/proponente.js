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

    self.Novo = function () {
        self.modoInclusao = true;
        Europa.Components.ClienteModal.AbrirModal(self.CallbackSelectCliente);
    };

    self.Editar = function () {

    };

    self.Excluir = function (objetoLinhaTabela) {
        Europa.Confirmacao.ChangeConfirmText(Europa.i18n.Messages.Confirmar);
        Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Confirmacao, Europa.String.Format(Europa.i18n.Messages.ConfirmacaoAcaoRegistroExclusaoProponente, Europa.i18n.Messages.Excluir.toLowerCase(), objetoLinhaTabela.NomeCliente));
        Europa.Confirmacao.ConfirmCallback = function () {
            var requestContent = {
                idPreProposta: Europa.Controllers.PreProposta.Proponente.IdPreProposta,
                idProponente: objetoLinhaTabela.Id
            };
            $.post(Europa.Controllers.PreProposta.Proponente.UrlExcluir, requestContent, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.PreProposta.Proponente.Reload();
                    Europa.Informacao.PosAcao(res);
                    // FIXME: achar uma forma melhor de notificar os demais grids
                    // O caminho é avisar o pai (pré-proposta) de determinada ação, e o PAI toma as ações necessárias
                    Europa.Controllers.PreProposta.DocumentoProponente.ReconstruirInformacoes();
                } else {
                    Europa.Informacao.PosAcao(res);
                }
            });
        };
        Europa.Confirmacao.Show();

    };

    self.Salvar = function (obj) {
        var url = self.modoInclusao ? self.UrlIncluir : self.UrlAlterar;
        $("#area_proponente .has-error").removeClass("has-error");
        // Europa.Validator.ClearForm("#form_pontovenda");
        $.post(url, { idPreProposta: self.IdPreProposta, proponente: obj, idCliente: self.IdClienteEdit }, function (res) {
            if (res.Sucesso) {
                self.Tabela.closeEdition();
                self.Reload();
                self.modoInclusao = true;
                // FIXME: achar uma forma melhor de notificar os demais grids
                // O caminho é avisar o pai (pré-proposta) de determinada ação, e o PAI toma as ações necessárias
                Europa.Controllers.PreProposta.DocumentoProponente.ReconstruirInformacoes();
            } else {
                self.AddError(res.Campos);
            }
            Europa.Informacao.PosAcao(res);
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
        .setColActions(actionsHtml, '50px')
        .setAutoInit()
        .setDefaultOrder([[3, 'desc'], [1, 'asc']])
        .setDefaultOptions('POST', Europa.Controllers.PreProposta.Proponente.UrlListar, Europa.Controllers.PreProposta.Proponente.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButtonEdit(Europa.Controllers.PreProposta.Proponente.Permissoes.Atualizar, "Editar", "fa fa-edit", "editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonDelete(Europa.Controllers.PreProposta.Proponente.Permissoes.Excluir, "Excluir", "fa fa-trash", "excluir(" + meta.row + ")", full.Situacao, full.Titular) +
            '</div>';
    }

    $scope.renderButtonEdit = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 3 || !Europa.Controllers.PreProposta.PodeManterAssociacoes()) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-steel')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.renderButtonDelete = function (hasPermission, title, icon, onClick, situacao, isTitular) {
        if (hasPermission !== 'true' || situacao === 3 || isTitular || !Europa.Controllers.PreProposta.PodeManterAssociacoes()) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-steel')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.editar = function (row) {
        $scope.rowEdit(row);
        var objetoLinhaTabela = Europa.Controllers.PreProposta.Proponente.Tabela.getRowData(row);
        Europa.Controllers.PreProposta.Proponente.CurrentEditData = objetoLinhaTabela;

        $('#ProponenteEditCpfCnpjCliente').val(Europa.Controllers.PreProposta.Proponente.FormatDocument(objetoLinhaTabela.CpfCnpjCliente));
        $('#ProponenteEditTitular').val(Europa.String.FormatBoolean(objetoLinhaTabela.Titular));
        $('#ProponenteEditCelular').val(Europa.String.FormatAsPhone(objetoLinhaTabela.Celular));
        $('#ProponenteEditResidencial').val(Europa.String.FormatAsPhone(objetoLinhaTabela.Residencial));
        $('#ProponenteEditParticipacao').mask("000");

        Europa.Controllers.PreProposta.Proponente.modoInclusao = false;
        Europa.Controllers.PreProposta.Proponente.IdClienteEdit = objetoLinhaTabela.IdCliente;
    };

    $scope.excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.PreProposta.Proponente.Tabela.getRowData(row);
        Europa.Controllers.PreProposta.Proponente.Excluir(objetoLinhaTabela);
    };

    function formatDocument(data) {
        return data? Europa.Mask.GetMaskedValue(data, Europa.Mask.Behavior.CpfCnpj) : "";
    }
}

Europa.Controllers.PreProposta.Proponente.FilterParams = function () {
    return {
        idPreProposta: Europa.Controllers.PreProposta.Proponente.IdPreProposta
    };
};

Europa.Controllers.PreProposta.Proponente.PreSalvar = function () {
    var objetoLinhaTabela = Europa.Controllers.PreProposta.Proponente.Tabela.getDataRowEdit();
    Europa.Controllers.PreProposta.Proponente.CurrentEditData.Participacao = objetoLinhaTabela.Participacao;
    Europa.Controllers.PreProposta.Proponente.Salvar(Europa.Controllers.PreProposta.Proponente.CurrentEditData);
};