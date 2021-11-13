Europa.Controllers.MatrizPontuacaoFidelidade = {};
Europa.Controllers.MatrizPontuacaoFidelidade.IdEmpreendimentos = [];

$(function () {
    Europa.Controllers.MatrizPontuacaoFidelidade.InitMatrizMatrizPontuacaoFidelidade();
    Europa.Controllers.MatrizPontuacaoFidelidade.BuscarMatriz();
    Europa.Controllers.MatrizPontuacaoFidelidade.BuscarMatrizNominal();

    setTimeout(
        function () {
            Europa.Controllers.MatrizPontuacaoFidelidade.InitDatepicker($("#InicioVigencia").val(), $("#TerminoVigencia").val());
        }, 300); 

    $("#tab1").click();
    
});

Europa.Controllers.MatrizPontuacaoFidelidade = {
    Url: {},
    Tabela: undefined,
    TabelaNominal: undefined,
    Editavel: true,
    PreCarregado: false,
    FilterRow: null,
    FilterRowNominal: null,
    FilterColumn: null,
    ModalidadeFixa: 1,
    ModalidadeNominal: 2
};

Europa.Controllers.MatrizPontuacaoFidelidade.InitMatrizMatrizPontuacaoFidelidade = function () {
    Europa.Controllers.MatrizPontuacaoFidelidade.EmpreendimentoAutocomplete = new Europa.Components.AutoCompleteEmpreendimento()
        .WithTargetSuffix("empreendimentos");
    Europa.Controllers.MatrizPontuacaoFidelidade.EmpreendimentoAutocomplete.Data = function (params) {
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
    Europa.Controllers.MatrizPontuacaoFidelidade.EmpreendimentoAutocomplete.Configure();

    if ($("#TipoPontuacaoFidelidade").val() == 2) {
        $(".descricao").removeClass("col-md-13");
        $(".descricao").addClass("col-md-8")
    }  

};

Europa.Controllers.MatrizPontuacaoFidelidade.OnRegionalChanged = function () {
    Europa.Controllers.MatrizPontuacaoFidelidade.BuscarMatriz();
    Europa.Controllers.MatrizPontuacaoFidelidade.EmpreendimentoAutocomplete.Clean();
};

Europa.Controllers.MatrizPontuacaoFidelidade.BuscarMatriz = function () {
    var regional = $("#Regional").val();
    var idPontuacaoFidelidade = $("#Id").val();
    var tipoPontuacaoFidelidade = $("#TipoPontuacaoFidelidade").val();
    var tipoCampanhaFidelidade = $("#TipoCampanhaFidelidade").val();
    //console.log(idPontuacaoFidelidade)
    if (regional === undefined || regional === "") {
        var tabela = Europa.Controllers.MatrizPontuacaoFidelidade.Tabela;
        if (tabela !== undefined) {
            tabela.destroyMerged();
        }
        return;
    }
    
    var query = {
        novo: Europa.Controllers.MatrizPontuacaoFidelidade.PreCarregado,
        idPontuacaoFidelidade: idPontuacaoFidelidade,
        regional: regional,
        editable: Europa.Controllers.MatrizPontuacaoFidelidade.Editavel,
        ultimaAtt: Europa.Controllers.MatrizPontuacaoFidelidade.BuscarAtivoAtual,
        tipoPontuacaoFidelidade: tipoPontuacaoFidelidade,
        tipoCampanhaFidelidade: tipoCampanhaFidelidade,
        idEmpreendimentos: Europa.Controllers.MatrizPontuacaoFidelidade.IdEmpreendimentos,
        idEmpresasVenda: Europa.Controllers.MatrizPontuacaoFidelidade.IdEmpresasVenda,
        modalidade: Europa.Controllers.MatrizPontuacaoFidelidade.ModalidadeFixa,
        qtdm: Europa.Controllers.MatrizPontuacaoFidelidade.QuantidadesMinimas
    };

    if (tipoCampanhaFidelidade > 1) {
        query.Progressao = $("#Progressao").val();
    }
    //console.log(query)
    Spinner.Show();
       
    $.post(Europa.Controllers.MatrizPontuacaoFidelidade.Url.Listar, query, function (res) {
        var resp = JSON.parse(res);

        var tabela = Europa.Controllers.MatrizPontuacaoFidelidade.Tabela;
        if (tabela !== undefined) {
            tabela.destroyMerged();
            $('#matriz_pontuacao_fidelidade').html("");
        }

        var data = resp.data;
        if (data === null || data === undefined) {
            data = [[]];
        }

        Europa.Controllers.MatrizPontuacaoFidelidade.Tabela = jexcel(document.getElementById('matriz_pontuacao_fidelidade'), {
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
                var cinza = '#e6e6e6';
                if (col > 2) {
                    //var col2 = col - 3;

                    //var pos = col2 - (col2 - col2 % 14);
                    //colGroup = (col2 - col2 % 7) / 7;

                    
                    //if ($("#TipoPontuacaoFidelidade").val() == "1") {
                    //    console.log(pos)
                    //    if (pos > 6) {
                    //        cell.style.backgroundColor = '#edf3ff';
                    //    }
                    //}                  
                    
                    //var pos = col - 5;
                    //if (pos % 4 == 0 && pos>1) {
                    //    console.log(pos)
                    //    Europa.Controllers.MatrizPontuacaoFidelidade.Pintar = !Europa.Controllers.MatrizPontuacaoFidelidade.Pintar;
                    //}

                    //if ($("#TipoPontuacaoFidelidade").val() == 1) {
                    //    if (Europa.Controllers.MatrizPontuacaoFidelidade.Pintar) {
                    //        console.log("Padrão")
                    //        cell.style.backgroundColor = '#edf3ff';
                    //    }
                    //}

                    var col2 = col - 3;
                    if ($("#TipoPontuacaoFidelidade").val() == 1) {
                        var pos = col2 - (col2 - col2 % 12);
                        colGroup = (col2 - col2 % 6) / 6;
                        if (pos == 4 || pos == 5) {
                            cell.style.backgroundColor = cinza;
                        }
                        else if (pos == 10 || pos == 11) {
                            cell.style.backgroundColor = cinza;
                        }
                        else if (pos > 5) {
                            cell.style.backgroundColor = '#edf3ff';
                        }
                    }
                    switch ($("#TipoCampanhaFidelidade").val()) {
                        case "1":
                            var pos = col2 - (col2 - col2 % 16);
                            colGroup = (col2 - col2 % 8) / 8;
                            if (pos == 3 || pos == 11) {
                                cell.style.backgroundColor = cinza;
                            }
                            else if (pos >= 5 && pos <= 7) {
                                cell.style.backgroundColor = cinza;
                            }
                            else if (pos >= 13 && pos <= 15) {
                                cell.style.backgroundColor = cinza;
                            }
                            else if (pos > 7) {
                                cell.style.backgroundColor = '#edf3ff';
                            }
                            break;
                        case "2":
                            var pos = col2 - (col2 - col2 % 18);
                            colGroup = (col2 - col2 % 9) / 9;
                            if (pos != 2 && pos != 4 && pos != 11 && pos != 13) {
                                cell.style.backgroundColor = cinza;
                            }
                            else if (pos > 8) {
                                cell.style.backgroundColor = '#edf3ff';
                            }
                            break;
                        case "3":
                            var pos = col2 - (col2 - col2 % 18);
                            colGroup = (col2 - col2 % 9) / 9;
                            if (pos <= 8) {
                                if (pos % 2 == 1) {
                                    cell.style.backgroundColor = cinza;
                                }
                                else if (pos == 8 || pos == 16) {
                                    cell.style.backgroundColor = cinza;
                                }
                            }
                            else if (pos > 8) {
                                if (pos % 2 == 0) {
                                    cell.style.backgroundColor = cinza;
                                }
                                else if (pos == 17) {
                                    cell.style.backgroundColor = cinza;
                                } else {
                                    cell.style.backgroundColor = '#edf3ff';
                                }
                                
                            }
                            break;
                    }
                }
                if (Europa.Controllers.MatrizPontuacaoFidelidade.FilterRow !== null && Europa.Controllers.MatrizPontuacaoFidelidade.FilterColumn !== null) {
                    if (Europa.Controllers.MatrizPontuacaoFidelidade.FilterRow === row && (Europa.Controllers.MatrizPontuacaoFidelidade.FilterColumn === colGroup || col === 2)) {
                        cell.style.backgroundColor = '#ffe589';
                    }
                } else if (Europa.Controllers.MatrizPontuacaoFidelidade.FilterRow === row) {
                    cell.style.backgroundColor = '#ffe589';
                } else if (Europa.Controllers.MatrizPontuacaoFidelidade.FilterColumn === colGroup) {
                    cell.style.backgroundColor = '#ffe589';
                }
            }
        });

        $(".jexcel_toolbar", "#matriz_pontuacao_fidelidade").hide();
        $(".jexcel_pagination", "#matriz_pontuacao_fidelidade").hide();
        $(".jexcel_contextmenu", "#matriz_pontuacao_fidelidade").hide();
        $(".jexcel_about", "#matriz_pontuacao_fidelidade").hide();

        Spinner.Hide();
    });
};

Europa.Controllers.MatrizPontuacaoFidelidade.InitDatepicker = function (inicioVigencia, terminoVigencia) {

    Europa.Controllers.MatrizPontuacaoFidelidade.InicioVigencia = new Europa.Components.DatePicker()
        .WithTarget("#InicioVigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate(Europa.Date.Now("DD/MM/YYYY"))
        .WithValue(inicioVigencia)
        .Configure();

    Europa.Controllers.MatrizPontuacaoFidelidade.TerminoVigencia = new Europa.Components.DatePicker()
        .WithTarget("#TerminoVigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate(Europa.Date.Now("DD/MM/YYYY"))
        .WithValue(terminoVigencia)
        .Configure();    
};

Europa.Controllers.MatrizPontuacaoFidelidade.OnChangeInicioVigencia = function () {
    Europa.Controllers.MatrizPontuacaoFidelidade.TerminoVigencia = new Europa.Components.DatePicker()
        .WithTarget("#TerminoVigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#InicioVigencia").val())
        .Configure();
};

Europa.Controllers.MatrizPontuacaoFidelidade.DatePicker = function () {
    setTimeout(
        function () {
            Europa.Components.DatePicker.AutoApply();
            $("#InicioVigencia").val(null);
            $("#TerminoVigencia").val(null);
        }, 300);

    setTimeout(
        function () {
            Europa.Controllers.MatrizPontuacaoFidelidade.InitDatepicker();
        }, 300);

    setTimeout(
        function () {
            $("#InicioVigencia").val(Europa.Controllers.MatrizPontuacaoFidelidade.Inicio);
            $("#TerminoVigencia").val(Europa.Controllers.MatrizPontuacaoFidelidade.Termino);
        }, 300);
};

Europa.Controllers.MatrizPontuacaoFidelidade.LimparFiltro = function () {
    Europa.Controllers.MatrizPontuacaoFidelidade.EmpreendimentoAutocomplete.Clean();
    Europa.Controllers.MatrizPontuacaoFidelidade.FilterRow = null;
    Europa.Controllers.MatrizPontuacaoFidelidade.FilterColumn = null;
    Europa.Controllers.MatrizPontuacaoFidelidade.Tabela.updateTable();
};

Europa.Controllers.MatrizPontuacaoFidelidade.AplicarFiltro = function () {
    var nomeEmpreendimento = Europa.Controllers.MatrizPontuacaoFidelidade.EmpreendimentoAutocomplete.Text();

    Europa.Controllers.MatrizPontuacaoFidelidade.FilterRow = null;
    Europa.Controllers.MatrizPontuacaoFidelidade.FilterRowNominal = null;

    if (nomeEmpreendimento === "") {
        Europa.Controllers.MatrizPontuacaoFidelidade.FilterRow = null;        
    } else {
        
        var linha = -1;
        var data = Europa.Controllers.MatrizPontuacaoFidelidade.Tabela.getData(false);
        var idEmpreendimento = Europa.Controllers.MatrizPontuacaoFidelidade.EmpreendimentoAutocomplete.Value();
        
        for (var i = 0; i < data.length; i++) {
            if (data[i][2].toString() === idEmpreendimento.toString()) {
                linha = i;
                break;
            }
        }

        if (linha > -1) {
            Europa.Controllers.MatrizPontuacaoFidelidade.FilterRow = linha;
            var topPos = $("tbody", ".jexcel").find("tr")[linha].offsetTop - 30;
            $('.jexcel_content')[0].scrollTop = topPos - ($(".jexcel_content").height() / 3);
            $("#tab1").click();            
        }

        var linhaNominal = -1;
        var dataNominal = Europa.Controllers.MatrizPontuacaoFidelidade.TabelaNominal.getData(false);
        
        for (var i = 0; i < dataNominal.length; i++) {
            if (dataNominal[i][2].toString() === idEmpreendimento.toString()) {
                linhaNominal = i;
                break;
            }
        }

        if (linhaNominal > -1) {
            Europa.Controllers.MatrizPontuacaoFidelidade.FilterRowNominal = linhaNominal;
            var topPos = $("tbody", ".jexcel").find("tr")[linhaNominal].offsetTop - 30;
            $('.jexcel_content')[0].scrollTop = topPos - ($(".jexcel_content").height() / 3);
            $("#tab2").click();
        }
    }
    
    Europa.Controllers.MatrizPontuacaoFidelidade.Tabela.updateTable();
    Europa.Controllers.MatrizPontuacaoFidelidade.TabelaNominal.updateTable();
};

Europa.Controllers.MatrizPontuacaoFidelidade.Salvar = function () {

    var itens = [];
    var tipoPontuacaFidelidade = $("#TipoPontuacaoFidelidade").val();
    var tipoCampanhaFidelidade = $("#TipoCampanhaFidelidade").val();

    var numCol = 6;

    if (Europa.Controllers.MatrizPontuacaoFidelidade.Tabela != undefined && Europa.Controllers.MatrizPontuacaoFidelidade.Tabela.options.data.length > 0) {        
        Europa.Controllers.MatrizPontuacaoFidelidade.Tabela.getData().forEach(function (row) {

            var getFloatValue = function (inputValue) {
                if (inputValue == 0) {
                    return "0";
                }
                var valor = parseFloat(inputValue.replace(',', '.'));
                if (isNaN(valor)) {
                    valor = 0;
                }
                return (valor + "").replace('.', ',');
            };

            var nomeEmpreendimento = row[0];
            var idPontuacaoFidelidade = row[1];
            var idEmpreendimento = row[2];
            for (var i = 3,idx = 1; i < row.length; i += numCol,idx++) {

                var nomeEmpresaVenda = Europa.Controllers.MatrizPontuacaoFidelidade.Tabela.options.nestedHeaders[0][idx].title;
                //console.log(nomeEmpresaVenda);

                var item = {
                    PontuacaoFidelidade: { Id: idPontuacaoFidelidade },
                    Empreendimento: { Id: idEmpreendimento, Nome: nomeEmpreendimento },
                    EmpresaVenda: { Id: row[i], NomeFantasia: nomeEmpresaVenda },
                    Id: row[i + 1],
                    Modalidade: Europa.Controllers.MatrizPontuacaoFidelidade.ModalidadeFixa
                };

                if (tipoPontuacaFidelidade == 1) {
                    item.PontuacaoFaixaUmMeio = getFloatValue(row[i + 2]);
                    item.PontuacaoFaixaDois = getFloatValue(row[i + 3]);
                    numCol = 6;
                }

                switch (tipoCampanhaFidelidade) {
                    case "1": 
                        item.FatorUmMeio = getFloatValue(row[i + 2]);
                        item.PontuacaoPadraoUmMeio = getFloatValue(row[i + 3]);

                        item.FatorDois = getFloatValue(row[i + 4]);
                        item.PontuacaoPadraoDois = getFloatValue(row[i + 5]);

                        numCol = 8;
                        break;

                    case "2":
                        item.FatorUmMeio = getFloatValue(row[i + 2]);
                        item.PontuacaoPadraoUmMeio = getFloatValue(row[i + 3]);

                        item.FatorDois = getFloatValue(row[i + 4]);
                        item.PontuacaoPadraoDois = getFloatValue(row[i + 5]);

                        item.QuantidadeMinima = getFloatValue(row[i + 6]);

                        numCol = 9;
                        break;

                    case "3":
                        item.FatorUmMeio = getFloatValue(row[i + 2]);
                        item.PontuacaoPadraoUmMeio = getFloatValue(row[i + 3]);

                        item.FatorDois = getFloatValue(row[i + 4]);
                        item.PontuacaoPadraoDois = getFloatValue(row[i + 5]);

                        item.QuantidadeMinima = getFloatValue(row[i + 6]);

                        numCol = 9;
                        break;
                }
                itens.push(item);
                //console.log(item)
            }
        });
    }

    if (Europa.Controllers.MatrizPontuacaoFidelidade.TabelaNominal != undefined && Europa.Controllers.MatrizPontuacaoFidelidade.TabelaNominal.options.data.length > 0) {
        Europa.Controllers.MatrizPontuacaoFidelidade.TabelaNominal.getData().forEach(function (row) {
            var getFloatValue = function (inputValue) {
                if (inputValue == 0) {
                    return "0";
                }
                var valor = parseFloat(inputValue.replace(',', '.'));
                if (isNaN(valor)) {
                    valor = 0;
                }
                return (valor + "").replace('.', ',');
            };

            var qtdeEvs = Europa.Controllers.MatrizPontuacaoFidelidade.TabelaNominal.options.nestedHeaders[0].length;
            var idx = 1;
            var nomeEmpreendimento = row[0];
            var idPontuacaoFidelidade = row[1];
            var idEmpreendimento = row[2];
            //console.log(row)
            var cols = 13;
            for (var i = 3; i < row.length; i += cols) {

                var nomeEmpresaVenda = Europa.Controllers.MatrizPontuacaoFidelidade.TabelaNominal.options.nestedHeaders[0][idx].title;
                if (idx < qtdeEvs-1) {
                    idx++;
                }

                var item = {
                    PontuacaoFidelidade: { Id: idPontuacaoFidelidade },
                    Empreendimento: { Id: idEmpreendimento, Nome: nomeEmpreendimento },
                    EmpresaVenda: { Id: row[i], NomeFantasia: nomeEmpresaVenda },
                    Id: row[i + 1],
                    Modalidade: Europa.Controllers.MatrizPontuacaoFidelidade.ModalidadeNominal
                };

                if (tipoPontuacaFidelidade == 1) {
                    item.PontuacaoFaixaUmMeioSeca = getFloatValue(row[i + 2]);
                    item.PontuacaoFaixaUmMeioNormal = getFloatValue(row[i + 3]);
                    item.PontuacaoFaixaUmMeioTurbinada = getFloatValue(row[i + 4]);

                    item.PontuacaoFaixaDoisSeca = getFloatValue(row[i + 5]);
                    item.PontuacaoFaixaDoisNormal = getFloatValue(row[i + 6]);
                    item.PontuacaoFaixaDoisTurbinada = getFloatValue(row[i + 7]);

                    item.PontuacaoPNESeca = getFloatValue(row[i + 8]);
                    item.PontuacaoPNENormal = getFloatValue(row[i + 9]);
                    item.PontuacaoPNETurbinada = getFloatValue(row[i + 10]);
                }
                
                switch (tipoCampanhaFidelidade) {
                    case "1": 
                        item.FatorUmMeioSeca = getFloatValue(row[i + 2]);
                        item.PontuacaoFaixaUmMeioSeca = getFloatValue(row[i + 3]);

                        item.FatorUmMeioNormal = getFloatValue(row[i + 4]);
                        item.PontuacaoFaixaUmMeioNormal = getFloatValue(row[i + 5]);

                        item.FatorUmMeioTurbinada = getFloatValue(row[i + 6]);
                        item.PontuacaoFaixaUmMeioTurbinada = getFloatValue(row[i + 7]);

                        item.FatorDoisSeca = getFloatValue(row[i + 8]);
                        item.PontuacaoFaixaDoisSeca = getFloatValue(row[i + 9]);

                        item.FatorDoisNormal = getFloatValue(row[i + 10]);
                        item.PontuacaoFaixaDoisNormal = getFloatValue(row[i +11]);

                        item.FatorDoisTurbinada = getFloatValue(row[i + 12]);
                        item.PontuacaoFaixaDoisTurbinada = getFloatValue(row[i + 13]);

                        item.FatorPNESeca = getFloatValue(row[i + 14]);
                        item.PontuacaoPNESeca = getFloatValue(row[i + 15]);

                        item.FatorPNENormal = getFloatValue(row[i + 16]);
                        item.PontuacaoPNENormal = getFloatValue(row[i + 17]);
                        
                        item.FatorPNETurbinada = getFloatValue(row[i + 18]);
                        item.PontuacaoPNETurbinada = getFloatValue(row[i + 19]);
                        cols = 22;
                        break;

                    case "2":
                        item.QuantidadeMinima = getFloatValue(row[i + 2]);
                        
                        item.FatorUmMeioSeca = getFloatValue(row[i + 3]);
                        item.PontuacaoFaixaUmMeioSeca = getFloatValue(row[i + 4]);

                        item.FatorUmMeioNormal = getFloatValue(row[i + 5]);
                        item.PontuacaoFaixaUmMeioNormal = getFloatValue(row[i + 6]);

                        item.FatorUmMeioTurbinada = getFloatValue(row[i + 7]);
                        item.PontuacaoFaixaUmMeioTurbinada = getFloatValue(row[i + 8]);

                        item.FatorDoisSeca = getFloatValue(row[i + 9]);
                        item.PontuacaoFaixaDoisSeca = getFloatValue(row[i + 10]);

                        item.FatorDoisNormal = getFloatValue(row[i + 11]);
                        item.PontuacaoFaixaDoisNormal = getFloatValue(row[i + 12]);

                        item.FatorDoisTurbinada = getFloatValue(row[i + 13]);
                        item.PontuacaoFaixaDoisTurbinada = getFloatValue(row[i + 14]);

                        item.FatorPNESeca = getFloatValue(row[i + 15]);
                        item.PontuacaoPNESeca = getFloatValue(row[i + 16]);

                        item.FatorPNENormal = getFloatValue(row[i + 17]);
                        item.PontuacaoPNENormal = getFloatValue(row[i + 18]);

                        item.FatorPNETurbinada = getFloatValue(row[i + 19]);
                        item.PontuacaoPNETurbinada = getFloatValue(row[i + 20]);

                        cols = 23;
                        break;

                    case "3":
                        item.QuantidadeMinima = getFloatValue(row[i + 2]);
                        
                        item.FatorUmMeioSeca = getFloatValue(row[i + 3]);
                        item.PontuacaoFaixaUmMeioSeca = getFloatValue(row[i + 4]);

                        item.FatorUmMeioNormal = getFloatValue(row[i + 5]);
                        item.PontuacaoFaixaUmMeioNormal = getFloatValue(row[i + 6]);

                        item.FatorUmMeioTurbinada = getFloatValue(row[i + 7]);
                        item.PontuacaoFaixaUmMeioTurbinada = getFloatValue(row[i + 8]);

                        item.FatorDoisSeca = getFloatValue(row[i + 9]);
                        item.PontuacaoFaixaDoisSeca = getFloatValue(row[i + 10]);

                        item.FatorDoisNormal = getFloatValue(row[i + 11]);
                        item.PontuacaoFaixaDoisNormal = getFloatValue(row[i + 12]);

                        item.FatorDoisTurbinada = getFloatValue(row[i + 13]);
                        item.PontuacaoFaixaDoisTurbinada = getFloatValue(row[i + 14]);

                        item.FatorPNESeca = getFloatValue(row[i + 15]);
                        item.PontuacaoPNESeca = getFloatValue(row[i + 16]);

                        item.FatorPNENormal = getFloatValue(row[i + 17]);
                        item.PontuacaoPNENormal = getFloatValue(row[i + 18]);

                        item.FatorPNETurbinada = getFloatValue(row[i + 19]);
                        item.PontuacaoPNETurbinada = getFloatValue(row[i + 20]);

                        cols = 23;
                        break;
                }
                itens.push(item);
            }
        });
    }

    var data = {
        itens: itens,
        pontuacaoFidelidade: {
            id: $("#Id").val(),
            regional: $("#Regional").val(),
            descricao: $("#Descricao").val(),
            TipoPontuacaoFidelidade: tipoPontuacaFidelidade,
            TipoCampanhaFidelidade: tipoCampanhaFidelidade,
            InicioVigencia: $("#InicioVigencia").val(),
            TerminoVigencia: $("#TerminoVigencia").val() + " 23:59:59",
            QuantidadeMinima: $("#QuantidadeMinima").val(),
            QuantidadesMinimas: "[" + Europa.Controllers.MatrizPontuacaoFidelidade.QuantidadesMinimas.toString() + "]",
            Progressao: $("#Progressao").val()
        }
    };

    //console.log(data)
    
    $.post(Europa.Controllers.MatrizPontuacaoFidelidade.Url.Salvar, data, function (res) {
        res.Campos.forEach(function (chave) {
            $("[name='" + chave + "']").parent().removeClass("has-error");
        });
        if (res.Sucesso) {
            Europa.Informacao.Hide = function () {
                window.location.href = Europa.Controllers.MatrizPontuacaoFidelidade.Url.Index + "?idPontuacaoFidelidade=" + res.Objeto.Id;
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

Europa.Controllers.MatrizPontuacaoFidelidade.GerarExcel = function () {
    var regional = $("#Regional").val();
    var idPontuacaoFidelidade = $("#Id").val();

    if (regional === undefined || regional === "") {
        var tabela = Europa.Controllers.MatrizPontuacaoFidelidade.Tabela;
        if (tabela !== undefined) {
            tabela.destroyMerged();
        }
        return;
    }

    query = "idPontuacaoFidelidade=" + idPontuacaoFidelidade
        + "&regional=" + regional;

    location.href = Europa.Controllers.MatrizPontuacaoFidelidade.Url.GerarExcel + "?" + query;
};

Europa.Controllers.MatrizPontuacaoFidelidade.GerarPdf = function () {
    $.post(Europa.Controllers.MatrizPontuacaoFidelidade.Url.GerarPdf, { pontuacaoFidelidade: $("#Id").val() }, function (res) {
        if (res.Sucesso) {
            $("#btn_download_pdf").css("display", 'inline');
        }
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.MatrizPontuacaoFidelidade.BuscarMatrizNominal = function () {
    var regional = $("#Regional").val();
    var idPontuacaoFidelidade = $("#Id").val();
    var tipoPontuacaoFidelidade = $("#TipoPontuacaoFidelidade").val();
    var tipoCampanhaFidelidade = $("#TipoCampanhaFidelidade").val();
    //console.log(idPontuacaoFidelidade)
    if (regional === undefined || regional === "") {
        var tabela = Europa.Controllers.MatrizPontuacaoFidelidade.TabelaNominal;
        if (tabela !== undefined) {
            tabela.destroyMerged();
        }
        return;
    }
    
    var query = {
        novo: Europa.Controllers.MatrizPontuacaoFidelidade.PreCarregado,
        idPontuacaoFidelidade: idPontuacaoFidelidade,
        regional: regional,
        editable: Europa.Controllers.MatrizPontuacaoFidelidade.Editavel,
        ultimaAtt: Europa.Controllers.MatrizPontuacaoFidelidade.BuscarAtivoAtual,
        tipoPontuacaoFidelidade: tipoPontuacaoFidelidade,
        tipoCampanhaFidelidade: tipoCampanhaFidelidade,
        idEmpreendimentos: Europa.Controllers.MatrizPontuacaoFidelidade.IdEmpreendimentos,
        idEmpresasVenda: Europa.Controllers.MatrizPontuacaoFidelidade.IdEmpresasVenda,
        modalidade: Europa.Controllers.MatrizPontuacaoFidelidade.ModalidadeNominal,
        qtdm: Europa.Controllers.MatrizPontuacaoFidelidade.QuantidadesMinimas
    };
    if (tipoCampanhaFidelidade > 1) {
        query.Progressao = $("#Progressao").val();
    }
    //console.log(query)
    Spinner.Show();

    $.post(Europa.Controllers.MatrizPontuacaoFidelidade.Url.Listar, query, function (res) {
        var resp = JSON.parse(res);

        var tabela = Europa.Controllers.MatrizPontuacaoFidelidade.TabelaNominal;
        if (tabela !== undefined) {
            tabela.destroyMerged();
            $('#matriz_pontuacao_fidelidade_nominal').html("");
        }

        var data = resp.data;
        if (data === null || data === undefined) {
            data = [[]];
        }

        Europa.Controllers.MatrizPontuacaoFidelidade.TabelaNominal = jexcel(document.getElementById('matriz_pontuacao_fidelidade_nominal'), {
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
                var cinza = '#e6e6e6';
                if (col > 2) {
                    var col2 = col - 3;
                    if ($("#TipoPontuacaoFidelidade").val() == 1) {
                        var pos = col2 - (col2 - col2 % 26);
                        colGroup = (col2 - col2 % 13) / 13;
                        
                        if (pos > 1 && pos < 5) {
                            cell.style.backgroundColor = cinza;
                        }
                        else if (pos > 7 && pos < 11) {
                            cell.style.backgroundColor = cinza;
                        }
                        else if (pos > 14 && pos < 18) {
                            cell.style.backgroundColor = cinza;
                        }
                        else if (pos > 20 && pos < 24) {
                            cell.style.backgroundColor = cinza;
                        }
                        else if (pos > 12) {
                            cell.style.backgroundColor = '#edf3ff';
                        }
                    }
                    switch ($("#TipoCampanhaFidelidade").val()) {
                        case "1":
                            var pos = col2 - (col2 - col2 % 44);
                            colGroup = (col2 - col2 % 22) / 22;
                            
                            if (pos % 2 == 1) {
                                cell.style.backgroundColor = cinza;
                            }
                            else if (pos == 20 || pos == 42) {
                                cell.style.backgroundColor = cinza;
                            }
                            else if (pos > 21) {
                                cell.style.backgroundColor = '#edf3ff';
                            }
                            break;
                        case "2":
                            var pos = col2 - (col2 - col2 % 46);
                            colGroup = (col2 - col2 % 23) / 23;
                            if (pos <= 22) {
                                if (pos % 2 == 0) {
                                    cell.style.backgroundColor = cinza;
                                }
                                else if (pos == 21) {
                                    cell.style.backgroundColor = cinza;
                                }
                            }
                            else if (pos > 22) {
                                if (pos % 2 == 1) {
                                    cell.style.backgroundColor = cinza;
                                }
                                else if (pos == 44) {
                                    cell.style.backgroundColor = cinza;
                                }
                                else {
                                    cell.style.backgroundColor = '#edf3ff';
                                }
                                
                            }
                            break;
                        case "3":
                            var pos = col2 - (col2 - col2 % 46);
                            colGroup = (col2 - col2 % 23) / 23;
                            if (pos <= 22) {
                                if (pos == 2) {
                                    cell.style.backgroundColor = '#ffffff';
                                }
                                else if (pos % 2 == 0) {
                                    cell.style.backgroundColor = cinza;
                                }
                                else if (pos == 21) {
                                    cell.style.backgroundColor = cinza;
                                }
                            }
                            else if (pos > 22) {
                                if (pos == 25) {
                                    cell.style.backgroundColor = '#edf3ff';
                                }
                                else if (pos % 2 == 1) {
                                    cell.style.backgroundColor = cinza;
                                }
                                else if (pos == 44) {
                                    cell.style.backgroundColor = cinza;
                                }
                                else {
                                    cell.style.backgroundColor = '#edf3ff';
                                }

                            }
                            break;
                    }
                }

                if (Europa.Controllers.MatrizPontuacaoFidelidade.FilterRowNominal !== null && Europa.Controllers.MatrizPontuacaoFidelidade.FilterColumn !== null) {
                    if (Europa.Controllers.MatrizPontuacaoFidelidade.FilterRowNominal === row && (Europa.Controllers.MatrizPontuacaoFidelidade.FilterColumn === colGroup || col === 2)) {
                        cell.style.backgroundColor = '#ffe589';
                    }
                } else if (Europa.Controllers.MatrizPontuacaoFidelidade.FilterRowNominal === row) {
                    cell.style.backgroundColor = '#ffe589';
                } else if (Europa.Controllers.MatrizPontuacaoFidelidade.FilterColumn === colGroup) {
                    cell.style.backgroundColor = '#ffe589';
                }
            }
        });

        $(".jexcel_toolbar", "#matriz_pontuacao_fidelidade_nominal").hide();
        $(".jexcel_pagination", "#matriz_pontuacao_fidelidade_nominal").hide();
        $(".jexcel_contextmenu", "#matriz_pontuacao_fidelidade_nominal").hide();
        $(".jexcel_about", "#matriz_pontuacao_fidelidade_nominal").hide();

        Spinner.Hide();
    });
};