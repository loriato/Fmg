var linhas;
Europa.Components.Datatable.Perfil = {};
Europa.Components.Datatable.Perfil.DataTable = {};
Europa.Components.Datatable.Perfil.createDT = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Components.Datatable.Perfil.DataTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Components.Datatable.Perfil.DataTable
        .setIdAreaHeader("PerfilDatatable_barra")
        .setDefaultLength(25)
        .setColumns([
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Perfis),
        ])
        .setOptionsSelect('POST', Europa.Components.Datatable.Perfil.listAction, Europa.Components.Datatable.Perfil.filterParams);

    $scope.onRowSelect = function (data) {
        Europa.Components.Datatable.Funcionalidade.CarregarDados(data.Id);
    }
};

Europa.Components.Datatable.Perfil.GetSelectedObjectsIds = function () {
    return Europa.Components.Datatable.Perfil.DataTable.getRowsSelect().Id;
};

DataTableApp.controller('PerfilDataTable', Europa.Components.Datatable.Perfil.createDT);


Europa.Components.Datatable.Perfil.Filtrar = function () {
    Europa.Components.Datatable.Perfil.DataTable.reloadData();
};

Europa.Components.Datatable.Perfil.filterParams = function() {
    var params = {
        nome: $("#filtroPerfils").val()
    };
    return params;
};

Europa.Components.Datatable.Perfil.Novo = function() {
    Europa.Controllers.PerfilModal.AbrirModal();
};