DataTableApp.controller('ConsultaAceitesContratosDatatable', ConsultaAceitesContratosDatatable);

function ConsultaAceitesContratosDatatable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.ConsultaAceitesContratos.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.ConsultaAceitesContratos.Tabela;

    self.setColumns([
        DTColumnBuilder.newColumn('CodigoFornecedor').withTitle('Código do Fornecedor').withOption('width', '150px'),
        DTColumnBuilder.newColumn('RazaoSocial').withTitle('Razão Social').withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeFantasia').withTitle('Nome Fantasia').withOption('width', '150px'),
        DTColumnBuilder.newColumn('CNPJ').withTitle('CNPJ').withOption('width', '150px'),
        DTColumnBuilder.newColumn('InscricaoMunicipal').withTitle('Inscrição Municipal').withOption('width', '150px'),
        DTColumnBuilder.newColumn('CreciJuridico').withTitle('Creci Jurídico').withOption('width', '150px'),
        DTColumnBuilder.newColumn('InscricaoEstadual').withTitle('Inscrição Estadual').withOption('width', '150px'),
        DTColumnBuilder.newColumn('LegislacaoFederal').withTitle('Legislação Federal').withOption('width', '150px'),
        DTColumnBuilder.newColumn('Simples').withTitle('Simples').withOption('width', '150px'),
        DTColumnBuilder.newColumn('SIMEI').withTitle('SIMEI').withOption('width', '150px'),
        DTColumnBuilder.newColumn('LucroPresumido').withTitle('Lucro Presumido').withOption('width', '150px'),
        DTColumnBuilder.newColumn('LucroReal').withTitle('Lucro Real').withOption('width', '150px'),
        DTColumnBuilder.newColumn('SituacaoEmpresaVenda').withTitle('Situação da Empresa de Venda').withOption('width', '150px'),
        DTColumnBuilder.newColumn('Cep').withTitle('Cep').withOption('width', '150px'),
        DTColumnBuilder.newColumn('Cidade').withTitle('Cidade').withOption('width', '150px'),
        DTColumnBuilder.newColumn('Estado').withTitle('Estado').withOption('width', '150px'),
        DTColumnBuilder.newColumn('Bairro').withTitle('Bairro').withOption('width', '150px'),
        DTColumnBuilder.newColumn('Complemento').withTitle('Complemento').withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
        DTColumnBuilder.newColumn('Numero').withTitle('Número').withOption('width', '150px'),
        DTColumnBuilder.newColumn('Logradouro').withTitle('Logradouro').withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeCorretor').withTitle('Nome do Corretor').withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeLoja').withTitle('Nome da Loja').withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeResponsavelTecnico').withTitle('Nome do Responsável Técnico').withOption('width', '150px'),
        DTColumnBuilder.newColumn('CategoriaEmpresaVenda').withTitle('Categoria da Empresa de Venda').withOption('width', '150px'),
        DTColumnBuilder.newColumn('NumeroRegistroCRECI').withTitle('Número Registro CRECI').withOption('width', '150px'),
        DTColumnBuilder.newColumn('SituacaoResponsavel').withTitle('Situação do Responsável').withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomePessoaContato').withTitle('Nome da Pessoa de Contato').withOption('width', '150px'),
        DTColumnBuilder.newColumn('CorretorVisualizarClientes').withTitle('Corretor Pode Visualizar Clientes?').withOption('width', '150px'),
        DTColumnBuilder.newColumn('AceiteIdContratoCorretagem').withTitle('Id do Aceite do Contrato de Corretagem').withOption('width', '150px'),
        DTColumnBuilder.newColumn('DtAceite').withTitle('Data de Aceite').withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
        DTColumnBuilder.newColumn('NomeUsuario').withTitle('Nome do Usuário').withOption('width', '150px'),
        DTColumnBuilder.newColumn('EmailUsuario').withTitle('Email do Usuário').withOption('width', '150px'),
        DTColumnBuilder.newColumn('LoginUsuario').withTitle('Login do Usuário').withOption('width', '150px'),
        DTColumnBuilder.newColumn('UsuarioTipoSituacao').withTitle('Tipo de Situação do Usuário').withOption('width', '150px'),
        DTColumnBuilder.newColumn('ContratoIdCriadoPor').withTitle('Contrato Criado Por').withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeUsuarioContratoCriadoPor').withTitle('Nome do Usuário Criador do Contrato').withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeUsuarioContratoAtualizadoPor').withTitle('Nome do Atualizador do Contrato').withOption('width', '150px'),
        DTColumnBuilder.newColumn('ContratoDtAtualizadoEm').withTitle('Contrato Atualizado Em').withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
        DTColumnBuilder.newColumn('ContentTypeDoubleCheck').withTitle('Content Type Double Check').withOption('width', '150px'),
        DTColumnBuilder.newColumn('DoubleCheck').withTitle('Double Check').withOption('width', '150px'),
        DTColumnBuilder.newColumn('CodigoToken').withTitle('Codigo Token').withOption('width', '150px'),
        DTColumnBuilder.newColumn('Ativo').withTitle('Ativo').withOption('width', '150px'),
        DTColumnBuilder.newColumn('AcessoDtCriadoEm').withTitle('Acesso Criado Em').withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
        DTColumnBuilder.newColumn('InicioSessao').withTitle('Inicio da Sessão').withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
        DTColumnBuilder.newColumn('FimSessao').withTitle('Fim da Sessão').withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
        DTColumnBuilder.newColumn('IpOrigem').withTitle('Ip de Origem').withOption('width', '150px'),
        DTColumnBuilder.newColumn('CodigoAutorizacao').withTitle('Código de Autorização').withOption('width', '150px'),
        DTColumnBuilder.newColumn('Servidor').withTitle('Servidor').withOption('width', '150px'),
        DTColumnBuilder.newColumn('Navegador').withTitle('Navegador').withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeSistema').withTitle('Nome do Sistema').withOption('width', '150px'),
        DTColumnBuilder.newColumn('ArquivoIdCriadoPor').withTitle('Criador do Arquivo').withOption('width', '150px'),
        DTColumnBuilder.newColumn('ArquivoDtCriadoEm').withTitle('Arquivo Criado Em').withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
        DTColumnBuilder.newColumn('ArquvioIdAtualizadoPor').withTitle('Atualizador do Arquvio').withOption('width', '150px'),
        DTColumnBuilder.newColumn('ArquivoDtAtualizadoEm').withTitle('Atualização do Arquivo').withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
        DTColumnBuilder.newColumn('NomeArquivo').withTitle('Nome do Arquivo').withOption('width', '150px'),
        DTColumnBuilder.newColumn('CodigoHash').withTitle('Código Hash').withOption('width', '150px'),
        DTColumnBuilder.newColumn('ContentType').withTitle('Content Type').withOption('width', '150px'),
        DTColumnBuilder.newColumn('ByThumbnail').withTitle('By Thumbnail').withOption('width', '150px'),
        DTColumnBuilder.newColumn('NrContentLength').withTitle('Content Length').withOption('width', '150px'),
        DTColumnBuilder.newColumn('FileExtension').withTitle('File Extension').withOption('width', '150px'),
        DTColumnBuilder.newColumn('Metadados').withTitle('Metadados').withOption('width', '150px'),
        DTColumnBuilder.newColumn('FalhaExtMetadados').withTitle('Falha Ext Metadados').withOption('width', '150px'),
    ])
        .setAutoInit()
        .setColActions(actionsHtml, '100px')
        .setIdAreaHeader("datatable_header")
        .setDefaultOrder([[2, 'asc']])
        .setOptionsMultiSelect('POST', Europa.Controllers.ConsultaAceitesContratos.UrlListar, Europa.Controllers.ConsultaAceitesContratos.Filtro);


    function actionsHtml(data, type, full, meta) {
        console.log(full.IdArquivo)
        return '<div>' +
            $scope.renderButtonDownload(Europa.Controllers.ConsultaAceitesContratos.Permissoes.Baixar, "Baixar Arquivo", "fa fa-download", "Baixar(" + full.IdArquivo + ")", full.IdArquivo) +
            '</div>';
    }


    $scope.renderButtonDownload = function (hasPermission, title, icon, onClick, situacao) {
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
    }


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
    }

    $scope.Baixar = function (idArquivo){
        console.log(idArquivo)

        //var params = {
        //    idArquivo: idArquivo
        //};
        location.href = Europa.Controllers.ConsultaAceitesContratos.UrlBaixarContrato + "?idArquivo=" + idArquivo;
        

        //$.post(Europa.Controllers.ConsultaAceitesContratos.UrlBaixarContrato, params, function (response) {
            //if (!response.Sucesso) {
            //    Europa.Informacao.PosAcao(response);
            //} else {
            //    var formExportar = $("#Exportar");
            //    $("#Exportar").find("input").remove();
            //    formExportar.attr("method", "post").attr("action", self.UrlBaixarBoleto);
            //    formExportar.addHiddenInputData(params);
            //    formExportar.submit();
            //}
        //    console.log(response);
    //    });
    }

};