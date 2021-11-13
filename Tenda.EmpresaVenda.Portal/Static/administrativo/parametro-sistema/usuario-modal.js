Europa.Components.UsuarioModal = {};
Europa.Components.UsuarioModal.DataTable = {};
Europa.Components.UsuarioModal.ActionSelecionar = undefined;

Europa.Components.UsuarioModal.AbrirModal = function (selectCallback) {
    $("#buscaUsuario").modal("show");
    Europa.Components.UsuarioModal.ActionSelecionar = selectCallback;
}

Europa.Components.UsuarioModal.Selecionar = function () {
    var aux = Europa.Components.UsuarioModal.DataTable.getRowsSelect();
    var data = {
        User: {
            Id: aux.Id,
            Nome: aux.Nome
        }
    }
    Europa.Components.UsuarioModal.ActionSelecionar(data);
    Europa.Components.UsuarioModal.CloseModal();
}

Europa.Components.UsuarioModal.CloseModal = function () {
    $("#buscaUsuario").modal("hide");
}

Europa.Components.UsuarioModal.GetRows = function () {
    return Europa.Components.UsuarioModal.DataTable.getRowsSelect();
}

DataTableApp.controller('listUsuarios', listUsuarios);

Europa.Components.UsuarioModal.filterParams = function () {
    var teste = [1];
    if ($("#BuscaUsuariosSuspensos").is(":checked")) {
        teste = [1, 2];
    }
    var params = {
        nameOrEmail: $("#usuarioFiltroNome").val(),
        situacoes: teste
    };
    return params;
}

Europa.Components.UsuarioModal.LimparBusca = function(){
    $("#usuarioFiltroNome").val("");
    if ($("#BuscaUsuariosSuspensos").is(":checked")) {
        $("#BuscaUsuariosSuspensos").click();
    }
    $("#modal_usuario_filtrar").click();
}

renderEnum = function (data, type, full) {
    return Europa.i18n.Enum.Resolve("Situacao", data);
}

function listUsuarios($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Components.UsuarioModal.DataTable = dataTableWrapper;

    dataTableWrapper.setIdAreaHeader("barra_datatable_modal_usuario")
        .setColumns([
        DTColumnBuilder.newColumn('User.Nome').withTitle(Europa.i18n.Messages.Nome),
        DTColumnBuilder.newColumn('User.Area.Nome').withTitle(Europa.i18n.Messages.Area),
        DTColumnBuilder.newColumn('User.Situacao').withTitle(Europa.i18n.Messages.Situacao).renderWith(renderEnum)
        ])
        .setAutoInit(false)
        .setDoubleClickOnRowActive()
        .setOptionsSelect('POST', Europa.Components.UsuarioModal.listAction, Europa.Components.UsuarioModal.filterParams);
    

    $scope.edit = function (id) {
        $scope.reloadData();
    }

    $scope.onDoubleClickOnRow = function (row) {
        if (row != undefined) {
            var aux = Europa.Components.UsuarioModal.DataTable.getRowsSelect();
            var data = {
                User: {
                    Id: aux.Id,
                    Nome: aux.Nome
                }
            }
            Europa.Components.UsuarioModal.ActionSelecionar(data);
            Europa.Components.UsuarioModal.CloseModal();
        }
    }
}