let Europa = {};
Europa.Controllers = {};
Europa.Components = {};
Europa.Components.Datatable = {};
Europa.Components.Modal = {};
Europa.i18n = {};
Europa.Form = {};

Europa.AjaxStart = function (event) {
    if (event.delegateTarget.activeElement.className.indexOf("ajax-global-false") <= -1
        && $(event.delegateTarget.activeElement).parents().closest(".ajax-global-false").length <= 0) {
        Spinner.Show();
    }
};

Europa.AjaxStop = function () {
    Spinner.Hide();
};

Europa.AjaxConfigure = function () {
    $(document).ajaxStart(function (event) {
        // Europa.AjaxStart(event);
    }).ajaxStop(function () {
        Europa.AjaxStop();
    });
};

Europa.RemoveSubstituteFields = function () {
    $(".replaced-input").remove();
};

Europa.HideSubstituteFields = function () {
    $(".replaced-input").hide();
};

Europa.ShowSubstituteFields = function () {
    $(".replaced-input").show();
};
Europa.AutoCompleteFix = function () {

    if (navigator.userAgent.indexOf('Safari') !== -1 && navigator.userAgent.indexOf('Chrome') === -1) {
        //Safari
        $('select.select2-container').change(function () {
            fillInEntry($(this));
        });

        $('select.select2-container').each(function () {
            fillInEntry($(this));
        });

    }

    function fillInEntry($elem) {
        var $selected = $elem.find('option:selected');

        if ($selected !== undefined) {
            setTimeout(function () {
                $("#select2-" + $elem.attr("id") + "-container").append($selected.text());
            }, 100);
        }
    }
}
Europa.Form.SerializeJson = function (idForm) {
    const ua = window.navigator.userAgent;
    const msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
        if ($(idForm).is('fieldset')) {
            return Europa.Form.SerializeJsonInputs($(idForm).find(':input').not('disabled').not('button'));
        }
    }

    const val = {};
    $(idForm).serializeArray().map(function (x) {
        if (val[x.name] !== undefined && val[x.name] === 'true') {
            return;
        }
        if (x.name.indexOf("[]") !== -1) {
            const name = x.name.replace("\[\]", "");
            if (val[name] === undefined) {
                val[name] = [];
            }
            val[name].push(x.value);
        } else {
            val[x.name] = x.value;
        }
    });

    return val;
};

Europa.Form.SerializeJsonInputs = function (inputs) {
    const val = {};
    $.each(inputs, function (idx, x) {
        if (val[$(x).attr('name')] !== undefined && $(x).is(':checked') === false) {
            return;
        }
        if ($(x).attr('name') !== undefined && $(x).attr('name').indexOf("[]") !== -1) {
            const name = $(x).attr('name').replace("\[\]", "");
            if (val[name] === undefined) {
                val[name] = [];
            }
            val[name].push($(x).val());
        } else {
            val[$(x).attr('name')] = $(x).val();
        }
    });
    return val;
};

$.fn.addHiddenInputData = function (data) {
    var keys = {};
    var addData = function (data, prefix) {
        for (var key in data) {
            var value = data[key];
            if (!prefix) {
                var nprefix = key;
            } else {
                var nprefix = prefix + '[' + key + ']';
            }
            if (typeof (value) == 'object') {
                addData(value, nprefix);
                continue;
            }
            keys[nprefix] = value;
        }
    };
    addData(data);
    var $form = $(this);
    $form.empty();
    for (var k in keys) {
        $form.addHiddenInput(k, keys[k]);
    }

};
$.fn.addHiddenInput = function (key, value) {
    var $input = $('<input type="hidden" name="' + key + '" />');
    $input.val(value);
    $(this).append($input);

};

Europa.Components.Cep = {};
//A variável abaixo é definida no /Shared/_AutoCompleteAction.cshtml;
Europa.Components.Cep.Action = undefined;
Europa.Components.Cep.UrlFor = function (cep) {
    return Europa.Components.Cep.Action + "?cep=" + cep;
};
Europa.Components.Cep.Search = function (cep, callback) {
    $.getJSON(Europa.Components.Cep.UrlFor(cep), function (dados) {
        if (dados.Sucesso != undefined && dados.Sucesso == false) {
            Europa.Informacao.ChangeHeaderAndContent('Atenção', dados.Objeto);
            Europa.Informacao.Show();
        }
        callback(dados);
    });
};

Europa.Components.ScrollTo = function (id) {
    let selector = Europa.String.RemoveHashtag(id);
    document.getElementById(selector).scrollIntoView(true);
    return false;
};

Europa.NavbarScrollControl = function (menuId, mainDivId) {
    var topMenu = $("#" + menuId);
    var menuItems = topMenu.children().children("a");
    var scrollItems = menuItems.map(function () {
        var item = $($(this).attr("href"));
        if (item.length) {
            return item;
        }
    });

    $("#" + mainDivId).scroll(function () {
        var topDistance = $(this).position().top;
        var fromTop = $(this).scrollTop() + topDistance;
        var curr = scrollItems.map(function (i) {
            if (i == 1) {
            }
            if (this.position().top + topDistance <= fromTop) {
                return this;
            }
        });
        curr = curr[curr.length - 1];
        var id = curr && curr.length ? curr[0].id : "";
        menuItems
            .parent().removeClass("active")
            .end().filter("[href='#" + id + "']").parent().addClass("active");
        ;
    });
}

Europa.OnTabChange = function (el, tabId, divId) {
    $("#" + tabId + " li").removeClass("active");
    $(el).addClass("active");
    $("#" + divId).animate(
        {scrollTop: $($(el).find("a").attr("href")).position().top},
        500);
}

Europa.GridClickLink = function () {
    $('.europa_detail').off().on("click",
        function (event) {
            var input = $(this);
            var url = input.data('href');
            if (event.metaKey || event.ctrlKey) {
                window.open(url, '_blank');
            } else {
                if (input.hasClass("as_tab")) {
                    window.open(url, '_blank');
                } else {
                    window.location.href = url;
                }
            }
        });
};

Europa.AddSubstituteFieldTo = function (input, css) {
    if ($(input).is("[aria-controls^='DataTables_Table']")) {
        return true;
    }
    input = $(input);
    var text;
    var isSelect = false;
    var isArea = false;
    var isInput = false;
    if (input.is('input')) {
        isInput = true;
        text = input.val();
    } else if (input.is('textarea')) {
        text = input.val();
        isArea = true;
    } else {
        text = input.find(':selected').text();
        isSelect = true;
    }
    text = $("<div></div>").text(text).html();

    if (input.attr('data-entity') !== undefined) {

        var entity = input.data('entity');
        var url = Europa.Components.DetailAction.Links[entity];
        var id = input.attr('data-id');
        var clickTel = "";
        var idParent = "";
        if (url) {
            if (id) {
                if (input.attr('data-entity') != "CallCliente") {
                    url = url + "/" + id;
                } else {
                    if (input.attr('data-id-parent') != undefined) {
                        idParent = input.attr('data-id-parent');
                    }
                    clickTel = ' onclick="Europa.IniciarAtendimentoTelefone(' + idParent + ')" ';
                    url = url + id;
                }
            }
            var title = '';
            if (input.data('title') !== undefined) {
                title = 'title="' + input.data('title') + '"';
            }
            text = '<a href="' + url + '" ' + title + clickTel + 'rel="noopener noreferrer" target="_blank">' + text + '</a>';
        }
    }
    var aux = "<div readonly class='form-control replaced-input' style='position:absolute; padding: 6px 6px;z-index:101;'> " + text + "</div>";
    aux = $.parseHTML(aux);
    aux = $(aux);
    if (isArea) {
        aux.css("height", input.outerHeight());
        aux.css("width", input.outerWidth());
    }

    if (isInput) {
        aux.addClass(input.attr('class'));
        if (entity == "CallCliente") {
            aux.addClass("europa_detail_skype");
            aux.css("text-decoration", "none");
        }
        aux.css("white-space", "nowrap");
        aux.css("overflow", "hidden");

        aux.each(function () {
            this.style.setProperty('width', input.outerWidth() + 'px', 'important');
        });
    }

    if (isArea) {
        aux.css("white-space", "pre-wrap");
        aux.css("word-wrap", "break-word");
        aux.css("overflow", "auto");
    }

    if (input.data('entity') && input.val()) {
        if (entity == "CallCliente") {
            aux.addClass("europa_detail_skype");
        } else {
            aux.addClass('europa_div_detail');
        }
        aux.attr('data-entity', input.data('entity'));
        aux.attr('data-id', isSelect ? input.val() : input.data('id'));
    }

    if (!input.hasClass('select2-container')) {
        aux.css("display", input.css('display'));
    }

    if (input.parent().find('.replaced-input').length > 0) {
        input.parent().find('.replaced-input').remove();
    }

    if (css) {
        aux.css(css);
    }

    $(input).before($(aux).prop("outerHTML"));
};

Europa.Components.DownloadFileNew = function (url, params) {
    Spinner.Show();
    var request = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(params)
    };
    fetch(url, request)
        .then(async function (response) {
            Spinner.Hide();
            if (response.ok) {
                const contentType = response.headers.get("content-type");
                if (contentType && contentType.indexOf("application/json") !== -1) {
                    response.json().then(res => {
                        Europa.Informacao.PosAcaoBaseResponse(res);
                    });
                }else {
                    var disposition = response.headers.get('Content-Disposition');
                    var filename = '';
                    var filenameRegex = /filename\*?=['"]?(?:UTF-\d['"]*)?([^;\r\n"']*)['"]?;?/
                        , matches = filenameRegex.exec(disposition);

                    if (matches !== null && matches[1]) {
                        filename = decodeURI(matches[1]);
                    }
                    var myBlob = await response.blob();
                    var downloadUrl = URL.createObjectURL(myBlob);
                    var a = document.createElement("a");
                    a.href = downloadUrl;
                    a.download = filename;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }
            } else {  
                Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Atencao, "Erro no download");
                Europa.Informacao.Show();
            }
        });
}

Europa.Components.DownloadFile = function (url, params) {
    $.ajax({
        type: 'POST',
        url: url,
        data: params,
        xhrFields: {
            responseType: 'blob'
        },
        beforeSend: function () {
            Spinner.Show();
        },
        error: function (res, a, b) {
            Spinner.Hide();
        },
        success: function (result, text, response) {
            var disposition = response.getResponseHeader('Content-Disposition');
            var filename = '';
            var filenameRegex = /filename\*?=['"]?(?:UTF-\d['"]*)?([^;\r\n"']*)['"]?;?/
                , matches = filenameRegex.exec(disposition);

            if (matches !== null && matches[1]) {
                filename = decodeURI(matches[1]);
            }

            var blob = new Blob([result], {type: result.ContentType});
            var downloadUrl = URL.createObjectURL(blob);
            var a = document.createElement("a");
            a.href = downloadUrl;
            a.download = filename;
            document.body.appendChild(a);
            a.click();
            a.remove();
            Spinner.Hide();
        }
    });
};

Europa.AjustTitleAndMenuAcitons = function () {
    var titlebarName = $('#titlebar-name');
    var titlebarButtons = $('#titlebar-buttons');
    var titlebarExtra = $('#titlebar-extra');

    var titleTarget = $('#titlebar-name-target');
    var buttonsTarget = $('#titlebar-buttons-target');
    var extraTarget = $('#titlebar-extra-target');


    titleTarget.html(titlebarName.show().html());
    buttonsTarget.html(titlebarButtons.show().html());

    if (titlebarExtra) {
        extraTarget.html(titlebarExtra.show().html());
        titlebarExtra.remove();
    }

    titlebarName.remove();
    titlebarButtons.remove();
};