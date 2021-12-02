Europa.Controllers.Usuario = {};
Europa.Controllers.Usuario.Form = "#form-usuario";
Europa.Controllers.Usuario.DataTable = {};


$(document).ready(function () {
    Europa.Controllers.Usuario.InitMask();
    Europa.Controllers.Usuario.InitDatePickers();
});

Europa.Controllers.Usuario.AbrirModalIncluir = function () {
    $("#incluir-usuario").modal('show');
};
Europa.Controllers.Usuario.FecharModalIncluir = function () {
    $("#incluir-usuario").modal('hide');
};


Europa.Controllers.Usuario.Tabela = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Controllers.Usuario.DataTable = dataTableWrapper;

    dataTableWrapper
        .setColumns([
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '25%'),
            DTColumnBuilder.newColumn('NumeroFuncional').withTitle('Número Funcional').withOption('width', '20%'),
            DTColumnBuilder.newColumn('Login').withTitle(Europa.i18n.Messages.Login).withOption('width', '20%'),
            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('type', 'enum-format-SituacaoUsuario').withOption('width', '25%')
        ])
        .setColActions(actionsHtml, '10%')
        .setDefaultOptions('POST', Europa.Controllers.Usuario.UrlListar, Europa.Controllers.Usuario.Params);

    function actionsHtml(data, type, full, meta) {
        var html = '<div>' + $scope.renderButton("Visualizar", "fas fa-eye", "Visualizar(" + data.Id + ")");
        html = html + '</div>';

        return html;
    }
    $scope.renderButton = function (title, icon, onClick) {
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn-actions')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.Visualizar = function (id) {
        window.location = Europa.Controllers.Usuario.UrlVisualizar + '/' + id;
    };
    
};

DataTableApp.controller('UsuarioDataTable', Europa.Controllers.Usuario.Tabela);

Europa.Controllers.Usuario.Params = function () {
    var param = {
        Nome: $("#filtro-nome").val(),
        Cpf: $("#filtro-cpf").val(),
        NumeroFuncional: $("#filtro-numero").val()
    };
    return param;
};

Europa.Controllers.Usuario.Filtrar = function () {
    Europa.Controllers.Usuario.DataTable.reloadData();
};

Europa.Controllers.Usuario.InitMask = function () {
    Europa.Mask.Cpf("#filtro-cpf",false);
    Europa.Mask.Int("#filtro-numero");
    Europa.Mask.Telefone("#Telefone");
    Europa.Mask.Cpf("#Cpf",true);
    Europa.Mask.Int("#NumeroFuncional");
};

Europa.Controllers.Usuario.InitDatePickers = function () {
    Europa.Controllers.Usuario.DataNascimento = new Europa.Components.DatePicker()
        .WithTarget("#DataNascimento")
        .WithParentEl("#form-usuario")
        .WithValue($("#DataNascimento").val())
        .WithMaxDate(Europa.Date.Now())
        .Configure();
    Europa.Mask.Apply("#DataNascimento", Europa.Mask.FORMAT_DATE);
};

Europa.Controllers.Usuario.Salvar = function () {
    var param = Europa.Form.SerializeJson(Europa.Controllers.Usuario.Form);
    $.post(Europa.Controllers.Usuario.UrlSalvar, param, function (res) {
        if (res.Success) {
            Europa.Controllers.Usuario.Filtrar();
            Europa.Controllers.Usuario.FecharModalIncluir();
        } else { }
        Europa.Informacao.PosAcaoBaseResponse(res);
    });
}