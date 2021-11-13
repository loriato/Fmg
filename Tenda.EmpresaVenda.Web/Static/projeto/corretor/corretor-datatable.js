
////////////////////////////////////////////////////////////////////////////////////
// Funções Datatable
////////////////////////////////////////////////////////////////////////////////////
DataTableApp.controller('corretoresTable', corretoresTable);

function corretoresTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.Corretor.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.Corretor.Tabela;
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('NomeEmpresaVenda').withTitle(Europa.i18n.Messages.EmpresaVendas).withOption('width', '13%'),
        DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '13%'),
        DTColumnBuilder.newColumn('Apelido').withTitle(Europa.i18n.Messages.NomeGuerra).withOption('width', '10%'),
        DTColumnBuilder.newColumn('Cpf').withTitle(Europa.i18n.Messages.Cpf).withOption('width', '8%').renderWith(formatCPF),
        DTColumnBuilder.newColumn('Creci').withTitle(Europa.i18n.Messages.Creci).withOption('width', '10%'),
        DTColumnBuilder.newColumn('Telefone').withTitle(Europa.i18n.Messages.Telefone).withOption('width', '10%').renderWith(formatTelefone),
        DTColumnBuilder.newColumn('Email').withTitle(Europa.i18n.Messages.Email).withOption('width', '10%'),
        DTColumnBuilder.newColumn('Funcao').withTitle(Europa.i18n.Messages.Funcao).withOption('width', '7%').withOption('type', 'enum-format-TipoFuncao'),
        DTColumnBuilder.newColumn('DataCredenciamento').withTitle(Europa.i18n.Messages.DataParceria).withOption('width', '12%').withOption("type", "date-format-DD/MM/YYYY"),
        DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('width', '10%').withOption('type', 'enum-format-Situacao'),
        DTColumnBuilder.newColumn('Perfis').withTitle(Europa.i18n.Messages.Perfil).withOption('width', '200px')
    ])
        .setIdAreaHeader("datatable_header")
        .setColActions(actionsHtml, '10%')
        .setAutoInit()
        .setOptionsMultiSelect('POST', Europa.Controllers.Corretor.UrlListar, Europa.Controllers.Corretor.FilterParams);

    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButtonPassword(Europa.Controllers.Corretor.Permissoes.Atualizar, "Reenviar Token Ativação", "fa fa-key", "reenviarTokenAtivacao(" + meta.row + ")") +
            $scope.renderButtonDetail(Europa.Controllers.Corretor.Permissoes.Visualizar, "Visualizar", "fa fa-eye", "detalhar(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonEdit(Europa.Controllers.Corretor.Permissoes.Atualizar, "Editar", "fa fa-edit", "editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonDelete(Europa.Controllers.Corretor.Permissoes.Excluir, "Excluir", "fa fa-trash", "excluir(" + meta.row + ")", full.Situacao) +
            '</div>';
    }

    function formatCNPJ(data) {
        if (data != null) {
            return Europa.Mask.GetMaskedValue(data, Europa.Mask.FORMAT_CNPJ);
        }
        return "";
    }

    
    function formatCPF(data) {
        if (data != null) {
            return Europa.Mask.GetMaskedValue(data, Europa.Mask.FORMAT_CPF);
        }
        return "";
    }

    function formatTelefone(data) {
        if (data != null) {
            if (data.length === 8) {
                return Europa.Mask.GetMaskedValue(data, Europa.Mask.FORMAT_TELEFONE_8);
            } else {
                return Europa.Mask.GetMaskedValue(data, Europa.Mask.FORMAT_TELEFONE_9);
            }
        }
        return "";
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
    $scope.renderButtonPassword = function (hasPermission, title, icon, onClick) {
        if (hasPermission !== 'true') {
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

    function registrosSelecionados() {
        var itens = Europa.Controllers.Corretor.Tabela.getRowsSelect();
        var registros = [];

        itens.forEach(function (item) {
            registros.push(item);
        });

        if (registros.length < 1) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.i18n.Messages.NenhumRegistroSelecionando);
            Europa.Informacao.Show();
            return null;
        }
        return registros;
    }

    $scope.editar = function (row) {
        var objetoLinhaTabela = Europa.Controllers.Corretor.Tabela.getRowData(row);
        Europa.Controllers.Corretor.Editar(objetoLinhaTabela.Id);

        Europa.Controllers.Corretor.AutoCompletePerfilPortal.Enable();
        Europa.Controllers.Corretor.PreencherPerfil(objetoLinhaTabela);

    };

    $scope.detalhar = function (row) {
        var objetoLinhaTabela = Europa.Controllers.Corretor.Tabela.getRowData(row);
        Europa.Controllers.Corretor.Detalhar(objetoLinhaTabela.Id);

        Europa.Controllers.Corretor.AutoCompletePerfilPortal.Disable();
        Europa.Controllers.Corretor.PreencherPerfil(objetoLinhaTabela);
    };

    $scope.reenviarTokenAtivacao = function (row) {
        var model = Europa.Controllers.Corretor.Tabela.getRowData(row);
        $.get(Europa.Controllers.Corretor.UrlReenviarTokenAtivacao, { idCorretor: model.Id }, function (res) {
            Europa.Informacao.PosAcao(res);
        });
    };


    $scope.excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.Corretor.Tabela.getRowData(row);
        Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, objetoLinhaTabela.Nome, function () {
            $.post(Europa.Controllers.Corretor.UrlExcluir, { idCorretor: objetoLinhaTabela.Id }, function (res) {
                if (res.Sucesso) {
                    Europa.Controllers.Corretor.Tabela.reloadData();
                    Europa.Informacao.PosAcao(res);
                } else {
                    Europa.Informacao.PosAcao(res);
                }
            });
        });
    };

    $scope.reativarItens = function () {
        var registros = registrosSelecionados();
        if (registros !== null) {
            Europa.Confirmacao.PreAcaoMulti(Europa.i18n.Messages.Ativar, function () {
                var idRegs = [];
                registros.forEach(function (item) {
                    idRegs.push(item.Id);
                });
                $.ajax({
                    url: Europa.Controllers.Corretor.UrlReativar,
                    dataType: 'json',
                    data: { idsCorretores: idRegs },
                    type: 'POST'
                }).done(function (data) {
                    if (data.Sucesso) {
                        Europa.Controllers.Corretor.FiltrarTabela();
                    }
                    Europa.Informacao.PosAcao(data);
                });
            });
        }
    };

    $scope.suspenderItens = function () {
        var registros = registrosSelecionados();
        if (registros !== null) {
            Europa.Confirmacao.PreAcaoMulti(Europa.i18n.Messages.Suspender, function () {
                var idRegs = [];
                registros.forEach(function (item) {
                    idRegs.push(item.Id);
                });
                $.ajax({
                    url: Europa.Controllers.Corretor.UrlSuspender,
                    dataType: 'json',
                    data: { idsCorretores: idRegs },
                    type: 'POST'
                }).done(function (data) {
                    if (data.Sucesso) {
                        Europa.Controllers.Corretor.FiltrarTabela();
                    }
                    Europa.Informacao.PosAcao(data);
                });
            });
        }
    };

    $scope.cancelarItens = function () {
        var registros = registrosSelecionados();
        if (registros !== null) {
            Europa.Confirmacao.PreAcaoMulti(Europa.i18n.Messages.Cancelar, function () {
                var idRegs = [];
                registros.forEach(function (item) {
                    idRegs.push(item.Id);
                });
                $.ajax({
                    url: Europa.Controllers.Corretor.UrlCancelar,
                    dataType: 'json',
                    data: { idsCorretores: idRegs },
                    type: 'POST'
                }).done(function (data) {
                    if (data.Sucesso) {
                        Europa.Controllers.Corretor.FiltrarTabela();
                    }
                    Europa.Informacao.PosAcao(data);
                });
            });
        }
    };
}

Europa.Controllers.Corretor.FilterParams = function () {
    var filtro = {
        Nome: $('#filtro_nome').val(),
        CpfCnpjCreci: $('#filtro_cpf_cnpj_creci').val(),
        Situacao: $('#filtro_situacoes').val(),
        EmpresaVenda: $('#filtro_empresa_vendas').val(),
        Funcao: $('#filtro_funcao').val(),
        Perfil: $('#filtro_perfil').val(),
        IdRegional: $('#autocomplete_regional').val(),
        UF: $('#filtro_estados').val()
    };
    return filtro;
};

Europa.Controllers.Corretor.FiltrarTabela = function () {
    Europa.Controllers.Corretor.Tabela.reloadData();
};

Europa.Controllers.Corretor.LimparFiltro = function () {
    $('#filtro_nome').val("");
    $('#filtro_cpf_cnpj_creci').val("");
    $('#filtro_situacoes').val(1).trigger('change');
    $('#filtro_empresa_vendas').val("");
    $('#filtro_funcao').val("").trigger('change');
    $('#filtro_perfil').val(" ");
    $('#autocomplete_regional').val("").trigger('change');
    $('#filtro_estados').val("").trigger('change');
};

Europa.Controllers.Corretor.ExportarPagina = function () {
    var params = Europa.Controllers.Corretor.FilterParams();
    params.order = Europa.Controllers.Corretor.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.Corretor.Tabela.lastRequestParams.draw;
    params.pageSize = Europa.Controllers.Corretor.Tabela.lastRequestParams.pageSize;
    params.start = Europa.Controllers.Corretor.Tabela.lastRequestParams.start;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.Corretor.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.Corretor.ExportarTodos = function () {
    var params = Europa.Controllers.Corretor.FilterParams();
    params.order = Europa.Controllers.Corretor.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.Corretor.Tabela.lastRequestParams.draw;
    var formExportar = $("#Exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.Corretor.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};