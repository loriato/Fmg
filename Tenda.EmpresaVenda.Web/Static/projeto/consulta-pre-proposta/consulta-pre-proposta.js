"use strict";

Europa.Controllers.ConsultaPreProposta = {};
Europa.Controllers.ConsultaPreProposta.Table = {}; 
Europa.Controllers.ConsultaPreProposta.ExibindoInfo = false;

$(function () {
    Europa.Controllers.ConsultaPreProposta.AlternarExibicaoInformacoes();
    Europa.Components.DatePicker.AutoApply();
    setTimeout(Europa.Controllers.ConsultaPreProposta.ConfigDatePicker, 300);

    Europa.Controllers.ConsultaPreProposta.AutoCompleteEmpresaVenda = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("empresa_venda")
        .Configure();

    Europa.Controllers.ConsultaPreProposta.AutoCompletePontoVenda = new Europa.Components.AutoCompletePontoVenda()
        .WithTargetSuffix("ponto_venda")
        .Configure();

    Europa.Controllers.ConsultaPreProposta.AutoCompleteCorretor = new Europa.Components.AutoCompleteCorretor()
        .WithTargetSuffix("corretor")
        .Configure();

    Europa.Controllers.ConsultaPreProposta.AutoCompleteBreveLancamento = new Europa.Components.AutoCompleteBreveLancamento()
        .WithTargetSuffix("breve_lancamento")
        .Configure();

    Europa.Controllers.ConsultaPreProposta.AutoCompleteViabilizador = new Europa.Components.AutoCompleteViabilizador()
        .WithTargetSuffix("viabilizador")
        .Configure();

    Europa.Controllers.ConsultaPreProposta.AutoCompleteStandVenda = new Europa.Components.AutoCompleteStandVenda()
        .WithTargetSuffix("stand_venda")
        .Configure();

    Europa.Controllers.ConsultaPreProposta.AutoCompleteRegionais = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regional")
        .Configure();

    $("#situacao_pre_proposta").select2({
        trags: true,
        width: '100%'
    });

    $("#filtro_estados").select2({
        trags: true,
        width: '100%'
    });


    $("#situacao_avalista").select2({
        trags: true,
        width: '100%'
    });

    Europa.Controllers.ConsultaPreProposta.ConfigureEmpresaVendaAutocomplete(Europa.Controllers.ConsultaPreProposta);

});

Europa.Controllers.ConsultaPreProposta.ConfigDatePicker = function () {
    Europa.Controllers.ConsultaPreProposta.ElaboracaoDe = new Europa.Components.DatePicker()
        .WithTarget("#ElaboracaoDe")
        .WithFormat("DD/MM/YYYY")
        .Configure();

    Europa.Controllers.ConsultaPreProposta.ElaboracaoAte = new Europa.Components.DatePicker()
        .WithTarget("#ElaboracaoAte")
        .WithFormat("DD/MM/YYYY")
        .Configure();
};

Europa.Controllers.ConsultaPreProposta.ConfigureEmpresaVendaAutocomplete = function (autocompleteWrapper) {
    autocompleteWrapper.AutoCompleteEmpresaVenda.Data = function (params) {
        return {
            start: 0,
            pageSize: 10,
            filter: [
                {
                    value: params.term,
                    column: this.param,
                    regex: true
                },
                {
                    value: function () {
                        return $("#filtro_estados").val();
                    },
                    column: 'regional'
                },
                {
                    value: function () {
                        return $("#autocomplete_stand_venda").val();
                    },
                    column:'standVenda'
                }
            ],
            order: [
                {
                    value: "asc",
                    column: this.param
                }
            ]
        };
    };

    $('#autocomplete_empresa_venda').on('change', function (e) {
        autocompleteWrapper.AutoCompletePontoVenda.Clean();
    });
    autocompleteWrapper.AutoCompleteEmpresaVenda.Configure();
}

Europa.Controllers.ConsultaPreProposta.ConfigurePontoVendaAutocomplete = function (autocompleteWrapper) {
    autocompleteWrapper.AutoCompletePontoVenda.Data = function (params) {
        return {
            start: 0,
            pageSize: 10,
            filter: [
                {
                    value: params.term,
                    column: this.param,
                    regex: true
                },
                {
                    value: function () {
                        return $("#autocomplete_empresa_venda").val();
                    },
                    column: 'idEmpresaVenda'
                },
                {
                    value: function () {
                        return $("#autocomplete_stand_venda").val();
                    },
                    column: 'standVenda'
                }
            ],
            order: [
                {
                    value: "asc",
                    column: this.param
                }
            ]
        };
    };
    autocompleteWrapper.AutoCompletePontoVenda.Configure();
}

Europa.Controllers.ConsultaPreProposta.OnChangeEmpresaFiltro = function () {
    var idEmp = Europa.Controllers.ConsultaPreProposta.AutoCompleteEmpresaVenda.Value();
    if (idEmp > 0) {
        if (idEmp !== undefined) {
            Europa.Controllers.ConsultaPreProposta.AutoCompletePontoVenda.Clean();
            Europa.Controllers.ConsultaPreProposta.ConfigurePontoVendaAutocomplete(Europa.Controllers.ConsultaPreProposta);
        }
    } else {
        Europa.Controllers.ConsultaPreProposta.AutoCompletePontoVenda.Clean();
    }
};

////////////////////////////////////////////////////////////////////////////////////
// Funções Datatable
////////////////////////////////////////////////////////////////////////////////////

$(function () {
    $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
    $(".ng-scope").css("word-wrap", "break-word");
});

DataTableApp.controller('consultaPrePropostaTable', consultaPrePropostaTable);

function consultaPrePropostaTable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.ConsultaPreProposta.Table = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.ConsultaPreProposta.Table;
    self.setColumns([
        DTColumnBuilder.newColumn('Regional').withTitle(Europa.i18n.Messages.Regional).withOption('width', '150px'),
        DTColumnBuilder.newColumn('UF').withTitle(Europa.i18n.Messages.UF).withOption('width', '100px'),
        DTColumnBuilder.newColumn('Codigo').withTitle(Europa.i18n.Messages.Codigo).withOption('width', '150px')
            .withOption("link", self.withOptionLink(Europa.Components.DetailAction.PreProposta, "Id")),
        DTColumnBuilder.newColumn('NomeCliente').withTitle(Europa.i18n.Messages.Cliente).withOption('width', '250px')
            .withOption("link", self.withOptionLink(Europa.Components.DetailAction.Cliente, "IdCliente")),
        DTColumnBuilder.newColumn('CpfCnpj').withTitle(Europa.i18n.Messages.CpfCnpj).withOption('width', '150px').renderWith(Europa.String.FormatCpf),
        DTColumnBuilder.newColumn('SituacaoPrePropostaSuatEvs').withTitle(Europa.i18n.Messages.SituacaoPreProposta).withOption('width', '150px'),
        DTColumnBuilder.newColumn('SituacaoAvalista').withTitle(Europa.i18n.Messages.SituacaoAvalista).withOption('width', '150px').withOption('type', 'enum-format-SituacaoAvalista'),
        DTColumnBuilder.newColumn('NumeroDocumentosPendentes').withTitle(Europa.i18n.Messages.NumeroDocumentosPendentes).withOption('width', '150px'),
        DTColumnBuilder.newColumn('MotivoParecer').withTitle(Europa.i18n.Messages.ParecerTenda).withOption('width', '600px'),
        DTColumnBuilder.newColumn('MotivoPendencia').withTitle(Europa.i18n.Messages.MotivoPendencia).withOption('width', '600px'),
        DTColumnBuilder.newColumn('NomeEmpresaVenda').withTitle(Europa.i18n.Messages.EmpresaVenda).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomePontoVenda').withTitle(Europa.i18n.Messages.PontoVenda).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeCorretor').withTitle(Europa.i18n.Messages.Corretor).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeViabilizador').withTitle(Europa.i18n.Messages.Viabilizador).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeCCA').withTitle(Europa.i18n.Messages.UltimoCCAResponsavel).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeElaborador').withTitle(Europa.i18n.Messages.Elaborador).withOption('width', '150px'),
        DTColumnBuilder.newColumn('NomeBreveLancamento').withTitle(Europa.i18n.Messages.Produto).withOption('width', '150px'),
        DTColumnBuilder.newColumn('Elaboracao').withTitle(Europa.i18n.Messages.DataElaboracao).withOption('width', '150px').renderWith(Europa.Date.toGeenDateTimeFormat),
        DTColumnBuilder.newColumn('DataEnvio').withTitle(Europa.i18n.Messages.DataUltimoEnvio).withOption('width', '150px').renderWith(renderDateTimeUtc),
        DTColumnBuilder.newColumn('NomeAssistenteAnalise').withTitle(Europa.i18n.Messages.AssistenteAnalise).withOption('width', '150px'),
        DTColumnBuilder.newColumn('TipoRenda').withTitle(Europa.i18n.Messages.TipoRenda).withOption('width', '150px').withOption('type', 'enum-format-TipoRenda'),
        DTColumnBuilder.newColumn('RendaApurada').withTitle(Europa.i18n.Messages.RendaFamiliar).withOption('width', '150px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('FgtsApurado').withTitle(Europa.i18n.Messages.FGTSApurado).withOption('width', '150px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('Entrada').withTitle(Europa.i18n.Messages.Entrada).withOption('width', '150px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('PreChaves').withTitle(Europa.i18n.Messages.PreChaves).withOption('width', '150px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('PreChavesIntermediaria').withTitle(Europa.i18n.Messages.PreChavesIntermediaria).withOption('width', '250px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('Fgts').withTitle(Europa.i18n.Messages.FGTS).withOption('width', '150px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('Subsidio').withTitle(Europa.i18n.Messages.Subsidio).withOption('width', '150px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('Financiamento').withTitle(Europa.i18n.Messages.Financiamento).withOption('width', '150px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('PosChaves').withTitle(Europa.i18n.Messages.PosChaves).withOption('width', '150px').renderWith(renderMoney),
        DTColumnBuilder.newColumn('StatusSicaq').withTitle(Europa.i18n.Messages.StatusSicaq).withOption('width', '150px').withOption('type', 'enum-format-StatusSicaq'),
        DTColumnBuilder.newColumn('StatusSicaqPrevio').withTitle(Europa.i18n.Messages.StatusSicaqPrevio).withOption('width', '150px').withOption('type', 'enum-format-StatusSicaq'),
        DTColumnBuilder.newColumn('NomeAnalistaSicaq').withTitle(Europa.i18n.Messages.AnalistaSicaq).withOption('width', '150px'),
        DTColumnBuilder.newColumn('DataSicaq').withTitle(Europa.i18n.Messages.DataHoraSicaq).withOption('width', '150px').withOption("type", "date-format-DD/MM/YYYY HH:mm:ss"),
        DTColumnBuilder.newColumn('DataSicaqPrevio').withTitle(Europa.i18n.Messages.DataHoraSicaqPrevio).withOption('width', '150px').withOption("type", "date-format-DD/MM/YYYY HH:mm:ss"),
        DTColumnBuilder.newColumn('ParcelaAprovada').withTitle(Europa.i18n.Messages.ParcelaAprovadaDoSICAQ).withOption('width', '250px').renderWith(renderValorParcela),
        DTColumnBuilder.newColumn('ParcelaAprovadaPrevio').withTitle(Europa.i18n.Messages.ParcelaAprovadaPrevioDoSICAQ).withOption('width', '250px').renderWith(renderValorParcelaPrevio),
        DTColumnBuilder.newColumn('ContadorSicaq').withTitle(Europa.i18n.Messages.ContadorSicaq).withOption('width', '175px'),
        DTColumnBuilder.newColumn('OrigemCliente').withTitle(Europa.i18n.Messages.OrigemCliente).withOption('width', '175px').withOption('type', 'enum-format-TipoOrigemCliente'),
        DTColumnBuilder.newColumn('Faturado').withTitle(Europa.i18n.Messages.Faturado).withOption('width', '175px').renderWith(renderBoolean),
    ])
        .setIdAreaHeader("consulta_pre_proposta_datatable_header")
        .setDefaultOrder([[2, 'desc']])
        .setAutoInit(false)
        .setDefaultOptions('POST', Europa.Controllers.ConsultaPreProposta.UrlListar, Europa.Controllers.ConsultaPreProposta.FilterParams);

    function renderMoney(data) {        
        if (data) {
            var valor = "R$ ";
            valor = valor + data.toFixed(2).replace(".", ",").replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
            return valor;
        }
        return "";
    }

    function renderValorParcela(data, type, full, meta) {
        if (full.DataSicaq) {
            var value = full.ParcelaAprovada;
            if (value === undefined || value === '' || value === null) {
                return "";
            }
            return "R$ "+value.toFixed(2).replace(".", ",").replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
        }
    }

    function renderValorParcelaPrevio(data, type, full, meta) {
        if (full.DataSicaq) {
            var value = full.ParcelaAprovadaPrevio;
            if (value === undefined || value === '' || value === null) {
                return "";
            }
            return "R$ " + value.toFixed(2).replace(".", ",").replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
        }
    }
    function renderDateTimeUtc(data) {
        return Europa.Date.toFormatDate.Utc(data, Europa.Date.FORMAT_DATE_HOUR);
    }

    function renderBoolean(data) {
        if (data) {
            return "Sim";
        }

        return "Não";
    }
}

Europa.Controllers.ConsultaPreProposta.FilterParams = function () {
    console.log("Filtro");
    var ret = {
        idEmpresaVenda: $("#autocomplete_empresa_venda").val(),
        idPontoVenda: $("#autocomplete_ponto_venda").val(),
        idCorretor: $("#autocomplete_corretor").val(),
        idBreveLancamento: $("#autocomplete_breve_lancamento").val(),
        Estados: $('#filtro_estados').val(),
        IdRegionais: $('#autocomplete_regional').val(),
        nomeCliente: $("#filtro_cliente").val(),
        elaboracaoDe: $("#ElaboracaoDe").val(),
        elaboracaoAte: $("#ElaboracaoAte").val(),
        dataEnvioDe: $("#DataEnvioDe").val(),
        dataEnvioAte: $("#DataEnvioAte").val(),
        codigoProposta: $("#filtro_codigo_proposta").val(),
        situacoes: $("#situacao_pre_proposta").val(),
        idViabilizador: $("#autocomplete_viabilizador").val(),
        IdStandVenda: $("#autocomplete_stand_venda").val(),
        Faturado: $("#filtro_faturado").val(),
        NomeCCA: $("#filtro_cca").val(),
        SituacaoAvalista: $("#situacao_avalista").val()

    };
    console.log(ret);
    return ret;
};

Europa.Controllers.ConsultaPreProposta.Filtrar = function () {
    Europa.Controllers.ConsultaPreProposta.Table.reloadData();
};

Europa.Controllers.ConsultaPreProposta.LimparFiltro = function () {
    $('#filtro_cliente').val("");
    $('#filtro_estados').val(0).trigger('change');
    $('#filtro_regionais').val("").trigger('change');
    $("#autocomplete_corretor").val("").trigger("change");
    $("#autocomplete_empresa_venda").val("").trigger("change");
    $("#autocomplete_stand_venda").val("").trigger("change");
    $("#autocomplete_ponto_venda").val("").trigger("change");
    $("#autocomplete_breve_lancamento").val("").trigger("change");
    $("#ElaboracaoDe").val("");
    $("#ElaboracaoAte").val("");
    $("#DataEnvioDe").val("");
    $("#DataEnvioAte").val("");
    $("#filtro_codigo_proposta").val("");
    $("#situacao_pre_proposta").val("").trigger("change");
    $("#autocomplete_viabilizador").val("").trigger("change");
    $("#filtro_faturado").val(0).trigger("change");
    $("#filtro_cca").val("");
    $("#situacao_avalista").val("").trigger("change");
    $('#autocomplete_regional').val("").trigger("change");

};


Europa.Controllers.ConsultaPreProposta.ExportarPagina = function () {
    var params = Europa.Controllers.ConsultaPreProposta.FilterParams();
    params.order = Europa.Controllers.ConsultaPreProposta.Table.lastRequestParams.order;
    params.draw = Europa.Controllers.ConsultaPreProposta.Table.lastRequestParams.draw;
    params.pageSize = Europa.Controllers.ConsultaPreProposta.Table.lastRequestParams.pageSize;
    params.start = Europa.Controllers.ConsultaPreProposta.Table.lastRequestParams.start;
    var formExportar = $("#form_exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.ConsultaPreProposta.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.ConsultaPreProposta.ExportarTodos = function () {
    var params = Europa.Controllers.ConsultaPreProposta.FilterParams();
    params.order = Europa.Controllers.ConsultaPreProposta.Table.lastRequestParams.order;
    params.draw = Europa.Controllers.ConsultaPreProposta.Table.lastRequestParams.draw;
    var formExportar = $("#form_exportar");
    formExportar.find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.ConsultaPreProposta.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.ConsultaPreProposta.AlternarExibicaoInformacoes = function () {
    if (!Europa.Controllers.ConsultaPreProposta.ExibindoInfo) {
        $('.mais-filtros').each(function () {
            $(this).hide();
        });
        $('#info-hide-filtro').html(Europa.i18n.Messages.MaisFiltros);
    } else {
        $('.mais-filtros').each(function () {
            $(this).fadeIn('slow');
        });
        setTimeout(function () {
            $('#info-hide-filtro').html(Europa.i18n.Messages.MenosFiltros);
        }, 300);
    }
    Europa.Controllers.ConsultaPreProposta.ExibindoInfo = !Europa.Controllers.ConsultaPreProposta.ExibindoInfo;
};

/////



//Europa.Controllers.ConsultaPreProposta.InitAutoCompleteEmpresaVenda = function () {
//    Europa.Controllers.ConsultaPreProposta.AutoCompleteEmpresaVenda = new Europa.Components.AutoCompleteEmpresaVendas()
//        .WithTargetSuffix("empresa_venda")
//        .WithOnChange(Europa.Controllers.ConsultaPreProposta.OnChangeEmpresaVenda)
//        .Configure();
//};

//Europa.Controllers.ConsultaPreProposta.OnChangeEmpresaVenda = function () {
//    console.log('changed');
//};

//Europa.Controllers.ConsultaPreProposta.InitAutoCompleteCorretor = function () {
//    Europa.Controllers.ConsultaPreProposta.AutoCompleteTorre = new Europa.Components.AutoCompleteTorre()
//        .WithTargetSuffix("sus01_torre")
//        .Configure();

//    Europa.Controllers.NegativoUnidade.AutoCompleteTorre.Data = function (params) {
//        return {
//            start: 0,
//            pageSize: 10,
//            filter: [
//                {
//                    value: params.term,
//                    column: this.param,
//                    regex: true
//                },
//                {
//                    value: function () {
//                        return Europa.Controllers.NegativoUnidade.AutoCompleteEmp.Value();
//                    },
//                    column: "IdEmpreendimento"
//                }
//            ],
//            order: [
//                {
//                    value: "asc",
//                    column: this.param
//                }
//            ]
//        };
//    };
//    Europa.Controllers.NegativoUnidade.AutoCompleteTorre.ProcessResult = function (data) {
//        var formattedResult = [];
//        data.records.forEach(function (element) {
//            formattedResult.push({ id: element.Id, text: element.Nome });
//        });
//        return {
//            results: formattedResult
//        };
//    };

//    Europa.Controllers.NegativoUnidade.AutoCompleteTorre.Disable();
//    $("#autocomplete_sus01_torre").on('change', Europa.Controllers.NegativoUnidade.OnChangeTorre);
//}

//Europa.Controllers.NegativoUnidade.InitAutoCompleteUnidade = function () {
//    Europa.Controllers.NegativoUnidade.AutoCompleteUnidade = new Europa.Components.AutoCompleteUnidadeLote()
//        .WithTargetSuffix("sus01_unidade")
//        .Configure();

//    Europa.Controllers.NegativoUnidade.AutoCompleteUnidade.Data = function (params) {
//        return {
//            start: 0,
//            pageSize: 10,
//            filter: [
//                {
//                    value: params.term,
//                    column: this.param,
//                    regex: true
//                },
//                {
//                    value: function () {
//                        return Europa.Controllers.NegativoUnidade.AutoCompleteTorre.Value();
//                    },
//                    column: 'IdTorre'
//                }
//            ],
//            order: [
//                {
//                    value: "asc",
//                    column: this.param
//                }
//            ]
//        };
//    };

//    Europa.Controllers.NegativoUnidade.AutoCompleteUnidade.Disable();
//    $("#autocomplete_sus01_unidade").on('change', Europa.Controllers.NegativoUnidade.OnChangeUnidade);
//}

//Europa.Controllers.NegativoUnidade.OnChangeEmpreendimento = function () {
//    var idEmp = Europa.Controllers.NegativoUnidade.AutoCompleteEmp.Value();
//    Europa.Controllers.NegativoUnidade.AutoCompleteTorre.SetValue('', '');
//    Europa.Controllers.NegativoUnidade.OnChangeTorre();

//    if (idEmp === null || idEmp === "") {

//        Europa.Controllers.NegativoUnidade.AutoCompleteTorre.Disable();
//        return;
//    }

//    Europa.Controllers.NegativoUnidade.AutoCompleteTorre.Enable();
//}

//Europa.Controllers.NegativoUnidade.OnChangeTorre = function () {
//    var idTorre = Europa.Controllers.NegativoUnidade.AutoCompleteTorre.Value();
//    Europa.Controllers.NegativoUnidade.AutoCompleteUnidade.SetValue('', '');

//    if (idTorre === null || idTorre === "") {

//        Europa.Controllers.NegativoUnidade.AutoCompleteUnidade.Disable();
//        return;
//    }

//    Europa.Controllers.NegativoUnidade.AutoCompleteUnidade.Enable();
//}

//Europa.Controllers.NegativoUnidade.OnChangeUnidade = function () {
//    var idUnidade = Europa.Controllers.NegativoUnidade.AutoCompleteUnidade.Value();
//    $(Europa.Controllers.NegativoUnidade.IdUnidadeSap).val('');
//    if (idUnidade == null || idUnidade == "") {
//        return;
//    }
//    Europa.Controllers.NegativoUnidade.ConstruirAreaRegras();
//}


//Europa.Controllers.NegativoUnidade.InitAutoComplete = function () {
//    Europa.Controllers.NegativoUnidade.InitAutoCompleteEmp();
//    Europa.Controllers.NegativoUnidade.InitAutoCompleteUnidade();
//    Europa.Controllers.NegativoUnidade.InitAutoCompleteTorre();
//}
