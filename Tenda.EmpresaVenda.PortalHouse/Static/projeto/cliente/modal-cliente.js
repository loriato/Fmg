Europa.Components.ClienteModal = {};
Europa.Components.ClienteModal.DataTable = {};
Europa.Components.ClienteModal.UrlListar = undefined;
Europa.Components.ClienteModal.ActionSelecionar = undefined;

$(document).ready(function () {
    Europa.Mask.Telefone("#cliente_filtro_telefone");
    Europa.Mask.CpfCnpj("#cliente_filtro_cpf_cnpj");
});

Europa.Components.ClienteModal.AbrirModal = function (selectCallback) {
    $("#modal_busca_clientes").modal("show");
    Europa.Components.ClienteModal.ActionSelecionar = selectCallback;
};

Europa.Components.ClienteModal.Selecionar = function () {
    if (Europa.Components.ClienteModal.DataTable.getRowsSelect()) {
        Europa.Components.ClienteModal.ActionSelecionar(Europa.Components.ClienteModal.DataTable.getRowsSelect().Id, Europa.Components.ClienteModal.DataTable.getRowsSelect().NomeCompleto);
        Europa.Components.ClienteModal.CloseModal();
    } else {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.i18n.Messages.NenhumRegistroSelecionando);
        Europa.Informacao.Show();
    }
};

Europa.Components.ClienteModal.CloseModal = function () {
    $("#modal_busca_clientes").modal("hide");
};

Europa.Components.ClienteModal.filterParams = function () {
    var params = {
        nome: $("#cliente_filtro_nome").val(),
        email: $("#cliente_filtro_email").val(),
        telefone: $("#cliente_filtro_telefone").val(),
        cpfCnpj: $("#cliente_filtro_cpf_cnpj").val()
    };
    return params;
};

Europa.Components.ClienteModal.LimparBusca = function () {
    $("#cliente_filtro_nome").val("");
    $("#cliente_filtro_email").val("");
    $("#cliente_filtro_telefone").val("");
    $("#cliente_filtro_cpf_cnpj").val("");
    Europa.Mask.Telefone("#cliente_filtro_telefone");
    Europa.Mask.CpfCnpj("#cliente_filtro_cpf_cnpj");
};


DataTableApp.controller('listClientes', function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Components.ClienteModal.DataTable = dataTableWrapper;

    dataTableWrapper.setIdAreaHeader("barra_datatable_modal_cliente")
        .setColumns([
            DTColumnBuilder.newColumn('NomeCompleto').withTitle(Europa.i18n.Messages.Nome).withOption('width', '30%'),
            DTColumnBuilder.newColumn('Email').withTitle(Europa.i18n.Messages.EmailPrincipal).withOption('width', '20%'),
            DTColumnBuilder.newColumn('TelefoneResidencial').withTitle(Europa.i18n.Messages.TelefonePrincipal).renderWith(formatTelefone).withOption('width', '15%'),
            DTColumnBuilder.newColumn('TelefoneComercial').withTitle(Europa.i18n.Messages.TelefoneComercial).renderWith(formatTelefone).withOption('width', '20%'),
            DTColumnBuilder.newColumn('CpfCnpj').withTitle(Europa.i18n.Messages.CpfCnpj).renderWith(formatCpfCnpj).withOption('width', '20%')
        ])
        .setAutoInit()
        .setDoubleClickOnRowActive()
        .setOptionsSelect('POST', Europa.Components.ClienteModal.UrlListar, Europa.Components.ClienteModal.filterParams);

    function formatTelefone(data, type, full) {
        return Europa.String.FormatTelefone(data);
    }

    function formatCpfCnpj(data, type, full) {
        if (data.length > 11) {
            return Europa.Mask.GetMaskedValue(data, Europa.Mask.FORMAT_CNPJ);
        } else {
            return Europa.Mask.GetMaskedValue(data, Europa.Mask.FORMAT_CPF);
        }
    }

    $scope.onDoubleClickOnRow = function (row, data) {
        if (row != undefined) {
            Europa.Components.ClienteModal.ActionSelecionar(data.Id, data.NomeCompleto);
            Europa.Components.ClienteModal.CloseModal();
        }
    }
});