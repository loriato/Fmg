"use strict";


Europa.Components.DatePicker = function () {
    this.target = undefined;
    this.singleDatePicker = true;
    this.parentEl = undefined;
    this.opens = "left";
    this.drops = "down";
    this.timePicker = false;
    this.timePickerIncrement = 1;
    this.isInvalidDate = undefined;
    this.selectCallBack = undefined;
    this.showCallBack = undefined;
    this.autoApply = true;
};

//Builder pattern
Europa.Components.DatePicker.prototype.WithFormat = function (format) {
    this.format = format;
    return this;
};

Europa.Components.DatePicker.prototype.WithAutoApply = function (autoApply) {
    this.autoApply = autoApply;
    return this;
};

Europa.Components.DatePicker.prototype.WithOpens = function (opens) {
    this.opens = opens;
    return this;
};

Europa.Components.DatePicker.prototype.WithDrops = function (drops) {
    this.drops = drops;
    return this;
};

Europa.Components.DatePicker.prototype.WithValue = function (value) {
    this.Value(value);
    return this;
};

Europa.Components.DatePicker.prototype.WithTarget = function (targetSelector) {
    this.target = targetSelector;
    return this;
};

Europa.Components.DatePicker.prototype.WithParentEl = function (parentEl) {
    this.parentEl = parentEl;
    return this;
};

Europa.Components.DatePicker.prototype.WithMinDate = function (minDate) {
    this.minDate = minDate;
    return this;
};

Europa.Components.DatePicker.prototype.WithMaxDate = function (maxDate) {
    this.maxDate = maxDate;
    return this;
};

Europa.Components.DatePicker.prototype.WithStartDate = function (startDate) {
    this.startDate = startDate;
    return this;
};

Europa.Components.DatePicker.prototype.WithEndDate = function (endDate) {
    this.endDate = endDate;
    return this;
};

Europa.Components.DatePicker.prototype.WithTimePicker = function () {
    this.timePicker = true;
    return this;
};

Europa.Components.DatePicker.prototype.WithTimePicker24h = function () {
    this.timePicker24Hour = true;
    return this;
};

Europa.Components.DatePicker.prototype.WithTimePickerIncrement = function (increment) {
    this.timePickerIncrement = increment;
    return this;
};

Europa.Components.DatePicker.prototype.IsSingle = function () {
    this.singleDatePicker = true;
    return this;
};

Europa.Components.DatePicker.prototype.IsRange = function () {
    this.singleDatePicker = false;
    return this;
};

Europa.Components.DatePicker.prototype.WithInvalidDateFunction = function (callback) {
    this.isInvalidDate = callback;
    return this;
};


Europa.Components.DatePicker.prototype.BaseConfig = function () {
    var self = this;
    return {
        "autoUpdateInput": false,
        "singleDatePicker": self.singleDatePicker,
        "autoApply": self.autoApply,
        "parentEl": self.parentEl,
        "showDropdowns": true,
        "locale": {
            "format": self.format,
            "separator": " - ",
            "applyLabel": "Selecionar",
            "cancelLabel": "Cancelar",
            "fromLabel": "De",
            "toLabel": "Até",
            "customRangeLabel": "Customizado",
            "weekLabel": "W",
            "daysOfWeek": [
                "Do",
                "Se",
                "Te",
                "Qa",
                "Qu",
                "Se",
                "Sa"
            ],
            "monthNames": [
                "Janeiro",
                "Fevereiro",
                "Março",
                "Abril",
                "Maio",
                "Junho",
                "Julho",
                "Agosto",
                "Setembro",
                "Outubro",
                "Novembro",
                "Dezembro"
            ],
            "firstDay": 1
        },
        "linkedCalendars": false,
        "opens": self.opens,
        "drops": self.drops,
        "timePicker": self.timePicker,
        "timePicker24Hour": self.timePicker24Hour,
        "timePickerIncrement": self.timePickerIncrement,
        "format": self.format,
        "minDate": self.minDate,
        "maxDate": self.maxDate,
        "startDate": self.startDate,
        "endDate": self.endDate,
        "isInvalidDate": self.isInvalidDate
    };
};

Europa.Components.DatePicker.prototype.WithOptions = function (options) {
    this.AditionalOptions = options;
    return this;
};

Europa.Components.DatePicker.prototype.WithSelectCallBack = function (callback) {
    this.selectCallBack = callback;
    return this;
};

Europa.Components.DatePicker.prototype.WithShowCallBack = function (callback) {
    this.showCallBack = callback;
    return this;
};

Europa.Components.DatePicker.prototype.Configure = function () {
    if (this.format == undefined) {
        this.format = "DD/MM/YYYY";
    }
    if (this.parentEl == undefined) {
        this.parentEl = "body";
    }
    this.maxlength = this.format.length;
    var component = $(this.target);
    var self = this;

    var baseConfig = self.BaseConfig();
    if (self.AditionalOptions !== undefined) {
        for (var key in self.AditionalOptions) {
            if (self.AditionalOptions.hasOwnProperty(key)) {
                baseConfig[key] = self.AditionalOptions[key];
            }
        }
    }

    component.daterangepicker(baseConfig, function (start, end, label) { });
    if (this.value == undefined || this.value === "") {
        component.val("");
    } else {
        component.val(Europa.Date.Format(this.value, this.format));
    }

    component.on("apply.daterangepicker", function (ev, picker) {
        if (self.singleDatePicker) {
            component.val(Europa.Date.Format(new Date(picker.startDate), self.format)).trigger("change");
        } else {
            component.val(Europa.Date.Format(new Date(picker.startDate), self.format) + ' - ' + Europa.Date.Format(new Date(picker.endDate), self.format));
        }
        if (self.selectCallBack) {
            self.selectCallBack();
        }
    });

    component.on("show.daterangepicker", function (ev, picker) {
        if (self.showCallBack) {
            self.showCallBack()
        }
    });

    component.on("cancel.daterangepicker", function (ev, picker) {
        component.val("").trigger("change");
    });


    component.on('focus, blur', function () {
        if($(this).val() === '') {
            $(this).data('daterangepicker').setStartDate(Europa.Date.Format(moment().startOf('day'), self.format));
            $(this).data('daterangepicker').setEndDate(Europa.Date.Format(moment().endOf('day'), self.format));
    }
    });

    return this;
};

Europa.Components.DatePicker.Mask = function (element, format) {
    var replaced = format.replace(/D/g, "0")
        .replace(/M/g, "0")
        .replace(/Y/g, "0")
        .replace(/H/g, "0")
        .replace(/S/g, "0");
    element.mask(replaced);
};

Europa.Components.DatePicker.prototype.Value = function () {
    if (arguments.length > 0) {
        this.value = arguments[0];
    }
    return this.value;
};

//Static Methods
Europa.Components.DatePicker.AutoApply = function () {
    $("input[datepicker]").each(function () {
        var element = $(this);
        var datepicker = new Europa.Components.DatePicker()
        .WithTarget(this)
        .WithFormat(element.attr("format"))
        .WithParentEl(element.attr("parent"))
        .WithValue(element.attr("value"));

        if (element.attr("min-date") != undefined) {
            datepicker = datepicker.WithMinDate(element.attr("min-date"));
        }

        if (element.attr("drops") != undefined) {
            datepicker = datepicker.WithDrops(element.attr("drops"));
        }
        if (element.attr("opens") != undefined) {
            datepicker = datepicker.WithOpens(element.attr("opens"));
        }

        datepicker = datepicker.Configure();
        Europa.Components.DatePicker.Mask(element, datepicker.format);
        element.attr("maxlength", datepicker.maxlength);

        $(this).on('focus, blur', function () {
            if ($(this).val() === '') {
                $(this).data('daterangepicker').setStartDate(Europa.Date.Format(moment().startOf('day'), datepicker.format));
                $(this).data('daterangepicker').setEndDate(Europa.Date.Format(moment().endOf('day'), datepicker.format));
            }
        });
    });
};

Europa.Components.DatePicker.Closest = function (element) {
    $(element).parent().parent().children().first().click();
};

Europa.Components.DatePicker.ClosestInput = function (element) {
    $(element).closest('.input-group').find('input[type=text]').click();
};

Europa.Components.DatePicker.LinkComponents = function (from, until) {
    var jFrom = $(from);
    var jUntil = $(until);

    var componentFrom = jFrom.data("daterangepicker");
    var componentUntil = jUntil.data("daterangepicker");

    jFrom.on("apply.daterangepicker", function (ev, picker) {
        componentUntil.minDate = picker.startDate;
    });

    jUntil.on("apply.daterangepicker", function (ev, picker) {
        componentFrom.maxDate = picker.startDate;
    });
};


Europa.Components.DatePicker.prototype.SetDate = function (date) {
    $(this.target).data('daterangepicker').setStartDate(date);
    $(this.target).data('daterangepicker').setEndDate(date);
    $(this.target).data('daterangepicker').updateView();
    return this;
};

Europa.Components.DatePicker.CleanEvents = function (element) {
    $(element).unbind("apply.daterangepicker");
    $(element).unbind("cancel.daterangepicker");
};

Europa.Components.DatePicker.GetTemplate = function (id) {
    var buf = '<div class="daterangepicker dropdown-menu" id="' + id + '">' +
        '<div class="calendar left">' +
        '<div class="daterangepicker_input">' +
        '<input class="input-mini form-control" type="text" name="daterangepicker_start" value="" />' +
        '<i class="far fa-calendar glyphicon glyphicon-calendar"></i>' +
        '<div class="calendar-time">' +
        '<div></div>' +
        '<i class="far fa-clock glyphicon glyphicon-time"></i>' +
        '</div>' +
        '</div>' +
        '<div class="calendar-table"></div>' +
        '</div>' +
        '<div class="calendar right">' +
        '<div class="daterangepicker_input">' +
        '<input class="input-mini form-control" type="text" name="daterangepicker_end" value="" />' +
        '<i class="far fa-calendar glyphicon glyphicon-calendar"></i>' +
        '<div class="calendar-time">' +
        '<div></div>' +
        '<i class="far fa-clock glyphicon glyphicon-time"></i>' +
        '</div>' +
               '</div>' +
               '<div class="calendar-table"></div>' +
           '</div>' +
           '<div class="ranges">' +
               '<div class="range_inputs">' +
                   '<button class="applyBtn" disabled="disabled" type="button"></button> ' +
                   '<button class="cancelBtn" type="button"></button>' +
               '</div>' +
           '</div>' +
       '</div>';
    return buf;
};
