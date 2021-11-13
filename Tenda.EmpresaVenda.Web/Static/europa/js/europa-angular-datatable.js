
function DataTableWrapper(vm, scope, compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    this.vm = vm;
    this.vm.dtInstance = {};
    this.scope = scope;
    this.compile = compile;
    this.DTOptionsBuilder = DTOptionsBuilder;
    this.DTColumnBuilder = DTColumnBuilder;
    this.DTColumnDefBuilder = DTColumnDefBuilder;
    this.hasColActions = false;
    this.filters = {};
    this.multiSelection = false;
    this.templateEdit = undefined;
    this.actionSave = {};
    this.actionCancel = this.closeEditionBtn;
    this.idxRowEditing = 0;
    this.autoInit = true;
    this.lastRequestParams = undefined;
    this.templatePaginator = '<"europa-datatable-acompanhamento"><"europa-datatable-container-block" <"europa-datatable-processing" r> <"europa-datatable-reload">  <"europa-datable-info" i> <"europa-datable-paginator"p> <"europa-datatable-max-visible" l> <"europa-datatable-navegar-para">>';
    this.showFooter = true;
    this.showHeader = true;
    this.doubleClickOnRow = false;
    this.defaultLength = undefined;
    this.paging = true;
    this.rowReorder = false;
    this.scrollY = undefined;
    this.footerCallback = undefined;
    this.footer = false;
    var languagePt = this.getPtBr();
    this.areaNavegarPara = {
        html: '<input type="text" name="datatables_navegar_para" placeholder="' + languagePt.navegarPara + '" title="' + languagePt.navegarParaTitle + '" class="form-control" />',
        maskInput: "#9"
    };
}

DataTableWrapper.prototype = {
    getActionsEdit: function () {
        this.btnSave = { html: '<button class="btn btn-success" title="salvar"><i class="fa fa-save"><i/></button>' };
        this.btnCancel = { html: '<button class="btn btn-danger" title="cancelar" ng-click="closeEdition"><i class="fa fa-ban"><i/></button>' };
        this.btnSave.to$ = $(this.btnSave.html);
        this.btnCancel.to$ = $(this.btnCancel.html);

        this.btnSave.to$.on("click", { wrapper: this }, this.actionSave);
        this.btnCancel.to$.on("click", { wrapper: this }, this.actionCancel);

        return $("<div></div>").append(this.btnSave.to$).append(this.btnCancel.to$);
    },

    setDefaultOptions: function (methodRequest, urlRequest, dataParams) {
        var self = this;

        this.vm.dtOptions = this.DTOptionsBuilder
            .newOptions()
            .withOption("fnServerData",
                function (sSource, aoData, fnCallback, oSettings) {
                    var draw = aoData[0].value;
                    var columns = aoData[1].value;
                    var orderAux = aoData[2].value;
                    var start = aoData[3].value;
                    var length = aoData[4].value;

                    var table = $(self.vm.dtInstance.dataTable).parent().parent().parent();
                    var reloadArea = table.find('.europa-datatable-reload');
                    reloadArea.hide();

                    var filter = [];
                    $.each(columns,
                        function (idx, val) {
                            var dataCol = $(this)[0];
                            if (dataCol.searchable && dataCol.search.value !== "") {
                                filter.push({
                                    "column": dataCol.data,
                                    "value": dataCol.search.value,
                                    "regex": dataCol.search.regex
                                });
                            }
                        });

                    var order = [];
                    $.each(orderAux,
                        function (idx, val) {
                            var dataCol = $(this)[0];
                            order.push({ "column": columns[orderAux[idx].column].data, "value": orderAux[idx].dir });
                        });

                    var mapParams = {};
                    mapParams["draw"] = draw;
                    mapParams["start"] = start;
                    mapParams["pageSize"] = length;
                    mapParams["order"] = order;
                    mapParams["filter"] = filter;

                    if (dataParams !== undefined) {
                        var params = dataParams();
                        for (var key in params) {
                            if (params.hasOwnProperty(key)) {
                                mapParams[key] = params[key];
                            }
                        }
                    }

                    self.lastRequestParams = mapParams;

                    if (urlRequest instanceof Function) {
                        urlRequest = urlRequest();
                    }

                    if (urlRequest !== undefined && self.autoInit === true) {
                        $.ajax({
                            method: methodRequest,
                            url: urlRequest,
                            data: mapParams
                        })
                            .then(function (result) {
                                var records = {
                                    'draw': draw,
                                    'recordsTotal': result.total,
                                    'recordsFiltered': result.filtered,
                                    'data': result.records
                                };
                                fnCallback(records);
                            });
                    } else {
                        var records = {
                            'draw': 0,
                            'recordsTotal': 0,
                            'recordsFiltered': 0,
                            'data': []
                        };
                        fnCallback(records);
                    }
                })
            .withOption('fnDrawCallback',
                function (oSettings, oData) {
                    Europa.GridClickLink();
                    if (self.reloadCallbackFunction !== undefined) {
                        self.reloadCallbackFunction();
                    }
                    var table = $(self.vm.dtInstance.dataTable).parent().parent().parent();
                    var reloadArea = table.find('.europa-datatable-reload');
                    var navegarArea = table.find('.europa-datatable-navegar-para');
                    var selectionButtons = "";
                    if (self.multiSelection) {
                        selectionButtons = '<button title="Selecionar Todos" class="btn btn-default btn-sm selectAllDt" aria-label="Selecionar Todos"> <i class="fa fa-check-square-o"></i></button>'
                            + '<button title="Remover Seleção" class="btn btn-default btn-sm deselectAllDt"> <i class="fa fa-square-o" aria-label="Remover Seleção"></i></button>';
                    }
                    reloadArea.html('<button title="Atualizar" class="btn btn-default btn-sm reloadDt"> <i class="fa fa-refresh"></i></button>' + selectionButtons);

                    if (self.pageExport) {
                        table.find('.europa-datatable-buttons').show();
                    }
                    reloadArea.show();
                    reloadArea.find('.reloadDt').on('click', function () {
                        reloadArea.hide();
                        self.scope.reloadData();
                    });

                    reloadArea.find('.selectAllDt').on('click', function () {
                        self.scope.selectAllRows();
                    });

                    reloadArea.find('.deselectAllDt').on('click', function () {
                        self.scope.deselectAllRows();
                    });

                    navegarArea.html(self.areaNavegarPara.html);
                    navegarArea.find('input').mask('9#');
                    navegarArea.find('input').keypress(function (event) {
                        var keycode = (event.keyCode ? event.keyCode : event.which);
                        if (keycode == '13') {
                            var page = parseInt(navegarArea.find('input').val());
                            self.navigate(page);
                        }
                    });
                    navegarArea.find('button').on('click', function () {
                        var page = parseInt(navegarArea.find('input').val());
                        self.navigate(page);
                    });
                })
            .withDataProp("data")
            .withOption("searching", false)
            .withOption("processing", true)
            .withOption("serverSide", true)
            .withOption("paging", this.paging)
            .withPaginationType("numbers")
            .withDisplayLength(10)
            .withPaginationType("full_numbers")
            .withOption("createdRow",
                function (row, data, dataIndex) {
                    self.compile(angular.element(row).contents())(self.scope);
                })
            .withOption("order",
                this.defaultOrder === undefined ? [this.hasColActions ? 1 : 0, "asc"] : this.defaultOrder)
            .withDOM('<<"europa-datatable-custom-header"><"europa-datatable-table"t ><"europa-datatable-footer" ' +
                self.templatePaginator +
                ">>")
            .withOption("responsive", true)
            .withOption("rowCallback",
                function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    if (self.selectable) {
                        $("td:not(.datatable-actions)", nRow).unbind("click");
                        $("td:not(.datatable-actions)", nRow)
                            .bind("click",
                                function () {
                                    self.scope.$apply(function () {
                                        if (self.scope.onRowSelect != undefined) {
                                            self.scope.onRowSelect(aData);
                                        }
                                    });
                                });
                    }
                    if (self.doubleClickOnRow) {
                        $("td:not(.datatable-actions)", nRow).unbind("dblclick");
                        $("td:not(.datatable-actions)", nRow)
                            .bind("dblclick",
                                function () {
                                    self.scope.$apply(function () {
                                        if (self.doubleClickOnRow != undefined) {
                                            self.scope.onDoubleClickOnRow(nRow, aData);
                                        }
                                    });
                                });
                    }

                    return nRow;
                })
            .withOption("footerCallback", function (row, data, start, end, display) {
                var api = this.api(), data;
                var table = $(self.vm.dtInstance.dataTable).parent().parent().parent();
                var acompanhamentoArea = table.find('.europa-datatable-acompanhamento');
                if (self.footer == true) {
                    self.footerCallback(api, acompanhamentoArea);
                }

            })
            .withOption("initComplete",
                function (settings, json) {
                    self.scope.selectAll = function () {
                        self.vm.dtInstance.DataTable.rows().select();
                    };

                    self.scope.getRowsSelect = function () {
                        var data = self.vm.dtInstance.DataTable.rows({ selected: true }).data();
                        if (self.multiSelection) {
                            var rows = [];
                            var size = self.vm.dtInstance.DataTable.rows({ selected: true }).count();
                            for (var i = 0; i < size; i++) {
                                rows[i] = data[i];
                            }
                            return rows;
                        }

                        return data[0];
                    };

                    self.scope.reloadData = function (callback, resetPaginator) {
                        self.autoInit = true;
                        var callbackA = self.reloadCallbackFunction;
                        if (callbackA === undefined && typeof callback == "function") {
                            callbackA = callback;
                        }
                        callback = callbackA;
                        self.vm.dtInstance.DataTable.ajax.reload(callback, resetPaginator);
                    };

                    self.scope.selectAllRows = function () {
                        if (self.multiSelection) {
                            self.vm.dtInstance.DataTable.rows().select();
                        }
                    };

                    self.scope.deselectAllRows = function () {
                        self.vm.dtInstance.DataTable.rows().deselect();
                    };

                    self.scope.getRowData = function (idx) {
                        return self.getRowData(idx);
                    };

                    self.scope.rowEdit = function (idxRow) {
                        self.rowEdit(idxRow);
                    };
                })
            .withOption("fnHeaderCallback",
                function (nRow, aaData, iStart, iEnd, aiDisplay) {
                    var head = $(nRow).parent();
                    var table = $(nRow).parent().parent().parent().parent();

                    if (self.templateEdit !== undefined && head.find(".newRow").length <= 0) {
                        head.append($('<tr class="newRow" style="display:none">'));
                    }

                    if (self.idAreaHeader != undefined) {
                        var headerCustom = table.find(".europa-datatable-custom-header");
                        headerCustom.show();
                        $("#" + self.idAreaHeader).prependTo(headerCustom);
                        $("#" + self.idAreaHeader).show();
                    }
                    if (self.showHeader != true) {
                        table.find(".europa-datatable-custom-header").hide();
                    }
                    if (self.showFooter != true) {
                        table.find(".europa-datatable-footer").hide();
                    }
                })
            .withBootstrap()
            .withOption("autoWidth", false);

        if (self.rowReorder) {
            this.vm.dtOptions.withOption('rowReorder', {
                selector: ':visible',
                update: false
            })
        }

        if (this.filters != undefined) {
            this.vm.dtOptions.withColumnFilter({
                aoColumns: this.filters
            });
        }

        if (this.defaultLength != undefined) {
            this.vm.dtOptions.withDisplayLength(this.defaultLength);
        }

        if (this.scrollY) {
            this.vm.dtOptions.withOption("scrollY", this.scrollY);
        }

        //this.DTOptionsBuilder.setLoadingTemplate('<img src="images/spinningwheel.gif">');

        this.defineLanguagePtBr();
    },

    navigate: function (page) {
        var max = this.vm.dtInstance.DataTable.page.info().pages;
        if (page > max) {
            page = max;
        }
        this.vm.dtInstance.DataTable.page(page - 1).draw('page');
    },

    setAutoInit: function () {
        this.autoInit = false;
        return this;
    },

    setRowReorder: function (allow) {
        this.rowReorder = allow;
        return this;
    },

    createRowNewData: function () {
        this.closeEdition();
        $(this.vm.dtInstance.dataTable).find(".newRow").show();
        var tr = $(this.vm.dtInstance.dataTable).find(".newRow");
        tr.html("");

        var th = $("<th class='center-btn-actions'>");
        th.append(this.compile(this.getActionsEdit())(this.scope));
        tr.append(th);

        $.each(this.templateEdit, function (idx, val) {
            var th = $("<th>");
            th.append(val);
            tr.append(th);
        });
        tr.append("</tr>");
    },

    setIdAreaHeader: function (idAreaHeader) {
        this.idAreaHeader = idAreaHeader;
        $("#" + this.idAreaHeader).hide();
        return this;
    },

    defineLanguagePtBr: function () {
        var ptBr = this.getPtBr();

        this.vm.dtOptions.withLanguage(ptBr);
    },


    getPtBr: function () {
        return {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "(_START_ até _END_) de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_",
            "sLoadingRecords": "Carregando...",
            "sProcessing": '<i class="fa fa-refresh faa-spin animated" style="color: #810608"> </i>',
            //"sProcessing": '<div class="spinner-loader"></div>',
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "<i class='fa fa-caret-right' title='Próxima página'></i>",
                "sPrevious": "<i class='fa fa-caret-left' title='Página anterior'></i>",
                "sFirst": "<i class='fa fa-step-backward' title='Primeira página'></i>",
                "sLast": "<i class='fa fa-step-forward' title='Última página'></i>"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            },
            navegarPara: "Navegar",
            navegarParaTitle: "Digite o número da página e pressione enter para navegar",
            ir: "Ir",
            select: {
                rows: {
                    _: "",
                    0: "",
                    1: ""
                }
            }
        };
    },

    setOptionsSelect: function (methodRequest, urlRequest, dataRequest, multi) {
        this.selectable = true;
        this.setDefaultOptions(methodRequest, urlRequest, dataRequest);
        this.vm.dtOptions.withSelect({
            style: multi ? "multi" : "single",
            selector: "td:not(.datatable-actions)"
        });
    },

    setOptionsMultiSelect: function (methodRequest, urlRequest, dataRequest) {
        this.multiSelection = true;
        this.setOptionsSelect(methodRequest, urlRequest, dataRequest, true);
    },

    withColActions: function (func, width) {
        this.hasColActions = true;

        return this.DTColumnBuilder.newColumn(null)
            .withTitle("Ações")
            .withClass("datatable-actions center-btn-actions")
            .withOption("width", width)
            .renderWith(func)
            .notSortable();
    },

    setColActions: function (func, width) {
        if (width == undefined) {
            width = "90px";
        }
        if (this.vm.dtColumns == undefined) {
            this.vm.dtColumns = this.withColActions(func, width);
        } else {
            this.vm.dtColumns.splice(0, 0, this.withColActions(func, width));
        }
        return this;
    },

    setFilters: function (filters) {
        this.filters = filters;
    },

    setColumns: function (cols, func) {
        var self = this;

        if (func != undefined) {
            cols.splice(0, 0, this.withColActions(func));
        }

        if (this.vm.dtColumns != undefined) {
            cols.splice(0, 0, this.vm.dtColumns);
        }

        this.vm.dtColumns = cols;
        $.each(this.vm.dtColumns, function (idx, val) {
            if (val.sDefaultContent == undefined) {
                val.sDefaultContent = "";
            }
            if (val.mRender == undefined) {
                val.mRender = self.renderColumn;
            }
        });
        return this;
    },

    renderDateSmall: function (value, type, full) {
        if (value === null) return "";

        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));

        return Europa.Date.toSmallDate(dt);
    },

    renderColumn: function (value, type, full, meta) {
        if (value === null || value === undefined || value.length === 0 || value.toString().trim() === "") {
            return "";
        }
        var typeColumn = meta.settings.aoColumns[meta.col].type;
        if (typeColumn != null) {
            var formatType = typeColumn.split("-format-");
            switch (formatType[0]) {
                case "enum":
                    return Europa.i18n.Enum.Resolve(formatType[1], value);
                case "date":
                    return Europa.Date.toFormatLongDate(value, formatType[1]);
                default:
                    return value;
            }
        }

        var linkColumn = meta.settings.aoColumns[meta.col].link;
        if (linkColumn != null) {
            var linkType = linkColumn.split("-url-");
            var url = linkType[0];
            var idColumn = linkType[1];
            var link = url;
            if (idColumn) {
                link = link + "/" + idColumn.split('.').reduce((a, b) => a[b], full);

            }
            var classe = "";
            var tab = meta.settings.aoColumns[meta.col].tab;
            if (tab != null && tab) {
                classe = classe.concat(" ").concat("as_tab");
            }
            return "<a class='" + classe + "' href='" + link + "'rel='noopener noreferrer' target='_blank'>" + value + "</a>";
        }
        return value;
    },

    setTemplateEdit: function (templateArr, primaryKey) {
        this.templateEdit = templateArr;
        if (primaryKey === undefined) {
            primaryKey = "Id";
        }
        this.templateEdit.primaryKey = primaryKey;
        return this;
    },

    setActionSave: function (action) {
        this.actionSave = action;
        return this;
    },

    setActionCancel: function (action) {
        this.actionCancelTable = action;
        return this;
    },

    closeEdition: function () {
        var event = { data: { wrapper: this } };
        this.closeEditionBtn(event);
    },

    closeEditionBtn: function (event) {
        var self = event.data.wrapper;
        var idxRow = self.idxRowEditing;
        if ($(self.vm.dtInstance.DataTable.table().table).find(".newRow").is(":visible")) {
            $(self.vm.dtInstance.DataTable.table().table).find(".newRow").hide();
            $(self.vm.dtInstance.DataTable.table().table).find(".newRow").html("");
            return;
        }

        var table = self.vm.dtInstance.DataTable;
        self.compile($(table.cell(idxRow, 0).node()).html(self.nodeActions))(self.scope);

        $.each(self.templateEdit, function (idx, val) {
            var cell = table.cell(idxRow, idx + 1);
            cell.data(cell.data());
        });

        $(self.vm.dtInstance.DataTable.table().table)
            .find(".newRow")
            .find(":input")
            .not(":button, :submit, :reset")
            .val("")
            .removeAttr("checked")
            .removeAttr("selected");

        $(self.vm.dtInstance.DataTable.table().table)
            .find("tbody")
            .find(":input")
            .not(":button, :submit, :reset")
            .val("")
            .removeAttr("checked")
            .removeAttr("selected");

        if (self.actionCancelTable !== undefined) {
            self.actionCancelTable();
        }
    },

    reloadData: function (callback, resetPaginator) {
        this.scope.reloadData(callback == undefined, resetPaginator);
    },

    getRowsSelect: function () {
        return this.scope.getRowsSelect();
    },

    rowEdit: function (idxRow) {
        this.closeEdition();

        var table = this.vm.dtInstance.DataTable;

        this.idxRowEditing = idxRow;
        this.nodeActions = $(table.cell(idxRow, 0).node()).html();

        this.compile($(table.cell(idxRow, 0).node()).html(this.getActionsEdit()))(this.scope);

        $.each(this.templateEdit, function (idx, val) {
            var cell = table.cell(idxRow, idx + 1);
            var input = $(val);
            if (input.is(':checkbox') || input.is(':radio')) {
                $.each(input, function (idx2, val2) {
                    if ($(this).val() + ' ' === cell.data() + ' ') {
                        $(this).prop('checked', true);
                    }
                });
            } else {
                input.val(cell.data());
            }
            var format = input.attr("field-format");
            if (format !== undefined) {
                input.val(Europa.WindowFunction(format, window, val));
            }

            $(cell.node()).html(input);
        });
    },

    renderButton: function (hasPermission, title, icon, onClick, disabled) {
        if (hasPermission === false || hasPermission === 'false' || hasPermission === 'False') {
            return "";
        }
        icon = $('<i/>').addClass(icon);

        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);
        if (disabled != undefined && disabled) {
            button.addClass('disabled');
        }

        return button.prop('outerHTML');
    },

    renderButtonLink: function (hasPermission, title, icon, href, disabled) {
        if (hasPermission === false || hasPermission === 'false' || hasPermission === 'False') {
            return "";
        }
        icon = $('<i/>').addClass(icon);

        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('href', href)
            .append(icon);;
        if (disabled != undefined && disabled) {
            button.addClass('disabled');
        }

        return button.prop('outerHTML');
    },

    renderWithLink: function (value, url, classe) {
        var aux = 'europa_detail';
        if (classe) {
            aux = aux + " " + classe;
        }
        return "<a class='" + aux + "' data-href='" + url + "'>" + value + "</a>";
    },

    renderWithLinkId: function (value, url, id, classe) {
        var link = url + "/" + id;
        return this.renderWithLink(value, link, classe);
    },

    withOptionLink: function (url, idColumn) {
        if (idColumn) {
            return url + "-url-" + idColumn;
        }
        return url;
    },

    getDataRowEdit: function () {
        var thead = $(this.vm.dtInstance.dataTable).find(".newRow");
        var tbody = $(this.vm.dtInstance.dataTable).find("tbody");
        var result = {};
        var values = undefined;
        if (thead.is(":visible")) {
            result[this.templateEdit.primaryKey] = 0;
            values = thead.find(":input").not("button");
        }
        else if (tbody.find(":input").not(":button").length > 0) {
            result[this.templateEdit.primaryKey] = this.vm.dtInstance.DataTable.table().row(this.idxRowEditing).data()[this.templateEdit.primaryKey];
            values = tbody.find(":input").not(":button");
        }
        if (values !== undefined) {
            $.each(values, function (idx, val) {
                result[$(val).attr("name")] = $(val).val() == null ? "" : $(val).val();
            });
        }
        return result;
    },

    getDataRowEdition: function () {
        var thead = $(this.vm.dtInstance.dataTable).find(".newRow");
        var tbody = $(this.vm.dtInstance.dataTable).find("tbody");
        var result = {};
        var values = undefined;
        var primary = undefined;

        if (thead.is(":visible")) {
            primary = 0;
            values = thead.find(":input").not("button");
        }
        else if (tbody.find(":input").not(":button").length > 0) {
            primary = this.vm.dtInstance.DataTable.table().row(this.idxRowEditing).data()[this.templateEdit.primaryKey];
            values = tbody.find(":input").not(":button");
        }
        if (values !== undefined) {
            result = Europa.Form.SerializeJsonInputs(values);
            result[this.templateEdit.primaryKey] = primary;
        }
        return result;
    },

    getAreaEdit: function () {
        var thead = $(this.vm.dtInstance.dataTable).find(".newRow");
        var tbody = $(this.vm.dtInstance.dataTable).find("tbody");
        if (thead.is(":visible")) {
            return thead;
        }
        return tbody;
    },

    getRowData: function (idxRow) {
        var tabela = this.vm.dtInstance.DataTable;
        var row = tabela.row(idxRow).data();
        return row;
    },

    setCallBackReload: function (func) {
        this.reloadCallbackFunction = func;
        return this;
    },

    setDefaultOrder: function (arr) {
        this.defaultOrder = arr;
        return this;
    },


    setHideFooter: function () {
        this.showFooter = false;
        return this;
    },

    setHideHeader: function () {
        this.showHeader = false;
        return this;
    },

    setDoubleClickOnRowActive: function () {
        this.doubleClickOnRow = true;
        return this;
    },

    setDefaultLength: function (length) {
        this.defaultLength = length;
        return this;
    },

    setPaging: function (isPaging) {
        this.paging = isPaging;
        return this;
    },

    setScrollY: function (length) {
        this.scrollY = length;
        return this;
    },

    setFooterCallback: function (callback) {
        this.footerCallback = callback;
        return this;
    },

    setFooter: function () {
        this.footer = true;
        return this;
    }
}