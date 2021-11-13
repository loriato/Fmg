////////////////////////////////////////////////////////////////////////////////////
// Funções Datatable
////////////////////////////////////////////////////////////////////////////////////
DataTableApp.controller('brevesLancamentosTable', brevesLancamentosTable);

function brevesLancamentosTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.BreveLancamento.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.BreveLancamento.Tabela;
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '15%'),
        DTColumnBuilder.newColumn('NomeRegional').withTitle(Europa.i18n.Messages.Regional).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Cidade').withTitle(Europa.i18n.Messages.Cidade).withOption('width', '15%'),
        DTColumnBuilder.newColumn('Estado').withTitle(Europa.i18n.Messages.Estado).withOption('width', '15%'),
        DTColumnBuilder.newColumn('DisponibilizarCatalogo').withTitle(Europa.i18n.Messages.DisponibilizarNoCatalogo).withOption('width', '15%').renderWith(formatBoolean),
        DTColumnBuilder.newColumn('NomeEmpreendimento').withTitle(Europa.i18n.Messages.Empreendimento).withOption('width', '15%')
    ])
        .setTemplateEdit([
            '<input type="text" class="form-control" name="Nome" value="" readonly="true">',
            '<input type="text" class="form-control" name="NomeRegional" value="" readonly="true">',
            '<input type="text" class="form-control" name="Cidade" value="" readonly="true">',
            '<input type="text" class="form-control" name="Estado" value="" readonly="true">',
            '<input type="text" class="form-control" name="DisponibilizarCatalogo_Edit_Mode" value="" readonly="true">',
            '<select class="form-control" id="autocomplete_empreendimento_associacao" name="Empreendimento"/>'
        ])
        .setActionSave(Europa.Controllers.BreveLancamento.ConfirmarSalvarAssociacao)
        .setIdAreaHeader("datatable_header")
        .setColActions(actionsHtml, '10%')
        .setAutoInit()
        .setDefaultOptions('POST', Europa.Controllers.BreveLancamento.UrlListar, Europa.Controllers.BreveLancamento.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButtonDetail(Europa.Controllers.BreveLancamento.Permissoes.Visualizar, "Visualizar", "fa fa-eye", "detalhar(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonEdit(Europa.Controllers.BreveLancamento.Permissoes.Atualizar, "Editar", "fa fa-edit", "editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonAssociar(Europa.Controllers.BreveLancamento.Permissoes.AssociarEmpreendimento, "Associar", "fa fa-link", "associar(" + meta.row + ")", full.Situacao, meta.row) +
            $scope.renderButtonBook(Europa.Controllers.BreveLancamento.Permissoes.Atualizar, "Book do Breve Lançamento", "fa fa-folder-open-o", "book(" + meta.row + ")") +
            $scope.renderButtonDelete(Europa.Controllers.BreveLancamento.Permissoes.Excluir, "Excluir", "fa fa-trash", "excluir(" + meta.row + ")", full.Situacao) +
            '</div>';
    }


    function formatBoolean(data) {
        if (data === true) {
            return Europa.i18n.Messages.Sim;
        }
        return Europa.i18n.Messages.Nao;
    }

    $scope.renderButtonEdit = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 3) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.renderButtonAssociar = function (hasPermission, title, icon, onClick, situacao, row) {
        var objetoLinhaTabela = Europa.Controllers.BreveLancamento.Tabela.getRowData(row);
        if (hasPermission !== 'true' || situacao === 3 || objetoLinhaTabela.IdEmpreendimento) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.renderButtonBook = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 3) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.renderButtonDetail = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 3) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.renderButtonDelete = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 3) {
            return "";
        }
        icon = $('<i/>').addClass(icon);
        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        return button.prop('outerHTML');
    };

    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (hasPermission === 'true') {
            icon = $('<i/>').addClass(icon);
            var button = $('<a />')
                .addClass('btn btn-default')
                .attr('title', title)
                .attr('ng-click', onClick)
                .append(icon);
            return button.prop('outerHTML');
        } else {
            return null;
        }
    };

    $scope.associar = function (row) {
        $scope.rowEdit(row);
        var objetoLinhaTabela = Europa.Controllers.BreveLancamento.Tabela.getRowData(row);
        $("[name='DisponibilizarCatalogo_Edit_Mode']").val(formatBoolean(objetoLinhaTabela.DisponibilizarCatalogo));
        Europa.Controllers.BreveLancamento.AutoCompleteEmpreendimentoAssociacao = new Europa.Components.AutoCompleteEmpreendimento()
            .WithTargetSuffix("empreendimento_associacao");
        Europa.Controllers.BreveLancamento.AutoCompleteEmpreendimentoAssociacao.Data = function (params) {
            var data = {
                start: 0,
                pageSize: 10,
                filter: [
                    {
                        value: params.term,
                        column: this.param,
                        regex: true
                    },
                    {
                        value: true,
                        column: 'semAssociacaoBreveLancamento'
                    }
                ],
                order: [
                    {
                        value: "asc",
                        column: this.param
                    }
                ]
            };
            if (this.paramCallback) {
                var object = this.paramCallback();
                for (var i in object) {
                    data.filter.push({
                        value: object[i],
                        column: i
                    })
                }
            }
            return data;
        };
        Europa.Controllers.BreveLancamento.AutoCompleteEmpreendimentoAssociacao.Configure();
        if (objetoLinhaTabela.IdEmpreendimento) {
            Europa.Controllers.BreveLancamento.AutoCompleteEmpreendimentoAssociacao.SetValue(objetoLinhaTabela.IdEmpreendimento, objetoLinhaTabela.NomeEmpreendimento)
        }
    };

    $scope.editar = function (row) {
        var objetoLinhaTabela = Europa.Controllers.BreveLancamento.Tabela.getRowData(row);
        Europa.Controllers.BreveLancamento.Editar(objetoLinhaTabela.Id);
    };

    $scope.detalhar = function (row) {
        var objetoLinhaTabela = Europa.Controllers.BreveLancamento.Tabela.getRowData(row);
        Europa.Controllers.BreveLancamento.Detalhar(objetoLinhaTabela.Id);
    };

    $scope.excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.BreveLancamento.Tabela.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.Nome, function () {
            $.post(Europa.Controllers.BreveLancamento.UrlExcluir, {idBreveLancamento: objetoLinhaTabela.Id}, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.BreveLancamento.Tabela.reloadData();
                    Europa.Informacao.PosAcao(res);
                } else {
                    Europa.Informacao.PosAcao(res);
                }
            });
        });
    };

    $scope.book = function (row) {
        var objetoLinhaTabela = Europa.Controllers.BreveLancamento.Tabela.getRowData(row);
        Europa.Controllers.BookBreveLancamento.Modal.Show(objetoLinhaTabela.Id, undefined);
    };
}

Europa.Controllers.BreveLancamento.FilterParams = function () {
    var idEmpreendimento = null;
    if (Europa.Controllers.BreveLancamento.AutoCompleteEmpreendimentoFiltro) {
        idEmpreendimento = Europa.Controllers.BreveLancamento.AutoCompleteEmpreendimentoFiltro.Value();
    }
    var filtro = {
        nome: $('#filtro_nome').val(),
        cidade: $('#filtro_cidade').val(),
        estados: $('#filtro_estados').val(),
        idEmpreendimento: idEmpreendimento,
        idRegional: $('#autocomplete_regional_filtro').val()
    };
    return filtro;
};

Europa.Controllers.BreveLancamento.FiltrarTabela = function () {
    Europa.Controllers.BreveLancamento.Tabela.reloadData();
};

Europa.Controllers.BreveLancamento.LimparFiltro = function () {
    $('#filtro_nome').val("");
    $('#filtro_cidade').val("");
    $('#filtro_estados').val("").trigger('change');
    $('#autocomplete_regional_filtro').val("").trigger('change');;
    if (Europa.Controllers.BreveLancamento.AutoCompleteEmpreendimentoFiltro) {
        Europa.Controllers.BreveLancamento.AutoCompleteEmpreendimentoFiltro.Clean();
    }
};

Europa.Controllers.BreveLancamento.ConfirmarSalvarAssociacao = function () {
    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Confirmacao
        , Europa.i18n.Messages.ConfirmarAssociacaoBreveLancamento);
    Europa.Confirmacao.ConfirmCallback = Europa.Controllers.BreveLancamento.SalvarAssociacao;
    Europa.Confirmacao.Show();
};

Europa.Controllers.BreveLancamento.SalvarAssociacao = function () {
    var objetoLinhaTabela = Europa.Controllers.BreveLancamento.Tabela.getDataRowEdit();

    $.post(Europa.Controllers.BreveLancamento.UrlAssociarComEmpreendimento
        , {
            idEmpreendimento: objetoLinhaTabela.Empreendimento,
            idBreveLancamento: objetoLinhaTabela.Id
        }, function (res) {
            if (res.Sucesso) {
                Europa.Controllers.BreveLancamento.Tabela.closeEdition();
                Europa.Controllers.BreveLancamento.Tabela.reloadData();
            }

            Europa.Informacao.PosAcao(res);
        });
}