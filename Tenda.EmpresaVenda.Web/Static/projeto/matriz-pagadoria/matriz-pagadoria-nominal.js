Europa.Controllers.MatrizPagadoria.PintarNominal = false;
$(function () {
});

Europa.Controllers.MatrizPagadoria.BuscarMatrizNominal = function () {
    var regional = $("#Regional").val();
    var idRegra = $("#Id").val();

    if (regional === undefined || regional === "") {
        var tabela = Europa.Controllers.MatrizPagadoria.TabelaNominal;
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
        modalidade: Europa.Controllers.MatrizPagadoria.ModalidadeNominal
    };
    Spinner.Show();
    
    $.post(Europa.Controllers.MatrizPagadoria.Url.Listar, query, function (res) {
        var resp = JSON.parse(res);

        var tabela = Europa.Controllers.MatrizPagadoria.TabelaNominal;
        if (tabela !== undefined) {
            tabela.destroyMerged();
            $('#matriz_pagadoria_nominal').html("");
        }

        var data = resp.data;
        if (data === null || data === undefined) {
            data = [[]];
        }

        Europa.Controllers.MatrizPagadoria.TabelaNominal = jexcel(document.getElementById('matriz_pagadoria_nominal'), {
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
                //var colGroup = -1;
                //col = col + 3;
                //if (col == 20) {
                //    Europa.Controllers.MatrizPagadoria.PintarNominal = !Europa.Controllers.MatrizPagadoria.PintarNominal;
                //} else if ((col - 20) % 14 == 0 && col > 20) {
                //    Europa.Controllers.MatrizPagadoria.PintarNominal = !Europa.Controllers.MatrizPagadoria.PintarNominal;
                //}

                //if (Europa.Controllers.MatrizPagadoria.PintarNominal) {
                //    cell.style.backgroundColor = '#edf3ff';
                //} else {
                //    cell.style.backgroundColor = '#ffffff';
                //}
                var colGroup = -1;
                cell.style.backgroundColor = '#ffffff';
                var cinza = '#e6e6e6';
                if (col > 2) {
                    var col2 = col - 3;
                    var pos = col2 - (col2 - col2 % 28);
                    colGroup = (col2 - col2 % 14) / 14;
                    if (pos > 1 && pos < 5) {
                        cell.style.backgroundColor = cinza;
                    }
                    else if (pos > 7 && pos < 11) {
                        cell.style.backgroundColor = cinza;
                    }
                    else if (pos > 7 && pos < 11) {
                        cell.style.backgroundColor = cinza;
                    }
                    else if (pos > 15 && pos <19 ) {
                        cell.style.backgroundColor = cinza;
                    }
                    else if (pos > 21 && pos < 25) {
                        cell.style.backgroundColor = cinza;
                    }
                    else if (pos > 13) {
                        cell.style.backgroundColor = '#edf3ff';
                    }
                }
                if (Europa.Controllers.MatrizPagadoria.FilterRowNominal !== null && Europa.Controllers.MatrizPagadoria.FilterColumn !== null) {
                    if (Europa.Controllers.MatrizPagadoria.FilterRowNominal === row && (Europa.Controllers.MatrizPagadoria.FilterColumn === colGroup || col === 2)) {
                        cell.style.backgroundColor = '#ffe589';
                    }
                } else if (Europa.Controllers.MatrizPagadoria.FilterRowNominal === row) {
                    cell.style.backgroundColor = '#ffe589';
                } else if (Europa.Controllers.MatrizPagadoria.FilterColumn === colGroup) {
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
