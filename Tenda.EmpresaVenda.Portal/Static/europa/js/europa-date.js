Europa.Date = {};
Europa.Date.Locale = "pt-BR";
Europa.Date.FORMAT_DATE_HOUR_NANOS = "DD/MM/YYYY HH:mm:ss.SSS";
Europa.Date.FORMAT_DATE_HOUR = "DD/MM/YYYY HH:mm";
Europa.Date.FORMAT_DATE = "DD/MM/YYYY";
Europa.Date.FORMAT_DATE_US = "YYYY-MM-DD";
Europa.Date.FORMAT_DATE_HOUR_MINUTE_SECOND = "DD/MM/YYYY HH:mm:ss";
Europa.Date.FORMAT_DATE_HOUR_MINUTE_SECOND_US = "YYYY-MM-DD-HHmmss";
Europa.Date.FORMAT_HOUR_MINUTE_SECOND = "HH:mm:ss";
Europa.Date.FORMAT_HOUR_MINUTE = "HH:mm";
Europa.Date.FORMAT_EXPORT = "YYYYMMDDHHmmss";
Europa.Date.WeekDaysValuesArray = [1, 2, 4, 8, 16, 32, 64];

Europa.Date.toGeenTimeFormat = function (date) {
    return Europa.Date.toFormatDate(date, Europa.Date.FORMAT_HOUR_MINUTE);
};

Europa.Date.toGeenDateTimeFormat = function (date) {
    return Europa.Date.toFormatDate(date, Europa.Date.FORMAT_DATE_HOUR);
};

Europa.Date.toGeenDateFormat = function (date) {
    return Europa.Date.toFormatDate(date, Europa.Date.FORMAT_DATE);
};

Europa.Date.toFormatDate = function (date, pattern) {
    if (date) {
        return moment(date).locale(Europa.Date.Locale).format(pattern);
    }
    return "";
};
Europa.Date.toFormatLongDate = function (value, pattern) {
    var date = Europa.Date.Parse(value);
    return moment(date).locale(Europa.Date.Locale).format(pattern);
};
Europa.Date.toSmallDate = function (date) {
    return Europa.Date.toFormatDate(date, "L");
};
Europa.Date.Now = function (pattern) {
    return Europa.Date.Format(new Date(), pattern == undefined ? Europa.Date.FORMAT_DATE : pattern);
};
Europa.Date.Format = function (value, pattern) {
    if (value) {
        return moment(value, pattern).locale(Europa.Date.Locale).format(pattern);
    }
    return "";
};

Europa.Date.FormatText = function (value, pattern, format) {
    if (value) {
        return moment(value, pattern).locale(Europa.Date.Locale).format(format);
    }
    return "";
};


Europa.Date.GetDiferenceBetwenDays = function (start, end) {
    var a = moment(start);
    var b = moment(end);

    return b.locale(Europa.Date.Locale).diff(a, 'days');
};

Europa.Date.IsDateOnWeekValues = function (date, weekDayValuesArray) {
    if (date == undefined || weekDayValuesArray == undefined || weekDayValuesArray.length == 0) {
        return false;
    }
    var weekDay = date.getDay();
    var dayValue = Europa.Date.WeekDaysValuesArray[weekDay];
    return $.inArray(dayValue, weekDayValuesArray) > -1;
};

Europa.Date.GetWeekDaysValues = function (dias) {
    var values = [];
    var binaryValueString = (dias >>> 0).toString(2);
    var size = binaryValueString.length - 1;
    for (var idx = 0; idx <= size; idx++) {
        var idxInt = parseInt(binaryValueString.charAt(idx));
        if (idxInt === 1) {
            var value = 1;
            for (var i = 0; i < size - idx; i++) {
                value = value * 2;
            }

            var indice = $.inArray(value, Europa.Date.WeekDaysValuesArray);
            if (indice > -1) {
                values.push(value);
            }
        }
    }
    return values;
};


Europa.Date.Parse = function (value) {
    if (value === null) {
        return null;
    }
    var regexDate = /Date\(([^)]+)\)/;
    var results = regexDate.exec(value);
    if (results == null) {
        return null;
    }
    var date = new Date(parseFloat(results[1]));
    return date;
};

Europa.Date.Moment = function (date) {
    return moment(date).locale(Europa.Date.Locale);
};

Europa.Date.AddDay = function (days, date) {
    return moment(date, Europa.Date.FORMAT_DATE).locale(Europa.Date.Locale).add(days, 'days').toDate();
};

Europa.Date.AddDayMoment = function (days, date) {
    return moment(date, Europa.Date.FORMAT_DATE).locale(Europa.Date.Locale).add(days, 'days');
};

Europa.Date.AddHourMoment = function (hours, date) {
    return moment(date, Europa.Date.FORMAT_DATE_HOUR).locale(Europa.Date.Locale).add(hours, 'hours');
};

Europa.Date.AddMinuteMoment = function (minutes, date) {
    return moment(date, Europa.Date.FORMAT_DATE_HOUR).locale(Europa.Date.Locale).add(minutes, 'minutes');
};

Europa.Date.SetTime = function (date, time) {
    return moment(date).locale(Europa.Date.Locale).set({ hour: time.Hours, minute: time.Minutes, second: time.Seconds }).toDate();
};

Europa.Date.GetAge = function (date) {
    if (date) {
        var today = new Date();
        var birthDate = new Date(moment(date, Europa.Date.FORMAT_DATE).locale(Europa.Date.Locale).format());
        var age = today.getFullYear() - birthDate.getFullYear();
        var m = today.getMonth() - birthDate.getMonth();
        if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }
        return age;
    }
    return "";
};

Europa.Date.ToFormatDateUtc = function (date, pattern) {
    if (date) {
        return moment(date).utc().locale(Europa.Date.Locale).format(pattern);
    }
    return "";
};


