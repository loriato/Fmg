Europa.String = {};
Europa.FormExtensions = {};
Europa.Messages = {};
Europa.Mascaras = {};

Europa.String.Hashtag = function (value) {
    return value.startsWith("#") ? value : "#"+value;
};

Europa.String.IsNullOrEmpty = function (value) {
    return value == null || value.toString().trim().length == 0;
};
Europa.String.IsNotNullAndNotEmpty = function (value) {
    return !Europa.String.IsNullOrEmpty(value);
};

Europa.Messages.ShowMessages = function (res, title) {
    if (res.Mensagens && res.Mensagens.length) {
        Europa.Informacao.ChangeHeaderAndContent(title == undefined ? Europa.i18n.Messages.Informacao : title, Europa.Messages.Format(res.Mensagens));
        Europa.Informacao.Show();
    }
};

Europa.String.Format = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }
    return s;
};

Europa.String.FormatDias = function(data) {
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

Europa.String.FormatCnpj = function (_cnpj) {
    if (_cnpj == undefined || _cnpj == null || _cnpj == "")
        return "";

    var _cnpj = String(_cnpj);

    var len = _cnpj.length;

    var digito = _cnpj.substring(len - 2, len);
    var mil = _cnpj.substring(len - 6, len - 2);
    var part3 = _cnpj.substring(len - 9, len - 6);
    var part2 = _cnpj.substring(len - 12, len - 9);
    var part1 = _cnpj.substring(0, len - 12);

    return part1 + "." + part2 + "." + part3 + "/" + mil + "-" + digito;
};

Europa.String.FormatCep = function (_cep) {
    if (_cep == undefined || _cep == null || _cep == "")
        return "";

    var _cep = String(_cep);

    var len = _cep.length;

    var part3 = _cep.substring(len - 3, len);
    var part2 = _cep.substring(len - 6, len - 3);
    var part1 = _cep.substring(0, len - 6);

    return part1 + "." + part2 + "-" + part3;
};


Europa.String.FormatAsGeenDateTime = function(stringDate) {
    return Europa.Date.toGeenDateTimeFormat(stringDate);
}

Europa.String.FormatAsGeenDate = function (stringDate) {
    return Europa.Date.toGeenDateFormat(stringDate);
}

Europa.String.FormatAsGeenTime = function(stringDate) {
    return Europa.Date.toGeenTimeFormat(stringDate);
}

Europa.String.FormatAsPhone = function(phoneNumber) {
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
}

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

Europa.String.FormatMoney = function(data) {
    if (data == undefined || data.length == 0) {
        return "";
    }
    data = data.toFixed(2).replace(".", ",").replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
    return data;
}

Europa.String.FormatDinheiro = function (data) {
    if (data == undefined || data == '' || data == null) {
        return "";
    }
     var valor = "R$ ";
     valor = valor + data.toFixed(2).replace(".", ",").replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
     return valor;
}

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

Europa.String.FormatBoolean = function (data) {
    if (data == null || data == undefined) {
        return "";
    }
    if (data) {
        return Europa.i18n.Messages.Sim;
    }
    else {
        return Europa.i18n.Messages.Nao;
    }
};