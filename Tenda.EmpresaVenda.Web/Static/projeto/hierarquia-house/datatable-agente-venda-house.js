
$(function () {
    
});

function DatatableAgenteVendaHouse($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.HierarquiaHouse.DatatableAgenteVendaHouse = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.HierarquiaHouse.DatatableAgenteVendaHouse;
    tabela
        .setTemplateEdit([
            '<input type="text" class="form-control" name="NomeUsuarioAgenteVenda" id="NomeUsuarioAgenteVenda" disabled >',
            '<select id="autocomplete_house" name="autocomplete_house" class="select2-container form-control"></select>',
            '<input type="text" class="form-control" name="Ativo" id="Ativo" disabled>',
        ])
        .setColumns([
            DTColumnBuilder.newColumn('NomeUsuarioAgenteVenda').withTitle(Europa.i18n.Messages.Nome).withOption("width", "130px").renderWith(renderAgenteVenda),
            DTColumnBuilder.newColumn('NomeHouse').withTitle(Europa.i18n.Messages.Loja).withOption("width", "100px"),
            DTColumnBuilder.newColumn('Ativo').withTitle(Europa.i18n.Messages.Ativo).withOption("width", "100px").renderWith(renderFlag)
        ])
        .setColActions(actionsHtml, '50px')
        .setActionSave(Europa.Controllers.HierarquiaHouse.VincularSupervisorAgenteVendaHouse)
        .setAutoInit(false)
        .setIdAreaHeader("agente_venda_house_datatable_barra")
        .setOptionsSelect('POST', Europa.Controllers.HierarquiaHouse.UrlListarAgenteVendaHouse, Europa.Controllers.HierarquiaHouse.FiltroAgenteVendaHouse);

    function actionsHtml(data, type, full, meta) {
        
        var button = '<div>';

        if ($("#IsSuperiorHouse").val() == 'True' ||
            $("#IdCoordenadorHouse").val() == $("#autocomplete_coordenador_house").val() ||
            Europa.Controllers.HierarquiaHouse.IdSupervisorHouse == $("#IdSupervisorHouse").val()) {

            if (full.Ativo) {
                button += $scope.renderButton(Europa.Controllers.HierarquiaHouse.Permissoes.Atualizar, "Desvincular", "fa fa-unlink", "DesvincularSupervisorAgenteVendaHouse(" + meta.row + ")");
            }

            if (!full.Ativo) {
                button += $scope.renderButton(Europa.Controllers.HierarquiaHouse.Permissoes.Atualizar, "Vincular", "fa fa-link", "VincularSupervisorAgenteVendaHouse(" + meta.row + ")");
            }
        }

        return button + '</div>';
    }

    function renderAgenteVenda(data, type, full, meta) {
        if (data) {
            return full.NomeUsuarioAgenteVenda + " - (" + full.LoginUsuarioAgenteVenda + ")";
        }

        return "";
    }

    function renderFlag(data, type, full, meta) {
        
        if (!Europa.Controllers.HierarquiaHouse.Permissoes.Incluir) {
            return "";
        }

        if (data) {
            return "Ativo";
        }

        return "";

    }

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (!hasPermission) {
            return "";
        }

        icon = $('<i/>').addClass(icon);

        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);

        return button.prop('outerHTML');
    }

    $scope.DesvincularSupervisorAgenteVendaHouse = function (row) {
        var objeto = Europa.Controllers.HierarquiaHouse.DatatableAgenteVendaHouse.getRowData(row);

        Europa.Controllers.HierarquiaHouse.DesvincularSupervisorAgenteVendaHouse(objeto);        
    };

    $scope.VincularSupervisorAgenteVendaHouse = function (row) {
        $scope.rowEdit(row);

        Europa.Controllers.HierarquiaHouse.InitAutoComplete();

        var obj = Europa.Controllers.HierarquiaHouse.DatatableAgenteVendaHouse.getRowData(row);
        
        Europa.Controllers.HierarquiaHouse.AgenteVendaEdit = {
            IdUsuarioAgenteVenda: obj.IdUsuarioAgenteVenda,
            NomeUsuarioAgenteVenda: obj.NomeUsuarioAgenteVenda,
            EmailUsuarioAgenteVenda: obj.EmailUsuarioAgenteVenda,
            IdAgenteVenda: obj.IdAgenteVenda,

            IdSupervisorHouse: Europa.Controllers.HierarquiaHouse.IdSupervisorHouse,
            NomeSupervisorHouse: Europa.Controllers.HierarquiaHouse.NomeSupervisorHouse,

            IdCoordenadorHouse: Europa.Controllers.HierarquiaHouse.IdCoordenadorHouse,

            IdHouse: obj.IdHouse,
            NomeHouse: obj.NomeHouse,
            RegionalHouse: obj.RegionalHouse,

            IdSupervisorAgenteVendaHouse: obj.IdSupervisorAgenteVendaHouse,
            IdAgenteVendaHouse: obj.IdAgenteVendaHouse,
            Ativo: obj.Ativo
        };

        if (obj.IdHouse) {
            Europa.Controllers.HierarquiaHouse.AutoCompleteHouse.SetValue(obj.IdHouse, obj.NomeHouse);
        }

        if (obj.Ativo) {
            $("#Ativo").val("Ativo");
        } else {
            $("#Ativo").val("Inativo")
        }
    };

};

DataTableApp.controller('DatatableAgenteVendaHouse', DatatableAgenteVendaHouse);

Europa.Controllers.HierarquiaHouse.FiltroAgenteVendaHouse = function () {
    if (Europa.Controllers.HierarquiaHouse.IdCoordenadorHouse < 1) {
        Europa.Controllers.HierarquiaHouse.IdSupervisorHouse = -1;
    }
    var filtro = {
        NomeAgenteVenda: $("#nomeAgenteVendaHouse").val(),
        IdSupervisorHouse: Europa.Controllers.HierarquiaHouse.IdSupervisorHouse,
        Ativo: $("#IdCoordenadorHouse").val() == $("#autocomplete_coordenador_house").val() || Europa.Controllers.HierarquiaHouse.IdSupervisorHouse == $("#IdSupervisorHouse").val()
    }

    return filtro;
}

Europa.Controllers.HierarquiaHouse.FiltrarDatatableAgenteVendaHouse = function () {
    if (Europa.Controllers.HierarquiaHouse.IdSupervisorHouse == -1 || Europa.Controllers.HierarquiaHouse.IdSupervisorHouse == undefined) {
        var res = {
            Sucesso: false,
            Mensagens: ["Selecione um supervisor(a)"]
        }
        Europa.Informacao.PosAcao(res);
        return;
    }

    Europa.Controllers.HierarquiaHouse.DatatableAgenteVendaHouse.reloadData(undefined,false);
}

Europa.Controllers.HierarquiaHouse.LimparFiltroAgenteVendaHouse = function () {
    $("#nomeAgenteVendaHouse").val("");
}

Europa.Controllers.HierarquiaHouse.VincularSupervisorAgenteVendaHouse = function () {
    Europa.Controllers.HierarquiaHouse.AgenteVendaEdit.IdHouse = $("#autocomplete_house").val();
    var objeto = Europa.Controllers.HierarquiaHouse.AgenteVendaEdit;

    console.log(objeto)

    $.post(Europa.Controllers.HierarquiaHouse.UrlVincularSupervisorAgenteVendaHouse, objeto, function (res) {
        console.log(res)
        if (res.Success) {
            Europa.Controllers.HierarquiaHouse.DatatableAgenteVendaHouse.closeEdition();
            Europa.Controllers.HierarquiaHouse.FiltrarDatatableAgenteVendaHouse();
        }

        Europa.Informacao.PosAcaoApi(res);
    });
}

Europa.Controllers.HierarquiaHouse.DesvincularSupervisorAgenteVendaHouse = function (hierarquiaHouseDto) {
    console.log(hierarquiaHouseDto)
    $.post(Europa.Controllers.HierarquiaHouse.UrlDesvincularSupervisorAgenteVendaHouse, hierarquiaHouseDto, function (res) {
        console.log(res)
        if (res.Success) {
            Europa.Controllers.HierarquiaHouse.FiltrarDatatableAgenteVendaHouse();
        }

        Europa.Informacao.PosAcaoApi(res);
    });
}