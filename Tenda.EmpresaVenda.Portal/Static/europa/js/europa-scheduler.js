"use strict";

Europa.Components.Scheduler = function(id) {
    this.id = id;
    this.selectorId = "#" + id;
    this.sendRequest = true;
    this.ajax = false;
    this.onlyWeekAndDay = false;
    this.height = "auto";
    this.dateFormat = "dd/MM/yyyy";
    this.reloadButton = false;
    this.scrollTime = '08:00:00';
}

Europa.Components.Scheduler.prototype = {
    WithAjax: function(urlRequest, typeRequest, dataParamsRequest, functionParseData, functionRequestError) {
        this.urlRequest = urlRequest;
        this.typeRequest = typeRequest;
        this.functionRequestError = functionRequestError;
        this.functionParseData = functionParseData;
        this.dataParamsRequest = dataParamsRequest;
        this.ajax = true;
        return this;
    },

    WithAutoInit: function(autoInit) {
        this.sendRequest = autoInit;
        return this;
    },

    GoToDate: function (date, autoUpdate) {
        this.sendRequest = autoUpdate? autoUpdate : true ;
        $(this.selectorId).fullCalendar("gotoDate", date);
    },

    ReloadCalendar: function() {
        this.sendRequest = true;
        $(this.selectorId).fullCalendar("refetchEvents");
    },

    Configure: function() {
        var self = this;
        var events = [];
        if (this.urlRequest != undefined) {
            events = function(start, end, timezone, callback) {
                if (self.sendRequest) {
                    var mapParams = {};
                    mapParams["dateStart"] = Europa.Date.toFormatDate(start, Europa.Date.FORMAT_DATE);
                    mapParams["dateEnd"] = Europa.Date.toFormatDate(end, Europa.Date.FORMAT_DATE);

                    if (self.dataParamsRequest !== undefined) {
                        var params = self.dataParamsRequest();
                        for (var key in params) {
                            if (params.hasOwnProperty(key)) {
                                mapParams[key] = params[key];
                            }
                        }
                    }
                    self.lastRequestParams = mapParams;
                    $.ajax({
                        url: self.urlRequest,
                        type: self.typeRequest,
                        data: mapParams,
                        success: function(result) {
                            var events = [];
                            if (result.Sucesso) {
                                $.each(result.Objeto,
                                    function (idx, val) {
                                        var event = self.functionParseData(val);
                                        if (event instanceof Array) {
                                            $.each(event, function (idx2, val2) { events.push(val2) });
                                        } else {
                                            events.push(val);
                                        }
                                    });
                            } else {
                                self.functionRequestError(result);
                            }
                            callback(events);
                        },
                        error: function(result) {
                            self.functionRequestError(result);
                        }
                    });
                }
            }
        };
        $(this.selectorId)
            .fullCalendar({
                themeSystem: 'jquery-ui',
                theme: false,
                height: self.height,
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: self.onlyWeekAndDay ? 'refresh agendaWeek,agendaDay' : 'refresh month,agendaWeek,agendaDay'
                },
                timeFormat: self.timeFormat != undefined ? self.timeFormat : 'HH(:mm)',
                defaultView: self.defaultView != undefined ? self.defaultView : 'month',
                slotDuration: '00:15',
                slotLabelInterval: '01:00',
                dateFormat: self.dateFormat,
                editable: false,
                locale: "pt-br",
                events: events,
                displayEventTime: false,
                eventClick: self.eventClickFunction,
                dayClick: self.dayClickFunction,
                scrollTime: self.scrollTime,
                minTime: "06:00:00",
                allDaySlot: false,
                nowIndicator: true,
                businessHours: {
                    start: '07:00',
                    end: '19:00'
                },
                customButtons: self.reloadButton == false ? {} : {
                    refresh: {
                        text: 'Recarregar agenda',
                        click: function () {
                            self.ReloadCalendar();
                        }
                    }
                },
                
                eventRender: function(event, element) {
                    if (self.eventEditFunction !== undefined || self.eventDeleteFunction !== undefined) {
                        var customTolltip = $("<div />");
                        if (self.eventEditFunction !== undefined) {
                            var optionEdit = $("<button type='button' title='editar' onclick='" +
                                self.eventEditFunction +
                                "(\"" +
                                event.id +
                                "\")' class='btn btn-default schedule-option schedule-option-edit'><i class='fa fa-edit'></i></button>");
                            customTolltip.append(optionEdit);
                        }
                        if (self.eventDeleteFunction !== undefined) {
                            var optionDelete = $("<button type='button' title='excluir' onclick='" +
                                self.eventDeleteFunction +
                                "(\"" +
                                event.id +
                                "\")' class='btn btn-default schedule-option schedule-option-delete'><i class='fa fa-trash'></i></button>");
                            customTolltip.append(optionDelete);
                        }

                        var view = $(self.selectorId).fullCalendar('getView');
                        var positionPopOver = event.start.isoWeekday() === 7 &&
                            view.name !== 'agendaDay' &&
                            view.name !== 'agendaWeek'
                            ? 'right'
                            : 'left';

                        element.popover({
                            title: "",
                            placement: positionPopOver,
                            html: true,
                            content: customTolltip.html(),
                            trigger: 'manual'
                        })
                            .on("mouseenter",
                                function() {
                                    var _this = this;
                                    $(this).popover("show");
                                    $(this)
                                        .siblings(".popover")
                                        .on("mouseleave",
                                            function() {
                                                $(_this).popover('hide');
                                            });
                                })
                            .on("mouseleave",
                                function() {
                                    var _this = this;
                                    setTimeout(function() {
                                        if (!$(".popover:hover").length) {
                                            $(_this).popover("hide");
                                        }
                                    },
                                        100);
                                });
                    }
                }
            });
        return this;
    },

    GetClientEvents: function(filter) {
        return $(this.selectorId).fullCalendar('clientEvents', filter);
    },

    SelectSourceEvent: function (id) {
        return $(this.selectorId).fullCalendar(this.ajax ? 'clientEvents' : 'getEventSourceById', id)[0];
    },

    WithEditNameEvent: function (functionEdit) {
        this.eventEditFunction = functionEdit;
        return this;
    },

    WithDeleteNameEvent: function (functionDelete) {
        this.eventDeleteFunction = functionDelete;
        return this;
    },

    WithDefaultView: function (view) {
        this.defaultView = view;
        return this;
    },

    WithScrollTime: function (scrollTime) {
        this.scrollTime = scrollTime;
        return this;
    }, 

    WithTimeFormat: function (format) {
        this.timeFormat = format;
        return this;
    },

    WithDateFormat: function (format) {
        this.formatDate = format;
        return this;
    },

    AddEventSource : function(param) {
        $(this.selectorId).fullCalendar('addEventSource', param);
        return this;
    },

    WithClickEvent: function (funcClick) {
        this.eventClickFunction = funcClick;
        return this;
    },

    WithOnlyWeekAndDayView: function () {
        this.onlyWeekAndDay = true;
        return this;
    },

    WithClickDay: function (funcClick) {
        this.dayClickFunction = funcClick;
        return this;
    },

    WithFilterRemoveEvent: function (filter) {
        this.filterRemove = filter;
        return this;
    },

    RemoveEvent: function (params) {
        var self = this;
        $(this.selectorId).fullCalendar('removeEvents', function(event) {
            return self.filterRemove(event, params);
        });
        return this;
    },

    WithHeight: function(value){
        this.height = value;
    },

    WithReloadButton: function () {
        this.reloadButton = true;
        return this;
    }

};