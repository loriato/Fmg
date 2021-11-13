$(function () {


    Europa.Controllers.GrupoCCA.AutoCompleteRegionais = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regional")
        .WithPlaceholder("Regional")
        .Configure();


    $("#filtro_estados").select2({
        trags: true,
        placeholder: 'Estado'
    });

});

function TableEmpresaVenda($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.GrupoCCA.EmpresaVendaTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.GrupoCCA.EmpresaVendaTable;
    tabela
        .setColumns([
            DTColumnBuilder.newColumn('NomeEmpresaVenda').withTitle(Europa.i18n.Messages.Nome).withOption("width","100px"),
            DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption("width", "100px"),
            DTColumnBuilder.newColumn('UF').withTitle(Europa.i18n.Messages.UF).withOption("width", "100px"),
            DTColumnBuilder.newColumn('Ativo').withTitle(Europa.i18n.Messages.Ativo).withOption("width", "100px").renderWith(renderFlag)

        ])
        .setAutoInit(false)
        .setIdAreaHeader("empresa_venda_datatable_barra")
        .setOptionsSelect('POST', Europa.Controllers.GrupoCCA.UrlListarEmpresaVenda, Europa.Controllers.GrupoCCA.FiltroEmpresaVenda);

    function renderFlag(data, type, full, meta) {

        if (Europa.Controllers.GrupoCCA.Permissoes.Incluir !== 'True') {
            return "";
        }

        var checkBox = '<label>';
        checkBox += '<input id="cb_ev_' + full.IdEmpresaVenda + '_' + full.IdGrupoCCA + '" type = "checkbox" value = "' + data + '"';

        if (data) {
            checkBox += ' checked="checked"';
        }
        
        checkBox += 'onchange = "Europa.Controllers.GrupoCCA.OnJoinEmpresaVenda(' + meta.row + ')" /> ';
        checkBox += '</label>';
        return checkBox;
    }       
    
};

DataTableApp.controller('EmpresaVendaDatatable', TableEmpresaVenda);

Europa.Controllers.GrupoCCA.FiltroEmpresaVenda = function () {
    var param = {
        IdGrupoCCA: Europa.Controllers.GrupoCCA.GrupoCCAId,
        UF: $('#filtro_estados').val(),
        IdRegional: $('#autocomplete_regional').val(),
        NomeEmpresaVenda: $("#filtroEmpresa").val()
    };
    return param;
};

Europa.Controllers.GrupoCCA.CarregarDadosEmpresaVenda = function (grupoId) {
    Europa.Controllers.GrupoCCA.GrupoCCAId = grupoId;
    Europa.Controllers.GrupoCCA.EmpresaVendaTable.closeEdition();
    Europa.Controllers.GrupoCCA.EmpresaVendaTable.reloadData();
}

Europa.Controllers.GrupoCCA.FiltrarEmpresaVenda = function () {
    if (Europa.Controllers.GrupoCCA.GrupoCCAId == 0 || Europa.Controllers.GrupoCCA.GrupoCCAId == undefined) {
        var res = {
            Sucesso: false,
            Mensagens: ["Selecione um grupo CCA"]
        }
        Europa.Informacao.PosAcao(res);
        return;
    }
    Europa.Controllers.GrupoCCA.EmpresaVendaTable.reloadData(undefined,false);
};

Europa.Controllers.GrupoCCA.LimparFiltroEmpresaVenda = function () {
    $("#filtroEmpresa").val("").trigger('change');
    $('#autocomplete_regional').val("").trigger('change');
    $('#filtro_estados').val("").trigger('change');
};

Europa.Controllers.GrupoCCA.OnJoinEmpresaVenda = function (row) {

    var full = Europa.Controllers.GrupoCCA.EmpresaVendaTable.getRowData(row);

    var data = {
        Id: full.IdGrupoCCAEmpresaVenda,
        GrupoCCA: { Id: full.IdGrupoCCA },
        EmpresaVenda: { Id: full.IdEmpresaVenda }
    };

    var backup = $("#cb_ev_" + full.IdEmpresaVenda + "_" + full.IdGrupoCCA + "").val() == "true";

    $.post(Europa.Controllers.GrupoCCA.UrlAtribuirEmpresaVendaAGrupo, { grupoCCAEmpresa: data }, function (resposta) {
        if (resposta.Sucesso) {
            Europa.Controllers.GrupoCCA.FiltrarEmpresaVenda();
        } else {
            Europa.Informacao.PosAcao(resposta);
            $("#cb_ev_" + full.IdEmpresaVenda + "_" + full.IdGrupoCCA + "").prop("checked", backup);
        }
    });

};
