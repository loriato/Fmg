Europa.Controllers.PreProposta.Historico = {};

DataTableApp.controller('historicoPrePropostaTable', historicoPrePropostaTable);

function historicoPrePropostaTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.PreProposta.Historico.Table = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.PreProposta.Historico.Table;
    self.setColumns([
        DTColumnBuilder.newColumn('CodigoPreProposta').withTitle(Europa.i18n.Messages.Codigo).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeResponsavelInicio').withTitle(Europa.i18n.Messages.Responsavel).withOption('width', '150px').renderWith(renderNomeResponsavelIni),
        DTColumnBuilder.newColumn('SituacaoInicio').withTitle(Europa.i18n.Messages.Situacao).withOption('width', '150px').withOption('type', 'enum-format-SituacaoProposta'),
        DTColumnBuilder.newColumn('Inicio').withTitle(Europa.i18n.Messages.Inicio).withOption('width', '150px').withOption("type", "date-format-DD/MM/YYYY HH:mm:ss"),
        DTColumnBuilder.newColumn('NomeResponsavelTermino').withTitle(Europa.i18n.Messages.Responsavel).withOption('width', '150px').renderWith(renderNomeResponsavelTer),
        DTColumnBuilder.newColumn('SituacaoTermino').withTitle(Europa.i18n.Messages.Situacao).withOption('width', '150px').withOption('type', 'enum-format-SituacaoProposta'),
        DTColumnBuilder.newColumn('Termino').withTitle(Europa.i18n.Messages.Termino).withOption('width', '150px').withOption("type", "date-format-DD/MM/YYYY HH:mm:ss"),
        DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('width', '150px').withOption('type', 'enum-format-Situacao'),
    ])
        .setIdAreaHeader("historico_pre_proposta_datatable_header")
        .setDefaultOrder([[3, 'desc']])
        .setAutoInit(false)
        .setDefaultOptions('POST', Europa.Controllers.PreProposta.UrlHistorico, Europa.Controllers.PreProposta.Historico.FilterParams);

    function renderNomeResponsavelIni(data, type, full, meta) {
        if (full.NomeResponsavelInicio == 'Administrador') {
            return 'System'
        }
        if (full.NomePerfilCCAInicial != null && full.NomeResponsavelInicio != null) {
            return full.NomeResponsavelInicio + " (" + full.NomePerfilCCAInicial+")";
        }
        return full.NomeResponsavelInicio;
    }
    function renderNomeResponsavelTer(data, type, full, meta) {
        if (full.NomeResponsavelTermino == 'Administrador') {
            return 'System';
        }
        if (full.NomePerfilCCAFinal != null && full.NomeResponsavelTermino != null) {
            return full.NomeResponsavelTermino + " (" + full.NomePerfilCCAFinal + ")";
        }
        return full.NomeResponsavelTermino;
    }
};

Europa.Controllers.PreProposta.Historico.FilterParams = function () {
    return {
        idPreProposta: $('#PreProposta_Id').val()
    };
};

Europa.Controllers.PreProposta.Historico.Filtrar = function () {
    // O JS está sendo utilizado em outros locais, só que tais locais não possuem a referencia a essa tabela
    if (Europa.Controllers.PreProposta.Historico.Table == undefined) { return; }
    Europa.Controllers.PreProposta.Historico.Table.reloadData();
};
