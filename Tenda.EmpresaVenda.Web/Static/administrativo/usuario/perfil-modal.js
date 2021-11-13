Europa.Components.ModalPerfil.SelectedData = function () {
    return Europa.Components.ModalPerfil.DataTable.getRowsSelect();
};

//Definir a área de filtros e fazer isso aqui automaticamente
Europa.Components.ModalPerfil.FilterData = function () {
    var suspensos = false;
    if (document.getElementById('idSuspensos').checked) {
        suspensos = true;
    }
    return {
        nome: $("#modal_perfil_nome").val(),
        buscaSuspensos: suspensos
    };
};

//Definir a área de filtros e fazer isso aqui automaticamente
Europa.Components.ModalPerfil.LimparBusca = function () {
    $("#modal_perfil_nome").val("");
    $("#idSuspensos").click();
    $("#modal_perfil_btn_filtrar").click();
}

DataTableApp.controller('listPerfis', listPerfis);

function listPerfis($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Components.ModalPerfil.DataTable = dataTableWrapper;

    dataTableWrapper.setIdAreaHeader("barra_datatable_modal_perfil")
        .setColumns([
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '50%'),
            DTColumnBuilder.newColumn('CriadoEm').withTitle(Europa.i18n.Messages.DataCriacao).renderWith(dataTableWrapper.renderDateSmall).withOption('width', '30%'),
            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).renderWith(actionsHtml)
        ])
        .setAutoInit(false)
        .setDoubleClickOnRowActive()
        .setOptionsSelect('GET', Europa.Components.ModalPerfil.DataTableAction(), Europa.Components.ModalPerfil.FilterData);


    function actionsHtml(data, type, full, meta) {
        return Europa.i18n.Enum.Resolve("Situacao", data);
    }


    $scope.edit = function (id) {
        $scope.reloadData();
    }

    $scope.onRowSelect = function (row) {
    }

    $scope.onDoubleClickOnRow = function (row) {
        if (row != undefined) {
            Europa.Components.ModalPerfil.Select();
            Europa.Components.ModalPerfil.CloseModal();
        }
    }
}

Europa.Components.ModalPerfil.CloseModal = function () {
    
}