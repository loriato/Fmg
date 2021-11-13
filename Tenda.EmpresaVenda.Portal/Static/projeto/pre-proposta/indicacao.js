Europa.Controllers.Indicacao = {};
Europa.Controllers.Indicacao.FormIndicado = "#form-1";
Europa.Controllers.Indicacao.AutoCompletes = { Estado: [], Cidade: [] };
var numClone = 1;
var baseForm = undefined;



$(function () {
    $('#add-form-btn').click(function () {
        clonarForm();
        Europa.Controllers.Indicacao.InitMask();
      //$("#confirmar_indicado").removeClass("hidden");
        Europa.Controllers.Indicacao.DisableButton();
    });
    baseForm = $("#form-1");
});

Europa.Controllers.Indicacao.AbrirModal = function () {
    $("#nao-indicar").prop('checked', false);
    Europa.Controllers.Indicacao.OnChangeCancelarIndicacao();
    deleteForm();
    numClone = 0;
    clonarForm();
    Europa.Controllers.Indicacao.InitAutoCompletes(numClone);
    Europa.Controllers.Indicacao.LimparForm();
    Europa.Controllers.Indicacao.InitMask();
    $("#modal-inclusao-indicacao").modal("show");
};

Europa.Controllers.Indicacao.InitAutoCompletes = function (indexForm) {
    const indexAutocomplete = indexForm - 1;
    Europa.Controllers.Indicacao.AutoCompletes.Estado[indexAutocomplete] = new Europa.Components.AutoCompleteEstadoIndique()
        .WithTargetSuffix("estado-" + indexForm)
        .WithOnChange(function () {
            Europa.Controllers.Indicacao.OnChangeEstado(indexAutocomplete);
        })
        .Configure();

    Europa.Controllers.Indicacao.AutoCompletes.Cidade[indexAutocomplete] = new Europa.Components.AutoCompleteCidadeIndique()
        .WithTargetSuffix("cidade-" + indexForm)
    Europa.Controllers.Indicacao.AutoCompletes.Cidade[indexAutocomplete].Data = function (params) {
        return {
            start: 0,
            pageSize: 10,
            filter: [
                {
                    value: params.term,
                    column: this.param,
                    regex: true
                },
                {
                    value: $("#autocomplete_estado-" + indexForm).val(),
                    column: 'idestado'
                }
            ],
            order: [
                {
                    value: "asc",
                    column: this.param
                }
            ]
        };
    };
       
    Europa.Controllers.Indicacao.AutoCompletes.Cidade[indexAutocomplete].Configure();
    Europa.Controllers.Indicacao.AutoCompletes.Cidade[indexAutocomplete].Disable();
};

Europa.Controllers.Indicacao.OnChangeEstado = function (indexAutocomplete) {
    Europa.Controllers.Indicacao.onChangeLiberar();
    if (indexAutocomplete >= 0) {
        var idEstado = Europa.Controllers.Indicacao.AutoCompletes.Estado[indexAutocomplete].Value();
        if (!idEstado) {
            Europa.Controllers.Indicacao.AutoCompletes.Cidade[indexAutocomplete].Disable();
            if (Europa.Controllers.Indicacao.AutoCompletes.Cidade[indexAutocomplete]) {
                Europa.Controllers.Indicacao.AutoCompletes.Cidade[indexAutocomplete].Clean();
            }

        } else {
            Europa.Controllers.Indicacao.AutoCompletes.Cidade[indexAutocomplete].Enable();
        }
    }

};

Europa.Controllers.Indicacao.LimparForm = function () {
    $("#nome-1").val("");
    $("#sobreNome-1").val("");
    $("#telefone-1").val("");
    $("#email-1").val("");
    $("#autocomplete_estado-1").val(null).trigger('change');
};

Europa.Controllers.Indicacao.InitMask = function () {
    Europa.Mask.Telefone('.mask-telephone');
}

function clonarForm() {
    ++numClone;
    const clone = baseForm.clone();
    
    clone.find("input").val("");
    // Incrementando os ids dos campos
    clone.find("[id$='1']").addBack("[id$='1']").each(function (index, elem) {
        const pos = elem.id.lastIndexOf('-') + 1;
        const newId = elem.id.substring(0, pos) + numClone;
        elem.id = newId;
    });
    clone.find("select[id^='autocomplete_']").each(function (index, elem) {
        $(elem).off().empty();      // Limpando os listeners e as options
        $(elem).next().remove();    // Removendo campos renderizados do select2
    });
    $("#forms-container").append(clone);
    var label = "#indicacao-" + numClone;
    $(label).text(numClone + ". INDICAÇÃO");
    Europa.Controllers.Indicacao.InitAutoCompletes(numClone);
    Europa.Util.RemoveFeedbackFields("#form-"+ numClone)
};

function montarForm() {
    const obj = [];
    $("#forms-container").children().each(function (index, form) {
        const data = Europa.Form.SerializeJson(form);
        data.Form = form.id;
        obj.push(data);
    });
    return obj;
};

function deleteForm() {
    $("#forms-container").children().remove();
};

function validarObj(data) {
    if (data.Nome == null || data.Nome == "") {
        return false;
    }
    if (data.Sobrenome == null || data.Sobrenome == "") {
        return false;
    }
    if (data.Telefone == null || data.Telefone == "") {
        return false;
    }
    if (data.Estado == null) {
        return false;

    }
    if (data.Cidade == null) {
        return false;
    }
    return true;
}

Europa.Controllers.Indicacao.Mensagens = function (response) {
    var mensagens = [];
    var i = 0;
    response.forEach(function (item) {
        i++;
        var number = item.Data.Form.replace("form-", "");
        if (i > 1) {
            mensagens.push("<br/><br/>")
        }
        mensagens.push("============== " + number + ".Indicação ==============<br/><br/>");
        mensagens.push(item.Messages.join("<br/>"));
    })
    Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Aviso, mensagens);
    Europa.Informacao.Show();
};

Europa.Controllers.Indicacao.EnableButton = function () {
    $("#confirmar_indicado").removeAttr("disabled");
    $("#add-form-btn").removeAttr("disabled");
};

Europa.Controllers.Indicacao.DisableButton = function () {
    $("#confirmar_indicado").attr("disabled", "disabled");
    $("#add-form-btn").attr("disabled", "disabled");
};

Europa.Controllers.Indicacao.onChangeLiberar = function () {
    $("#forms-container").children().each(function (index, form) {
        const data = Europa.Form.SerializeJson(form);
        data.Form = form.id;
        var decisao = validarObj(data);
        if (decisao) {
            Europa.Controllers.Indicacao.EnableButton();
            
        } else {
            Europa.Controllers.Indicacao.DisableButton();
            return false;
        }
        
    });
};

Europa.Controllers.Indicacao.OnChangeCancelarIndicacao = function () {
    if ($('#nao-indicar').prop('checked')) {
        $("#cancelar_indicado").removeClass("hidden");
        $("#confirmar_indicado").addClass("hidden");
    } else {
        $("#cancelar_indicado").addClass("hidden");
        $("#confirmar_indicado").removeClass("hidden");
    }
};

Europa.Controllers.Indicacao.CadastrarIndicado = function () {
    var param = montarForm();
    var sucessos = 0;
    var idCliente = $('#PreProposta_Cliente_Id').val();
    
    $.post(Europa.Controllers.Indicacao.UrlCadastrarIndicado, { indicacaoDto: param, idCliente: idCliente }, function (res) {
        res.forEach(function (item) {
            let idForm = "#" + item.Data.Form;
            if (item.Success) {
                sucessos++;
                $(idForm).remove();
            } else {
                Europa.Informacao.Hide = function () {
                    $(Europa.Informacao.Attr.Modal).modal("hide");
                }
                Europa.Util.HandleFeedbackFields(item, idForm);
            }
        });
        Europa.Controllers.Indicacao.Mensagens(res);
        if (sucessos == param.length) {
            $("#modal-inclusao-indicacao").modal("hide");
            Europa.Informacao.Hide = function () {
                $(Europa.Informacao.Attr.Modal).modal("hide");
                if (enviarPreProposta) {
                    Europa.Controllers.PreProposta.EnviarPreProposta();
                } 
            }
        }
    });
};

Europa.Controllers.Indicacao.SalvarRecusaIndicacao = function () {
    var idPreProposta = $("#PreProposta_Id").val();
    $.post(Europa.Controllers.Indicacao.UrlSalvarRecusaIndicacao, { idPreProposta: idPreProposta }, function (res) {
        if (res.Success) {
            $("#modal-inclusao-indicacao").modal("hide");
            if (enviarPreProposta) {
                Europa.Controllers.PreProposta.EnviarPreProposta();
            }
        }
    });
};