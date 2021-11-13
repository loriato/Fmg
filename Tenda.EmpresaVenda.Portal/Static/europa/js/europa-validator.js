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


Europa.Validator.ValidCpf = function (strCPF) {
    var Soma;
    var Resto;
    Soma = 0;
    strCPF = strCPF.replace(/\D/g, "");
    if (strCPF.length != 11 ||
        strCPF == "00000000000" ||
        strCPF == "11111111111" ||
        strCPF == "22222222222" ||
        strCPF == "33333333333" ||
        strCPF == "44444444444" ||
        strCPF == "55555555555" ||
        strCPF == "66666666666" ||
        strCPF == "77777777777" ||
        strCPF == "88888888888" ||
        strCPF == "99999999999") return false;

    for (var i = 1; i <= 9; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i);
    Resto = (Soma * 10) % 11;

    if ((Resto == 10) || (Resto == 11)) Resto = 0;
    if (Resto != parseInt(strCPF.substring(9, 10))) return false;

    Soma = 0;
    for (var i = 1; i <= 10; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i);
    Resto = (Soma * 10) % 11;

    if ((Resto == 10) || (Resto == 11)) Resto = 0;
    if (Resto != parseInt(strCPF.substring(10, 11))) return false;
    return true;
}