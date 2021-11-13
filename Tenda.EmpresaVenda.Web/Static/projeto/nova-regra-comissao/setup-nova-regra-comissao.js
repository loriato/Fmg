$(function () {
    Europa.Controllers.NovaRegraComissao.InitDatepicker();
});

Europa.Controllers.NovaRegraComissao.FiltroTreeViewEv = function () {
    var idRegraReferencia = $("#IdRegraComissaoReferencia", "#modalSetupRegraComissao").val();
    var regional = $("#Regional", "#modalSetupRegraComissao").val();

    return { idRegraReferencia: idRegraReferencia, regional: regional };
};

Europa.Controllers.NovaRegraComissao.FiltroTreeViewEvSegundaEtapa = function () {
    var evsSelecionadas = [];

    Europa.Controllers.NovaRegraComissao.Parametro.EvsSelecionadas.forEach(function (ev) {
        evsSelecionadas.push(ev.id);
    });
    //console.log(evsSelecionadas)
    return {idEvs: evsSelecionadas };
};

Europa.Controllers.NovaRegraComissao.AtualizarTreeViewEvSegundaEtapa = function () {
    Europa.Controllers.NovaRegraComissao.Parametro.Regional = $("#Regional", "#modalSetupRegraComissao").val();

    Europa.Controllers.NovaRegraComissao.TreeViewEv
        .WithAjax("POST",
            Europa.Controllers.NovaRegraComissao.Url.ListarTreeIdEvs,
            Europa.Controllers.NovaRegraComissao.FiltroTreeViewEvSegundaEtapa)
        .WithShowCheckbox(true)
        .WithRowCheck(true)
        .WithExpandIcon(false)
        .WithCollapseIcon(false)
        .WithCheckRootSiblings(true)
        .Configure();
};

Europa.Controllers.NovaRegraComissao.AtualizarTreeViewEv = function () {
    Europa.Controllers.NovaRegraComissao.Parametro.Regional = $("#Regional", "#modalSetupRegraComissao").val();

    Europa.Controllers.NovaRegraComissao.TreeViewEv
        .WithAjax("GET",
            Europa.Controllers.NovaRegraComissao.Url.ListarEvs,
            Europa.Controllers.NovaRegraComissao.FiltroTreeViewEv)
        .WithShowCheckbox(true)
        .WithRowCheck(true)
        .WithExpandIcon(false)
        .WithCollapseIcon(false)
        .WithCheckRootSiblings(true)
        .Configure();    
};

Europa.Controllers.NovaRegraComissao.AbrirSetup = function () {
    $("#IdRegraComissaoReferencia", "#modalSetupRegraComissao").val("");
    $("#Regional", "#modalSetupRegraComissao").attr("disabled", false);
    $("#Regional", "#modalSetupRegraComissao").val($("#Regional option:first", "#modalSetupRegraComissao").val());
    $("#opcao_buscar_ultima_wrapper","#modalSetupRegraComissao").css("display", "block");

    Europa.Controllers.NovaRegraComissao.AtualizarTreeViewEv();    

    Europa.Controllers.NovaRegraComissao.OnChangeInterface();

    $("#modalSetupRegraComissao").show();
};

Europa.Controllers.NovaRegraComissao.OnRegionalChanged = function () {
    Europa.Controllers.NovaRegraComissao.AtualizarTreeViewEv();
};

Europa.Controllers.NovaRegraComissao.FecharSetup = function () {  
    $("#btn-preencher-itens-diferenciados").addClass("hidden");
    $("#btn-gerar-regra-comissao").addClass("hidden");
    $("#btn-proximo").removeClass("hidden");
    $("#EtapaSetup").val(1);
    $("#modalSetupRegraComissao").hide();
    Europa.Controllers.NovaRegraComissao.OnChangeInterface();
    Europa.Controllers.NovaRegraComissao.TreeViewEv = new Europa.Components.TreeView("empresa_venda_treeview_primeira_etapa");
    Europa.Controllers.NovaRegraComissao.AtualizarTreeViewEv();
    Europa.Controllers.NovaRegraComissao.IdxEv = 0;
    $("#setup-descricao").val("");
    $("#vigencia").addClass("hidden");

    Europa.Controllers.NovaRegraComissao.Parametro = {
        Regional: "",
        EvsSelecionadas: [],
        EvsDiferenciadas: [],
        EvsComuns: [],
        ItensComuns: [],
        ItensDiferenciados: [],
    };
};

Europa.Controllers.NovaRegraComissao.MarcarTodos = function () {
    Europa.Controllers.NovaRegraComissao.TreeViewEv.CheckAllNodes();
};

Europa.Controllers.NovaRegraComissao.DesmarcarTodos = function () {
    Europa.Controllers.NovaRegraComissao.TreeViewEv.UncheckAllNodes();
};

Europa.Controllers.NovaRegraComissao.AvancarEtapa = function () {
    var etapaAtual = $("#EtapaSetup").val();

    //console.log("Etapa atual - " + etapaAtual);

    //Europa.Controllers.NovaRegraComissao.OnChangeInterface();
    
    switch (etapaAtual) {
        case "1":
            Europa.Controllers.NovaRegraComissao.PrimeiraEtapa();
            Europa.Controllers.NovaRegraComissao.OnChangeInterface();
            break;
        case "2":
            Europa.Controllers.NovaRegraComissao.SegundaEtapa();
            Europa.Controllers.NovaRegraComissao.OnChangeInterface();
            break;
        case "3":
            var valido = Europa.Controllers.NovaRegraComissao.TerceiraEtapa();
            //console.log(valido)
            if (valido) {
                Europa.Controllers.NovaRegraComissao.OnChangeInterface();
            }
            break;
        case "4":
            Europa.Controllers.NovaRegraComissao.QuartaEtapa();
            Europa.Controllers.NovaRegraComissao.OnChangeInterface();
            break;
        case "5":
            Europa.Controllers.NovaRegraComissao.QuintaEtapa();
            Europa.Controllers.NovaRegraComissao.OnChangeInterface();
            break;
    }
};

Europa.Controllers.NovaRegraComissao.OnChangeInterface = function () {
    var etapaAtual = $("#EtapaSetup").val();

    $("#primeira-etapa").addClass("hidden");
    $("#segunda-etapa").addClass("hidden");
    $("#terceira-etapa").addClass("hidden");
    $("#quarta-etapa").addClass("hidden");
    $("#quinta-etapa").addClass("hidden");

    switch (etapaAtual) {
        case "1":
            $("#modalSetupRegraComissao_titulo", "#modalSetupRegraComissao")[0].innerText = "1ª Etapa: Selecione as Empresas de Vendas";
            $("#primeira-etapa").removeClass("hidden");
            break;
        case "2":
            $("#modalSetupRegraComissao_titulo", "#modalSetupRegraComissao")[0].innerText = "2ª Etapa: Quais imobiliárias das selecionadas previamente receberão uma regra de comissão diferenciada?";
            $("#segunda-etapa").removeClass("hidden");
            Europa.Controllers.NovaRegraComissao.TreeViewEv = new Europa.Components.TreeView("empresa_venda_treeview_segunda_etapa")
            Europa.Controllers.NovaRegraComissao.AtualizarTreeViewEvSegundaEtapa();
            break;
        case "3":
            $("#modalSetupRegraComissao_titulo", "#modalSetupRegraComissao")[0].innerText = "3ª Etapa: Preencha a regra de comissão para o grupo em comum";
            $("#terceira-etapa").removeClass("hidden");

            Europa.Controllers.NovaRegraComissao.SepararEvs();

            if (Europa.Controllers.NovaRegraComissao.Parametro.EvsComuns.length == 0) {
                $("#EtapaSetup").val(4);
                Europa.Controllers.NovaRegraComissao.OnChangeInterface();
            }

            //montar matriz fixa
            Europa.Controllers.NovaRegraComissao.MontarMatrizFixa();

            //montar matriz nominal
            Europa.Controllers.NovaRegraComissao.MontarMatrizNominal();

            $("#tab1").click();

            break;
        case "4":
            $("#modalSetupRegraComissao_titulo", "#modalSetupRegraComissao")[0].innerText = "4ª Etapa: Preencha a regra de comissão para a Empresa de Venda específica";
            $("#terceira-etapa").removeClass("hidden");
            $("#quarta-etapa").removeClass("hidden");
            
            $("#btn-preencher-itens-diferenciados").removeClass("hidden");
            $("#btn-proximo").addClass("hidden");

            $("#tab1").click();

            if (Europa.Controllers.NovaRegraComissao.Parametro.EvsDiferenciadas.length == 0) {
                $("#EtapaSetup").val(5);
                Europa.Controllers.NovaRegraComissao.OnChangeInterface();
            }

            //Montar matriz fixa por ev
            Europa.Controllers.NovaRegraComissao.MontarMatrizFixaEv();

            //Montar matriz nominal por ev
            Europa.Controllers.NovaRegraComissao.MontarMatrizNominalEv();

            break;
        case "5":
            $("#modalSetupRegraComissao_titulo", "#modalSetupRegraComissao")[0].innerText = "5ª Etapa: Preencha o cabeçalho da Regra de comissão";
            $("#quinta-etapa").removeClass("hidden");

            $("#setup-regional").val(Europa.Controllers.NovaRegraComissao.Parametro.Regional)
            
            $("#btn-proximo").addClass("hidden");
            $("#btn-preencher-itens-diferenciados").addClass("hidden");
            $("#btn-gerar-regra-comissao").removeClass("hidden");
            Europa.Controllers.NovaRegraComissao.InitDatepicker();

            break;
    }
};

Europa.Controllers.NovaRegraComissao.PrimeiraEtapa = function () {
    
    var res = {
        Sucesso: false,
        Mensagens: []
    }

    var evsSelecionadas = Europa.Controllers.NovaRegraComissao.TreeViewEv.GetCheckedNodes();

    if (evsSelecionadas.length < 1) {

        res.Mensagens.push("Selecione ao menos uma Empresa de Vendas")
        Europa.Informacao.PosAcao(res);
        return;
    }

    Europa.Controllers.NovaRegraComissao.Parametro.EvsSelecionadas = evsSelecionadas;

    $("#EtapaSetup").val(2)

    $("#primeira-etapa").addClass("hidden");

};

Europa.Controllers.NovaRegraComissao.SegundaEtapa = function () {
    
    var evsDiferenciadas = Europa.Controllers.NovaRegraComissao.TreeViewEv.GetCheckedNodes();

    Europa.Controllers.NovaRegraComissao.Parametro.EvsDiferenciadas = evsDiferenciadas;

    $("#EtapaSetup").val(3)
    
    $("#segunda-etapa").addClass("hidden");

}

Europa.Controllers.NovaRegraComissao.TerceiraEtapa = function () {

    var itens = Europa.Controllers.NovaRegraComissao.PreencherItensComuns();

    var valido = Europa.Controllers.NovaRegraComissao.ValidarItemRegraComissao(itens);

    if (!valido) {
        console.log("Há itens invalidos")
        return false;
    }

    $("#EtapaSetup").val(4);
    $("#terceira-etapa").addClass("hidden");
    
    return true;
}

//próxima EV
Europa.Controllers.NovaRegraComissao.QuartaEtapa = function () {
    if (Europa.Controllers.NovaRegraComissao.IdxEv == Europa.Controllers.NovaRegraComissao.Parametro.EvsDiferenciadas.length) {
        $("#btn-preencher-itens-diferenciados").addClass("hidden");
        $("#btn-proximo").removeClass("hidden");

        console.log("Todas EVS preenchidas")

        return;
    }

    var itensDiferenciados = Europa.Controllers.NovaRegraComissao.PreencherItensDiferenciados();
    //console.log(itensDiferenciados)

    var itensValidos = Europa.Controllers.NovaRegraComissao.ValidarItemRegraComissao(itensDiferenciados);
    //console.log(itensValidos)

    if (!itensValidos) {
        console.log("Há itens inválidos");
        return;
    }

    itensDiferenciados.forEach(function (item) {
        //console.log(item)
        Europa.Controllers.NovaRegraComissao.Parametro.ItensDiferenciados.push(item);
    });

    Europa.Controllers.NovaRegraComissao.IdxEv++;

    if (Europa.Controllers.NovaRegraComissao.IdxEv == Europa.Controllers.NovaRegraComissao.Parametro.EvsDiferenciadas.length) {
        $("#btn-preencher-itens-diferenciados").addClass("hidden");
        $("#btn-proximo").removeClass("hidden");

        console.log("Todas EVS preenchidas")

        $("#EtapaSetup").val(5);

        Europa.Controllers.NovaRegraComissao.OnChangeInterface();

        return;
    }

    Europa.Controllers.NovaRegraComissao.MontarMatrizFixaEv();
    Europa.Controllers.NovaRegraComissao.MontarMatrizNominalEv();
};

Europa.Controllers.NovaRegraComissao.QuintaEtapa = function () {
    console.log("Quinta etapa")

    $("#EtapaSetup").val(6)
    console.log("Fim da Quinta Etapa");

    $("#quinta-etapa").addClass("hidden");

    console.log(Europa.Controllers.NovaRegraComissao.Parametro);
}

//montar matriz fixa
Europa.Controllers.NovaRegraComissao.MontarMatrizFixa = function () {
    var regional = Europa.Controllers.NovaRegraComissao.Parametro.Regional;

    if (regional === undefined || regional === "") {
        var tabela = Europa.Controllers.NovaRegraComissao.MatrizFixa;
        if (tabela !== undefined) {
            tabela.destroyMerged();
        }
        return;
    }

    var setup = Europa.Controllers.NovaRegraComissao.Parametro;
    setup.Modalidade = 1;

    Spinner.Show();

    $.post(Europa.Controllers.NovaRegraComissao.Url.MontarMatriz, setup, function (res) {

        var resp = JSON.parse(res);

        var tabela = Europa.Controllers.NovaRegraComissao.MatrizFixa;
        if (tabela !== undefined) {
            tabela.destroyMerged();
            $('#matriz_fixa').html("");
        }

        var data = resp.data;
        if (data === null || data === undefined) {
            data = [[]];
        }

        Europa.Controllers.NovaRegraComissao.MatrizFixa = jexcel(document.getElementById('matriz_fixa'), {
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
                if (Europa.Controllers.NovaRegraComissao.FilterRow !== null && Europa.Controllers.NovaRegraComissao.FilterColumn !== null) {
                    if (Europa.Controllers.NovaRegraComissao.FilterRow === row && (Europa.Controllers.NovaRegraComissao.FilterColumn === colGroup || col === 2)) {
                        cell.style.backgroundColor = '#ffe589';
                    }
                } else if (Europa.Controllers.NovaRegraComissao.FilterRow === row) {
                    cell.style.backgroundColor = '#ffe589';
                } else if (Europa.Controllers.NovaRegraComissao.FilterColumn === colGroup) {
                    cell.style.backgroundColor = '#ffe589';
                }
            }
        });

        $(".jexcel_toolbar", "#matriz_fixa").hide();
        $(".jexcel_pagination", "#matriz_fixa").hide();
        $(".jexcel_contextmenu", "#matriz_fixa").hide();
        $(".jexcel_about", "#matriz_fixa").hide();

        Spinner.Hide();

        //console.log(resp)
    });

};

//montar matriz nominal
Europa.Controllers.NovaRegraComissao.MontarMatrizNominal = function () {
    var regional = Europa.Controllers.NovaRegraComissao.Parametro.Regional;

    if (regional === undefined || regional === "") {
        var tabela = Europa.Controllers.NovaRegraComissao.MatrizNominal;
        if (tabela !== undefined) {
            tabela.destroyMerged();
        }
        return;
    }

    var setup = Europa.Controllers.NovaRegraComissao.Parametro;
    setup.Modalidade = 2;

    Spinner.Show();

    $.post(Europa.Controllers.NovaRegraComissao.Url.MontarMatriz, setup, function (res) {

        var resp = JSON.parse(res);

        var tabela = Europa.Controllers.NovaRegraComissao.MatrizNominal;
        if (tabela !== undefined) {
            tabela.destroyMerged();
            $('#matriz_nominal').html("");
        }

        var data = resp.data;
        if (data === null || data === undefined) {
            data = [[]];
        }

        Europa.Controllers.NovaRegraComissao.MatrizNominal = jexcel(document.getElementById('matriz_nominal'), {
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
                if (Europa.Controllers.NovaRegraComissao.FilterRow !== null && Europa.Controllers.NovaRegraComissao.FilterColumn !== null) {
                    if (Europa.Controllers.NovaRegraComissao.FilterRow === row && (Europa.Controllers.NovaRegraComissao.FilterColumn === colGroup || col === 2)) {
                        cell.style.backgroundColor = '#ffe589';
                    }
                } else if (Europa.Controllers.NovaRegraComissao.FilterRow === row) {
                    cell.style.backgroundColor = '#ffe589';
                } else if (Europa.Controllers.NovaRegraComissao.FilterColumn === colGroup) {
                    cell.style.backgroundColor = '#ffe589';
                }
            }
        });

        $(".jexcel_toolbar", "#matriz_nominal").hide();
        $(".jexcel_pagination", "#matriz_nominal").hide();
        $(".jexcel_contextmenu", "#matriz_nominal").hide();
        $(".jexcel_about", "#matriz_nominal").hide();

        Spinner.Hide();

        //console.log(resp)
    });

};

//Preencher itens comuns
Europa.Controllers.NovaRegraComissao.PreencherItensComuns = function () {
    var itens = [];

    //Matriz fixa
    if (Europa.Controllers.NovaRegraComissao.MatrizFixa.options.data.length > 0) {
        Europa.Controllers.NovaRegraComissao.MatrizFixa.getData().forEach(function (row) {

            var quantidadeColunasItem = 7;

            var getFloatValue = function (inputValue) {
                var valor = parseFloat(inputValue.replace(',', '.'));
                if (isNaN(valor)) {
                    valor = 0;
                }
                return (valor + "").replace('.', ',');
            };
            var idx = 1;

            for (var i = 0; i < row.length; i += quantidadeColunasItem, idx++) {
                
                var item = {
                    Id: 0,
                    RegraComissao: { Id: 0 },
                    EmpresaVenda: { Id: 0 },
                    Empreendimento: { Nome: row[i], id: row[i + 1] },
                    faixaUmMeio: getFloatValue(row[i + 2]),
                    faixaDois: getFloatValue(row[i + 3]),
                    ValorKitCompleto: getFloatValue(row[i + 4]),
                    ValorConformidade: getFloatValue(row[i + 5]),
                    ValorRepasse: getFloatValue(row[i + 6]),
                    TipoModalidadeComissao: Europa.Controllers.NovaRegraComissao.ModalidadeFixa
                };

                itens.push(item);
            }
        });
    }

    //Matriz nominal
    if (Europa.Controllers.NovaRegraComissao.MatrizNominal.options.data.length > 0) {
        Europa.Controllers.NovaRegraComissao.MatrizNominal.getData().forEach(function (row) {

            var getFloatValue = function (inputValue) {
                var valor = parseFloat(inputValue.replace(',', '.'));
                if (isNaN(valor)) {
                    valor = 0;
                }
                return (valor + "").replace('.', ',');
            };
            var idx = 1
            for (var i = 0; i < row.length; i += 14, idx++) {
                
                var item = {
                    Id: 0,
                    RegraComissao: { Id: 0 },
                    EmpresaVenda: { Id: 0 },
                    Empreendimento: { Nome: row[i], Id: row[i + 1] },
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
                    TipoModalidadeComissao: Europa.Controllers.NovaRegraComissao.ModalidadeNominal
                };

                itens.push(item);
            }
        });
    }

    Europa.Controllers.NovaRegraComissao.Parametro.ItensComuns = itens;

    return itens;
};

//Separar EVS
Europa.Controllers.NovaRegraComissao.SepararEvs = function () {
    var idEvsSelecionadas = [];
    var idEvsDiferenciadas = [];
    var idEvsComuns = [];

    Europa.Controllers.NovaRegraComissao.Parametro.EvsDiferenciadas.forEach(function (ev) {
        idEvsDiferenciadas.push(ev.id);
    });

    Europa.Controllers.NovaRegraComissao.Parametro.EvsSelecionadas.forEach(function (ev) {
        idEvsSelecionadas.push(ev.id);

        if (!idEvsDiferenciadas.includes(ev.id)) {
            idEvsComuns.push(ev.id);
            Europa.Controllers.NovaRegraComissao.Parametro.EvsComuns.push(ev);
        }

    });

    console.log(idEvsSelecionadas)
    console.log(idEvsDiferenciadas)
    console.log(idEvsComuns)

    //console.log(Europa.Controllers.NovaRegraComissao.Parametro);
};

//Montar matriz fixa por ev
Europa.Controllers.NovaRegraComissao.MontarMatrizFixaEv = function () {

    if (Europa.Controllers.NovaRegraComissao.Parametro.EvsDiferenciadas.length == 0) {
        console.log("Não há evs diferenciadas")
        return;
    }

    var idxEv = Europa.Controllers.NovaRegraComissao.IdxEv;

    var ev = Europa.Controllers.NovaRegraComissao.Parametro.EvsDiferenciadas[idxEv];

    //console.log(ev);

    var regional = Europa.Controllers.NovaRegraComissao.Parametro.Regional;

    if (regional === undefined || regional === "") {
        var tabela = Europa.Controllers.NovaRegraComissao.MatrizFixa;
        if (tabela !== undefined) {
            tabela.destroyMerged();
        }
        return;
    }

    var setup = Europa.Controllers.NovaRegraComissao.Parametro;
    setup.Modalidade = 1;
    setup.IdEmpresaVenda = ev.id;

    Spinner.Show();

    $.post(Europa.Controllers.NovaRegraComissao.Url.MontarMatriz, setup, function (res) {

        var resp = JSON.parse(res);

        var tabela = Europa.Controllers.NovaRegraComissao.MatrizFixa;
        if (tabela !== undefined) {
            tabela.destroyMerged();
            $('#matriz_fixa').html("");
        }

        var data = resp.data;
        if (data === null || data === undefined) {
            data = [[]];
        }

        Europa.Controllers.NovaRegraComissao.MatrizFixa = jexcel(document.getElementById('matriz_fixa'), {
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
                if (Europa.Controllers.NovaRegraComissao.FilterRow !== null && Europa.Controllers.NovaRegraComissao.FilterColumn !== null) {
                    if (Europa.Controllers.NovaRegraComissao.FilterRow === row && (Europa.Controllers.NovaRegraComissao.FilterColumn === colGroup || col === 2)) {
                        cell.style.backgroundColor = '#ffe589';
                    }
                } else if (Europa.Controllers.NovaRegraComissao.FilterRow === row) {
                    cell.style.backgroundColor = '#ffe589';
                } else if (Europa.Controllers.NovaRegraComissao.FilterColumn === colGroup) {
                    cell.style.backgroundColor = '#ffe589';
                }
            }
        });

        $(".jexcel_toolbar", "#matriz_fixa").hide();
        $(".jexcel_pagination", "#matriz_fixa").hide();
        $(".jexcel_contextmenu", "#matriz_fixa").hide();
        $(".jexcel_about", "#matriz_fixa").hide();

        Spinner.Hide();

        //console.log(resp)
    });
};

//Montar matriz nominal por ev
Europa.Controllers.NovaRegraComissao.MontarMatrizNominalEv = function () {
    if (Europa.Controllers.NovaRegraComissao.Parametro.EvsDiferenciadas.length == 0) {
        console.log("Não há evs diferenciadas")
        return;
    }

    var idxEv = Europa.Controllers.NovaRegraComissao.IdxEv;

    var ev = Europa.Controllers.NovaRegraComissao.Parametro.EvsDiferenciadas[idxEv];

    //console.log(ev);

    var regional = Europa.Controllers.NovaRegraComissao.Parametro.Regional;

    if (regional === undefined || regional === "") {
        var tabela = Europa.Controllers.NovaRegraComissao.MatrizNominal;
        if (tabela !== undefined) {
            tabela.destroyMerged();
        }
        return;
    }

    var setup = Europa.Controllers.NovaRegraComissao.Parametro;
    setup.Modalidade = 2;
    setup.IdEmpresaVenda = ev.id;

    Spinner.Show();

    $.post(Europa.Controllers.NovaRegraComissao.Url.MontarMatriz, setup, function (res) {

        var resp = JSON.parse(res);

        var tabela = Europa.Controllers.NovaRegraComissao.MatrizNominal;
        if (tabela !== undefined) {
            tabela.destroyMerged();
            $('#matriz_nominal').html("");
        }

        var data = resp.data;
        if (data === null || data === undefined) {
            data = [[]];
        }

        Europa.Controllers.NovaRegraComissao.MatrizNominal = jexcel(document.getElementById('matriz_nominal'), {
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
                if (Europa.Controllers.NovaRegraComissao.FilterRow !== null && Europa.Controllers.NovaRegraComissao.FilterColumn !== null) {
                    if (Europa.Controllers.NovaRegraComissao.FilterRow === row && (Europa.Controllers.NovaRegraComissao.FilterColumn === colGroup || col === 2)) {
                        cell.style.backgroundColor = '#ffe589';
                    }
                } else if (Europa.Controllers.NovaRegraComissao.FilterRow === row) {
                    cell.style.backgroundColor = '#ffe589';
                } else if (Europa.Controllers.NovaRegraComissao.FilterColumn === colGroup) {
                    cell.style.backgroundColor = '#ffe589';
                }
            }
        });

        $(".jexcel_toolbar", "#matriz_nominal").hide();
        $(".jexcel_pagination", "#matriz_nominal").hide();
        $(".jexcel_contextmenu", "#matriz_nominal").hide();
        $(".jexcel_about", "#matriz_nominal").hide();

        Spinner.Hide();

        //console.log(resp)
    });

};

//Preencher itens diferenciados
Europa.Controllers.NovaRegraComissao.PreencherItensDiferenciados = function () {

    var itens = [];

    var empresaVenda = Europa.Controllers.NovaRegraComissao.Parametro.EvsDiferenciadas[Europa.Controllers.NovaRegraComissao.IdxEv];

    //console.log(empresaVenda)

    //Matriz fixa
    if (Europa.Controllers.NovaRegraComissao.MatrizFixa.options.data.length > 0) {
        Europa.Controllers.NovaRegraComissao.MatrizFixa.getData().forEach(function (row) {

            var quantidadeColunasItem = 7;

            var getFloatValue = function (inputValue) {
                var valor = parseFloat(inputValue.replace(',', '.'));
                if (isNaN(valor)) {
                    valor = 0;
                }
                return (valor + "").replace('.', ',');
            };
            var idx = 1;

            for (var i = 0; i < row.length; i += quantidadeColunasItem, idx++) {

                var item = {
                    Id: 0,
                    RegraComissao: { Id: 0 },
                    EmpresaVenda: { Id: empresaVenda.id },
                    Empreendimento: { Nome: row[i], id: row[i + 1] },
                    faixaUmMeio: getFloatValue(row[i + 2]),
                    faixaDois: getFloatValue(row[i + 3]),
                    ValorKitCompleto: getFloatValue(row[i + 4]),
                    ValorConformidade: getFloatValue(row[i + 5]),
                    ValorRepasse: getFloatValue(row[i + 6]),
                    TipoModalidadeComissao: Europa.Controllers.NovaRegraComissao.ModalidadeFixa
                };

                //Europa.Controllers.NovaRegraComissao.Parametro.ItensDiferenciados.push(item);
                itens.push(item);
            }
        });
    }

    //Matriz nominal
    if (Europa.Controllers.NovaRegraComissao.MatrizNominal.options.data.length > 0) {
        Europa.Controllers.NovaRegraComissao.MatrizNominal.getData().forEach(function (row) {

            var getFloatValue = function (inputValue) {
                var valor = parseFloat(inputValue.replace(',', '.'));
                if (isNaN(valor)) {
                    valor = 0;
                }
                return (valor + "").replace('.', ',');
            };
            var idx = 1
            for (var i = 0; i < row.length; i += 14, idx++) {

                var item = {
                    Id: 0,
                    RegraComissao: { Id: 0 },
                    EmpresaVenda: { Id: empresaVenda.id },
                    Empreendimento: { Nome: row[i], Id: row[i + 1] },
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
                    TipoModalidadeComissao: Europa.Controllers.NovaRegraComissao.ModalidadeNominal
                };
                //Europa.Controllers.NovaRegraComissao.Parametro.ItensDiferenciados.push(item);
                itens.push(item);
            }
        });
    }
        
    //console.log(itens)
    return itens;
}

Europa.Controllers.NovaRegraComissao.ValidarItemRegraComissao = function(itens){

    var valido = true;
    var msgs = [];

    itens.forEach(function (item) {
        //console.log(item)
        var conformidade = parseFloat(item.ValorConformidade.replace(',', '.'));
        var kitCompleto = parseFloat(item.ValorKitCompleto.replace(',', '.'));
        var repasse = parseFloat(item.ValorRepasse.replace(',', '.'));

        var porcentagemTotal = conformidade + kitCompleto + repasse;

        //console.log(porcentagemTotal)
        if (porcentagemTotal > 0 && Math.abs(porcentagemTotal - 100) > 0) {
            msgs.push("Os valores de Kit Completo, Conformidade e Repasse da Empresa de Vendas em relação ao Empreendimento [" + item.Empreendimento.Nome + "] estão inválidos. Os valores somados devem ter o total de 100%");
            valido = false;
            //console.log(msgs)
        }
    });

    var res = {
        Sucesso: valido,
        Mensagens: msgs
    };

    Europa.Informacao.PosAcao(res);

    return valido;
}

Europa.Controllers.NovaRegraComissao.OnChangeTipoRegraComissao = function () {
    var tipo = $("#setup-tipo-regra-comissao").val();

    console.log(tipo)

    if (tipo == 2) {
        $("#vigencia").removeClass("hidden");
    } else {
        $("#vigencia").addClass("hidden")
    }
}

//Gerar regra de comissão
Europa.Controllers.NovaRegraComissao.GerarRegraComissao = function () {
    var setup = Europa.Controllers.NovaRegraComissao.Parametro;

    var tipo = $("#setup-tipo-regra-comissao").val();

    if (tipo == 2) {
        setup.InicioVigencia = $("#setup-inicio-vigencia").val()
        setup.TerminoVigencia = $("#setup-termino-vigencia").val()
    }

    setup.TipoRegraComissao = tipo;

    setup.Descricao = $("#setup-descricao").val();

    var evsComuns = [];

    Europa.Controllers.NovaRegraComissao.Parametro.EvsComuns.forEach(function (ev) {
        evsComuns.push(ev.id);
    })

    Europa.Controllers.NovaRegraComissao.Parametro.EvsComuns = evsComuns;

    var evsDiferenciadas = [];

    Europa.Controllers.NovaRegraComissao.Parametro.EvsDiferenciadas.forEach(function (ev) {
        evsDiferenciadas.push(ev.id);
    })

    Europa.Controllers.NovaRegraComissao.Parametro.EvsDiferenciadas = evsDiferenciadas;

    $.post(Europa.Controllers.NovaRegraComissao.Url.GerarRegraComissao, setup, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.NovaRegraComissao.FecharSetup();
            Europa.Controllers.NovaRegraComissao.GerarPdf(res.Objeto.Id);
        }
        Europa.Informacao.PosAcao(res);
    });
};

//Gerar PDF
Europa.Controllers.NovaRegraComissao.GerarPdf = function (idRegraComissao) {
    console.log("aqui")
    $.post(Europa.Controllers.NovaRegraComissao.Url.GerarPdf, { regraComissao: idRegraComissao }, function (res) {
        if (res.Sucesso) {
            $("#btn_download_pdf").css("display", 'inline');
        }
        Europa.Informacao.PosAcao(res);
    });
};

Europa.Controllers.NovaRegraComissao.InitDatepicker = function () {
    Europa.Controllers.NovaRegraComissao.InicioVigencia = new Europa.Components.DatePicker()
        .WithTarget("#setup-inicio-vigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate(Europa.Date.Now("DD/MM/YYYY"))
        .WithValue(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

    Europa.Controllers.NovaRegraComissao.TerminoVigencia = new Europa.Components.DatePicker()
        .WithTarget("#setup-termino-vigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#setup-inicio-vigencia").val())
        .WithValue($("#setup-inicio-vigencia").val())
        .Configure();
};

Europa.Controllers.NovaRegraComissao.OnChangeInicioVigencia = function () {
    Europa.Controllers.NovaRegraComissao.TerminoVigencia = new Europa.Components.DatePicker()
        .WithTarget("#setup-termino-vigencia")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#setup-inicio-vigencia").val())
        .WithValue($("#setup-inicio-vigencia").val())
        .Configure();
}