Europa.Controllers.GrupoCCA.IncluirGrupoCCA = false;

$(function () {})

function TableGrupoCCA($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.GrupoCCA.GrupoCCATable = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabela = Europa.Controllers.GrupoCCA.GrupoCCATable;
    tabela
        .setTemplateEdit([
            '<input type="text" class="form-control" name="Descricao" id="Descricao" maxlength="50">'
        ])
        .setColumns([
            DTColumnBuilder.newColumn('Descricao').withTitle(Europa.i18n.Messages.Descricao)
        ])
        .setColActions(actionsHtml, '100px')
        .setActionSave(Europa.Controllers.GrupoCCA.SalvarGrupoCCA)
        .setIdAreaHeader("GrupoCCA_datatable_barra")
        .setOptionsSelect('POST', Europa.Controllers.GrupoCCA.UrlListarGruposCCA, Europa.Controllers.GrupoCCA.FiltroGrupoCCA);

    function actionsHtml(data, type, full, meta) {
        var button = '<div>';
        button += $scope.renderButton(Europa.Controllers.GrupoCCA.Permissoes.Atualizar, "Editar", "fa fa-edit", "Editar(" + meta.row + ")");
        button += $scope.renderButton(Europa.Controllers.GrupoCCA.Permissoes.Excluir, "Excluir", "fa fa-trash", "Excluir(" + meta.row + ")");
        button += '</div >';
        return button;
    }

    $scope.onRowSelect = function (data) {
        if (Europa.Controllers.GrupoCCA.GrupoCCATable.getDataRowEdit().Id === undefined || Europa.Controllers.GrupoCCA.GrupoCCATable.getDataRowEdit().Descricao === undefined) {
            if (Europa.Controllers.GrupoCCA.GrupoCCAId !== undefined) {
                if (Europa.Controllers.GrupoCCA.GrupoCCAId !== data.Id) {
                    Europa.Controllers.GrupoCCA.CarregarDadosEmpresaVenda(data.Id);
                    Europa.Controllers.GrupoCCA.CarregarDadosUsuario(data.Id);
                } else {
                    Europa.Controllers.GrupoCCA.GrupoCCAId = undefined;
                    Europa.Controllers.GrupoCCA.EmpresaVendaTable.closeEdition();
                    Europa.Controllers.GrupoCCA.EmpresaVendaTable.reloadData();

                    Europa.Controllers.GrupoCCA.UsuarioTable.closeEdition();
                    Europa.Controllers.GrupoCCA.UsuarioTable.reloadData();
                }
            } else {
                Europa.Controllers.GrupoCCA.CarregarDadosEmpresaVenda(data.Id);
                Europa.Controllers.GrupoCCA.CarregarDadosUsuario(data.Id);
            }            
        }
    }

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (hasPermission !== 'True') {
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

    $scope.Editar = function (row) {
        Europa.Controllers.GrupoCCA.IncluirGrupoCCA = false;
        $scope.rowEdit(row);
    };

    $scope.Excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.GrupoCCA.GrupoCCATable.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.Descricao, function () {
            Europa.Controllers.GrupoCCA.ExcluirGrupoCCA(objetoLinhaTabela.Id);
        });
    };
};

DataTableApp.controller('GrupoCCADatatable', TableGrupoCCA);

Europa.Controllers.GrupoCCA.FiltroGrupoCCA = function () {
    var param = {
        Descricao: $("#filtroGrupos").val()
    };
    return param;
};

Europa.Controllers.GrupoCCA.FiltrarGrupoCCA = function () {
    Europa.Controllers.GrupoCCA.GrupoCCATable.reloadData();
};

Europa.Controllers.GrupoCCA.LimparFiltroGrupoCCA = function () {
    $("#filtroGrupos").val("");
};

Europa.Controllers.GrupoCCA.NovoGrupoCCA = function () {
    Europa.Controllers.GrupoCCA.GrupoCCATable.createRowNewData();
    Europa.Controllers.GrupoCCA.IncluirGrupoCCA = true;
};

Europa.Controllers.GrupoCCA.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.GrupoCCA.LimparErro = function () {
    $("[name='Descricao']").parent().removeClass("has-error");
};

Europa.Controllers.GrupoCCA.SalvarGrupoCCA = function () {
    var obj = Europa.Controllers.GrupoCCA.GrupoCCATable.getDataRowEdit();
    var url = Europa.Controllers.GrupoCCA.IncluirGrupoCCA ? Europa.Controllers.GrupoCCA.UrlIncluirGrupoCCA : Europa.Controllers.GrupoCCA.UrlAtualizarGrupoCCA

    $.post(url, { grupo: obj }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);
        if (resposta.Sucesso) {
            Europa.Controllers.GrupoCCA.GrupoCCATable.closeEdition();
            Europa.Controllers.GrupoCCA.GrupoCCATable.reloadData();
            Europa.Controllers.GrupoCCA.LimparErro();
        } else {
            Europa.Controllers.GrupoCCA.AdicionarErro(resposta.Campos);
        }
    });
};

Europa.Controllers.GrupoCCA.ExcluirGrupoCCA = function (id) {
  
    $.post(Europa.Controllers.GrupoCCA.UrlExcluirGrupoCCA, { id: id }, function (resposta) {        
        if (resposta.Sucesso) {
            Europa.Controllers.GrupoCCA.FiltrarGrupoCCA();
            Europa.Controllers.GrupoCCA.GrupoCCAId = 0;
            Europa.Controllers.GrupoCCA.FiltrarEmpresaVenda();
        }      
        Europa.Informacao.PosAcao(resposta);
    });
    
};