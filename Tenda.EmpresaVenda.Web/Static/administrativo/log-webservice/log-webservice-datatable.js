Europa.Components.Datatable.LogWebService = {};
Europa.Components.Datatable.LogWebService.createDT = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

    Europa.Components.Datatable.LogWebService.DataTable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);

    Europa.Components.Datatable.LogWebService.DataTable
        .setColumns([
            DTColumnBuilder.newColumn('CriadoEm').withTitle(Europa.i18n.Messages.Horario).withOption('width', '150px').withOption("type", "date-format-DD/MM/YYYY HH:mm:ss"),
            DTColumnBuilder.newColumn('Endpoint').withTitle(Europa.i18n.Messages.Endpoint),
            DTColumnBuilder.newColumn('Action').withTitle(Europa.i18n.Messages.Operacao),
            DTColumnBuilder.newColumn('Stage').withTitle(Europa.i18n.Messages.Sentido).renderWith(formatStage).withOption('width', '150px'),
            DTColumnBuilder.newColumn('Content').withTitle(Europa.i18n.Messages.Conteudo).renderWith(formatContent).withOption('type', 'code')
        ])
        .setColActions(actionsHtml, '70px')
        .setDefaultOrder([[1, 'desc']])
        .setAutoInit(false)
        .setDefaultOptions('POST', Europa.Components.Datatable.LogWebService.listAction, Europa.Components.Datatable.LogWebService.filterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>'
            + Europa.Components.Datatable.LogWebService.DataTable.renderButton(true, Europa.i18n.Messages.Detalhar, 'fa fa-eye', 'detalharLog(' + meta.row + ')')
            + Europa.Components.Datatable.LogWebService.DataTable.renderButton(true, Europa.i18n.Messages.Copiar, 'fa fa-copy', 'copiarConteudo(' + meta.row + ')')
            + '</div>';
    };

    function formatContent(data) {
        var result;
        if (data.length > 100) {
            result = data.substring(0, 100) + "...";
        }
        return '<textarea rows="2" style="width:100%; border:none; resize:none;overflow:hidden" readonly="readonly" onfocus="this.blur();">' + result + '</textarea>';
    }

    function formatStage(data) {
        if (data == 1) {
            return "Antes de Serializar";
        } else if (data == 2) {
            return "Depois de Serializar";
        } else if (data == 4) {
            return "Antes de Deserializar";
        } else if (data == 8) {
            return "Depois de Deserializar";
        }
    }

    $scope.detalharLog = function (id) {
        Europa.Components.Datatable.LogWebService.DetalharLog(id);
    };

    $scope.copiarConteudo = function (id) {
        var data = Europa.Components.Datatable.LogWebService.DataTable.getRowData(id);
        var aux = document.createElement("textarea");
        aux.value = data.Content;
        document.body.appendChild(aux);
        aux.select();
        document.execCommand("copy");
        document.body.removeChild(aux);
    };
};

DataTableApp.controller('LogWebServiceDataTable', Europa.Components.Datatable.LogWebService.createDT);


Europa.Components.Datatable.LogWebService.filterParams = function() {
    var filtro = {
        Endpoint: $("#form_filtro_log_webservice").find("#Endpoint").val(),
        Operacao: $("#form_filtro_log_webservice").find("#Action").val(),
        Conteudo: $("#form_filtro_log_webservice").find("#Content").val(),
        HorarioInicio: $("#form_filtro_log_webservice").find("#HorarioInicio").val(),
        HorarioFim: $("#form_filtro_log_webservice").find("#HorarioFim").val(),
        Stage: $("#form_filtro_log_webservice").find("#Stage").val()
    };
    return filtro;
};

Europa.Components.Datatable.LogWebService.Filtrar = function() {
    Europa.Components.Datatable.LogWebService.DataTable.reloadData();
};

Europa.Components.Datatable.LogWebService.DetalharLog = function(id) {
    var data = Europa.Components.Datatable.LogWebService.DataTable.getRowData(id);
    Europa.Controllers.LogWebService.Modal.AbrirModal(data);
};
