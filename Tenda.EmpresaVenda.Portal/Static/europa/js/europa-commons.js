Europa.String = {};
Europa.Util = {};
Europa.Messages = {};
Europa.FormExtensions = {};
Europa.Mascaras = {};
Europa.Validator = {};

Europa.String.Hashtag = function (value) {
    return value.startsWith("#") ? value : "#" + value;
};

Europa.String.Truncate = function (value, maxLength) {
    let len = value.length;
    if (len > maxLength) {
        return value.substring(0, maxLength);
    }
    return value;
};

Europa.String.TruncateInput = function (input, maxLength) {
    let len = $(input).val().length;
    if (len > maxLength) {
        $(input).val($(input).val().substring(0, maxLength));
    }
};

Europa.String.RemoveHashtag = function (value) {
    return value.startsWith("#") ? value.replace("#", "") : value;
};

/**
 * @return {boolean}
 */
Europa.String.IsNullOrEmpty = function (value) {
    return value == null || value.toString().trim().length === 0;
};

/**
 * @return {boolean}
 */
Europa.String.IsNotNullAndNotEmpty = function (value) {
    return !Europa.String.IsNullOrEmpty(value);
};

Europa.String.FormatBoolean = function (data) {
    if (data == null || data == undefined) {
        return "";
    }
    if (data == true || data == "true" || data == "True") {
        return Europa.i18n.Messages.Sim;
    } else {
        return Europa.i18n.Messages.Nao;
    }
};

Europa.Util.GetUrlParameterByName = function (name) {
    if (!name) return "";
    let query = window.location.search.toString();
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    let regexS = "[\\?&]" + name + "=([^&#]*)";
    let regex = new RegExp(regexS);
    let results = regex.exec(query);
    if (results == null) return "";
    else return decodeURIComponent(results[1].replace(/\+/g, " "));
};

Europa.Util.GetSearchUrl = function () {
    let query = window.location.search.toString();
    return decodeURIComponent(query);
};

Europa.Util.HandleResponseMessages = function (response) {
    if (response && response.Messages && response.Messages.length > 0) {
        if (response.Success) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Sucesso, response.Messages.join("<br/>"));
        } else {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, response.Messages.join("<br/>"));
        }
        Europa.Informacao.Show();
    }
};

Europa.Util.RemoveFeedbackFields = function (formId) {
    let formSelector = Europa.String.Hashtag(formId);
    $(".is-invalid", formSelector).removeClass("is-invalid");
};

Europa.Util.HandleFeedbackFields = function (response, formId) {
    Europa.Util.RemoveFeedbackFields(formId);
    if (response && response.Fields && response.Fields.length > 0) {
        let formSelector = Europa.String.Hashtag(formId);
        $.each(response.Fields, function (i, item) {
            let fields = $('[name="' + item.Key + '"]', formSelector);
            response.Messages.push(item.Value);
            $.each(fields, function (i, field) {
                if (field) {
                    let feedback;
                    if ($(field).is(':radio')) {
                        $(field).parent().addClass("is-invalid");
                        feedback = $(field).closest('.radio-group').parent().find(".invalid-feedback")[0];
                    } else {
                        feedback = $(field).parent().find(".invalid-feedback")[0];
                    }
                    $(field, formSelector).addClass("is-invalid");
                    $(field, formId).blur();
                    $(feedback, formSelector).html(item.Value);
                }
                $(field, formId).blur();
            })
        });
    }
};

Europa.Util.CalcularIdade = function (dataNasc) {
    let dataAtual = new Date();
    let anoAtual = dataAtual.getFullYear();
    let anoNascParts = dataNasc.split('/');
    let diaNasc = anoNascParts[0];
    let mesNasc = anoNascParts[1];
    let anoNasc = anoNascParts[2].split(' ')[0];
    let idade = anoAtual - anoNasc;
    let mesAtual = dataAtual.getMonth() + 1;
    if (mesAtual < mesNasc) {
        idade--;
    } else {
        if (mesAtual == mesNasc) {
            if (dataAtual.getDate() < diaNasc) {
                idade--;
            }
        }
    }
    return idade < 0 ? 0 : idade;
};

Europa.Util.ObjectToFormData = function (formData, data, parentKey) {
    if (data && typeof data === 'object' && !(data instanceof Date) && !(data instanceof File) && !(data instanceof Blob)) {
        Object.keys(data).forEach(key => {
            Europa.Util.ObjectToFormData(formData, data[key], parentKey ? `${parentKey}[${key}]` : key);
        });
    } else {
        const value = data == null ? '' : data;

        formData.append(parentKey, value);
    }
}

Europa.String.Format = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }
    return s;
};

Europa.String.FormatDias = function (data) {
    var msg = [];

    switch (data) {
        case (62):
            msg = "Dias da Semana";
            break;
        case (65):
            msg = "Final de Semana";
            break;
        case (127):
            msg = "Todos os dias";
            break;
        default:
            if (data >= 64) {
                msg.push(" Sábado");
                data = data - 64;
            }
            if (data >= 32) {
                msg.push(" Sexta");
                data = data - 32;
            }
            if (data >= 16) {
                msg.push(" Quinta");
                data = data - 16;
            }
            if (data >= 8) {
                msg.push(" Quarta");
                data = data - 8;
            }
            if (data >= 4) {
                msg.push(" Terça");
                data = data - 4;
            }
            if (data >= 2) {
                msg.push(" Segunda");
                data = data - 2;
            }
            if (data >= 1) {
                msg.push(" Domingo");
                data = data - 1;
            }
            msg = msg.reverse();
            break;
    }
    return msg;
};

Europa.String.FormatCpf = function (_cpf) {
    if (_cpf == undefined || _cpf == null || _cpf == "")
        return "";

    var cpf = String(_cpf);

    var primeiroCampo = cpf.substring(0, 3);
    var segundoCampo = cpf.substring(3, 6);
    var terceiroCampo = cpf.substring(6, 9);
    var quartoCampo = cpf.substring(9, 11);

    return primeiroCampo + "." + segundoCampo + "." + terceiroCampo + "-" + quartoCampo;
};

Europa.String.FormatAsGeenDateTime = function (stringDate) {
    return Europa.Date.toGeenDateTimeFormat(stringDate);
}

Europa.String.FormatAsGeenDate = function (stringDate) {
    return Europa.Date.toGeenDateFormat(stringDate);
}

Europa.String.FormatAsGeenTime = function (stringDate) {
    return Europa.Date.toGeenTimeFormat(stringDate);
}

Europa.String.FormatAsPhone = function (phoneNumber) {
    if (phoneNumber == null || phoneNumber == "")
        return;

    var telefone = String(phoneNumber);
    var primeiroCampo = null;
    var segundoCampo = null;
    var terceiroCampo = null;

    if (telefone.length > 10) {
        primeiroCampo = telefone.substring(0, 2);
        segundoCampo = telefone.substring(2, 7);
        terceiroCampo = telefone.substring(7);
    } else {
        primeiroCampo = telefone.substring(0, 2);
        segundoCampo = telefone.substring(2, 6);
        terceiroCampo = telefone.substring(6, 10);
    }
    return "(" + primeiroCampo + ")" + " " + segundoCampo + "-" + terceiroCampo;
}

Europa.String.FormatAsMoney = function (value) {
    if (Europa.String.IsNotNullAndNotEmpty(value)) {
        value = value.replace(",", ".");
        value = parseFloat(value).toFixed(2);
        var result = parseFloat(value).toLocaleString('pt-BR');
        return "R$ " + result;
    }
    return "R$ 0,00";
};

Europa.String.FormatMoney = function (value) {
    if (value === undefined || value === '' || value === null) {
        return "";
    }
    return 'R$ ' + value.toFixed(2).replace(".", ",").replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
};

Europa.String.FormatAsDecimal = function (val) {
    var value = val.toString();
    if (Europa.String.IsNotNullAndNotEmpty(value)) {
        value = value.replace(",", ".");
        value = parseFloat(value).toFixed(2);
        var result = parseFloat(value);
        return result;
    }
    return 0.00;
}

Europa.String.Insert = function (target, indexOf, textToInsert) {
    return target.slice(0, indexOf) + textToInsert + target.slice(indexOf);
};

Europa.Messages.Format = function (msgs) {
    var content = "";
    if (msgs instanceof Array) {
        $.each(msgs,
            function (idx, val) {
                content += "<div class='col-xs-24'>" + val + "</div>";
            });
    } else {
        content = "<div class='col-xs-24'>" + msgs + "</div>";
    }
    return content;
};

Europa.FormExtensions.Fill = function (frm, data) {
    $.each(data, function (key, value) {
        var $ctrl = $('[name*=' + key + ']', frm[0]);
        switch ($ctrl.attr("type")) {
            case "text":
            case "hidden":
                $ctrl.val(value);
                break;
            case "radio": case "checkbox":
                $ctrl.each(function () {
                    if ($(this).attr('value') == 0) {
                        $(this).attr("checked", "Dias");
                    } else {
                        $(this).attr("checked", "Horas");
                    }


                });
                break;
            default:
                $ctrl.val(value);
        }
    });
};

Europa.Mascaras.AplicarMascaraCpfCnpj = function () {
    var cpfCnpj = function (val) {
        return val.length > 14 ? '00.000.000/0000-00' : '000.000.000-009';
    },
        optionsDocumento = {
            onKeyPress: function (val, e, field, options) {
                field.mask(cpfCnpj(val), options);
            },
            clearIfNotMatch: false
        };

    $(".mascaraCpfCnpj").unmask().mask(cpfCnpj, optionsDocumento);
};

Europa.String.FormatTelefone = function (data) {
    if (Europa.String.IsNullOrEmpty(data)) {
        return "";
    }
    var input = $("<div></div>");

    switch (data.length) {
        case 8:
            input.mask("0000-0000");
            break;
        case 9:
            input.mask("00000-0000");
            break;
        case 10:
            input.mask("(00) 0000-0000");
            break;
        case 11:
            input.mask("(00) 00000-0000");
            break;
        default:
            return data;
    }

    return input.masked(data);
};

Europa.String.FormatSkypeLink = function (data, idCliente) {
    if (data != null) {
        var link = 'tel:55' + data;
        var formated = Europa.String.FormatAsPhone(data)
        return "<a class='europa_detail_skype' style='text-decoration: none' title='Ligar com Skype' href='" + link + "' onclick='Europa.IniciarAtendimentoTelefone(" + idCliente + ")'>" + formated + "</a>";
    }
    return "";
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
};