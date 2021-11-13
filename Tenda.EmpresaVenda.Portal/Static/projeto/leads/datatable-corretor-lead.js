$(function () {

});

DataTableApp.controller('CorretorDatatable', corretorDatatable);

function corretorDatatable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.Leads.TabelaCorretor = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.Leads.TabelaCorretor;
   
    self.setColumns([
        DTColumnBuilder.newColumn('NomeLead').withTitle(Europa.i18n.Messages.Nome).withOption('width', '150px'),
        DTColumnBuilder.newColumn('SituacaoLead').withTitle(Europa.i18n.Messages.Situacao)
            .withOption('width', '100px').withOption('type', 'enum-format-SituacaoLead'),
        DTColumnBuilder.newColumn('Bairro').withTitle(Europa.i18n.Messages.Bairro).withOption('width', '150px'),
        DTColumnBuilder.newColumn('Cidade').withTitle(Europa.i18n.Messages.Cidade).withOption('width', '150px'),
        DTColumnBuilder.newColumn('Uf').withTitle(Europa.i18n.Messages.Estado).withOption('width', '150px'),
        DTColumnBuilder.newColumn('Pacote').withTitle(Europa.i18n.Messages.Pacote).withOption('width', '150px'),
    ])
        //.setIdAreaHeader("diretpr_datatable_header") 
        .setColActions(actionsHtml, '50px')
        .setActionSave(Europa.Controllers.Leads.PreSalvar)
        .setDefaultOrder([[1, 'asc']])
        .setAutoInit(false)
        .setOptionsSelect('POST', Europa.Controllers.Leads.UrlListarDatatable, Europa.Controllers.Leads.FiltroCorretor);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButtonVisualizar(Europa.Controllers.Leads.Permissoes.Visualizar, "Visualizar Lead", "fa fa-eye", "detalhar(" + meta.row + ")") +
            //$scope.renderButtonIncluir(Europa.Controllers.Leads.Permissoes.Incluir, "Incluir Corretor", "fa fa-plus", "incluir(" + meta.row + ")", full) +
            //$scope.renderButtonAtualizar(Europa.Controllers.Leads.Permissoes.Atualizar, "Atualizar Corretor", "fa fa-edit", "editar(" + meta.row + ")", full) +
            //$scope.renderButtonExcluir(Europa.Controllers.Leads.Permissoes.Excluir, "Excluir Corretor", "fa fa-trash", "excluir(" + meta.row + ")", full) +
            '</div>';
    }

    $scope.renderButtonVisualizar = function (hasPermission, title, icon, onClick) {
        if (hasPermission != 'True') {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-steel')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.renderButtonAtualizar = function (hasPermission, title, icon, onClick, full) {
        if (hasPermission != 'True') {
            return "";
        }

        if (full.IdCorretor == 0) {
            return "";
        }

        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-steel')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.renderButtonExcluir = function (hasPermission, title, icon, onClick, full) {
        if (hasPermission != 'True') {
            return "";
        }

        if (full.IdCorretor == 0) {
            return "";
        }

        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-steel')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.renderButtonIncluir = function (hasPermission, title, icon, onClick, full) {
        if (hasPermission != 'True') {
            return "";
        }

        if (full.IdCorretor > 0) {
            return "";
        }

        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-steel')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.detalhar = function (row) {
        var obj = Europa.Controllers.Leads.TabelaCorretor.getRowData(row);
        Europa.Controllers.Leads.ModalDetalhar(obj);
    };

    $scope.incluir = function (row) {
        $scope.rowEdit(row);
        var obj = Europa.Controllers.Leads.TabelaDiretor.getRowData(row);

        Europa.Controllers.Leads.IdLeadEmpresasVenda = obj.Id;
        $("#NomeLead").html(obj.NomeLead);
        var situacao = Europa.i18n.Enum.Resolve("SituacaoLead", obj.SituacaoLead);
        $("#SituacaoLead").html(situacao);
        $("#Bairro").html(obj.Bairro);
        $("#Cidade").html(obj.Cidade);
        $("#Uf").html(obj.Uf);

        Europa.Controllers.Leads.InitAutoCompleteCorretor();

    };

    $scope.editar = function (row) {
        $scope.rowEdit(row);
        var obj = Europa.Controllers.Leads.TabelaDiretor.getRowData(row);

        Europa.Controllers.Leads.IdLeadEmpresasVenda = obj.Id;
        $("#NomeLead").html(obj.NomeLead);
        var situacao = Europa.i18n.Enum.Resolve("SituacaoLead", obj.SituacaoLead);
        $("#SituacaoLead").html(situacao);
        $("#Bairro").html(obj.Bairro);
        $("#Cidade").html(obj.Cidade);
        $("#Uf").html(obj.Uf);

        Europa.Controllers.Leads.InitAutoCompleteCorretor();

        Europa.Controllers.Leads.AutoCompleteCorretorLead.SetValue(obj.IdCorretor, obj.NomeCorretor);

    };

    $scope.excluir = function (row) {
        var obj = Europa.Controllers.Leads.TabelaDiretor.getRowData(row);
        Europa.Controllers.Leads.RemoverAtribuicao(obj.Id);
    };
};

Europa.Controllers.Leads.FiltrarDatatableCorretor = function () {
    Europa.Controllers.Leads.TabelaCorretor.reloadData(undefined, false);
};