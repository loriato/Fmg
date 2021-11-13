Europa.Validator = {};

Europa.Validator.ClearField = function(name,form) {
    form = form.charAt(0) == "#" ? form : "#" + form;
    $("[name='" + name + "']", form).closest('.form-group').removeClass("has-error");
};

Europa.Validator.ClearForm = function (form) {
    form = form.charAt(0) == "#" ? form : "#" + form;
    $(".has-error", form).removeClass("has-error");
};

Europa.Validator.InvalidateListWithPrefix = function (fields, form, prefix) {
    if (fields instanceof Array) {
        var aux = $.map(fields, function (value, i) {
            return prefix + "." + value;
        });
        Europa.Validator.InvalidateList(aux, form);
    }
};

Europa.Validator.InvalidateList = function (fields, form) {
    form = form.charAt(0) == "#" ? form : "#" + form;
    $(form).validate({
        ignore: ":hidden",
        highlight: function (element) {
            if (element != undefined) {
                $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
            }
        },
        showErrors: function(errorMap, errorList) {
            this.numberOfInvalids();
            this.defaultShowErrors();
        },
        unhighlight: function (element) {
            if (element != undefined) {
                $(element).closest('.form-group').removeClass('has-error');
            }
        },
        success: function (element) {
            if (element != undefined) {
                $(element).closest('.form-group').removeClass('has-error');
            }
        },
        errorPlacement: function (error, element) {
            return true;
        },
        onfocusout: false,
        onkeyup: false
    });

    var validator = $(form).validate();
    validator.form();
    Europa.Validator.ClearForm(form);
    if (fields instanceof Array) {
        var list = {};
        $.each(fields,
            function (idx, val) {
                if ($("[name='" + val + "']", form).length > 0) {
                    list[val] = "";
                }
            });
        validator.showErrors(list);
    }
};


