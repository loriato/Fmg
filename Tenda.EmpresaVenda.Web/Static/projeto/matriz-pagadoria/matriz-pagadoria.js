
Europa.Controllers.MatrizPagadoria = {
    Url: {},
    Tabela: undefined,
    Editavel: true,
    PreCarregado: false,
    FilterRow: null,
    FilterRowNominal: null,
    FilterColumn: null,
    TabelaNominal: undefined,
    ModalidadeFixa: 1,
    ModalidadeNominal: 2,
    EvsSelecionadas:[]
};

$(function () {
    Europa.Controllers.MatrizPagadoria.EmpreendimentoAutocomplete = new Europa.Components.AutoCompleteEmpreendimento()
        .WithTargetSuffix("empreendimentos");
    Europa.Controllers.MatrizPagadoria.EmpreendimentoAutocomplete.Data = function (params) {
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
    Europa.Controllers.MatrizPagadoria.EmpreendimentoAutocomplete.Configure();

    Europa.Controllers.MatrizPagadoria.EmpresaVendasAutocomplete = new Europa.Components.AutoCompleteEmpresaVendas()
        .WithTargetSuffix("empresa_venda");
    Europa.Controllers.MatrizPagadoria.EmpresaVendasAutocomplete.Data = function (params) {
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
    Europa.Controllers.MatrizPagadoria.EmpresaVendasAutocomplete.Configure();

    Europa.Controllers.MatrizPagadoria.Inicio = $("#InicioVigencia").val();
    Europa.Controllers.MatrizPagadoria.Termino = $("#TerminoVigencia").val();

    Europa.Controllers.MatrizPagadoria.DatePicker();
    Europa.Controllers.MatrizPagadoria.OnChangeVigencia();

    if (Europa.Controllers.MatrizPagadoria.IdEmpresaVenda > 0) {
        Europa.Controllers.MatrizPagadoria.EmpresaVendasAutocomplete.SetValue(Europa.Controllers.MatrizPagadoria.IdEmpresaVenda, Europa.Controllers.MatrizPagadoria.NomeEmpresaVenda)
    }

    //$("#Descricao").removeAttr("readOnly");    
});

Europa.Controllers.MatrizPagadoria.DatePicker = function () {       
    setTimeout(
        function () {
            Europa.Components.DatePicker.AutoApply();
            $("#InicioVigencia").val(null);
            $("#TerminoVigencia").val(null);
        }, 300);

    setTimeout(
        function () {
            Europa.Controllers.MatrizPagadoria.InitDatepicker();
        }, 300);   

    setTimeout(
        function () {
            $("#InicioVigencia").val(Europa.Controllers.MatrizPagadoria.Inicio);
            $("#TerminoVigencia").val(Europa.Controllers.MatrizPagadoria.Termino);
        }, 300);  
};

Europa.Controllers.MatrizPagadoria.InitDatepicker = function (inicioVigencia, terminoVigencia) {
    Europa.Controllers.MatrizPagadoria.InicioVigencia = new Europa.Components.DatePicker()
        .WithTarget("#InicioVigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate(Europa.Date.Now("DD/MM/YYYY"))
        .WithValue(inicioVigencia)
        .Configure();

    Europa.Controllers.MatrizPagadoria.TerminoVigencia = new Europa.Components.DatePicker()
        .WithTarget("#TerminoVigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate(Europa.Date.Now("DD/MM/YYYY"))
        .WithValue(terminoVigencia)
        .Configure();
};

Europa.Controllers.MatrizPagadoria.OnChangeVigencia = function () {
    var tipo = $("#Tipo").val();
    if (tipo == 2 || Europa.i18n.Messages.TipoRegraComissao_Campanha === tipo) {
        $("#vigencia").removeClass("hidden");
    } else {
        $("#vigencia").addClass("hidden")
    }
};

Europa.Controllers.MatrizPagadoria.OnChangeInicioVigencia = function () {
    Europa.Controllers.MatrizPagadoria.TerminoVigencia = new Europa.Components.DatePicker()
        .WithTarget("#TerminoVigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#InicioVigencia").val())
        .Configure();
};

Europa.Controllers.MatrizPagadoria.OnRegionalChanged = function () {
    Europa.Controllers.MatrizPagadoria.BuscarMatriz();
    Europa.Controllers.MatrizPagadoria.BuscarMatrizNominal();
    Europa.Controllers.MatrizPagadoria.EmpreendimentoAutocomplete.Clean();
    Europa.Controllers.MatrizPagadoria.EmpresaVendasAutocomplete.Clean();
};

Europa.Controllers.MatrizPagadoria.LimparFiltro = function () {
    Europa.Controllers.MatrizPagadoria.EmpreendimentoAutocomplete.Clean();
    Europa.Controllers.MatrizPagadoria.EmpresaVendasAutocomplete.Clean();
    Europa.Controllers.MatrizPagadoria.FilterRow = null;
    Europa.Controllers.MatrizPagadoria.FilterRowNominal = null;
    Europa.Controllers.MatrizPagadoria.FilterColumn = null;
    Europa.Controllers.MatrizPagadoria.Tabela.updateTable();
    Europa.Controllers.MatrizPagadoria.TabelaNominal.updateTable();
};

Europa.Controllers.MatrizPagadoria.AplicarFiltro = function () {
    var nomeEmpreendimento = Europa.Controllers.MatrizPagadoria.EmpreendimentoAutocomplete.Text();
    var nomeEmpresaVendas = Europa.Controllers.MatrizPagadoria.EmpresaVendasAutocomplete.Text();

    var atualizar = false;

    if (!Europa.Controllers.MatrizPagadoria.Evs.includes(Europa.Controllers.MatrizPagadoria.EmpresaVendasAutocomplete.Value())) {
        atualizar = true;
    }

    if (Europa.Controllers.MatrizPagadoria.EmpresaVendasAutocomplete.Value() == null) {
        Europa.Controllers.MatrizPagadoria.Evs = []
    } else {
        Europa.Controllers.MatrizPagadoria.Evs = [Europa.Controllers.MatrizPagadoria.EmpresaVendasAutocomplete.Value()];
        Europa.Controllers.MatrizPagadoria.IdEmpresaVenda = Europa.Controllers.MatrizPagadoria.EmpresaVendasAutocomplete.Value();
    }
    
    if (atualizar) {
        Europa.Controllers.MatrizPagadoria.BuscarMatriz();
        Europa.Controllers.MatrizPagadoria.BuscarMatrizNominal();
    }

    Europa.Controllers.MatrizPagadoria.FilterRow = null;
    Europa.Controllers.MatrizPagadoria.FilterRowNominal = null;

    if (nomeEmpreendimento === "") {
        Europa.Controllers.MatrizPagadoria.FilterRow = null;
        Europa.Controllers.MatrizPagadoria.FilterRowNominal = null;
    } else {
        var linha = -1;
        var linhaNominal = -1; 
        var idEmpreendimento = Europa.Controllers.MatrizPagadoria.EmpreendimentoAutocomplete.Value();

        if (Europa.Controllers.MatrizPagadoria.Tabela.getRowData(0) != undefined) {
            var data = Europa.Controllers.MatrizPagadoria.Tabela.getData(false);
            for (var i = 0; i < data.length; i++) {
                if (data[i][2].toString() === idEmpreendimento.toString()) {
                    linha = i;
                    break;
                }
            }
        }

        if (Europa.Controllers.MatrizPagadoria.TabelaNominal.getRowData(0) != undefined) {
            var dataNominal = Europa.Controllers.MatrizPagadoria.TabelaNominal.getData(false);
            for (var i = 0; i < dataNominal.length; i++) {
                if (dataNominal[i][2].toString() === idEmpreendimento.toString()) {
                    linhaNominal = i;
                    break;
                }
            }
        }

        if (linha > -1) {
            Europa.Controllers.MatrizPagadoria.FilterRow = linha;
            var topPos = $("tbody", ".jexcel").find("tr")[linha].offsetTop - 30;
            $('.jexcel_content')[0].scrollTop = topPos - ($('.jexcel_content').height() / 3);
            if (nomeEmpresaVendas === "") {
                var leftPos = $("tbody", ".jexcel").find("tr")[linha].offsetLeft;
                $('.jexcel_content')[0].scrollLeft = leftPos - ($(document).width() / 3);
            }
            $("#tab1").click();
        } else if (linhaNominal > -1) {
            Europa.Controllers.MatrizPagadoria.FilterRowNominal = linhaNominal;
            var topPos = $("tbody", ".jexcel").find("tr")[linhaNominal].offsetTop - 30;
            $('.jexcel_content')[0].scrollTop = topPos - ($(".jexcel_content").height() / 3);
            if (nomeEmpresaVendas === "") {
                var leftPos = $("tbody", ".jexcel").find("tr")[linhaNominal].offsetLeft;
                $('.jexcel_content')[0].scrollLeft = leftPos - ($(document).width() / 3);
            }
            $("#tab2").click();
        } else {
            Europa.Controllers.MatrizPagadoria.FilterRow = null;
            Europa.Controllers.MatrizPagadoria.FilterRowNominal = null;
        }
    }
    if (nomeEmpresaVendas === "") {
        Europa.Controllers.MatrizPagadoria.FilterColumn = null;
    } else {
        var coluna = -1;
        var nestedHeaders = Europa.Controllers.MatrizPagadoria.Tabela.options.nestedHeaders[0];
        var idEmpresaVendas = parseInt(Europa.Controllers.MatrizPagadoria.EmpresaVendasAutocomplete.Value());
        for (var i = 0; i < nestedHeaders.length; i++) {
            if (nestedHeaders[i].headerId === idEmpresaVendas) {
                coluna = i - 1;
                break;
            }
        }

        var nestedHeadersNominal = Europa.Controllers.MatrizPagadoria.TabelaNominal.options.nestedHeaders[0];
        for (var i = 0; i < nestedHeadersNominal.length; i++) {
            if (nestedHeadersNominal[i].headerId === idEmpresaVendas) {
                coluna = i - 1;
                break;
            }
        }

        if (coluna > -1) {
            Europa.Controllers.MatrizPagadoria.FilterColumn = coluna;
            var leftPos = $("[headerid='" + idEmpresaVendas + "']")[0].offsetLeft - 5;
            $('.jexcel_content')[0].scrollLeft = leftPos - ($(document).width() / 3);
        } else {
            Europa.Controllers.MatrizPagadoria.FilterColumn = null;
        }
    }
    Europa.Controllers.MatrizPagadoria.Tabela.updateTable();
    Europa.Controllers.MatrizPagadoria.TabelaNominal.updateTable();
};

Europa.Controllers.MatrizPagadoria.Salvar = function () {
    var itens = [];

    var numEvs = Europa.Controllers.MatrizPagadoria.Evs.length;

    if (Europa.Controllers.MatrizPagadoria.Tabela.options.data.length > 0) {
        Europa.Controllers.MatrizPagadoria.Tabela.getData().forEach(function (row) {

            var nomeEmpreendimento = row[0];
            var idRegraComissao = row[1];
            var idEmpreendimento = row[2];

            var quantidadeColunasItem = 7;

            var getFloatValue = function (inputValue) {
                var valor = parseFloat(inputValue.replace(',', '.'));
                if (isNaN(valor)) {
                    valor = 0;
                }
                return (valor + "").replace('.', ',');
            };
            var idx = 1;

            if (numEvs == 1) {
                var i = 3;
                var nomeEmpresaVenda = Europa.Controllers.MatrizPagadoria.Tabela.options.nestedHeaders[0][1].title;
                var item = {
                    regracomissao: { id: row[1] },
                    empreendimento: { id: row[2], nome: row[0] },
                    empresavenda: { id: row[i], nomefantasia: nomeEmpresaVenda },
                    id: row[4],
                    faixaUmMeio: getFloatValue(row[5]),
                    faixaDois: getFloatValue(row[6]),
                    valorKitCompleto: getFloatValue(row[7]),
                    valorConformidade: getFloatValue(row[8]),
                    valorRepasse: getFloatValue(row[9]),
                    tipoModalidadeComissao: Europa.Controllers.MatrizPagadoria.ModalidadeFixa
                };

                itens.push(item);
            } else {
                for (var i = 3; i < row.length; i += 7, idx++) {
                    var nomeEmpresaVenda = Europa.Controllers.MatrizPagadoria.Tabela.options.nestedHeaders[0][idx].title;

                    var item = {
                        RegraComissao: { Id: row[1] },
                        Empreendimento: { id: row[2], Nome: row[0] },
                        EmpresaVenda: { Id: row[i], NomeFantasia: nomeEmpresaVenda },
                        Id: row[i + 1],
                        faixaUmMeio: getFloatValue(row[i + 2]),
                        faixaDois: getFloatValue(row[i + 3]),
                        valorKitCompleto: getFloatValue(row[i + 4]),
                        valorConformidade: getFloatValue(row[i + 5]),
                        valorRepasse: getFloatValue(row[i + 6]),
                        TipoModalidadeComissao: Europa.Controllers.MatrizPagadoria.ModalidadeFixa
                    };

                    itens.push(item);
                }
            }
        });
    }

    if (Europa.Controllers.MatrizPagadoria.TabelaNominal.options.data.length > 0) {
        Europa.Controllers.MatrizPagadoria.TabelaNominal.getData().forEach(function (row) {

            var getFloatValue = function (inputValue) {
                var valor = parseFloat(inputValue.replace(',', '.'));
                if (isNaN(valor)) {
                    valor = 0;
                }
                return (valor + "").replace('.', ',');
            };
             var idx = 1
            for (var i = 3; i < row.length; i += 14,idx++) {
                var nomeEmpresaVenda = Europa.Controllers.MatrizPagadoria.TabelaNominal.options.nestedHeaders[0][idx].title;

                var item = {
                    RegraComissao: { Id: row[1] },
                    Empreendimento: { id: row[2], Nome: row[0] },
                    EmpresaVenda: { Id: row[i], NomeFantasia: nomeEmpresaVenda },
                    Id: row[i + 1],
                    MenorValorNominalUmMeio: getFloatValue(row[i + 2]),
                    IgualValorNominalUmMeio: getFloatValue(row[i + 3]),
                    MaiorValorNominalUmMeio: getFloatValue(row[i + 4]),
                    MenorValorNominalDois: getFloatValue(row[i + 5]),
                    IgualValorNominalDois: getFloatValue(row[i + 6]),
                    MaiorValorNominalDois: getFloatValue(row[i + 7]),
                    MenorValorNominalPNE: getFloatValue(row[i + 8]),
                    IgualValorNominalPNE: getFloatValue(row[i + 9]),
                    MaiorValorNominalPNE: getFloatValue(row[i + 10]),
                    ValorKitCompleto: getFloatValue(row[i + 11]),
                    ValorConformidade: getFloatValue(row[i + 12]),
                    ValorRepasse: getFloatValue(row[i + 13]),
                    TipoModalidadeComissao: Europa.Controllers.MatrizPagadoria.ModalidadeNominal
                };
                itens.push(item);
            }
        });
    }
    
    var data = {
        itens: itens,
        regraComissao: {
            id: Europa.Controllers.MatrizPagadoria.PreCarregado ? 0 : $("#Id").val(),
            regional: $("#Regional").val(),
            descricao: $("#Descricao").val(),
            tipo: $("#Tipo").val(),
            inicioVigencia: $("#InicioVigencia").val(),
            terminoVigencia: $("#TerminoVigencia").val()
        }
    };

    $.post(Europa.Controllers.MatrizPagadoria.Url.Salvar, data, function (res) {
        res.Campos.forEach(function (chave) {
            $("[name='" + chave + "']").parent().removeClass("has-error");
        });
        if (res.Sucesso) {
            Europa.Informacao.Hide = function () {
                var url = Europa.Controllers.MatrizPagadoria.Url.Index + "?regra=" + res.Objeto.Id;

                if (numEvs == 1) {
                    url += "&IdEmpresaVenda=" + Europa.Controllers.MatrizPagadoria.IdEmpresaVenda;
                }

                window.location.href = url;
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

Europa.Controllers.MatrizPagadoria.BuscarMatriz = function () {
    var regional = $("#Regional").val();
    var idRegra = $("#Id").val();

    if (regional === undefined || regional === "") {
        var tabela = Europa.Controllers.MatrizPagadoria.Tabela;
        if (tabela !== undefined) {
            tabela.destroyMerged();
        }
        return;
    }

    var query = {
        novo: Europa.Controllers.MatrizPagadoria.PreCarregado,
        idRegraComissao: idRegra,
        regional: regional,
        editable: Europa.Controllers.MatrizPagadoria.Editavel,
        listaEvs: Europa.Controllers.MatrizPagadoria.Evs,
        ultimaAtt: Europa.Controllers.MatrizPagadoria.BuscarAtivoAtual,
        modalidade: Europa.Controllers.MatrizPagadoria.ModalidadeFixa
    };
    Spinner.Show();
    $.post(Europa.Controllers.MatrizPagadoria.Url.Listar, query, function (res) {
        var resp = JSON.parse(res);

        var tabela = Europa.Controllers.MatrizPagadoria.Tabela;
        if (tabela !== undefined) {
            tabela.destroyMerged();
            $('#matriz_pagadoria').html("");
        }

        var data = resp.data;
        if (data === null || data === undefined) {
            data = [[]];
        }

        Europa.Controllers.MatrizPagadoria.Tabela = jexcel(document.getElementById('matriz_pagadoria'), {
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
                if (Europa.Controllers.MatrizPagadoria.FilterRow !== null && Europa.Controllers.MatrizPagadoria.FilterColumn !== null) {
                    if (Europa.Controllers.MatrizPagadoria.FilterRow === row && (Europa.Controllers.MatrizPagadoria.FilterColumn === colGroup || col === 2)) {
                        cell.style.backgroundColor = '#ffe589';
                    }
                } else if (Europa.Controllers.MatrizPagadoria.FilterRow === row) {
                    cell.style.backgroundColor = '#ffe589';
                } else if (Europa.Controllers.MatrizPagadoria.FilterColumn === colGroup) {
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

Europa.Controllers.MatrizPagadoria.GerarPdf = function () {
    $.post(Europa.Controllers.MatrizPagadoria.Url.GerarPdf, {regraComissao: $("#Id").val()}, function (res) {
        if (res.Sucesso) {
            $("#btn_download_pdf").css("display", 'inline');
        }
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.MatrizPagadoria.GerarExcel = function () {
    var regional = $("#Regional").val();
    var idRegra = $("#Id").val();

    if (regional === undefined || regional === "") {
        var tabela = Europa.Controllers.MatrizPagadoria.Tabela;
        if (tabela !== undefined) {
            tabela.destroyMerged();
        }
        return;
    }

    query = "idRegraComissao=" + idRegra
        + "&regional=" + regional
        + "&ultimaAtt=" + Europa.Controllers.MatrizPagadoria.BuscarAtivoAtual;
    if (Europa.Controllers.MatrizPagadoria.Evs) {
        Europa.Controllers.MatrizPagadoria.Evs.forEach(function (item) {
            query = query + "&listaEvs=" + item;
        });
    }
    location.href = Europa.Controllers.MatrizPagadoria.Url.GerarExcel + "?" + query;
};

Europa.Controllers.MatrizPagadoria.ConfirmarInterromperCampanha = function (idRegra) {
    Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.Atencao,
        Europa.i18n.Messages.MsgInterromperCampanha,
        Europa.i18n.Messages.Confirmar,
        function () {
            Europa.Controllers.MatrizPagadoria.InterromperCampanha(idRegra);
        });    
};

Europa.Controllers.MatrizPagadoria.InterromperCampanha = function (idRegra) {
    var param = {
        idRegra: idRegra
    };

    $.post(Europa.Controllers.MatrizPagadoria.UrlInterromperCampanha, param, function (res) {
        Europa.Informacao.PosAcao(res);
        if (res.Sucesso) {
            $("#btn_interromper_campanha").addClass("hidden");
        }
    });
};

Europa.Controllers.MatrizPagadoria.AtualizarItens = function () {
    var itens = [];

    var numEvs = Europa.Controllers.MatrizPagadoria.Evs.length;

    //pegando itens da matriz fixa
    if (Europa.Controllers.MatrizPagadoria.Tabela.options.data.length > 0) {
        Europa.Controllers.MatrizPagadoria.Tabela.getData().forEach(function (row) {

            var nomeEmpreendimento = row[0];
            var idRegraComissao = row[1];
            var idEmpreendimento = row[2];

            var quantidadeColunasItem = 7;

            var getFloatValue = function (inputValue) {
                var valor = parseFloat(inputValue.replace(',', '.'));
                if (isNaN(valor)) {
                    valor = 0;
                }
                return (valor + "").replace('.', ',');
            };
            var idx = 1;

            console.log(row)

            if (numEvs == 1) {
                var i = 3;
                var nomeEmpresaVenda = Europa.Controllers.MatrizPagadoria.Tabela.options.nestedHeaders[0][1].title;
                var item = {
                    regracomissao: { id: row[1] },
                    empreendimento: { id: row[2], nome: row[0] },
                    empresavenda: { id: row[i], nomefantasia: nomeEmpresaVenda },
                    id: row[4],
                    faixaUmMeio: getFloatValue(row[5]),
                    faixaDois: getFloatValue(row[6]),
                    valorKitCompleto: getFloatValue(row[7]),
                    valorConformidade: getFloatValue(row[8]),
                    valorRepasse: getFloatValue(row[9]),
                    tipoModalidadeComissao: Europa.Controllers.MatrizPagadoria.ModalidadeFixa
                };
                
                itens.push(item);
            } else {
                for (var i = 3; i < row.length; i += 7, idx++) {
                    var nomeEmpresaVenda = Europa.Controllers.MatrizPagadoria.Tabela.options.nestedHeaders[0][idx].title;

                    var item = {
                        regracomissao: { id: row[1] },
                        empreendimento: { id: row[2], nome: row[0] },
                        empresavenda: { id: row[i], nomefantasia: nomeEmpresaVenda },
                        id: row[i + 1],
                        faixaUmMeio: getFloatValue(row[i + 2]),
                        faixaDois: getFloatValue(row[i + 3]),
                        valorKitCompleto: getFloatValue(row[i + 4]),
                        valorConformidade: getFloatValue(row[i + 5]),
                        valorRepasse: getFloatValue(row[i + 6]),
                        tipoModalidadeComissao: Europa.Controllers.MatrizPagadoria.ModalidadeFixa
                    };

                    itens.push(item);
                }
            }
        });
    }

    var data = {
        itens: itens
    };

    console.log(data)
    return
    $.post(Europa.Controllers.MatrizPagadoria.Url.AtualizarItens, data, function (res) {
        console.log(res)

        Europa.Informacao.PosAcao(res);
    })
    
}


