Europa.Controllers.MatrizRegraComissaoPadrao.PintarNominal = false;
$(function () {
});

Europa.Controllers.MatrizRegraComissaoPadrao.BuscarMatrizNominal = function () {
    var regional = $("#Regional").val();
    var idRegra = $("#Id").val();

    if (regional === undefined || regional === "") {
        var tabela = Europa.Controllers.MatrizRegraComissaoPadrao.TabelaNominal;
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
        modalidade: Europa.Controllers.MatrizRegraComissaoPadrao.ModalidadeNominal
    };
    Spinner.Show();

    $.post(Europa.Controllers.MatrizRegraComissaoPadrao.Url.Listar, query, function (res) {
        var resp = JSON.parse(res);

        var tabela = Europa.Controllers.MatrizRegraComissaoPadrao.TabelaNominal;
        if (tabela !== undefined) {
            tabela.destroyMerged();
            $('#matriz_pagadoria_nominal').html("");
        }

        var data = resp.data;
        if (data === null || data === undefined) {
            data = [[]];
        }

        Europa.Controllers.MatrizRegraComissaoPadrao.TabelaNominal = jexcel(document.getElementById('matriz_pagadoria_nominal'), {
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
                var cinza = '#e6e6e6';
                //col = col + 3;
                //if (col == 20) {
                //    Europa.Controllers.MatrizRegraComissaoPadrao.PintarNominal = !Europa.Controllers.MatrizRegraComissaoPadrao.PintarNominal;
                //} else if ((col - 20) % 14 == 0 && col > 20) {
                //    Europa.Controllers.MatrizRegraComissaoPadrao.PintarNominal = !Europa.Controllers.MatrizRegraComissaoPadrao.PintarNominal;
                //}

                //if (Europa.Controllers.MatrizRegraComissaoPadrao.PintarNominal) {
                //    cell.style.backgroundColor = '#edf3ff';
                //} else {
                //    cell.style.backgroundColor = '#ffffff';
                //}
                if ((col > 3 && col < 7) || (col > 9 && col < 13)) {
                    cell.style.backgroundColor = cinza;
                } else {
                    cell.style.backgroundColor = '#ffffff';
                }
                

                if (Europa.Controllers.MatrizRegraComissaoPadrao.FilterRowNominal !== null && Europa.Controllers.MatrizRegraComissaoPadrao.FilterColumn !== null) {
                    if (Europa.Controllers.MatrizRegraComissaoPadrao.FilterRowNominal === row && (Europa.Controllers.MatrizRegraComissaoPadrao.FilterColumn === colGroup || col === 2)) {
                        cell.style.backgroundColor = '#ffe589';
                    }
                } else if (Europa.Controllers.MatrizRegraComissaoPadrao.FilterRowNominal === row) {
                    cell.style.backgroundColor = '#ffe589';
                } else if (Europa.Controllers.MatrizRegraComissaoPadrao.FilterColumn === colGroup) {
                    cell.style.backgroundColor = '#ffe589';
                }
            }
        });

        $(".jexcel_toolbar", "#matriz_pagadoria_nominal").hide();
        $(".jexcel_pagination", "#matriz_pagadoria_nominal").hide();
        $(".jexcel_contextmenu", "#matriz_pagadoria_nominal").hide();
        $(".jexcel_about", "#matriz_pagadoria_nominal").hide();

        Spinner.Hide();
    });
};
