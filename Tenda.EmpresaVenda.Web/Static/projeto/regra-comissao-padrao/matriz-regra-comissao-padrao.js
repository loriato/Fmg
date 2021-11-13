Europa.Controllers.MatrizRegraComissaoPadrao = {
    Url: {},
    Tabela: undefined,
    Editavel: true,
    PreCarregado: false,
    FilterRow: null,
    FilterRowNominal: null,
    FilterColumn: null,
    TabelaNominal: undefined,
    ModalidadeFixa: 1,
    ModalidadeNominal: 2
};

$(function () {
    Europa.Controllers.MatrizRegraComissaoPadrao.EmpreendimentoAutocomplete = new Europa.Components.AutoCompleteEmpreendimento()
        .WithTargetSuffix("empreendimentos");
    Europa.Controllers.MatrizRegraComissaoPadrao.EmpreendimentoAutocomplete.Data = function (params) {
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
                    value: $("#Regional").val(),
                    column: 'regional'
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
    Europa.Controllers.MatrizRegraComissaoPadrao.EmpreendimentoAutocomplete.Configure();

    Europa.Controllers.MatrizRegraComissaoPadrao.EmpresaVendasAutocomplete = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("empresa_venda");
    Europa.Controllers.MatrizRegraComissaoPadrao.EmpresaVendasAutocomplete.Data = function (params) {
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
                    value: $("#Regional").val(),
                    column: 'regional'
                },
                {
                    value: $("#EmpresasVendasSelecionadas").val(),
                    column: 'idspermitidos'
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
    Europa.Controllers.MatrizRegraComissaoPadrao.EmpresaVendasAutocomplete.Configure();
});

Europa.Controllers.MatrizRegraComissaoPadrao.OnRegionalChanged = function () {
    Europa.Controllers.MatrizRegraComissaoPadrao.BuscarMatriz();
    Europa.Controllers.MatrizRegraComissaoPadrao.EmpreendimentoAutocomplete.Clean();
    Europa.Controllers.MatrizRegraComissaoPadrao.EmpresaVendasAutocomplete.Clean();
};

Europa.Controllers.MatrizRegraComissaoPadrao.LimparFiltro = function () {
    Europa.Controllers.MatrizRegraComissaoPadrao.EmpreendimentoAutocomplete.Clean();
    Europa.Controllers.MatrizRegraComissaoPadrao.EmpresaVendasAutocomplete.Clean();
    Europa.Controllers.MatrizRegraComissaoPadrao.FilterRow = null;
    Europa.Controllers.MatrizRegraComissaoPadrao.FilterRowNominal = null;
    Europa.Controllers.MatrizRegraComissaoPadrao.FilterColumn = null;
    Europa.Controllers.MatrizRegraComissaoPadrao.Tabela.updateTable();
};

Europa.Controllers.MatrizRegraComissaoPadrao.AplicarFiltro = function () {
    var nomeEmpreendimento = Europa.Controllers.MatrizRegraComissaoPadrao.EmpreendimentoAutocomplete.Text();
    var nomeEmpresaVendas = Europa.Controllers.MatrizRegraComissaoPadrao.EmpresaVendasAutocomplete.Text();

    Europa.Controllers.MatrizRegraComissaoPadrao.FilterRow = null;
    Europa.Controllers.MatrizRegraComissaoPadrao.FilterRowNominal = null;

    if (nomeEmpreendimento === "") {
        Europa.Controllers.MatrizRegraComissaoPadrao.FilterRow = null;
    } else {
        var linha = -1;
        var idEmpreendimento = Europa.Controllers.MatrizRegraComissaoPadrao.EmpreendimentoAutocomplete.Value();
        if (Europa.Controllers.MatrizRegraComissaoPadrao.Tabela.getRowData(0) != undefined) {
            var data = Europa.Controllers.MatrizRegraComissaoPadrao.Tabela.getData(false);
            for (var i = 0; i < data.length; i++) {
                if (data[i][2].toString() === idEmpreendimento.toString()) {
                    linha = i;
                    break;
                }
            }
        }

        var linhaNominal = -1;
        if (Europa.Controllers.MatrizRegraComissaoPadrao.TabelaNominal.getRowData(0) != undefined) {
            var dataNominal = Europa.Controllers.MatrizRegraComissaoPadrao.TabelaNominal.getData(false);
            for (var i = 0; i < dataNominal.length; i++) {
                if (dataNominal[i][2].toString() === idEmpreendimento.toString()) {
                    linhaNominal = i;
                    break;
                }
            }
        }

        if (linha > -1) {
            Europa.Controllers.MatrizRegraComissaoPadrao.FilterRow = linha;
            var topPos = $("tbody", ".jexcel").find("tr")[linha].offsetTop - 30;
            $('.jexcel_content')[0].scrollTop = topPos - ($(".jexcel_content").height() / 3);
            if (nomeEmpresaVendas === "") {
                var leftPos = $("tbody", ".jexcel").find("tr")[linha].offsetLeft;
                $('.jexcel_content')[0].scrollLeft = leftPos - ($(document).width() / 3);
            }

            $("#tab1").click();
        } else if (linhaNominal > -1) {
            Europa.Controllers.MatrizRegraComissaoPadrao.FilterRowNominal = linhaNominal;
            var topPos = $("tbody", ".jexcel").find("tr")[linhaNominal].offsetTop - 30;
            $('.jexcel_content')[0].scrollTop = topPos - ($(".jexcel_content").height() / 3);
            if (nomeEmpresaVendas === "") {
                var leftPos = $("tbody", ".jexcel").find("tr")[linhaNominal].offsetLeft;
                $('.jexcel_content')[0].scrollLeft = leftPos - ($(document).width() / 3);
            }

            $("#tab2").click();
        }else {
            Europa.Controllers.MatrizRegraComissaoPadrao.FilterRow = null;
        }
    }
    if (nomeEmpresaVendas === "") {
        Europa.Controllers.MatrizRegraComissaoPadrao.FilterColumn = null;
    } else {
        var coluna = -1;
        var nestedHeaders = Europa.Controllers.MatrizRegraComissaoPadrao.Tabela.options.nestedHeaders[0];
        var idEmpresaVendas = parseInt(Europa.Controllers.MatrizRegraComissaoPadrao.EmpresaVendasAutocomplete.Value());
        for (var i = 0; i < nestedHeaders.length; i++) {
            if (nestedHeaders[i].headerId === idEmpresaVendas) {
                coluna = i - 1;
                break;
            }
        }

        if (coluna > -1) {
            Europa.Controllers.MatrizRegraComissaoPadrao.FilterColumn = coluna;
            var leftPos = $("[headerid='" + idEmpresaVendas + "']")[0].offsetLeft - 5;
            $('.jexcel_content')[0].scrollLeft = leftPos - ($(document).width() / 3);
        } else {
            Europa.Controllers.MatrizRegraComissaoPadrao.FilterColumn = null;
        }
    }
    Europa.Controllers.MatrizRegraComissaoPadrao.Tabela.updateTable();
    Europa.Controllers.MatrizRegraComissaoPadrao.TabelaNominal.updateTable();
};

Europa.Controllers.MatrizRegraComissaoPadrao.Salvar = function () {
    var itens = [];
    if (Europa.Controllers.MatrizRegraComissaoPadrao.Tabela.options.data.length > 0) {
        Europa.Controllers.MatrizRegraComissaoPadrao.Tabela.getData().forEach(function (row) {
            const POSICAO_TABELA = {
                ID_ITEM_REGRA_COMISSAO: 0,
                FAIXA_UM_MEIO: 1,
                FAIXA_DOIS: 2,
                VALOR_KIT_COMPLETO: 3,
                VALOR_CONFIRMIDADE: 4,
                VALOR_REPASSE: 5
            };

            var nomeEmpreendimento = row[0];
            var idRegraComissao = row[1];
            var idEmpreendimento = row[2];

            var quantidadeColunasItem = 7;

            var item = {};

            var getFloatValue = function (inputValue) {
                var valor = parseFloat(inputValue.replace(',', '.'));
                if (isNaN(valor)) {
                    valor = 0;
                }
                return (valor + "").replace('.', ',');
            };

            for (var i = 3; i < row.length; i++) {
                var valor = 0;
                switch ((i - 3) % quantidadeColunasItem) {
                    case POSICAO_TABELA.ID_ITEM_REGRA_COMISSAO:
                        item = {
                            idEmpreendimento: idEmpreendimento,
                            idRegraComissaoPadrao: idRegraComissao,
                        };
                        item.idItemRegraComissaoPadrao = row[i];
                        break;
                    case POSICAO_TABELA.FAIXA_UM_MEIO:
                        item.faixaUmMeio = getFloatValue(row[i]);
                        break;
                    case POSICAO_TABELA.FAIXA_DOIS:
                        item.faixaDois = getFloatValue(row[i]);
                        break;
                    case POSICAO_TABELA.VALOR_KIT_COMPLETO:
                        item.valorKitCompleto = getFloatValue(row[i]);
                        break;
                    case POSICAO_TABELA.VALOR_CONFIRMIDADE:
                        item.valorConformidade = getFloatValue(row[i]);
                        break;
                    case POSICAO_TABELA.VALOR_REPASSE:
                        item.valorRepasse = getFloatValue(row[i]);
                        item.Modalidade = Europa.Controllers.MatrizRegraComissaoPadrao.ModalidadeFixa;
                        itens.push(item);
                        break;
                }
            }
        });
    }

    if (Europa.Controllers.MatrizRegraComissaoPadrao.TabelaNominal.options.data.length > 0) {
        Europa.Controllers.MatrizRegraComissaoPadrao.TabelaNominal.getData().forEach(function (row) {
            console.log(row)
            var getFloatValue = function (inputValue) {
                var valor = parseFloat(inputValue.replace(',', '.'));
                if (isNaN(valor)) {
                    valor = 0;
                }
                return (valor + "").replace('.', ',');
            };

            for (var i = 3; i < row.length; i += 13) {
                var item = {
                    IdRegraComissao: row[1],
                    IdEmpreendimento: row[2],
                    IdItemRegraComissaoPadrao: row[i],
                    MenorValorNominalUmMeio: getFloatValue(row[i + 1]),
                    IgualValorNominalUmMeio: getFloatValue(row[i + 2]),
                    MaiorValorNominalUmMeio: getFloatValue(row[i + 3]),
                    MenorValorNominalDois: getFloatValue(row[i + 4]),
                    IgualValorNominalDois: getFloatValue(row[i + 5]),
                    MaiorValorNominalDois: getFloatValue(row[i + 6]),
                    MenorValorNominalPNE: getFloatValue(row[i + 7]),
                    IgualValorNominalPNE: getFloatValue(row[i + 8]),
                    MaiorValorNominalPNE: getFloatValue(row[i + 9]),
                    ValorKitCompleto: getFloatValue(row[i + 10]),
                    ValorConformidade: getFloatValue(row[i + 11]),
                    ValorRepasse: getFloatValue(row[i + 12]),
                    Modalidade: Europa.Controllers.MatrizRegraComissaoPadrao.ModalidadeNominal
                };
                itens.push(item);
                console.log(item)
            }
        });
    }
    var data = {
        itens: itens,
        regraComissao: {
            id: Europa.Controllers.MatrizRegraComissaoPadrao.PreCarregado ? 0 : $("#Id").val(),
            regional: $("#Regional").val(),
            descricao: $("#Descricao").val()
        }
    };

    $.post(Europa.Controllers.MatrizRegraComissaoPadrao.Url.Salvar, data, function (res) {
        res.Campos.forEach(function (chave) {
            $("[name='" + chave + "']").parent().removeClass("has-error");
        });
        if (res.Sucesso) {
            Europa.Informacao.Hide = function () {
                window.location.href = Europa.Controllers.MatrizRegraComissaoPadrao.Url.Index + "/Matriz?regra=" + res.Objeto.Id;
            };
        } else {
            Europa.Informacao.Hide = function () {
                $(Europa.Informacao.Attr.Modal).modal("hide");
            };
            if (res && res.Campos && res.Campos.length > 0) {
                res.Campos.forEach(function (chave) {
                    $("[name='" + chave + "']").parent().addClass("has-error");
                });
            }
        }
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.MatrizRegraComissaoPadrao.BuscarMatriz = function () {
    var regional = $("#Regional").val();
    var idRegra = $("#Id").val();

    if (regional === undefined || regional === "") {
        var tabela = Europa.Controllers.MatrizRegraComissaoPadrao.Tabela;
        if (tabela !== undefined) {
            tabela.destroyMerged();
        }
        return;
    }

    var query = {
        novo: Europa.Controllers.MatrizRegraComissaoPadrao.PreCarregado,
        idRegraComissao: idRegra,
        regional: regional,
        editable: Europa.Controllers.MatrizRegraComissaoPadrao.Editavel,
        listaEvs: Europa.Controllers.MatrizRegraComissaoPadrao.Evs,
        ultimaAtt: Europa.Controllers.MatrizRegraComissaoPadrao.BuscarAtivoAtual,
        modalidade: Europa.Controllers.MatrizRegraComissaoPadrao.ModalidadeFixa
    };
    Spinner.Show();
    $.post(Europa.Controllers.MatrizRegraComissaoPadrao.Url.Listar, query, function (res) {
        var resp = JSON.parse(res);

        var tabela = Europa.Controllers.MatrizRegraComissaoPadrao.Tabela;
        if (tabela !== undefined) {
            tabela.destroyMerged();
            $('#matriz_pagadoria').html("");
        }

        var data = resp.data;
        if (data === null || data === undefined) {
            data = [[]];
        }

        Europa.Controllers.MatrizRegraComissaoPadrao.Tabela = jexcel(document.getElementById('matriz_pagadoria'), {
            data: data,
            columns: resp.columns,
            nestedHeaders: resp.nestedHeaders,
            allowInsertRow: false,
            allowInsertColumn: false,
            allowDeleteColumn: false,
            allowDeleteRow: false,
            allowRenameColumn: false,
            allowComments: false,
            allowExport: false,
            columnSorting: false,
            columnDrag: false,
            columnResize: true,
            rowResize: false,
            rowDrag: false,
            editable: true,
            allowManualInsertRow: false,
            allowManualInsertColumn: false,
            tableOverflow: false,
            showRowNumber: false,
            updateTable: function (instance, cell, col, row, val, label, cellName) {
                var colGroup = -1;
                cell.style.backgroundColor = '#ffffff';
                if (col > 2) {
                    var col2 = col - 3;
                    var pos = col2 - (col2 - col2 % 14);
                    colGroup = (col2 - col2 % 7) / 7;

                    if (pos > 6) {
                        cell.style.backgroundColor = '#edf3ff';
                    }
                }
                if (Europa.Controllers.MatrizRegraComissaoPadrao.FilterRow !== null && Europa.Controllers.MatrizRegraComissaoPadrao.FilterColumn !== null) {
                    if (Europa.Controllers.MatrizRegraComissaoPadrao.FilterRow === row && (Europa.Controllers.MatrizRegraComissaoPadrao.FilterColumn === colGroup || col === 2)) {
                        cell.style.backgroundColor = '#ffe589';
                    }
                } else if (Europa.Controllers.MatrizRegraComissaoPadrao.FilterRow === row) {
                    cell.style.backgroundColor = '#ffe589';
                } else if (Europa.Controllers.MatrizRegraComissaoPadrao.FilterColumn === colGroup) {
                    cell.style.backgroundColor = '#ffe589';
                }
            }
        });

        $(".jexcel_toolbar", "#matriz_pagadoria").hide();
        $(".jexcel_pagination", "#matriz_pagadoria").hide();
        $(".jexcel_contextmenu", "#matriz_pagadoria").hide();
        $(".jexcel_about", "#matriz_pagadoria").hide();

        Spinner.Hide();
    });
};

Europa.Controllers.MatrizRegraComissaoPadrao.GerarPdf = function () {
    $.post(Europa.Controllers.MatrizRegraComissaoPadrao.Url.GerarPdf, {regraComissao: $("#Id").val()}, function (res) {
        if (res.Sucesso) {
            $("#btn_download_pdf").css("display", 'inline');
        }
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.MatrizRegraComissaoPadrao.GerarExcel = function () {
    var regional = $("#Regional").val();
    var idRegra = $("#Id").val();

    if (regional === undefined || regional === "") {
        var tabela = Europa.Controllers.MatrizRegraComissaoPadrao.Tabela;
        if (tabela !== undefined) {
            tabela.destroyMerged();
        }
        return;
    }

    query = "idRegraComissao=" + idRegra
        + "&regional=" + regional
        + "&ultimaAtt=" + Europa.Controllers.MatrizRegraComissaoPadrao.BuscarAtivoAtual;
    location.href = Europa.Controllers.MatrizRegraComissaoPadrao.Url.GerarExcel + "?" + query;
};

Europa.Controllers.MatrizRegraComissaoPadrao.DownloadPdf = function (idRegraComissao) {
    location.href = Europa.Controllers.MatrizRegraComissaoPadrao.Url.DownloadPdf + "?idRegra=" + idRegraComissao;
};