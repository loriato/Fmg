
$(function () {

});

function DatatableSupervisorHouse($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.HierarquiaHouse.DatatableSupervisorHouse = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.HierarquiaHouse.DatatableSupervisorHouse;
    tabela
        .setTemplateEdit([
            '<input type="text" class="form-control" name="Supervisor" id="Supervisor" disabled="disabled">',
            '<select id="autocomplete_coordenador_house_novo" name="autocomplete_coordenador_house_novo" class="select2-container form-control "></select>',

        ])
        .setColumns([
            DTColumnBuilder.newColumn('NomeSupervisorHouse').withTitle(Europa.i18n.Messages.Nome).withOption("width", "100px").renderWith(renderSupervisor),
            DTColumnBuilder.newColumn('NomeCoordenadorHouse').withTitle(Europa.i18n.Messages.Coordenador).withOption("width", "100px").renderWith(renderCoordenador)

        ])
        .setActionSave(Europa.Controllers.HierarquiaHouse.PreSalvarCoordenadorSupervisor)
        .setAutoInit(false)
        .setColActions(actionsHtml, '50px')
        .setIdAreaHeader("supervisor_house_datatable_barra")
        .setOptionsSelect('POST', Europa.Controllers.HierarquiaHouse.UrlListarSupervisorHouse, Europa.Controllers.HierarquiaHouse.FiltroSupervisorHouse);

    function actionsHtml(data, type, full, meta) {
        var button = '<div>';

        if ($("#IsSuperiorHouse").val()=='True' || $("#IdCoordenadorHouse").val() == $("#autocomplete_coordenador_house").val()) {
            
            if (full.Ativo) {
                button += $scope.renderButton(Europa.Controllers.HierarquiaHouse.Permissoes.Atualizar, "Desvincular", "fa fa-unlink", "DesvincularCoordenadorSupervisor(" + meta.row + ")");
            }

            if (!full.Ativo) {
                if ($("#IsSuperiorHouse").val()=='True') {
                    button += $scope.renderButton($("#IsSuperiorHouse").val()=='True', "Vincular", "fa fa-link", "SelecionarCoordenador(" + meta.row + ")");
                } else {
                    button += $scope.renderButton(Europa.Controllers.HierarquiaHouse.Permissoes.Atualizar, "Vincular", "fa fa-link", "VincularCoordenadorSupervisor(" + meta.row + ")");
                }
            }
        }

        return button + '</div>';
    }

    function renderSupervisor(data, type, full, meta) {
        if (data) {
            return full.NomeSupervisorHouse + " - (" + full.LoginSupervisorHouse+")";
        }

        return "";
    }

    function renderCoordenador(data, type, full, meta) {
        if (data) {
            return full.NomeCoordenadorHouse + " - (" + full.LoginCoordenadorHouse + ")";
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

    $scope.onRowSelect = function (data) {

        if (Europa.Controllers.HierarquiaHouse.IdSupervisorHouse == data.IdSupervisorHouse) {
            Europa.Controllers.HierarquiaHouse.IdSupervisorHouse = -1;
            Europa.Controllers.HierarquiaHouse.NomeSupervisorHouse = "";
            Europa.Controllers.HierarquiaHouse.IdCoordenadorHouse = -1;
        } else {
            Europa.Controllers.HierarquiaHouse.IdSupervisorHouse = data.IdSupervisorHouse;
            Europa.Controllers.HierarquiaHouse.NomeSupervisorHouse = data.NomeSupervisorHouse;
            Europa.Controllers.HierarquiaHouse.IdCoordenadorHouse = data.IdCoordenadorHouse;
        }

        Europa.Controllers.HierarquiaHouse.DatatableAgenteVendaHouse.reloadData();
        
    }

    $scope.VincularCoordenadorSupervisor = function (row) {
        var objeto = Europa.Controllers.HierarquiaHouse.DatatableSupervisorHouse.getRowData(row);

        Europa.Controllers.HierarquiaHouse.VincularCoordenadorSupervisor(objeto);
    }
    
    $scope.DesvincularCoordenadorSupervisor = function (row) {
        var objeto = Europa.Controllers.HierarquiaHouse.DatatableSupervisorHouse.getRowData(row);

        Europa.Controllers.HierarquiaHouse.DesvincularCoordenadorSupervisor(objeto);
    }

    $scope.SelecionarCoordenador = function (row) {
        $scope.rowEdit(row);

        Europa.Controllers.HierarquiaHouse.AutoCompleteCoordenadorHouseNovo = new Europa.Components.AutoCompleteCoordenadorHouse()
            .WithTargetSuffix("coordenador_house_novo")
            .Configure();
        
    }
};

DataTableApp.controller('DatatableSupervisorHouse', DatatableSupervisorHouse);


Europa.Controllers.HierarquiaHouse.FiltroSupervisorHouse = function () {
    var filtro = {
        NomeSupervisorHouse: $("#nomeSupervisorHouse").val(),
        IdCoordenadorHouse: $("#autocomplete_coordenador_house").val(),
        Ativo: $("#IdCoordenadorHouse").val() == $("#autocomplete_coordenador_house").val()
    }

    return filtro;
}

Europa.Controllers.HierarquiaHouse.FiltrarDatatableSupervisorHouse = function () {
    Europa.Controllers.HierarquiaHouse.IdSupervisorHouse = -1;
    Europa.Controllers.HierarquiaHouse.DatatableSupervisorHouse.reloadData();
    Europa.Controllers.HierarquiaHouse.DatatableAgenteVendaHouse.reloadData();
}

Europa.Controllers.HierarquiaHouse.LimparFiltroSupervisorHouse = function () {
    $("#nomeSupervisorHouse").val("");

    Europa.Controllers.HierarquiaHouse.FiltrarDatatableSupervisorHouse();
}

Europa.Controllers.HierarquiaHouse.VincularCoordenadorSupervisor = function (hierarquiaHouseDto) {
    $.post(Europa.Controllers.HierarquiaHouse.UrlVincularCoordenadorSupervisor, hierarquiaHouseDto, function (res) {
        if (res.Success) {
            Europa.Controllers.HierarquiaHouse.FiltrarDatatableSupervisorHouse();            
        }
        Europa.Informacao.PosAcaoApi(res);
    })
}

Europa.Controllers.HierarquiaHouse.DesvincularCoordenadorSupervisor = function (hierarquiaHouseDto) {
    $.post(Europa.Controllers.HierarquiaHouse.UrlDesvincularCoordenadorSupervisor, hierarquiaHouseDto, function (res) {
        if (res.Success) {
            Europa.Controllers.HierarquiaHouse.FiltrarDatatableSupervisorHouse();            
        }
        Europa.Informacao.PosAcaoApi(res);
    })
}

Europa.Controllers.HierarquiaHouse.PreSalvarCoordenadorSupervisor = function () {
    var objetoLinhaTabela = Europa.Controllers.HierarquiaHouse.DatatableSupervisorHouse.getDataRowEdit();

    var hierarquiaHouseDto = {
        IdSupervisorHouse: objetoLinhaTabela.Id,
        IdCoordenadorHouse: parseInt(objetoLinhaTabela.autocomplete_coordenador_house_novo)
    }

    Europa.Controllers.HierarquiaHouse.VincularCoordenadorSupervisor(hierarquiaHouseDto)
};