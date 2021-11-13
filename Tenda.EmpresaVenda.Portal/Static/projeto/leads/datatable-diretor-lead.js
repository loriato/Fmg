Europa.Controllers.Leads.IdLeadEmpresasVenda = undefined;
Europa.Controllers.Leads.IdUsuarioCorretor = undefined;
Europa.Controllers.Leads.DescricaoPacote = undefined;

$(function () {
    Europa.Controllers.Leads.AutoCompleteCorretorLote = new Europa.Components.AutoCompleteCorretorEmpresaVenda()
        .WithTargetSuffix("corretor_lote")
        .Configure();

});

Europa.Controllers.Leads.onChangePacote = function () {
    if ($("#pacote_diretor").val() != "") {
        Europa.Controllers.Leads.AbrirPacote();
    }
}

Europa.Controllers.Leads.OnClickSelecionarLeads = function () {
    var autorizar = Europa.Controllers.Leads.ValidarFiltroDiretor();

    if (!autorizar) {
        return;
    }

    if ($("#selecionar_leads").val() == "true") {
        $("#selecionar_leads").val("false");
        $("#selecionar_leads").prop("checked", false);

        $("#check_false").removeClass("hidden");

        $("#check_true").addClass("hidden");
        $(".corretor").addClass("hidden");
        $(".selectAllDt").attr("disabled", "disabled");
        $(".deselectAllDt").attr("disabled", "disabled");
        Europa.Controllers.Leads.DeselecionarRadio();
        Europa.Controllers.Leads.TabelaDiretor.scope.deselectAllRows();

    } else {
        $("#selecionar_leads").val("true");
        $("#selecionar_leads").prop("checked", true);

        $("#check_false").addClass("hidden");

        $("#check_true").removeClass("hidden");
        $(".corretor").removeClass("hidden");
        Europa.Controllers.Leads.AutoCompleteCorretorLote.Clean();
        Europa.Controllers.Leads.SelecionarRadioTodos();
        Europa.Controllers.Leads.OnClickSelecionar();
    }
};

Europa.Controllers.Leads.OnClickSelecionar = function () {
    Europa.Controllers.Leads.TabelaDiretor.scope.deselectAllRows();
    if ($('input[name=leads-selecionados]:checked').val() == 'false') {
        $(".deselectAllDt").removeAttr("disabled");
        $(".selectAllDt").removeAttr("disabled");
       
    } else {
        $(".selectAllDt").attr("disabled", "disabled");
        $(".deselectAllDt").attr("disabled", "disabled");
    }
}

Europa.Controllers.Leads.SelecionarRadioTodos = function () {
    var $radios = $('input:radio[name=leads-selecionados]');
    if ($radios.is(':checked') === false) {
        $radios.filter('[value=true]').prop('checked', true);
    }
}

Europa.Controllers.Leads.DeselecionarRadio = function () {
    var inputs = document.querySelectorAll('input[type="radio"]');
    for (var i = 0, l = inputs.length; i < l; i++) {
        inputs[i].checked = false
    }
}

Europa.Controllers.Leads.ValidarSalvarCorretor = function () {
    var radio = $('input[name=leads-selecionados]:checked').val();
    var leads = Europa.Controllers.Leads.TabelaDiretor.scope.getRowsForceSelect();
    if (radio == undefined || radio == "") {
        var res = {
            Sucesso: false,
            Mensagens: ["Selecione o Campo 'Todos' ou 'Selecionados'"]
        }
        Europa.Informacao.PosAcao(res);
        return false;
    }
    else if (leads.length == 0 && $('input[name=leads-selecionados]:checked').val() == 'false') {
        var res = {
            Sucesso: false,
            Mensagens: ["Selecione um lead"]
        };

        Europa.Informacao.PosAcao(res);
        return false;
    }
    return true;
};

Europa.Controllers.Leads.AtribuirCorretorLeadsPagina = function () {
    var validar = Europa.Controllers.Leads.ValidarSalvarCorretor();
    if (validar) {
        var leads = Europa.Controllers.Leads.TabelaDiretor.scope.getRowsForceSelect();
        var url = undefined;
        if ($('input[name=leads-selecionados]:checked').val() == 'true') {
            url = Europa.Controllers.Leads.UrlAtribuirCorretorLeadsLoteTodos;
        }
        else {
            url = Europa.Controllers.Leads.UrlAtribuirCorretorLeadsLote;
        }


        var ids = [];
        leads.forEach(function (elem) {
            ids.push(elem.Id);
        });

        var leads = {
            IdsLeadsEmpresasVendas: ids,
            IdCorretor: Europa.Controllers.Leads.AutoCompleteCorretorLote.Value(),
            NomeCorretor: Europa.Controllers.Leads.AutoCompleteCorretorLote.Text(),
            Pacote: Europa.Controllers.Leads.DescricaoPacote
        };
        $.post(url, leads, function (res) {
            if (res.Sucesso) {
                Europa.Controllers.Leads.OnClickSelecionarLeads();
                Europa.Controllers.Leads.FiltrarDatatableDiretor();
            }
            Europa.Informacao.PosAcao(res);
        });
    }
};

DataTableApp.controller('DiretorDatatable', diretorDatatable);

function diretorDatatable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.Leads.TabelaDiretor = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.Leads.TabelaDiretor;
    self.setTemplateEdit([
        '<div id="NomeLead"></div>',
        '<div id="SituacaoLead"></div>',
        '<div id="Bairro"></div>',
        '<div id="Cidade"></div>',
        '<div id="Uf"></div>',
        '<div name="NomeCorretor"><select id="autocomplete_corretor_lead" class="form-control"></select></div>'

    ])
    self.setColumns([
        DTColumnBuilder.newColumn('NomeLead').withTitle(Europa.i18n.Messages.Nome).withOption('width', '150px'),
        DTColumnBuilder.newColumn('SituacaoLead').withTitle(Europa.i18n.Messages.Situacao)
            .withOption('width', '100px').withOption('type', 'enum-format-SituacaoLead'),
        DTColumnBuilder.newColumn('Bairro').withTitle(Europa.i18n.Messages.Bairro).withOption('width', '150px'),
        DTColumnBuilder.newColumn('Cidade').withTitle(Europa.i18n.Messages.Cidade).withOption('width', '150px'),
        DTColumnBuilder.newColumn('Uf').withTitle(Europa.i18n.Messages.Estado).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeCorretor').withTitle(Europa.i18n.Messages.Corretor).withOption('width', '150px'),
    ])
        //.setIdAreaHeader("diretpr_datatable_header") 
        .setColActions(actionsHtml, '50px')
        .setActionSave(Europa.Controllers.Leads.PreSalvar)
        .setDefaultOrder([[1, 'asc']])
        .setAutoInit(false)
        .setDefaultOptions('POST', Europa.Controllers.Leads.UrlListarDatatable, Europa.Controllers.Leads.FiltroDiretor);
    self.vm.dtOptions.withSelect({
        style: "api", //Modo api desabilita a seleção, permitindo apenas seleção via api (código)
        selector: "td:not(.datatable-actions)"
    });
    self.selectable = true;
    self.multiSelection = true;
    $scope.onRowSelect = function (data, row) {
        var todos = $('input[name=leads-selecionados]:checked').val();
        if(todos === 'false'){
            if($(row).hasClass("selected")){
                Europa.Controllers.Leads.TabelaDiretor.vm.dtInstance.DataTable.row(row).deselect();
            } else{
                Europa.Controllers.Leads.TabelaDiretor.vm.dtInstance.DataTable.row(row).select();
            }
        }
    }

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButtonVisualizar(Europa.Controllers.Leads.Permissoes.Visualizar, "Visualizar Lead", "fa fa-eye", "detalhar(" + meta.row + ")") +
            $scope.renderButtonIncluir(Europa.Controllers.Leads.Permissoes.Incluir, "Incluir Corretor", "fa fa-plus", "incluir(" + meta.row + ")",full) +
            $scope.renderButtonAtualizar(Europa.Controllers.Leads.Permissoes.Atualizar, "Atualizar Corretor", "fa fa-edit", "editar(" + meta.row + ")",full) +
            $scope.renderButtonExcluir(Europa.Controllers.Leads.Permissoes.Excluir, "Excluir Corretor", "fa fa-trash", "excluir(" + meta.row + ")",full) +
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

    $scope.renderButtonAtualizar = function (hasPermission, title, icon, onClick,full) {
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
        var obj = Europa.Controllers.Leads.TabelaDiretor.getRowData(row);
        Europa.Controllers.Leads.ModalDetalhar(obj);
    };

    $scope.incluir = function (row) {
        $scope.rowEdit(row);
        var obj = Europa.Controllers.Leads.TabelaDiretor.getRowData(row);

        Europa.Controllers.Leads.IdLeadEmpresasVenda = obj.Id;
        Europa.Controllers.Leads.IdUsuarioCorretor = obj.IdUsuario;

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

Europa.Controllers.Leads.InitAutoCompleteCorretor = function () {
    Europa.Controllers.Leads.AutoCompleteCorretorLead = new Europa.Components.AutoCompleteCorretorEmpresaVenda()
        .WithTargetSuffix("corretor_lead")
        .Configure();
};

Europa.Controllers.Leads.PreSalvar = function () {
    var lead = {
        Id: Europa.Controllers.Leads.IdLeadEmpresasVenda,
        IdCorretor: $("#autocomplete_corretor_lead").val(),
        NomeCorretor: Europa.Controllers.Leads.AutoCompleteCorretorLead.Text()
    };

    $.post(Europa.Controllers.Leads.UrlAtribuirCorretorLead, lead, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.Leads.TabelaDiretor.closeEdition();
            Europa.Controllers.Leads.FiltrarDatatableDiretor();
        }
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.Leads.FiltrarDatatableDiretor = function () {

    var autorizar = Europa.Controllers.Leads.ValidarFiltroDiretor();

    if (!autorizar) {
        return;
    }

    if ($("#selecionar_leads").val() == "true") {
        Europa.Controllers.Leads.OnClickSelecionarLeads();
    }

    Europa.Controllers.Leads.TabelaDiretor.reloadData(undefined, false);

    setTimeout(function () {
        $(".selectAllDt").attr("disabled", "disabled");
        $(".deselectAllDt").attr("disabled", "disabled");
    }, 1000);
    
    Europa.Controllers.Leads.DescricaoPacote = $("#pacote_diretor").val();
};

Europa.Controllers.Leads.RemoverAtribuicao = function (id) {

    var lead = {
        Id: id
    };

    $.post(Europa.Controllers.Leads.UrlRemoverAtribuicao, lead, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.Leads.FiltrarDatatableDiretor();
        }
        Europa.Informacao.PosAcao(res);
    });
}