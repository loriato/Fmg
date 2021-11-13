Europa.Components.Datatable.LogErro = {};
Europa.Components.Datatable.LogErro.createDT = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

    Europa.Components.Datatable.LogErro.DataTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);

    Europa.Components.Datatable.LogErro.DataTable
        .setColumns([
            DTColumnBuilder.newColumn('CriadoEm').withTitle(Europa.i18n.Messages.Horario).withOption('width', '12%').withOption("type", "date-format-DD/MM/YYYY HH:mm:ss"),
            DTColumnBuilder.newColumn('Type').withTitle(Europa.i18n.Messages.Tipo).withOption('width', '12%'),
            DTColumnBuilder.newColumn('Message').withTitle(Europa.i18n.Messages.Mensagem).withOption('width', '16%'),
            DTColumnBuilder.newColumn('Source').withTitle(Europa.i18n.Messages.Origem).withOption('width', '10%'),
            DTColumnBuilder.newColumn('OriginClass').withTitle(Europa.i18n.Messages.Classe).withOption('width', '9%'),
            DTColumnBuilder.newColumn('Caller').withTitle(Europa.i18n.Messages.Metodo).withOption('width', '9%'),
            DTColumnBuilder.newColumn('CodeLine').withTitle(Europa.i18n.Messages.Linha).withOption('width', '3%'),
            DTColumnBuilder.newColumn('Target').withTitle(Europa.i18n.Messages.MetodoOrigem).withOption('width', '10%'),
            DTColumnBuilder.newColumn('Stacktrace').renderWith(formatContent).withOption('type', 'code').withOption("visible", false)
        ])
        .setColActions(actionsHtml, '5%')
        .setDefaultOrder([[1, 'desc']])
        .setAutoInit(false)
        .setDefaultOptions('POST', Europa.Components.Datatable.LogErro.listAction, Europa.Components.Datatable.LogErro.filterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>'
            + Europa.Components.Datatable.LogErro.DataTable.renderButton(true, Europa.i18n.Messages.Detalhar, 'fa fa-eye', 'detalharLog(' + meta.row + ')')
            + Europa.Components.Datatable.LogErro.DataTable.renderButton(true, Europa.i18n.Messages.Copiar, 'fa fa-copy', 'copiarConteudo(' + meta.row + ')')
            + '</div>';
    };

    function formatContent(data) {
        var result = "";
        if (data != undefined && data.length > 100) {
            result = data.substring(0, 100) + "...";
        }
        return result;
    }

    $scope.detalharLog = function (id) {
        Europa.Components.Datatable.LogErro.DetalharLog(id);
    };

    $scope.copiarConteudo = function (id) {
        var data = Europa.Components.Datatable.LogErro.DataTable.getRowData(id);
        var aux = document.createElement("textarea");
        aux.value = data.Stacktrace;
        document.body.appendChild(aux);
        aux.select();
        document.execCommand("copy");
        document.body.removeChild(aux);
    };
};

DataTableApp.controller('LogErroDataTable', Europa.Components.Datatable.LogErro.createDT);


Europa.Components.Datatable.LogErro.filterParams = function () {
    var filtro = {
        Type: $("#form_filtro_log_erro").find("#Type").val(),
        Message: $("#form_filtro_log_erro").find("#Message").val(),
        Source: $("#form_filtro_log_erro").find("#Source").val(),
        HorarioInicio: $("#form_filtro_log_erro").find("#HorarioInicio").val(),
        HorarioFim: $("#form_filtro_log_erro").find("#HorarioFim").val(),
        Stacktrace: $("#form_filtro_log_erro").find("#Stacktrace").val()
    };
    return filtro;
};

Europa.Components.Datatable.LogErro.Filtrar = function () {
    Europa.Components.Datatable.LogErro.DataTable.reloadData();
};

Europa.Components.Datatable.LogErro.DetalharLog = function (id) {
    var data = Europa.Components.Datatable.LogErro.DataTable.getRowData(id);
    Europa.Controllers.LogErro.Modal.AbrirModal(data);
};
