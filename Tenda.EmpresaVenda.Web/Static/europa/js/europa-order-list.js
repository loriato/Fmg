'use strict'

Europa.Components.OrderList = function() {
    this.prefix = "order_list_";
    this.itemTemplate = "<li class='ui-state-default europa-order-li' data-value='{0}' data-initial-order='{1}'>{2}</li>";
    return this;
};

Europa.Components.OrderList.prototype.WithTargetSuffix = function(target) {
    this.id = this.prefix + target;
    return this;
};

Europa.Components.OrderList.prototype.WithRawData = function (rawData) {
    this.rawData = rawData;
    return this;
};

Europa.Components.OrderList.prototype.WithData = function(data) {
    this.data = data;
    return this;
};

Europa.Components.OrderList.prototype.Instance = function () {
    return $("#" + this.id);
};

Europa.Components.OrderList.prototype.Configure = function() {
    var self = this;
    var target = $("#" + this.id);

    if (self.rawData != undefined) {
        self.data = [];
        for (var index = 0; index < self.rawData.length; index++) {
            var rawRow = self.rawData[index];
            var row = self.FormatRow(rawRow);
            self.data.push(row);
        };
    }

    var data = self.data;
    data.sort(function (a, b) { return a.Order - b.Order });

    var content = "";
    for (var index = 0; index < data.length; index++) {
        var model = data[index];
        content += Europa.String.Format(self.itemTemplate, model.Value, model.Order, model.Text)
    };
    target.html(content);

    target.sortable({
        placeholder: "europa-state-highlight"
    });

    target.disableSelection();

    return this;
};

Europa.Components.OrderList.prototype.Data = function() {
    var component = $("#" + this.id);

    var data = [];
    var index = 1;
    component.find('li').each(function (i) {
        var row = $(this);
        var value = row.attr('data-value');
        var newOrder = index;
        var order = row.attr('data-initial-order');
        var text = row.html();

        var object = {
            Value: value,
            Order: newOrder,
            OldOrder: order,
            Text: text
        };

        index++;
        data.push(object);
    });

    return data;
};

Europa.Components.OrderList.prototype.FormatRow = function (rawRow) {
    return rawRow;
};


Europa.Components.OrderList.prototype.SortByText = function () {
    var self = this;
    var target = $("#" + this.id);

    var data = self.data;
    data.sort(function (a, b) { return a.Text.localeCompare(b.Text) });
    var content = "";
    for (var index = 0; index < data.length; index++) {
        var model = data[index];
        content += Europa.String.Format(self.itemTemplate, model.Value, model.Order, model.Text)
    };
    target.html(content);
    target.sortable({
        placeholder: "ui-state-highlight"
    });

    target.disableSelection();
}