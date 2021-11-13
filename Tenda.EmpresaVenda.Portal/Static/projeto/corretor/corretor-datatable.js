
////////////////////////////////////////////////////////////////////////////////////
// Funções Datatable
////////////////////////////////////////////////////////////////////////////////////
DataTableApp.controller('corretoresTable', corretoresTable);

function corretoresTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.Corretor.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var tabelaWrapper = Europa.Controllers.Corretor.Tabela;
    tabelaWrapper.setColumns([
        DTColumnBuilder.newColumn('Situacao').withTitle("").withClass("datatable-actions center-btn-actions datatable_item")
            .withOption('width', '2%').withOption('type', 'enum-format-Situacao').notSortable().renderWith(formatStatus),
        DTColumnBuilder.newColumn('CriadoEm').withTitle("DATA DE CADASTRO")
            .withClass("datatable_item").withOption("type", "date-format-DD/MM/YYYY").withOption('width', '12%'),
        DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome.toUpperCase())
            .withClass("datatable_item").withOption('width', '20%'),
        //DTColumnBuilder.newColumn('Apelido').withTitle(Europa.i18n.Messages.NomeGuerra).withOption('width', '12%'),
        DTColumnBuilder.newColumn('Cpf').withTitle(Europa.i18n.Messages.Cpf.toUpperCase())
            .withClass("datatable_item").withOption('width', '10%').renderWith(formatCPF),
        DTColumnBuilder.newColumn('Rg').withTitle(Europa.i18n.Messages.RG.toUpperCase())
            .withClass("datatable_item").withOption('width', '10%'),
        //DTColumnBuilder.newColumn('Cnpj').withTitle(Europa.i18n.Messages.Cnpj).withOption('width', '10%').renderWith(formatCNPJ),
        //DTColumnBuilder.newColumn('Creci').withTitle(Europa.i18n.Messages.Creci).withOption('width', '10%'),
        //DTColumnBuilder.newColumn('Telefone').withTitle(Europa.i18n.Messages.Telefone).withOption('width', '10%').renderWith(formatTelefone),
        DTColumnBuilder.newColumn('Funcao').withTitle("Cargo".toUpperCase())
            .withClass("datatable_item").withOption('width', '10%').withOption('type', 'enum-format-TipoFuncao'),
        //DTColumnBuilder.newColumn('Email').withTitle(Europa.i18n.Messages.Email).withClass("datatable_item").withOption('width', '10%'),
        DTColumnBuilder.newColumn('DataCredenciamento').withTitle("Data de Admissão".toUpperCase())
            .withClass("datatable_item").withOption('width', '10%').withOption("type", "date-format-DD/MM/YYYY"),
        DTColumnBuilder.newColumn('Perfis').withTitle(Europa.i18n.Messages.Perfil.toUpperCase()).withClass("datatable_item").withOption('width', '200px'),
        DTColumnBuilder.newColumn(null).withTitle("").withClass("datatable-actions center-btn-actions")
            .withClass("datatable_item").withOption("width", "10%").notSortable().renderWith(actionsHtml)
    ])
        .setShowStatus()
        .setDefaultOrder([[2, 'asc']])
        .setIdAreaHeader("datatable_header")
        .setAutoInit()
        .setOptionsMultiSelect('POST', Europa.Controllers.Corretor.UrlListar, Europa.Controllers.Corretor.FilterParams);
    
    function actionsHtml(data, type, full, meta) {
        return '<div>' +
            $scope.renderButtonPassword(Europa.Controllers.Corretor.Permissoes.Atualizar, "Reenviar Token Ativação", "fa fa-key", "reenviarTokenAtivacao(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonDetail(Europa.Controllers.Corretor.Permissoes.Visualizar, "Visualizar", "fa fa-eye", "detalhar(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonEdit(Europa.Controllers.Corretor.Permissoes.Atualizar, "Editar", "fa fa-edit", "editar(" + meta.row + ")", full.Situacao) +
            $scope.renderButtonDelete(Europa.Controllers.Corretor.Permissoes.Excluir, "Remover", "fa fa-trash", "excluir(" + meta.row + ")", full.Situacao) +
            '</div>';
    }

    function formatCNPJ(data) {
        return Europa.Mask.GetMaskedValue(data, Europa.Mask.FORMAT_CNPJ);
    }

    function formatCPF(data) {
        return Europa.Mask.GetMaskedValue(data, Europa.Mask.FORMAT_CPF);
    }

    function formatTelefone(data) {
        if (data.length === 8) {
            return Europa.Mask.GetMaskedValue(data, Europa.Mask.FORMAT_TELEFONE_8);
        } else {
            return Europa.Mask.GetMaskedValue(data, Europa.Mask.FORMAT_TELEFONE_9);
        }
    }
    
    function formatStatus(data) {
        var color = "--steel";
        var title = "";
        if(data == 1){
            color = "--tealish";
            title = Europa.i18n.Messages.SituacaoUsuario_Ativo;
        }else if (data == 2){
            color = "--marigold";
            title = Europa.i18n.Messages.SituacaoUsuario_Suspenso;
        } else if (data == 3) {
            title = Europa.i18n.Messages.SituacaoUsuario_Cancelado;
        }
        var html = "<i title='" + title + "' class='fa fa-circle-o' style='color: var(" + color + ");' aria-hidden='true'></i>";
        return html;
    }

    $scope.renderButtonEdit = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 3) {
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
    $scope.renderButtonPassword = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 2 || situacao === 3) {
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
    $scope.renderButtonDetail = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 3) {
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

    $scope.renderButtonDelete = function (hasPermission, title, icon, onClick, situacao) {
        if (hasPermission !== 'true' || situacao === 3) {
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
    $scope.renderButton = function (hasPermission, title, icon, onClick) {
        if (hasPermission === 'true') {
            icon = $('<i/>').addClass(icon);
            var button = $('<a />')
                .addClass('btn btn-steel')
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
    };

    $scope.detalhar = function (row) {
        var objetoLinhaTabela = Europa.Controllers.Corretor.Tabela.getRowData(row);
        Europa.Controllers.Corretor.Detalhar(objetoLinhaTabela.Id);

        Europa.Controllers.Corretor.AutoCompletePerfilPortal.Disable();
    };

    $scope.reenviarTokenAtivacao = function (row) {
        var model = Europa.Controllers.Corretor.Tabela.getRowData(row);
        $.get(Europa.Controllers.Corretor.UrlReenviarTokenAtivacao, { idCorretor: model.Id }, function (res) {
            Europa.Informacao.PosAcao(res);
        });
    };

    $scope.excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.Corretor.Tabela.getRowData(row);
        Europa.Controllers.Corretor.Excluir(objetoLinhaTabela.Id);
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
        Situacao: $('#filtro_situacoes').val()
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
};