"use strict";

Europa.Components.Modal = function () {
    this.prefix = "modal_";
    return this;
};

Europa.Components.Modal.prototype = function() {
    return this.prototype;
}

/* Builder Pattern Methods */
Europa.Components.Modal.prototype.WithTargetSuffix = function (targetSuffix) {
    this.suffix = targetSuffix;
    this.completePrefix = this.prefix + targetSuffix + "_";
    this.id = this.ApplySelector(targetSuffix);
    return this;
};

Europa.Components.Modal.prototype.WithAction = function (action) {
    this.action = action;
    return this;
};

Europa.Components.Modal.prototype.WithSelectFuncion = function (callback) {
    this.selectCallback = callback;
    return this;
};

Europa.Components.Modal.prototype.WithCancelFuncion = function (callback) {
    this.cancelCallback = callback;
    return this;
};

/* Internal Commons */
Europa.Components.Modal.prototype.ApplySelector = function (value) {
    return "#" + this.prefix + value;
};
Europa.Components.Modal.prototype.RemovePrefix = function (value) {
    return value.replace(this.completePrefix, "");
};

/* External Functions */
Europa.Components.Modal.prototype.Show = function () {
    $(this.id).modal("show");
};

Europa.Components.Modal.prototype.Hide = function () {
    $(this.id).modal("hide");
};

Europa.Components.Modal.prototype.DataTableAction = function () {
    return this.action;
};

Europa.Components.Modal.prototype.SelectedData = function () {
    
};

Europa.Components.Modal.prototype.SerializeFilter = function () {
    var filterArea = this.id + "_area_filtro";
    var inputs = $(filterArea).find(":input").not("button");

    var result = [];
    $.each(inputs, function (idx, val) {
        var $input = $(val);
        result[Europa.Components.Modal.RemovePrefix($input.attr("id"))] = $input.val() == null ? "" : $input.val();
    });
    console.log(result);
};

Europa.Components.Modal.prototype.Select = function () {
    if (this.selectCallback) {
        this.selectCallback(this.SelectedData());
    }
    this.Hide();
};

Europa.Components.Modal.prototype.Cancel = function () {
    if (this.cancelCallback) {
        this.cancelCallback();
    }
    this.Hide();
};

Europa.Components.Modal.prototype.CleanFilter = function () {
    //Pegar todos atributos dentro do filtro e limpar
    //Depois solicitar uma nova busca
};
